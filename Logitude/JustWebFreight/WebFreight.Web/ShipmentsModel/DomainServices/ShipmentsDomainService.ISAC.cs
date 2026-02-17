using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        public ISACValidator GetISACValidator(string myShipmentId, int myTenant)
        {
            ISACValidator myResult = new ISACValidator()
            {
                Id = myTenant,
                Tenant = myTenant,
                ShipmentId = myShipmentId,
                IsValid = true,
            };

            myResult.MasterErrors = new List<string>();
            myResult.HouseValidators = new List<ISACHouseValidator>();

            shipmentQuery = new ShipmentQuery(myTenant);
            ShipmentPM entityPM = shipmentQuery.GetSinglePMWithoutComposition(myShipmentId, myTenant);

            if (entityPM != null)
            {
                myResult.ShipmentLevelCode = entityPM.ShipmentLevelCode;

                AddressRepository addressRepository = new AddressRepository(myTenant);

                #region Validate Tenant fields
                string myPIMA = null;
                string myLocalCustomsCode = null;

                Tenant loggedTenant = TenantRepository.GetSingleTenant(myTenant, true);
                if (loggedTenant != null)
                {
                    myLocalCustomsCode = loggedTenant.LocalCustomsCode;
                }

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                    TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(myResult.Tenant);
                    if (tenantManagement != null)
                    {
                        myPIMA = tenantManagement.PIMA;
                    }
                }

                if (string.IsNullOrEmpty(myPIMA))
                {
                    myResult.MasterErrors.Add("Tenant communication parameter (PIMA) is missing");
                }

                if (string.IsNullOrEmpty(myLocalCustomsCode))
                {
                    myResult.MasterErrors.Add("Tenant Local Customs Code (Agent Code) is missing");
                }
                #endregion

                if (entityPM.ShipmentLevelCode == "D")
                {
                    myResult.ShipmentNumber = entityPM.ShipmentNumber;

                    if (string.IsNullOrEmpty(entityPM.DescriptionOfGoods))
                    {
                        myResult.MasterErrors.Add("Description Of Goods field is missing");
                    }

                    #region Shipper
                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                    {
                        myResult.MasterErrors.Add("Shipper field is missing");
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(entityPM.ShipperAddressId))
                        {
                            myResult.MasterErrors.Add("Shipper Address field is missing");
                        }

                        else
                        {
                            Address myAddress = addressRepository.GetSingleAddress(entityPM.ShipperAddressId, myTenant);
                            if (string.IsNullOrEmpty(myAddress.Address1) && string.IsNullOrEmpty(myAddress.Address2))
                            {
                                myResult.MasterErrors.Add("Shipper Address1 & Address2 fields is missing");
                            }
                        }
                    }
                    #endregion

                    #region Consignee
                    if (string.IsNullOrEmpty(entityPM.ConsigneeId))
                    {
                        myResult.MasterErrors.Add("Consignee field is missing");
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(entityPM.ConsigneeAddressId))
                        {
                            myResult.MasterErrors.Add("Consignee Address field is missing");
                        }

                        else
                        {
                            Address myAddress = addressRepository.GetSingleAddress(entityPM.ConsigneeAddressId, myTenant);
                            if (string.IsNullOrEmpty(myAddress.Address1) && string.IsNullOrEmpty(myAddress.Address2))
                            {
                                myResult.MasterErrors.Add("Consignee Address1 & Address2 fields is missing");
                            }
                        }
                    }
                    #endregion

                    #region Validate Master fields

                    if (string.IsNullOrEmpty(entityPM.Master))
                    {
                        myResult.MasterErrors.Add("Master field is missing");
                    }

                    if (entityPM.GrossWeight == null || entityPM.GrossWeight == 0)
                    {
                        myResult.MasterErrors.Add("Gross Weight field is missing");
                    }

                    if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
                    {
                        myResult.MasterErrors.Add("Main Carriage Airline field is missing");
                    }

                    else
                    {
                        //if (string.IsNullOrEmpty(entityPM.TenantZeroAirlinePIMA))
                        //{
                        //    myResult.MasterErrors.Add("Airline communication parameter (PIMA) is missing");
                        //}

                        if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierNumber))
                        {
                            myResult.MasterErrors.Add("Main Carriage flight Number field is missing");
                        }

                        if (entityPM.MainCarriageETD == null)
                        {
                            myResult.MasterErrors.Add("Main Carriage ETD field is missing");
                        }
                    }
                    #endregion

                    myResult.IsValid = myResult.MasterErrors.Count == 0 ? true : false;
                }

                else if (entityPM.ShipmentLevelCode == "H")
                {
                    ISACHouseValidator iSACHouseValidator = new ISACHouseValidator()
                    {
                        Id = entityPM.Id,
                        ISACValidatorId = myResult.Id,
                        Tenant = myTenant,
                        House = entityPM.House,
                        ShipmentNumber = entityPM.ShipmentNumber,
                    };

                    List<string> myErrors = new List<string>();

                    #region Fields
                    if (string.IsNullOrEmpty(entityPM.House))
                    {
                        myErrors.Add("House field is missing");
                    }

                    if (entityPM.GrossWeight == null || entityPM.GrossWeight == 0)
                    {
                        myErrors.Add("Gross Weight field is missing");
                    }

                    if (string.IsNullOrEmpty(entityPM.DescriptionOfGoods))
                    {
                        myErrors.Add("Description Of Goods field is missing");
                    }
                    #endregion

                    #region Shipper
                    if (string.IsNullOrEmpty(entityPM.ShipperId))
                    {
                        myErrors.Add("Shipper field is missing");
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(entityPM.ShipperAddressId))
                        {
                            myErrors.Add("Shipper Address field is missing");
                        }

                        else
                        {
                            Address myAddress = addressRepository.GetSingleAddress(entityPM.ShipperAddressId, myTenant);
                            if (string.IsNullOrEmpty(myAddress.Address1) && string.IsNullOrEmpty(myAddress.Address2))
                            {
                                myErrors.Add("Shipper Address1 & Address2 fields is missing");
                            }
                        }
                    }
                    #endregion

                    #region Consignee
                    if (string.IsNullOrEmpty(entityPM.ConsigneeId))
                    {
                        myErrors.Add("Consignee field is missing");
                    }

                    else
                    {
                        if (string.IsNullOrEmpty(entityPM.ConsigneeAddressId))
                        {
                            myErrors.Add("Consignee Address field is missing");
                        }

                        else
                        {
                            Address myAddress = addressRepository.GetSingleAddress(entityPM.ConsigneeAddressId, myTenant);
                            if (string.IsNullOrEmpty(myAddress.Address1) && string.IsNullOrEmpty(myAddress.Address2))
                            {
                                myErrors.Add("Consignee Address1 & Address2 fields is missing");
                            }
                        }
                    }
                    #endregion

                    if (string.IsNullOrEmpty(entityPM.MasterShipmentDataId))
                    {
                        myResult.MasterErrors.Add("This House is not connected to Master");
                    }

                    else
                    {
                        ShipmentPM entityMasterPM = shipmentQuery.GetSinglePMWithoutComposition(entityPM.MasterShipmentDataId, myTenant);
                        if (entityMasterPM != null)
                        {
                            myResult.ShipmentNumber = entityMasterPM.ShipmentNumber;

                            #region Validate Master fields

                            if (string.IsNullOrEmpty(entityMasterPM.Master))
                            {
                                myResult.MasterErrors.Add("Master field is missing");
                            }

                            if (entityMasterPM.GrossWeight == null || entityMasterPM.GrossWeight == 0)
                            {
                                myResult.MasterErrors.Add("Gross Weight field is missing");
                            }

                            if (string.IsNullOrEmpty(entityMasterPM.MainCarriageCarrierId))
                            {
                                myResult.MasterErrors.Add("Main Carriage Airline field is missing");
                            }

                            else
                            {
                                //if (string.IsNullOrEmpty(entityMasterPM.TenantZeroAirlinePIMA))
                                //{
                                //    myResult.MasterErrors.Add("Airline communication parameter (PIMA) is missing");
                                //}

                                if (string.IsNullOrEmpty(entityMasterPM.MainCarriageCarrierNumber))
                                {
                                    myResult.MasterErrors.Add("Main Carriage flight Number field is missing");
                                }

                                if (entityMasterPM.MainCarriageETD == null)
                                {
                                    myResult.MasterErrors.Add("Main Carriage ETD field is missing");
                                }
                            }
                            #endregion
                        }
                    }

                    iSACHouseValidator.Errors = myErrors;
                    iSACHouseValidator.IsValid = myErrors.Count == 0 ? true : false;
                    myResult.HouseValidators.Add(iSACHouseValidator);
                    myResult.IsValid = myResult.MasterErrors.Count == 0 && myErrors.Count == 0 ? true : false;
                }

                else if (entityPM.ShipmentLevelCode == "C")
                {
                    myResult.ShipmentNumber = entityPM.ShipmentNumber;

                    #region Validate Master fields

                    if (string.IsNullOrEmpty(entityPM.Master))
                    {
                        myResult.MasterErrors.Add("Master field is missing");
                    }

                    if (entityPM.GrossWeight == null || entityPM.GrossWeight == 0)
                    {
                        myResult.MasterErrors.Add("Gross Weight field is missing");
                    }

                    if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
                    {
                        myResult.MasterErrors.Add("Main Carriage Airline field is missing");
                    }

                    else
                    {
                        //if (string.IsNullOrEmpty(entityPM.TenantZeroAirlinePIMA))
                        //{
                        //    myResult.MasterErrors.Add("Airline communication parameter (PIMA) is missing");
                        //}

                        if (string.IsNullOrEmpty(entityPM.MainCarriageCarrierNumber))
                        {
                            myResult.MasterErrors.Add("Main Carriage flight Number field is missing");
                        }

                        if (entityPM.MainCarriageETD == null)
                        {
                            myResult.MasterErrors.Add("Main Carriage ETD field is missing");
                        }
                    }
                    #endregion

                    ShipmentRepository shipmentRepository = new ShipmentRepository(myTenant);
                    List<Shipment> allHouses = shipmentRepository.GetHouseShipmentsForMaster(myShipmentId, myTenant);
                    if (allHouses.Count == 0)
                    {
                        myResult.MasterErrors.Add("No Connected houses found");
                    }

                    else
                    {
                        foreach (Shipment item in allHouses)
                        {
                            ISACHouseValidator iSACHouseValidator = new ISACHouseValidator()
                            {
                                Id = item.Id,
                                ISACValidatorId = myResult.Id,
                                Tenant = myTenant,
                                House = item.House,
                                ShipmentNumber = item.ShipmentNumber,
                            };

                            List<string> myErrors = new List<string>();

                            #region Fields
                            if (string.IsNullOrEmpty(item.House))
                            {
                                myErrors.Add("House field is missing");
                            }

                            if (item.GrossWeight == null || item.GrossWeight == 0)
                            {
                                myErrors.Add("Gross Weight field is missing");
                            }

                            if (string.IsNullOrEmpty(item.DescriptionOfGoods))
                            {
                                myErrors.Add("Description Of Goods field is missing");
                            }
                            #endregion

                            #region Shipper
                            if (string.IsNullOrEmpty(item.ShipperId))
                            {
                                myErrors.Add("Shipper field is missing");
                            }

                            else
                            {
                                if (string.IsNullOrEmpty(item.ShipperAddressId))
                                {
                                    myErrors.Add("Shipper Address field is missing");
                                }

                                else
                                {
                                    Address myAddress = addressRepository.GetSingleAddress(item.ShipperAddressId, myTenant);
                                    if (string.IsNullOrEmpty(myAddress.Address1) && string.IsNullOrEmpty(myAddress.Address2))
                                    {
                                        myErrors.Add("Shipper Address1 & Address2 fields is missing");
                                    }
                                }
                            }
                            #endregion

                            #region Consignee
                            if (string.IsNullOrEmpty(item.ConsigneeId))
                            {
                                myErrors.Add("Consignee field is missing");
                            }

                            else
                            {
                                if (string.IsNullOrEmpty(item.ConsigneeAddressId))
                                {
                                    myErrors.Add("Consignee Address field is missing");
                                }

                                else
                                {
                                    Address myAddress = addressRepository.GetSingleAddress(item.ConsigneeAddressId, myTenant);
                                    if (string.IsNullOrEmpty(myAddress.Address1) && string.IsNullOrEmpty(myAddress.Address2))
                                    {
                                        myErrors.Add("Consignee Address1 & Address2 fields is missing");
                                    }
                                }
                            }
                            #endregion

                            iSACHouseValidator.Errors = myErrors;
                            iSACHouseValidator.IsValid = myErrors.Count == 0 ? true : false;
                            myResult.HouseValidators.Add(iSACHouseValidator);
                        }

                        if (myResult.HouseValidators.Where(d => d.IsValid == false).Any())
                        {
                            myResult.MasterErrors.Add("All connected houses must be valid");
                        }
                    }

                    myResult.IsValid = myResult.MasterErrors.Count == 0 ? true : false;
                }
            }            

            return myResult;
        }
    }

    public class ISACValidator
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string ShipmentLevelCode { get; set; }
        public bool IsValid { get; set; }
        public List<string> MasterErrors { get; set; }

        [Include]
        [Association("ISACValidatorISACHouseValidators", "Id", "ISACValidatorId")]
        public List<ISACHouseValidator> HouseValidators { get; set; }
    }

    public class ISACHouseValidator
    {
        [Key]
        public string Id { get; set; }
        public int ISACValidatorId { get; set; }
        public int Tenant { get; set; }
        public string House { get; set; }
        public string ShipmentNumber { get; set; }
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
    }
}