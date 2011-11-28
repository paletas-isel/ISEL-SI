using System.IO;
using System.Net;
using System.Text;

namespace GmailContacts.Extensions
{
    public static class HttpWebRequestExtensions
    {
        public static string GetStringResponse(this HttpWebRequest request)
        {
            StringBuilder responseBody = new StringBuilder();
            int readNr;

            try
            {
                byte[] read = new byte[1024];
                WebResponse r = request.GetResponse();
                Stream responseStream = r.GetResponseStream();
                while ((readNr = responseStream.Read(read, 0, read.Length)) != 0)
                {
                    responseBody.Append(Encoding.UTF8.GetString(read, 0, readNr));
                }
            }
            catch (WebException e)
            {
                byte[] read = new byte[1024];
                Stream responseStream = e.Response.GetResponseStream();
                while ((readNr = responseStream.Read(read, 0, read.Length)) != 0)
                {
                    responseBody.Append(Encoding.UTF8.GetString(read, 0, readNr));
                }
                return null;
            }

            return responseBody.ToString();
        }
    }
}