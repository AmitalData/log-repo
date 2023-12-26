using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Helpers;
using UnifreightIIG.Common.SystemTableServiceReference;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Logitude.SystemLogs;
using System.Data;
using System.Xml.Linq;
using Logitude.Server.Tools;
using Logitude.AmitalMessaging.Utils;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Helpers.ClosedTable;
using Logitude.Customs.Data.EntityMapping;
using Logitude.BL.Helpers;

namespace Logitude.CustomsMessaging.Helpers
{
    public class LoadCustomClosedTables
    {
        public static void FillCustomClosedTablesData(int? tenant)
        {

            InitializeSettings();
            ICustomContext customContext = CustomContext.GetContext(0);

            var myMehesSystemTables = new SystemTables();


            //Logitude.CustomsMessaging.Helpers.SystemTables systemTables = new Logitude.CustomsMessaging.Helpers.SystemTables();
            //CustomDocumentTypeMetaDataRepository customDocumentTypeMetaDataRepository = new CustomDocumentTypeMetaDataRepository(customContext);
            //List<CustomDocumentTypeMetaData> customDocumentTypeMetaDataList = customDocumentTypeMetaDataRepository.GetAll().ToList();
            //var metadataData = systemTables.GetAsTableData(TableID: "1892", pageNumber: 1, pageSize: 50);
            //metadataData.WriteXml(@"C:\table1892AsTableData.xml");

            //CustomMetaDataTypeRepository CustomMetaDataTypeRep = new CustomMetaDataTypeRepository(customContext);
            //var CustomMetaDataTypeSystemTables = myMehesSystemTables.GetTableData("1892");
          
            //XElement xmlElements = new XElement("CustomMetaDataTypeSystemTables", CustomMetaDataTypeSystemTables.Select(i => new XElement("SYSTBL_NG_9001_MSG_SystemTablesResponseTableData", i)));
            //xmlElements.Save(@"C:\table1892.xml");
            //if (metadataData != null)
            //{
            //    var content = metadataData.Tables[0];
            //    foreach (DataRow row in content.Rows)
            //    {
            //        CustomDocumentTypeMetaData metadataRecord = (from a in customDocumentTypeMetaDataList
            //                                                     where a.MetaDataTypeCode == row.ItemArray[8].ToString() && a.DocumentTypeCode == row.ItemArray[0].ToString()
            //                                                     select a).FirstOrDefault();
            //        if (metadataRecord == null)
            //        {
            //            metadataRecord = new CustomDocumentTypeMetaData()
            //            {
            //                DocumentTypeCode = row.ItemArray[0].ToString(),
            //                MetaDataTypeCode = row.ItemArray[8].ToString(),
            //                Format = row.ItemArray[4].ToString(),
            //                Mandatory = bool.Parse(row.ItemArray[6].ToString()),
            //            };
            //            customDocumentTypeMetaDataRepository.Add(metadataRecord);
            //            customDocumentTypeMetaDataList.Add(metadataRecord);
            //        }
            //        else
            //        {
            //            metadataRecord.Format = row.ItemArray[4].ToString();
            //            metadataRecord.Mandatory = bool.Parse(row.ItemArray[6].ToString());
            //            customDocumentTypeMetaDataRepository.Update(metadataRecord);
            //        }
            //        customDocumentTypeMetaDataRepository.SubmitChanges();
            //    }
            //    try
            //    {
            //        customDocumentTypeMetaDataRepository.SubmitChanges();
            //    }
            //    catch (System.Exception ex)
            //    {

            //    }
            //}

            //return;

            //InternationalSiteRepository internationalSiteRepository = new InternationalSiteRepository(customContext);
            //List<InternationalSite> internationalSiteList = internationalSiteRepository.GetAll().ToList();
            //if (internationalSiteList.Count == 0)
            //{
            //    InternationalSite Site = new InternationalSite() { Code = "IT", EnglishName = "Italy", LocalName = "איטליה", SearchFields = "IT", };

            //    internationalSiteRepository.Add(Site);
            //    internationalSiteRepository.SubmitChanges();
            //}

            ConstraintProcessTypeRepository constraintProcessTypeRepository = new ConstraintProcessTypeRepository(customContext);
            List<ConstraintProcessType> constraintProcessTypeList = constraintProcessTypeRepository.GetAll().ToList();
            if (constraintProcessTypeList.Count == 0)
            {
                ConstraintProcessType constraintProcessType1 = new ConstraintProcessType() { Code = "1", LocalName = "אילוץ הגשה", SearchFields = ("1,אילוץ הגשה").ToLower(), };
                ConstraintProcessType constraintProcessType2 = new ConstraintProcessType() { Code = "2", LocalName = "אילוץ התרה", SearchFields = ("2,אילוץ התרה").ToLower(), };

                constraintProcessTypeRepository.Add(constraintProcessType1);
                constraintProcessTypeRepository.Add(constraintProcessType2);
                constraintProcessTypeRepository.SubmitChanges();
            }

            PayerActivityTypeRepository payerActivityTypeRepository = new PayerActivityTypeRepository(customContext);
            List<PayerActivityType> payerActivityTypeList = payerActivityTypeRepository.GetAll().ToList();
            if (payerActivityTypeList.Count == 0)
            {
                PayerActivityType payerActivityType1 = new PayerActivityType() { Code = "1", LocalName = "סוכן", SearchFields = "1", };
                PayerActivityType payerActivityType2 = new PayerActivityType() { Code = "4", LocalName = "יבואן", SearchFields = "4", };


                payerActivityTypeRepository.Add(payerActivityType1);
                payerActivityTypeRepository.Add(payerActivityType2);
                payerActivityTypeRepository.SubmitChanges();
            }



            MeasureQualifierRepository measureQualifierRepository = new MeasureQualifierRepository(customContext);
            List<MeasureQualifier> measureQualifierList = measureQualifierRepository.GetAll().ToList();
            if (measureQualifierList.Count == 0)
            {
                MeasureQualifier measureQualifier1 = new MeasureQualifier() { Code = "1", LocalName = " כמות יחידות בחשבון", SearchFields = "1", };
                MeasureQualifier measureQualifier2 = new MeasureQualifier() { Code = "2", LocalName = "כמות סטטיסטית", SearchFields = "2", };
                MeasureQualifier measureQualifier3 = new MeasureQualifier() { Code = "3", LocalName = "כמות נוספת", SearchFields = "3", };


                measureQualifierRepository.Add(measureQualifier1);
                measureQualifierRepository.Add(measureQualifier2);
                measureQualifierRepository.Add(measureQualifier3);

                measureQualifierRepository.SubmitChanges();
            }


            CustomsDocumentStatusTypeRepository customsDocumentStatusTypeRepository = new CustomsDocumentStatusTypeRepository(customContext);
            List<CustomsDocumentStatusType> customsDocumentStatusTypeList = customsDocumentStatusTypeRepository.GetAll().ToList();
            var myInVerificationProccess = customsDocumentStatusTypeList.FirstOrDefault(rec => rec.Code == "8");// itzik  Not found in DSV
                                                                                                                
            /*
             * i add this script  instead  - due logitude.update - go to azure - and long time not used correctly !
INSERT INTO   CustomsDocumentStatusTypes (     CODE, ENGLISHNAME, LOCALNAME,SEARCHFIELDS,INACTIVE )  SELECT     '8', 'In Verification Process', 'בתהליך אימות','8',0  FROM DUAL WHERE NOT EXISTS (     SELECT 1      FROM CustomsDocumentStatusTypes      WHERE CODE = '8'  ); 
                                                                                                    /
                                                                                                    commit ;
                                                                                                    /             

*/

            if (customsDocumentStatusTypeList.Count == 0)
            {
                CustomsDocumentStatusType CustomsDocumentStatusType1 = new CustomsDocumentStatusType() { Code = "1", LocalName = "נשלח", EnglishName = "Sent", SearchFields = "1", };
                CustomsDocumentStatusType CustomsDocumentStatusType2 = new CustomsDocumentStatusType() { Code = "2", LocalName = "נכשל", EnglishName = "Fail", SearchFields = "2", };
                CustomsDocumentStatusType CustomsDocumentStatusType3 = new CustomsDocumentStatusType() { Code = "3", LocalName = "נדרש", EnglishName = "Needed", SearchFields = "3", };
                CustomsDocumentStatusType CustomsDocumentStatusType4 = new CustomsDocumentStatusType() { Code = "4", LocalName = "אומת", EnglishName = "Verified", SearchFields = "4", };
                CustomsDocumentStatusType CustomsDocumentStatusType5 = new CustomsDocumentStatusType() { Code = "5", LocalName = "אומת בנוכחות הלקוח", EnglishName = "Verified With Customer Presents", SearchFields = "5", };
                CustomsDocumentStatusType CustomsDocumentStatusType6 = new CustomsDocumentStatusType() { Code = "6", LocalName = "נדחה אימות", EnglishName = "Verify Rejected", SearchFields = "6", };
                CustomsDocumentStatusType CustomsDocumentStatusType7 = new CustomsDocumentStatusType() { Code = "7", LocalName = "נשלח ללא תשובה", EnglishName = "Sent Without Answer", SearchFields = "7", };

                CustomsDocumentStatusType CustomsDocumentStatusType8 = GetInVerificationProccess();

                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType1);
                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType2);
                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType3);
                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType4);
                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType5);
                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType6);
                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType7);
                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType8);

                customsDocumentStatusTypeRepository.SubmitChanges();
            }
            else if (myInVerificationProccess == null)
            {

                CustomsDocumentStatusType CustomsDocumentStatusType8 = GetInVerificationProccess();

                customsDocumentStatusTypeRepository.Add(CustomsDocumentStatusType8);

                customsDocumentStatusTypeRepository.SubmitChanges();
            }


            CustomerRoleTypeRepository customerRoleTypeRepository = new CustomerRoleTypeRepository(customContext);
            List<CustomerRoleType> customerRoleTypeList = customerRoleTypeRepository.GetAll().ToList();
            if (customerRoleTypeList.Count == 0)
            {
                CustomerRoleType customerRoleType1 = new CustomerRoleType() { Code = "1", LocalName = "סוכן", SearchFields = "1", };
                CustomerRoleType customerRoleType2 = new CustomerRoleType() { Code = "4", LocalName = "יבואן", SearchFields = "4", };


                customerRoleTypeRepository.Add(customerRoleType1);
                customerRoleTypeRepository.Add(customerRoleType2);
                customerRoleTypeRepository.SubmitChanges();
            }







           

        }

        private static CustomsDocumentStatusType GetInVerificationProccess() // itzik  Not found in DSV
        {
            return new CustomsDocumentStatusType() { Code = "8", LocalName = "בתהליך אימות ", EnglishName = "In Verification Proccess", SearchFields = "8", };
        }

        public static void FillCustomsClosedTablesInDb(int tenant, ClientProgressBarIndicatorService clientProgressBarIndicatorService=null)
        {
            InitializeSettings();
            ICustomContext customContext = CustomContext.GetContext(0);
            var myMehesSystemTables = new SystemTables();
            if (clientProgressBarIndicatorService != null) clientProgressBarIndicatorService.StartBroadcast("ממתין לתשובת המכס (סכמת טבלאות מכס)");
            var closedSystemTables = myMehesSystemTables.GetTableData("TableConfiguration", tenant);
            CustomsClosedTableRepository customsClosedTableRepository = new CustomsClosedTableRepository(customContext);
            Dictionary<string, CustomsClosedTable> customsClosedTables = customsClosedTableRepository.GetAll().ToDictionary(d => d.Id, t => t);
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> addedClosedTables = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>();
            if (clientProgressBarIndicatorService != null) clientProgressBarIndicatorService.StartBroadcast("בונה סכמת טבלאות מכס");
            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData leadDocumentExceptionTypeTable = closedSystemTables.Where(d => d.id == "1517").FirstOrDefault();
            ObjectTable leadDocumentExceptionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.LeadDocumentExceptionType", 0, false);
            InsertClosedTableRecord(leadDocumentExceptionTypeTable, leadDocumentExceptionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(leadDocumentExceptionTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData tradeAgreementTable = closedSystemTables.Where(d => d.id == "2009").FirstOrDefault();
            ObjectTable tradeAgreementObjectTable = objectTableRepository.GetObjectTableByName("Customs.TradeAgreement", 0, false);
            InsertClosedTableRecord(tradeAgreementTable, tradeAgreementObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(tradeAgreementTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData originCriterionTable = closedSystemTables.Where(d => d.id == "1977").FirstOrDefault();
            ObjectTable originCriterionObjectTable = objectTableRepository.GetObjectTableByName("Customs.OriginCriterion", 0, false);
            InsertClosedTableRecord(originCriterionTable, originCriterionObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(originCriterionTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData requestReasonCodeEnumTable = closedSystemTables.Where(d => d.id == "1960").FirstOrDefault();
            ObjectTable requestReasonCodeEnumObjectTable = objectTableRepository.GetObjectTableByName("Customs.RequestReasonCodeEnum", 0, false);
            InsertClosedTableRecord(requestReasonCodeEnumTable, requestReasonCodeEnumObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(requestReasonCodeEnumTable);
                                                              
            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData certificateOfOriginTypeCodeEnumTable = closedSystemTables.Where(d => d.id == "1958").FirstOrDefault();
            ObjectTable certificateOfOriginTypeCodeEnumObjectTable = objectTableRepository.GetObjectTableByName("Customs.CertificateOfOriginTypeCodeEnum", 0, false);
            InsertClosedTableRecord(certificateOfOriginTypeCodeEnumTable, certificateOfOriginTypeCodeEnumObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(certificateOfOriginTypeCodeEnumTable);  
            
            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData certificateOfOriginStatusCodeEnumTable = closedSystemTables.Where(d => d.id == "1957").FirstOrDefault();
            ObjectTable certificateOfOriginStatusCodeEnumObjectTable = objectTableRepository.GetObjectTableByName("Customs.CertificateOfOriginStatusCodeEnum", 0, false);
            InsertClosedTableRecord(certificateOfOriginStatusCodeEnumTable, certificateOfOriginStatusCodeEnumObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(certificateOfOriginStatusCodeEnumTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData deliverySiteTypeTable = closedSystemTables.Where(d => d.id == "2012").FirstOrDefault();
            ObjectTable deliverySiteTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DeliverySiteType", 0, false);
            InsertClosedTableRecord(deliverySiteTypeTable, deliverySiteTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(deliverySiteTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData internalBorderSiteTypeTable = closedSystemTables.Where(d => d.id == "2013").FirstOrDefault();
            ObjectTable internalBorderSiteTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.InternalBorderSiteType", 0, false);
            InsertClosedTableRecord(internalBorderSiteTypeTable, internalBorderSiteTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(internalBorderSiteTypeTable);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData registeredWarehouseSiteTypeTable = closedSystemTables.Where(d => d.id == "2014").FirstOrDefault();
            ObjectTable registeredWarehouseSiteTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.RegisteredWarehouseSiteType", 0, false);
            InsertClosedTableRecord(registeredWarehouseSiteTypeTable, registeredWarehouseSiteTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(registeredWarehouseSiteTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData constraintTypeTable = closedSystemTables.Where(d => d.id == "1574").FirstOrDefault();
            ObjectTable constraintTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ConstraintType", 0, false);
            InsertClosedTableRecord(constraintTypeTable, constraintTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(constraintTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData constraintStatusTable = closedSystemTables.Where(d => d.id == "1577").FirstOrDefault();
            ObjectTable constraintStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.ConstraintStatus", 0, false);
            InsertClosedTableRecord(constraintStatusTable, constraintStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(constraintStatusTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customMetaDataTypeTable = closedSystemTables.Where(d => d.id == "1892").FirstOrDefault();
            ObjectTable customMetaDataTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomMetaDataType", 0, false);
            InsertClosedTableRecord(customMetaDataTypeTable, customMetaDataTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customMetaDataTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customDocumentTypeTable = closedSystemTables.Where(d => d.id == "13").FirstOrDefault();
            ObjectTable customDocumentTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomDocumentType", 0, false);
            InsertClosedTableRecord(customDocumentTypeTable, customDocumentTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customDocumentTypeTable);

           
            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData entityTypeLookupTable = closedSystemTables.Where(d => d.id == "1183").FirstOrDefault();
            ObjectTable entityTypeLookupObjectTable = objectTableRepository.GetObjectTableByName("Customs.EntityTypeLookup", 0, false);
            InsertClosedTableRecord(entityTypeLookupTable, entityTypeLookupObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(entityTypeLookupTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData governmentProcedureTypeTable = closedSystemTables.Where(d => d.id == "1354").FirstOrDefault();
            ObjectTable governmentProcedureTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.GovernmentProcedureType", 0, false);
            InsertClosedTableRecord(governmentProcedureTypeTable, governmentProcedureTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(governmentProcedureTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData siteTypeTable = closedSystemTables.Where(d => d.id == "29").FirstOrDefault();
            ObjectTable siteTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SiteType", 0, false);
            InsertClosedTableRecord(siteTypeTable, siteTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(siteTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData siteLookupTable = closedSystemTables.Where(d => d.id == "1339").FirstOrDefault();
            ObjectTable siteLookupObjectTable = objectTableRepository.GetObjectTableByName("Customs.SiteLookup", 0, false);
            InsertClosedTableRecord(siteLookupTable, siteLookupObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(siteLookupTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData checkEntityTypeTable = closedSystemTables.Where(d => d.id == "1508").FirstOrDefault();
            ObjectTable checkEntityTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CheckEntityType", 0, false);
            InsertClosedTableRecord(checkEntityTypeTable, checkEntityTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(checkEntityTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData checkQueueTypeTable = closedSystemTables.Where(d => d.id == "1518").FirstOrDefault();
            ObjectTable checkQueueTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CheckQueueType", 0, false);
            InsertClosedTableRecord(checkQueueTypeTable, checkQueueTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(checkQueueTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData checkRepresentativeTypeTable = closedSystemTables.Where(d => d.id == "1569").FirstOrDefault();
            ObjectTable CheckRepresentativeTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CheckRepresentativeType", 0, false);
            InsertClosedTableRecord(checkRepresentativeTypeTable, CheckRepresentativeTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(checkRepresentativeTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData cargoIdentifireTypeTable = closedSystemTables.Where(d => d.id == "1259").FirstOrDefault();
            ObjectTable cargoIdentifireTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CargoIdentifireType", 0, false);
            InsertClosedTableRecord(cargoIdentifireTypeTable, cargoIdentifireTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(cargoIdentifireTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData autonomyTypeTable = closedSystemTables.Where(d => d.id == "1264").FirstOrDefault();
            ObjectTable autonomyTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AutonomyType", 0, false);
            InsertClosedTableRecord(autonomyTypeTable, autonomyTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(autonomyTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paragraphTypeTable = closedSystemTables.Where(d => d.id == "1120").FirstOrDefault();
            ObjectTable paragraphTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ParagraphType", 0, false);
            InsertClosedTableRecord(paragraphTypeTable, paragraphTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paragraphTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData measurmentUnitTable = closedSystemTables.Where(d => d.id == "1385").FirstOrDefault();
            ObjectTable measurmentUnitObjectTable = objectTableRepository.GetObjectTableByName("Customs.MeasurmentUnit", 0, false);
            InsertClosedTableRecord(measurmentUnitTable, measurmentUnitObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(measurmentUnitTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData attachmentTypeTable = closedSystemTables.Where(d => d.id == "1585").FirstOrDefault();
            ObjectTable attachmentTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AttachmentType", 0, false);
            InsertClosedTableRecord(attachmentTypeTable, attachmentTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(attachmentTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData certificateExemptionTypeTable = closedSystemTables.Where(d => d.id == "1423").FirstOrDefault();
            ObjectTable certificateExemptionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CertificateExemptionType", 0, false);
            InsertClosedTableRecord(certificateExemptionTypeTable, certificateExemptionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(certificateExemptionTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData confirmationTypeTable = closedSystemTables.Where(d => d.id == "1604").FirstOrDefault();
            ObjectTable confirmationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ConfirmationType", 0, false);
            InsertClosedTableRecord(confirmationTypeTable, confirmationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(confirmationTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData IncotemrsFileValidationTable = closedSystemTables.Where(d => d.id == "23928").FirstOrDefault();
            ObjectTable IncotemrsFileValidationObjectTable = objectTableRepository.GetObjectTableByName("Customs.IncotemrsFileValidation", 0, false);
            InsertClosedTableRecord(IncotemrsFileValidationTable, IncotemrsFileValidationObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(IncotemrsFileValidationTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData modificationAndDiscountTypeTable = closedSystemTables.Where(d => d.id == "1416").FirstOrDefault();
            ObjectTable modificationAndDiscountTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ModificationAndDiscountType", 0, false);
            InsertClosedTableRecord(modificationAndDiscountTypeTable, modificationAndDiscountTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(modificationAndDiscountTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData transportMeansTypeTable = closedSystemTables.Where(d => d.id == "1307").FirstOrDefault();
            ObjectTable transportMeansTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.TransportMeansType", 0, false);
            InsertClosedTableRecord(transportMeansTypeTable, transportMeansTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(transportMeansTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customerTypeGeneralTable = closedSystemTables.Where(d => d.id == "1294").FirstOrDefault();
            ObjectTable customerTypeGeneralObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomerTypeGeneral", 0, false);
            InsertClosedTableRecord(customerTypeGeneralTable, customerTypeGeneralObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customerTypeGeneralTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData genderTable = closedSystemTables.Where(d => d.id == "15").FirstOrDefault();
            ObjectTable genderObjectTable = objectTableRepository.GetObjectTableByName("Customs.Gender", 0, false);
            InsertClosedTableRecord(genderTable, genderObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(genderTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData passportTypeTable = closedSystemTables.Where(d => d.id == "1295").FirstOrDefault();
            ObjectTable passportTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.PassportType", 0, false);
            InsertClosedTableRecord(passportTypeTable, passportTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(passportTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData addressContactStateTable = closedSystemTables.Where(d => d.id == "1177").FirstOrDefault();
            ObjectTable addressContactStateObjectTable = objectTableRepository.GetObjectTableByName("Customs.AddressContactState", 0, false);
            InsertClosedTableRecord(addressContactStateTable, addressContactStateObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(addressContactStateTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsAddressTypeTable = closedSystemTables.Where(d => d.id == "88").FirstOrDefault();
            ObjectTable customsAddressTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsAddressType", 0, false);
            InsertClosedTableRecord(customsAddressTypeTable, customsAddressTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsAddressTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData addressPurposeTable = closedSystemTables.Where(d => d.id == "1036").FirstOrDefault();
            ObjectTable addressPurposeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AddressPurpose", 0, false);
            InsertClosedTableRecord(addressPurposeTable, addressPurposeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(addressPurposeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData contactRoleTypeTable = closedSystemTables.Where(d => d.id == "84").FirstOrDefault();
            ObjectTable contactRoleTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ContactRoleType", 0, false);
            InsertClosedTableRecord(contactRoleTypeTable, contactRoleTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(contactRoleTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData authorizedSignerPermitTable = closedSystemTables.Where(d => d.id == "1107").FirstOrDefault();
            ObjectTable authorizedSignerPermitObjectTable = objectTableRepository.GetObjectTableByName("Customs.AuthorizedSignerPermit", 0, false);
            InsertClosedTableRecord(authorizedSignerPermitTable, authorizedSignerPermitObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(authorizedSignerPermitTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData cityTable = closedSystemTables.Where(d => d.id == "6").FirstOrDefault();
            ObjectTable cityObjectTable = objectTableRepository.GetObjectTableByName("Customs.City", 0, false);
            InsertClosedTableRecord(cityTable, cityObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(cityTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData declarationStatusTypeTable = closedSystemTables.Where(d => d.id == "1981").FirstOrDefault();
            ObjectTable declarationStatusTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DeclarationStatusType", 0, false);
            InsertClosedTableRecord(declarationStatusTypeTable, declarationStatusTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(declarationStatusTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData communicationTypeTable = closedSystemTables.Where(d => d.id == "7").FirstOrDefault();
            ObjectTable communicationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CommunicationType", 0, false);
            InsertClosedTableRecord(communicationTypeTable, communicationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(communicationTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData salesTaxExemptionTypeTable = closedSystemTables.Where(d => d.id == "1702").FirstOrDefault();
            ObjectTable salesTaxExemptionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SalesTaxExemptionType", 0, false);
            InsertClosedTableRecord(salesTaxExemptionTypeTable, salesTaxExemptionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(salesTaxExemptionTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData termsOfSaleTypeTypeTable = closedSystemTables.Where(d => d.id == "1426").FirstOrDefault();
            ObjectTable termsOfSaleTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.TermsOfSaleType", 0, false);
            InsertClosedTableRecord(termsOfSaleTypeTypeTable, termsOfSaleTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(termsOfSaleTypeTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData invoiceTypeTypeTable = closedSystemTables.Where(d => d.id == "1404").FirstOrDefault();
            ObjectTable invoiceTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.InvoiceType", 0, false);
            InsertClosedTableRecord(invoiceTypeTypeTable, invoiceTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(invoiceTypeTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsBookTypeTable = closedSystemTables.Where(d => d.id == "1065").FirstOrDefault();
            ObjectTable customsBookTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsBookType", 0, false);
            InsertClosedTableRecord(customsBookTypeTable, customsBookTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsBookTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData subCountryTable = closedSystemTables.Where(d => d.id == "1137").FirstOrDefault();
            ObjectTable subCountryObjectTable = objectTableRepository.GetObjectTableByName("Customs.SubCountry", 0, false);
            InsertClosedTableRecord(subCountryTable, subCountryObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(subCountryTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData countryGroupTable = closedSystemTables.Where(d => d.id == "1138").FirstOrDefault();
            ObjectTable countryGroupObjectTable = objectTableRepository.GetObjectTableByName("Customs.CountryGroup", 0, false);
            InsertClosedTableRecord(countryGroupTable, countryGroupObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(countryGroupTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData entitlementTypeTable = closedSystemTables.Where(d => d.id == "1930").FirstOrDefault();
            ObjectTable entitlementTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.EntitlementType", 0, false);
            InsertClosedTableRecord(entitlementTypeTable, entitlementTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(entitlementTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData leadDocumentTypeTable = closedSystemTables.Where(d => d.id == "1375").FirstOrDefault();
            ObjectTable leadDocumentTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.LeadDocumentType", 0, false);
            InsertClosedTableRecord(leadDocumentTypeTable, leadDocumentTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(leadDocumentTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData packageMeasureQualifierTable = closedSystemTables.Where(d => d.id == "1593").FirstOrDefault();
            ObjectTable packageMeasureQualifierObjectTable = objectTableRepository.GetObjectTableByName("Customs.PackageMeasureQualifier", 0, false);
            InsertClosedTableRecord(packageMeasureQualifierTable, packageMeasureQualifierObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(packageMeasureQualifierTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData packingTypeTable = closedSystemTables.Where(d => d.id == "1091").FirstOrDefault();
            ObjectTable packingTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.PackingType", 0, false);
            InsertClosedTableRecord(packingTypeTable, packingTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(packingTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vendorTypeTable = closedSystemTables.Where(d => d.id == "1007").FirstOrDefault();
            ObjectTable vendorTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.VendorType", 0, false);
            InsertClosedTableRecord(vendorTypeTable, vendorTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vendorTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData PoaStatusTypeLookUpTable = closedSystemTables.Where(d => d.id == "1599").FirstOrDefault();
            ObjectTable PoaStatusTypeLookUpObjectTable = objectTableRepository.GetObjectTableByName("Customs.PoaStatusTypeLookUp", 0, false);
            InsertClosedTableRecord(PoaStatusTypeLookUpTable, PoaStatusTypeLookUpObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(PoaStatusTypeLookUpTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CustomerClassificationTypeTable = closedSystemTables.Where(d => d.id == "1055").FirstOrDefault();
            ObjectTable CustomerClassificationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomerClassificationType", 0, false);
            InsertClosedTableRecord(CustomerClassificationTypeTable, CustomerClassificationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CustomerClassificationTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData SecurityClearenceTypeCodeTable = closedSystemTables.Where(d => d.id == "23674").FirstOrDefault();
            ObjectTable SecurityClearenceTypeCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SecurityClearenceTypeCode", 0, false);
            InsertClosedTableRecord(SecurityClearenceTypeCodeTable, SecurityClearenceTypeCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(SecurityClearenceTypeCodeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData SupplierPartyTypeTable = closedSystemTables.Where(d => d.id == "1609").FirstOrDefault();
            ObjectTable SupplierPartyTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SupplierPartyType", 0, false);
            InsertClosedTableRecord(SupplierPartyTypeTable, SupplierPartyTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(SupplierPartyTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ExportDeliveryDocumentMessageSenderCodeTable = closedSystemTables.Where(d => d.id == "23676").FirstOrDefault();
            ObjectTable ExportDeliveryDocumentMessageSenderCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ExportDeliveryDocumentMessage", 0, false);
            InsertClosedTableRecord(ExportDeliveryDocumentMessageSenderCodeTable, ExportDeliveryDocumentMessageSenderCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ExportDeliveryDocumentMessageSenderCodeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData NDMessageActionCodeTable = closedSystemTables.Where(d => d.id == "1998").FirstOrDefault();
            ObjectTable NDMessageActionCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.NDMessageActionCode", 0, false);
            InsertClosedTableRecord(NDMessageActionCodeTable, NDMessageActionCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(NDMessageActionCodeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData HandingCodeTable = closedSystemTables.Where(d => d.id == "23793").FirstOrDefault();
            ObjectTable HandingCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.HandingCode", 0, false);
            InsertClosedTableRecord(HandingCodeTable, HandingCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(HandingCodeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CustomerIdentificationTypeTable = closedSystemTables.Where(d => d.id == "93").FirstOrDefault();
            ObjectTable CustomerIdentificationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomerIdentificationType", 0, false);
            InsertClosedTableRecord(CustomerIdentificationTypeTable, CustomerIdentificationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CustomerIdentificationTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CustomerIndicationTypeTable = closedSystemTables.Where(d => d.id == "98").FirstOrDefault();
            ObjectTable CustomerIndicationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomerIndicationType", 0, false);
            InsertClosedTableRecord(CustomerIndicationTypeTable, CustomerIndicationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CustomerIndicationTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData LogisticActionRequestTypeTable = closedSystemTables.Where(d => d.id == "23747").FirstOrDefault();
            ObjectTable LogisticActionRequestTypeTableObjectTable = objectTableRepository.GetObjectTableByName("Customs.LogisticActionRequestType", 0, false);
            InsertClosedTableRecord(LogisticActionRequestTypeTable, LogisticActionRequestTypeTableObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(LogisticActionRequestTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData LogisticActionResponseRequestSTable = closedSystemTables.Where(d => d.id == "23748").FirstOrDefault();
            ObjectTable LogisticActionResponseRequestSObjectTable = objectTableRepository.GetObjectTableByName("Customs.LogisticActionResponseReqS", 0, false);
            InsertClosedTableRecord(LogisticActionResponseRequestSTable, LogisticActionResponseRequestSObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(LogisticActionResponseRequestSTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData DeliveryTypeTable = closedSystemTables.Where(d => d.id == "23675").FirstOrDefault();
            ObjectTable DeliveryTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DeliveryType", 0, false);
            InsertClosedTableRecord(DeliveryTypeTable, DeliveryTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(DeliveryTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData PoaAuthorizationTypeLookupeTable = closedSystemTables.Where(d => d.id == "1595").FirstOrDefault();
            ObjectTable PoaAuthorizationTypeLookupObjectTable = objectTableRepository.GetObjectTableByName("Customs.PoaAuthorizationTypeLookup", 0, false);
            InsertClosedTableRecord(PoaAuthorizationTypeLookupeTable, PoaAuthorizationTypeLookupObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(PoaAuthorizationTypeLookupeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CoolingReportingMethodTable = closedSystemTables.Where(d => d.id == "23799").FirstOrDefault();
            ObjectTable CoolingReportingMethodObjectTable = objectTableRepository.GetObjectTableByName("Customs.CoolingReportingMethod", 0, false);
            InsertClosedTableRecord(CoolingReportingMethodTable, CoolingReportingMethodObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CoolingReportingMethodTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CargoTypeTable = closedSystemTables.Where(d => d.id == "1558").FirstOrDefault();
            ObjectTable CargoTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CargoType", 0, false);
            InsertClosedTableRecord(CargoTypeTable, CargoTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CargoTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paymentTypeTable = closedSystemTables.Where(d => d.id == "1897").FirstOrDefault();
            ObjectTable paymentTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.PaymentType", 0, false);
            InsertClosedTableRecord(paymentTypeTable, paymentTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paymentTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData currencyTypeTable = closedSystemTables.Where(d => d.id == "1144").FirstOrDefault();
            ObjectTable currencyTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CurrencyType", 0, false);
            InsertClosedTableRecord(currencyTypeTable, currencyTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(currencyTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsPaymentTermTable = closedSystemTables.Where(d => d.id == "1425").FirstOrDefault();
            ObjectTable customsPaymentTermObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsPaymentTerm", 0, false);
            InsertClosedTableRecord(customsPaymentTermTable, customsPaymentTermObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsPaymentTermTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData dangerousGoodsPackingReqTable = closedSystemTables.Where(d => d.id == "1266").FirstOrDefault();
            ObjectTable dangerousGoodsPackingReqObjectTable = objectTableRepository.GetObjectTableByName("Customs.DangerousGoodsPackingReq", 0, false);
            InsertClosedTableRecord(dangerousGoodsPackingReqTable, dangerousGoodsPackingReqObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(dangerousGoodsPackingReqTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData LogisticsReferenceTypeTable = closedSystemTables.Where(d => d.id == "2032").FirstOrDefault();
            ObjectTable LogisticsReferenceTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.LogisticsReferenceType", 0, false);
            InsertClosedTableRecord(LogisticsReferenceTypeTable, LogisticsReferenceTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(LogisticsReferenceTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ReferenceStatusTable = closedSystemTables.Where(d => d.id == "2130").FirstOrDefault();
            ObjectTable ReferenceStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.ReferenceStatus", 0, false);
            InsertClosedTableRecord(ReferenceStatusTable, ReferenceStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ReferenceStatusTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ReferenceInputTypeTable = closedSystemTables.Where(d => d.id == "1713").FirstOrDefault();
            ObjectTable ReferenceInputTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ReferenceInputType", 0, false);
            InsertClosedTableRecord(ReferenceInputTypeTable, ReferenceInputTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ReferenceInputTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData itemGovernmentProcedureTypeTable = closedSystemTables.Where(d => d.id == "1422").FirstOrDefault();
            ObjectTable itemGovernmentProcedureTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ItemGovernmentProcedureType", 0, false);
            InsertClosedTableRecord(itemGovernmentProcedureTypeTable, itemGovernmentProcedureTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(itemGovernmentProcedureTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customerActivityTypeTable = closedSystemTables.Where(d => d.id == "97").FirstOrDefault();
            ObjectTable customerActivityTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomerActivityType", 0, false);
            InsertClosedTableRecord(customerActivityTypeTable, customerActivityTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customerActivityTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData StuffingSiteTypeTable = closedSystemTables.Where(d => d.id == "23792").FirstOrDefault();
            ObjectTable StuffingSiteTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.StuffingSiteType", 0, false);
            InsertClosedTableRecord(StuffingSiteTypeTable, StuffingSiteTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(StuffingSiteTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ContainerTypeTable = closedSystemTables.Where(d => d.id == "1366").FirstOrDefault();
            ObjectTable ContainerTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ContainerType", 0, false);
            InsertClosedTableRecord(ContainerTypeTable, ContainerTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ContainerTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paymentOrderTypeTable = closedSystemTables.Where(d => d.id == "1116").FirstOrDefault();
            ObjectTable paymentOrderTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.PaymentOrderType", 0, false);
            InsertClosedTableRecord(paymentOrderTypeTable, paymentOrderTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paymentOrderTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paymentProcessTable = closedSystemTables.Where(d => d.id == "1646").FirstOrDefault();
            ObjectTable paymentProcessObjectTable = objectTableRepository.GetObjectTableByName("Customs.PaymentProcess", 0, false);
            InsertClosedTableRecord(paymentProcessTable, paymentProcessObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paymentProcessTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paymentOrderStatusTable = closedSystemTables.Where(d => d.id == "1133").FirstOrDefault();
            ObjectTable paymentOrderStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.PaymentOrderStatus", 0, false);
            InsertClosedTableRecord(paymentOrderStatusTable, paymentOrderStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paymentOrderStatusTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData organizationUnitTypeTable = closedSystemTables.Where(d => d.id == "20").FirstOrDefault();
            ObjectTable organizationUnitTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.OrganizationUnitType", 0, false);
            InsertClosedTableRecord(organizationUnitTypeTable, organizationUnitTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(organizationUnitTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paymentMethodTypeTable = closedSystemTables.Where(d => d.id == "1121").FirstOrDefault();
            ObjectTable paymentMethodTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.PaymentMethodType", 0, false);
            InsertClosedTableRecord(paymentMethodTypeTable, paymentMethodTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paymentMethodTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData bankTable = closedSystemTables.Where(d => d.id == "1112").FirstOrDefault();
            ObjectTable bankObjectTable = objectTableRepository.GetObjectTableByName("Customs.Bank", 0, false);
            InsertClosedTableRecord(bankTable, bankObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(bankTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData FullnessCodeTable = closedSystemTables.Where(d => d.id == "1365").FirstOrDefault();
            ObjectTable FullnessCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.FullnessCode", 0, false);
            InsertClosedTableRecord(FullnessCodeTable, FullnessCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(FullnessCodeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsBranchTable = closedSystemTables.Where(d => d.id == "1118").FirstOrDefault();
            ObjectTable customsBranchObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsBranch", 0, false);
            InsertClosedTableRecord(customsBranchTable, customsBranchObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsBranchTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paymentMethodStatusTable = closedSystemTables.Where(d => d.id == "1153").FirstOrDefault();
            ObjectTable paymentMethodStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.PaymentMethodStatus", 0, false);
            InsertClosedTableRecord(paymentMethodStatusTable, paymentMethodStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paymentMethodStatusTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData paymentProtestTypeTable = closedSystemTables.Where(d => d.id == "1191").FirstOrDefault();
            ObjectTable paymentProtestTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.PaymentProtestType", 0, false);
            InsertClosedTableRecord(paymentProtestTypeTable, paymentProtestTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(paymentProtestTypeTable);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData productIdentificationTypeTable = closedSystemTables.Where(d => d.id == "1592").FirstOrDefault();
            ObjectTable productIdentificationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ProductIdentificationType", 0, false);
            InsertClosedTableRecord(productIdentificationTypeTable, productIdentificationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(productIdentificationTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData productNameTypeTable = closedSystemTables.Where(d => d.id == "1439").FirstOrDefault();
            ObjectTable productNameTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ProductNameType", 0, false);
            InsertClosedTableRecord(productNameTypeTable, productNameTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(productNameTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsCountryTable = closedSystemTables.Where(d => d.id == "1136").FirstOrDefault();
            ObjectTable customsCountryObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsCountry", 0, false);
            InsertClosedTableRecord(customsCountryTable, customsCountryObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsCountryTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsHouseTypeTable = closedSystemTables.Where(d => d.id == "2011").FirstOrDefault();
            ObjectTable customsHouseTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsHouseType", 0, false);
            InsertClosedTableRecord(customsHouseTypeTable, customsHouseTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsHouseTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData collateralTypeTable = closedSystemTables.Where(d => d.id == "1549").FirstOrDefault();
            ObjectTable collateralTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CollateralType", 0, false);
            InsertClosedTableRecord(collateralTypeTable, collateralTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(collateralTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData collateralRequestStatusTable = closedSystemTables.Where(d => d.id == "1547").FirstOrDefault();
            ObjectTable collateralRequestStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.CollateralRequestStatus", 0, false);
            InsertClosedTableRecord(collateralRequestStatusTable, collateralRequestStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(collateralRequestStatusTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData returnConditionTable = closedSystemTables.Where(d => d.id == "1292").FirstOrDefault();
            ObjectTable returnConditionObjectTable = objectTableRepository.GetObjectTableByName("Customs.ReturnCondition", 0, false);
            InsertClosedTableRecord(returnConditionTable, returnConditionObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(returnConditionTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData collateralAnswerTypeTable = closedSystemTables.Where(d => d.id == "1552").FirstOrDefault();
            ObjectTable collateralAnswerTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CollateralAnswerType", 0, false);
            InsertClosedTableRecord(collateralAnswerTypeTable, collateralAnswerTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(collateralAnswerTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData collateralAnswerStatusTable = closedSystemTables.Where(d => d.id == "1553").FirstOrDefault();
            ObjectTable collateralAnswerStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.CollateralAnswerStatus", 0, false);
            InsertClosedTableRecord(collateralAnswerStatusTable, collateralAnswerStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(collateralAnswerStatusTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData constraintProcessTypeTable = closedSystemTables.Where(d => d.id == "1576").FirstOrDefault();
            ObjectTable constraintProcessTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ConstraintProcessType", 0, false);
            InsertClosedTableRecord(constraintProcessTypeTable, constraintProcessTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(constraintProcessTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData constraintApprovalDecisionTypeTable = closedSystemTables.Where(d => d.id == "1575").FirstOrDefault();
            ObjectTable constraintApprovalDecisionObjectTable = objectTableRepository.GetObjectTableByName("Customs.ConstraintApprovalDecision", 0, false);
            InsertClosedTableRecord(constraintApprovalDecisionTypeTable, constraintApprovalDecisionObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(constraintApprovalDecisionTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData unloadingSiteTypeTable = closedSystemTables.Where(d => d.id == "2192").FirstOrDefault();
            ObjectTable UnloadingSiteTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.UnloadingSiteType", 0, false);
            InsertClosedTableRecord(unloadingSiteTypeTable, UnloadingSiteTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(unloadingSiteTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData morningMessageTypeTable = closedSystemTables.Where(d => d.id == "2028").FirstOrDefault();
            ObjectTable morningMessageTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.MorningMessageType", 0, false);
            InsertClosedTableRecord(morningMessageTypeTable, morningMessageTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(morningMessageTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vendorStatusTable = closedSystemTables.Where(d => d.id == "1011").FirstOrDefault();
            ObjectTable vendorStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.VendorStatus", 0, false);
            InsertClosedTableRecord(vendorStatusTable, vendorStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vendorStatusTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vendorTransactionType = closedSystemTables.Where(d => d.id == "1033").FirstOrDefault();
            ObjectTable vendorTransactionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.VendorTransactionType", 0, false);
            InsertClosedTableRecord(vendorTransactionType, vendorTransactionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vendorTransactionType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CancelRequestRejectReasonType = closedSystemTables.Where(d => d.id == "1927").FirstOrDefault();
            ObjectTable CancelRequestRejectReasonTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CancelRequestRejectReasonType", 0, false);
            InsertClosedTableRecord(CancelRequestRejectReasonType, CancelRequestRejectReasonTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CancelRequestRejectReasonType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData validCustomsItem = closedSystemTables.Where(d => d.id == "1966").FirstOrDefault();
            ObjectTable validCustomsItemObjectTable = objectTableRepository.GetObjectTableByName("Customs.ValidCustomsItem", 0, false);
            InsertClosedTableRecord(validCustomsItem, validCustomsItemObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(validCustomsItem);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData tradeLevyExamptType = closedSystemTables.Where(d => d.id == "1437").FirstOrDefault();
            ObjectTable TradeLevyExamptTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.TradeLevyExamptType", 0, false);
            InsertClosedTableRecord(tradeLevyExamptType, TradeLevyExamptTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(tradeLevyExamptType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData importerPeriodicDeclarationStatus = closedSystemTables.Where(d => d.id == "1206").FirstOrDefault();
            ObjectTable ImporterPeriodicDeclarationStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.ImporterPeriodicDeclarStatus", 0, false);
            InsertClosedTableRecord(importerPeriodicDeclarationStatus, ImporterPeriodicDeclarationStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(importerPeriodicDeclarationStatus);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData guaranteeCertificateType = closedSystemTables.Where(d => d.id == "1352").FirstOrDefault();
            ObjectTable guaranteeCertificateTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.GuaranteeCertificateType", 0, false);
            InsertClosedTableRecord(guaranteeCertificateType, guaranteeCertificateTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(guaranteeCertificateType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData debtNotificationType = closedSystemTables.Where(d => d.id == "1714").FirstOrDefault();
            ObjectTable debtNotificationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DebtNotificationType", 0, false);
            InsertClosedTableRecord(debtNotificationType, debtNotificationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(debtNotificationType);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData depositCustomerActivity = closedSystemTables.Where(d => d.id == "1300").FirstOrDefault();
            ObjectTable depositCustomerActivityObjectTable = objectTableRepository.GetObjectTableByName("Customs.DepositCustomerActivity", 0, false);
            InsertClosedTableRecord(depositCustomerActivity, depositCustomerActivityObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(depositCustomerActivity);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData requestStatus = closedSystemTables.Where(d => d.id == "1145").FirstOrDefault();
            ObjectTable requestStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.RequestStatus", 0, false);
            InsertClosedTableRecord(requestStatus, requestStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(requestStatus);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData demanderType = closedSystemTables.Where(d => d.id == "1444").FirstOrDefault();
            ObjectTable demanderTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DemanderType", 0, false);
            InsertClosedTableRecord(demanderType, demanderTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(demanderType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData storageMessageType = closedSystemTables.Where(d => d.id == "1373").FirstOrDefault();
            ObjectTable storageMessageTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.StorageMessageType", 0, false);
            InsertClosedTableRecord(storageMessageType, storageMessageTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(storageMessageType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData specialActionDescriptionType = closedSystemTables.Where(d => d.id == "1383").FirstOrDefault();
            ObjectTable specialActionDescriptionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SpecialActionDescriptionType", 0, false);
            InsertClosedTableRecord(specialActionDescriptionType, specialActionDescriptionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(specialActionDescriptionType);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData internationalSite = closedSystemTables.Where(d => d.id == "1344").FirstOrDefault();
            ObjectTable internationalSiteObjectTable = objectTableRepository.GetObjectTableByName("Customs.InternationalSite", 0, false);
            InsertClosedTableRecord(internationalSite, internationalSiteObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(internationalSite);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData proceduralFaultStatus = closedSystemTables.Where(d => d.id == "1468").FirstOrDefault();
            ObjectTable proceduralFaultStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.ProceduralFaultStatus", 0, false);
            InsertClosedTableRecord(proceduralFaultStatus, proceduralFaultStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(proceduralFaultStatus);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData proceduralFaultInputSourceType = closedSystemTables.Where(d => d.id == "1545").FirstOrDefault();
            ObjectTable proceduralFaultInputSourceTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ProceduralFaultInSourceType", 0, false);
            InsertClosedTableRecord(proceduralFaultInputSourceType, proceduralFaultInputSourceTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(proceduralFaultInputSourceType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData faultInspectionType = closedSystemTables.Where(d => d.id == "1464").FirstOrDefault();
            ObjectTable faultInspectionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.FaultInspectionType", 0, false);
            InsertClosedTableRecord(faultInspectionType, faultInspectionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(faultInspectionType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData proceduralFaultType = closedSystemTables.Where(d => d.id == "1463").FirstOrDefault();
            ObjectTable proceduralFaultTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ProceduralFaultType", 0, false);
            InsertClosedTableRecord(proceduralFaultType, proceduralFaultTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(proceduralFaultType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData agenttalkbacktype = closedSystemTables.Where(d => d.id == "1461").FirstOrDefault();
            ObjectTable agenttalkbacktypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AgentTalkBackType", 0, false);
            InsertClosedTableRecord(agenttalkbacktype, agenttalkbacktypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(agenttalkbacktype);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData proceduralFaultInputProcessType = closedSystemTables.Where(d => d.id == "1466").FirstOrDefault();
            ObjectTable proceduralFaultInputProcessTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ProceduralFaultInProcessType", 0, false);
            InsertClosedTableRecord(proceduralFaultInputProcessType, proceduralFaultInputProcessTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(proceduralFaultInputProcessType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ransomViolationType = closedSystemTables.Where(d => d.id == "1489").FirstOrDefault();
            ObjectTable ransomViolationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.RansomViolationType", 0, false);
            InsertClosedTableRecord(ransomViolationType, ransomViolationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ransomViolationType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData depositFileType = closedSystemTables.Where(d => d.id == "1298").FirstOrDefault();
            ObjectTable depositFileTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DepositFileType", 0, false);
            InsertClosedTableRecord(depositFileType, depositFileTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(depositFileType);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData cargoIdentityQualifier = closedSystemTables.Where(d => d.id == "1326").FirstOrDefault();
            ObjectTable cargoIdentityQualifierObjectTable = objectTableRepository.GetObjectTableByName("Customs.CargoIdentityQualifier", 0, false);
            InsertClosedTableRecord(cargoIdentityQualifier, cargoIdentityQualifierObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(cargoIdentityQualifier);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vehiclePoolType = closedSystemTables.Where(d => d.id == "1470").FirstOrDefault();
            ObjectTable vehiclePoolTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.VehiclePoolType", 0, false);
            InsertClosedTableRecord(vehiclePoolType, vehiclePoolTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vehiclePoolType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vehiclePriceListType = closedSystemTables.Where(d => d.id == "1594").FirstOrDefault();
            ObjectTable vehiclePriceListTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.VehiclePriceListType", 0, false);
            InsertClosedTableRecord(vehiclePriceListType, vehiclePriceListTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vehiclePriceListType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vehicleManufacturer = closedSystemTables.Where(d => d.id == "1940").FirstOrDefault();
            ObjectTable vehicleManufacturerObjectTable = objectTableRepository.GetObjectTableByName("Customs.VehicleManufacturer", 0, false);
            InsertClosedTableRecord(vehicleManufacturer, vehicleManufacturerObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vehicleManufacturer);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData converterType = closedSystemTables.Where(d => d.id == "1611").FirstOrDefault();
            ObjectTable converterTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ConverterType", 0, false);
            InsertClosedTableRecord(converterType, converterTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(converterType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vehicleTecnologyType = closedSystemTables.Where(d => d.id == "2262").FirstOrDefault();
            ObjectTable vehicleTecnologyTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.VehicleTecnologyType", 0, false);
            InsertClosedTableRecord(vehicleTecnologyType, vehicleTecnologyTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vehicleTecnologyType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData fuelType = closedSystemTables.Where(d => d.id == "1555").FirstOrDefault();
            ObjectTable fuelTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.FuelType", 0, false);
            InsertClosedTableRecord(fuelType, fuelTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(fuelType);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vehicleType = closedSystemTables.Where(d => d.id == "1946").FirstOrDefault();
            ObjectTable vehicleTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.VehicleType", 0, false);
            InsertClosedTableRecord(vehicleType, vehicleTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vehicleType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customerIdentifyType = closedSystemTables.Where(d => d.id == "1570").FirstOrDefault();
            ObjectTable customerIdentifyTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomerIdentifyType", 0, false);
            InsertClosedTableRecord(customerIdentifyType, customerIdentifyTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customerIdentifyType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData checkEssenceLookup = closedSystemTables.Where(d => d.id == "1509").FirstOrDefault();
            ObjectTable checkEssenceLookupObjectTable = objectTableRepository.GetObjectTableByName("Customs.CheckEssenceLookup", 0, false);
            InsertClosedTableRecord(checkEssenceLookup, checkEssenceLookupObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(checkEssenceLookup);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData authority = closedSystemTables.Where(d => d.id == "1158").FirstOrDefault();
            ObjectTable authorityObjectTable = objectTableRepository.GetObjectTableByName("Customs.Authority", 0, false);
            InsertClosedTableRecord(authority, authorityObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(authority);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vehicleReductionType = closedSystemTables.Where(d => d.id == "1616").FirstOrDefault();
            ObjectTable vehicleReductionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.VehicleReductionType", 0, false);
            InsertClosedTableRecord(vehicleReductionType, vehicleReductionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vehicleReductionType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData vehicleSafetyAccessory = closedSystemTables.Where(d => d.id == "2280").FirstOrDefault();
            ObjectTable vehicleSafetyAccessoryObjectTable = objectTableRepository.GetObjectTableByName("Customs.VehicleSafetyAccessoryType", 0, false);
            InsertClosedTableRecord(vehicleSafetyAccessory, vehicleSafetyAccessoryObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(vehicleSafetyAccessory);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData guaranteeCustomerActivity = closedSystemTables.Where(d => d.id == "2185").FirstOrDefault();
            ObjectTable guaranteeCustomerActivityObjectTable = objectTableRepository.GetObjectTableByName("Customs.GuaranteeCustomerActivity", 0, false);
            InsertClosedTableRecord(guaranteeCustomerActivity, guaranteeCustomerActivityObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(guaranteeCustomerActivity);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData checkTypeLookup = closedSystemTables.Where(d => d.id == "1522").FirstOrDefault();
            ObjectTable checkTypeLookupObjectTable = objectTableRepository.GetObjectTableByName("Customs.CheckTypeLookup", 0, false);
            InsertClosedTableRecord(checkTypeLookup, checkTypeLookupObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(checkTypeLookup);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData continuousMessagesTypeCode = closedSystemTables.Where(d => d.id == "1483").FirstOrDefault();
            ObjectTable continuousMessagesTypeCodesObjectTable = objectTableRepository.GetObjectTableByName("Customs.ContinuousMessagesTypeCode", 0, false);
            InsertClosedTableRecord(continuousMessagesTypeCode, continuousMessagesTypeCodesObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(continuousMessagesTypeCode);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData claimEntityType = closedSystemTables.Where(d => d.id == "1305").FirstOrDefault();
            ObjectTable claimEntityTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ClaimEntity", 0, false);
            InsertClosedTableRecord(claimEntityType, claimEntityTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(claimEntityType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData commercialSale = closedSystemTables.Where(d => d.id == "1113").FirstOrDefault();
            ObjectTable commercialSaleObjectTable = objectTableRepository.GetObjectTableByName("Customs.CommercialSale", 0, false);
            InsertClosedTableRecord(commercialSale, commercialSaleObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(commercialSale);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData claimExplanationCode = closedSystemTables.Where(d => d.id == "1899").FirstOrDefault();
            ObjectTable claimExplanationCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ClaimExplanationCode", 0, false);
            InsertClosedTableRecord(claimExplanationCode, claimExplanationCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(claimExplanationCode);

            var ImporterTypeForClaim = closedSystemTables.Where(d => d.id == "2022").FirstOrDefault();
            var ImporterTypeForClaimObjectTable = objectTableRepository.GetObjectTableByName("Customs.ImporterTypeForClaim", 0, false);
            InsertClosedTableRecord(ImporterTypeForClaim, ImporterTypeForClaimObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ImporterTypeForClaim);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData processingReason = closedSystemTables.Where(d => d.id == "1178").FirstOrDefault();
            ObjectTable processingReasonObjectTable = objectTableRepository.GetObjectTableByName("Customs.ProcessingReason", 0, false);
            InsertClosedTableRecord(processingReason, processingReasonObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(processingReason);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData amendmentRequestStatus = closedSystemTables.Where(d => d.id == "1590").FirstOrDefault();
            ObjectTable amendmentRequestStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.AmendmentRequestStatus", 0, false);
            InsertClosedTableRecord(amendmentRequestStatus, amendmentRequestStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(amendmentRequestStatus);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData declarationStatementType = closedSystemTables.Where(d => d.id == "2286").FirstOrDefault();
            ObjectTable declarationStatementTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DeclarationStatementType", 0, false);
            InsertClosedTableRecord(declarationStatementType, declarationStatementTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(declarationStatementType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData amendmentFieldReasonType = closedSystemTables.Where(d => d.id == "1429").FirstOrDefault();
            ObjectTable amendmentFieldReasonTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AmendmentFieldReasonType", 0, false);
            InsertClosedTableRecord(amendmentFieldReasonType, amendmentFieldReasonTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(amendmentFieldReasonType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData courtInstance = closedSystemTables.Where(d => d.id == "1180").FirstOrDefault();
            ObjectTable courtInstanceObjectTable = objectTableRepository.GetObjectTableByName("Customs.CourtInstance", 0, false);
            InsertClosedTableRecord(courtInstance, courtInstanceObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(courtInstance);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData importerDeclarationType = closedSystemTables.Where(d => d.id == "23589").FirstOrDefault();
            ObjectTable importerDeclarationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ImporterDeclarationType", 0, false);
            InsertClosedTableRecord(importerDeclarationType, importerDeclarationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(importerDeclarationType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsInsuranceCompany = closedSystemTables.Where(d => d.id == "1215").FirstOrDefault();
            ObjectTable customsInsuranceCompanyObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsInsuranceCompany", 0, false);
            InsertClosedTableRecord(customsInsuranceCompany, customsInsuranceCompanyObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsInsuranceCompany);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData customsCargoStatus = closedSystemTables.Where(d => d.id == "1390").FirstOrDefault();
            ObjectTable customsCargoStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.CargoStatus", 0, false);
            InsertClosedTableRecord(customsCargoStatus, customsCargoStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(customsCargoStatus);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData freightPaymentMethod = closedSystemTables.Where(d => d.id == "1345").FirstOrDefault();
            ObjectTable freightPaymentMethodObjectTable = objectTableRepository.GetObjectTableByName("Customs.FreightPaymentMethod", 0, false);
            InsertClosedTableRecord(freightPaymentMethod, freightPaymentMethodObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(freightPaymentMethod);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData uIMessage = closedSystemTables.Where(d => d.id == "43").FirstOrDefault();
            ObjectTable uIMessageObjectTable = objectTableRepository.GetObjectTableByName("Customs.UIMessage", 0, false);
            InsertClosedTableRecord(uIMessage, uIMessageObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(uIMessage);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ActionCode = closedSystemTables.Where(d => d.id == "2228").FirstOrDefault();
            ObjectTable ActionCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ActionCode", 0, false);
            InsertClosedTableRecord(ActionCode, ActionCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ActionCode);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData SplitOrMergeReason = closedSystemTables.Where(d => d.id == "1455").FirstOrDefault();
            ObjectTable SplitOrMergeReasonObjectTable = objectTableRepository.GetObjectTableByName("Customs.SplitOrMergeReason", 0, false);
            InsertClosedTableRecord(SplitOrMergeReason, SplitOrMergeReasonObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(SplitOrMergeReason);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CargoSplitRequestStatus = closedSystemTables.Where(d => d.id == "2274").FirstOrDefault();
            ObjectTable CargoSplitRequestStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.CargoSplitRequestStatus", 0, false);
            InsertClosedTableRecord(CargoSplitRequestStatus, CargoSplitRequestStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CargoSplitRequestStatus);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData TreatmentWay = closedSystemTables.Where(d => d.id == "1347").FirstOrDefault();
            ObjectTable TreatmentWayObjectTable = objectTableRepository.GetObjectTableByName("Customs.TreatmentWay", 0, false);
            InsertClosedTableRecord(TreatmentWay, TreatmentWayObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(TreatmentWay);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData seizureMethodType = closedSystemTables.Where(d => d.id == "2285").FirstOrDefault();
            ObjectTable seizureMethodTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SeizureMethodType", 0, false);
            InsertClosedTableRecord(seizureMethodType, seizureMethodTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(seizureMethodType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData decisionType = closedSystemTables.Where(d => d.id == "1157").FirstOrDefault();
            ObjectTable decisionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.DecisionType", 0, false);
            InsertClosedTableRecord(decisionType, decisionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(decisionType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData TPGFileType = closedSystemTables.Where(d => d.id == "1596").FirstOrDefault();
            ObjectTable TPGFileTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.TPGFileType", 0, false);
            InsertClosedTableRecord(TPGFileType, TPGFileTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(TPGFileType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData seizureFactorType = closedSystemTables.Where(d => d.id == "2035").FirstOrDefault();
            ObjectTable seizureFactorTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SeizureFactorType", 0, false);
            InsertClosedTableRecord(seizureFactorType, seizureFactorTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(seizureFactorType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData refundCustomerActivityType = closedSystemTables.Where(d => d.id == "1248").FirstOrDefault();
            ObjectTable refundCustomerActivityTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.RefundCustomerActivityType", 0, false);
            InsertClosedTableRecord(refundCustomerActivityType, refundCustomerActivityTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(refundCustomerActivityType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData transferCargoMethodType = closedSystemTables.Where(d => d.id == "42").FirstOrDefault();
            ObjectTable transferCargoMethodTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.TransferCargoMethodType", 0, false);
            InsertClosedTableRecord(transferCargoMethodType, transferCargoMethodTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(transferCargoMethodType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData gatepassReturnCode = closedSystemTables.Where(d => d.id == "1589").FirstOrDefault();
            ObjectTable gatepassReturnCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.GatepassReturnCode", 0, false);
            InsertClosedTableRecord(gatepassReturnCode, gatepassReturnCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(gatepassReturnCode);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData updateCode = closedSystemTables.Where(d => d.id == "1564").FirstOrDefault();
            ObjectTable updateCodeObjectTable = objectTableRepository.GetObjectTableByName("Customs.UpdateCode", 0, false);
            InsertClosedTableRecord(updateCode, updateCodeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(updateCode);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData continuousRequestType = closedSystemTables.Where(d => d.id == "1156").FirstOrDefault();
            ObjectTable continuousRequestTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ContinuousRequestType", 0, false);
            InsertClosedTableRecord(continuousRequestType, continuousRequestTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(continuousRequestType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData requestType = closedSystemTables.Where(d => d.id == "1653").FirstOrDefault();
            ObjectTable requestTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.RequestType", 0, false);
            InsertClosedTableRecord(requestType, requestTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(requestType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData approvedProfession = closedSystemTables.Where(d => d.id == "1644").FirstOrDefault();
            ObjectTable approvedProfessionObjectTable = objectTableRepository.GetObjectTableByName("Customs.ApprovedProfession", 0, false);
            InsertClosedTableRecord(approvedProfession, approvedProfessionObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(approvedProfession);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData hazardousSubstance = closedSystemTables.Where(d => d.id == "1363").FirstOrDefault();
            ObjectTable hazardousSubstanceObjectTable = objectTableRepository.GetObjectTableByName("Customs.HazardousSubstance", 0, false);
            InsertClosedTableRecord(hazardousSubstance, hazardousSubstanceObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(hazardousSubstance);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData amendmentType = closedSystemTables.Where(d => d.id == "1430").FirstOrDefault();
            ObjectTable amendmentTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AmendmentType", 0, false);
            InsertClosedTableRecord(amendmentType, amendmentTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(amendmentType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData sealReason = closedSystemTables.Where(d => d.id == "1459").FirstOrDefault();
            ObjectTable sealReasonObjectTable = objectTableRepository.GetObjectTableByName("Customs.SealUpdateReasonType", 0, false);
            InsertClosedTableRecord(sealReason, sealReasonObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(sealReason);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData sealCompletenes = closedSystemTables.Where(d => d.id == "1273").FirstOrDefault();
            ObjectTable sealCompletenesObjectTable = objectTableRepository.GetObjectTableByName("Customs.SealCompletenes", 0, false);
            InsertClosedTableRecord(sealCompletenes, sealCompletenesObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(sealCompletenes);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData sealType = closedSystemTables.Where(d => d.id == "1272").FirstOrDefault();
            ObjectTable sealTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.SealType", 0, false);
            InsertClosedTableRecord(sealType, sealTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(sealType);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CustomsShip = closedSystemTables.Where(d => d.id == "1308").FirstOrDefault();
            ObjectTable CustomsShipObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomsShip", 0, false);
            InsertClosedTableRecord(CustomsShip, CustomsShipObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CustomsShip);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CustomerRoleType = closedSystemTables.Where(d => d.id == "1432").FirstOrDefault();
            ObjectTable CustomerRoleTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CustomerRoleType", 0, false);
            InsertClosedTableRecord(CustomerRoleType, CustomerRoleTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CustomerRoleType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData PartyRelationshipType = closedSystemTables.Where(d => d.id == "2113").FirstOrDefault();
            ObjectTable PartyRelationshipTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.PartyRelationshipType", 0, false);
            InsertClosedTableRecord(PartyRelationshipType, PartyRelationshipTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(PartyRelationshipType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ClassificationType = closedSystemTables.Where(d => d.id == "1384").FirstOrDefault();
            ObjectTable ClassificationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ClassificationType", 0, false);
            InsertClosedTableRecord(ClassificationType, ClassificationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ClassificationType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData TransactionNatureType = closedSystemTables.Where(d => d.id == "1328").FirstOrDefault();
            ObjectTable TransactionNatureTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.TransactionNatureType", 0, false);
            InsertClosedTableRecord(TransactionNatureType, TransactionNatureTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(TransactionNatureType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ClaimReasonType = closedSystemTables.Where(d => d.id == "1528").FirstOrDefault();
            ObjectTable ClaimReasonTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ClaimReasonType", 0, false);
            InsertClosedTableRecord(ClaimReasonType, ClaimReasonTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ClaimReasonType);



            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData AmountType = closedSystemTables.Where(d => d.id == "1436").FirstOrDefault();
            ObjectTable AmountTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AmountType", 0, false);
            InsertClosedTableRecord(AmountType, AmountTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(AmountType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData NbcDeclarationType = closedSystemTables.Where(d => d.id == "2112").FirstOrDefault();
            ObjectTable NbcDeclarationTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.NbcDeclarationType", 0, false);
            InsertClosedTableRecord(NbcDeclarationType, NbcDeclarationTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(NbcDeclarationType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ExporterRoleType = closedSystemTables.Where(d => d.id == "23783").FirstOrDefault();
            ObjectTable ExporterRoleTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.ExporterRoleType", 0, false);
            InsertClosedTableRecord(ExporterRoleType, ExporterRoleTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ExporterRoleType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData BuyerRoleTypeType = closedSystemTables.Where(d => d.id == "23784").FirstOrDefault();
            ObjectTable BuyerRoleTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.BuyerRoleType", 0, false);
            InsertClosedTableRecord(BuyerRoleTypeType, BuyerRoleTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(BuyerRoleTypeType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData AutonomyRegionType = closedSystemTables.Where(d => d.id == "1937").FirstOrDefault();
            ObjectTable AutonomyRegionTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AutonomyRegionType", 0, false);
            InsertClosedTableRecord(AutonomyRegionType, AutonomyRegionTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(AutonomyRegionType);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CancellationRequestStatus = closedSystemTables.Where(d => d.id == "1932").FirstOrDefault();
            ObjectTable CancellationRequestStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.CancellationRequestStatus", 0, false);
            InsertClosedTableRecord(CancellationRequestStatus, CancellationRequestStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CancellationRequestStatus);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData CancellationReasonRequestType = closedSystemTables.Where(d => d.id == "1906").FirstOrDefault();
            ObjectTable CancellationReasonRequestTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.CancellationReasonRequestType", 0, false);
            InsertClosedTableRecord(CancellationReasonRequestType, CancellationReasonRequestTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(CancellationReasonRequestType);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData amendRequestRejectReasonTypeTable = closedSystemTables.Where(d => d.id == "1606").FirstOrDefault();
            ObjectTable amendRequestRejectReasonTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.AmendRequestRejectReasonType", 0, false);
            InsertClosedTableRecord(amendRequestRejectReasonTypeTable, amendRequestRejectReasonTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(amendRequestRejectReasonTypeTable);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData amendmentFieldStatusTypeTable = closedSystemTables.Where(d => d.id == "1431").FirstOrDefault();
            ObjectTable amendmentFieldStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.AmendmentFieldStatusType", 0, false);
            InsertClosedTableRecord(amendmentFieldStatusTypeTable, amendmentFieldStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(amendmentFieldStatusTypeTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData loadingSiteTypeTable = closedSystemTables.Where(d => d.id == "23774").FirstOrDefault();
            ObjectTable loadingSiteTypeObjectTable = objectTableRepository.GetObjectTableByName("Customs.LoadingSiteType", 0, false);
            InsertClosedTableRecord(loadingSiteTypeTable, loadingSiteTypeObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(loadingSiteTypeTable);


            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData  amendCancellRequestInitiatorTable = closedSystemTables.Where(d => d.id == "1451").FirstOrDefault();
            ObjectTable amendCancellRequestInitiatorObjectTable = objectTableRepository.GetObjectTableByName("Customs.AmendCancellRequestInitiator", 0, false);
            InsertClosedTableRecord(amendCancellRequestInitiatorTable, amendCancellRequestInitiatorObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(amendCancellRequestInitiatorTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData documentRejectTypeInitiatorTable = closedSystemTables.Where(d => d.id == "1665").FirstOrDefault();
            ObjectTable documentRejectTypeInitiatorObjectTable = objectTableRepository.GetObjectTableByName("Customs.DocumentRejectType", 0, false);
            InsertClosedTableRecord(documentRejectTypeInitiatorTable, documentRejectTypeInitiatorObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(documentRejectTypeInitiatorTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData releaseMessageTypeInitiatorTable = closedSystemTables.Where(d => d.id == "1967").FirstOrDefault();
            ObjectTable ReleaseMessageTypeInitiatorObjectTable = objectTableRepository.GetObjectTableByName("Customs.ReleaseMessageType", 0, false);
            InsertClosedTableRecord(releaseMessageTypeInitiatorTable, ReleaseMessageTypeInitiatorObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(releaseMessageTypeInitiatorTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData TradeAgreementProtocolTable = closedSystemTables.Where(d => d.id == "23900").FirstOrDefault();
            ObjectTable TradeAgreementProtocolObjectTable = objectTableRepository.GetObjectTableByName("Customs.TradeAgreementProtocol", 0, false);
            InsertClosedTableRecord(TradeAgreementProtocolTable, TradeAgreementProtocolObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(TradeAgreementProtocolTable);

            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData ExportLogisticPermitActionTable = closedSystemTables.Where(d => d.id == "23766").FirstOrDefault();
            ObjectTable ExportLogisticPermitActionObjectTable = objectTableRepository.GetObjectTableByName("Customs.ExportLogisticPermitAction", 0, false);
            InsertClosedTableRecord(ExportLogisticPermitActionTable, ExportLogisticPermitActionObjectTable, customsClosedTables, customsClosedTableRepository);
            addedClosedTables.Add(ExportLogisticPermitActionTable);


            //SYSTBL_NG_9001_MSG_SystemTablesResponseTableData collateralAnswerStatusTable = closedSystemTables.Where(d => d.id == "1553").FirstOrDefault();
            //ObjectTable collateralAnswerStatusObjectTable = objectTableRepository.GetObjectTableByName("Customs.CollateralAnswerStatus", 0, false);
            //InsertClosedTableRecord(collateralAnswerStatusTable, collateralAnswerStatusObjectTable, customsClosedTables, customsClosedTableRepository);
            //addedClosedTables.Add(collateralAnswerStatusTable);

            //var newclosedSystemTables = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>(closedSystemTables); 
            var justAdded = new List<string>();
            foreach (SYSTBL_NG_9001_MSG_SystemTablesResponseTableData closedTable in closedSystemTables)
            {

                bool exists = (from a in addedClosedTables
                               where a.id == closedTable.id
                               select a).Any();

                if (!exists)
                {
                    if (!justAdded.Contains(closedTable.id))
                    {
                        InsertClosedTableRecord(closedTable, null, customsClosedTables, customsClosedTableRepository, false);
                        justAdded.Add(closedTable.id);
                }
            }
            }

            customsClosedTableRepository.SubmitChanges();
        }

        static void InsertClosedTableRecord(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData systemRecord, ObjectTable objectTable,
            Dictionary<string, CustomsClosedTable> customsClosedTables, CustomsClosedTableRepository customsClosedTableRepository,
            bool insert2customsClosedTables = true
            )
        {
            if (!customsClosedTables.Keys.Contains(systemRecord.id))
            {
                CustomsClosedTable closedTable = new CustomsClosedTable()
                {
                    Id = systemRecord.id,
                    CustomsLocalName = systemRecord.extraStringData,
                    CustomsName = systemRecord.name,
                    DbName = objectTable != null ? objectTable.DBTableName : null,
                    ObjectTableId = objectTable != null ? objectTable.Id : null,
                 
                    SearchFields = (systemRecord.id + "," + systemRecord.extraStringData + "," + systemRecord.name + "," + (objectTable != null ? objectTable.DBTableName : null) + "," + (objectTable != null ? objectTable.Name : null)).ToLower(),
                    
                    StatusCode = "1",
                    Existed = objectTable == null ? false : true,
                };
                customsClosedTableRepository.Add(closedTable);
                if (insert2customsClosedTables)//NOT IN FOR
                {
                    customsClosedTables.Add(closedTable.Id, closedTable); ///itzik Why Not Adding : Cause Double Add !!    
                }

            }

            
            else
            {
                CustomsClosedTable closedTable = customsClosedTables[systemRecord.id];
                closedTable.CustomsLocalName = systemRecord.extraStringData;
                closedTable.CustomsName = systemRecord.name;
                closedTable.DbName = objectTable != null ? objectTable.DBTableName : null;
                closedTable.ObjectTableId = objectTable != null ? objectTable.Id : null;
                closedTable.Existed = objectTable != null ? true : false;
                closedTable.SearchFields = (systemRecord.id + "," + systemRecord.extraStringData + "," + systemRecord.name + "," + (objectTable != null ? objectTable.DBTableName : null) + "," + (objectTable != null ? objectTable.Name : null)).ToLower();
                customsClosedTableRepository.Update(closedTable);
            }
        }

        protected static void InitializeSettings()
        {
            int unifreightIIGServiceTimeout = 361;
            // int.TryParse(ConfigurationManager.AppSettings["UnifreightIIGServiceTimeout"], out unifreightIIGServiceTimeout);

            //UnifreightIIGCommonSetting.New()
            //         .SetConsumerID("038623617")
            //         .SetUnifreightIIGServiceAddress(ConfigurationManager.AppSettings["ConsumerID"])

            //         .SetUnifreightIIGServiceAddress(@"http://localhost:5050/UnifreightIIG/GatewayService/Basic")
            //         .SetUnifreightIIGServiceAddress(@"http://iiggateway.cloudapp.net:5050/UnifreightIIG/GatewayService/Basic")
            //         .SetUnifreightIIGServiceAddress(ConfigurationManager.AppSettings["UnifreightIIGServiceAddress"])
            //         .SetUnifreightIIGServiceTimeout(unifreightIIGServiceTimeout)
            //         .CreateSetting();

        }

        public static bool dontLoad { get { return false; } }

        public static void UpdateSingleClosedTable(string tableId, SystemTableRequestParams requestParams, SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse)
        {
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> entitySystemTables = null;
            bool errorHandel = false;
            int rowUpdateAdded = 0;
            ICustomContext customContext = CustomContext.GetContext(0);
            CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(customContext);
            CustomsClosedTable table = closedTableRep.GetSingle(new CustomsClosedTableKeys() { Id = tableId });
            try
            {
               
                if (tableId != "1892")
                {
                   

                    InitializeSettings();

                    ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);

                    table.StatusCode = "2";
                    closedTableRep.Update(table);
                    closedTableRep.SubmitChanges();
                    ObjectTable objectTable = objectTableRepository.GetSingleObjectTable(table.ObjectTableId, 0, false);
                    var myMehesSystemTables = new SystemTables();
                    string tableName = objectTable.Name.Substring(8);
                    string repositoryName = "Logitude.Customs.Data.Repsitories." + tableName + "Repository";
                    string pocoName = "Logitude.Customs.Data.EntityPOCOs." + tableName;
                    Assembly assembly = Assembly.Load("Logitude.Customs.Data");
                    Type repositoryType = assembly.GetType(repositoryName);
                    Type pocoType = assembly.GetType(pocoName);

                    object[] paramArray = { customContext };
                    object repository = Activator.CreateInstance(repositoryType, paramArray);

                    if (customResponse != null)
                    {
                        
                        //entitySystemTables = customResponse.TableData.OrderBy(rec => rec.id).ToList();
                        entitySystemTables = (new ClosedTableUniqeListGenericService()).MakeUniqeList(customResponse.TableData.ToList());    
                    }
                    else
                    {
                        entitySystemTables = myMehesSystemTables.GetTableData(tableId, requestParams.Tenant);
                    }
                    
                    MethodInfo getAllMethodInfo = repositoryType.GetMethod("GetAll");
                    object[] GetAllParamArray = { };
                    object entityTypeListObject = getAllMethodInfo.Invoke(repository, GetAllParamArray);


                    IEnumerable entityTypeListEnum = entityTypeListObject as IEnumerable;
                    ///maybe it will be helpfull  -in peltransport i get  ORA-01002: פעולת שליפה שלא ברצף הפעולות הנכון
                    List<object> entityTypeList = new List<object>();
                    foreach (object o in entityTypeListEnum)
                    {
                        entityTypeList.Add(o);
                    }
                    
                    List<object> addedEntitiesList = new List<object>();

                    BlockInactiveRows(entitySystemTables, repositoryType, repository, entityTypeList);

                    foreach (var systemrecord in entitySystemTables)
                    {
                        bool recordExists = false;
                        object existedRecord = null;
                        foreach (object o in entityTypeList)
                        {
                            Type objectType = o.GetType();
                            PropertyInfo codeInfo = objectType.GetProperty("Code");
                            string code = codeInfo.GetValue(o).ToString();
                            //ITZIK HOW write lowercase 'sp'!-'SP' in db if (systemrecord.id == code)
                            if (systemrecord.id.ToUpper() == code.ToUpper())
                            {
                                recordExists = true;
                                existedRecord = o;
                                break;
                            }
                        }

                        foreach (object o in addedEntitiesList)
                        {
                            Type objectType = o.GetType();
                            PropertyInfo codeInfo = objectType.GetProperty("Code");
                            string code = codeInfo.GetValue(o).ToString();
                            if (systemrecord.id == code)
                            {
                                recordExists = true;
                                break;
                            }
                        }

                        if (!recordExists)
                        {
                            object newPoco = Activator.CreateInstance(pocoType);
                            Type newPocoType = newPoco.GetType();
                            StringLengthAttribute strLenAttr = newPocoType.GetProperty("Code").GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>()
                                //.Single();
                                .FirstOrDefault();
                            strLenAttr = strLenAttr ?? GetNewstrLenAttr();
                            int maxLength = strLenAttr.MaximumLength;
                            if (systemrecord.id.Length <= maxLength)
                            {
                                PropertyInfo codeInfo = newPocoType.GetProperty("Code");
                                PropertyInfo localNameInfo = newPocoType.GetProperty("LocalName");
                                PropertyInfo searchFieldsInfo = newPocoType.GetProperty("SearchFields");
                              
                                if (tableId == "1118")
                                {
                                    //PropertyInfo bankCodeInfo = newPocoType.GetProperty("BankCode");
                                    //string numericData = systemrecord.extraNumericData != null ? systemrecord.extraNumericData.ToString() : null;
                                    //bankCodeInfo.SetValue(newPoco, systemrecord.extraNumericData);
                                    SetBankCode(systemrecord, newPoco, newPoco.GetType());
                                }
                                SetInactive(systemrecord, newPoco);
                                codeInfo.SetValue(newPoco, systemrecord.id);
                                localNameInfo.SetValue(newPoco, systemrecord.name);
                                searchFieldsInfo.SetValue(newPoco, (systemrecord.id + "," + systemrecord.name).ToLower());
                                if (tableId == "1136" || tableId == "1385")
                                {
                                    PropertyInfo malamIdInfo = newPocoType.GetProperty("MalamId");
                                    malamIdInfo.SetValue(newPoco, systemrecord.malamID);
                                }

                                if (tableName == "SubCountry")
                                {
                                    PropertyInfo countryCodeInfo = newPocoType.GetProperty("CountryCode");
                                    string[] idArray = systemrecord.id.Split('-');
                                    string countryCode = idArray[0];
                                    countryCodeInfo.SetValue(newPoco, countryCode);
                                }

                                MethodInfo addInfo = repositoryType.GetMethod("Add");
                                object[] addParams = { newPoco };
                                addInfo.Invoke(repository, addParams);
                                rowUpdateAdded++;
                                addedEntitiesList.Add(newPoco);
                            }
                            else
                            { }
                        }
                        else
                        {
                            if (existedRecord != null)
                            {
                                Type existedRecordType = existedRecord.GetType();
                                PropertyInfo localNameInfo = existedRecordType.GetProperty("LocalName");
                                PropertyInfo searchFieldsInfo = existedRecordType.GetProperty("SearchFields");

                                SetInactive(systemrecord, existedRecord);
                                localNameInfo.SetValue(existedRecord, systemrecord.name);
                                searchFieldsInfo.SetValue(existedRecord, (systemrecord.id + "," + systemrecord.name).ToLower());

                                if (tableName == "SubCountry")
                                {
                                    PropertyInfo countryCodeInfo = existedRecordType.GetProperty("CountryCode");
                                    string[] idArray = systemrecord.id.Split('-');
                                    string countryCode = idArray[0];
                                    countryCodeInfo.SetValue(existedRecord, countryCode);
                                }

                                if (tableId == "1118")
                                {
                                    SetBankCode(systemrecord, existedRecord, existedRecordType);
                                }

                                if (tableId == "1136" )
                                {
                                    PropertyInfo malamIdInfo = existedRecordType.GetProperty("MalamId");
                                    ///malamIdInfo.SetValue(exitedRecordType, systemrecord.malamID);
                                    malamIdInfo.SetValue(existedRecord, systemrecord.malamID.ToString()); //itzik    exitedRecordType <>existedRecord   + ToString()!!
                                }
                                if (tableId == "1385")
                                {
                                    PropertyInfo malamIdInfo = existedRecordType.GetProperty("MalamId");
                                    ///malamIdInfo.SetValue(exitedRecordType, systemrecord.malamID);
                                    malamIdInfo.SetValue(existedRecord, systemrecord.malamID); //itzik    exitedRecordType <>existedRecord   + ToString()!!
                                }
                                MethodInfo updateInfo = repositoryType.GetMethod("Update");
                                object[] addParams = { existedRecord };
                                rowUpdateAdded++;
                                updateInfo.Invoke(repository, addParams);
                            }

                        }
                    }



                    MethodInfo submitInfo = repositoryType.GetMethod("SubmitChanges");
                    object[] submitParams = { };
                    submitInfo.Invoke(repository, submitParams);
                    
                    //table.StatusCode = "3";
                    //table.LastUpdateDate = DateTime.Now;
                    //closedTableRep.Update(table);
                    //closedTableRep.SubmitChanges();
                    int? realTenant = null;
                    if (requestParams != null)
                    {
                        realTenant = requestParams.Tenant;
                    }
                    TableLastUpdateM myTableLastUpdateM = new TableLastUpdateM() { ObjectTableId = table.ObjectTableId };
                    if (requestParams != null)
                    {
                        myTableLastUpdateM.AlternativeUserId = requestParams.LoggingUserId;
                        myTableLastUpdateM.AlternativeUserTenant = requestParams.Tenant;

                    }
                    UpdateStatusCode(closedTableRep, table, DateTime.Now, (rowUpdateAdded + rowUpdateAdded > 0), myTableLastUpdateM);
                }
                else
                {
                    Update1892Table(customResponse);
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                errorHandel = true;
                UpdateStatusCode(closedTableRep, table, null, (rowUpdateAdded + rowUpdateAdded > 0), null);
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //if (Debugger.IsAttached) Debugger.Break();
            }
            catch (System.Exception ee)
            {
                errorHandel = true;
                throw;
            }
            finally
            {
                //if (customResponse != null)
                //{
                //    LogComm(tableId, entitySystemTables, errorHandel, rowUpdateAdded);
                //}
            }
        }
        
        private static void UpdateStatusCode(CustomsClosedTableRepository closedTableRep, CustomsClosedTable table, DateTime? LastUpdateDate, bool hasChanged,
            TableLastUpdateM myTableLastUpdateM)
        {
            table.StatusCode = "3";
            if (LastUpdateDate.HasValue)
            {
                table.LastUpdateDate = LastUpdateDate;
            }
            if (hasChanged)
            {
                TableLastUpdateClass.UpdateTableHistory(0, table.DbName, myTableLastUpdateM);
            }
            closedTableRep.Update(table);
            closedTableRep.SubmitChanges();
        }

        private static void SetInactive(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData systemrecord, object poco)
        {
            PropertyInfo inactiveInfo = poco.GetType().GetProperty("Inactive");
            if (inactiveInfo == null)
            {
                inactiveInfo = poco.GetType().GetProperty("InActive");
            }
            if (inactiveInfo != null)
            {
                var bInactive = GetInactive(systemrecord);
                inactiveInfo.SetValue(poco, bInactive);
            }
        }

        private static bool GetInactive(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData systemrecord)
        {
            return (systemrecord.state <= 0);
        }

        private static StringLengthAttribute GetNewstrLenAttr()
        {
            return new StringLengthAttribute(100); 
        }

        private static void SetBankCode(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData systemrecord, object existedRecord, Type exitedRecordType)
        {
            try
            {


                PropertyInfo bankCodeInfo = exitedRecordType.GetProperty("BankCode");
                PropertyInfo bankIdKeyInfo = exitedRecordType.GetProperty("Id");
                //string numericData = systemrecord.extraNumericData != null ? systemrecord.extraNumericData.ToString() : null;
                string bankCode = null;
                string id = null;
               
                if (systemrecord.extraNumericData != null)
                {
                    bankCode = systemrecord.extraNumericData.ToString();
                }

                object idValue = bankIdKeyInfo.GetValue(existedRecord);
                if (idValue == null)
                {
                    id = systemrecord.id + "," + bankCode;
                }

                Debug.WriteLine(systemrecord.id + "-" + (bankCode ?? ""));
                bankCodeInfo.SetValue(existedRecord, bankCode);
                bankIdKeyInfo.SetValue(existedRecord, id);
            }
            catch (System.Exception ee)
            {

                throw new System.Exception(
                    "1118:CustomsBranches:SetBankCode Exception : systemrecord .id=" + systemrecord.id +
                    " BankCode/extraNumericData :" + systemrecord.extraNumericData +
                    Environment.NewLine + ee.Message);
            }
        }


        private static void Update1892Table(SYSTBL_NG_9001_MSG_SystemTablesResponse customResponse = null)
        {
            InitializeSettings();
            ICustomContext customContext = CustomContext.GetContext(0);

            var myMehesSystemTables = new SystemTables();


            Logitude.CustomsMessaging.Helpers.SystemTables systemTables = new Logitude.CustomsMessaging.Helpers.SystemTables();
            CustomDocumentTypeMetaDataRepository customDocumentTypeMetaDataRepository = new CustomDocumentTypeMetaDataRepository(customContext);
            List<CustomDocumentTypeMetaData> customDocumentTypeMetaDataList = customDocumentTypeMetaDataRepository.GetAll().ToList();
            CustomMetaDataTypeRepository metaDataTypeRepository = new CustomMetaDataTypeRepository(customContext);
            List<CustomMetaDataType> metaDataTypeList = metaDataTypeRepository.GetAll().ToList();
            List<CustomMetaDataType> newAddedMetaDataTypeList = new List<CustomMetaDataType>();

            DataSet metadataData = null;
            if (customResponse == null)
            {
                metadataData = systemTables.GetAsTableData(TableID: "1892", pageNumber: 1, pageSize: 505);
            }
            else
            {
                metadataData = SystemTables.DataSetReadXML(customResponse.TableAsDataSetTableData);
            }
                
                
            if (metadataData != null)
            {
                var content = metadataData.Tables[0];
                IEnumerable<DataRow> contentRows = content.AsEnumerable().ToList();
                if (false && DateTime.Now < new DateTime(2015, 11, 19))
                {
                    contentRows =
                    contentRows.ToList().Where(row => GetMetaDataTypeCode(row) == "130" && GetDocumentTypeCode(row) == "IL_418");
                }
                InsertNewCustomDocumentType(contentRows, customContext);
                foreach (DataRow row in //content.Rows
                    contentRows
                    )
                {
                    
                    CustomMetaDataType type = (from a in metaDataTypeList
                                               where a.Code == GetMetaDataTypeCode(row)
                                               select a).FirstOrDefault();

                    CustomMetaDataType addedType = (from a in newAddedMetaDataTypeList
                                                    where a.Code == GetMetaDataTypeCode(row)
                                                    select a).FirstOrDefault();
                    if (type == null && addedType == null)
                    {
                        type = new CustomMetaDataType()
                        {
                            Code = GetMetaDataTypeCode(row),
                            LocalName = row.ItemArray[5].ToString(),
                            SearchFields = (GetMetaDataTypeCode(row) + "," + row.ItemArray[5].ToString()).ToLower(),
                        };
                        metaDataTypeRepository.Add(type);
                        newAddedMetaDataTypeList.Add(type);
                    }
                    else if (type != null)
                    {
                        type.LocalName = row.ItemArray[5].ToString();
                        type.SearchFields = (GetMetaDataTypeCode(row) + "," + row.ItemArray[5].ToString()).ToLower();
                        metaDataTypeRepository.Update(type);
                    }

                }
                metaDataTypeRepository.SubmitChanges();


                var customDocumentTypeMetaDataListToInactive = new List<CustomDocumentTypeMetaData>(customDocumentTypeMetaDataList.Where(r => r.Inactive == false).ToList());
                 
                foreach (DataRow row in //content.Rows
                    contentRows
                    )
                {

                    
                    CustomDocumentTypeMetaData metadataRecord = (from a in customDocumentTypeMetaDataList
                                                                 where 
                                                                 a.MetaDataTypeCode == GetMetaDataTypeCode(row)//[8]: "130"
                                                                 &&
                                                                 a.DocumentTypeCode == GetDocumentTypeCode(row) // [0]: "IL_418"
                                                                 select a).FirstOrDefault();
                    if (metadataRecord == null)
                    {
                        metadataRecord = new CustomDocumentTypeMetaData()
                        {
                            DocumentTypeCode = GetDocumentTypeCode(row),
                            MetaDataTypeCode = GetMetaDataTypeCode(row),
                            Format = row.ItemArray[4].ToString(),
                            Mandatory = bool.Parse(row.ItemArray[6].ToString()),
                            ValuesTable = row.ItemArray[8]?.ToString(),
                        };
                        //if (metadataRecord.MetaDataTypeCode == "87") //13419
                        //{
                        //    metadataRecord.Mandatory = true;
                        //}
                        customDocumentTypeMetaDataRepository.Add(metadataRecord);
                        customDocumentTypeMetaDataList.Add(metadataRecord);
                    }
                    else
                    {
                        var activeCDTMD= customDocumentTypeMetaDataListToInactive
                            .FirstOrDefault(
                            r => 
                                r.MetaDataTypeCode == metadataRecord.MetaDataTypeCode &&
                                r.DocumentTypeCode == metadataRecord.DocumentTypeCode);
                        if (activeCDTMD!=null)
                        {
                            customDocumentTypeMetaDataListToInactive.Remove(activeCDTMD); 
                        }
                        metadataRecord.Format = row.ItemArray[4].ToString();
                        metadataRecord.Mandatory = bool.Parse(row.ItemArray[6].ToString());
                        if (row.ItemArray[8] != null)
                        {
                            metadataRecord.ValuesTable = row.ItemArray[8].ToString();
                        }
                        //if (metadataRecord.MetaDataTypeCode == "87") //13419
                        //{
                        //    metadataRecord.Mandatory = true;
                        //}
                        customDocumentTypeMetaDataRepository.Update(metadataRecord);
                    }
                    try
                    {
                        customDocumentTypeMetaDataRepository.SubmitChanges();
                    }
                    catch (System.Exception)
                    {
                        
                        throw;
                    }
                    
                }
                foreach (var item in customDocumentTypeMetaDataListToInactive)
                {
                    item.Inactive = true;
                    customDocumentTypeMetaDataRepository.Update(item);
                    try
                    {
                        customDocumentTypeMetaDataRepository.SubmitChanges();
                    }
                    catch (System.Exception)
                    {

                        throw;
                    }
                }     

                try
                {
                    customDocumentTypeMetaDataRepository.SubmitChanges();
                }
                catch (System.Exception ex)
                {

                }
            }
        }

        private static void InsertNewCustomDocumentType(IEnumerable<DataRow> contentRows, ICustomContext customContext)
        {
            var mustHaveCustomDocumentTypeList = contentRows.Select(row => GetDocumentTypeCode(row)).Distinct().ToList();
            var repo = new CustomDocumentTypeRepository(customContext);

            var dbCodeList = repo.GetAll().Select(cdt => cdt.Code).ToList();
            var pleaseAdd = mustHaveCustomDocumentTypeList.Except(dbCodeList);
            if (pleaseAdd.Count() < 1)
            {
                return;
            }
            foreach (var item in pleaseAdd)
            {
                var mehes = contentRows.First(rec => GetDocumentTypeCode(rec) == item);
                repo.Add(new CustomDocumentType()
                {
                    Code = item,
                    LocalName = mehes.ItemArray[2].ToString(),
                    SearchFields = (GetMetaDataTypeCode(mehes) + "," + mehes.ItemArray[5].ToString()).ToLower()
                });

            }
            repo.SubmitChanges();

        }


        private static string GetDocumentTypeCode(DataRow row)
        {
            return row.ItemArray[0].ToString();
        }

        private static string GetMetaDataTypeCode(DataRow row)
        {
            /*

            */
            var MetaDataTypeCode = 
                //row.ItemArray[8].ToString();
                row.ItemArray[7].ToString();
            if (String.IsNullOrWhiteSpace(MetaDataTypeCode))
            {
                throw new System.Exception(
@"MetaDataTypeCode is null (row.ItemArray[7].ToString()) burn at 2016/03/02
?row.ItemArray
{object[9]}
    [0]: ""IL_1""
    [1]: ""1""
    [2]: ""'ק דוגמא""
    [3]: ""1""
    [4]: ""String""
    [5]: ""שם לקוח""
    [6]: ""true""
    [7]: ""21""
    [8]: ""1168""");
            }
            return MetaDataTypeCode;
        }

        private static void LogComm(string tableId, List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> entitySystemTables, bool errorHandel, int rowUpdateAdded)
        {
        
            try
            {
                string xml = "Message from custom didn't received";
                if (entitySystemTables != null)
                {
                    xml = XmlGenericUtil<List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>>.SerializeObject(entitySystemTables, true);
                }
                string log = "Success ," + rowUpdateAdded + " rows affected";
                if (errorHandel)
                {
                    log = "Failed , In the " + rowUpdateAdded + " rows ";
                }
                var communicationsParams = new CommunicationsParams()
                    {
                        Tenant = 0,
                        Subject = "IIG Table ( " + tableId + " )",
                    Status = errorHandel ? "F" : "D",
                        To = "Customs",
                        CommunicationLogTypeCode = "T",
                        FolderName = "customs",
                        From = "IIGC", //  "Logitude",
                        InOut = "I",

                    XMLData = xml,
                        Logs = log
                    };
                Communications.AddCommunicationLog(communicationsParams);
            }
            catch //(System.Exception)
            {
                //BL 4 logging  only do not re throw
                //if (Debugger.IsAttached) Debugger.Break();
                //throw;

            }

        }

        public static void UpdateSingleClosedTable(ICustomContext customContext, CustomsClosedTable closedTable, Dictionary<string, ObjectTable> objectTables, int tenant)
        {
            if (closedTable.Id != "1892")
            {
                InitializeSettings();

                ObjectTable objectTable = objectTables[closedTable.ObjectTableId];

                var myMehesSystemTables = new SystemTables();
                string tableName = objectTable.Name.Substring(8);
                var entitySystemTables = myMehesSystemTables.GetTableData(closedTable.Id, tenant);
                if (entitySystemTables.Count > 2000)
                {

                }
                var featureEnable = true;
                if (featureEnable)
                {
                    var entitySystemTablesExt = new List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt>();
                    foreach (var item in entitySystemTables)
                    {
                        entitySystemTablesExt.Add(SYSTBL_NG_9001_MSG_SystemTablesResponseTableDataExt.CreateNew(item));
                    }

                    var closedTableService = ClosedTableServiceFactory.CreateNew(customContext, tableName, entitySystemTablesExt, tenant);
                    if (closedTableService != null)
                    {
                        closedTableService.UpdateSingleClosedTable();
                        return;
                    }    
                }
                

                

                string repositoryName = "Logitude.Customs.Data.Repsitories." + tableName + "Repository";
                string pocoName = "Logitude.Customs.Data.EntityPOCOs." + tableName;
                Assembly assembly = Assembly.Load("Logitude.Customs.Data");
                Type repositoryType = assembly.GetType(repositoryName);
                Type pocoType = assembly.GetType(pocoName);

                object[] paramArray = { customContext };
                object repository = Activator.CreateInstance(repositoryType, paramArray);
                //var entitySystemTables = myMehesSystemTables.GetTableData(closedTable.Id);
                MethodInfo getAllMethodInfo = repositoryType.GetMethod("GetAll");
                object[] GetAllParamArray = { };
                object entityTypeListObject = getAllMethodInfo.Invoke(repository, GetAllParamArray);

                IEnumerable entityTypeList = entityTypeListObject as IEnumerable;
                List<object> addedEntitiesList = new List<object>();

                BlockInactiveRows(entitySystemTables, repositoryType, repository, entityTypeList);
                ObjectFieldRepository objectFieldRep = new ObjectFieldRepository(0);
                List<ObjectField> objectFields = objectFieldRep.GetPMObjectFieldsByObjectTableId(objectTable.Id, 0).ToList();
                foreach (var systemrecord in entitySystemTables)
                {
                    
                    bool recordExists = false;
                    object existedRecord = null;
                    foreach (object o in entityTypeList)
                    {
                        Type objectType = o.GetType();
                        PropertyInfo codeInfo = objectType.GetProperty("Code");
                        string code = codeInfo.GetValue(o).ToString();
                        if (systemrecord.id == code)
                        {
                            recordExists = true;
                            existedRecord = o;
                            break;
                        }
                    }

                    foreach (object o in addedEntitiesList)
                    {
                        Type objectType = o.GetType();
                        PropertyInfo codeInfo = objectType.GetProperty("Code");
                        string code = codeInfo.GetValue(o).ToString();
                        if (systemrecord.id == code)
                        {
                            recordExists = true;
                            break;
                        }


                    }

                    if (!recordExists)
                    {
                        object newPoco = Activator.CreateInstance(pocoType);
                        Type newPocoType = newPoco.GetType();
                        ObjectField field = objectFields.Where(d => d.FieldName == "Code").FirstOrDefault();
                       // StringLengthAttribute strLenAttr = newPocoType.GetProperty("Code").GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().Single();
                       // int maxLength = strLenAttr.MaximumLength;
                        if (systemrecord.id.Length <= field.MaxLength)
                        {
                            PropertyInfo codeInfo = newPocoType.GetProperty("Code");
                            PropertyInfo localNameInfo = newPocoType.GetProperty("LocalName");
                            PropertyInfo searchFieldsInfo = newPocoType.GetProperty("SearchFields");


                            codeInfo.SetValue(newPoco, systemrecord.id);
                            localNameInfo.SetValue(newPoco, systemrecord.name);
                            searchFieldsInfo.SetValue(newPoco, (systemrecord.id + "," + systemrecord.name).ToLower());

                            if (tableName == "SubCountry")
                            {
                                PropertyInfo countryCodeInfo = newPocoType.GetProperty("CountryCode");
                                string[] idArray = systemrecord.id.Split('-');
                                string countryCode = idArray[0];
                                countryCodeInfo.SetValue(newPoco, countryCode);
                            }

                            if (closedTable.Id == "1385")
                            {
                                PropertyInfo malamIdInfo = newPocoType.GetProperty("MalamId");
                                malamIdInfo.SetValue(newPoco, systemrecord.id);

                            }

                            MethodInfo addInfo = repositoryType.GetMethod("Add");
                            object[] addParams = { newPoco };
                            addInfo.Invoke(repository, addParams);

                            addedEntitiesList.Add(newPoco);
                        }
                    }
                    else
                    {
                        if (existedRecord != null)
                        {
                            Type exitedRecordType = existedRecord.GetType();
                            PropertyInfo localNameInfo = exitedRecordType.GetProperty("LocalName");
                            PropertyInfo searchFieldsInfo = exitedRecordType.GetProperty("SearchFields");


                            SetInactive(systemrecord, existedRecord);

                            localNameInfo.SetValue(existedRecord, systemrecord.name);
                            searchFieldsInfo.SetValue(existedRecord, (systemrecord.id + "," + systemrecord.name).ToLower());

                            if (tableName == "SubCountry")
                            {
                                PropertyInfo countryCodeInfo = exitedRecordType.GetProperty("CountryCode");
                                string[] idArray = systemrecord.id.Split('-');
                                string countryCode = idArray[0];
                                countryCodeInfo.SetValue(existedRecord, countryCode);
                            }

                            if (closedTable.Id == "1385")
                            {
                                PropertyInfo malamIdInfo = exitedRecordType.GetProperty("MalamId");
                                malamIdInfo.SetValue(existedRecord, systemrecord.id);

                            }

                            MethodInfo updateInfo = repositoryType.GetMethod("Update");
                            object[] addParams = { existedRecord };
                            updateInfo.Invoke(repository, addParams);
                        }

                    }
                }
                MethodInfo submitInfo = repositoryType.GetMethod("SubmitChanges");
                object[] submitParams = { };
                submitInfo.Invoke(repository, submitParams);
            }
            else
            {
                Update1892Table();
            }
        }

        private static void BlockInactiveRows(List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> entitySystemTables, Type repositoryType, object repository, IEnumerable entityTypeList)
        {
            List<string> ids = new List<string>();

            foreach (var item in entitySystemTables)
            {

                ids.Add(item.id);

            }

            foreach (object table in entityTypeList)
            {

                Type objectType = table.GetType();
                PropertyInfo codeInfo = objectType.GetProperty("Code");
                string code = codeInfo.GetValue(table).ToString();
                if (!ids.Contains(code))
                {
                    if (table != null)
                    {
                        Type exitedRecordType = table.GetType();
                        PropertyInfo InactiveInfo = exitedRecordType.GetProperty("Inactive");

                        InactiveInfo.SetValue(table, true);



                        MethodInfo updateInfo = repositoryType.GetMethod("Update");
                        object[] addParams = { table };
                        updateInfo.Invoke(repository, addParams);
                    }
                }
            }
        }

        public static void UpdateAllClosedTables(int tenant, RequestParamsBase requestParams = null)
        {
            ClientProgressBarIndicatorService clientProgressBarIndicatorService = null;
            if (requestParams != null)
            {
                clientProgressBarIndicatorService = new ClientProgressBarIndicatorService(requestParams);
                clientProgressBarIndicatorService.StartBroadcast("מחשב");
                
                clientProgressBarIndicatorService.StartBroadcast("בונה סכמת טבלאות מכס");
            }

            //FillCustomsClosedTablesInDb(tenant);

            try
            {
                
                FillCustomsClosedTablesInDb(
                    //requestParams.Tenant
                    tenant
                    , clientProgressBarIndicatorService);

                Stopwatch stopwatch = new Stopwatch();
                InitializeSettings();
                ICustomContext customContext = CustomContext.GetContext(0);
                CustomsClosedTableRepository closedTableRep = new CustomsClosedTableRepository(customContext);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(0);
                Dictionary<string, ObjectTable> objectTables = objectTableRepository.GetObjectsByTenant(0).ToDictionary(d => d.Id, o => o);
                List<CustomsClosedTable> closedTables = closedTableRep.GetExistedClosedTables();
                closedTables = closedTables.OrderBy(d => int.Parse(d.Id)).ToList();
                stopwatch.Start();

                //foreach (CustomsClosedTable closedTable in closedTables)
                var dbclosedTables =
                    closedTables
                    //.Where( rec => rec.Existed   )
                    .Where(rec => !String.IsNullOrEmpty(rec.ObjectTableId)).ToList();
                var tot = dbclosedTables.Count;
                int i = 0;
                foreach (var closedTable in dbclosedTables)
			    {
                    i++;

                    if (closedTable.Id == "1344")
                    {
                        //no need !!!- yaron !!
                        //to big !!!
                        //continue;
                        closedTable.Id = "2653";
                    }
#if Mohaamad_Pleasecommentitandnotremoveit
                    

                    closedTable.StatusCode = "2";
                    closedTableRep.Update(closedTable);
                    closedTableRep.SubmitChanges();
                    try
                    {
                        UpdateSingleClosedTable(customContext, closedTable, objectTables, tenant);
                    }
                    catch (System.Exception eeeess)
                    {
                        continue;
                    }
                    closedTable.StatusCode = "3";
                    closedTable.LastUpdateDate = DateTime.Now;
                    closedTableRep.Update(closedTable);
                    closedTableRep.SubmitChanges();
                    int? realTenant = null;
                    if (requestParams != null)
                    {
                        realTenant = requestParams.Tenant;
                    }
                    TableLastUpdateM myTableLastUpdateM = new TableLastUpdateM() { ObjectTableId = closedTable.ObjectTableId };
                    if (requestParams != null)
                    {
                        myTableLastUpdateM.AlternativeUserId = requestParams.LoggingUserId;
                        myTableLastUpdateM.AlternativeUserTenant = requestParams.Tenant;

                    }
                    //UpdateStatusCode(closedTableRep, closedTable, DateTime.Now, true, myTableLastUpdateM);
                    continue;
#endif
                    var openComm = true;
                    if (openComm && (closedTable.Id != "1892"))
                    {
                        
                        if (requestParams != null)
                        {
                            clientProgressBarIndicatorService.StartStep(
                                string.Format("בונה תקשורת לטבלה {0} {1}/{2}", 
                                ""///closedTable.DbName
                                , (i + 1), (tot + 1))
                                );
                        }
                        
                        string tableId = closedTable.Id;
                        //LoadCustomClosedTables.UpdateSingleClosedTable(tableId);
                        var messageService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
                        messageService.Send(new Logitude.CustomsMessaging.Common.RequestParams.SystemTableRequestParams()
                        {
                            TableId = tableId,
                            AsTableData = (tableId == "1892"),
                            Tenant = tenant,//_CustomsSetting.Tenant ,
                            RequestVIA = 
                            //Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceInteractive,
                            Logitude.CustomsMessaging.Common.RequestParams.SendRequestVIA.WebServiceBatch
                        });
                        continue;
                    }
#if false
                   
                    if (closedTable.Id == "1136" || closedTable.Id == "1137" || closedTable.Id == "1326" || closedTable.Id == "2009" || closedTable.Id == "2011")
                    {

                        continue;
                    }
                
                    if (requestParams != null)
                    {
                        clientProgressBarIndicatorService.StartStep(
                                string.Format("בונה טבלה {0} {1}/{2}", closedTable.DbName, (i + 1), (tot + 1))
                                );
                    }
                    closedTable.StatusCode = "2";
                    closedTableRep.Update(closedTable);
                    closedTableRep.SubmitChanges();
                    UpdateSingleClosedTable(customContext, closedTable, objectTables, tenant);
                    //closedTable.StatusCode = "3";
                    //closedTable.LastUpdateDate = DateTime.Now;
                    //closedTableRep.Update(closedTable);
                    //closedTableRep.SubmitChanges();
                    int? realTenant = null;
                    if (requestParams != null)
                    {
                        realTenant = requestParams.Tenant;
                    }
                    TableLastUpdateM myTableLastUpdateM = new TableLastUpdateM() { ObjectTableId = closedTable.ObjectTableId };
                    if (requestParams != null)
                    {
                        myTableLastUpdateM.AlternativeUserId = requestParams.LoggingUserId;
                        myTableLastUpdateM.AlternativeUserTenant = requestParams.Tenant;

                    }
                    UpdateStatusCode(closedTableRep, closedTable, DateTime.Now, true, myTableLastUpdateM);
                    #endif
                }

                stopwatch.Stop();
                var elapsed = stopwatch.Elapsed;
                if (requestParams != null)
                {
                    clientProgressBarIndicatorService.StartStep(@"בונה ערכי ברירת מחדל"
                        );
                }
                FillCustomClosedTablesData(tenant);
            }



            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //if (Debugger.IsAttached) Debugger.Break();
            }
            catch (System.Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Update all customs tables", null, null);
            }

            }



        }
    }
