 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class TapagConnectionTableRepository:IRepository<TapagConnectionTable>
   {
   
        private ICustomContext currentContext;
        public TapagConnectionTableRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TapagConnectionTableRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TapagConnectionTable GetSingle(string tapagid, string declarationid, int tenant)
        {
            return (from a in context.TapagConnectionTables
                    where a.TapagId == tapagid && a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TapagConnectionTable> GetAll(int tenant)
        {
            return from a in context.TapagConnectionTables  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TapagConnectionTable GetSingle(EntityKeyFields entityKeys)
        {
            TapagConnectionTableKeys keys = entityKeys as TapagConnectionTableKeys;
            return (from a in context.TapagConnectionTables
                    where a.TapagId == keys.TapagId && a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TapagConnectionTable entity)
        {
            onAdd();
            context.TapagConnectionTables.Add(entity);
        }

        public void Remove(TapagConnectionTable entity)
        {
            context.TapagConnectionTables.Attach(entity);
            context.TapagConnectionTables.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TapagConnectionTable entity)
        {
            onUpdate();
            context.TapagConnectionTables.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TapagConnectionTable> All()
        {
            return context.TapagConnectionTables.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 