namespace it_news_reader.parser.entities;

public class Sites
{
    private Sites(string domain, string url, string articleTag, string titleTag,
        string textTag, string linkTag, string imageLinkTag)
    {
        Domain = domain;
        Url = url;
        ArticleTag = articleTag;
        TitleTag = titleTag;
        TextTag = textTag;
        LinkTag = linkTag;
        ImageLinkTag = imageLinkTag;
    }

    public string Domain { get; private set; }
    public string Url { get; private set; }
    public string ArticleTag { get; private set; }
    public string TitleTag { get; private set; }
    public string TextTag { get; private set; }
    public string LinkTag { get; private set; }
    public string ImageLinkTag { get; private set; }

    public static Sites Habr => new("https://habr.com", "https://habr.com/ru/news/",
        "//article",
        "//a[@class='tm-title__link']",
        "//div[@class='tm-article-body tm-article-snippet__lead']",
        "//a[@class='tm-article-snippet__readmore']",
        "//img[@class='tm-article-snippet__lead-image']");
}