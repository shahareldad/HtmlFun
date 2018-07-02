using System;
using System.IO;
using System.Web.Http;
using Newtonsoft.Json;

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
            WriteLog("WindowsServiceRunningHttpCommands.DeleteFileController.Get fileName: " + filename);

            try
            {
                File.Delete(filename);
                return JsonConvert.SerializeObject(new { result = "success", data = "file deleted" });
            }
            catch (Exception ex)
            {
                WriteLog("WindowsServiceRunningHttpCommands.DeleteFileController.Get Failed: " + ex.Message);
                return JsonConvert.SerializeObject(new { result = "failure", data = ex.Message });
            }
        }

        private void WriteLog(string msg)
        {
            File.AppendAllText(@"c:\temp\WindowsServiceRunningHttpCommands.txt", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + msg + Environment.NewLine);
        }
    }
}
