using Microsoft.VisualBasic;

namespace it_news_reader;

using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class QueryBuilder
{
    // supported commands
    private const string SELECT = "select";
    private const string UPDATE = "update";
    private const string INSERT = "insert";
    private const string DELETE = "delete";

    // query config
    private string _type = "";
    private string _table = "";
    private readonly List<string> _fields = new List<string>();
    private readonly List<string> _values = new List<string>();
    private readonly List<string> _where = new List<string>();
    private List<string> _order = new List<string>();
    private readonly List<string> _limit = new List<string>();

    // query patterns
    private readonly Dictionary<string, string> _patterns = new Dictionary<string, string>
    {
        { SELECT, "select %fields% from %table% %where% %limit%;" },
        { UPDATE, "update %table% SET %values% %where%;" },
        { INSERT, "insert into %table% (%fields%) values (%values%);" },
        { DELETE, "delete from  %table% %where%;" }
    };

    
    public QueryBuilder Select(string fields)
    {
        this._type = QueryBuilder.SELECT;
        this._fields.Add(fields);
        return this;
    }

    public QueryBuilder Insert(string table)
    {
        this._table = table;
        this._type = QueryBuilder.INSERT;
        return this;
    }

    public QueryBuilder Update()
    {
        this._type = QueryBuilder.UPDATE;
        return this;
    }

    public QueryBuilder Delete()
    {
        this._type = QueryBuilder.DELETE;
        return this;
    }

    public QueryBuilder Field(string field)
    {
        this._fields.Add(field);
        return this;
    }

    public QueryBuilder From(string table)
    {
        this._table = table;
        return this;
    }

    public QueryBuilder AndWhere(Where where)
    {
        this._where.Add(this._where.Count == 0 ? $"where {where.Get()}" : $" and {where.Get()}");
        return this;
    }

    public QueryBuilder OrWhere(Where where)
    {
        this._where.Add(this._where.Count == 0 ? $"where {where.Get()}" : $" or {where.Get()}");
        return this;
    }

    public QueryBuilder AddLimit(Limit limit)
    {
        this._limit.Add(limit.Get());
        return this;
    }

    public QueryBuilder Values(string values)
    {
        this._values.Add(values);
        return this;
    }

    public string Get()
    {
        return Strings.RTrim(this._patterns[this._type].Replace("%table%", this._table)
            .Replace("%fields%", String.Join(",", this._fields.ToArray()))
            .Replace("%where%", String.Join("", this._where.ToArray()))
            .Replace("%limit%", String.Join("", this._limit.ToArray()))
            .Replace("%values%", String.Join(",", this._values.ToArray())));
    }
}