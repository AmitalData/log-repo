using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.Amital
{
    public class AmitalRestrictOwnerService : IAmitalRestrictOwnerService
    {

        public AmitalRestrictOwnerModel GetAmitalRestrictOwnerModel(bool getFromCache,int tenant = 1, string UnifreightUserId = null)
        {
            if (string.IsNullOrWhiteSpace(UnifreightUserId))
            {
                //UnifreightUserId = AuthenticationUtil.ResolveUnifreightUserId(tenant);
                if (RequestSheetContext.Current != null)
                {
                    var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                    if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                    if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                    {
                        UserRepository userRep = new UserRepository(tenant);
                        User user = userRep.GetSingleUser(loggingUserIdFromRS, tenant, true);
                        if (user != null)
                        {
                            if (!String.IsNullOrWhiteSpace(user.Code))
                            {
                                UnifreightUserId = user.Code;
                            }
                        }

                    }
                }
                if (String.IsNullOrWhiteSpace(UnifreightUserId))
                {
                    UnifreightUserId = AuthenticationUtil.ResolveUnifreightUserId(tenant);
                }
            }
            if (String.IsNullOrWhiteSpace(UnifreightUserId))
            {
                return null;
            }


            string entityKeyString = "AmitalRestrictOwner:" + UnifreightUserId + "_" + tenant.ToString();
            var amitalRestrictOwnerModel = CacheManager.GetOrInsertNewObject<AmitalRestrictOwnerModel>(
                entityKeyString,  () =>
            {
                var model = GetResctOwnerListBL(tenant, UnifreightUserId);
                return model;
            }, getFromCache);
            return amitalRestrictOwnerModel;
        }

        AmitalRestrictOwnerModel GetResctOwnerListBL(int tenant , string UnifreightUserId )
        {

            var myAmitalRestrictOwnerModel = new AmitalRestrictOwnerModel()
            { Tenant = tenant, UnifreightUserId = UnifreightUserId };
            CustomsSettingRepository custSettingsRepo = new CustomsSettingRepository(tenant);
            CustomsSetting custSettings = custSettingsRepo.GetSettingByTenant(tenant);
            if (custSettings != null && !custSettings.IsConnectedToUniFreight)
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                UserFreelancerGroupRepository userFreelancerGroupRepository = new UserFreelancerGroupRepository(commonContext);
                UserFreelancerGroupQuery userFreelancerGroupQuery = new UserFreelancerGroupQuery(userFreelancerGroupRepository);
                var groups = userFreelancerGroupQuery.GetUserFreelancerGroupsByUserId(UnifreightUserId, tenant).ToList();
                DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(tenant);
                if(groups.Count == 0)
                {
                    return myAmitalRestrictOwnerModel;
                }
                var cards = defaultValueQueryService.GetCardIdsByDefaultValue("ISRAEL", "CIM_RESTRICTGRP", groups, tenant);
                myAmitalRestrictOwnerModel.Cards = cards;
                return myAmitalRestrictOwnerModel;


            }
            else
            {
                var amitalCustomFileCommunicationModel = new Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase(
                   Logitude.Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess,
                   "GWSFUSERDATA", "GetResctOwnerList")
                {
                    Tenant = 1,
                    objectTableName = "",
                    CommunicationLoggingEntityReference = "",
                    EntityId = "",
                    UserId = UnifreightUserId,
                    CommunicationSubject = "GetResctOwnerList",

                    //LogitudeFile = myFile,
                };


                var myDictionary = new Dictionary<string, string>();
                myDictionary.Add("USR_CODE", UnifreightUserId);

                var myUServerCommunicationService = new Logitude.Customs.BL.Messaging.Amital.UServerCommunicationService
                    <Logitude.Customs.BL.Messaging.Amital.AmitalCommunicationModelBase, Dictionary<string, string>>(
                    amitalCustomFileCommunicationModel, myDictionary);
                var info = myUServerCommunicationService.Send(true, true);
                if (info.GenericResponseObj.Status != "0")
                {
                    throw new Exception("Send  Communication GetResctOwnerList to UServer  Failed  :" + info.GenericResponseObj.Message);
                }

                var xml = info.GenericResponseObj.ResponseXml;
                var resList = UnifreightListsUtil.Deserialize(xml);


                var sIsRestrictedOwner = UnifreightListsUtil.GetValue(ref resList, "IsRestrictedOwner");
                bool IsRestrictedOwner;
                if (!bool.TryParse(sIsRestrictedOwner, out IsRestrictedOwner))
                {
                    throw new Exception("Response From UServer GetResctOwnerList  IsRestrictedOwner is not Boolean:" + info.GenericResponseObj.ResponseXml);
                }

                myAmitalRestrictOwnerModel.IsRestrictedOwner = IsRestrictedOwner;
                if (myAmitalRestrictOwnerModel.IsRestrictedOwner)
                {
                    var ResctOwnerList = UnifreightListsUtil.GetValue(ref resList, "ResctOwnerList");
                    if (string.IsNullOrWhiteSpace(ResctOwnerList))
                    {
                        //yuval said its ok without list  ---- //throw new Exception()
                    }
                    else
                    {
                        myAmitalRestrictOwnerModel.Cards = ResctOwnerList.Split(',').ToList();
                    }
                }

                return myAmitalRestrictOwnerModel;
            }
        }
    }

    //public class AmitalRestrictOwnerModel {
    //    public AmitalRestrictOwnerModel()
    //    {
    //        Cards = new List<string>();
    //    }
    //    //key
    //    public int Tenant { get; set; }
    //    public string UnifreightUserId { get; set; }

    //    /// vAlue
        

    //    public bool IsRestrictedOwner { get; set; }
    //    public List<string> Cards { get; set; }
    //}
}
