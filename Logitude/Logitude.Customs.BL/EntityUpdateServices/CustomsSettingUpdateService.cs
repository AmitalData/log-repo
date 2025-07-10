using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using System.Xml;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.Resolvers;
using Logitude.Customs.Data.EntityPOCOs;
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
    }
}