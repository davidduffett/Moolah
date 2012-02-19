using System.IO;
using System.Net;
using System.Text;

namespace Moolah
{
    public interface IHttpClient
    {
        string Post(string url, string data);
    }

    public class HttpClient : IHttpClient
    {
        public string Post(string url, string data)
        {
            var request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            using (var requestStream = request.GetRequestStream())
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                requestStream.Write(bytes, 0, bytes.Length);
            }
            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var responseData = new StreamReader(responseStream, Encoding.UTF8))
            {
                return responseData.ReadToEnd();
            }
        }
    }
}