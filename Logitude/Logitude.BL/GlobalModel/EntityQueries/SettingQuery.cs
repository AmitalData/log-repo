using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class SettingQuery
    {
        IDictionary<string, object> cache = new Dictionary<string, object>();

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

        public SettingPM GetSinglePM()
        {
            SettingPM entity;
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
                      }).FirstOrDefault();

            return entity;
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

        public string GetJwtToken(int tenant)
        {
            string cacheKey = "jwt" + tenant;
            int expireDays = 1;

            if (cache.ContainsKey(cacheKey))
            {
                JwtCache jwtCache = (JwtCache)cache[cacheKey];
                if (jwtCache.expires.AddMinutes(-5) > DateTime.Now)
                    return jwtCache.jwt;
            }

            var claims = new[] { new Claim("tenant", tenant.ToString()) };
            string privateKey = "huosssssssssfioghsoahfroaguohgauiohfdfugioghsdahusfauiosfdayuiosgayuiosgayuiosgayuiosrgayuiosrgayuioyuiosrgahuiosgahuiotceapnu92567954678546"; // GetSinglePM().PrivateKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(null, null, claims, null, DateTime.Now.AddDays(expireDays), creds);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            cache[cacheKey] = new JwtCache() { jwt = tokenString, expires = DateTime.Now.AddDays(expireDays) };
            return tokenString;
        }
    }

    class JwtCache
    {
        public DateTime expires { get; set; }
        public string jwt { get; set; }
    }
}