using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityKeys;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel.Repsitories
{
    public class CCUCUSTITEMRepository : IRepository<CCUCUSTITEM>
    {
        private AmitalContext currentContext;
        public CCUCUSTITEMRepository(int tenant)
        {
            currentContext = AmitalContext.GetContext(tenant);
        }

        public CCUCUSTITEMRepository(AmitalContext context)
        {
            currentContext = context;
        }

        public CCUCUSTITEM GetSingle(int FILENO, int LINENO)
        {
            return (from a in context.CCUCUSTITEMs
                    where a.FILENO == FILENO && a.LINENO == LINENO
                    select a).FirstOrDefault();
        }

        public IQueryable<CCUCUSTITEM> GetAll()
        {
            return from a in context.CCUCUSTITEMs
                   select a;
        }

        public void Add(CCUCUSTITEM entity)
        {
            context.CCUCUSTITEMs.Add(entity);
        }

        public void Remove(CCUCUSTITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCUSTITEMs.Attach(entity);
            }
            //context.AddToCCUCUSTITEMs 
            context.CCUCUSTITEMs.Remove(entity);
        }

        public void Update(CCUCUSTITEM entity)
        {
            //if (entity.EntityState == System.Data.EntityState.Unchanged)
            {
                context.CCUCUSTITEMs.Attach(entity); context.SetAsModified(entity);
            }
        }

        public List<CCUCUSTITEM> All()
        {
            return context.CCUCUSTITEMs.ToList();
        }

        private AmitalContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public CCUCUSTITEM GetSingle(EntityKeyFields entityKeys)
        {
            var keys = entityKeys as CCUCUSTITEMKeys;
            return this.GetSingle(keys.FILENO, keys.LINENO);

        }

        public List<CCUCUSTITEM> GetMulti(EntityKeyFields entityKeys)
        {
            var my105List = new List<CCUCUSTITEM>();
            var keys = entityKeys as CCUSUPITEMKeys;

            if (false)
            {
                var CCUSUPITEMRepo = new CCUSUPITEMRepository(this.context);
                var myParent103 =
                    //context.CCUSUPITEMs.FirstOrDefault(rec => rec.FILENO == keys.FILENO && rec.ACCLINENO == keys.ACCLINENO && rec.LINENO == keys.LINENO);
                    CCUSUPITEMRepo.GetSingle(keys);
                if (!myParent103.ITEMLINENO.HasValue)
                {
                    return my105List;
                }
                var cur105 = GetSingle(myParent103.FILENO, myParent103.ITEMLINENO.Value);
                if (cur105 != null)
                {
                    my105List.Add(cur105);
                }
            }

            var join105and103 = (from b in context.CCUSUPITEMs.Where(rec => rec.FILENO == keys.FILENO && rec.ACCLINENO == keys.ACCLINENO && rec.LINENO == keys.LINENO)
                                 from a in context.CCUCUSTITEMs.Where(rec => rec.FILENO == keys.FILENO && rec.LINENO == b.ITEMLINENO.Value)
                                 select new { my103 = b, my105 = a }).FirstOrDefault();
            if (join105and103 != null)
            {
                if (join105and103.my105 != null)
                {
                    my105List.Add(join105and103.my105);
                }
            }

            return my105List;

        }
        public int FastDeleteMulti(CCUFILEMKeys parentEntityKeys)
        {
            var keys = parentEntityKeys as CCUFILEMKeys;
            return context.DeleteWhere<CCUCUSTITEM>(rec => rec.FILENO == keys.FILENO);
        }

        public List<CCUCUSTITEM> GetFile105(int? FILENO)
        {
            var my105List = new List<CCUCUSTITEM>();

            return (from a in context.CCUCUSTITEMs
                    where a.FILENO == FILENO
                    select a).ToList();

        }
    }
}
	 