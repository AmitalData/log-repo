
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsPartnerFtpQueryService : EntityQueryService<CustomsPartnerFtp, CustomsPartnerFtpKeys, CustomsPartnerFtpPM, object, CustomsPartnerFtpKeys>
    {
            public CustomsPartnerFtpPM GetBy(int tenant,string InterfaceName, string PartnerCode, string TypeCode)
        {
            var q = this.repository.GetAll(tenant).Where(r => r.InterfaceName == InterfaceName && r.PartnerCode == PartnerCode && r.TypeCode == TypeCode);
            var poco = q.FirstOrDefault();
            if (poco== null)
            {
                return new CustomsPartnerFtpPM();
            }
            var pm=this.GetEntityPM(poco);
            return pm;

        }
    }
}
