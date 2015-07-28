using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZSchools.DataParser.Services.ExcelReaderService
{
    public interface IExcelReaderService
    {
        IEnumerable<T> Read<T>(string filePath, int headerRow) where T : new();
    }
}
