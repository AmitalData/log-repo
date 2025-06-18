using System;
using System.Text;

namespace AmitalCloud.Infrastructure.APITools.ExternalServices
{
    public static partial class FactoryServicePool<T>
    {
        class StateServiceModel
        {

            public ExternalServicePM ExternalService { get; set; }



            public DateTime LastUsedAt { get; set; }
            public bool IsLastUsedSuccess { get; set; }


            public override string ToString()
            {
                return new StringBuilder().AppendLine(this.ExternalService.ServiceAddressUrl).ToString();
            }
        }
    }
}
