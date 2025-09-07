using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class OceanInsightsRequestQuery
    {
        OceanInsightsRequestRepository repository;

        public OceanInsightsRequestQuery(int tenant)
        {
            repository = new OceanInsightsRequestRepository(tenant);
        }

        public OceanInsightsRequestQuery(OceanInsightsRequestRepository repository)
        {
            this.repository = repository;
        }

        public OceanInsightsRequestPM GetSinglePM(string id, int tenant)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.Tenant == tenant && a.Id == id
                                               select new OceanInsightsRequestPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         SCACCode = a.SCACCode,
                                                         Tenant = a.Tenant,
                                                         UpdateDate = a.UpdateDate,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                         FromPushPage = a.FromPushPage,
														 System = a.System,
                                                         IsClosed = a.IsClosed
											   }).FirstOrDefault();

              
            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsIdTenant(string id, int tenant)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.Tenant == tenant && a.OceanInsigntId == id
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage,
												   System = a.System,
                                                   IsClosed = a.IsClosed
                                               }).FirstOrDefault();


            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsId(string id)
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.OceanInsigntId == id
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage,
												   System = a.System,
                                                   IsClosed = a.IsClosed
                                               }).FirstOrDefault();


            return myResult;
        }

        public List<OceanInsightsRequestPM> GetAllByOceanInsightsId(string id)
        {
            List<OceanInsightsRequestPM> myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.OceanInsigntId == id
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage,
												   System = a.System,
                                                   IsClosed = a.IsClosed
                                               }).ToList();


            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(string ScacCode, string ContainerNo, int Tenant,string System="")
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.ContainerNumber == ContainerNo && a.SCACCode == ScacCode && a.Tenant == Tenant && ((string.IsNullOrEmpty(System) && string.IsNullOrEmpty(a.System)) || a.System== System) && !a.IsClosed
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage,
												   System = a.System,
                                                   IsClosed = a.IsClosed
                                               }).FirstOrDefault();


            return myResult;
        }

        public OceanInsightsRequestPM GetSinglePMByOceanInsightsByCareierScacBLNoTenant(string CarrierScac, string BLNumber, int Tenant,string System="")
        {
            OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
                                               where a.BLNumber == BLNumber && a.SCACCode == CarrierScac && a.Tenant == Tenant && ((string.IsNullOrEmpty(System) && string.IsNullOrEmpty(a.System)) || a.System == System) && !a.IsClosed
                                               select new OceanInsightsRequestPM()
                                               {
                                                   Id = a.Id,
                                                   ContainerNumber = a.ContainerNumber,
                                                   CreateDate = a.CreateDate,
                                                   OceanInsigntId = a.OceanInsigntId,
                                                   SCACCode = a.SCACCode,
                                                   Tenant = a.Tenant,
                                                   UpdateDate = a.UpdateDate,
                                                   Type = a.Type,
                                                   BLNumber = a.BLNumber,
                                                   FromPushPage = a.FromPushPage,
                                                   IsClosed = a.IsClosed
                                               }).FirstOrDefault();


            return myResult;
        }


		public OceanInsightsRequestPM GetSinglePMByOceanInsightsByScacCodeContainerNo(string ScacCode, string ContainerNo)
		{
			OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
											   where a.ContainerNumber == ContainerNo && a.SCACCode == ScacCode
											   select new OceanInsightsRequestPM()
											   {
												   Id = a.Id,
												   ContainerNumber = a.ContainerNumber,
												   CreateDate = a.CreateDate,
												   OceanInsigntId = a.OceanInsigntId,
												   SCACCode = a.SCACCode,
												   Tenant = a.Tenant,
												   UpdateDate = a.UpdateDate,
												   Type = a.Type,
												   BLNumber = a.BLNumber,
												   FromPushPage = a.FromPushPage,
												   System = a.System,
                                                   IsClosed = a.IsClosed
                                               }).FirstOrDefault();


			return myResult;
		}

		public OceanInsightsRequestPM GetSinglePMByOceanInsightsByCareierScacBLNo(string CarrierScac, string BLNumber)
		{
			OceanInsightsRequestPM myResult = (from a in repository.Context.OceanInsightsRequests
											   where a.BLNumber == BLNumber && a.SCACCode == CarrierScac
											   select new OceanInsightsRequestPM()
											   {
												   Id = a.Id,
												   ContainerNumber = a.ContainerNumber,
												   CreateDate = a.CreateDate,
												   OceanInsigntId = a.OceanInsigntId,
												   SCACCode = a.SCACCode,
												   Tenant = a.Tenant,
												   UpdateDate = a.UpdateDate,
												   Type = a.Type,
												   BLNumber = a.BLNumber,
												   FromPushPage = a.FromPushPage,
												   System = a.System,
                                                   IsClosed = a.IsClosed
                                               }).FirstOrDefault();


			return myResult;
		}

		public List<OceanInsightsRequestPM> GetOceanInsightsRequestPMs(int tenant)
        {
            List<OceanInsightsRequestPM> myResult = (from a in repository.Context.OceanInsightsRequests
                                                     where a.Tenant == tenant 
                                                     select new OceanInsightsRequestPM()
                                                     {
                                                         Id = a.Id,
                                                         ContainerNumber = a.ContainerNumber,
                                                         CreateDate = a.CreateDate,
                                                         OceanInsigntId = a.OceanInsigntId,
                                                         SCACCode = a.SCACCode,
                                                         Tenant = a.Tenant,
                                                         UpdateDate = a.UpdateDate,
                                                         Type = a.Type,
                                                         BLNumber = a.BLNumber,
                                                         FromPushPage = a.FromPushPage,
														 System = a.System,
                                                         IsClosed = a.IsClosed
                                                     }).ToList(); 
            return myResult;
        }

    }
}