using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for ShipmentCMRWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ShipmentCMRWebService : System.Web.Services.WebService
    { 
        [WebMethod]
        public byte[] GetDeliveryData(string entityId, int tenant, string userId, string documentTypeCopyId)
        {
            CMRDataProvider cmrDataProvider = GetDeliveryDataProvider(entityId, tenant, userId, documentTypeCopyId);
            XmlSerializer serializer = new XmlSerializer(typeof(CMRDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, cmrDataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public CMRDataProvider GetDeliveryDataProvider(string entityId, int tenant, string userId, string documentTypeCopyId)
        {
            CMRDataProvider cmrDataProvider = new CMRDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ShipmentRepository shipmentRepositoroy = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepositoroy);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(tenant);

            ShipmentPM shipment = null;
            shipment = shipmentQuery.GetSinglePM(entityId, tenant);

            Tenant tenantSettings = (from a in commonContext.Tenants
                                     where a.Id == tenant
                                     select a).FirstOrDefault();

            User currentUser = (from a in commonContext.Users
                                where a.Id == userId
                                select a).FirstOrDefault();

            string loggedUserEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
            if (currentUser == null)
            {
                if (tenant != 0)
                {
                    currentUser = (from a in commonContext.Users
                                   where a.Contact.Email == loggedUserEmail && a.Tenant == tenant
                                   select a).FirstOrDefault();
                }
                else
                {
                    currentUser = (from a in commonContext.Users
                                   where a.Contact.Email == loggedUserEmail && a.Tenant == 0
                                   select a).FirstOrDefault();
                }
            }

            if (currentUser == null && tenant != 0)
            {
                currentUser = (from a in commonContext.Users
                               where a.Contact.Email == loggedUserEmail && a.Tenant == 0
                               select a).FirstOrDefault();
            }

            if (shipment != null && tenantSettings != null)
            {
                //------------partners-------------------------//
                Card shipperClient = (from a in commonContext.Cards
                                      where a.Id == shipment.ShipperId
                                      select a).FirstOrDefault();

                if (shipment.ConsigneeId != null)
                {
                    Card consigneeClientAbroad = (from a in commonContext.Cards
                                                  where a.Id == shipment.ConsigneeId
                                                  select a).FirstOrDefault();

                    cmrDataProvider.ConsigneeName = consigneeClientAbroad.EnglishName;

                    Address consigneePickupAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(shipment.ConsigneeId, "P", tenant);
                    if (consigneePickupAddress != null)
                    {
                        cmrDataProvider.ConsigneePickUpAddress = DataProviders.General.GetAddress(consigneePickupAddress);
                    }
                    else
                    {
                        Address consigneeMainAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(shipment.ConsigneeId, "M", tenant);
                        if (consigneeMainAddress != null)
                        {
                            cmrDataProvider.ConsigneePickUpAddress = DataProviders.General.GetAddress(consigneeMainAddress);
                        }
                    }
                }

                cmrDataProvider.ShipperName = shipperClient != null ? shipperClient.EnglishName : "";

                if (shipment.ShipperAddressId != null)
                {
                    Address shipperClientAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);


                    if (shipperClientAddress != null)
                    {
                        if (shipperClientAddress.IsLocalLanguage)
                        {
                            if (shipperClient != null && !string.IsNullOrEmpty(shipperClient.LocalName))
                            {
                                cmrDataProvider.ShipperName = shipperClient != null ? shipperClient.LocalName : "";
                            }
                        }

                        cmrDataProvider.ShipperAddress = cmrDataProvider.ShipperAddress + DataProviders.General.GetAddress(shipperClientAddress);
                    }
                }

                if (shipment.ConsigneeAddressId != null)
                {
                    Address consigneeClientAbroadAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);

                    if (consigneeClientAbroadAddress != null)
                    {
                        cmrDataProvider.ConsigneeAddress = DataProviders.General.GetAddress(consigneeClientAbroadAddress);
                    }
                }

                if (tenantSettings.AddressId != null)
                {

                    cmrDataProvider.CompanyName = tenantSettings.Company;
                    Address tenantAddress = addressRepository.GetSingleAddress(tenantSettings.AddressId, tenant);



                    if (tenantAddress != null)
                    {
                        cmrDataProvider.TenantAddress = DataProviders.General.GetAddress(tenantAddress);
                    }
                }

                //*************** General Info **********************

                //************* pickup from ****************
                cmrDataProvider.PickUpAddressAndDate = cmrDataProvider.ShipperAddress != null ? cmrDataProvider.ShipperAddress : "";//shipmentdelivery.FromAddress != null ? shipmentdelivery.FromAddress : "";
                cmrDataProvider.Note = shipment.Notes != null ? shipment.Notes : "";

                //************* Delivery To ****************
                cmrDataProvider.DeliveryToAddress = cmrDataProvider.ConsigneeAddress != null ? cmrDataProvider.ConsigneeAddress : "";
                cmrDataProvider.ShipmentWeightUnit = shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "";
                cmrDataProvider.ShipmentVolumUnit = shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "";
                // Inland + Domestic
                if (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
                {
                    Address fromAddress = addressRepository.GetSingleAddress(shipment.MainCarriageFromAddressId, tenant);
                    Address toAddress = addressRepository.GetSingleAddress(shipment.MainCarriageToAddressId, tenant);


                    if (fromAddress != null)
                    {
                        cmrDataProvider.FromLocation = fromAddress.City + " " + (fromAddress.Country != null ? fromAddress.Country.Code : "");
                    }

                    if (toAddress != null)
                    {
                        cmrDataProvider.ToLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                    }
                }
                else
                {
                    PortQuery portQuery = new PortQuery(tenant);
                    PortPM toPort = portQuery.GetSinglePM(shipment.MainCarriageToPortId, tenant);
                    PortPM fromPort = portQuery.GetSinglePM(shipment.MainCarriageFromPortId, tenant);

                    cmrDataProvider.FromLocation = fromPort != null ? fromPort.Code + " " + fromPort.EnglishName : "";
                    cmrDataProvider.ToLocation = toPort != null ? toPort.Code + " " + toPort.EnglishName : "";
                }

                if (!string.IsNullOrEmpty(shipment.MoveTypeId))
                {
                    MoveType moveType = context.MoveTypes.Where(m => m.Id == shipment.MoveTypeId).FirstOrDefault();

                    if (moveType != null)
                    {
                        cmrDataProvider.MoveTypeCode = moveType.Code;
                        cmrDataProvider.MoveTypeName = moveType.MoveTypeEnglishName;
                    }
                }

                #region Packaegs

                cmrDataProvider.ContainersList = new List<ContainerData>();

                List<ShipmentPackage> packages = shipmentsContext.ShipmentPackages.Where(d => d.ShipmentId == shipment.Id && d.Tenant == tenant).ToList();
                int counter = 1;
                int numberofpackages = 0;
                cmrDataProvider.HsCode = "";
                double? totalWeight = 0;
                double? totalVolume = 0;

                foreach (ShipmentPackage package in packages)
                {
                    ContainerData containerRecord = new ContainerData();
                    counter++;
                    List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => d.ShipmentPackageId == package.Id && d.Tenant == package.Tenant).ToList();

                    PackageType packtype = (from pa in commonContext.PackageTypes
                                            where pa.Id == package.PackageTypeId
                                            select pa).FirstOrDefault();

                    numberofpackages += package.Quantity.Value;
                    totalWeight += (package.Weight != null ? package.Weight : 0);
                    totalVolume += (package.Volume != null ? package.Volume : 0);

                    cmrDataProvider.HsCode = cmrDataProvider.HsCode + (package.Harmonize != null ? "," + package.Harmonize : "");

                    containerRecord.HsCode = package.Harmonize != null ? "," + package.Harmonize : "";
                    containerRecord.Weight = (package.Weight != null ? package.Weight.ToString() : "0") + "  " + cmrDataProvider.ShipmentWeightUnit;
                    containerRecord.Volume = (package.Volume != null ? package.Volume.ToString() : "0") + "  " + cmrDataProvider.ShipmentVolumUnit;
                    containerRecord.Description = package.Description != null ? package.Description : " ";
                    containerRecord.Pieces = package.Quantity != null ? package.Quantity : 0;
                    containerRecord.MarksAndNumbers_Direct = package.MarksAndNumbers;
                    if (packtype != null)
                    {
                        if (packtype.IsContainer)
                        {
                            containerRecord.MarksAndNumbers = "1 X " + packtype.PrintAs + Environment.NewLine + package.ContainerNumber;

                            if (!string.IsNullOrEmpty(package.ShipperSeal))
                            {
                                containerRecord.MarksAndNumbers = containerRecord.MarksAndNumbers + Environment.NewLine + "Shipper Seal: " + package.ShipperSeal;
                            }

                            containerRecord.MarksAndNumbers = containerRecord.MarksAndNumbers + Environment.NewLine + package.Description;
                        }

                        else
                        {
                            containerRecord.MarksAndNumbers = package.Quantity + " X " + packtype.EnglishName + Environment.NewLine + package.Description;
                        }
                    }

                    this.FillInsidePackagesList(insidePackages, containerRecord, commonContext);

                    cmrDataProvider.ContainersList.Add(containerRecord);
                }

                #endregion

                if (cmrDataProvider.HsCode != null)
                {
                    cmrDataProvider.HsCode = cmrDataProvider.HsCode.TrimStart(',');
                }

                cmrDataProvider.NumberOfPackages = numberofpackages.ToString();
                cmrDataProvider.WeightUnit = tenantSettings.GrossWeightUnitCode;
                cmrDataProvider.TotalWeight = totalWeight.ToString();
                cmrDataProvider.Volume = totalVolume.ToString();

                if (shipment.IncotermId != null)
                {
                    Incoterm incoterm = (from a in commonContext.Incoterms
                                         where a.Id == shipment.IncotermId
                                         select a).FirstOrDefault();
                    if (incoterm != null)
                    {
                        cmrDataProvider.SendersInstructions = cmrDataProvider.SendersInstructions + Environment.NewLine + "Incoterm: " + incoterm.Code + "-" + incoterm.Name;
                    }
                }

                cmrDataProvider.TruckNumberAndTrailerNumber = shipment.MainCarriageCarrierNumber;

                Branch branch = (from a in commonContext.Branches
                                 where a.Id == currentUser.BranchId
                                 select a).FirstOrDefault();
                if (branch != null)
                {
                    cmrDataProvider.IssueBranchAndDate = branch.EnglishName + "," + DateTime.Now.Date.ToString();
                    cmrDataProvider.IssueBranch = branch.EnglishName;
                }

                cmrDataProvider.IssueDate = DateTime.Now.Date;
                cmrDataProvider.PickUpOrDeliveryNumber = shipment.ShipmentNumber;


                // custom fields//
                List<FormCustomField> customfieldsList = commonContext.FormCustomFields.ToList();
                List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.ToList();

                //----cash on delivery---//
                FormCustomField cashOnDeliveryField = (from a in customfieldsList
                                                       where a.FieldCode == "CashOnDelivery" && a.EntityId == shipment.Id
                                                       select a).FirstOrDefault();
                DocumentTypeCustomField cashOnDeliveryDocumentCustom = (from a in documentCustomfieldsList
                                                                        where a.FieldCode == "CashOnDelivery"
                                                                        select a).FirstOrDefault();
                if (cashOnDeliveryField != null)
                {
                    cmrDataProvider.CashOnDelivery = cashOnDeliveryField.Value;
                }
                else if (cashOnDeliveryDocumentCustom != null)
                {
                    cmrDataProvider.CashOnDelivery = cashOnDeliveryDocumentCustom.DefaultValue;
                }

                //--Documents attached--//

                FormCustomField documentsAttachedField = (from a in customfieldsList
                                                          where a.FieldCode == "DocumentsAttached" && a.EntityId == shipment.Id
                                                          select a).FirstOrDefault();
                DocumentTypeCustomField documentsAttachedDocumentCustom = (from a in documentCustomfieldsList
                                                                           where a.FieldCode == "DocumentsAttached"
                                                                           select a).FirstOrDefault();
                if (documentsAttachedField != null)
                {
                    cmrDataProvider.DocumentsAttached = documentsAttachedField.Value;
                }
                else if (documentsAttachedDocumentCustom != null)
                {
                    cmrDataProvider.DocumentsAttached = documentsAttachedDocumentCustom.DefaultValue;
                }


                //----Instructions(13)---//
                FormCustomField instructions13Field = (from a in customfieldsList
                                                       where a.FieldCode == "Instructions(13)" && a.EntityId == shipment.Id
                                                       select a).FirstOrDefault();
                DocumentTypeCustomField instructions13DocumentCustom = (from a in documentCustomfieldsList
                                                                        where a.FieldCode == "Instructions(13)"
                                                                        select a).FirstOrDefault();
                if (instructions13Field != null)
                {
                    cmrDataProvider.Instructions_13 = instructions13Field.Value;
                }
                else if (instructions13DocumentCustom != null)
                {
                    cmrDataProvider.Instructions_13 = instructions13DocumentCustom.DefaultValue;
                }

                cmrDataProvider.SendersInstructions = cmrDataProvider.Instructions_13 != null ? cmrDataProvider.Instructions_13 : "";

                DocumentTypeCopy documenttypecopy = (from copy in commonContext.DocumentTypeCopies
                                                     where copy.Id == documentTypeCopyId
                                                     select copy).FirstOrDefault();
                if (documenttypecopy != null)
                {
                    string[] copyNameandNumber = new string[5];
                    copyNameandNumber = GetCopyNameAndNumber(documenttypecopy.Code);

                    cmrDataProvider.CopyNumber = copyNameandNumber[0];
                    cmrDataProvider.CopyNameFirstLanguage = copyNameandNumber[1];
                    cmrDataProvider.CopyNameSecondLanguage = copyNameandNumber[2];
                    cmrDataProvider.EnglishLanguage = copyNameandNumber[3];
                    cmrDataProvider.RussianLanguage = copyNameandNumber[4];
                }
            }
            //------------------------------------------------
            try
            {
                Type cmrType = cmrDataProvider.GetType();
                PropertyInfo[] properties = cmrType.GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    Type piType = pi.PropertyType;

                    if (piType.Name != "Double")
                    {
                        if (pi.GetValue(cmrDataProvider, null) == null || pi.GetValue(cmrDataProvider, null).ToString() == "0" || pi.GetValue(cmrDataProvider, null).ToString() == "00.00")
                        {
                            pi.SetValue(cmrDataProvider, "", null);
                        }

                    }
                }
            }
            catch { }

            return cmrDataProvider;
        }

        private void FillInsidePackagesList(List<InsideShipmentPackage> insidePackages, ContainerData containerRecord, ICommonDataContext commonContext)
        {
            containerRecord.InsidePackagesLines = new List<InsidePackageLine>();
            
            foreach (InsideShipmentPackage insideItem in insidePackages)
            {
                PackageType insidePackageType = (from pa in commonContext.PackageTypes
                                                 where pa.Id == insideItem.PackageTypeId
                                                 select pa).FirstOrDefault();

                InsidePackageLine insidePackage = new InsidePackageLine();

                insidePackage.PackageType = insidePackageType == null ? "" : insidePackageType.EnglishName;
                insidePackage.Quantity = insideItem.Quantity;

                if (insideItem.Length != null && insideItem.Width != null && insideItem.Height != null)
                {
                    insidePackage.Dimensions = insideItem.Length + "x" + insideItem.Width + "x" + insideItem.Height;
                }

                insidePackage.Volume = insideItem.Volume;
                insidePackage.VolumetricWeight = insideItem.VolumetricWeight;
                insidePackage.Weight = insideItem.Weight;
                insidePackage.Description = insideItem.Description;
                insidePackage.Reference1 = insideItem.Reference1;
                insidePackage.Reference2 = insideItem.Reference2;
                insidePackage.Reference3 = insideItem.Reference3;
                insidePackage.CommodityNumber = insideItem.CommodityNumber;
                containerRecord.InsidePackagesLines.Add(insidePackage);
            }
        }

        private string[] GetCopyNameAndNumber(string doccopycode)
        {
            string[] result = new string[5];

            switch (doccopycode)
            {
                case "SC":
                    result[0] = "1"; // copy number
                    result[1] = "Izvod za pošiljatelj"; // first language
                    result[2] = "exemplaire de l'expéditeur"; // second language
                    result[3] = "Copy for Sender";
                    result[4] = "Копия для Отправителя";
                    break;

                case "RC":
                    result[0] = "2";
                    result[1] = "Izvod za prejemnika";
                    result[2] = "Exemplaire du destinataire";
                    result[3] = "Copy for Consignee";
                    result[4] = "Копия для Получателя";
                    break;

                case "CC":
                    result[0] = "3";
                    result[1] = "Izvod za prevoznika";
                    result[2] = "Exemplaire du transporteur";
                    result[3] = "Copy for Carrier";
                    result[4] = "Копия для Перевозчика";
                    break;

                case "3A":
                    result[0] = "3A";
                    break;

                case "3B":
                    result[0] = "3B";
                    break;

                case "3C":
                    result[0] = "3C";
                    break;

                case "3D":
                    result[0] = "3D";
                    break;
            }
            return result;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
    }
}
