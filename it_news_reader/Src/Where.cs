namespace it_news_reader;

internal class Where
{
    private readonly string _table;
    private readonly string _op;
    private readonly dynamic _value;

    public Where(string table, dynamic value, string op = "=")
    {
        this._table = table;
        this._op = op;
        this._value = value;
    }

    public string Get()
    {
        return $"{_table} {_op} {_value}";
    }
}