 
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
   public partial class SplitOrMergeReasonRepository:IRepository<SplitOrMergeReason>
   {
   
        private ICustomContext currentContext;
        public SplitOrMergeReasonRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SplitOrMergeReasonRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SplitOrMergeReason GetSingle(string code)
        {
            return (from a in context.SplitOrMergeReasons
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SplitOrMergeReason> GetAll()
        {
            return from a in context.SplitOrMergeReasons  
                   select a;
        }
				 
        public SplitOrMergeReason GetSingle(EntityKeyFields entityKeys)
        {
            SplitOrMergeReasonKeys keys = entityKeys as SplitOrMergeReasonKeys;
            return (from a in context.SplitOrMergeReasons
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SplitOrMergeReason entity)
        {
            onAdd();
            context.SplitOrMergeReasons.Add(entity);
        }

        public void Remove(SplitOrMergeReason entity)
        {
            context.SplitOrMergeReasons.Attach(entity);
            context.SplitOrMergeReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SplitOrMergeReason entity)
        {
            onUpdate();
            context.SplitOrMergeReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SplitOrMergeReason> All()
        {
            return context.SplitOrMergeReasons.ToList();
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
	 