using Azure;
using System;

namespace Simplog.Data.AzureSearch
{
    public class AzureSerchResponse 
    {
        public AzureSerchResponse(Response response) 
        {
            Response = response;
            Status = response.Status;
            Content = response.Content;
        }

        public Response Response { get; set; }
        public int Status { get; set; }
        public BinaryData Content { get; set; }
        
    }
}
