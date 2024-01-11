 
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
   public partial class SignatureTypeRepository:IRepository<SignatureType>
   {
   
        private ICustomContext currentContext;
        public SignatureTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SignatureTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SignatureType GetSingle(string code)
        {
            return (from a in context.SignatureTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SignatureType> GetAll()
        {
            return from a in context.SignatureTypes  
                   select a;
        }
				 
        public SignatureType GetSingle(EntityKeyFields entityKeys)
        {
            SignatureTypeKeys keys = entityKeys as SignatureTypeKeys;
            return (from a in context.SignatureTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SignatureType entity)
        {
            onAdd();
            context.SignatureTypes.Add(entity);
        }

        public void Remove(SignatureType entity)
        {
            context.SignatureTypes.Attach(entity);
            context.SignatureTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SignatureType entity)
        {
            onUpdate();
            context.SignatureTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SignatureType> All()
        {
            return context.SignatureTypes.ToList();
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
	 