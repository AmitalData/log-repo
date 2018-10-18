using System.Linq;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public IQueryable<UserLoginLog> GetUserLoginLogs(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            userLoginLogRepository = new UserLoginLogRepository(tenant);
            return userLoginLogRepository.GetUserLoginLogs(tenant);
        }

        public IQueryable<UserLoginLogPM> GetUserLoginLogPMsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            userLoginLogQuery = new UserLoginLogQuery(tenant);
            return userLoginLogQuery.GetUserLoginLogPMsByTenant(tenant);
        }

        public void InsertUserLoginLog(UserLoginLogPM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }
            UserLoginLogService service = new UserLoginLogService(objectContext, entityPm.Tenant);
            service.Create(entityPm);

            //userLoginLogRepository = new UserLoginLogRepository(objectContext);
            //UserLoginLog newEntity = new UserLoginLog();
            //newEntity.Id = IdCounter.GetNumber("UserLoginLog", entityPm.Tenant).ToString();
            //entityPm.Id = newEntity.Id;
            //MapUserLoginLogUserLoginLogPM(entityPm, newEntity);
            //userLoginLogRepository.Add(newEntity);
        }

        public void UpdateUserLoginLogDate(UserLoginLogPM currententityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currententityPm.Tenant);
            }
            UserLoginLogService service = new UserLoginLogService(objectContext, currententityPm.Tenant);
            service.Update(currententityPm);

            //userLoginLogRepository = new UserLoginLogRepository(objectContext);
            //UserLoginLog updatedEntity = userLoginLogRepository.GetSingleUserLoginLog(currententityPm.Id, currententityPm.Tenant, false);
            //MapUserLoginLogUserLoginLogPM(currententityPm, updatedEntity);
            //userLoginLogRepository.Update(updatedEntity);
        }

        public void DeleteUserLoginLog(UserLoginLog entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            userLoginLogRepository = new UserLoginLogRepository(objectContext);
            userLoginLogRepository.Remove(entity);
        }

        //public void MapUserLoginLogUserLoginLogPM(UserLoginLogPM userLoginLogPm, UserLoginLog userLoginLog)
        //{
        //    userLoginLog.Tenant = userLoginLogPm.Tenant;
        //    userLoginLog.IP = userLoginLogPm.IP;
        //    userLoginLog.Browser = userLoginLogPm.Browser;
        //    userLoginLog.UserId = userLoginLogPm.UserId;
        //    userLoginLog.GMTDateTime = userLoginLogPm.GMTDateTime;
        //    userLoginLog.LocalDateTime = userLoginLogPm.LocalDateTime;
        //    userLoginLog.UserAgent = userLoginLogPm.UserAgent;
        //}
    }
}