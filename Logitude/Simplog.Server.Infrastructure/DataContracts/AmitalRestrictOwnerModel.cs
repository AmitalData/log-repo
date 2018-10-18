using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.DataContracts
{
    public class AmitalRestrictOwnerModel
    {
        public AmitalRestrictOwnerModel()
        {
            Cards = new List<string>();
        }
        //key
        public int Tenant { get; set; }
        public string UnifreightUserId { get; set; }

        /// vAlue


        public bool IsRestrictedOwner { get; set; }
        public List<string> Cards { get; set; }
    }

    public interface IAmitalRestrictOwnerService
    {
        AmitalRestrictOwnerModel GetAmitalRestrictOwnerModel(bool getFromCache, int tenant = 1, string UnifreightUserId = null);
    }
}
