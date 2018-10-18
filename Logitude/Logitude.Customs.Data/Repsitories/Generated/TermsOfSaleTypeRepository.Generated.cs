 
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
   public partial class TermsOfSaleTypeRepository:IRepository<TermsOfSaleType>
   {
   
        private ICustomContext currentContext;
        public TermsOfSaleTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TermsOfSaleTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TermsOfSaleType GetSingle(string code)
        {
            return (from a in context.TermsOfSaleTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TermsOfSaleType> GetAll()
        {
            return from a in context.TermsOfSaleTypes  
                   select a;
        }
				 
        public TermsOfSaleType GetSingle(EntityKeyFields entityKeys)
        {
            TermsOfSaleTypeKeys keys = entityKeys as TermsOfSaleTypeKeys;
            return (from a in context.TermsOfSaleTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TermsOfSaleType entity)
        {
            onAdd();
            context.TermsOfSaleTypes.Add(entity);
        }

        public void Remove(TermsOfSaleType entity)
        {
            context.TermsOfSaleTypes.Attach(entity);
            context.TermsOfSaleTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TermsOfSaleType entity)
        {
            onUpdate();
            context.TermsOfSaleTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TermsOfSaleType> All()
        {
            return context.TermsOfSaleTypes.ToList();
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
	 