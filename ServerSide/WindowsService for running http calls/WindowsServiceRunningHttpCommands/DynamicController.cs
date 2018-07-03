using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace WindowsServiceRunningHttpCommands
{
    public class DynamicController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="appName">"C:\CLScanner\CLScanner.Client.exe"</param>
        /// <param name="appParameters">Sample: "FileName:c:\temp\1.pdf ScanColor:false UseDuplex:false ShowScannerUI:false SetDevice:AV121"</param>
        [HttpGet]
        public string Get(string appName, string appParameters)
        {
            WriteLog("LadpcWindowsService.DynamicController.Get appName: " + appName);
            WriteLog("LadpcWindowsService.DynamicController.Get appParameters: " + appParameters);

            try
            {
                ApplicationLoader.PROCESS_INFORMATION procInfo;
                ApplicationLoader.StartProcessAndBypassUAC(appName + " " + appParameters, out procInfo);
            }
            catch (Exception ex)
            {
                WriteLog("LadpcWindowsService.DynamicController.Get Failed: ApplicationLoader: " + ex.Message);
                return JsonConvert.SerializeObject(new { result = "failure", data = ex.Message });
            }

            var processName = Path.GetFileNameWithoutExtension(appName);
            var notToWait = ConfigurationManager.AppSettings["ProcessesNotToWait"].Split(',');
            if (notToWait.Contains(processName))
            {
                return JsonConvert.SerializeObject(new { result = "success", data = "A non waitable process request" });
            }

            try
            {
                var counter = int.Parse(ConfigurationManager.AppSettings["WaitProcessToStart"]);
                Process[] processes = null;
                do
                {
                    processes = Process.GetProcessesByName(processName);
                    counter--;
                    if (processes.Length > 0)
                        break;
                    Thread.Sleep(1000);
                } while (counter > 0);

                counter = int.Parse(ConfigurationManager.AppSettings["WaitProcessToEnd"]);
                do
                {
                    processes = Process.GetProcessesByName(processName);
                    counter--;
                    if (processes.Length == 0)
                        break;
                    Thread.Sleep(1000);
                } while (processes.Length > 0);

                return JsonConvert.SerializeObject(new { result = "success", data = "Process request completed successfully" });
            }
            catch (Exception ex)
            {
                WriteLog("LadpcWindowsService.DynamicController.Get Failed: " + ex.Message);
                return JsonConvert.SerializeObject(new { result = "failure", data = ex.Message });
            }
        }

        private HttpResponseMessage BuildMessage(Exception ex = null)
        {
            var status = HttpStatusCode.OK;
            var resultString = new { result = "Success message" }.ToString();
            if (ex != null)
            {
                status = HttpStatusCode.ServiceUnavailable;
                resultString = new { result = "Failed message" }.ToString();
            }

            HttpResponseMessage response = Request.CreateResponse(status, "value from CreateResponse");
            response.Content = new StringContent(resultString, Encoding.Unicode);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromMinutes(20)
            };
            return response;
        }

        private void WriteLog(string msg)
        {
            File.AppendAllText(@"c:\temp\LadpcWindowsService.txt", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + msg + Environment.NewLine);
        }
    }
}