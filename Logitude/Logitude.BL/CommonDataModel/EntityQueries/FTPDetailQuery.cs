using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class FTPDetailQuery
    {
        FTPDetailRepository repository;

        public FTPDetailQuery()
        {
            repository = new FTPDetailRepository();
        }

        public FTPDetailQuery(int tenant)
        {
            repository = new FTPDetailRepository(tenant);
        }

        public FTPDetailQuery(FTPDetailRepository rep)
        {
            repository = rep;
        }

        public FTPDetailPM GetSinglePM(string id, int tenant)
        {
            FTPDetailPM detail = (from d in repository.context.FTPDetails
                                  where d.Id == id && d.Tenant == tenant
                                  select new FTPDetailPM()
                                  {
                                      Id = d.Id,
                                      Tenant = d.Tenant,
                                      UserName = d.UserName,
                                      Password = d.Password,
                                      Host = d.Host,
                                      Folder = d.Folder,
                                      CreateDate = d.CreateDate,
                                      UpdateDate = d.UpdateDate,
                                      CreatedByUserId = d.CreatedByUserId,
                                      UpdatedByUserId = d.UpdatedByUserId,
                                      InActive = d.InActive,
                                      UseSFTP = d.UseSFTP,
                                  }).FirstOrDefault();
            
            return detail;
        }

        public IQueryable<FTPDetailPM> GetFTPDetailsForSetting(int settingId, int tenant)
        {
            IQueryable<FTPDetailPM> result = (from d in repository.context.FTPDetails
                                                         where d.Tenant == settingId
                                                         select new FTPDetailPM()
                                                         {
                                                             Id = d.Id,
                                                             Tenant = d.Tenant,
                                                             UserName = d.UserName,
                                                             Password = d.Password,
                                                             Host = d.Host,
                                                             Folder = d.Folder,
                                                             CreateDate = d.CreateDate,
                                                             UpdateDate = d.UpdateDate,
                                                             CreatedByUserId = d.CreatedByUserId,
                                                             UpdatedByUserId = d.UpdatedByUserId,
                                                             InActive = d.InActive,
                                                             UseSFTP = d.UseSFTP,
                                                         });

            return result;
        }

        public IQueryable<FTPDetailList> GetIQueryableEntityList(IQueryable<FTPDetail> iQueryable)
        {
            IQueryable<FTPDetailList> result = from d in iQueryable
                                               select new FTPDetailList()
                                               {
                                                   Id = d.Id,
                                                   Tenant = d.Tenant,
                                                   UserName = d.UserName,
                                                   Password = d.Password,
                                                   Host = d.Host,
                                                   Folder = d.Folder,
                                                   CreateDate = d.CreateDate,
                                                   UpdateDate = d.UpdateDate,
                                                   CreatedByUserId = d.CreatedByUserId,
                                                   UpdatedByUserId = d.UpdatedByUserId,
                                                   InActive = d.InActive,
                                                   UseSFTP = d.UseSFTP,
                                               };
            return result;
        }
    }
}
