using it_news_reader.parser;
using it_news_reader.parser.entities;

namespace it_news_reader;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Console.WriteLine("===================");
        Console.WriteLine("Application started");
        Console.WriteLine("===================");
        ApplicationConfiguration.Initialize();

        List<Sites> sites = new List<Sites>();
        sites.Add(Sites.Tproger);
        sites.Add(Sites.Habr);
        var manager = new SqliteManager();
        ArticlesParser.GetArticles(sites).ForEach(i =>
        {
            var insertQuery = new QueryBuilder().Insert("articles")
                .Field("title,text,link,image")
                .Values(ToStringList(new[]
                {
                    i.GetTitle().Replace("'", "’"),
                    i.GetText().Replace("'", "’"),
                    i.GetLink(), i.GetImageLink()
                })).Get();
            manager.Insert(insertQuery);
        });

        Application.Run(new MainForm());
    }

    private static string ToStringList(string[] values)
    {
        return "'" + string.Join("','", values) + "'";
    }
}