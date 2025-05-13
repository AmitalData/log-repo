using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

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
            StatusTypeCommunication statusTypeCode;
            string exception = string.Empty;
            var apiCommunicationLog = new ApiCommunicationLog();
            ApiResponse<TResponse> responseToReturn;
            try
            {                
                var httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromMilliseconds(request.Header.Timeout)
                };

                serializedRequestData = JsonConvert.SerializeObject(request.Data);                

                //Send request   
                var response=await _retryPolicy.ExecuteAsync(() =>
                {
                    var retryRequest = new HttpRequestMessage(request.Header.Method, request.Url);

                    foreach (var header in request.Header.ToDictionary())
                        retryRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);

                    if (request.Data != null && MethodSupportsBody(request.Header.Method))
                    {
                        retryRequest.Content = new StringContent(
                            serializedRequestData,
                            Encoding.UTF8,
                            request.Header.ContentType);
                    }

                    var result=   httpClient.SendAsync(retryRequest);
                    rawResponseContent = result.Result?.Content?.ReadAsStringAsync()?.Result;                    
                    return result;                                       
                });                            
               
                if (response.IsSuccessStatusCode)
                {                    
                    responseToReturn = JsonConvert.DeserializeObject<TResponse>(rawResponseContent);
                }
                else
                {
                    //Improve failure logging
                    responseToReturn = ApiResponse<TResponse>.Fail(
                        $"Request failed. StatusCode: {(int)response.StatusCode} Response: {rawResponseContent}",
                        (int)response.StatusCode
                    );
                }                
            }
            catch (HttpRequestException ex)
            {
                exception = $"HTTP request error: {ex.Message}";
                responseToReturn =ApiResponse<TResponse>.Fail(exception, 9001);
            }
            catch (TaskCanceledException ex)
            {
                exception = $"Request timeout or canceled: {ex.Message}";
                responseToReturn =ApiResponse<TResponse>.Fail(exception, 9002);
            }
            catch (JsonSerializationException ex)
            {
                exception = $"Serialization error: {ex.Message}";
                responseToReturn =ApiResponse<TResponse>.Fail(exception, 9003);
            }
            catch (Exception ex)
            {
                exception = $"Exception: {ex.Message}. StackTrace: {ex.StackTrace}";
                responseToReturn =ApiResponse<TResponse>.Fail(exception, 9000);
            }
            _ = Task.Run(() =>
            {
                try
                {
                    var responseForLog = rawResponseContent;
                    if (string.IsNullOrEmpty(exception))
                    {
                        statusTypeCode=StatusTypeCommunication.Done;

                    }
                    else
                    {
                        statusTypeCode = StatusTypeCommunication.Failed;
                        responseForLog= exception;
                    }                                       

                    apiCommunicationLog.AddCommunicationLog(serializedRequestData, responseForLog, request.Tenant, statusTypeCode);                    
                }
                catch 
                {
                    //Later, register here for the log by nlog
                }
            });

            return responseToReturn;
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
