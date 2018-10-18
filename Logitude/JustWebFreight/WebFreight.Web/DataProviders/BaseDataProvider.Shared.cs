
using System;

namespace WebFreight.Web.DataProviders
{
    public class BaseDataProvider
    {
        public DateTime Today_DateTime { get; set; }
        public byte[] Logo { get; set; }
        public string Address { get; set; }
        public string GeneralAddress { get; set; }

    }
}