using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketUI.Util;

public class ConsoleTable
{
    public ConsoleTable() : this(new List<int>())
    {
    }

    public ConsoleTable(IEnumerable<int> paddings)
    {
        Columns = new List<string>();
        Rows = new List<object[]>();
        Paddings = paddings.ToList();
        RowsWithoutColumns = new Dictionary<int, string>();
    }

    private List<int> Paddings { get; }
    private List<string> Columns { get; }
    private List<object[]> Rows { get; }
    private Dictionary<int, string> RowsWithoutColumns { get; }
    private List<object[]> ModifiedRows { get; set; }

    public ConsoleTable AddColumn(params string[] strings)
    {
        foreach (var s in strings.Where(x => x != null))
            Columns.Add(s);

        return this;
    }

    public ConsoleTable AddRow(params object[] strings)
    {
        Rows.Add(strings);

        return this;
    }

    public ConsoleTable AddRowWithoutColumn(string str)
    {
        RowsWithoutColumns.Add(Rows.Count - 1, str);

        return this;
    }

    public override string ToString()
    {
        var strBuilder = new StringBuilder();
        //if column have decimal type will format him {0.00}
        CreatingModifyingRowsForDecimalTypes();
        
        var paddings = GetCalculatedPadding();

        var format = Enumerable.Range(0, Columns.Count)
            .Select(i => " | {" + i + "}")
            .Aggregate((s, a) => s + a) + " |";
        var header = string.Format(format, Columns.Select((c, i) => new string(' ', (paddings[i] - c.Length) / 2) + c +
                                                                    new string(' ',
                                                                        paddings[i] - c.Length -
                                                                        (paddings[i] - c.Length) / 2))
            .ToArray<object>());
        var formattedRows = ModifiedRows.Select(row =>
            string.Format(format, row.Select((c, i) =>
                new string(' ', (paddings[i] - c.ToString().Length) / 2) + c +
                new string(' ',
                    paddings[i] - c.ToString().Length -
                    (paddings[i] - c.ToString().Length) / 2)
            ).ToArray<object>())).ToList();

        var longestLine = Math.Max(header.Length, Rows.Any() ? formattedRows.Max(row => row.Length) : 0);
        var divider = new string(' ', Paddings?.Sum() / 2 ?? 0) + " " +
                      string.Join("", Enumerable.Repeat("-", longestLine - 1)) + " ";
        //add indent if Padding property not null
        header = new string(' ', Paddings?.Sum() / 2 ?? 0) + header;
        formattedRows = formattedRows.Select(x => new string(' ', Paddings?.Sum() / 2 ?? 0) + x).ToList();
        //create resulted string
        strBuilder.AppendLine(header);
        strBuilder.AppendLine(divider);
        for (var i = 0; i < formattedRows.Count; i++)
        {
            strBuilder.AppendLine(formattedRows[i]);
            if (RowsWithoutColumns.ContainsKey(i)) strBuilder.AppendLine(RowsWithoutColumns[i]);
        }

        return strBuilder.ToString();
    }

    private void CreatingModifyingRowsForDecimalTypes()
    {        
        var decimalTypeIndexes = Rows.First().Select((v, i) => v is decimal?i:-1);
        ModifiedRows = Rows.Select(x => 
                x.Select((v, i) => decimalTypeIndexes.Contains(i)? $"{v:0.00}" : v).
                    ToArray()).
            ToList();
    }

    public List<int> GetCalculatedPadding()
    {
        CreatingModifyingRowsForDecimalTypes();
        return Columns
            .Select((_, i) => ModifiedRows.Select(row => row[i])
                .Union(new[] { Columns[i] })
                .Where(value => value != null)
                .Select(value => value.ToString().Length + 2).Max())
            .ToList();
    }
}