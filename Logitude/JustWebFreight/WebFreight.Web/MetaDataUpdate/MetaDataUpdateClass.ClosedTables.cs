using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Transactions;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        public void LoadBaseTablesForDataBases()
        {
            GlobalDBRepository globalDbRep = new GlobalDBRepository();
            List<GlobalDB> dbList = globalDbRep.GetGlobalDBs().ToList();

            foreach (GlobalDB db in dbList)
            {
                LoadBaseTablesForConnection(db.DBConnection);
            }
        }
        public void UpgradeClosedTablesForTenantZero()
        {
            isUpdate = true;
            List<GlobalDB> dbList = null;
            using (TransactionScope scop = TransactionFactory.GetNewTransaction(new TimeSpan(0, 5, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                GlobalDBRepository globalDbRep = new GlobalDBRepository();
                dbList = globalDbRep.GetGlobalDBs().ToList();
                scop.Complete();
            }

            foreach (GlobalDB db in dbList)
            {
                LoadBaseTablesForConnection(db.DBConnection);
            }
        }

        private void LoadBaseTablesForConnection(string connectionStr)
        {
            WebFreightContext tempContext = new WebFreightContext(DatabaseInitializer.GetConnection(connectionStr));
            CommonDataContext commonContext = new CommonDataContext(DatabaseInitializer.GetConnection(connectionStr));
            ShipmentsContext shipmentContext = new ShipmentsContext(DatabaseInitializer.GetConnection(connectionStr));
            InvoiceContext invoiceContext = new InvoiceContext(DatabaseInitializer.GetConnection(connectionStr));
            QuotesContext quotesContext = new QuotesContext(DatabaseInitializer.GetConnection(connectionStr));

            // string dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString; ;
            string dbConnectionInfo = "";
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Oracle_Globalstr"].ConnectionString;

            }
            else
            {
                dbConnectionInfo = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            }
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo);
            GlobalContext globalContext = new GlobalContext(connection);

            #region PasswordPolicy
            PasswordPolicyRepository passwordPolicySrepository = new PasswordPolicyRepository(commonContext);
            AddClosedTables.AddPasswordPolicies(new PasswordPolicyDetails() { Code = "MEDU", PasswordStrength = "Meduim" }, passwordPolicySrepository);
            AddClosedTables.AddPasswordPolicies(new PasswordPolicyDetails() { Code = "STRO", PasswordStrength = "Strong" }, passwordPolicySrepository);
            AddClosedTables.AddPasswordPolicies(new PasswordPolicyDetails() { Code = "VSTR", PasswordStrength = "Very Strong" }, passwordPolicySrepository);
            passwordPolicySrepository.SubmitChanges();
            #endregion

            #region WeightUnit
            WeightUnitRepository weightRep = new WeightUnitRepository(commonContext);
            AddClosedTables.AddWeightUnits(new WeightUnitDetails() { Code = "KG", Name = "Kilogram" }, weightRep);
            AddClosedTables.AddWeightUnits(new WeightUnitDetails() { Code = "LB", Name = "Pound" }, weightRep);
            AddClosedTables.AddWeightUnits(new WeightUnitDetails() { Code = "MT", Name = "Metric Ton" }, weightRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddWeightUnits(new WeightUnitDetails() { Code = "TST", Name = "Test" }, weightRep);
            }
            //============================================

            weightRep.SubmitChanges();
            #endregion

            #region QuoteTemplateSectionTyp
            QuoteTemplateSectionTypeRepository quoteTemplateSectionTypeRep = new QuoteTemplateSectionTypeRepository(quotesContext);
            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "PH", Name = "PageHeader" }, quoteTemplateSectionTypeRep);
            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "QH", Name = "QuoteHeader" }, quoteTemplateSectionTypeRep);
            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "QI", Name = "QuoteIntroduction" }, quoteTemplateSectionTypeRep);

            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "QD", Name = "QuoteDetails " }, quoteTemplateSectionTypeRep);

            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "S", Name = "Section" }, quoteTemplateSectionTypeRep);
            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "PP", Name = "PricingPackages" }, quoteTemplateSectionTypeRep);
            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "PC", Name = "PricingContainers" }, quoteTemplateSectionTypeRep);
            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "PB", Name = "PageBreak " }, quoteTemplateSectionTypeRep);
            AddClosedTables.AddQuoteTemplateSectionTypes(new QuoteTemplateSectionTypeDetails() { Code = "PF", Name = "PageFooter" }, quoteTemplateSectionTypeRep);
            quoteTemplateSectionTypeRep.SubmitChanges();
            #endregion

            #region BorderType
            BorderTypeRepository bordertypeRep = new BorderTypeRepository(quotesContext);
            AddClosedTables.AddBorderTypes(new BorderTypeDetails() { Code = "NONE", Name = "None" }, bordertypeRep);
            AddClosedTables.AddBorderTypes(new BorderTypeDetails() { Code = "ALL", Name = "All" }, bordertypeRep);
            AddClosedTables.AddBorderTypes(new BorderTypeDetails() { Code = "BOX", Name = "Box" }, bordertypeRep);
            AddClosedTables.AddBorderTypes(new BorderTypeDetails() { Code = "HORIZONTALLINES", Name = "HorizontalLines" }, bordertypeRep);
            AddClosedTables.AddBorderTypes(new BorderTypeDetails() { Code = "VERTICALLINES", Name = "VerticalLines" }, bordertypeRep);

            bordertypeRep.SubmitChanges();
            #endregion

            #region RateClass
            RateClassRepository retesRep = new RateClassRepository(commonContext);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "M", Name = "Minimum Charge" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "B", Name = "Basic Charge" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "C", Name = "Specific Commodity Rate" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "E", Name = "ULD Additional Rate" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "K", Name = "Rate Per Kilogram" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "N", Name = "Normal Rate" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "Q", Name = "Quantity Rate" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "R", Name = "Class Rate Reduction" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "S", Name = "Class Rate Surcharge" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "U", Name = "ULD Basic Charge or Rate" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "X", Name = "ULD Additional Information" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "Y", Name = "ULD Discount" }, retesRep);
            AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "P", Name = "International Priority Service Rate" }, retesRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddRateClasses(new RateClassDetails() { Code = "TST", Name = "Test" }, retesRep);
            }
            //============================================
            retesRep.SubmitChanges();
            #endregion

            #region DocumentTypeCategory
            DocumentTypeCategoryRepository DocsRep = new DocumentTypeCategoryRepository(commonContext);
            AddClosedTables.AddDocumentTypeCategories(new DocumentTypeCategoryDetails() { Code = "P", Name = "Operational Documents" }, DocsRep);
            AddClosedTables.AddDocumentTypeCategories(new DocumentTypeCategoryDetails() { Code = "A", Name = "Accounting Documents" }, DocsRep);
            AddClosedTables.AddDocumentTypeCategories(new DocumentTypeCategoryDetails() { Code = "O", Name = "Others" }, DocsRep);


            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddDocumentTypeCategories(new DocumentTypeCategoryDetails() { Code = "TST", Name = "Test" }, DocsRep);
            }
            //============================================
            DocsRep.SubmitChanges();
            #endregion

            #region CustomerTenantAccessStatusType

            CustomerTenantAccessStatusTypeRepository CusTenantAccRep = new CustomerTenantAccessStatusTypeRepository(commonContext);
            AddClosedTables.AddCustomerTenantAccessStatusTypes(new CustomerTenantAccessStatusTypeDetails() { Code = "W", EnglishName = "Waiting For Approval", LocalName = "Waiting For Approval" }, CusTenantAccRep);
            AddClosedTables.AddCustomerTenantAccessStatusTypes(new CustomerTenantAccessStatusTypeDetails() { Code = "A", EnglishName = "Accepted", LocalName = "Accepted" }, CusTenantAccRep);
            AddClosedTables.AddCustomerTenantAccessStatusTypes(new CustomerTenantAccessStatusTypeDetails() { Code = "IA", EnglishName = "InActive", LocalName = "InActive" }, CusTenantAccRep);
            AddClosedTables.AddCustomerTenantAccessStatusTypes(new CustomerTenantAccessStatusTypeDetails() { Code = "IP", EnglishName = "In Progress", LocalName = "In Progress" }, CusTenantAccRep);
            AddClosedTables.AddCustomerTenantAccessStatusTypes(new CustomerTenantAccessStatusTypeDetails() { Code = "N", EnglishName = "New", LocalName = "New" }, CusTenantAccRep);


            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddCustomerTenantAccessStatusTypes(new CustomerTenantAccessStatusTypeDetails() { Code = "TST", EnglishName = "Test" }, CusTenantAccRep);
            }
            //============================================
            CusTenantAccRep.SubmitChanges();
            #endregion

            //-------------Dimensions Unit---------------
            DimensionsUnitRepository dimensionsRep = new DimensionsUnitRepository(commonContext);
            AddClosedTables.AddDimensionsUnits(new DimensionsUnitDetails() { Code = "Cm", Name = "Cm" }, dimensionsRep);
            AddClosedTables.AddDimensionsUnits(new DimensionsUnitDetails() { Code = "Inc", Name = "Inch" }, dimensionsRep);
            AddClosedTables.AddDimensionsUnits(new DimensionsUnitDetails() { Code = "Ft", Name = "Ft" }, dimensionsRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddDimensionsUnits(new DimensionsUnitDetails() { Code = "TST", Name = "Test" }, dimensionsRep);
            }
            //============================================
            dimensionsRep.SubmitChanges();

            #region Due Type
            DueTypeRepository duetypeRep = new DueTypeRepository(commonContext);
            AddClosedTables.AddDueTypes(new DueTypeDetails() { Code = "CA", Name = "Carrier" }, duetypeRep);
            AddClosedTables.AddDueTypes(new DueTypeDetails() { Code = "AG", Name = "Agent" }, duetypeRep);
            AddClosedTables.AddDueTypes(new DueTypeDetails() { Code = "TX", Name = "Tax" }, duetypeRep);
            AddClosedTables.AddDueTypes(new DueTypeDetails() { Code = "VL", Name = "Valuation" }, duetypeRep);
            AddClosedTables.AddDueTypes(new DueTypeDetails() { Code = "NO", Name = "none" }, duetypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddDueTypes(new DueTypeDetails() { Code = "TS", Name = "Test" }, duetypeRep);
            }
            //============================================
            duetypeRep.SubmitChanges();
            #endregion

            //-------------Transport Mode---------------
            TransportModeRepository transModeRep = new TransportModeRepository(tempContext);
            AddClosedTables.AddTransportModes(new TransportModeDetails() { Id = "A", Name = "Air" }, transModeRep);
            AddClosedTables.AddTransportModes(new TransportModeDetails() { Id = "O", Name = "Ocean" }, transModeRep);
            AddClosedTables.AddTransportModes(new TransportModeDetails() { Id = "I", Name = "Inland" }, transModeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddTransportModes(new TransportModeDetails() { Id = "T", Name = "Test" }, transModeRep);
            }
            //============================================
            transModeRep.SubmitChanges();

            //-------------Direction---------------
            DirectionRepository directionRep = new DirectionRepository(tempContext);
            AddClosedTables.AddDirections(new DirectionDetails() { Id = "E", Name = "Export" }, directionRep);
            AddClosedTables.AddDirections(new DirectionDetails() { Id = "I", Name = "Import" }, directionRep);
            AddClosedTables.AddDirections(new DirectionDetails() { Id = "D", Name = "Domestic" }, directionRep);
            AddClosedTables.AddDirections(new DirectionDetails() { Id = "R", Name = "Drop" }, directionRep);
            AddClosedTables.AddDirections(new DirectionDetails() { Id = "C", Name = "Customs Import" }, directionRep);
            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddDirections(new DirectionDetails() { Id = "T", Name = "Test" }, directionRep);
            }
            //============================================
            directionRep.SubmitChanges();

            //-------------Prepaid Collect---------------
            PrepaidCollectRepository prepaidCollectRep = new PrepaidCollectRepository(tempContext);
            AddClosedTables.AddPrepaidCollects(new PrepaidCollectDetails() { Id = "P", Name = "Prepaid", DisplayInLOV = true }, prepaidCollectRep);
            AddClosedTables.AddPrepaidCollects(new PrepaidCollectDetails() { Id = "C", Name = "Collect", DisplayInLOV = true }, prepaidCollectRep);
            AddClosedTables.AddPrepaidCollects(new PrepaidCollectDetails() { Id = "B", Name = "Both", DisplayInLOV = false }, prepaidCollectRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddPrepaidCollects(new PrepaidCollectDetails() { Id = "T", Name = "Test" }, prepaidCollectRep);
            }
            //============================================
            prepaidCollectRep.SubmitChanges();

            //-------------Shipment Type---------------
            ShipmentTypeRepository shipmentTypesRep = new ShipmentTypeRepository(shipmentContext);
            AddClosedTables.AddShipmentTypes(new ShipmentTypeDetails() { Id = "FCLD", Name = "FCL", TransportModeId = "O" }, shipmentTypesRep);
            AddClosedTables.AddShipmentTypes(new ShipmentTypeDetails() { Id = "LCLD", Name = "LCL", TransportModeId = "O" }, shipmentTypesRep);
            AddClosedTables.AddShipmentTypes(new ShipmentTypeDetails() { Id = "MyGO", Name = "My Groupage Ocean", TransportModeId = "O" }, shipmentTypesRep);
            AddClosedTables.AddShipmentTypes(new ShipmentTypeDetails() { Id = "FTL", Name = "FTL", TransportModeId = "I" }, shipmentTypesRep);
            AddClosedTables.AddShipmentTypes(new ShipmentTypeDetails() { Id = "LTL", Name = "LTL", TransportModeId = "I" }, shipmentTypesRep);
            AddClosedTables.AddShipmentTypes(new ShipmentTypeDetails() { Id = "MyGI", Name = "My Groupage Inland", TransportModeId = "I" }, shipmentTypesRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddShipmentTypes(new ShipmentTypeDetails() { Id = "TST", Name = "Test", TransportModeId = "I" }, shipmentTypesRep);
            }
            //============================================
            shipmentTypesRep.SubmitChanges();

            //-------------Partner Type---------------
            PartnerTypeRepository partnerTypesRep = new PartnerTypeRepository(commonContext);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "AG", Name = "Agent" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "CG", Name = "Custom Agent" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "SG", Name = "Shipping Agent" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "TR", Name = "Trucker" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "AL", Name = "AirLine" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "SL", Name = "Shipping Line" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "OT", Name = "Others" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "CS", Name = "Customer" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "VD", Name = "Vendor" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "WH", Name = "Warehouse" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "CO", Name = "Coloader" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "CC", Name = "Custom Clearance" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "PO", Name = "Potential Customer" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "FL", Name = "Freelancer" }, partnerTypesRep);
            AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "PT", Name = "Participant" }, partnerTypesRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddPartnerTypes(new PartnerTypeDetails() { Id = "TS", Name = "Test" }, partnerTypesRep);
            }
            //============================================
            partnerTypesRep.SubmitChanges();

            //-------------Address Type---------------
            AddressTypeRepository addressTypeRep = new AddressTypeRepository(commonContext);
            AddClosedTables.AddAddressTypes(new AddressTypeDetails() { Id = "M", Name = "Main" }, addressTypeRep);
            AddClosedTables.AddAddressTypes(new AddressTypeDetails() { Id = "B", Name = "Billing" }, addressTypeRep);
            AddClosedTables.AddAddressTypes(new AddressTypeDetails() { Id = "P", Name = "Pickup Delivery" }, addressTypeRep);
            AddClosedTables.AddAddressTypes(new AddressTypeDetails() { Id = "L", Name = "Local Address" }, addressTypeRep);
            AddClosedTables.AddAddressTypes(new AddressTypeDetails() { Id = "O", Name = "Others" }, addressTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddAddressTypes(new AddressTypeDetails() { Id = "T", Name = "Test" }, addressTypeRep);
            }
            //============================================
            addressTypeRep.SubmitChanges();

            //-------------Entity Date---------------
            EntityDateRepository entityDateRep = new EntityDateRepository(commonContext);
            AddClosedTables.AddEntityDates(new EntityDateDetails() { Id = "PICD", Name = "Pick Up Departure Date" }, entityDateRep);
            AddClosedTables.AddEntityDates(new EntityDateDetails() { Id = "PICA", Name = "Pick Up Arrival Date" }, entityDateRep);
            AddClosedTables.AddEntityDates(new EntityDateDetails() { Id = "Open", Name = "Open Date" }, entityDateRep);
            AddClosedTables.AddEntityDates(new EntityDateDetails() { Id = "McAr", Name = "Main Carriage Arrivel" }, entityDateRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddEntityDates(new EntityDateDetails() { Id = "TST", Name = "Test" }, entityDateRep);
            }
            //============================================
            entityDateRep.SubmitChanges();

            //-------------Field Data Type---------------
            FieldDataTypesRepository fieldDataTypeRep = new FieldDataTypesRepository(tempContext);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Text", Name = "Text" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "nText", Name = "nText" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "DateTime", Name = "DateTime" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Date", Name = "Date" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Integer", Name = "Integer" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "UnsInteger", Name = "UnsInteger" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Double", Name = "Double" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "SigDouble", Name = "SigDouble" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Boolean", Name = "Boolean" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Decimal", Name = "Decimal" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "UnsDecimal", Name = "UnsDecimal" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "LookUp", Name = "LookUp" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Constant", Name = "Constant" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "PickList", Name = "PickList" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "List", Name = "List" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Emails", Name = "Emails" }, fieldDataTypeRep);
            AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "Byte[]", Name = "Byte[]" }, fieldDataTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddFieldDataTypes(new FieldDataTypeDetails() { Code = "TST", Name = "Test" }, fieldDataTypeRep);
            }
            //============================================
            fieldDataTypeRep.SubmitChanges();

            //-------------Text Code Type---------------
            TextCodeTypesRepository textCodesTypeRep = new TextCodeTypesRepository(tempContext);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "T", Name = "Table" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "F", Name = "Field" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "B", Name = "Button And Action" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "M", Name = "Message" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "H", Name = "Help Text" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "O", Name = "Other" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "Q", Name = "Query" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "S", Name = "Screen" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "G", Name = "General" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "MH", Name = "Menu Header" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "MC", Name = "Maintenance" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "QC", Name = "Query Column" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "CH", Name = "Column Header" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "TH", Name = "Tab Header" }, textCodesTypeRep);
            AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "TIP", Name = "Tip" }, textCodesTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddTextCodeTypes(new TextCodeTypeDetails() { Code = "TS", Name = "Test" }, textCodesTypeRep);
            }
            //============================================
            textCodesTypeRep.SubmitChanges();

            //-------------Charges Group---------------
            ChargesGroupRepository chargesRep = new ChargesGroupRepository(tempContext);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Freight", Code = "FRT", Name = "Freight" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Surcharges", Code = "SCH", Name = "Surcharges" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Other Charges", Code = "OCH", Name = "Other Charges" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Documentation Charges", Code = "DCCH", Name = "Documentation Charges" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Documentation Charges Ex", Code = "DCCHX", Name = "Documentation Charges Ex" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Handling Charges", Code = "HNDCH", Name = "Handling Charges" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Custom Charges", Code = "CUSCH", Name = "Custom Charges" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Valuation", Code = "VAL", Name = "Valuation" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Tax", Code = "TAX", Name = "Tax" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "None", Code = "NONE", Name = "None" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Disbursement", Code = "DIS", Name = "Disbursement" }, chargesRep);
            AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Id = IdCounter.GetNumber("ChargesGroup", 0).ToString(), Tenant = 0, LocalName = "Commission", Code = "COMM", Name = "Commission" }, chargesRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddChargesGroups(new ChargesGroupDetails() { Code = "TST", Name = "Test" }, chargesRep);
            }
            //============================================
            chargesRep.SubmitChanges();

            //-------------Volume Unit---------------
            VolumeUnitRepository volumeRep = new VolumeUnitRepository(tempContext);
            AddClosedTables.AddVolumeUnits(new VolumeUnitDetails() { Code = "CBM", Name = "CBM" }, volumeRep);
            AddClosedTables.AddVolumeUnits(new VolumeUnitDetails() { Code = "CBI", Name = "CBI" }, volumeRep);
            AddClosedTables.AddVolumeUnits(new VolumeUnitDetails() { Code = "CBF", Name = "CBF" }, volumeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddVolumeUnits(new VolumeUnitDetails() { Code = "TST", Name = "Test" }, volumeRep);
            }
            //============================================
            volumeRep.SubmitChanges();

            //-------------Shipment Receivable Line Status---------------
            ShipmentReceivableLineStatusRepository statusTypRep = new ShipmentReceivableLineStatusRepository(shipmentContext);
            AddClosedTables.AddShipmentReceivableLineStatus(new ShipmentReceivableLineStatusDetails() { Code = "OAMT", Name = "Open Amount" }, statusTypRep);
            AddClosedTables.AddShipmentReceivableLineStatus(new ShipmentReceivableLineStatusDetails() { Code = "ACCT", Name = "Closed" }, statusTypRep);
            AddClosedTables.AddShipmentReceivableLineStatus(new ShipmentReceivableLineStatusDetails() { Code = "APPD", Name = "Approved" }, statusTypRep);
            AddClosedTables.AddShipmentReceivableLineStatus(new ShipmentReceivableLineStatusDetails() { Code = "DRFT", Name = "Draft" }, statusTypRep);
            AddClosedTables.AddShipmentReceivableLineStatus(new ShipmentReceivableLineStatusDetails() { Code = "EMPT", Name = "Empty" }, statusTypRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddShipmentReceivableLineStatus(new ShipmentReceivableLineStatusDetails() { Code = "TSTT", Name = "Test" }, statusTypRep);
            }
            //============================================
            statusTypRep.SubmitChanges();

            //-------------Shipment Payable Line Status---------------
            ShipmentPayableLineStatusRepository payablestatusRep = new ShipmentPayableLineStatusRepository(shipmentContext);
            AddClosedTables.AddShipmentPayableLineStatus(new ShipmentPayableLineStatusDetails() { Code = "EMPT", Name = "Empty" }, payablestatusRep);
            AddClosedTables.AddShipmentPayableLineStatus(new ShipmentPayableLineStatusDetails() { Code = "OAMT", Name = "Open" }, payablestatusRep);
            AddClosedTables.AddShipmentPayableLineStatus(new ShipmentPayableLineStatusDetails() { Code = "ACCT", Name = "Closed" }, payablestatusRep);
            AddClosedTables.AddShipmentPayableLineStatus(new ShipmentPayableLineStatusDetails() { Code = "PACC", Name = "Partially Closed" }, payablestatusRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddShipmentPayableLineStatus(new ShipmentPayableLineStatusDetails() { Code = "TSTT", Name = "Test" }, payablestatusRep);
            }
            //============================================
            payablestatusRep.SubmitChanges();


            #region Shipment Payable Status
            ShipmentPayableStatusRepository shipmentpayablestatusRep = new ShipmentPayableStatusRepository(shipmentContext);

            AddClosedTables.AddShipmentPayableStatus(new ShipmentPayableStatusDetails() { Code = "NOPA", Name = "No Payables" }, shipmentpayablestatusRep);
            AddClosedTables.AddShipmentPayableStatus(new ShipmentPayableStatusDetails() { Code = "OPEN", Name = "Open" }, shipmentpayablestatusRep);
            AddClosedTables.AddShipmentPayableStatus(new ShipmentPayableStatusDetails() { Code = "CLSD", Name = "Closed" }, shipmentpayablestatusRep);

            //AddClosedTables.AddShipmentPayableStatus(new ShipmentPayableStatusDetails() { Code = "COST", Name = "Costed" }, shipmentpayablestatusRep);
            //AddClosedTables.AddShipmentPayableStatus(new ShipmentPayableStatusDetails() { Code = "PRPD", Name = "Partially Paid" }, shipmentpayablestatusRep);
            //AddClosedTables.AddShipmentPayableStatus(new ShipmentPayableStatusDetails() { Code = "PAID", Name = "Paid" }, shipmentpayablestatusRep);


            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddShipmentPayableStatus(new ShipmentPayableStatusDetails() { Code = "TSTT", Name = "Test" }, shipmentpayablestatusRep);
            }

            shipmentpayablestatusRep.SubmitChanges();
            #endregion

            #region Shipment Receivable Status
            ShipmentReceivableStatusRepository shipmentReceivableStatusRepository = new ShipmentReceivableStatusRepository(shipmentContext);

            AddClosedTables.AddShipmentReceivableStatus(new ShipmentReceivableStatusDetails() { Code = "NORE", Name = "No Receivables" }, shipmentReceivableStatusRepository);
            AddClosedTables.AddShipmentReceivableStatus(new ShipmentReceivableStatusDetails() { Code = "OPEN", Name = "Open" }, shipmentReceivableStatusRepository);
            AddClosedTables.AddShipmentReceivableStatus(new ShipmentReceivableStatusDetails() { Code = "CLSD", Name = "Closed" }, shipmentReceivableStatusRepository);

            //AddClosedTables.AddShipmentReceivableStatus(new ShipmentReceivableStatusDetails() { Code = "PRIN", Name = "Partially Invoiced" }, shipmentReceivableStatusRepository);

            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddShipmentReceivableStatus(new ShipmentReceivableStatusDetails() { Code = "TSTT", Name = "Test" }, shipmentReceivableStatusRepository);
            }

            shipmentReceivableStatusRepository.SubmitChanges();
            #endregion

            //-------------Quote Type---------------
            QuoteTypeRepository quoteTypesRep = new QuoteTypeRepository(quotesContext);
            AddClosedTables.AddQuoteType(new QuoteTypeDetails() { Code = "A", Name = "Spot Rate" }, quoteTypesRep);
            AddClosedTables.AddQuoteType(new QuoteTypeDetails() { Code = "P", Name = "Routing Rates" }, quoteTypesRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddQuoteType(new QuoteTypeDetails() { Code = "T", Name = "Test" }, quoteTypesRep);
            }
            //============================================
            quoteTypesRep.SubmitChanges();

            //-------------MarkUp Type---------------
            MarkUpTypeRepository markUpTypeRep = new MarkUpTypeRepository(quotesContext);
            AddClosedTables.AddMarkUpType(new MarkUpTypeDetails() { Code = "P", Name = "Percentage" }, markUpTypeRep);
            AddClosedTables.AddMarkUpType(new MarkUpTypeDetails() { Code = "F", Name = "Fixed" }, markUpTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddMarkUpType(new MarkUpTypeDetails() { Code = "T", Name = "Test" }, markUpTypeRep);
            }
            //============================================
            markUpTypeRep.SubmitChanges();

            //-------------ARInvoice Type---------------
            ARInvoiceTypeRepository invoiceTypeRep = new ARInvoiceTypeRepository(invoiceContext);
            AddClosedTables.AddInvoiceType(new InvoiceTypeDetails() { Code = "IN", Name = "Invoice" }, invoiceTypeRep);
            AddClosedTables.AddInvoiceType(new InvoiceTypeDetails() { Code = "TX", Name = "Tax Invoice" }, invoiceTypeRep);
            AddClosedTables.AddInvoiceType(new InvoiceTypeDetails() { Code = "CD", Name = "Credit Note" }, invoiceTypeRep);
            AddClosedTables.AddInvoiceType(new InvoiceTypeDetails() { Code = "MN", Name = "Manifest Invoice" }, invoiceTypeRep);
            AddClosedTables.AddInvoiceType(new InvoiceTypeDetails() { Code = "CI", Name = "Customs Invoice" }, invoiceTypeRep);
            AddClosedTables.AddInvoiceType(new InvoiceTypeDetails() { Code = "CC", Name = "Customs Credit Note" }, invoiceTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddInvoiceType(new InvoiceTypeDetails() { Code = "TS", Name = "Test" }, invoiceTypeRep);
            }
            //============================================
            invoiceTypeRep.SubmitChanges();

            //-------------ARInvoice Status---------------
            ARInvoiceStatusRepository invoiceStatusRep = new ARInvoiceStatusRepository(invoiceContext);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "DR", Name = "Draft" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "PD", Name = "Paid" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "AD", Name = "Unpaid" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "VD", Name = "Void" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "PP", Name = "Partially Paid" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "AC", Name = "Auto Credit" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "AR", Name = "Auto Credited" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "CN", Name = "Connected" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "NT", Name = "Not Connected" }, invoiceStatusRep);
            AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "LL", Name = "Cancelled" }, invoiceStatusRep);
            invoiceStatusRep.SubmitChanges();

            #region ARInvoiceTransferStatus
            ARInvoiceTransferStatusRepository transferStatusRep = new ARInvoiceTransferStatusRepository(invoiceContext);
            AddClosedTables.AddARInvoiceTransferStatus(new ARInvoiceTransferStatusDetails() { Code = "RD", Name = "Ready" }, transferStatusRep);
            AddClosedTables.AddARInvoiceTransferStatus(new ARInvoiceTransferStatusDetails() { Code = "NR", Name = "Not Ready" }, transferStatusRep);
            AddClosedTables.AddARInvoiceTransferStatus(new ARInvoiceTransferStatusDetails() { Code = "TR", Name = "Transferred" }, transferStatusRep);
            AddClosedTables.AddARInvoiceTransferStatus(new ARInvoiceTransferStatusDetails() { Code = "BL", Name = "Blocked" }, transferStatusRep);
            AddClosedTables.AddARInvoiceTransferStatus(new ARInvoiceTransferStatusDetails() { Code = "ET", Name = "Error In Transfer" }, transferStatusRep);
            AddClosedTables.AddARInvoiceTransferStatus(new ARInvoiceTransferStatusDetails() { Code = "IP", Name = "In progress" }, transferStatusRep);

            transferStatusRep.SubmitChanges();
            #endregion

            #region APInvoiceTransferStatus
            APInvoiceTransferStatusRepository apTransferStatusRep = new APInvoiceTransferStatusRepository(invoiceContext);
            AddClosedTables.AddAPInvoiceTransferStatus(new APInvoiceTransferStatusDetails() { Code = "RD", Name = "Ready" }, apTransferStatusRep);
            AddClosedTables.AddAPInvoiceTransferStatus(new APInvoiceTransferStatusDetails() { Code = "NR", Name = "Not Ready" }, apTransferStatusRep);
            AddClosedTables.AddAPInvoiceTransferStatus(new APInvoiceTransferStatusDetails() { Code = "TR", Name = "Transferred" }, apTransferStatusRep);
            AddClosedTables.AddAPInvoiceTransferStatus(new APInvoiceTransferStatusDetails() { Code = "BL", Name = "Blocked" }, apTransferStatusRep);
            AddClosedTables.AddAPInvoiceTransferStatus(new APInvoiceTransferStatusDetails() { Code = "ET", Name = "Error In Transfer" }, apTransferStatusRep);
            AddClosedTables.AddAPInvoiceTransferStatus(new APInvoiceTransferStatusDetails() { Code = "IP", Name = "In progress" }, apTransferStatusRep);

            apTransferStatusRep.SubmitChanges();
            #endregion

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddInvoiceStatus(new InvoiceStatusDetails() { Code = "TS", Name = "Test" }, invoiceStatusRep);
            }
            //============================================


            //-------------Menu Type---------------
            MenuTypeRepository menuTypeRep = new MenuTypeRepository(tempContext);
            AddClosedTables.AddMenuType(new MenuTypeDetails() { Code = "Main", Name = "Main" }, menuTypeRep);
            AddClosedTables.AddMenuType(new MenuTypeDetails() { Code = "MTC", Name = "Maintenance" }, menuTypeRep);
            AddClosedTables.AddMenuType(new MenuTypeDetails() { Code = "CSM", Name = "Customs Maintenance" }, menuTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddMenuType(new MenuTypeDetails() { Code = "TST", Name = "Test" }, menuTypeRep);
            }
            //============================================
            menuTypeRep.SubmitChanges();

            //-------------Category Type--------------
            CategoryTypeRepository categoryTypeRep = new CategoryTypeRepository(tempContext);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "Par", Name = "Partners" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "Bil", Name = "Billing" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "Oth", Name = "Others" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "Loc", Name = "Locations" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "CRM", Name = "CRM" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "TKT", Name = "Tickets" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "CSM", Name = "Customs" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "ACC", Name = "Accounting" }, categoryTypeRep);
            AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "BUP", Name = "Business Process" }, categoryTypeRep);

            //AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "Ven", Name = "Vendors" }, categoryTypeRep);
            //AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "Cli", Name = "Clients" }, categoryTypeRep);
            //AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "Tab", Name = "Tables" }, categoryTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddCategoryType(new CategoryTypeDetails() { Code = "TST", Name = "Test" }, categoryTypeRep);
            }
            //============================================
            categoryTypeRep.SubmitChanges();

            //-------------Vat Type--------------
            if (!isUpdate)
            {
                VatTypeRepository vatTypeRep = new VatTypeRepository(commonContext);
                VatType vat1 = new VatType() { Code = "ZERO", EnglishName = "Zero", LocalName = "Zero", Id = IdCounter.GetNumber("VatType", 0).ToString(), Tenant = 0 };
                VatType vat2 = new VatType() { Code = "STD", EnglishName = "STD", LocalName = "STD", Id = IdCounter.GetNumber("VatType", 0).ToString(), Tenant = 0 };
                VatType vat3 = new VatType() { Code = "EXMPT", EnglishName = "Excempt", LocalName = "Excempt", Id = IdCounter.GetNumber("VatType", 0).ToString(), Tenant = 0 };
                vatTypeRep.Add(vat1);
                vatTypeRep.Add(vat2);
                vatTypeRep.Add(vat3);
                vatTypeRep.SubmitChanges();
            }

            //-------------Template Format--------------
            TemplateFormatRepository templateFormatRep = new TemplateFormatRepository(commonContext);
            AddClosedTables.AddTemplateFormat(new TemplateFormatDetails() { Code = "P", Name = "Print" }, templateFormatRep);
            AddClosedTables.AddTemplateFormat(new TemplateFormatDetails() { Code = "M", Name = "Message" }, templateFormatRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddTemplateFormat(new TemplateFormatDetails() { Code = "T", Name = "Test" }, templateFormatRep);
            }
            //============================================
            templateFormatRep.SubmitChanges();

            //-------------Rule Type--------------
            RuleTypeRepository ruletypeRep = new RuleTypeRepository(tempContext);
            AddClosedTables.AddRuleType(new RuleTypeDetails() { Code = "REQ", Name = "Required" }, ruletypeRep);
            AddClosedTables.AddRuleType(new RuleTypeDetails() { Code = "EVAL", Name = "Entity Validation" }, ruletypeRep);
            AddClosedTables.AddRuleType(new RuleTypeDetails() { Code = "BLCK", Name = "Block Field" }, ruletypeRep);
            AddClosedTables.AddRuleType(new RuleTypeDetails() { Code = "SETV", Name = "Set Field Value" }, ruletypeRep);
            AddClosedTables.AddRuleType(new RuleTypeDetails() { Code = "DUPL", Name = "Field Duplication" }, ruletypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddRuleType(new RuleTypeDetails() { Code = "TST", Name = "Test" }, ruletypeRep);
            }
            //============================================
            ruletypeRep.SubmitChanges();

            //-------------Trigger Type--------------
            TriggerTypeRepository triggerTypeRepository = new TriggerTypeRepository(tempContext);
            AddClosedTables.AddTriggerType(new TriggerTypeDetails() { Code = "ALLW", Name = "Allways" }, triggerTypeRepository);
            AddClosedTables.AddTriggerType(new TriggerTypeDetails() { Code = "COND", Name = "Condition" }, triggerTypeRepository);
            AddClosedTables.AddTriggerType(new TriggerTypeDetails() { Code = "FLDC", Name = "Field Changed" }, triggerTypeRepository);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddTriggerType(new TriggerTypeDetails() { Code = "TSTT", Name = "Test" }, triggerTypeRepository);
            }
            //============================================
            triggerTypeRepository.SubmitChanges();

            //-------------Rule Notification Type---------------
            RuleNotificationTypeRepository ruleNotificationTypeRepository = new RuleNotificationTypeRepository(tempContext);
            AddClosedTables.AddRuleNotificationType(new RuleNotificationTypeDetails() { Code = "ERR", Name = "Error" }, ruleNotificationTypeRepository);
            AddClosedTables.AddRuleNotificationType(new RuleNotificationTypeDetails() { Code = "WAR", Name = "Warning" }, ruleNotificationTypeRepository);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddRuleNotificationType(new RuleNotificationTypeDetails() { Code = "TST", Name = "Test" }, ruleNotificationTypeRepository);
            }
            //============================================
            ruleNotificationTypeRepository.SubmitChanges();

            //-------------Shipment Customer Type---------------
            ShipmentCustomerTypeRepository shipmentCustomerTypeRep = new ShipmentCustomerTypeRepository(shipmentContext);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "SHI", Name = "Shipper" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "CON", Name = "Consignee" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "AGT", Name = "Agent" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "IGT", Name = "Issuing Carrier Agent" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "CAE", Name = "Customs Agent Export" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "CAI", Name = "Customs Agent Import" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "NT1", Name = "Notify 1" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "NT2", Name = "Notify 2" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "SNE", Name = "Shipper Not Exporter" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "CNI", Name = "Consignee Not Importer" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "FOR", Name = "Freight Forwarder" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "COL", Name = "Coloader" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "CCP", Name = "Custom Clearance Point" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "CSD", Name = "Consolidator" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "REA", Name = "Releasing Agent" }, shipmentCustomerTypeRep);
            AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "OTH", Name = "Other", ShowInLOV = false, }, shipmentCustomerTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddShipmentCustomerType(new ShipmentCustomerTypeDetails() { Code = "TST", Name = "Test" }, shipmentCustomerTypeRep);
            }
            //============================================
            shipmentCustomerTypeRep.SubmitChanges();

            //-------------Quote Customer Type---------------
            QuoteCustomerTypeRepository quoteCustomerTypeRep = new QuoteCustomerTypeRepository(quotesContext);
            AddClosedTables.AddQuoteCustomerType(new QuoteCustomerTypeDetails() { Code = "SHI", Name = "Shipper" }, quoteCustomerTypeRep);
            AddClosedTables.AddQuoteCustomerType(new QuoteCustomerTypeDetails() { Code = "CON", Name = "Consignee" }, quoteCustomerTypeRep);
            AddClosedTables.AddQuoteCustomerType(new QuoteCustomerTypeDetails() { Code = "AGT", Name = "Agent" }, quoteCustomerTypeRep);
            AddClosedTables.AddQuoteCustomerType(new QuoteCustomerTypeDetails() { Code = "NOT", Name = "Notify" }, quoteCustomerTypeRep);
            AddClosedTables.AddQuoteCustomerType(new QuoteCustomerTypeDetails() { Code = "OTH", Name = "Other", ShowInLOV = false, }, quoteCustomerTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddQuoteCustomerType(new QuoteCustomerTypeDetails() { Code = "TST", Name = "Test" }, quoteCustomerTypeRep);
            }
            //============================================
            quoteCustomerTypeRep.SubmitChanges();

            //-------------PickUp Delivery From To Type---------------
            PickUpDeliveryFromToTypeRepository fromToTypeRep = new PickUpDeliveryFromToTypeRepository(shipmentContext);
            AddClosedTables.AddPickUpDeliveryFromToTypes(new PickUpDeliveryFromToTypeDetails() { Code = "PORT", Name = "Port" }, fromToTypeRep);
            AddClosedTables.AddPickUpDeliveryFromToTypes(new PickUpDeliveryFromToTypeDetails() { Code = "PART", Name = "Partner" }, fromToTypeRep);
            AddClosedTables.AddPickUpDeliveryFromToTypes(new PickUpDeliveryFromToTypeDetails() { Code = "CASL", Name = "Casual Address" }, fromToTypeRep);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddPickUpDeliveryFromToTypes(new PickUpDeliveryFromToTypeDetails() { Code = "TST", Name = "Test" }, fromToTypeRep);
            }
            //============================================
            fromToTypeRep.SubmitChanges();

            //-------------PickUp Delivery Type---------------
            PickUpDeliveryTypeRepository pickUpdeliveryTypeRep = new PickUpDeliveryTypeRepository(shipmentContext);
            AddClosedTables.AddPickUpDeliveryTypes(new PickUpDeliveryTypeDetails() { Code = "DELV", Name = "Delivery" }, pickUpdeliveryTypeRep);
            AddClosedTables.AddPickUpDeliveryTypes(new PickUpDeliveryTypeDetails() { Code = "PICK", Name = "Pick up" }, pickUpdeliveryTypeRep);
            AddClosedTables.AddPickUpDeliveryTypes(new PickUpDeliveryTypeDetails() { Code = "EMPT", Name = "Empty Container Return" }, pickUpdeliveryTypeRep);
            pickUpdeliveryTypeRep.SubmitChanges();

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddClosedTables.AddPickUpDeliveryTypes(new PickUpDeliveryTypeDetails() { Code = "TST", Name = "Test" }, pickUpdeliveryTypeRep);
            }
            //============================================

            //-------------Tarrif Type---------------
            TarrifTypeRepository tarrifTypeRepository = new TarrifTypeRepository(commonContext);
            AddClosedTables.AddTarrifType(new TarrifTypeDetails() { Code = "F", Name = "Freight" }, tarrifTypeRepository);
            AddClosedTables.AddTarrifType(new TarrifTypeDetails() { Code = "S", Name = "Surcharges" }, tarrifTypeRepository);
            tarrifTypeRepository.SubmitChanges();

            //-------------Tarrif From To Type---------------
            TarrifFromToTypeRepository tarrifFromToTypeRepository = new TarrifFromToTypeRepository(commonContext);
            AddClosedTables.AddTarrifFromToType(new TarrifFromToTypeDetails() { Code = "F", Name = "From" }, tarrifFromToTypeRepository);
            AddClosedTables.AddTarrifFromToType(new TarrifFromToTypeDetails() { Code = "T", Name = "To" }, tarrifFromToTypeRepository);
            tarrifFromToTypeRepository.SubmitChanges();

            //-------------Next Leg---------------
            NextLegRepository nextLegRepository = new NextLegRepository(shipmentContext);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "PICK", Name = "Pick Up" }, nextLegRepository);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "ONCR", Name = "On Carriage" }, nextLegRepository);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "MAL1", Name = "Main Carriage Leg1" }, nextLegRepository);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "MAL2", Name = "Main Carriage Leg2" }, nextLegRepository);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "MAL3", Name = "Main Carriage Leg3" }, nextLegRepository);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "MAL4", Name = "Main Carriage Leg4" }, nextLegRepository);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "PRCR", Name = "Pre Carriage" }, nextLegRepository);
            AddClosedTables.AddNextLeg(new NextLegDetails() { Code = "DELV", Name = "Delivery" }, nextLegRepository);
            nextLegRepository.SubmitChanges();

            //-------------Shipment Level---------------
            ShipmentLevelRepository shipmentLevelRepository = new ShipmentLevelRepository(shipmentContext);
            AddClosedTables.AddShipmentLevels(new ShipmentLevelDetails() { Code = "D", Name = "Direct" }, shipmentLevelRepository);
            AddClosedTables.AddShipmentLevels(new ShipmentLevelDetails() { Code = "C", Name = "Consol" }, shipmentLevelRepository);
            AddClosedTables.AddShipmentLevels(new ShipmentLevelDetails() { Code = "H", Name = "House" }, shipmentLevelRepository);
            AddClosedTables.AddShipmentLevels(new ShipmentLevelDetails() { Code = "A", Name = "Customs" }, shipmentLevelRepository);
            shipmentLevelRepository.SubmitChanges();

            //----------Account Types---------
            AccountTypeRepository accountTypeRepositroy = new AccountTypeRepository(invoiceContext);
            AddClosedTables.AddAccountTypes(new AccountTypeDetails() { Code = "AR", Name = "Account Receivables" }, accountTypeRepositroy);
            AddClosedTables.AddAccountTypes(new AccountTypeDetails() { Code = "AP", Name = "Account Payables" }, accountTypeRepositroy);
            AddClosedTables.AddAccountTypes(new AccountTypeDetails() { Code = "IN", Name = "Income" }, accountTypeRepositroy);
            AddClosedTables.AddAccountTypes(new AccountTypeDetails() { Code = "CO", Name = "Cost of goods sold" }, accountTypeRepositroy);
            AddClosedTables.AddAccountTypes(new AccountTypeDetails() { Code = "CC", Name = "Credit Card" }, accountTypeRepositroy);
            AddClosedTables.AddAccountTypes(new AccountTypeDetails() { Code = "BN", Name = "Bank" }, accountTypeRepositroy);
            AddClosedTables.AddAccountTypes(new AccountTypeDetails() { Code = "UF", Name = "undeposited funds" }, accountTypeRepositroy);
            accountTypeRepositroy.SubmitChanges();

            AccountingTransferTypeRepository accountingTransferTypeRepository = new AccountingTransferTypeRepository(invoiceContext);
            AddClosedTables.AddAccountingTransferTypes(new AccountingTransferTypeDetails() { Code = "ARIN", Name = "ARInvoice Transfer" }, accountingTransferTypeRepository);
            AddClosedTables.AddAccountingTransferTypes(new AccountingTransferTypeDetails() { Code = "ARPA", Name = "ARPayment Transfer" }, accountingTransferTypeRepository);
            AddClosedTables.AddAccountingTransferTypes(new AccountingTransferTypeDetails() { Code = "APIN", Name = "APInvoice Transfer" }, accountingTransferTypeRepository);
            AddClosedTables.AddAccountingTransferTypes(new AccountingTransferTypeDetails() { Code = "APPA", Name = "APPayment Transfer" }, accountingTransferTypeRepository);
            accountingTransferTypeRepository.SubmitChanges();

            //---------AR Payment Status---------
            ARPaymentStatusRepository arPaymentStatusRepository = new ARPaymentStatusRepository(invoiceContext);
            AddClosedTables.AddARPaymentstatus(new ARPaymentStatusDetails() { Code = "DR", Name = "Draft" }, arPaymentStatusRepository);
            AddClosedTables.AddARPaymentstatus(new ARPaymentStatusDetails() { Code = "AD", Name = "Approved" }, arPaymentStatusRepository);
            AddClosedTables.AddARPaymentstatus(new ARPaymentStatusDetails() { Code = "VD", Name = "Void" }, arPaymentStatusRepository);
            AddClosedTables.AddARPaymentstatus(new ARPaymentStatusDetails() { Code = "CL", Name = "Closed" }, arPaymentStatusRepository);
            arPaymentStatusRepository.SubmitChanges();

            //---------AP Invoice Status---------
            APInvoiceStatusRepository apInvoiceStatusRepository = new APInvoiceStatusRepository(invoiceContext);
            AddClosedTables.AddAPInvoiceStatus(new APInvoiceStatusDetails() { Code = "WA", Name = "Waiting For Approval" }, apInvoiceStatusRepository);
            AddClosedTables.AddAPInvoiceStatus(new APInvoiceStatusDetails() { Code = "AD", Name = "Approved" }, apInvoiceStatusRepository);
            AddClosedTables.AddAPInvoiceStatus(new APInvoiceStatusDetails() { Code = "PD", Name = "Paid" }, apInvoiceStatusRepository);
            AddClosedTables.AddAPInvoiceStatus(new APInvoiceStatusDetails() { Code = "PP", Name = "Partially Paid" }, apInvoiceStatusRepository);
            AddClosedTables.AddAPInvoiceStatus(new APInvoiceStatusDetails() { Code = "VD", Name = "Void" }, apInvoiceStatusRepository);

            apInvoiceStatusRepository.SubmitChanges();

            //---------AP Invoice Type---------
            APInvoiceTypeRepository apInvoiceTypeRepository = new APInvoiceTypeRepository(invoiceContext);
            AddClosedTables.AddAPInvoiceTypes(new APInvoiceTypeDetails() { Code = "IN", Name = "Invoice" }, apInvoiceTypeRepository);
            AddClosedTables.AddAPInvoiceTypes(new APInvoiceTypeDetails() { Code = "CD", Name = "Credit" }, apInvoiceTypeRepository);
            apInvoiceTypeRepository.SubmitChanges();

            //---------AP Payment Status---------
            APPaymentStatusRepository apPaymentStatusRepository = new APPaymentStatusRepository(invoiceContext);
            AddClosedTables.AddAPPaymentstatus(new APPaymentStatusDetails() { Code = "DR", Name = "Draft" }, apPaymentStatusRepository);
            AddClosedTables.AddAPPaymentstatus(new APPaymentStatusDetails() { Code = "AD", Name = "Approved" }, apPaymentStatusRepository);
            AddClosedTables.AddAPPaymentstatus(new APPaymentStatusDetails() { Code = "VD", Name = "Void" }, apPaymentStatusRepository);
            AddClosedTables.AddAPPaymentstatus(new APPaymentStatusDetails() { Code = "CL", Name = "Closed" }, apPaymentStatusRepository);
            apPaymentStatusRepository.SubmitChanges();

            //-------- SAT Interfaces ----------//
            SATInterfaceRepository sATInterfaceRepository = new SATInterfaceRepository(invoiceContext);
            AddClosedTables.AddSATInterface(new SATInterfaceDetails() { Code = "NONE", Name = "None" }, sATInterfaceRepository);
            AddClosedTables.AddSATInterface(new SATInterfaceDetails() { Code = "PROF", Name = "Profact 3.2" }, sATInterfaceRepository);
            AddClosedTables.AddSATInterface(new SATInterfaceDetails() { Code = "PROF33", Name = "Profact 3.3" }, sATInterfaceRepository);
            AddClosedTables.AddSATInterface(new SATInterfaceDetails() { Code = "CONT", Name = "Contpaq" }, sATInterfaceRepository);


            sATInterfaceRepository.SubmitChanges();

            //---------shipmentpayable amount type ------//
            ShipmentPayableAmountTypeRepository payableAmountTypeRep = new ShipmentPayableAmountTypeRepository(shipmentContext);
            AddClosedTables.AddShipmentPayableAmountTypes(new ShipmentPayableAmountTypeDetails() { Code = "ACCU", Name = "Accrual" }, payableAmountTypeRep);
            AddClosedTables.AddShipmentPayableAmountTypes(new ShipmentPayableAmountTypeDetails() { Code = "NEXP", Name = "Not Expected" }, payableAmountTypeRep);
            payableAmountTypeRep.SubmitChanges();

            //-------------Permission Type---------------
            PermissionTypeRepository permissionTypeRepository = new PermissionTypeRepository(tempContext);
            AddClosedTables.AddPermissionTypes(new PermissionTypeDetails() { Code = "RDUP", Name = "Read/Update" }, permissionTypeRepository);
            AddClosedTables.AddPermissionTypes(new PermissionTypeDetails() { Code = "NOAC", Name = "No Access" }, permissionTypeRepository);
            AddClosedTables.AddPermissionTypes(new PermissionTypeDetails() { Code = "READ", Name = "Read" }, permissionTypeRepository);
            permissionTypeRepository.SubmitChanges();

            //-------------Feature Type---------------
            FeatureTypeRepository featureTypeRepository = new FeatureTypeRepository(commonContext);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "NEW", Name = "New" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "UPDT", Name = "Update" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "READ", Name = "Read" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "ACT", Name = "Actions" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "SET", Name = "Settings" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "MENU", Name = "Menu" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "AREA", Name = "Area" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "OTH", Name = "Others" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "MODL", Name = "Module" }, featureTypeRepository);
            AddClosedTables.AddFeatureTypes(new FeatureTypeDetails() { Code = "QUER", Name = "Query" }, featureTypeRepository);

            featureTypeRepository.SubmitChanges();

            //-------------Role Type---------------
            RoleTypeRepository roleTypeRepository = new RoleTypeRepository(commonContext);
            AddClosedTables.AddRoleTypes(new RoleTypeDetails() { Code = "SA", Name = "Sales" }, roleTypeRepository);
            AddClosedTables.AddRoleTypes(new RoleTypeDetails() { Code = "AC", Name = "Accounting" }, roleTypeRepository);
            AddClosedTables.AddRoleTypes(new RoleTypeDetails() { Code = "IN", Name = "Internal User" }, roleTypeRepository);
            roleTypeRepository.SubmitChanges();

            //-------------Object Table Type---------------
            ObjectTableTypeRepository objectTableTypeRepository = new ObjectTableTypeRepository(tempContext);
            AddClosedTables.AddObjectTableTypes(new ObjectTableTypeDetails() { Code = "MD", Name = "Master Data" }, objectTableTypeRepository);
            AddClosedTables.AddObjectTableTypes(new ObjectTableTypeDetails() { Code = "BR", Name = "Business Record" }, objectTableTypeRepository);
            objectTableTypeRepository.SubmitChanges();

            //-------------Package---------------
            //PackageRepository packageRepository = new PackageRepository(commonContext);
            //AddClosedTables.AddPackages(new PackageDetails() { Code = "BASC", Name = "Basic" }, packageRepository);
            //AddClosedTables.AddPackages(new PackageDetails() { Code = "BUSN", Name = "Business" }, packageRepository);
            //AddClosedTables.AddPackages(new PackageDetails() { Code = "ECON", Name = "Economy" }, packageRepository);
            //AddClosedTables.AddPackages(new PackageDetails() { Code = "EAWB", Name = "E-AWB" }, packageRepository);
            //AddClosedTables.AddPackages(new PackageDetails() { Code = "DVMT", Name = "Development" }, packageRepository);
            //AddClosedTables.AddPackages(new PackageDetails() { Code = "CUST", Name = "Customs" }, packageRepository);
            //AddClosedTables.AddPackages(new PackageDetails() { Code = "IMPO", Name = "LogBox" }, packageRepository);
            //packageRepository.SubmitChanges();

            //--------- AWB Charges Codes ------//
            AWBChargesCodeRepository awbChargeCodesRepository = new AWBChargesCodeRepository(shipmentContext);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CC", Name = "All Charges Collect" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CZ", Name = "All Charges Collect by Credit Card" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CG", Name = "All Charges Collect by GBL" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PP", Name = "All Charges Prepaid Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PX", Name = "All Charges Prepaid Credit" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PZ", Name = "All Charges Prepaid by Credit Card" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PG", Name = "All Charges Prepaid by GBL" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CP", Name = "Destination Collect Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CX", Name = "Destination Collect Credit" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CM", Name = "Destination Collect by MCO" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "NC", Name = "No Charge" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "NT", Name = "No Weight Charge - Other Charges Collect" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "NZ", Name = "No Weight Charge - Other Charges Prepaid by Credit Card" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "NG", Name = "No Weight Charge - Other Charges Prepaid by GBL" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "NP", Name = "No Weight Charge - Other Charges Prepaid Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "NX", Name = "No Weight Charge - Other Charges Prepaid Credit" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CA", Name = "Partial Collect Credit - Partial Prepaid Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CB", Name = "Partial Collect Credit - Partial Prepaid Credit" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CE", Name = "Partial Collect Credit Card - Partial Prepaid Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "CH", Name = "Partial Collect Credit Card - Partial Prepaid Credit" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PC", Name = "Partial Prepaid Cash - Partial Collect Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PD", Name = "Partial Prepaid Credit - Partial Collect Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PE", Name = "Partial Prepaid Credit Card - Partial Collect Cash" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PH", Name = "Partial Prepaid Credit Card - Partial Collect Credit" }, awbChargeCodesRepository);
            AddClosedTables.AddAWBChargesCodes(new AWBChargesCodeDetails() { Code = "PF", Name = "Partial Prepaid Credit Card - Partial Collect Credit Card" }, awbChargeCodesRepository);
            awbChargeCodesRepository.SubmitChanges();

            //--------- AWB Special Handling Code ------//
            /*
            AWBSpecialHandlingCodeRepository awbSpecialHandlingCodeRepository = new AWBSpecialHandlingCodeRepository(shipmentContext);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ECC", Name = "Shipment with electroniclly concluded cgo contract with no accompanyng paper AWB" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ACT", Name = "Active Temperature Controlled System" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "AOG", Name = "Aircraft on Ground" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "BUP", Name = "Bulk Unitization Programme, Shipper/Consignee Handled Unit" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "CAO", Name = "Cargo Aircraft Only" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "CAT", Name = "Cargo Attendant Accompanying Shipment" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "COM", Name = "Company Mail" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "COL", Name = "Cool Goods" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RDS", Name = "Diagnostic Specimens" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "DIP", Name = "Diplomatic Mail" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "EAP", Name = "e-freight Consignment with Accompanying Paper Documents" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "EAW", Name = "e-freight Consignment with No Accompanying Paper Documents" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "REQ", Name = "Excepted Quantities of Dangerous Goods" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RRE", Name = "Excepted Quantities of Radioactive Material" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PES", Name = "Fish/Seafood" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PEF", Name = "Flowers" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "EAT", Name = "Food Stuffs" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "FRI", Name = "Frozen Goods Subject to Veterinary/Phytosanitary Inspections" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "FRO", Name = "Frozen Goods" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PEP", Name = "Fruits and Vegetables" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ATT", Name = "Goods Attached to Air Waybill" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "GOG", Name = "Hanging Garments" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "HEG", Name = "Hatching Eggs" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "HEA", Name = "Heavy Cargo/150 kilograms and over per piece" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "HUM", Name = "Human Remains in Coffin" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PEA", Name = "Hunting trophies, skin, hide and all articles made from or containing parts of species listed in the CITES (Convention on International Trade in Endangered Species) appendices" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "SCO", Name = "Cargo Secure forAll-Cargo Aircraft Only" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "SPF", Name = "Laboratory Animals" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "SPX", Name = "Cargo Secure for Passenger and All-Cargo Aircraft" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "LIC", Name = "License Required" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "AVI", Name = "Live Animal" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "LHO", Name = "Living Human Organs/Blood" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "MAL", Name = "Mail" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PEM", Name = "Meat" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "MUW", Name = "Munitions of War" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "NWP", Name = "Newspapers, Magazines" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "OBX", Name = "Obnoxious Cargo" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "BIG", Name = "Outsized" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "OHG", Name = "Overhang Item" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PAC", Name = "Passenger and Cargo" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PER", Name = "Perishable Cargo" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "PIL", Name = "Pharmaceuticals" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "XPS", Name = "Priority Small Package" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "QRT", Name = "Quick Ramp Transfer" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RAC", Name = "Reserved Air Cargo" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "SHL", Name = "Save Human Life" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "WET", Name = "Shipments of Wet Material not Packed in Watertight Containers" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "SWP", Name = "Sporting Weapons" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "SUR", Name = "Surface Transportation" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "FIL", Name = "Undeveloped/Unexposed Film" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "VAL", Name = "Valuable Cargo" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "VOL", Name = "Volume" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "VUN", Name = "Vulnerable Cargo" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ELI", Name = "Lithium Ion Batteries otherwise excepted from the IATA DGR" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ELM", Name = "Lithium Metal Batteries otherwise excepted from the IATA DGR" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RCM", Name = "Corrosive RCM" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RCL", Name = "Cryogenic Liquids" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RFW", Name = "Dangerous When Wet" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ICE", Name = "Dry Ice" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "REX", Name = "To be reserved for normally forbidden Explosives, Divisions 1.1, 1.2, 1.3, 1.4F, 1.5 and 1.6" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RCX", Name = "Explosives 1.3C" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RGX", Name = "Explosives 1.3G" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RLI", Name = "Fully Regulated Lithium Ion Batteries (Class 9)" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RLM", Name = "Fully Regulated Lithium Metal Batteries (Class 9)" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RXB", Name = "Explosives 1.4B" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RXC", Name = "Explosives 1.4C" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RXD", Name = "Explosives 1.4D" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RXE", Name = "Explosives 1.4E" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RXG", Name = "Explosives 1.4G" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RXS", Name = "Explosives 1.4S" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RFG", Name = "Flammable Gas" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RFL", Name = "Flammable Liquid" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RFS", Name = "Flammable Solid" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RIS", Name = "Infectious Substance" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "MAG", Name = "Magnetized Material" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RMD", Name = "Miscellaneous Dangerous Goods" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RNG", Name = "Non-Flammable Non-Toxic Gas" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ROP", Name = "Organic Peroxide" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "ROX", Name = "Oxidizer" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RPD", Name = "Toxic Substance" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RPG", Name = "Toxic Gas" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RSP", Name = "Polymeric Beads" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RRW", Name = "Radioactive Material Category I-White" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RRY", Name = "Radioactive Material Categories II-Yellow and III-Yellow" }, awbSpecialHandlingCodeRepository);
            AddClosedTables.AddAWBSpecialHandlingCodes(new AWBSpecialHandlingCodeDetails() { Code = "RSC", Name = "Spontaneously Combustible" }, awbSpecialHandlingCodeRepository);
            awbSpecialHandlingCodeRepository.SubmitChanges();
            */

            //-------------Shared Logistics Update Status---------------
            SharedLogisticsUpdateStatusRepository sharedLogisticsUpdateStatusRepository = new SharedLogisticsUpdateStatusRepository(tempContext);
            AddClosedTables.AddSharedLogisticsUpdateStatus(new SharedLogisticsUpdateStatusDetails() { Code = "ACPT", Name = "Accepted" }, sharedLogisticsUpdateStatusRepository);
            AddClosedTables.AddSharedLogisticsUpdateStatus(new SharedLogisticsUpdateStatusDetails() { Code = "REJT", Name = "Rejected" }, sharedLogisticsUpdateStatusRepository);
            AddClosedTables.AddSharedLogisticsUpdateStatus(new SharedLogisticsUpdateStatusDetails() { Code = "WAIT", Name = "Waiting" }, sharedLogisticsUpdateStatusRepository);
            sharedLogisticsUpdateStatusRepository.SubmitChanges();

            //-------------FHL Status---------------
            FHLStatusRepository fHLStatusRepository = new FHLStatusRepository(shipmentContext);
            AddClosedTables.AddFHLStatus(new FHLStatusDetails() { Code = "ACPT", Name = "Accepted" }, fHLStatusRepository);
            AddClosedTables.AddFHLStatus(new FHLStatusDetails() { Code = "SENT", Name = "Sent" }, fHLStatusRepository);
            AddClosedTables.AddFHLStatus(new FHLStatusDetails() { Code = "NSEN", Name = "Not Sent" }, fHLStatusRepository);
            AddClosedTables.AddFHLStatus(new FHLStatusDetails() { Code = "PSEN", Name = "Partially Sent" }, fHLStatusRepository);
            AddClosedTables.AddFHLStatus(new FHLStatusDetails() { Code = "EROR", Name = "Error" }, fHLStatusRepository);
            fHLStatusRepository.SubmitChanges();

            //-------------FWB Status---------------
            FWBStatusRepository FWBStatusRepository = new FWBStatusRepository(shipmentContext);
            AddClosedTables.AddFWBStatus(new FWBStatusDetails() { Code = "ACPT", Name = "Accepted" }, FWBStatusRepository);
            AddClosedTables.AddFWBStatus(new FWBStatusDetails() { Code = "SENT", Name = "Sent" }, FWBStatusRepository);
            AddClosedTables.AddFWBStatus(new FWBStatusDetails() { Code = "NSEN", Name = "Not Sent" }, FWBStatusRepository);
            AddClosedTables.AddFWBStatus(new FWBStatusDetails() { Code = "EROR", Name = "Error" }, FWBStatusRepository);
            FWBStatusRepository.SubmitChanges();

            //-------------AWB Status---------------
            AWBStatusRepository aWBStatusRepository = new AWBStatusRepository(shipmentContext);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "DIS", Name = "Discrepancy" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "AWD", Name = "Arrival documents delivery" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "AWR ", Name = "Arrival documents received " }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "NFD", Name = "Notify about arrival" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "ARR", Name = "Arrived" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "BKD", Name = "Booked" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "CCD", Name = "Cleared by Customs" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "TRM", Name = "To be Transferred" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "MAN", Name = "Manifested" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "DLV", Name = "Delivered" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "DDL", Name = "Door-delivery to consignee" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "RCF", Name = "Received from Flight" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "RCS", Name = "Received from Shipper" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "RCT", Name = "Received from Transfer" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "TFD", Name = "Transferred" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "PRE", Name = "In Preparation" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "CRC", Name = "Reported by Customs" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "TGC", Name = "Consigement tranferred to customs/government control" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "DEP", Name = "Departed" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "FOH", Name = "On-Hand" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "DOC", Name = "Documents Received" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "OSI", Name = "Other Service Information" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "FNA", Name = "FNA" }, aWBStatusRepository);
            AddClosedTables.AddAWBStatus(new AWBStatusDetails() { Code = "FMA", Name = "FMA" }, aWBStatusRepository);
            aWBStatusRepository.SubmitChanges();

            //---------Recurring Periods---------
            RecurringPeriodRepository recurringPeriodRepository = new RecurringPeriodRepository(globalContext);
            AddClosedTables.AddRecurringPeriods(new RecurringPeriodDetails() { Code = "MO", Name = "Monthly" }, recurringPeriodRepository);
            AddClosedTables.AddRecurringPeriods(new RecurringPeriodDetails() { Code = "YE", Name = "Yearly" }, recurringPeriodRepository);
            AddClosedTables.AddRecurringPeriods(new RecurringPeriodDetails() { Code = "QU", Name = "Quarterly" }, recurringPeriodRepository);
            recurringPeriodRepository.SubmitChanges();

            //---------Entity Last Activity Types---------
            EntityLastActivityTypeRepository entityLastActivityTypeRepository = new EntityLastActivityTypeRepository(tempContext);
            AddClosedTables.AddEntityLastActivityTypes(new EntityLastActivityTypeDetails() { Code = "N", Name = "New" }, entityLastActivityTypeRepository);
            AddClosedTables.AddEntityLastActivityTypes(new EntityLastActivityTypeDetails() { Code = "U", Name = "Update" }, entityLastActivityTypeRepository);
            AddClosedTables.AddEntityLastActivityTypes(new EntityLastActivityTypeDetails() { Code = "V", Name = "View" }, entityLastActivityTypeRepository);
            entityLastActivityTypeRepository.SubmitChanges();

            //---------Payment Methods---------
            PaymentMethodRepository paymentMethodRepository = new PaymentMethodRepository(globalContext);
            AddClosedTables.AddPaymentMethods(new PaymentMethodDetails() { Code = "CC", Name = "Credit Card" }, paymentMethodRepository);
            AddClosedTables.AddPaymentMethods(new PaymentMethodDetails() { Code = "BT", Name = "Bank Transfer" }, paymentMethodRepository);
            AddClosedTables.AddPaymentMethods(new PaymentMethodDetails() { Code = "CH", Name = "Cheque" }, paymentMethodRepository);
            AddClosedTables.AddPaymentMethods(new PaymentMethodDetails() { Code = "PP", Name = "Pay Pal" }, paymentMethodRepository);
            paymentMethodRepository.SubmitChanges();

            //---------Payment Channels---------
            PaymentChannelRepository paymentChannelRepository = new PaymentChannelRepository(globalContext);
            AddClosedTables.AddPaymentChannels(new PaymentChannelDetails() { Code = "PL", Name = "Bluesnap" }, paymentChannelRepository);
            AddClosedTables.AddPaymentChannels(new PaymentChannelDetails() { Code = "DI", Name = "Direct" }, paymentChannelRepository);
            paymentChannelRepository.SubmitChanges();

            //---------Event Type Categories---------
            EventTypeCategoryRepository eventTypeCategoryRepository = new EventTypeCategoryRepository(tempContext);
            AddClosedTables.AddEventTypeCategories(new EventTypeCategoryDetails() { Code = "LEG", Name = "Routings" }, eventTypeCategoryRepository);
            AddClosedTables.AddEventTypeCategories(new EventTypeCategoryDetails() { Code = "OPE", Name = "Operations" }, eventTypeCategoryRepository);
            AddClosedTables.AddEventTypeCategories(new EventTypeCategoryDetails() { Code = "LOG", Name = "Logs" }, eventTypeCategoryRepository);
            AddClosedTables.AddEventTypeCategories(new EventTypeCategoryDetails() { Code = "DOC", Name = "Documents" }, eventTypeCategoryRepository);
            eventTypeCategoryRepository.SubmitChanges();

            //---------Accounting Systems---------
            //AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(commonContext);
            //AddClosedTables.AddAccountingSystems(new AccountingSystemDetails() { Code = "NO", Name = "None", AllowManuallyDueDate =true, IsJournalMode= true, AllowMinusInvoiceLines = true, ShowDownloadScreen = true }, accountingSystemRepository);
            //AddClosedTables.AddAccountingSystems(new AccountingSystemDetails() { Code = "HV", Name = "Hashavshevet", AllowManuallyDueDate = true, IsJournalMode = true, AllowMinusInvoiceLines = true, ShowDownloadScreen = true }, accountingSystemRepository);
            //AddClosedTables.AddAccountingSystems(new AccountingSystemDetails() { Code = "RH", Name = "Rivheet", AllowManuallyDueDate = true, IsJournalMode = true, AllowMinusInvoiceLines = true, ShowDownloadScreen = true }, accountingSystemRepository);
            //AddClosedTables.AddAccountingSystems(new AccountingSystemDetails() { Code = "QB", Name = "Quick Books", IsExternalCodesFromTable = true, IsSingleTaxPerInvoice = true, IsSingleCurrencyAccount = true, IsTaxItemManaged = true, AllowMinusInvoiceLines = false, ShowDownloadScreen = false }, accountingSystemRepository);
            //AddClosedTables.AddAccountingSystems(new AccountingSystemDetails() { Code = "GI", Name = "Logitude Generic Interface", IsSingleTaxPerInvoice = true, AllowManuallyDueDate = true, AllowMinusInvoiceLines = true, ShowDownloadScreen = true, IsTaxItemManaged = false, IsJournalMode = true }, accountingSystemRepository);
            //AddClosedTables.AddAccountingSystems(new AccountingSystemDetails() { Code = "AI", Name = "Logitude Advanced Generic Interface", IsSingleTaxPerInvoice = true, AllowManuallyDueDate = true, AllowMinusInvoiceLines = true, ShowDownloadScreen = true, IsTaxItemManaged = false, IsJournalMode = true, AllowARInvoicesTransfer = true }, accountingSystemRepository);
            //accountingSystemRepository.SubmitChanges();

            //---------Shared Logistics Invitation Status---------
            SharedLogisticsInvitationStatusRepository sharedLogisticsInvitationStatusRepository = new SharedLogisticsInvitationStatusRepository(tempContext);
            AddClosedTables.AddSharedLogisticsInvitationStatus(new SharedLogisticsInvitationStatusDetails() { Code = 1, Name = "Not Invited" }, sharedLogisticsInvitationStatusRepository);
            AddClosedTables.AddSharedLogisticsInvitationStatus(new SharedLogisticsInvitationStatusDetails() { Code = 2, Name = "Invited" }, sharedLogisticsInvitationStatusRepository);
            AddClosedTables.AddSharedLogisticsInvitationStatus(new SharedLogisticsInvitationStatusDetails() { Code = 3, Name = "Activated" }, sharedLogisticsInvitationStatusRepository);
            sharedLogisticsInvitationStatusRepository.SubmitChanges();

            //---------Product Type---------
            ProductTypeRepository productTypeRepository = new ProductTypeRepository(commonContext);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "AE", Name = "Air Export" }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "AI", Name = "Air Import" }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "AD", Name = "Air Domestic", InActive = true }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "AR", Name = "Air Drop", InActive = true }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "OE", Name = "Ocean Export" }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "OI", Name = "Ocean Import" }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "OD", Name = "Ocean Domestic", InActive = true }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "OR", Name = "Ocean Drop", InActive = true }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "IE", Name = "Inland Export" }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "II", Name = "Inland Import" }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "ID", Name = "Inland Domestic", InActive = true }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "IR", Name = "Inland Drop", InActive = true }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "CI", Name = "Customs Import" }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "IN", Name = "Insurance", InActive = true }, productTypeRepository);
            AddClosedTables.AddProductType(new ProductTypeDetails() { Code = "DL", Name = "Delivery", InActive = true }, productTypeRepository);
            productTypeRepository.SubmitChanges();

            //---------Product Period---------
            ProductPeriodRepository productPeriodRepository = new ProductPeriodRepository(commonContext);
            AddClosedTables.AddProductPeriod(new ProductPeriodDetails() { Code = "MO", Name = "Monthly", SearchFields = "MO,Monthly" }, productPeriodRepository);
            AddClosedTables.AddProductPeriod(new ProductPeriodDetails() { Code = "QU", Name = "Quarterly", SearchFields = "QU,Quarterly" }, productPeriodRepository);
            AddClosedTables.AddProductPeriod(new ProductPeriodDetails() { Code = "YE", Name = "Yearly", SearchFields = "YE,Yearly" }, productPeriodRepository);
            productPeriodRepository.SubmitChanges();

            //---------Contact Done Method---------
            ContactDoneMethodRepository doneRepository = new ContactDoneMethodRepository(commonContext);
            AddClosedTables.AddContactDoneMethod(new ContactDoneMethodDetails() { Code = "PC", Name = "Phone Call", SearchFields = "PC,Phone Call" }, doneRepository);
            AddClosedTables.AddContactDoneMethod(new ContactDoneMethodDetails() { Code = "EM", Name = "Email", SearchFields = "EM,Email" }, doneRepository);
            AddClosedTables.AddContactDoneMethod(new ContactDoneMethodDetails() { Code = "GT", Name = "Gift", SearchFields = "GT,Gift" }, doneRepository);
            AddClosedTables.AddContactDoneMethod(new ContactDoneMethodDetails() { Code = "SM", Name = "Sms", SearchFields = "SM,Sms" }, doneRepository);
            AddClosedTables.AddContactDoneMethod(new ContactDoneMethodDetails() { Code = "NO", Name = "None", SearchFields = "NO,None" }, doneRepository);
            doneRepository.SubmitChanges();

            #region VatUniqueType
            VatUniqueTypeRepository vatUniqueTypeRepository = new VatUniqueTypeRepository(commonContext);
            AddClosedTables.AddVatUniqueTypeMethod(new VatUniqueTypeDetails() { Code = "UNT", Name = "Not Unique", ViewOrder = 0 }, vatUniqueTypeRepository);
            AddClosedTables.AddVatUniqueTypeMethod(new VatUniqueTypeDetails() { Code = "USC", Name = "Unique for a specific country", ViewOrder = 1 }, vatUniqueTypeRepository);
            AddClosedTables.AddVatUniqueTypeMethod(new VatUniqueTypeDetails() { Code = "UFA", Name = "Unique for all countries", ViewOrder = 2 }, vatUniqueTypeRepository);
            vatUniqueTypeRepository.SubmitChanges();
            #endregion

            #region VatMandatoryType
            VatMandatoryTypeRepository vatMandatoryTypeRepository = new VatMandatoryTypeRepository(commonContext);
            AddClosedTables.AddVatMandatoryTypeMethod(new VatMandatoryTypeDetails() { Code = "MNT", Name = "Not Mandatory", ViewOrder = 0 }, vatMandatoryTypeRepository);
            AddClosedTables.AddVatMandatoryTypeMethod(new VatMandatoryTypeDetails() { Code = "MSC", Name = "Mandatory for a specific country", ViewOrder = 1 }, vatMandatoryTypeRepository);
            AddClosedTables.AddVatMandatoryTypeMethod(new VatMandatoryTypeDetails() { Code = "MFA", Name = "Mandatory for all countries", ViewOrder = 2 }, vatMandatoryTypeRepository);

            vatUniqueTypeRepository.SubmitChanges();
            #endregion

            #region VatFormatType
            VatFormatTypeRepository vatFormatTypeRepository = new VatFormatTypeRepository(commonContext);
            AddClosedTables.AddVatFormatTypeMethod(new VatFormatTypeDetails() { Code = "NOF", Name = "No Format", ViewOrder = 0 }, vatFormatTypeRepository);
            AddClosedTables.AddVatFormatTypeMethod(new VatFormatTypeDetails() { Code = "FSC", Name = "Apply for a Specific Country", ViewOrder = 1 }, vatFormatTypeRepository);
            AddClosedTables.AddVatFormatTypeMethod(new VatFormatTypeDetails() { Code = "FAC", Name = "Apply for All Countries", ViewOrder = 2 }, vatFormatTypeRepository);
            vatFormatTypeRepository.SubmitChanges();
            #endregion

            #region Quote Closing Reason
            QuoteClosingReasonRepository quoteClosingReasonRep = new QuoteClosingReasonRepository(quotesContext);
            AddClosedTables.AddQuoteClosingReason(new QuoteClosingReasonDetails() { Code = "EQ", Name = "Expensive Quote" }, quoteClosingReasonRep);
            AddClosedTables.AddQuoteClosingReason(new QuoteClosingReasonDetails() { Code = "GS", Name = "Given directly to the Shipping Line" }, quoteClosingReasonRep);
            AddClosedTables.AddQuoteClosingReason(new QuoteClosingReasonDetails() { Code = "LC", Name = "Lost to Competitor" }, quoteClosingReasonRep);
            AddClosedTables.AddQuoteClosingReason(new QuoteClosingReasonDetails() { Code = "LS", Name = "Lack of Service in the Last Shipment" }, quoteClosingReasonRep);
            AddClosedTables.AddQuoteClosingReason(new QuoteClosingReasonDetails() { Code = "XQ", Name = "Expired Quote" }, quoteClosingReasonRep);

            quoteClosingReasonRep.SubmitChanges();
            #endregion

            //-------------Customer Status---------------
            CustomerStatusRepository CustomerStatusRep = new CustomerStatusRepository(commonContext);
            AddClosedTables.AddCustomerStatus(new CustomerStatusDetails() { Code = "POT", Name = "Potential" }, CustomerStatusRep);
            AddClosedTables.AddCustomerStatus(new CustomerStatusDetails() { Code = "WAC", Name = "Waiting for Activation" }, CustomerStatusRep);
            AddClosedTables.AddCustomerStatus(new CustomerStatusDetails() { Code = "ACT", Name = "Active" }, CustomerStatusRep);
            AddClosedTables.AddCustomerStatus(new CustomerStatusDetails() { Code = "INA", Name = "Inactive" }, CustomerStatusRep);
            CustomerStatusRep.SubmitChanges();

            FeatureAccessLevelRepository featureAccessLevelRepository = new FeatureAccessLevelRepository(commonContext);
            AddClosedTables.AddFeatureAccessLevel(new FeatureAccessLevelDetails() { Code = "NO", Name = "None" }, featureAccessLevelRepository);
            AddClosedTables.AddFeatureAccessLevel(new FeatureAccessLevelDetails() { Code = "US", Name = "User" }, featureAccessLevelRepository);
            AddClosedTables.AddFeatureAccessLevel(new FeatureAccessLevelDetails() { Code = "BU", Name = "Business Unit" }, featureAccessLevelRepository);
            AddClosedTables.AddFeatureAccessLevel(new FeatureAccessLevelDetails() { Code = "PR", Name = "Parent" }, featureAccessLevelRepository);
            AddClosedTables.AddFeatureAccessLevel(new FeatureAccessLevelDetails() { Code = "OR", Name = "Organization" }, featureAccessLevelRepository);
            featureAccessLevelRepository.SubmitChanges();

            //---------Payment Currencies---------
            PaymentCurrencyRepository paymentCurrencyRepository = new PaymentCurrencyRepository(globalContext);
            AddClosedTables.AddPaymentCurrencies(new PaymentCurrencyDetails() { Code = "USD", Name = "United States Of America Dollar" }, paymentCurrencyRepository);
            //AddClosedTables.AddPaymentCurrencies(new PaymentCurrencyDetails() { Code = "NIS", Name = "Shekel" }, paymentCurrencyRepository);
            //AddClosedTables.AddPaymentCurrencies(new PaymentCurrencyDetails() { Code = "EUR", Name = "Euro" }, paymentCurrencyRepository);
            paymentCurrencyRepository.SubmitChanges();

            //---------Tenant Types---------
            TenantTypeRepository tenantTypeRepository = new TenantTypeRepository(globalContext);
            AddClosedTables.AddTenantTypes(new TenantTypeDetails() { Code = "FOR", Name = "Forwarder" }, tenantTypeRepository);
            AddClosedTables.AddTenantTypes(new TenantTypeDetails() { Code = "SHC", Name = "Shipper/Consignee" }, tenantTypeRepository);
            AddClosedTables.AddTenantTypes(new TenantTypeDetails() { Code = "AIR", Name = "Airlines" }, tenantTypeRepository);
            AddClosedTables.AddTenantTypes(new TenantTypeDetails() { Code = "CUT", Name = "Customs" }, tenantTypeRepository);
            AddClosedTables.AddTenantTypes(new TenantTypeDetails() { Code = "CRM", Name = "CRM" }, tenantTypeRepository);
            tenantTypeRepository.SubmitChanges();


            SharedManifestsStatusRepository sharedManifestsStatusRepository = new SharedManifestsStatusRepository(commonContext);
            AddClosedTables.AddSharedManifestsStatus(new SharedManifestsStatusDetails() { StatusCode = "WAIT", StatusName = "Waiting", SearchFields = "WAIT,Waiting" }, sharedManifestsStatusRepository);
            AddClosedTables.AddSharedManifestsStatus(new SharedManifestsStatusDetails() { StatusCode = "COMP", StatusName = "Completed", SearchFields = "CREA,Completed" }, sharedManifestsStatusRepository);
            AddClosedTables.AddSharedManifestsStatus(new SharedManifestsStatusDetails() { StatusCode = "CANC", StatusName = "Cancelled", SearchFields = "CANC,Cancelled" }, sharedManifestsStatusRepository);
            sharedManifestsStatusRepository.SubmitChanges();

            CustomsInterfaces(commonContext);



            //---------SAT Payment Methods---------
            SATPaymentMethodRepository sATPaymentMethodRepository = new SATPaymentMethodRepository(invoiceContext);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "01", Name = "Cash", LocalName = "Efectivo" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "02", Name = "Check", LocalName = "Cheque nominativo" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "03", Name = "Bank Transfer", LocalName = "Transferencia electrónica de fondos" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "04", Name = "Credit Card", LocalName = "Tarjeta de crédito" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "05", Name = "Electronic Wallet", LocalName = "Monedero electrónico" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "06", Name = "Gift Card", LocalName = "Dinero electrónico" }, sATPaymentMethodRepository);
            //AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "07", Name = "Digital Bank Card", LocalName = "Tarjetas digitales" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "08", Name = "Food Coupon", LocalName = "Vales de despensa" }, sATPaymentMethodRepository);
            //AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "09", Name = "Goods", LocalName = "Bienes" }, sATPaymentMethodRepository);
            //AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "10", Name = "Service", LocalName = "Servicio" }, sATPaymentMethodRepository);
            //AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "11", Name = "3rd Party payment", LocalName = "Por Cuenta de Tercero" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "12", Name = "Grant", LocalName = "Dación en pago" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "13", Name = "Payment in subrogation", LocalName = "Pago por subrogación" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "14", Name = "3rd Party Payment", LocalName = "Pago por consignación" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "15", Name = "Condonation", LocalName = "Condonación" }, sATPaymentMethodRepository);
            //AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "16", Name = "Cancellation", LocalName = "Cancelacion" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "17", Name = "Offset", LocalName = "Compensación" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "23", Name = "Novation", LocalName = "Novación" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "24", Name = "Confusion", LocalName = "Confusión" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "25", Name = "Debt Remittance", LocalName = "Remisión de deuda" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "26", Name = "Prescription or expiration", LocalName = "Prescripción o caducidad" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "27", Name = "To the satisfaction of the creditor", LocalName = "A satisfacción del acreedor" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "28", Name = "Debit Card", LocalName = "Tarjeta de débito" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "29", Name = "Service Card", LocalName = "Tarjeta de servicios" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "30", Name = "Advance Payments", LocalName = "Aplicación de anticipos" }, sATPaymentMethodRepository);
            //AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "98", Name = "No Apply", LocalName = "NA" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "99", Name = "To define", LocalName = "Por definir" }, sATPaymentMethodRepository);
            AddClosedTables.AddSATPaymentMethods(new SATPaymentMethodDetails() { Code = "31", Name = "Exchange Bureau", LocalName = "Intermediario pagos" }, sATPaymentMethodRepository);

            sATPaymentMethodRepository.SubmitChanges();


            #region WarehouseType
            WarehouseTypeRepository warehouseTypeRep = new WarehouseTypeRepository(commonContext);
            AddClosedTables.AddWarehouseTypes(new WarehouseTypeDetails() { Code = "TM", Name = "Terminal", SearchFields = "TM,Terminal" }, warehouseTypeRep);
            AddClosedTables.AddWarehouseTypes(new WarehouseTypeDetails() { Code = "BO", Name = "Bonded", SearchFields = "BO,Bonded" }, warehouseTypeRep);
            warehouseTypeRep.SubmitChanges();
            #endregion

            UpdateLoginPolicyClosedTable(commonContext);
            UpdateMetodoPagoClosedTable(commonContext);
            UpgradeAWBOCIClosedTables(shipmentContext);
            UpgradeAccountingInformationIdentifiers(shipmentContext);
            UpdateManifestStatusClosedTable(shipmentContext);
            UpdateOtherParticipantIdClosedTable(shipmentContext);
            UpdateARInvoiceLineActionsClosedTable(invoiceContext);
            UpdateCustomsTransmissionsStatusClosedTable(shipmentContext);
            UpdateOBLTypes(shipmentContext);
            UpdateRegistryDateTypes(commonContext);
            UpdateUsoCFDIClosedTable(commonContext);
            UpdateShipmentCustomsMessageTypes(shipmentContext);
            UpdateSATTransferStatusClosedTable(invoiceContext);
            UpdateSATInvoiceStatusClosedTable(invoiceContext);
            UpdateINTTRASettingModeClosedTable(commonContext);
            UpdateTemperatureUnits(commonContext);
            UpdatePickUpDeliveryTransportModes(shipmentContext);
            UpdateINTTRASIStatuses(shipmentContext);
            UpdateINTTRAStatuses(shipmentContext);
            UpdateINTTRADocumentTypes(shipmentContext);
        }

        public static void CustomsInterfaces(CommonDataContext commonContext)
        {
            //---------Customs Interfaces---------
            CustomsInterfaceRepository customsInterfaceRepository = new CustomsInterfaceRepository(commonContext);
            AddClosedTables.AddCustomsInterfaces(new CustomsInterfaceDetails() { Code = "NO", Name = "None", InterfaceType = "NO" }, customsInterfaceRepository);
            AddClosedTables.AddCustomsInterfaces(new CustomsInterfaceDetails() { Code = "ABM", Name = "ABM CustomsWare", InterfaceType = "LO" }, customsInterfaceRepository);
            AddClosedTables.AddCustomsInterfaces(new CustomsInterfaceDetails() { Code = "ART", Name = "Artemus", InterfaceType = "IM" }, customsInterfaceRepository);
            AddClosedTables.AddCustomsInterfaces(new CustomsInterfaceDetails() { Code = "CBP", Name = "CBP direct", InterfaceType = "EX" }, customsInterfaceRepository);
            //cache on client USE : "Update Customs With Out Tenant 0"
            //INSERT INTO "CUSTOMSINTERFACES" (CODE, NAME, SEARCHFIELDS, INACTIVE, INTERFACETYPE) VALUES ('CMN', 'Maman Courier', 'CMN,Maman Courier', '0', 'IM');
            ///AddClosedTables.AddCustomsInterfaces(new CustomsInterfaceDetails() { Code = "CMN", Name = "Maman Courier", InterfaceType = "IM" }, customsInterfaceRepository);

            customsInterfaceRepository.SubmitChanges();
        }

        private void UpdateINTTRASIStatuses(ShipmentsContext myContext)
        {
            INTTRASIStatusRepository myRepository = new INTTRASIStatusRepository(myContext);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "NSEN", Name = "Not Sent" }, myRepository);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "SENT", Name = "Sent" }, myRepository);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "ACPT", Name = "Accepted" }, myRepository);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "EROR", Name = "Error" }, myRepository);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "ACCR", Name = "Accepted by carrier" }, myRepository);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "RJCR", Name = "Rejected by carrier" }, myRepository);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "ACIN", Name = "Accepted by INTTRA" }, myRepository);
            AddClosedTables.AddINTTRASIStatus(new CodeNameDetails() { Code = "RJIN", Name = "Rejected by INTTRA" }, myRepository);
            myRepository.SubmitChanges();
        }
        private void UpdateINTTRAStatuses(ShipmentsContext myContext)
        {
            INTTRAStatusRepository myRepository = new INTTRAStatusRepository(myContext);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "3", Name = "RETURNED TO SHIPPER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "2", Name = "RETURNED TO CARRIER'S TERMINAL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "A", Name = "ARRIVED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "A1", Name = "AGRICULTURE CANADA HOLD" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "A2", Name = "AGRICULTURE CANADA RELEASED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "A3", Name = "AGRICULTURE CANADA REFUSED ENTRY" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "A4", Name = "AGRICULTURE CANADA CONDITIONAL RELEASE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AA", Name = "PICK-UP APPOINTMENT DATE AND TIME " }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AC", Name = "AWAITING CLEARANCE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AD", Name = "DELIVERY APPOINTMENT DATE AND TIME" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AE", Name = "LOADED ON VESSEL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AF", Name = "DEPARTED PICKUP LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AG", Name = "ESTIMATED DELIVERY" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AH", Name = "ATTEMPTED DELIVERY" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AI", Name = "SHIPMENT HAS BEEN RECONSIGNED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AJ", Name = "TENDERED FOR DELIVERY" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AL", Name = "LOADED ON RAIL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AM", Name = "LOADED ON TRUCK" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AN", Name = "DELIVERED TO AIR CARRIER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AO", Name = "LOADED ON BARGE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AP", Name = "LOADED ON FEEDER VESSEL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AR", Name = "RAIL ARRIVAL AT DESTINATION INTERMODAL RAMP" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AV", Name = "AVAILABLE FOR DELIVERY" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "AW", Name = "AWAITING EXPORT" }, myRepository);

            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "B", Name = "BAD ORDER (INOPERATIVE OR DAMAGED)" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "BA", Name = "SET OFF AT AGENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "BC", Name = "STORAGE - IN - TRANSIT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "BD", Name = "RECOMMITTED DELIVERY DATE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "BE", Name = "EST. TIME OF ARRIVAL AT SCHEDULED PICK-UP LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "BF", Name = "BOOKING CONFIRMED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "BR", Name = "BILL OF LADING RELEASED" }, myRepository);

            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "C", Name = "ESTIMATED TO DEPART TERMINAL LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "C1", Name = "CANADA CUSTOMS HOLD" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "C2", Name = "CANADA CUSTOMS INSPECTION SCHEDULED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CA", Name = "SHIPMENT CANCELLED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CB", Name = "CHASSIS TIE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CC", Name = "CHASSIS UN-TIE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CD", Name = "RECEIVED AT ORIGIN" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CI", Name = "PASSING" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CO", Name = "CARGO RECEIVED AT CONTRACTUAL PLACE OF RECEIPT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CR", Name = "CARRIER RELEASE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CS", Name = "CONTAINER SEALED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CT", Name = "CUSTOMS RELEASED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CU", Name = "CARRIER AND CUSTOMS RELEASE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "CV", Name = "CONTAINER REHANDLED" }, myRepository);

            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "D", Name = "COMPLETED UNLOADING AT DELIVERY LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "DA", Name = "REPAIR AUTHORIZATION REQUESTED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "DC", Name = "UNIT CLEANED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "DN", Name = "DELIVERY NOT CONFIRMED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "DP", Name = "UNIT PRE-TRIPPED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "DR", Name = "REPAIR AUTHORIZATION RECEIVED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "DS", Name = "DAMAGE SURVEY REQUESTED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "DT", Name = "DAMAGE SURVEY COMPLETED " }, myRepository);

            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "E", Name = "ESTIMATED TO ARRIVE (EN ROUTE)" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "EA", Name = "ESTIMATE APPROVED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "ED", Name = "EARLY DELIVERY APPOINTMENT DATE AND/OR TIME" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "EE", Name = "EMPTY EQUIPMENT DISPATCHED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "EI", Name = "INTERCHANGE INFORMATION RECEIVED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "EP", Name = "EARLY PICKUP APPOINTMENT DATE AND/OR TIME" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "ER", Name = "ESTIMATE RECEIVED" }, myRepository);

            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "F", Name = "IN FLIGHT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "FP", Name = "FREIGHT PAID" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "FT", Name = "FREE TIME EXPIRED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "G", Name = "REPAIRED AND/OR RELEASED FROM BAD ORDER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "GI", Name = "TERMINAL GATE INSPECTION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "GO", Name = "GENERAL ORDER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "H", Name = "EQUIPMENT SHOPPED FOR HEAVY REPAIR" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "HA", Name = "HELD - PROTECTIVE SERVICE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "HE", Name = "HELD-AWAITING SHIPPER'S EXPORT DOCUMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "HF", Name = "HELD-AWAITING RECOUNT/WEIGHT/DESCRIPTION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "HG", Name = "HELD ON GROUND" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "HH", Name = "HELD-NO BOOKING NUMBER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "HI", Name = "HELD-TITLE CLEARANCE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "HR", Name = "HOLD RELEASED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "I", Name = "IN-GATE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "IB", Name = "U.S. CUSTOMS IN-BOND MOVEMENT AUTHORIZED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "IR", Name = "MOVEMENT TYPE CHANGED FROM IN-BOND TO NOT IN-BOND" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "J", Name = "DELIVERED TO CONNECTING LINE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "K", Name = "ARRIVED AT CUSTOMS" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "L", Name = "LOADING" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "LD", Name = "LATE DELIVERY APPOINTMENT DATE AND/OR TIME" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "LP", Name = "LATE PICKUP APPOINTMENT DATE AND/OR TIME" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "MT", Name = "EMPTY COMMITTED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "N", Name = "NO PAPERWORK RECEIVED WITH SHIPMENT OR EQUIPMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NC", Name = "CONFIRMATION OF NOTIFICATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "ND", Name = "TECHNICAL CHARGES DUE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NF", Name = "FREE TIME TO EXPIRE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NH", Name = "NO HAZARDOUS MATERIAL DOCUMENT RECEIVED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NO", Name = "OCEAN CHARGES PAID" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NP", Name = "TERMINAL CHARGES PAID" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NR", Name = "SHIPMENT INFORMATION NOT RECEIVED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NS", Name = "NO SEAL ON LOAD" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NT", Name = "NOTIFICATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "NU", Name = "NOTIFICATION REFUSED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "O", Name = "PAPERWORK REC'D-DID NOT RECEIVE SHIPMENT OR EQUIPMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "OA", Name = "OUT-GATE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "OB", Name = "ORIGINAL BILL OF LADING RECEIVED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "OF", Name = "OFF-HIRE CONTAINER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "OH", Name = "ON HAND" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "ON", Name = "ON-HIRE CONTAINER" }, myRepository);

            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "P", Name = "DEPARTED TERMINAL LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PA", Name = "US CUSTOM HOLD INTENSIVE EXAMINATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PB", Name = "US CUSTOM HOLD INSUFFICIENT PAPERWORK" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PC", Name = "US CUSTOM HOLD DISCREPANCY IN PAPERWORK" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PD", Name = "US CUSTOM HOLD DISCREPANCY IN PIECE COUNT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PE", Name = "US CUSTOM HOLD HOLD BY COAST GUARD" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PF", Name = "US CUSTOM HOLD HOLD BY F.B.I." }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PG", Name = "US CUSTOM HOLD HOLD BY LOCAL LAW ENFORCEMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PH", Name = "US CUSTOM HOLD HOLD BY COURT IMPOSED LIEN" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PI", Name = "US CUSTOM HOLD HOLD BY FOOD AND DRUG" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PJ", Name = "US CUSTOM HOLD HOLD BY FISH AND WILDLIFE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PK", Name = "US CUSTOM HOLD HOLD BY DRUG ENFORCEMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PL", Name = "US DEPT. AGR. HOLD FOR INTENSIVE INVESTIGATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PM", Name = "US DEPT. AGR. HOLD FOR UNREGISTERED PRODUCER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PN", Name = "US DEPT. AGR. HOLD FOR RESTRICTED COMMODITY" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PO", Name = "US DEPT. AGR. HOLD FOR INSECT INFESTATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PP", Name = "US DEPT. AGR. HOLD FOR BACTERIAL CONTAMINATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PQ", Name = "U.S. CUSTOMS HOLD AT PLACE OF VESSEL ARRIVAL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PR", Name = "U.S. CUSTOMS HOLD AT IN-BOND DESTINATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PS", Name = "U.S. DEPT. OF AGRIC. HOLD AT PLACE OF VESSEL ARRIVAL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PT", Name = "U.S. DEPT OF AGRICULTURE HOLD AT IN-BOND DESTINATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PU", Name = "OTHER U.S. AGENCY HOLD AT PLACE OF VESSEL ARRIVAL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PV", Name = "OTHER U.S. AGENCY HOLD AT IN-BOND DESTINATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PW", Name = "U.S. DEPARTMENT OF AGRICULTURE HOLD FOR FUMIGATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "PX", Name = "U.S. DEPT. OF AGRIC HOLD FOR INSPECTION OR DOCUMENTAT" }, myRepository);

            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "R", Name = "RECEIVED FROM PRIOR CARRIER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "RA", Name = "PICKUP APPOINTMENT REQUESTED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "RB", Name = "DELIVERY APPOINTMENT REQUESTED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "RC", Name = "RESERVE CONTAINER AGAINST BOOKING" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "RD", Name = "RETURN CONTAINER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "RI", Name = "MOVEMENT TYPE CHANGED FROM NOT IN-BOND TO IN-BOND" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "RL", Name = "RAIL DEPARTURE FROM ORIGIN INTERMODAL RAMP" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "RN", Name = "RENOTIFICATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "S", Name = "SPOTTED AT CONSIGNEE'S LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "SA", Name = "SHIPMENT SPLIT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "SB", Name = "SHIPMENT CONSOLIDATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "SC", Name = "SEALS ALTERED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "SD", Name = "SHIPMENT DELAYED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "SI", Name = "RECEIPT OF SHIPPING INSTRUCTIONS" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "SN", Name = "SHIPMENT NOT AUTHORIZED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "T", Name = "AT TERMINAL; INTRA-TERMINAL MOVEMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "TC", Name = "HELD FOR TERMINAL CHARGES" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "TM", Name = "INTRA-TERMINAL MOVEMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "TO", Name = "TERMINATE TO OWNER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "U", Name = "UNLOADING" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UA", Name = "UNIT - LEASED TO CONNECTING LINE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UB", Name = "UNIT - RETURNED FROM CONNECTING LINE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UC", Name = "UNIT - SHOPPED HELD AT TERMINAL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UD", Name = "UNIT - COFC/TOFC SERVICE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UE", Name = "UNIT - PICKUP/DELIVERY SERVICE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UP", Name = "UNABLE TO PROCESS SHIPMENT" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UR", Name = "UNLOADED FROM A RAIL CAR" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UV", Name = "UNLOADED FROM VESSEL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "UW", Name = "INTERMODAL UNIT WEIGHED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "V", Name = "VESSEL REHANDLE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "VA", Name = "VESSEL ARRIVAL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "VD", Name = "VESSEL DEPARTURE" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "W", Name = "RELEASED BY CUSTOMER" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "WH", Name = "WEIGHT TOO HEAVY - HIGHWAY" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "WR", Name = "WEIGHT TOO HEAVY - RAIL" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X", Name = "REMOVED FROM CUSTOMER DOCK OR SIDING" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X1", Name = "ARRIVED AT DELIVERY LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X2", Name = "ESTIMATED TIME OF ARRIVAL AT CONSIGNEE LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X3", Name = "ARRIVED AT PICK-UP LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X4", Name = "ARRIVED AT TERMINAL LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X5", Name = "ARRIVED AT DELIVERY LOCATION LOADING DOCK" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X6", Name = "EN ROUTE TO DELIVERY LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X7", Name = "EN ROUTE TO PICK-UP LOCATION" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X8", Name = "ARRIVED AT PICK-UP LOCATION LOADING DOCK" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "X9", Name = "DELIVERY APPOINTMENT SECURED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "XA", Name = "PICK-UP APPOINTMENT SECURED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "XB", Name = "SHIPMENT ACKNOWLEDGED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "Y", Name = "CONSTRUCTIVELY PLACED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "Z", Name = "ACTUALLY PLACED" }, myRepository);
            AddClosedTables.AddINTTRAStatus(new CodeNameDetails() { Code = "ZZ", Name = "MUTUALLY DEFINED" }, myRepository);


            myRepository.SubmitChanges();
        }
        private void UpdateINTTRADocumentTypes(ShipmentsContext myContext)
        {
            //  BillOfLadingOriginal        BL Original
            //  BillOfLadingCopy            BL Copy
            //  SeaWayBill                  Sea Waybill
            //  HouseBillOfLading           House BL

            INTTRADocumentTypeRepository myRepository = new INTTRADocumentTypeRepository(myContext);
            AddClosedTables.AddINTTRADocumentType(new CodeNameDetails() { Code = "ORIG", Name = "BL Original" }, myRepository);
            AddClosedTables.AddINTTRADocumentType(new CodeNameDetails() { Code = "COPY", Name = "BL Copy" }, myRepository);
            AddClosedTables.AddINTTRADocumentType(new CodeNameDetails() { Code = "BILL", Name = "Sea Waybill" }, myRepository);
            AddClosedTables.AddINTTRADocumentType(new CodeNameDetails() { Code = "LADN", Name = "House BL" }, myRepository);
            myRepository.SubmitChanges();
        }

        private void UpdateINTTRASettingModeClosedTable(CommonDataContext commonContext)
        {
            INTTRASettingModeRepository entityRepository = new INTTRASettingModeRepository(commonContext);
            AddClosedTables.AddINTTRASettingMode(new CodeNameDetails() { Code = "TEST", Name = "Test" }, entityRepository);
            AddClosedTables.AddINTTRASettingMode(new CodeNameDetails() { Code = "PROD", Name = "Production" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpgradeAWBOCIClosedTables(ShipmentsContext shipmentContext)
        {
            AWBCustomsInformationRepository aWBCustomsInfoRepository = new AWBCustomsInformationRepository(shipmentContext);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "AC", Name = "Account Consignor" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "E", Name = "Authorized Economic Operator" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "A", Name = "Automated Broker Interface (ABI) Filer Code" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "C", Name = "Certificate Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "D", Name = "Dangerous Goods" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "L", Name = "Exemption Legend" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "V", Name = "Invoice Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "I", Name = "Item Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "F", Name = "Facilities Information and Resource Management Systems (FIRMS) Code" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "KC", Name = "Known Consignor (consignor for both passenger and all cargo aircraft only)" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "M", Name = "Movement Reference Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "P", Name = "Packing List Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "RA", Name = "Regulated Agent" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "N", Name = "Seal Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "S", Name = "System Downtime Reference" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "T", Name = "Trader Identification Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "U", Name = "Unique Consignment Reference Number" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "DL", Name = "Dangerous Goods" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "SM", Name = "Screening Method" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "SD", Name = "Security Status Date and Time" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "SN", Name = "Security Status Name of Issuer" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "SS", Name = "Security Status" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "ST", Name = "Security Textual Statement" }, aWBCustomsInfoRepository);
            AddClosedTables.AddAWBCustomsInfo(new AWBCustomsInfoDetails() { Code = "ED", Name = "Expiry Date" }, aWBCustomsInfoRepository);
            aWBCustomsInfoRepository.SubmitChanges();


            AWBInformationRepository aWBInformationRepository = new AWBInformationRepository(shipmentContext);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ACC", Name = "Accounting Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "AGT", Name = "Agent" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ARD", Name = "Agent Reference Data" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "API", Name = "Air Waybill Piece Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "AIR", Name = "Airline Header" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ALA", Name = "Allotment Availability Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ALI", Name = "Allotment Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ALR", Name = "Allotment Remaining" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ALT", Name = "Allotment Total" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "AUD", Name = "Allotment Used Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "NFY", Name = "Also Notify" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "AMD", Name = "Amendment Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "AID", Name = "Arrival Information Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ATH", Name = "Authorization" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "AVS", Name = "Availability Supplementary Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ABI", Name = "AWB Amount Detail Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ACS", Name = "AWB Charge Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ACD", Name = "AWB Consignment Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CER", Name = "AWB Content Certification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ISU", Name = "AWB Issue Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ARI", Name = "AWB Recapitulation Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ABS", Name = "AWB Supplementary Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ABT", Name = "AWB Total Amount Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ATW", Name = "AWB Total Weight Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "BGD", Name = "Baggage Detail Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "BGT", Name = "Baggage Tag Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "REF", Name = "Booking References" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CCL", Name = "Cargo Control Location" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CRD", Name = "Carrier Reference Data" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CWI", Name = "CASS AWB Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CBD", Name = "CASS Billing Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CBI", Name = "CASS Billing Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CBP", Name = "CASS Billing Period" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CIN", Name = "CASS Identification Number" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CIH", Name = "CASS Invoice Header Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CDC", Name = "CC Charges in Destination Currency" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CAI", Name = "CCA/Adjustment Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CAS", Name = "CCA/Adjustment Supplementary Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CTI", Name = "CCA/Adjustment Total Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CTW", Name = "CCA/Adjustment Total Weight Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RQD", Name = "Charge Calculation Answer Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RQT", Name = "Charge Calculation Answer Totals" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RQH", Name = "Charge Calculation Request Header" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RQU", Name = "Charge Calculation Request — ULD" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RQV", Name = "Charge Calculation Request — Volume" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CDI", Name = "Charge Declarations" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CVD", Name = "Charge Declarations" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "COL", Name = "Collect Charge Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "COI", Name = "Commission Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CNE", Name = "Consignee" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CCD", Name = "Consignment Control Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CMI", Name = "Consignment Onward Movement Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CID", Name = "Correction Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CBR", Name = "Courier Baggage Receiver" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CBS", Name = "Courier Baggage Sender" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CBV", Name = "Courier Baggage Voucher Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CUR", Name = "Currency Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CUS", Name = "Customer Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CAN", Name = "Customs Action Notification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CND", Name = "Customs Notification Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "COR", Name = "Customs Origin" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DTN", Name = "Date/Time of Notification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DES", Name = "Despatch Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DAI", Name = "DGD Additional Handling Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DAP", Name = "DGD “All Packed in One” Indication" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DAT", Name = "DGD “All Packed in One” Total" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DCI", Name = "DGD Emergency Contact Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DHD", Name = "DGD Header Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DAU", Name = "DGD Item authorization" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DII", Name = "DGD Item Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DNR", Name = "DGD Item Number" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DPI", Name = "DGD Item Packing Group and Instructions" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DQP", Name = "DGD Item Quantity and Type of Packing" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DSN", Name = "DGD Item Shipping Name" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DOS", Name = "DGD Overpack Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DRA", Name = "DGD Radioactive Activity Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DRC", Name = "DGD Radioactive Consignment Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DRP", Name = "DGD Radioactive Packing Instructions" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DSU", Name = "DGD Signatory Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DIM", Name = "Dimensions Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DOC", Name = "Documentation Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "CRR", Name = "Embargo Carriage Restrictions" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "COM", Name = "Embargoed Commodities" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "JST", Name = "Embargo Justification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RTS", Name = "Embargo Routes/Areas" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "EIC", Name = "Empty Equipment in Compartment Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "EXP", Name = "Export" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "FLT", Name = "Flight Booking" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TXT", Name = "Free Text Description" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "GRI", Name = "Grand AWB Recapitulation Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "GTI", Name = "Grand Total Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HDL", Name = "Handling Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HTS", Name = "Harmonised Tariff Schedule Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HPI", Name = "House Waybill Piece Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HAH", Name = "HWB Agent’s Head Office" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HCD", Name = "HWB Consignment Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HLC", Name = "HWB Letter of Credit Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HBS", Name = "House Waybill Summary Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "IMP", Name = "Import" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ITA", Name = "Invoice Total Amount Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ITW", Name = "Invoice Total Weight Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MCH", Name = "Mail Consignment Header" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MCT", Name = "Mail Consignment Total" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MID", Name = "Mail Inbound Data" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MLI", Name = "Mail Label Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MOD", Name = "Mail Outbound Data" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MSD", Name = "Mail Status Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MUD", Name = "Mail ULD Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MBI", Name = "Master Waybill Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MSU", Name = "Message Sequence and ULD Origin" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MAT", Name = "Message Advice Type" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MPI", Name = "Movement Priority Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "NBI", Name = "Net Billing Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "NNS", Name = "Net/Net Sales" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "NEW", Name = "New Information" }, aWBInformationRepository);

            aWBInformationRepository.SubmitChanges();

            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "NOM", Name = "Nominated Handling Party" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "OLD", Name = "Original Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "OTH", Name = "Other Charges" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "OCI", Name = "Other Customs Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "OPI", Name = "Other Participant Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "OSI", Name = "Other Service Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "PAS", Name = "Passenger Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "PPD", Name = "Prepaid Charge Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "PRD", Name = "Planning Request Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "PID", Name = "Product Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RTD", Name = "Rate Description" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RID", Name = "Rate Information Answer Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RIH", Name = "Rate Information Answer Header" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RIR", Name = "Rate Information Request Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ACK", Name = "Reason for Acknowledgement" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RCI", Name = "Recapitulation Amount Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RTI", Name = "Recapitulation Total Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "REC", Name = "Receptacle Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "RTG", Name = "Routing" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SII", Name = "Sales Incentive Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SAR", Name = "Schedule and Availability Information Request Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SAA", Name = "Schedule and Availability Information Answer Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SKH", Name = "Schedule Information Answer Header" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SRI", Name = "Shipment Reference Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SHP", Name = "Shipper" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SCI", Name = "Special Customs Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SPH", Name = "Special Handling Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SSR", Name = "Special Service Request" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "STS", Name = "Status Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SLC", Name = "Status List Criteria" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "STI", Name = "Storage Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SRA", Name = "Supplementary Rate Information Answer Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SRR", Name = "Supplementary Rate Information Request Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SSI", Name = "Supplementary Status Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SCS", Name = "Surface Charge Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SDI", Name = "Surface Delivery Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SPI", Name = "Surface Pickup Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SVA", Name = "Surface Vehicle Arrival Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SVL", Name = "Surface Vehicle Delay Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SVD", Name = "Surface Vehicle Departure Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "SVN", Name = "Surface Vehicle Next Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TXS", Name = "Tax Summary" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TID", Name = "Terminal Identification" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TOT", Name = "Total Amount" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TAR", Name = "Total AWB Recapitulation Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TCC", Name = "Total Collect Charges" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TRN", Name = "Transfer/Transit Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "TRA", Name = "Transit" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "UCI", Name = "ULD Connection Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ULD", Name = "ULD Description" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "UDI", Name = "ULD Destination Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "UII", Name = "ULD Inclusion Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "UMI", Name = "ULD Movement Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "UPI", Name = "Unique Piece Information" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "VOD", Name = "Vehicle Operator Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "VCD", Name = "Void/Cancel Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "WBD", Name = "Waybill Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "WBL", Name = "Waybill Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "WBH", Name = "Waybill Header Details" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "WBI", Name = "Waybill Identification" }, aWBInformationRepository);

            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "HWB", Name = "House Waybill" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "MAL", Name = "Mail" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "DCL", Name = "Declarant" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "BRK", Name = "Broker" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "OSS", Name = "The Regulated Agent Accepting the Security Status for a Consignment Issued by Another Regulated Agent" }, aWBInformationRepository);
            AddClosedTables.AddAWBInformation(new AWBInformationDetails() { Code = "ISS", Name = "The Regulated Agent Issuing the Security Status for a Consignment" }, aWBInformationRepository);

            aWBInformationRepository.SubmitChanges();

        }
        private void UpgradeAccountingInformationIdentifiers(ShipmentsContext shipmentContext)
        {
            AccountingInformationIdentifierRepository entityRepository = new AccountingInformationIdentifierRepository(shipmentContext);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "CRN", Name = "Credit Card Number" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "CRD", Name = "Credit Card Expiry Date" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "CRI", Name = "Credit Card Issuance Name (Name Shown on the Credit Card)" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "GEN", Name = "General Information" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "GBL", Name = "Government Bill of Lading" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "MCO", Name = "Miscellaneous Charge Order" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "STL", Name = "Mode of Settlement" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "RET", Name = "Return to Origin" }, entityRepository);
            AddClosedTables.AddAccountingInformationIdentifier(new AccountingInformationIdentifierDetails() { Code = "SRN", Name = "Shipper’s Reference Number" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateManifestStatusClosedTable(ShipmentsContext shipmentContext)
        {
            ManifestStatusRepository entityRepository = new ManifestStatusRepository(shipmentContext);
            AddClosedTables.AddManifestStatus(new ManifestStatusDetails() { Code = "NSEN", Name = "Not sent" }, entityRepository);
            AddClosedTables.AddManifestStatus(new ManifestStatusDetails() { Code = "SENT", Name = "Sent" }, entityRepository);
            AddClosedTables.AddManifestStatus(new ManifestStatusDetails() { Code = "RCVD", Name = "Received" }, entityRepository);
            AddClosedTables.AddManifestStatus(new ManifestStatusDetails() { Code = "DECL", Name = "Declined" }, entityRepository);
            AddClosedTables.AddManifestStatus(new ManifestStatusDetails() { Code = "ACCP", Name = "Accepted" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateCustomsTransmissionsStatusClosedTable(ShipmentsContext shipmentContext)
        {
            CustomsTransmissionsStatusRepository entityRepository = new CustomsTransmissionsStatusRepository(shipmentContext);
            AddClosedTables.AddCustomsTransmissionsStatus(new CustomsTransmissionsStatusDetails() { Code = "NSEN", Name = "Not sent" }, entityRepository);
            AddClosedTables.AddCustomsTransmissionsStatus(new CustomsTransmissionsStatusDetails() { Code = "SENT", Name = "Sent" }, entityRepository);
            AddClosedTables.AddCustomsTransmissionsStatus(new CustomsTransmissionsStatusDetails() { Code = "EROR", Name = "Error" }, entityRepository);
            AddClosedTables.AddCustomsTransmissionsStatus(new CustomsTransmissionsStatusDetails() { Code = "ACPT", Name = "Accepted" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateOtherParticipantIdClosedTable(ShipmentsContext shipmentContext)
        {
            OtherParticipantIdRepository entityRepository = new OtherParticipantIdRepository(shipmentContext);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "AIR", Name = "Airline" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "APT", Name = "Airport Authority" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "AGT", Name = "Agent" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "BRK", Name = "Broker" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "CAG", Name = "Commissionable Agent" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "CNE", Name = "Consignee" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "CTM", Name = "Customs" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "DEC", Name = "Deconsolidator" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "FFW", Name = "Freight Forwarder" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "GHA", Name = "Ground Handling Agent" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "PTT", Name = "Post Office" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "SHP", Name = "Shipper" }, entityRepository);
            AddClosedTables.AddOtherParticipantId(new CodeNameDetails() { Code = "TRK", Name = "Trucker" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateARInvoiceLineActionsClosedTable(InvoiceContext myContext)
        {
            ARInvoiceLineActionRepository entityRepository = new ARInvoiceLineActionRepository(myContext);
            AddClosedTables.AddARInvoiceLineAction(new CodeNameDetails() { Code = "1", Name = "Revenue", LocalName = "הכנסה" }, entityRepository);
            AddClosedTables.AddARInvoiceLineAction(new CodeNameDetails() { Code = "2", Name = "Expenise", LocalName = "הוצאה" }, entityRepository);
            AddClosedTables.AddARInvoiceLineAction(new CodeNameDetails() { Code = "3", Name = "Deposit", LocalName = "פקדון" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateLoginPolicyClosedTable(CommonDataContext commonContext)
        {
            LoginPolicyRepository entityRepository = new LoginPolicyRepository(commonContext);

            AddClosedTables.AddLoginPolicy(new LoginPolicyDetails() { Code = "NOREST", Name = "No Restriction" }, entityRepository);
            AddClosedTables.AddLoginPolicy(new LoginPolicyDetails() { Code = "COMPIP", Name = "Company IPs only" }, entityRepository);
            AddClosedTables.AddLoginPolicy(new LoginPolicyDetails() { Code = "TFAUTH", Name = "Two Factor Authentication" }, entityRepository);

            entityRepository.SubmitChanges();
        }
        private void UpdateMetodoPagoClosedTable(CommonDataContext commonContext)
        {
            MetodoPagoRepository entityRepository = new MetodoPagoRepository(commonContext);

            AddClosedTables.AddMetodoPago(new MetodoPagoDetails() { Code = "PUE", Name = "Pago en una sola exhibición" }, entityRepository);
            AddClosedTables.AddMetodoPago(new MetodoPagoDetails() { Code = "PPD", Name = "Pago en parcialidades o diferido" }, entityRepository);


            entityRepository.SubmitChanges();
        }
        private void UpdateUsoCFDIClosedTable(CommonDataContext commonContext)
        {
            UsoCFDIRepository entityRepository = new UsoCFDIRepository(commonContext);

            AddClosedTables.AddUsoCFDI(new UsoCFDIDetails() { Code = "G01", Name = "Adquisición de mercancias" }, entityRepository);
            AddClosedTables.AddUsoCFDI(new UsoCFDIDetails() { Code = "G02", Name = "Devoluciones, descuentos o bonificaciones" }, entityRepository);
            AddClosedTables.AddUsoCFDI(new UsoCFDIDetails() { Code = "G03", Name = "Gastos en general" }, entityRepository);
            AddClosedTables.AddUsoCFDI(new UsoCFDIDetails() { Code = "P01", Name = "Por definir" }, entityRepository);


            entityRepository.SubmitChanges();
        }
        private void UpdateSATTransferStatusClosedTable(InvoiceContext invoiceContext)
        {
            SATTransferStatusRepository entityRepository = new SATTransferStatusRepository(invoiceContext);
            AddClosedTables.AddSATTransferStatus(new SATTransferStatusDetails() { Code = "NT", Name = "Not Transferred" }, entityRepository);
            AddClosedTables.AddSATTransferStatus(new SATTransferStatusDetails() { Code = "TG", Name = "Transferring" }, entityRepository);
            AddClosedTables.AddSATTransferStatus(new SATTransferStatusDetails() { Code = "TD", Name = "Transferred" }, entityRepository);
            AddClosedTables.AddSATTransferStatus(new SATTransferStatusDetails() { Code = "TE", Name = "Transferred with Errors" }, entityRepository);
            AddClosedTables.AddSATTransferStatus(new SATTransferStatusDetails() { Code = "ND", Name = "No Transfer Needed" }, entityRepository);
            AddClosedTables.AddSATTransferStatus(new SATTransferStatusDetails() { Code = "CS", Name = "Cancellation Request Sent" }, entityRepository);


            entityRepository.SubmitChanges();
        }
        private void UpdateSATInvoiceStatusClosedTable(InvoiceContext invoiceContext)
        {
            SATInvoiceStatusRepository entityRepository = new SATInvoiceStatusRepository(invoiceContext);
            AddClosedTables.AddSATInvoiceStatus(new SATInvoiceStatusDetails() { Code = "NO", Name = "Not Opened in SAT" }, entityRepository);
            AddClosedTables.AddSATInvoiceStatus(new SATInvoiceStatusDetails() { Code = "OP", Name = "Opened in SAT" }, entityRepository);
            AddClosedTables.AddSATInvoiceStatus(new SATInvoiceStatusDetails() { Code = "PP", Name = "Partially Paid in SAT" }, entityRepository);
            AddClosedTables.AddSATInvoiceStatus(new SATInvoiceStatusDetails() { Code = "PD", Name = "Paid in SAT" }, entityRepository);

            entityRepository.SubmitChanges();
        }
        public void LoadMeasurements()
        {
            MeasurementRepository = new MeasurementRepository(0);
            Dictionary<string, Measurement> TenantMeasurements = MeasurementRepository.GetMeasurementsByTenant(0).ToDictionary(d => d.Code, a => a);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "GRWT", Name = "Gross Weight", ShortName = "Gr Weight" }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "CHWT", Name = "Chargeable Weight / WM", ShortName = "Ch Weight" }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "VOLU", Name = "Volume", ShortName = "Volume" }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "FIXD", Name = "Fixed", ShortName = "Fixed" }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "BCNT", Name = "By Container Type", ShortName = "By Container Type", IsContainerMeasurement = true }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "BTEU", Name = "By TEU", ShortName = "By TEU", IsContainerMeasurement = true }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "PRFR", Name = "Percent of Freight", ShortName = "Percent of Freight", IsContainerMeasurement = false }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "PRVL", Name = "Percent of Value", ShortName = "Percent of Value", IsContainerMeasurement = false }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "GWTN", Name = "Gross Weight in Ton", ShortName = "Gr Weight in Ton" }, MeasurementRepository, TenantMeasurements);
            AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "QTY", Name = "Quantity", ShortName = "Quantity" }, MeasurementRepository, TenantMeasurements);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddMeasurements.AddMeasurement(new MeasurementDetails() { Code = "TST", Name = "TEST", ShortName = "TEST" }, MeasurementRepository, TenantMeasurements);
            }
            //============================================

            MeasurementRepository.SubmitChanges();
        }
        public void LoadCreditCardTypes()
        {
            CreditCardTypeRepository = new CreditCardTypeRepository(0);
            Dictionary<string, CreditCardType> TenantCreditCardTypes = CreditCardTypeRepository.GetCreditCardTypes(0).ToDictionary(d => d.Code, a => a);

            AddCreditCardTypes.AddCreditCardType(new CreditCardTypeDetails() { Code = "VI", Name = "Visa" }, CreditCardTypeRepository, TenantCreditCardTypes);
            AddCreditCardTypes.AddCreditCardType(new CreditCardTypeDetails() { Code = "AX", Name = "AMEX" }, CreditCardTypeRepository, TenantCreditCardTypes);
            AddCreditCardTypes.AddCreditCardType(new CreditCardTypeDetails() { Code = "IN", Name = "Interac " }, CreditCardTypeRepository, TenantCreditCardTypes);
            AddCreditCardTypes.AddCreditCardType(new CreditCardTypeDetails() { Code = "MC", Name = "MC" }, CreditCardTypeRepository, TenantCreditCardTypes);

            CreditCardTypeRepository.SubmitChanges();
        }



        public void LoadMoveTypes()
        {
            MoveTypeRepository = new MoveTypeRepository(0);
            Dictionary<string, MoveType> TenantMoveTypes = MoveTypeRepository.GetMoveTypesByTenant(0).ToDictionary(d => d.Code, a => a);

            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "ATA", MoveTypeEnglishName = "Airport to Airport", MoveTypeLocalName = "Airport to Airport", TransportModeId = "A" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "ATD", MoveTypeEnglishName = "Airport to Door", MoveTypeLocalName = "Airport to Door", TransportModeId = "A" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "DTA", MoveTypeEnglishName = "Door to Airport", MoveTypeLocalName = "Door to Airport", TransportModeId = "A" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "DTD", MoveTypeEnglishName = "Door to Door", MoveTypeLocalName = "Door to Door", TransportModeId = "A" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "HTH", MoveTypeEnglishName = "House (Door) to House", MoveTypeLocalName = "House (Door) to House", TransportModeId = "O" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "HTP", MoveTypeEnglishName = "House (Door) to Port", MoveTypeLocalName = "House (Door) to Port", TransportModeId = "O" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "PTH", MoveTypeEnglishName = "Port to House (Door)", MoveTypeLocalName = "Port to House (Door)", TransportModeId = "O" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "PTP", MoveTypeEnglishName = "Port to Port", MoveTypeLocalName = "Port to Port", TransportModeId = "O" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "TTT", MoveTypeEnglishName = "Terminal to Terminal", MoveTypeLocalName = "Terminal to Terminal", TransportModeId = "I" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "TTW", MoveTypeEnglishName = "Terminal to Warehouse (Door)", MoveTypeLocalName = "Terminal to Warehouse (Door)", TransportModeId = "I" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "WTT", MoveTypeEnglishName = "Warehouse (Door) to Terminal", MoveTypeLocalName = "Warehouse (Door) to Terminal", TransportModeId = "I" }, MoveTypeRepository, TenantMoveTypes);
            AddMoveTypes.AddMoveType(new MoveTypeDetails() { Code = "WTW", MoveTypeEnglishName = "Warehouse (Door) to Warehouse", MoveTypeLocalName = "Warehouse (Door) to Warehouse", TransportModeId = "I" }, MoveTypeRepository, TenantMoveTypes);

            MoveTypeRepository.SubmitChanges();
        }
        public void LoadRanks()
        {
            ICommonDataContext ObjectContext = CommonDataContext.GetContext(0);
            RankRepository = new RankRepository(ObjectContext);
            Dictionary<string, Rank> tenantRanks = RankRepository.GetRanks(0).ToDictionary(d => d.Code, a => a);

            AddRanks.AddRank(new RankDetails() { Name = "Silver", Tenant = 0, Code = "1", SearchFields = "Silver,1" }, RankRepository, tenantRanks);
            AddRanks.AddRank(new RankDetails() { Name = "Gold", Tenant = 0, Code = "2", SearchFields = "Gold,2" }, RankRepository, tenantRanks);
            AddRanks.AddRank(new RankDetails() { Name = "Platinum", Tenant = 0, Code = "3", SearchFields = "Platinum,3" }, RankRepository, tenantRanks);

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddRanks.AddRank(new RankDetails() { Name = "test", Tenant = 0, Code = "TT" }, RankRepository, tenantRanks);
                //Update
                AddRanks.AddRank(new RankDetails() { Name = "Updated_Platenium", Tenant = 0, Code = "PL" }, RankRepository, tenantRanks);
            }
            //============================================

            RankRepository.SubmitChanges();
        }
        public void LoadTranslationHeaders()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            TranslationHeaderRepository = new TranslationHeaderRepository(ObjectContext);
            Dictionary<string, TranslationHeader> tenantTranslationHeaders = TranslationHeaderRepository.GetTranslationHeadersByTenant(0).ToDictionary(d => d.Code, a => a);
            //AddTranslationHeaders.AddTranslationHeader(new TranslationHeaderDetails() { Description = "Arabic", Tenant = 0 }, TranslationHeaderRepository, tenantTranslationHeaders);
            AddTranslationHeaders.AddTranslationHeader(new TranslationHeaderDetails() { Code = "EN", Description = "English", Tenant = 0 }, TranslationHeaderRepository, tenantTranslationHeaders);
            //AddTranslationHeaders.AddTranslationHeader(new TranslationHeaderDetails() { Description = "Spanish", Tenant = 0 }, TranslationHeaderRepository, tenantTranslationHeaders);
            //AddTranslationHeaders.AddTranslationHeader(new TranslationHeaderDetails() { Description = "Italiano", Tenant = 0 }, TranslationHeaderRepository, tenantTranslationHeaders);



            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                AddTranslationHeaders.AddTranslationHeader(new TranslationHeaderDetails() { Description = "Test", Tenant = 0 }, TranslationHeaderRepository, tenantTranslationHeaders);
            }
            //============================================

            TranslationHeaderRepository.SubmitChanges();
        }
        private void UpdateOBLTypes(ShipmentsContext myContext)
        {
            OBLTypeRepository entityRepository = new OBLTypeRepository(myContext);
            AddClosedTables.AddOBLType(new CodeNameDetails() { Code = "OBLR", Name = "OBL Required" }, entityRepository);
            AddClosedTables.AddOBLType(new CodeNameDetails() { Code = "EXPR", Name = "Express Release" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateRegistryDateTypes(CommonDataContext myContext)
        {
            RegistryDateTypeRepository entityRepository = new RegistryDateTypeRepository(myContext);
            AddClosedTables.AddRegistryDateType(new CodeNameDetails() { Code = "None", Name = "None" }, entityRepository);
            AddClosedTables.AddRegistryDateType(new CodeNameDetails() { Code = "FARP", Name = "First AR Inoice Approval Date" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateShipmentCustomsMessageTypes(ShipmentsContext myContext)
        {
            ShipmentCustomsMessageTypeRepository entityRepository = new ShipmentCustomsMessageTypeRepository(myContext);
            AddClosedTables.AddShipmentCustomsMessageType(new CodeNameDetails() { Code = "ASVO", Name = "Artemus Voyage" }, entityRepository);
            AddClosedTables.AddShipmentCustomsMessageType(new CodeNameDetails() { Code = "ARBL", Name = "Artemus Bill of Lading" }, entityRepository);
            AddClosedTables.AddShipmentCustomsMessageType(new CodeNameDetails() { Code = "CBAS", Name = "CBP AES" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdateTemperatureUnits(CommonDataContext myContext)
        {
            TemperatureUnitRepository entityRepository = new TemperatureUnitRepository(myContext);
            AddClosedTables.AddTemperatureUnit(new CodeNameDetails() { Code = "CEL", Name = "Celsius" }, entityRepository);
            AddClosedTables.AddTemperatureUnit(new CodeNameDetails() { Code = "FAH", Name = "Fahrenheit" }, entityRepository);
            entityRepository.SubmitChanges();
        }
        private void UpdatePickUpDeliveryTransportModes(ShipmentsContext myContext)
        {
            PickUpDeliveryTransportModeRepository myRepository = new PickUpDeliveryTransportModeRepository(myContext);
            AddClosedTables.AddPickUpDeliveryTransportMode(new CodeNameDetails() { Code = "BYTR", Name = "By Truck" }, myRepository);
            AddClosedTables.AddPickUpDeliveryTransportMode(new CodeNameDetails() { Code = "BYRA", Name = "By Rail" }, myRepository);
            myRepository.SubmitChanges();
        }
    }
}


