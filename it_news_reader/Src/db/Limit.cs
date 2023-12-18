namespace it_news_reader;

internal class Limit
{
    private readonly int Lim;
    private readonly int Offset;

    public Limit(int offset, int lim)
    {
        Offset = offset;
        Lim = lim;
        




    }
   

    public string Get()
    {
        return $"limit {Lim} offset {Offset}";
    }
}