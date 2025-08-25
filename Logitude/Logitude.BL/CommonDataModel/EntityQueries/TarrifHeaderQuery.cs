using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TarrifHeaderQuery
    {
        TarrifHeaderRepository repository;



        public TarrifHeaderQuery(int tenant)
        {
            repository = new TarrifHeaderRepository(tenant);
        }

        public TarrifHeaderQuery(TarrifHeaderRepository repository)
        {
            this.repository = repository;
        }

        public TarrifHeaderPM GetSinglePM(string id, int tenant)
        {
            TarrifChargeQuery tarrifChargeQuery = new TarrifChargeQuery(tenant);
            TarrifFromToQuery tarrifFromToQuery = new TarrifFromToQuery(tenant);

            TarrifHeaderPM tarrifHeader = (from a in repository.context.TarrifHeaders
                                           where a.Id == id
                                           select new TarrifHeaderPM()
                                           {
                                               Id = a.Id,
                                               CardId = a.CardId,
                                               CreateDate = a.CreateDate,
                                               FromDate = a.FromDate,
                                               InActive = a.InActive,
                                               Notes = a.Notes,
                                               TarrifTypeCode = a.TarrifTypeCode,
                                               ToDate = a.ToDate,
                                               TransitTimeNotes = a.TransitTimeNotes,
                                               Tenant = a.Tenant,
                                           }).FirstOrDefault();

            tarrifHeader.TarrifCharges = tarrifChargeQuery.GetTarrifChargesByTarrifHeaderId(tarrifHeader.Id, tarrifHeader.Tenant).ToList();
            tarrifHeader.TarrifFromToes = tarrifFromToQuery.GetTarrifFromToByTarrifHeaderId(tarrifHeader.Id, tarrifHeader.Tenant).ToList();

            return tarrifHeader;
        }


        public TarrifHeaderPM GetSingleTarrifHeaderPM(string id, int tenant)
        {
            TarrifChargeQuery tarrifChargeQuery = new TarrifChargeQuery(tenant);
            TarrifFromToQuery tarrifFromToQuery = new TarrifFromToQuery(tenant);

            TarrifHeaderPM tarrifHeader = (from a in repository.context.TarrifHeaders
                                           where a.Id == id
                                           select new TarrifHeaderPM()
                                           {
                                               Id = a.Id,
                                               CardId = a.CardId,
                                               CreateDate = a.CreateDate,
                                               FromDate = a.FromDate,
                                               InActive = a.InActive,
                                               Notes = a.Notes,
                                               TarrifTypeCode = a.TarrifTypeCode,
                                               ToDate = a.ToDate,
                                               TransitTimeNotes = a.TransitTimeNotes,
                                               Tenant = a.Tenant,
                                           }).FirstOrDefault();

            tarrifHeader.TarrifCharges = tarrifChargeQuery.GetTarrifChargesByTarrifHeaderId(tarrifHeader.Id, tarrifHeader.Tenant).ToList();
            tarrifHeader.TarrifFromToes = tarrifFromToQuery.GetTarrifFromToByTarrifHeaderId(tarrifHeader.Id, tarrifHeader.Tenant).ToList();

            return tarrifHeader;
        }

        public IQueryable<TarrifHeaderPM> GetTarrifHeaderPMsByTenant(int tenant)
        {
            return from a in repository.context.TarrifHeaders
                   where a.Tenant == tenant
                   select new TarrifHeaderPM()
                   {
                       Id = a.Id,
                       CardId = a.CardId,
                       CreateDate = a.CreateDate,
                       FromDate = a.FromDate,
                       InActive = a.InActive,
                       Notes = a.Notes,
                       TarrifTypeCode = a.TarrifTypeCode,
                       ToDate = a.ToDate,
                       TransitTimeNotes = a.TransitTimeNotes,
                       Tenant = a.Tenant,
                   };
        }

        public List<TarrifHeaderPM> GetTarrifHeadersByCardIdAndTypeCode(string cardId, string typeCode, bool getAll, int tenant)
        {
            List<TarrifHeaderPM> result = new List<TarrifHeaderPM>();
            TarrifChargeQuery tarrifChargeQuery = new TarrifChargeQuery(tenant);
            TarrifFromToQuery tarrifFromToQuery = new TarrifFromToQuery(tenant);

            List<TarrifHeader> listOfAll = repository.context.TarrifHeaders.Where(d => d.Tenant == tenant && d.TarrifTypeCode == typeCode).ToList();
            List<TarrifHeader> listByCarrier = string.IsNullOrEmpty(cardId) ? listOfAll : listOfAll.Where(d => d.CardId == cardId).ToList();
            List<TarrifHeader> listByActive = getAll ? listByCarrier : listByCarrier.Where(d => d.InActive == false).ToList();

            foreach (TarrifHeader a in listByActive)
            {
                TarrifHeaderPM item = new TarrifHeaderPM()
                {
                    Id = a.Id,
                    CardId = a.CardId,
                    CreateDate = a.CreateDate,
                    FromDate = a.FromDate,
                    InActive = a.InActive,
                    Notes = a.Notes,
                    TarrifTypeCode = a.TarrifTypeCode,
                    ToDate = a.ToDate,
                    TransitTimeNotes = a.TransitTimeNotes,
                    Tenant = a.Tenant,
                };

                item.TarrifCharges = tarrifChargeQuery.GetTarrifChargesByTarrifHeaderId(item.Id, item.Tenant).ToList();
                item.TarrifFromToes = tarrifFromToQuery.GetTarrifFromToByTarrifHeaderId(item.Id, item.Tenant).ToList();

                List<TarrifFromToPM> fromTo = item.TarrifFromToes;
                if (fromTo.Count() == 0)
                {
                    item.FromLocationString = "Anywhere";
                    item.ToLocationString = "Anywhere";
                    item.FromLocationCode = "A";
                    item.ToLocationCode = "A";
                }

                else
                {
                    List<TarrifFromToPM> fromPorts = fromTo.Where(d => d.PortId != null && d.TarrifFromToTypeCode == "F").ToList();
                    List<TarrifFromToPM> fromCountrys = fromTo.Where(d => d.CountryId != null && d.TarrifFromToTypeCode == "F").ToList();
                    List<TarrifFromToPM> toPorts = fromTo.Where(d => d.PortId != null && d.TarrifFromToTypeCode == "T").ToList();
                    List<TarrifFromToPM> toCountrys = fromTo.Where(d => d.CountryId != null && d.TarrifFromToTypeCode == "T").ToList();

                    if (fromPorts.Count() > 0)
                    {
                        string str = String.Empty;
                        foreach (TarrifFromToPM t in fromPorts)
                        {
                            str = str + t.PortCode + ",";
                            item.FromLocationList.Add(t.PortCode);
                        }

                        item.FromLocationString = str;
                        item.FromLocationCode = "P";
                    }

                    if (fromCountrys.Count() > 0)
                    {
                        string str = String.Empty;
                        foreach (TarrifFromToPM t in fromCountrys)
                        {
                            str = str + t.CountryCode + ",";
                            item.FromLocationList.Add(t.CountryCode);
                        }

                        item.FromLocationString = str;
                        item.FromLocationCode = "C";
                    }

                    if (toPorts.Count() > 0)
                    {
                        string str = String.Empty;
                        foreach (TarrifFromToPM t in toPorts)
                        {
                            str = str + t.PortCode + ",";
                            item.ToLocationList.Add(t.PortCode);
                        }

                        item.ToLocationString = str;
                        item.ToLocationCode = "P";
                    }

                    if (toCountrys.Count() > 0)
                    {
                        string str = String.Empty;
                        foreach (TarrifFromToPM t in toCountrys)
                        {
                            str = str + t.CountryCode + ",";
                            item.ToLocationList.Add(t.CountryCode);
                        }

                        item.ToLocationString = str;
                        item.ToLocationCode = "P";
                    }
                }

                result.Add(item);
            }

            return result.OrderByDescending(d => d.CreateDate).ToList();
        }

        public IQueryable<TarrifHeaderList> GetIQueryableEntityList(IQueryable<TarrifHeader> iQueryable)
        {
            IQueryable<TarrifHeaderList> result = from a in iQueryable
                                                  select new TarrifHeaderList()
                                                  {
                                                      Id = a.Id,
                                                      CardId = a.CardId,
                                                      CreateDate = a.CreateDate,
                                                      FromDate = a.FromDate,
                                                      InActive = a.InActive,
                                                      Notes = a.Notes,
                                                      TarrifTypeCode = a.TarrifTypeCode,
                                                      ToDate = a.ToDate,
                                                      TransitTimeNotes = a.TransitTimeNotes,
                                                      Tenant = a.Tenant,
                                                  };
            return result;
        }
    }
}