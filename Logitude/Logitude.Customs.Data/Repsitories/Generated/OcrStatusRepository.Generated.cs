 
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
   public partial class OcrStatusRepository:IRepository<OcrStatus>
   {
   
        private ICustomContext currentContext;
        public OcrStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public OcrStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  OcrStatus GetSingle(string code)
        {
            return (from a in context.OcrStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<OcrStatus> GetAll()
        {
            return from a in context.OcrStatuses  
                   select a;
        }
				 
        public OcrStatus GetSingle(EntityKeyFields entityKeys)
        {
            OcrStatusKeys keys = entityKeys as OcrStatusKeys;
            return (from a in context.OcrStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OcrStatus entity)
        {
            onAdd();
            context.OcrStatuses.Add(entity);
        }

        public void Remove(OcrStatus entity)
        {
            context.OcrStatuses.Attach(entity);
            context.OcrStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OcrStatus entity)
        {
            onUpdate();
            context.OcrStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OcrStatus> All()
        {
            return context.OcrStatuses.ToList();
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
	 