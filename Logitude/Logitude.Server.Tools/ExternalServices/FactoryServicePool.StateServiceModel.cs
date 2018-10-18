using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.ExternalServices
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
