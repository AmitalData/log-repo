using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UsersReleaseNotesDisplayRepository : IRepository<UsersReleaseNotesDisplay>
    {
        ICommonDataContext commonDataContext;

        public UsersReleaseNotesDisplayRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public UsersReleaseNotesDisplayRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<UsersReleaseNotesDisplay> GetUsersReleaseNotesDisplays(int tenant)
        {
            return (from record in context.UsersReleaseNotesDisplays where record.Tenant == tenant select record);
        }

        public UsersReleaseNotesDisplay GetSingleUsersReleaseNotesDisplay(string id, int tenant)
        {
            return (from record in context.UsersReleaseNotesDisplays where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public UsersReleaseNotesDisplay GetSingleUsersReleaseNotesDisplayByUserId(string userId, int tenant)
        {
            return (from record in context.UsersReleaseNotesDisplays where record.UserId == userId && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(UsersReleaseNotesDisplay entity)
        {
            this.context.UsersReleaseNotesDisplays.Add(entity);
        }

        public void Remove(UsersReleaseNotesDisplay entity)
        {
            try
            {
                this.context.UsersReleaseNotesDisplays.Attach(entity);
            }
            catch { }
            this.context.UsersReleaseNotesDisplays.Remove(entity);

        }

        public void Update(UsersReleaseNotesDisplay entity)
        {
            try
            {
                this.context.UsersReleaseNotesDisplays.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);

        }

        public List<UsersReleaseNotesDisplay> All()
        {
            return this.context.UsersReleaseNotesDisplays.ToList<UsersReleaseNotesDisplay>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }
        
        public List<UsersReleaseNotesDisplay> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public UsersReleaseNotesDisplay GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
