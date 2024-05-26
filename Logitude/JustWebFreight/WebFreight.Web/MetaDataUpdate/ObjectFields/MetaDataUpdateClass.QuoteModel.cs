using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        //private void CreateQuoteModelObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    CreateQuoteFields(objectFields, textCodes);
        //    CreateQuoteTypeFields(objectFields, textCodes);
        //    CreateQuoteChargeFields(objectFields, textCodes);
        //    CreateQuoteCostChargeFields(objectFields, textCodes);
        //    CreateQuoteSaleChargeFields(objectFields, textCodes);
        //    CreateQuotePriceStepsFields(objectFields, textCodes);
        //    CreateQuoteCustomerTypeFields(objectFields, textCodes);
        //    CreateQuoteSalesTotalFields(objectFields, textCodes);            
        //    CreateQuotePackageFields(objectFields, textCodes);
        //    CreateQuoteTemplateFields(objectFields, textCodes);
        //    CreateQuoteTemplateSectionFields(objectFields, textCodes);
        //    CreateQuoteTemplateSectionTypeFields(objectFields, textCodes);
        //    CreateQuoteTemplateSettingFields(objectFields, textCodes);
        //    CreateQuoteClosingReasonFields(objectFields, textCodes);            
        //    CreateQuoteStageFields(objectFields, textCodes);
        //    CreateQuoteRatingFields(objectFields, textCodes);
        //    CreateMarkUpTypeFields(objectFields, textCodes);
        //    CreateFixedAmountTypeFields(objectFields, textCodes);
        //    CreateQuoteTotalVatsFields(objectFields, textCodes);
        //    CreateQuoteVATsTotalFields(objectFields, textCodes);
        //    CreateQuoteSettingFields(objectFields, textCodes);
        //}

        //private void CreateQuoteFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Follow Ups",
        //        FullFieldLable = "NumberOfFollowUps",
        //        FieldName = "NumberOfFollowUps",
        //        FieldsDataType = "Integer",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "NumberOfFollowUpsLable",
        //        ListLableDefaultText = "Number Of Follow Ups",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "NumberOfFollowUps",
        //        PMPropertyPath = "NumberOfFollowUps",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notify",
        //        FullFieldLable = "NotifyId",
        //        FieldName = "NotifyId",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        LookUpTableId = CardsObject.Id,
        //        ListPropertyPath = "NotifyId",
        //        PMPropertyPath = "NotifyId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notify Address",
        //        FullFieldLable = "NotifyAddressId",
        //        FieldName = "NotifyAddressId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = AddressObject.Id,
        //        ListPropertyPath = "NotifyAddressId",
        //        PMPropertyPath = "NotifyAddressId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notify Contact",
        //        FullFieldLable = "NotifyContactId",
        //        FieldName = "NotifyContactId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = ContactsObject.Id,
        //        ListPropertyPath = "NotifyContactId",
        //        PMPropertyPath = "NotifyContactId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notify",
        //        FullFieldLable = "NotifyName",
        //        FieldName = "NotifyName",
        //        FieldsDataType = "Text",
        //        MaxLength = 70,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 70,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "NotifyNameListLable",
        //        ListLableDefaultText = "Notify",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "NotifyName",
        //        PMPropertyPath = "NotifyName",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Charges By VAT",
        //        FullFieldLable = "IsChargesByVAT",
        //        FieldName = "IsChargesByVAT",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsChargesByVAT",
        //        PMPropertyPath = "IsChargesByVAT",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Total VAT per quote",
        //        FullFieldLable = "TotalVATPerQuote",
        //        FieldName = "TotalVATPerQuote",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "TotalVATPerQuote",
        //        IsMulti = true,
        //        MultiTableId = QuoteVATsTotalObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Value of Goods",
        //        FullFieldLable = "ValueOfGoods",
        //        FieldName = "ValueOfGoods",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ValueOfGoodsListLable",
        //        ListLableDefaultText = "Value of Goods",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ValueOfGoods",
        //        PMPropertyPath = "ValueOfGoods",
        //        DigitsAfterPoint = 2,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Value of Goods Currency",
        //        FullFieldLable = "ValueOfGoodsCurrencyId",
        //        ShortFieldLable = "ValueOfGoodsCurrencyId",
        //        ShortFieldLableDefaultText = "Value of Goods Currency",
        //        FieldName = "ValueOfGoodsCurrencyId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ValueOfGoodsCurrencyId",
        //        PMPropertyPath = "ValueOfGoodsCurrencyId",
        //        LookUpTableId = CurrenciesObject.Id
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sales Total Amounts",
        //        FullFieldLable = "SalesTotalAmounts",
        //        FieldName = "SalesTotalAmounts",
        //        FieldsDataType = "Text",
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "SalesTotalAmounts",
        //        PMPropertyPath = "SalesTotalAmounts",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Sales Totals",
        //        FullFieldLable = "QuoteSalesTotals",
        //        FieldName = "QuoteSalesTotals",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "QuoteSalesTotals",
        //        IsMulti = true,
        //        MultiTableId = QuoteSalesTotalObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "TEU",
        //        FullFieldLable = "TEU",
        //        FieldName = "TEU",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "TEUListLable",
        //        ListLableDefaultText = "TEU",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "TEU",
        //        PMPropertyPath = "TEU",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Move Type",
        //        FullFieldLable = "MoveTypeId",
        //        FieldName = "MoveTypeId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = MoveTypeObject.Id,
        //        PMPropertyPath = "MoveTypeId",
        //        ListPropertyPath = "MoveTypeId",
        //        DependencyFilter1Type = "Path",
        //        DependencyFilter1Value = "TransportModeId",
        //        CanFilter = true,
        //        Operator = "Equals",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);



        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Move Type",
        //        FullFieldLable = "MoveTypeName",
        //        FieldName = "MoveTypeName",
        //        CanFilter = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        LookUpTableId = MoveTypeObject.Id,
        //        ListPropertyPath = "MoveTypeName",
        //        PMPropertyPath = "MoveTypeName",
        //        DisplayInList = true,
        //        ListFieldLable = "MoveTypeName",
        //        ListLableDefaultText = "Move Type",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Agent",
        //        FullFieldLable = "AgentId",
        //        FieldName = "AgentId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = CardsObject.Id,
        //        ListPropertyPath = "AgentId",
        //        PMPropertyPath = "AgentId",
        //        DependencyFilter1Type = "Constant",
        //        DependencyFilter1Value = "AG"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Agent",
        //        FullFieldLable = "AgentName",
        //        FieldName = "AgentName",
        //        FieldsDataType = "Text",
        //        MaxLength = 70,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = false,
        //        DisplayInList = true,
        //        ListFieldLable = "AgentNameListLable",
        //        ListLableDefaultText = "Agent",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = CardsObject.Id,
        //        ListPropertyPath = "AgentName",
        //        PMPropertyPath = "AgentName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Agent Address",
        //        FullFieldLable = "AgentAddressId",
        //        FieldName = "AgentAddressId",
        //        ShortFieldLable = "AgentAddressId",
        //        ShortFieldLableDefaultText = "Address",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = AddressObject.Id,
        //        ListPropertyPath = "AgentAddressId",
        //        PMPropertyPath = "AgentAddressId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Agent Contact",
        //        FullFieldLable = "AgentContactId",
        //        FieldName = "AgentContactId",
        //        ShortFieldLable = "AgentContactId",
        //        ShortFieldLableDefaultText = "Contact",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = ContactsObject.Id,
        //        ListPropertyPath = "AgentContactId",
        //        PMPropertyPath = "AgentContactId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "ETD Label",
        //        FullFieldLable = "ETDLabel",
        //        FieldName = "ETDLabel",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "ETDLabel",
        //        PMPropertyPath = "ETDLabel",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "ETA Label",
        //        FullFieldLable = "ETALabel",
        //        FieldName = "ETALabel",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "ETALabel",
        //        PMPropertyPath = "ETALabel",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Transit Time",
        //        FullFieldLable = "TransitTime",
        //        FieldName = "TransitTime",
        //        FieldsDataType = "nText",
        //        MaxLength = 30,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayInList = true,
        //        ListFieldLable = "TransitTimeLable",
        //        ListLableDefaultText = "Transit Time",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "TransitTime",
        //        PMPropertyPath = "TransitTime",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Departure Frequency",
        //        FullFieldLable = "DepartureFrequency",
        //        FieldName = "DepartureFrequency",
        //        FieldsDataType = "nText",
        //        MaxLength = 30,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayInList = true,
        //        ListFieldLable = "DepartureFrequencyLable",
        //        ListLableDefaultText = "Departure Frequency",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "DepartureFrequency",
        //        PMPropertyPath = "DepartureFrequency",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "ETD",
        //        FullFieldLable = "ETD",
        //        FieldName = "ETD",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = false,
        //        DisplayInList = true,
        //        ListFieldLable = "ETDListLable",
        //        ListLableDefaultText = "ETD",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ETD",
        //        PMPropertyPath = "ETD",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "ETA",
        //        FullFieldLable = "ETA",
        //        FieldName = "ETA",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = false,
        //        DisplayInList = true,
        //        ListFieldLable = "ETAListLable",
        //        ListLableDefaultText = "ETA",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ETA",
        //        PMPropertyPath = "ETA",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Product Code",
        //        FullFieldLable = "ProductCode",
        //        FieldName = "ProductCode",
        //        FieldsDataType = "Text",
        //        MaxLength = 2,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ProductCodeListLable",
        //        ListLableDefaultText = "Product Code",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ProductCode",
        //        PMPropertyPath = "ProductCode",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Updated By",
        //        FullFieldLable = "UpdatedByUserId",
        //        FieldName = "UpdatedByUserId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        IsRequired = true,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = UsersObject.Id,
        //        ListPropertyPath = "UpdatedByUserId",
        //        PMPropertyPath = "UpdatedByUserId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Update Date",
        //        FullFieldLable = "UpdateDate",
        //        FieldName = "UpdateDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        Operator = "Equals",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "UpdateDateListLable",
        //        ListLableDefaultText = "Update Date",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //        ListPropertyPath = "UpdateDate",
        //        PMPropertyPath = "UpdateDate",
        //        IsTimeFrameFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Close automatically as declined after",
        //        FullFieldLable = "IsAutomaticallyClosed",
        //        FieldName = "IsAutomaticallyClosed",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsAutomaticallyClosedLable",
        //        ListLableDefaultText = "Is Automatically Closed",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsAutomaticallyClosed",
        //        PMPropertyPath = "IsAutomaticallyClosed",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Close Date",
        //        FullFieldLable = "AutomaticallyCloseDate",
        //        FieldName = "AutomaticallyCloseDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "AutomaticallyCloseDateLable",
        //        ListLableDefaultText = "Automatically Close Date",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "AutomaticallyCloseDate",
        //        PMPropertyPath = "AutomaticallyCloseDate",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Close Days",
        //        FullFieldLable = "AutomaticallyCloseDays",
        //        FieldName = "AutomaticallyCloseDays",
        //        FieldsDataType = "Integer",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "AutomaticallyCloseDaysLable",
        //        ListLableDefaultText = "Automatically Close Days",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "AutomaticallyCloseDays",
        //        PMPropertyPath = "AutomaticallyCloseDays",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Stage",
        //        FullFieldLable = "StageId",
        //        FieldName = "StageId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = QuoteStageObject.Id,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Tenant = 0,
        //        CanFilter = true,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "StageId",
        //        PMPropertyPath = "StageId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Stage",
        //        FullFieldLable = "StageName",
        //        FieldName = "StageName",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayInList = true,
        //        ListFieldLable = "StageNameLable",
        //        ListLableDefaultText = "Stage",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "StageName",
        //        PMPropertyPath = "StageName",
        //        DataTemplateName = "QuoteStageDataTemplate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Stage due date",
        //        FullFieldLable = "StageDueDate",
        //        FieldName = "StageDueDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "StageDueDateLable",
        //        ListLableDefaultText = "Stage due date",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "StageDueDate",
        //        PMPropertyPath = "StageDueDate",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Stage max days",
        //        FullFieldLable = "StageMaxDays",
        //        FieldName = "StageMaxDays",
        //        FieldsDataType = "Integer",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "StageMaxDays",
        //        PMPropertyPath = "StageMaxDays",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Rating",
        //        FullFieldLable = "RatingCode",
        //        FieldName = "RatingCode",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = QuoteRatingObject.Id,
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Tenant = 0,
        //        CanFilter = true,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "RatingCode",
        //        PMPropertyPath = "RatingCode",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Rating",
        //        FullFieldLable = "RatingName",
        //        FieldName = "RatingName",
        //        FieldsDataType = "Text",
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Tenant = 0,
        //        DisplayInList = true,
        //        ListFieldLable = "RatingName",
        //        ListLableDefaultText = "Rating",
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "RatingName",
        //        PMPropertyPath = "RatingName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Rating index order",
        //        FullFieldLable = "RatingIndexOrder",
        //        FieldName = "RatingIndexOrder",
        //        FieldsDataType = "Integer",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "RatingIndexOrder",
        //        PMPropertyPath = "RatingIndexOrder",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Last Activity Date",
        //        FullFieldLable = "LastActivityDate",
        //        FieldName = "LastActivityDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ListFieldLable = "LastActivityDateLable",
        //        ListLableDefaultText = "Last Activity Date",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "LastActivityDate",
        //        PMPropertyPath = "LastActivityDate",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Last Activity Subject",
        //        FullFieldLable = "LastActivitySubject",
        //        FieldName = "LastActivitySubject",
        //        FieldsDataType = "nText",
        //        MaxLength = 255,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        CanFilter = true,
        //        DisplayInList = false,
        //        ListFieldLable = "LastActivitySubjectLable",
        //        ListLableDefaultText = "Last Activity Subject",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Contains",
        //        ListPropertyPath = "LastActivitySubject",
        //        PMPropertyPath = "LastActivitySubject",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Last Activity",
        //        FullFieldLable = "LastActivityTypeCode",
        //        FieldName = "LastActivityTypeCode",
        //        FieldsDataType = "Text",
        //        MaxLength = 2,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ListFieldLable = "LastActivityTypeCodeLable",
        //        ListLableDefaultText = "Last Activity",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "LastActivityTypeCode",
        //        PMPropertyPath = "LastActivityTypeCode",
        //        DataTemplateName = "QuoteLastActivityTypeDataTemplate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Next Activity Date",
        //        FullFieldLable = "NextActivityDate",
        //        FieldName = "NextActivityDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ListFieldLable = "NextActivityDateLable",
        //        ListLableDefaultText = "Next Activity Date",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "NextActivityDate",
        //        PMPropertyPath = "NextActivityDate",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //        DataTemplateName = "QuoteNextActivityDateTemplate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Next Activity Subject",
        //        FullFieldLable = "NextActivitySubject",
        //        FieldName = "NextActivitySubject",
        //        FieldsDataType = "nText",
        //        MaxLength = 255,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        CanFilter = true,
        //        DisplayInList = false,
        //        ListFieldLable = "NextActivitySubjectLable",
        //        ListLableDefaultText = "Next Activity Subject",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Contains",
        //        ListPropertyPath = "NextActivitySubject",
        //        PMPropertyPath = "NextActivitySubject",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Next Activity",
        //        FullFieldLable = "NextActivityTypeCode",
        //        FieldName = "NextActivityTypeCode",
        //        FieldsDataType = "Text",
        //        MaxLength = 2,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ListFieldLable = "NextActivityTypeCodeLable",
        //        ListLableDefaultText = "Next Activity",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "NextActivityTypeCode",
        //        PMPropertyPath = "NextActivityTypeCode",
        //        DataTemplateName = "QuoteNextActivityTypeDataTemplate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Routing",
        //        FullFieldLable = "Routing",
        //        FieldName = "Routing",
        //        FieldsDataType = "Text",
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Routing",
        //        PMPropertyPath = "Routing",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DataTemplateName = "QuoteRoutingDataTemplate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Salesman",
        //        FullFieldLable = "SalesmanName",
        //        FieldName = "SalesmanName",
        //        FieldsDataType = "Text",
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        DisplayInList = true,
        //        ListFieldLable = "SalesmanNameLable",
        //        ListLableDefaultText = "Salesman",
        //        ListPropertyPath = "SalesmanName",
        //        PMPropertyPath = "SalesmanName",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Incoterm",
        //        FullFieldLable = "IncotermCode",
        //        FieldName = "IncotermCode",
        //        FieldsDataType = "Text",
        //        MaxLength = 3,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        DisplayInList = true,
        //        ListFieldLable = "IncotermCodeLable",
        //        ListLableDefaultText = "Incoterm",
        //        ListPropertyPath = "IncotermCode",
        //        PMPropertyPath = "IncotermCode",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Subject",
        //        FullFieldLable = "Subject",
        //        FieldName = "Subject",
        //        FieldsDataType = "nText",
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        DisplayInList = true,
        //        ListFieldLable = "SubjectLable",
        //        ListLableDefaultText = "Subject",
        //        ListPropertyPath = "Subject",
        //        PMPropertyPath = "Subject",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Subject Edited",
        //        FullFieldLable = "IsSubjectEdited",
        //        FieldName = "IsSubjectEdited",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsSubjectEdited",
        //        PMPropertyPath = "IsSubjectEdited",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Business Unit",
        //        FullFieldLable = "BusinessUnitId",
        //        FieldName = "BusinessUnitId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = BusinessUnitObject.Id,
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "BusinessUnitId",
        //        PMPropertyPath = "BusinessUnitId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Business Unit",
        //        FullFieldLable = "BusinessUnitName",
        //        FieldName = "BusinessUnitName",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "BusinessUnitNameListLable",
        //        ListLableDefaultText = "Business Unit",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "BusinessUnitName",
        //        PMPropertyPath = "BusinessUnitName",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Usage Count",
        //        FullFieldLable = "UsageCount",
        //        FieldName = "UsageCount",
        //        CanFilter = true,
        //        FieldsDataType = "Integer",
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "UsageCountListLable",
        //        Operator = "Equals",
        //        ListLableDefaultText = "Usage Count",
        //        ListPropertyPath = "UsageCount",
        //        PMPropertyPath = "UsageCount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Last Usage Date",
        //        FullFieldLable = "LastUsageDate",
        //        FieldName = "LastUsageDate",
        //        CanFilter = true,
        //        FieldsDataType = "DateTime",
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "LastUsageDateListLable",
        //        Operator = "Equals",
        //        ListLableDefaultText = "Last Usage Date",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //        ListPropertyPath = "LastUsageDate",
        //        PMPropertyPath = "LastUsageDate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Closing Reason",
        //        FullFieldLable = "QuoteClosingReasonCode",
        //        FieldName = "QuoteClosingReasonCode",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 2,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        LookUpTableId = QuoteClosingReasonObject.Id,
        //        PMPropertyPath = "QuoteClosingReasonCode",
        //        ListPropertyPath = "QuoteClosingReasonCode",
        //        Operator = "Equals",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Closing Reason",
        //        FullFieldLable = "QuoteClosingReasonName",
        //        FieldName = "QuoteClosingReasonName",
        //        FieldsDataType = "Text",
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "ClosingReasonNameListLable",
        //        ListLableDefaultText = "Closing Reason",
        //        ValidForQuerySection1 = "Quote",
        //        PMPropertyPath = "QuoteClosingReasonName",
        //        ListPropertyPath = "QuoteClosingReasonName",
        //        Operator = "StartsWith",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    //MyQuotes
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        FieldName = "MyQuotes",
        //        ObjectTableName = "Quote",
        //        FieldsDataType = "Boolean",
        //        MinLength = 0,
        //        MaxLength = 0,
        //        IsRequired = false,
        //        SystemRequired = false,
        //        SystemMaxLength = 0,
        //        IsCustomFilter = true,
        //        Operator = "Equals",
        //        PMPropertyPath = "MyQuotes",
        //        ListPropertyPath = "MyQuotes",
        //        FullFieldLable = "MyQuotes",
        //        DefaultText = "My Quotes",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Accepted Without Shipments",
        //        FullFieldLable = "AcceptedWithoutShipments",
        //        FieldName = "AcceptedWithoutShipments",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "AcceptedWithoutShipmentsListLabel",
        //        ListLableDefaultText = "Accepted Without Shipments",
        //        ValidForQuerySection1 = "Quote",
        //        IsCustomFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "AcceptedWithoutShipments",
        //        PMPropertyPath = "AcceptedWithoutShipments",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Location",
        //        FullFieldLable = "FromLocation",
        //        FieldName = "FromLocation",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "FromLocation",
        //        ListPropertyPath = "FromLocation",
        //        Operator = "StartsWith",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Location",
        //        FullFieldLable = "ToLocation",
        //        FieldName = "ToLocation",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "ToLocation",
        //        ListPropertyPath = "ToLocation",
        //        Operator = "StartsWith",
        //        DisplayInEntityVariables = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Partner",
        //        FullFieldLable = "FromPartnerId",
        //        FieldName = "FromPartnerId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = CardsObject.Id,
        //        PMPropertyPath = "FromPartnerId",
        //        ListPropertyPath = "FromPartnerId",
        //        Operator = "StartsWith",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Partner",
        //        FullFieldLable = "ToPartnerId",
        //        FieldName = "ToPartnerId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = CardsObject.Id,
        //        PMPropertyPath = "ToPartnerId",
        //        ListPropertyPath = "ToPartnerId",
        //        Operator = "StartsWith",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Partner Address",
        //        FullFieldLable = "FromPartnerAddressId",
        //        FieldName = "FromPartnerAddressId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = AddressObject.Id,
        //        PMPropertyPath = "FromPartnerAddressId",
        //        ListPropertyPath = "FromPartnerAddressId",
        //        Operator = "StartsWith",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Partner Address",
        //        FullFieldLable = "ToPartnerAddressId",
        //        FieldName = "ToPartnerAddressId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = AddressObject.Id,
        //        PMPropertyPath = "ToPartnerAddressId",
        //        ListPropertyPath = "ToPartnerAddressId",
        //        Operator = "StartsWith",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Fixed Price",
        //        FullFieldLable = "IsFixedPrice",
        //        FieldName = "IsFixedPrice",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 0,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsFixedPrice",
        //        PMPropertyPath = "IsFixedPrice"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Customer Ref 1",
        //        FullFieldLable = "CustomerReference1",
        //        FieldName = "CustomerReference1",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "CustomerReference1ListLable",
        //        ListLableDefaultText = "Customer Ref 1",
        //        DisplayInList = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "CustomerReference1",
        //        PMPropertyPath = "CustomerReference1"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Customer Ref 2",
        //        FullFieldLable = "CustomerReference2",
        //        FieldName = "CustomerReference2",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "CustomerReference2ListLable",
        //        ListLableDefaultText = "Customer Ref 2",
        //        DisplayInList = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "CustomerReference2",
        //        PMPropertyPath = "CustomerReference2"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Customer Contact",
        //        FullFieldLable = "CustomerContactId",
        //        FieldName = "CustomerContactId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        LookUpTableId = ContactsObject.Id,
        //        ListPropertyPath = "CustomerContactId",
        //        PMPropertyPath = "CustomerContactId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Total Amount In Local Currency",
        //        FullFieldLable = "CostTotalAmountInLocalCurrency",
        //        FieldName = "CostTotalAmountInLocalCurrency",
        //        ShortFieldLable = "CostTotalAmount",
        //        ShortFieldLableDefaultText = "Cost",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostTotalAmountInLocalCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Total Amount In Local Currency",
        //        FullFieldLable = "SaleTotalAmountInLocalCurrency",
        //        FieldName = "SaleTotalAmountInLocalCurrency",
        //        ShortFieldLable = "SaleTotalAmount",
        //        ShortFieldLableDefaultText = "Sale",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleTotalAmountInLocalCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Total Amount In Sale Currency",
        //        FullFieldLable = "CostTotalAmountInSaleCurrency",
        //        FieldName = "CostTotalAmountInSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostTotalAmountInSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Total Amount In Sale Currency",
        //        FullFieldLable = "SaleTotalAmountInSaleCurrency",
        //        FieldName = "SaleTotalAmountInSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleTotalAmountInSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Estimate Profit",
        //        FullFieldLable = "EstimateProfit",
        //        FieldName = "EstimateProfit",
        //        FieldsDataType = "Double",
        //        MaxLength = 0,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "EstimateProfit",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Estimate Profit In Sale Currency",
        //        FullFieldLable = "EstimateProfitInSaleCurrency",
        //        FieldName = "EstimateProfitInSaleCurrency",
        //        ShortFieldLable = "ProfitInSaleCurrency",
        //        ShortFieldLableDefaultText = "Profit In Sale Currency",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "EstimateProfitInSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipper Name",
        //        FullFieldLable = "ShipperName",
        //        FieldName = "ShipperName",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "ShipperName",
        //        ListPropertyPath = "ShipperName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Consignee Name",
        //        FullFieldLable = "ConsigneeName",
        //        FieldName = "ConsigneeName",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "ConsigneeName",
        //        ListPropertyPath = "ConsigneeName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Delivery Address",
        //        FullFieldLable = "DeliveryAddress",
        //        FieldName = "DeliveryAddress",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "DeliveryAddress",
        //        ListPropertyPath = "DeliveryAddress",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "PickUp Address",
        //        FullFieldLable = "PickUpAddress",
        //        FieldName = "PickUpAddress",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "PickUpAddress",
        //        ListPropertyPath = "PickUpAddress",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Pickup Location",
        //        FullFieldLable = "PickupLocation",
        //        FieldName = "PickupLocation",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 500,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "PickupLocation",
        //        ListPropertyPath = "PickupLocation",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Delivery Location",
        //        FullFieldLable = "DeliveryLocation",
        //        FieldName = "DeliveryLocation",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 500,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "DeliveryLocation",
        //        ListPropertyPath = "DeliveryLocation",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Currency",
        //        FullFieldLable = "SaleCurrencyId",
        //        FieldName = "SaleCurrencyId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = true,
        //        CanFilter = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleCurrencyId",
        //        ListPropertyPath = "SaleCurrencyId",
        //        LookUpTableId = CurrenciesObject.Id
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Exchange Rate",
        //        FullFieldLable = "ExchangeRate",
        //        FieldName = "ExchangeRate",
        //        FieldsDataType = "Double",
        //        IsRequired = true,
        //        MaxLength = 0,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "ExchangeRate",
        //        ListPropertyPath = "ExchangeRate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Customer Name",
        //        FullFieldLable = "CustomerName",
        //        FieldName = "CustomerName",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        DisplayInList = true,
        //        ListFieldLable = "CustomerNameListLable",
        //        ListLableDefaultText = "Customer",
        //        ValidForQuerySection1 = "Quote",
        //        PMPropertyPath = "CustomerName",
        //        ListPropertyPath = "CustomerName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Customer Note",
        //        FullFieldLable = "CustomerNote",
        //        FieldName = "CustomerNote",
        //        FieldsDataType = "Text",
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CustomerNote",
        //        ListPropertyPath = "CustomerNote",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Cancelled",
        //        FullFieldLable = "IsCancelled",
        //        FieldName = "IsCancelled",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        IsCustomFilter = true,
        //        ListPropertyPath = "IsCancelled",
        //        PMPropertyPath = "IsCancelled"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Cost Charges",
        //        FullFieldLable = "QuoteCostCharges",
        //        FieldName = "QuoteCostCharges",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "QuoteCostCharges",
        //        IsMulti = true,
        //        MultiTableId = QuoteCostChargesObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Sale Charges",
        //        FullFieldLable = "QuoteSaleCharges",
        //        FieldName = "QuoteSaleCharges",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "QuoteSaleCharges",
        //        IsMulti = true,
        //        MultiTableId = QuoteSaleChargesObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Packages",
        //        FullFieldLable = "QuotePackages",
        //        FieldName = "QuotePackages",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "QuotePackages",
        //        IsMulti = true,
        //        MultiTableId = QuotePackageObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Price Steps",
        //        FullFieldLable = "QuotePriceSteps",
        //        FieldName = "QuotePriceSteps",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "QuotePriceSteps",
        //        IsMulti = true,
        //        MultiTableId = QuotePriceStepObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Follow Up Owner",
        //        FullFieldLable = "FollowUpOwner",
        //        FieldName = "FollowUpOwner",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        ObjectTablePlural = "Quotes",
        //        ObjectTableSingular = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        DisplayInList = true,
        //        ListFieldLable = "FollowUpOwnerListLable",
        //        ListLableDefaultText = "F/U Owner",
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "FollowUpOwner",
        //        PMPropertyPath = "FollowUpOwner"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Follow Up Owner",
        //        FullFieldLable = "FollowUpOwnerId",
        //        FieldName = "FollowUpOwnerId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = UsersObject.Id,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        ObjectTablePlural = "Quotes",
        //        ObjectTableSingular = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 15,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "FollowUpOwnerId",
        //        PMPropertyPath = "FollowUpOwnerId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Total Receivables Amount",
        //        FullFieldLable = "TotalReceivablesAmount",
        //        FieldName = "TotalReceivablesAmount",
        //        FieldsDataType = "Double",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "TotalReceivablesAmountListLable",
        //        ListLableDefaultText = "Total Receivables Amount",
        //        DisplayInList = false,
        //        Operator = "Equals",
        //        ListPropertyPath = "TotalReceivablesAmount",
        //        PMPropertyPath = "TotalReceivablesAmount"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote No.",
        //        FullFieldLable = "QuoteNumber",
        //        FieldName = "QuoteNumber",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "QuoteListLable",
        //        ListLableDefaultText = "Quote No.",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "QuoteNumber",
        //        PMPropertyPath = "QuoteNumber",
        //        DisplayInDocumentReferences = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Carrier",
        //        FullFieldLable = "MainCarriageCarrierId",
        //        FieldName = "MainCarriageCarrierId",
        //        ShortFieldLable = "CarrierId",
        //        ShortFieldLableDefaultText = "Carrier",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        CanFilter = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        LookUpTableId = CarriersObject.Id,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "MainCarriageCarrierIdLable",
        //        ListLableDefaultText = "Carrier",
        //        DisplayInList = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "MainCarriageCarrierId",
        //        PMPropertyPath = "MainCarriageCarrierId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Total Containers",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "TotalContainers",
        //        FieldName = "TotalContainers",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "STotalContainersListLable",
        //        ListLableDefaultText = "Total Containers",
        //        PMPropertyPath = "TotalContainers",
        //        ListPropertyPath = "TotalContainers",
        //        Operator = "StartsWith",
        //        DisplayInList = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayOnly = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Carrier",
        //        FullFieldLable = "MainCarriageCarrierName",
        //        FieldName = "MainCarriageCarrierName",
        //        FieldsDataType = "Text",
        //        MaxLength = 70,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "MainCarriageCarrierNameListLable",
        //        ListLableDefaultText = "Carrier",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "MainCarriageCarrierName",
        //        PMPropertyPath = "MainCarriageCarrierName"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Direction",
        //        FullFieldLable = "DirectionId",
        //        FieldName = "DirectionId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = true,
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        LookUpTableId = DirectionsObject.Id,
        //        DataTemplateName = "DirectionDataTemplate",
        //        ListFieldLable = "DirectionIdListLable",
        //        ListLableDefaultText = "Direction",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListPropertyPath = "DirectionId",
        //        PMPropertyPath = "DirectionId",
        //        IsRestrictable = true,
        //        ColumnHeaderTemplateName = "DirectionHeaderTemplate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Transport Mode",
        //        FullFieldLable = "TransportModeId",
        //        FieldName = "TransportModeId",
        //        FieldsDataType = "LookUp",
        //        ShortFieldLable = "TransportModeId",
        //        ShortFieldLableDefaultText = "Transport",
        //        IsRequired = true,
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        LookUpTableId = TransportModesObject.Id,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "TransportModeIdListLable",
        //        DataTemplateName = "TransportModeTemplete",
        //        ListLableDefaultText = "Transport Mode",
        //        ListPropertyPath = "TransportModeId",
        //        PMPropertyPath = "TransportModeId",
        //        IsRestrictable = true,
        //        ColumnHeaderTemplateName = "TransportModeHeaderTemplate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Department",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "DepartmentId",
        //        FieldName = "DepartmentId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = DepartmentsObject.Id,
        //        ListFieldLable = "DepartmentListLable",
        //        ListLableDefaultText = "Department",
        //        DisplayInList = false,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "DepartmentId",
        //        PMPropertyPath = "DepartmentId",
        //        IsRestrictable = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Department",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "DepartmentName",
        //        FieldName = "DepartmentName",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = null,
        //        ListFieldLable = "DepartmentNameListLable",
        //        ListLableDefaultText = "Department",
        //        DisplayInList = true,
        //        CanFilter = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "DepartmentName",
        //        PMPropertyPath = "DepartmentName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Branch",
        //        FullFieldLable = "BranchId",
        //        FieldName = "BranchId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = BranchesObject.Id,
        //        ListFieldLable = "BranchListLable",
        //        ListLableDefaultText = "Branch",
        //        DisplayInList = false,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "BranchId",
        //        PMPropertyPath = "BranchId",
        //        IsRestrictable = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Branch",
        //        FullFieldLable = "BranchName",
        //        FieldName = "BranchName",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = null,
        //        ListFieldLable = "BranchNameListLable",
        //        ListLableDefaultText = "Branch",
        //        DisplayInList = true,
        //        CanFilter = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "BranchName",
        //        PMPropertyPath = "BranchName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Type",
        //        FullFieldLable = "ShipmentTypeId",
        //        FieldName = "ShipmentTypeId",
        //        CanFilter = true,
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListFieldLable = "ShipmentTypeIdListFieldLable",
        //        ListLableDefaultText = "Type",
        //        LookUpTableId = ShipmentTypesObject.Id,
        //        ListPropertyPath = "ShipmentTypeId",
        //        PMPropertyPath = "ShipmentTypeId",
        //        DependencyFilter1Type = "Path",
        //        DependencyFilter1Value = "TransportModeId",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipment Type",
        //        FullFieldLable = "ShipmentType",
        //        FieldName = "ShipmentType",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "ShipmentTypeListFieldLable",
        //        ListLableDefaultText = "Type",
        //        LookUpTableId = ShipmentTypesObject.Id,
        //        ListPropertyPath = "ShipmentType",
        //        PMPropertyPath = "ShipmentType"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Customer Type",
        //        FullFieldLable = "QuoteCustomerTypeCode",
        //        FieldName = "QuoteCustomerTypeCode",
        //        FieldsDataType = "LookUp",
        //        IsRequired = true,
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        LookUpTableId = QuoteCustomerTypeObject.Id,
        //        PMPropertyPath = "QuoteCustomerTypeCode"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Customer",
        //        FullFieldLable = "CustomerId",
        //        FieldName = "CustomerId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "CustomerIdListLable",
        //        ListLableDefaultText = "Customer",
        //        LookUpTableId = CardsObject.Id,
        //        PMPropertyPath = "CustomerId",
        //        HelpTextCode = "CustomerId",
        //        HelpTextDefaultText = "Indicates who the customer is, so that Logitude knows to refer to the relevant partner for statistics, billing and shared logistics. For Export, the Shipper is selected automatically. For Import, the Consignee is selected automatically.",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipper",
        //        FullFieldLable = "ShipperId",
        //        FieldName = "ShipperId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "ShipperListFieldLable",
        //        ListLableDefaultText = "Shipper",
        //        LookUpTableId = CardsObject.Id,
        //        ListPropertyPath = "ShipperId",
        //        PMPropertyPath = "ShipperId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipper",
        //        FullFieldLable = "Shipper",
        //        FieldName = "Shipper",
        //        FieldsDataType = "Text",
        //        LookUpTableId = CardsObject.Id,
        //        MaxLength = 70,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "ShipperListLable",
        //        ListLableDefaultText = "Shipper",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Shipper",
        //        PMPropertyPath = "Shipper"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipper Contact",
        //        FullFieldLable = "ShipperContact",
        //        FieldName = "ShipperContactId",
        //        ShortFieldLable = "ShipperContactId",
        //        ShortFieldLableDefaultText = "Contact",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = ContactsObject.Id,
        //        ListPropertyPath = "ShipperContactId",
        //        PMPropertyPath = "ShipperContactId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipper Ref 1",
        //        FullFieldLable = "ShipperReference1",
        //        FieldName = "ShipperReference1",
        //        ShortFieldLable = "ShipperReference1",
        //        ShortFieldLableDefaultText = "Reference1",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ShipperReferenceListLable",
        //        ListLableDefaultText = "Shipper Ref.",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "ShipperReference1",
        //        PMPropertyPath = "ShipperReference1"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipper Reference 2",
        //        FullFieldLable = "ShipperReference2",
        //        FieldName = "ShipperReference2",
        //        ShortFieldLable = "ShipperReference2",
        //        ShortFieldLableDefaultText = "Shipper Ref 2",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListPropertyPath = "ShipperReference2",
        //        PMPropertyPath = "ShipperReference2"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Consignee",
        //        FullFieldLable = "ConsigneeId",
        //        FieldName = "ConsigneeId",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "ConsigneeIdListLable",
        //        ListLableDefaultText = "ConsigneeId",
        //        LookUpTableId = CardsObject.Id,
        //        ListPropertyPath = "ConsigneeId",
        //        PMPropertyPath = "ConsigneeId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Consignee",
        //        FullFieldLable = "Consignee",
        //        FieldName = "Consignee",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 70,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListFieldLable = "ConsigneeListLable",
        //        ListLableDefaultText = "Consignee",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        LookUpTableId = CardsObject.Id,
        //        ListPropertyPath = "Consignee",
        //        PMPropertyPath = "Consignee"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Consignee Contact",
        //        FullFieldLable = "ConsigneeContactId",
        //        FieldName = "ConsigneeContactId",
        //        ShortFieldLable = "ConsigneeContactId",
        //        ShortFieldLableDefaultText = "Contact",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = ContactsObject.Id,
        //        ListPropertyPath = "ConsigneeContactId",
        //        PMPropertyPath = "ConsigneeContactId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Consignee Ref 1",
        //        FullFieldLable = "ConsigneeReference1",
        //        FieldName = "ConsigneeReference1",
        //        ShortFieldLable = "ConsigneeReference1",
        //        ShortFieldLableDefaultText = "Reference1",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListPropertyPath = "ConsigneeReference1",
        //        PMPropertyPath = "ConsigneeReference1"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Consignee Reference 2",
        //        FullFieldLable = "ConsigneeReference2",
        //        FieldName = "ConsigneeReference2",
        //        ShortFieldLable = "ConsigneeReference2",
        //        ShortFieldLableDefaultText = "Consignee Ref 2",
        //        FieldsDataType = "Text",
        //        MaxLength = 50,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListPropertyPath = "ConsigneeReference2",
        //        PMPropertyPath = "ConsigneeReference2"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Port",
        //        FullFieldLable = "FromPortId",
        //        ShortFieldLable = "FromPortId",
        //        ShortFieldLableDefaultText = "From",
        //        FieldName = "FromPortId",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        FieldsDataType = "LookUp",
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListFieldLable = "FromPortIdListLable",
        //        ListLableDefaultText = "From",
        //        LookUpTableId = PortsObject.Id,
        //        ListPropertyPath = "FromPortId",
        //        PMPropertyPath = "FromPortId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    //Used in Query columns
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Port Code",
        //        FullFieldLable = "FromPortCode",
        //        ShortFieldLable = "FromPortCode",
        //        ShortFieldLableDefaultText = "From Port Code",
        //        FieldName = "FromPort",
        //        FieldsDataType = "Text",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListFieldLable = "FromPortCodeListLable",
        //        ListLableDefaultText = "From",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListPropertyPath = "FromPort",
        //        PMPropertyPath = "FromPortCode",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Port",
        //        FullFieldLable = "ToPortId",
        //        ShortFieldLable = "ToPortId",
        //        ShortFieldLableDefaultText = "To",
        //        FieldName = "ToPortId",
        //        CanFilter = true,
        //        IsRequired = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = PortsObject.Id,
        //        ListFieldLable = "ToPortIdListLable",
        //        ListLableDefaultText = "To",
        //        Operator = "Equals",
        //        ListPropertyPath = "ToPortId",
        //        PMPropertyPath = "ToPortId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    //Used in Query columns
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Port Code",
        //        FullFieldLable = "ToPortCode",
        //        ShortFieldLable = "ToPortCode",
        //        ShortFieldLableDefaultText = "To Port Code",
        //        FieldName = "ToPort",
        //        FieldsDataType = "Text",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ToPortCodeListLable",
        //        ListLableDefaultText = "To",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ToPort",
        //        PMPropertyPath = "ToPort"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Incoterm",
        //        FullFieldLable = "IncotermId",
        //        FieldName = "IncotermId",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        LookUpTableId = IncotermsObject.Id,
        //        ListPropertyPath = "IncotermId",
        //        PMPropertyPath = "IncotermId",
        //        HelpTextCode = "IncotermId",
        //        HelpTextDefaultText = "Incoterm",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Salesman",
        //        FullFieldLable = "SalesmanUserId",
        //        FieldName = "SalesmanUserId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = UsersObject.Id,
        //        ListPropertyPath = "SalesmanUserId",
        //        PMPropertyPath = "SalesmanUserId",
        //        DependencyFilter3Type = "Constant",
        //        DependencyFilter3Value = "True",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Created By",
        //        FullFieldLable = "CreatedByUserId",
        //        FieldName = "CreatedByUserId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        IsRequired = true,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        LookUpTableId = UsersObject.Id,
        //        ListPropertyPath = "CreatedByUserId",
        //        PMPropertyPath = "CreatedByUserId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Created By",
        //        FullFieldLable = "CreatedByUser",
        //        FieldName = "CreatedByUser",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListFieldLable = "CreatedByUserListLable",
        //        ListLableDefaultText = "Created by",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        //LookUpTableId = UsersObject.Id,
        //        ListPropertyPath = "CreatedByUser",
        //        PMPropertyPath = "CreatedByUser",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Open Date",
        //        FullFieldLable = "OpenDate",
        //        FieldName = "OpenDate",
        //        CanFilter = true,
        //        FieldsDataType = "DateTime",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        IsRequired = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "OpenDateListLable",
        //        Operator = "Equals",
        //        ListLableDefaultText = "Open Date",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //        ListPropertyPath = "OpenDate",
        //        PMPropertyPath = "OpenDate",
        //        IsTimeFrameFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sent Date",
        //        FullFieldLable = "SentDate",
        //        FieldName = "SentDate",
        //        CanFilter = true,
        //        FieldsDataType = "DateTime",
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "SentDateListLable",
        //        Operator = "Equals",
        //        ListLableDefaultText = "Sent Date",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //        ListPropertyPath = "SentDate",
        //        PMPropertyPath = "SentDate",
        //        IsTimeFrameFilter = true,
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Accepted Date",
        //        FullFieldLable = "AcceptedDate",
        //        FieldName = "AcceptedDate",
        //        CanFilter = true,
        //        FieldsDataType = "DateTime",
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "AcceptedDateListLable",
        //        Operator = "Equals",
        //        ListLableDefaultText = "Accepted Date",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //        ListPropertyPath = "AcceptedDate",
        //        PMPropertyPath = "AcceptedDate",
        //        IsTimeFrameFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Declined Date",
        //        FullFieldLable = "DeclinedDate",
        //        FieldName = "DeclinedDate",
        //        CanFilter = true,
        //        FieldsDataType = "DateTime",
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "DeclinedDateListLable",
        //        Operator = "Equals",
        //        ListLableDefaultText = "Declined Date",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //        ListPropertyPath = "DeclinedDate",
        //        PMPropertyPath = "DeclinedDate",
        //        IsTimeFrameFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notes",
        //        MultiLine = true,
        //        FullFieldLable = "Notes",
        //        FieldName = "Notes",
        //        FieldsDataType = "nText",
        //        MaxLength = 500,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "NotesListLable",
        //        ListLableDefaultText = "Notes",
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Notes",
        //        PMPropertyPath = "Notes",
        //        DataTemplateName = "NotesDataTemplate",
        //        HelpTextCode = "Notes",
        //        HelpTextDefaultText = "The maximum number of characters allowed in this field is 500.",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Description of Goods",
        //        MultiLine = true,
        //        FullFieldLable = "DescriptionOfGoods",
        //        FieldName = "DescriptionOfGoods",
        //        FieldsDataType = "Text",
        //        MaxLength = 512,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "DescriptionOfGoods",
        //        PMPropertyPath = "DescriptionOfGoods"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Closed",
        //        FullFieldLable = "IsClosed",
        //        FieldName = "IsClosed",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListPropertyPath = "IsClosed",
        //        PMPropertyPath = "IsClosed",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    //AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    //{
        //    //    DefaultText = "Open Quotes",
        //    //    FullFieldLable = "OpenQuotes",
        //    //    FieldName = "OpenQuotes",
        //    //    FieldsDataType = "Constant",
        //    //    MaxLength = 15,
        //    //    MinLength = 0,
        //    //    ObjectTableId = QuoteObject.Id,
        //    //    ObjectTableName = QuoteObject.Name,
        //    //    Tenant = 0,
        //    //    TextCodeType = "F",
        //    //    Operator = "Equals",
        //    //    IsCustomFilter = true,
        //    //}, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Closed Quotes",
        //        FullFieldLable = "ClosedQuotes",
        //        FieldName = "ClosedQuotes",
        //        FieldsDataType = "Constant",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        IsCustomFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Leading Currency",
        //        FullFieldLable = "LeadingCurrencyId",
        //        FieldName = "LeadingCurrencyId",
        //        FieldsDataType = "LookUp",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        MinLength = 0,
        //        MaxLength = 15,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        LookUpTableId = CurrenciesObject.Id,
        //        ListFieldLable = "LeadingCurrencyIdLable",
        //        ListLableDefaultText = "LeadingCurrencyId",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        InActive = true,
        //        ListPropertyPath = "LeadingCurrencyId",
        //        PMPropertyPath = "LeadingCurrencyId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Volume Unit Code",
        //        FullFieldLable = "VolumeUnitCode",
        //        FieldName = "VolumeUnitCode",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 3,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        LookUpTableId = VolumeUnitObject.Id,
        //        ListFieldLable = "VolumeUnitCodeLable",
        //        ListLableDefaultText = "VolumeUnitCode",
        //        Operator = "Equals",
        //        ListPropertyPath = "VolumeUnitCode",
        //        PMPropertyPath = "VolumeUnitCode"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Dimensions Unit Code",
        //        FullFieldLable = "DimensionsUnitCode",
        //        FieldName = "DimensionsUnitCode",
        //        FieldsDataType = "LookUp",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        MinLength = 0,
        //        MaxLength = 3,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        LookUpTableId = DimensionsUnitObject.Id,
        //        ListFieldLable = "DimensionsUnitCodeLable",
        //        ListLableDefaultText = "DimensionsUnit Code",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "DimensionsUnitCode",
        //        PMPropertyPath = "DimensionsUnitCode"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Gross Weight Unit Code",
        //        FullFieldLable = "GrossWeightUnitCode",
        //        FieldName = "GrossWeightUnitCode",
        //        FieldsDataType = "LookUp",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        MinLength = 0,
        //        MaxLength = 3,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        LookUpTableId = WeightUnitObject.Id,
        //        ListFieldLable = "GrossWeightUnitCodeLable",
        //        ListLableDefaultText = "Gross Weight Unit Code",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "GrossWeightUnitCode",
        //        PMPropertyPath = "GrossWeightUnitCode"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Chargeable Weight Unit Code",
        //        FullFieldLable = "ChargeableWeightUnitCode",
        //        FieldName = "ChargeableWeightUnitCode",
        //        ShortFieldLable = "WtMsrUnitCode",
        //        ShortFieldLableDefaultText = "Wt / Msr Unit Code",
        //        FieldsDataType = "LookUp",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        MinLength = 0,
        //        MaxLength = 3,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        LookUpTableId = WeightUnitObject.Id,
        //        ListFieldLable = "ChargeableWeightUnitCodeLable",
        //        ListLableDefaultText = "Chargeable Weight Unit Code",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ChargeableWeightUnitCode",
        //        PMPropertyPath = "ChargeableWeightUnitCode"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Volume (%VolumeCode)",
        //        FullFieldLable = "Volume",
        //        FieldName = "Volume",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        ListFieldLable = "VolumeLable",
        //        ListLableDefaultText = "Volume",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "Volume",
        //        PMPropertyPath = "Volume"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Volumetric Weight (%ChargWeightCode)",
        //        FullFieldLable = "VolumetricWeight",
        //        FieldName = "VolumetricWeight",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        ListFieldLable = "VolumetricWeightLable",
        //        ListLableDefaultText = "Volumetric Weight",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "VolumetricWeight",
        //        PMPropertyPath = "VolumetricWeight"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Gross Weight (%GrossWeightCode)",
        //        FullFieldLable = "GrossWeight",
        //        FieldName = "GrossWeight",
        //        FieldsDataType = "Double",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ListFieldLable = "GrossWeightListLable",
        //        ListLableDefaultText = "Gross Weight",
        //        DataTemplateName = "GrossWeightDataTemplate",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "GrossWeight",
        //        PMPropertyPath = "GrossWeight",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Gross Weight (KG)",
        //        FullFieldLable = "GrossWeightInKG",
        //        FieldName = "GrossWeightInKG",
        //        FieldsDataType = "Double",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ListFieldLable = "GrossWeightInKGListLable",
        //        ListLableDefaultText = "Gross Weight (KG)",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "GrossWeightInKG",
        //        PMPropertyPath = "GrossWeightInKG",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Gross Weight per Ton",
        //        FullFieldLable = "GrossWeightPerTon",
        //        FieldName = "GrossWeightPerTon",
        //        FieldsDataType = "Double",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ListFieldLable = "GrossWeightPerTonListLable",
        //        ListLableDefaultText = "Gross Weight per Ton",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "GrossWeightPerTon",
        //        PMPropertyPath = "GrossWeightPerTon",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Chargeable Weight (%ChargWeightCode)",
        //        FullFieldLable = "ChargeableWeight",
        //        FieldName = "ChargeableWeight",
        //        ShortFieldLable = "WtMsr",
        //        ShortFieldLableDefaultText = "Wt / Msr (%ChargWeightCode)",
        //        FieldsDataType = "Double",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        ListFieldLable = "ChargeableWeightListLable",
        //        ListLableDefaultText = "ChargW",
        //        CanFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "ChargeableWeight",
        //        PMPropertyPath = "ChargeableWeight"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Ratio",
        //        FullFieldLable = "Ratio",
        //        FieldName = "Ratio",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "RatioLable",
        //        ListLableDefaultText = "Ratio",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "Ratio",
        //        PMPropertyPath = "Ratio"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Dim Factor",
        //        FullFieldLable = "DimFactor",
        //        FieldName = "DimFactor",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "DimFactorLable",
        //        ListLableDefaultText = "DimFactor",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "DimFactor",
        //        PMPropertyPath = "DimFactor"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Number Of Packages",
        //        FullFieldLable = "NumberOfPackages",
        //        FieldName = "NumberOfPackages",
        //        FieldsDataType = "Integer",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        ListFieldLable = "NumberOfPackagesLable",
        //        ListLableDefaultText = "Number Of Packages",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "NumberOfPackages",
        //        PMPropertyPath = "NumberOfPackages"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Number of Containers",
        //        FullFieldLable = "NumberOfContainers",
        //        FieldName = "NumberOfContainers",
        //        FieldsDataType = "Integer",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        ListFieldLable = "NumberOfContainersLable",
        //        ListLableDefaultText = "Number Of Containers",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "NumberOfContainers",
        //        PMPropertyPath = "NumberOfContainers"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Dangerous Goods",
        //        FullFieldLable = "IsDangerous",
        //        FieldName = "IsDangerous",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsDangerousLable",
        //        ListLableDefaultText = "Dangerous Goods",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsDangerous",
        //        PMPropertyPath = "IsDangerous",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Expiration days",
        //        FullFieldLable = "ExpirationDays",
        //        FieldName = "ExpirationDays",
        //        FieldsDataType = "Integer",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ExpirationDaysLable",
        //        ListLableDefaultText = "Expiration days",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ExpirationDays",
        //        PMPropertyPath = "ExpirationDays",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Expiration date",
        //        FullFieldLable = "ExpirationDate",
        //        FieldName = "ExpirationDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ExpirationDateLable",
        //        ListLableDefaultText = "Expiration date",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "ExpirationDate",
        //        PMPropertyPath = "ExpirationDate",
        //        ConverterName = "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Expired Quote",
        //        FullFieldLable = "IsExpiredQuote",
        //        FieldName = "IsExpiredQuote",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        IsCustomFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Price by break",
        //        FullFieldLable = "IsFreightBySteps",
        //        FieldName = "IsFreightBySteps",
        //        ShortFieldLable = "IsFreightBySteps",
        //        ShortFieldLableDefaultText = "Price by break",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsFreightByStepsLable",
        //        ListLableDefaultText = "Price by break",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsFreightBySteps",
        //        PMPropertyPath = "IsFreightBySteps",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);
            
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Adhoc",
        //        FullFieldLable = "IsAdhoc",
        //        FieldName = "IsAdhoc",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsAdhocLable",
        //        ListLableDefaultText = "Is Adhoc",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsAdhoc",
        //        PMPropertyPath = "IsAdhoc",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Reference",
        //        FullFieldLable = "Reference",
        //        FieldName = "Reference",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        IsCustomFilter = true,
        //        Operator = "Custom",
        //        HelpTextDefaultText = "Searching by :\n1: Shipper Reference 1\n2: Shipper Reference 2\n3: Consignee Reference 1\n4: Consignee Reference 2\n5: Quote Number",
        //        HelpTextCode = "Reference",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Or To Port",
        //        FullFieldLable = "FromOrToPort",
        //        FieldName = "FromOrToPort",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        IsCustomFilter = true,
        //        Operator = "Custom"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Follow Up Date",
        //        FullFieldLable = "FUDate",
        //        FieldName = "FollowUpDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        DisplayInList = true,
        //        ListFieldLable = "FollowUpDateListLable",
        //        ListLableDefaultText = "F/U Date",
        //        DataTemplateName = "DateTimeDataTemplate",
        //        Operator = "LargerThan",
        //        ListPropertyPath = "FollowUpDate",
        //        PMPropertyPath = "FollowUpDate",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Follow Up Type",
        //        FullFieldLable = "FUType",
        //        FieldName = "FollowUpType",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        DisplayInList = true,
        //        ListFieldLable = "FollowUpTypeListLable",
        //        ListLableDefaultText = "F/U Type",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "FollowUpType",
        //        PMPropertyPath = "FollowUpType",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Follow Up Type",
        //        FullFieldLable = "FollowUpTypeId",
        //        FieldName = "FollowUpTypeId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = EventTypesObject.Id,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        DisplayInList = false,
        //        ListFieldLable = "FollowUpTypeIdListLable",
        //        ListLableDefaultText = "F/U Type",
        //        Operator = "Equals",
        //        ListPropertyPath = "FollowUpTypeId",
        //        PMPropertyPath = "FollowUpTypeId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Today's Follow Ups",
        //        FullFieldLable = "TodayFollowUps",
        //        FieldName = "TodayFollowUps",
        //        FieldsDataType = "Constant",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        IsCustomFilter = true,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        Operator = "Custom"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Tomorrow's Follow Ups",
        //        FullFieldLable = "TomorrowFollowUps",
        //        FieldName = "TomorrowFollowUps",
        //        FieldsDataType = "Constant",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        IsCustomFilter = true,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        Operator = "Custom"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Due Date Follow Ups",
        //        FullFieldLable = "DueDateFollowUps",
        //        FieldName = "DueDateFollowUps",
        //        FieldsDataType = "Constant",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        IsCustomFilter = true,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        Operator = "Custom"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "All Follow Ups",
        //        FullFieldLable = "AllFollowUps",
        //        FieldName = "AllFollowUps",
        //        FieldsDataType = "Constant",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        IsCustomFilter = true,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        Operator = "Custom"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "My Follow Ups",
        //        FullFieldLable = "MyFollowUps",
        //        FieldName = "MyFollowUps",
        //        FieldsDataType = "Constant",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 40,
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        IsCustomFilter = true,
        //        Operator = "Custom"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Follow Up Notes",
        //        FullFieldLable = "FUNotes",
        //        FieldName = "FollowUpNotes",
        //        FieldsDataType = "Text",
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        SystemMaxLength = 250,
        //        DisplayInList = true,
        //        ListFieldLable = "FollowUpNotesListLable",
        //        ListLableDefaultText = "Notes",
        //        DataTemplateName = "FollowUpNoteDataTemplate",
        //        ValidForQuerySection1 = "QuoteFollowUp",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "FollowUpNotes",
        //        PMPropertyPath = "FollowUpNotes",
        //        HasTemplate = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 1",
        //        FullFieldLable = "PackageType1Id",
        //        FieldName = "PackageType1Id",
        //        ShortFieldLable = "PackageTypeId",
        //        ShortFieldLableDefaultText = "Package Type",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        LookUpTableId = PackageTypesObject.Id,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType1IdListLable",
        //        ListLableDefaultText = "Package Type 1",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType1Id",
        //        PMPropertyPath = "PackageType1Id"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 2",
        //        FullFieldLable = "PackageType2Id",
        //        FieldName = "PackageType2Id",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        LookUpTableId = PackageTypesObject.Id,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType2IdListLable",
        //        ListLableDefaultText = "Package Type 2",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType2Id",
        //        PMPropertyPath = "PackageType2Id"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 3",
        //        FullFieldLable = "PackageType3Id",
        //        FieldName = "PackageType3Id",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        LookUpTableId = PackageTypesObject.Id,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType3IdListLable",
        //        ListLableDefaultText = "Package Type 3",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType3Id",
        //        PMPropertyPath = "PackageType3Id"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 4",
        //        FullFieldLable = "PackageType4Id",
        //        FieldName = "PackageType4Id",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        LookUpTableId = PackageTypesObject.Id,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType4IdListLable",
        //        ListLableDefaultText = "Package Type 4",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType4Id",
        //        PMPropertyPath = "PackageType4Id"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 5",
        //        FullFieldLable = "PackageType5Id",
        //        FieldName = "PackageType5Id",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        LookUpTableId = PackageTypesObject.Id,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType5IdListLable",
        //        ListLableDefaultText = "Package Type 5",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType5Id",
        //        PMPropertyPath = "PackageType5Id"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 1 Quantity",
        //        FullFieldLable = "PackageType1Quantity",
        //        FieldName = "PackageType1Quantity",
        //        ShortFieldLable = "PackageTypeQuantity",
        //        ShortFieldLableDefaultText = "Quantity",
        //        FieldsDataType = "Integer",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType1QuantityListLable",
        //        ListLableDefaultText = "Package Type 1 Quantity",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType1Quantity",
        //        PMPropertyPath = "PackageType1Quantity"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 2 Quantity",
        //        FullFieldLable = "PackageType2Quantity",
        //        FieldName = "PackageType2Quantity",
        //        FieldsDataType = "Integer",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType2QuantityListLable",
        //        ListLableDefaultText = "Package Type 2 Quantity",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType2Quantity",
        //        PMPropertyPath = "PackageType2Quantity"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 3 Quantity",
        //        FullFieldLable = "PackageType3Quantity",
        //        FieldName = "PackageType3Quantity",
        //        FieldsDataType = "Integer",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType3QuantityListLable",
        //        ListLableDefaultText = "Package Type 3 Quantity",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType3Quantity",
        //        PMPropertyPath = "PackageType3Quantity"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 4 Quantity",
        //        FullFieldLable = "PackageType4Quantity",
        //        FieldName = "PackageType4Quantity",
        //        FieldsDataType = "Integer",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType4QuantityListLable",
        //        ListLableDefaultText = "Package Type 4 Quantity",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType4Quantity",
        //        PMPropertyPath = "PackageType4Quantity"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type 5 Quantity",
        //        FullFieldLable = "PackageType5Quantity",
        //        FieldName = "PackageType5Quantity",
        //        FieldsDataType = "Integer",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "PackageType5QuantityListLable",
        //        ListLableDefaultText = "Package Type 5 Quantity",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        Operator = "Equals",
        //        ListPropertyPath = "PackageType5Quantity",
        //        PMPropertyPath = "PackageType5Quantity"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote charges",
        //        FullFieldLable = "QuoteCharges",
        //        FieldName = "QuoteCharges",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        MultiTableId = QuoteChargeObject.Id,
        //        ListFieldLable = "QuoteChargesListLable",
        //        ListLableDefaultText = "Quote charges",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "QuoteCharges",
        //        PMPropertyPath = "QuoteCharges",
        //        IsMulti = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Type",
        //        FullFieldLable = "QuoteTypeCode",
        //        FieldName = "QuoteTypeCode",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = QuoteTypeObject.Id,
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        IsRequired = true,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayInList = false,
        //        Operator = "Equals",
        //        ListPropertyPath = "QuoteTypeCode",
        //        PMPropertyPath = "QuoteTypeCode",
        //        HelpTextCode = "QuoteTypeCode",
        //        HelpTextDefaultText = "Spot Rate: Use for quoting prices for a specific shipment with a given quantity.%nRouting Rates: Use for quoting your rates for package types, per unit or by price break levels.",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Type",
        //        FullFieldLable = "QuoteTypeName",
        //        FieldName = "QuoteTypeName",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayInList = true,
        //        ListFieldLable = "QuoteTypeNameListLable",
        //        ListLableDefaultText = "Quote Type",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "QuoteTypeName",
        //        PMPropertyPath = "QuoteTypeName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search partners / ports / ref.# / notes",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        IsRequired = false,
        //        MaxLength = 1500,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = "Quote",
        //        ObjectTablePlural = "Quotes",
        //        ObjectTableSingular = "Quote",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        SystemMaxLength = 40,
        //        SystemRequired = false,
        //        ValidForQuerySection1 = "Quote",
        //        ValidForQuerySection2 = "QuoteFollowUp",
        //        DisplayInList = false,
        //        IsCustomFilter = false,
        //        Operator = "Contains",
        //        HelpTextDefaultText = "Searching by :\n1:quote numbers\n2: References\n3: consignee and shipper names\n4: from port to port",
        //        HelpTextCode = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Include PickUp",
        //        FullFieldLable = "IncludePickUp",
        //        FieldName = "IncludePickUp",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IncludePickUpListLable",
        //        ListLableDefaultText = "Include PickUp",
        //        CanFilter = true,
        //        ListPropertyPath = "IncludePickUp",
        //        PMPropertyPath = "IncludePickUp",
        //        Operator = "Equals",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Include Delivery",
        //        FullFieldLable = "IncludeDelivery",
        //        FieldName = "IncludeDelivery",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IncludeDeliveryListLable",
        //        ListLableDefaultText = "Include Delivery",
        //        CanFilter = true,
        //        ListPropertyPath = "IncludeDelivery",
        //        PMPropertyPath = "IncludeDelivery",
        //        Operator = "Equals",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "PickUp Address",
        //        FullFieldLable = "PickUpAddressId",
        //        FieldName = "PickUpAddressId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = AddressObject.Id,
        //        ListFieldLable = "PickUpAddressIdListLable",
        //        ListLableDefaultText = "PickUp Address",
        //        CanFilter = true,
        //        ListPropertyPath = "PickUpAddressId",
        //        PMPropertyPath = "PickUpAddressId",
        //        Operator = "Equals",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Delivery Address",
        //        FullFieldLable = "DeliveryAddressId",
        //        FieldName = "DeliveryAddressId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = AddressObject.Id,
        //        ListFieldLable = "DeliveryAddressIdListLable",
        //        ListLableDefaultText = "Delivery Address",
        //        CanFilter = true,
        //        ListPropertyPath = "DeliveryAddressId",
        //        PMPropertyPath = "DeliveryAddressId",
        //        Operator = "Equals",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Address City",
        //        FullFieldLable = "FromAddressCity",
        //        FieldName = "FromAddressCity",
        //        ShortFieldLable = "FromAddressCity",
        //        ShortFieldLableDefaultText = "City",
        //        FieldsDataType = "nText",
        //        MaxLength = 25,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "FromAddressCityListLable",
        //        ListLableDefaultText = "From Address City",
        //        CanFilter = true,
        //        ListPropertyPath = "FromAddressCity",
        //        PMPropertyPath = "FromAddressCity",
        //        Operator = "StartsWith"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Address Country",
        //        FullFieldLable = "FromAddressCountryId",
        //        FieldName = "FromAddressCountryId",
        //        FieldsDataType = "LookUp",
        //        ShortFieldLable = "FromAddressCountryId",
        //        ShortFieldLableDefaultText = "Country",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = CountriesObject.Id,
        //        ListFieldLable = "FromAddressCountryListLable",
        //        ListLableDefaultText = "From Address Country",
        //        CanFilter = true,
        //        ListPropertyPath = "FromAddressCountryId",
        //        PMPropertyPath = "FromAddressCountryId",
        //        Operator = "StartsWith",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "From Address Zip Code",
        //        FullFieldLable = "FromAddressZipCode",
        //        FieldName = "FromAddressZipCode",
        //        ShortFieldLable = "FromAddressZipCode",
        //        ShortFieldLableDefaultText = "Zip Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "FromAddressZipCodeListLable",
        //        ListLableDefaultText = "From Address Zip Code",
        //        CanFilter = false,
        //        ListPropertyPath = "FromAddressZipCode",
        //        PMPropertyPath = "FromAddressZipCode",
        //        Operator = "StartsWith"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Address City",
        //        FullFieldLable = "ToAddressCity",
        //        FieldName = "ToAddressCity",
        //        ShortFieldLable = "ToAddressCity",
        //        ShortFieldLableDefaultText = "City",
        //        FieldsDataType = "nText",
        //        MaxLength = 25,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ToAddressCityListLable",
        //        ListLableDefaultText = "To Address City",
        //        CanFilter = true,
        //        ListPropertyPath = "ToAddressCity",
        //        PMPropertyPath = "ToAddressCity",
        //        Operator = "StartsWith"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Address Country",
        //        FullFieldLable = "ToAddressCountryId",
        //        FieldName = "ToAddressCountryId",
        //        FieldsDataType = "LookUp",
        //        ShortFieldLable = "ToAddressCountryId",
        //        ShortFieldLableDefaultText = "Country",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = CountriesObject.Id,
        //        ListFieldLable = "ToAddressCountryListLable",
        //        ListLableDefaultText = "To Address Country",
        //        CanFilter = true,
        //        ListPropertyPath = "ToAddressCountryId",
        //        PMPropertyPath = "ToAddressCountryId",
        //        Operator = "StartsWith",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "To Address Zip Code",
        //        FullFieldLable = "ToAddressZipCode",
        //        FieldName = "ToAddressZipCode",
        //        ShortFieldLable = "ToAddressZipCode",
        //        ShortFieldLableDefaultText = "Zip Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ToAddressZipCodeListLable",
        //        ListLableDefaultText = "To Address Zip Code",
        //        CanFilter = false,
        //        ListPropertyPath = "ToAddressZipCode",
        //        PMPropertyPath = "ToAddressZipCode",
        //        Operator = "StartsWith"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Draft Quote",
        //        FullFieldLable = "IsDraftQuote",
        //        FieldName = "IsDraftQuote",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsDraftQuoteListLabel",
        //        ListLableDefaultText = "Is Draft Quote",
        //        ValidForQuerySection1 = "Quote",
        //        IsCustomFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "IsDraftQuote",
        //        PMPropertyPath = "IsDraftQuote",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Created Quote",
        //        FullFieldLable = "IsCreatedQuote",
        //        FieldName = "IsCreatedQuote",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsCreatedQuoteListLabel",
        //        ListLableDefaultText = "Is Created Quote",
        //        ValidForQuerySection1 = "Quote",
        //        IsCustomFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "IsCreatedQuote",
        //        PMPropertyPath = "IsCreatedQuote",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Sent Quote",
        //        FullFieldLable = "IsSentQuote",
        //        FieldName = "IsSentQuote",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsSentQuoteListLabel",
        //        ListLableDefaultText = "Is Sent Quote",
        //        ValidForQuerySection1 = "Quote",
        //        IsCustomFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "IsSentQuote",
        //        PMPropertyPath = "IsSentQuote",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Accepted Quote",
        //        FullFieldLable = "IsAcceptedQuote",
        //        FieldName = "IsAcceptedQuote",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsAcceptedQuoteListLabel",
        //        ListLableDefaultText = "Is Accepted Quote",
        //        ValidForQuerySection1 = "Quote",
        //        IsCustomFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "IsAcceptedQuote",
        //        PMPropertyPath = "IsAcceptedQuote",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Open Quotes",
        //        FullFieldLable = "OpenQuotes",
        //        FieldName = "OpenQuotes",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        IsCustomFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "OpenQuotes",
        //        PMPropertyPath = "OpenQuotes",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);



        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Total Per Container",
        //        FullFieldLable = "TotalPerContainer",
        //        FieldName = "TotalPerContainer",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "TotalPerContainerLable",
        //        ListLableDefaultText = "Total Per Container",
        //        ValidForQuerySection1 = "Quote",
        //        Operator = "Equals",
        //        ListPropertyPath = "TotalPerContainer",
        //        PMPropertyPath = "TotalPerContainer",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);



        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Quote Data External",
        //        FullFieldLable = "IsQuoteDataExternal",
        //        FieldName = "IsQuoteDataExternal",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsQuoteDataExternalListLabel",
        //        ListLableDefaultText = "Is Quote Data External",
        //        ValidForQuerySection1 = "Quote",
        //        CanFilter = true,
        //        DisplayOnly = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "IsQuoteDataExternal",
        //        PMPropertyPath = "IsQuoteDataExternal",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Quote Document External",
        //        FullFieldLable = "IsQuoteDocumentExternal",
        //        FieldName = "IsQuoteDocumentExternal",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsQuoteDocumentExternalListLabel",
        //        ListLableDefaultText = "Is Quote Document External",
        //        ValidForQuerySection1 = "Quote",
        //        CanFilter = true,
        //        DisplayOnly = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "IsQuoteDocumentExternal",
        //        PMPropertyPath = "IsQuoteDocumentExternal",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Total Sale Including VAT Amount In Sale Currency",
        //        FullFieldLable = "TotalSaleIncludingVATAmountInSaleCurrency",
        //        FieldName = "TotalSaleIncludingVATAmountInSaleCurrency",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        Operator = "Equals",
        //        ListPropertyPath = "TotalSaleIncludingVATAmountInSaleCurrency",
        //        PMPropertyPath = "TotalSaleIncludingVATAmountInSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Total Sale Including VAT Amount In Local Currency",
        //        FullFieldLable = "TotalSaleIncludingVATAmountInLocalCurrency",
        //        FieldName = "TotalSaleIncludingVATAmountInLocalCurrency",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteObject.Id,
        //        ObjectTableName = QuoteObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ValidForQuerySection1 = "Quote",
        //        Operator = "Equals",
        //        ListPropertyPath = "TotalSaleIncludingVATAmountInLocalCurrency",
        //        PMPropertyPath = "TotalSaleIncludingVATAmountInLocalCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    // End
        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteTypeFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTypeObject.Id,
        //        ObjectTableName = QuoteTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        IsRequired = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code",
        //        ListLableDefaultText = "Code",
        //        ListFieldLable = "Code",
        //        //DisplayOnLookUp = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTypeObject.Id,
        //        ObjectTableName = QuoteTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name",
        //        IsRequired = true,
        //        DisplayOnLookUp = true,
        //        CanFilter = true,
        //        ListLableDefaultText = "Name",
        //        ListFieldLable = "Name",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search..",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        IsRequired = false,
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTypeObject.Id,
        //        ObjectTableName = "QuoteType",
        //        ObjectTablePlural = "QuoteTypes",
        //        ObjectTableSingular = "QuoteType",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        SystemMaxLength = 40,
        //        SystemRequired = false,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "QuoteType",
        //        ValidForQuerySection2 = "QuoteTypeFollowUp",
        //        DisplayInList = false,
        //        IsCustomFilter = false,
        //        Operator = "Contains",
        //        HelpTextDefaultText = "Searching by :\n1: code\n2: name",
        //        HelpTextCode = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteChargeFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Price Steps",
        //        FullFieldLable = "QuoteChargePriceSteps",
        //        FieldName = "QuoteChargePriceSteps",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "QuoteChargePriceSteps",
        //        IsMulti = true,
        //        MultiTableId = QuotePriceStepObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeId",
        //        FieldName = "VatTypeId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = VatTypesObject.Id,
        //        Operator = "Equals",
        //        ListPropertyPath = "VatTypeId",
        //        PMPropertyPath = "VatTypeId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Percentage",
        //        FullFieldLable = "VatPercentage",
        //        FieldName = "VatPercentage",
        //        FieldsDataType = "SigDouble",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatPercentage",
        //        PMPropertyPath = "VatPercentage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeName",
        //        FieldName = "VatTypeName",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "VatTypeName",
        //        PMPropertyPath = "VatTypeName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Amount",
        //        FullFieldLable = "VatAmount",
        //        FieldName = "VatAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatAmount",
        //        PMPropertyPath = "VatAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Price by break",
        //        FullFieldLable = "IsChargeBySteps",
        //        FieldName = "IsChargeBySteps",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        MaxLength = 1,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "IsChargeBySteps",
        //        PMPropertyPath = "IsChargeBySteps",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Max Amount",
        //        FullFieldLable = "CostMaxAmount",
        //        FieldName = "CostMaxAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostMaxAmount",
        //        ListPropertyPath = "CostMaxAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Min Amount",
        //        FullFieldLable = "CostMinAmount",
        //        FieldName = "CostMinAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostMinAmount",
        //        ListPropertyPath = "CostMinAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Min Amount",
        //        FullFieldLable = "SaleMinAmount",
        //        FieldName = "SaleMinAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleMinAmount",
        //        ListPropertyPath = "SaleMinAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Max Amount",
        //        FullFieldLable = "SaleMaxAmount",
        //        FieldName = "SaleMaxAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleMaxAmount",
        //        ListPropertyPath = "SaleMaxAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Is Fixed Rate",
        //        FullFieldLable = "SaleIsFixedRate",
        //        FieldName = "SaleIsFixedRate",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "SaleIsFixedRate"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Exchange Rate",
        //        FullFieldLable = "CostExchangeRate",
        //        FieldName = "CostExchangeRate",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        IsRequired = true,
        //        PMPropertyPath = "CostExchangeRate",
        //        ListPropertyPath = "CostExchangeRate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Currency",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "CostCurrencyId",
        //        FieldName = "CostCurrencyId",
        //        FieldsDataType = "LookUp",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostCurrencyId",
        //        ListPropertyPath = "CostCurrencyId",
        //        LookUpTableId = CurrenciesObject.Id
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Is Fixed Rate",
        //        FullFieldLable = "CostIsFixedRate",
        //        FieldName = "CostIsFixedRate",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "CostIsFixedRate"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Charges Type",
        //        FullFieldLable = "ChargesTypeId",
        //        FieldName = "ChargesTypeId",
        //        FieldsDataType = "LookUp",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "ChargesTypeId",
        //        ListLableDefaultText = "Charge Type",
        //        Operator = "Equals",
        //        PMPropertyPath = "ChargesTypeId",
        //        ListPropertyPath = "ChargesTypeId",
        //        LookUpTableId = ChargesTypesObject.Id
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Vendor",
        //        FullFieldLable = "VendorId",
        //        FieldName = "VendorId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "VendorId",
        //        ListLableDefaultText = "Vendor",
        //        Operator = "Equals",
        //        PMPropertyPath = "VendorId",
        //        ListPropertyPath = "VendorId",
        //        LookUpTableId = CardsObject.Id,
        //        HelpTextCode = "VendorId",
        //        HelpTextDefaultText = "The vendor of the service will be transferred to the shipment together with the cost price.",
        //        HelpLocalDefaultText = "הספק של השירות יועבר לשילוח יחד עם סכום העלות",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Currency",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SaleCurrency",
        //        FieldName = "SaleCurrencyId",
        //        FieldsDataType = "LookUp",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        DisplayInList = true,
        //        ListFieldLable = "SaleCurrencyId",
        //        ListLableDefaultText = "Curr",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleCurrencyId",
        //        ListPropertyPath = "SaleCurrencyId",
        //        LookUpTableId = CurrenciesObject.Id
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Exchange Rate",
        //        FullFieldLable = "SaleExchangeRate",
        //        FieldName = "SaleExchangeRate",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleExchangeRate",
        //        ListPropertyPath = "SaleExchangeRate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "MarkUp Type Code",
        //        FullFieldLable = "MarkUpTypeCode",
        //        FieldName = "MarkUpTypeCode",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "MarkUpTypeCode",
        //        LookUpTableId = MarkUpTypeObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "MarkUp",
        //        FullFieldLable = "MarkUpValue",
        //        FieldName = "MarkUpValue",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "MarkUpValue",
        //        ListPropertyPath = "MarkUpValue",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is All IN",
        //        FullFieldLable = "IsAllIN",
        //        FieldName = "IsAllIN",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        DisplayInList = true,
        //        ListFieldLable = "IsAllIn",
        //        ListLableDefaultText = "All In",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "IsAllIN",
        //        HelpTextCode = "IsAllIN",
        //        HelpTextDefaultText = "How it works: select check box. Amount will be added to the freight amount and this charge type will not be invoiced.",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notes",
        //        FullFieldLable = "Notes",
        //        FieldName = "Notes",
        //        FieldsDataType = "nText",
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "Notes",
        //        DataTemplateName = "NotesDataTemplate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    /* Cost Fields */
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost UOM",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "CostMeasurementId",
        //        FieldName = "CostMeasurementId",
        //        FieldsDataType = "LookUp",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "CostMeasurementId",
        //        ListLableDefaultText = "UOM",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostMeasurementId",
        //        ListPropertyPath = "CostMeasurementId",
        //        LookUpTableId = MeasurementsObject.Id,
        //        HelpTextCode = "CostMeasurementId",
        //        HelpTextDefaultText = "The unit of measurement (e.g. CHWT-chargeable weight) on which the cost price is based displays from the definition of the selected charge type. You can select another measurement.",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Quantity",
        //        FullFieldLable = "CostQuantity",
        //        FieldName = "CostQuantity",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostQuantity",
        //        ListPropertyPath = "CostQuantity",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Unit Price",
        //        FullFieldLable = "CostUnitPrice",
        //        FieldName = "CostUnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostUnitPrice",
        //        ListPropertyPath = "CostUnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Total Amount",
        //        FullFieldLable = "CostTotalAmount",
        //        FieldName = "CostTotalAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostTotalAmount",
        //        ListPropertyPath = "CostTotalAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Total Amount Local",
        //        FullFieldLable = "CostTotalAmountLocal",
        //        FieldName = "CostTotalAmountLocal",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostTotalAmountLocal",
        //        ListPropertyPath = "CostTotalAmountLocal",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 1 Unit Price",
        //        FullFieldLable = "CostContainerType1UnitPrice",
        //        FieldName = "CostContainerType1UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType1UnitPrice",
        //        ListPropertyPath = "CostContainerType1UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 2 Unit Price",
        //        FullFieldLable = "CostContainerType2UnitPrice",
        //        FieldName = "CostContainerType2UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType2UnitPrice",
        //        ListPropertyPath = "CostContainerType2UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 3 Unit Price",
        //        FullFieldLable = "CostContainerType3UnitPrice",
        //        FieldName = "CostContainerType3UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType3UnitPrice",
        //        ListPropertyPath = "CostContainerType3UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 4 Unit Price",
        //        FullFieldLable = "CostContainerType4UnitPrice",
        //        FieldName = "CostContainerType4UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType4UnitPrice",
        //        ListPropertyPath = "CostContainerType4UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 5 Unit Price",
        //        FullFieldLable = "CostContainerType5UnitPrice",
        //        FieldName = "CostContainerType5UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType5UnitPrice",
        //        ListPropertyPath = "CostContainerType5UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    /* Sale Fields */
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale UOM",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SaleMeasurementId",
        //        FieldName = "SaleMeasurementId",
        //        FieldsDataType = "LookUp",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleMeasurementId",
        //        ListPropertyPath = "SaleMeasurementId",
        //        LookUpTableId = MeasurementsObject.Id,
        //        HelpTextCode = "SaleMeasurementId",
        //        HelpTextDefaultText = "The unit of measurement (e.g. CHWT-chargeable weight) on which the sale price is based displays from the definition of the selected charge type. You can select another measurement.",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Quantity",
        //        FullFieldLable = "SaleQuantity",
        //        FieldName = "SaleQuantity",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleQuantity",
        //        ListPropertyPath = "SaleQuantity",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price",
        //        FullFieldLable = "SaleUnitPrice",
        //        FieldName = "SaleUnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice",
        //        ListPropertyPath = "SaleUnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Total Amount",
        //        FullFieldLable = "SaleTotalAmount",
        //        FieldName = "SaleTotalAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleTotalAmount",
        //        ListPropertyPath = "SaleTotalAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Total Amount Local",
        //        FullFieldLable = "SaleTotalAmountLocal",
        //        FieldName = "SaleTotalAmountLocal",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleTotalAmountLocal",
        //        ListPropertyPath = "SaleTotalAmountLocal",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 1 Unit Price",
        //        FullFieldLable = "SaleContainerType1UnitPrice",
        //        FieldName = "SaleContainerType1UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType1UnitPrice",
        //        ListPropertyPath = "SaleContainerType1UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 2 Unit Price",
        //        FullFieldLable = "SaleContainerType2UnitPrice",
        //        FieldName = "SaleContainerType2UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType2UnitPrice",
        //        ListPropertyPath = "SaleContainerType2UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 3 Unit Price",
        //        FullFieldLable = "SaleContainerType3UnitPrice",
        //        FieldName = "SaleContainerType3UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType3UnitPrice",
        //        ListPropertyPath = "SaleContainerType3UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 4 Unit Price",
        //        FullFieldLable = "SaleContainerType4UnitPrice",
        //        FieldName = "SaleContainerType4UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType4UnitPrice",
        //        ListPropertyPath = "SaleContainerType4UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 5 Unit Price",
        //        FullFieldLable = "SaleContainerType5UnitPrice",
        //        FieldName = "SaleContainerType5UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType5UnitPrice",
        //        ListPropertyPath = "SaleContainerType5UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price In Sale Currency",
        //        FullFieldLable = "SaleUnitPriceInSaleCurrency",
        //        FieldName = "SaleUnitPriceInSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPriceInSaleCurrency",
        //        ListPropertyPath = "SaleUnitPriceInSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price 1 In Sale Currency",
        //        FullFieldLable = "SaleUnitPrice1InSaleCurrency",
        //        FieldName = "SaleUnitPrice1InSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice1InSaleCurrency",
        //        ListPropertyPath = "SaleUnitPrice1InSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price 2 In Sale Currency",
        //        FullFieldLable = "SaleUnitPrice2InSaleCurrency",
        //        FieldName = "SaleUnitPrice2InSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice2InSaleCurrency",
        //        ListPropertyPath = "SaleUnitPrice2InSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price 3 In Sale Currency",
        //        FullFieldLable = "SaleUnitPrice3InSaleCurrency",
        //        FieldName = "SaleUnitPrice3InSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice3InSaleCurrency",
        //        ListPropertyPath = "SaleUnitPrice3InSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price 4 In Sale Currency",
        //        FullFieldLable = "SaleUnitPrice4InSaleCurrency",
        //        FieldName = "SaleUnitPrice4InSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice4InSaleCurrency",
        //        ListPropertyPath = "SaleUnitPrice4InSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price 5 In Sale Currency",
        //        FullFieldLable = "SaleUnitPrice5InSaleCurrency",
        //        FieldName = "SaleUnitPrice5InSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice5InSaleCurrency",
        //        ListPropertyPath = "SaleUnitPrice5InSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Amount In Sale Currency",
        //        FullFieldLable = "SaleAmountInSaleCurrency",
        //        FieldName = "SaleAmountInSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteChargeObject.Id,
        //        ObjectTableName = QuoteChargeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleAmountInSaleCurrency",
        //        ListPropertyPath = "SaleAmountInSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteCostChargeFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeId",
        //        FieldName = "VatTypeId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = VatTypesObject.Id,
        //        Operator = "Equals",
        //        ListPropertyPath = "VatTypeId",
        //        PMPropertyPath = "VatTypeId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Percentage",
        //        FullFieldLable = "VatPercentage",
        //        FieldName = "VatPercentage",
        //        FieldsDataType = "SigDouble",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatPercentage",
        //        PMPropertyPath = "VatPercentage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeName",
        //        FieldName = "VatTypeName",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "VatTypeName",
        //        PMPropertyPath = "VatTypeName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "UOM Percentage",
        //        FullFieldLable = "UOMPercentage",
        //        FieldName = "UOMPercentage",
        //        FieldsDataType = "Text",
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        MaxLength = 1,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "UOMPercentage",
        //        PMPropertyPath = "UOMPercentage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Charges Type",
        //        FullFieldLable = "ChargesType",
        //        FieldName = "ChargesTypeId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "ChargesTypeId",
        //        LookUpTableId = ChargesTypesObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Currency",
        //        FullFieldLable = "Currency",
        //        FieldName = "CurrencyId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CurrencyId",
        //        LookUpTableId = CurrenciesObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Exchange Rate",
        //        FullFieldLable = "SaleExchangeRate",
        //        FieldName = "SaleExchangeRate",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleExchangeRate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "MarkUpTypeCode",
        //        FullFieldLable = "MarkUpTypeCode",
        //        FieldName = "MarkUpTypeCode",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "MarkUpTypeCode",
        //        LookUpTableId = MarkUpTypeObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "MarkUpValue",
        //        FullFieldLable = "MarkUpValue",
        //        FieldName = "MarkUpValue",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "MarkUpValue",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notes",
        //        FullFieldLable = "Notes",
        //        FieldName = "Notes",
        //        FieldsDataType = "Text",
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "Notes",
        //        DataTemplateName = "NotesDataTemplate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    /* Cost Fields */
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost UOM",
        //        FullFieldLable = "Cost UOM",
        //        FieldName = "CostMeasurementId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostMeasurementId",
        //        LookUpTableId = MeasurementsObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Quantity",
        //        FullFieldLable = "CostQuantity",
        //        FieldName = "CostQuantity",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostQuantity",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Unit Price",
        //        FullFieldLable = "CostUnitPrice",
        //        FieldName = "CostUnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostUnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Total Amount",
        //        FullFieldLable = "CostTotalAmount",
        //        FieldName = "CostTotalAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostTotalAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Total Amount Local",
        //        FullFieldLable = "CostTotalAmountLocal",
        //        FieldName = "CostTotalAmountLocal",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostTotalAmountLocal",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 1 Unit Price",
        //        FullFieldLable = "CostContainerType1UnitPrice",
        //        FieldName = "CostContainerType1UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType1UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 2 Unit Price",
        //        FullFieldLable = "CostContainerType2UnitPrice",
        //        FieldName = "CostContainerType2UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType2UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 3 Unit Price",
        //        FullFieldLable = "CostContainerType3UnitPrice",
        //        FieldName = "CostContainerType3UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType3UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 4 Unit Price",
        //        FullFieldLable = "CostContainerType4UnitPrice",
        //        FieldName = "CostContainerType4UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType4UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Container Type 5 Unit Price",
        //        FullFieldLable = "CostContainerType5UnitPrice",
        //        FieldName = "CostContainerType5UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostContainerType5UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Min Amount",
        //        FullFieldLable = "CostMinAmount",
        //        FieldName = "CostMinAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostMinAmount",
        //        ListPropertyPath = "CostMinAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Max Amount",
        //        FullFieldLable = "CostMaxAmount",
        //        FieldName = "CostMaxAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCostChargesObject.Id,
        //        ObjectTableName = QuoteCostChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostMaxAmount",
        //        ListPropertyPath = "CostMaxAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteSaleChargeFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeId",
        //        FieldName = "VatTypeId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = VatTypesObject.Id,
        //        Operator = "Equals",
        //        ListPropertyPath = "VatTypeId",
        //        PMPropertyPath = "VatTypeId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Percentage",
        //        FullFieldLable = "VatPercentage",
        //        FieldName = "VatPercentage",
        //        FieldsDataType = "SigDouble",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatPercentage",
        //        PMPropertyPath = "VatPercentage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeName",
        //        FieldName = "VatTypeName",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "VatTypeName",
        //        PMPropertyPath = "VatTypeName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Amount",
        //        FullFieldLable = "VatAmount",
        //        FieldName = "VatAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatAmount",
        //        PMPropertyPath = "VatAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "UOM Percentage",
        //        FullFieldLable = "UOMPercentage",
        //        FieldName = "UOMPercentage",
        //        FieldsDataType = "Text",
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        MaxLength = 1,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "UOMPercentage",
        //        PMPropertyPath = "UOMPercentage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Price Breaks",
        //        FullFieldLable = "PriceBreaks",
        //        FieldName = "PriceBreaks",
        //        FieldsDataType = "nText",
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        PMPropertyPath = "PriceBreaks",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Charges Type",
        //        FullFieldLable = "ChargesType",
        //        FieldName = "ChargesTypeId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "ChargesTypeId",
        //        LookUpTableId = ChargesTypesObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Currency",
        //        FullFieldLable = "Currency",
        //        FieldName = "CurrencyId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CurrencyId",
        //        LookUpTableId = CurrenciesObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Exchange Rate",
        //        FullFieldLable = "SaleExchangeRate",
        //        FieldName = "SaleExchangeRate",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleExchangeRate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "MarkUpTypeCode",
        //        FullFieldLable = "MarkUpTypeCode",
        //        FieldName = "MarkUpTypeCode",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "MarkUpTypeCode",
        //        LookUpTableId = MarkUpTypeObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "MarkUpValue",
        //        FullFieldLable = "MarkUpValue",
        //        FieldName = "MarkUpValue",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "MarkUpValue",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "FixedAmountCode",
        //        FullFieldLable = "FixedAmountCode",
        //        FieldName = "FixedAmountCode",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "FixedAmountCode",
        //        LookUpTableId = FixedAmountObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "ForeignAmountFixed",
        //        FullFieldLable = "ForeignAmountFixed",
        //        FieldName = "ForeignAmountFixed",
        //        FieldsDataType = "Text",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "ForeignAmountFixed",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "LocalAmountFixed",
        //        FullFieldLable = "LocalAmountFixed",
        //        FieldName = "LocalAmountFixed",
        //        FieldsDataType = "Text",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "LocalAmountFixed",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "IsAllIN",
        //        FullFieldLable = "IsAllIN",
        //        FieldName = "IsAllIN",
        //        FieldsDataType = "Text",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "IsAllIN",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notes",
        //        FullFieldLable = "Notes",
        //        FieldName = "Notes",
        //        FieldsDataType = "Text",
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        PMPropertyPath = "Notes",
        //        DataTemplateName = "NotesDataTemplate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    /* Sale Fields */
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale UOM",
        //        FullFieldLable = "Sale UOM",
        //        FieldName = "SaleMeasurementId",
        //        FieldsDataType = "LookUp",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleMeasurementId",
        //        LookUpTableId = MeasurementsObject.Id,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Quantity",
        //        FullFieldLable = "SaleQuantity",
        //        FieldName = "SaleQuantity",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleQuantity",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price",
        //        FullFieldLable = "SaleUnitPrice",
        //        FieldName = "SaleUnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Total Amount",
        //        FullFieldLable = "SaleTotalAmount",
        //        FieldName = "SaleTotalAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleTotalAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Total Amount Local",
        //        FullFieldLable = "SaleTotalAmountLocal",
        //        FieldName = "SaleTotalAmountLocal",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleTotalAmountLocal",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 1 Unit Price",
        //        FullFieldLable = "SaleContainerType1UnitPrice",
        //        FieldName = "SaleContainerType1UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType1UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 2 Unit Price",
        //        FullFieldLable = "SaleContainerType2UnitPrice",
        //        FieldName = "SaleContainerType2UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType2UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 3 Unit Price",
        //        FullFieldLable = "SaleContainerType3UnitPrice",
        //        FieldName = "SaleContainerType3UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType3UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 4 Unit Price",
        //        FullFieldLable = "SaleContainerType4UnitPrice",
        //        FieldName = "SaleContainerType4UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType4UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Container Type 5 Unit Price",
        //        FullFieldLable = "SaleContainerType5UnitPrice",
        //        FieldName = "SaleContainerType5UnitPrice",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleContainerType5UnitPrice",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Min Amount",
        //        FullFieldLable = "CostMinAmount",
        //        FieldName = "CostMinAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        DisplayInEntityVariables = false,
        //        PMPropertyPath = "CostMinAmount",
        //        ListPropertyPath = "CostMinAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Min Amount",
        //        FullFieldLable = "SaleMinAmount",
        //        FieldName = "SaleMinAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleMinAmount",
        //        ListPropertyPath = "SaleMinAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Max Amount",
        //        FullFieldLable = "SaleMaxAmount",
        //        FieldName = "SaleMaxAmount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSaleChargesObject.Id,
        //        ObjectTableName = QuoteSaleChargesObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleMaxAmount",
        //        ListPropertyPath = "SaleMaxAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}

        //private void CreateQuotePriceStepsFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Break from",
        //        FullFieldLable = "Step",
        //        FieldName = "Step",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuotePriceStepObject.Id,
        //        ObjectTableName = QuotePriceStepObject.Name,
        //        Tenant = 0,
        //        IsRequired = true,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "Step",
        //        ListPropertyPath = "Step",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost Unit Price",
        //        FullFieldLable = "CostUnitPrice",
        //        FieldName = "CostUnitPrice",
        //        FieldsDataType = "Double",
        //        ShortFieldLable = "CostStepPrices",
        //        ShortFieldLableDefaultText = "Cost break price",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePriceStepObject.Id,
        //        ObjectTableName = QuotePriceStepObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CostUnitPrice",
        //        ListPropertyPath = "CostUnitPrice",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale Unit Price",
        //        FullFieldLable = "SaleUnitPrice",
        //        FieldName = "SaleUnitPrice",
        //        FieldsDataType = "Double",
        //        ShortFieldLable = "SaleStepPrices",
        //        ShortFieldLableDefaultText = "Sale break price",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePriceStepObject.Id,
        //        ObjectTableName = QuotePriceStepObject.Name,
        //        Tenant = 0,
        //        IsRequired = true,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "SaleUnitPrice",
        //        ListPropertyPath = "SaleUnitPrice",
        //        DisplayInEntityVariables = false,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Markup Value",
        //        FullFieldLable = "MarkupValue",
        //        FieldName = "MarkupValue",
        //        ShortFieldLable = "MarkupValue",
        //        ShortFieldLableDefaultText = "Markup",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePriceStepObject.Id,
        //        ObjectTableName = QuotePriceStepObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "MarkupValue",
        //        ListPropertyPath = "MarkupValue",
        //        HelpTextCode = "MarkupValue",
        //        HelpTextDefaultText = "The fixed markup amount to add on the cost price in order to get the sale price.",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteCustomerTypeFields(Dictionary<string, ObjectField> objectfields, Dictionary<string, TextCode> textcodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "ShowInLOV",
        //        ObjectTableId = QuoteCustomerTypeObject.Id,
        //        ObjectTableName = QuoteCustomerTypeObject.Name,
        //        FullFieldLable = "ShowInLOV",
        //        FieldName = "ShowInLOV",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        Tenant = 0,
        //        TextCodeType = "F",                
        //        ListPropertyPath = "ShowInLOV",
        //        PMPropertyPath = "ShowInLOV",
        //        Operator = "Equals"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectfields, textcodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        IsRequired = true,
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCustomerTypeObject.Id,
        //        ObjectTableName = QuoteCustomerTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "CodeListLable",
        //        ListLableDefaultText = "Code",
        //        CanFilter = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectfields, textcodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        DisplayOnLookUp = true,
        //        DisplayInLookUpIndex = 0,
        //        DisplayInSearchWindowList = true,
        //        DisplayInSearchWindowListIndex = 0,
        //        DisplayInSearchWindowFilters = true,
        //        DisplayInSearchWindowFiltersIndex = 0,
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        FieldsDataType = "Text",
        //        IsRequired = true,
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCustomerTypeObject.Id,
        //        ObjectTableName = QuoteCustomerTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        LookUpTableId = null,
        //        ListFieldLable = "NameListLable",
        //        ListLableDefaultText = "Name",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectfields, textcodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search..",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        IsRequired = false,
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteCustomerTypeObject.Id,
        //        ObjectTableName = "QuoteCustomerType",
        //        ObjectTablePlural = "QuoteCustomerTypes",
        //        ObjectTableSingular = "QuoteCustomerType",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        SystemMaxLength = 40,
        //        SystemRequired = false,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "QuoteCustomerType",
        //        ValidForQuerySection2 = "QuoteCustomerTypeFollowUp",
        //        DisplayInList = false,
        //        IsCustomFilter = false,
        //        Operator = "Contains",
        //        HelpTextDefaultText = "Searching by :\n1: code\n2: name",
        //        HelpTextCode = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectfields, textcodes);


        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteSalesTotalFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Amount",
        //        FullFieldLable = "Amount",
        //        FieldName = "Amount",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSalesTotalObject.Id,
        //        ObjectTableName = QuoteSalesTotalObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "Amount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Currency",
        //        FullFieldLable = "CurrencyCode",
        //        FieldName = "CurrencyCode",
        //        FieldsDataType = "Text",
        //        MaxLength = 3,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSalesTotalObject.Id,
        //        ObjectTableName = QuoteSalesTotalObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CurrencyCode",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuotePackageFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Dimensions (L-W-H) (%UnitCode)",
        //        FullFieldLable = "Dimensions",
        //        FieldName = "Dimensions",
        //        FieldsDataType = "Text",
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        DisplayInList = true,
        //        ListFieldLable = "DimensionsListLable",
        //        ListLableDefaultText = "Dimensions (L-W-H)",
        //        ListPropertyPath = "Dimensions",
        //        PMPropertyPath = "Dimensions"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type",
        //        FullFieldLable = "PackageTypeId",
        //        FieldName = "PackageTypeId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = PackageTypesObject.Id,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListPropertyPath = "PackageTypeId",
        //        PMPropertyPath = "PackageTypeId",
        //        CanFilter = true,
        //        Operator = "Equals",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Package Type",
        //        FullFieldLable = "PackageTypeName",
        //        FieldName = "PackageTypeName",
        //        FieldsDataType = "Text",
        //        MaxLength = 250,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "PackageTypeNameListLable",
        //        ListLableDefaultText = "Package Type",
        //        ListPropertyPath = "PackageTypeName",
        //        PMPropertyPath = "PackageTypeName",
        //        CanFilter = true,
        //        Operator = "StartsWith"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Pieces",
        //        FullFieldLable = "Quantity",
        //        FieldName = "Quantity",
        //        FieldsDataType = "Integer",
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList=true,
        //        ListFieldLable = "QuantityListLable",
        //        ListLableDefaultText = "Quantity",
        //        ListPropertyPath = "Quantity",
        //        PMPropertyPath = "Quantity",
        //        CanFilter = true,
        //        Operator = "Equals",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Gross Weight (%WeightCode)",
        //        FullFieldLable = "GrossWeight",
        //        FieldName = "GrossWeight",
        //        FieldsDataType = "Double",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "GrossWeightListLable",
        //        ListLableDefaultText = "Gross Weight",
        //        ListPropertyPath = "GrossWeight",
        //        PMPropertyPath = "GrossWeight",
        //        CanFilter = true,
        //        Operator = "Equals"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Volume (%VolumeCode)",
        //        FullFieldLable = "Volume",
        //        FieldName = "Volume",
        //        FieldsDataType = "Double",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "VolumeListLable",
        //        ListLableDefaultText = "Volume",
        //        ListPropertyPath = "Volume",
        //        PMPropertyPath = "Volume",
        //        CanFilter = true,
        //        Operator = "Equals"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Height",
        //        FullFieldLable = "Height",
        //        FieldName = "Height",
        //        FieldsDataType = "Double",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "HeightListLable",
        //        ListLableDefaultText = "Height",
        //        ListPropertyPath = "Height",
        //        PMPropertyPath = "Height",
        //        CanFilter = true,
        //        Operator = "Equals"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Width",
        //        FullFieldLable = "Width",
        //        FieldName = "Width",
        //        FieldsDataType = "Double",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "WidthListLable",
        //        ListLableDefaultText = "Width",
        //        ListPropertyPath = "Width",
        //        PMPropertyPath = "Width",
        //        CanFilter = true,
        //        Operator = "Equals"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Length",
        //        FullFieldLable = "Length",
        //        FieldName = "Length",
        //        FieldsDataType = "Double",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "LengthListLable",
        //        ListLableDefaultText = "Length",
        //        ListPropertyPath = "Length",
        //        PMPropertyPath = "Length",
        //        CanFilter = true,
        //        Operator = "Equals"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Volumetric Weight (%WeightCode)",
        //        FullFieldLable = "VolumetricWeight",
        //        FieldName = "VolumetricWeight",
        //        FieldsDataType = "Double",
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuotePackageObject.Id,
        //        ObjectTableName = QuotePackageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "VolumetricWeightListLable",
        //        ListLableDefaultText = "Volumetric Weight",
        //        ListPropertyPath = "VolumetricWeight",
        //        PMPropertyPath = "VolumetricWeight",
        //        CanFilter = true,
        //        Operator = "Equals"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteTemplateFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "HeaderDocId",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "HeaderDocId",
        //        FieldName = "HeaderDocId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "HeaderDocListLable",
        //        ListLableDefaultText = "HeaderDocId",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "HeaderDocId",
        //        PMPropertyPath = "HeaderDocId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);



        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Template Type Code",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "TemplateTypeCode",
        //        FieldName = "TemplateTypeCode",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "TemplateTypeCodeListLable",
        //        ListLableDefaultText = "TemplateTypeCode",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ListPropertyPath = "TemplateTypeCode",
        //        PMPropertyPath = "TemplateTypeCode"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);





        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search..",
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "Text",
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Contains",
        //        PMPropertyPath = "SearchFields",
        //        ListPropertyPath = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "FooterDocId",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "FooterDocId",
        //        FieldName = "FooterDocId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "FooterDocIdListLable",
        //        ListLableDefaultText = "FooterDocId",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "FooterDocId",
        //        PMPropertyPath = "FooterDocId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "QuoteTemplateSettingId",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "QuoteTemplateSettingId",
        //        FieldName = "QuoteTemplateSettingId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "QuoteTemplateSettingIdListLable",
        //        ListLableDefaultText = "QuoteTemplateSettingId",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "QuoteTemplateSettingId",
        //        PMPropertyPath = "QuoteTemplateSettingId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        DisplayOnLookUp = true,
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
              
        //        TextCodeType = "F",
        //        ListFieldLable = "NameListLable",
        //        ListLableDefaultText = "Name",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "QuoteTemplate",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Template Type",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "TemplateTypeName",
        //        FieldName = "TemplateTypeName",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "TemplateTypeNameListLable",
        //        ListLableDefaultText = "Template Type",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ListPropertyPath = "TemplateTypeName",
        //        PMPropertyPath = "TemplateTypeName",
        //        ValidForQuerySection1 = "QuoteTemplate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Default",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "IsDefault",
        //        FieldName = "IsDefault",
        //        FieldsDataType = "Boolean",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 100,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsDefaultListLable",
        //        ListLableDefaultText = "Is Default",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ListPropertyPath = "IsDefault",
        //        PMPropertyPath = "IsDefault",
        //        ValidForQuerySection1 = "QuoteTemplate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Show Local Language",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "ShowLocalLanguage",
        //        FieldName = "ShowLocalLanguage",
        //        FieldsDataType = "Boolean",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,

        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "ShowLocalLanguageListLable",
        //        ListLableDefaultText = "Show Local Language",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ListPropertyPath = "ShowLocalLanguage",
        //        PMPropertyPath = "ShowLocalLanguage",
        //        ValidForQuerySection1 = "QuoteTemplate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Is Template",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "IsTemplate",
        //        FieldName = "IsTemplate",
        //        FieldsDataType = "Boolean",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "IsTemplate",
        //        PMPropertyPath = "IsTemplate"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);



        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Original Quote Template Id",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "OriginalQuoteTemplateId",
        //        FieldName = "OriginalQuoteTemplateId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "OriginalQuoteTemplateId",
        //        PMPropertyPath = "OriginalQuoteTemplateId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);



        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Create Date",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "CreateDate",
        //        FieldName = "CreateDate",
        //        FieldsDataType = "DateTime",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "CreateDate",
        //        PMPropertyPath = "CreateDate"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Update Date",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "UpdateDate",
        //        FieldName = "UpdateDate",
        //        FieldsDataType = "DateTime",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "UpdateDate",
        //        PMPropertyPath = "UpdateDate"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Created By UserId",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "CreatedByUserId",
        //        FieldName = "CreatedByUserId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "CreatedByUserId",
        //        PMPropertyPath = "CreatedByUserId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Updated By UserId",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "UpdatedByUserId",
        //        FieldName = "UpdatedByUserId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "UpdatedByUserId",
        //        PMPropertyPath = "UpdatedByUserId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);







        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "In Active",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "InActive",
        //        FieldName = "InActive",
        //        FieldsDataType = "Boolean",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = false,
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "InActiveListLable",
        //        ListLableDefaultText = "In Active",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ListPropertyPath = "Inactive",
        //        PMPropertyPath = "Inactive",
        //        ValidForQuerySection1 = "QuoteTemplate",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);





        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Enabled for Customers",
        //        DisplayOnLookUp = false,
        //        IsRequired = false,
        //        FullFieldLable = "IsEnabledForCustomers",
        //        FieldName = "IsEnabledForCustomers",
        //        FieldsDataType = "Boolean",

        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsEnabledForCustomersListLable",
        //        ListLableDefaultText = "IsEnabledForCustomers",
        //        DisplayInList = true,
        //        MultiLine = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "IsEnabledForCustomers",
        //        PMPropertyPath = "IsEnabledForCustomers",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Copy at Signup",
        //        DisplayOnLookUp = false,
        //        IsRequired = false,
        //        FullFieldLable = "IsCopiedAtSignup",
        //        FieldName = "IsCopiedAtSignup",
        //        FieldsDataType = "Boolean",

        //        ObjectTableId = QuoteTemplateObject.Id,
        //        ObjectTableName = QuoteTemplateObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "IsCopiedAtSignupListLable",
        //        ListLableDefaultText = "IsCopiedAtSignup",
        //        DisplayInList = true,
        //        MultiLine = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "IsCopiedAtSignup",
        //        PMPropertyPath = "IsCopiedAtSignup",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteTemplateSectionFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Template Id",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "QuoteTemplateId",
        //        FieldName = "QuoteTemplateId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateSectionObject.Id,
        //        ObjectTableName = QuoteTemplateSectionObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = true,

        //        ListPropertyPath = "QuoteTemplateId",
        //        PMPropertyPath = "QuoteTemplateId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Section DocId",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SectionDocId",
        //        FieldName = "SectionDocId",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateSectionObject.Id,
        //        ObjectTableName = QuoteTemplateSectionObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = true,
        //        ListPropertyPath = "SectionDocId",
        //        PMPropertyPath = "SectionDocId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Order",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "Order",
        //        FieldName = "Order",
        //        FieldsDataType = "Integer",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,

        //        ObjectTableId = QuoteTemplateSectionObject.Id,
        //        ObjectTableName = QuoteTemplateSectionObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = true,
        //        ListPropertyPath = "Order",
        //        PMPropertyPath = "Order"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Template Section Type Code",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "QuoteTemplateSectionTypeCode",
        //        FieldName = "QuoteTemplateSectionTypeCode",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 2,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateSectionObject.Id,
        //        ObjectTableName = QuoteTemplateSectionObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = true,
        //        ListPropertyPath = "QuoteTemplateSectionTypeCode",
        //        PMPropertyPath = "QuoteTemplateSectionTypeCode"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteTemplateSectionTypeFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateSectionTypeObject.Id,
        //        ObjectTableName = QuoteTemplateSectionTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateSectionTypeObject.Id,
        //        ObjectTableName = QuoteTemplateSectionTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteTemplateSettingFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Display Sales Currency",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "DisplaySalesCurrency",
        //        FieldName = "DisplaySalesCurrency",
        //        FieldsDataType = "Boolean",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,

        //        ObjectTableId = QuoteTemplateSettingObject.Id,
        //        ObjectTableName = QuoteTemplateSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "DisplaySalesCurrency",
        //        PMPropertyPath = "DisplaySalesCurrency"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Column Header Foreground",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "ColumnHeaderForeground",
        //        FieldName = "ColumnHeaderForeground",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        Isoveridden = false,
        //        IsRequired = true,
        //        MaxLength = 10,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTemplateSettingObject.Id,
        //        ObjectTableName = QuoteTemplateSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = false,
        //        CanFilter = false,
        //        ListPropertyPath = "ColumnHeaderForeground",
        //        PMPropertyPath = "ColumnHeaderForeground"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);
        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteClosingReasonFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 2,
        //        MinLength = 0,
        //        ObjectTableId = QuoteClosingReasonObject.Id,
        //        ObjectTableName = QuoteClosingReasonObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        IsRequired = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        FieldsDataType = "Text",
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteClosingReasonObject.Id,
        //        ObjectTableName = QuoteClosingReasonObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name",
        //        IsRequired = true,
        //        DisplayOnLookUp = true,
        //        CanFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search..",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        IsRequired = false,
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteClosingReasonObject.Id,
        //        ObjectTableName = "QuoteClosingReason",
        //        ObjectTablePlural = "Quote Closing Reasons",
        //        ObjectTableSingular = "Quote Closing Reason",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        SystemMaxLength = 40,
        //        SystemRequired = false,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "QuoteClosingReason",
        //        DisplayInList = false,
        //        IsCustomFilter = false,
        //        Operator = "Contains",
        //        HelpTextDefaultText = "Searching by :\n1: code\n2: name",
        //        HelpTextCode = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteStageFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Inactive",
        //        FullFieldLable = "InActive",
        //        FieldName = "InActive",
        //        FieldsDataType = "Boolean",
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        ListFieldLable = "InActiveListLable",
        //        ListLableDefaultText = "Inactive",
        //        DisplayInList = true,
        //        CanFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "InActive",
        //        PMPropertyPath = "InActive",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        IsRequired = true,
        //        CanFilter = true,
        //        DisplayInList = true,
        //        Operator = "StartsWith",
        //        ListFieldLable = "CodeLabel",
        //        ListLableDefaultText = "Code",
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        DisplayOnLookUp = true,
        //        DisplayInLookUpIndex = 1,
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        IsRequired = true,
        //        CanFilter = true,
        //        DisplayInList = true,
        //        Operator = "StartsWith",
        //        ListFieldLable = "NameLabel",
        //        ListLableDefaultText = "Name",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Max days",
        //        FullFieldLable = "MaxDays",
        //        FieldName = "MaxDays",
        //        FieldsDataType = "Integer",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        Operator = "Equals",
        //        ListFieldLable = "MaxDaysLabel",
        //        ListLableDefaultText = "Max days",
        //        ListPropertyPath = "MaxDays",
        //        PMPropertyPath = "MaxDays"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Update Date",
        //        FullFieldLable = "UpdateDate",
        //        FieldName = "UpdateDate",
        //        FieldsDataType = "DateTime",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        Operator = "Equals",
        //        DisplayInList = true,
        //        ListFieldLable = "UpdateDateLabel",
        //        ListLableDefaultText = "Update Date",
        //        ListPropertyPath = "UpdateDate",
        //        PMPropertyPath = "UpdateDate"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Updated By",
        //        FullFieldLable = "UpdatedByUserId",
        //        FieldName = "UpdatedByUserId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = UsersObject.Id,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        Operator = "Equals",
        //        ListPropertyPath = "UpdatedByUserId",
        //        PMPropertyPath = "UpdatedByUserId"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Updated By User",
        //        FullFieldLable = "UpdatedByUserName",
        //        FieldName = "UpdatedByUserName",
        //        FieldsDataType = "Text",
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        ListFieldLable = "UpdatedByUserNameListLable",
        //        ListLableDefaultText = "Updated By User",
        //        Operator = "Equals",
        //        ListPropertyPath = "UpdatedByUserName",
        //        PMPropertyPath = "UpdatedByUserName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search code, name",
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "nText",
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Contains",
        //        PMPropertyPath = "SearchFields",
        //        ListPropertyPath = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Rank",
        //        FullFieldLable = "Rank",
        //        FieldName = "Rank",
        //        FieldsDataType = "Integer",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteStageObject.Id,
        //        ObjectTableName = QuoteStageObject.Name,
        //        ValidForQuerySection1 = QuoteStageObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayInList = true,
        //        Operator = "Equals",
        //        ListFieldLable = "RankLabel",
        //        ListLableDefaultText = "Rank",
        //        ListPropertyPath = "Rank",
        //        PMPropertyPath = "Rank"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteRatingFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteRatingObject.Id,
        //        ObjectTableName = QuoteRatingObject.Name,
        //        ValidForQuerySection1 = QuoteRatingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        IsRequired = true,
        //        CanFilter = true,
        //        DisplayInList = true,
        //        Operator = "StartsWith",
        //        ListFieldLable = "CodeLabel",
        //        ListLableDefaultText = "Code",
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        DisplayOnLookUp = true,
        //        DisplayInLookUpIndex = 1,
        //        FieldsDataType = "nText",
        //        MaxLength = 60,
        //        MinLength = 0,
        //        ObjectTableId = QuoteRatingObject.Id,
        //        ObjectTableName = QuoteRatingObject.Name,
        //        ValidForQuerySection1 = QuoteRatingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        IsRequired = true,
        //        CanFilter = true,
        //        DisplayInList = true,
        //        Operator = "StartsWith",
        //        ListFieldLable = "NameLabel",
        //        ListLableDefaultText = "Name",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Index Order",
        //        FullFieldLable = "IndexOrder",
        //        FieldName = "IndexOrder",
        //        FieldsDataType = "Integer",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteRatingObject.Id,
        //        ObjectTableName = QuoteRatingObject.Name,
        //        ValidForQuerySection1 = QuoteRatingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        CanFilter = true,
        //        DisplayInList = true,
        //        Operator = "Equals",
        //        ListFieldLable = "IndexOrderLabel",
        //        ListLableDefaultText = "Index Order",
        //        ListPropertyPath = "IndexOrder",
        //        PMPropertyPath = "IndexOrder"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search code, name",
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "nText",
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = QuoteRatingObject.Id,
        //        ObjectTableName = QuoteRatingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Contains",
        //        PMPropertyPath = "SearchFields",
        //        ListPropertyPath = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateMarkUpTypeFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{



        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = MarkUpTypeObject.Id,
        //        ObjectTableName = MarkUpTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        IsRequired = true,
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = MarkUpTypeObject.Id,
        //        ObjectTableName = MarkUpTypeObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name",
        //        DisplayOnLookUp = true,
        //        CanFilter = true,
        //        IsRequired = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search..",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "Text",
        //        IsCustom = false,
        //        IsRequired = false,
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = MarkUpTypeObject.Id,
        //        ObjectTableName = "MarkUpType",
        //        ObjectTablePlural = "MarkUpTypes",
        //        ObjectTableSingular = "MarkUpType",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        SystemMaxLength = 40,
        //        SystemRequired = false,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "MarkUpType",
        //        ValidForQuerySection2 = "MarkUpTypeFollowUp",
        //        DisplayInList = false,
        //        IsCustomFilter = false,
        //        Operator = "Contains",
        //        HelpTextDefaultText = "Searching by :\n1: code\n2: name",
        //        HelpTextCode = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateFixedAmountTypeFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{


        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Code",
        //        FullFieldLable = "Code",
        //        FieldName = "Code",
        //        FieldsDataType = "Text",
        //        MaxLength = 4,
        //        MinLength = 0,
        //        ObjectTableId = FixedAmountObject.Id,
        //        ObjectTableName = FixedAmountObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Code",
        //        PMPropertyPath = "Code"
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Name",
        //        FullFieldLable = "Name",
        //        FieldName = "Name",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = FixedAmountObject.Id,
        //        ObjectTableName = FixedAmountObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "Name",
        //        PMPropertyPath = "Name",
        //        DisplayOnLookUp = true,
        //        CanFilter = true,
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteTotalVatsFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Currency VAT Amount",
        //        FullFieldLable = "QuoteCurrencyVATAmount",
        //        FieldName = "QuoteCurrencyVATAmount",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 1,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "QuoteCurrencyVATAmount",
        //        PMPropertyPath = "QuoteCurrencyVATAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Currency Vatable Amount",
        //        FullFieldLable = "QuoteCurrencyVatableAmount",
        //        FieldName = "QuoteCurrencyVatableAmount",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 1,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "QuoteCurrencyVatableAmount",
        //        PMPropertyPath = "QuoteCurrencyVatableAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Local Currency VAT Amount",
        //        FullFieldLable = "LocalCurrencyVATAmount",
        //        FieldName = "LocalCurrencyVATAmount",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 1,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "LocalCurrencyVATAmount",
        //        PMPropertyPath = "LocalCurrencyVATAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Local Currency Vatable Amount",
        //        FullFieldLable = "LocalCurrencyVatableAmount",
        //        FieldName = "LocalCurrencyVatableAmount",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 1,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "LocalCurrencyVatableAmount",
        //        PMPropertyPath = "LocalCurrencyVatableAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Profit Currency VAT Amount",
        //        FullFieldLable = "ProfitCurrencyVATAmount",
        //        FieldName = "ProfitCurrencyVATAmount",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 1,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "ProfitCurrencyVATAmount",
        //        PMPropertyPath = "ProfitCurrencyVATAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Profit Currency Vatable Amount",
        //        FullFieldLable = "ProfitCurrencyVatableAmount",
        //        FieldName = "ProfitCurrencyVatableAmount",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 1,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "ProfitCurrencyVatableAmount",
        //        PMPropertyPath = "ProfitCurrencyVatableAmount",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Percent",
        //        FullFieldLable = "VatPercent",
        //        FieldName = "VatPercent",
        //        FieldsDataType = "Double",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 1,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatPercent",
        //        PMPropertyPath = "VatPercent",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "External VAT Card",
        //        FullFieldLable = "ExternalVATCard",
        //        FieldName = "ExternalVATCard",
        //        FieldsDataType = "Text",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 25,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "ExternalVATCard",
        //        PMPropertyPath = "ExternalVATCard",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "External TAX Item Id",
        //        FullFieldLable = "ExternalTAXItemId",
        //        FieldName = "ExternalTAXItemId",
        //        FieldsDataType = "Text",
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        MaxLength = 25,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "ExternalTAXItemId",
        //        PMPropertyPath = "ExternalTAXItemId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeId",
        //        FieldName = "VatTypeId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = VatTypesObject.Id,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = QuoteTotalVatObject.Id,
        //        ObjectTableName = QuoteTotalVatObject.Name,
        //        ValidForQuerySection1 = QuoteTotalVatObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatTypeId",
        //        PMPropertyPath = "VatTypeId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote",
        //        FullFieldLable = "QuoteId",
        //        FieldName = "QuoteId",
        //        FieldsDataType = "LookUp",
        //        LookUpTableId = QuoteObject.Id,
        //        IsRequired = true,
        //        MaxLength = 15,
        //        MinLength = 0,
        //        ObjectTableId = APInvoiceTotalVatObject.Id,
        //        ObjectTableName = APInvoiceTotalVatObject.Name,
        //        ValidForQuerySection1 = APInvoiceTotalVatObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "QuoteId",
        //        PMPropertyPath = "QuoteId",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteVATsTotalFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Percentage",
        //        FullFieldLable = "VatPercentage",
        //        FieldName = "VatPercentage",
        //        FieldsDataType = "SigDouble",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteVATsTotalObject.Id,
        //        ObjectTableName = QuoteVATsTotalObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        ListPropertyPath = "VatPercentage",
        //        PMPropertyPath = "VatPercentage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "VAT Type",
        //        FullFieldLable = "VatTypeName",
        //        FieldName = "VatTypeName",
        //        FieldsDataType = "Text",
        //        MaxLength = 40,
        //        MinLength = 0,
        //        ObjectTableId = QuoteVATsTotalObject.Id,
        //        ObjectTableName = QuoteVATsTotalObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "StartsWith",
        //        ListPropertyPath = "VatTypeName",
        //        PMPropertyPath = "VatTypeName",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Amount in Sale Currency",
        //        FullFieldLable = "AmountInSaleCurrency",
        //        FieldName = "AmountInSaleCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteVATsTotalObject.Id,
        //        ObjectTableName = QuoteVATsTotalObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "AmountInSaleCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Amount in Local Currency",
        //        FullFieldLable = "AmountInLocalCurrency",
        //        FieldName = "AmountInLocalCurrency",
        //        FieldsDataType = "Double",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteVATsTotalObject.Id,
        //        ObjectTableName = QuoteVATsTotalObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "AmountInLocalCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
        //private void CreateQuoteSettingFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Agent",
        //        FullFieldLable = "CopyAgent",
        //        FieldName = "CopyAgent",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyAgent",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Notify",
        //        FullFieldLable = "CopyNotify",
        //        FieldName = "CopyNotify",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyNotify",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Shipper",
        //        FullFieldLable = "CopyShipper",
        //        FieldName = "CopyShipper",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyShipper",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Consignee",
        //        FullFieldLable = "CopyConsignee",
        //        FieldName = "CopyConsignee",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyConsignee",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Main Carriage",
        //        FullFieldLable = "CopyMainCarriage",
        //        FieldName = "CopyMainCarriage",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyMainCarriage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Include Pickup",
        //        FullFieldLable = "CopyPickup",
        //        FieldName = "CopyPickup",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyPickup",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Include Delivery",
        //        FullFieldLable = "CopyDelivery",
        //        FieldName = "CopyDelivery",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyDelivery",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Charges Types",
        //        FullFieldLable = "CopyChargesTypes",
        //        FieldName = "CopyChargesTypes",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyChargesTypes",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Cost",
        //        FullFieldLable = "CopyChargesCost",
        //        FieldName = "CopyChargesCost",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyChargesCost",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Sale",
        //        FullFieldLable = "CopyChargesSale",
        //        FieldName = "CopyChargesSale",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "CopyChargesSale",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Edit Main Carriage",
        //        FullFieldLable = "EditMainCarriage",
        //        FieldName = "EditMainCarriage",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "EditMainCarriage",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Quote Sale Currency Default",
        //        FullFieldLable = "IsSaleAsCostCurrency",
        //        FieldName = "IsSaleAsCostCurrency",
        //        FieldsDataType = "Boolean",
        //        MaxLength = 1,
        //        MinLength = 0,
        //        ObjectTableId = QuoteSettingObject.Id,
        //        ObjectTableName = QuoteSettingObject.Name,
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        Operator = "Equals",
        //        PMPropertyPath = "IsSaleAsCostCurrency",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();
        //}
    }
}