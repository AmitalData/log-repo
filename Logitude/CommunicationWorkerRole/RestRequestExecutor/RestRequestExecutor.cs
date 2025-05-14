using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class RestRequestExecutor
    {
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
            string serializedRequestData = string.Empty;
            string rawResponseContent = string.Empty;
            string communicationLogId = string.Empty;

            string exception = string.Empty;
            var apiCommunicationLog = new ApiCommunicationLog();
            var responseToReturn = new ApiResponse<TResponse>();
            try
            {
                var httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(request.Header.Timeout)
                };

                serializedRequestData = JsonConvert.SerializeObject(request.Data);

                //Send request   
                var response = await _retryPolicy.ExecuteAsync(() =>
                {
                    var httpRequest = BuildHttpRequest(request, serializedRequestData);
                    var result = httpClient.SendAsync(httpRequest);
                    rawResponseContent = result.Result?.Content?.ReadAsStringAsync()?.Result;
                    return result;
                });

                responseToReturn = HandleResponse<TResponse>(response, rawResponseContent);
            }
            catch (Exception ex)
            {
                var error = HandleException<TResponse>(ex);
                responseToReturn.ErrorCode = error.ErrorCode;
                responseToReturn.ErrorMessage = error.ErrorMessage;
            }
            _ = Task.Run(() => LogCommunication(apiCommunicationLog, serializedRequestData, rawResponseContent, exception, request.Tenant));
            return responseToReturn;
        }


        private HttpRequestMessage BuildHttpRequest<TRequest>(ApiRequest<TRequest> request, string body)
        {
            var httpRequest = new HttpRequestMessage(request.Header.Method, request.Url);

            foreach (var header in request.Header.ToDictionary())
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);

            if (request.Data != null && MethodSupportsBody(request.Header.Method))
            {
                httpRequest.Content = new StringContent(body, Encoding.UTF8, request.Header.ContentType);
            }

            return httpRequest;
        }

        private ApiResponse<TResponse> HandleResponse<TResponse>(HttpResponseMessage response, string rawContent)
        {
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<TResponse>(rawContent);

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

        private void LogCommunication(ApiCommunicationLog logger, string request, string responseOrException, string exception, int tenant)
        {
            try
            {
                var status = new StatusTypeCommunication();
                var contentToLog = string.Empty;
                if (string.IsNullOrEmpty(exception))
                {
                    contentToLog = responseOrException;
                    status = StatusTypeCommunication.Done;
                }
                else
                {
                    contentToLog = exception;
                    status = StatusTypeCommunication.Failed;
                }

                logger.AddCommunicationLog(request, contentToLog, tenant, status);
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
                throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrWhiteSpace(request.Url))
                throw new ArgumentException("Url must be provided.", nameof(request.Url));
            if (request.Header == null)
                throw new ArgumentException("Header must be provided.", nameof(request.Header));
        }
    }
}
