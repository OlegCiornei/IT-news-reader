using Microsoft.VisualBasic;

namespace it_news_reader;

internal class QueryBuilder
{
    // supported commands
    private const string SELECT = "select";
    private const string UPDATE = "update";
    private const string INSERT = "insert";
    private const string DELETE = "delete";
    private readonly List<string> _fields = new();
    private readonly List<string> _limit = new();

    // query patterns
    private readonly Dictionary<string, string> _patterns = new()
    {
        { SELECT, "select %fields% from %table% %where% %limit%;" },
        { UPDATE, "update %table% SET %values% %where%;" },
        { INSERT, "insert or replace into %table% (%fields%) values (%values%);" },
        { DELETE, "delete from  %table% %where%;" }
    };

    private readonly List<string> _values = new();
    private readonly List<string> _where = new();
    private List<string> _order = new();
    private string _table = "";

    // query config
    private string _type = "";


    public QueryBuilder Select(string fields)
    {
        _type = SELECT;
        _fields.Add(fields);
        return this;
    }

    public QueryBuilder Insert(string table)
    {
        _table = table;
        _type = INSERT;
        return this;
    }

    public QueryBuilder Update()
    {
        _type = UPDATE;
        return this;
    }

    public QueryBuilder Delete(string table)
    {
        _table = table;
        _type = DELETE;
        return this;
    }

    public QueryBuilder Field(string field)
    {
        _fields.Add(field);
        return this;
    }

    public QueryBuilder From(string table)
    {
        _table = table;
        return this;
    }

    public QueryBuilder AndWhere(Where where)
    {
        _where.Add(_where.Count == 0 ? $"where {where.Get()}" : $" and {where.Get()}");
        return this;
    }

    public QueryBuilder OrWhere(Where where)
    {
        _where.Add(_where.Count == 0 ? $"where {where.Get()}" : $" or {where.Get()}");
        return this;
    }

    public QueryBuilder AddLimit(Limit limit)
    {
        _limit.Add(limit.Get());
        return this;
    }

    public QueryBuilder Values(string values)
    {
        _values.Add(values);
        return this;
    }

    public string Get()
    {
        return Strings.RTrim(_patterns[_type].Replace("%table%", _table)
            .Replace("%fields%", string.Join(",", _fields.ToArray()))
            .Replace("%where%", string.Join("", _where.ToArray()))
            .Replace("%limit%", string.Join("", _limit.ToArray()))
            .Replace("%values%", string.Join(",", _values.ToArray())));
    }
}