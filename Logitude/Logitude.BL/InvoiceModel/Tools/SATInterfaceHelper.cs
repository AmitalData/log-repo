using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
//using Profact.TimbraCFDI;
//using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.Resolvers;

namespace Logitude.BL.InvoiceModel.Tools
{
    public class SATInterfaceHelper
    {
        ARInvoicePM EntityPM;
        public void SendSATRequestFile(ARInvoicePM entityPM, ARInvoice entityPoco)
        {
            this.EntityPM = entityPM;
            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            if (satSetting != null)
            {
                if (satSetting.ActivationDate != null && entityPM.ApprovedDate < satSetting.ActivationDate)
                {
                    return;
                }

                switch (satSetting.SATInterfaceCode)
                {
                    case "PROF":
                    case "PROF33":
                        SendProfactoXML33(entityPM, entityPoco, satSetting);
                        break;

                    case "CONT":
                        SendContpaqFile(entityPM);
                        break;

                }

            }
        }

        #region SendInvoiceContpaqFile
        private void SendContpaqFile(ARInvoicePM ARInvoice)
        {
            int tenant = ARInvoice.Tenant;
            AddressRepository addressRepository = new AddressRepository(tenant);
            AddressQuery addressQuery = new AddressQuery(addressRepository);
            AddressPM address = addressQuery.GetSingleAddressPM(ARInvoice.BillToAddressId, tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            ShipmentPM shipmentPM = shipmentQuery.GetSingleShipmentPMByNumber(ARInvoice.MainEntityReference, tenant);

            ChargesTypeRepository chargeTypeRepository = new ChargesTypeRepository(tenant);
            ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(chargeTypeRepository);

            CardRepository cardRepository = new CardRepository(tenant);
            CardQuery cardQuery = new CardQuery(cardRepository);

            CardPM cardPM = cardQuery.GetSinglePM(ARInvoice.BillToId, tenant);

            ARInvoiceTotalVATRepository aRInvoiceTotalVATRepositoryRepository = new ARInvoiceTotalVATRepository(tenant);
            ARInvoiceTotalVATQuery arInvoiceTotalVatQuery = new ARInvoiceTotalVATQuery(aRInvoiceTotalVATRepositoryRepository);

            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName("ARInvoice", 0, true);


            string myObjectTableId = null;
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }

            StringBuilder FlatFile = new StringBuilder();
            string branchExternalId = "";
            Branch branch = BranchRepository.GetSingleBranch(ARInvoice.BranchId, tenant, true);
            if (branch != null)
                branchExternalId = branch.ExternalId;


            FlatFile.Append("INVOICE DATE|" + ARInvoice.ApprovedDate != null ? (String.Format("{0:dd/MM/yyyy hh:mm:ss}", ((DateTime?)ARInvoice.ApprovedDate))) : "" + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("CODE|" + cardPM.Code + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("NAME CUSTOMER|" + ARInvoice.BillToName + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("VAT NO CUSTOMER|" + ARInvoice.VatNumber + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS 1 MAIN ADDRESS CUSTOMER|" + address.Address1 + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS 2 MAIN ADDRESS CUSTOMER|" + address.Address2 + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN CITY|" + address.City + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN ZIP CODE|" + address.ZipCode + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN STATE|" + address.StateEnglishName + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ADDRESS MAIN COUNTRY|" + address.CountryName + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("REFERENCE NO|" + ARInvoice.MainEntityReference + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("BRANCH|" + branchExternalId + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("MAWB|" + ARInvoice.MasterNumber + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("HAWB|" + ARInvoice.HouseNumber + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("INVOICE CUSTOMER REF|" + ARInvoice.CustomerRef + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ORDER/BOOKING DATAILS QUANTITY|" + (shipmentPM != null ? (shipmentPM.NumberOfPackages + "") : "") + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ORDER/BOOKING DATAILS GROSS WEIGHT(MARITIMO) CHARGEABLE WEIGHT (AEREO)|" + (shipmentPM != null ? ((shipmentPM.TransportModeId == "A" ? shipmentPM.ChargeableWeight + "" : shipmentPM.GrossWeight + "")) : "") + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ORDER/BOOKING VOLUME (CBM)|" + (shipmentPM != null ? shipmentPM.VolumeInCBM + "" : "") + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ROUTING FROM|" + (shipmentPM != null ? (shipmentPM.MainCarriageFromPortCode + " / " + shipmentPM.MainCarriageFromPortCountryCode) : "") + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("ROUTING TO|" + (shipmentPM != null ? (shipmentPM.MainCarriageFinalDestinationPortCode + " / " + shipmentPM.MainCarriageFinalDestinationPortCountryCode) : "") + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("DESCRIPCION OF GOODS|" + (shipmentPM != null ? (shipmentPM.DescriptionOfGoods) : "") + "|");
            FlatFile.Append("\r\n");
            FlatFile.Append("TYPE OF SHIPMENT|" + (shipmentPM != null ? (shipmentPM.TransportModeId == "A" ? "Aereo" : shipmentPM.TransportModeId == "O" ? "MARITIMO" : "terrestre") : "") + " | ");
            FlatFile.Append("\r\n");
            FlatFile.Append("MAIN CARRIAGE TRANSPORT|" + (shipmentPM != null ? shipmentPM.MainCarriageCarrierName : "") + "|");
            FlatFile.Append("\r\n");
            var index = 0;

            double? sumTotalInvoiceAmount = 0;
            foreach (ARInvoiceLinePM line in ARInvoice.InvoiceLines.Where(p => !p.IsExpense))
            {
                sumTotalInvoiceAmount += line.InvoiceCurrencyAmount;
                VatType vattype = VatTypeRepository.GetSingleVatType(line.VatTypeId, line.Tenant, true);
                var vatTypeCode = "";
                if (vattype != null)
                {
                    vatTypeCode = vattype.Code;
                }

                ChargesType chargeType = ChargesTypeRepository.GetSingleChargesType(line.ChargesTypeId, line.Tenant, true);
                var chargeTypeCode = "";
                if (chargeType != null)
                {
                    chargeTypeCode = chargeType.Code;
                }
                FlatFile.Append("RECEIVABLES" + (index + 1) + "|" + chargeTypeCode + "|" + line.Quantity + "|" + branchExternalId + vatTypeCode + chargeTypeCode + "|" + chargeType.EnglishName + "|" + line.VatPercentage + "%||" + line.InvoiceCurrencyAmount + "|");
                FlatFile.Append("\r\n");
                index++;
            }

            FlatFile.Append("SUBTOTAL INVOICE|" + sumTotalInvoiceAmount + "|");
            FlatFile.Append("\r\n");
            ICommonDataContext commonContext = CommonDataContext.GetContext(ARInvoice.Tenant);
            List<ARInvoiceTotalVATPM> FilteredArInvoicesTotalvats = new List<ARInvoiceTotalVATPM>();
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(commonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(commonContext);

            List<ChargesType> allChargesTypes = chargesTypeRepository.GetChargesTypes(ARInvoice.Tenant).ToList();
            List<VatType> allVatTypes = commonContext.VatTypes.Where(d => d.Tenant == ARInvoice.Tenant).ToList();
            List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(ARInvoice.Tenant, TenantServerConfigration.GetCurrentDateTime(ARInvoice.Tenant).Date);
            List<VATTypesGroup> allVatGroups = (from d in commonContext.VATTypesGroups
                                                where d.Tenant == ARInvoice.Tenant
                                                select d).ToList();
            FilteredArInvoicesTotalvats = this.CalculateNoneExpenseTotalVats(ARInvoice, allVatTypes, allVatPercentages, allVatGroups, allChargesTypes);

            for (int i = 0; i < FilteredArInvoicesTotalvats.Count; i++)
            {
                sumTotalInvoiceAmount += FilteredArInvoicesTotalvats[i].InvoiceCurrencyVATAmount;
                if (FilteredArInvoicesTotalvats[i].VATPercent == 16)
                {
                    FlatFile.Append("STD 16% INVOICE|" + FilteredArInvoicesTotalvats[i].InvoiceCurrencyVATAmount + "|");
                }
                else
                {
                    if (FilteredArInvoicesTotalvats[i].VATPercent != 0)
                        FlatFile.Append("TOTAL VAT TYPE " + FilteredArInvoicesTotalvats[i].VATPercent + "% |" + FilteredArInvoicesTotalvats[i].InvoiceCurrencyVATAmount + "|");
                }
                if (FilteredArInvoicesTotalvats[i].VATPercent != 0)
                    FlatFile.Append("\r\n");
            }
            FlatFile.Append("TOTAL INVOICE|" + sumTotalInvoiceAmount + "|");
            FlatFile.Append("\r\n");
            double InvoiceEXChangeRate = ARInvoice.InvoiceCurrencyExchangeRate != null ? double.Parse(ARInvoice.InvoiceCurrencyExchangeRate + "") : 0;
            FlatFile.Append("CURRENCY|" + ARInvoice.InvoiceCurrencyCode + "|" + string.Format("{0:0.000000}", InvoiceEXChangeRate) + "|");
            FlatFile.Append("\r\n");

            FlatFile.Append("NOTES|" + ARInvoice.PrintNotes + "|");
            FlatFile.Append("\r\n");

            string OperationID = "", OperationFirstName = "", OperationLastName = "";
            ShipmentRepository _repo = new ShipmentRepository(tenant);
            Shipment shipment = _repo.GetSingleShipment(ARInvoice.MainEntityId, tenant);



            string PartnerCode = "";
            string SalesManName = "";
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            string SalesManUserId = ARInvoice.SalesmanUserId != null ? ARInvoice.SalesmanUserId : (shipment != null ? shipment.SalesmanUserId : null);//incase of cosilidate . Added by Rabaia
            if (SalesManUserId != null)
            {
                Contact SalesManContact = myCommonContext.Contacts.Where(d => d.Tenant == tenant && d.Id == SalesManUserId).FirstOrDefault();
                if (SalesManContact == null) // in case of customerCare added by Rabaia
                {
                    SalesManContact = myCommonContext.Contacts.Where(d => d.Tenant == 0 && d.Id == SalesManUserId).FirstOrDefault();
                }
                SalesManName = SalesManContact.EnglishName;
                ComputingPartner ComputingPartner = myCommonContext.ComputingPartners.Where(d => d.Tenant == tenant && d.Code == "Contpaq").FirstOrDefault();
                if (ComputingPartner == null)
                    ComputingPartner = myCommonContext.ComputingPartners.Where(d => d.Tenant == 0 && d.Code == "G-Contpaq").FirstOrDefault();
                if (ComputingPartner != null)
                {
                    ComputingPartnerTable Table = myCommonContext.ComputingPartnerTables.Where(d => d.Tenant == ComputingPartner.Tenant && d.ComputingPartnerId == ComputingPartner.Id && d.ObjectTable.Name == "User").FirstOrDefault();
                    if (Table != null)
                    {
                        ComputingPartnerTranslation entityCard = myCommonContext.ComputingPartnerTranslations.Where(d => d.OurCode == SalesManContact.Email && d.Tenant == ComputingPartner.Tenant && d.ComputingPartnerId == ComputingPartner.Id && d.ObjectTableId == Table.ObjectTableId).FirstOrDefault();
                        if (entityCard != null)
                        {
                            PartnerCode = entityCard.PartnerCode;
                        }
                    }

                }

            }


            FlatFile.Append("SALESMAN|" + PartnerCode + "|" + SalesManName);


            bool NameReverted = false;
            if (shipment != null)
            {
                switch (shipment.DirectionId)
                {
                    case "R":
                        {
                            OperationID = "3";
                            OperationFirstName = "Otros Sectores";
                        }
                        break;
                    case "I":
                        {
                            OperationID = "1";
                            OperationFirstName = "Importación";

                        }
                        break;

                    case "E":
                        {
                            OperationID = "2";
                            OperationFirstName = "Exportación";

                        }
                        break;

                    case "D":
                        {
                            OperationID = "4";
                            OperationFirstName = "Nacional";
                            NameReverted = true;
                        }
                        break;
                }


                switch (shipment.TransportModeId)
                {
                    case "A":
                        {
                            OperationLastName = "Aérea";
                        }
                        break;

                    case "I":
                        {
                            if (OperationID == "4")
                                OperationLastName = "Terrestre";
                            else
                                OperationLastName = "Terrestres";
                        }
                        break;


                    case "O":
                        {
                            if (OperationID == "4")
                                OperationLastName = "Marítimos";
                            else
                                OperationLastName = "Marítima";

                        }
                        break;
                }

            }
            string FullOperationName = "";

            if (NameReverted)
            {
                FullOperationName = OperationLastName + " " + OperationFirstName;
            }
            else
            {
                FullOperationName = OperationFirstName + " " + OperationLastName;
            }
            FlatFile.Append("\r\n");
            FlatFile.Append("TYPE OPERATION|" + OperationID + "|" + FullOperationName);
            FlatFile.Append("\r\n");

            //   String fileContainer = FlatFile.ToString();
            //  string path = @"D:\Flatfile.txt";

            //   File.WriteAllText(path, fileContainer);

            //  return FlatFile.ToString();

            // byte[] profactoXmlData = LogitudeXmlSerializer.SerializeObject<Comprobante>(comprobante);
            //BuildProfactCommunicationLog(profactoXmlData, entityPM);

            byte[] fileData = Encoding.UTF8.GetBytes(FlatFile.ToString());

            DropBoxActionsHelper dropBoxHelper = new DropBoxActionsHelper(tenant);
            dropBoxHelper.CreateDropBoxCommunicationLog(tenant, myObjectTableId, fileData, ARInvoice.InvoiceNumber + ".txt", "SAT", "SAT Interface", ARInvoice.Id, "Contpaq", ARInvoice.InvoiceNumber);
        }



        #endregion


        #region SendInvoiceProfactoXML


        private void SendProfactoXML33(ARInvoicePM entityPM, ARInvoice entityPoco, SATInterfaceSetting satSetting)
        {



            ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(entityPM.Tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            AddressRepository addressReposirory = new AddressRepository(commonContext);
            CardRepository cardRepository = new CardRepository(commonContext);
            BranchRepository branchRepository = new BranchRepository(commonContext);
            CurrencyRepository currencyRepository = new CurrencyRepository(commonContext);
            MeasurementRepository measurementRepository = new MeasurementRepository(commonContext);
            ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(commonContext);
            VatTypeRepository vatTypeRepository = new VatTypeRepository(commonContext);


            IInvoiceContext invoiceCotnext = InvoiceContext.GetContext(entityPM.Tenant);
            ARInvoiceTotalVATQuery aRInvoiceTotalVATQuery = new ARInvoiceTotalVATQuery(entityPM.Tenant);

            List<ChargesType> allChargesTypes = chargesTypeRepository.GetChargesTypes(entityPM.Tenant).ToList();
            List<Measurement> allMeasurements = measurementRepository.GetMeasurements(entityPM.Tenant).ToList();
            //List<VatType> vatTypes = vatTypeRepository.GetVatTypes(entityPM.Tenant).ToList();

            if (entityPM.InvoiceLines.All(l => allChargesTypes.First(c => c.Id == l.ChargesTypeId).IsExpense == true))//l.IsExpense == true &&
            {
                entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "ND";
                return;
            }

            List<ARInvoiceTotalVATPM> totalNoneExpenseVats = new List<ARInvoiceTotalVATPM>();
            VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(commonContext);
            VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);


            List<VatType> allVatTypes = commonContext.VatTypes.Where(d => d.Tenant == entityPM.Tenant).ToList();
            List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(entityPM.Tenant, TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant).Date);
            List<VATTypesGroup> allVatGroups = (from d in commonContext.VATTypesGroups
                                                where d.Tenant == entityPM.Tenant
                                                select d).ToList();

            Currency invoiceCurrency = currencyRepository.GetSingleCurrency(entityPM.InvoiceCurrencyId, entityPM.Tenant);
            Address branchAddress = null;
            Address mainAddress = null;
            Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
            if (!string.IsNullOrEmpty(entityPM.BranchId))
            {
                Branch branch = branchRepository.GetSingleBranch(entityPM.BranchId, entityPM.Tenant);
                if (!string.IsNullOrEmpty(branch.AddressId))
                {
                    branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
                }
            }


            Address billToAddress = null;
            if (!string.IsNullOrEmpty(entityPM.BillToAddressId))
            {
                billToAddress = addressReposirory.GetSingleAddress(entityPM.BillToAddressId, entityPM.Tenant);
            }
            else
                throw new ApplicationException("Bill to Address is required ");

            Card billToCard = cardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant);

            List<SATPaymentMethod> allPaymentMethods = (from d in invoiceCotnext.SATPaymentMethods select d).ToList();

            if (string.IsNullOrEmpty(currentTenant.VatNumber))
            {
                throw new ApplicationException("Company Vat Number is required");
            }

            if (entityPM.InvoiceDate == null)
            {
                throw new ApplicationException("Invoice Date is required");
            }

            //if (string.IsNullOrEmpty(billToCard.VatNumber))
            //{
            //    throw new ApplicationException("Bill to Vat Number is required");
            //}
            if (string.IsNullOrEmpty(entityPM.SATPaymentMethodCode))
            {
                throw new ApplicationException("Forma Pago is required");
            }

            if (branchAddress != null)
            {

                if (string.IsNullOrEmpty(branchAddress.ZipCode))
                {
                    throw new ApplicationException("Branch Address ZipCode is required ");
                }

                mainAddress = branchAddress;
            }
            else
            {
                if (currentTenant.Address != null)
                {
                    if (string.IsNullOrEmpty(currentTenant.Address.ZipCode))
                    {
                        throw new ApplicationException("Company Address ZipCode is required ");
                    }

                    mainAddress = currentTenant.Address;
                }
                else
                    throw new ApplicationException("Company Address is required ");
            }

            if (string.IsNullOrEmpty(entityPM.MetodoPagoCode))
            {
                if (!string.IsNullOrEmpty(billToCard.MetodoPagoCode))
                    entityPM.MetodoPagoCode = billToCard.MetodoPagoCode;
                else
                    entityPM.MetodoPagoCode = satSetting.MetodoPagoCode;
            }

            if (string.IsNullOrEmpty(entityPM.MetodoPagoCode))
            {
                throw new ApplicationException("Metodo Pago is required ");
            }

            //Creamos un comprobante por medio de la entidad Comprobante
            string serie = "A";
            string folio = entityPM.InvoiceNumber;

            string counterPrefix = null;
            if (entityPM.IsConstituentInvoice)
            {
                counterPrefix = TableCounter.GetCounterPrefix(entityPM.Tenant, "CNST", "CNS", null);
            }
            else if (entityPM.IsConsolidationInvoice)
            {
                counterPrefix = TableCounter.GetCounterPrefix(entityPM.Tenant, "INVC", "CON", null);
            }
            else
            {
                counterPrefix = TableCounter.GetCounterPrefix(entityPM.Tenant, "INVC", entityPM.ARInvoiceTypeCode, null);
            }

            if (!String.IsNullOrEmpty(counterPrefix))
            {
                serie = counterPrefix;

                if (!string.IsNullOrEmpty(entityPM.InvoiceNumber))
                {
                    if (entityPM.InvoiceNumber.Contains(counterPrefix))
                    {
                        folio = entityPM.InvoiceNumber.Remove(0, counterPrefix.Length);
                    }
                }
            }

            SATPaymentMethod satPaymentMethod = allPaymentMethods.Where(a => a.Code == entityPM.SATPaymentMethodCode).FirstOrDefault();
            Profact.TimbraCFDI33.Comprobante comprobante = new Profact.TimbraCFDI33.Comprobante();

            //Llenamos datos del comprobante
            if (invoiceCurrency.Code != "MXN")
            {
                comprobante.TipoCambio = (entityPM.InvoiceCurrencyExchangeRate != null ? Convert.ToDecimal(entityPM.InvoiceCurrencyExchangeRate.Value) : 0);
                comprobante.TipoCambioSpecified = true;
            }

            comprobante.Moneda = invoiceCurrency.Code;
            comprobante.Serie = serie;
            comprobante.Version = "3.3";
            comprobante.Folio = folio;
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            // var invoiceDate = entityPM.InvoiceDate.Value.ToUniversalTime();
            comprobante.Fecha = entityPM.InvoiceDate.Value;//entityPM.InvoiceDate != null ? entityPM.InvoiceDate.Value : currentDateTime;c
            comprobante.Fecha = new DateTime(comprobante.Fecha.Year, comprobante.Fecha.Month, comprobante.Fecha.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);
            //comprobante.Fecha = comprobante.Fecha.ToUniversalTime();
            //comprobante.formaDePago = "una sola exhibición";

            comprobante.FormaPago = satPaymentMethod.Code;
            comprobante.FormaPagoSpecified = true;

            comprobante.MetodoPago = entityPM.MetodoPagoCode;//"PUE";
            comprobante.MetodoPagoSpecified = true;

            comprobante.SubTotal = Math.Abs((entityPM.SubTotalInInvoiceCurrency != null ? (decimal)entityPM.SubTotalInInvoiceCurrency.Value : 0));
            comprobante.Total = Math.Abs((entityPM.AmountInInvoiceCurrency != null ? (decimal)entityPM.AmountInInvoiceCurrency.Value : 0));
            //TIPO DE COMPROBANTE 
            //Ingreso: Factura 1, Rec honorarios 4, rec de arrendamiento 5, Rec donativos 7, Nota de cargo 3
            //Egreso: Nota de credito 2
            //Traslado: Carta porte  6


            //Llenamos datos del emisor
            comprobante.Emisor = new Profact.TimbraCFDI33.ComprobanteEmisor();
            comprobante.Emisor.Rfc = currentTenant.VatNumber;
            comprobante.Emisor.Nombre = currentTenant.Company;
            comprobante.Emisor.RegimenFiscal = "601";


            //Llena datos del receptor
            comprobante.Receptor = new Profact.TimbraCFDI33.ComprobanteReceptor();
            //comprobante.Receptor.Rfc = billToCard.VatNumber;
            comprobante.Receptor.Nombre = billToCard.EnglishName;

            string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
            if (billToAddress.Country != null)
            {
                billToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, "G-Profact", "Country");
            }

            if (billToCountryCode == "MEX" || billToCountryCode == "MX")
            {
                if (!string.IsNullOrEmpty(billToCard.VatNumber))
                    comprobante.Receptor.Rfc = billToCard.VatNumber;
                else
                    comprobante.Receptor.Rfc = "AAA010101AAA";
            }
            else
            {
                if (!string.IsNullOrEmpty(billToCard.SATForeignRFC))
                    comprobante.Receptor.Rfc = billToCard.SATForeignRFC;
                else
                    comprobante.Receptor.Rfc = "XEXX010101000";
            }

            //comprobante.Receptor.ResidenciaFiscal = billToCountryCode;
            //comprobante.Receptor.ResidenciaFiscalSpecified = true;
            //Llena domicilio del receptor

            //comprobante.TipoDeComprobante = "I";
            if (!string.IsNullOrEmpty(entityPM.UsoCFDICode))
            {
                comprobante.Receptor.UsoCFDI = entityPM.UsoCFDICode;
            }
            else
            {

                if (entityPM.ARInvoiceTypeCode == "CD")
                {
                    comprobante.Receptor.UsoCFDI = "G02";
                }
                else
                {
                    comprobante.Receptor.UsoCFDI = "G03";
                }
            }

            if (entityPM.ARInvoiceTypeCode == "CD")
            {
                comprobante.TipoDeComprobante = "E";//Profact.TimbraCFDI33.tip
            }
            else
            {
                comprobante.TipoDeComprobante = "I";//Profact.TimbraCFDI33.ComprobanteTipoDeComprobante.ingreso;
            }
            BuildRelatedInvoiceTag(entityPM, invoiceCotnext, comprobante);

            if (branchAddress != null && !string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                comprobante.LugarExpedicion = branchAddress.ZipCode;//(mainAddress.City != null ? mainAddress.City : "") + ", " + (mainAddress.State != null ? mainAddress.State.EnglishName : "Estado expedido en");//"Mexico, Distrito Federal";
            }
            else
                comprobante.LugarExpedicion = currentTenant.Address.ZipCode;



            //Llenamos los conceptos

            decimal TotalImpuestosRetenidos = 0;
            decimal TotalImpuestosTrasladados = 0;
            bool hasExpenses = entityPM.InvoiceLines.Any(l => allChargesTypes.First(c => c.Id == l.ChargesTypeId).IsExpense);
            List<Profact.TimbraCFDI33.ComprobanteConcepto> conceptosList = new List<Profact.TimbraCFDI33.ComprobanteConcepto>();
            foreach (ARInvoiceLinePM line in entityPM.InvoiceLines)
            {
                if (!allChargesTypes.First(c => c.Id == line.ChargesTypeId).IsExpense)
                {
                    Profact.TimbraCFDI33.ComprobanteConcepto concepto = new Profact.TimbraCFDI33.ComprobanteConcepto();
                    concepto.Cantidad = Math.Abs((line.Quantity != null ? ((decimal)line.Quantity.Value) : 0));
                    concepto.Unidad = "SERVICIO";
                    //concepto.noIdentificacion = "1";
                    concepto.Descripcion = line.Description;
                    concepto.Importe = GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? ((decimal)line.InvoiceCurrencyAmount.Value) : 0)));
                    decimal valorUnitario = Math.Abs(concepto.Cantidad != 0 ? (concepto.Importe / concepto.Cantidad) : 0);
                    concepto.ValorUnitario = GetDecimalWith3DigitsAfterPointIfZero(Math.Abs(Math.Truncate(valorUnitario * 1000000m) / 1000000m));

                    concepto.ClaveProdServ = allChargesTypes.FirstOrDefault(c => c.Id == line.ChargesTypeId).SATExternalId;
                    var lineMeasurement = allMeasurements.FirstOrDefault(m => m.Id == line.MeasurementId);
                    if (lineMeasurement != null)
                        concepto.ClaveUnidad = computingPartnerHelper.GetComputingPartnerCodeTranslation(lineMeasurement.Code, "G-Profact", "Measurement");//"C81";
                                                                                                                                                           //line.mea
                    if (string.IsNullOrEmpty(concepto.ClaveProdServ))
                    {
                        throw new Exception("Measurement on charge type is required");
                    }

                    if (string.IsNullOrEmpty(concepto.ClaveProdServ))
                    {
                        throw new Exception("SAT External Id on charge type is required");
                    }

                    //if (entityPM.InternalNotes == "11" || entityPM.InternalNotes == "12")
                    //{
                    //    this.CalucalteLineTotalsNew(line, concepto, allVatTypes, allVatPercentages, allVatGroups, conceptosList);
                    //}
                    //else
                    this.CalucalteLineTotals(line, concepto, allVatTypes, allVatPercentages, allVatGroups, conceptosList);


                    conceptosList.Add(concepto);
                }
            }

            comprobante.Conceptos = conceptosList.ToArray();

            //Llenamos impuestos
            List<Profact.TimbraCFDI33.ComprobanteImpuestos> impuestosList = new List<Profact.TimbraCFDI33.ComprobanteImpuestos>();
            List<Profact.TimbraCFDI33.ComprobanteImpuestosTraslado> trasladoList = new List<Profact.TimbraCFDI33.ComprobanteImpuestosTraslado>();
            List<Profact.TimbraCFDI33.ComprobanteImpuestosRetencion> retencionList = new List<Profact.TimbraCFDI33.ComprobanteImpuestosRetencion>();
            List<ARInvoiceTotalVATPM> arTotalVats = new List<ARInvoiceTotalVATPM>();
            if (!hasExpenses)
            {
                arTotalVats = aRInvoiceTotalVATQuery.GetTotalVATs(entityPM.Id, entityPM.Tenant).ToList();

            }
            else
            {
                arTotalVats = this.CalculateNoneExpenseTotalVats(entityPM, allVatTypes, allVatPercentages, allVatGroups, allChargesTypes);
                double? localAmountTotal = 0;
                double? invoiceAmountTotal = 0;
                double? profitAmountTotal = 0;
                double? invoiceVatableAmountTotal = 0;
                foreach (ARInvoiceTotalVATPM record in arTotalVats)
                {
                    localAmountTotal += record.LocalVATAmount;
                    invoiceAmountTotal += record.InvoiceCurrencyVATAmount;
                    profitAmountTotal += record.ProfitCurrencyVATAmount;
                    invoiceVatableAmountTotal += record.InvoiceCurrencyVatableAmount;
                }
                //var subtotal = Math.Abs(invoiceVatableAmountTotal.Value);
                //var total = Math.Abs((invoiceVatableAmountTotal + invoiceAmountTotal).Value);
                //comprobante.SubTotal = Math.Abs((decimal)subtotal);
                //comprobante.Total = Math.Abs((decimal)total);

                double subtotal = 0;
                List<ARInvoiceLinePM> myDataLines = entityPM.InvoiceLines.Where(d => !allChargesTypes.First(c => c.Id == d.ChargesTypeId).IsExpense && d.VatTypeId != null).ToList();
                foreach (var line in myDataLines)
                {
                    subtotal += (line.InvoiceCurrencyAmount != null ? line.InvoiceCurrencyAmount.Value : 0);
                }

                comprobante.SubTotal = Math.Abs((decimal)subtotal);
                var total = Math.Abs((subtotal + invoiceAmountTotal).Value);
                comprobante.Total = Math.Abs((decimal)total);
                //comprobante.Total = Math.Abs((decimal)total);
            }


            foreach (ARInvoiceTotalVATPM totalVat in arTotalVats)
            {
                if (totalVat.VATPercent >= 0)
                {
                    var totalVatVatType = allVatTypes.FirstOrDefault(d => d.Id == totalVat.VatTypeId);

                    TotalImpuestosTrasladados += Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0));
                    string _totaltipoFactor = (totalVat.VATPercent == 0 && totalVatVatType.Code == "EXMPT" ? "Exento" : "Tasa");
                    string total_tasaOCuota = totalVat.VATPercent != 0 ? (totalVat.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(totalVat.VATPercent.Value / 100).ToString()), '0', 8) : "") : "0.000000";
                    Profact.TimbraCFDI33.ComprobanteImpuestosTraslado traslado = trasladoList.FirstOrDefault(t => t.TipoFactor == _totaltipoFactor);//&& (totalVat.VATPercent != 0)
                    if (traslado == null
                        || (traslado != null && (totalVat.VATPercent != 0 && traslado.TasaOCuota == "0.000000")
                        || (totalVat.VATPercent == 0 && traslado.TasaOCuota != "0.000000")))
                    {
                        //if (totalVat.VATPercent != 0)
                        //{
                        if (_totaltipoFactor != "Exento")
                        {
                            traslado = new Profact.TimbraCFDI33.ComprobanteImpuestosTraslado()
                            {
                                Importe = GetDecimalWith2DigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0))),
                                Impuesto = "002",
                                TasaOCuota = total_tasaOCuota,
                                TipoFactor = _totaltipoFactor,//(totalVat.VATPercent == 0 ? "Exento" : "Tasa"),
                            };

                            //if (traslado.TipoFactor == "Tasa")
                            //{
                            //    traslado.TasaOCuota = (totalVat.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(totalVat.VATPercent.Value / 100).ToString()), '0', 8) : "");//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                            //    traslado.Importe = Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? ((decimal)totalVat.InvoiceCurrencyVATAmount.Value) : 0));
                            //    //traslado.ImporteSpecified = true;
                            //    //traslado.TasaOCuotaSpecified = true;
                            //}

                            trasladoList.Add(traslado);
                        }
                        //}
                    }
                    else
                    {
                        if (traslado.TipoFactor == "Tasa")
                        {
                            traslado.Importe = GetDecimalWith2DigitsAfterPoint(TotalImpuestosTrasladados);
                        }
                    }


                }
                else
                {
                    TotalImpuestosRetenidos += Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? (Math.Abs((decimal)totalVat.InvoiceCurrencyVATAmount.Value)) : 0));
                    Profact.TimbraCFDI33.ComprobanteImpuestosRetencion retencion = retencionList.FirstOrDefault();
                    if (retencion == null)
                    {
                        //if (retencionList.Any())
                        //{
                        retencion = new Profact.TimbraCFDI33.ComprobanteImpuestosRetencion()
                        {
                            Importe = GetDecimalWith2DigitsAfterPoint(Math.Abs((totalVat.InvoiceCurrencyVATAmount != null ? (Math.Abs((decimal)totalVat.InvoiceCurrencyVATAmount.Value)) : 0))),
                            Impuesto = "002",
                        };

                        retencionList.Add(retencion);
                        // }
                    }
                    else
                    {
                        retencion.Importe = GetDecimalWith2DigitsAfterPoint(TotalImpuestosRetenidos);
                    }


                }
            }

            decimal totalTraslados = 0;
            decimal totalRetenciones = 0;
            foreach (var c in comprobante.Conceptos)
            {
                if (c.Impuestos != null)
                {
                    if (c.Impuestos.Traslados != null)
                    {
                        foreach (var t in c.Impuestos.Traslados)
                        {
                            totalTraslados += t.Importe;
                        }
                    }
                    if (c.Impuestos.Retenciones != null)
                    {
                        foreach (var r in c.Impuestos.Retenciones)
                        {
                            totalRetenciones += r.Importe;
                        }
                    }
                }
            }

            decimal roundedTotalTraslados = (decimal)MethodHelper.Roundd((double)totalTraslados, 2);
            decimal roundedTotalRetenciones = (decimal)MethodHelper.Roundd((double)totalRetenciones, 2);

            if ((trasladoList.Count > 0) || retencionList.Count > 0)
            {
                comprobante.Impuestos = new Profact.TimbraCFDI33.ComprobanteImpuestos();

                if (trasladoList.Count > 0)
                {
                    comprobante.Impuestos.Traslados = trasladoList.ToArray();
                    comprobante.Impuestos.TotalImpuestosTrasladados = GetDecimalWith2DigitsAfterPoint(totalTraslados);//Math.Abs(TotalImpuestosTrasladados);
                    comprobante.Impuestos.TotalImpuestosTrasladadosSpecified = true;

                    if (comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa").Any())
                    {
                        Profact.TimbraCFDI33.ComprobanteImpuestosTraslado traslado = comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa").FirstOrDefault();
                        if (comprobante.Impuestos.Traslados.Count() > 1)
                        {
                            traslado = comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa" && t.TasaOCuota != "0.000000").FirstOrDefault();
                        }

                        if (traslado.Importe != GetDecimalWith2DigitsAfterPoint(totalTraslados))
                        {
                            decimal precentage = (decimal.Parse(traslado.TasaOCuota.TrimEnd('0')) * 100);
                            throw new Exception("Due to the SAT Invoice Transmission we calculate the VAT amount per line. There is a difference between the lines VAT sum and the total VAT (" + totalTraslados + ") at the invoice level. You are not allowed to approve the invoice unless you adjust the lines with the following VAT : " + precentage.ToString().TrimEnd('0').TrimEnd('.') + "%");
                        }

                        //comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Tasa").FirstOrDefault().Importe = GetDecimalWith2DigitsAfterPoint(totalTraslados);
                        traslado.Importe = GetDecimalWith2DigitsAfterPoint(totalTraslados);
                    }

                    if (comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Exento").Any())
                    {
                        comprobante.Impuestos.Traslados.Where(t => t.TipoFactor == "Exento").FirstOrDefault().Importe = GetDecimalWith2DigitsAfterPoint(0);
                    }

                }

                if (retencionList.Count > 0)
                {
                    comprobante.Impuestos.Retenciones = retencionList.ToArray();
                    comprobante.Impuestos.TotalImpuestosRetenidos = GetDecimalWith2DigitsAfterPoint(totalRetenciones);//Math.Abs(TotalImpuestosRetenidos);
                    comprobante.Impuestos.TotalImpuestosRetenidosSpecified = true;


                    Profact.TimbraCFDI33.ComprobanteImpuestosRetencion retencion = comprobante.Impuestos.Retenciones[0];
                    if (retencion.Importe != GetDecimalWith2DigitsAfterPoint(totalRetenciones))
                    {
                        Profact.TimbraCFDI33.ComprobanteConcepto concepto = comprobante.Conceptos.Where(c => c.Impuestos.Retenciones != null && c.Impuestos.Retenciones.Count() > 0).FirstOrDefault();
                        if (concepto != null)
                        {
                            Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion conceptoRetencion = concepto.Impuestos.Retenciones.FirstOrDefault();
                            if (conceptoRetencion != null)
                            {
                                decimal precentage = conceptoRetencion.TasaOCuota * 100;
                                throw new Exception("Due to the SAT Invoice Transmission we calculate the VAT amount per line. There is a difference between the lines VAT sum and the total VAT at the invoice level. You are not allowed to approve the invoice unless you adjust the lines with the following VAT : " + precentage.ToString().TrimEnd('0').TrimEnd('.') + "%");
                            }
                        }

                    }

                    comprobante.Impuestos.Retenciones[0].Importe = GetDecimalWith2DigitsAfterPoint(totalRetenciones);

                }

            }

            decimal calculatedTotal = comprobante.SubTotal + totalTraslados - totalRetenciones;
            if (calculatedTotal != comprobante.Total)
            {

            }

            BuildProfactCommunicationLog33(comprobante, entityPM.Tenant, entityPM.Id, entityPM.InvoiceNumber.ToString());

            entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "TG";

        }

        private void CalucalteLineTotals(ARInvoiceLinePM line, Profact.TimbraCFDI33.ComprobanteConcepto concepto,
            List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<Profact.TimbraCFDI33.ComprobanteConcepto> conceptosList)
        {
            List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado> lineTranslados = new List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado>();
            List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion> lineRetencions = new List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion>();

            VatType lineVatType = allVatTypes.Where(d => d.Id == line.VatTypeId).FirstOrDefault();

            if (!lineVatType.IsMultiPercentage)
            {
                if (line.VatPercentage >= 0)
                {
                    Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado traslado = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado()
                    {
                        Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0))),
                        Impuesto = "002",
                        TipoFactor = (line.VatPercentage == 0 && lineVatType.Code == "EXMPT" ? "Exento" : "Tasa"),

                    };

                    if (traslado.TipoFactor == "Tasa")
                    {
                        traslado.TasaOCuota = line.VatPercentage != 0 ? (line.VatPercentage != null ? StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8) : "") : "0.000000";//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                        traslado.Importe = GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2));
                        traslado.ImporteSpecified = true;
                        traslado.TasaOCuotaSpecified = true;
                    }

                    lineTranslados.Add(traslado);
                }
                else
                {

                    Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion retencion = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion()
                    {
                        Impuesto = "002",
                        TipoFactor = (line.VatPercentage == 0 ? "Exento" : "Tasa"),
                        TasaOCuota = (line.VatPercentage != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8)) : 0),//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                        Importe = GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2)),
                    };

                    //VatType lineVatType = vatTypes.FirstOrDefault(v => v.Id == line.VatTypeId);
                    if (lineVatType.Code != "EXMPT")
                    {

                        retencion.Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0)));//retencion.Importe;
                    }

                    lineRetencions.Add(retencion);
                }
            }
            else
            {
                List<ARInvoiceTotalVATPM> totalNoneExpenseVats = new List<ARInvoiceTotalVATPM>();

                List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();
                List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == line.VatTypeId).ToList();
                foreach (VATTypesGroup itemGroup in myVatGroups)
                {
                    InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                    {
                        Id = itemGroup.SingleVATTypeId,
                        VatTypeId = itemGroup.SingleVATTypeId,
                        LocalCurrencyAmount = line.LocalCurrencyAmount,
                        InvoiceCurrencyAmount = line.InvoiceCurrencyAmount,
                        ProfitCurrencyAmount = line.ProfitCurrencyAmount,
                    };

                    VatType vatType = allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                    if (vatType != null)
                    {
                        newItem.ExternalVatCard = vatType.ReceivablesExternalId;
                        newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                    }

                    VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                    if (myPercentagePM != null)
                    {
                        newItem.VatTypePercentage = myPercentagePM.Percentage;
                    }

                    group_Source.Add(newItem);
                }


                List<InvoiceTotalsClass> group_data
       = (from items in group_Source
          group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId } into g
          select new InvoiceTotalsClass()
          {
              Id = g.Key.VatTypeId,
              VatTypeId = g.Key.VatTypeId,
              VatTypePercentage = g.Key.VatTypePercentage,
              LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
              InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
              ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
              ExternalVatCard = g.Key.ExternalVatCard,
              ExternalTAXItemId = g.Key.ExternalTAXItemId
          }).ToList();


                foreach (InvoiceTotalsClass item in group_data)
                {
                    ARInvoiceTotalVATPM record = new ARInvoiceTotalVATPM()
                    {

                        Tenant = line.Tenant,
                        ARInvoiceId = line.ARInvoiceId,
                        VatTypeId = item.Id,
                        VATPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                        LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                        InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                        ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                        ExternalVATCard = item.ExternalVatCard,
                        ExternalTAXItemId = item.ExternalTAXItemId,
                    };

                    record.LocalVATAmount = MethodHelper.Roundd((record.LocalVatableAmount * record.VATPercent / 100), 2);
                    record.InvoiceCurrencyVATAmount = MethodHelper.Roundd((record.InvoiceCurrencyVatableAmount * record.VATPercent / 100), 2);
                    record.ProfitCurrencyVATAmount = MethodHelper.Roundd((record.ProfitVatableAmount * record.VATPercent / 100), 2);
                    totalNoneExpenseVats.Add(record);


                }

                foreach (ARInvoiceTotalVATPM lineTotal in totalNoneExpenseVats)
                {
                    if (lineTotal.VATPercent >= 0)
                    {
                        Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado traslado = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado()
                        {
                            Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0))),
                            Impuesto = "002",
                            TipoFactor = (lineTotal.VATPercent == 0 && lineVatType.Code == "EXMPT" ? "Exento" : "Tasa"),
                        };

                        if (traslado.TipoFactor == "Tasa")
                        {
                            traslado.TasaOCuota = lineTotal.VATPercent != 0 ? (lineTotal.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8) : "") : "0.000000";//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                            traslado.Importe = GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2));
                            traslado.ImporteSpecified = true;
                            traslado.TasaOCuotaSpecified = true;
                        }

                        lineTranslados.Add(traslado);
                    }
                    else
                    {
                        Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion retencion = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion()
                        {
                            Impuesto = "002",
                            TipoFactor = (lineTotal.VATPercent == 0 ? "Exento" : "Tasa"),
                            TasaOCuota = (lineTotal.VATPercent != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8)) : 0),//(lineTotal.VatTypePercentage != null ? (decimal)(Math.Abs(lineTotal.VatTypePercentage.Value / 100)) : 0),
                            Importe = GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2)),//Math.Abs(((lineTotal.InvoiceCurrencyAmount != null ? ((decimal)lineTotal.InvoiceCurrencyAmount.Value) : 0) * ((lineTotal.VATPercent != null ? (decimal)lineTotal.VATPercent.Value : 0) / 100))),
                        };

                        if (lineVatType.Code != "EXMPT")
                        {
                            retencion.Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0)));//retencion.Importe;
                        }

                        lineRetencions.Add(retencion);
                    }
                }



            }
            // concepto.
            concepto.Impuestos = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestos();

            if (lineTranslados.Count > 0)
            {
                concepto.Impuestos.Traslados = lineTranslados.ToArray();

            }
            if (lineRetencions.Count > 0)
            {
                concepto.Impuestos.Retenciones = lineRetencions.ToArray();
            }



        }

        private decimal GetImporte(double? invoiceAmount, double? vatPercentage)
        {
            decimal result = 0;
            int numberOfDigitsAfterPoint = 2;
            double amount = Math.Abs((invoiceAmount != null ? (invoiceAmount.Value) : 0));
            double percentage = (vatPercentage != null ? vatPercentage.Value : 0);
            double higher = 0;
            double lower = 0;
            var m = Math.Exp(-numberOfDigitsAfterPoint) / 2;
            lower = (amount - Math.Exp(-numberOfDigitsAfterPoint) / 2) * (percentage / 100);
            higher = (amount + Math.Exp(-numberOfDigitsAfterPoint) / (2 - Math.Exp(-12))) * (percentage / 100);

            if (this.EntityPM.InternalNotes == "11")
            {
                result = (decimal)(Math.Truncate(100 * lower) / 100);//GetDecimalWith2DigitsAfterPoint((decimal)lower);
            }
            else
            {
                result = (decimal)MethodHelper.Roundd(higher, 2);
                //result = GetDecimalWith2DigitsAfterPoint((decimal)higher);
            }

            return result;
            //GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) *(line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2)),


        }
        private void CalucalteLineTotalsNew(ARInvoiceLinePM line, Profact.TimbraCFDI33.ComprobanteConcepto concepto,
    List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<Profact.TimbraCFDI33.ComprobanteConcepto> conceptosList)
        {
            List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado> lineTranslados = new List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado>();
            List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion> lineRetencions = new List<Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion>();

            VatType lineVatType = allVatTypes.Where(d => d.Id == line.VatTypeId).FirstOrDefault();

            if (!lineVatType.IsMultiPercentage)
            {
                if (line.VatPercentage >= 0)
                {
                    Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado traslado = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado()
                    {
                        Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0))),
                        Impuesto = "002",
                        TipoFactor = (line.VatPercentage == 0 && lineVatType.Code == "EXMPT" ? "Exento" : "Tasa"),

                    };

                    if (traslado.TipoFactor == "Tasa")
                    {
                        traslado.TasaOCuota = line.VatPercentage != 0 ? (line.VatPercentage != null ? StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8) : "") : "0.000000"; ;//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                        traslado.Importe = GetImporte(line.InvoiceCurrencyAmount, line.VatPercentage);//GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2));
                        traslado.ImporteSpecified = true;
                        traslado.TasaOCuotaSpecified = true;
                    }

                    lineTranslados.Add(traslado);
                }
                else
                {

                    Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion retencion = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion()
                    {
                        Impuesto = "002",
                        TipoFactor = (line.VatPercentage == 0 ? "Exento" : "Tasa"),
                        TasaOCuota = (line.VatPercentage != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(line.VatPercentage.Value / 100).ToString()), '0', 8)) : 0),//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                        Importe = GetImporte(line.InvoiceCurrencyAmount, line.VatPercentage),//GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(((line.InvoiceCurrencyAmount != null ? (line.InvoiceCurrencyAmount.Value) : 0) * ((line.VatPercentage != null ? line.VatPercentage.Value : 0) / 100))), 2)),
                    };

                    //VatType lineVatType = vatTypes.FirstOrDefault(v => v.Id == line.VatTypeId);
                    if (lineVatType.Code != "EXMPT")
                    {

                        retencion.Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((line.InvoiceCurrencyAmount != null ? (decimal)line.InvoiceCurrencyAmount.Value : 0)));//retencion.Importe;
                    }

                    lineRetencions.Add(retencion);
                }
            }
            else
            {
                List<ARInvoiceTotalVATPM> totalNoneExpenseVats = new List<ARInvoiceTotalVATPM>();

                List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();
                List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == line.VatTypeId).ToList();
                foreach (VATTypesGroup itemGroup in myVatGroups)
                {
                    InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                    {
                        Id = itemGroup.SingleVATTypeId,
                        VatTypeId = itemGroup.SingleVATTypeId,
                        LocalCurrencyAmount = line.LocalCurrencyAmount,
                        InvoiceCurrencyAmount = line.InvoiceCurrencyAmount,
                        ProfitCurrencyAmount = line.ProfitCurrencyAmount,
                    };

                    VatType vatType = allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                    if (vatType != null)
                    {
                        newItem.ExternalVatCard = vatType.ReceivablesExternalId;
                        newItem.ExternalTAXItemId = vatType.ExternalTAXItemId;
                    }

                    VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                    if (myPercentagePM != null)
                    {
                        newItem.VatTypePercentage = myPercentagePM.Percentage;
                    }

                    group_Source.Add(newItem);
                }


                List<InvoiceTotalsClass> group_data
       = (from items in group_Source
          group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId } into g
          select new InvoiceTotalsClass()
          {
              Id = g.Key.VatTypeId,
              VatTypeId = g.Key.VatTypeId,
              VatTypePercentage = g.Key.VatTypePercentage,
              LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
              InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
              ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
              ExternalVatCard = g.Key.ExternalVatCard,
              ExternalTAXItemId = g.Key.ExternalTAXItemId
          }).ToList();


                foreach (InvoiceTotalsClass item in group_data)
                {
                    ARInvoiceTotalVATPM record = new ARInvoiceTotalVATPM()
                    {

                        Tenant = line.Tenant,
                        ARInvoiceId = line.ARInvoiceId,
                        VatTypeId = item.Id,
                        VATPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                        LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                        InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                        ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                        ExternalVATCard = item.ExternalVatCard,
                        ExternalTAXItemId = item.ExternalTAXItemId,
                    };

                    record.LocalVATAmount = MethodHelper.Roundd((record.LocalVatableAmount * record.VATPercent / 100), 2);
                    record.InvoiceCurrencyVATAmount = (double)GetImporte(record.InvoiceCurrencyVatableAmount, record.VATPercent);//MethodHelper.Roundd((record.InvoiceCurrencyVatableAmount * record.VATPercent / 100), 2);
                    record.ProfitCurrencyVATAmount = (double)GetImporte(record.InvoiceCurrencyVatableAmount, record.VATPercent);//MethodHelper.Roundd((record.ProfitVatableAmount * record.VATPercent / 100), 2);
                    totalNoneExpenseVats.Add(record);


                }

                foreach (ARInvoiceTotalVATPM lineTotal in totalNoneExpenseVats)
                {
                    if (lineTotal.VATPercent >= 0)
                    {
                        Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado traslado = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosTraslado()
                        {
                            Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0))),
                            Impuesto = "002",
                            TipoFactor = (lineTotal.VATPercent == 0 && lineVatType.Code == "EXMPT" ? "Exento" : "Tasa"),
                        };

                        if (traslado.TipoFactor == "Tasa")
                        {
                            traslado.TasaOCuota = lineTotal.VATPercent != 0 ? (lineTotal.VATPercent != null ? StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8) : "") : "0.000000"; ;//(line.VatPercentage != null ? (decimal)(Math.Abs(line.VatPercentage.Value / 100)) : 0),
                            traslado.Importe = (decimal)lineTotal.InvoiceCurrencyVATAmount;//GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2));
                            traslado.ImporteSpecified = true;
                            traslado.TasaOCuotaSpecified = true;
                        }

                        lineTranslados.Add(traslado);
                    }
                    else
                    {
                        Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion retencion = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestosRetencion()
                        {
                            Impuesto = "002",
                            TipoFactor = (lineTotal.VATPercent == 0 ? "Exento" : "Tasa"),
                            TasaOCuota = (lineTotal.VATPercent != null ? decimal.Parse(StringHelper.StringPadRight((Math.Abs(lineTotal.VATPercent.Value / 100).ToString()), '0', 8)) : 0),//(lineTotal.VatTypePercentage != null ? (decimal)(Math.Abs(lineTotal.VatTypePercentage.Value / 100)) : 0),
                            Importe = (decimal)lineTotal.InvoiceCurrencyVATAmount,//GetDecimalWith2DigitsAfterPoint((decimal)MethodHelper.Roundd(Math.Abs(lineTotal.InvoiceCurrencyVATAmount != null ? lineTotal.InvoiceCurrencyVATAmount.Value : 0), 2)),//Math.Abs(((lineTotal.InvoiceCurrencyAmount != null ? ((decimal)lineTotal.InvoiceCurrencyAmount.Value) : 0) * ((lineTotal.VATPercent != null ? (decimal)lineTotal.VATPercent.Value : 0) / 100))),
                        };

                        if (lineVatType.Code != "EXMPT")
                        {
                            retencion.Base = GetDecimalWith2DigitsAfterPoint(Math.Abs((lineTotal.InvoiceCurrencyVatableAmount != null ? (decimal)lineTotal.InvoiceCurrencyVatableAmount.Value : 0)));//retencion.Importe;
                        }

                        lineRetencions.Add(retencion);
                    }
                }



            }
            // concepto.
            concepto.Impuestos = new Profact.TimbraCFDI33.ComprobanteConceptoImpuestos();

            if (lineTranslados.Count > 0)
            {
                concepto.Impuestos.Traslados = lineTranslados.ToArray();

            }
            if (lineRetencions.Count > 0)
            {
                concepto.Impuestos.Retenciones = lineRetencions.ToArray();
            }



        }


        private static void BuildRelatedInvoiceTag(ARInvoicePM entityPM, IInvoiceContext invoiceCotnext, Profact.TimbraCFDI33.Comprobante comprobante)
        {
            string relatedInvoiceUUID = "";
            string tipoRelacion = "";
            ARInvoice relatedInvoice = null;
            if (entityPM.IsAutoCredit)
            {
                relatedInvoice = (from a in invoiceCotnext.ARInvoices
                                  where a.Id == entityPM.CreditedByARInvoiceId && a.Tenant == entityPM.Tenant
                                  select a).FirstOrDefault();
            }
            else if (!string.IsNullOrEmpty(entityPM.RelatedInvoice))
            {
                relatedInvoice = (from a in invoiceCotnext.ARInvoices
                                  where a.InvoiceNumber == entityPM.RelatedInvoice && a.Tenant == entityPM.Tenant
                                  select a).FirstOrDefault();
            }

            if (relatedInvoice != null && relatedInvoice.SATTransferStatusCode == "TD" && relatedInvoice.StatusCode == "VD")
            {
                return;
            }

            if (relatedInvoice != null && !string.IsNullOrEmpty(relatedInvoice.SATXML))
            {
                Profact.TimbraCFDI33.Comprobante oldComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(relatedInvoice.SATXML);

                if (oldComprobante.Complemento.Any != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = oldComprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        relatedInvoiceUUID = digitalTi.UUID.Trim();
                    }
                }
            }

            if (entityPM.ARInvoiceTypeCode == "CD")
            {
                tipoRelacion = "01";
            }
            else
            {
                List<ARInvoice> shipmentInvoices = (from a in invoiceCotnext.ARInvoiceEntities
                                                    where a.EntityId == entityPM.MainEntityId && a.ARInvoiceId != entityPM.Id && a.Tenant == entityPM.Tenant
                                                    select a.ARInvoice).ToList();

                if (shipmentInvoices != null)
                {
                    if (shipmentInvoices.Any(a => a.IsAutoCredit))
                    {
                        tipoRelacion = "04";
                    }
                    else if (shipmentInvoices.Count >= 1)
                    {
                        tipoRelacion = "02";
                    }
                }

            }

            if (!string.IsNullOrEmpty(relatedInvoiceUUID))
            {
                comprobante.CfdiRelacionados = new Profact.TimbraCFDI33.ComprobanteCfdiRelacionados();
                comprobante.CfdiRelacionados.TipoRelacion = tipoRelacion;
                comprobante.CfdiRelacionados.CfdiRelacionado = new List<Profact.TimbraCFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado>()
                        {
                            new Profact.TimbraCFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado() {UUID = relatedInvoiceUUID },
                        }.ToArray();
            }
        }




        public void HandleInvoiceSATCancellation(ARInvoicePM entityPM, ARInvoice entityPoco)
        {
            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            if (satSetting != null)
            {
                switch (satSetting.SATInterfaceCode)
                {
                    case "PROF":
                    case "PROF33":
                        HandleProfactInvoiceCancellation(entityPM, entityPoco);
                        break;
                    case "CONT":
                        //SendContpaqCancellation(entityPM);
                        break;

                }

            }
        }


        private void HandleProfactInvoiceCancellation(ARInvoicePM entityPM, ARInvoice entityPoco)
        {
            if (entityPoco.SATTransferStatusCode == "TG")
                throw new ApplicationException("You are not allowed to void the invoice while its status is Transferring to SAT");

            if (!string.IsNullOrEmpty(entityPoco.SATXML))
            {
                SendARInvoiceSATCancellationRequest(entityPM, entityPoco);
                entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "TG";
            }
            else
            {
                entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "ND";
                //entityPoco.TransmissionError = entityPM.TransmissionError = null;
            }
        }

        private void SendARInvoiceSATCancellationRequest(ARInvoicePM entityPM, ARInvoice entityPoco)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] profactoXMLData = encoding.GetBytes(entityPoco.SATXML);

            Profact.TimbraCFDI33.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(profactoXMLData);

            this.BuildProfactCommunicationLog33(comprobante, entityPM.Tenant, entityPM.Id, entityPM.InvoiceNumber.ToString(), true);
        }

        private void BuildProfactCommunicationLog33(Profact.TimbraCFDI33.Comprobante comprobante, int tenant, string entityId, string entityReference, bool isCancellation = false, bool isPayment = false)
        {
            byte[] profactoXmlData = { };


            string logSubject = "SAT Interface";
            if (isCancellation)
            {
                logSubject = "SAT Interface Cancellation Request";
                if (isPayment)
                {
                    logSubject = "Payment SAT Interface Cancellation";
                }

                if (comprobante.Complemento.Any != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);

                        //Rfc Emisor
                        string rfcEmisor = comprobante.Emisor.Rfc.Trim();

                        //Folio Fiscal - UUID
                        string folioFiscal = digitalTi.UUID.Trim();

                        SATCancellation cancellatio = new SATCancellation() { rfcEmisor = rfcEmisor, folioFiscal = folioFiscal };
                        profactoXmlData = LogitudeXmlSerializer.SerializeObject<SATCancellation>(cancellatio);
                    }
                }
            }
            else
            {
                if (isPayment)
                {
                    logSubject = "Payment SAT Interface";
                }

                profactoXmlData = LogitudeXmlSerializer.SerializeObject<Profact.TimbraCFDI33.Comprobante>(comprobante);
            }



            string logFolder = "SATInterface";

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            TenantRepository tenantRepository = new TenantRepository(commonContext);

            Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);

            ObjectTableRepository myObjectTabelRepository = new ObjectTableRepository(tenant);
            string tableName = "ARInvoice";
            if (isPayment)
                tableName = "ARPayment";

            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(tableName, 0, true);
            string myObjectTableId = null;
            if (objectTable != null)
            {
                myObjectTableId = objectTable.Id;
            }


            //ContactRepository contactRepository = new ContactRepository(commonContext);
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(tenant);//contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);

            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);

            Document document = new Document()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Extension = "xml",
                FileSize = profactoXmlData.Length,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder = logFolder.ToLower(),
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = "Profact 3.3",
                InOut = "O",
                From = currentTenant.Company,
                EntityId = entityId,
                ObjectTableId = myObjectTableId,
                Subject = logSubject,
                Tenant = tenant,
                CommunicationLogTypeCode = "T",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = loggedContact.Id,
                DocumentId = document.Id,
                EntityReference = entityReference,
                SearchFields = entityReference + "," + logFolder + "," + "O" + "," + logSubject,
                CreateDateUTC = DateTime.UtcNow,
                QueueName = "SATInterface",
            };


            communicationLogRepository.Add(commLog);
            communicationLogRepository.SubmitChanges();

            string filename = document.Id + "." + document.Extension;
            string filePath = "tenant" + commLog.Tenant + "/" + Simplog.Server.Infrastructure.Azure.StorageAcountDetails.GetBlobNameByLocation(filename, document.Folder);
            Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Server.Tools.StorageService.IBlobService), "StorageService", new Microsoft.Practices.Unity.ParameterOverride("", 1)) as Server.Tools.StorageService.IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = document.Id,
                FolderName = document.Folder,
                Extension = document.Extension,
                Tenant = tenant,
                FileSize = profactoXmlData.Length,

            };

            storageservice.Write(profactoXmlData, fileInfo);

            DbQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("SATInterface", 0);
            Dictionary<string, string> param = new Dictionary<string, string>() { { "CommunicationLogId", commLog.Id }, { "Tenant", tenant.ToString() }, { "CancellationRequest", isCancellation.ToString() } };
            queueservice.Send(param, tenant);

            if (!isCancellation)
            {
                string eventTypeCode = "INTS";
                if (isPayment)
                {
                    eventTypeCode = "PATS";
                }
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    EntityId = entityId,
                    ObjectTableName = tableName,
                    Tenant = tenant,
                    UserId = loggedContact.Id,
                    EventTypeCode = eventTypeCode,
                });
            }
        }



        private List<ARInvoiceTotalVATPM> CalculateNoneExpenseTotalVats(ARInvoicePM entityPM,
            List<VatType> allVatTypes, List<VatTypePercentagePM> allVatPercentages, List<VATTypesGroup> allVatGroups, List<ChargesType> chargesTypes)
        {
            int tenant = entityPM.Tenant;

            List<ARInvoiceTotalVATPM> totalNoneExpenseVats = new List<ARInvoiceTotalVATPM>();
            List<ARInvoiceLinePM> myDataLines = entityPM.InvoiceLines.Where(d => !chargesTypes.First(c => c.Id == d.ChargesTypeId).IsExpense && d.VatTypeId != null).ToList();
            if (myDataLines.Count > 0)
            {

                List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();

                foreach (ARInvoiceLinePM item in myDataLines)
                {
                    #region
                    VatType lineVatType = allVatTypes.Where(d => d.Id == item.VatTypeId).FirstOrDefault();

                    if (lineVatType != null)
                    {
                        if (!lineVatType.IsMultiPercentage)
                        {
                            InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                            {
                                Id = item.VatTypeId,
                                VatTypeId = item.VatTypeId,
                                VatTypePercentage = item.VatPercentage,
                                LocalCurrencyAmount = item.LocalCurrencyAmount,
                                InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                ExternalVatCard = lineVatType.ReceivablesExternalId,
                                ExternalTAXItemId = lineVatType.ExternalTAXItemId,
                            };

                            group_Source.Add(newItem);
                        }

                        else
                        {
                            List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == item.VatTypeId).ToList();
                            foreach (VATTypesGroup itemGroup in myVatGroups)
                            {
                                InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                {
                                    Id = itemGroup.SingleVATTypeId,
                                    VatTypeId = itemGroup.SingleVATTypeId,
                                    LocalCurrencyAmount = item.LocalCurrencyAmount,
                                    InvoiceCurrencyAmount = item.InvoiceCurrencyAmount,
                                    ProfitCurrencyAmount = item.ProfitCurrencyAmount,
                                };

                                VatType vatType = allVatTypes.Where(d => d.Id == itemGroup.SingleVATTypeId).FirstOrDefault();
                                if (vatType != null)
                                {
                                    newItem.ExternalVatCard = vatType.ReceivablesExternalId;
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

                List<InvoiceTotalsClass> group_data
                    = (from items in group_Source
                       group items by new { items.VatTypeId, items.VatTypePercentage, items.ExternalVatCard, items.ExternalTAXItemId } into g
                       select new InvoiceTotalsClass()
                       {
                           Id = g.Key.VatTypeId,
                           VatTypeId = g.Key.VatTypeId,
                           VatTypePercentage = g.Key.VatTypePercentage,
                           LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                           InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                           ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                           ExternalVatCard = g.Key.ExternalVatCard,
                           ExternalTAXItemId = g.Key.ExternalTAXItemId
                       }).ToList();


                double? localAmountTotal = 0;
                double? invoiceAmountTotal = 0;
                double? profitAmountTotal = 0;
                double? invoiceVatableAmountTotal = 0;
                foreach (InvoiceTotalsClass item in group_data)
                {
                    ARInvoiceTotalVATPM record = new ARInvoiceTotalVATPM()
                    {

                        Tenant = entityPM.Tenant,
                        ARInvoiceId = entityPM.Id,
                        VatTypeId = item.Id,
                        VATPercent = MethodHelper.Roundd(item.VatTypePercentage, 2),
                        LocalVatableAmount = MethodHelper.Roundd(item.LocalCurrencyAmount, 2),
                        InvoiceCurrencyVatableAmount = MethodHelper.Roundd(item.InvoiceCurrencyAmount, 2),
                        ProfitVatableAmount = MethodHelper.Round(item.ProfitCurrencyAmount, 2),
                        ExternalVATCard = item.ExternalVatCard,
                        ExternalTAXItemId = item.ExternalTAXItemId,
                    };

                    record.LocalVATAmount = MethodHelper.Round((record.LocalVatableAmount * record.VATPercent / 100), 2);
                    record.InvoiceCurrencyVATAmount = MethodHelper.Round((record.InvoiceCurrencyVatableAmount * record.VATPercent / 100), 2);
                    record.ProfitCurrencyVATAmount = MethodHelper.Round((record.ProfitVatableAmount * record.VATPercent / 100), 2);
                    totalNoneExpenseVats.Add(record);

                    localAmountTotal += record.LocalVATAmount;
                    invoiceAmountTotal += record.InvoiceCurrencyVATAmount;
                    profitAmountTotal += record.ProfitCurrencyVATAmount;

                    invoiceVatableAmountTotal += record.InvoiceCurrencyVatableAmount;
                }

                var subtotal = invoiceVatableAmountTotal;
                var total = invoiceVatableAmountTotal + invoiceAmountTotal;


                // comprobante.subTotal = (decimal)subtotal.Value;
                //comprobante.total = (decimal)total.Value; 



            }


            return totalNoneExpenseVats;
        }

        #endregion


        #region SendPaymentProfactoXML

        public void SendPaymentProfactoXML(string paymentId, int tenant)
        {

            ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            AddressRepository addressReposirory = new AddressRepository(commonContext);
            CardRepository cardRepository = new CardRepository(commonContext);
            BranchRepository branchRepository = new BranchRepository(commonContext);
            CurrencyRepository currencyRepository = new CurrencyRepository(commonContext);

            List<Currency> currencies = currencyRepository.GetCurrencies(tenant).ToList();
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM entityPM = paymentQuery.GetSinglePM(paymentId, tenant);
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(entityPM.Tenant);
            ARPaymentRepository arPaymentRepository = new ARPaymentRepository(entityPM.Tenant);
            ARPayment entityPoco = arPaymentRepository.GetSingleARPayment(paymentId, tenant);

            //SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
            //SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            //if (satSetting != null)
            //{
            //    if (satSetting.ActivationDate != null && entityPM.RegisterDate < satSetting.ActivationDate)
            //    {
            //        return;
            //    }
            //}

            if (entityPM.MetodoPagoCode == "PUE")
            {
                throw new Exception("You can't proceed with this operation. You are allowed to send to SAT only when the Metodo Pago value is PPD");
            }

            if (entityPM.SATTransferStatusCode == "TD" && (entityPM.StatusCode == "VD"))
            {
                throw new Exception("You can't send this payment to SAT because it was cancelled");
            }

            List<string> invoiceIds = entityPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();

            List<ARInvoice> paymentARInvoices = (from a in invoiceContext.ARInvoices
                                                 where a.Tenant == tenant && invoiceIds.Contains(a.Id)
                                                 select a).ToList();

            if (paymentARInvoices.Any(p => string.IsNullOrEmpty(p.SATXML)))
            {
                throw new Exception("Some Invoices are not approved from sat");
            }

            Currency paymentCurrency = currencies.FirstOrDefault(c => c.Id == entityPM.PaymentCurrencyId);
            Address branchAddress = null;
            Address mainAddress = null;
            Tenant currentTenant = tenantRepository.GetSingleTenant(entityPM.Tenant);
            if (!string.IsNullOrEmpty(entityPM.BranchId))
            {
                Branch branch = branchRepository.GetSingleBranch(entityPM.BranchId, entityPM.Tenant);
                if (!string.IsNullOrEmpty(branch.AddressId))
                {
                    branchAddress = addressReposirory.GetSingleAddress(branch.AddressId, branch.Tenant);
                }
            }


            Address billToAddress = null;
            if (!string.IsNullOrEmpty(entityPM.BillToAddressId))
            {
                billToAddress = addressReposirory.GetSingleAddress(entityPM.BillToAddressId, entityPM.Tenant);
            }
            else
                throw new ApplicationException("Bill to Address is required ");

            Card billToCard = cardRepository.GetSingleCard(entityPM.BillToId, entityPM.Tenant);

            List<SATPaymentMethod> allPaymentMethods = (from d in invoiceContext.SATPaymentMethods select d).ToList();

            if (string.IsNullOrEmpty(currentTenant.VatNumber))
            {
                throw new ApplicationException("Company Vat Number is required");
            }

            if (string.IsNullOrEmpty(billToCard.SATPaymentMethodCode))
            {
                throw new ApplicationException("Bill to Forma Pago is required");
            }

            if (string.IsNullOrEmpty(entityPM.SATPaymentMethodCode))
            {
                throw new ApplicationException("Forma Pago is required");
            }

            if (branchAddress != null)
            {

                if (string.IsNullOrEmpty(branchAddress.ZipCode))
                {
                    throw new ApplicationException("Branch Address ZipCode is required ");
                }

                mainAddress = branchAddress;
            }
            else
            {
                if (currentTenant.Address != null)
                {
                    if (string.IsNullOrEmpty(currentTenant.Address.ZipCode))
                    {
                        throw new ApplicationException("Company Address ZipCode is required ");
                    }

                    mainAddress = currentTenant.Address;
                }
                else
                    throw new ApplicationException("Company Address is required ");
            }

            string serie = "A";
            string folio = entityPM.PaymentNo;

            string counterPrefix = null;
            counterPrefix = TableCounter.GetCounterPrefix(entityPM.Tenant, "ARPT", "DR", null);
            if (!String.IsNullOrEmpty(counterPrefix))
            {
                serie = counterPrefix;

                if (!string.IsNullOrEmpty(entityPM.PaymentNo))
                {
                    if (entityPM.PaymentNo.Contains(counterPrefix))
                    {
                        folio = entityPM.PaymentNo.Remove(0, counterPrefix.Length);
                    }
                }
            }

            SATPaymentMethod satPaymentMethod = allPaymentMethods.Where(a => a.Code == entityPM.SATPaymentMethodCode).FirstOrDefault();
            Profact.TimbraCFDI33.Comprobante comprobante = new Profact.TimbraCFDI33.Comprobante();
            if (!string.IsNullOrEmpty(entityPM.SATXML))
            {
                Profact.TimbraCFDI33.Comprobante paymentComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(entityPM.SATXML);
                if (paymentComprobante.Complemento.Any != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = paymentComprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        comprobante.CfdiRelacionados = new Profact.TimbraCFDI33.ComprobanteCfdiRelacionados();
                        comprobante.CfdiRelacionados.TipoRelacion = "04";
                        comprobante.CfdiRelacionados.CfdiRelacionado = new List<Profact.TimbraCFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado>() { new Profact.TimbraCFDI33.ComprobanteCfdiRelacionadosCfdiRelacionado() { UUID = digitalTi.UUID } }.ToArray();
                    }
                }
            }

            comprobante.Total = 0;
            comprobante.Version = "3.3";
            comprobante.TipoDeComprobante = "P";
            comprobante.SubTotal = 0;
            comprobante.Serie = serie;
            comprobante.Moneda = "XXX";
            DateTime currentDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            comprobante.Fecha = currentDateTime;//entityPM.RegisterDate != null ? entityPM.RegisterDate.Value : DateTime.Now;
            comprobante.Fecha = new DateTime(comprobante.Fecha.Year, comprobante.Fecha.Month, comprobante.Fecha.Day, currentDateTime.Hour, currentDateTime.Minute, currentDateTime.Second);

            if (branchAddress != null && !string.IsNullOrEmpty(branchAddress.ZipCode))
            {
                comprobante.LugarExpedicion = branchAddress.ZipCode;
            }
            else
                comprobante.LugarExpedicion = currentTenant.Address.ZipCode;

            comprobante.Folio = folio;

            comprobante.Emisor = new Profact.TimbraCFDI33.ComprobanteEmisor();
            comprobante.Emisor.Rfc = currentTenant.VatNumber;
            comprobante.Emisor.Nombre = currentTenant.Company;
            comprobante.Emisor.RegimenFiscal = "601";


            string billToCountryCode = (billToAddress != null ? (billToAddress.Country != null ? billToAddress.Country.Code : null) : null);
            if (billToAddress.Country != null)
            {
                billToCountryCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(billToAddress.Country.Code, "G-Profact", "Country");
            }

            //Llena datos del receptor
            comprobante.Receptor = new Profact.TimbraCFDI33.ComprobanteReceptor();
            //comprobante.Receptor.Rfc = billToCard.VatNumber;
            if (billToCountryCode == "MEX" || billToCountryCode == "MX")
            {
                if (!string.IsNullOrEmpty(billToCard.VatNumber))
                    comprobante.Receptor.Rfc = billToCard.VatNumber;
                else
                    comprobante.Receptor.Rfc = "AAA010101AAA";
            }
            else
            {
                if (!string.IsNullOrEmpty(billToCard.SATForeignRFC))
                    comprobante.Receptor.Rfc = billToCard.SATForeignRFC;
                else
                    comprobante.Receptor.Rfc = "XEXX010101000";

                comprobante.Receptor.ResidenciaFiscal = billToCountryCode;
                comprobante.Receptor.ResidenciaFiscalSpecified = true;
            }
            comprobante.Receptor.Nombre = billToCard.EnglishName;
            comprobante.Receptor.UsoCFDI = "P01";


            List<Profact.TimbraCFDI33.ComprobanteConcepto> conceptosList = new List<Profact.TimbraCFDI33.ComprobanteConcepto>();
            Profact.TimbraCFDI33.ComprobanteConcepto concepto = new Profact.TimbraCFDI33.ComprobanteConcepto()
            {
                Cantidad = 1,
                Descripcion = "Pago",
                Importe = 0,
                ValorUnitario = 0,
                ClaveProdServ = "84111506",
                ClaveUnidad = "ACT",
            };

            conceptosList.Add(concepto);
            comprobante.Conceptos = conceptosList.ToArray();



            List<Profact.TimbraCFDI33.Complementos.Pagos10.PagosPago> pagosPagoList = new List<Profact.TimbraCFDI33.Complementos.Pagos10.PagosPago>();
            Profact.TimbraCFDI33.Complementos.Pagos10.Pagos pagos = new Profact.TimbraCFDI33.Complementos.Pagos10.Pagos();
            pagos.Version = "1.0";
            Profact.TimbraCFDI33.Complementos.Pagos10.PagosPago pagoItem = new Profact.TimbraCFDI33.Complementos.Pagos10.PagosPago();
            pagoItem.Monto = GetDecimalWith2DigitsAfterPoint(entityPM.AmountInPaymentCurrency != null ? (decimal)entityPM.AmountInPaymentCurrency.Value : 0);//paymentInvoice.ForeignAmount != null ? (decimal)paymentInvoice.ForeignAmount.Value : 0;
            pagoItem.MonedaP = paymentCurrency.Code;//invoiceCurrency.Code;//computingPartnerHelper.GetComputingPartnerCodeTranslation(entityPM.PaymentCurrencyCode, "G-Profact", "Currency");

            if (entityPM.SATPaymentMethodCode == "03" && entityPM.TipoCadenaPago == "01")
            {
                if (string.IsNullOrEmpty(entityPM.CertPago))
                    throw new ApplicationException("Cert Pago is required");

                if (string.IsNullOrEmpty(entityPM.SelloPago))
                    throw new ApplicationException("Sello Pago is required");

                if (string.IsNullOrEmpty(entityPM.CadPago))
                    throw new ApplicationException("Cad Pago is required");

                //Encoding encoding = Encoding.ASCII;
                //byte[] certPago = encoding.GetBytes(entityPM.CertPago);
                //byte[] selloPago = encoding.GetBytes(entityPM.SelloPago);

                pagoItem.TipoCadPagoSpecified = true;
                pagoItem.TipoCadPago = "01";
                pagoItem.CertPago = entityPM.CertPago;
                if (!string.IsNullOrEmpty(entityPM.CadPago))
                    pagoItem.CadPago = entityPM.CadPago.Replace("|", "&#124;");
                pagoItem.SelloPago = entityPM.SelloPago;

            }


            if (paymentCurrency.Code != "MXN")
            {
                pagoItem.TipoCambioP = (entityPM.PaymentCurrencyExchangeRate != null ? Convert.ToDecimal(entityPM.PaymentCurrencyExchangeRate.Value) : 0);
                pagoItem.TipoCambioPSpecified = true;
            }

            pagoItem.FormaDePagoP = entityPM.SATPaymentMethodCode;
            pagoItem.FechaPago = entityPM.RegisterDate.Value;

            TimeSpan time = new TimeSpan(12, 00, 00);
            DateTime resultdate = pagoItem.FechaPago.Date + time;
            pagoItem.FechaPago = resultdate;
            if (entityPM.FechaPago != null)// && FeatureToggleHelper.HasFeatureToggle("FPG", tenant))
            {
                pagoItem.FechaPago = entityPM.FechaPago.Value;
            }

            List<Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado> doctos = new List<Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado>();
            int number = 1;
            paymentARInvoices.ForEach(invoice =>
            {
                Profact.TimbraCFDI33.Comprobante invoiceComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(invoice.SATXML);

                List<ARInvoicePayment> allInvoicePayments = (from a in invoiceContext.ARInvoicePayments.Include("ARPayment")
                                                             where a.ARInvoiceId == invoice.Id && a.Tenant == invoice.Tenant
                                                             select a).ToList();

                ARPaymentInvoicePM paymentInvoice = entityPM.PaymentInvoices.First(p => p.ARInvoiceId == invoice.Id);
                Currency invoiceCurrency = currencies.FirstOrDefault(c => c.Id == invoice.InvoiceCurrencyId);

                Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado doctoItem = new Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado();
                doctoItem.MonedaDR = invoiceCurrency.Code;//computingPartnerHelper.GetComputingPartnerCodeTranslation(invoiceCurrency.Code, "G-Profact", "Currency");//

                ARInvoicePayment currentARInvoicePayment = allInvoicePayments.FirstOrDefault(p => p.ARPaymentId == entityPM.Id);
                decimal previouslySentPaymentsTotal = 0;
                decimal invoiceAmount = (decimal)invoiceComprobante.Total;//invoice.AmountInInvoiceCurrency.Value; // 
                decimal currentPaymentAmount = (decimal)currentARInvoicePayment.ForeignAmount;

                if (entityPM.AmountInPaymentCurrency == invoice.AmountInInvoiceCurrency) // one payment
                {
                    //doctoItem.MetodoDePagoDR = "PUE";
                    doctoItem.NumParcialidad = "1";
                }
                else
                {
                    List<ARInvoicePayment> sentInvoicePayments = allInvoicePayments.Where(a => a.ARPayment.SATXML != null && a.ARPayment.SATTransferStatusCode == "TD" && a.ARPaymentId != entityPM.Id).ToList();
                    if (sentInvoicePayments.Count == 0) // first payment
                    {
                        //doctoItem.MetodoDePagoDR = "PPD";
                        doctoItem.NumParcialidad = "1";
                    }
                    else
                    {
                        sentInvoicePayments.ForEach(p =>
                        {
                            previouslySentPaymentsTotal += (decimal)(p.ForeignAmount.Value);

                        });

                        // doctoItem.MetodoDePagoDR = "PPD";
                        doctoItem.NumParcialidad = (sentInvoicePayments.Count() + 1).ToString();
                    }

                }

                 
                decimal imSaldoAnt = (invoiceAmount - previouslySentPaymentsTotal);
                doctoItem.ImpSaldoAnt = GetDecimalWith2DigitsAfterPoint((invoiceAmount - previouslySentPaymentsTotal)); // previous amount (not sent to profact amount)
                doctoItem.ImpSaldoAntSpecified = true;

                doctoItem.ImpPagado = GetDecimalWith2DigitsAfterPoint(currentPaymentAmount); // current amount to pay
                doctoItem.ImpPagadoSpecified = true;
                if (doctoItem.ImpSaldoAnt != 0)
                {
                    doctoItem.ImpSaldoInsoluto = GetDecimalWith2DigitsAfterPoint(doctoItem.ImpSaldoAnt - doctoItem.ImpPagado); // remaining after the current payment
                    doctoItem.ImpSaldoInsolutoSpecified = true;
                }

                if (paymentInvoice.ExchangeRate != null && invoiceCurrency.Code != paymentCurrency.Code)
                {
                    if (paymentCurrency.Code == "MXN" || paymentCurrency.Code == "MX")
                    {
                        decimal tipoCambioDR = 1 / (decimal)paymentInvoice.ExchangeRate.Value;
                        doctoItem.TipoCambioDR = GetDecimalWith6DigitsAfterPoint((tipoCambioDR)) + decimal.Parse("0.000001");
                    }
                    else
                        doctoItem.TipoCambioDR = (decimal)paymentInvoice.ExchangeRate.Value;

                    doctoItem.TipoCambioDRSpecified = true;
                }


                if (invoiceComprobante.Complemento.Any != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = invoiceComprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        doctoItem.IdDocumento = digitalTi.UUID;
                    }
                }

                doctoItem.Serie = invoiceComprobante.Serie;
                doctoItem.Folio = invoiceComprobante.Folio;
                doctoItem.MetodoDePagoDR = invoiceComprobante.MetodoPago;

                doctos.Add(doctoItem);

                number++;
            });

            if ((entityPM.AccountingPaymentMethodCode == "CC" || entityPM.AccountingPaymentMethodCode == "BT"
               || entityPM.AccountingPaymentMethodCode == "CH")
               && !string.IsNullOrEmpty(entityPM.ChequeOrPaymentRef))
            {
                pagoItem.NumOperacion = StringHelper.TruncateLongString(entityPM.ChequeOrPaymentRef, 100);
            }
            // 

            pagoItem.DoctoRelacionado = doctos.ToArray();
            pagosPagoList.Add(pagoItem);


            pagos.Pago = pagosPagoList.ToArray();

            List<XmlElement> LXmlComplementos = new List<XmlElement>();
            System.Xml.Serialization.XmlSerializerNamespaces nsPagos = new System.Xml.Serialization.XmlSerializerNamespaces();
            nsPagos.Add("pago10", "http://www.sat.gob.mx/Pagos");
            string xmlPagos = Profact.TimbraCFDI.XMLUtilerias.SerializaObjeto(pagos, typeof(Profact.TimbraCFDI33.Complementos.Pagos10.Pagos), nsPagos);
            XmlDocument docNominas = new XmlDocument();
            docNominas.LoadXml(xmlPagos);
            comprobante.PagosSpecified = true;
            LXmlComplementos.Add(docNominas.DocumentElement);

            comprobante.Complemento = new Profact.TimbraCFDI33.ComprobanteComplemento();
            comprobante.Complemento.Any = LXmlComplementos.ToArray<XmlElement>();

            this.BuildProfactCommunicationLog33(comprobante, entityPM.Tenant, entityPM.Id, entityPM.PaymentNo.ToString(), false, true);

            entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "TG";
            arPaymentRepository.Update(entityPoco);
            arPaymentRepository.SubmitChanges();


        }

        public void HandlePaymentSATCancellation(ARPaymentPM entityPM, ARPayment entityPoco)
        {
            SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(entityPM.Tenant);
            SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(entityPM.Tenant);
            if (satSetting != null)
            {
                switch (satSetting.SATInterfaceCode)
                {
                    case "PROF":
                    case "PROF33":
                        HandleProfactPaymentCancellation(entityPM, entityPoco);
                        break;
                    case "CONT":
                        //SendContpaqCancellation(entityPM);
                        break;

                }

            }
        }

        private void HandleProfactPaymentCancellation(ARPaymentPM entityPM, ARPayment entityPoco)
        {
            if (!string.IsNullOrEmpty(entityPoco.SATXML))
            {
                Encoding encoding = Encoding.UTF8;
                byte[] profactoXMLData = encoding.GetBytes(entityPoco.SATXML);

                Profact.TimbraCFDI33.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(profactoXMLData);

                this.BuildProfactCommunicationLog33(comprobante, entityPM.Tenant, entityPM.Id, entityPM.PaymentNo.ToString(), true, true);
                entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "TG";
            }
            else
            {
                entityPoco.SATTransferStatusCode = entityPM.SATTransferStatusCode = "ND";
                //entityPoco.TransmissionError = entityPM.TransmissionError = null;
            }
        }


        public static Profact.TimbraCFDI.ResultadoConsultaEstatusSAT GetSATStatus(int tenant, string entitySATXML)
        {
            Profact.TimbraCFDI.ResultadoConsultaEstatusSAT resultadoConsultaEstatusSAT = null;
            Profact.TimbraCFDI33.Conector conector = GetProfactConnector(tenant);
            Profact.TimbraCFDI33.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(entitySATXML);
            if (comprobante.Complemento.Any != null)
            {
                List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                if (timbreFiscalDigitalElement != null)
                {
                    Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                    string rfcEmisor = comprobante.Emisor.Rfc.Trim();
                    //Folio Fiscal - UUID
                    string uuID = digitalTi.UUID.Trim();

                    resultadoConsultaEstatusSAT = conector.ConsultaEstatusSAT(uuID);//"43b5277e-c306-4c83-8a4d-b15af03b7804");

                }
            }

            return resultadoConsultaEstatusSAT;
        }

        public static Profact.TimbraCFDI33.Conector GetProfactConnector(int tenant)
        {
            Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository sATInterfaceSettingRepository = new Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository(tenant);
            Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);

            bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
            Profact.TimbraCFDI33.Conector conector = new Profact.TimbraCFDI33.Conector(isProduction);
            //Establecemos las credenciales para el permiso de conexión
            conector.EstableceCredenciales(satSetting.Token);

            return conector;
        }


        public XmlElement Serialize(object target)
        {

            XmlDocument document = new XmlDocument();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            ns.Add("", "");

            XmlElement returnVal;

            XmlSerializer serializer = new XmlSerializer(target.GetType());

            MemoryStream ms = new MemoryStream();

            XmlTextWriter tw = new XmlTextWriter(ms, UTF8Encoding.UTF8);

            XmlDocument doc = new XmlDocument();

            tw.Formatting = Formatting.Indented;

            tw.IndentChar = ' ';

            serializer.Serialize(tw, target, ns);

            ms.Seek(0, SeekOrigin.Begin);

            doc.Load(ms);

            returnVal = document.ImportNode(doc.DocumentElement, true) as XmlElement;

            return returnVal;

        }

        #endregion

        public void UpdatePaymentInvoicesSATStatus(ARPayment payment, Profact.TimbraCFDI33.Comprobante paymentComprobante, ARInvoiceRepository arinvoiceRep, ARPaymentRepository arpaymentRep)
        {
            int tenant = payment.Tenant;
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM entityPM = paymentQuery.GetSinglePM(payment.Id, tenant);

            List<string> invoiceIds = entityPM.PaymentInvoices.Select(f => f.ARInvoiceId).ToList();

            List<ARInvoice> currentPaymentARInvoices = (from a in arinvoiceRep.context.ARInvoices
                                                        where a.Tenant == tenant && invoiceIds.Contains(a.Id) && a.SATXML != null
                                                        select a).ToList();

            List<ARInvoiceSATDetails> currentPaymentInvoiceDetails = GetCurrentPaymentInvoicesDetails(currentPaymentARInvoices);
            XmlElement xmlPagos = paymentComprobante.Complemento.Any[0];
            Profact.TimbraCFDI33.Complementos.Pagos10.Pagos pagos = Profact.TimbraCFDI.XMLUtilerias.DeserializaObjeto<Profact.TimbraCFDI33.Complementos.Pagos10.Pagos>(xmlPagos.OuterXml);
            Profact.TimbraCFDI33.Complementos.Pagos10.PagosPago pagoItem = pagos.Pago[0];
            List<Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado> doctos = pagoItem.DoctoRelacionado.ToList();
            foreach (Profact.TimbraCFDI33.Complementos.Pagos10.PagosPagoDoctoRelacionado doctoItem in doctos)
            {
                ARInvoiceSATDetails relatedInvoice = currentPaymentInvoiceDetails.FirstOrDefault(d => d.UUID == doctoItem.IdDocumento);
                if (relatedInvoice != null)
                {
                    List<ARInvoicePayment> sentApprovedInvoicePayments = (from a in arinvoiceRep.context.ARInvoicePayments
                                                                          where a.ARInvoiceId == relatedInvoice.Invoice.Id && a.Tenant == relatedInvoice.Invoice.Tenant && !string.IsNullOrEmpty(a.ARPayment.SATXML)
                                                                          && a.ARPayment.SATTransferStatusCode == "TD" && (a.ARPayment.StatusCode == "AD" || a.ARPayment.StatusCode == "CL")
                                                                          select a).ToList();

                    List<ARInvoicePayment> sentVoidedInvoicePayments = (from a in arinvoiceRep.context.ARInvoicePayments
                                                                        where a.ARInvoiceId == relatedInvoice.Invoice.Id && a.Tenant == relatedInvoice.Invoice.Tenant && !string.IsNullOrEmpty(a.ARPayment.SATXML)
                                                                        && a.ARPayment.SATTransferStatusCode == "TD" && (a.ARPayment.StatusCode == "VD" || a.ARPayment.StatusCode == "DR" || a.ARPayment.StatusCode == "AC")
                                                                        select a).ToList();

                    double previouslySentPaymentsTotal = 0;
                    sentApprovedInvoicePayments.ForEach(p =>
                    {
                        previouslySentPaymentsTotal += (p.ForeignAmount.Value);

                    });

                    double previouslyCanceledSentPaymentsTotal = 0;
                    sentVoidedInvoicePayments.ForEach(p =>
                    {
                        previouslyCanceledSentPaymentsTotal += (p.ForeignAmount.Value);

                    });

                    if (previouslySentPaymentsTotal != 0)
                    {
                        if (relatedInvoice.Invoice.AmountInInvoiceCurrency.Value == previouslySentPaymentsTotal)
                        {
                            relatedInvoice.Invoice.SATInvoiceStatusCode = "PD";
                        }
                        else
                        {
                            relatedInvoice.Invoice.SATInvoiceStatusCode = "PP";
                        }
                    }
                    else
                    {
                        relatedInvoice.Invoice.SATInvoiceStatusCode = "OP";
                    }

                    arinvoiceRep.Update(relatedInvoice.Invoice);
                }
            }

            arinvoiceRep.SubmitChanges();
        }

        private List<ARInvoiceSATDetails> GetCurrentPaymentInvoicesDetails(List<ARInvoice> currentPaymentARInvoices)
        {
            List<ARInvoiceSATDetails> paymentInvoicesDetails = new List<ARInvoiceSATDetails>();
            foreach (ARInvoice invoice in currentPaymentARInvoices)
            {
                Profact.TimbraCFDI33.Comprobante invoiceComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(invoice.SATXML);

                if (invoiceComprobante.Complemento.Any != null)
                {
                    List<System.Xml.XmlElement> myLXmlComplementos = invoiceComprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
                    var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
                    if (timbreFiscalDigitalElement != null)
                    {
                        Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);
                        paymentInvoicesDetails.Add(new ARInvoiceSATDetails() { UUID = digitalTi.UUID, Invoice = invoice });
                    }
                }
            }



            return paymentInvoicesDetails;
        }


        private decimal GetDecimalWith3DigitsAfterPointIfZero(decimal dNumber)
        {
            decimal result = decimal.Parse(dNumber.ToString("0.00"));
            if (result == 0 && dNumber != 0)
                result = decimal.Parse(dNumber.ToString("0.000"));
            return result;
        }
        private decimal GetDecimalWith2DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.00"));
        }

        private decimal GetDecimalWith6DigitsAfterPoint(decimal dNumber)
        {
            return decimal.Parse(dNumber.ToString("0.000000"));
        }

    }

    public class SATCancellation
    {
        public string rfcEmisor { get; set; }
        public string folioFiscal { get; set; }
    }

    public class ARInvoiceSATDetails
    {
        public string UUID { get; set; }
        public ARInvoice Invoice { get; set; }
    }



}
