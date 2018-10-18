 
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
   public partial class CustomsInsuranceCompanyRepository:IRepository<CustomsInsuranceCompany>
   {
   
        private ICustomContext currentContext;
        public CustomsInsuranceCompanyRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsInsuranceCompanyRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsInsuranceCompany GetSingle(string code)
        {
            return (from a in context.CustomsInsuranceCompanies
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsInsuranceCompany> GetAll()
        {
            return from a in context.CustomsInsuranceCompanies  
                   select a;
        }
				 
        public CustomsInsuranceCompany GetSingle(EntityKeyFields entityKeys)
        {
            CustomsInsuranceCompanyKeys keys = entityKeys as CustomsInsuranceCompanyKeys;
            return (from a in context.CustomsInsuranceCompanies
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsInsuranceCompany entity)
        {
            onAdd();
            context.CustomsInsuranceCompanies.Add(entity);
        }

        public void Remove(CustomsInsuranceCompany entity)
        {
            context.CustomsInsuranceCompanies.Attach(entity);
            context.CustomsInsuranceCompanies.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsInsuranceCompany entity)
        {
            onUpdate();
            context.CustomsInsuranceCompanies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsInsuranceCompany> All()
        {
            return context.CustomsInsuranceCompanies.ToList();
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
	 