using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System.Text.RegularExpressions;
using Simplog.Data.CommonDataModel.Repositories;
using System.ComponentModel;
using WebFreight.Web.WebServices;
using Logitude.BL.Helpers;
using Logitude.BL.DataContracts;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools.Helpers;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;
using System.Web;
using Logitude.Server.Tools;
using System.Drawing;

namespace WebFreight.Web.ReportsWebServices
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class InvoiceWebService : WebService
    {
        [WebMethod]
        public byte[] GetInvoiceData(string invoiceId, string documentTypeCopyId, int tenant)
        {
            InvoiceDataProvider invoicedataprovider = GetInvoiceDataProvider(invoiceId, documentTypeCopyId, tenant);

            XmlSerializer serializer = new XmlSerializer(typeof(InvoiceDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, invoicedataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public InvoiceDataProvider GetInvoiceDataProvider(string invoiceId, string documentTypeCopyId, int tenant)
        {
            InvoiceDataProvider dataProvider = new InvoiceDataProvider();

            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(tenant);
            ARInvoiceRepository invoiceRepository = new ARInvoiceRepository(invoiceCotnext);
            ARInvoice myInvoice = invoiceRepository.GetSingleInvoice(invoiceId);

            if (myInvoice != null)
            {
                if (myInvoice.IsConsolidationInvoice)
                {
                    dataProvider = GetConsolidationInvoiceDataProvider(myInvoice, invoiceRepository, invoiceCotnext, documentTypeCopyId, tenant);
                }
                else if (myInvoice.IsGeneralInvoice)
                {
                    dataProvider = GetConsolidationInvoiceDataProvider(myInvoice, invoiceRepository, invoiceCotnext, documentTypeCopyId, tenant);
                }
                else
                {
                    dataProvider = GetARInvoiceDataProvider(myInvoice, invoiceRepository, invoiceCotnext, documentTypeCopyId, tenant);
                }

                this.FillDocumentCustomFields(myInvoice, dataProvider, documentTypeCopyId, tenant);
            }

            return dataProvider;
        }

        private InvoiceDataProvider GetARInvoiceDataProvider(ARInvoice currentInvoice, ARInvoiceRepository invoiceRepository, IInvoiceContext invoiceCotnext, string documentTypeCopyId, int tenant)
        {
            InvoiceDataProvider invoicedataprovider = new InvoiceDataProvider();

            if (currentInvoice != null)
            {
                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                IWebFreightContext webFreighContext = WebFreightContext.GetContext(tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
                ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
                ShipmentPM shipment = null;

                AddressRepository addressRepository = new AddressRepository(commonContext);
                ContactRepository contactRepository = new ContactRepository(commonContext);
                UserRepository userRepository = new UserRepository(commonContext);
                AddressQuery addressQuery = new AddressQuery(addressRepository);

                Tenant tenantSettings = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();

                WebServiceHelper myServicHelper = new WebServiceHelper(tenant);
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();

                ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(invoiceRepository);
                SATInterfaceSettingRepository satInterfaceSettingRepository = new SATInterfaceSettingRepository(invoiceCotnext);
                SATInterfaceSetting satSetting = satInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);

                if (tenantSettings != null)
                {
                    invoicedataprovider.TenantName = tenantSettings.Company;
                    invoicedataprovider.FMCNumber = tenantSettings.FMCNumber;

                    AddressPM tenantAddress = addressQuery.GetSingleAddressPM(tenantSettings.AddressId, tenantSettings.Id, false);
                    if (tenantAddress != null)
                    {
                        invoicedataprovider.Address1 = tenantAddress.Address1;
                        invoicedataprovider.Address2 = tenantAddress.Address2;
                        invoicedataprovider.City = tenantAddress.City;
                        invoicedataprovider.Country = tenantAddress.CountryName;
                        invoicedataprovider.TenantFax = tenantAddress.FaxNumber;
                        invoicedataprovider.TenantPhone = tenantAddress.PhoneNumber;
                        invoicedataprovider.State = tenantAddress.StateEnglishName;
                        invoicedataprovider.ZipCode = tenantAddress.ZipCode;
                        invoicedataprovider.State = tenantAddress.StateEnglishName;
                        invoicedataprovider.TenantStateCode = tenantAddress.StateCode;
                    }
                }

                List<VatType> allVATTypes = (from d in commonContext.VatTypes where d.Tenant == tenant select d).ToList();
                List<VATTypesGroup> allVATTypesGroups = (from d in commonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();
                List<VatTypePercentage> allVatTypesPercentages = (from d in commonContext.VatTypePercentages where d.Tenant == tenant select d).ToList();
                List<Currency> allCurrencies = (from d in commonContext.Currencies where d.Tenant == tenant select d).ToList();
                List<Measurement> allMeasurements = (from d in commonContext.Measurements where d.Tenant == tenant select d).ToList();
                List<ChargesType> allChargesTypes = (from d in commonContext.ChargesTypes where d.Tenant == tenant select d).ToList();

                #region Start

                string invoiceTypeCode = "";

                #region ObjectTable | DocumentTypeCopy
                ObjectTable currentObjectTable = null;

                if (currentInvoice.ARInvoiceTypeCode == "MN")
                {
                    currentObjectTable = (from obj in webFreighContext.ObjectTables where obj.Name == "Master" && (obj.Tenant == 0) select obj).FirstOrDefault();
                }

                else
                {
                    currentObjectTable = (from obj in webFreighContext.ObjectTables where obj.Name == "Shipment" && (obj.Tenant == 0) select obj).FirstOrDefault();
                }

                if (currentObjectTable != null)
                {
                    invoicedataprovider.Signature = tenantSettings.Signature != null ? tenantSettings.Signature : "";
                    invoicedataprovider.VatNumber = tenantSettings.VatNumber != null ? tenantSettings.VatNumber : "";

                    DocumentTypeCopy documenttypecopy = (from copy in commonContext.DocumentTypeCopies where copy.Id == documentTypeCopyId select copy).FirstOrDefault();

                    switch (currentObjectTable.Name)
                    {
                        case "Shipment":
                            {
                                shipment = shipmentQuery.GetSinglePM(currentInvoice.MainEntityId, tenantSettings.Id);
                                break;
                            }

                        case "Master":
                            {
                                shipment = shipmentQuery.GetSinglePM(currentInvoice.MainEntityId, tenantSettings.Id);
                                break;
                            }

                            //case "Quote":
                            //    quote = (from a in quoteContext.Quotes
                            //             where a.Id == currentInvoice.MainEntityId && a.Tenant == tenantSettings.Id
                            //             select a).FirstOrDefault();
                            //break;
                    }

                    if (documenttypecopy != null)
                    {
                        invoicedataprovider.CopyName = documenttypecopy.Name != null ? documenttypecopy.Name : "";
                        invoicedataprovider.CopyNameNoDraft = documenttypecopy.Name != null ? documenttypecopy.Name : "";
                        switch (documenttypecopy.Code)
                        {
                            case "999CI":
                            case "999S":
                            case "999M":
                            case "999C":
                            case "999G":
                                invoicedataprovider.CopyName_hebrew = "מקור";
                                break;

                            case "999C1":
                            case "999G1":
                                invoicedataprovider.CopyName_hebrew = "עותק";
                                break;

                            case "999C2":
                                invoicedataprovider.CopyName_hebrew = "עותק 2";
                                break;

                            case "999C3":
                                invoicedataprovider.CopyName_hebrew = "עותק אלקטרוני";
                                break;
                        }
                    }
                }
                #endregion

                #region Invoice Properies
                ARInvoiceType invoicetype = (from inty in invoiceCotnext.ARInvoiceTypes where inty.Code == currentInvoice.ARInvoiceTypeCode select inty).FirstOrDefault();
                invoiceTypeCode = invoicetype.Code;
                invoicedataprovider.Status = currentInvoice.Status != null ? currentInvoice.Status.Name : "";
                invoicedataprovider.HouseNumber = currentInvoice.HouseNumber != null ? currentInvoice.HouseNumber : "";
                invoicedataprovider.MasterNumber = currentInvoice.MasterNumber != null ? currentInvoice.MasterNumber : "";
                invoicedataprovider.InvoiceType_label = invoicetype != null ? invoicetype.Name : "";
                invoicedataprovider.Type = invoiceTypeCode == "CD" ? "Credit" : "Debit";
                invoicedataprovider.MasterInternalNumber = shipment.MasterShipmentNumber != null ? shipment.MasterShipmentNumber : "";
                invoicedataprovider.CustomsDeclarationNumber = shipment.CustomsDeclarationNumber != null ? shipment.CustomsDeclarationNumber : "";
                invoicedataprovider.ProjectNumber = shipment.ProjectNumber != null ? shipment.ProjectNumber : "";

                if (currentInvoice.IsAutoCredit)
                {
                    ARInvoice OriginalInvoice = invoiceCotnext.ARInvoices.Where(i => i.CancelledByARInvoiceId == currentInvoice.Id).FirstOrDefault();
                    if (OriginalInvoice != null)
                    {
                        invoicedataprovider.OriginalInvoiceNumber = OriginalInvoice.InvoiceNumber;
                    }
                }

                if (!string.IsNullOrEmpty(invoicedataprovider.OriginalInvoiceNumber))
                {
                    invoicedataprovider.OriginalInvoiceNumber_label = "Original Invoice Number";
                }

                else
                {
                    invoicedataprovider.OriginalInvoiceNumber_label = "";
                }

                if (currentInvoice.StatusCode == "DR")
                {
                    invoicedataprovider.WaterMark = invoicedataprovider.Status;
                    invoicedataprovider.CopyName = invoicedataprovider.Status;
                    invoicedataprovider.CopyName_hebrew = "פרופורמה";
                    invoicedataprovider.InvoiceNumber = currentInvoice.DraftNumber != null ? currentInvoice.DraftNumber : "";
                }

                else if (currentInvoice.StatusCode == "LL")
                {
                    invoicedataprovider.WaterMark = invoicedataprovider.Status;
                    invoicedataprovider.CopyName = invoicedataprovider.Status;
                    invoicedataprovider.CopyName_hebrew = "מבוטלת";
                    invoicedataprovider.InvoiceNumber = currentInvoice.DraftNumber != null ? currentInvoice.DraftNumber : "";
                }

                else
                {
                    invoicedataprovider.InvoiceNumber = currentInvoice.InvoiceNumber != null ? currentInvoice.InvoiceNumber : "";
                }

                if (invoicedataprovider.Status == "Void")
                {
                    invoicedataprovider.WaterMark = invoicedataprovider.Status;
                }

                if (currentInvoice.IsConstituentInvoice)
                {
                    invoicedataprovider.WaterMark = "Constituent";
                    invoicedataprovider.InvoiceType_label = "Constituent Invoice No: ";
                }


                if (invoicetype != null)
                {
                    if (invoicetype.Code == "CD")
                    {
                        invoicedataprovider.InvoiceType_labelHebrew = "הודעת זיכוי";
                        invoicedataprovider.InvoiceType_label_Spanish = "Nota de Credito";
                    }
                    else if (invoicetype.Code == "IN")
                    {
                        invoicedataprovider.InvoiceType_labelHebrew = "חשבונית";
                        invoicedataprovider.InvoiceType_label_Spanish = "Factura";
                    }
                    else if (invoicetype.Code == "TX")
                    {
                        invoicedataprovider.InvoiceType_labelHebrew = "Tax Invoice";
                        invoicedataprovider.InvoiceType_label_Spanish = "Tax Invoice";
                    }
                }


                invoicedataprovider.AccountingNumber = currentInvoice.DebitAccount != null ? currentInvoice.DebitAccount : !string.IsNullOrEmpty(invoicedataprovider.DebitAccount) ? invoicedataprovider.DebitAccount : "";

                User salesman = userRepository.GetSingleUser(shipment.SalesmanUserId, shipment.Tenant, false);
                if (salesman != null)
                {
                    if (salesman.Contact != null)
                    {
                        invoicedataprovider.SalesMan = salesman.Contact.EnglishName;
                    }
                }

                invoicedataprovider.InvoiceDate = currentInvoice.InvoiceDate != null ? String.Format("{0:dd.MMM.yyyy}", currentInvoice.InvoiceDate) : "";
                invoicedataprovider.InvoiceDateAsDateFormat = currentInvoice.InvoiceDate;
                invoicedataprovider.DueDate = currentInvoice.DueDate != null ? String.Format("{0:dd.MMM.yyyy}", currentInvoice.DueDate) : "";
                invoicedataprovider.DueDateAsDateFormat = currentInvoice.DueDate;
                invoicedataprovider.ApprovedDate = currentInvoice.ApprovedDate != null ? String.Format("{0:dd.MMM.yyyy}", currentInvoice.ApprovedDate) : "";
                invoicedataprovider.ApprovedDateAsDateFormat = currentInvoice.ApprovedDate;

                if (currentInvoice.ApprovedByUser != null)
                {
                    if (currentInvoice.ApprovedByUser.Contact != null)
                    {
                        invoicedataprovider.ApprovedBy = currentInvoice.ApprovedByUser.Contact.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(currentInvoice.BranchId))
                {
                    BranchRepository branchRepository = new BranchRepository(tenant);
                    Branch branch = branchRepository.GetSingleBranch(currentInvoice.BranchId, tenant);

                    if (branch != null)
                    {
                        invoicedataprovider.BranchName = branch.EnglishName;
                        invoicedataprovider.BranchSignature = branch.Signature;

                        if (!string.IsNullOrEmpty(branch.AddressId))
                        {
                            Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, tenant);
                            invoicedataprovider.BranchAddress = DataProviders.General.GetAddress(branchAddress);
                        }
                    }
                }

                #endregion

                #region Shipment
                if (shipment != null)
                {
                    invoicedataprovider.ShipmentLevel = shipment.ShipmentLevelName;
                    invoicedataprovider.ShipmentCreateDate = shipment.CreateDateTime == null ? "" : String.Format("{0:dd.MMM.yyyy}", shipment.CreateDateTime);
                    invoicedataprovider.ShipmentCreateDateAsDateFormat = shipment.CreateDateTime;
                    invoicedataprovider.InsidePackagesDetails = shipment.NumberOfInsidePackagesDetails;

                    invoicedataprovider.IsAir = shipment.TransportModeId == "A" ? true : false;
                    invoicedataprovider.IsOcean = shipment.TransportModeId == "O" ? true : false;
                    invoicedataprovider.IsInland = shipment.TransportModeId == "I" ? true : false;
                    invoicedataprovider.VoyageNo = shipment.MainCarriageCarrierNumber;
                    invoicedataprovider.BookingConfirmationNumber = shipment.BookingConfirmationNumber;
                    invoicedataprovider.DeclarationNumber = shipment.DeclarationNumber;
                    invoicedataprovider.DeclarationDate = shipment.DeclarationDate;
                    invoicedataprovider.CustomsClearanceDate = shipment.CustomsClearanceDate;
                    invoicedataprovider.ENSNumber = shipment.ENSNumber;
                    invoicedataprovider.ENSDate = shipment.ENSDate;
                    invoicedataprovider.FreightRelease = shipment.FreightRelease;
                    invoicedataprovider.TerminalAvailable = shipment.TerminalAvailable;
                    invoicedataprovider.ISFNumber = shipment.ISFNumber;
                    invoicedataprovider.ISFDate = shipment.ISFDate;
                    invoicedataprovider.ITNumber = shipment.ITNumber;
                    invoicedataprovider.ITDate = shipment.ITDate;
                    invoicedataprovider.DocumentsClosingDate = shipment.DocumentsClosingDate;
                    invoicedataprovider.CompleteShipmentType = shipment.TransportModeName + " " + shipment.DirectionName;
                    invoicedataprovider.AMSBL = shipment.AMSBL;
                    invoicedataprovider.MainHarmonize = shipment.MainHarmonize;
                    invoicedataprovider.AgentReference1 = shipment.AgentReference1;
                    invoicedataprovider.AgentReference2 = shipment.AgentReference2;

                    if (!string.IsNullOrEmpty(shipment.OBLTypeCode))
                    {
                        OBLType type = shipmentsContext.OBLTypes.Where(d => d.Code == shipment.OBLTypeCode).FirstOrDefault();

                        if (type != null)
                        {
                            invoicedataprovider.OBLType = type.Name;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.FreightPrepaidCollectId))
                    {
                        PrepaidCollect entity = webFreighContext.PrepaidCollects.Where(d => d.Id == shipment.FreightPrepaidCollectId).FirstOrDefault();
                        if (entity != null)
                        {
                            invoicedataprovider.FreightPC = entity.Name;
                        }
                    }

                    #region Via

                    int viaCount = 0;
                    string viaCodes = "";
                    string viaListText = "";

                    if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId) && !string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
                    {
                        PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.Transshipment1FromPortId, true);
                        if (myPort != null)
                        {
                            viaCount += 1;
                            invoicedataprovider.Via1 = myPort.Code + " " + myPort.EnglishName;
                            viaListText = invoicedataprovider.Via1;
                            viaCodes = string.IsNullOrEmpty(viaCodes) ? myPort.Code : viaCodes + ", " + myPort.Code;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId) && !string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
                    {
                        PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.Transshipment2FromPortId, true);
                        if (myPort != null)
                        {
                            viaCount += 1;
                            invoicedataprovider.Via2 = myPort.Code + " " + myPort.EnglishName;
                            viaListText = invoicedataprovider.Via2;
                            viaCodes = string.IsNullOrEmpty(viaCodes) ? myPort.Code : viaCodes + ", " + myPort.Code;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.Transshipment3FromPortId) && !string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
                    {
                        PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.Transshipment3FromPortId, true);
                        if (myPort != null)
                        {
                            viaCount += 1;
                            invoicedataprovider.Via3 = myPort.Code + " " + myPort.EnglishName;
                            viaListText = invoicedataprovider.Via3;
                            viaCodes = string.IsNullOrEmpty(viaCodes) ? myPort.Code : viaCodes + ", " + myPort.Code;
                        }
                    }

                    if (viaCount == 0)
                    {
                        invoicedataprovider.ViaList = "";
                    }

                    else if (viaCount == 1)
                    {
                        invoicedataprovider.ViaList = viaListText;
                    }

                    else if (viaCount > 1)
                    {
                        invoicedataprovider.ViaList = viaCodes;
                    }
                    #endregion

                    if (shipment.TransportModeId == "A")
                    {
                        invoicedataprovider.ShipmentType = "Air";
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(shipment.ShipmentTypeId))
                        {
                            invoicedataprovider.ShipmentType = "Air";
                        }

                        else
                        {
                            string type = "";

                            switch (shipment.ShipmentTypeId.ToUpper())
                            {
                                case "LCLD": { type = "LCL"; break; }
                                case "FCLD": { type = "FCL"; break; }
                                case "MyGI":
                                case "MyGO":
                                    {
                                        type = "Groupage"; break;
                                    }

                                case "LTL": { type = "LTL"; break; }
                                case "FTL": { type = "FTL"; break; }
                                default: { type = "Air"; break; }
                            }

                            invoicedataprovider.ShipmentType = type;
                        }
                    }

                    invoicedataprovider.ShipmentNumber = shipment.ShipmentNumber != null ? shipment.ShipmentNumber : "";

                    string grossWeightUnitCode = shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "";
                    string chargeableWeightUnitCode = shipment.ChargeableWeightUnitCode != null ? shipment.ChargeableWeightUnitCode : "";
                    string volumeUnitCode = shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "";

                    invoicedataprovider.GrossWeight = shipment.GrossWeight != null ? String.Format("{0:#,0.00}", shipment.GrossWeight.Value) + " " + grossWeightUnitCode : "";
                    invoicedataprovider.ChargeableWeight = shipment.ChargeableWeight != null ? String.Format("{0:#,0.00}", shipment.ChargeableWeight.Value) + " " + chargeableWeightUnitCode : "";
                    invoicedataprovider.BillToVatNumber = currentInvoice.VatNumber != null ? currentInvoice.VatNumber : "";
                    invoicedataprovider.ClientRef1 = shipment.CustomerReference1 != null ? shipment.CustomerReference1 : "";
                    invoicedataprovider.ClientRef2 = shipment.CustomerReference2 != null ? shipment.CustomerReference2 : "";
                    invoicedataprovider.ConsigneeRef1 = shipment.ConsigneeReference1 != null ? shipment.ConsigneeReference1 : "";
                    invoicedataprovider.ConsigneeRef2 = shipment.ConsigneeReference2 != null ? shipment.ConsigneeReference2 : "";
                    invoicedataprovider.DescriptionOfGoods = shipment.DescriptionOfGoods != null ? shipment.DescriptionOfGoods : "";

                    if (!string.IsNullOrEmpty(shipment.ShipperReference1))
                    {
                        invoicedataprovider.ShipperRefNo = shipment.ShipperReference1;
                    }
                    if (!string.IsNullOrEmpty(shipment.ShipperReference2))
                    {
                        if (!string.IsNullOrEmpty(shipment.ShipperReference1))
                        {
                            invoicedataprovider.ShipperRefNo = shipment.ShipperReference1 + " , " + shipment.ShipperReference2;
                        }
                        else
                        {
                            invoicedataprovider.ShipperRefNo = shipment.ShipperReference2;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.ConsigneeReference1))
                    {
                        invoicedataprovider.PONumber = shipment.ConsigneeReference1;
                    }
                    if (!string.IsNullOrEmpty(shipment.ConsigneeReference2))
                    {
                        if (!string.IsNullOrEmpty(shipment.ConsigneeReference1))
                        {
                            invoicedataprovider.PONumber = shipment.ConsigneeReference1 + " , " + shipment.ConsigneeReference2;
                        }
                        else
                        {
                            invoicedataprovider.PONumber = shipment.ConsigneeReference2;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.CustomerReference1))
                    {
                        invoicedataprovider.CustomerReferences = shipment.CustomerReference1;
                    }
                    if (!string.IsNullOrEmpty(shipment.CustomerReference2))
                    {
                        if (!string.IsNullOrEmpty(shipment.CustomerReference1))
                        {
                            invoicedataprovider.CustomerReferences = shipment.CustomerReference1 + " , " + shipment.CustomerReference2;
                        }
                        else
                        {
                            invoicedataprovider.CustomerReferences = shipment.CustomerReference2;
                        }
                    }

                    if (shipment.TransportModeId == "A")
                    {
                        invoicedataprovider.MainCarriageCarrierNumber = shipment.MainCarriageCarrierCode + shipment.MainCarriageCarrierNumber;
                    }
                    else
                    {
                        invoicedataprovider.MainCarriageCarrierNumber = shipment.MainCarriageCarrierNumber;
                    }

                    if (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
                    {
                        invoicedataprovider.MainCarriageCarrierNumber = shipment.TruckNumber;
                    }

                    invoicedataprovider.MainCarriageCarrierDate = shipment.MainCarriageATA != null ? String.Format("{0:dd.MMM.yy}", shipment.MainCarriageATA) : shipment.MainCarriageETA != null ? String.Format("{0:dd.MMM.yy}", shipment.MainCarriageETA) : "";

                    invoicedataprovider.Volume = shipment.Volume != null ? String.Format("{0:#,0.00}", shipment.Volume) + " " + volumeUnitCode : "";
                    invoicedataprovider.VolumetricWeight = shipment.VolumetricWeight != null ? String.Format("{0:#,0.00}", shipment.VolumetricWeight) + " " + chargeableWeightUnitCode : "";
                    invoicedataprovider.Notes = currentInvoice.PrintNotes != null ? currentInvoice.PrintNotes : "";

                    //invoice payment term
                    PaymentTerm paymentterm = (from pa in commonContext.PaymentTerms
                                               where pa.Id == currentInvoice.PaymentTermId
                                               select pa).FirstOrDefault();
                    if (paymentterm != null)
                    {
                        invoicedataprovider.PaymentTerm = paymentterm.EnglishName != null ? paymentterm.EnglishName : "";
                        invoicedataprovider.PaymentTerm_Local = paymentterm.LocalName != null ? paymentterm.LocalName : "";
                        invoicedataprovider.PaymentTermDescription = paymentterm.Description != null ? paymentterm.Description : "";
                        invoicedataprovider.PaymentTermLocalDescription = paymentterm.LocalDescription != null ? paymentterm.LocalDescription : "";
                    }

                    #region Shipper
                    if (!string.IsNullOrEmpty(shipment.ShipperId))
                    {
                        Card myCard = CardRepository.GetSingleCard(shipment.ShipperId, tenant, true);
                        if (myCard != null)
                        {
                            invoicedataprovider.ClientNumber = myCard.Code;
                            invoicedataprovider.DebitAccount = myCard.ReceivablesAccountingCard != null ? myCard.ReceivablesAccountingCard : "";
                            invoicedataprovider.Shipper = myCard.EnglishName;
                            invoicedataprovider.Shipper_LocalName = myCard.LocalName != null ? myCard.LocalName : "";

                            if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                            {
                                Address myAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                                if (myAddress != null)
                                {
                                    invoicedataprovider.ShipperAddress = DataProviders.General.GetAddress(myAddress);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Consignee
                    if (!string.IsNullOrEmpty(shipment.ConsigneeId))
                    {
                        Card myCard = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
                        if (myCard != null)
                        {
                            invoicedataprovider.Consignee = myCard.EnglishName;
                            invoicedataprovider.Consignee_LocalName = myCard.LocalName != null ? myCard.LocalName : "";

                            if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                            {
                                Address myAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                                if (myAddress != null)
                                {
                                    invoicedataprovider.ConsigneeAddress = DataProviders.General.GetAddress(myAddress);
                                }
                            }
                        }
                    }
                    #endregion

                    #region Notify1
                    if (!string.IsNullOrEmpty(shipment.Notify1Id))
                    {
                        Card myCard = CardRepository.GetSingleCard(shipment.Notify1Id, tenant, true);
                        if (myCard != null)
                        {
                            invoicedataprovider.Notify1VATNumber = myCard.VatNumber;
                            
                            if (!string.IsNullOrEmpty(shipment.Notify1AddressId))
                            {
                                Address myAddress = addressRepository.GetSingleAddress(shipment.Notify1AddressId, tenant);
                                if (myAddress != null)
                                {
                                    invoicedataprovider.Notify1Address = DataProviders.General.GetAddress(myAddress);
                                }
                            }
                        }
                    }
                    #endregion

                    if (shipment.DirectionId == "E")
                    {
                        invoicedataprovider.MainCarriageExpectedDate = shipment.MainCarriageETD != null ? String.Format("{0:dd.MMM.yy}", shipment.MainCarriageETD) : "";
                    }
                    else if (shipment.DirectionId == "I")
                    {
                        invoicedataprovider.MainCarriageExpectedDate = shipment.MainCarriageETA != null ? String.Format("{0:dd.MMM.yy}", shipment.MainCarriageETA) : "";
                    }

                    ShipmentPickUpDelivery myLastDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                             where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                             select d).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

                    if (myLastDelivery != null)
                    {
                        invoicedataprovider.DeliveryETD = myLastDelivery.ETD;
                        invoicedataprovider.DeliveryAddress = myServicHelper.GetPickUpAddress(myLastDelivery);
                    }

                    #region LoadingPlace

                    ShipmentPickUpDelivery myFirstPickup = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                            where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "PICK"
                                                            select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                    invoicedataprovider.LoadingPlace = myServicHelper.GetPlaceOfLoading(shipment, myFirstPickup);
                    invoicedataprovider.LoadingPlaceShipper = myServicHelper.GetPlaceOfLoading(shipment, myFirstPickup, "Shipper");

                    if (myFirstPickup != null)
                    {
                        invoicedataprovider.PickupETD = myFirstPickup.ETD;
                        invoicedataprovider.PickupAddress = myServicHelper.GetPickUpAddress(myFirstPickup);
                    }

                    #endregion

                    #region PlaceOfDelivery

                    ShipmentPickUpDelivery myDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                         where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                         select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();


                    invoicedataprovider.PlaceOfDelivery = myServicHelper.GetPlaceOfDelivery(shipment, myDelivery);
                    invoicedataprovider.DeliveryFrom = myServicHelper.GetFromDeliveryName(shipment, myDelivery);
                    invoicedataprovider.DeliveryTo = myServicHelper.GetToDeliveryName(shipment, myDelivery);
                    invoicedataprovider.Incoterm = shipment.IncotermCode;

                    if (myDelivery != null)
                    {
                        ShipmentPickUpDeliveryPackageRepository rep = new ShipmentPickUpDeliveryPackageRepository(tenant);
                        List<ShipmentPickUpDeliveryPackage> deliveryPackages = rep.GetPackagesByDeliveryId(myDelivery.Id, tenant).ToList();

                        if (deliveryPackages != null)
                        {
                            invoicedataprovider.DeliveryPackagesQuantity = deliveryPackages.Sum(d => d.Quantity);
                            invoicedataprovider.DeliveryPackagesWeight = deliveryPackages.Sum(d => d.Weight);
                        }

                        invoicedataprovider.DeliveryTrailerNo = myDelivery.TrailerNumber;
                        invoicedataprovider.DeliveryDriverName = myDelivery.Driver;
                    }

                    #endregion

                    #region Pickup and delivery
                    ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(shipmentsContext);
                    ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(shipmentPickUpDeliveryRepository);
                    ShipmentPickUpQuery shipmentPickUpQuery = new ShipmentPickUpQuery(shipmentPickUpDeliveryRepository);

                    List<ShipmentPickUpPM> allPickUps = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipment.Id, shipment.Tenant);
                    List<ShipmentDeliveryPM> allDeliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipment.Id, shipment.Tenant);

                    ShipmentPickUpPM pickup = allPickUps.Where(a => a.PickUpDeliveryTypeCode == "PICK" && a.PickUpDeliveryNumber == shipment.ShipmentNumber + "/" + shipment.ShipmentPickUpIndex).FirstOrDefault();
                    ShipmentDeliveryPM delivery = allDeliveries.Where(a => a.PickUpDeliveryTypeCode == "DELV" && a.PickUpDeliveryNumber == shipment.ShipmentNumber + "/" + shipment.ShipmentDeliveryIndex).FirstOrDefault();

                    ShipmentPickUpPM firstPickup = allPickUps.Where(a => a.PickUpDeliveryTypeCode == "PICK").OrderBy(d => d.PickUpDeliveryNumber).FirstOrDefault();
                    ShipmentDeliveryPM lastDelivery = allDeliveries.Where(a => a.PickUpDeliveryTypeCode == "DELV").OrderByDescending(d => d.PickUpDeliveryNumber).FirstOrDefault();

                    if (firstPickup != null)
                    {
                        if (firstPickup.PickUpDeliveryFromTypeCode == "PART")
                        {
                            AddressPM address = addressQuery.GetSingleAddressPM(firstPickup.FromAddressId, tenant, false);
                            if (address != null)
                            {
                                invoicedataprovider.Origin = address.City + " " + address.CountryCode;
                            }
                        }
                        else if (firstPickup.PickUpDeliveryFromTypeCode == "PORT")
                        {
                            invoicedataprovider.Origin = firstPickup.FromPortName;
                        }
                        else
                        {
                            invoicedataprovider.Origin = firstPickup.FromAddressCity + " " + firstPickup.FromAddressCountryCode;
                        }
                    }

                    if (lastDelivery != null)
                    {
                        if (lastDelivery.PickUpDeliveryToTypeCode == "PART")
                        {
                            AddressPM address = addressQuery.GetSingleAddressPM(lastDelivery.ToAddressId, tenant, false);
                            if (address != null)
                            {
                                invoicedataprovider.FinalDestination = address.City + " " + address.CountryCode;
                            }
                        }
                        else if (lastDelivery.PickUpDeliveryToTypeCode == "PORT")
                        {
                            invoicedataprovider.FinalDestination = lastDelivery.ToPortName;
                        }
                        else
                        {
                            invoicedataprovider.FinalDestination = lastDelivery.ToAddressCity + " " + lastDelivery.ToAddressCountryCode;
                        }
                    }

                    if (pickup != null)
                    {
                        invoicedataprovider.PickUpATD = pickup.ATD != null ? String.Format("{0:dd.MMM.yy}", pickup.ATD) : "";
                    }

                    if (delivery != null)
                    {
                        invoicedataprovider.DeliveryATA = delivery.ATA != null ? String.Format("{0:dd.MMM.yy}", delivery.ATA) : "";
                    }
                    #endregion

                    #region Last Distination
                    Port mainCarriageFromPort = (from a in commonContext.Ports.Include("Country")
                                                 where a.Id == shipment.MainCarriageFromPortId

                                                 select a).FirstOrDefault();
                    Port mainCarriageToPort = (from a in commonContext.Ports.Include("Country")
                                               where a.Id == shipment.MainCarriageToPortId
                                               select a).FirstOrDefault();

                    Port finalDistinationPort = (from a in commonContext.Ports.Include("Country")
                                                 where a.Id == shipment.MainCarriageFinalDestinationPortId
                                                 select a).FirstOrDefault();

                    Port onCarriageToPort = (from a in commonContext.Ports.Include("Country")
                                             where a.Id == shipment.OnCarriageToPortId
                                             select a).FirstOrDefault();

                    if (mainCarriageFromPort != null)
                    {
                        invoicedataprovider.MainCarriageFromPortName = mainCarriageFromPort.EnglishName;
                        invoicedataprovider.MainCarriageFromPort_LocalName = mainCarriageFromPort.LocalName != null ? mainCarriageFromPort.LocalName : "";
                        invoicedataprovider.MainCarriageFromPortCode = mainCarriageFromPort.Code + (mainCarriageFromPort.Country != null ? " (" + mainCarriageFromPort.Country.Code + ")" : "");
                    }

                    if (mainCarriageToPort != null)
                    {
                        invoicedataprovider.MainCarriageToPortName = mainCarriageToPort.EnglishName;
                        invoicedataprovider.MainCarriageToPort_LocalName = mainCarriageToPort.LocalName != null ? mainCarriageToPort.LocalName : "";
                        invoicedataprovider.MainCarriageToPortCode = mainCarriageToPort.Code + (mainCarriageToPort.Country != null ? " (" + mainCarriageToPort.Country.Code + ")" : "");
                    }

                    //Last distination
                    if (finalDistinationPort != null)
                    {
                        invoicedataprovider.FinalDestinationPortName = finalDistinationPort.EnglishName;
                        invoicedataprovider.FinalDestinationPort_LocalName = finalDistinationPort.LocalName != null ? finalDistinationPort.LocalName : "";
                        invoicedataprovider.FinalDestinationPortCode = finalDistinationPort.Code + (finalDistinationPort.Country != null ? " (" + finalDistinationPort.Country.Code + ")" : "");
                        invoicedataprovider.FinalDestinationPortCountryLocalName = finalDistinationPort.Country != null ? finalDistinationPort.Country.LocalName : "";
                    }

                    if (onCarriageToPort != null)
                    {
                        invoicedataprovider.FianlDestinationInclOnCarriagePortName = onCarriageToPort.EnglishName;
                        invoicedataprovider.FianlDestinationInclOnCarriagePortCode = onCarriageToPort.Code + (onCarriageToPort.Country != null ? " (" + onCarriageToPort.Country.Code + ")" : "");
                    }
                    else if (finalDistinationPort != null)
                    {
                        invoicedataprovider.FianlDestinationInclOnCarriagePortName = finalDistinationPort.EnglishName;
                        invoicedataprovider.FianlDestinationInclOnCarriagePortCode = finalDistinationPort.Code + (finalDistinationPort.Country != null ? " (" + finalDistinationPort.Country.Code + ")" : "");
                    }

                    #endregion

                    #region ReleasingAgent
                    string myReleasingAgentId = shipment.ReleasingAgentId;
                    string myReleasingAgentAddressId = shipment.ReleasingAgentAddressId;
                    if (!string.IsNullOrEmpty(myReleasingAgentId))
                    {
                        Card myPartnerCard = CardRepository.GetSingleCard(myReleasingAgentId, tenant, true);

                        if (myPartnerCard != null)
                        {
                            invoicedataprovider.ReleasingAgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                            invoicedataprovider.ReleasingAgentName = myPartnerCard.EnglishName;
                            if (!string.IsNullOrEmpty(myReleasingAgentAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(myReleasingAgentAddressId, tenant);

                                if (myPartnerAddress != null)
                                {
                                    if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                    {
                                        invoicedataprovider.ReleasingAgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                    }

                                    invoicedataprovider.ReleasingAgentAddress = invoicedataprovider.ReleasingAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                    if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                    {
                                        invoicedataprovider.ReleasingAgentAddress = invoicedataprovider.ReleasingAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Agent
                    string myAgentId = shipment.AgentId;
                    string myAgentAddressId = shipment.AgentAddressId;
                    if (!string.IsNullOrEmpty(myAgentId))
                    {
                        Card myPartnerCard = CardRepository.GetSingleCard(myAgentId, tenant, true);

                        if (myPartnerCard != null)
                        {
                            invoicedataprovider.AgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                            invoicedataprovider.AgentName = myPartnerCard.EnglishName;
                            if (!string.IsNullOrEmpty(myAgentAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(myAgentAddressId, tenant);

                                if (myPartnerAddress != null)
                                {
                                    if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                    {
                                        invoicedataprovider.AgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                    }

                                    invoicedataprovider.AgentAddress = invoicedataprovider.AgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                    if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                    {
                                        invoicedataprovider.AgentAddress = invoicedataprovider.AgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    invoicedataprovider.ETD = shipment.MainCarriageETD != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETD) : "";
                    invoicedataprovider.ATD = shipment.MainCarriageATD != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageATD) : "";
                    invoicedataprovider.ETA = shipment.MainCarriageFinalDestinationETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageFinalDestinationETA) : "";
                    invoicedataprovider.ATA = shipment.MainCarriageFinalDestinationATA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageFinalDestinationATA) : "";

                    invoicedataprovider.ETDAsDateFormat = shipment.MainCarriageETD;
                    invoicedataprovider.ATDAsDateFormat = shipment.MainCarriageATD;
                    invoicedataprovider.ETAAsDateFormat = shipment.MainCarriageFinalDestinationETA;
                    invoicedataprovider.ATAAsDateFormat = shipment.MainCarriageFinalDestinationATA;

                    // Inland + Domestic
                    if (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
                    {
                        Address fromAddress = addressRepository.GetSingleAddress(shipment.MainCarriageFromAddressId, tenant);
                        Address toAddress = addressRepository.GetSingleAddress(shipment.MainCarriageToAddressId, tenant);

                        if (fromAddress != null)
                        {
                            invoicedataprovider.FromLocation = fromAddress.City + " " + (fromAddress.Country != null ? fromAddress.Country.Code : "");
                        }

                        if (toAddress != null)
                        {
                            invoicedataprovider.ToLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                            invoicedataprovider.FinalLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                        }
                    }
                    else
                    {
                        invoicedataprovider.FromLocation = mainCarriageFromPort.Code + " " + mainCarriageFromPort.EnglishName;
                        invoicedataprovider.FromLocation_PortName = mainCarriageFromPort.EnglishName;
                        invoicedataprovider.FromLocation_CountryName = mainCarriageFromPort.Country == null ? null : mainCarriageFromPort.Country.EnglishName;

                        invoicedataprovider.ToLocation = mainCarriageToPort.Code + " " + mainCarriageToPort.EnglishName;
                        invoicedataprovider.ToLocation_PortName = mainCarriageToPort.EnglishName;
                        invoicedataprovider.ToLocation_CountryName = mainCarriageToPort.Country == null ? null : mainCarriageToPort.Country.EnglishName;

                        invoicedataprovider.FinalLocation = finalDistinationPort != null ? (finalDistinationPort.Code + " " + finalDistinationPort.EnglishName) : "";
                        invoicedataprovider.FinalLocation_PortName = finalDistinationPort != null ? finalDistinationPort.EnglishName : "";
                        invoicedataprovider.FinalLocation_CountryName = finalDistinationPort != null ? (finalDistinationPort.Country == null ? null : finalDistinationPort.Country.EnglishName) : "";
                    }

                    if (shipment.Transshipment1FromPortId == null && shipment.Transshipment2FromPortId == null && shipment.Transshipment3FromPortId == null && finalDistinationPort == null)
                    {
                        invoicedataprovider.FinalDestinationPortName = ""; ;
                        invoicedataprovider.FinalDestinationPortCode = "";
                        invoicedataprovider.MainCarriageLastDestination_label = "";
                        invoicedataprovider.FinalLocation = "";
                    }

                    if (shipment.TransportModeId == "A")
                    {
                        invoicedataprovider.MainCarriageMAWBOBLBL_label = "M.A.W.B";
                        invoicedataprovider.MainCarriageMAWBOBLBL_label_new = "MAWB";
                        invoicedataprovider.MainCarriageCarrierType_label = "Flight";
                        invoicedataprovider.MainCarriageCarrier_label = "Airline";
                        invoicedataprovider.MainCarriageLastDestination_label = "Final Destination";
                        invoicedataprovider.HouseNumber_Label = "HAWB";
                        invoicedataprovider.HouseNumber_HBL = "HAWB";
                        invoicedataprovider.Containers_Label = "";
                    }
                    else if (shipment.TransportModeId == "O")
                    {
                        invoicedataprovider.MainCarriageMAWBOBLBL_label = "MBL";
                        invoicedataprovider.MainCarriageMAWBOBLBL_label_new = "MBL";
                        invoicedataprovider.MainCarriageCarrierType_label = "Voyage";
                        invoicedataprovider.MainCarriageCarrier_label = "Shipping line";
                        invoicedataprovider.MainCarriageLastDestination_label = "Discharge Port";
                        invoicedataprovider.MainCarriageVessel_Label = "Vessel";
                        invoicedataprovider.HouseNumber_Label = "FBL";
                        invoicedataprovider.HouseNumber_HBL = "HBL";
                        invoicedataprovider.Containers_Label = "Containers";

                        //Vessel name
                        Vessel maincarriagevessel = (from mc in commonContext.Vessels
                                                     where mc.Id == shipment.MainCarriageVesselId
                                                     select mc).FirstOrDefault();

                        if (maincarriagevessel != null)
                        {
                            invoicedataprovider.MainCarriageVesselName = maincarriagevessel.EnglishName;
                            invoicedataprovider.MainCarriageVessel_LocalName = maincarriagevessel.LocalName != null ? maincarriagevessel.LocalName : "";
                        }
                    }
                    else if (shipment.TransportModeId == "I")
                    {
                        invoicedataprovider.MainCarriageMAWBOBLBL_label = "CMR";
                        invoicedataprovider.MainCarriageMAWBOBLBL_label_new = "CMR/RWB";
                        invoicedataprovider.MainCarriageCarrierType_label = "Truck";
                        invoicedataprovider.MainCarriageCarrier_label = "Trucker";
                        invoicedataprovider.MainCarriageLastDestination_label = "Final Destination";
                        invoicedataprovider.MainCarriageVessel_Label = "";
                        invoicedataprovider.HouseNumber_Label = "House";
                        invoicedataprovider.HouseNumber_HBL = "House";
                        invoicedataprovider.Containers_Label = "Trailers/Wagons";
                    }

                    invoicedataprovider.MainCarriageMAWBOBLBL = shipment.Master != null ? shipment.Master : "";

                    Card maincarriagecarrier = (from mc in commonContext.Cards.Include("Airline")
                                                where mc.Id == shipment.MainCarriageCarrierId
                                                select mc).FirstOrDefault();

                    if (maincarriagecarrier != null)
                    {
                        invoicedataprovider.MainCarriageCarrier = maincarriagecarrier.EnglishName != null ? maincarriagecarrier.EnglishName : "";
                        invoicedataprovider.maincarriagecarrierLocalName = maincarriagecarrier.LocalName != null ? maincarriagecarrier.LocalName : "";
                        invoicedataprovider.MainCarriageCarrierPrefix = shipment.AirlinePrefix != null ? shipment.AirlinePrefix : "";
                    }

                    if (!string.IsNullOrEmpty(invoicedataprovider.MainCarriageCarrierPrefix) && !string.IsNullOrEmpty(invoicedataprovider.MainCarriageMAWBOBLBL))
                    {
                        invoicedataprovider.MainCarriageMAWBOBLBL = invoicedataprovider.MainCarriageCarrierPrefix + "-" + invoicedataprovider.MainCarriageMAWBOBLBL;
                    }

                    #region Assemblies
                    if (shipment.ShipmentAssemblies.Count > 0)
                    {
                        invoicedataprovider.Assemblies = new List<ShipmentAssemblyLine>();

                        foreach (ShipmentAssemblyPM assembly in shipment.ShipmentAssemblies)
                        {
                            invoicedataprovider.Assemblies.Add(new ShipmentAssemblyLine()
                            {
                                House = assembly.House,
                                ShipperName = assembly.ShipperName
                            });
                        }
                    }
                    #endregion

                    #region Warehouse Leg
                    invoicedataprovider.WarehouseLegExpectedEntryDate = shipment.WarehouseLegExpectedEntryDate;
                    invoicedataprovider.WarehouseLegActualEntryDate = shipment.WarehouseLegActualEntryDate;
                    invoicedataprovider.WarehouseLegExpectedReleaseDate = shipment.WarehouseLegExpectedReleaseDate;
                    invoicedataprovider.WarehouseLegActualReleaseDate = shipment.WarehouseLegActualReleaseDate;
                    invoicedataprovider.WarehouseLegLastFreeDate = shipment.WarehouseLegLastFreeDate;
                    invoicedataprovider.WarehouseLegRemarks = shipment.WarehouseLegRemarks;
                    invoicedataprovider.WarehouseLegReference = shipment.WarehouseLegReference;
                    invoicedataprovider.WarehouseLegTerminalName = shipment.WarehouseLegTerminalName;
                    if (shipment.WarehouseLegAddressId != null)
                    {
                        Address warehouseAddress = addressRepository.GetSingleAddress(shipment.WarehouseLegAddressId, tenant);
                        invoicedataprovider.WarehouseLegAddress = DataProviders.General.GetAddress(warehouseAddress);
                    }
                    invoicedataprovider.WarehouseLegEntryDate = shipment.WarehouseLegEntryDate;
                    invoicedataprovider.WarehouseLegReleaseDate = shipment.WarehouseLegReleaseDate;
                    invoicedataprovider.WarehouseLegTerminalCode = shipment.WarehouseLegTerminalCode;
                    #endregion

                    #region pickups
                    List<ShipmentPickUpPM> pickUps = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipment.Id, tenant);

                    if (pickUps.Count > 0)
                    {
                        invoicedataprovider.PickUpsLines = new List<PickUpDeliveryLine>();

                        foreach (ShipmentPickUpPM pick in pickUps)
                        {
                            PickUpDeliveryLine newItem = new PickUpDeliveryLine();
                            newItem.ETD = pick.ETD;
                            newItem.ETA = pick.ETA;
                            newItem.ATD = pick.ATD;
                            newItem.ATA = pick.ATA;
                            newItem.Notes = pick.Notes;
                            newItem.TransportMode = pick.TransportModeName;
                            newItem.Weight = pick.ShipmentPickUpDeliveryPackages.Sum(s => s.Weight);
                            myServicHelper.GetPickUpFromAddress(pick, newItem, addressRepository, tenant);

                            foreach (ShipmentPickUpDeliveryPackagePM package in pick.ShipmentPickUpDeliveryPackages)
                            {
                                PackageLine newPackage = new PackageLine();
                                newPackage.PackageDescriptionOfGoods = package.Description;
                                newPackage.ContainerNumber = package.ContainerNumber;
                                newPackage.SealNumber = package.ShipperSeal;
                                newPackage.HSCode = package.Harmonize;
                                newPackage.PackageQuantity = package.Quantity.ToString();
                                newPackage.PackageTypeName = package.PackageTypeName;
                                newPackage.PackageVolume_Double = package.Volume;
                                newPackage.PackageGrossWeight = String.Format("{0:0,0.00}", package.Weight);

                                if (package.Length != null && package.Width != null && package.Height != null)
                                {
                                    newPackage.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                                }

                                newItem.PickUpDeliveryPackages.Add(newPackage);
                            }

                            invoicedataprovider.PickUpsLines.Add(newItem);
                        }
                    }
                    #endregion

                    #region deliveries
                    List<ShipmentDeliveryPM> deliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipment.Id, tenant);

                    if (deliveries.Count > 0)
                    {
                        invoicedataprovider.DeliveriesLines = new List<PickUpDeliveryLine>();

                        foreach (ShipmentDeliveryPM deliv in deliveries)
                        {
                            PickUpDeliveryLine newItem = new PickUpDeliveryLine();
                            newItem.ETD = deliv.ETD;
                            newItem.ETA = deliv.ETA;
                            newItem.ATD = deliv.ATD;
                            newItem.ATA = deliv.ATA;
                            newItem.Notes = deliv.Notes;
                            newItem.TransportMode = deliv.TransportModeName;
                            newItem.Weight = deliv.ShipmentPickUpDeliveryPackages.Sum(s => s.Weight);
                            myServicHelper.GetDeliveryToAddress(deliv, newItem, addressRepository, tenant);

                            foreach (ShipmentPickUpDeliveryPackagePM package in deliv.ShipmentPickUpDeliveryPackages)
                            {
                                PackageLine newPackage = new PackageLine();
                                newPackage.PackageDescriptionOfGoods = package.Description;
                                newPackage.ContainerNumber = package.ContainerNumber;
                                newPackage.SealNumber = package.ShipperSeal;
                                newPackage.HSCode = package.Harmonize;
                                newPackage.PackageQuantity = package.Quantity.ToString();
                                newPackage.PackageTypeName = package.PackageTypeName;
                                newPackage.PackageVolume_Double = package.Volume;
                                newPackage.PackageGrossWeight = String.Format("{0:0,0.00}", package.Weight);

                                if (package.Length != null && package.Width != null && package.Height != null)
                                {
                                    newPackage.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                                }

                                newItem.PickUpDeliveryPackages.Add(newPackage);
                            }

                            invoicedataprovider.DeliveriesLines.Add(newItem);
                        }
                    }
                    #endregion
                }
                #endregion

                #region Bill To
                invoicedataprovider.CustomerRef = currentInvoice.CustomerRef;

                Card billToCard = (from a in commonContext.Cards where a.Id == currentInvoice.BillToId select a).FirstOrDefault();
                if (billToCard != null)
                {
                    invoicedataprovider.BillTo = billToCard.EnglishName != null ? billToCard.EnglishName + Environment.NewLine : "";
                    invoicedataprovider.BillTo_LocalName = billToCard.LocalName != null ? billToCard.LocalName : "";
                    invoicedataprovider.IRSPlace = billToCard.IRSPlace;
                    invoicedataprovider.IRSNumber = billToCard.IRSNumber;
                    invoicedataprovider.BillToCustomerCode = billToCard.Code;

                    Address billToCardAddress = addressRepository.GetSingleAddress(currentInvoice.BillToAddressId, tenant);
                    if (billToCardAddress != null)
                    {
                        if (billToCardAddress != null)
                        {
                            invoicedataprovider.BillToAddress_NoName = DataProviders.General.GetAddress(billToCardAddress);
                            invoicedataprovider.BillToStateCode = billToCardAddress.State == null ? null : billToCardAddress.State.Code;

                            if (billToCardAddress.IsLocalLanguage && !string.IsNullOrEmpty(invoicedataprovider.BillTo_LocalName))
                            {
                                invoicedataprovider.BillToAddress = invoicedataprovider.BillTo_LocalName + Environment.NewLine + DataProviders.General.GetAddress(billToCardAddress);
                                invoicedataprovider.BillToAddress_OneLine = Environment.NewLine + DataProviders.General.GetAddress_OneLine(billToCardAddress);
                                invoicedataprovider.BillToAddressDescription = billToCardAddress.Description;
                            }

                            else
                            {
                                invoicedataprovider.BillToAddress = invoicedataprovider.BillTo + DataProviders.General.GetAddress(billToCardAddress);
                                invoicedataprovider.BillToAddress_OneLine = DataProviders.General.GetAddress_OneLine(billToCardAddress);
                                invoicedataprovider.BillToAddressDescription = billToCardAddress.Description;
                            }

                            invoicedataprovider.SAT.BillToZipCode = billToCardAddress.ZipCode;
                            invoicedataprovider.BillToTelephone = billToCardAddress.PhoneNumber;
                        }
                    }

                    if (billToCard.PartnerTypeId == "CS")
                    {
                        CustomerQuery customerQuery = new CustomerQuery(tenant);
                        CustomerPM customer = customerQuery.GetSinglePM(billToCard.Id, tenant);
                        if (customer != null)
                        {

                            customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, customer, invoicedataprovider);
                        }
                    }

                    Contact billToContact = contactRepository.GetSingleContact(billToCard.PrimaryContactId, tenant);
                    if (billToContact != null)
                    {
                        invoicedataprovider.ContactPersonName = billToContact.EnglishName;
                        invoicedataprovider.ContactPersonEmail = billToContact.Email;
                        invoicedataprovider.BillToPrimaryContactMobile = billToContact.Mobile;
                        invoicedataprovider.BillToPrimaryContactBusinessPhone = billToContact.BusinessPhone;
                    }
                }
                #endregion

                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, invoicedataprovider);
                ARInvoicePM invoicePM = invoiceQuery.GetSinglePM(currentInvoice.Id, currentInvoice.Tenant);

                customFieldResolver.SetDataProviderCustomFieldsValues("ARInvoice", tenant, invoicePM, invoicedataprovider);

                #region PrintByUser
                User issuedByuser = (from user in commonContext.Users.Include("Contact") where user.Id == currentInvoice.IssuedByUserId select user).FirstOrDefault();
                if (issuedByuser != null)
                {
                    Contact contact = issuedByuser.Contact;
                    if (contact != null)
                    {
                        invoicedataprovider.IssuedByUser = contact.EnglishName != null ? contact.EnglishName : "";
                        invoicedataprovider.IssuedByUser_LocalName = contact.LocalName != null ? contact.LocalName : "";
                    }

                    currentInvoice.PrintByUserId = issuedByuser.Id;
                    currentInvoice.PrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                    switch (currentInvoice.StatusCode)
                    {
                        case "AD":
                        case "VD":
                        case "PD":
                        case "PP":
                        case "AR":
                        case "AC":
                            {
                                currentInvoice.IsPrinted = true;
                                break;
                            }
                    }

                    invoiceRepository.Update(currentInvoice);
                    invoiceRepository.SubmitChanges();
                }
                #endregion

                #region No. of Containers

                string alpha = "";
                string num = "";
                string alphaFormat = @"[^A-Za-z]*";
                string numericFormat = @"[^0-9]*";

                List<ShipmentPackage> shipmentPackagesList = shipmentsContext.ShipmentPackages.Where(sh => sh.ShipmentId == shipment.Id).ToList();
                if (shipmentPackagesList.Count > 0)
                {
                    string myTotalContainers = "";
                    int? myNumberofPackages = shipmentPackagesList.Sum(s => s.Quantity);

                    var grouped = (from a in shipmentPackagesList
                                   where a.PackageTypeId != null
                                   group a by a.PackageTypeId into g
                                   select new
                                   {
                                       PackageTypeId = g.Key,
                                       Quantity = g.Sum(s => s.Quantity)
                                   });

                    foreach (var item in grouped)
                    {
                        PackageType myPackageType = (from pa in commonContext.PackageTypes
                                                     where pa.Id == item.PackageTypeId
                                                     select pa).FirstOrDefault();

                        if (myPackageType != null)
                        {
                            if (myPackageType.IsContainer)
                            {
                                alpha = Regex.Replace(myPackageType.Code, alphaFormat, string.Empty, RegexOptions.Compiled);
                                num = Regex.Replace(myPackageType.Code, numericFormat, string.Empty, RegexOptions.Compiled);

                                string itemText = item.Quantity.ToString() + " x " + num + "'" + alpha;
                                myTotalContainers = string.IsNullOrEmpty(myTotalContainers) ? itemText : myTotalContainers + ", " + itemText;
                            }
                        }
                    }

                    string myContainersNumbersText = "";
                    string myContainersNumbersAndTypesText = "";
                    string myPackageDetails = "";
                    string myLCLContainersNumbersText = "";
                    foreach (ShipmentPackage item in shipmentPackagesList)
                    {
                        if (string.IsNullOrEmpty(myPackageDetails))
                        {

                        }
                        else
                        {
                            myPackageDetails = Environment.NewLine + myPackageDetails;
                        }

                        if (shipment.TransportModeId == "A")
                        {
                            myPackageDetails = myPackageDetails + item.Quantity == null ? "0" : item.Quantity.ToString();
                        }

                        else
                        {
                            PackageType myPackageType = (from pa in commonContext.PackageTypes where pa.Id == item.PackageTypeId select pa).FirstOrDefault();

                            if (myPackageType != null)
                            {
                                myPackageDetails = myPackageDetails + item.Quantity == null ? "0" : item.Quantity.ToString() + " " + myPackageType.EnglishName;
                            }
                            else
                            {
                                myPackageDetails = myPackageDetails + item.Quantity == null ? "0" : item.Quantity.ToString();
                            }
                        }

                        if (!string.IsNullOrEmpty(item.ContainerNumber))
                        {
                            myLCLContainersNumbersText = string.IsNullOrEmpty(myLCLContainersNumbersText) ? item.ContainerNumber : myLCLContainersNumbersText + ", " + item.ContainerNumber;

                            PackageType myPackageType = (from pa in commonContext.PackageTypes where pa.Id == item.PackageTypeId select pa).FirstOrDefault();
                            if (myPackageType != null)
                            {
                                if (myPackageType.IsContainer)
                                {
                                    string type = !string.IsNullOrEmpty(myPackageType.PrintAs) ? myPackageType.PrintAs : myPackageType.Code;
                                    string itemText = item.ContainerNumber + " " + type;

                                    myContainersNumbersText = string.IsNullOrEmpty(myContainersNumbersText) ? item.ContainerNumber : myContainersNumbersText + ", " + item.ContainerNumber;
                                    myContainersNumbersAndTypesText = string.IsNullOrEmpty(myContainersNumbersAndTypesText) ? itemText : myContainersNumbersAndTypesText + "," + itemText;
                                }
                            }
                        }
                    }

                    invoicedataprovider.PackageDetails = myPackageDetails;

                    string myPackagesInDetails = null;
                    switch (shipment.TransportModeId)
                    {
                        case "A":
                            {
                                myPackagesInDetails = myNumberofPackages == null ? "0" : myNumberofPackages.ToString();
                                break;
                            }

                        case "I":
                            {
                                myPackagesInDetails = myNumberofPackages == null ? "0" : myNumberofPackages.ToString();
                                myPackagesInDetails += (myNumberofPackages == 1) ? " Trailer" : " Trailers";
                                break;
                            }

                        case "O":
                            {
                                if (shipment.ShipmentTypeId == "FCLD")
                                {
                                    myPackagesInDetails = myNumberofPackages == null ? "0" : myNumberofPackages.ToString();
                                    myPackagesInDetails += (myNumberofPackages == 1) ? " Container" : " Containers";
                                }

                                else
                                {
                                    foreach (var item in grouped)
                                    {
                                        PackageType myPackageType = (from pa in commonContext.PackageTypes
                                                                     where pa.Id == item.PackageTypeId
                                                                     select pa).FirstOrDefault();
                                        if (myPackageType != null)
                                        {
                                            string itemText = item.Quantity + " " + myPackageType.EnglishName;
                                            myPackagesInDetails += string.IsNullOrEmpty(myPackagesInDetails) ? itemText : " " + itemText;
                                        }
                                    }
                                }

                                break;
                            }
                    }

                    invoicedataprovider.PackagesInDetails = myPackagesInDetails;
                    invoicedataprovider.TotalContainers = myTotalContainers;
                    invoicedataprovider.NumberofPackages = myNumberofPackages.ToString();
                    invoicedataprovider.ContainersNumbersArray = myContainersNumbersText;
                    invoicedataprovider.ContainersNumbersAndTypesArray = myContainersNumbersAndTypesText;
                    invoicedataprovider.LCLContainersNumbersArray = myLCLContainersNumbersText;
                }
                #endregion

                #region Others
                List<ARInvoiceLine> invoiceLines = invoiceCotnext.ARInvoiceLines.Where(invline => invline.ARInvoiceId == currentInvoice.Id && invline.Tenant == currentInvoice.Tenant).OrderBy(d => d.ChargesType.ViewOrder).ToList();
                Currency invoicecurrency = (from fc in commonContext.Currencies where fc.Id == currentInvoice.InvoiceCurrencyId select fc).FirstOrDefault();
                Currency localcurrency = (from fc in commonContext.Currencies where fc.Id == currentInvoice.LocalCurrencyId select fc).FirstOrDefault();

                if (localcurrency != null)
                {
                    invoicedataprovider.LocalCurrency = localcurrency.Code;
                    invoicedataprovider.LocalCurrencySign = localcurrency.Sign;
                }

                if (invoicecurrency != null)
                {
                    invoicedataprovider.InvoiceCurrency = invoicecurrency.Code;
                    invoicedataprovider.InvoiceCurrencySign = invoicecurrency.Sign;
                    invoicedataprovider.InvoicecurrencyLocalName = invoicecurrency.LocalName;
                }
                //********************************

                List<string> foriegnCurrencies = invoiceLines.GroupBy(g => new { g.ForiegnCurrencyId }).Select(s => s.Key.ForiegnCurrencyId).ToList();

                StringBuilder localString = new StringBuilder();
                StringBuilder invoiceString = new StringBuilder();

                if (currentInvoice.LocalCurrencyId == currentInvoice.InvoiceCurrencyId && foriegnCurrencies.Count == 0)
                {
                    invoicedataprovider.LocalCurrencyExchangeRates = "";
                    invoicedataprovider.InvoiceCurrencyExchangeRates = "";
                }

                else if (currentInvoice.LocalCurrencyId == currentInvoice.InvoiceCurrencyId && foriegnCurrencies.Count > 0)
                {
                    string id;
                    Currency foriegnCurrency = null;

                    for (int i = 0; i < foriegnCurrencies.Count; i++)
                    {
                        id = foriegnCurrencies[i];
                        double? foriegnExchangeRate = invoiceLines.Where(d => d.ForiegnCurrencyId == id).FirstOrDefault().ForiegnExchangeRate;

                        if (id != currentInvoice.LocalCurrencyId && id != currentInvoice.InvoiceCurrencyId)
                        {
                            foriegnCurrency = commonContext.Currencies.Where(fc => fc.Id == id).FirstOrDefault();

                            localString.AppendLine("1 " + foriegnCurrency.Code + " = " + foriegnExchangeRate + " " + currentInvoice.LocalCurrency.Code);
                            invoiceString.AppendLine("1 " + foriegnCurrency.Code + " = " + (foriegnExchangeRate / currentInvoice.InvoiceCurrencyExchangeRate) + " " + currentInvoice.InvoiceCurrency.Code);
                        }
                    }
                    invoicedataprovider.LocalCurrencyExchangeRates = localString.ToString();
                    invoicedataprovider.InvoiceCurrencyExchangeRates = invoiceString.ToString();
                }
                else if (currentInvoice.LocalCurrencyId != currentInvoice.InvoiceCurrencyId && foriegnCurrencies.Count == 0)
                {
                    localString.AppendLine("1 " + currentInvoice.InvoiceCurrency.Code + " = " + currentInvoice.InvoiceCurrencyExchangeRate + " " + currentInvoice.LocalCurrency.Code);
                    invoiceString.AppendLine("1 " + currentInvoice.LocalCurrency.Code + " = " + (1 / currentInvoice.InvoiceCurrencyExchangeRate) + " " + currentInvoice.InvoiceCurrency.Code);

                    invoicedataprovider.LocalCurrencyExchangeRates = localString.ToString();
                    invoicedataprovider.InvoiceCurrencyExchangeRates = invoiceString.ToString();
                }
                else if (currentInvoice.LocalCurrencyId != currentInvoice.InvoiceCurrencyId && foriegnCurrencies.Count > 0)
                {
                    string id;
                    Currency foriegnCurrency = null;

                    for (int i = 0; i < foriegnCurrencies.Count; i++)
                    {
                        id = foriegnCurrencies[i];
                        foriegnCurrency = commonContext.Currencies.Where(fc => fc.Id == id).FirstOrDefault();
                        double? foriegnExchangeRate = invoiceLines.Where(d => d.ForiegnCurrencyId == id).FirstOrDefault().ForiegnExchangeRate;

                        if (currentInvoice.LocalCurrencyId != id && currentInvoice.InvoiceCurrencyId != id)
                        {
                            localString.AppendLine("1 " + foriegnCurrency.Code + " = " + foriegnExchangeRate + " " + currentInvoice.LocalCurrency.Code);
                            invoiceString.AppendLine("1 " + foriegnCurrency.Code + " = " + (foriegnExchangeRate / currentInvoice.InvoiceCurrencyExchangeRate) + " " + currentInvoice.InvoiceCurrency.Code);
                        }
                    }

                    localString.AppendLine("1 " + currentInvoice.InvoiceCurrency.Code + " = " + currentInvoice.InvoiceCurrencyExchangeRate + " " + currentInvoice.LocalCurrency.Code);
                    invoiceString.AppendLine("1 " + currentInvoice.LocalCurrency.Code + " = " + (1 / currentInvoice.InvoiceCurrencyExchangeRate) + " " + currentInvoice.InvoiceCurrency.Code);
                    invoicedataprovider.LocalCurrencyExchangeRates = localString.ToString();
                    invoicedataprovider.InvoiceCurrencyExchangeRates = invoiceString.ToString();
                }
                else
                {
                    invoicedataprovider.LocalCurrencyExchangeRates = "";
                    invoicedataprovider.InvoiceCurrencyExchangeRates = "";
                }
                //********************************
                double? invoiceSubTotals = currentInvoice.SubTotalInLocalCurrency;
                double? invoiceSubTotals_Local = currentInvoice.SubTotalInInvoiceCurrency;
                double? invoiceAmount = currentInvoice.AmountInInvoiceCurrency;
                double? invoiceAmountLocal = currentInvoice.AmountInLocalCurrency;
                double? profitAmount = currentInvoice.AmountInProfitCurrency;

                if (invoiceTypeCode == "CD")
                {
                    if (invoiceSubTotals < 0)
                    {
                        invoiceSubTotals = invoiceSubTotals * -1;
                    }

                    if (invoiceSubTotals_Local < 0)
                    {
                        invoiceSubTotals_Local = invoiceSubTotals_Local * -1;
                    }

                    if (invoiceAmount < 0)
                    {
                        invoiceAmount = invoiceAmount * -1;
                    }

                    if (invoiceAmountLocal < 0)
                    {
                        invoiceAmountLocal = invoiceAmountLocal * -1;
                    }

                    if (profitAmount < 0)
                    {
                        profitAmount = profitAmount * -1;
                    }
                }

                invoicedataprovider.SubTotalLocalCurr = invoiceSubTotals != null ? String.Format("{0:#,0.00}", invoiceSubTotals.Value) : "";
                invoicedataprovider.SubTotalInvoiceCurr = invoiceSubTotals_Local != null ? String.Format("{0:#,0.00}", invoiceSubTotals_Local.Value) : "";
                invoicedataprovider.TotalInvoiceCurr = invoiceAmount != null ? invoiceAmount.Value : 0;
                invoicedataprovider.TotalLocalCurr = invoiceAmountLocal != null ? String.Format("{0:#,0.00}", invoiceAmountLocal.Value) : "";
                invoicedataprovider.TotalProfitCurr = profitAmount != null ? profitAmount.Value : 0;

                var result = invoicedataprovider.TotalInvoiceCurr - Math.Truncate(invoicedataprovider.TotalInvoiceCurr);
                var Firstdigits = (int)(Math.Round(result, 2) * 100);
                string str = "";
                string strWithZeros = "";
                if (Firstdigits < 10 && Firstdigits > 0)
                {
                    str = 0 + "" + Firstdigits + "/100";
                    strWithZeros = 0 + "" + Firstdigits + "/100";
                }
                else
                {
                    str = Firstdigits + "/100";
                    strWithZeros = Firstdigits + "/100";
                    if (Firstdigits == 0)
                        strWithZeros = 0 + "" + Firstdigits + "/100";

                }

                if ((int)(Math.Round(result, 2) * 100) <= 0)
                {
                    str = "";
                }

                var FrenchFractions = "";
                var FrenchFractionsWords = "";
                if (Firstdigits > 0)
                {
                    FrenchFractions = Firstdigits + " Cts";
                    FrenchFractionsWords = "et " + NumbersToFrench(Firstdigits) + " centimes";
                }

                invoicedataprovider.AmountInWordsSpanish = FirstCharToUpper(NumbersToSpanish((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName + " " + str;
                invoicedataprovider.AmountInWordsEnglish = FirstCharToUpper(NumbersToEnglish((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName + " " + str;
                invoicedataprovider.AmountInWordsEnglishNoFR = FirstCharToUpper(NumbersToEnglish((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName;
                invoicedataprovider.AmountInWordsFrench = FirstCharToUpper(NumbersToFrench((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName + " " + FrenchFractions;
                invoicedataprovider.AmountInWordsFrenchWithFR = FirstCharToUpper(NumbersToFrench((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName + " " + FrenchFractionsWords;
                invoicedataprovider.AmountInWordsFrenchNoFR = FirstCharToUpper(NumbersToFrench((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName;
                invoicedataprovider.AmountInWordsSpanishWithZero = FirstCharToUpper(NumbersToSpanish((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName + " " + strWithZeros;
                invoicedataprovider.AmountsInEnglishWithZero = FirstCharToUpper(NumbersToEnglish((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName + " " + strWithZeros;
                invoicedataprovider.AmountInWordsRussian = FirstCharToUpper(NumbersToRussian((int)invoiceAmount.Value) + " ") + invoicedataprovider.InvoicecurrencyLocalName + " " + strWithZeros;


                invoicedataprovider.AmountDueInInvoiceCurrency = currentInvoice.AmountDue;
                invoicedataprovider.AmountDueInLocalCurrency = currentInvoice.AmountDueInLocalCurrency;
                #endregion

                #region Vatable amounts

                VatType VAT_ZERO = allVATTypes.Where(d => d.Code == "ZERO").FirstOrDefault();
                VatType VAT_REIM = allVATTypes.Where(d => d.Code == "REIM").FirstOrDefault();
                VatType VAT_12 = allVATTypes.Where(d => d.Code == "12").FirstOrDefault();

                if (VAT_ZERO != null)
                {
                    invoicedataprovider.VatableAmount_ZERO = invoiceLines.Where(d => d.VatTypeId == VAT_ZERO.Id).Sum(s => s.InvoiceCurrencyAmount);
                }

                if (VAT_REIM != null)
                {
                    invoicedataprovider.VatableAmount_REIM = invoiceLines.Where(d => d.VatTypeId == VAT_REIM.Id).Sum(s => s.InvoiceCurrencyAmount);
                }

                if (VAT_12 != null)
                {
                    invoicedataprovider.VatableAmount_12 = invoiceLines.Where(d => d.VatTypeId == VAT_12.Id).Sum(s => s.InvoiceCurrencyAmount);
                }

                #endregion

                #region USD-MXN exchange rate

                Currency USDCurrency = commonContext.Currencies.Where(d => d.Code == "USD" && d.Tenant == tenant).FirstOrDefault();
                Currency MXNCurrency = commonContext.Currencies.Where(d => d.Code == "MXN" && d.Tenant == tenant).FirstOrDefault();

                if (USDCurrency != null && MXNCurrency != null)
                {
                    LastRate rate = this.GetCurrencysExchangeRate(tenant, MXNCurrency, USDCurrency, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
                    if (rate != null)
                    {
                        invoicedataprovider.USD_MXN_ExchangeRate = rate.Rate;
                    }
                }

                #endregion

                #region InvoiceToAccountingExchangeRate
                invoicedataprovider.InvoiceToAccountingExchangeRate = currentInvoice.InvoiceCurrencyExchangeRate;
                #endregion

                #region Invoice Lines

                invoicedataprovider.InvoiceLinesList = new List<ReportInvoiceLine>();
                invoicedataprovider.ExpenseInvoiceLinesList = new List<ReportInvoiceLine>();
                invoicedataprovider.NoExpenseInvoiceLinesList = new List<ReportInvoiceLine>();

                if (currentInvoice.ARInvoiceTypeCode == "MN")
                {
                    double totalQuantity = 0;

                    #region LOOP Lines
                    var lines = (from a in invoiceLines
                                 group a by new
                                 {
                                     a.ChargesTypeId,
                                     a.Description,
                                     a.ForiegnCurrencyId,
                                     a.ForiegnExchangeRate,
                                     a.LocalDescription,
                                     a.MeasurementId,
                                     a.VatTypeId,
                                     a.Notes,
                                     a.IsExpense
                                 } into gr
                                 select new
                                 {
                                     ChargesTypeId = gr.Key.ChargesTypeId,
                                     Description = gr.Key.Description,
                                     ForiegnCurrencyId = gr.Key.ForiegnCurrencyId,
                                     ForiegnExchangeRate = gr.Key.ForiegnExchangeRate,
                                     LocalDescription = gr.Key.LocalDescription,
                                     Notes = gr.Key.Notes,
                                     MeasurementId = gr.Key.MeasurementId,
                                     VatTypeId = gr.Key.VatTypeId,
                                     IsExpense = gr.Key.IsExpense,
                                     LocalAmount = gr.Sum(d => (d.LocalCurrencyAmount != null ? d.LocalCurrencyAmount.Value : 0)),
                                     InvoiceAmount = gr.Sum(d => (d.InvoiceCurrencyAmount != null ? d.InvoiceCurrencyAmount.Value : 0)),
                                     ForeignAmount = gr.Sum(d => (d.ForiegnCurrencyAmount != null ? d.ForiegnCurrencyAmount.Value : 0)),
                                     Quantity = gr.Sum(d => (d.Quantity != null ? d.Quantity.Value : 0)),
                                 }).ToList();

                    foreach (var invoiceline in lines)
                    {
                        ReportInvoiceLine reportinvoiceline = new ReportInvoiceLine();

                        Currency foreigncurrency = allCurrencies.Where(d => d.Id == invoiceline.ForiegnCurrencyId).FirstOrDefault();

                        reportinvoiceline.ForeignCurrency = foreigncurrency != null ? foreigncurrency.Code : "";
                        reportinvoiceline.ForeignCurrencySign = foreigncurrency != null ? foreigncurrency.Sign : "";

                        reportinvoiceline.Notes = invoiceline.Notes != null ? invoiceline.Notes : "";
                        reportinvoiceline.Description = invoiceline.Description != null ? invoiceline.Description : "";
                        reportinvoiceline.LocalDescription = invoiceline.LocalDescription != null ? invoiceline.LocalDescription : "";
                        reportinvoiceline.DescriptionAndNotes = reportinvoiceline.Description + Environment.NewLine + reportinvoiceline.Notes;
                        reportinvoiceline.IsExpense = invoiceline.IsExpense;
                        reportinvoiceline.CalculatedUnitPrice = Math.Round(invoiceline.InvoiceAmount / invoiceline.Quantity, 2);

                        double? lineAmount_Foreign = invoiceline.ForeignAmount;
                        double? lineAmount_Invoice = invoiceline.InvoiceAmount;
                        double? lineAmount_Local = invoiceline.LocalAmount;

                        if (lineAmount_Local < 0)
                        {
                            reportinvoiceline.DebitInLocalAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.DebitInInvoiceAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.DebitInForeignAmount = String.Format("{0:#,0.00}", 0);

                            reportinvoiceline.CreditInLocalAmount = String.Format("{0:#,0.00}", lineAmount_Local * -1);
                            reportinvoiceline.CreditInInvoiceAmount = String.Format("{0:#,0.00}", lineAmount_Invoice * -1);
                            reportinvoiceline.CreditInForeignAmount = String.Format("{0:#,0.00}", lineAmount_Foreign * -1);
                        }

                        else
                        {
                            reportinvoiceline.DebitInLocalAmount = String.Format("{0:#,0.00}", lineAmount_Local);
                            reportinvoiceline.DebitInInvoiceAmount = String.Format("{0:#,0.00}", lineAmount_Invoice);
                            reportinvoiceline.DebitInForeignAmount = String.Format("{0:#,0.00}", lineAmount_Foreign);

                            reportinvoiceline.CreditInLocalAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.CreditInInvoiceAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.CreditInForeignAmount = String.Format("{0:#,0.00}", 0);
                        }

                        if (invoiceTypeCode == "CD")
                        {
                            lineAmount_Foreign = lineAmount_Foreign * -1;
                            lineAmount_Invoice = lineAmount_Invoice * -1;
                            lineAmount_Local = lineAmount_Local * -1;
                        }

                        reportinvoiceline.LocalAmount = String.Format("{0:#,0.00}", lineAmount_Local);
                        reportinvoiceline.InvoiceAmount = String.Format("{0:#,0.00}", lineAmount_Invoice);
                        reportinvoiceline.ForeignAmount = String.Format("{0:#,0.00}", lineAmount_Foreign);
                        reportinvoiceline.LocalAmount_Double = lineAmount_Local;
                        reportinvoiceline.InvoiceAmount_Double = lineAmount_Invoice;

                        reportinvoiceline.VatAmountIncludeMultiInInvoiceCurrency = this.GetVatAmountIncludeMultiInInvoiceCurrencyField(invoiceline.VatTypeId, invoiceline.InvoiceAmount, invoiceTypeCode, allVATTypes, allVatTypesPercentages, allVATTypesGroups);

                        if (!string.IsNullOrEmpty(invoiceline.VatTypeId))
                        {
                            #region Vats
                            VatType vattype = allVATTypes.Where(d => d.Id == invoiceline.VatTypeId).FirstOrDefault();
                            if (vattype != null)
                            {
                                reportinvoiceline.VatType = vattype.EnglishName != null ? vattype.EnglishName : "";
                                reportinvoiceline.VatTypeLocalName = vattype.LocalName != null ? vattype.LocalName : "";
                                reportinvoiceline.VATDescription = vattype.Description;
                                reportinvoiceline.VATLocalDescription = vattype.LocalDescription;

                                if (!vattype.IsMultiPercentage)
                                {
                                    List<VatTypePercentage> vattypepercentageList = (from percentage in commonContext.VatTypePercentages where percentage.VatTypeId == vattype.Id orderby percentage.FromDate descending select percentage).ToList();

                                    if (vattypepercentageList.Count > 0)
                                    {
                                        reportinvoiceline.VatTypePercentage = vattypepercentageList[0].Percentage != null ? String.Format("{0:#,0.00}", vattypepercentageList[0].Percentage) : "";

                                        if (vattypepercentageList[0].Percentage != null && vattypepercentageList[0].Percentage > 0)
                                        {
                                            reportinvoiceline.VatIndication = "*";
                                            reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceAmount);
                                            reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceAmount;
                                        }

                                        else
                                        {
                                            reportinvoiceline.VatIndication = "";
                                            reportinvoiceline.NONVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceAmount);
                                            reportinvoiceline.NONVATableAmountInInvoiceCurrency_double = invoiceline.InvoiceAmount;
                                        }

                                        double vatamountinlocalcurrency = (invoiceline.LocalAmount * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;
                                        double vatamountininvoicecurrecy = (invoiceline.InvoiceAmount * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;
                                        double vatAmountInForeignCurrecy = (invoiceline.ForeignAmount * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;

                                        if (invoiceTypeCode == "CD")
                                        {
                                            vatamountinlocalcurrency = vatamountinlocalcurrency * -1;
                                            vatamountininvoicecurrecy = vatamountininvoicecurrecy * -1;
                                            vatAmountInForeignCurrecy = vatAmountInForeignCurrecy * -1;
                                        }

                                        reportinvoiceline.VatAmountInLocalCurrency = String.Format("{0:#,0.00}", vatamountinlocalcurrency);
                                        reportinvoiceline.VatAmountInInvoiceCurrency = String.Format("{0:#,0.00}", vatamountininvoicecurrecy);
                                        reportinvoiceline.VatAmountInForeignCurrency = String.Format("{0:#,0.00}", vatAmountInForeignCurrecy);

                                        reportinvoiceline.LocalAmountWithVAT = lineAmount_Local + vatamountinlocalcurrency;
                                    }
                                }

                                else
                                {
                                    reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceAmount);
                                    reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceAmount;
                                }
                            }
                            #endregion
                        }

                        Measurement myMeasurement = allMeasurements.Where(d => d.Id == invoiceline.MeasurementId).FirstOrDefault();
                        if (myMeasurement != null)
                        {
                            reportinvoiceline.Measurement = myMeasurement.Code;

                            if (myMeasurement.Code == "PRVL" || myMeasurement.Code == "PRFR")
                            {
                                reportinvoiceline.UOMPercentage = "%";
                            }

                            if (satSetting != null && (satSetting.SATInterfaceCode == "PROF" || satSetting.SATInterfaceCode == "PROF33"))
                            {
                                ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(currentInvoice.Tenant);
                                reportinvoiceline.ClaveUnidad = computingPartnerHelper.GetComputingPartnerCodeTranslation(myMeasurement.Code, "G-Profact", "Measurement");
                            }
                        }

                        reportinvoiceline.Quantity = String.Format("{0:#,0.00}", invoiceline.Quantity);
                        totalQuantity += invoiceline.Quantity;

                        ChargesType chargetype = allChargesTypes.Where(d => d.Id == invoiceline.ChargesTypeId).FirstOrDefault();
                        if (chargetype != null)
                        {
                            reportinvoiceline.ChargeType = chargetype.EnglishName != null ? chargetype.EnglishName : "";
                            reportinvoiceline.ChargeTypeLocalName = chargetype.LocalName != null ? chargetype.LocalName : "";
                            reportinvoiceline.ChrageTypeCode = chargetype.Code != null ? chargetype.Code : "";

                            reportinvoiceline.ClaveProdServ = chargetype.SATExternalId;
                        }

                        if (foreigncurrency != null && invoicecurrency != null)
                        {
                            double? foreignExchangeRate = 0;

                            if (foreigncurrency == invoicecurrency)
                            {
                                foreignExchangeRate = 1;
                            }

                            else
                            {
                                foreignExchangeRate = invoiceline.ForiegnExchangeRate / currentInvoice.InvoiceCurrencyExchangeRate;
                            }

                            reportinvoiceline.ForeignToInvoiceExchangeRate = "1 " + foreigncurrency.Code + " = " + Math.Round(foreignExchangeRate.Value, 2) + " " + invoicecurrency.Code;
                        }

                        invoicedataprovider.InvoiceLinesList.Add(reportinvoiceline);
                    }

                    #endregion

                    invoicedataprovider.Quantity = String.Format("{0:#,0.##}", totalQuantity);
                }

                else
                {
                    double totalQuantity = 0;

                    #region LOOP Lines  
                    foreach (ARInvoiceLine invoiceline in invoiceLines)
                    {
                        ReportInvoiceLine reportinvoiceline = new ReportInvoiceLine();

                        Currency foreigncurrency = allCurrencies.Where(d => d.Id == invoiceline.ForiegnCurrencyId).FirstOrDefault();

                        reportinvoiceline.ForeignCurrency = foreigncurrency != null ? foreigncurrency.Code : "";
                        reportinvoiceline.ForeignCurrencySign = foreigncurrency != null ? foreigncurrency.Sign : "";
                        reportinvoiceline.Notes = invoiceline.Description != null ? invoiceline.Notes : "";
                        reportinvoiceline.Description = invoiceline.Description != null ? invoiceline.Description : "";
                        reportinvoiceline.LocalDescription = invoiceline.LocalDescription != null ? invoiceline.LocalDescription : "";
                        reportinvoiceline.DescriptionAndNotes = reportinvoiceline.Description + Environment.NewLine + reportinvoiceline.Notes;
                        reportinvoiceline.IsExpense = invoiceline.IsExpense;
                        reportinvoiceline.CalculatedUnitPrice = Math.Round((invoiceline.InvoiceCurrencyAmount / invoiceline.Quantity).Value, 2);

                        double? line_UnitPrice = invoiceline.UnitPrice;
                        double? lineAmount_Local = invoiceline.LocalCurrencyAmount;
                        double? lineAmount_Invoice = invoiceline.InvoiceCurrencyAmount;
                        double? lineAmount_Foreign = invoiceline.ForiegnCurrencyAmount;

                        if (line_UnitPrice < 0)
                        {
                            reportinvoiceline.DebitInLocalAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.DebitInInvoiceAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.DebitInForeignAmount = String.Format("{0:#,0.00}", 0);

                            reportinvoiceline.CreditInLocalAmount = String.Format("{0:#,0.00}", lineAmount_Local * -1);
                            reportinvoiceline.CreditInInvoiceAmount = String.Format("{0:#,0.00}", lineAmount_Invoice * -1);
                            reportinvoiceline.CreditInForeignAmount = String.Format("{0:#,0.00}", lineAmount_Foreign * -1);
                        }

                        else
                        {
                            reportinvoiceline.DebitInLocalAmount = String.Format("{0:#,0.00}", lineAmount_Local);
                            reportinvoiceline.DebitInInvoiceAmount = String.Format("{0:#,0.00}", lineAmount_Invoice);
                            reportinvoiceline.DebitInForeignAmount = String.Format("{0:#,0.00}", lineAmount_Foreign);

                            reportinvoiceline.CreditInLocalAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.CreditInInvoiceAmount = String.Format("{0:#,0.00}", 0);
                            reportinvoiceline.CreditInForeignAmount = String.Format("{0:#,0.00}", 0);
                        }

                        if (invoiceTypeCode == "CD")
                        {
                            line_UnitPrice = line_UnitPrice * -1;
                            lineAmount_Foreign = lineAmount_Foreign * -1;
                            lineAmount_Invoice = lineAmount_Invoice * -1;
                            lineAmount_Local = lineAmount_Local * -1;
                        }

                        reportinvoiceline.UnitPrice = line_UnitPrice != null ? String.Format("{0:#,0.00}", line_UnitPrice) : "";
                        reportinvoiceline.UnitPriceDouble = line_UnitPrice == null ? 0 : line_UnitPrice.Value;
                        reportinvoiceline.LocalAmount = lineAmount_Local != null ? String.Format("{0:#,0.00}", lineAmount_Local.Value) : "";
                        reportinvoiceline.InvoiceAmount = lineAmount_Invoice != null ? String.Format("{0:#,0.00}", lineAmount_Invoice.Value) : "";
                        reportinvoiceline.ForeignAmount = lineAmount_Foreign != null ? String.Format("{0:#,0.00}", lineAmount_Foreign.Value) : "";
                        reportinvoiceline.LocalAmount_Double = lineAmount_Local;
                        reportinvoiceline.InvoiceAmount_Double = lineAmount_Invoice;

                        reportinvoiceline.VatAmountIncludeMultiInInvoiceCurrency = this.GetVatAmountIncludeMultiInInvoiceCurrencyField(invoiceline.VatTypeId, invoiceline.InvoiceCurrencyAmount, invoiceTypeCode, allVATTypes, allVatTypesPercentages, allVATTypesGroups);

                        #region VAT
                        if (invoiceline.VatTypeId != null)
                        {
                            VatType vattype = allVATTypes.Where(d => d.Id == invoiceline.VatTypeId).FirstOrDefault();
                            if (vattype != null)
                            {
                                reportinvoiceline.VatType = vattype.EnglishName != null ? vattype.EnglishName : "";
                                reportinvoiceline.VatTypePercentage = "";
                                reportinvoiceline.VATDescription = vattype.Description;
                                reportinvoiceline.VATLocalDescription = vattype.LocalDescription;

                                if (!vattype.IsMultiPercentage)
                                {
                                    double? myPercentage = invoiceline.VatPercentage;
                                    reportinvoiceline.VatTypePercentage = myPercentage != null ? String.Format("{0:#,0.00}", myPercentage) + " %" : "";

                                    if (myPercentage != null && myPercentage != 0)
                                    {
                                        reportinvoiceline.VatIndication = "*";
                                        reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceCurrencyAmount);
                                        reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceCurrencyAmount;
                                    }
                                    else
                                    {
                                        reportinvoiceline.VatIndication = "";
                                        reportinvoiceline.NONVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceCurrencyAmount);
                                        reportinvoiceline.NONVATableAmountInInvoiceCurrency_double = invoiceline.InvoiceCurrencyAmount;
                                    }

                                    double vatamountinlocalcurrency = ((invoiceline.LocalCurrencyAmount != null ? invoiceline.LocalCurrencyAmount : 0) * (myPercentage != null ? (myPercentage / 100) : 0)).Value;
                                    double vatamountininvoicecurrecy = ((invoiceline.InvoiceCurrencyAmount != null ? invoiceline.InvoiceCurrencyAmount : 0) * (myPercentage != null ? (myPercentage / 100) : 0)).Value;
                                    double vatamountinForeigncurrecy = ((invoiceline.ForiegnCurrencyAmount != null ? invoiceline.ForiegnCurrencyAmount : 0) * (myPercentage != null ? (myPercentage / 100) : 0)).Value;

                                    if (invoiceTypeCode == "CD")
                                    {
                                        vatamountinlocalcurrency = vatamountinlocalcurrency * -1;
                                        vatamountininvoicecurrecy = vatamountininvoicecurrecy * -1;
                                        vatamountinForeigncurrecy = vatamountinForeigncurrecy * -1;
                                    }

                                    reportinvoiceline.VatAmountInLocalCurrency = String.Format("{0:#,0.00}", vatamountinlocalcurrency);
                                    reportinvoiceline.VatAmountInInvoiceCurrency = String.Format("{0:#,0.00}", vatamountininvoicecurrecy);
                                    reportinvoiceline.VatAmountInForeignCurrency = String.Format("{0:#,0.00}", vatamountinForeigncurrecy);

                                    reportinvoiceline.LocalAmountWithVAT = lineAmount_Local + vatamountinlocalcurrency;
                                }

                                else
                                {
                                    reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceCurrencyAmount);
                                    reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceCurrencyAmount;
                                }
                            }
                        }
                        #endregion

                        Measurement myMeasurement = allMeasurements.Where(d => d.Id == invoiceline.MeasurementId).FirstOrDefault();
                        if (myMeasurement != null)
                        {
                            reportinvoiceline.Measurement = myMeasurement.Code;

                            if (myMeasurement.Code == "PRVL" || myMeasurement.Code == "PRFR")
                            {
                                reportinvoiceline.UOMPercentage = "%";
                            }

                            if (satSetting != null && (satSetting.SATInterfaceCode == "PROF" || satSetting.SATInterfaceCode == "PROF33"))
                            {
                                ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(currentInvoice.Tenant);
                                reportinvoiceline.ClaveUnidad = computingPartnerHelper.GetComputingPartnerCodeTranslation(myMeasurement.Code, "G-Profact", "Measurement");
                            }
                        }

                        reportinvoiceline.Quantity = invoiceline.Quantity != null ? String.Format("{0:#,0.00}", invoiceline.Quantity) : "";
                        totalQuantity += invoiceline.Quantity != null ? invoiceline.Quantity.Value : 0;

                        ChargesType chargetype = allChargesTypes.Where(d => d.Id == invoiceline.ChargesTypeId).FirstOrDefault();
                        if (chargetype != null)
                        {
                            reportinvoiceline.ChargeType = chargetype.EnglishName != null ? chargetype.EnglishName : "";
                            reportinvoiceline.ChargeTypeLocalName = chargetype.LocalName != null ? chargetype.LocalName : "";
                            reportinvoiceline.ChrageTypeCode = chargetype.Code != null ? chargetype.Code : "";
                            reportinvoiceline.ClaveProdServ = chargetype.SATExternalId;
                        }

                        if (foreigncurrency != null && invoicecurrency != null)
                        {
                            double? foreignExchangeRate = 0;

                            if (foreigncurrency == invoicecurrency)
                            {
                                foreignExchangeRate = 1;
                            }

                            else
                            {
                                foreignExchangeRate = invoiceline.ForiegnExchangeRate / currentInvoice.InvoiceCurrencyExchangeRate;
                            }

                            reportinvoiceline.ForeignToInvoiceExchangeRate = "1 " + foreigncurrency.Code + " = " + Math.Round(foreignExchangeRate.Value, 2) + " " + invoicecurrency.Code;
                        }

                        invoicedataprovider.InvoiceLinesList.Add(reportinvoiceline);
                    }
                    #endregion

                    invoicedataprovider.Quantity = String.Format("{0:#,0.##}", totalQuantity);
                }

                invoicedataprovider.TotalVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoicedataprovider.InvoiceLinesList.Sum(s => s.VATableAmountInInvoiceCurrency_double));
                invoicedataprovider.TotalNONVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoicedataprovider.InvoiceLinesList.Sum(s => s.NONVATableAmountInInvoiceCurrency_double));

                invoicedataprovider.ExpenseInvoiceLinesList = invoicedataprovider.InvoiceLinesList.Where(d => d.IsExpense).ToList();
                invoicedataprovider.NoExpenseInvoiceLinesList = invoicedataprovider.InvoiceLinesList.Where(d => !d.IsExpense).ToList();
                #endregion

                #region Total VAT

                List<ARInvoiceTotalVAT> totalVats = invoiceCotnext.ARInvoiceTotalVATs.Where(d => d.ARInvoiceId == currentInvoice.Id && d.Tenant == currentInvoice.Tenant).ToList();

                this.GenerateTotalVATs(invoicedataprovider, totalVats, commonContext, invoiceTypeCode);
                this.GenerateTotalVATs_Expense(invoicedataprovider, commonContext, invoiceLines, invoiceTypeCode, tenant);

                #endregion

                if (invoicedataprovider.Status == "Draft")
                {
                    invoicedataprovider.Draft_labelHebrew = "פרופורמה";
                    invoicedataprovider.Draft_label = "Draft";
                }

                else
                {
                    invoicedataprovider.Draft_labelHebrew = "";
                    invoicedataprovider.Draft_label = "";
                }

                invoicedataprovider.Logo = DataProviders.General.GetLogo(tenantSettings.Id);

                Card cards = (from a in commonContext.Cards
                              where a.Id == currentInvoice.BillTo.Id
                              select a).FirstOrDefault();

                invoicedataprovider.InvoiceSection1 = tenantSettings.InvoiceSection1;
                invoicedataprovider.InvoiceSection2 = tenantSettings.InvoiceSection2;
                invoicedataprovider.BankDetails = tenantSettings.BankDetails;

                invoicedataprovider.BillToBankName = cards.BankName;
                invoicedataprovider.BillToBankAddress = cards.BankAddress;
                invoicedataprovider.BillToAccountNumber = cards.AccountNumber;
                invoicedataprovider.BillToIBANNumber = cards.IBANNumber;
                invoicedataprovider.BillToSwift = cards.Swift;
                #endregion

                #region SATInterface Properties

                if (!string.IsNullOrEmpty(currentInvoice.SATPaymentMethodCode))
                {
                    SATPaymentMethod arPaymentMethod = (from inty in invoiceCotnext.SATPaymentMethods where inty.Code == currentInvoice.SATPaymentMethodCode select inty).FirstOrDefault();
                    if (arPaymentMethod != null)
                    {
                        invoicedataprovider.SAT.PaymentMethodCode = currentInvoice.SATPaymentMethodCode;
                        invoicedataprovider.SAT.PaymentMethodName = arPaymentMethod.Name;
                        invoicedataprovider.SAT.PaymentMethodLocalName = arPaymentMethod.LocalName;
                    }
                }


                if (satSetting != null && (satSetting.SATInterfaceCode == "PROF" || satSetting.SATInterfaceCode == "PROF33"))
                {
                    if (!string.IsNullOrEmpty(currentInvoice.SATXML))
                    {
                        if (satSetting.SATInterfaceCode == "PROF")
                        {
                            MapProfact32Fields(currentInvoice, invoicedataprovider, tenantSettings);
                        }
                        else
                        {
                            MapProfact33Fields(currentInvoice, invoicedataprovider, tenantSettings);
                        }
                    }
                    else
                    {

                        invoicedataprovider.SAT.RegimenFiscal = "601";
                        if (currentInvoice.ARInvoiceTypeCode == "CD")
                            invoicedataprovider.SAT.TipoDeComprobante = "E";
                        else
                            invoicedataprovider.SAT.TipoDeComprobante = "I";

                        if (!string.IsNullOrEmpty(currentInvoice.MetodoPagoCode))
                        {
                            invoicedataprovider.SAT.MetodoPago = (currentInvoice.MetodoPagoCode == "PUE" ? "PUE Pago en una sola exhibición" : "PPD Pago en parcialidades o diferido");
                        }
                        invoicedataprovider.SAT.FormadePago = currentInvoice.SATPaymentMethodCode;


                        invoicedataprovider.WaterMark = "Draft";
                        invoicedataprovider.CopyName = "Draft";
                        invoicedataprovider.CopyName_hebrew = "פרופורמה";
                        invoicedataprovider.InvoiceNumber = currentInvoice.DraftNumber != null ? currentInvoice.DraftNumber : "";
                    }
                }

                #endregion

                #region Expense
                invoicedataprovider.ExpenseSubTotalLocalCurr = invoicedataprovider.ExpenseInvoiceLinesList.Sum(s => s.LocalAmount_Double);
                invoicedataprovider.ExpenseSubTotalInvoiceCurr = invoicedataprovider.ExpenseInvoiceLinesList.Sum(s => s.InvoiceAmount_Double);

                if (invoicedataprovider.ExpenseTotalVatList != null)
                {
                    invoicedataprovider.ExpenseTotalLocalCurr = invoicedataprovider.ExpenseSubTotalLocalCurr + invoicedataprovider.ExpenseTotalVatList.Sum(s => s.TotalVatAmountInLocalCurrency_Double);
                    invoicedataprovider.ExpenseTotalInvoiceCurr = invoicedataprovider.ExpenseSubTotalInvoiceCurr + invoicedataprovider.ExpenseTotalVatList.Sum(s => s.TotalVatAmountInInvoiceCurrency_Double);
                    var resultOfExpenseTotal = decimal.Parse(invoicedataprovider.ExpenseTotalInvoiceCurr + "") - Math.Truncate(decimal.Parse(invoicedataprovider.ExpenseTotalInvoiceCurr + ""));
                    var resulyFirstdigits = (int)(Math.Round(resultOfExpenseTotal, 2) * 100);
                    string resultstr = "";
                    if (resulyFirstdigits < 10 && resulyFirstdigits > 0)
                        resultstr = 0 + "" + resulyFirstdigits + "/100";
                    else
                        resultstr = resulyFirstdigits + "/100";

                    if ((int)(Math.Round(resultOfExpenseTotal, 2) * 100) <= 0)
                    {
                        resultstr = "";
                    }

                    var FrenchFractionsExpense = "";
                    if (resulyFirstdigits > 0)
                    {
                        FrenchFractionsExpense = resulyFirstdigits + " Cts";
                    }
                    invoicedataprovider.AmountInWordsExpenseTotalInvoiceCurrSpanish = NumbersToSpanish((int)invoicedataprovider.ExpenseTotalInvoiceCurr) + " " + invoicedataprovider.InvoicecurrencyLocalName + " " + resultstr;
                    invoicedataprovider.AmountInWordsExpenseTotalInvoiceCurrFrench = NumbersToFrench((int)invoicedataprovider.ExpenseTotalInvoiceCurr) + " " + invoicedataprovider.InvoicecurrencyLocalName + " " + FrenchFractionsExpense;
                }
                #endregion

                #region NoExpense
                invoicedataprovider.NoExpenseSubTotalLocalCurr = invoicedataprovider.NoExpenseInvoiceLinesList.Sum(s => s.LocalAmount_Double);
                invoicedataprovider.NoExpenseSubTotalInvoiceCurr = invoicedataprovider.NoExpenseInvoiceLinesList.Sum(s => s.InvoiceAmount_Double);

                if (invoicedataprovider.NoExpenseTotalVatList != null)
                {
                    invoicedataprovider.NoExpenseTotalLocalCurr = invoicedataprovider.NoExpenseSubTotalLocalCurr + invoicedataprovider.NoExpenseTotalVatList.Sum(s => s.TotalVatAmountInLocalCurrency_Double);
                    invoicedataprovider.NoExpenseTotalInvoiceCurr = invoicedataprovider.NoExpenseSubTotalInvoiceCurr + invoicedataprovider.NoExpenseTotalVatList.Sum(s => s.TotalVatAmountInInvoiceCurrency_Double);

                    var resultOfNoneExpenseTotal = decimal.Parse(invoicedataprovider.NoExpenseTotalInvoiceCurr + "") - Math.Truncate(decimal.Parse(invoicedataprovider.NoExpenseTotalInvoiceCurr + ""));
                    var resulyFirstdigits = (int)(Math.Round(resultOfNoneExpenseTotal, 2) * 100);
                    string resultstr = "";
                    if (resulyFirstdigits < 10 && resulyFirstdigits > 0)
                        resultstr = 0 + "" + resulyFirstdigits + "/100";
                    else
                        resultstr = resulyFirstdigits + "/100";

                    if ((int)(Math.Round(resultOfNoneExpenseTotal, 2) * 100) <= 0)
                    {
                        resultstr = "";
                    }

                    var FrenchFractionsNoneExpense = "";
                    if (resulyFirstdigits > 0)
                    {
                        FrenchFractionsNoneExpense = resulyFirstdigits + " Cts";
                    }


                    invoicedataprovider.AmountInWordsNoExpenseTotalInvoiceCurrSpanish = NumbersToSpanish((int)invoicedataprovider.NoExpenseTotalInvoiceCurr) + " " + invoicedataprovider.InvoicecurrencyLocalName + " " + resultstr;
                    invoicedataprovider.AmountInWordsNoExpenseTotalInvoiceCurrFrench = NumbersToFrench((int)invoicedataprovider.NoExpenseTotalInvoiceCurr) + " " + invoicedataprovider.InvoicecurrencyLocalName + " " + FrenchFractionsNoneExpense;
                }
                #endregion

                #region DepositBank
                if (currentInvoice.BankAccountLiteId != null)
                {
                    BankAccountLite myBankAccountLite = (from d in invoiceCotnext.BankAccountLites where d.Tenant == tenant && d.Id == currentInvoice.BankAccountLiteId select d).FirstOrDefault();
                    if (myBankAccountLite != null)
                    {
                        invoicedataprovider.DepositBankEnglishName = myBankAccountLite.EnglishName;
                        invoicedataprovider.DepositBankLocalName = myBankAccountLite.LocalName;
                        invoicedataprovider.DepositBankSwiftCode = myBankAccountLite.SwiftCode;
                        invoicedataprovider.DepositBankIBAN = myBankAccountLite.IBAN;
                    }
                }
                #endregion
            }

            //------------------------------------------------
            Type invoiceType = invoicedataprovider.GetType();
            PropertyInfo[] properties = invoiceType.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                try
                {
                    if (pi.Name != "InvoiceLinesList")
                    {
                        if (pi.GetValue(invoicedataprovider, null) == null || pi.GetValue(invoicedataprovider, null).ToString() == "0" || pi.GetValue(invoicedataprovider, null).ToString() == "00.00")
                        {
                            pi.SetValue(invoicedataprovider, "", null);
                        }
                    }
                }

                catch
                {

                }
            }

            return invoicedataprovider;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return "";
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        private static void MapProfact32Fields(ARInvoice currentInvoice, InvoiceDataProvider invoicedataprovider, Tenant tenantSettings)
        {
            Profact.TimbraCFDI.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.Comprobante>(currentInvoice.SATXML);
            //invoicedataprovider.SelloSAT = comprobante.sello;
            //invoicedataprovider.NoCertificadoSAT = comprobante.noCertificado;
            if (comprobante.Complemento.Any != null)
            {
                List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                if (timbreFiscalDigitalElement != null)
                {
                    Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);

                    invoicedataprovider.SAT.SelloSAT = digitalTi.selloSAT;
                    invoicedataprovider.SAT.NoCertificadoSAT = digitalTi.noCertificadoSAT;
                    invoicedataprovider.SAT.SelloCFD = digitalTi.selloCFD;
                    invoicedataprovider.SAT.UUID = digitalTi.UUID;

                    invoicedataprovider.SAT.FechaTimbardo = digitalTi.FechaTimbrado;
                    invoicedataprovider.SAT.NoCertificado = comprobante.noCertificado;

                    string invtotal = invoicedataprovider.TotalInvoiceCurr.ToString("0000000000.000000");

                    invoicedataprovider.SAT.QR = "?re=" + tenantSettings.VatNumber + "&rr=" + invoicedataprovider.BillToVatNumber + "&tt=" + invtotal + "&id=" + digitalTi.UUID;





                    //invoicedataprovider.SAT.RegimenFiscal
                    invoicedataprovider.SAT.FormadePago = comprobante.formaDePago;
                    if (comprobante.Emisor.RegimenFiscal.Length > 0)
                    {
                        invoicedataprovider.SAT.RegimenFiscal = comprobante.Emisor.RegimenFiscal[0].Regimen;
                    }


                    if (comprobante.tipoDeComprobante == Profact.TimbraCFDI.ComprobanteTipoDeComprobante.egreso)
                    {
                        invoicedataprovider.SAT.TipoDeComprobante = "Egreso";
                    }
                    else
                    {
                        invoicedataprovider.SAT.TipoDeComprobante = "Ingreso";
                    }

                }


                //< cfdi:RegimenFiscal Regimen = "Regimen general de ley personas morales" />
                /*
                 * 
                 * Add to the invoice data provider a new variable:
                    call it QRSAT
                    It should display the following: ?re=tenant VAT& rr=Bill to VAT&tt=Invoice Total amount&id=UUID
                    Tenant VAT, Bill to VAT and UUID all are variables
                    The total invoice amount should have 10 digits on the left side of the (.) and 6 digits on the right side. Therefore, you should always add padding zeros to match what we need.
                    Ex: 0000000915.120000, 0000011253.126000
                 * */

                string cadenaOriginalString = GetCadenaOrignialField(currentInvoice, "32");
                invoicedataprovider.SAT.CadenaOriginal = cadenaOriginalString;
            }
        }

        private static void MapProfact33Fields(ARInvoice currentInvoice, InvoiceDataProvider invoicedataprovider, Tenant tenantSettings)
        {
            UsoCFDIRepository usoCFDIRepository = new UsoCFDIRepository(currentInvoice.Tenant);
            List<UsoCFDI> allUsoCFDIs = usoCFDIRepository.GetUsoCFDIs().ToList();
            Profact.TimbraCFDI33.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(currentInvoice.SATXML);
            //invoicedataprovider.SelloSAT = comprobante.sello;
            //invoicedataprovider.NoCertificadoSAT = comprobante.noCertificado;
            if (comprobante.Complemento.Any != null)
            {
                List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                if (timbreFiscalDigitalElement != null)
                {
                    //Profact.TimbraCFDI.get


                    Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);


                    invoicedataprovider.SAT.SelloSAT = GetSATTimbreFiscalDigitalValue("SelloSAT", timbreFiscalDigitalElement); //digitalTi.selloSAT;
                    invoicedataprovider.SAT.NoCertificadoSAT = GetSATTimbreFiscalDigitalValue("NoCertificadoSAT", timbreFiscalDigitalElement);//digitalTi.noCertificadoSAT;
                    invoicedataprovider.SAT.SelloCFD = GetSATTimbreFiscalDigitalValue("SelloCFD", timbreFiscalDigitalElement);//digitalTi.selloCFD;
                    invoicedataprovider.SAT.UUID = digitalTi.UUID;
                    invoicedataprovider.SAT.FechaTimbardo = digitalTi.FechaTimbrado;

                    invoicedataprovider.SAT.NoCertificado = comprobante.NoCertificado;


                    //invoicedataprovider.SAT.QR = "?re=" + tenantSettings.VatNumber + "&rr=" + invoicedataprovider.BillToVatNumber + "&tt=" + invtotal + "&id=" + digitalTi.UUID;
                    string invtotal = invoicedataprovider.TotalInvoiceCurr.ToString();//.ToString("0000000000.000000");
                    string fe = invoicedataprovider.SAT.SelloCFD.Substring(invoicedataprovider.SAT.SelloCFD.Length - 8, 8);

                    invoicedataprovider.SAT.QR = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx?" + "&id=" + digitalTi.UUID + "&re=" + tenantSettings.VatNumber + "&rr=" + invoicedataprovider.BillToVatNumber + "&tt=" + invtotal
                        + "&fe=" + fe;


                    //invoicedataprovider.SAT.RegimenFiscal
                    invoicedataprovider.SAT.FormadePago = comprobante.FormaPago;
                    if (comprobante.Emisor.RegimenFiscal != null && comprobante.Emisor.RegimenFiscal.Length > 0)
                    {
                        invoicedataprovider.SAT.RegimenFiscal = comprobante.Emisor.RegimenFiscal;
                    }


                    if (comprobante.TipoDeComprobante == "E")
                    {
                        invoicedataprovider.SAT.TipoDeComprobante = "Egreso";
                    }
                    else
                    {
                        invoicedataprovider.SAT.TipoDeComprobante = "Ingreso";
                    }
                    invoicedataprovider.SAT.LugardeExpedicion = comprobante.LugarExpedicion;

                    if (!string.IsNullOrEmpty(comprobante.MetodoPago))
                    {
                        invoicedataprovider.SAT.MetodoPago = (comprobante.MetodoPago == "PUE" ? "PUE Pago en una sola exhibición" : "PPD Pago en parcialidades o diferido");
                    }

                    UsoCFDI usoCFDI = allUsoCFDIs.FirstOrDefault(f => f.Code == comprobante.Receptor.UsoCFDI);
                    if (usoCFDI != null)
                    {
                        invoicedataprovider.SAT.usoCFDI = usoCFDI.Code + " " + usoCFDI.Name;
                    }

                    if (!string.IsNullOrEmpty(currentInvoice.SATAdditionalFieldsXML))
                    {
                        SATAdditionalFields additionalFields = LogitudeXmlSerializer.DeserializeObject<SATAdditionalFields>(currentInvoice.SATAdditionalFieldsXML);
                        invoicedataprovider.SAT.CadenaOriginal = additionalFields.CadenaOriginal;
                        if (!string.IsNullOrEmpty(additionalFields.QRImage))
                        {
                            invoicedataprovider.SAT.QRImage = Image.FromStream(new MemoryStream(Convert.FromBase64String(additionalFields.QRImage)));
                        }

                        //if (!string.IsNullOrEmpty(additionalFields.QRImage))
                        //{
                        //    ImageDetailRepository imageDetailRep = new ImageDetailRepository(currentInvoice.Tenant);
                        //    ImageDetail imgDet = imageDetailRep.GetSingleImageDetail(additionalFields.QRImage, currentInvoice.Tenant);

                        //    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

                        //    //tempcloudBlockBlob.PutBlock(blockIdsList[bufferNumber], memorystream, null);
                        //    BlobFileInfo fileInfo = new BlobFileInfo()
                        //    {
                        //        FileName = additionalFields.QRImage,
                        //        FolderName = "SATInterface",
                        //        Extension = imgDet.Extension,
                        //        Tenant = currentInvoice.Tenant,
                        //        FileSize = imgDet.Size,

                        //    };

                        //    invoicedataprovider.SAT.QRImage = storageservice.Read(fileInfo);


                        //}
                    }

                    if (comprobante.CfdiRelacionados != null)
                    {
                        invoicedataprovider.SAT.TipoRelacion = comprobante.CfdiRelacionados.TipoRelacion;
                        if (invoicedataprovider.SAT.TipoRelacion == "01")
                        {
                            invoicedataprovider.SAT.TipoRelacion = "01 Nota de crédito de los documentos relacionados";
                        }
                        else if (invoicedataprovider.SAT.TipoRelacion == "02")
                        {
                            invoicedataprovider.SAT.TipoRelacion = "02 Nota de débito de los documentos relacionados";
                        }
                        else if (invoicedataprovider.SAT.TipoRelacion == "04")
                        {
                            invoicedataprovider.SAT.TipoRelacion = "04 Sustitución de los CFDI previos";
                        }

                        if (comprobante.CfdiRelacionados.CfdiRelacionado != null && comprobante.CfdiRelacionados.CfdiRelacionado.Length != 0)
                        {
                            invoicedataprovider.SAT.CFDIRelacionado = comprobante.CfdiRelacionados.CfdiRelacionado[0].UUID;
                        }

                    }

                }


                //< cfdi:RegimenFiscal Regimen = "Regimen general de ley personas morales" />
                /*
                 * 
                 * Add to the invoice data provider a new variable:
                    call it QRSAT
                    It should display the following: ?re=tenant VAT& rr=Bill to VAT&tt=Invoice Total amount&id=UUID
                    Tenant VAT, Bill to VAT and UUID all are variables
                    The total invoice amount should have 10 digits on the left side of the (.) and 6 digits on the right side. Therefore, you should always add padding zeros to match what we need.
                    Ex: 0000000915.120000, 0000011253.126000
                 * */
            }


            //Cargar el XML

            //string cadenaOriginalString = GetCadenaOrignialField(currentInvoice, "33");
            //invoicedataprovider.SAT.CadenaOriginal = cadenaOriginalString;

        }

        private static string GetSATTimbreFiscalDigitalValue(string attributeName, System.Xml.XmlElement timbreFiscalDigitalElement)
        {
            string value = "";
            if (timbreFiscalDigitalElement != null && timbreFiscalDigitalElement.Attributes[attributeName] != null)
            {
                value = timbreFiscalDigitalElement.Attributes[attributeName].Value;
            }

            return value;
        }

        private static string GetCadenaOrignialField(ARInvoice currentInvoice, string profactVersion)
        {
            XmlTextReader mlTxtreader = new XmlTextReader(new System.IO.StringReader(currentInvoice.SATXML));
            mlTxtreader.Read();
            XPathDocument myXPathDoc = new XPathDocument(mlTxtreader);
            string path = HttpContext.Current.Server.MapPath("~/Resources");
            path += "\\SATFiles\\";
            string fileName = "";
            if (profactVersion == "33")
            {
                fileName = "cadenaoriginal_3_3.xslt";
            }
            else
                fileName = "cadenaoriginal_3_2.xslt";

            path += fileName;

            XslCompiledTransform myXslTrans = new XslCompiledTransform();

            //string xsltFileContent = LogitudeCacheManager.ServerCache.GetFromCache(fileName);

            myXslTrans.Load(path);

            StringWriter str = new StringWriter();
            XmlTextWriter myWriter = new XmlTextWriter(str);

            //Aplicando transformacion
            myXslTrans.Transform(myXPathDoc, null, myWriter);

            //Resultado
            string cadenaOriginalString = str.ToString();
            return cadenaOriginalString;
        }

        private InvoiceDataProvider GetConsolidationInvoiceDataProvider(ARInvoice entityPOCO, ARInvoiceRepository invoiceRepository, IInvoiceContext invoiceCotnext, string documentTypeCopyId, int tenant)
        {
            InvoiceDataProvider invoiceDataProvider = new InvoiceDataProvider();

            if (entityPOCO != null)
            {
                string invoiceId = entityPOCO.Id;
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                AddressRepository addressRepository = new AddressRepository(commonContext);
                ContactRepository contactRepository = new ContactRepository(commonContext);
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
                WebServiceHelper myServicHelper = new WebServiceHelper(tenant);
                SATInterfaceSettingRepository satInterfaceSettingRepository = new SATInterfaceSettingRepository(invoiceCotnext);
                SATInterfaceSetting satSetting = satInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);
                Tenant tenantSettings = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();
                List<VatType> allVATTypes = (from d in commonContext.VatTypes where d.Tenant == tenant select d).ToList();
                List<VATTypesGroup> allVATTypesGroups = (from d in commonContext.VATTypesGroups where d.Tenant == tenant select d).ToList();
                List<VatTypePercentage> allVatTypesPercentages = (from d in commonContext.VatTypePercentages where d.Tenant == tenant select d).ToList();
                List<Currency> allCurrencies = (from d in commonContext.Currencies where d.Tenant == tenant select d).ToList();
                List<Measurement> allMeasurements = (from d in commonContext.Measurements where d.Tenant == tenant select d).ToList();
                List<ChargesType> allChargesTypes = (from d in commonContext.ChargesTypes where d.Tenant == tenant select d).ToList();

                #region Tenant Properties
                Tenant myTenant = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();
                if (myTenant != null)
                {
                    invoiceDataProvider.Signature = myTenant.Signature != null ? myTenant.Signature : "";
                    invoiceDataProvider.VatNumber = myTenant.VatNumber != null ? myTenant.VatNumber : "";
                    invoiceDataProvider.InvoiceSection1 = myTenant.InvoiceSection1;
                    invoiceDataProvider.InvoiceSection2 = myTenant.InvoiceSection2;
                    invoiceDataProvider.BankDetails = myTenant.BankDetails;
                    invoiceDataProvider.Logo = DataProviders.General.GetLogo(myTenant.Id);
                }
                #endregion

                #region DocumentTypeCopy
                DocumentTypeCopy myDocumentTypeCopy = (from copy in commonContext.DocumentTypeCopies where copy.Id == documentTypeCopyId select copy).FirstOrDefault();
                if (myDocumentTypeCopy != null)
                {

                    invoiceDataProvider.CopyName = myDocumentTypeCopy.Name != null ? myDocumentTypeCopy.Name : "";
                    invoiceDataProvider.CopyNameNoDraft = myDocumentTypeCopy.Name != null ? myDocumentTypeCopy.Name : "";

                    switch (myDocumentTypeCopy.Code)
                    {
                        case "999CI":
                        case "999S":
                        case "999M":
                        case "999C":
                        case "999G":
                            invoiceDataProvider.CopyName_hebrew = "מקור";
                            break;

                        case "999C1":
                        case "999G1":
                            invoiceDataProvider.CopyName_hebrew = "עותק";
                            break;

                        case "999C2":
                            invoiceDataProvider.CopyName_hebrew = "עותק 2";
                            break;

                        case "999C3":
                            invoiceDataProvider.CopyName_hebrew = "עותק אלקטרוני";
                            break;
                    }
                }
                #endregion

                #region Invoice Properies

                ARInvoiceType invoiceType = (from inty in invoiceCotnext.ARInvoiceTypes where inty.Code == entityPOCO.ARInvoiceTypeCode select inty).FirstOrDefault();
                if (invoiceType != null)
                {
                    invoiceDataProvider.InvoiceType_label = invoiceType != null ? invoiceType.Name : "";
                }

                switch (entityPOCO.ARInvoiceTypeCode)
                {
                    case "CD":
                        {
                            invoiceDataProvider.InvoiceType_labelHebrew = "הודעת זיכוי";
                            invoiceDataProvider.InvoiceType_label_Spanish = "Nota de Credito";
                            break;
                        }

                    case "IN":
                        {
                            invoiceDataProvider.InvoiceType_labelHebrew = "חשבונית";
                            invoiceDataProvider.InvoiceType_label_Spanish = "Factura";
                            break;
                        }

                    case "TX":
                        {
                            invoiceDataProvider.InvoiceType_labelHebrew = "Tax Invoice";
                            invoiceDataProvider.InvoiceType_label_Spanish = "Tax Invoice";
                            break;
                        }
                }

                invoiceDataProvider.Status = entityPOCO.Status != null ? entityPOCO.Status.Name : "";
                invoiceDataProvider.HouseNumber = entityPOCO.HouseNumber != null ? entityPOCO.HouseNumber : "";
                invoiceDataProvider.MasterNumber = entityPOCO.MasterNumber != null ? entityPOCO.MasterNumber : "";
                invoiceDataProvider.AccountingNumber = entityPOCO.DebitAccount != null ? entityPOCO.DebitAccount : !string.IsNullOrEmpty(invoiceDataProvider.DebitAccount) ? invoiceDataProvider.DebitAccount : "";
                invoiceDataProvider.InvoiceDate = entityPOCO.InvoiceDate != null ? String.Format("{0:dd.MMM.yyyy}", entityPOCO.InvoiceDate) : "";
                invoiceDataProvider.InvoiceDateAsDateFormat = entityPOCO.InvoiceDate;
                invoiceDataProvider.DueDate = entityPOCO.DueDate != null ? String.Format("{0:dd.MMM.yyyy}", entityPOCO.DueDate) : "";
                invoiceDataProvider.DueDateAsDateFormat = entityPOCO.DueDate;
                invoiceDataProvider.CustomerRef = entityPOCO.CustomerRef;
                invoiceDataProvider.BillToVatNumber = entityPOCO.VatNumber != null ? entityPOCO.VatNumber : "";
                invoiceDataProvider.Notes = entityPOCO.PrintNotes != null ? entityPOCO.PrintNotes : "";

                invoiceDataProvider.ApprovedDate = entityPOCO.ApprovedDate != null ? String.Format("{0:dd.MMM.yyyy}", entityPOCO.ApprovedDate) : "";
                invoiceDataProvider.ApprovedDateAsDateFormat = entityPOCO.ApprovedDate;

                invoiceDataProvider.AmountDueInInvoiceCurrency = entityPOCO.AmountDue;
                invoiceDataProvider.AmountDueInLocalCurrency = entityPOCO.AmountDueInLocalCurrency;

                if (entityPOCO.ApprovedByUser != null)
                {
                    if (entityPOCO.ApprovedByUser.Contact != null)
                    {
                        invoiceDataProvider.ApprovedBy = entityPOCO.ApprovedByUser.Contact.EnglishName;
                    }
                }

                if (entityPOCO.IsAutoCredit)
                {
                    invoiceDataProvider.OriginalInvoiceNumber = invoiceCotnext.ARInvoices.Where(i => i.CancelledByARInvoiceId == invoiceId).FirstOrDefault().InvoiceNumber;
                }

                if (!string.IsNullOrEmpty(invoiceDataProvider.OriginalInvoiceNumber))
                {
                    invoiceDataProvider.OriginalInvoiceNumber_label = "Original Invoice Number";
                }

                else
                {
                    invoiceDataProvider.OriginalInvoiceNumber_label = "";
                }

                if (entityPOCO.StatusCode == "DR")
                {
                    invoiceDataProvider.WaterMark = invoiceDataProvider.Status;
                    invoiceDataProvider.CopyName = invoiceDataProvider.Status;
                    invoiceDataProvider.CopyName_hebrew = "פרופורמה";
                    invoiceDataProvider.InvoiceNumber = entityPOCO.DraftNumber != null ? entityPOCO.DraftNumber : "";
                    invoiceDataProvider.Draft_labelHebrew = "פרופורמה";
                    invoiceDataProvider.Draft_label = "Draft";
                }

                else if (entityPOCO.StatusCode == "LL")
                {
                    invoiceDataProvider.WaterMark = invoiceDataProvider.Status;
                    invoiceDataProvider.CopyName = invoiceDataProvider.Status;
                    invoiceDataProvider.CopyName_hebrew = "מבוטלת";
                    invoiceDataProvider.InvoiceNumber = entityPOCO.DraftNumber != null ? entityPOCO.DraftNumber : "";
                    invoiceDataProvider.Draft_labelHebrew = "מבוטלת";
                    invoiceDataProvider.Draft_label = "Cancelled";
                }

                else
                {
                    invoiceDataProvider.Draft_labelHebrew = "";
                    invoiceDataProvider.Draft_label = "";
                    invoiceDataProvider.InvoiceNumber = entityPOCO.InvoiceNumber != null ? entityPOCO.InvoiceNumber : "";
                }

                if (invoiceDataProvider.Status == "Void")
                {
                    invoiceDataProvider.WaterMark = invoiceDataProvider.Status;
                }

                if (!string.IsNullOrEmpty(entityPOCO.PaymentTermId))
                {
                    PaymentTermRepository paymentTermRepository = new PaymentTermRepository(tenant);
                    PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTerm(entityPOCO.PaymentTermId, tenant);

                    if (paymentTerm != null)
                    {
                        invoiceDataProvider.PaymentTerm = paymentTerm.EnglishName != null ? paymentTerm.EnglishName : "";
                        invoiceDataProvider.PaymentTerm_Local = paymentTerm.LocalName != null ? paymentTerm.LocalName : "";
                        invoiceDataProvider.PaymentTermDescription = paymentTerm.Description != null ? paymentTerm.Description : "";
                        invoiceDataProvider.PaymentTermLocalDescription = paymentTerm.LocalDescription != null ? paymentTerm.LocalDescription : "";
                    }
                }
                #endregion

                #region Bill To Properties
                if (!string.IsNullOrEmpty(entityPOCO.BillToId))
                {
                    Card billToCard = CardRepository.GetSingleCard(entityPOCO.BillToId, tenant, false);
                    if (billToCard != null)
                    {
                        invoiceDataProvider.BillTo = billToCard.EnglishName != null ? billToCard.EnglishName + Environment.NewLine : "";
                        invoiceDataProvider.BillTo_LocalName = billToCard.LocalName != null ? billToCard.LocalName : "";
                        invoiceDataProvider.BillToCustomerCode = billToCard.Code;

                        if (!string.IsNullOrEmpty(entityPOCO.BillToAddressId))
                        {
                            Address billToAddress = addressRepository.GetSingleAddress(entityPOCO.BillToAddressId, tenant);

                            if (billToAddress != null)
                            {
                                invoiceDataProvider.BillToAddress_NoName = DataProviders.General.GetAddress(billToAddress);

                                if (billToAddress.IsLocalLanguage && !string.IsNullOrEmpty(invoiceDataProvider.BillTo_LocalName))
                                {
                                    invoiceDataProvider.BillToAddress = invoiceDataProvider.BillTo_LocalName + Environment.NewLine + DataProviders.General.GetAddress(billToAddress);
                                    invoiceDataProvider.BillToAddressDescription = billToAddress.Description;
                                }

                                else
                                {
                                    invoiceDataProvider.BillToAddress = invoiceDataProvider.BillTo + DataProviders.General.GetAddress(billToAddress);
                                    invoiceDataProvider.BillToAddressDescription = billToAddress.Description;
                                }

                                invoiceDataProvider.SAT.BillToZipCode = billToAddress.ZipCode;
                                invoiceDataProvider.BillToTelephone = billToAddress.PhoneNumber;
                            }
                        }

                        if (billToCard.PartnerTypeId == "CS")
                        {
                            CustomerQuery customerQuery = new CustomerQuery(tenant);
                            CustomerPM customerPM = customerQuery.GetSinglePM(billToCard.Id, tenant);
                            if (customerPM != null)
                            {
                                customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, customerPM, invoiceDataProvider);
                            }
                        }

                        Contact billToContact = contactRepository.GetSingleContact(billToCard.PrimaryContactId, tenant);
                        if (billToContact != null)
                        {
                            invoiceDataProvider.ContactPersonName = billToContact.EnglishName;
                            invoiceDataProvider.ContactPersonEmail = billToContact.Email;
                            invoiceDataProvider.BillToPrimaryContactMobile = billToContact.Mobile;
                            invoiceDataProvider.BillToPrimaryContactBusinessPhone = billToContact.BusinessPhone;
                        }

                        invoiceDataProvider.BillToBankName = billToCard.BankName;
                        invoiceDataProvider.BillToBankAddress = billToCard.BankAddress;
                        invoiceDataProvider.BillToAccountNumber = billToCard.AccountNumber;
                        invoiceDataProvider.BillToIBANNumber = billToCard.IBANNumber;
                        invoiceDataProvider.BillToSwift = billToCard.Swift;
                    }
                }
                #endregion

                #region CustomFieldResolver
                ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(invoiceRepository);
                ARInvoicePM entityPM = invoiceQuery.GetSinglePM(invoiceId, tenant);
                customFieldResolver.SetDataProviderCustomFieldsValues("ARInvoice", tenant, entityPM, invoiceDataProvider);
                #endregion

                #region PrintByUser
                if (!string.IsNullOrEmpty(entityPOCO.IssuedByUserId))
                {
                    UserRepository userRepository = new UserRepository(tenant);
                    User issuedByuser = userRepository.GetSingleUser(entityPOCO.IssuedByUserId, tenant, true);
                    if (issuedByuser != null)
                    {
                        Contact contact = issuedByuser.Contact;
                        if (contact != null)
                        {
                            invoiceDataProvider.IssuedByUser = contact.EnglishName != null ? contact.EnglishName : "";
                            invoiceDataProvider.IssuedByUser_LocalName = contact.LocalName != null ? contact.LocalName : "";
                        }

                        entityPOCO.PrintByUserId = issuedByuser.Id;
                        entityPOCO.PrintDate = TenantServerConfigration.GetCurrentDateTime(tenant);

                        switch (entityPOCO.StatusCode)
                        {
                            case "AD":
                            case "VD":
                            case "PD":
                            case "PP":
                            case "AR":
                            case "AC":
                                {
                                    entityPOCO.IsPrinted = true;
                                    break;
                                }
                        }

                        invoiceRepository.Update(entityPOCO);
                        invoiceRepository.SubmitChanges();
                    }
                }
                #endregion

                #region Invoice Lines

                Currency invoicecurrency = (from fc in commonContext.Currencies where fc.Id == entityPOCO.InvoiceCurrencyId select fc).FirstOrDefault();

                invoiceDataProvider.InvoiceLinesList = new List<ReportInvoiceLine>();
                invoiceDataProvider.ExpenseInvoiceLinesList = new List<ReportInvoiceLine>();
                invoiceDataProvider.NoExpenseInvoiceLinesList = new List<ReportInvoiceLine>();

                List<ARInvoiceLine> invoiceLines = invoiceCotnext.ARInvoiceLines.Where(d => d.ARInvoiceId == invoiceId && d.Tenant == tenant).OrderBy(o => o.ChargesType.ViewOrder).ToList();

                List<string> foriegnCurrencies = invoiceLines.GroupBy(g => new { g.ForiegnCurrencyId }).Select(s => s.Key.ForiegnCurrencyId).ToList();

                if (entityPOCO.ARInvoiceTypeCode == "MN")
                {
                    double totalQuantity = 0;

                    #region LOOP Lines
                    var lines = (from a in invoiceLines
                                 group a by new
                                 {
                                     a.ChargesTypeId,
                                     a.Description,
                                     a.ForiegnCurrencyId,
                                     a.ForiegnExchangeRate,
                                     a.LocalDescription,
                                     a.MeasurementId,
                                     a.VatTypeId,
                                     a.Notes,
                                     a.IsExpense
                                 } into gr
                                 select new
                                 {
                                     ChargesTypeId = gr.Key.ChargesTypeId,
                                     Description = gr.Key.Description,
                                     ForiegnCurrencyId = gr.Key.ForiegnCurrencyId,
                                     ForiegnExchangeRate = gr.Key.ForiegnExchangeRate,
                                     LocalDescription = gr.Key.LocalDescription,
                                     MeasurementId = gr.Key.MeasurementId,
                                     VatTypeId = gr.Key.VatTypeId,
                                     Notes = gr.Key.Notes,
                                     IsExpense = gr.Key.IsExpense,
                                     LocalAmount = gr.Sum(d => (d.LocalCurrencyAmount != null ? d.LocalCurrencyAmount.Value : 0)),
                                     InvoiceAmount = gr.Sum(d => (d.InvoiceCurrencyAmount != null ? d.InvoiceCurrencyAmount.Value : 0)),
                                     ForeignAmount = gr.Sum(d => (d.ForiegnCurrencyAmount != null ? d.ForiegnCurrencyAmount.Value : 0)),
                                     Quantity = gr.Sum(d => (d.Quantity != null ? d.Quantity.Value : 0)),
                                 }).ToList();

                    foreach (var invoiceline in lines)
                    {
                        ReportInvoiceLine reportinvoiceline = new ReportInvoiceLine();

                        Currency foreigncurrency = allCurrencies.Where(d => d.Id == invoiceline.ForiegnCurrencyId).FirstOrDefault();

                        reportinvoiceline.ForeignCurrency = foreigncurrency != null ? foreigncurrency.Code : "";

                        reportinvoiceline.Notes = invoiceline.Notes != null ? invoiceline.Notes : "";
                        reportinvoiceline.Description = invoiceline.Description != null ? invoiceline.Description : "";
                        reportinvoiceline.LocalDescription = invoiceline.LocalDescription != null ? invoiceline.LocalDescription : "";
                        reportinvoiceline.DescriptionAndNotes = reportinvoiceline.Description + Environment.NewLine + reportinvoiceline.Notes;
                        reportinvoiceline.IsExpense = invoiceline.IsExpense;
                        reportinvoiceline.CalculatedUnitPrice = Math.Round(invoiceline.InvoiceAmount / invoiceline.Quantity, 2);

                        double? lineAmount_Foreign = invoiceline.ForeignAmount;
                        double? lineAmount_Invoice = invoiceline.InvoiceAmount;
                        double? lineAmount_Local = invoiceline.LocalAmount;

                        if (entityPOCO.ARInvoiceTypeCode == "CD")
                        {
                            lineAmount_Foreign = lineAmount_Foreign * -1;
                            lineAmount_Invoice = lineAmount_Invoice * -1;
                            lineAmount_Local = lineAmount_Local * -1;
                        }

                        reportinvoiceline.LocalAmount = String.Format("{0:#,0.00}", lineAmount_Local);
                        reportinvoiceline.InvoiceAmount = String.Format("{0:#,0.00}", lineAmount_Invoice);
                        reportinvoiceline.ForeignAmount = String.Format("{0:#,0.00}", lineAmount_Foreign);
                        reportinvoiceline.LocalAmount_Double = lineAmount_Local;
                        reportinvoiceline.InvoiceAmount_Double = lineAmount_Invoice;

                        #region Vats
                        reportinvoiceline.VatAmountIncludeMultiInInvoiceCurrency = this.GetVatAmountIncludeMultiInInvoiceCurrencyField(invoiceline.VatTypeId, invoiceline.InvoiceAmount, entityPOCO.ARInvoiceTypeCode, allVATTypes, allVatTypesPercentages, allVATTypesGroups);

                        VatType vattype = allVATTypes.Where(d => d.Id == invoiceline.VatTypeId).FirstOrDefault();
                        if (vattype != null)
                        {
                            reportinvoiceline.VatType = vattype.EnglishName != null ? vattype.EnglishName : "";
                            reportinvoiceline.VatTypeLocalName = vattype.LocalName != null ? vattype.LocalName : "";
                            reportinvoiceline.VATDescription = vattype.Description;
                            reportinvoiceline.VATLocalDescription = vattype.LocalDescription;

                            if (!vattype.IsMultiPercentage)
                            {
                                List<VatTypePercentage> vattypepercentageList = (from percentage in commonContext.VatTypePercentages where percentage.VatTypeId == vattype.Id orderby percentage.FromDate descending select percentage).ToList();

                                if (vattypepercentageList.Count > 0)
                                {
                                    reportinvoiceline.VatTypePercentage = vattypepercentageList[0].Percentage != null ? String.Format("{0:#,0.00}", vattypepercentageList[0].Percentage) : "";

                                    if (vattypepercentageList[0].Percentage != null && vattypepercentageList[0].Percentage > 0)
                                    {
                                        reportinvoiceline.VatIndication = "*";
                                        reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceAmount);
                                        reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceAmount;
                                    }
                                    else
                                    {
                                        reportinvoiceline.VatIndication = "";
                                        reportinvoiceline.NONVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceAmount);
                                        reportinvoiceline.NONVATableAmountInInvoiceCurrency_double = invoiceline.InvoiceAmount;
                                    }

                                    double vatamountinlocalcurrency = (invoiceline.LocalAmount * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;
                                    double vatamountininvoicecurrecy = (invoiceline.InvoiceAmount * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;
                                    double vatamountinForeigncurrecy = (invoiceline.ForeignAmount * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;

                                    if (entityPOCO.ARInvoiceTypeCode == "CD")
                                    {
                                        vatamountinlocalcurrency = vatamountinlocalcurrency * -1;
                                        vatamountininvoicecurrecy = vatamountininvoicecurrecy * -1;
                                        vatamountinForeigncurrecy = vatamountinForeigncurrecy * -1;
                                    }

                                    reportinvoiceline.VatAmountInLocalCurrency = String.Format("{0:#,0.00}", vatamountinlocalcurrency);
                                    reportinvoiceline.VatAmountInInvoiceCurrency = String.Format("{0:#,0.00}", vatamountininvoicecurrecy);
                                    reportinvoiceline.VatAmountInForeignCurrency = String.Format("{0:#,0.00}", vatamountinForeigncurrecy);

                                    reportinvoiceline.LocalAmountWithVAT = lineAmount_Local + vatamountinlocalcurrency;
                                }
                            }

                            else
                            {
                                reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceAmount);
                                reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceAmount;
                            }
                        }
                        #endregion

                        Measurement myMeasurement = allMeasurements.Where(d => d.Id == invoiceline.MeasurementId).FirstOrDefault();
                        if (myMeasurement != null)
                        {
                            reportinvoiceline.Measurement = myMeasurement.Code;

                            if (myMeasurement.Code == "PRVL" || myMeasurement.Code == "PRFR")
                            {
                                reportinvoiceline.UOMPercentage = "%";
                            }

                            if (satSetting != null && (satSetting.SATInterfaceCode == "PROF" || satSetting.SATInterfaceCode == "PROF33"))
                            {
                                ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(entityPOCO.Tenant);
                                reportinvoiceline.ClaveUnidad = computingPartnerHelper.GetComputingPartnerCodeTranslation(myMeasurement.Code, "G-Profact", "Measurement");
                            }
                        }

                        reportinvoiceline.Quantity = String.Format("{0:#,0.00}", invoiceline.Quantity);
                        totalQuantity += invoiceline.Quantity;

                        ChargesType chargetype = allChargesTypes.Where(d => d.Id == invoiceline.ChargesTypeId).FirstOrDefault();
                        if (chargetype != null)
                        {
                            reportinvoiceline.ChargeType = chargetype.EnglishName != null ? chargetype.EnglishName : "";
                            reportinvoiceline.ChargeTypeLocalName = chargetype.LocalName != null ? chargetype.LocalName : "";
                            reportinvoiceline.ChrageTypeCode = chargetype.Code != null ? chargetype.Code : "";
                            reportinvoiceline.ClaveProdServ = chargetype.SATExternalId;
                        }

                        if (foreigncurrency != null && invoicecurrency != null)
                        {
                            double? foreignExchangeRate = 0;

                            if (foreigncurrency == invoicecurrency)
                            {
                                foreignExchangeRate = 1;
                            }

                            else
                            {
                                foreignExchangeRate = invoiceline.ForiegnExchangeRate / entityPOCO.InvoiceCurrencyExchangeRate;
                            }

                            reportinvoiceline.ForeignToInvoiceExchangeRate = "1 " + foreigncurrency.Code + " = " + Math.Round(foreignExchangeRate.Value, 2) + " " + invoicecurrency.Code;
                        }

                        invoiceDataProvider.InvoiceLinesList.Add(reportinvoiceline);
                    }
                    #endregion

                    invoiceDataProvider.Quantity = String.Format("{0:#,0.00}", totalQuantity);
                }

                else
                {
                    double totalQuantity = 0;

                    #region LOOP Lines
                    foreach (ARInvoiceLine invoiceline in invoiceLines)
                    {
                        ReportInvoiceLine reportinvoiceline = new ReportInvoiceLine();

                        Currency foreigncurrency = allCurrencies.Where(d => d.Id == invoiceline.ForiegnCurrencyId).FirstOrDefault();

                        reportinvoiceline.ForeignCurrency = foreigncurrency != null ? foreigncurrency.Code : "";

                        reportinvoiceline.Notes = invoiceline.Notes != null ? invoiceline.Notes : "";
                        reportinvoiceline.Description = invoiceline.Description != null ? invoiceline.Description : "";
                        reportinvoiceline.LocalDescription = invoiceline.LocalDescription != null ? invoiceline.LocalDescription : "";
                        reportinvoiceline.DescriptionAndNotes = reportinvoiceline.Description + Environment.NewLine + reportinvoiceline.Notes;
                        reportinvoiceline.IsExpense = invoiceline.IsExpense;
                        reportinvoiceline.CalculatedUnitPrice = Math.Round((invoiceline.InvoiceCurrencyAmount / invoiceline.Quantity).Value, 2);

                        double? line_UnitPrice = invoiceline.UnitPrice;
                        double? lineAmount_Foreign = invoiceline.ForiegnCurrencyAmount;
                        double? lineAmount_Invoice = invoiceline.InvoiceCurrencyAmount;
                        double? lineAmount_Local = invoiceline.LocalCurrencyAmount;

                        if (entityPOCO.ARInvoiceTypeCode == "CD")
                        {
                            line_UnitPrice = line_UnitPrice * -1;
                            lineAmount_Foreign = lineAmount_Foreign * -1;
                            lineAmount_Invoice = lineAmount_Invoice * -1;
                            lineAmount_Local = lineAmount_Local * -1;
                        }

                        reportinvoiceline.UnitPrice = line_UnitPrice != null ? String.Format("{0:#,0.00}", line_UnitPrice) : "";
                        reportinvoiceline.UnitPriceDouble = line_UnitPrice == null ? 0 : line_UnitPrice.Value;
                        reportinvoiceline.LocalAmount = lineAmount_Local != null ? String.Format("{0:#,0.00}", lineAmount_Local.Value) : "";
                        reportinvoiceline.InvoiceAmount = lineAmount_Invoice != null ? String.Format("{0:#,0.00}", lineAmount_Invoice.Value) : "";
                        reportinvoiceline.ForeignAmount = lineAmount_Foreign != null ? String.Format("{0:#,0.00}", lineAmount_Foreign.Value) : "";
                        reportinvoiceline.LocalAmount_Double = lineAmount_Local;
                        reportinvoiceline.InvoiceAmount_Double = lineAmount_Invoice;

                        #region VAT

                        reportinvoiceline.VatAmountIncludeMultiInInvoiceCurrency = this.GetVatAmountIncludeMultiInInvoiceCurrencyField(invoiceline.VatTypeId, invoiceline.InvoiceCurrencyAmount, entityPOCO.ARInvoiceTypeCode, allVATTypes, allVatTypesPercentages, allVATTypesGroups);

                        VatType vattype = allVATTypes.Where(d => d.Id == invoiceline.VatTypeId).FirstOrDefault();
                        if (vattype != null)
                        {
                            reportinvoiceline.VatType = vattype.EnglishName != null ? vattype.EnglishName : "";
                            reportinvoiceline.VATDescription = vattype.Description;
                            reportinvoiceline.VATLocalDescription = vattype.LocalDescription;

                            if (!vattype.IsMultiPercentage)
                            {
                                List<VatTypePercentage> vattypepercentageList = (from percentage in commonContext.VatTypePercentages
                                                                                 where percentage.VatTypeId == vattype.Id
                                                                                 orderby percentage.FromDate descending
                                                                                 select percentage).ToList();

                                if (vattypepercentageList.Count > 0)
                                {
                                    reportinvoiceline.VatTypePercentage = vattypepercentageList[0].Percentage != null ? String.Format("{0:#,0.00}", vattypepercentageList[0].Percentage) : "";

                                    if (vattypepercentageList[0].Percentage != null && vattypepercentageList[0].Percentage > 0)
                                    {
                                        reportinvoiceline.VatIndication = "*";
                                        reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceCurrencyAmount);
                                        reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceCurrencyAmount;
                                    }
                                    else
                                    {
                                        reportinvoiceline.VatIndication = "";
                                        reportinvoiceline.NONVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceCurrencyAmount);
                                        reportinvoiceline.NONVATableAmountInInvoiceCurrency_double = invoiceline.InvoiceCurrencyAmount;
                                    }

                                    double vatamountinlocalcurrency = ((invoiceline.LocalCurrencyAmount != null ? invoiceline.LocalCurrencyAmount : 0) * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;
                                    double vatamountininvoicecurrecy = ((invoiceline.InvoiceCurrencyAmount != null ? invoiceline.InvoiceCurrencyAmount : 0) * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;
                                    double vatamountinForeigncurrecy = ((invoiceline.ForiegnCurrencyAmount != null ? invoiceline.ForiegnCurrencyAmount : 0) * (vattypepercentageList[0].Percentage != null ? (vattypepercentageList[0].Percentage / 100) : 0)).Value;

                                    if (entityPOCO.ARInvoiceTypeCode == "CD")
                                    {
                                        vatamountinlocalcurrency = vatamountinlocalcurrency * -1;
                                        vatamountininvoicecurrecy = vatamountininvoicecurrecy * -1;
                                        vatamountinForeigncurrecy = vatamountinForeigncurrecy * -1;
                                    }

                                    reportinvoiceline.VatAmountInLocalCurrency = String.Format("{0:#,0.00}", vatamountinlocalcurrency);
                                    reportinvoiceline.VatAmountInInvoiceCurrency = String.Format("{0:#,0.00}", vatamountininvoicecurrecy);
                                    reportinvoiceline.VatAmountInForeignCurrency = String.Format("{0:#,0.00}", vatamountinForeigncurrecy);

                                    reportinvoiceline.LocalAmountWithVAT = lineAmount_Local + vatamountinlocalcurrency;
                                }
                            }

                            else
                            {
                                reportinvoiceline.VATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceline.InvoiceCurrencyAmount);
                                reportinvoiceline.VATableAmountInInvoiceCurrency_double = invoiceline.InvoiceCurrencyAmount;
                            }
                        }
                        #endregion

                        Measurement myMeasurement = allMeasurements.Where(d => d.Id == invoiceline.MeasurementId).FirstOrDefault();
                        if (myMeasurement != null)
                        {
                            reportinvoiceline.Measurement = myMeasurement.Code;

                            if (myMeasurement.Code == "PRVL" || myMeasurement.Code == "PRFR")
                            {
                                reportinvoiceline.UOMPercentage = "%";
                            }

                            if (satSetting != null && (satSetting.SATInterfaceCode == "PROF" || satSetting.SATInterfaceCode == "PROF33"))
                            {
                                ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(entityPOCO.Tenant);
                                reportinvoiceline.ClaveUnidad = computingPartnerHelper.GetComputingPartnerCodeTranslation(myMeasurement.Code, "G-Profact", "Measurement");
                            }
                        }

                        reportinvoiceline.Quantity = invoiceline.Quantity != null ? String.Format("{0:#,0.00}", invoiceline.Quantity) : "";
                        totalQuantity += invoiceline.Quantity != null ? invoiceline.Quantity.Value : 0;

                        ChargesType chargetype = allChargesTypes.Where(d => d.Id == invoiceline.ChargesTypeId).FirstOrDefault();
                        if (chargetype != null)
                        {
                            reportinvoiceline.ChargeType = chargetype.EnglishName != null ? chargetype.EnglishName : "";
                            reportinvoiceline.ChargeTypeLocalName = chargetype.LocalName != null ? chargetype.LocalName : "";
                            reportinvoiceline.ChrageTypeCode = chargetype.Code != null ? chargetype.Code : "";
                            reportinvoiceline.ClaveProdServ = chargetype.SATExternalId;
                        }

                        if (foreigncurrency != null && invoicecurrency != null)
                        {
                            double? foreignExchangeRate = 0;

                            if (foreigncurrency == invoicecurrency)
                            {
                                foreignExchangeRate = 1;
                            }

                            else
                            {
                                foreignExchangeRate = invoiceline.ForiegnExchangeRate / entityPOCO.InvoiceCurrencyExchangeRate;
                            }

                            reportinvoiceline.ForeignToInvoiceExchangeRate = "1 " + foreigncurrency.Code + " = " + Math.Round(foreignExchangeRate.Value, 2) + " " + invoicecurrency.Code;
                        }

                        invoiceDataProvider.InvoiceLinesList.Add(reportinvoiceline);
                    }

                    #endregion

                    invoiceDataProvider.Quantity = String.Format("{0:#,0.00}", totalQuantity);
                }

                invoiceDataProvider.TotalVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceDataProvider.InvoiceLinesList.Sum(s => s.VATableAmountInInvoiceCurrency_double));
                invoiceDataProvider.TotalNONVATableAmountInInvoiceCurrency = String.Format("{0:#,0.00}", invoiceDataProvider.InvoiceLinesList.Sum(s => s.NONVATableAmountInInvoiceCurrency_double));

                invoiceDataProvider.ExpenseInvoiceLinesList = invoiceDataProvider.InvoiceLinesList.Where(d => d.IsExpense).ToList();
                invoiceDataProvider.NoExpenseInvoiceLinesList = invoiceDataProvider.InvoiceLinesList.Where(d => !d.IsExpense).ToList();
                #endregion

                #region Currency Codes
                Currency localCurrency = (from fc in commonContext.Currencies where fc.Id == entityPOCO.LocalCurrencyId select fc).FirstOrDefault();
                if (localCurrency != null)
                {
                    invoiceDataProvider.LocalCurrency = localCurrency.Code;
                    invoiceDataProvider.LocalCurrencySign = localCurrency.Sign;
                }

                Currency invoiceCurrency = (from fc in commonContext.Currencies where fc.Id == entityPOCO.InvoiceCurrencyId select fc).FirstOrDefault();
                if (invoiceCurrency != null)
                {
                    invoiceDataProvider.InvoiceCurrency = invoiceCurrency.Code;
                    invoiceDataProvider.InvoiceCurrencySign = invoiceCurrency.Sign;
                    invoiceDataProvider.InvoicecurrencyLocalName = invoiceCurrency.LocalName;

                }
                #endregion

                #region Exchange Rates
                StringBuilder localString = new StringBuilder();
                StringBuilder invoiceString = new StringBuilder();

                if (entityPOCO.LocalCurrencyId == entityPOCO.InvoiceCurrencyId && foriegnCurrencies.Count == 0)
                {
                    invoiceDataProvider.LocalCurrencyExchangeRates = "";
                    invoiceDataProvider.InvoiceCurrencyExchangeRates = "";
                }

                else if (entityPOCO.LocalCurrencyId == entityPOCO.InvoiceCurrencyId && foriegnCurrencies.Count > 0)
                {
                    string id;
                    Currency foriegnCurrency = null;

                    for (int i = 0; i < foriegnCurrencies.Count; i++)
                    {
                        id = foriegnCurrencies[i];
                        double? foriegnExchangeRate = invoiceLines.Where(d => d.ForiegnCurrencyId == id).FirstOrDefault().ForiegnExchangeRate;

                        if (id != entityPOCO.LocalCurrencyId && id != entityPOCO.InvoiceCurrencyId)
                        {
                            foriegnCurrency = commonContext.Currencies.Where(fc => fc.Id == id).FirstOrDefault();

                            localString.AppendLine("1 " + foriegnCurrency.Code + " = " + foriegnExchangeRate + " " + entityPOCO.LocalCurrency.Code);
                            invoiceString.AppendLine("1 " + foriegnCurrency.Code + " = " + (foriegnExchangeRate / entityPOCO.InvoiceCurrencyExchangeRate) + " " + entityPOCO.InvoiceCurrency.Code);
                        }
                    }

                    invoiceDataProvider.LocalCurrencyExchangeRates = localString.ToString();
                    invoiceDataProvider.InvoiceCurrencyExchangeRates = invoiceString.ToString();
                }

                else if (entityPOCO.LocalCurrencyId != entityPOCO.InvoiceCurrencyId && foriegnCurrencies.Count == 0)
                {
                    localString.AppendLine("1 " + entityPOCO.InvoiceCurrency.Code + " = " + entityPOCO.InvoiceCurrencyExchangeRate + " " + entityPOCO.LocalCurrency.Code);
                    invoiceString.AppendLine("1 " + entityPOCO.LocalCurrency.Code + " = " + (1 / entityPOCO.InvoiceCurrencyExchangeRate) + " " + entityPOCO.InvoiceCurrency.Code);

                    invoiceDataProvider.LocalCurrencyExchangeRates = localString.ToString();
                    invoiceDataProvider.InvoiceCurrencyExchangeRates = invoiceString.ToString();
                }

                else if (entityPOCO.LocalCurrencyId != entityPOCO.InvoiceCurrencyId && foriegnCurrencies.Count > 0)
                {
                    string id;
                    Currency foriegnCurrency = null;

                    for (int i = 0; i < foriegnCurrencies.Count; i++)
                    {
                        id = foriegnCurrencies[i];
                        foriegnCurrency = commonContext.Currencies.Where(fc => fc.Id == id).FirstOrDefault();
                        double? foriegnExchangeRate = invoiceLines.Where(d => d.ForiegnCurrencyId == id).FirstOrDefault().ForiegnExchangeRate;

                        if (entityPOCO.LocalCurrencyId != id && entityPOCO.InvoiceCurrencyId != id)
                        {
                            localString.AppendLine("1 " + foriegnCurrency.Code + " = " + foriegnExchangeRate + " " + entityPOCO.LocalCurrency.Code);
                            invoiceString.AppendLine("1 " + foriegnCurrency.Code + " = " + (foriegnExchangeRate / entityPOCO.InvoiceCurrencyExchangeRate) + " " + entityPOCO.InvoiceCurrency.Code);
                        }
                    }

                    localString.AppendLine("1 " + entityPOCO.InvoiceCurrency.Code + " = " + entityPOCO.InvoiceCurrencyExchangeRate + " " + entityPOCO.LocalCurrency.Code);
                    invoiceString.AppendLine("1 " + entityPOCO.LocalCurrency.Code + " = " + (1 / entityPOCO.InvoiceCurrencyExchangeRate) + " " + entityPOCO.InvoiceCurrency.Code);
                    invoiceDataProvider.LocalCurrencyExchangeRates = localString.ToString();
                    invoiceDataProvider.InvoiceCurrencyExchangeRates = invoiceString.ToString();
                }

                else
                {
                    invoiceDataProvider.LocalCurrencyExchangeRates = "";
                    invoiceDataProvider.InvoiceCurrencyExchangeRates = "";
                }
                #endregion

                #region Total Amounts
                double? invoiceSubTotals = entityPOCO.SubTotalInLocalCurrency;
                double? invoiceSubTotals_Local = entityPOCO.SubTotalInInvoiceCurrency;
                double? invoiceAmount = entityPOCO.AmountInInvoiceCurrency;
                double? invoiceAmountLocal = entityPOCO.AmountInLocalCurrency;
                double? profitAmount = entityPOCO.AmountInProfitCurrency;

                if (entityPOCO.ARInvoiceTypeCode == "CD")
                {
                    if (invoiceSubTotals < 0)
                    {
                        invoiceSubTotals = invoiceSubTotals * -1;
                    }

                    if (invoiceSubTotals_Local < 0)
                    {
                        invoiceSubTotals_Local = invoiceSubTotals_Local * -1;
                    }

                    if (invoiceAmount < 0)
                    {
                        invoiceAmount = invoiceAmount * -1;
                    }

                    if (invoiceAmountLocal < 0)
                    {
                        invoiceAmountLocal = invoiceAmountLocal * -1;
                    }

                    if (profitAmount < 0)
                    {
                        profitAmount = profitAmount * -1;
                    }
                }

                invoiceDataProvider.SubTotalLocalCurr = invoiceSubTotals != null ? String.Format("{0:#,0.00}", invoiceSubTotals.Value) : "";
                invoiceDataProvider.SubTotalInvoiceCurr = invoiceSubTotals_Local != null ? String.Format("{0:#,0.00}", invoiceSubTotals_Local.Value) : "";
                invoiceDataProvider.TotalInvoiceCurr = invoiceAmount != null ? invoiceAmount.Value : 0;
                invoiceDataProvider.TotalLocalCurr = invoiceAmountLocal != null ? String.Format("{0:#,0.00}", invoiceAmountLocal.Value) : "";
                invoiceDataProvider.TotalProfitCurr = profitAmount != null ? profitAmount.Value : 0;

                var result = invoiceDataProvider.TotalInvoiceCurr - Math.Truncate(invoiceDataProvider.TotalInvoiceCurr);
                var Firstdigits = (int)(Math.Round(result, 2) * 100);
                string str = "";
                string strWithZeros = "";
                if (Firstdigits < 10 && Firstdigits > 0)
                {
                    str = 0 + "" + Firstdigits + "/100";
                    strWithZeros = 0 + "" + Firstdigits + "/100";
                }
                else
                {
                    str = Firstdigits + "/100";
                    strWithZeros = Firstdigits + "/100";
                    if (Firstdigits == 0)
                        strWithZeros = 0 + "" + Firstdigits + "/100";
                }

                if ((int)(Math.Round(result, 2) * 100) <= 0)
                {
                    str = "";
                }
                var FrenchFractionsWords = "";
                var FrenchFractions = "";
                if (Firstdigits > 0)
                {
                    FrenchFractions = Firstdigits + " Cts";
                    FrenchFractionsWords = "et " + NumbersToFrench(Firstdigits) + " centimes";
                }


                invoiceDataProvider.AmountInWordsSpanish = FirstCharToUpper(NumbersToSpanish((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName + " " + str;
                invoiceDataProvider.AmountInWordsEnglish = FirstCharToUpper(NumbersToEnglish((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName + " " + str;
                invoiceDataProvider.AmountInWordsEnglishNoFR = FirstCharToUpper(NumbersToEnglish((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName;
                invoiceDataProvider.AmountInWordsFrench = FirstCharToUpper(NumbersToFrench((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName + " " + FrenchFractions;
                invoiceDataProvider.AmountInWordsFrenchNoFR = FirstCharToUpper(NumbersToFrench((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName;
                invoiceDataProvider.AmountInWordsFrenchWithFR = FirstCharToUpper(NumbersToFrench((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName + " " + FrenchFractionsWords;
                invoiceDataProvider.AmountInWordsSpanishWithZero = FirstCharToUpper(NumbersToSpanish((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName + " " + strWithZeros;
                invoiceDataProvider.AmountsInEnglishWithZero = FirstCharToUpper(NumbersToEnglish((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName + " " + strWithZeros;
                invoiceDataProvider.AmountInWordsRussian = FirstCharToUpper(NumbersToRussian((int)invoiceAmount.Value) + " ") + invoiceDataProvider.InvoicecurrencyLocalName + " " + strWithZeros;

                #endregion

                #region Total VAT
                List<ARInvoiceTotalVAT> totalVats = invoiceCotnext.ARInvoiceTotalVATs.Where(d => d.ARInvoiceId == invoiceId && d.Tenant == tenant).ToList();

                this.GenerateTotalVATs(invoiceDataProvider, totalVats, commonContext, entityPOCO.ARInvoiceTypeCode);
                this.GenerateTotalVATs_Expense(invoiceDataProvider, commonContext, invoiceLines, entityPOCO.ARInvoiceTypeCode, tenant);

                #endregion

                #region Constituents
                if (!entityPOCO.IsGeneralInvoice)
                {
                    invoiceDataProvider.ConstituentInvoicesList = new List<ReportConstituentInvoiceLine>();

                    List<ARInvoice> myInvoices = invoiceRepository.GetConnectedInvoices(tenant, entityPOCO.Id).ToList();

                    if (myInvoices.Count > 0)
                    {
                        List<string> invoicesIds = myInvoices.Select(s => s.Id).ToList();
                        List<string> shipmentsIds = myInvoices.Where(d => d.MainEntityId != null).Select(s => s.MainEntityId).ToList();

                        List<ARInvoiceTotalVAT> myTotalVATs = new List<ARInvoiceTotalVAT>();
                        if (invoicesIds.Count > 0)
                        {
                            ARInvoiceTotalVATRepository vatRepository = new ARInvoiceTotalVATRepository(tenant);
                            myTotalVATs = vatRepository.GetTotalVATsFromInvoiceIdList(invoicesIds, tenant);
                        }

                        List<ShipmentDataView> myShipments = new List<ShipmentDataView>();
                        List<ShipmentPackage> allShipmentsPackages = new List<ShipmentPackage>();

                        if (shipmentsIds.Count > 0)
                        {
                            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                            myShipments = shipmentRepository.GetShipmentsFromIdList(shipmentsIds, tenant);

                            ShipmentPackageRepository packageRepository = new ShipmentPackageRepository(tenant);
                            allShipmentsPackages = packageRepository.GetPackagesFromShipmentsIds(shipmentsIds, tenant).ToList();
                        }

                        foreach (ARInvoice item in myInvoices)
                        {
                            ReportConstituentInvoiceLine myRecord = new ReportConstituentInvoiceLine()
                            {
                                InvoiceId = item.Id,
                                InvoiceNumber = item.InvoiceNumber,
                                CustomerRef = item.CustomerRef,
                                InvoiceDate = item.InvoiceDate == null ? "" : String.Format("{0:dd.MMM.yyyy}", item.InvoiceDate),
                                BillToName = item.BillTo == null ? "" : item.BillTo.EnglishName,
                                InvoiceCurrencyCode = item.InvoiceCurrency == null ? "" : item.InvoiceCurrency.Code,
                                HouseNumber = item.HouseNumber,
                                MasterNumber = item.MasterNumber,
                                MainEntityReference = item.MainEntityReference,
                                SubTotalInInvoiceCurrency = item.SubTotalInInvoiceCurrency == null ? "" : String.Format("{0:N2}", item.SubTotalInInvoiceCurrency.Value),
                                SubTotalInLocalCurrency = item.SubTotalInLocalCurrency == null ? "" : String.Format("{0:N2}", item.SubTotalInLocalCurrency.Value),
                                AmountInInvoiceCurrency = item.AmountInInvoiceCurrency == null ? "" : String.Format("{0:N2}", item.AmountInInvoiceCurrency.Value),
                                AmountInLocalCurrency = item.AmountInLocalCurrency == null ? "" : String.Format("{0:N2}", item.AmountInLocalCurrency.Value),
                                AmountInProfitCurrency = item.AmountInProfitCurrency == null ? "" : String.Format("{0:N2}", item.AmountInProfitCurrency.Value),
                            };

                            double? myTotalVAT = myTotalVATs.Where(d => d.ARInvoiceId == item.Id).Sum(s => s.InvoiceCurrencyVATAmount);
                            double? myTotalVATLocal = myTotalVATs.Where(d => d.ARInvoiceId == item.Id).Sum(s => s.LocalVATAmount);
                            double? myVatableAmount = myTotalVATs.Where(d => d.ARInvoiceId == item.Id).Sum(s => s.InvoiceCurrencyVatableAmount);
                            double? myVatableAmountLocal = myTotalVATs.Where(d => d.ARInvoiceId == item.Id).Sum(s => s.LocalVatableAmount);

                            myRecord.TotalVAT = myTotalVAT == null ? "" : String.Format("{0:N2}", myTotalVAT);
                            myRecord.TotalVATLocal = myTotalVATLocal == null ? "" : String.Format("{0:N2}", myTotalVATLocal);
                            myRecord.VatableAmount = myVatableAmount == null ? "" : String.Format("{0:N2}", myVatableAmount);
                            myRecord.VatableAmountLocal = myVatableAmountLocal == null ? "" : String.Format("{0:N2}", myVatableAmountLocal);

                            if (!string.IsNullOrEmpty(item.MainEntityId))
                            {
                                ShipmentDataView myShipment = myShipments.Where(d => d.Id == item.MainEntityId).FirstOrDefault();
                                if (myShipment != null)
                                {
                                    myRecord.MainCarriageATD = myShipment.MainCarriageATD;
                                    myRecord.Shipper = myShipment.ShipperName;
                                    myRecord.Consignee = myShipment.ConsigneeName;
                                    myRecord.Carrier = myShipment.MainCarriageCarrierName;
                                    myRecord.CarrierNumber = myShipment.MainCarriageCarrierNumber;
                                    myRecord.DescriptionOfGoods = myShipment.DescriptionOfGoods;
                                    myRecord.Volume = myShipment.Volume == null ? "" : String.Format("{0:N2}", myShipment.Volume.Value);
                                    myRecord.GrossWeight = myShipment.GrossWeight == null ? "" : String.Format("{0:N2}", myShipment.GrossWeight.Value);
                                    myRecord.ChargeableWeight = myShipment.ChargeableWeight == null ? "" : String.Format("{0:N2}", myShipment.ChargeableWeight.Value);
                                    myRecord.PackagesQuantity = myShipment.PackagesQuantity == null ? "" : String.Format("{0:N0}", myShipment.PackagesQuantity.Value);
                                    myRecord.ShipmentNumber = myShipment.ShipmentNumber;
                                    myRecord.ShipmentRouting = myShipment.Routing;

                                    #region From:To Location
                                    if (myShipment.TransportModeId == "I" && myShipment.DirectionId == "D")
                                    {
                                        if (!string.IsNullOrEmpty(myShipment.MainCarriageFromAddressId))
                                        {
                                            Address myAddress = addressRepository.GetSingleAddress(myShipment.MainCarriageFromAddressId, tenant);
                                            if (myAddress != null)
                                            {
                                                myRecord.FromLocation = myAddress.City + " " + (myAddress.Country != null ? myAddress.Country.Code : "");
                                            }
                                        }

                                        if (!string.IsNullOrEmpty(myShipment.MainCarriageToAddressId))
                                        {
                                            Address myAddress = addressRepository.GetSingleAddress(myShipment.MainCarriageToAddressId, tenant);
                                            if (myAddress != null)
                                            {
                                                myRecord.ToLocation = myAddress.City + " " + (myAddress.Country != null ? myAddress.Country.Code : "");
                                                myRecord.FinalDestination = myAddress.City;
                                            }
                                        }
                                    }

                                    else
                                    {
                                        myRecord.FromLocation = myShipment.MainCarriageFromPortCode + " " + myShipment.MainCarriageFromPortName;
                                        myRecord.ToLocation = myShipment.MainCarriageToPortCode + " " + myShipment.MainCarriageToPortName;
                                        myRecord.FinalDestination = myShipment.MainCarriageFinalDestinationPortCode;
                                    }
                                    #endregion

                                    #region LoadingPlace

                                    ShipmentPickUpDelivery myFirstPickup = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                                            where d.ShipmentId == myShipment.Id && d.PickUpDeliveryTypeCode == "PICK"
                                                                            select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                                    myRecord.LoadingPlace = myServicHelper.GetPlaceOfLoading(myShipment, myFirstPickup);

                                    #endregion

                                    #region PlaceOfDelivery

                                    ShipmentPickUpDelivery myDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                                         where d.ShipmentId == myShipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                                         select d).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

                                    myRecord.PlaceOfDelivery = myServicHelper.GetPlaceOfDelivery(myShipment, myDelivery);
                                    myRecord.DeliveryFrom = myServicHelper.GetFromDeliveryName(myShipment, myDelivery);
                                    myRecord.DeliveryTo = myServicHelper.GetToDeliveryName(myShipment, myDelivery);
                                    myRecord.Incoterm = myShipment.IncotermCode;

                                    if (myDelivery != null)
                                    {
                                        ShipmentPickUpDeliveryPackageRepository rep = new ShipmentPickUpDeliveryPackageRepository(tenant);
                                        List<ShipmentPickUpDeliveryPackage> deliveryPackages = rep.GetPackagesByDeliveryId(myDelivery.Id, tenant).ToList();

                                        if (deliveryPackages != null)
                                        {
                                            myRecord.DeliveryPackagesQuantity = deliveryPackages.Sum(d => d.Quantity);
                                            myRecord.DeliveryPackagesWeight = deliveryPackages.Sum(d => d.Weight);
                                        }

                                        myRecord.DeliveryTrailerNo = myDelivery.TrailerNumber;
                                        myRecord.DeliveryDriverName = myDelivery.Driver;
                                    }

                                    #endregion

                                    #region PackagesType
                                    List<ShipmentPackage> myPackages = (from a in allShipmentsPackages where a.ShipmentId == myShipment.Id select a).ToList();
                                    if (myPackages.Count > 0)
                                    {
                                        var myGroup = (from a in myPackages
                                                       where a.PackageTypeId != null
                                                       group a by a.PackageTypeId into g
                                                       select new
                                                       {
                                                           PackageTypeId = g.Key,
                                                           Count = g.Count()
                                                       });

                                        string myPackagesTypesText = "";

                                        if (myGroup.Count() > 0)
                                        {
                                            foreach (var s in myGroup)
                                            {
                                                PackageType myPackageType = PackageTypeRepository.GetSinglePackageType(s.PackageTypeId, tenant, true);
                                                if (myPackageType != null)
                                                {
                                                    myPackagesTypesText = string.IsNullOrEmpty(myPackagesTypesText) ? myPackageType.EnglishName : myPackagesTypesText + "," + myPackageType.EnglishName;
                                                }
                                            }
                                        }

                                        myRecord.PackagesType = myPackagesTypesText;
                                    }
                                    #endregion
                                }
                            }

                            invoiceDataProvider.ConstituentInvoicesList.Add(myRecord);
                            invoiceDataProvider.ConstituentInvoices = string.IsNullOrEmpty(invoiceDataProvider.ConstituentInvoices) ? item.InvoiceNumber : (invoiceDataProvider.ConstituentInvoices + "," + item.InvoiceNumber);
                        }
                    }
                }
                #endregion

                #region invoiceDataProviderType
                Type invoiceDataProviderType = invoiceDataProvider.GetType();
                PropertyInfo[] properties = invoiceDataProviderType.GetProperties();
                foreach (PropertyInfo item in properties)
                {
                    try
                    {
                        if (item.Name != "InvoiceLinesList")
                        {
                            if (item.GetValue(invoiceDataProvider, null) == null || item.GetValue(invoiceDataProvider, null).ToString() == "0" || item.GetValue(invoiceDataProvider, null).ToString() == "00.00")
                            {
                                item.SetValue(invoiceDataProvider, "", null);
                            }
                        }
                    }

                    catch
                    {

                    }
                }
                #endregion

                #region USD-MXN exchange rate
                Currency USDCurrency = commonContext.Currencies.Where(d => d.Code == "USD" && d.Tenant == tenant).FirstOrDefault();
                Currency MXNCurrency = commonContext.Currencies.Where(d => d.Code == "MXN" && d.Tenant == tenant).FirstOrDefault();

                if (USDCurrency != null && MXNCurrency != null)
                {
                    LastRate rate = this.GetCurrencysExchangeRate(tenant, MXNCurrency, USDCurrency, TenantServerConfigration.GetCurrentDateTime(tenant).Date);
                    if (rate != null)
                    {
                        invoiceDataProvider.USD_MXN_ExchangeRate = rate.Rate;
                    }
                }

                #endregion

                #region InvoiceToAccountingExchangeRate

                Currency invoiceCurrency0 = commonContext.Currencies.Where(d => d.Id == entityPOCO.InvoiceCurrencyId && d.Tenant == tenant).FirstOrDefault();
                Currency tenantAccountingCurrency = commonContext.Currencies.Where(d => d.Id == myTenant.CurrencyId && d.Tenant == tenant).FirstOrDefault();

                if (invoiceCurrency0 != null && tenantAccountingCurrency != null && entityPOCO.ApprovedDate != null)
                {
                    if (invoiceCurrency0.Id == tenantAccountingCurrency.Id)
                    {
                        invoiceDataProvider.InvoiceToAccountingExchangeRate = 1;
                    }

                    else
                    {
                        invoiceDataProvider.InvoiceToAccountingExchangeRate = entityPOCO.InvoiceCurrencyExchangeRate;

                        //LastRate rate = this.GetCurrencysExchangeRate(tenant, invoiceCurrency0, tenantAccountingCurrency, entityPOCO.ApprovedDate.Value.Date);
                        //if (rate != null)
                        //{
                        //    invoiceDataProvider.InvoiceToAccountingExchangeRate = rate.Rate;
                        //}
                    }
                }

                #endregion

                #region SATInterface Properties

                if (!string.IsNullOrEmpty(entityPOCO.SATPaymentMethodCode))
                {
                    SATPaymentMethod arPaymentMethod = (from inty in invoiceCotnext.SATPaymentMethods where inty.Code == entityPOCO.SATPaymentMethodCode select inty).FirstOrDefault();
                    if (arPaymentMethod != null)
                    {
                        invoiceDataProvider.SAT.PaymentMethodCode = entityPOCO.SATPaymentMethodCode;
                        invoiceDataProvider.SAT.PaymentMethodName = arPaymentMethod.Name;
                        invoiceDataProvider.SAT.PaymentMethodLocalName = arPaymentMethod.LocalName;
                    }
                }


                if (satSetting != null && (satSetting.SATInterfaceCode == "PROF" || satSetting.SATInterfaceCode == "PROF33"))
                {
                    if (!string.IsNullOrEmpty(entityPOCO.SATXML))
                    {
                        if (satSetting.SATInterfaceCode == "PROF")
                        {
                            MapProfact32Fields(entityPOCO, invoiceDataProvider, tenantSettings);
                        }
                        else
                        {
                            MapProfact33Fields(entityPOCO, invoiceDataProvider, tenantSettings);
                        }
                    }
                    else
                    {
                        invoiceDataProvider.SAT.RegimenFiscal = "601";
                        if (entityPOCO.ARInvoiceTypeCode == "CD")
                            invoiceDataProvider.SAT.TipoDeComprobante = "E";
                        else
                            invoiceDataProvider.SAT.TipoDeComprobante = "I";

                        if (!string.IsNullOrEmpty(entityPM.MetodoPagoCode))
                        {
                            invoiceDataProvider.SAT.MetodoPago = (entityPM.MetodoPagoCode == "PUE" ? "PUE Pago en una sola exhibición" : "PPD Pago en parcialidades o diferido");
                        }
                        invoiceDataProvider.SAT.FormadePago = entityPOCO.SATPaymentMethodCode;

                        invoiceDataProvider.WaterMark = "Draft";
                        invoiceDataProvider.CopyName = "Draft";
                        invoiceDataProvider.CopyName_hebrew = "פרופורמה";
                        invoiceDataProvider.InvoiceNumber = entityPOCO.DraftNumber != null ? entityPOCO.DraftNumber : "";
                    }
                }

                #endregion

                #region Expense
                invoiceDataProvider.ExpenseSubTotalLocalCurr = invoiceDataProvider.ExpenseInvoiceLinesList.Sum(s => s.LocalAmount_Double);
                invoiceDataProvider.ExpenseSubTotalInvoiceCurr = invoiceDataProvider.ExpenseInvoiceLinesList.Sum(s => s.InvoiceAmount_Double);
              
                if (invoiceDataProvider.ExpenseTotalVatList != null)
                {
                    invoiceDataProvider.ExpenseTotalLocalCurr = invoiceDataProvider.ExpenseSubTotalLocalCurr + invoiceDataProvider.ExpenseTotalVatList.Sum(s => s.TotalVatAmountInLocalCurrency_Double);
                    invoiceDataProvider.ExpenseTotalInvoiceCurr = invoiceDataProvider.ExpenseSubTotalInvoiceCurr + invoiceDataProvider.ExpenseTotalVatList.Sum(s => s.TotalVatAmountInInvoiceCurrency_Double);

                    var resultOfExpenseTotal = decimal.Parse(invoiceDataProvider.ExpenseTotalInvoiceCurr + "") - Math.Truncate(decimal.Parse(invoiceDataProvider.ExpenseTotalInvoiceCurr + ""));
                    var resulyFirstdigits = (int)(Math.Round(resultOfExpenseTotal, 2) * 100);
                    string resultstr = "";
                    if (resulyFirstdigits < 10 && resulyFirstdigits > 0)
                        resultstr = 0 + "" + resulyFirstdigits + "/100";
                    else
                        resultstr = resulyFirstdigits + "/100";

                    if ((int)(Math.Round(resultOfExpenseTotal, 2) * 100) <= 0)
                    {
                        resultstr = "";
                    }

                    var FrenchFractionsExpense = "";
                    if (resulyFirstdigits > 0)
                    {
                        FrenchFractionsExpense = resulyFirstdigits + " Cts";
                    }



                    invoiceDataProvider.AmountInWordsExpenseTotalInvoiceCurrSpanish = NumbersToSpanish((int)invoiceDataProvider.ExpenseTotalInvoiceCurr) + " " + invoiceDataProvider.InvoicecurrencyLocalName + " " + resultstr;
                    invoiceDataProvider.AmountInWordsExpenseTotalInvoiceCurrFrench = NumbersToFrench((int)invoiceDataProvider.ExpenseTotalInvoiceCurr) + " " + invoiceDataProvider.InvoicecurrencyLocalName + " " + FrenchFractionsExpense;

                }
                #endregion

                #region NoExpense
                invoiceDataProvider.NoExpenseSubTotalLocalCurr = invoiceDataProvider.NoExpenseInvoiceLinesList.Sum(s => s.LocalAmount_Double);
                invoiceDataProvider.NoExpenseSubTotalInvoiceCurr = invoiceDataProvider.NoExpenseInvoiceLinesList.Sum(s => s.InvoiceAmount_Double);

                if (invoiceDataProvider.NoExpenseTotalVatList != null)
                {
                    invoiceDataProvider.NoExpenseTotalLocalCurr = invoiceDataProvider.NoExpenseSubTotalLocalCurr + invoiceDataProvider.NoExpenseTotalVatList.Sum(s => s.TotalVatAmountInLocalCurrency_Double);
                    invoiceDataProvider.NoExpenseTotalInvoiceCurr = invoiceDataProvider.NoExpenseSubTotalInvoiceCurr + invoiceDataProvider.NoExpenseTotalVatList.Sum(s => s.TotalVatAmountInInvoiceCurrency_Double);

                    var resultOfNoneExpenseTotal = decimal.Parse(invoiceDataProvider.NoExpenseTotalInvoiceCurr + "") - Math.Truncate(decimal.Parse(invoiceDataProvider.NoExpenseTotalInvoiceCurr + ""));
                    var resulyFirstdigits = (int)(Math.Round(resultOfNoneExpenseTotal, 2) * 100);
                    string resultstr = "";
                    if (resulyFirstdigits < 10 && resulyFirstdigits > 0)
                        resultstr = 0 + "" + resulyFirstdigits + "/100";
                    else
                        resultstr = resulyFirstdigits + "/100";

                    if ((int)(Math.Round(resultOfNoneExpenseTotal, 2) * 100) <= 0)
                    {
                        resultstr = "";
                    }

                    var FrenchFractionsNoneExpense = "";
                    if (resulyFirstdigits > 0)
                    {
                        FrenchFractionsNoneExpense = resulyFirstdigits + " Cts";
                    }

                    invoiceDataProvider.AmountInWordsNoExpenseTotalInvoiceCurrSpanish = NumbersToSpanish((int)invoiceDataProvider.NoExpenseTotalInvoiceCurr) + " " + invoiceDataProvider.InvoicecurrencyLocalName + " " + resultstr;
                    invoiceDataProvider.AmountInWordsNoExpenseTotalInvoiceCurrFrench = NumbersToFrench((int)invoiceDataProvider.NoExpenseTotalInvoiceCurr) + " " + invoiceDataProvider.InvoicecurrencyLocalName + " " + FrenchFractionsNoneExpense;
                }
                #endregion

                #region DepositBank
                if (entityPOCO.BankAccountLiteId != null)
                {
                    BankAccountLite myBankAccountLite = (from d in invoiceCotnext.BankAccountLites where d.Tenant == tenant && d.Id == entityPOCO.BankAccountLiteId select d).FirstOrDefault();
                    if(myBankAccountLite != null)
                    {
                        invoiceDataProvider.DepositBankEnglishName = myBankAccountLite.EnglishName;
                        invoiceDataProvider.DepositBankLocalName = myBankAccountLite.LocalName;
                        invoiceDataProvider.DepositBankSwiftCode = myBankAccountLite.SwiftCode;
                        invoiceDataProvider.DepositBankIBAN = myBankAccountLite.IBAN;
                    }
                }
                #endregion
            }

            return invoiceDataProvider;
        }

        private void GenerateTotalVATs(InvoiceDataProvider invoiceDataProvider, List<ARInvoiceTotalVAT> totalVats, ICommonDataContext commonContext, string invoiceTypeCode)
        {
            invoiceDataProvider.TotalVatList = new List<TotalVat>();
            invoiceDataProvider.TotalVatListNonZero = new List<TotalVat>();

            StringBuilder vatTypesDescriptionSt = new StringBuilder();
            StringBuilder vatTypesLocalDescriptionSt = new StringBuilder();
            double? TotalVats = 0;
            foreach (ARInvoiceTotalVAT item in totalVats)
            {
                TotalVats += item.InvoiceCurrencyVATAmount != null ? item.InvoiceCurrencyVATAmount : 0;
                VatType vatType = (from vat in commonContext.VatTypes where vat.Id == item.VatTypeId select vat).FirstOrDefault();

                if (vatType != null)
                {
                    #region

                    TotalVat newRecord = new TotalVat()
                    {
                        Id = vatType.Id,
                        Type = vatType.EnglishName == null ? "" : vatType.EnglishName,
                        TypeLocalName = vatType.LocalName == null ? "" : vatType.LocalName,
                        Description = vatType.Description == null ? "" : vatType.Description,
                        LocalDescription = vatType.LocalDescription == null ? "" : vatType.LocalDescription,
                    };

                    newRecord.Percentage = item.VatPercent != null ? String.Format("{0:#,0.00}", Math.Abs(item.VatPercent.Value)) : "";


                    if (item.VatPercent != 0)
                    {
                        invoiceDataProvider.TotalVatListNonZero.Add(newRecord);
                    }

                    if (!string.IsNullOrEmpty(newRecord.Description))
                    {
                        vatTypesDescriptionSt.AppendLine(newRecord.Description);
                    }

                    if (!string.IsNullOrEmpty(newRecord.LocalDescription))
                    {
                        vatTypesLocalDescriptionSt.AppendLine(newRecord.LocalDescription);
                    }

                    double? VatableAmount = item.InvoiceCurrencyVatableAmount;
                    double? VatableAmountLocal = item.LocalVatableAmount;
                    double? TotalVAT = item.InvoiceCurrencyVATAmount;
                    double? TotalVAT_Local = item.LocalVATAmount;

                    if (invoiceTypeCode == "CD")
                    {
                        VatableAmount = VatableAmount * -1;
                        VatableAmountLocal = VatableAmountLocal * -1;
                        TotalVAT = TotalVAT * -1;
                        TotalVAT_Local = TotalVAT_Local * -1;
                    }

                    newRecord.InvoiceCurrencyVatAmount = String.Format("{0:#,0.00}", VatableAmount);
                    newRecord.LocalVatAmout = String.Format("{0:#,0.00}", VatableAmountLocal);
                    newRecord.TotalVatAmountInInvoiceCurrency = TotalVAT != null ? String.Format("{0:#,0.00}", Math.Abs(TotalVAT.Value)) : "";
                    newRecord.TotalVatAmountInLocalCurrency = TotalVAT_Local != null ? String.Format("{0:#,0.00}", Math.Abs(TotalVAT_Local.Value)) : "";
                    newRecord.VATAmount = TotalVAT == null ? 0 : TotalVAT.Value;
                    newRecord.VatableAmount = VatableAmount == null ? 0 : VatableAmount.Value;

                    invoiceDataProvider.TotalVatList.Add(newRecord);
                    #endregion
                }
            }
            invoiceDataProvider.TotalVats = TotalVats;
            invoiceDataProvider.VatTypesDescription = vatTypesDescriptionSt.ToString();
            invoiceDataProvider.VatTypesLocalDescription = vatTypesLocalDescriptionSt.ToString();
            invoiceDataProvider.VATAmounts = this.BuildVATAmounts(invoiceDataProvider.TotalVatList);
            invoiceDataProvider.VatableAmounts = this.BuildVatableAmounts(invoiceDataProvider.TotalVatList);
            invoiceDataProvider.VATAmountsNonZero = this.BuildVATAmounts(invoiceDataProvider.TotalVatListNonZero);
            invoiceDataProvider.VatableAmountsNonZero = this.BuildVatableAmounts(invoiceDataProvider.TotalVatListNonZero);
        }

        private void GenerateTotalVATs_Expense(InvoiceDataProvider invoiceDataProvider, ICommonDataContext myCommonContext, List<ARInvoiceLine> lines, string invoiceTypeCode, int tenant)
        {
            invoiceDataProvider.ExpenseTotalVatList = new List<TotalVat>();
            invoiceDataProvider.NoExpenseTotalVatList = new List<TotalVat>();

            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);

            List<ARInvoiceLine> myDataLines = lines.Where(d => d.VatTypeId != null).ToList();
            List<VatType> allVatTypes = myCommonContext.VatTypes.Where(d => d.Tenant == tenant).ToList();
            List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, TenantServerConfigration.GetCurrentDateTime(tenant).Date);

            if (myDataLines.Count > 0)
            {
                List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups
                                                    where d.Tenant == tenant
                                                    select d).ToList();

                List<InvoiceTotalVATItem> group_Source = new List<InvoiceTotalVATItem>();

                foreach (ARInvoiceLine item in myDataLines)
                {
                    #region
                    VatType lineVatType = allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                    if (lineVatType != null)
                    {
                        if (!lineVatType.IsMultiPercentage)
                        {
                            InvoiceTotalVATItem newItem = new InvoiceTotalVATItem()
                            {
                                Id = item.VatTypeId,
                                VatTypeId = item.VatTypeId,
                                VatTypePercentage = item.VatPercentage,
                                LocalCurrencyAmount = item.LocalCurrencyAmount,
                                InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                ExternalVatCard = lineVatType.ExternalVATCard,
                                ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                                IsExpense = item.IsExpense,
                            };

                            group_Source.Add(newItem);
                        }

                        else
                        {
                            List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();
                            foreach (VATTypesGroup itemGroup in myVatGroups)
                            {
                                InvoiceTotalVATItem newItem = new InvoiceTotalVATItem()
                                {
                                    Id = itemGroup.SingleVATTypeId,
                                    VatTypeId = itemGroup.SingleVATTypeId,
                                    LocalCurrencyAmount = item.LocalCurrencyAmount,
                                    InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                    ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                    IsExpense = item.IsExpense,
                                };

                                VatType vatType = allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                if (vatType != null)
                                {
                                    newItem.ExternalVatCard = vatType.ExternalVATCard;
                                    newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                                }

                                VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                if (myPercentagePM != null)
                                {
                                    newItem.VatTypePercentage = myPercentagePM.Percentage;
                                }

                                group_Source.Add(newItem);
                            }
                        }
                    }
                    #endregion
                }

                List<InvoiceTotalVATItem> group_data_expense
                    = (from items in group_Source
                       where items.IsExpense
                       group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId } into g
                       select new InvoiceTotalVATItem()
                       {
                           Id = g.Key.VatTypeId,
                           VatTypeId = g.Key.VatTypeId,
                           VatTypePercentage = g.Key.VatTypePercentage,
                           LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                           InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                           ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                           ExternalVatCard = g.Key.ExternalVatCard,
                           ExternalTAXItemId = g.Key.ExternalTAXItemId,
                       }).ToList();

                List<InvoiceTotalVATItem> group_data_noExpense
                    = (from items in group_Source
                       where !items.IsExpense
                       group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId } into g
                       select new InvoiceTotalVATItem()
                       {
                           Id = g.Key.VatTypeId,
                           VatTypeId = g.Key.VatTypeId,
                           VatTypePercentage = g.Key.VatTypePercentage,
                           LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                           InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                           ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                           ExternalVatCard = g.Key.ExternalVatCard,
                           ExternalTAXItemId = g.Key.ExternalTAXItemId,
                       }).ToList();

                foreach (InvoiceTotalVATItem item in group_data_expense)
                {
                    VatType vatType = (from vat in myCommonContext.VatTypes where vat.Id == item.VatTypeId select vat).FirstOrDefault();

                    if (vatType != null)
                    {
                        TotalVat newRecord = new TotalVat()
                        {
                            Id = vatType.Id,
                            Type = vatType.EnglishName == null ? "" : vatType.EnglishName,
                            TypeLocalName = vatType.LocalName == null ? "" : vatType.LocalName,
                            Description = vatType.Description == null ? "" : vatType.Description,
                            LocalDescription = vatType.LocalDescription == null ? "" : vatType.LocalDescription,
                        };

                        newRecord.Percentage = item.VatTypePercentage != null ? String.Format("{0:#,0.00}", Math.Abs(item.VatTypePercentage.Value)) : "";

                        double? VatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2);
                        double? VatableAmountLocal = MethodHelper.Roundd(item.LocalCurrencyAmount, 2);
                        double? TotalVAT = MethodHelper.Round((VatableAmount * item.VatTypePercentage / 100), 2);
                        double? TotalVAT_Local = MethodHelper.Round((VatableAmountLocal * item.VatTypePercentage / 100), 2);

                        if (invoiceTypeCode == "CD")
                        {
                            VatableAmount = VatableAmount * -1;
                            VatableAmountLocal = VatableAmountLocal * -1;
                            TotalVAT = TotalVAT * -1;
                            TotalVAT_Local = TotalVAT_Local * -1;
                        }

                        newRecord.InvoiceCurrencyVatAmount = String.Format("{0:#,0.00}", VatableAmount);
                        newRecord.LocalVatAmout = String.Format("{0:#,0.00}", VatableAmountLocal);
                        newRecord.TotalVatAmountInInvoiceCurrency = TotalVAT != null ? String.Format("{0:#,0.00}", Math.Abs(TotalVAT.Value)) : "";
                        newRecord.TotalVatAmountInLocalCurrency = TotalVAT_Local != null ? String.Format("{0:#,0.00}", Math.Abs(TotalVAT_Local.Value)) : "";
                        newRecord.TotalVatAmountInInvoiceCurrency_Double = TotalVAT;
                        newRecord.TotalVatAmountInLocalCurrency_Double = TotalVAT_Local;
                        newRecord.VATAmount = TotalVAT == null ? 0 : TotalVAT.Value;
                        newRecord.VatableAmount = VatableAmount == null ? 0 : VatableAmount.Value;

                        invoiceDataProvider.ExpenseTotalVatList.Add(newRecord);
                    }
                }

                foreach (InvoiceTotalVATItem item in group_data_noExpense)
                {
                    VatType vatType = (from vat in myCommonContext.VatTypes where vat.Id == item.VatTypeId select vat).FirstOrDefault();

                    if (vatType != null)
                    {
                        TotalVat newRecord = new TotalVat()
                        {
                            Id = vatType.Id,
                            Type = vatType.EnglishName == null ? "" : vatType.EnglishName,
                            TypeLocalName = vatType.LocalName == null ? "" : vatType.LocalName,
                            Description = vatType.Description == null ? "" : vatType.Description,
                            LocalDescription = vatType.LocalDescription == null ? "" : vatType.LocalDescription,
                        };

                        newRecord.Percentage = item.VatTypePercentage != null ? String.Format("{0:#,0.00}", Math.Abs(item.VatTypePercentage.Value)) : "";

                        double? VatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2);
                        double? VatableAmountLocal = MethodHelper.Roundd(item.LocalCurrencyAmount, 2);
                        double? TotalVAT = MethodHelper.Round((VatableAmount * item.VatTypePercentage / 100), 2);
                        double? TotalVAT_Local = MethodHelper.Round((VatableAmountLocal * item.VatTypePercentage / 100), 2);

                        if (invoiceTypeCode == "CD")
                        {
                            VatableAmount = VatableAmount * -1;
                            VatableAmountLocal = VatableAmountLocal * -1;
                            TotalVAT = TotalVAT * -1;
                            TotalVAT_Local = TotalVAT_Local * -1;
                        }

                        newRecord.InvoiceCurrencyVatAmount = String.Format("{0:#,0.00}", VatableAmount);
                        newRecord.LocalVatAmout = String.Format("{0:#,0.00}", VatableAmountLocal);
                        newRecord.TotalVatAmountInInvoiceCurrency = TotalVAT != null ? String.Format("{0:#,0.00}", Math.Abs(TotalVAT.Value)) : "";
                        newRecord.TotalVatAmountInLocalCurrency = TotalVAT_Local != null ? String.Format("{0:#,0.00}", Math.Abs(TotalVAT_Local.Value)) : "";
                        newRecord.TotalVatAmountInInvoiceCurrency_Double = TotalVAT;
                        newRecord.TotalVatAmountInLocalCurrency_Double = TotalVAT_Local;
                        newRecord.VATAmount = TotalVAT == null ? 0 : TotalVAT.Value;
                        newRecord.VatableAmount = VatableAmount == null ? 0 : VatableAmount.Value;

                        invoiceDataProvider.NoExpenseTotalVatList.Add(newRecord);
                    }
                }
            }
        }

        private string BuildVATAmounts(List<TotalVat> list)
        {
            string myResult = "";

            if (list.Count > 0)
            {
                List<string> ids = (from a in list
                                    group a by a.Id into g
                                    select g.Key).ToList();

                foreach (string id in ids)
                {
                    string name = list.Where(d => d.Id == id).FirstOrDefault().Type;
                    double amount = list.Where(d => d.Id == id).Sum(s => s.VATAmount);
                    string myItem = "Subtotal - " + name + ": " + String.Format("{0:#,0.00}", amount);

                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = myItem;
                    }

                    else
                    {
                        myResult = myResult + Environment.NewLine + myItem;
                    }
                }
            }

            return myResult;
        }

        private string BuildVatableAmounts(List<TotalVat> list)
        {
            string myResult = "";

            if (list.Count > 0)
            {
                List<string> ids = (from a in list
                                    group a by a.Id into g
                                    select g.Key).ToList();

                foreach (string id in ids)
                {
                    string name = list.Where(d => d.Id == id).FirstOrDefault().Type;
                    double amount = list.Where(d => d.Id == id).Sum(s => s.VatableAmount);
                    string myItem = "Subtotal - " + name + ": " + String.Format("{0:#,0.00}", amount);

                    if (string.IsNullOrEmpty(myResult))
                    {
                        myResult = myItem;
                    }

                    else
                    {
                        myResult = myResult + Environment.NewLine + myItem;
                    }
                }
            }

            return myResult;
        }

        private LastRate GetCurrencysExchangeRate(int tenant, Currency localCurrency, Currency foreignCurrency, DateTime rateDate)
        {
            LastRate result = null;

            IWebFreightContext objectContext = WebFreightContext.GetContext(tenant);
            RatesTableRepository ratesTablesRepository = new RatesTableRepository(objectContext);
            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);

            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, foreignCurrency.Id, localCurrency.Id, rateDate);
            if (lastRate != null)
            {
                lastRate.BaseCurrencyId = localCurrency.Id;
                lastRate.BaseCurrencyCode = localCurrency.Code;
                result = lastRate;
            }

            return result;
        }

        private string NumbersToSpanish(double number)
        {
            string s = number.ToString();
            string a, c, b, result = "";
            int j, orlen;
            if (s == "0") { return ("cero"); }
            orlen = s.Length;
            if ((s.Length % 3) > 0)
                s = " " + s;
            if ((s.Length % 3) > 0)
                s = " " + s;
            for (var i = 0; i < s.Length; i = i + 3)
            {
                j = s.Length - i - 1;
                a = s.Substring(j, 1);
                b = s.Substring(j - 1, 1);
                c = s.Substring(j - 2, 1);
                if (a != " ")
                {
                    if ((i == 3) & (c + b + a != "000")) { result = "mil " + result; }
                    else if (((i == 6) & (c + b + a != "000")) & (orlen == 7) & (a == "1")) { result = "millón " + result; }
                    else if ((i == 6) & (c + b + a != "000")) { result = "millones " + result; }
                    else if ((i == 9) & (c + b + a != "000")) { result = "mil millones" + result; }
                    else if (((i == 12) & (c + b + a != "000")) & (orlen == 13) & (a == "1")) { result = "billón " + result; }
                    else if ((i == 12) & (c + b + a != "000")) { result = "billones " + result; }
                    else if ((i == 15) & (c + b + a != "000")) { result = "mil billones" + result; }
                    else if (((i == 18) & (c + b + a != "000")) & (orlen == 19) & (a == "1")) { result = "trillón " + result; }
                    else if ((i == 18) & (c + b + a != "000")) { result = "trillones " + result; }
                    else if ((i == 21) & (c + b + a != "000")) { result = "mil trillones " + result; }
                    else if (((i == 24) & (c + b + a != "000")) & (orlen == 25) & (a == "1")) { result = "quadrillón " + result; }
                    else if ((i == 24) & (c + b + a != "000")) { result = "quadrillones " + result; }
                    else if ((i == 27) & (c + b + a != "000")) { result = "mil quadrillones " + result; }
                    else if (((i == 30) & (c + b + a != "000")) & (orlen == 31) & (a == "1")) { result = "quintillón " + result; }
                    else if ((i == 30) & (c + b + a != "000")) { result = "quintillones " + result; }
                    else if ((i == 33) & (c + b + a != "000")) { result = "mil quintillones " + result; }
                    else if (((i == 36) & (c + b + a != "000")) & (orlen == 37) & (a == "1")) { result = "sextillón " + result; }
                    else if ((i == 36) & (c + b + a != "000")) { result = "sextillones " + result; }
                    else if ((i == 39) & (c + b + a != "000")) { result = "mil sextillones " + result; }
                    else if (((i == 42) & (c + b + a != "000")) & (orlen == 43) & (a == "1")) { result = "septillón " + result; }
                    else if ((i == 42) & (c + b + a != "000")) { result = "septillones " + result; }
                    else if ((i == 45) & (c + b + a != "000")) { result = "milseptillones " + result; }
                    else if (((i == 48) & (c + b + a != "000")) & (orlen == 49) & (a == "1")) { result = "octillón " + result; }
                    else if ((i == 48) & (c + b + a != "000")) { result = "octillones " + result; }
                    else if ((i == 51) & (c + b + a != "000")) { result = "mil octillones " + result; }
                    else if (((i == 54) & (c + b + a != "000")) & (orlen == 55) & (a == "1")) { result = "nonillón " + result; }
                    else if ((i == 57) & (c + b + a != "000")) { result = "nonillones " + result; }
                    else if (i == 60) { result = "mil nonillones " + result; }
                }
                if ((b != 1 + "") || (b == " "))
                {
                    if (a == 1 + "" && b != 2 + "") { result = "un " + result; }
                    else if (a == 2 + "" && b != 2 + "") { result = "dos " + result; }
                    else if (a == 3 + "" && b != 2 + "") { result = "tres " + result; }
                    else if (a == 4 + "" && b != 2 + "") { result = "cuatro " + result; }
                    else if (a == 5 + "" && b != 2 + "") { result = "cinco " + result; }
                    else if (a == 6 + "" && b != 2 + "") { result = "seis " + result; }
                    else if (a == 7 + "" && b != 2 + "") { result = "siete " + result; }
                    else if (a == 8 + "" && b != 2 + "") { result = "ocho " + result; }
                    else if (a == 9 + "" && b != 2 + "") { result = "nueve " + result; }
                }
                if ((b != " ") & (b != "0"))
                {
                    if ((b == 1 + "") | (b == 2 + ""))
                    {
                        if (b + a == 10 + "") { result = "diez " + result; }
                        else if (b + a == 11 + "") { result = "once " + result; }
                        else if (b + a == 12 + "") { result = "doce " + result; }
                        else if (b + a == 13 + "") { result = "trece " + result; }
                        else if (b + a == 14 + "") { result = "catorce " + result; }
                        else if (b + a == 15 + "") { result = "quince " + result; }
                        else if (b + a == 16 + "") { result = "dieciséis " + result; }
                        else if (b + a == 17 + "") { result = "diecisiete " + result; }
                        else if (b + a == 18 + "") { result = "dieciocho " + result; }
                        else if (b + a == 19 + "") { result = "diecinueve " + result; }
                        else if (b + a == 20 + "") { result = "veinte " + result; }
                        else if (b + a == 21 + "") { result = "veintiuno " + result; }
                        else if (b + a == 22 + "") { result = "veintidós " + result; }
                        else if (b + a == 23 + "") { result = "veintitrés " + result; }
                        else if (b + a == 24 + "") { result = "veinticuatro " + result; }
                        else if (b + a == 25 + "") { result = "veinticinco " + result; }
                        else if (b + a == 26 + "") { result = "veintiséis " + result; }
                        else if (b + a == 27 + "") { result = "veintisiete " + result; }
                        else if (b + a == 28 + "") { result = "veintiocho " + result; }
                        else if (b + a == 29 + "") { result = "veintinueve " + result; }
                    }
                    else
                    {
                        var temp = "";

                        if (a != 0 + "") { temp = "y "; }
                        if (b == 3 + "") { result = "treinta " + temp + result; }
                        else if (b == 4 + "") { result = "cuarenta " + temp + result; }
                        else if (b == 5 + "") { result = "cincuenta " + temp + result; }
                        else if (b == 6 + "") { result = "sesenta " + temp + result; }
                        else if (b == 7 + "") { result = "setenta " + temp + result; }
                        else if (b == 8 + "") { result = "ochenta " + temp + result; }
                        else if (b == 9 + "") { result = "noventa " + temp + result; }

                    }
                }
                if ((c != " ") & (c != "0"))
                {
                    if ((a == "0") & (b == "0"))
                    {
                        if (c == 1 + "") { result = "cien " + result; }
                        else if (c == 2 + "") { result = "doscientos " + result; }
                        else if (c == 3 + "") { result = "trescientos " + result; }
                        else if (c == 4 + "") { result = "cuatrocientos " + result; }
                        else if (c == 5 + "") { result = "quinientos " + result; }
                        else if (c == 6 + "") { result = "seiscientos " + result; }
                        else if (c == 7 + "") { result = "setecientos " + result; }
                        else if (c == 8 + "") { result = "ochocientos " + result; }
                        else if (c == 9 + "") { result = "novecientos " + result; }
                    }
                    else
                    {
                        if (c == 1 + "") { result = "ciento " + result; }
                        else if (c == 2 + "") { result = "doscientos " + result; }
                        else if (c == 3 + "") { result = "trescientos " + result; }
                        else if (c == 4 + "") { result = "cuatrocientos " + result; }
                        else if (c == 5 + "") { result = "quinientos " + result; }
                        else if (c == 6 + "") { result = "seiscientos " + result; }
                        else if (c == 7 + "") { result = "setecientos " + result; }
                        else if (c == 8 + "") { result = "ochocientos " + result; }
                        else if (c == 9 + "") { result = "novecientos " + result; }
                    }
                }
            }
            result = FixSpaces(result);
            result = Trim(result);
            if (result.Length >= 7)
                if (result.Substring(0, 7) == "un mil ") result = result.Substring(3, result.Length - 3);
            if (result.Length >= 3)
                if (result.Substring(result.Length - 3, 3) == " un") result = result + "o";
            if (result == "un ") result = "uno";
            if (result.Substring(result.Length - 2, 2) == "y ")
                result = result.Substring(0, result.Length - 2);
            if (InStr(result, "millones") != RInStr(result, "millones"))
            {
                var z = InStr(result, "millones");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));

            }
            if (InStr(result, "billones") != RInStr(result, "billones"))
            {
                var z = InStr(result, "billones");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));
            }
            if (InStr(result, "trillones") != RInStr(result, "trillones"))
            {
                var z = InStr(result, "trillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 8, result.Length - (z + 8));
            }
            if (InStr(result, "quadrillones") != RInStr(result, "quadrillones"))
            {
                var z = InStr(result, "quadrillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "quintillones") != RInStr(result, "quintillones"))
            {
                var z = InStr(result, "quintillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "sextillones") != RInStr(result, "sextillones"))
            {
                var z = InStr(result, "sextillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "septillones") != RInStr(result, "septillones"))
            {
                var z = InStr(result, "septillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "octillones") != RInStr(result, "octillones"))
            {
                var z = InStr(result, "octillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            if (InStr(result, "nonillones") != RInStr(result, "nonillones"))
            {
                var z = InStr(result, "nonillones");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            result = FixSpaces(result);
            return (result);

        }
        private string NumbersToFrench(double number)
        {
            string s = number.ToString();
            string a, c, b, result = "";
            int j, orlen;
            if (s == "0") { return ("z�ro"); }
            orlen = s.Length;
            if ((s.Length % 3) > 0)
                s = " " + s;
            if ((s.Length % 3) > 0)
                s = " " + s;
            for (var i = 0; i < s.Length; i = i + 3)
            {
                j = s.Length - i - 1;
                a = s.Substring(j, 1);
                b = s.Substring(j - 1, 1);
                c = s.Substring(j - 2, 1);
                if (a != " ")
                {
                    if ((i == 3) & (c + b + a != "000")) { result = "mille " + result; }
                    else if (((i == 6) & (c + b + a != "000")) & (orlen == 7) & (a == "1")) { result = "millón " + result; }
                    else if ((i == 6) & (c + b + a != "000")) { result = "millions " + result; }
                    else if ((i == 9) & (c + b + a != "000")) { result = "mille millions" + result; }
                    else if (((i == 12) & (c + b + a != "000")) & (orlen == 13) & (a == "1")) { result = "billion " + result; }
                    else if ((i == 12) & (c + b + a != "000")) { result = "billions " + result; }
                    else if ((i == 15) & (c + b + a != "000")) { result = "mille billions" + result; }
                    else if (((i == 18) & (c + b + a != "000")) & (orlen == 19) & (a == "1")) { result = "trillion " + result; }
                    else if ((i == 18) & (c + b + a != "000")) { result = "trillions " + result; }
                    else if ((i == 21) & (c + b + a != "000")) { result = "mille trillions " + result; }
                    else if (((i == 24) & (c + b + a != "000")) & (orlen == 25) & (a == "1")) { result = "quadrillion " + result; }
                    else if ((i == 24) & (c + b + a != "000")) { result = "quadrillions " + result; }
                    else if ((i == 27) & (c + b + a != "000")) { result = "mille quadrillions " + result; }
                    else if (((i == 30) & (c + b + a != "000")) & (orlen == 31) & (a == "1")) { result = "quintillion " + result; }
                    else if ((i == 30) & (c + b + a != "000")) { result = "quintillions " + result; }
                    else if ((i == 33) & (c + b + a != "000")) { result = "mille quintillions " + result; }
                    else if (((i == 36) & (c + b + a != "000")) & (orlen == 37) & (a == "1")) { result = "sextillion " + result; }
                    else if ((i == 36) & (c + b + a != "000")) { result = "sextillions " + result; }
                    else if ((i == 39) & (c + b + a != "000")) { result = "mille sextillions " + result; }
                    else if (((i == 42) & (c + b + a != "000")) & (orlen == 43) & (a == "1")) { result = "septillion " + result; }
                    else if ((i == 42) & (c + b + a != "000")) { result = "septillions " + result; }
                    else if ((i == 45) & (c + b + a != "000")) { result = "milseptillions " + result; }
                    else if (((i == 48) & (c + b + a != "000")) & (orlen == 49) & (a == "1")) { result = "octillion " + result; }
                    else if ((i == 48) & (c + b + a != "000")) { result = "octillions " + result; }
                    else if ((i == 51) & (c + b + a != "000")) { result = "mille octillions " + result; }
                    else if (((i == 54) & (c + b + a != "000")) & (orlen == 55) & (a == "1")) { result = "nonillion " + result; }
                    else if ((i == 57) & (c + b + a != "000")) { result = "nonillions " + result; }
                    else if (i == 60) { result = "mille nonillions " + result; }
                }
                if (((b != 1 + "") & (b != 7 + "")) | (b == " "))
                {
                    if (a == 1 + "") { result = "un " + result; }
                    else if (a == 2 + "") { result = "deux " + result; }
                    else if (a == 3 + "") { result = "trois " + result; }
                    else if (a == 4 + "") { result = "quatre " + result; }
                    else if (a == 5 + "") { result = "cinq " + result; }
                    else if (a == 6 + "") { result = "six " + result; }
                    else if (a == 7 + "") { result = "sept " + result; }
                    else if (a == 8 + "") { result = "huit " + result; }
                    else if (a == 9 + "") { result = "neuf " + result; }
                }
                if ((b != " ") & (b != "0"))
                {
                    if ((b == 1 + "") | (b == 7 + ""))
                    {
                        if (b + a == 10 + "") { result = "dix " + result; }
                        else if (b + a == 11 + "") { result = "onze " + result; }
                        else if (b + a == 12 + "") { result = "douze " + result; }
                        else if (b + a == 13 + "") { result = "treize " + result; }
                        else if (b + a == 14 + "") { result = "quatorze " + result; }
                        else if (b + a == 15 + "") { result = "quinze " + result; }
                        else if (b + a == 16 + "") { result = "seize " + result; }
                        else if (b + a == 17 + "") { result = "dix-sept " + result; }
                        else if (b + a == 18 + "") { result = "dix-huit " + result; }
                        else if (b + a == 19 + "") { result = "dix-neuf " + result; }
                        else if (b + a == 70 + "") { result = "soixante-dix " + result; }
                        else if (b + a == 71 + "") { result = "soixante-onze " + result; }
                        else if (b + a == 72 + "") { result = "soixante-douze " + result; }
                        else if (b + a == 73 + "") { result = "soixante-treize " + result; }
                        else if (b + a == 74 + "") { result = "soixante-quatorze " + result; }
                        else if (b + a == 75 + "") { result = "soixante-quinze " + result; }
                        else if (b + a == 76 + "") { result = "soixante-seize " + result; }
                        else if (b + a == 77 + "") { result = "soixante-dix-sept " + result; }
                        else if (b + a == 78 + "") { result = "soixante-dix-huit " + result; }
                        else if (b + a == 79 + "") { result = "soixante-dix-neuf " + result; }
                    }
                    else
                    {
                        var temp = "";

                        if (a == 1 + "") { temp = "et "; }
                        if (int.Parse(a) > 1) { temp = "-"; }
                        if (b == 2 + "") { result = "vingt " + temp + result; }
                        else if (b == 3 + "") { result = "trente " + temp + result; }
                        else if (b == 4 + "") { result = "quarante " + temp + result; }
                        else if (b == 5 + "") { result = "cinquante " + temp + result; }
                        else if (b == 6 + "") { result = "soixante " + temp + result; }
                        else if (b == 7 + "") { result = "soixante-dix " + temp + result; }
                        else if (b == 8 + "") { result = "quatre-vingts " + temp + result; }
                        else if (b == 9 + "") { result = "quatre-vingt-dix " + temp + result; }
                    }
                }
                if ((c != " ") & (c != "0"))
                {
                    if (c == 1 + "") { result = "cent " + result; }
                    else if (c == 2 + "") { result = "deux cents " + result; }
                    else if (c == 3 + "") { result = "trois cents " + result; }
                    else if (c == 4 + "") { result = "quatre cents " + result; }
                    else if (c == 5 + "") { result = "cinq cents " + result; }
                    else if (c == 6 + "") { result = "six cents " + result; }
                    else if (c == 7 + "") { result = "sept cents " + result; }
                    else if (c == 8 + "") { result = "huit cents " + result; }
                    else if (c == 9 + "") { result = "neuf cents " + result; }
                }
            }
            result = FixSpaces(result);
            result = Trim(result);
            if (result.Length >= 9)
                if (result.Substring(0, 9) == "un mille ")
                    result = result.Substring(3, result.Length - 3);
            if (result.Length >= 3)
                if (result.Substring(result.Length - 3, 3) == "et ")
                    result = result.Substring(0, result.Length - 3);
            if (result.Substring(result.Length - 2, 2) == " -")
                result = result.Substring(0, result.Length - 2);
            if (InStr(result, "millions") != RInStr(result, "millions"))
            {
                var z = InStr(result, "millions");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));
            }
            if (InStr(result, "billions") != RInStr(result, "billions"))
            {
                var z = InStr(result, "billions");

                result = result.Substring(0, z - 1) + result.Substring(z + 7, result.Length - (z + 7));
            }
            if (InStr(result, "trillions") != RInStr(result, "trillions"))
            {
                var z = InStr(result, "trillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 8, result.Length - (z + 8));
            }
            if (InStr(result, "quadrillions") != RInStr(result, "quadrillions"))
            {
                var z = InStr(result, "quadrillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "quintillions") != RInStr(result, "quintillions"))
            {
                var z = InStr(result, "quintillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 11, result.Length - (z + 11));
            }
            if (InStr(result, "sextillions") != RInStr(result, "sextillions"))
            {
                var z = InStr(result, "sextillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "septillions") != RInStr(result, "septillions"))
            {
                var z = InStr(result, "septillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 10, result.Length - (z + 10));
            }
            if (InStr(result, "octillions") != RInStr(result, "octillions"))
            {
                var z = InStr(result, "octillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            if (InStr(result, "nonillions") != RInStr(result, "nonillions"))
            {
                var z = InStr(result, "nonillions");

                result = result.Substring(0, z - 1) + result.Substring(z + 9, result.Length - (z + 9));
            }
            result = FixSpaces(result);
            while (InStr(result, " -") > 0)
            {
                var z = InStr(result, " -");
                result = result.Substring(0, z - 1) + result.Substring(z, result.Length - z);
            }
            return (result);

        }


        private string NumbersToRussian(double numberNumeric)
        {
            string ZERO_NAME = "нуль";
            string ONE_THOUSANT_NAME = "одна";
            string[] HUNDRED_NAMES = { "", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот" };
            string[] TEN_NAMES = { "", "", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто" };
            string[] UNIT_NAMES = { ZERO_NAME, "один", "два", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять" };
            List<string[]> TRIPLET_NAMES = new List<string[]>();
            string[] Undifined = { null };
            string[] Triplet1 = { "тысяча", "тысячи", "тысяч" };
            string[] Triplet2 = { "миллион", "миллиона", "миллионов" };
            string[] Triplet3 = { "миллиард", "миллиарда", "миллиардов" };
            string[] Triplet4 = { "триллион", "триллиона", "триллионов" };
            string[] Triplet5 = { "квадрилион", "квадрилиона", "квадрилионов" };
            TRIPLET_NAMES.Add(Undifined);
            TRIPLET_NAMES.Add(Triplet1);
            TRIPLET_NAMES.Add(Triplet1);
            TRIPLET_NAMES.Add(Triplet2);
            TRIPLET_NAMES.Add(Triplet3);
            TRIPLET_NAMES.Add(Triplet4);
            TRIPLET_NAMES.Add(Triplet5);

            string[] TEN_UNIT_NAMES = { "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать" };


            List<string> numberInWords = new List<string>();
            if(numberNumeric<=-1)
            {
                numberNumeric *= -1;
                numberInWords.Add("минус");
            }

            string number = numberNumeric + "";


            int length = number.Length;

            for (int i = 0; i < length; i += 1)
            {
                int pos = length - 1 - i;
                decimal d = pos / 3;
                decimal tripletIndex = Math.Floor(d);
                int digitPosition = pos % 3;
                int digitValue = int.Parse(number[i] + "");

                if (digitPosition == 2)
                {
                    numberInWords.Add(HUNDRED_NAMES[digitValue]);
                    continue;
                }
                if (digitPosition == 1)
                {
                    if (digitValue == 1)
                    {
                        numberInWords.Add(TEN_UNIT_NAMES[int.Parse(number[i + 1] + "")]);
                    }
                    else
                    {
                        numberInWords.Add(TEN_NAMES[digitValue]);
                    }
                    continue;
                }
                if (digitPosition == 0)
                {
                    int prevDigitValue = -99999999;
                    if (i - 1 >= 0)
                    {
                        prevDigitValue = int.Parse(number[i - 1] + "");

                    }

                    if (digitValue == 0)
                    {
                        if (length == 1)
                        {
                            numberInWords.Add(ZERO_NAME);
                        }
                    }
                    else if (prevDigitValue != 1 || prevDigitValue == -99999999)
                    {
                        numberInWords.Add(tripletIndex == 1 && digitValue == 1 ? ONE_THOUSANT_NAME : UNIT_NAMES[digitValue]);
                    }

                    string[] tripletNames = TRIPLET_NAMES[(int)tripletIndex];
                    if (tripletNames.Length > 1)
                    {

                        if (prevDigitValue == 1)
                        {
                            numberInWords.Add(pluralEnding(10 + digitValue, tripletNames));
                        }
                        else
                        {
                            numberInWords.Add(pluralEnding(digitValue, tripletNames));
                        }
                    }
                    continue;
                }
            }

            return String.Join(" ", numberInWords);




        }


        private string pluralEnding(int number, string[] variants)
        {
            var one = variants[0];
            string two = variants[1];
            string five = variants[2];
            number = Math.Abs(number);
            number %= 100;
            if (number >= 5 && number <= 20)
            {
                return five;
            }
            number %= 10;
            if (number == 1)
            {
                return one;
            }
            if (number >= 2 && number <= 4)
            {
                return two;
            }
            return five;

        }


        private string NumbersToEnglish(double number)
        {
            string s = number.ToString();
            string a, c, b, result = "";
            int j, orlen;
            if (s == "0") { return ("zero"); }
            orlen = s.Length;
            if ((s.Length % 3) > 0)
                s = ' ' + s;
            if ((s.Length % 3) > 0)
                s = ' ' + s;
            for (var i = 0; i < s.Length; i = i + 3)
            {
                j = s.Length - i - 1;
                a = s.Substring(j, 1);
                b = s.Substring(j - 1, 1);
                c = s.Substring(j - 2, 1);
                if (a != " ")
                {
                    if ((i == 3) & (c + b + a != "000")) { result = "thousand " + result; }
                    else if ((i == 6) & (c + b + a != "000")) { result = "million " + result; }
                    else if ((i == 9) & (c + b + a != "000")) { result = "billion " + result; }
                    else if ((i == 12) & (c + b + a != "000")) { result = "trillion " + result; }
                    else if ((i == 15) & (c + b + a != "000")) { result = "quadrillion " + result; }
                    else if ((i == 18) & (c + b + a != "000")) { result = "quintillion " + result; }
                    else if ((i == 21) & (c + b + a != "000")) { result = "sextillion " + result; }
                    else if ((i == 24) & (c + b + a != "000")) { result = "septillion " + result; }
                    else if ((i == 27) & (c + b + a != "000")) { result = "octillion " + result; }
                    else if ((i == 30) & (c + b + a != "000")) { result = "nonillion " + result; }
                    else if ((i == 33) & (c + b + a != "000")) { result = "decillion " + result; }
                    else if ((i == 36) & (c + b + a != "000")) { result = "undecillion " + result; }
                    else if ((i == 39) & (c + b + a != "000")) { result = "duodecillion " + result; }
                    else if ((i == 42) & (c + b + a != "000")) { result = "tredecillion " + result; }
                    else if ((i == 45) & (c + b + a != "000")) { result = "quattuordecillion " + result; }
                    else if ((i == 48) & (c + b + a != "000")) { result = "quindecillion " + result; }
                    else if ((i == 51) & (c + b + a != "000")) { result = "sexdecillion " + result; }
                    else if ((i == 54) & (c + b + a != "000")) { result = "septendecillion " + result; }
                    else if ((i == 57) & (c + b + a != "000")) { result = "octodecillion " + result; }
                    else if (i == 60) { result = "novemdecillion " + result; }
                }
                if ((b != "1") | (b == " "))
                {
                    if (a == 1+"") { result = "one " + result; }
                    else if (a == 2 + "") { result = "two " + result; }
                    else if (a == 3 + "") { result = "three " + result; }
                    else if (a == 4 + "") { result = "four " + result; }
                    else if (a == 5 + "") { result = "five " + result; }
                    else if (a == 6 + "") { result = "six " + result; }
                    else if (a == 7 + "") { result = "seven " + result; }
                    else if (a == 8 + "") { result = "eight " + result; }
                    else if (a == 9 + "") { result = "nine " + result; }
                }
                if ((b != " ") & (b != "0"))
                {
                    if (b == "1")
                    {
                        if (a == 0 + "") { result = "ten " + result; }
                        else if (a == 1 + "") { result = "eleven " + result; }
                        else if (a == 2 + "") { result = "twelve " + result; }
                        else if (a == 3 + "") { result = "thirteen " + result; }
                        else if (a == 4 + "") { result = "fourteen " + result; }
                        else if (a == 5 + "") { result = "fifteen " + result; }
                        else if (a == 6 + "") { result = "sixteen " + result; }
                        else if (a == 7 + "") { result = "seventeen " + result; }
                        else if (a == 8 + "") { result = "eighteen " + result; }
                        else if (a == 9 + "") { result = "nineteen " + result; }
                    }
                    else
                    {
                        if (b == 2 + "") { result = "twenty " + result; }
                        else if (b == 3 + "") { result = "thirty " + result; }
                        else if (b == 4 + "") { result = "fourty " + result; }
                        else if (b == 5 + "") { result = "fifty " + result; }
                        else if (b == 6 + "") { result = "sixty " + result; }
                        else if (b == 7 + "") { result = "seventy " + result; }
                        else if (b == 8 + "") { result = "eighty " + result; }
                        else if (b == 9 + "") { result = "ninety " + result; }
                    }
                }
                if ((c != " ") & (c != "0"))
                {
                    if (c == 1 + "") { result = "one hundred " + result; }
                    else if (c == 2 + "") { result = "two hundred " + result; }
                    else if (c == 3 + "") { result = "three hundred " + result; }
                    else if (c == 4 + "") { result = "four hundred " + result; }
                    else if (c == 5 + "") { result = "five hundred " + result; }
                    else if (c == 6 + "") { result = "six hundred " + result; }
                    else if (c == 7 + "") { result = "seven hundred " + result; }
                    else if (c == 8 + "") { result = "eight hundred " + result; }
                    else if (c == 9 + "") { result = "nine hundred " + result; }
                }
            }
            result = Trim(result);
            result = FixSpaces(result);
            return (result);


        }


        public string NumbersToHebrew(double number)
        {
            string rv = NumbersToHebrew(number, "", "", false);
            return rv;
        }


        public static IEnumerable<String> SliceRow<String>(String[,] array, int row)
        {
            for (var i = 0; i < array.GetLength(1); i++)
            {
                yield return array[row, i];
            }
        }

        public string NumbersToHebrew(double amount, string curr_name, String subunit_name, Boolean cents_in_words)
        {
            String shkalim = "";
            String agorot = "";
            String shah = "";
            String pattern = "";
            String left = "";
            String right = "";
            String ag = "";

            int pos = 0;
            int digit = 0;
            int len = 0;
            int n = 0;
            int prec = 0;


            String add = "";

            String[,] tr;
            String number = "";
            String text = "";
            String ve = "";
            String asar = "";
            String meot = "";
            String elef = "";
            String mili = "";
            String cents = "";


            Boolean eng = false;
            String str1;


            prec = 2;// sample: 2.1 => 2.10


            shah = curr_name;

          //  eng = !((shah[0] >= 'א' && shah[0] <= 'ת') | shah[0] == '₪');

            ag = subunit_name;
            number = amount.ToString();

            if (!number.ToLower().Contains('.'))
            {
                shkalim = number;
                agorot = "";
            }
            else
            {
                string[] words = number.Split('.');
                shkalim = words[0];
                agorot = words[1];
                if (agorot.Length == 1) // e.g. 60 --> 6
                {
                    agorot = agorot + "0";
                }
                agorot = agorot.Substring(0, prec);
                agorot = agorot.PadRight(prec, '0');
            }

            tr = new String[,]
            {
                {"אחד", "אלף", "מאה", "עשר", "עשרת אלפים"},//1
                {"שנים", "אלפיים", "מאתיים", "עשרים", "שני"},
                {"שלושה", "שלושת אלפים", "שלוש", "שלושים", ""},//3
                {"ארבעה", "ארבעת אלפים", "ארבע", "ארבעים", ""},
                {"חמישה", "חמשת אלפים", "חמש", "חמישים", ""},//5
                {"שישה", "ששת אלפים", "שש", "שישים", ""},
                {"שבעה", "שבעת אלפים", "שבע", "שבעים", ""},//7
                {"שמונה", "שמונת אלפים", "שמונה", "שמונים", ""},
                {"תשעה", "תשעת אלפים", "תשע", "תשעים", ""},//9
            };

            ve = " ו";
            asar = " עשר";
            meot = " מאות";
            elef = " אלף";
            mili = " מיליון";

            len = shkalim.Length;
            pos = len;

            while (pos > 0)
            {

                n = len - pos + 1;//counting from leftmost digit to the right; starting with 1 

                digit = shkalim[n - 1] - '0';//This works because each character is internally represented by a number.
                String[] str;
                if (digit > 0)
                {
                    str = SliceRow(tr, digit - 1).ToArray();//one_dimensional slice of the corresponding digit 
                }
                else
                {
                    str = new String[] { "", "", "", "", "" };
                }
                //     num = n - 2;

                //     if (num < 1) { num = 1; }

                //     num = $Number(shkalim.Substring(num - 1, n);

                add = "";

                switch (pos)
                {
                    case 1:
                        if (n > 1 && shkalim[n - 2] == '1')// *10 - *19
                        {
                            if (digit == 0) // *10
                            {
                                str1 = tr[0, 3]; // eser
                                if (text != "" && !String.IsNullOrEmpty(str1))
                                {
                                    text = text + ve;
                                }
                                text = text + str1;
                            }
                            else
                            { // *11 - *19
                                str1 = str[0]; // ahad, shneim, slosha, ...
                                if (text != "")
                                {
                                    //text = text + " ";
                                    if (len > 1 && digit > 0)
                                    {
                                        str1 = ve + str1;
                                    }
                                    else
                                    {
                                        text = text + " ";
                                    }

                                }

                                text = text + str1 + asar;
                            }
                        }
                        else // *##
                        {
                            if (digit == 2 && len == 1)
                            {
                                str1 = str[4]; // shnei 
                            }
                            else
                            {
                                str1 = str[0]; // ehad, shnaiim, slosha, ... 
                            }
                            if (len > 1 && digit > 0)
                            {
                                str1 = ve + str1;
                            }
                            text = text + str1;
                        }
                        break;

                    case 2:
                        str1 = str[3]; // eser, esrim, shloshim, ...
                        if (digit > 1)
                        {
                            if (text != "" && !String.IsNullOrEmpty(str1))
                            {
                                text = text + " ";
                            }
                            text = text + str1;
                        }
                        break;

                    case 3:
                        str1 = str[2]; // mea, mataiim, shlosh, arba, ...
                        if (digit > 0)
                        {
                            if (text != "" && !String.IsNullOrEmpty(str1))
                            {
                                str1 = " " + str1;
                            }
                            if (digit > 2) { add = meot; }
                        }
                        text = text + str1 + add;
                        break;

                    case 4:
                        if (n > 1 && shkalim[n - 2] == '1')// *10 - *19
                        {
                            if (digit == 0) // *10
                            {
                                if (len == 5)
                                {
                                    text = tr[0, 4];
                                } // aseret alafim 
                                else
                                {
                                    str1 = tr[0, 3]; //eser
                                    if (text != "" && !String.IsNullOrEmpty(str1))
                                    {
                                        text = text + ve;
                                    }
                                    text = text + str1 + elef;
                                }
                            }
                            else // *11 - *19
                            {
                                str1 = str[0]; // ahad, shneim, slosha, ...  
                                if (text != "")
                                {
                                    //text = text + " ";
                                    if (len > 1 && digit > 0)
                                    {
                                        str1 = ve + str1;
                                    }
                                    else
                                    {
                                        text = text + " ";
                                    }

                                }

                                text = text + str1 + asar + elef;

                            }
                        }
                        else // *## 
                        {
                            if (pos == 4 && len == 4)
                            {
                                text = str[1]; // elef, alpaiim, shloshet alafim, ...
                            }
                            else if (n > 0 && shkalim[n - 1] - '0' > 0 || n > 1 && shkalim[n - 2] - '0' > 0 || n > 2 && shkalim[n - 3] - '0' > 0)
                            {

                                str1 = str[0]; // ehad, shnaiim, slosha, ...
                                if (text != "" && !String.IsNullOrEmpty(str1))
                                {
                                    text = text + ve;
                                }
                                text = text + str1 + elef;
                            }
                        }
                        break;

                    case 5:
                        str1 = str[3]; // eser, esrim, shloshim, ...
                        if (digit > 1)
                        {
                            if (text != "") { text = text + " "; }
                            text = text + str1;
                        }
                        break;

                    case 6:
                        str1 = str[2]; // mea, mataiim, shlosh, arba, ...
                        if (digit > 0)
                        {
                            if (text != "")
                            {
                                str1 = " " + str1;
                            }
                            if (digit > 2) { add = meot; }
                        }
                        text = text + str1 + add;
                        break;

                    case 7:
                        if (n > 1 && shkalim[n - 2] == '1')// *10 - *19
                        {
                            if (digit == 0) // *10
                            {
                                    str1 = tr[0, 3]; //eser
                                    if (str1 == "עשר") { str1 = "עשרה"; }
                                    if (text != "" && !String.IsNullOrEmpty(str1))
                                    {
                                        text = text + ve;
                                    }
                                    text = text + str1;
                            }
                            else // *11 - *19
                            {
                                str1 = str[0]; // ahad, shneim, slosha, ...  
                                if (text != "")
                                {
                                    //text = text + " ";
                                    if (len > 1 && digit > 0)
                                    {
                                        str1 = ve + str1;
                                    }
                                    else
                                    {
                                        text = text + " ";
                                    }

                                }

                                text = text + str1 + asar;
                            }
                        }
                        else // *## 
                        {
                            if (n > 0 && shkalim[n - 1] - '0' > 0 || n > 1 && shkalim[n - 2] - '0' > 0 || n > 2 && shkalim[n - 3] - '0' > 0)
                            {

                                if (digit == 2 && len == 7)
                                {
                                    str1 = str[4]; // shnei 
                                }
                                else
                                {
                                    str1 = str[0]; // ehad, shnaiim, slosha, ... 
                                }
                                if (text != "" && !String.IsNullOrEmpty(str1))
                                {
                                    text = text + ve;
                                }
                                text = text + str1;
                            }
                        }

                        text = text + mili;
                        break;

                    case 8:
                        str1 = str[3]; // eser, esrim, shloshim, ...
                        if (digit > 1)
                        {
                            if (text != "") { text = text + " "; }
                            text = text + str1;
                        }
                        break;

                    case 9:
                        str1 = str[2]; // mea, mataiim, shlosh, arba, ...
                        if (digit > 0)
                        {
                            if (text != "")
                            {
                                str1 = " " + str1;
                            }
                            if (digit > 2) { add = meot; }
                        }
                        text = text + str1 + add;
                        break;

                    default:
                        break;
                } // switch 

                pos -= 1;
            } // while 

            text = text + " " + shah;

            decimal dcents = 0M;
            if (!Decimal.TryParse(agorot, out dcents))
            {
                dcents = 0M;
            }
            dcents = decimal.Round(dcents, 2);
            cents = dcents.ToString();

            if (String.IsNullOrWhiteSpace(subunit_name))
            {
                if (dcents == 0)
                {
                    text = text + " בלבד";
                    text = Trim(text);
                    text = FixSpaces(text);
                    return (text);
                }
                else if (cents.Length == 1)
                {
                    cents = "0" + cents;
                }
                text = text + " + " + cents + "/100";
            }
            else if (!cents_in_words)
            {
                if (dcents == 0)
                {
                    text = text + " בלבד";
                    text = Trim(text);
                    text = FixSpaces(text);
                    return (text);
                }
                else if (cents.Length == 1)
                {
                    cents = "0" + cents;
                }
                text = text + " + " + cents + " " + subunit_name;
            }

            else if (cents_in_words)
            {
                if (dcents == 0)
                {
                    text = text + " בלבד";
                }
                else
                {
                    text = text + " +";
                    len = cents.Length;
                    pos = len;
                    while (pos > 0)
                    {
                        n = len - pos + 1;
                        digit = cents[n - 1] - '0';//This works because each character is internally represented by a number.
                        String[] str;
                        if (digit > 0)
                        {
                            str = SliceRow(tr, digit - 1).ToArray();//one_dimensional slice of the corresponding digit 
                        }
                        else
                        {
                            str = new String[] { "", "", "", "", "" };
                        }

                        //                      v_num = v_n - 2
                        //                      If(v_num < 1) v_num = 1
                        //                      v_num = $Number(v_cents[v_num, v_n])
                        add = "";
                        switch (pos)
                        {
                            case 1:
                                if (n > 1 && cents[n - 2] == '1')// *10 - *19
                                {
                                    if (digit == 0) // *10
                                    {
                                        str1 = tr[0, 3]; // eser
                                        if (text != "") { text = text + ve; }
                                        text = text + str1;
                                    }
                                    else
                                    { // *11 - *19
                                        str1 = str[0]; // ahad, shneim, slosha, ...
                                        if (subunit_name == "אגורות" && str1 == "שנים")
                                        {
                                            str1 = "שתים";
                                        }
                                        if (text != "")
                                        {
                                            //text = text + " ";
                                            if (len > 1 && digit > 0)
                                            {
                                                str1 = ve + str1;
                                            }
                                            else
                                            {
                                                text = text + " ";
                                            }

                                        }

                                        text = text + str1 + asar + elef;
                                    }
                                }
                                else // *##
                                {
                                    if (digit == 2 && len == 1)
                                    {
                                        str1 = str[4]; // shnei 
                                        if (subunit_name == "אגורות")
                                        {
                                            str1 = "שתי";
                                        }
                                    }
                                    else
                                    {
                                        str1 = str[0]; // ehad, shnaiim, slosha, ... 
                                        if (subunit_name == "אגורות" && str1 == "שנים")
                                        {
                                            str1 = "שתים";
                                        }
                                    }
                                    if (len > 1 && digit > 0) { str1 = ve + str1; }
                                    text = text + str1;
                               }

                                break;

                            case 2:
                                str1 = str[3]; // eser, esrim, shloshim, ...
                                if (digit > 1)
                                {
                                    if (text != "") { text = text + " "; }
                                    text = text + str1;
                                }
                                break;
                            default:
                                break;
                        } // switch

                        pos -= 1;

                    } // while
                    text = text + " " + subunit_name;

                }
            }
            else
            {
                if (dcents == 0)
                {
                    text = text + " בלבד";
                }
                else if (dcents == 1)
                {
                    text = text + " + " + cents + " " + subunit_name;
                }
                else
                {
                    text = text + " + " + cents + " " + subunit_name;
                }
            }

            if (eng)
            {
                pattern = " " + shah;
                if (text.Contains(pattern))
                {
                    string[] words = text.Split(pattern.ToArray<Char>());
                    left = words[0];
                    right = words[1];
                    text = left + right + pattern;
                }
            }

            text = Trim(text);
            text = FixSpaces(text);
            return (text);
        }


        private string FixSpaces(string s)
        {
            var t = "";
            for (var i = 0; i < s.Length; i++)
            {
                if (i > 0)
                {
                    if (!((s.Substring(i - 1, 1) == " ") & (s.Substring(i, 1) == " ")))
                        t = t + s.Substring(i, 1);
                }
                else
                    t = t + s.Substring(i, 1);
            }
            return t;


        }

        private string Trim(string s)
        {
            return LTrim(RTrim(s));
        }

        private string LTrim(string s)
        {
            var i = 0;
            var j = 0;
            for (i = 0; i <= s.Length - 1; i++)
                if (s.Substring(i, 1) != " ")
                {
                    j = i;
                    break;
                }
            return s.Substring(j, s.Length);
        }
        private string RTrim(string s)
        {
            var j = 0;
            for (var i = s.Length - 1; i > -1; i--)
                if (s.Substring(i, 1) != " ")
                {
                    j = i;
                    break;
                }
            return s.Substring(0, j + 1);
        }

        private int InStr(string n, string s1, string s2 = null)
        {
            if (s2 == null)
                return (n.IndexOf(s1) + 1);
            else
                return (s1.IndexOf(s2, int.Parse(n)) + 1);
        }

        private int RInStr(string n, string s1, string s2 = null)
        {
            if (s2 == null)
                return (n.LastIndexOf(s1) + 1);
            else
                return (s1.LastIndexOf(s2, int.Parse(n)) + 1);
        }

        private void FillDocumentCustomFields(ARInvoice myInvoice, InvoiceDataProvider myDataProvider, string documentTypeCopyId, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentTypeCopy documenttypecopy = (from copy in commonContext.DocumentTypeCopies where copy.Id == documentTypeCopyId select copy).FirstOrDefault();

            string documentTypeId = null;
            if(documenttypecopy != null)
            {
                documentTypeId = documenttypecopy.DocumentTypeId;
            }

            if (!string.IsNullOrEmpty(documentTypeId))
            {
                List<FormCustomField> customfieldsList = commonContext.FormCustomFields.Where(fc => fc.DocumentTypeId == documentTypeId).ToList();
                List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.Where(fc => fc.DocumentTypeId == documentTypeId).ToList();
                
                FormCustomField shipper2CustomField = (from a in customfieldsList
                                                       where a.FieldCode == "Shipper2" && a.EntityId == myInvoice.MainEntityId
                                                       select a).FirstOrDefault();

                DocumentTypeCustomField shipper2DocumentCustom = (from a in documentCustomfieldsList
                                                                  where a.FieldCode == "Shipper2"
                                                                  select a).FirstOrDefault();

                FormCustomField shipper3CustomField = (from a in customfieldsList
                                                       where a.FieldCode == "Shipper3" && a.EntityId == myInvoice.MainEntityId
                                                       select a).FirstOrDefault();

                DocumentTypeCustomField shipper3DocumentCustom = (from a in documentCustomfieldsList
                                                                  where a.FieldCode == "Shipper3"
                                                                  select a).FirstOrDefault();

                FormCustomField shipper4CustomField = (from a in customfieldsList
                                                       where a.FieldCode == "Shipper4" && a.EntityId == myInvoice.MainEntityId
                                                       select a).FirstOrDefault();

                DocumentTypeCustomField shipper4DocumentCustom = (from a in documentCustomfieldsList
                                                                  where a.FieldCode == "Shipper4"
                                                                  select a).FirstOrDefault();

                FormCustomField shipper5CustomField = (from a in customfieldsList
                                                       where a.FieldCode == "Shipper5" && a.EntityId == myInvoice.MainEntityId
                                                       select a).FirstOrDefault();

                DocumentTypeCustomField shipper5DocumentCustom = (from a in documentCustomfieldsList
                                                                  where a.FieldCode == "Shipper5"
                                                                  select a).FirstOrDefault();

                FormCustomField HAWB2CustomField = (from a in customfieldsList
                                                    where a.FieldCode == "HAWB2" && a.EntityId == myInvoice.MainEntityId
                                                    select a).FirstOrDefault();

                DocumentTypeCustomField HAWB2DocumentCustom = (from a in documentCustomfieldsList
                                                               where a.FieldCode == "HAWB2"
                                                               select a).FirstOrDefault();

                FormCustomField HAWB3CustomField = (from a in customfieldsList
                                                    where a.FieldCode == "HAWB3" && a.EntityId == myInvoice.MainEntityId
                                                    select a).FirstOrDefault();

                DocumentTypeCustomField HAWB3DocumentCustom = (from a in documentCustomfieldsList
                                                               where a.FieldCode == "HAWB3"
                                                               select a).FirstOrDefault();

                FormCustomField HAWB4CustomField = (from a in customfieldsList
                                                    where a.FieldCode == "HAWB4" && a.EntityId == myInvoice.MainEntityId
                                                    select a).FirstOrDefault();

                DocumentTypeCustomField HAWB4DocumentCustom = (from a in documentCustomfieldsList
                                                               where a.FieldCode == "HAWB4"
                                                               select a).FirstOrDefault();

                FormCustomField HAWB5CustomField = (from a in customfieldsList
                                                    where a.FieldCode == "HAWB5" && a.EntityId == myInvoice.MainEntityId
                                                    select a).FirstOrDefault();

                DocumentTypeCustomField HAWB5DocumentCustom = (from a in documentCustomfieldsList
                                                               where a.FieldCode == "HAWB5"
                                                               select a).FirstOrDefault();
                
                myDataProvider.Shipper2 = shipper2CustomField != null ? shipper2CustomField.Value : (shipper2DocumentCustom != null ? shipper2DocumentCustom.DefaultValue : "");
                myDataProvider.Shipper3 = shipper3CustomField != null ? shipper3CustomField.Value : (shipper3DocumentCustom != null ? shipper3DocumentCustom.DefaultValue : "");
                myDataProvider.Shipper4 = shipper4CustomField != null ? shipper4CustomField.Value : (shipper4DocumentCustom != null ? shipper4DocumentCustom.DefaultValue : "");
                myDataProvider.Shipper5 = shipper5CustomField != null ? shipper5CustomField.Value : (shipper5DocumentCustom != null ? shipper5DocumentCustom.DefaultValue : "");
                myDataProvider.HAWB2 = HAWB2CustomField != null ? HAWB2CustomField.Value : (HAWB2DocumentCustom != null ? HAWB2DocumentCustom.DefaultValue : "");
                myDataProvider.HAWB3 = HAWB3CustomField != null ? HAWB3CustomField.Value : (HAWB3DocumentCustom != null ? HAWB3DocumentCustom.DefaultValue : "");
                myDataProvider.HAWB4 = HAWB4CustomField != null ? HAWB4CustomField.Value : (HAWB4DocumentCustom != null ? HAWB4DocumentCustom.DefaultValue : "");
                myDataProvider.HAWB5 = HAWB5CustomField != null ? HAWB5CustomField.Value : (HAWB5DocumentCustom != null ? HAWB5DocumentCustom.DefaultValue : "");
            }
        }

        private string GetVatAmountIncludeMultiInInvoiceCurrencyField(string lineVatTypeId, double? invoiceLineAmount, string invoiceTypeCode, List<VatType> allVATTypes, List<VatTypePercentage> allVatTypesPercentages, List<VATTypesGroup> allVATTypesGroups)
        {
            string myResult = "";

            if (!string.IsNullOrEmpty(lineVatTypeId))
            {
                VatType myVatType = allVATTypes.Where(d => d.Id == lineVatTypeId).FirstOrDefault();
                if (myVatType != null)
                {
                    double lineAmount = invoiceLineAmount == null ? 0 : invoiceLineAmount.Value;

                    if (!myVatType.IsMultiPercentage)
                    {
                        string myLineResult = "";
                        double vatPercentage = 0;
                        double lineVATAmount = 0;
                        VatTypePercentage myVatTypePercentage = (from d in allVatTypesPercentages where d.VatTypeId == lineVatTypeId orderby d.FromDate descending select d).FirstOrDefault();
                        if (myVatTypePercentage != null)
                        {
                            if (myVatTypePercentage.Percentage != null)
                            {
                                vatPercentage = myVatTypePercentage.Percentage.Value;
                            }
                        }

                        lineVATAmount = lineAmount * vatPercentage / 100;
                        if (invoiceTypeCode == "CD")
                        {
                            lineVATAmount = lineVATAmount * -1;
                        }

                        myLineResult += myVatType.EnglishName + " " + vatPercentage + "%" + "   " + String.Format("{0:#,0.00}", lineVATAmount);
                        myResult = myLineResult;
                    }

                    else
                    {
                        List<VATTypesGroup> myVATTypesGroups = allVATTypesGroups.Where(d => d.GroupVATTypeId == lineVatTypeId).ToList();
                        foreach (VATTypesGroup item in myVATTypesGroups)
                        {
                            VatType singleVatType = allVATTypes.Where(d => d.Id == item.SingleVATTypeId).FirstOrDefault();

                            if (singleVatType != null)
                            {
                                string myLineResult = "";
                                double vatPercentage = 0;
                                double lineVATAmount = 0;
                                VatTypePercentage myVatTypePercentage = (from d in allVatTypesPercentages where d.VatTypeId == singleVatType.Id orderby d.FromDate descending select d).FirstOrDefault();
                                if (myVatTypePercentage != null)
                                {
                                    if (myVatTypePercentage.Percentage != null)
                                    {
                                        vatPercentage = myVatTypePercentage.Percentage.Value;
                                    }
                                }

                                lineVATAmount = lineAmount * vatPercentage / 100;
                                if (invoiceTypeCode == "CD")
                                {
                                    lineVATAmount = lineVATAmount * -1;
                                }

                                myLineResult += singleVatType.EnglishName + " " + vatPercentage + "%" + "   " + String.Format("{0:#,0.00}", lineVATAmount);

                                if (string.IsNullOrEmpty(myResult))
                                {
                                    myResult = myLineResult;
                                }

                                else
                                {
                                    myResult += Environment.NewLine + myLineResult;
                                }
                            }
                        }
                    }
                }
            }

            return myResult;
        }
    }

    public class InvoiceTotalVATItem
    {
        [Key]
        public string Id { get; set; }
        public string VatTypeId { get; set; }
        public double? VatTypePercentage { get; set; }
        public string RowLabel { get; set; }
        public double? LocalCurrencyAmount { get; set; }
        public double? InvoiceCurrencyAmount { get; set; }
        public double? ProfitCurrencyAmount { get; set; }
        public string ExternalVatCard { get; set; }
        public string ExternalTAXItemId { get; set; }
        public string VatTypeCell { get; set; }
        public bool IsExpense { get; set; }
    }
}
