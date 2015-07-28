using System;
using System.Net;

namespace NZSchools.DataParser.Services.WebClientService
{
    public class WebClientService : IWebClientService
    {
        public bool DownloadFile(string url, string filePath)
        {
            try
            {
                using (var client = new WebClient())
                {
                    Logger.Info(string.Format("{0}: {1}", "Started Downloading", url));
                    client.DownloadFile(url, filePath);
                    Logger.Info("Finished Downloading.");
                }

                return true;
            }
            catch (ArgumentNullException ane)
            {
                Logger.Error(ane);
                return false;
            }
            catch (WebException we)
            {
                Logger.Error(we);
                return false;
            }
            catch (NotSupportedException nse)
            {
                Logger.Error(nse);
                return false;
            }
        }
    }
}
