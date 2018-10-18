using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.WebServices
{
    public class ExportImportHelper
    {
        public string ImportRoleFeatures(byte[] data)
        {
            string ErrorMessage = "";
            string datastring = Encoding.ASCII.GetString(data);

            List<string> allUpdatedRolesIds = new List<string>();

            RoleRepository roleRep = new RoleRepository(0);
            RoleFeatureRepository rolefeaturerep = new RoleFeatureRepository(0);
            FeatureRepository featurrep = new FeatureRepository(0);
            ObjectTableRepository objecttablerep = new ObjectTableRepository(0);
            List<Role> roles = roleRep.GetRoles(0).ToList();
            Dictionary<string, bool> roleFeatureDictionary = new Dictionary<string, bool>();

            using (StringReader reader = new StringReader(datastring))
            {
                string line;
                bool roleFeaturesDeleted = false;
                int counter = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string[] linedata = line.Split(',');

                        string roleCode = linedata[0];
                        string objecttablename = linedata[1];
                        string featurecode = linedata[2];
                        string featureAccessLevelCode = linedata[3];

                        ObjectTable table = objecttablerep.GetObjectTableByName(objecttablename, 0, true);
                        Feature feature = featurrep.GetSingleFeatureByCode(table.Id, featurecode, 0);
                        Role role = roles.Where(d => d.Code == roleCode).FirstOrDefault();

                        if (role != null)
                        {
                            if (!allUpdatedRolesIds.Contains(role.Id))
                            {
                                allUpdatedRolesIds.Add(role.Id);
                            }

                            #region
                            if (!roleFeatureDictionary.Keys.Contains(role.Id))
                            {
                                roleFeatureDictionary.Add(role.Id, false);
                            }

                            roleFeaturesDeleted = roleFeatureDictionary[role.Id];
                            if (!roleFeaturesDeleted)
                            {
                                rolefeaturerep.DeleteAllRoleFeaturesByRole(role.Id);
                                rolefeaturerep.SubmitChanges();
                                roleFeatureDictionary[role.Id] = true;
                            }

                            if (feature != null)
                            {
                                FeatureQuery featureQuery = new FeatureQuery(featurrep);
                                FeaturePM currentFeature = featureQuery.GetSingleFeaturePM(feature.Id);

                                if (!string.IsNullOrEmpty(currentFeature.RoleId))
                                {
                                    if (CacheManager.CacheWrapper.Get(currentFeature.RoleId) != null)
                                    {
                                        CacheManager.CacheWrapper.Invalidate(currentFeature.RoleId);
                                    }
                                }

                                else if (!string.IsNullOrEmpty(currentFeature.PackageCode))
                                {
                                    if (CacheManager.CacheWrapper.Get(currentFeature.PackageCode) != null)
                                    {
                                        CacheManager.CacheWrapper.Invalidate(currentFeature.PackageCode);
                                    }
                                }

                                string listName = "featureslist" + currentFeature.RoleId + currentFeature.Tenant;

                                if (CacheManager.CacheWrapper.Get(listName) != null)
                                {
                                    CacheManager.CacheWrapper.Invalidate(listName);
                                }

                                RoleFeature rolefeature = rolefeaturerep.GetRoleFeatureByRoleAndFeature(role.Id, feature.Id, 0);
                                if (rolefeature == null)
                                {
                                    rolefeature = new RoleFeature()
                                    {
                                        Id = IdCounter.GetNumber("RoleFeature", 0),
                                        FeatureId = feature.Id,
                                        RoleId = role.Id,
                                        Tenant = 0,
                                        FeatureAccessLevelCode = featureAccessLevelCode,
                                    };

                                    rolefeaturerep.Add(rolefeature);
                                    counter++;
                                }

                                if (counter == 500)
                                {
                                    counter = 0;
                                    rolefeaturerep.SubmitChanges();
                                }
                            }

                            #endregion
                        }
                    }

                    catch (Exception ex)
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                        {
                            ErrorMessage += Environment.NewLine + GetExceptionMessage(ex);
                        }

                        else
                        {
                            ErrorMessage = GetExceptionMessage(ex);
                        }

                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "Import role features from csv", "", null);
                    }
                }

                try
                {
                    rolefeaturerep.SubmitChanges();

                    if (allUpdatedRolesIds.Count > 0)
                    {
                        int tenant = 0;
                        string email = HttpContext.Current.User.Identity.Name;
                        string loggedUserId = this.GetLoggedUserId(email, tenant);

                        FeatureChangeRepository myRepository = new FeatureChangeRepository(tenant);

                        foreach (string myRoleId in allUpdatedRolesIds)
                        {
                            FeatureChange myFeatureChange = new FeatureChange()
                            {
                                Tenant = tenant,
                                Id = IdCounter.GetNumber("FeatureChange", tenant).ToString(),
                                EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                                Name = "Role features updated from a file",
                                UserId = loggedUserId,
                                RoleId = myRoleId,
                            };

                            myFeatureChange.SearchFields = myFeatureChange.Name;
                            myRepository.Add(myFeatureChange);
                        }

                        myRepository.SubmitChanges();
                    }
                }

                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        ErrorMessage += Environment.NewLine + GetExceptionMessage(ex);
                    }
                    else
                    {
                        ErrorMessage = GetExceptionMessage(ex);
                    }

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "Import role features from csv", "", null);
                }

                return ErrorMessage;
            }
        }
        public string ImportPackageFeatures(byte[] data)
        {
            string ErrorMessage = "";
            string datastring = Encoding.ASCII.GetString(data);

            List<string> allUpdatedPackagesCodes = new List<string>();

            PackageFeatureRepository packfeaturerep = new PackageFeatureRepository(0);
            FeatureRepository featurrep = new FeatureRepository(0);
            ObjectTableRepository objecttablerep = new ObjectTableRepository(0);
            PackageFeatureRepository packageFeatureRep = new PackageFeatureRepository(0);
            PackageRepository packageRepository = new PackageRepository(0);
            List<Package> packages = packageRepository.GetPackages().ToList();

            using (StringReader reader = new StringReader(datastring))
            {

                string line;
                bool packageFeaturesDeleted = false;
                Dictionary<string, bool> packageFeaturesDeletedDictionary = new Dictionary<string, bool>();
                int counter = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string[] linedata = line.Split(',');

                        string packagecode = linedata[0];
                        Package package = packages.Where(d => d.Code == packagecode).FirstOrDefault();
                        if (package != null)
                        {
                            if (!allUpdatedPackagesCodes.Contains(package.Code))
                            {
                                allUpdatedPackagesCodes.Add(package.Code);
                            }

                            #region
                            string objecttablename = linedata[1];
                            string featurecode = linedata[2];
                            if (!packageFeaturesDeletedDictionary.Keys.Contains(packagecode))
                            {
                                packageFeaturesDeletedDictionary.Add(packagecode, false);
                            }
                            packageFeaturesDeleted = packageFeaturesDeletedDictionary[packagecode];
                            if (!packageFeaturesDeleted)
                            {
                                packageFeatureRep.DeleteAllPackageFeaturesByPackage(packagecode);
                                packageFeatureRep.SubmitChanges();
                                packageFeaturesDeletedDictionary[packagecode] = true;
                            }

                            ObjectTable table = objecttablerep.GetObjectTableByName(objecttablename, 0, true);
                            Feature feature = featurrep.GetSingleFeatureByCode(table.Id, featurecode, 0);


                            if (feature != null)
                            {
                                FeatureQuery featureQuery = new FeatureQuery(featurrep);
                                FeaturePM currentFeature = featureQuery.GetSingleFeaturePM(feature.Id);

                                if (!string.IsNullOrEmpty(currentFeature.RoleId))
                                {
                                    if (CacheManager.CacheWrapper.Get(currentFeature.RoleId) != null)
                                    {
                                        CacheManager.CacheWrapper.Invalidate(currentFeature.RoleId);
                                    }
                                }

                                else if (!string.IsNullOrEmpty(currentFeature.PackageCode))
                                {
                                    if (CacheManager.CacheWrapper.Get(currentFeature.PackageCode) != null)
                                    {
                                        CacheManager.CacheWrapper.Invalidate(currentFeature.PackageCode);
                                    }
                                }

                                string listName = "featureslist" + currentFeature.RoleId + currentFeature.Tenant;

                                if (CacheManager.CacheWrapper.Get(listName) != null)
                                {
                                    CacheManager.CacheWrapper.Invalidate(listName);
                                }


                                PackageFeature packagefeature = packfeaturerep.GetSinglePackageFeatureByPackageAndFeature(packagecode, feature.Id, 0);
                                if (packagefeature == null)
                                {
                                    packagefeature = new PackageFeature()
                                    {
                                        Id = IdCounter.GetNumber("PackageFeature", 0),
                                        FeatureId = feature.Id,
                                        PackageCode = packagecode,
                                        Tenant = 0,
                                    };

                                    packfeaturerep.Add(packagefeature);

                                    counter++;
                                }

                                if (counter == 500)
                                {
                                    counter = 0;
                                    packfeaturerep.SubmitChanges();

                                }
                            }

                            #endregion
                        }
                    }

                    catch (Exception ex)
                    {
                        if (!string.IsNullOrEmpty(ErrorMessage))
                        {
                            ErrorMessage += Environment.NewLine + GetExceptionMessage(ex);
                        }
                        else
                        {
                            ErrorMessage = GetExceptionMessage(ex);
                        }

                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "Import package features from csv", "", null);
                    }
                }

                try
                {
                    packfeaturerep.SubmitChanges();

                    if (allUpdatedPackagesCodes.Count > 0)
                    {
                        int tenant = 0;
                        string email = HttpContext.Current.User.Identity.Name;
                        string loggedUserId = this.GetLoggedUserId(email, tenant);

                        FeatureChangeRepository myRepository = new FeatureChangeRepository(tenant);

                        foreach (string myPackageCode in allUpdatedPackagesCodes)
                        {
                            FeatureChange myFeatureChange = new FeatureChange()
                            {
                                Tenant = tenant,
                                Id = IdCounter.GetNumber("FeatureChange", tenant).ToString(),
                                EventDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                                Name = "Package features updated from a file",
                                UserId = loggedUserId,
                                PackageCode = myPackageCode,
                            };

                            myFeatureChange.SearchFields = myFeatureChange.Name;
                            myRepository.Add(myFeatureChange);
                        }

                        myRepository.SubmitChanges();
                    }
                }

                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(ErrorMessage))
                    {
                        ErrorMessage += Environment.NewLine + GetExceptionMessage(ex);
                    }
                    else
                    {
                        ErrorMessage = GetExceptionMessage(ex);
                    }

                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "Import package features from csv", "", null);
                }
            }
                       
            return ErrorMessage;
        }
        private string GetExceptionMessage(Exception ex)
        {
            string ErrorMessage = ex.Message;

            if (ex.InnerException != null)
            {
                ErrorMessage += Environment.NewLine + ex.InnerException.Message;

                if (ex.InnerException.InnerException != null)
                {
                    ErrorMessage += Environment.NewLine + ex.InnerException.InnerException.Message;

                    if (ex.InnerException.InnerException.InnerException != null)
                    {
                        ErrorMessage += Environment.NewLine + ex.InnerException.InnerException.InnerException.Message;
                    }
                }

            }
            return ErrorMessage;
        }
        private string GetLoggedUserId(string loggedUserEmail, int tenant)
        {
            string loggedUserId = null;
            ContactQuery contactQuery = new ContactQuery(tenant);
            ContactPM loggedContactPM = contactQuery.GetContactByNameAndTenant(loggedUserEmail, tenant, true);
            if (loggedContactPM == null)
            {
                loggedContactPM = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
            }

            if (loggedContactPM != null)
            {
                loggedUserId = loggedContactPM.Id;
            }

            return loggedUserId;
        }

    }
}