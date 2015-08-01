using System;
using Microsoft.Practices.Unity;
using NZSchools.DataParser.Services.ExcelReaderService;
using NZSchools.DataParser.Services.WebClientService;
using NZSchools.DataParser.Services.RestService;
using NZSchools.DataParser.Services.JsonService;
using System.Threading.Tasks;

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

            Task.Run(async () =>
            {
                await Start();
            }).Wait();
            

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IExcelReaderService, ExcelReaderService>();
            container.RegisterType<IJsonService, JsonService>();
            container.RegisterType<IRestService, RestService>();
            container.RegisterType<IWebClientService, WebClientService>();
        }

        private static void ResolveTypes(IUnityContainer container)
        {
            _fileDownloader = container.Resolve<FileDownloader>();
            _parser = container.Resolve<Parser>();
        }

        private static async Task Start()
        {
            if (!_fileDownloader.GetLatest())
            {
                Logger.Error("Failed to download file. Exiting.");
                return;
            }

            await _parser.Begin();
        }
    }
}
