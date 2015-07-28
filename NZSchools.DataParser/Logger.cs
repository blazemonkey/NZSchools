using System;

namespace NZSchools.DataParser
{
    public static class Logger
    {
        private static log4net.ILog Log { get; set; }

        static Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
            Log = log4net.LogManager.GetLogger(typeof(Logger));            
        }
        
        public static void Error(object msg)
        {
            Console.WriteLine("{0},{1},{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Error", msg);
            Log.Error(msg);
        }

        public static void Error(object msg, Exception ex)
        {
            Console.WriteLine("{0},{1},{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Error", msg);
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Console.WriteLine("{0},{1},{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Error", ex.StackTrace);
            Log.Error(ex.Message, ex);
        }

        public static void Info(object msg)
        {
            Console.WriteLine("{0},{1},{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "Info", msg);
            Log.Info(msg);
        }
    }
}
