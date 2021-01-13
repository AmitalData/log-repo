 
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
   public partial class ReleaseMessageTypeRepository:IRepository<ReleaseMessageType>
   {
   
        private ICustomContext currentContext;
        public ReleaseMessageTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ReleaseMessageTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReleaseMessageType GetSingle(string code)
        {
            return (from a in context.ReleaseMessageTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReleaseMessageType> GetAll()
        {
            return from a in context.ReleaseMessageTypes  
                   select a;
        }
				 
        public ReleaseMessageType GetSingle(EntityKeyFields entityKeys)
        {
            ReleaseMessageTypeKeys keys = entityKeys as ReleaseMessageTypeKeys;
            return (from a in context.ReleaseMessageTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReleaseMessageType entity)
        {
            onAdd();
            context.ReleaseMessageTypes.Add(entity);
        }

        public void Remove(ReleaseMessageType entity)
        {
            context.ReleaseMessageTypes.Attach(entity);
            context.ReleaseMessageTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReleaseMessageType entity)
        {
            onUpdate();
            context.ReleaseMessageTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReleaseMessageType> All()
        {
            return context.ReleaseMessageTypes.ToList();
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
	 