 
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
   public partial class CommercialSaleRepository:IRepository<CommercialSale>
   {
   
        private ICustomContext currentContext;
        public CommercialSaleRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CommercialSaleRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CommercialSale GetSingle(string code)
        {
            return (from a in context.CommercialSales
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CommercialSale> GetAll()
        {
            return from a in context.CommercialSales  
                   select a;
        }
				 
        public CommercialSale GetSingle(EntityKeyFields entityKeys)
        {
            CommercialSaleKeys keys = entityKeys as CommercialSaleKeys;
            return (from a in context.CommercialSales
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CommercialSale entity)
        {
            onAdd();
            context.CommercialSales.Add(entity);
        }

        public void Remove(CommercialSale entity)
        {
            context.CommercialSales.Attach(entity);
            context.CommercialSales.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CommercialSale entity)
        {
            onUpdate();
            context.CommercialSales.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CommercialSale> All()
        {
            return context.CommercialSales.ToList();
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
	 