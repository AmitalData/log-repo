namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Result { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }

        public static ApiResponse<T> Ok(T result)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Result = result,
                ErrorMessage = string.Empty,
                ErrorCode = 0
            };
        }
        public static ApiResponse<T> Fail(string errorMessage,int errorCode)
        {
            return new ApiResponse<T>
            {
                Success = false,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode                
            };
        }
        public override string ToString() {
            return Success    ? $"Success: {Result?.ToString() ?? "null"}"    : $"Failure: {ErrorMessage ?? "No message"} (Code: {ErrorCode})";}
        public static explicit operator ApiResponse<T>(T result) => Ok(result);
    }
}

