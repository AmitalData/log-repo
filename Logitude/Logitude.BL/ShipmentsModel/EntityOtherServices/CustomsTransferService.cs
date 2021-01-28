using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityOtherServices
{
    public class CustomsTransferService
    {
        private int tenant;
        private string fileName;
        private string transferType;
        private List<ShipmentDataView> shipments;
        public byte[] ComputedData = null;
        private System.IO.MemoryStream memory;
        private IWorkbook workbook;
        private IWorksheet sheet1;
        private DataTable dataTable;
        private ExcelEngine excelEngine;
        private ICommonDataContext commoContext;
        private IShipmentsContext shipmentsContext;
        private ShipmentPackageRepository shipmentPackageRepository;
        private ShippingLineRepository shippingLineRepository;
        private AddressRepository addressRepository;
        private CountryCityRepository countryCityRepository;
        private PortRepository PortRepository;
        public CustomsTransferService(List<ShipmentDataView> shipments, string filename, string type, int tenant)
        {
            this.tenant = tenant;
            this.fileName = filename;
            this.transferType = type;
            this.shipments = shipments;

            commoContext = CommonDataContext.GetContext(tenant);
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            shipmentPackageRepository = new ShipmentPackageRepository(shipmentsContext);
            shippingLineRepository = new ShippingLineRepository(commoContext);
            addressRepository = new AddressRepository(commoContext);
            countryCityRepository = new CountryCityRepository(commoContext);
            PortRepository = new PortRepository(commoContext);

            this.InitializeExcelFile();            
        }

        private void InitializeExcelFile()
        {
            memory = new System.IO.MemoryStream();
            excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            this.workbook = excelEngine.Excel.Workbooks.Create(1);
            workbook.Version = ExcelVersion.Excel2007;

            this.CreateAndDesignExcelSheet();
            this.CreateAndDesignDataTable();
        }
        private void CreateAndDesignExcelSheet()
        {
            sheet1 = workbook.Worksheets[0];

            switch (this.transferType)
            {
                case "AMOS":
                    {
                        sheet1.Range["A1:AX1"].CellStyle.Font.Bold = true;
                        sheet1.Range["A1:AX1"].CellStyle.Font.Size = 10;
                        sheet1.Range["A1:AX1"].CellStyle.Font.FontName = "Calibri";
                        sheet1.Range["A1:AX1"].CellStyle.Font.Color = ExcelKnownColors.Black;
                        sheet1.Range["A1:AX1"].CellStyle.Color = System.Drawing.Color.FromArgb(255, 242, 220, 219);
                        sheet1.Range["A1:AX1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        sheet1.Range["K1"].EntireColumn.IsStringsPreserved = true;
                        sheet1.Range["O1"].EntireColumn.IsStringsPreserved = true;
                        break;
                    }


                case "AMAS":
                    {
                        sheet1.Range["A1:AB1"].CellStyle.Font.Bold = true;
                        sheet1.Range["A1:AB1"].CellStyle.Font.Size = 10;
                        sheet1.Range["A1:AB1"].CellStyle.Font.FontName = "Calibri";
                        sheet1.Range["A1:AB1"].CellStyle.Font.Color = ExcelKnownColors.White;
                        sheet1.Range["A1:AB1"].CellStyle.Color = System.Drawing.Color.Orange;
                        sheet1.Range["A1:AB1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                        sheet1.Range["B1"].EntireColumn.IsStringsPreserved = true;
                        sheet1.Range["G1"].EntireColumn.IsStringsPreserved = true;
                        break;
                    }
            }
        }
        private void CreateAndDesignDataTable()
        {
            dataTable = new DataTable();

            switch (this.transferType)
            {
                case "AMOS":
                    {
                        dataTable.Columns.Add("Consecutive");
                        dataTable.Columns.Add("Unique_Number_of_Manifest");
                        dataTable.Columns.Add("Code_CAAT");
                        dataTable.Columns.Add("Kind_of_Movement");
                        dataTable.Columns.Add("Customs_Section");
                        dataTable.Columns.Add("Electronic_Acknowledgment_of_Recepction");
                        dataTable.Columns.Add("Key_Carrier");
                        dataTable.Columns.Add("Name_of_Vessel");
                        dataTable.Columns.Add("Number_of_Trip");
                        dataTable.Columns.Add("Type_of_Operation");
                        dataTable.Columns.Add("Number_Of_BL", typeof(string));
                        dataTable.Columns.Add("Port_of_Loading_Unloading");
                        dataTable.Columns.Add("Country_of_Port_of_Loading_Unloading");
                        dataTable.Columns.Add("Type_of_BL");
                        dataTable.Columns.Add("Number_of_BL_Reference", typeof(string));
                        dataTable.Columns.Add("Type_of_Port");
                        dataTable.Columns.Add("Key_of_port");
                        dataTable.Columns.Add("Country_of_Port");
                        dataTable.Columns.Add("Type_Figure1");
                        dataTable.Columns.Add("Name1");
                        dataTable.Columns.Add("Identification_Key_Fiscal1");
                        dataTable.Columns.Add("Place_of_Residence1");
                        dataTable.Columns.Add("Type_Figure2");
                        dataTable.Columns.Add("Name2");
                        dataTable.Columns.Add("Identification_Key_Fiscal2");
                        dataTable.Columns.Add("Place_of_residence2");
                        dataTable.Columns.Add("Type_Figure3");
                        dataTable.Columns.Add("Name3");
                        dataTable.Columns.Add("Identification_Key_Fiscal3");
                        dataTable.Columns.Add("Place_of_residence3");
                        dataTable.Columns.Add("Sequence1");
                        dataTable.Columns.Add("Type_of_Goods");
                        dataTable.Columns.Add("Type_of_packing");
                        dataTable.Columns.Add("Marks_and_numbers");
                        dataTable.Columns.Add("Total_Number_of_Pieces1");
                        dataTable.Columns.Add("Gross_weight_Volume");
                        dataTable.Columns.Add("Unit_of_measurement");
                        dataTable.Columns.Add("General_Description");
                        dataTable.Columns.Add("Sequence2");
                        dataTable.Columns.Add("Additional_information_of_Goods");
                        dataTable.Columns.Add("Class_Division_of_Goods");
                        dataTable.Columns.Add("Number_of_United_Nations");
                        dataTable.Columns.Add("Contact");
                        dataTable.Columns.Add("Name");
                        dataTable.Columns.Add("Sequence3");
                        dataTable.Columns.Add("Container_Number");
                        dataTable.Columns.Add("Container_Type");
                        dataTable.Columns.Add("Service_Type_Code");
                        dataTable.Columns.Add("Gross_Weight");
                        dataTable.Columns.Add("Total_Number_of_Pieces2");
                        break;
                    }

                case "AMAS":
                    {
                        dataTable.Columns.Add("Type Oper");
                        dataTable.Columns.Add("Master B/L", typeof(string));
                        dataTable.Columns.Add("ORIGIN_AIRPORT_CODE");
                        dataTable.Columns.Add("ORIGIN_AIRPORT");
                        dataTable.Columns.Add("DESTINATION_AIRPORT_CODE");
                        dataTable.Columns.Add("DESTINATION_AIRPORT");
                        dataTable.Columns.Add("House B/L", typeof(string));
                        dataTable.Columns.Add("CURRENCY");
                        dataTable.Columns.Add("Code IATA Carrier");
                        dataTable.Columns.Add("CARRIER (AEROLINEA)");
                        dataTable.Columns.Add("PIECES");
                        dataTable.Columns.Add("GROSS_WEIGHT");
                        dataTable.Columns.Add("SHIPPER_NAME");
                        dataTable.Columns.Add("SHIPPER_STREET");
                        dataTable.Columns.Add("SHIPPER_COUNTRY_Code");
                        dataTable.Columns.Add("SHIPPER_COUNTRY");
                        dataTable.Columns.Add("SHIPPER_CITY_Code");
                        dataTable.Columns.Add("SHIPPER_CITY");
                        dataTable.Columns.Add("CONSIGNEE_NAME");
                        dataTable.Columns.Add("CONSIGNEE_STREET");
                        dataTable.Columns.Add("CONSIGNEE_COUNTRY_code");
                        dataTable.Columns.Add("CONSIGNEE_COUNTRY");
                        dataTable.Columns.Add("CONSIGNEE_CITY_code");
                        dataTable.Columns.Add("CONSIGNEE_CITY");
                        dataTable.Columns.Add("GOOD_DESCRIPTION");
                        dataTable.Columns.Add("Number_of_United_Nations");
                        dataTable.Columns.Add("Number_of_United_Nations_Code");
                        dataTable.Columns.Add("Additional_information_of_Goods");
                        break;
                    }
            }
        }

        public void Transfer()
        {
            switch(this.transferType)
            {
                case "AMOS":
                    {
                        this.ComputedData = this.ExportOceanAMANACShipmentToExcel();
                        break;
                    }

                case "AMAS":
                    {
                        this.ComputedData = this.ExportAirAMANACShipmentToExcel();
                        break;
                    }
            }

            this.CreateAndWriteBlobFileToStorage();
        }        

        public byte[] ExportOceanAMANACShipmentToExcel()
        {
            if (shipments != null && shipments.Count > 0)
            {
                Tenant myTenant = TenantRepository.GetSingleTenant(tenant, true);
                ShippingLine carrier;
                Address shipperAddress;
                Address consigneeAddress;

                List<string> shipmentsIds = shipments.Select(s => s.Id).ToList();
                IQueryable<ShipmentPackage> shipmentPackages  = shipmentPackageRepository.GetPackagesFromShipmentsIds(shipmentsIds, tenant);

                int index = 1;
                foreach (ShipmentDataView item in shipments)
                {
                    carrier = shippingLineRepository.GetSingleShippingLine(item.MainCarriageCarrierId, tenant);
                    shipperAddress = addressRepository.GetSingleAddress(item.ShipperAddressId, tenant);
                    consigneeAddress = addressRepository.GetSingleAddress(item.ConsigneeAddressId, tenant);
                    List<ShipmentPackage> myShipmentPackages = shipmentPackages.Where(d => d.ShipmentId == item.Id).ToList();
                    List<ShipmentPackage> dangerousShipmentPackages = myShipmentPackages.Where(d => d.IsDangerous).ToList();

                    string marksAndNumbers = "";
                    string grossWeight = "";
                    string generalDescription = "";
                    string dangerousClassNumber = "";
                    string dangerousUNNumber = "";
                    string containerNumber = "";
                    string containerType = "";
                    string shipmentType = item.ShipmentTypeName;
                    int totalInsidePackages = item.NumberOfInsidePackages;

                    string portId = "";
                    if (item.ShipmentLevelCode == "H")
                    {
                        portId = item.DirectionId == "E" ? item.FromPortId : item.ToPortId;
                    }

                    else
                    {
                        portId = item.DirectionId == "E" ? item.MainCarriageFromPortId : item.MainCarriageFinalDestinationPortId;
                    }

                    Port customsSectionPort = null;
                    if(!string.IsNullOrEmpty(portId))
                    {
                        customsSectionPort = PortRepository.GetSinglePort(tenant, portId);
                    }

                    #region packages
                    foreach (ShipmentPackage package in myShipmentPackages)
                    {
                        if (string.IsNullOrEmpty(marksAndNumbers))
                        {
                            marksAndNumbers = package.MarksAndNumbers;
                        }

                        else
                        {
                            marksAndNumbers += ", " + package.MarksAndNumbers;
                        }

                        if (string.IsNullOrEmpty(grossWeight))
                        {
                            grossWeight = package.Weight == null ? "" : package.Weight.ToString();
                        }

                        else
                        {
                            grossWeight += ", " + package.Weight;
                        }

                        if (string.IsNullOrEmpty(generalDescription))
                        {
                            generalDescription = package.Description;
                        }

                        else
                        {
                            generalDescription += ", " + package.Description;
                        }
                        
                        if (string.IsNullOrEmpty(containerNumber))
                        {
                            containerNumber = package.ContainerNumber;
                        }

                        else
                        {
                            containerNumber += ", " + package.ContainerNumber;
                        }

                        if (string.IsNullOrEmpty(containerType))
                        {
                            containerType = package.PackageType == null ? "" : package.PackageType.Code;
                        }

                        else
                        {
                            containerType += ", " + (package.PackageType == null ? "" : package.PackageType.Code);
                        }
                    }
                    #endregion

                    #region dangerous packages
                    foreach (ShipmentPackage package in dangerousShipmentPackages)
                    {
                        if (string.IsNullOrEmpty(dangerousClassNumber))
                        {
                            dangerousClassNumber = package.ClassNumber;
                        }

                        else
                        {
                            dangerousClassNumber += ", " + package.ClassNumber;
                        }

                        if (string.IsNullOrEmpty(dangerousUNNumber))
                        {
                            dangerousUNNumber = package.UnNumber;
                        }

                        else
                        {
                            dangerousUNNumber += ", " + package.UnNumber;
                        }                        
                    }
                    #endregion

                    DataRow row = dataTable.NewRow();
                    row[0] = index++;
                    //row[1] = ;
                    row[2] = myTenant == null ? "" : myTenant.CAAT;
                    row[3] = "8";
                    row[4] = customsSectionPort == null ? "" : customsSectionPort.CombinedCode;
                    //row[5] = ;
                    row[6] = carrier == null ? "" : carrier.SCACCode;
                    row[7] = item.MainCarriageVesselName;
                    row[8] = item.MainCarriageCarrierNumber;
                    row[9] = item.DirectionId == "E" ? "2" : "1";

                    if (!string.IsNullOrEmpty(item.House))
                    {
                        string house = item.House.Trim();
                        house = Regex.Replace(item.House, @"[^0-9a-zA-Z.,+]+", "");

                        row[10] = house;
                    }

                    row[11] = item.ShipmentLevelCode == "H" ? item.FromPortCode : item.MainCarriageFromPortCode;
                    row[12] = item.ShipmentLevelCode == "H"? item.FromPortCountryCode : item.MainCarriageFromPortCountryCode;
                    row[13] = "H";

                    if (!string.IsNullOrEmpty(item.Master))
                    {
                        string master = item.Master.Trim();
                        master = Regex.Replace(item.Master, @"[^0-9a-zA-Z.,+]+", "");

                        row[14] = master;
                    }

                    row[15] = "2";
                    row[16] = item.ShipmentLevelCode == "H" ? item.ToPortCode : item.MainCarriageFinalDestinationPortCode;
                    row[17] = item.ShipmentLevelCode == "H"? item.ToPortCountryCode : item.MainCarriageFinalDestinationCountryCode;
                    row[18] = "1";
                    row[19] = item.ShipperName;
                    //row[20] = ;
                    row[21] = this.GetAddress(shipperAddress);
                    row[22] = "2";
                    row[23] = item.ConsigneeName;
                    //row[24] = ;
                    row[25] = this.GetAddress(consigneeAddress);
                    row[26] = "3";
                    row[27] = item.ConsigneeName;
                    //row[28] = ;
                    row[29] = this.GetAddress(consigneeAddress);
                    row[30] = "1";

                    if (dangerousShipmentPackages.Count > 0 || item.IsDangerous)
                    {
                        row[31] = "12";
                    }

                    else
                    {
                        row[31] = "0";
                    }

                    row[32] = myShipmentPackages.Count > 0 ? "1" : "";
                    row[33] = marksAndNumbers;
                    row[34] = item.ShipmentTypeId == "FCLD" ? totalInsidePackages : item.NumberOfPackages;
                    row[35] = grossWeight;
                    row[36] = "";
                    row[37] = generalDescription;
                    row[38] = "";
                    //row[39] = "";
                    row[40] = dangerousClassNumber;
                    row[41] = dangerousUNNumber;
                    row[42] = "";
                    row[43] = "";
                    row[44] = "1";
                    row[45] = containerNumber;
                    row[46] = containerType;
                    row[47] = shipmentType;
                    row[48] = grossWeight;
                    row[49] = item.ShipmentTypeId == "FCLD" ? totalInsidePackages : item.NumberOfPackages;

                    dataTable.Rows.Add(row);
                }
            }

            try
            {
                sheet1.ImportDataTable(dataTable, true, 1, 1);
                workbook.SaveAs(memory);
                workbook.Close();
                excelEngine.Dispose();
                return memory.ToArray();
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        public byte[] ExportAirAMANACShipmentToExcel()
        {   
            if (shipments != null && shipments.Count > 0)
            {
                Address shipperAddress;
                Address consigneeAddress;

                foreach (ShipmentDataView item in shipments)
                {
                    shipperAddress = addressRepository.GetSingleAddress(item.ShipperAddressId, tenant);
                    consigneeAddress = addressRepository.GetSingleAddress(item.ConsigneeAddressId, tenant);

                    string typeOper = item.TransportModeId;
                    switch(item.DirectionId)
                    {
                        case "I":
                            {
                                typeOper = "IA";
                                break;
                            }

                        default:
                            {
                                typeOper = "EA";
                                break;
                            }
                    }

                    DataRow row = dataTable.NewRow();
                    row[0] = typeOper;
                    row[1] = item.AirlinePrefix + item.Master;
                    row[2] = item.MainCarriageFromPortCode;
                    row[3] = item.MainCarriageFromPortName;
                    row[4] = item.MainCarriageFinalDestinationPortCode;
                    row[5] = item.MainCarriageFinalDestinationPortName;
                    
                    if (!string.IsNullOrEmpty(item.House))
                    {
                        string house = item.House.Trim();
                        house = Regex.Replace(item.House, @"[^0-9a-zA-Z.,+]+", "");

                        row[6] = house;
                    }
                                        
                    if (!string.IsNullOrEmpty(item.ValueOfGoodsCurrencyId))
                    {
                        Currency currency = CurrencyRepository.GetSingleCurrency(item.ValueOfGoodsCurrencyId, tenant, true);
                        if (currency != null)
                        {
                            row[7] = currency.Code;
                        }
                    }
                    
                    row[8] = item.MainCarriageCarrierCode;
                    row[9] = item.MainCarriageCarrierName;
                    row[10] = item.NumberOfPackages;
                    row[11] = MethodHelper.Round(item.GrossWeight, 3);
                    row[12] = item.ShipperName;
                    row[13] = this.ComputeAddressStreet(shipperAddress);

                    if (shipperAddress != null)
                    {
                        if (!string.IsNullOrEmpty(shipperAddress.CountryId))
                        {
                            Country shipperCountry = CountryRepository.GetSingleCountry(shipperAddress.CountryId, tenant, true);
                            if (shipperCountry != null)
                            {
                                row[14] = shipperCountry.Code;
                                row[15] = shipperCountry.EnglishName;
                            }

                            CountryCity countryCity = countryCityRepository.GetSingleCountryCityByNameAndCountry(shipperAddress.City, shipperAddress.CountryId, tenant);
                            if (countryCity != null)
                            {
                                row[16] = countryCity.Code;
                            }
                        }

                        row[17] = shipperAddress.City;
                    }

                    row[18] = item.ConsigneeName;
                    row[19] = this.ComputeAddressStreet(consigneeAddress);

                    if (consigneeAddress != null)
                    {
                        if (!string.IsNullOrEmpty(consigneeAddress.CountryId))
                        {
                            Country consigneeCountry = CountryRepository.GetSingleCountry(consigneeAddress.CountryId, tenant, true);
                            if (consigneeCountry != null)
                            {
                                row[20] = consigneeCountry.Code;
                                row[21] = consigneeCountry.EnglishName;
                            }

                            CountryCity countryCity = countryCityRepository.GetSingleCountryCityByNameAndCountry(consigneeAddress.City, consigneeAddress.CountryId, tenant);
                            if (countryCity != null)
                            {
                                row[22] = countryCity.Code;
                            }
                        }

                        row[23] = consigneeAddress.City;
                    }
                     
                    row[24] = item.DescriptionOfGoods;
                    row[25] = item.IsDangerous ? "ED" : "";
                    row[26] = item.DangerousUnNumber;
                    row[27] = item.DescriptionOfGoods;

                    dataTable.Rows.Add(row);
                }
            }

            try
            {
                sheet1.ImportDataTable(dataTable, true, 1, 1);
                workbook.SaveAs(memory);
                return memory.ToArray();
            }

            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private string ComputeAddressStreet(Address address)
        {
            string street = "";

            if (address != null)
            {
                if (!string.IsNullOrEmpty(address.Address1))
                {
                    street = address.Address1;
                }

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    if (string.IsNullOrEmpty(street))
                    {
                        street = address.Address2;
                    }

                    else
                    {
                        street = street + "," + address.Address2;
                    }
                }
            }

            return street;
        }
        public string GetAddress(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (address.State != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + " " + (address.State.LocalName != null ? address.State.LocalName : "");
                    }

                    else
                    {
                        resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
                    }
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.EnglishName;
                    }
                }
            }

            return resultAddress;
        }

        private void CreateAndWriteBlobFileToStorage()
        {
            if (ComputedData != null)
            {
                string[] fileProps = fileName.Split('.');

                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileProps[0],
                    HasExternalContainer = true,
                    Extension = "xlsx",
                    Tenant = tenant,
                    FileSize = ComputedData.Length,
                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                storageservice.Write(ComputedData, fileInfo);
            }
        }
    }
}
