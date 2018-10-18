using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ContactDoneMethodRepository:IRepository<ContactDoneMethod>
    {
        ICommonDataContext commonDataContext;
        
        public ContactDoneMethodRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ContactDoneMethodRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ContactDoneMethodRepository(ICommonDataContext context)
        {
            commonDataContext= context;
        }

        public IQueryable<ContactDoneMethod> GetContactDoneMethods()
        {
            return context.ContactDoneMethods;
        }

        public IQueryable<ContactDoneMethod> GetAll()
        {
            return context.ContactDoneMethods;
        }

        public ContactDoneMethod GetSingleContactDoneMethod(string code)
        {
            return (from a in context.ContactDoneMethods
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(ContactDoneMethod entity)
        {
            context.ContactDoneMethods.Add(entity);
        }

        public void Remove(ContactDoneMethod entity)
        {
            try
            {
                context.ContactDoneMethods.Attach(entity);
            }
            catch { }
            context.ContactDoneMethods.Remove(entity);
        }

        public void Update(ContactDoneMethod entity)
        {
            try
            {
                context.ContactDoneMethods.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ContactDoneMethod> All()
        {
            return context.ContactDoneMethods.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ContactDoneMethod> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ContactDoneMethod GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
