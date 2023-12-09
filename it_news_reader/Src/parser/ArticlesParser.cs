using System.Text.RegularExpressions;
using HtmlAgilityPack;
using it_news_reader.parser.entities;

namespace it_news_reader.parser;

abstract class ArticlesParser
{
    public static List<Article> GetArticles(List<Sites> sites)
    {
        foreach (var site in sites)
        {
            string url = site.Url;
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var articlesParsed = doc.DocumentNode.SelectNodes(site.ArticleTag);
            var titles = doc.DocumentNode.SelectNodes(site.TitleTag);
            var texts = doc.DocumentNode.SelectNodes(site.TextTag);
            var links = doc.DocumentNode.SelectNodes(site.LinkTag);
            var imageLinks = doc.DocumentNode.SelectNodes(site.ImageLinkTag);

            if (titles != null)
            {
                List<Article> articles = new List<Article>();
                for (int currentArticle = 0; currentArticle < titles.Count; currentArticle++)
                {
                    string title = titles[currentArticle].InnerText.Trim();
                    string text = texts[currentArticle].InnerText
                        .Replace("Читать далее", "")
                        .Replace("&nbsp;", " ")
                        .Trim();
                    string link = ExtractSubstringBetweenPatterns(
                        links[currentArticle].OuterHtml, "href=\"", "\" class=");

                    string imageLink = null!;
                    if (articlesParsed[currentArticle].OuterHtml.Contains("<img src=\""))
                    {
                        imageLink = ExtractSubstringBetweenPatterns(
                            articlesParsed[currentArticle].OuterHtml, "<img src=\"", "\"");
                    }

                    articles.Add(new Article(title, text, site.Domain + link, imageLink!));
                }

                return articles;
            }

            Console.WriteLine("Unable to find news.");
        }

        return new List<Article>();
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