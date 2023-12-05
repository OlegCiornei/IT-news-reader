using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;

namespace it_news_reader;

internal class SqliteManager //: IDBManager
{
    private readonly string _connectionString;

    public SqliteManager()
    {
        var workingDirectory = Environment.CurrentDirectory;
        var path = Directory.GetParent(workingDirectory).Parent.Parent.FullName + "/Data/sqlite.db";
        _connectionString = $"Data Source={path}";
        if (File.Exists(path))
        {
            return;
        }
        
        // create db file
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            connection.Close();
        }
    }


    public DataTable Select(string query, params object[] parameters)
    {
        var entries = new DataTable();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                AddParameters(command, parameters);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            entries.Columns.Add(new DataColumn(reader.GetName(i)));
                    }

                    int j = 0;
                    while (reader.Read())
                    {
                        var row = entries.NewRow();
                        entries.Rows.Add(row);

                        for (int i = 0; i < reader.FieldCount; i++)
                            entries.Rows[j][i] = (reader.GetValue(i));

                        j++;
                    }
                }
            }

            connection.Close();
        }

        return entries;
    }

    public int Insert(string query, params object[] parameters)
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(query, connection))
            {
                AddParameters(command, parameters);
                return command.ExecuteNonQuery();
            }
        }
    }

    private void AddParameters(SqliteCommand command, object[] parameters)
    {
        if (parameters == null)
        {
            return;
        }

        for (int i = 0; i < parameters.Length; i++)
        {
            command.Parameters.AddWithValue($"$param{i + 1}", parameters[i]);
        }
    }
}