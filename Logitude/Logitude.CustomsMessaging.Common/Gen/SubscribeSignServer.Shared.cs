using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.Gen
{
    public class SubscribeSignServer
    {
        public string SignCertificate { get ; set; }
        SignCertificateClass _MySignCertificateClass;
        public string CompanyTenant { get; set; }
        public SignCertificateClass MySignCertificateClass
        {
            get
            {
                return _MySignCertificateClass =
                    _MySignCertificateClass ??
                    SignCertificateClass.Get(SignCertificate);
            }

        }

        public DateTime LastAccessedAt { get; set; }

        public List<int> TenantListFromPersonID { get; set; }

        public bool IsPersonalSignOn { get; set; }
        public bool IsCompanySignOn { get; set; }
    }
    public class SignCertificateClass
    {
        private SignCertificateClass(string signCertificate)
        {
            try
            {
                SignCertificate = signCertificate ?? "";
                PersonId = GetPersonID(SignCertificate);
                CustomsAgentId = GetCustomsAgentId(SignCertificate);
                SignerName = GetValue(signCertificate, "SN") + " " + GetValue(signCertificate, "G");

                MachineName = GetValue(signCertificate, "MachineName");
                UserName = GetValue(signCertificate, "UserName");
            }
            catch (Exception)
            {

                throw new Exception("Could Not Create SignCertificateClass From signCertificate ");
            }

        }


        public string SignCertificate { get; private set; }
        public string PersonId { get; private set; }
        public string CustomsAgentId { get; private set; }
        public string SignerName { get; private set; }

        public string MachineName { get; private set; }
        public string UserName { get; private set; }


        public override string ToString()
        {
            return this.PersonId + "," + this.CustomsAgentId;
        }
        public static string GetPersonID(string CurrentSignCertificate)
        {
            return GetValueLast9Char(CurrentSignCertificate, "SERIALNUMBER");
        }

        private static string GetValueLast9Char(string CurrentSignCertificate, string key)
        {
            string value = GetValue(CurrentSignCertificate, key);
            value = value ?? "";
            if (value.Length < 10) return "";
            var last9Char = value.Substring(value.Length - 9);
            return last9Char;
        }

        private static string GetValue(string CurrentSignCertificate, string key)
        {
            string value = null;
            if (String.IsNullOrWhiteSpace(CurrentSignCertificate)) return "";
            var codevalue = CurrentSignCertificate.Split(',')
                .FirstOrDefault(rec =>
                    rec.Trim().StartsWith(key + "=", StringComparison.OrdinalIgnoreCase));
            if (String.IsNullOrWhiteSpace(codevalue)) return "";
            value = codevalue.Split('=').Last().Trim();
            return value;
        }
        public static string GetCustomsAgentId(string CurrentSignCertificate)
        {
            var companyName = GetValueLast9Char(CurrentSignCertificate, "O");
            //if (companyName.Equals("049028392", StringComparison.OrdinalIgnoreCase))
            //{
            //    companyName = "550221105";
            //}

            return companyName;
        }

        public static SignCertificateClass Get(string signCertificate)
        {
            return new SignCertificateClass(signCertificate);
        }
    }

    public class SubscribeSignServerStatus : SubscribeSignServer
    {
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
        public DateTime LastSuccessSignningAt { get; set; }
        public bool? IsOk { get; set; }
        public string VersionByFeatures { get; set; }
    }

    public class SignStationStatus
    {
        
        public string MachineName { get; set; }
        public string UserName { get; set; }

        public string CurrentSignCertificate { get; set; }
        public bool isCompanySignOn { get; set; }
        public bool isPersonalSignOn { get; set; }

        
        public string Status { get; set; }
        public bool? IsOk { get; set; }
        public DateTime LastSuccessSignningAt { get; set; }
        public string VersionByFeatures { get; set; }

    }

}
