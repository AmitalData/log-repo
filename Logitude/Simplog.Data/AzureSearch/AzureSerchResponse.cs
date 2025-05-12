using Azure;
using System;

namespace Simplog.Data.AzureSearch
{
    public class AzureSerchResponse 
    {
        public AzureSerchResponse(Response response, int count = 0) 
        {
            Response = response;
            Status = response.Status;
            Content = response.Content;
            Count = count;
        }

        public Response Response { get; set; }
        public int Status { get; set; }
        public int Count { get; set; }
        public BinaryData Content { get; set; }
        
    }
}
