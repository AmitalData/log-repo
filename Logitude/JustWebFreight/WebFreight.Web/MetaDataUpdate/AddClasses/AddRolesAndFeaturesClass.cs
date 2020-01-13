using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddRolesAndFeaturesClass
    {

        public static Role AddRole(RoleDetails roleDetails, RoleRepository roleRepository, Dictionary<string, Role> tenantRoles)
        {
            if (tenantRoles.Keys.Contains(roleDetails.Code))
            {
                Role updatedRole = tenantRoles[roleDetails.Code];
                updatedRole.Name = roleDetails.Name;
                updatedRole.Tenant = roleDetails.Tenant;
                updatedRole.RoleTypeCode = roleDetails.RoleTypeCode;
                updatedRole.Description = roleDetails.Description;
                roleRepository.Update(updatedRole);
                return updatedRole;
            }

            else
            {
                Role newRole = new Role()
                {
                    Tenant = roleDetails.Tenant,
                    Name = roleDetails.Name,
                    Code = roleDetails.Code,
                    RoleTypeCode = roleDetails.RoleTypeCode,
                    Description = roleDetails.Description,

                    Id = IdCounter.GetNumber("Role", roleDetails.Tenant).ToString(),
                };

                roleRepository.Add(newRole);
                return newRole;
            }
        }

        public static Feature AddFeature(FeatureDetails featureDetails, FeatureRepository featuresRepository, TextCodeRepository textCodeReposit, Dictionary<string, Feature> tenantFearures, Dictionary<string, TextCode> textCodes)
        {

            ObjectTableRepository Repo = new ObjectTableRepository(0);
            var table = Repo.GetSingleObjectTable(featureDetails.ObjectTableId, featureDetails.Tenant, false);
            string NewKey = "";
            if (ObjectTablesKeys.Keys.ContainsKey(table.Name))
            {
                NewKey = ObjectTablesKeys.Keys[table.Name];
            }
            //if (table.UpdateKey != NewKey)
            //{
            featureDetails.Code = featureDetails.Code.Trim();
            featureDetails.FeatureUniqeCode = table.Name + "." + featureDetails.Code;

            if (featureDetails.FeatureTypeCode == "MODL")
            {
                featureDetails.Packagable = true;
            }

            if (featureDetails.Code == "UPDATE" || featureDetails.Code == "READ" || featureDetails.Code == "NEW")
            {
                featureDetails.Packagable = false;
            }

            if (tenantFearures.Keys.Contains(featureDetails.Code + featureDetails.ObjectTableId))
            {
                Feature updatedFeature = tenantFearures[featureDetails.Code + featureDetails.ObjectTableId];
                updatedFeature.ObjectTableId = featureDetails.ObjectTableId;
                updatedFeature.Tenant = featureDetails.Tenant;
                updatedFeature.FeatureTypeCode = featureDetails.FeatureTypeCode;
                updatedFeature.Code = featureDetails.Code;
                updatedFeature.Packagable = featureDetails.Packagable;
                //updatedFeature.IsBusinessUnitEnabled = featureDetails.IsBusinessUnitEnabled;
                updatedFeature.FeatureUniqeCode = featureDetails.FeatureUniqeCode;

                TextCode updatedTextCode = null;
                if (textCodes.Keys.Contains(featureDetails.NameTextCodeCode + featureDetails.Tenant + featureDetails.ObjectTableId))
                {
                    updatedTextCode = textCodes[featureDetails.NameTextCodeCode + featureDetails.Tenant + featureDetails.ObjectTableId];
                }

                if (updatedTextCode == null)
                {
                    updatedTextCode = new TextCode()
                    {
                        Id = IdCounter.GetNumber("TextCode", featureDetails.Tenant).ToString(),
                        Tenant = featureDetails.Tenant,
                        ObjectTableId = featureDetails.ObjectTableId,
                        DefaultText = featureDetails.NameTextCodeDefaultText,
                        Code = featureDetails.NameTextCodeCode,
                        TextCodeTypeCode = "O",
                    };

                        updatedFeature.NameTextCodeId = updatedTextCode.Id;
                        updatedFeature.NameTextCodeCode = updatedTextCode.Code;
                        textCodeReposit.Add(updatedTextCode);
                    }

                else
                {
                    updatedTextCode = textCodes[featureDetails.NameTextCodeCode + featureDetails.Tenant + featureDetails.ObjectTableId];
                    updatedTextCode.DefaultText = featureDetails.NameTextCodeDefaultText;
                    updatedTextCode.ObjectTableId = featureDetails.ObjectTableId;
                    updatedTextCode.Tenant = featureDetails.Tenant;
                    updatedTextCode.InActive = false;

                    textCodeReposit.Update(updatedTextCode);//ORA-02291: אילוץ כלילות (AMINET_MAIN.FK_919762609) הופר - מפתח אב לא נמצא
                    

                }
                featuresRepository.Update(updatedFeature);
                //table.UpdateKey = NewKey;
                //Repo.Update(table);
                //Repo.SubmitChanges();
                return updatedFeature;
            }

            else
            {
                TextCode newTextCode = null;
                if (textCodes.Keys.Contains(featureDetails.NameTextCodeCode + featureDetails.Tenant + featureDetails.ObjectTableId))
                {
                    newTextCode = textCodes[featureDetails.NameTextCodeCode + featureDetails.Tenant + featureDetails.ObjectTableId];
                }

                if (newTextCode == null)
                {
                    newTextCode = new TextCode()
                    {
                        Id = IdCounter.GetNumber("TextCode", featureDetails.Tenant).ToString(),
                        Tenant = featureDetails.Tenant,
                        ObjectTableId = featureDetails.ObjectTableId,
                        DefaultText = featureDetails.NameTextCodeDefaultText,
                        Code = featureDetails.NameTextCodeCode.Trim(),
                        TextCodeTypeCode = "O",
                    };

                    textCodeReposit.Add(newTextCode);
                    textCodes.Add(featureDetails.NameTextCodeCode + featureDetails.Tenant + featureDetails.ObjectTableId, newTextCode);
                }

                    Feature newFeature = new Feature()
                    {
                        Id = IdCounter.GetNumber("Feature", featureDetails.Tenant).ToString(),
                        Tenant = featureDetails.Tenant,
                        ObjectTableId = featureDetails.ObjectTableId,
                        Code = featureDetails.Code.Trim(),
                        NameTextCodeId = newTextCode.Id,
                        NameTextCodeCode = newTextCode.Code,
                        FeatureTypeCode = featureDetails.FeatureTypeCode,
                        Packagable = featureDetails.Packagable,
                        IsBusinessUnitEnabled = featureDetails.IsBusinessUnitEnabled,
                        IsOld = false,
                        IsCoreFeature = featureDetails.IsCoreFeature,
                        FeatureUniqeCode = featureDetails.FeatureUniqeCode
                    };

                featuresRepository.Add(newFeature);
                //table.UpdateKey = NewKey;
                //Repo.Update(table);
                //Repo.SubmitChanges();
                return newFeature;
            }
            //}
            //else
            //{
            //    Feature updatedFeature = tenantFearures[featureDetails.Code + featureDetails.ObjectTableId];
            //    return updatedFeature;
            //}

        }

        //public static void AddRoleFeature(string featureId, string roleId, RoleFeatureRepository roleFeatureRepository, List<RoleFeature> roleFeatures,int tenant)
        //{
        //    bool exists = (from a in roleFeatures
        //                   where a.FeatureId == featureId && a.RoleId == roleId
        //                   select a).Any();

        //    if (!exists)
        //    {
        //        RoleFeature newRoleFeature = new RoleFeature()
        //        {
        //            RoleId = roleId,
        //            FeatureId = featureId,
        //            Id = IdCounter.GetNumber("RoleFeature",tenant).ToString(),
        //            Tenant = tenant,
        //            FeatureAccessLevelCode = "OR",
        //        };
        //        roleFeatureRepository.Add(newRoleFeature);
        //    }
        //}

    }
}