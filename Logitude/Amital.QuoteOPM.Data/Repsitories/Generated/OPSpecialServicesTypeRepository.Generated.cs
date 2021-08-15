 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Amital.QuoteOPM.Data.Repsitories
{
   public partial class OPSpecialServicesTypeRepository:IRepository<OPSpecialServicesType>
   {
   
        private IQuoteOPMContext currentContext;
        public OPSpecialServicesTypeRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public OPSpecialServicesTypeRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OPSpecialServicesType GetSingle(string id, int tenant)
        {
            return (from a in context.OPSpecialServicesTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OPSpecialServicesType> GetAll(int tenant)
        {
            return from a in context.OPSpecialServicesTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OPSpecialServicesType GetSingle(EntityKeyFields entityKeys)
        {
            OPSpecialServicesTypeKeys keys = entityKeys as OPSpecialServicesTypeKeys;
            return (from a in context.OPSpecialServicesTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OPSpecialServicesType entity)
        {
            onAdd();
            context.OPSpecialServicesTypes.Add(entity);
        }

        public void Remove(OPSpecialServicesType entity)
        {
            context.OPSpecialServicesTypes.Attach(entity);
            context.OPSpecialServicesTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OPSpecialServicesType entity)
        {
            onUpdate();
            context.OPSpecialServicesTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OPSpecialServicesType> All()
        {
            return context.OPSpecialServicesTypes.ToList();
        }

        private IQuoteOPMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 