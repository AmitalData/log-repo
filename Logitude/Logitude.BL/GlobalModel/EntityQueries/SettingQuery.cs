using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.Helpers;
using System.Transactions;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class SettingQuery
    {
        SettingRepository repository;

        public SettingQuery()
        {
            repository = new SettingRepository();
        }

        public SettingQuery(SettingRepository settingrepository)
        {
            repository = settingrepository;
        }

        public SettingPM GetSinglePM(string id)
        {
            SettingPM entity;
            entity = (from a in repository.context.Settings
                      where a.Id == id
                      select new SettingPM()
                      {
                          Id = a.Id,
                          //LogitudeURL = a.LogitudeURL,
                          //ChampURL = a.ChampURL,
                          DeploymentStage = a.DeploymentStage,
                          ChampEnv = a.ChampEnv,
                          WorkEnvironment = a.WorkEnvironment,
                          //CustomerCareIP = a.CustomerCareIP,
                          //TotangoServiceId = a.TotangoServiceId,
                          UsingAzure = a.UsingAzure,
                          //StorageAccountName = a.StorageAccountName,
                          //StorageAccountKey = a.StorageAccountKey,
                          IsLogEnabled = a.IsLogEnabled,
                          LogitudeCRMTenantNumber = a.LogitudeCRMTenantNumber,
                          //AutoSignupEmail = a.AutoSignupEmail,
                          //AutoSignupPassword = a.AutoSignupPassword,
                          CheckConnectionURL = a.CheckConnectionURL,
                          LogoCode = a.LogoCode,
                          //StorageType = a.StorageType,
                          GLSHKURL = a.GLSHKURL,
                          GLSHKEnv = a.GLSHKEnv,
                          CustomerTenantsURL = a.CustomerTenantsURL,
                          ForwarderTenantsURL = a.ForwarderTenantsURL,
                          QueueServiceMode = a.QueueServiceMode,
                          StorageServiceMode = a.StorageServiceMode,
                          HtmlVersion = a.HtmlVersion,
                          AndroidAppLink = a.AndroidAppLink,
                          IOSAppLink = a.IOSAppLink,
                          SameUserLoginEnabled = a.SameUserLoginEnabled,
                          DocumentFilingEmailDomain = a.DocumentFilingEmailDomain,
                          System2RedirectFraction = a.System2RedirectFraction,
                          ReportsRunUsingWR = a.ReportsRunUsingWR,

                      }).FirstOrDefault();

            return entity;
        }

        public SettingPM GetSinglePMFromCahche() => CacheHelper.GetFromCache("SettingPM", GetSinglePM);

        public SettingPM GetSinglePM()
        {
            
            SettingPM entity;
            try
            {
                var b = (from a in repository.context.Settings
                         select a
                          );
               
                    entity = (from a in repository.context.Settings
                              select new SettingPM()
                              {
                                  Id = a.Id,
                                  //LogitudeURL = a.LogitudeURL,
                                  //ChampURL = a.ChampURL,
                                  DeploymentStage = a.DeploymentStage,
                                  ChampEnv = a.ChampEnv,
                                  WorkEnvironment = a.WorkEnvironment,
                                  //CustomerCareIP = a.CustomerCareIP,
                                  //TotangoServiceId = a.TotangoServiceId,
                                  UsingAzure = a.UsingAzure,
                                  //StorageAccountName = a.StorageAccountName,
                                  //StorageAccountKey = a.StorageAccountKey,
                                  IsLogEnabled = a.IsLogEnabled,
                                  LogitudeCRMTenantNumber = a.LogitudeCRMTenantNumber,
                                  //AutoSignupEmail = a.AutoSignupEmail,
                                  //AutoSignupPassword = a.AutoSignupPassword,
                                  CheckConnectionURL = a.CheckConnectionURL,
                                  LogoCode = a.LogoCode,
                                  //StorageType = a.StorageType,
                                  GLSHKURL = a.GLSHKURL,
                                  GLSHKEnv = a.GLSHKEnv,
                                  CustomerTenantsURL = a.CustomerTenantsURL,
                                  ForwarderTenantsURL = a.ForwarderTenantsURL,
                                  QueueServiceMode = a.QueueServiceMode,
                                  StorageServiceMode = a.StorageServiceMode,
                                  HtmlVersion = a.HtmlVersion,
                                  AndroidAppLink = a.AndroidAppLink,
                                  IOSAppLink = a.IOSAppLink,
                                  SameUserLoginEnabled = a.SameUserLoginEnabled,
                                  DocumentFilingEmailDomain = a.DocumentFilingEmailDomain,
                                  System2RedirectFraction = a.System2RedirectFraction,
                                  ReportsRunUsingWR = a.ReportsRunUsingWR,
                                  ExportUrl = a.ExportUrl,
                              }).FirstOrDefault();
                 
                    return entity;
        
               
            }
            catch (Exception ex)
            {
                var b = ex;
                return null;
            }


           
        }

        public IQueryable<SettingPM> GetSettingPMsByTenant()
        {
            IQueryable<SettingPM> Settings = from a in repository.context.Settings
                                             select new SettingPM()
                                             {
                                                 Id = a.Id,
                                                 //LogitudeURL = a.LogitudeURL,
                                                 //ChampURL = a.ChampURL,
                                                 DeploymentStage = a.DeploymentStage,
                                                 ChampEnv = a.ChampEnv,
                                                 WorkEnvironment = a.WorkEnvironment,
                                                 //CustomerCareIP = a.CustomerCareIP,
                                                 //TotangoServiceId = a.TotangoServiceId,
                                                 UsingAzure = a.UsingAzure,
                                                 //StorageAccountName = a.StorageAccountName,
                                                 //StorageAccountKey = a.StorageAccountKey,
                                                 IsLogEnabled = a.IsLogEnabled,
                                                 LogitudeCRMTenantNumber = a.LogitudeCRMTenantNumber,
                                                 //AutoSignupEmail = a.AutoSignupEmail,
                                                 //AutoSignupPassword = a.AutoSignupPassword,
                                                 LogoCode = a.LogoCode,
                                                 //StorageType = a.StorageType,
                                                 GLSHKURL = a.GLSHKURL,
                                                 GLSHKEnv = a.GLSHKEnv,
                                                 CustomerTenantsURL = a.CustomerTenantsURL,
                                                 ForwarderTenantsURL = a.ForwarderTenantsURL,
                                                 QueueServiceMode = a.QueueServiceMode,
                                                 StorageServiceMode = a.StorageServiceMode,
                                                 HtmlVersion = a.HtmlVersion,
                                                 AndroidAppLink = a.AndroidAppLink,
                                                 IOSAppLink = a.IOSAppLink,
                                                 SameUserLoginEnabled = a.SameUserLoginEnabled,
                                                 DocumentFilingEmailDomain = a.DocumentFilingEmailDomain,
                                                 System2RedirectFraction = a.System2RedirectFraction,
                                                 ReportsRunUsingWR = a.ReportsRunUsingWR,
                                             };

            return Settings;
        }
    }
}