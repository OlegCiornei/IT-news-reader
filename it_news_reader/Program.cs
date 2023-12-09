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
        ArticlesParser.GetArticles(sites).ForEach(i => Console.WriteLine("{0}", i));

        //Application.Run(new MainFrom());
    }
}