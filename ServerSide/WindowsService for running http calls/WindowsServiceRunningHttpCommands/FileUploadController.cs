using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web.Http;

namespace WindowsServiceRunningHttpCommands
{
    public class FileUploadController : ApiController
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="filename">Full file path to read text content</param>
        [HttpGet]
        public string Get(string filename, string url)
        {
            WriteLog("WindowsServiceRunningHttpCommands.FileUploadController.Get fileName: " + filename);

            try
            {
                var responseString = string.Empty;
                using (var stream = File.OpenRead(filename))
                {
                    var length = stream.Length;
                    byte[] postData = new byte[length];
                    stream.Read(postData, 0, (int)length);
                    using (WebClient client = new WebClient())
                    {
                        client.Credentials = CredentialCache.DefaultCredentials;
                        var responseByteArray = client.UploadData(url, "POST", postData);
                        responseString = client.Encoding.GetString(responseByteArray);
                    }
                }

                return JsonConvert.SerializeObject(new { result = "success", data = responseString });
            }
            catch (Exception ex)
            {
                var msg = string.Empty;
                while (ex != null)
                {
                    WriteLog("WindowsServiceRunningHttpCommands.FileUploadController.Get Failed: " + ex.Message);
                    msg += ex.Message;
                    ex = ex.InnerException;
                }
                return JsonConvert.SerializeObject(new { result = "failure", data = ex.Message });
            }
        }

        private void WriteLog(string msg)
        {
            File.AppendAllText(@"c:\temp\WindowsServiceRunningHttpCommands.txt", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss") + ": " + msg + Environment.NewLine);
        }
    }
}