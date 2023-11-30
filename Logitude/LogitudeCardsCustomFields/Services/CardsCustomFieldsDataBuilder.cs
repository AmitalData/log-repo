using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogitudeCardsCustomFields.Services
{
    public class CardsCustomFieldsDataBuilder
    {
        private ObjectTable cardObjectTable;
        private List<string> cardsTablesNames;
        private WebFreightContext webFreightContext;
        private int totalCountOfCardCustomFields = 0;
        public CardsCustomFieldsDataBuilder()
        {
            CacheManager.CacheWrapper = new NoCache4uWrapper();
            Initialize();
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
            cardsTablesNames = GetCardsDataTablesNames();
        }

        public void Build()
        {
            if (webFreightContext == null) return;

            CleareInsertedCardsDataScriptFile();
            IQueryable<ObjectTable> allObjectTables = GetAllIQueryableObjectTables(webFreightContext);
            cardObjectTable = allObjectTables.Where(objectTable => objectTable.Name == "Card").FirstOrDefault();
            if (cardObjectTable == null || string.IsNullOrEmpty(cardObjectTable.Id))
            {
                MessageBox.Show("Card Object Table is missing!");
                return;
            }

            List<ObjectField> allCardsCusotomObjectFields = GetAllCardsCusotomObjectFields(webFreightContext, allObjectTables);
            allCardsCusotomObjectFields.GroupBy(objectField => objectField.Tenant).ToList().ForEach(tenantGroup =>
            {
                BuildCardDataScriptsForSpecificTenant(tenantGroup.Key, allCardsCusotomObjectFields);
            });
        }

        private void BuildCardDataScriptsForSpecificTenant(int tenant, List<ObjectField> allCardsCusotomObjectFields)
        {
            totalCountOfCardCustomFields = 0;
            List<ObjectField> allCardsTenantCusotomObjectFields = allCardsCusotomObjectFields.Where(a => a.Tenant == tenant).ToList();
            for (int i = 0; i < cardsTablesNames.Count(); i++)
            {
                BuildCardDataScriptsForSpecificPartner(tenant, allCardsTenantCusotomObjectFields, cardsTablesNames[i]);
            }
        }

        private void BuildCardDataScriptsForSpecificPartner(int tenant, List<ObjectField> allCardsTenantCusotomObjectFields, string cardObjectTableName)
        {
            int cardTenantCusotomObjectFieldsCount = allCardsTenantCusotomObjectFields.Where(a => a.ObjectTable.Name == cardObjectTableName).Count();

            if (cardTenantCusotomObjectFieldsCount == 0) return;

            switch (cardObjectTableName)
            {
                case "Customer":
                    BuildCustomerDataScript(tenant, cardTenantCusotomObjectFieldsCount);
                    break;
                case "Agent":
                    BuildAgentDataScript(tenant, cardTenantCusotomObjectFieldsCount);
                    break;
                case "CustomAgent":
                    BuildCustomAgentDataScript(tenant, cardTenantCusotomObjectFieldsCount);
                    break;
                case "Trucker":
                    BuildTruckerDataScript(tenant, cardTenantCusotomObjectFieldsCount);
                    break;
                case "Vendor":
                    BuildVendorDataScript(tenant, cardTenantCusotomObjectFieldsCount);
                    break;
                case "Warehouse":
                    BuildWarehouseDataScript(tenant, cardTenantCusotomObjectFieldsCount);
                    break;
                case "ShippingAgent":
                    BuildShippingAgentDataScript(tenant, cardTenantCusotomObjectFieldsCount);
                    break;
            }


            totalCountOfCardCustomFields += cardTenantCusotomObjectFieldsCount;

        }

        private List<ObjectField> GetAllCardsCusotomObjectFields(WebFreightContext webFreightContext, IQueryable<ObjectTable> allObjectTables)
        {
            List<ObjectTable> allCardsObjectTables = allObjectTables.Where(objectTable => cardsTablesNames.Contains(objectTable.Name)).ToList();
            List<string> allCardsObjectTablesIds = allCardsObjectTables.Select(objectTable => objectTable.Id).ToList();
            ObjectFieldRepository objectFieldRepository = new ObjectFieldRepository(webFreightContext);
            IQueryable<ObjectField> allObjectFields = objectFieldRepository.GetObjectFields();
            List<ObjectField> allCardsCusotomObjectFields = allObjectFields.Where(objectField => objectField.Tenant != 0 && objectField.IsCustom && allCardsObjectTablesIds.Contains(objectField.ObjectTableId)).ToList();
            return allCardsCusotomObjectFields;
        }

        private IQueryable<ObjectTable> GetAllIQueryableObjectTables(WebFreightContext webFreightContext)
        {
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(webFreightContext);
            IQueryable<ObjectTable> allObjectTables = objectTableRepository.GetObjects();
            return allObjectTables;
        }

        private static void CleareInsertedCardsDataScriptFile()
        {
            using (StreamWriter writer = new StreamWriter("../../GeneratedScripts/InsertedCardsDataScript.sql"))
            {
                writer.WriteLine("");
            }
        }

        private void BuildCustomerDataScript(int tenant, int cardTenantCusotomObjectFieldsCount)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            (from card in commonContext.Customers
             where card.Tenant == tenant
             select card).Where(card => !string.IsNullOrEmpty(card.Field1) || !string.IsNullOrEmpty(card.Field2) || !string.IsNullOrEmpty(card.Field3) || !string.IsNullOrEmpty(card.Field4) || !string.IsNullOrEmpty(card.Field5) || !string.IsNullOrEmpty(card.Field6) || !string.IsNullOrEmpty(card.Field7) || !string.IsNullOrEmpty(card.Field8) || !string.IsNullOrEmpty(card.Field9) || !string.IsNullOrEmpty(card.Field10)).ToList().ForEach(card =>
             {
                 BuildCardDataScript(tenant, new CardCustomDetails
                 {
                     Id = card.Id,
                     Field1 = card.Field1,
                     Field2 = card.Field2,
                     Field3 = card.Field3,
                     Field4 = card.Field4,
                     Field5 = card.Field5,
                     Field6 = card.Field6,
                     Field7 = card.Field7,
                     Field8 = card.Field8,
                     Field9 = card.Field9,
                     Field10 = card.Field10,
                 }, totalCountOfCardCustomFields + 1, totalCountOfCardCustomFields + cardTenantCusotomObjectFieldsCount);
             });
        }

        private void BuildAgentDataScript(int tenant, int cardTenantCusotomObjectFieldsCount)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            (from card in commonContext.Agents
             where card.Tenant == tenant
             select card).Where(card => !string.IsNullOrEmpty(card.Field1) || !string.IsNullOrEmpty(card.Field2) || !string.IsNullOrEmpty(card.Field3) || !string.IsNullOrEmpty(card.Field4) || !string.IsNullOrEmpty(card.Field5) || !string.IsNullOrEmpty(card.Field6) || !string.IsNullOrEmpty(card.Field7) || !string.IsNullOrEmpty(card.Field8) || !string.IsNullOrEmpty(card.Field9) || !string.IsNullOrEmpty(card.Field10)).ToList().ForEach(card =>
             {
                 BuildCardDataScript(tenant, new CardCustomDetails
                 {
                     Id = card.Id,
                     Field1 = card.Field1,
                     Field2 = card.Field2,
                     Field3 = card.Field3,
                     Field4 = card.Field4,
                     Field5 = card.Field5,
                     Field6 = card.Field6,
                     Field7 = card.Field7,
                     Field8 = card.Field8,
                     Field9 = card.Field9,
                     Field10 = card.Field10,
                 }, totalCountOfCardCustomFields + 1, totalCountOfCardCustomFields + cardTenantCusotomObjectFieldsCount);
             });
        }

        private void BuildCustomAgentDataScript(int tenant, int cardTenantCusotomObjectFieldsCount)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            (from card in commonContext.CustomAgents
             where card.Tenant == tenant
             select card).Where(card => !string.IsNullOrEmpty(card.Field1) || !string.IsNullOrEmpty(card.Field2) || !string.IsNullOrEmpty(card.Field3) || !string.IsNullOrEmpty(card.Field4) || !string.IsNullOrEmpty(card.Field5) || !string.IsNullOrEmpty(card.Field6) || !string.IsNullOrEmpty(card.Field7) || !string.IsNullOrEmpty(card.Field8) || !string.IsNullOrEmpty(card.Field9) || !string.IsNullOrEmpty(card.Field10)).ToList().ForEach(card =>
             {
                 BuildCardDataScript(tenant, new CardCustomDetails
                 {
                     Id = card.Id,
                     Field1 = card.Field1,
                     Field2 = card.Field2,
                     Field3 = card.Field3,
                     Field4 = card.Field4,
                     Field5 = card.Field5,
                     Field6 = card.Field6,
                     Field7 = card.Field7,
                     Field8 = card.Field8,
                     Field9 = card.Field9,
                     Field10 = card.Field10,
                 }, totalCountOfCardCustomFields + 1, totalCountOfCardCustomFields + cardTenantCusotomObjectFieldsCount);
             });
        }

        private void BuildTruckerDataScript(int tenant, int cardTenantCusotomObjectFieldsCount)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            (from card in commonContext.Truckers
             where card.Tenant == tenant
             select card).Where(card => !string.IsNullOrEmpty(card.Field1) || !string.IsNullOrEmpty(card.Field2) || !string.IsNullOrEmpty(card.Field3) || !string.IsNullOrEmpty(card.Field4) || !string.IsNullOrEmpty(card.Field5) || !string.IsNullOrEmpty(card.Field6) || !string.IsNullOrEmpty(card.Field7) || !string.IsNullOrEmpty(card.Field8) || !string.IsNullOrEmpty(card.Field9) || !string.IsNullOrEmpty(card.Field10)).ToList().ForEach(card =>
             {
                 BuildCardDataScript(tenant, new CardCustomDetails
                 {
                     Id = card.Id,
                     Field1 = card.Field1,
                     Field2 = card.Field2,
                     Field3 = card.Field3,
                     Field4 = card.Field4,
                     Field5 = card.Field5,
                     Field6 = card.Field6,
                     Field7 = card.Field7,
                     Field8 = card.Field8,
                     Field9 = card.Field9,
                     Field10 = card.Field10,
                 }, totalCountOfCardCustomFields + 1, totalCountOfCardCustomFields + cardTenantCusotomObjectFieldsCount);
             });
        }

        private void BuildVendorDataScript(int tenant, int cardTenantCusotomObjectFieldsCount)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            (from card in commonContext.Vendors
             where card.Tenant == tenant
             select card).Where(card => !string.IsNullOrEmpty(card.Field1) || !string.IsNullOrEmpty(card.Field2) || !string.IsNullOrEmpty(card.Field3) || !string.IsNullOrEmpty(card.Field4) || !string.IsNullOrEmpty(card.Field5) || !string.IsNullOrEmpty(card.Field6) || !string.IsNullOrEmpty(card.Field7) || !string.IsNullOrEmpty(card.Field8) || !string.IsNullOrEmpty(card.Field9) || !string.IsNullOrEmpty(card.Field10)).ToList().ForEach(card =>
             {
                 BuildCardDataScript(tenant, new CardCustomDetails
                 {
                     Id = card.Id,
                     Field1 = card.Field1,
                     Field2 = card.Field2,
                     Field3 = card.Field3,
                     Field4 = card.Field4,
                     Field5 = card.Field5,
                     Field6 = card.Field6,
                     Field7 = card.Field7,
                     Field8 = card.Field8,
                     Field9 = card.Field9,
                     Field10 = card.Field10,
                 }, totalCountOfCardCustomFields + 1, totalCountOfCardCustomFields + cardTenantCusotomObjectFieldsCount);
             });
        }

        private void BuildWarehouseDataScript(int tenant, int cardTenantCusotomObjectFieldsCount)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            (from card in commonContext.Warehouses
             where card.Tenant == tenant
             select card).Where(card => !string.IsNullOrEmpty(card.Field1) || !string.IsNullOrEmpty(card.Field2) || !string.IsNullOrEmpty(card.Field3) || !string.IsNullOrEmpty(card.Field4) || !string.IsNullOrEmpty(card.Field5) || !string.IsNullOrEmpty(card.Field6) || !string.IsNullOrEmpty(card.Field7) || !string.IsNullOrEmpty(card.Field8) || !string.IsNullOrEmpty(card.Field9) || !string.IsNullOrEmpty(card.Field10)).ToList().ForEach(card =>
             {
                 BuildCardDataScript(tenant, new CardCustomDetails
                 {
                     Id = card.Id,
                     Field1 = card.Field1,
                     Field2 = card.Field2,
                     Field3 = card.Field3,
                     Field4 = card.Field4,
                     Field5 = card.Field5,
                     Field6 = card.Field6,
                     Field7 = card.Field7,
                     Field8 = card.Field8,
                     Field9 = card.Field9,
                     Field10 = card.Field10,
                 }, totalCountOfCardCustomFields + 1, totalCountOfCardCustomFields + cardTenantCusotomObjectFieldsCount);
             });
        }

        private void BuildShippingAgentDataScript(int tenant, int cardTenantCusotomObjectFieldsCount)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            (from card in commonContext.ShippingAgents
             where card.Tenant == tenant
             select card).Where(card => !string.IsNullOrEmpty(card.Field1) || !string.IsNullOrEmpty(card.Field2) || !string.IsNullOrEmpty(card.Field3) || !string.IsNullOrEmpty(card.Field4) || !string.IsNullOrEmpty(card.Field5) || !string.IsNullOrEmpty(card.Field6) || !string.IsNullOrEmpty(card.Field7) || !string.IsNullOrEmpty(card.Field8) || !string.IsNullOrEmpty(card.Field9) || !string.IsNullOrEmpty(card.Field10)).ToList().ForEach(card =>
             {
                 BuildCardDataScript(tenant, new CardCustomDetails
                 {
                     Id = card.Id,
                     Field1 = card.Field1,
                     Field2 = card.Field2,
                     Field3 = card.Field3,
                     Field4 = card.Field4,
                     Field5 = card.Field5,
                     Field6 = card.Field6,
                     Field7 = card.Field7,
                     Field8 = card.Field8,
                     Field9 = card.Field9,
                     Field10 = card.Field10,
                 }, totalCountOfCardCustomFields + 1, totalCountOfCardCustomFields + cardTenantCusotomObjectFieldsCount);
             });
        }

        private void BuildCardDataScript(int tenant, CardCustomDetails cardCustomDetails, int startNumber, int endNumber)
        {
            string existData = File.ReadAllText("../../GeneratedScripts/InsertedCardsDataScript.sql");
            string generatedIdName = "@CardCustomObjectId" + tenant + cardCustomDetails.Id.Replace("-", "");
            string stringFieldNumbers = getStringFieldNumbers(startNumber, endNumber);
            using (StreamWriter writer = new StreamWriter("../../GeneratedScripts/InsertedCardsDataScript.sql"))
            {
                writer.WriteLine(existData);
                writer.WriteLine("---------------------------");
                writer.WriteLine("declare " + generatedIdName + " as varchar(15)");
                writer.WriteLine("EXECUTE usp_GetNextTableIdValue " + generatedIdName + " OUTPUT,'CustomFieldsMainObject'");
                writer.WriteLine("Insert Into CustomFieldsMainObjects(Id,Tenant,EntityId,ObjectTableId," + stringFieldNumbers + ")");
                writer.WriteLine("Values(" + generatedIdName + ", " + tenant + ", '" + cardCustomDetails.Id + "', '" + cardObjectTable.Id + "', " + GetStringScriptValues(cardCustomDetails, stringFieldNumbers) + ")");
            }
        }

        private string GetStringScriptValues(CardCustomDetails cardCustomDetails, string stringFieldNumbers)
        {
            int fieldsCount = stringFieldNumbers.Split(',').Length;
            string res = "";
            if(fieldsCount > 0)
            {
                res += GetFieldValue(cardCustomDetails.Field1);
            }
            if (fieldsCount > 1)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field2);
            }
            if (fieldsCount > 2)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field3);
            }
            if (fieldsCount > 3)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field4);
            }
            if (fieldsCount > 4)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field5);
            }
            if (fieldsCount > 5)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field6);
            }
            if (fieldsCount > 6)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field7);
            }
            if (fieldsCount > 7)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field8);
            }
            if (fieldsCount > 8)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field9);
            }
            if (fieldsCount > 9)
            {
                res += ", " + GetFieldValue(cardCustomDetails.Field10);
            }
            return res;
        }

        private string getStringFieldNumbers(int startNumber, int endNumber)
        {
            string res = "";
            int incNumber = startNumber;
            while (endNumber >= incNumber)
            {
                res += "Field" + incNumber;
                if (endNumber != incNumber) res += ",";
                incNumber += 1;
            }

            return res;
        }

        private string GetFieldValue(string fieldValue)
        {
            if (string.IsNullOrEmpty(fieldValue)) return "null";

            return "'" + fieldValue + "'";
        }

        private static List<string> GetCardsDataTablesNames()
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
    }
}

public class CardCustomDetails
{
    public string Id { get; set; }
    public string Field1 { get; set; }
    public string Field2 { get; set; }
    public string Field3 { get; set; }
    public string Field4 { get; set; }
    public string Field5 { get; set; }
    public string Field6 { get; set; }
    public string Field7 { get; set; }
    public string Field8 { get; set; }
    public string Field9 { get; set; }
    public string Field10 { get; set; }
}
