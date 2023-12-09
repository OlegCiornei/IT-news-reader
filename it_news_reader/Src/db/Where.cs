namespace it_news_reader;

internal class Where
{
    private readonly string _op;
    private readonly string _table;
    private readonly dynamic _value;

    public Where(string table, dynamic value, string op = "=")
    {
        _table = table;
        _op = op;
        _value = value;
    }

    public string Get()
    {
        return $"{_table} {_op} {_value}";
    }
}