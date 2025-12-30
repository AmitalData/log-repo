 using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.Repositories;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        private void CreateTableCounters()
        {
            List<ObjectTable> tenantObjectTables = ObjectTableRepository.GetObjectsByTenant(0).ToList();
            CounterRepository = new CounterRepository(ObjectContext);
            List<Counter> zeroCounters = CounterRepository.GetCounters(0).ToList();
            ObjectContext = WebFreightContext.GetContext(0);
            ObjectTable taxDeductionReportObject = ObjectContext.ObjectTables.Where(d => d.Name == "TaxDeductionReport" && d.Tenant == 0).FirstOrDefault();

            #region Shipment Counters

            if (!zeroCounters.Where(c => c.Code == "SHIP" && c.Tenant == 0).Any())
            {
                Counter shipmentCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "Shipment").FirstOrDefault().Id,//ShipmentObject.Id,
                    Code = "SHIP",
                    Tenant = 0,
                    Name = "Shipment",
                };

                CounterRepository.Add(shipmentCounter);

                CounterDefinition shipment_Export_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = shipmentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "A",
                };

                CounterDefinition shipment_Export_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = shipmentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "O",
                };

                CounterDefinition shipment_Export_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = shipmentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "I",
                };

                CounterDefinition shipment_Import_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = shipmentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "A",
                };

                CounterDefinition shipment_Import_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = shipmentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "O",
                };

                CounterDefinition shipment_Import_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = shipmentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "I",
                };



                //CounterDefinition shipment_Domestic_Air_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = shipmentCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "A",
                //};
                //CounterDefinition shipment_Domestic_Ocean_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = shipmentCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "O",
                //};

                //CounterDefinition shipment_Domestic_Inland_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = shipmentCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "I",
                //};

                CounterDefinitionRepository.Add(shipment_Export_Air_Counter);
                CounterDefinitionRepository.Add(shipment_Export_Ocean_Counter);
                CounterDefinitionRepository.Add(shipment_Export_Inland_Counter);
                CounterDefinitionRepository.Add(shipment_Import_Air_Counter);
                CounterDefinitionRepository.Add(shipment_Import_Ocean_Counter);
                CounterDefinitionRepository.Add(shipment_Import_Inland_Counter);

                //CounterDefinitionRepository.Add(shipment_Domestic_Air_Counter);
                //CounterDefinitionRepository.Add(shipment_Domestic_Ocean_Counter);
                //CounterDefinitionRepository.Add(shipment_Domestic_Inland_Counter);

            }
            #endregion

            #region ShipmentPickUpDelivery Counters
            if (!zeroCounters.Where(c => c.Code == "SHDV" && c.Tenant == 0).Any())
            {
                Counter shipmentDeliveryCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "ShipmentPickUpDelivery").FirstOrDefault().Id,
                    Code = "SHDV",
                    Tenant = 0,
                    Name = "Shipment Delivery Number",
                };

                CounterDefinition shipmentDelivery_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = shipmentDeliveryCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "DLV",
                };

                CounterRepository.Add(shipmentDeliveryCounter);
                CounterDefinitionRepository.Add(shipmentDelivery_CounterDef);
            }
            #endregion

            #region Master Counters

            if (!zeroCounters.Where(c => c.Code == "MAST" && c.Tenant == 0).Any())
            {
                Counter MasterCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = MasterObject.Id,
                    Code = "MAST",
                    Tenant = 0,
                    Name = "Master",

                };

                CounterRepository.Add(MasterCounter);

                CounterDefinition Master_Export_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "A",
                };
                CounterDefinition Master_Export_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "O",
                };

                CounterDefinition Master_Export_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "I",
                };

                CounterDefinition Master_Import_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "A",
                };

                CounterDefinition Master_Import_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "O",
                };

                CounterDefinition Master_Import_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "I",
                };




                //
                //CounterDefinition Master_Domestic_Air_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = MasterCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "A",
                //};
                //CounterDefinition Master_Domestic_Ocean_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = MasterCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "O",
                //};

                //CounterDefinition Master_Domestic_Inland_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = MasterCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "I",
                //};

                CounterDefinitionRepository.Add(Master_Export_Air_Counter);
                CounterDefinitionRepository.Add(Master_Export_Ocean_Counter);
                CounterDefinitionRepository.Add(Master_Export_Inland_Counter);
                CounterDefinitionRepository.Add(Master_Import_Air_Counter);
                CounterDefinitionRepository.Add(Master_Import_Ocean_Counter);
                CounterDefinitionRepository.Add(Master_Import_Inland_Counter);

                //CounterDefinitionRepository.Add(Master_Domestic_Air_Counter);
                //CounterDefinitionRepository.Add(Master_Domestic_Ocean_Counter);
                //CounterDefinitionRepository.Add(Master_Domestic_Inland_Counter);
            }
            #endregion

            #region HAWBExportCounter
            if (!zeroCounters.Where(c => c.Code == "HAWB" && c.Tenant == 0).Any())
            {
                Counter hawbCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "Shipment").FirstOrDefault().Id,//ShipmentObject.Id,
                    Code = "HAWB",
                    Tenant = 0,
                    Name = "Export HAWB/FBL/HBL",

                };

                CounterRepository.Add(hawbCounter);

            }

            #endregion

            #region Quote Counters
            if (!zeroCounters.Where(c => c.Code == "QUOT" && c.Tenant == 0).Any())
            {
                Counter quoteCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "Quote").FirstOrDefault().Id,//QuoteObject.Id,
                    Code = "QUOT",
                    Tenant = 0,
                    Name = "Quote",

                };

                CounterRepository.Add(quoteCounter);

                CounterDefinition Quote_Export_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = quoteCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "A",
                };
                CounterDefinition Quote_Export_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = quoteCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "O",
                };

                CounterDefinition Quote_Export_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = quoteCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "I",
                };

                CounterDefinition Quote_Import_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = quoteCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "A",
                };

                CounterDefinition Quote_Import_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = quoteCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "O",
                };

                CounterDefinition Quote_Import_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = quoteCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "I",
                };

                //
                //CounterDefinition Quote_Domestic_Air_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = quoteCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "A",
                //};
                //CounterDefinition Quote_Domestic_Ocean_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = quoteCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "O",
                //};

                //CounterDefinition Quote_Domestic_Inland_Counter = new CounterDefinition()
                //{
                //    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                //    CounterId = quoteCounter.Id,
                //    Tenant = 0,
                //    StartNumber = 1000,
                //    Parameter1 = "D",
                //    Parameter2 = "I",
                //};

                CounterDefinitionRepository.Add(Quote_Export_Air_Counter);
                CounterDefinitionRepository.Add(Quote_Export_Ocean_Counter);
                CounterDefinitionRepository.Add(Quote_Export_Inland_Counter);
                CounterDefinitionRepository.Add(Quote_Import_Air_Counter);
                CounterDefinitionRepository.Add(Quote_Import_Ocean_Counter);
                CounterDefinitionRepository.Add(Quote_Import_Inland_Counter);

                //CounterDefinitionRepository.Add(Quote_Domestic_Air_Counter);
                //CounterDefinitionRepository.Add(Quote_Domestic_Ocean_Counter);
                //CounterDefinitionRepository.Add(Quote_Domestic_Inland_Counter);
            }
            #endregion

            #region AR Invoice Counters

            if (!zeroCounters.Where(c => c.Code == "CNST" && c.Tenant == 0).Any())
            {
                Counter myCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "Invoice").FirstOrDefault().Id,//InvoiceObject.Id,
                    Code = "CNST",
                    Tenant = 0,
                    Name = "Constituent Invoice",
                };

                CounterDefinition myCounterDefinition_01 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "CNS ",
                };

                CounterRepository.Add(myCounter);
                CounterDefinitionRepository.Add(myCounterDefinition_01);
            }

            if (!zeroCounters.Where(c => c.Code == "INVC" && c.Tenant == 0).Any())
            {
                Counter myCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "Invoice").FirstOrDefault().Id,//InvoiceObject.Id,
                    Code = "INVC",
                    Tenant = 0,
                    Name = "A/R Invoice",
                };

                CounterDefinition myCounterDefinition_01 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "IN",
                };

                CounterDefinition myCounterDefinition_02 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "TX",
                };

                CounterDefinition myCounterDefinition_03 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "CD",
                };

                CounterDefinition myCounterDefinition_04 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "MN",
                };

                CounterDefinition myCounterDefinition_05 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "CON",
                };

                CounterDefinition myCounterDefinition_06 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "CI",
                    Prefix = "CI",
                };

                CounterDefinition myCounterDefinition_07 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "CC",
                    Prefix = "CC",
                };

                CounterDefinition myCounterDefinition_10 = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = myCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "COD",
                    Prefix = "COD",
                };

                CounterRepository.Add(myCounter);
                CounterDefinitionRepository.Add(myCounterDefinition_01);
                CounterDefinitionRepository.Add(myCounterDefinition_02);
                CounterDefinitionRepository.Add(myCounterDefinition_03);
                CounterDefinitionRepository.Add(myCounterDefinition_04);
                CounterDefinitionRepository.Add(myCounterDefinition_05);
                CounterDefinitionRepository.Add(myCounterDefinition_06);
                CounterDefinitionRepository.Add(myCounterDefinition_07);
                CounterDefinitionRepository.Add(myCounterDefinition_10);


                if (SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Cloud))
                {
                    CounterDefinition myCounterDefinition_08 = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                        CounterId = myCounter.Id,
                        Tenant = 0,
                        StartNumber = 1000,
                        Parameter1 = "IT",
                        Prefix = "IT",
                    };

                    CounterDefinition myCounterDefinition_09 = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                        CounterId = myCounter.Id,
                        Tenant = 0,
                        StartNumber = 1000,
                        Parameter1 = "IC",
                        Prefix = "IC",
                    };

                    CounterDefinitionRepository.Add(myCounterDefinition_08);
                    CounterDefinitionRepository.Add(myCounterDefinition_09);
                }
           

         
               

            }
            #endregion

            #region AP Invoice Counters
            if (!zeroCounters.Where(c => c.Code == "APIC" && c.Tenant == 0).Any())
            {
                Counter APinvoiceCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "APInvoice").FirstOrDefault().Id,//APInvoiceObject.Id,
                    Code = "APIC",
                    Tenant = 0,
                    Name = "A/P Invoice",

                };

                CounterDefinition APInvoice_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = APinvoiceCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "IN",
                };

                CounterRepository.Add(APinvoiceCounter);
                CounterDefinitionRepository.Add(APInvoice_Counter);
            }
            #endregion

            #region Documents Filling Counters


            if (!zeroCounters.Where(c => c.Code == "DOFI" && c.Tenant == 0).Any())
            {
                Counter DocumentsFillingCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = DocumentsFilingObject.Id,
                    Code = "DOFI",
                    Tenant = 0,
                    Name = "Documents Filling",

                };

                CounterDefinition DocumentsFilling_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = DocumentsFillingCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "IN",
                };

                CounterRepository.Add(DocumentsFillingCounter);
                CounterDefinitionRepository.Add(DocumentsFilling_Counter);
            }


            #endregion

            #region AR Payment Counters

            if (!zeroCounters.Where(c => c.Code == "ARPT" && c.Tenant == 0).Any())
            {
                Counter arPaymentCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "ARPayment").FirstOrDefault().Id,//ARPaymentObject.Id,
                    Code = "ARPT",
                    Tenant = 0,
                    Name = "A/R Payment",
                };

                CounterDefinition ARPayment_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = arPaymentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "DR",
                };

                CounterRepository.Add(arPaymentCounter);
                CounterDefinitionRepository.Add(ARPayment_CounterDef);

            }
            #endregion

            #region AP Payment Counters

            if (!zeroCounters.Where(c => c.Code == "APPT" && c.Tenant == 0).Any())
            {
                Counter apPaymentCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "APPayment").FirstOrDefault().Id,//APPaymentObject.Id,
                    Code = "APPT",
                    Tenant = 0,
                    Name = "A/P Payment",
                };

                CounterDefinition APPayment_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = apPaymentCounter.Id,
                    Tenant = 0,
                    StartNumber = 1000,
                    Parameter1 = "DR",
                };

                CounterRepository.Add(apPaymentCounter);
                CounterDefinitionRepository.Add(APPayment_CounterDef);
            }
            #endregion

            #region Card Counters

            if (!zeroCounters.Where(c => c.Code == "CADC" && c.Tenant == 0).Any())
            {
                Counter cardCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = tenantObjectTables.Where(o => o.Name == "Card").FirstOrDefault().Id,//APPaymentObject.Id,
                    Code = "CADC",
                    Tenant = 0,
                    Name = "Card",
                };

                CounterDefinition AccountingPartner_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "AC",
                    UsePerBranch = true,
                };

                CounterRepository.Add(cardCounter);
                CounterDefinitionRepository.Add(AccountingPartner_CounterDef);
                CounterDefinition Agent_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "AG",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(Agent_CounterDef);
                CounterDefinition AirLine_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "AL",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(AirLine_CounterDef);
                CounterDefinition CustomAgent_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "CG",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(CustomAgent_CounterDef);
                CounterDefinition Coloader_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "CO",  
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(Coloader_CounterDef);

                CounterDefinition Customer_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "CS",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(Customer_CounterDef);

                CounterDefinition Others_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "OT",  
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(Others_CounterDef);

                CounterDefinition PotentialCustomer_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "PO",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(PotentialCustomer_CounterDef);

                CounterDefinition ShippingAgent_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "SG",  
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(ShippingAgent_CounterDef);
                CounterDefinition ShippingLine_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "SL",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(ShippingLine_CounterDef);
                CounterDefinition Trucker_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "TR",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(Trucker_CounterDef);
                CounterDefinition Vendor_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "VD",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(Vendor_CounterDef);
                CounterDefinition Warehouse_CounterDef = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = cardCounter.Id,
                    Tenant = 0,
                    StartNumber = -1,
                    Parameter1 = "WH",
                    UsePerBranch = true,
                };
                CounterDefinitionRepository.Add(Warehouse_CounterDef);

            }
            #endregion



            this.ObjectContext.SaveChanges();
        }

        private void CreateDomesticCounterDefinitions()
        {
            CounterRepository = new CounterRepository(ObjectContext);
            CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);

            #region SHIPMENT


            List<Counter> shipCounters = CounterRepository.GetCountersByCode("SHIP");

            foreach (Counter shipmentCounter in shipCounters)
            {

                if (CounterDefinitionRepository.GetSingleCounterDefinition(shipmentCounter.Tenant, shipmentCounter.Id, "D", "A") == null)
                {




                    CounterDefinition shipment_Domestic_Air_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", shipmentCounter.Tenant).ToString(),
                        CounterId = shipmentCounter.Id,
                        Tenant = shipmentCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "A",
                        Prefix = "AD",
                    };



                    CounterDefinition shipment_Domestic_Ocean_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", shipmentCounter.Tenant).ToString(),
                        CounterId = shipmentCounter.Id,
                        Tenant = shipmentCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "O",
                        Prefix = "OD"
                    };

                    CounterDefinition shipment_Domestic_Inland_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", shipmentCounter.Tenant).ToString(),
                        CounterId = shipmentCounter.Id,
                        Tenant = shipmentCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "I",
                        Prefix = "ID"
                    };

                    UpdateCounterDefinitionData(shipmentCounter, shipment_Domestic_Air_Counter);
                    UpdateCounterDefinitionData(shipmentCounter, shipment_Domestic_Ocean_Counter);
                    UpdateCounterDefinitionData(shipmentCounter, shipment_Domestic_Inland_Counter);

                    CounterDefinitionRepository.Add(shipment_Domestic_Air_Counter);
                    CounterDefinitionRepository.Add(shipment_Domestic_Ocean_Counter);
                    CounterDefinitionRepository.Add(shipment_Domestic_Inland_Counter);

                }
            }

            #endregion

            #region MASTER



            List<Counter> mastCounters = CounterRepository.GetCountersByCode("MAST");

            foreach (Counter masterCounter in mastCounters)
            {
                if (CounterDefinitionRepository.GetSingleCounterDefinition(masterCounter.Tenant, masterCounter.Id, "D", "A") == null)
                {

                    CounterDefinition Master_Domestic_Air_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", masterCounter.Tenant).ToString(),
                        CounterId = masterCounter.Id,
                        Tenant = masterCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "A",
                    };
                    CounterDefinition Master_Domestic_Ocean_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", masterCounter.Tenant).ToString(),
                        CounterId = masterCounter.Id,
                        Tenant = masterCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "O",
                    };

                    CounterDefinition Master_Domestic_Inland_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", masterCounter.Tenant).ToString(),
                        CounterId = masterCounter.Id,
                        Tenant = masterCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "I",
                    };

                    UpdateCounterDefinitionData(masterCounter, Master_Domestic_Air_Counter);
                    UpdateCounterDefinitionData(masterCounter, Master_Domestic_Ocean_Counter);
                    UpdateCounterDefinitionData(masterCounter, Master_Domestic_Inland_Counter);

                    CounterDefinitionRepository.Add(Master_Domestic_Air_Counter);
                    CounterDefinitionRepository.Add(Master_Domestic_Ocean_Counter);
                    CounterDefinitionRepository.Add(Master_Domestic_Inland_Counter);
                }
            }
            #endregion

            #region QOUTE

            List<Counter> quoteCounters = CounterRepository.GetCountersByCode("QUOT");

            foreach (Counter quoteCounter in quoteCounters)
            {
                if (CounterDefinitionRepository.GetSingleCounterDefinition(quoteCounter.Tenant, quoteCounter.Id, "D", "A") == null)
                {
                    CounterDefinition Quote_Domestic_Air_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", quoteCounter.Tenant).ToString(),
                        CounterId = quoteCounter.Id,
                        Tenant = quoteCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "A",
                    };
                    CounterDefinition Quote_Domestic_Ocean_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", quoteCounter.Tenant).ToString(),
                        CounterId = quoteCounter.Id,
                        Tenant = quoteCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "O",
                    };

                    CounterDefinition Quote_Domestic_Inland_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", quoteCounter.Tenant).ToString(),
                        CounterId = quoteCounter.Id,
                        Tenant = quoteCounter.Tenant,
                        StartNumber = 1000,
                        Parameter1 = "D",
                        Parameter2 = "I",
                    };

                    UpdateCounterDefinitionData(quoteCounter, Quote_Domestic_Air_Counter);
                    UpdateCounterDefinitionData(quoteCounter, Quote_Domestic_Ocean_Counter);
                    UpdateCounterDefinitionData(quoteCounter, Quote_Domestic_Inland_Counter);

                    CounterDefinitionRepository.Add(Quote_Domestic_Air_Counter);
                    CounterDefinitionRepository.Add(Quote_Domestic_Ocean_Counter);
                    CounterDefinitionRepository.Add(Quote_Domestic_Inland_Counter);
                }
            }
            #endregion

            ObjectContext.SaveChanges();
        }

        private void CreateDropDirectionCounterDefinitions()
        {
            CounterRepository = new CounterRepository(ObjectContext);
            CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);

            #region Shipment

            List<Counter> shipCounters = CounterRepository.GetCountersByCode("SHIP");

            foreach (Counter shipmentCounter in shipCounters)
            {
                if (CounterDefinitionRepository.GetSingleCounterDefinition(shipmentCounter.Tenant, shipmentCounter.Id, "R", "A") == null)
                {
                    CounterDefinition oldDef = CounterDefinitionRepository.GetSingleCounterDefinition(shipmentCounter.Tenant, shipmentCounter.Id, "E", "A");
                    bool uniquePrefix = oldDef != null ? oldDef.UniquePerPrefix : false;
                    int startnum = oldDef != null ? (oldDef.UniquePerPrefix ? 1000 : oldDef.StartNumber) : 1000;

                    CounterDefinition shipment_Drop_Air_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", shipmentCounter.Tenant).ToString(),
                        CounterId = shipmentCounter.Id,
                        Tenant = shipmentCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "A",
                        Prefix = "AR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    CounterDefinition shipment_Drop_Ocean_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", shipmentCounter.Tenant).ToString(),
                        CounterId = shipmentCounter.Id,
                        Tenant = shipmentCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "O",
                        Prefix = "OR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    CounterDefinition shipment_Drop_Inland_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", shipmentCounter.Tenant).ToString(),
                        CounterId = shipmentCounter.Id,
                        Tenant = shipmentCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "I",
                        Prefix = "IR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    //UpdateCounterDefinitionData(shipmentCounter, shipment_Drop_Air_Counter);
                    //UpdateCounterDefinitionData(shipmentCounter, shipment_Drop_Ocean_Counter);
                    //UpdateCounterDefinitionData(shipmentCounter, shipment_Drop_Inland_Counter);

                    CounterDefinitionRepository.Add(shipment_Drop_Air_Counter);
                    CounterDefinitionRepository.Add(shipment_Drop_Ocean_Counter);
                    CounterDefinitionRepository.Add(shipment_Drop_Inland_Counter);
                }
            }

            #endregion

            #region Master

            List<Counter> mastCounters = CounterRepository.GetCountersByCode("MAST");

            foreach (Counter masterCounter in mastCounters)
            {
                if (CounterDefinitionRepository.GetSingleCounterDefinition(masterCounter.Tenant, masterCounter.Id, "R", "A") == null)
                {
                    CounterDefinition oldDef = CounterDefinitionRepository.GetSingleCounterDefinition(masterCounter.Tenant, masterCounter.Id, "E", "A");
                    bool uniquePrefix = oldDef != null ? oldDef.UniquePerPrefix : false;
                    int startnum = oldDef != null ? (oldDef.UniquePerPrefix ? 1000 : oldDef.StartNumber) : 1000;

                    CounterDefinition Master_Drop_Air_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", masterCounter.Tenant).ToString(),
                        CounterId = masterCounter.Id,
                        Tenant = masterCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "A",
                        Prefix = "AR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    CounterDefinition Master_Drop_Ocean_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", masterCounter.Tenant).ToString(),
                        CounterId = masterCounter.Id,
                        Tenant = masterCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "O",
                        Prefix = "OR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    CounterDefinition Master_Drop_Inland_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", masterCounter.Tenant).ToString(),
                        CounterId = masterCounter.Id,
                        Tenant = masterCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "I",
                        Prefix = "IR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    UpdateCounterDefinitionData(masterCounter, Master_Drop_Air_Counter);
                    UpdateCounterDefinitionData(masterCounter, Master_Drop_Ocean_Counter);
                    UpdateCounterDefinitionData(masterCounter, Master_Drop_Inland_Counter);

                    CounterDefinitionRepository.Add(Master_Drop_Air_Counter);
                    CounterDefinitionRepository.Add(Master_Drop_Ocean_Counter);
                    CounterDefinitionRepository.Add(Master_Drop_Inland_Counter);
                }
            }
            #endregion

            #region Quote

            List<Counter> quoteCounters = CounterRepository.GetCountersByCode("QUOT");

            foreach (Counter quoteCounter in quoteCounters)
            {
                if (CounterDefinitionRepository.GetSingleCounterDefinition(quoteCounter.Tenant, quoteCounter.Id, "R", "A") == null)
                {
                    CounterDefinition oldDef = CounterDefinitionRepository.GetSingleCounterDefinition(quoteCounter.Tenant, quoteCounter.Id, "E", "A");
                    bool uniquePrefix = oldDef != null ? oldDef.UniquePerPrefix : false;
                    int startnum = oldDef != null ? (oldDef.UniquePerPrefix ? 1000 : oldDef.StartNumber) : 1000;

                    CounterDefinition Quote_Drop_Air_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", quoteCounter.Tenant).ToString(),
                        CounterId = quoteCounter.Id,
                        Tenant = quoteCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "A",
                        Prefix = "AR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    CounterDefinition Quote_Drop_Ocean_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", quoteCounter.Tenant).ToString(),
                        CounterId = quoteCounter.Id,
                        Tenant = quoteCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "O",
                        Prefix = "OR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    CounterDefinition Quote_Drop_Inland_Counter = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", quoteCounter.Tenant).ToString(),
                        CounterId = quoteCounter.Id,
                        Tenant = quoteCounter.Tenant,
                        StartNumber = startnum,
                        Parameter1 = "R",
                        Parameter2 = "I",
                        Prefix = "IR",
                        UniquePerPrefix = uniquePrefix,
                    };

                    UpdateCounterDefinitionData(quoteCounter, Quote_Drop_Air_Counter);
                    UpdateCounterDefinitionData(quoteCounter, Quote_Drop_Ocean_Counter);
                    UpdateCounterDefinitionData(quoteCounter, Quote_Drop_Inland_Counter);

                    CounterDefinitionRepository.Add(Quote_Drop_Air_Counter);
                    CounterDefinitionRepository.Add(Quote_Drop_Ocean_Counter);
                    CounterDefinitionRepository.Add(Quote_Drop_Inland_Counter);
                }
            }
            #endregion

            ObjectContext.SaveChanges();
        }

        private void UpdateCounterDefinitionData(Counter counter, CounterDefinition counterDefinition)
        {
            //CounterRepository = new CounterRepository(ObjectContext);
            //CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);
            //CounterDefinitionQuery query = new CounterDefinitionQuery(CounterDefinitionRepository);
            ////bool uniquePerPrefix = 

            //List<CounterDefinitionPM> counterDefinitions = query.GetCounterDefinitionsByCounterId(counter.Id, counter.Tenant).ToList();


            //bool uniquePerPrefix = counterDefinitions.Where(
            //       c => c.Tenant == counter.Tenant &&
            //        c.CounterId == counter.Id && c.UniquePerPrefix == true
            //        ).Any();


            //IEnumerable<System.Linq.IGrouping<string, CounterDefinitionPM>> groupingList = counterDefinitions.Where(
            //       c => c.Tenant == counter.Tenant &&
            //        c.CounterId == counter.Id
            //        ).GroupBy(c => c.Prefix);


            //counterDefinition.UniquePerPrefix = uniquePerPrefix;
            //if (uniquePerPrefix)
            //{
            //    switch (groupingList.Count())
            //    {
            //        case 1:
            //            {
            //                counterDefinition.Prefix = counterDefinitions.FirstOrDefault().Prefix;
            //                counterDefinition.StartNumber = counterDefinitions.FirstOrDefault().StartNumber;
            //                //SameAllTrans.IsChecked = true;
            //                //SameImpExp.IsChecked = true;
            //                break;
            //            }

            //        case 2:
            //            {
            //                System.Linq.IGrouping<string, CounterDefinitionPM> elemnt = groupingList.FirstOrDefault();
            //                List<CounterDefinitionPM> groub = groupingList.Where(g => g.Key == elemnt.Key).FirstOrDefault().ToList();

            //                bool equals1 = true;
            //                if (groub.Count == 3)
            //                {
            //                    equals1 = (groub[1].Parameter1 == groub[2].Parameter1) ? true : false;
            //                }

            //                if ((groub[0].Parameter1 == groub[1].Parameter1) && equals1)
            //                {
            //                    if (uniquePerPrefix)
            //                    {
            //                        counterDefinition.StartNumber = 1000;
            //                    }
            //                    else
            //                    {
            //                        counterDefinition.StartNumber = counterDefinitions.FirstOrDefault().StartNumber;
            //                    }

            //                    counterDefinition.Prefix = "DOM";

            //                    //chkSameAllTrans.IsChecked = true;
            //                    //chkDiffImpExp.IsChecked = true;

            //                }

            //                bool equals2 = true;
            //                if (groub.Count == 3)
            //                {
            //                    equals2 = (groub[1].Parameter2 == groub[2].Parameter2) ? true : false;
            //                }

            //                if ((groub[0].Parameter2 == groub[1].Parameter2) && equals2)
            //                {
            //                    foreach (IGrouping<string, CounterDefinitionPM> el in groupingList)
            //                    {
            //                        List<CounterDefinitionPM> internalList = el.ToList();
            //                        if (counterDefinition.Parameter2 == internalList.FirstOrDefault().Parameter2)
            //                        {
            //                            counterDefinition.Prefix = internalList.FirstOrDefault().Prefix;
            //                            counterDefinition.StartNumber = internalList.FirstOrDefault().StartNumber;
            //                            break;
            //                        }

            //                    }

            //                    //chkSameImpExp.IsChecked = true;
            //                    //chDiffAllTrans.IsChecked = true;
            //                }

            //                break;
            //            }

            //        case 3:
            //            {
            //                System.Linq.IGrouping<string, CounterDefinitionPM> elemnt = groupingList.FirstOrDefault();
            //                List<CounterDefinitionPM> groub = groupingList.Where(g => g.Key == elemnt.Key).FirstOrDefault().ToList();

            //                bool equals1 = true;
            //                if (groub.Count == 3)
            //                {
            //                    equals1 = (groub[1].Parameter1 == groub[2].Parameter1) ? true : false;
            //                }

            //                if ((groub[0].Parameter1 == groub[1].Parameter1) && equals1)
            //                {
            //                    if (uniquePerPrefix)
            //                    {
            //                        counterDefinition.StartNumber = 1000;
            //                    }
            //                    else
            //                    {
            //                        counterDefinition.StartNumber = counterDefinitions.FirstOrDefault().StartNumber;
            //                    }

            //                    counterDefinition.Prefix = "DOM";

            //                    //chkSameAllTrans.IsChecked = true;
            //                    //chkDiffImpExp.IsChecked = true;

            //                }

            //                bool equals2 = true;
            //                if (groub.Count == 3)
            //                {
            //                    equals2 = (groub[1].Parameter2 == groub[2].Parameter2) ? true : false;
            //                }

            //                if ((groub[0].Parameter2 == groub[1].Parameter2) && equals2)
            //                {
            //                    foreach (IGrouping<string, CounterDefinitionPM> el in groupingList)
            //                    {
            //                        List<CounterDefinitionPM> internalList = el.ToList();
            //                        if (counterDefinition.Parameter2 == internalList.FirstOrDefault().Parameter2)
            //                        {
            //                            counterDefinition.Prefix = internalList.FirstOrDefault().Prefix;
            //                            counterDefinition.StartNumber = internalList.FirstOrDefault().StartNumber;
            //                            break;
            //                        }

            //                    }

            //                    //chkSameImpExp.IsChecked = true;
            //                    //chDiffAllTrans.IsChecked = true;
            //                }

            //                break;
            //            }

            //        case 6:
            //            {
            //                counterDefinition.Prefix = counterDefinition.Parameter2 + counterDefinition.Parameter1;
            //                if (uniquePerPrefix)
            //                {
            //                    counterDefinition.StartNumber = 1000;
            //                }
            //                else
            //                {
            //                    counterDefinition.StartNumber = counterDefinitions.FirstOrDefault().StartNumber;
            //                }
            //                //DiffAllTrans.IsChecked = true;
            //                //DiffImpExp.IsChecked = true;
            //                break;
            //            }

            //        case 9:
            //            {
            //                counterDefinition.Prefix = counterDefinition.Parameter2 + counterDefinition.Parameter1;
            //                if (uniquePerPrefix)
            //                {
            //                    counterDefinition.StartNumber = 1000;
            //                }
            //                else
            //                {
            //                    counterDefinition.StartNumber = counterDefinitions.FirstOrDefault().StartNumber;
            //                }
            //                //DiffAllTrans.IsChecked = true;
            //                //DiffImpExp.IsChecked = true;
            //                break;
            //            }

            //    }

            //}
            //else
            //{
            //    counterDefinition.Prefix = counterDefinitions.FirstOrDefault().Prefix;
            //    counterDefinition.StartNumber = counterDefinitions.FirstOrDefault().StartNumber;
            //}
        }

        public int CreateMasterCounter(int tenant)
        {
            ObjectContext = WebFreightContext.GetContext(tenant);
            CounterRepository = new CounterRepository(ObjectContext);
            CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);

            #region Master Counters

            if (CounterRepository.GetCounterByCode("MAST", tenant) == null)
            {
                ObjectTable theMasterObject = ObjectContext.ObjectTables.Where(na => na.Name == "Master" && na.Tenant == 0).FirstOrDefault();


                Counter MasterCounter = new Counter()
                {
                    Id = IdCounter.GetNumber("Counter", 0).ToString(),
                    ObjectTableId = theMasterObject.Id,
                    Code = "MAST",
                    Tenant = tenant,
                    Name = "Master",

                };

                CounterRepository.Add(MasterCounter);

                CounterDefinition Master_Export_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = tenant,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "A",
                };
                CounterDefinition Master_Export_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = tenant,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "O",
                };

                CounterDefinition Master_Export_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = tenant,
                    StartNumber = 1000,
                    Parameter1 = "E",
                    Parameter2 = "I",
                };

                CounterDefinition Master_Import_Air_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = tenant,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "A",
                };

                CounterDefinition Master_Import_Ocean_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = tenant,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "O",
                };

                CounterDefinition Master_Import_Inland_Counter = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", 0).ToString(),
                    CounterId = MasterCounter.Id,
                    Tenant = tenant,
                    StartNumber = 1000,
                    Parameter1 = "I",
                    Parameter2 = "I",
                };

                CounterDefinitionRepository.Add(Master_Export_Air_Counter);
                CounterDefinitionRepository.Add(Master_Export_Ocean_Counter);
                CounterDefinitionRepository.Add(Master_Export_Inland_Counter);
                CounterDefinitionRepository.Add(Master_Import_Air_Counter);
                CounterDefinitionRepository.Add(Master_Import_Ocean_Counter);
                CounterDefinitionRepository.Add(Master_Import_Inland_Counter);

                this.ObjectContext.SaveChanges();

            }


            return 0;
            #endregion
        }

        public void CreateShipmentDeliveryCounters()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            CounterRepository = new CounterRepository(ObjectContext);
            CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);
            List<Counter> counters = CounterRepository.All().ToList();

            List<ObjectTable> objectTables = ObjectTableRepository.GetObjectsByTenant(0).ToList();
            ObjectTable shipmentPickUpDeliveryObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ShipmentPickUpDelivery" && d.Tenant == 0).FirstOrDefault();

            List<GlobalTenant> globalTenants = GlobalTenantRepository.GetGlobalTenants();

            foreach (GlobalTenant tenant in globalTenants)
            {
                if (!counters.Where(c => c.Code == "SHDV" && c.Tenant == tenant.Id).Any())
                {
                    Counter shipmentDeliveryCounter = new Counter()
                    {
                        Id = IdCounter.GetNumber("Counter", tenant.Id).ToString(),
                        ObjectTableId = shipmentPickUpDeliveryObjectTable.Id,
                        Code = "SHDV",
                        Tenant = tenant.Id,
                        Name = "Shipment Delivery Number",
                    };

                    CounterDefinition shipmentDelivery_CounterDef = new CounterDefinition()
                    {
                        Id = IdCounter.GetNumber("CounterDefinition", tenant.Id).ToString(),
                        CounterId = shipmentDeliveryCounter.Id,
                        Tenant = tenant.Id,
                        StartNumber = 1000,
                        Parameter1 = "DLV",
                    };

                    CounterRepository.Add(shipmentDeliveryCounter);
                    CounterDefinitionRepository.Add(shipmentDelivery_CounterDef);
                }
            }
            this.ObjectContext.SaveChanges();
        }
    }
}