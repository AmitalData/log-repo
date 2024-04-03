using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Newtonsoft.Json;
using Simplog.Server.Infrastructure.DataContracts.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Logitude.BL.Helpers
{
    public class DigitalFieldSecuritesHelper
    {
        public List<DigitalTextCodeObject> GetDigitalTextCodeObjects(int tenant, string objectTableId, string profileCode, bool isTranslation, string LanguageCode)
        {
            DigitalTextCodeRepository digitalTextCodeRepository = new DigitalTextCodeRepository(tenant);

            if (profileCode == "null")
            {
                profileCode = "CS";
            }

            var defaultTextCode = digitalTextCodeRepository.GetDigitalTextCodes(0, objectTableId, profileCode, LanguageCode).FirstOrDefault();

            if (defaultTextCode == null)
            {
                return new List<DigitalTextCodeObject>();
            }

            var defaultCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(defaultTextCode.Labels);

            var customCodesObject = new List<DigitalTextCodeObject>();

            if (tenant != 0)
            {
                var customTextCodes = digitalTextCodeRepository.GetDigitalTextCodes(tenant, objectTableId, profileCode, LanguageCode).FirstOrDefault();

                if (customTextCodes != null)
                {
                    customCodesObject = JsonConvert.DeserializeObject<List<DigitalTextCodeObject>>(customTextCodes.Labels);
                }
                else
                {
                    return  defaultCodesObject;
                }
            }

            if (isTranslation)
            {
                foreach (var item in customCodesObject)
                {
                    var data = defaultCodesObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));
                    if (data != null)
                    {
                        if (!string.IsNullOrWhiteSpace(item.DisplayText))
                        {
                            data.DefaultText = item.DisplayText;
                        }
                        continue;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(item.DisplayText))
                        {
                            item.DefaultText = item.DisplayText;
                        }

                        defaultCodesObject.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in customCodesObject)
                {
                    var temp = defaultCodesObject.FirstOrDefault(a => a.TextCode.Equals(item.TextCode));

                    if (temp != null)
                    {
                        temp.DisplayText = item.DisplayText;
                    }
                    else
                    {
                        defaultCodesObject.Add(item);
                    }
                }
            }

            return defaultCodesObject;
        }

        public List<DigitalFeildSecurityObject> GitDigitalSecuritesFeilds(string objectTableId, string profileCode, int tenant, bool singleApi = true, bool isAll = false)
        {
            var digitalFieldSecurity = GetDigitalFieldSecurityQuery(0, objectTableId, profileCode);

            if (digitalFieldSecurity == null)
            {
                return new List<DigitalFeildSecurityObject>();
            }

            var defaultDigitalFieldSecurity = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(digitalFieldSecurity?.DefaultSettings);
            var customDigitalFeildSecurityObject = new List<DigitalFeildSecurityObject>();

            if (tenant != 0)
            {
                var customDigitalFieldSecurityList = GetDigitalFieldSecurityQuery(tenant, objectTableId, profileCode);

                if (customDigitalFieldSecurityList != null)
                {
                    customDigitalFeildSecurityObject = JsonConvert.DeserializeObject<List<DigitalFeildSecurityObject>>(customDigitalFieldSecurityList?.DefaultSettings);

                    foreach (var item in customDigitalFeildSecurityObject)
                    {
                        var temp = defaultDigitalFieldSecurity.FirstOrDefault(a => a.FieldCode.Equals(item.FieldCode, StringComparison.InvariantCultureIgnoreCase));

                        if (temp != null)
                        {
                            temp.HasPermission = item.HasPermission;
                            temp.CreatedBy = item.CreatedBy;
                            temp.CreatedOn = item.CreatedOn;
                            temp.ModifiedOn = item.ModifiedOn;
                            temp.ModifiedBy = item.ModifiedBy;
                            temp.IsPm = item.IsPm;
                            temp.IsList = item.IsList;
                            continue;
                        }
                        else
                        {
                            defaultDigitalFieldSecurity.Add(item);
                        }
                    }
                }
                else
                {
                    if (!isAll)
                    {
                        if (!singleApi)
                        {
                            defaultDigitalFieldSecurity = DiscardUnfoundFeildsFromList(defaultDigitalFieldSecurity);
                        }
                        else
                        {
                            defaultDigitalFieldSecurity = DiscardUnfoundFeildsFromObject(defaultDigitalFieldSecurity);
                        }
                    }

                    return defaultDigitalFieldSecurity;
                }
            }

            if (!isAll)
            {
                if (!singleApi)
                {
                    defaultDigitalFieldSecurity = DiscardUnfoundFeildsFromList(defaultDigitalFieldSecurity);
                }
                else
                {
                    defaultDigitalFieldSecurity = DiscardUnfoundFeildsFromObject(defaultDigitalFieldSecurity);
                }
            }

            if (!customDigitalFeildSecurityObject.Any())
            {
                return defaultDigitalFieldSecurity;
            }

            return defaultDigitalFieldSecurity;
        }

        public DigitalFieldSecurityList GetDigitalFieldSecurityQuery(int tenant, string objectTableId, string profileCode)
        {
            DigitalFieldSecurityRepository digitalFieldSecurityRepository = new DigitalFieldSecurityRepository(tenant);
            var digitalFieldSecurity = digitalFieldSecurityRepository.GetDigitalFieldSecurity(tenant, objectTableId, profileCode)
                                                                     .Select(x => new DigitalFieldSecurityList
                                                                     {
                                                                         Id = x.Id,
                                                                         ObjectTableId = x.ObjectTableId,
                                                                         Tenant = x.Tenant,
                                                                         DefaultSettings = x.DefaultSettings,
                                                                         CreateDate = x.CreateDate,
                                                                         UpdateDate = x.UpdateDate,
                                                                         ProfileId = x.ProfileId
                                                                     })
                                                                     .FirstOrDefault();
            return digitalFieldSecurity;
        }

        public bool CheckIfFieldInuse(CheckObjectFieldExistenceRequest checkObjectFieldExistenceRequest, int tenant)
        {
            var helper = new DigitalFieldSecuritesHelper();
            var defaultDigitalFieldSecurity = helper.GitDigitalSecuritesFeilds(checkObjectFieldExistenceRequest.ObjectTableId,
                                                                               checkObjectFieldExistenceRequest.ProfileCode, tenant, false, true);

            var defaultDigitalFieldTenant0 = helper.GitDigitalSecuritesFeilds(checkObjectFieldExistenceRequest.ObjectTableId,
                                                                               checkObjectFieldExistenceRequest.ProfileCode, 0, false, true);

            var res = false;

            if (defaultDigitalFieldSecurity.Any(a => a.FieldCode.Equals(checkObjectFieldExistenceRequest.FieldCode, StringComparison.InvariantCultureIgnoreCase))
                || defaultDigitalFieldTenant0.Any(a => a.FieldCode.Equals(checkObjectFieldExistenceRequest.FieldCode, StringComparison.InvariantCultureIgnoreCase)))
            {
                res = true;
            }

            return res;
        }

        public bool DoesPropertyExistInDynamic(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }

        #region private

        private List<DigitalFeildSecurityObject> DiscardUnfoundFeildsFromList(List<DigitalFeildSecurityObject> defaultDigitalFieldSecurity)
        {
            return defaultDigitalFieldSecurity.Where(a => a.IsList).ToList();
        }

        private List<DigitalFeildSecurityObject> DiscardUnfoundFeildsFromObject(List<DigitalFeildSecurityObject> defaultDigitalFieldSecurity)
        {
            return defaultDigitalFieldSecurity.Where(a => a.IsPm).ToList();
        }

        #endregion private
    }
}
