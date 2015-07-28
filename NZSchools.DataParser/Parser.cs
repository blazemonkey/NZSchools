using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZSchools.DataParser.Models;
using NZSchools.DataParser.Services.ExcelReaderService;

namespace NZSchools.DataParser
{
    public class Parser
    {
        private const string DirectoryFilePath = "Data\\Directory.xls";

        private readonly IExcelReaderService _excel;

        public Parser(IExcelReaderService excel)
        {
            _excel = excel;
        }

        public void Begin()
        {
            _excel.Read<Directory>(DirectoryFilePath, 3);
        }
    }
}
