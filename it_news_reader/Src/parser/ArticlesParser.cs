using System.Text.RegularExpressions;
using HtmlAgilityPack;
using it_news_reader.parser.entities;

namespace it_news_reader.parser;

abstract class ArticlesParser
{
    public static List<Article> GetArticles(List<Sites> sites)
    {
        List<Article> articles = new List<Article>();
        foreach (var site in sites)
        {
            string url = site.Url;
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var articlesParsed = doc.DocumentNode.SelectNodes(site.ArticleTag);
            var titles = doc.DocumentNode.SelectNodes(site.TitleTag);
            var texts = doc.DocumentNode.SelectNodes(site.TextTag);
            var links = doc.DocumentNode.SelectNodes(site.LinkTag);

            if (articlesParsed != null)
            {
                for (int currentArticle = 0; currentArticle < articlesParsed.Count; currentArticle++)
                {
                    string title = titles[currentArticle].InnerText.Trim();
                    string text = texts[currentArticle].InnerText
                        .Replace("Читать далее", "")
                        .Replace("&nbsp;", " ")
                        .Trim();
                    string link = ExtractSubstringBetweenPatterns(
                        links[currentArticle].OuterHtml, site.StartLinkPattern, site.EndLinkPattern);

                    string imageLink = null!;
                    if (articlesParsed[currentArticle].OuterHtml.Contains(site.ImageStartLinkPattern))
                    {
                        imageLink = ExtractSubstringBetweenPatterns(
                            articlesParsed[currentArticle].OuterHtml, site.ImageStartLinkPattern,
                            site.ImageEndLinkPattern);
                    }

                    articles.Add(new Article(title, text, site.Domain + link, imageLink!));
                }
            }
            else
            {
                Console.WriteLine(@"Unable to find news on " + site.Domain);
            }
        }

        return articles;
    }

    private static string ExtractSubstringBetweenPatterns(string input, string startPattern, string endPattern)
    {
        Regex regex = new Regex($"{Regex.Escape(startPattern)}(.*?){Regex.Escape(endPattern)}");
        Match match = regex.Match(input);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return string.Empty;
    }
}