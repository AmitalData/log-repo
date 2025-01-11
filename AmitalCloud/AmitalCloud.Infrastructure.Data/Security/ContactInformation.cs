using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Security
{
    public class ContactInformation
    {
        public int Tenant { get; set; }
        public string ContactEmail { get; set; }
        public bool IsLogitudeAdmin { get; set; }
        public string AccessLevelCode { get; set; }
        public bool DontShowLocalLabels { get; set; }
        //public string ComputingPartnerCode { get; set; }
        private List<string> myRolesIds;
        public List<string> RolesIds
        {
            get
            {
                if (myRolesIds == null)
                {
                    myRolesIds = new List<string>();
                }

                return myRolesIds;
            }
            set
            {
                myRolesIds = value;
            }
        }
        private List<string> myPackagesCodes;
        public List<string> PackagesCodes
        {
            get
            {
                if (myPackagesCodes == null)
                {
                    myPackagesCodes = new List<string>();
                }
                return myPackagesCodes;
            }
            set
            {
                myPackagesCodes = value;
            }
        }
        public bool IsApi { get; set; }
    }
}
