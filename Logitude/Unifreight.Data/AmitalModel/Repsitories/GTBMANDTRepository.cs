using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class GTBMANDTRepository : IRepository<GTBMANDT>
    {
        private AmitalContext currentContext;
        public GTBMANDTRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public GTBMANDTRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public GTBMANDT GetSingle(string CLIENTCODE, string FORMNAME, string ENTITY, string FIELDNAME)
        {
            return (from a in context.GTBMANDTs
                    where a.CLIENTCODE == CLIENTCODE && a.FORMNAME == FORMNAME && a.ENTITY == ENTITY && a.FIELDNAME == FIELDNAME
                    select a).FirstOrDefault();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public List<GTBMANDT> GetMulti(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBMANDTKeys;

            return (from a in context.GTBMANDTs
                    where a.CLIENTCODE == keys.CLIENTCODE && a.ENTITY == keys.ENTITY && a.FORMNAME == keys.FORMNAME
                    select a).ToList();
        }

        public GTBMANDT GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as GTBMANDTKeys;
            return this.GetSingle(keys.CLIENTCODE, keys.ENTITY, keys.FORMNAME, keys.FIELDNAME);
        }

        public List<GTBMANDT> GetAllMandatory(string CLIENTCODE, string FORMNAME)
        {
            return (from a in context.GTBMANDTs
                    where a.CLIENTCODE == CLIENTCODE && a.FORMNAME == FORMNAME && a.ATTR == "M"
                    select a).ToList();
        }

        public List<GTBMANDT> GetTabMandatory(string CLIENTCODE, string FORMNAME, string ENTITY)
        {
            return (from a in context.GTBMANDTs
                    where a.CLIENTCODE == CLIENTCODE && a.FORMNAME == FORMNAME && a.ENTITY == ENTITY && a.ATTR == "M"
                    select a).ToList();
        }

        public void Add(GTBMANDT entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(GTBMANDT entity)
        {
            throw new NotImplementedException();
        }

        public void Update(GTBMANDT entity)
        {
            throw new NotImplementedException();
        }

        public List<GTBMANDT> All()
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }
    }
}
