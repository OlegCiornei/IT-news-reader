using System;
using System.Threading.Tasks;
using it_news_reader.parser;
using it_news_reader.parser.entities;

namespace it_news_reader.job
{
    internal class Worker
    {
        private readonly SqliteManager _sqliteManager;
        private readonly ArticlesParser _articlesParser;

        public Worker()
        {
            _sqliteManager = new SqliteManager();
        }

        public async Task StartAsync(CancellationToken cancellationToken, List<Sites> sites)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("sadasd");
                DeleteOldData();
                
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
                    _sqliteManager.Insert(insertQuery);
                });
               
                // await Task.Delay(TimeSpan.FromHours(4));
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
          
        }
        
        private static string ToStringList(string[] values)
        {
            return "'" + string.Join("','", values) + "'";
        }

        private void DeleteOldData()
        {
            string query = new QueryBuilder().Delete("articles").AndWhere(new Where("date", "$param1", "<")).Get();
            DateTime cutoffDate = DateTime.Now.AddHours(-24);
            _sqliteManager.Delete(query, cutoffDate);
        }
    }
}