using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Tariff.Models
{
    public class TariffSettingPM
    {
        public string Id { get; set; }
        public string DefaultPriceSteps { get; set; }
        public int Tenant { get; set; }
        public string DefaultWarningPercentage { get; set; }
        public string AirDefaultStepsId { get; set; }
        public string LCLDefaultStepsId { get; set; }
        public string AirDefaultSteps { get; set; }
        public string LCLDefaultSteps { get; set; }
        public string ContainerDefaults { get; set; }
        public string DefaultCurrencyId { get; set; }
        public int ChangeSetOp { get; set; }
        public object EncodeBase64NVARCHARFieldsBy { get; set; }
    }
}
