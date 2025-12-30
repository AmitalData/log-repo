
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
using Logitude.Customs.BL.BL;
namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsPartnerFtpQueryService : EntityQueryService<CustomsPartnerFtp, CustomsPartnerFtpKeys, CustomsPartnerFtpPM, object, CustomsPartnerFtpKeys>
    {
            public CustomsPartnerFtpPM GetBy(int tenant,string InterfaceName, string PartnerCode, string TypeCode)
        {
            var q = this.repository.GetAll(tenant).Where(r => r.InterfaceName.ToUpper() == InterfaceName.ToUpper() && r.PartnerCode.ToUpper() == PartnerCode.ToUpper() && r.TypeCode == TypeCode);
            var poco = q.FirstOrDefault();
            if (poco== null)
            {
                return new CustomsPartnerFtpPM();
            }
            var pm=this.GetEntityPM(poco);
            return pm;

        }
        public List<CustomsPartnerFtpPM> GetAllTenantBy(string InterfaceName, string PartnerCode, string TypeCode)
        {
            var pocos = this.repository.GetAllTenantBy(InterfaceName , PartnerCode ,TypeCode);
            
            if (pocos == null)
            {
                return new List<CustomsPartnerFtpPM>();
            }
            var pms = pocos.Select(r => this.GetEntityPM(r)).ToList();
             
            return pms;

        }
        public List<CustomsPartnerFtpPM> GetListBy(int tenant, string InterfaceName, string PartnerCode, string TypeCode)
        {
            var q = this.repository.GetAll(tenant).Where(r => r.InterfaceName.Contains(InterfaceName) && r.PartnerCode == PartnerCode && r.TypeCode == TypeCode);
            var list = q.ToList();
            if (list == null)
            {
                return new List<CustomsPartnerFtpPM>();
            }
            List<CustomsPartnerFtpPM> listPM=new List<CustomsPartnerFtpPM>();
            foreach(var p in list)
            {
                listPM.Add(this.GetEntityPM(p));
            }
            return listPM;

        }

        public CustomsPartnerFtpPM GetBy(int tenant, string InterfaceName, bool getFromCahce = true)
        {
            string key = "CustomsPartnerFtp" + tenant + ";" + InterfaceName;
            
            Func<CustomsPartnerFtpPM> f = ( ) =>
            {
                CustomsPartnerFtp poco = repository.GetAll(tenant).Where(r => r.InterfaceName == InterfaceName).FirstOrDefault();
                return poco == null ? null : GetEntityPM(poco);
            };

            CustomsPartnerFtpPM res = getFromCahce ? CacheHelper.GetFromCache(key, f) : f();
            
            return res;
        }
    }
}
