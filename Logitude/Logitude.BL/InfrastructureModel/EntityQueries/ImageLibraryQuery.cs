using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

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

        public List<ImageLibraryList> GetAllForTenant(int tenant)
        {
            var domain = LogitudeSettings.LogitudeURL;
            List<ImageLibraryList> result = repository.context.ImageLibraries.Include(a => a.ImageDetail)
                .Where(a => a.Tenant == tenant  || a.Tenant == 0).Select(entity => new ImageLibraryList
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
                    URL = domain + "/WebPages/ImageLibraryDownloadPage.aspx?securityId=" + entity.SecurityId + "&tenant=" + tenant,
                    Extension = entity.ImageDetail.Extension

                }).ToList();

            return result;
        }
    }
}
