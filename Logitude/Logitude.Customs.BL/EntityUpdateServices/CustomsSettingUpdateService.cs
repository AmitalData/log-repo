using Logitude.BL.Resolvers;
using Logitude.Customs.BL.CLoseTable;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Logitude.Customs.BL.Messaging.FtpOutParams;
namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsSettingUpdateService
    {
        protected override void OnCreating(CustomsSettingPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            entityPM.Id = Guid.NewGuid().ToString();
            base.OnCreating(entityPM, entityParentPM);
        }
        protected override void OnUpdating(CustomsSettingPM entityPM)
        {
            if(entityPM.CompanyType == "B")
            {  
			     CourierPendingReasonRepository courierPendingReasonRepository = new CourierPendingReasonRepository(entityPM.Tenant);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "900",Tenant = entityPM.Tenant, EnglishName = "Collection", LocalName = "גביה", Inactive = false, ErrorPlace = "2", UnifreightStatusCode = "VPE", RequiresApproval = false, RequiresPayment = false }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "901",Tenant = entityPM.Tenant, EnglishName = "Autonomy", LocalName = "אוטונומיה", Inactive = false, ErrorPlace = "1" }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "902",Tenant = entityPM.Tenant, EnglishName = "Missing ID", LocalName = "חסר ת.ז.", Inactive = false, ErrorPlace = "1", RequiresApproval = false, RequiresPayment = false }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "903",Tenant = entityPM.Tenant, EnglishName = "Invalid phone number", LocalName = "מספר טלפון לא תקין", Inactive = false, ErrorPlace = "1" }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "904",Tenant = entityPM.Tenant, EnglishName = "Weight", LocalName = "משקל", Inactive = false, RequiresApproval = true, RequiresPayment = false }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "905",Tenant = entityPM.Tenant, EnglishName = "Shipping not to Israel", LocalName = "משלוח לא לישראל", Inactive = false }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "906",Tenant = entityPM.Tenant, EnglishName = "Commercial Customer", LocalName = "לקוח מסחרי", Inactive = false }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "907",Tenant = entityPM.Tenant, EnglishName = "Quantity Of Goods", LocalName = "כמות סחורה", Inactive = false }, courierPendingReasonRepository);
			     this.AddCourierPendingReason(new CourierPendingReason() { Code = "908",Tenant = entityPM.Tenant, EnglishName = "required power of attorney", LocalName = "נדרש יפוי כח", Inactive = false }, courierPendingReasonRepository);
                    
			     courierPendingReasonRepository.SubmitChanges();				
			}


            bool isChanged = false;
            List<string> changesText = new List<string>();
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(entityPM.Tenant);
            string newValueText = TextCodesTranslator.TranslateText("CustomsSetting.O.NewValue", entityPM.Tenant, showLocal);
            string oldValueText = TextCodesTranslator.TranslateText("CustomsSetting.O.OldValue", entityPM.Tenant, showLocal);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update && !string.IsNullOrEmpty(this.EntityChangeFieldXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(this.EntityChangeFieldXml);
                foreach (XmlNode xmlnode in doc?.DocumentElement)
                {
                    var changes = xmlnode?.InnerXml?.Split(new string[] { "<c" }, StringSplitOptions.None)?.Skip(1)?.ToArray();
                    foreach (var change in changes)
                    {
                        string translatedFieldName = "", fieldName = "", oldValue = "", newValue = "";
                        var fieldNameFrom = change.IndexOf("f=\"");
                        if (change.Length > fieldNameFrom + 3)
                        {
                            var fieldNameSubStrined = change.Substring(fieldNameFrom + 3);
                            var fieldNameFromDoubleQuote = fieldNameSubStrined.IndexOf("\"");
                            fieldName = fieldNameSubStrined.Substring(0, fieldNameFromDoubleQuote);
                            translatedFieldName = TextCodesTranslator.TranslateText("Customs.CustomsSetting.F." + fieldName, entityPM.Tenant, showLocal);
                        }
                        var oFrom = change.IndexOf("o=\"");
                        if (change.Length > oFrom + 3)
                        {
                            var oSubStrined = change.Substring(oFrom + 3);
                            var oFromDoubleQuote = oSubStrined.IndexOf("\"");
                            oldValue = oSubStrined.Substring(0, oFromDoubleQuote);
                        }
                        var nFrom = change.IndexOf("n=\"");
                        if (change.Length > nFrom + 3)
                        {
                            var nSubStrined = change.Substring(nFrom + 3);
                            var nFromDoubleQuote = nSubStrined.IndexOf("\"");
                            newValue = nSubStrined.Substring(0, nFromDoubleQuote);
                        }
                        if (oldValue != newValue)
                        {
                            isChanged = true;
                            changesText.Add(String.Format("{0} - {1}: {2}, {3}: {4}", translatedFieldName, oldValueText, oldValue, newValueText, newValue));
                        }
                    }
                }
                if (isChanged)
                {
                    ContactRepository contactRep = new ContactRepository(entityPM.Tenant);

                    string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
                    Contact contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
                    EventTracer.CreateTraceEvent(new EventTracerArgs() { EntityId = entityPM.Id, ObjectTableName = "Customs.CustomsSetting", Tenant = entityPM.Tenant, UserId = contact.Id, EventTypeCode = "UPDT", Notes = String.Join(Environment.NewLine, changesText) });
                }
            }
        }
		public void AddCourierPendingReason(CourierPendingReason courierPendingReason, CourierPendingReasonRepository courierPendingReasonRepository)
        {
            var existing = courierPendingReasonRepository.GetSingle(courierPendingReason.Code, courierPendingReason.Tenant);
            if (existing == null)
            {
				CourierPendingReason newCourierPendingReason = new CourierPendingReason()
				{
					Id = courierPendingReason.Code,
					Code = courierPendingReason.Code,
					Tenant = courierPendingReason.Tenant,
					Inactive = courierPendingReason.Inactive,
					ErrorPlace = courierPendingReason.ErrorPlace,
					RequiresApproval = courierPendingReason.RequiresApproval,
					RequiresPayment = courierPendingReason.RequiresPayment,
					LocalName = courierPendingReason.LocalName,
					EnglishName = courierPendingReason.EnglishName,
					SearchFields = (courierPendingReason.Code + "," + courierPendingReason.LocalName + "," + courierPendingReason.EnglishName).ToLower()
				};
				courierPendingReasonRepository.Add(courierPendingReason);
            }
		}
	}
}