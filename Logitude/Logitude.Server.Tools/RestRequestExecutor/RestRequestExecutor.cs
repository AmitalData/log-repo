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

namespace Logitude.Server.Tools.RestRequestExecutor
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

        private static readonly JsonSerializerSettings _apiJsonSettings =
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include
            };

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

        public async Task<ApiResponse<TResponse>> ExecuteAsync<TRequest, TResponse>(
            ApiRequest<TRequest> request)
        {
            ValidateRequest(request);

            string rawResponseContent = string.Empty;
            var apiCommunicationLog = new ApiCommunicationLog();
            var responseToReturn = new ApiResponse<TResponse>();

            var requestJson = JsonConvert.SerializeObject(request.Data, _apiJsonSettings);
            _ = Task.Run(() => LogCommunicationAsync(
                apiCommunicationLog,
                requestJson,
                string.Empty,
                true,                       
                request.Tenant,
                request.RequestComm));

            try
            {
                var response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var httpClient = new HttpClient { Timeout = TimeSpan.FromMilliseconds(request.Header.Timeout) };
                    var httpRequest = BuildHttpRequest(request);
                    var result = await httpClient.SendAsync(httpRequest);
                    rawResponseContent = await result.Content.ReadAsStringAsync();
                    return result;
                });

                if (request.IsSoapRequest)
                    rawResponseContent = JsonConvert.SerializeObject(rawResponseContent);

                responseToReturn = HandleResponse<TResponse>(response, rawResponseContent);
            }
            catch (Exception ex)
            {
                var error = HandleException<TResponse>(ex);
                responseToReturn.ErrorCode = error.ErrorCode;
                responseToReturn.ErrorMessage = error.ErrorMessage;
                rawResponseContent = error.ErrorMessage;
            }

            _ = Task.Run(() => LogCommunicationAsync(
                apiCommunicationLog,
                string.Empty,
                rawResponseContent,
                responseToReturn.Success,
                request.Tenant,
                request.ResponseComm));

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
                    var json = JsonConvert.SerializeObject(request.Data, _apiJsonSettings); 
                    httpRequest.Content = new StringContent(json, Encoding.UTF8, request.Header.ContentType);
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

        private async Task LogCommunicationAsync(ApiCommunicationLog logger,
            string request,
            string responseOrException,
            bool success,
            int tenant,
            ApiCommunicationConstants communications)
        {
            try
            {
                var status = success==true? StatusTypeCommunication.Done: StatusTypeCommunication.Failed;
                await logger.AddCommunicationLogAsync(request, responseOrException, tenant, status, communications);
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

            ValidateField(request.Url, nameof(request.Url));

            if (request.Header == null)
                throw new ValidationServiceException("Header must be provided.", nameof(request.Header));
            if (request.Header.Method == null)
                throw new ValidationServiceException("Method must be provided.", nameof(request.Header.Method));

            if (request.RequestComm == null)
                throw new ValidationServiceException("RequestComm is null.", nameof(request.RequestComm));
            if (request.ResponseComm == null)
                throw new ValidationServiceException("ResponseComm is null.", nameof(request.ResponseComm));

            ValidateField(request.RequestComm.EntityId, nameof(request.RequestComm.EntityId));
            ValidateField(request.RequestComm.Subject, nameof(request.RequestComm.Subject));
            ValidateField(request.RequestComm.ObjectTableId, nameof(request.RequestComm.ObjectTableId));
        }
        private string SerializeToXml<T>(T data)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true });

            xmlSerializer.Serialize(xmlWriter, data);
            return stringWriter.ToString();
        }
        void ValidateField(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationServiceException( $"{paramName} must be provided.");
        }

    }
}
