using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NZSchools.DataParser.Services.WebClientService;

namespace NZSchools.DataParser
{
    public class FileDownloader
    {
        private const string DirectoryUrl = "http://www.educationcounts.govt.nz/__data/assets/excel_doc/0004/62572/Directory-School-current.xls";
        private const string DirectoryFilePath = "..\\..\\Data\\Directory.xls";

        private readonly IWebClientService _webClient;

        public FileDownloader(IWebClientService webClient)
        {
            _webClient = webClient;
        }

        public bool GetLatest()
        {
            var result = _webClient.DownloadFile(DirectoryUrl, DirectoryFilePath);
            return result;
        }
    }
}
