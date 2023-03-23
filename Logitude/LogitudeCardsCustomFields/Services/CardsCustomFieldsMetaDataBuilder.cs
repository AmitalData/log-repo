using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogitudeCardsCustomFields.Services
{
    public class CardsCustomFieldsMetaDataBuilder
    {
        private bool IsBuildCardsCustomObjectFields = false;
        private bool IsGenerateMetaDataScripts = false;
        private List<string> cardsTablesNames;
        private WebFreightContext webFreightContext;
        private ObjectFieldRepository objectFieldRepository;
        private ObjectFieldQuery objectFieldQuery;
        private ObjectTable cardObjectTable;
        public CardsCustomFieldsMetaDataBuilder(bool isBuildCardsCustomObjectFields, bool isGenerateMetaDataScripts)
        {
            CacheManager.CacheWrapper = new NoCache4uWrapper();
            IsBuildCardsCustomObjectFields = isBuildCardsCustomObjectFields;
            IsGenerateMetaDataScripts = isGenerateMetaDataScripts;
            Initialize();
            cardsTablesNames = GetCardsTablesNames();
        }

        private void Initialize()
        {
            string globalConnectionString = ConfigurationManager.ConnectionStrings["Globalstr"]?.ToString();
            string mainConnectionString = ConfigurationManager.ConnectionStrings["Mainstr"]?.ToString();
            if (string.IsNullOrEmpty(globalConnectionString) || string.IsNullOrEmpty(mainConnectionString))
            {
                MessageBox.Show("Connection is Empty");
                return;
            }
            webFreightContext = new WebFreightContext(DatabaseInitializer.GetConnection(mainConnectionString));
            objectFieldRepository = new ObjectFieldRepository(webFreightContext);
            objectFieldQuery = new ObjectFieldQuery(objectFieldRepository);
        }

        public void Build()
        {
            if (webFreightContext == null) return;

            ClearUpdatedObjectFieldsAndTextCodesScriptFile();
            List<ObjectFieldPM> allCardsCusotomObjectFieldsPMs = GetAllCardsCusotomObjectFieldsPMs();
            if (cardObjectTable == null || string.IsNullOrEmpty(cardObjectTable.Id))
            {
                MessageBox.Show("Card Object Table is missing!");
                return;
            }

            allCardsCusotomObjectFieldsPMs.GroupBy(objectFieldPM => objectFieldPM.Tenant).ToList().ForEach(tenantGroup =>
            {
                BuildCardMetaDataForSpecificTenant(tenantGroup.Key, allCardsCusotomObjectFieldsPMs);
            });

            FinishScriptFile();
        }

        private void BuildCardMetaDataForSpecificTenant(int tenant, List<ObjectFieldPM> allCardsCusotomObjectFieldsPMs)
        {
            List<ObjectFieldPM> allCardsTenantCusotomObjectFieldsPMs = allCardsCusotomObjectFieldsPMs.Where(a => a.Tenant == tenant).ToList();
            int cardFieldIndex = 1;
            for (int i = 0; i < cardsTablesNames.Count(); i++)
            {
                cardFieldIndex = BuildCardMetaDataForSpecificPartner(allCardsTenantCusotomObjectFieldsPMs, cardFieldIndex, cardsTablesNames[i]);
            }
        }

        private int BuildCardMetaDataForSpecificPartner(List<ObjectFieldPM> allCardsTenantCusotomObjectFieldsPMs, int cardFieldIndex, string cardObjectTableName)
        {
            List<ObjectFieldPM> cardTenantCusotomObjectFieldsPMs = allCardsTenantCusotomObjectFieldsPMs.Where(a => a.ObjectTableName == cardObjectTableName).OrderBy(a => Int32.Parse(a.FieldName.Replace("Field",""))).ToList();
            for (int oldCardFieldIndex = 0; oldCardFieldIndex < cardTenantCusotomObjectFieldsPMs.Count(); oldCardFieldIndex++)
            {
                CreateNewCardObjectFieldAndBuildMetaDataScriptsForSpecificPartner(cardFieldIndex, cardObjectTableName, oldCardFieldIndex, cardTenantCusotomObjectFieldsPMs[oldCardFieldIndex]);
                cardFieldIndex++;
            }

            return cardFieldIndex;
        }

        private void CreateNewCardObjectFieldAndBuildMetaDataScriptsForSpecificPartner(int cardFieldIndex, string cardObjectTableName, int oldCardFieldIndex, ObjectFieldPM selectedObjectFieldPM)
        {
            CreateNewCardObjectFieldPM(selectedObjectFieldPM, cardFieldIndex);
            
            if (cardObjectTableName != "Customer")
            {
                BuildScript(cardFieldIndex, oldCardFieldIndex, selectedObjectFieldPM);
            }
        }

        private void CreateNewCardObjectFieldPM(ObjectFieldPM selectedObjectFieldPM, int cardFieldIndex)
        {
            if (!IsBuildCardsCustomObjectFields) return;

            ObjectFieldPM objectFieldPM = GetNewCardObjectFieldPM(selectedObjectFieldPM, cardFieldIndex);
            try
            {
                new ObjectFieldService(webFreightContext, selectedObjectFieldPM.Tenant).Create(objectFieldPM);
            }
            catch (Exception exception)
            {
                if (exception.Message == "An Object Field with the same code already exists")
                {
                    objectFieldPM.Code = objectFieldPM.Code + "C" + new Random().Next(0, 50);
                    new ObjectFieldService(webFreightContext, selectedObjectFieldPM.Tenant).Create(objectFieldPM);
                }
                else
                {
                    throw new ApplicationException(exception.Message);
                }
            }
        }

        private List<ObjectFieldPM> GetAllCardsCusotomObjectFieldsPMs()
        {
            List<string> allCardsObjectTablesIds = GetAllCardsObjectTablesIds();
            IQueryable<ObjectFieldPM> allObjectFieldsPMs = objectFieldQuery.GetAllGetObjectFieldsPMs();
            List<ObjectFieldPM> allCardsCusotomObjectFieldsPMs = allObjectFieldsPMs.Where(objectField => objectField.Tenant != 0 && objectField.IsCustom && allCardsObjectTablesIds.Contains(objectField.ObjectTableId)).ToList();
            return allCardsCusotomObjectFieldsPMs;
        }

        private List<string> GetAllCardsObjectTablesIds()
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(webFreightContext);
            IQueryable<ObjectTable> allObjectTables = objectTableRepository.GetObjects();
            cardObjectTable = allObjectTables.Where(objectTable => objectTable.Name == "Card").FirstOrDefault();
            List<ObjectTable> allCardsObjectTables = allObjectTables.Where(objectTable => cardsTablesNames.Contains(objectTable.Name)).ToList();
            List<string> allCardsObjectTablesIds = allCardsObjectTables.Select(objectTable => objectTable.Id).ToList();
            return allCardsObjectTablesIds;
        }

        private static void ClearUpdatedObjectFieldsAndTextCodesScriptFile()
        {
            using (StreamWriter writer = new StreamWriter("../../GeneratedScripts/UpdatedObjectFieldsAndTextCodesScript.sql"))
            {
                writer.WriteLine("");
            }
        }

        private void BuildScript(int cardFieldIndex, int oldCardFieldIndex, ObjectFieldPM selectedObjectFieldPM)
        {
            if (!IsGenerateMetaDataScripts) return;

            string generatedFieldCode = "@fieldcode_" + selectedObjectFieldPM.Tenant + "_" + cardFieldIndex;
            int oldCardFieldNumber = oldCardFieldIndex + 1;
            string existData = File.ReadAllText("../../GeneratedScripts/UpdatedObjectFieldsAndTextCodesScript.sql");
            using (StreamWriter writer = new StreamWriter("../../GeneratedScripts/UpdatedObjectFieldsAndTextCodesScript.sql"))
            {
                writer.WriteLine(existData);
                writer.WriteLine("---------------------------");
                writer.WriteLine("declare " + generatedFieldCode + " nvarchar(1000)");
                writer.WriteLine("set " + generatedFieldCode + " = (select FieldCode from objectfields where id = '" + selectedObjectFieldPM.Id + "')");
                writer.WriteLine("update objectfields set FieldCode = REPLACE(" + generatedFieldCode + ", 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "'),FieldName = 'Field" + cardFieldIndex + "' where id = '" + selectedObjectFieldPM.Id + "'");
                if (!string.IsNullOrEmpty(selectedObjectFieldPM.FullNameTextCodeId))
                {
                    writer.WriteLine("-----");
                    writer.WriteLine("declare " + generatedFieldCode + "FullName nvarchar(1000)");
                    writer.WriteLine("set " + generatedFieldCode + "FullName = (select FullNameTextCodeCode from objectfields where id = '" + selectedObjectFieldPM.Id + "')");
                    writer.WriteLine("update objectfields set FullNameTextCodeCode = REPLACE(" + generatedFieldCode + "FullName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.Id + "'");
                    writer.WriteLine("update textcodes set Code = REPLACE(" + generatedFieldCode + "FullName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.FullNameTextCodeId + "'");
                }
                if (!string.IsNullOrEmpty(selectedObjectFieldPM.ShortNameTextCodeId))
                {
                    writer.WriteLine("-----");
                    writer.WriteLine("declare " + generatedFieldCode + "ShortName nvarchar(1000)");
                    writer.WriteLine("set " + generatedFieldCode + "ShortName = (select ShortNameTextCodeCode from objectfields where id = '" + selectedObjectFieldPM.Id + "')");
                    writer.WriteLine("update objectfields set ShortNameTextCodeCode = REPLACE(" + generatedFieldCode + "ShortName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.Id + ")");
                    writer.WriteLine("update textcodes set Code = REPLACE(" + generatedFieldCode + "ShortName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.ShortNameTextCodeId + "'");
                }
                if (!string.IsNullOrEmpty(selectedObjectFieldPM.HelpTextCodeId))
                {
                    writer.WriteLine("-----");
                    writer.WriteLine("declare " + generatedFieldCode + "HelpName nvarchar(1000)");
                    writer.WriteLine("set " + generatedFieldCode + "HelpName = (select HelpTextCodeCode from objectfields where id = '" + selectedObjectFieldPM.Id + "')");
                    writer.WriteLine("update objectfields set HelpTextCodeCode = REPLACE(" + generatedFieldCode + "HelpName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.Id + "'");
                    writer.WriteLine("update textcodes set Code = REPLACE(" + generatedFieldCode + "HelpName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.HelpTextCodeId + "'");
                }
                if (!string.IsNullOrEmpty(selectedObjectFieldPM.ListTextCodeId))
                {
                    writer.WriteLine("-----");
                    writer.WriteLine("declare " + generatedFieldCode + "ListName nvarchar(1000)");
                    writer.WriteLine("set " + generatedFieldCode + "ListName = (select ListTextCodeCode from objectfields where id = '" + selectedObjectFieldPM.Id + "')");
                    writer.WriteLine("update objectfields set ListTextCodeCode = REPLACE(" + generatedFieldCode + "ListName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.Id + "'");
                    writer.WriteLine("update textcodes set Code = REPLACE(" + generatedFieldCode + "ListName, 'Field" + oldCardFieldNumber + "', 'Field" + cardFieldIndex + "') where id = '" + selectedObjectFieldPM.ListTextCodeId + "'");
                }
            }
        }

        private ObjectFieldPM GetNewCardObjectFieldPM(ObjectFieldPM selectedObjectFieldPM, int cardFieldIndex)
        {
            return new ObjectFieldPM
            {
                FieldName = "Field" + (cardFieldIndex),
                CanFilter = selectedObjectFieldPM.CanFilter,
                Code = selectedObjectFieldPM.Code,
                FullNameTextCodeCode = selectedObjectFieldPM.FullNameTextCodeDefaultText,
                ListTextCodeCode = selectedObjectFieldPM.ListTextCodeDefaultText,
                HelpTextCodeCode = selectedObjectFieldPM.HelpTextCodeDefaultText,
                DataTypeCode = selectedObjectFieldPM.DataTypeCode,
                DigitsAfterPoint = selectedObjectFieldPM.DigitsAfterPoint,
                DisplayInEntityVariables = selectedObjectFieldPM.DisplayInEntityVariables,
                DisplayInList = selectedObjectFieldPM.DisplayInList,
                IsCustom = true,
                NumberOfDigits = selectedObjectFieldPM.NumberOfDigits,
                ObjectTableId = cardObjectTable.Id,
                Tenant = selectedObjectFieldPM.Tenant,
                LookUpTableId = selectedObjectFieldPM.LookUpTableId,
                CustomPickListCode = selectedObjectFieldPM.CustomPickListCode,
                IsRequiered = selectedObjectFieldPM.IsRequiered,
                DisplayOnly = selectedObjectFieldPM.DisplayOnly,
                MaxLength = selectedObjectFieldPM.MaxLength,
                MinLength = selectedObjectFieldPM.MinLength,
                MultiLine = selectedObjectFieldPM.MultiLine,
                DefaultAdditionalTreeFilters = selectedObjectFieldPM.DefaultAdditionalTreeFilters
            };
        }

        private static List<string> GetCardsTablesNames()
        {
            return new List<string>
            {
                "Customer",
                "Agent",
                "AccountingPartner",
                "AirLine",
                "CustomAgent",
                "CustomsShipper",
                "Participant",
                "ShippingAgent",
                "ShippingLine",
                "Trucker",
                "Vendor",
                "Warehouse"
            };
        }

        private static void FinishScriptFile()
        {
            string existData = File.ReadAllText("../../GeneratedScripts/UpdatedObjectFieldsAndTextCodesScript.sql");
            using (StreamWriter writer = new StreamWriter("../../GeneratedScripts/UpdatedObjectFieldsAndTextCodesScript.sql"))
            {
                writer.WriteLine(existData);
                writer.WriteLine("---------------------------");
                writer.WriteLine("-----------Finish----------");
                writer.WriteLine("---------------------------");
                writer.WriteLine("--select * from querycolumns where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=querycolumns.ObjectFieldId)");
                writer.WriteLine("--update querycolumns set ObjectFieldCode = (select FieldCode from objectfields where Id=querycolumns.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=querycolumns.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from ScreenFields where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ScreenFields.ObjectFieldId)");
                writer.WriteLine("--update ScreenFields set ObjectFieldCode = (select FieldCode from objectfields where Id=ScreenFields.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ScreenFields.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from AdvancedQueryFilters where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=AdvancedQueryFilters.ObjectFieldId)");
                writer.WriteLine("--update AdvancedQueryFilters set ObjectFieldCode = (select FieldCode from objectfields where Id=AdvancedQueryFilters.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=AdvancedQueryFilters.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from RuleConditionFields where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=RuleConditionFields.ObjectFieldId)");
                writer.WriteLine("--update RuleConditionFields set ObjectFieldCode = (select FieldCode from objectfields where Id=RuleConditionFields.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=RuleConditionFields.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from ObjectTableRuleFields where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ObjectTableRuleFields.ObjectFieldId)");
                writer.WriteLine("--update ObjectTableRuleFields set ObjectFieldCode = (select FieldCode from objectfields where Id=ObjectTableRuleFields.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ObjectTableRuleFields.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from ObjectTableRules where tenant<>0 and TriggerFieldCode <> (select FieldCode from objectfields where Id=ObjectTableRules.TriggerFieldId)");
                writer.WriteLine("--update ObjectTableRules set TriggerFieldCode = (select FieldCode from objectfields where Id=ObjectTableRules.TriggerFieldId)  where tenant<>0 and TriggerFieldCode <> (select FieldCode from objectfields where Id=ObjectTableRules.TriggerFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from AirlineMessagingRules where tenant<>0 and RuleFieldCode <> (select FieldCode from objectfields where Id=AirlineMessagingRules.RuleFieldId)");
                writer.WriteLine("--update AirlineMessagingRules set RuleFieldCode = (select FieldCode from objectfields where Id=AirlineMessagingRules.RuleFieldId)  where tenant<>0 and RuleFieldCode <> (select FieldCode from objectfields where Id=AirlineMessagingRules.RuleFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from Restrictions where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=Restrictions.ObjectFieldId)");
                writer.WriteLine("--update Restrictions set ObjectFieldCode = (select FieldCode from objectfields where Id=Restrictions.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=Restrictions.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from CustomerFieldsUpdateSettings where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=CustomerFieldsUpdateSettings.ObjectFieldId)");
                writer.WriteLine("--update CustomerFieldsUpdateSettings set ObjectFieldCode = (select FieldCode from objectfields where Id=CustomerFieldsUpdateSettings.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=CustomerFieldsUpdateSettings.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from ObjectFieldValidations where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ObjectFieldValidations.ObjectFieldId)");
                writer.WriteLine("--update ObjectFieldValidations set ObjectFieldCode = (select FieldCode from objectfields where Id=ObjectFieldValidations.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ObjectFieldValidations.ObjectFieldId)");
                writer.WriteLine("");
                writer.WriteLine("--select * from ObjectFieldModifications where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ObjectFieldModifications.ObjectFieldId)");
                writer.WriteLine("--update ObjectFieldModifications set ObjectFieldCode = (select FieldCode from objectfields where Id=ObjectFieldModifications.ObjectFieldId)  where tenant<>0 and ObjectFieldCode <> (select FieldCode from objectfields where Id=ObjectFieldModifications.ObjectFieldId)");
                writer.WriteLine("------End-------");
            }
        }
    }
}
