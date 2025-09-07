using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class MentionQuery
    {
        MentionRepository repository;



        public MentionQuery(int tenant)
        {
            repository = new MentionRepository(tenant);
        }

        public MentionQuery(MentionRepository repository)
        {
            this.repository = repository;
        }
        public MentionPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                id = Regex.Replace(id, " ", "+");
            }
            MentionPM MentionPM = (from entity in repository.context.Mentions.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                                   where entity.Id == id && entity.Tenant == tenant
                                   select new MentionPM()
                                   {
                                       Id = entity.Id,
                                       Tenant = entity.Tenant,
                                       CreatedByUserId = entity.CreatedByUserId,
                                       CreateDate = entity.CreateDate,
                                       UpdateDate = entity.UpdateDate,
                                       UpdatedByUserId = entity.UpdatedByUserId,
                                       SearchFields = entity.SearchFields,
                                       Name = entity.Name,
                                       Description = entity.Description,
                                       InActive = entity.InActive,
                                   }).FirstOrDefault();

            return MentionPM;
        }

        public IQueryable<MentionList> GetIQueryableEntityList(IQueryable<Mention> iQueryable)
        {
            IQueryable<MentionList> result = from entity in iQueryable.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                                             select new MentionList()
                                             {
                                                 Id = entity.Id,
                                                 Tenant = entity.Tenant,
                                                 CreatedByUserId = entity.CreatedByUserId,
                                                 CreateDate = entity.CreateDate,
                                                 UpdateDate = entity.UpdateDate,
                                                 UpdatedByUserId = entity.UpdatedByUserId,
                                                 SearchFields = entity.SearchFields,
                                                 Name = entity.Name,
                                                 Description = entity.Description,
                                                 InActive = entity.InActive,
                                                 CreatedByUserName = entity.CreatedByUser != null && entity.CreatedByUser.Contact != null ? entity.CreatedByUser.Contact.EnglishName : null,
                                                 UpdatedByUserName = entity.UpdatedByUser != null && entity.UpdatedByUser.Contact != null ? entity.UpdatedByUser.Contact.EnglishName : null,
                                             };

            return result;
        }
    }
}
