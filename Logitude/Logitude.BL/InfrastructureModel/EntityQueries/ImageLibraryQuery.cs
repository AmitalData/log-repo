using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ImageLibraryQuery
    {
        ImageLibraryRepository repository;

        public ImageLibraryQuery()
        {
            repository = new ImageLibraryRepository();
        }

        public ImageLibraryQuery(int tenant)
        {
            repository = new ImageLibraryRepository(tenant);
        }

        public ImageLibraryQuery(ImageLibraryRepository repository)
        {
            this.repository = repository;
        }

        public ImageLibraryPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                id = Regex.Replace(id, " ", "+");
            }
            ImageLibraryPM imageLibraryPM = (from entity in repository.context.ImageLibraries.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact").Include("Document")
                                             where entity.Id == id && entity.Tenant == tenant
                                             select new ImageLibraryPM()
                                             {
                                                 Id = entity.Id,
                                                 ImageDetailId = entity.ImageDetailId,
                                                 Name = entity.Name,
                                                 CreatedByUserId = entity.CreatedByUserId,
                                                 CreateDate = entity.CreateDate,
                                                 SearchFields = entity.SearchFields,
                                                 UpdateDate = entity.UpdateDate,
                                                 UpdatedByUserId = entity.UpdatedByUserId,
                                                 SecurityId = entity.SecurityId,
                                                 Tenant = entity.Tenant,
                                             }).FirstOrDefault();

            return imageLibraryPM;
        }

        public IQueryable<ImageLibraryList> GetIQueryableEntityList(IQueryable<ImageLibrary> iQueryable)
        {
            IQueryable<ImageLibraryList> result = from entity in iQueryable
                                                    select new ImageLibraryList()
                                                    {
                                                        Id = entity.Id,
                                                        ImageDetailId = entity.ImageDetailId,
                                                        Name = entity.Name,
                                                        CreatedByUserId = entity.CreatedByUserId,
                                                        CreateDate = entity.CreateDate,
                                                        SearchFields = entity.SearchFields,
                                                        UpdateDate = entity.UpdateDate,
                                                        UpdatedByUserId = entity.UpdatedByUserId,
                                                        SecurityId = entity.SecurityId,
                                                        Tenant = entity.Tenant,
                                                    };
            return result;
        }
    }
}
