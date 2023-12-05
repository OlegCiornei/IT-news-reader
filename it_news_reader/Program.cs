using System.Data;

namespace it_news_reader;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // How to Use QueryBuilder
        var query = new QueryBuilder().Select("id, name")
            .From("test")
            .AndWhere(new Where("id", "1", "<="))
            .Get();
        System.Console.WriteLine(query);
        
        // with params
        var queryWithParams = new QueryBuilder().Select("*")
            .From("test")
            .AndWhere(new Where("id", "$param1"))
            .Get();
        
        // How to Use SqliteManager
        var result = new SqliteManager().Select(query);
        foreach (DataRow m in result.Rows)
        {
            System.Console.Write(m["id"].ToString() + "|");
            System.Console.WriteLine(m["name"].ToString());
        }
        
        // with params
        var resultWithParams = new SqliteManager().Select(queryWithParams, "1");
        foreach (DataRow m in resultWithParams.Rows)
        {
            System.Console.Write(m["id"].ToString() + "|");
            System.Console.WriteLine(m["name"].ToString());
        }
        
        // insert example
        string insertQuery = new QueryBuilder().Insert("test")
            .Field("name")
            .Values("'Hello, Anton!'")
            .Get();
        System.Console.WriteLine(insertQuery);
        new SqliteManager().Insert(insertQuery);
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}