namespace it_news_reader;

class Limit
{
    private int Offset;
    private int Lim;

    public Limit(int offset, int lim)
    {
        this.Offset = offset;
        this.Lim = lim;
    }

    public string Get()
    {
        return $"limit {Lim} offset {Offset}";
    }
}