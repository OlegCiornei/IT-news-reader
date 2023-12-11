namespace it_news_reader.parser.entities;

public class Sites
{
    private Sites(string domain, string url, string articleTag, string titleTag,
        string textTag, string linkTag, string startLinkPattern, string endLinkPattern,
        string imageStartLinkPattern, string imageEndLinkPatter)
    {
        Domain = domain;
        Url = url;
        ArticleTag = articleTag;
        TitleTag = titleTag;
        TextTag = textTag;
        LinkTag = linkTag;
        StartLinkPattern = startLinkPattern;
        EndLinkPattern = endLinkPattern;
        ImageStartLinkPatter = imageStartLinkPattern;
        ImageEndLinkPatter = imageEndLinkPatter;
    }

    public string Domain { get; private set; }
    public string Url { get; private set; }
    public string ArticleTag { get; private set; }
    public string TitleTag { get; private set; }
    public string TextTag { get; private set; }
    public string LinkTag { get; private set; }
    public string StartLinkPattern { get; private set; }
    public string EndLinkPattern { get; private set; }
    public string ImageStartLinkPatter { get; private set; }
    public string ImageEndLinkPatter { get; private set; }

    public static Sites Habr => new("https://habr.com", "https://habr.com/ru/news/",
        "//article",
        "//a[@class='tm-title__link']",
        "//div[@class='tm-article-body tm-article-snippet__lead']",
        "//a[@class='tm-article-snippet__readmore']",
        "href=\"","\"",
        "<img src=\"", "\"");
    
    public static Sites Tproger => new("https://tproger.ru", "https://tproger.ru/",
        "//div[@class='tp-post-card__content']",
        "//h2[@class='tp-post-card__title']",
        "//p[@class='tp-post-card__text']",
        "//a[@class='tp-post-card__link']",
        "<a class=\"tp-post-card__link\" href=\"", "\"",
        "img class=\"tp-image__image\" src=\"", "\"");
}