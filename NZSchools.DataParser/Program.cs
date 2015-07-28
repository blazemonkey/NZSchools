using System;
using Microsoft.Practices.Unity;
using NZSchools.DataParser.Services.ExcelReaderService;
using NZSchools.DataParser.Services.WebClientService;

namespace NZSchools.DataParser
{
    class Program
    {
        private static FileDownloader _fileDownloader;
        private static Parser _parser;

        static void Main()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            ResolveTypes(container);

            Start();

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IExcelReaderService, ExcelReaderService>();
            container.RegisterType<IWebClientService, WebClientService>();
        }

        private static void ResolveTypes(IUnityContainer container)
        {
            _fileDownloader = container.Resolve<FileDownloader>();
            _parser = container.Resolve<Parser>();
        }

        private static void Start()
        {
            //if (!_fileDownloader.GetLatest())
            //{
            //    Logger.Error("Failed to download file. Exiting.");
            //    return;
            //}

            _parser.Begin();
        }
    }
}
