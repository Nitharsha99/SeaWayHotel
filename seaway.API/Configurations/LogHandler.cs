using seaway.API.Controllers;
using System.Net;
using System.Net.Http;
using System.IO;


namespace seaway.API.Configurations
{
    public class LogHandler
    {
        private readonly ILogger<LogHandler> _logger;

        public LogHandler(ILogger<LogHandler> logger)
        {
            _logger = logger;
        }

        public MegaLogData mapLogData(HttpRequestMessage request, HttpResponseMessage response)
        {
            try
            {
                string requestBody = request.Content != null ? request.Content.ReadAsStringAsync().Result : null;
                string responseBody = response.Content != null ? response.Content.ReadAsStringAsync().Result : null;

                MegaLogData log = new MegaLogData
                {
                    RequestContentType = request?.Content?.Headers.ContentType?.ToString(),
                    RequestMethod = request?.Method.Method,
                    RequestUrl = request?.RequestUri?.AbsoluteUri,
                    RequestBody = requestBody,
                    RequestTimestamp = DateTime.Now,
                    ResponseContentType = response?.Content?.Headers.ContentType?.ToString(),
                    ResponseStatusCode = (HttpStatusCode)response?.StatusCode,
                    ResponseTimestamp = DateTime.Now,
                    content = responseBody

                };
                return log;
            }
            catch (Exception ex)
            {
                return new MegaLogData
                {
                    RequestMethod = request.Method.Method,
                    RequestTimestamp = DateTime.Now,
                    RequestUrl = request.RequestUri?.AbsoluteUri,
                    RequestBody = ex.Message + " " + ex.StackTrace
                };
            }
        }
        public void setLogTrace(HttpRequestMessage request, HttpResponseMessage response, string responseBody, string requestPath) 
        {
            MegaLogData logData = mapLogData(request, response);
            if(string.IsNullOrEmpty(logData.content))
            {
                logData.content = responseBody;
            }

            if(logData.RequestUrl == null)
            {
                logData.RequestUrl = requestPath;
            }
            _logger.LogTrace("*************************************************************    ");
            _logger.LogTrace("Request : " + logData.RequestMethod + " - " + logData.RequestUrl);
            _logger.LogTrace("Request Timestamp : " + logData.RequestTimestamp.ToString("dd-MM-yyyy HH:mm:ss"));
            _logger.LogTrace("Request Body : " + logData.RequestBody);
            _logger.LogTrace("Response Timstamp : " + logData.ResponseTimestamp.ToString("dd-MM-yyyy HH:mm:ss"));
            _logger.LogTrace("Response Status Code : " + logData.ResponseStatusCode.ToString());
            _logger.LogTrace("Response Body : " + logData.content);
            _logger.LogTrace("*************************************************************   ");

        }
    }

    public class MegaLogData
    {
        public string? RequestMethod { get; set; }
        public string? RequestUrl { get; set; }
        public string? RequestBody { get; set; }
        public DateTime RequestTimestamp { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public DateTime ResponseTimestamp { get; set; }
        public string? content { get; set; }
    }
}
