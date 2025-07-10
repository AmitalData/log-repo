using System;

namespace CommunicationWorkerRole.RestRequestExecutor
{
    public class ApiRequest<T>
    {
        public string DeclarationId { get; set; }
        public int Tenant { get; set; }

        public T Data { get; set; }
        public string Url { get; set; }

        public ApiRequestHeader Header { get; set; }

        public bool IsSoapRequest =>
        Header?.ContentType?.Equals("text/xml", StringComparison.OrdinalIgnoreCase) == true;
    }
}
