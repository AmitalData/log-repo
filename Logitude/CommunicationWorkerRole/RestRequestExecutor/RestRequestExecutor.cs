using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class RestRequestExecutor
    {
        private static readonly string SoapEnvelopeTemplate = @"<?xml version=""1.0"" encoding=""utf-8""?>
             <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
               <soap:Body>
               {0}
              </soap:Body>
           </soap:Envelope>";

        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        public RestRequestExecutor()
        {
            _retryPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );
        }
        public RestRequestExecutor(int maxRetries)
        {
            _retryPolicy = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(maxRetries, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );
        }

        public async Task<ApiResponse<TResponse>> ExecuteAsync<TRequest, TResponse>(ApiRequest<TRequest> request)
        {
            //Validate input
            ValidateRequest(request);            
            string rawResponseContent = string.Empty;                        
            var apiCommunicationLog = new ApiCommunicationLog();
            var responseToReturn = new ApiResponse<TResponse>();
            try
            {                              
                //Send request   
                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var httpClient = new HttpClient {Timeout = TimeSpan.FromMilliseconds(request.Header.Timeout)};
                    var httpRequest = BuildHttpRequest(request);
                    var result = await httpClient.SendAsync(httpRequest);
                    rawResponseContent = await result.Content.ReadAsStringAsync();                    
                    return result;
                });

                if (request.IsSoapRequest)
                {
                    rawResponseContent = JsonConvert.SerializeObject(rawResponseContent);
                }

                responseToReturn = HandleResponse<TResponse>(response, rawResponseContent);
            }
            catch (Exception ex)
            {
                var error = HandleException<TResponse>(ex);
                responseToReturn.ErrorCode = error.ErrorCode;
                responseToReturn.ErrorMessage = error.ErrorMessage;
                rawResponseContent= error.ErrorMessage;
            }
            
            _ = Task.Run(() => LogCommunicationAsync(apiCommunicationLog,
                JsonConvert.SerializeObject(request.Data),
                rawResponseContent,
                responseToReturn.Success,
                request.Tenant));                       
            return responseToReturn;
        }


        private HttpRequestMessage BuildHttpRequest<TRequest>(ApiRequest<TRequest> request)
        {
            var httpRequest = new HttpRequestMessage(request.Header.Method, request.Url);

            foreach (var header in request.Header.ToDictionary())
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);            
            if (request.Data != null && MethodSupportsBody(request.Header.Method))
            {
                if (request.IsSoapRequest)
                {                    
                    var soapBody =  string.Format(SerializeToXml(request.Data)); 
                    httpRequest.Content = new StringContent(soapBody, Encoding.UTF8, "text/xml");
                }
                else
                {
                    httpRequest.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, request.Header.ContentType);
                }
            }

            return httpRequest;
        }

        private ApiResponse<TResponse> HandleResponse<TResponse>(HttpResponseMessage response, string rawContent)
        {
            if (response.IsSuccessStatusCode)
                return (ApiResponse<TResponse>)JsonConvert.DeserializeObject<TResponse>(rawContent);

            return ApiResponse<TResponse>.Fail(
                $"Request failed. StatusCode: {(int)response.StatusCode}, Response: {rawContent}",
                (int)response.StatusCode
            );
        }

        private ApiResponse<TResponse> HandleException<TResponse>(Exception ex)
        {
            string message;
            int code;

            switch (ex)
            {
                case HttpRequestException httpEx:
                    message = $"HTTP request error: {httpEx.Message}";
                    code = 9001;
                    break;
                case TaskCanceledException cancelEx:
                    message = $"Request timeout or canceled: {cancelEx.Message}";
                    code = 9002;
                    break;
                case JsonSerializationException jsonEx:
                    message = $"Serialization error: {jsonEx.Message}";
                    code = 9003;
                    break;
                default:
                    message = $"Unhandled exception: {ex.Message}. StackTrace: {ex.StackTrace}";
                    code = 9000;
                    break;
            }

            return ApiResponse<TResponse>.Fail(message, code);
        }

        private async Task LogCommunicationAsync(ApiCommunicationLog logger, string request, string responseOrException,bool success,  int tenant)
        {
            try
            {
                var status = success==true? StatusTypeCommunication.Done: StatusTypeCommunication.Failed;
                await logger.AddCommunicationLogAsync(request, responseOrException, tenant, status);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }                       
        private bool MethodSupportsBody(HttpMethod method)
        {
            return method == HttpMethod.Post || method == HttpMethod.Put;
        }

        private void ValidateRequest<TRequest>(ApiRequest<TRequest> request)
        {
            if (request == null)
                throw new ValidationServiceException("request is null");            
            if (string.IsNullOrWhiteSpace(request.Url))
                throw new ValidationServiceException("Url must be provided.", nameof(request.Url));
            if (request.Header == null)
                throw new ValidationServiceException("Header must be provided.", nameof(request.Header));
            if (request.Header.Method == null)
                throw new ValidationServiceException("Method must be provided.", nameof(request.Header.Method));
        }        
        private string SerializeToXml<T>(T data)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true });

            xmlSerializer.Serialize(xmlWriter, data);
            return stringWriter.ToString();
        }

    }
}
