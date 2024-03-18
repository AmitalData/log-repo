 
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
   public partial class CB_ComputationMethodDataRepository:IRepository<CB_ComputationMethodData>
   {
   
        private ICustomContext currentContext;
        public CB_ComputationMethodDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_ComputationMethodDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_ComputationMethodData GetSingle(int id)
        {
            return (from a in context.CB_ComputationMethodDatas
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_ComputationMethodData> GetAll()
        {
            return from a in context.CB_ComputationMethodDatas  
                   select a;
        }
				 
        public CB_ComputationMethodData GetSingle(EntityKeyFields entityKeys)
        {
            CB_ComputationMethodDataKeys keys = entityKeys as CB_ComputationMethodDataKeys;
            return (from a in context.CB_ComputationMethodDatas
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_ComputationMethodData entity)
        {
            onAdd();
            context.CB_ComputationMethodDatas.Add(entity);
        }

        public void Remove(CB_ComputationMethodData entity)
        {
            context.CB_ComputationMethodDatas.Attach(entity);
            context.CB_ComputationMethodDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_ComputationMethodData entity)
        {
            onUpdate();
            context.CB_ComputationMethodDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_ComputationMethodData> All()
        {
            return context.CB_ComputationMethodDatas.ToList();
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
	 