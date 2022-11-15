using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFreight.Web.WcfApi;

namespace CustomsWorkerRole.Test
{
    public partial class clsTester
    {

       

        public static void ReAnalyze2470_CustomsWithheld(int tenant,int maxretry)//מעוכב מכס	
        {

            var objectTableDecId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var qs = new DeclarationCourierStatusQueryService(tenant);
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
            var q = qs.GetQCustomsWithheld(tenant);
            var decs = q.ToList();
            Debug.WriteLine($"GetQCustomsWithheld({decs.Count})");
            var crsList = new List<string>();
            foreach (var decId in decs)
            {
                Debug.WriteLine($"currentdecId={decId}");
                var list = customsRequestsSheetQueryService.GetRequestByInterfaceTypeCode(tenant, "2470", objectTableDecId, decId, null);
                if (list?.Count > 0)
                {
                    Debug.WriteLine($"GetRequestByInterfaceTypeCode(2470).count={list?.Count}");
                    var customsRequestsSheetPM = list.FirstOrDefault(x => x.RequestStatusCode == "30");

                    if (customsRequestsSheetPM != null)
                    {

                        Debug.WriteLine($"customsRequestsSheetPM.id={customsRequestsSheetPM.Id}");
                        string mess = null;
                        try
                        {
                            if (maxretry > 0)
                            {
                                maxretry--;

                                ChangeAnalyzeFailAndReQueue(tenant, customsRequestsSheetPM);
                            }
                            mess = null;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.ToString());
                            
                        }
                    }


                }

            }

        }
        public static void RequeByID(int tenant, string customsRequestsSheetId)
        {

            
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
            var customsRequestsSheetPM = customsRequestsSheetQueryService.GetSingle(customsRequestsSheetId,false,false);
            
            
                ChangeAnalyzeFailAndReQueue(tenant, customsRequestsSheetPM);
            
        }

        public static void ChangeAnalyzeFailAndReQueue(int tenant, Logitude.Customs.Def.EntityPMs.CustomsRequestsSheetPM customsRequestsSheetPM)
        {

            if (customsRequestsSheetPM?.RequestStatusEnum != Logitude.CustomsMessaging.Common.ResponseData.SheetStatusEnum.Analyzed)
            {
                Debug.WriteLine($"status not Analyzed-abort {customsRequestsSheetPM?.Id}");
                return;
            }
                
            using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
            {

                var customContext = CustomContext.GetContext(tenant);
                var customsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

                //customsRequestsSheetPM.RequestStatusCode = "25";
                customsRequestsSheetPM.RequestStatusEnum= Logitude.CustomsMessaging.Common.ResponseData.SheetStatusEnum.AnalyzeFailed;
                customsRequestsSheetPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                customsRequestsSheetUpdateService.Update(customsRequestsSheetPM, true);

                MessagingServiceFactoryHelper
                .ResolveAndReQueue(customsRequestsSheetPM.InterfaceTypeCode, customsRequestsSheetPM.Tenant, customsRequestsSheetPM.Id);
                Debug.WriteLine($"customsRequestsSheetPM.id={customsRequestsSheetPM.Id}:done");
                scopeNewCRS.Complete();

            }
        }

