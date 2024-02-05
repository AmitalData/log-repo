
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class ChartOfAccountRepository : IRepository<ChartOfAccount>
    {

        public List<ChartOfAccount> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public bool CheckWhetherCodeExists(string code, string id, int tenant)
        {
            bool exists;
            if (String.IsNullOrEmpty(id))
            {
                exists = (from a in context.ChartOfAccounts
                          where a.Code == code && a.Tenant == tenant
                          select a).Any();
            }
            else
            {
                exists = (from a in context.ChartOfAccounts
                          where a.Code == code && a.Tenant == tenant && a.Id != id
                          select a).Any();
            }
            return exists;
        }

        public bool CheckWhetherConnected(string code, string id, int tenant)
        {
            bool connected = false;
            if (!String.IsNullOrEmpty(id))
            {
                connected = (from a in context.GLAccounts
                             where a.ChartOfAccountsId == id
                             select a).Any();
            }
            return connected;
        }

        public List<ChartOfAccount> GetByCode(String code, int tenant)
        {
            if (String.IsNullOrEmpty(code))
            {
                List<ChartOfAccount> rv = new List<ChartOfAccount>();
                return rv;
            }
            else
            {
                IQueryable<ChartOfAccount> query = from a in context.ChartOfAccounts
                                                   where a.Code == code && a.Tenant == tenant
                                                   select a;
                if (query.Any())
                {
                    return (query).ToList();
                }
                else
                {
                    List<ChartOfAccount> rv = new List<ChartOfAccount>();
                    return rv;
                }
            }
        }

        public ChartOfAccount GetSingleByCode(string code, int tenant)
        {
            return (from a in context.ChartOfAccounts
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ChartOfAccount> GetByTypeParentID(string COATypeCode, string ParentCOAId)
        {
            var q = (from a in context.ChartOfAccounts
                     where a.TypeCode == COATypeCode
                     select a);
            if (!string.IsNullOrWhiteSpace(ParentCOAId))
            {
                q = q.Where(a => a.ParentId == ParentCOAId);
            }
            else
            {
                q = q.Where(a => a.ParentId == null);
            }
            return q;

        }
        public List<ChartOfAccount> GetMainParent(int tenant)
        {
            var q = (from a in context.ChartOfAccounts
                     where a.Tenant == tenant
                     where a.TypeCode == a.Code// MY!!! 
                     where a.ParentId == null
                     
                     select a);
            
            return q.ToList();

        }


        public List<ChartOfAccount> GetAllByTenant(int tenant)
        {
            var q = (from a in context.ChartOfAccounts
                     where a.Tenant == tenant
                     select a);

            return q.ToList();

        }
    }

}
