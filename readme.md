# IT-news-reader

main features - accumulate It news from Habr, Reddit, and etc and display them to user

//examples of interaction with DB

var query = new QueryBuilder().Select("id, name")
    .From("test")
    .AndWhere(new Where("id", "1", "<="))
    .Get();
Console.WriteLine(query);

var queryWithParams = new QueryBuilder().Select("*")
    .From("test")
    .AndWhere(new Where("id", "$param1"))
    .Get();

var result = new SqliteManager().Select(query);
foreach (DataRow m in result.Rows)
{
    Console.Write(m["id"] + "|");
    Console.WriteLine(m["name"].ToString());
}

var resultWithParams = new SqliteManager().Select(queryWithParams, "1");
foreach (DataRow m in resultWithParams.Rows)
{
    Console.Write(m["id"] + "|");
    Console.WriteLine(m["name"].ToString());
}

var insertQuery = new QueryBuilder().Insert("test")
    .Field("name")
    .Values("'Hello, Anton!'")
    .Get();
Console.WriteLine(insertQuery);
new SqliteManager().Insert(insertQuery);