        public static void Check_UserWcfService(int tenant)
        {
            var xmlstring = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><root><UserPM xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><ExtensionData /><ActiveModified>0</ActiveModified><Anniversary xsi:nil=\"true\" /><Birthday xsi:nil=\"true\" /><BranchId>TLV</BranchId><BranchName>Tel Aviv</BranchName><BusinessPhone>03333332</BusinessPhone><BusinessUnitId>1</BusinessUnitId><Code>BELLA</Code><ComputedLocalName>בלה</ComputedLocalName><Contact><Anniversary xsi:nil=\"true\" /><AnniversaryReminder>false</AnniversaryReminder><BirthDayOfYear xsi:nil=\"true\" /><Birthday xsi:nil=\"true\" /><BirthdayReminder>false</BirthdayReminder><DisconectFromCard>false</DisconectFromCard><DisplayGettingStarted>false</DisplayGettingStarted><DoneDate xsi:nil=\"true\" /><DontShowLocal>false</DontShowLocal><Email>TESTTT@AMITAL.CO.IL</Email><EnglishName>BELLA</EnglishName><FieldsChanged>false</FieldsChanged><HasCardContact>false</HasCardContact><HasPassword>false</HasPassword><Id>BELLA</Id><InActive>false</InActive><IndexColor xsi:nil=\"true\" /><IsAirExport>false</IsAirExport><IsAirImport>false</IsAirImport><IsAll>false</IsAll><IsCreatedWithPartner>false</IsCreatedWithPartner><IsCustomsImport>false</IsCustomsImport><IsHybrid>true</IsHybrid><IsInlandDomestic>false</IsInlandDomestic><IsInlandExport>false</IsInlandExport><IsInlandImport>false</IsInlandImport><IsLocked>false</IsLocked><IsOceanExport>false</IsOceanExport><IsOceanImport>false</IsOceanImport><IsSendNotificationForMobile>false</IsSendNotificationForMobile><IsUser>false</IsUser><LocalName>בלה</LocalName><MustChangePassword>false</MustChangePassword><NumberOfRetries>0</NumberOfRetries><SetAsPrimaryForCard>false</SetAsPrimaryForCard><SharedMobileAppAlertonExceptions>false</SharedMobileAppAlertonExceptions><SharedMobileAppAlertsforFollowedShipment>false</SharedMobileAppAlertsforFollowedShipment><SignupRole>false</SignupRole><Tenant>1</Tenant></Contact><CreateDate>2021-06-30T12:13:24.4114336</CreateDate><DepartmentId>---</DepartmentId><DepartmentName>---</DepartmentName><Email>TESTTT@AMITAL.CO.IL</Email><EnglishName>BELLA</EnglishName><EntityChanged>false</EntityChanged><ExpirationDate xsi:nil=\"true\" /><ExpirationDaysLeft>0</ExpirationDaysLeft><Id>1-6859</Id><InActive>false</InActive><InternetAccess>false</InternetAccess><IsBranchRestricted>false</IsBranchRestricted><IsCustomerCare>false</IsCustomerCare><IsDistributor>false</IsDistributor><IsFreelancer>true</IsFreelancer><IsHybrid>true</IsHybrid><IsProductRestricted>false</IsProductRestricted><IsSalesman>false</IsSalesman><IsShowContactDetailsInTheMobileApp>false</IsShowContactDetailsInTheMobileApp><LicencedUser>false</LicencedUser><LocalName>בלה</LocalName><Mobile>0533333322</Mobile><Roles><UserRolesPM><Added>true</Added><Exists>false</Exists><Id>FRL1</Id><Removed>false</Removed><Tenant>1</Tenant></UserRolesPM></Roles><SearchFields>testtt@amital.co.il,BELLA,בלה,BELLA</SearchFields><SignupRole>false</SignupRole><Tenant>1</Tenant><UserLastLogin><ExtensionData /><ComputerId>3d12b27d-f422-404b-94c6-bcbceadbb7f1</ComputerId><Id>1-6859</Id><LoginDateTime>2021-07-04T13:19:30.8321699</LoginDateTime><Tenant>1</Tenant></UserLastLogin><UserPermittedBranches /><UserPermittedProducts /><UserType>R</UserType></UserPM></root>";
            //UserPM entityPM = XmlGenericUtil<UserPM>.DeSerializeObject(xmlstring);
            UserQuery userQuery = new UserQuery(tenant);
            UserPM entityPM = userQuery.GetSinglePMByCode("BELLA",tenant);
            var userWcfService = new UserWcfService();
            userWcfService.Upsert(entityPM,false);
            
        }
    }
}
