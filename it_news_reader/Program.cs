using it_news_reader.job;
using it_news_reader.parser;
using it_news_reader.parser.entities;

namespace it_news_reader;

internal static class Program
{
    [STAThread]
    private static async Task Main()
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
        
        var formTask = Task.Run(() => Application.Run(new MainForm()));
        
        using (var cancellationTokenSource = new CancellationTokenSource())
        {
            var worker = new Worker();

            var workerTask = Task.Run(() => worker.StartAsync(cancellationTokenSource.Token, sites));

            Console.WriteLine("Press any key to stop the worker...");
            Console.ReadLine();

            cancellationTokenSource.Cancel();
            await workerTask;
        }

        await formTask;
    }

    private static string ToStringList(string[] values)
    {
        return "'" + string.Join("','", values) + "'";
    }
}