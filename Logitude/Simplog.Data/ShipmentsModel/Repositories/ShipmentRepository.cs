using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using System;
using Simplog.Data.Helpers;
using System.Data.SqlClient;
using System.Text;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ShipmentRepository : IRepository<Shipment>
    {
        IShipmentsContext shipmentsContext;

        public ShipmentRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public ShipmentRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public ShipmentRepository()
        {
            shipmentsContext = new ShipmentsContext();
        }

        public string GetMasterFHLStatus(string masterShipmentId)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(masterShipmentId))
            {
                List<Shipment> iQueryable = (from a in context.Shipments
                                             where
                                             a.ShipmentLevelCode == "H"
                                             && a.MasterShipmentDataId == masterShipmentId
                                             && a.Id != masterShipmentId
                                             select a).ToList();

                int allHousesCount = iQueryable.Count();
                int allErrorsCount = iQueryable.Where(d => d.FHLStatusCode == "EROR").Count();
                int allAcceptCount = iQueryable.Where(d => d.FHLStatusCode == "ACPT").Count();
                int allSentCount = iQueryable.Where(d => d.FHLStatusCode == "SENT").Count();
                int allNotSentCount = iQueryable.Where(d => d.FHLStatusCode == "NSEN").Count();
                int allPartiallySentCount = iQueryable.Where(d => d.FHLStatusCode == "PSEN").Count();

                if (allErrorsCount == allHousesCount)
                {
                    myResult = "EROR";
                }

                else if (allAcceptCount == allHousesCount)
                {
                    myResult = "ACPT";
                }

                else if (allSentCount == allHousesCount)
                {
                    myResult = "SENT";
                }

                else if (allNotSentCount == allHousesCount)
                {
                    myResult = "NSEN";
                }

                else if (allPartiallySentCount == allHousesCount)
                {
                    myResult = "PSEN";
                }

                else if (allSentCount > 0 && allNotSentCount > 0)
                {
                    myResult = "PSEN";
                }
            }

            return myResult;
        }
        public string GetMasterCargonautFHLStatus(string masterShipmentId)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(masterShipmentId))
            {
                List<Shipment> iQueryable = (from a in context.Shipments
                                             where
                                             a.ShipmentLevelCode == "H"
                                             && a.MasterShipmentDataId == masterShipmentId
                                             && a.Id != masterShipmentId
                                             select a).ToList();

                int allHousesCount = iQueryable.Count();
                int allErrorsCount = iQueryable.Where(d => d.CargonautFHLStatusCode == "EROR").Count();
                int allAcceptCount = iQueryable.Where(d => d.CargonautFHLStatusCode == "ACPT").Count();
                int allSentCount = iQueryable.Where(d => d.CargonautFHLStatusCode == "SENT").Count();
                int allNotSentCount = iQueryable.Where(d => d.CargonautFHLStatusCode == "NSEN").Count();
                int allPartiallySentCount = iQueryable.Where(d => d.CargonautFHLStatusCode == "PSEN").Count();

                if (allErrorsCount == allHousesCount)
                {
                    myResult = "EROR";
                }

                else if (allAcceptCount == allHousesCount)
                {
                    myResult = "ACPT";
                }

                else if (allSentCount == allHousesCount)
                {
                    myResult = "SENT";
                }

                else if (allNotSentCount == allHousesCount)
                {
                    myResult = "NSEN";
                }

                else if (allPartiallySentCount == allHousesCount)
                {
                    myResult = "PSEN";
                }

                else if (allSentCount > 0 && allNotSentCount > 0)
                {
                    myResult = "PSEN";
                }
            }

            return myResult;
        }

        public int GetAllShipmentsCount(int tenant)
        {
            return (from d in context.Shipments where d.Tenant == tenant && d.ShipmentLevelCode != "C" && d.IsCancelled == false select d).Count();
        }

        public int GetAllMastersCount(int tenant)
        {
            return (from d in context.Shipments
                    where d.Tenant == tenant && d.ShipmentLevelCode != "H" && d.ShipmentLevelCode != "A" && !d.IsCancelled
                    select d).Count();
        }

        public string GetShipmentStatusName(string id, int tenant)
        {
            string result = "";
            string statusId = (from s in context.Shipments where s.Tenant == tenant && s.Id != id select s.StatusId).FirstOrDefault();

            if (!string.IsNullOrEmpty(statusId))
            {
                EntityStatus status = EntityStatusRepository.GetSingleEntityStatus(statusId, tenant, true);
                if (status != null)
                {
                    result = status.Name;
                }
            }
            return result;
        }

        public byte[] GetLastModifiedTimeStamp(string shipmentId, int tenant)
        {
            //return (from a in context.Shipments
            //        where a.Id == shipmentId && a.Tenant == tenant
            //        select a).FirstOrDefault().LastModified;
            return null;
        }

        public string GetShipmentLevelCode(string shipmentId)
        {
            return (from a in context.Shipments where a.Id == shipmentId select a).FirstOrDefault().ShipmentLevelCode;
        }

        public string GetShipmentProfitCurrencyId(string shipmentId)
        {
            return (from a in context.Shipments where a.Id == shipmentId select a).FirstOrDefault().ProfitCurrencyId;
        }

        public List<Shipment> GetHouseShipmentsForMaster(string masterId, int tenant)
        {
            List<Shipment> consoles = (from s in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType").Include("FHLStatus").Include("CargonautFHLStatus")
                                       where s.Tenant == tenant
                                       && s.Id != masterId 
                                       && s.MasterShipmentDataId == masterId
                                       select s).ToList();
            return consoles;
        }

        public IQueryable<Shipment> GetShipments(int tenant)
        {
            return (from record in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType") where record.Tenant == tenant select record);
        }
        public IQueryable<ShipmentDataView> GetFilterdShipments(int tenant, string SearchFields)
        { 
            return (from record in context.ShipmentSearch(SearchFields)//FunctionTableValue<Shipment>("udf_ShipmentSearch", parameters) 
                    where record.Tenant == tenant 
                    select record);
        }
        public IQueryable<Shipment> GetShipmentsWithMasterData(int tenant)
        {
            return (from record in context.Shipments.Include("ShipmentMasterData").Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType") where record.Tenant == tenant select record);
        }

        public int GetShipmentsCountByQuoteId(string quoteId, int tenant)
        {
            return (from d in context.Shipments
                    where d.Tenant == tenant && d.QuoteId == quoteId
                    select d).Count();
        }

        public List<Shipment> GetNotConnectedCustomShipments(int tenant, string fileNumber)
        {
            return (from record in context.Shipments
                    where record.CustomFileNumber == fileNumber && string.IsNullOrEmpty(record.CustomFileId) && record.Tenant == tenant
                    select record).ToList();
        }


        public IQueryable<string> GetConnectedCustomShipmentsId(int tenant, string shipmentId)
        {
            return (from record in context.Shipments
                    where record.CustomFileId == shipmentId && record.Tenant == tenant
                    select record.Id);
        }
        
        public IQueryable<Shipment> GetConnectedCustomShipments(int tenant, string shipmentId)
        {
            return (from record in context.Shipments
                    where record.CustomFileId == shipmentId && record.Tenant == tenant
                    select record);
        }
        
        public bool HasConnectedCustomShipments(int tenant, string id)
        {
            return (from record in context.Shipments
                    where record.CustomFileId == id && record.Tenant == tenant
                    select record).Any();
        }

        public ShipmentMasterData GetSingleShipmentMasterData(string id, int tenant)
        {
            return (from record in context.ShipmentMasterDatas where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Shipment GetSingleShipment(string id, int tenant)
        {
            return (from record in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public Shipment getSingleShipmentBySecurityId(string SecurityKey, int tenant)
        {
            return (from record in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType") where record.SecurityKey == SecurityKey && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Shipment getSingleShipmentBySecurityIdAndId(string id,string SecurityKey, int tenant)
        {
            return (from record in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType") where record.SecurityKey == SecurityKey && record.Id==id && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public Shipment GetSingleShipmentwithOutIncludes(string id, int tenant)
        {
            return (from record in context.Shipments where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public Shipment GetSingleShipmentByNumberWithOutIncludes(string shipmentNumber, int tenant)
        {
            return (from record in context.Shipments where record.ShipmentNumber == shipmentNumber && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public bool IsShipmentArchived(string id, int tenant)
        {
            return (from record in context.Shipments where record.Id == id && record.Tenant == tenant select record.IsOperationalClosed).FirstOrDefault();
        }

        public string getShipmentDirection(string id, int tenant)
        {
            return (from record in context.Shipments where record.Id == id && record.Tenant == tenant select record.TransportModeId).FirstOrDefault();
        }

        public Shipment GetSingleShipmentOnlyByNumber(string number, int tenant)
        {
            return (from record in context.Shipments where record.ShipmentNumber == number && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Shipment GetSingleShipmentByShipmentNumber(string number, int tenant)
        {
            return (from record in context.Shipments where record.ShipmentNumber == number && record.Tenant == tenant select record).FirstOrDefault();
        }

        public bool CheckShipmentExistsByNumber(string number, int tenant)
        {
            return (from d in context.Shipments where d.ShipmentNumber == number && d.Tenant == tenant select d).Any();
        }

        public IQueryable<ShipmentDataView> GetMasterViewsByTenant(int tenant)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);
            return from a in dataViewEntities.ShipmentDataViews
                   where a.Tenant == tenant && (a.ShipmentLevelCode == "D" || a.ShipmentLevelCode == "C")
                   select a;
        }

        public IQueryable<ShipmentDataView> GetShipmentViewsByTenant(int tenant)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);

            IQueryable<ShipmentDataView> result = (from f in dataViewEntities.ShipmentDataViews where f.Tenant == tenant select f);

            return result;
        }

        public IQueryable<ShipmentCountryDashboardView> GetShipmentDataViewsForCountriesDashboard(int tenant, string customerid, string directionId, string transportmodeId)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);

            IQueryable<ShipmentCountryDashboardView> result = (from f in dataViewEntities.ShipmentCountryDashboardViews where f.Tenant == tenant && !f.IsCancelled select f);
            if (!string.IsNullOrEmpty(customerid))
            {
                result = (from f in result where f.CustomerId == customerid select f);
            }
            else
            {
                result = (from f in result where f.CustomerId !=null select f);
            }
            if (!string.IsNullOrEmpty(directionId))
            {
                result = (from f in result where f.DirectionId == directionId select f);
            }
            if (!string.IsNullOrEmpty(transportmodeId))
            {
                result = (from f in result where f.TransportModeId == transportmodeId select f);
            }

            return result;
        }
        
        public IQueryable<ShipmentsCustomersDashboardView> GetShipmentDataViewsForCustomersDashboard(int tenant, string directionId, string transportmodeId)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);

            IQueryable<ShipmentsCustomersDashboardView> result = (from f in dataViewEntities.ShipmentsCustomersDashboardViews where f.Tenant == tenant && !f.IsCancelled select f);
            
                result = (from f in result where f.CustomerId != null select f);
            
            if (!string.IsNullOrEmpty(directionId) && directionId!="All")
            {
                result = (from f in result where f.DirectionId == directionId select f);
            }
            if (!string.IsNullOrEmpty(transportmodeId) && transportmodeId != "All")
            {
                result = (from f in result where f.TransportModeId == transportmodeId select f);
            }

            return result;
        }
        
        public IQueryable<ShipmentDirectionTransmodeView> GetShipmentDataViewsForDirectionAndTransmodeDashboard(int tenant, string customerid)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);

            IQueryable<ShipmentDirectionTransmodeView> result = (from f in dataViewEntities.ShipmentDirectionTransmodeViews where f.Tenant == tenant && !f.IsCancelled select f);
            if (!string.IsNullOrEmpty(customerid))
            {
                result = (from f in result where f.CustomerId == customerid select f);
            }
            else
            {
                result = (from f in result where f.CustomerId !=null select f);
            }

            return result;
        }

        public IQueryable<ShipmentsCustomersDashboardView> GetShipmentDataViewsForCustomersDashboard(int tenant)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);

            IQueryable<ShipmentsCustomersDashboardView> result = (from f in dataViewEntities.ShipmentsCustomersDashboardViews where f.Tenant == tenant && !f.IsCancelled select f);


            return result;
        }

        public IQueryable<ShipmentFollowUpDataView> GetShipmentFollowUpDataViewByTenant(int tenant)
        {
            IShipmentFollowUpDataViewContext dataViewEntities = ShipmentFollowUpDataViewContext.GetContext(tenant);
            IQueryable<ShipmentFollowUpDataView> result = (from a in dataViewEntities.ShipmentFollowUpDataViews where a.Tenant == tenant && a.IsCancelled == false select a);
            return result;
        }

        public int GetShipmentDataCount(int tenant)
        {
            return (from a in context.Shipments
                    where a.Tenant == tenant && ((a.ShipmentLevelCode == "D") || (a.ShipmentLevelCode == "H"))
                    select a).Count();
        }

        public IQueryable<ShipmentDataView> GetShipmentViewsByTenantAndMasterId(string masterId, int tenant)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);
            return from a in dataViewEntities.ShipmentDataViews
                   where a.Tenant == tenant && a.MasterShipmentDataId == masterId && ((a.ShipmentLevelCode == "D") || (a.ShipmentLevelCode == "H"))
                   select a;
        }

        public IQueryable<Shipment> GetShipmentByTenant(int tenant, bool isClosed, string cardId)
        {
            IQueryable<Shipment> shipments = null;
            if (!string.IsNullOrEmpty(cardId))
            {
                shipments = from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                            where a.Tenant == tenant && a.IsOperationalClosed == isClosed && (a.ShipperId == cardId || a.AgentId == cardId)
                            select a;
            }
            else
            {
                shipments = from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                            where a.Tenant == tenant && a.IsOperationalClosed == isClosed
                            select a;
            }
            return shipments;
        }

        public List<Shipment> GetSpecificAmountOfShipmentsByTenant(int tenant, int skip, int take)
        {
            List<Shipment> shipments = (from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                                        where a.Tenant == tenant
                                        select a).OrderBy(d => d.Id).Skip(skip).Take(take).ToList();
            return shipments;
        }

        public int GetShipmentsCount(int tenant)
        {
            return (from a in context.Shipments
                    where a.Tenant == tenant
                    select a).Count();
        }

        public IQueryable<Shipment> GetShipmentByTenant(int tenant, string cardId)
        {
            IQueryable<Shipment> shipments = null;

            if (!string.IsNullOrEmpty(cardId))
            {
                shipments = from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                            where a.Tenant == tenant && (a.AgentId == cardId || a.ShipperId == cardId) && (a.ShipmentLevelCode == "H" || (a.ShipmentLevelCode == "D"))
                            select a;
            }
            else
            {
                shipments = from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                            where a.Tenant == tenant && (a.ShipmentLevelCode == "H" || (a.ShipmentLevelCode == "D"))
                            select a;

            }
            return shipments;
        }

        public IQueryable<Shipment> GetMasterByTenant(int tenant, string cardId)
        {
            IQueryable<Shipment> shipments = null;

            if (!string.IsNullOrEmpty(cardId))
            {
                shipments = from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                            where a.Tenant == tenant && (a.AgentId == cardId || a.ShipperId == cardId) && (a.ShipmentLevelCode == "D" || a.ShipmentLevelCode == "C")
                            select a;
            }
            else
            {
                shipments = from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                            where a.Tenant == tenant && (a.ShipmentLevelCode == "D" || a.ShipmentLevelCode == "C")
                            select a;
            }
            return shipments;
        }

        public IQueryable<Shipment> GetFirst20ShipmnetsByTenant(int tenant, string shipmentId)
        {
            IQueryable<Shipment> shipments = (from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                                              where a.Tenant == tenant && a.Id.CompareTo(shipmentId) <= 1 && a.Id.CompareTo(shipmentId) >= 0
                                              select a).Take(1);
            return shipments;
        }

        public string GetShipmentPrepaidCollectId(string shipmentId, int tenant, string chargesGroupCode)
        {
            if (chargesGroupCode == "FRT")
            {
                return (from a in context.Shipments where a.Tenant == tenant select a.FreightPrepaidCollectId).FirstOrDefault();
            }

            return (from a in context.Shipments where a.Tenant == tenant select a.OtherPrepaidCollectId).FirstOrDefault();
        }

        public Shipment GetFirstShipment(int tenant)
        {
            return (from a in context.Shipments.Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public int GetOpenShipmentsCountForCustomer(int tenant, string customerid)
        {
            int count = (from a in context.Shipments
                         where a.ShipmentLevelCode != "C" && a.Tenant == tenant && a.IsOperationalClosed == false && a.IsAccountingClosed == false && a.CustomerId == customerid && a.IsCancelled == false
                         select a).Count();
            return count;
        }

        public int GetAllShipmentsCountForCustomer(int tenant, string customerid)
        {
            int count = (from a in context.Shipments
                         where a.ShipmentLevelCode != "C" && a.Tenant == tenant && a.CustomerId == customerid && a.IsCancelled == false
                         select a).Count();
            return count;
        }

        public double GetOpenReceivablesForCustomer(int tenant, string customerid)
        {
            double? openReceivables = (from a in context.Shipments
                                       where a.CustomerId == customerid && a.Tenant == tenant && a.IsCancelled == false && a.IsOperationalClosed == false && a.IsAccountingClosed == false
                                       select a.OpenReceivablesInLocalCurrency).Sum();
            return openReceivables != null ? openReceivables.Value : 0;
        }

        public Shipment GetShipmentByMasterAndAirline(string myMasterNumber, string myAirlineId, int myTenant)
        {
            Shipment myEntity = null;

            myEntity = (from myShipment in context.Shipments
                        join db_Masters in context.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                        from myMasterData in ShipmentsMasters.DefaultIfEmpty()
                        where myShipment.Tenant == myTenant
                        && myShipment.ShipmentLevelCode != "H"
                        && myShipment.IsCancelled == false
                        && myMasterData.Master == myMasterNumber
                        && myMasterData.InterlineId == myAirlineId
                        select myShipment).FirstOrDefault();


            if (myEntity == null)
            {
                myEntity = (from myShipment in context.Shipments
                            join db_Masters in context.ShipmentMasterDatas on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                            from myMasterData in ShipmentsMasters.DefaultIfEmpty()
                            where myShipment.Tenant == myTenant
                            && myShipment.ShipmentLevelCode != "H"
                            && myShipment.IsCancelled == false
                            && myMasterData.Master == myMasterNumber
                            && myMasterData.MainCarriageCarrierId == myAirlineId
                            select myShipment).FirstOrDefault();
            }

            return myEntity;
        }

        public Shipment GetShipmentByHouseAndAirline(string myMaster, string myHouse, string myAirlineId, int myTenant)
        {
            Shipment myEntity = null;

            Shipment myMasterData = this.GetShipmentByMasterAndAirline(myMaster, myAirlineId, myTenant);

            if (myMasterData != null)
            {
                myEntity = (from a in context.Shipments
                            where
                            a.Tenant == myTenant
                            && a.IsCancelled == false
                            && a.ShipmentLevelCode == "H"
                            && a.House == myHouse
                            && a.MasterShipmentDataId == myMasterData.Id
                            select a).FirstOrDefault();
            }

            return myEntity;
        }

        public Shipment GetShipmentByFSRData(string master, string airlineId, string directionId, int tenant)
        {
            Shipment shipment = (from a in context.Shipments.Include("ShipmentMasterData").Include("FromPort").Include("ToPort").Include("ProfitCurrency").Include("CustomerCard").Include("EntityStatus").Include("ShipmentType").Include("Incoterm").Include("ShipmentReceivableStatus").Include("ShipmentLevel").Include("NextLeg").Include("ShipmentType")
                                 where a.ShipmentMasterData.Master == master
                                 && a.Tenant == tenant
                                 && a.ShipmentMasterData.MainCarriageCarrierId == airlineId
                                 && a.DirectionId == directionId
                                 && a.TransportModeId == "A"
                                 select a).FirstOrDefault();
            return shipment;
        }

        public List<Shipment> GetShipmentsListFromIdList(List<string> ids, int tenant)
        {
            List<Shipment> shipments = new List<Shipment>();

            if (ids.Count > 0)
            {
                shipments = (from a in context.Shipments
                             where a.Tenant == tenant && ids.Contains(a.Id)
                             select a).ToList();
            }

            return shipments;
        }
        
        public List<ShipmentDataView> GetShipmentsFromIdList(List<string> ids, int tenant)
        {
            IShipmentDataViewContext shipmentdataviewcontext = ShipmentDataViewContext.GetContext(tenant);
            List<ShipmentDataView> shipments = new List<ShipmentDataView>();
            //List<ShipmentDataView> shipments = (from a in shipmentdataviewcontext.ShipmentDataViews
            //                                    where a.Tenant == tenant && ids.Contains(a.Id)
            //                                    select a).ToList();
            if (ids.Count() != 0)
            {
                var values = new StringBuilder();
                values.AppendFormat("{0}", "'" + ids[0] + "'");
                for (int i = 1; i < ids.Count; i++)
                    values.AppendFormat(", {0}", "'" + ids[i] + "'");

                var sql = string.Format(
                    "SELECT * FROM ShipmentDataView WHERE id IN ({0})",
                    values);


                shipments = shipmentdataviewcontext.GetActiveDbContext().Database.SqlQuery<ShipmentDataView>(sql).ToList();
            }

            return shipments;
        }

        public IQueryable<Shipment> GetShipmentsForUnpaidInvoicesReport(List<string> shipmentIds)
        {
            IQueryable<Shipment> shipments = from a in context.Shipments.Include("ShipperCard").Include("ShipmentMasterData").Include("ToPort")
                                             where shipmentIds.Contains(a.Id)
                                             select a;
            return shipments;
        }

        public void Add(Shipment entity)
        {
            context.Shipments.Add(entity);
        }

        public void Remove(Shipment entity)
        {
            context.Shipments.Attach(entity);
            context.Shipments.Remove(entity);
        }

        public void Update(Shipment entity)
        {
            try
            {
                context.Shipments.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Shipment> All()
        {
            return context.Shipments.ToList();
        }

        public IShipmentsContext context
        {
            get { return shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Shipment> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Shipment GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ShipmentDataView> GetOceanDirectShipmentsByTenant(int tenant)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);
            IQueryable<ShipmentDataView> result = (from f in dataViewEntities.ShipmentDataViews
                                                   where f.Tenant == tenant && f.TransportModeId == "O" && (f.ShipmentLevelCode == "D" || f.ShipmentLevelCode == "C")
                                                   select f);

            return result;
        }

        public bool IsCustomerConnectedToShipments(string customerId, int tenant)
        {
            return (from f in context.Shipments where f.CustomerId == customerId && f.Tenant == tenant select f).Any();
        }

        public ShipmentDataView GetSingleShipmentDataView(string shipmentId, int tenant)
        {
            IShipmentDataViewContext dataViewEntities = ShipmentDataViewContext.GetContext(tenant);

            ShipmentDataView result = (from f in dataViewEntities.ShipmentDataViews where f.Id == shipmentId && f.Tenant == tenant select f).FirstOrDefault();

            return result;
        }

        public ShipmentFollowUpDataView GetSingleShipmentFollowupDataView(string shipmentId, int tenant)
        {
            IShipmentFollowUpDataViewContext dataViewEntities = ShipmentFollowUpDataViewContext.GetContext(tenant);

            ShipmentFollowUpDataView result = (from f in dataViewEntities.ShipmentFollowUpDataViews where f.Id == shipmentId && f.Tenant == tenant select f).FirstOrDefault();

            return result;
        }

        public IQueryable<Shipment> GetByCreateDate(DateTime StartDate, DateTime EndDate)
        {
            return context.Shipments.Where(a => (a.CreateDateTime >= StartDate && a.CreateDateTime < EndDate));
        }

        public IQueryable<ShipmentDataView> GetSentAWBShipmentDataViews(int tenant)
        {
            IShipmentDataViewContext dataViewContext = ShipmentDataViewContext.GetContext(tenant);
            return dataViewContext.ShipmentDataViews.Where(s => s.TransportModeId == "A" && s.Tenant == tenant).Where(d => (d.ShipmentLevelCode == "H" && d.FHLStatusDate != null) || (d.ShipmentLevelCode != "H" && d.FWBStatusDate != null));
        }

        public IQueryable<ShipmentDataView> GetShipmentsByQuoteId(string quoteId, int tenant)
        {
            IShipmentDataViewContext dataViewContext = ShipmentDataViewContext.GetContext(tenant);

            return (from record in dataViewContext.ShipmentDataViews
                    where record.Tenant == tenant && record.QuoteId == quoteId
                    select record);
        }
        
        public string GetShipmentIdByShipmentNumber(string shipmentNumber, int tenant)
        {
            return (from f in context.Shipments where f.ShipmentNumber == shipmentNumber && f.Tenant == tenant select f.Id).FirstOrDefault();
        }

        public string GetShipmentNumberByShipmentIdTenant(string shipmentId, int tenant)
        {
            return (from a in context.Shipments where a.Id == shipmentId && a.Tenant == tenant select a.ShipmentNumber).FirstOrDefault();
        }

        public string GetForwarderShipmentNumberByShipmentNumberTenant(string shipmentNumber, int tenant)
        {
            return (from a in context.Shipments where a.ShipmentNumber == shipmentNumber && a.Tenant == tenant select a.ForwarderShipmentNumber).FirstOrDefault();
        }

        public string GetForwarderShipmentNumberByShipmentIdTenant(string id, int tenant)
        {
            return (from a in context.Shipments where a.Id == id && a.Tenant == tenant select a.ForwarderShipmentNumber).FirstOrDefault();
        }
        
        public string GetAgentContactByShipemntId(string shipmentId, int tenant)
        {
            return (from a in context.Shipments where a.Id == shipmentId && a.Tenant == tenant select a.AgentContactId).FirstOrDefault();


        }
        
        public IQueryable<ShipmentDataView> GetSentAWBShipmentsForAWBReport(int tenant)
        {
            IShipmentDataViewContext dataViewContext = ShipmentDataViewContext.GetContext(tenant);
            return dataViewContext.ShipmentDataViews.Where(s => s.TransportModeId == "A").Where(d => (d.ShipmentLevelCode == "H" && d.FHLStatusDate != null) || (d.ShipmentLevelCode != "H" && d.FWBStatusDate != null));
        }

        public Shipment GetShipmentByAgentSharedManifestRef(string agentSharedManifestRef, int tenant)
        {

            return (from a in context.Shipments
                    where a.Tenant == tenant && a.AgentSharedManifestRef == agentSharedManifestRef
                    select a).FirstOrDefault();



        }

        public IQueryable<Shipment> GetConnectedHouses(string masterId, int tenant)
        {
            return (from a in context.Shipments
                    where a.Tenant == tenant
                    && a.MasterShipmentDataId == masterId
                    && a.ShipmentLevelCode == "H"
                    select a);
        }
        
        public IQueryable<Shipment> GetSharingShipments(int tenant)
        {
           return (from a in context.Shipments where a.Tenant == tenant && a.ManifestLastSharingDate!=null &&  a.IsManifestSentToAgent select a);
        }
        
        public bool IsQuoteConnectedToShipment(string quoteId, int tenant)
        {
            return (from a in context.Shipments where a.QuoteId == quoteId && a.Tenant == tenant select a).Any();


        }

        public bool IsShipmentAccountingClosed(string shipmentId)
        {
            bool myResult = false;

            if (!string.IsNullOrEmpty(shipmentId))
            {
                Shipment iShipment = (from a in context.Shipments where a.Id == shipmentId select a).FirstOrDefault();
                if (iShipment != null)
                {
                    myResult = iShipment.IsAccountingClosed;
                }
            }

            return myResult;
        }

        public IQueryable<Shipment> GetMissingMasterDataShipments()
        {
            return (from d in context.Shipments
                    where d.ShipmentLevelCode != "H"
                    && d.MasterShipmentDataId == null
                    select d).OrderBy(o => o.CreateDateTime).Take(20);
        }

        public int GetHouseShipmentsCountForMaster(string masterId, int tenant)
        {
            int count = (from s in context.Shipments
                                       where s.Tenant == tenant
                                       && s.Id != masterId
                                       && s.MasterShipmentDataId == masterId
                                       select s).Count();
            return count;
        }
    }
}
