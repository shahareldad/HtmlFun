using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Http;

namespace WindowsServiceRunningHttpCommands
{
    public class DeleteFileController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="filename">Full file path to read text content</param>
        [HttpGet]
        public string Get(string filename)
        {
            WriteLog("LadpcWindowsService.DeleteFileController.Get fileName: " + filename);

            try
            {
                File.Delete(filename);
                return JsonConvert.SerializeObject(new { result = "success", data = "file deleted" });
            }
            catch (Exception ex)
            {
                WriteLog("LadpcWindowsService.DeleteFileController.Get Failed: " + ex.Message);
                return JsonConvert.SerializeObject(new { result = "failure", data = ex.Message });
            }
        }

        private void WriteLog(string msg)
        {
            File.AppendAllText(@"c:\temp\LadpcWindowsService.txt", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + msg + Environment.NewLine);
        }
    }
}