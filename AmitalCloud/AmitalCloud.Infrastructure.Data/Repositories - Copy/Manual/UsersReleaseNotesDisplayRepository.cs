using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class UsersReleaseNotesDisplayRepository : IRepository<UsersReleaseNotesDisplay,string>
    {
        IAmitalCloudContext currentContext;

        public UsersReleaseNotesDisplayRepository()
        {
            currentContext = new AmitalCloudContext();
        }

        public UsersReleaseNotesDisplayRepository(IAmitalCloudContext context)
        {
            currentContext = context;
        }

        public UsersReleaseNotesDisplayRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
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

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }
        
        public List<UsersReleaseNotesDisplay> GetMulti(IEntityKeyFields<UsersReleaseNotesDisplay,string> entityKeys)
        {
            throw new NotImplementedException();
        }

        public UsersReleaseNotesDisplay GetSingle(IEntityKeyFields<UsersReleaseNotesDisplay,string> entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
