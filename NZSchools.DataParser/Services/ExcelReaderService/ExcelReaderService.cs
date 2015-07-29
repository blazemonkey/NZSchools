using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToExcel.Extensions;

namespace NZSchools.DataParser.Services.ExcelReaderService
{
    public class ExcelReaderService : IExcelReaderService
    {
        private List<string> _headerRows;

        public ExcelReaderService()
        {
            _headerRows = new List<string>();
        }

        public IEnumerable<T> Read<T>(string filePath, int headerRow) where T : new()
        {
            using (var book = new LinqToExcel.ExcelQueryFactory(filePath))
            {
                try
                {
                    var rows = book.Worksheet("Directory").ToList();

                    _headerRows = new List<string>(rows[headerRow - 1].Select(x => x.Value.ToString().RegexReplace(@"[/^*]+", "")));
                    var list = new List<T>();

                    foreach (var row in rows.Skip(headerRow).TakeWhile(x => !string.IsNullOrEmpty(x[0].ToString())))
                    {
                        var t = new T();
                        foreach (var info in t.GetType().GetProperties())
                        {
                            var index = _headerRows.IndexOf(_headerRows.First(
                                x => x.ToLower().Replace(" ", "") == info.Name.ToLower().Replace(" ", "")));
                            if (info.PropertyType == typeof(string))
                                info.SetValue(t, Convert.ChangeType(string.IsNullOrEmpty(row[index]) ? "" : row[index].ToString(), info.PropertyType, null));

                            if (info.PropertyType.IsValueType)
                                info.SetValue(t, Convert.ChangeType(string.IsNullOrEmpty(row[index]) ? "0" : row[index].ToString(), info.PropertyType, null));
                        }

                        list.Add(t);
                    }

                    return list;
                }
                catch (InvalidOperationException ioe)
                {
                    Logger.Error(ioe);
                    Logger.Error("Couldn't find a matching column from property");
                    return null;
                }
            }
        }
    }
}
