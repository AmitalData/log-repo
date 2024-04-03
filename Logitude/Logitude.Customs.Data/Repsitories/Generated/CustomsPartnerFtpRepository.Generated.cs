 
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
   public partial class CustomsPartnerFtpRepository:IRepository<CustomsPartnerFtp>
   {
   
        private ICustomContext currentContext;
        public CustomsPartnerFtpRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsPartnerFtpRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsPartnerFtp GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsPartnerFtps
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsPartnerFtp> GetAll(int tenant)
        {
            return from a in context.CustomsPartnerFtps  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsPartnerFtp GetSingle(EntityKeyFields entityKeys)
        {
            CustomsPartnerFtpKeys keys = entityKeys as CustomsPartnerFtpKeys;
            return (from a in context.CustomsPartnerFtps
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsPartnerFtp entity)
        {
            onAdd();
            context.CustomsPartnerFtps.Add(entity);
        }

        public void Remove(CustomsPartnerFtp entity)
        {
            context.CustomsPartnerFtps.Attach(entity);
            context.CustomsPartnerFtps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsPartnerFtp entity)
        {
            onUpdate();
            context.CustomsPartnerFtps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsPartnerFtp> All()
        {
            return context.CustomsPartnerFtps.ToList();
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
	 