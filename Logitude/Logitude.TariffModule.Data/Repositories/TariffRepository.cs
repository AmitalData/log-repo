 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.TariffModule.Data.Repositories
{
   public partial class TariffRepository:IRepository<Tariff>
   {
        
		public List<Tariff> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public IQueryable<TariffLine> GetAllTariffLines(int tenant)
        {
            return from a in context.TariffLines
                   where a.Tenant == tenant
                   select a;
        }


        public IQueryable<TariffLine> GetAllTariffLinesByTariffIds(string[] Ids,int tenant)
        {
            return from a in context.TariffLines
                   where a.Tenant == tenant && Ids.Contains(a.TariffId)
                   select a;
        }

        public IQueryable<Tariff> GetAllTariff(string[] Ids,int tenant)
        {
            return from a in context.Tariffs
                   where a.Tenant == tenant && Ids.Contains(a.Id)
                   select a;
        }

        public IQueryable<Tariff> GetActiveCustomsChargesTariffs(int tenant)
        {
            return from a in context.Tariffs.Include("CustomsBroker")
                   where a.Tenant == tenant && !a.InActive && (a.TypeCode == "ICC" || a.TypeCode == "ECC")
                   select a;
        }

        public IQueryable<Tariff> GetSurchargeTariffsByCodeAndSellerId(string[] ids,string typeCode, int tenant)
        {
            var code = "ASC";
            if(typeCode == "OLC")
            {
                code = "OSC";
            }

            else if (typeCode == "OFC")
            {
                code = "OFS";
            }

            return from a in context.Tariffs
                   where a.Tenant == tenant && ids.Contains(a.SellerId) && a.TypeCode== code
                   select a;
        }

        public IQueryable<TariffVersion> GetAllTariffVersionsByTariffIds(string[] Ids, int tenant)
        {
            return from a in context.TariffVersions
                   where a.Tenant == tenant && Ids.Contains(a.TariffId) && a.IsDraft==false 
                   select a;
        }

        public IQueryable<TariffVersionAllInCharge> GetAllTariffAllInOnVersionsByTariffIds(string[] Ids,int[]VersionIds, int tenant)
        {
            return from a in context.TariffVersionAllInCharges
                   where a.Tenant == tenant && Ids.Contains(a.TariffId) && VersionIds.Contains(a.Version)
                   select a;
        }

        public IQueryable<Tariff> GetAllFromIdList(List<string> ids, int tenant)
        {
            IQueryable<Tariff> entities = (from a in context.Tariffs where a.Tenant == tenant && ids.Contains(a.Id) select a);
            return entities;
        }

        public IQueryable<TariffLine> GetAllTariffLinesByTariffId(string tariffId, int tenant)
        {
            return from a in context.TariffLines
                   where a.Tenant == tenant && a.TariffId == tariffId
                   select a;
        }
    }
}
   