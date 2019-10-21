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
        private ICommonDataContext commoContext;
        private IShipmentsContext shipmentsContext;
        private ShipmentPayableRepository shipmentPayableRepository;
        private ShippingLineRepository shippingLineRepository;
        private AddressRepository addressRepository;
        public CustomsTransferService(List<ShipmentDataView> shipments, string filename, string type, int tenant)
        {
            this.tenant = tenant;
            this.fileName = filename;
            this.transferType = type;
            this.shipments = shipments;

            commoContext = CommonDataContext.GetContext(tenant);
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            shipmentPayableRepository = new ShipmentPayableRepository(shipmentsContext);
            shippingLineRepository = new ShippingLineRepository(commoContext);
            addressRepository = new AddressRepository(commoContext);

            this.InitializeExcelFile();            
        }

        private void InitializeExcelFile()
        {
            memory = new System.IO.MemoryStream();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            this.workbook = excelEngine.Excel.Workbooks.Create(1);

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
                        sheet1.Range["A1:AC1"].CellStyle.Font.Bold = true;
                        sheet1.Range["A1:AC1"].CellStyle.Font.Size = 10;
                        sheet1.Range["A1:AC1"].CellStyle.Font.FontName = "Calibri";
                        sheet1.Range["A1:AC1"].CellStyle.Font.Color = ExcelKnownColors.Black;
                        sheet1.Range["A1:AC1"].CellStyle.Color = System.Drawing.Color.FromArgb(255, 242, 220, 219);
                        sheet1.Range["A1:AC1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;

                        break;
                    }


                case "AMAS":
                    {
                        sheet1.Range["A1:AA1"].CellStyle.Font.Bold = true;
                        sheet1.Range["A1:AA1"].CellStyle.Font.Size = 10;
                        sheet1.Range["A1:AA1"].CellStyle.Font.FontName = "Calibri";
                        sheet1.Range["A1:AA1"].CellStyle.Font.Color = ExcelKnownColors.White;
                        sheet1.Range["A1:AA1"].CellStyle.Color = System.Drawing.Color.Orange;
                        sheet1.Range["A1:AA1"].CellStyle.HorizontalAlignment = ExcelHAlign.HAlignCenter;
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
                        dataTable.Columns.Add("Electronic_Acknowledgment_of_Recepction");
                        dataTable.Columns.Add("Key_Carrier");
                        dataTable.Columns.Add("Name_of_Vessel");
                        dataTable.Columns.Add("Number_of_Trip");
                        dataTable.Columns.Add("Type_of_Operation");
                        dataTable.Columns.Add("Number_Of_BL");
                        dataTable.Columns.Add("Port_of_Loading_Discharge");
                        dataTable.Columns.Add("Country_of_Port_of_Loading_Discharge");
                        dataTable.Columns.Add("Type_of_BL");
                        dataTable.Columns.Add("Number_of_BL_Reference");
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
                        break;
                    }

                case "AMAS":
                    {
                        dataTable.Columns.Add("Master B/L");
                        dataTable.Columns.Add("ORIGIN_AIRPORT_CODE");
                        dataTable.Columns.Add("ORIGIN_AIRPORT");
                        dataTable.Columns.Add("DESTINATION_AIRPORT_CODE");
                        dataTable.Columns.Add("DESTINATION_AIRPORT");
                        dataTable.Columns.Add("House B/L");
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
                Tenant myTenant;
                ShippingLine carrier;
                Address shipperAddress;
                Address consigneeAddress;

                foreach (ShipmentDataView item in shipments)
                {
                    myTenant = TenantRepository.GetSingleTenant(tenant, true);
                    carrier = shippingLineRepository.GetSingleShippingLine(item.MainCarriageCarrierId, tenant);
                    shipperAddress = addressRepository.GetSingleAddress(item.ShipperAddressId, tenant);
                    consigneeAddress = addressRepository.GetSingleAddress(item.ConsigneeAddressId, tenant);

                    DataRow row = dataTable.NewRow();
                    row[0] = "1";
                    //row[1] = ;
                    row[2] = myTenant == null ? "" : myTenant.CAAT;
                    row[3] = "8";
                    //row[4] = ;
                    row[5] = carrier == null ? "" : carrier.SCACCode;
                    row[6] = item.MainCarriageVesselName;
                    row[7] = item.MainCarriageCarrierNumber;
                    row[8] = item.DirectionId == "E" ? "2" : "1";
                    row[9] = item.House;
                    //row[10] = ;
                    //row[11] = ;
                    row[12] = "H";
                    row[13] = item.House;
                    row[14] = "2";
                    //row[15] = ;
                    //row[16] = ;
                    row[17] = "1";
                    row[18] = item.ShipperName;
                    //row[19] = ;
                    row[20] = this.GetAddress(shipperAddress);
                    row[21] = "2";
                    row[22] = item.ConsigneeName;
                    //row[23] = ;
                    row[24] = this.GetAddress(consigneeAddress);
                    row[25] = "3";
                    row[26] = item.ConsigneeName;
                    //row[27] = ;
                    row[28] = this.GetAddress(consigneeAddress);

                    dataTable.Rows.Add(row);
                }
            }
            
            sheet1.ImportDataTable(dataTable, true, 1, 1);
            workbook.SaveAs(memory);
            return memory.ToArray();
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
                    List<ShipmentPayable> shipmentPayables = shipmentPayableRepository.GetShipemntPayablesByShipmentId(item.Id, tenant);

                    DataRow row = dataTable.NewRow();
                    row[0] = item.LongMaster;
                    row[1] = item.MainCarriageFromPortCode;
                    row[2] = item.MainCarriageFromPortName;
                    row[3] = item.MainCarriageFinalDestinationPortCode;
                    row[4] = item.MainCarriageFinalDestinationPortName;
                    row[5] = item.House;

                    if (shipmentPayables.Count > 0)
                    {
                        ShipmentPayable shipmentPayable = shipmentPayables.Where(d => d.ChargesType != null && d.ChargesType.ChargesGroupCode == "FRT").FirstOrDefault();
                        if (shipmentPayable != null)
                        {
                            Currency currency = CurrencyRepository.GetSingleCurrency(shipmentPayable.CurrencyId, tenant, true);
                            if (currency != null)
                            {
                                row[6] = shipmentPayable.Currency.Code;
                            }
                        }
                    }

                    row[7] = item.MainCarriageCarrierCode;
                    row[8] = item.MainCarriageCarrierName;
                    row[9] = item.NumberOfPackages;
                    row[10] = MethodHelper.Round(item.GrossWeight, 3);
                    row[11] = item.ShipperName;
                    row[12] = this.ComputeAddressStreet(shipperAddress);

                    if (shipperAddress != null && !string.IsNullOrEmpty(shipperAddress.CountryId))
                    {
                        Country shipperCountry = CountryRepository.GetSingleCountry(shipperAddress.CountryId, tenant, true);
                        if (shipperCountry != null)
                        {
                            row[13] = shipperCountry.Code;
                            row[14] = shipperCountry.EnglishName;
                        }
                    }

                    //row[15] = shipment.ShipperCityCode;
                    row[16] = shipperAddress == null ? "" : shipperAddress.City;
                    row[17] = item.ConsigneeName;
                    row[18] = this.ComputeAddressStreet(consigneeAddress);

                    if (consigneeAddress != null && !string.IsNullOrEmpty(consigneeAddress.CountryId))
                    {
                        Country consigneeCountry = CountryRepository.GetSingleCountry(consigneeAddress.CountryId, tenant, true);
                        if (consigneeCountry != null)
                        {
                            row[19] = consigneeCountry.Code;
                            row[20] = consigneeCountry.EnglishName;
                        }
                    }

                    row[21] = item.MainCarriageFinalDestinationPortCode;
                    row[22] = consigneeAddress == null ? "" : consigneeAddress.City;
                    row[23] = item.DescriptionOfGoods;
                    row[24] = item.IsDangerous ? "ED" : "";
                    row[25] = item.DangerousUnNumber;
                    row[26] = item.DescriptionOfGoods;

                    dataTable.Rows.Add(row);
                }
            }

            sheet1.ImportDataTable(dataTable, true, 1, 1);
            workbook.SaveAs(memory);
            return memory.ToArray();
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
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = fileName,
                    FolderName = "others",
                    Extension = "xls",
                    Tenant = tenant,
                    FileSize = ComputedData.Length,
                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                storageservice.Write(ComputedData, fileInfo);
            }
        }
    }
}
