using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SpecFlow.Models
{
    public static class HttpRequestType
    {
        public enum NoBodyRequestType
        {
            Get,
            Delete
        };

        public enum BodyRequestType
        {
            Post,
            Put
        };
    }
}