 
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
   public partial class MorningMessageTypeRepository:IRepository<MorningMessageType>
   {
   
        private ICustomContext currentContext;
        public MorningMessageTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public MorningMessageTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  MorningMessageType GetSingle(string code)
        {
            return (from a in context.MorningMessageTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MorningMessageType> GetAll()
        {
            return from a in context.MorningMessageTypes  
                   select a;
        }
				 
        public MorningMessageType GetSingle(EntityKeyFields entityKeys)
        {
            MorningMessageTypeKeys keys = entityKeys as MorningMessageTypeKeys;
            return (from a in context.MorningMessageTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MorningMessageType entity)
        {
            onAdd();
            context.MorningMessageTypes.Add(entity);
        }

        public void Remove(MorningMessageType entity)
        {
            context.MorningMessageTypes.Attach(entity);
            context.MorningMessageTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MorningMessageType entity)
        {
            onUpdate();
            context.MorningMessageTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MorningMessageType> All()
        {
            return context.MorningMessageTypes.ToList();
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
	 