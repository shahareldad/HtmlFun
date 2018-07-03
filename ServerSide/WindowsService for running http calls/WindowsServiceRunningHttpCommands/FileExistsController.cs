using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Http;

namespace WindowsServiceRunningHttpCommands
{
    public class FileExistsController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="filename">Full file path to check if exists</param>
        [HttpGet]
        public string Get(string filename)
        {
            WriteLog("WindowsServiceRunningHttpCommands.FileExitsController.Get fileName: " + filename);

            try
            {
                var fileExists = File.Exists(filename);
                return JsonConvert.SerializeObject(new { result = "success", data = fileExists });
            }
            catch (Exception ex)
            {
                WriteLog("WindowsServiceRunningHttpCommands.FileExitsController.Get Failed: " + ex.Message);
                return JsonConvert.SerializeObject(new { result = "failure", data = ex.Message });
            }
        }

        private void WriteLog(string msg)
        {
            File.AppendAllText(@"c:\temp\WindowsServiceRunningHttpCommands.txt", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + msg + Environment.NewLine);
        }
    }
}