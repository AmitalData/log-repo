 
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
   public partial class CustomsCollateralsAnswerRepository:IRepository<CustomsCollateralsAnswer>
   {
   
        private ICustomContext currentContext;
        public CustomsCollateralsAnswerRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsCollateralsAnswerRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsCollateralsAnswer GetSingle(string customscollateralid, int linenumber, int tenant)
        {
            return (from a in context.CustomsCollateralsAnswers
                    where a.CustomsCollateralId == customscollateralid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsCollateralsAnswer> GetAll(int tenant)
        {
            return from a in context.CustomsCollateralsAnswers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsCollateralsAnswer GetSingle(EntityKeyFields entityKeys)
        {
            CustomsCollateralsAnswerKeys keys = entityKeys as CustomsCollateralsAnswerKeys;
            return (from a in context.CustomsCollateralsAnswers
                    where a.CustomsCollateralId == keys.CustomsCollateralId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsCollateralsAnswer entity)
        {
            onAdd();
            context.CustomsCollateralsAnswers.Add(entity);
        }

        public void Remove(CustomsCollateralsAnswer entity)
        {
            context.CustomsCollateralsAnswers.Attach(entity);
            context.CustomsCollateralsAnswers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsCollateralsAnswer entity)
        {
            onUpdate();
            context.CustomsCollateralsAnswers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsCollateralsAnswer> All()
        {
            return context.CustomsCollateralsAnswers.ToList();
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
	 