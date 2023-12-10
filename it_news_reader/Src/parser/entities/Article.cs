namespace it_news_reader.parser.entities;

public class Article
{
    private string Title { get; set; }
    private string Text { get; set; }
    private string Link { get; set; }
    private string ImageLink { get; set; }

    public Article(string title, string text, string link, string imageLink)
    {
        Title = title;
        Text = text;
        Link = link;
        ImageLink = imageLink;
    }

    public string GetTitle()
    {
        return Title;
    }

    public string GetText()
    {
        return Text;
    }

    public string GetLink()
    {
        return Link;
    }

    public string GetImageLink()
    {
        return ImageLink;
    }

    public override string ToString()
    {
        return "\t" + Title + Environment.NewLine +
               Text + Environment.NewLine +
               Link + Environment.NewLine +
               ImageLink + Environment.NewLine;
    }
}