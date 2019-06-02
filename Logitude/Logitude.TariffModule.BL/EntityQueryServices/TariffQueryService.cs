using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Logitude.TariffModule.BL.DataContracts;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Logitude.TariffModule.BL.EntityQueryServices
{
    public partial class TariffQueryService
   {



        public override void GetComposition(EntityKeyFields entityKeys, TariffPM entityPM)
        {
            ITariffModuleContext context = MainContext as ITariffModuleContext; 
            TariffKeys tariffKeys = entityKeys as TariffKeys;
            
            TariffVersionQueryService tariffVersionQueryService = new TariffVersionQueryService(context);
            entityPM.TariffVersions = tariffVersionQueryService.GetMulti(tariffKeys, true);
        }



        public TariffsSummary GetCount(int tenant)
        {
            TariffsSummary tariffsSummary = new TariffsSummary() { Id = tenant };
            tariffsSummary.AirFreightCount=this.repository.GetAll(tenant).Where(p => p.TypeCode == "AFC").Count();
            tariffsSummary.AirSurchargeCount = this.repository.GetAll(tenant).Where(p => p.TypeCode == "ASC").Count();
            return tariffsSummary;
        }


        public List<TariffSearchSummary> GetTariffSearchSummary(string fromport,string toport,DateTime? BetweenDate,double weight,int tenant)
        {
            AirlineRepository airlineRepository = new AirlineRepository(tenant);
            AirlineQuery airlineQuery = new AirlineQuery(airlineRepository);
            List<TariffSearchSummary> tariffSearchSummaries = new List<TariffSearchSummary>();
          IQueryable<TariffLine> iQueryable =  this.repository.GetAllTariffLines(tenant);
            iQueryable = iQueryable.Where(p => p.OriginPortId == fromport && p.DestinationPortId == toport && System.Data.Entity.DbFunctions.TruncateTime(p.StartDate) <= BetweenDate && System.Data.Entity.DbFunctions.TruncateTime(p.ExpirationDate) >= BetweenDate);
            TariffSettingRepository tariffSettingRepository = new TariffSettingRepository(tenant);
            List<TariffSetting> setting = tariffSettingRepository.GetAll(tenant).ToList();
            List<string> Steps = new List<string>();
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);

            int propIndex = -1;
            if (setting != null)
            {
                if (setting.Count > 0)
                {
                    Steps = setting[0].DefaultPriceSteps.Split(',').ToList();
                    int index = 0;
                    Steps.ForEach(item =>
                    {
                        if (float.Parse(item) > weight)
                        {
                            propIndex = index;
                            return;
                        }

                        else if (float.Parse(item) == weight)
                        {
                            propIndex = ++index;
                            return;
                        }

                        else
                        {
                            index++;
                        }

                    });

                    if (propIndex == -1)
                    {
                        propIndex = Steps.Count;
            }
                    else if (propIndex == 0)
                    {
                        propIndex = 1;
                    }

                }
          
            }

          

            List<TariffResult> items = new List<TariffResult>() ;
            if (propIndex == 1)
            {
                 items = (from item in iQueryable 
                                                       group iQueryable by new 
                                                       {
                                                           item.TariffId,
                                                           item.Step1Price,
                                                           item.Version,

                                                       } into g 
                                                       select new TariffResult()
                                                       {
                                                           price = g.Min(p=>g.Key.Step1Price),
                                                           tariffid = g.Key.TariffId,
                                                           TariffVersion = g.Key.Version,


                                                       }).ToList();
            }
            else if (propIndex == 2)
            {
                items = (from item in iQueryable
                         group iQueryable by new
                         {
                             item.TariffId,
                             item.Step2Price,
                             item.Version,

                         } into g
                         select new TariffResult()
                         {
                             price = g.Min(p => g.Key.Step2Price),
                             tariffid = g.Key.TariffId,
                             TariffVersion=g.Key.Version,
                         }).ToList();
            }

            else if(propIndex == 3)
            {
                items = (from item in iQueryable
                         group iQueryable by new
                         {
                             item.TariffId,
                             item.Step3Price,
                             item.Version,

                         } into g
                         select new TariffResult()
                         {
                             price = g.Min(p => g.Key.Step3Price),
                             tariffid = g.Key.TariffId,
                             TariffVersion = g.Key.Version,


                         }).ToList();
            }

            else if (propIndex == 4)
            {
                items = (from item in iQueryable
                         group iQueryable by new
                         {
                             item.TariffId,
                             item.Step4Price,
                             item.Version,

                         } into g
                         select new TariffResult()
                         {
                             price = g.Min(p => g.Key.Step4Price),
                             tariffid = g.Key.TariffId,
                             TariffVersion = g.Key.Version,


                         }).ToList();
            }

            else if (propIndex == 5)
            {
                items = (from item in iQueryable
                         group iQueryable by new
                         {
                             item.TariffId,
                             item.Step5Price,
                             item.Version,

                         } into g
                         select new TariffResult()
                         {
                             price = g.Min(p => g.Key.Step5Price),
                             tariffid = g.Key.TariffId,
                             TariffVersion = g.Key.Version,


                         }).ToList();
            }

            else if (propIndex == 6)
            {
                items = (from item in iQueryable
                         group iQueryable by new
                         {
                             item.TariffId,
                             item.Step6Price,
                             item.Version,

                         } into g
                         select new TariffResult()
                         {
                             price = g.Min(p => g.Key.Step6Price),
                             tariffid = g.Key.TariffId,
                             TariffVersion = g.Key.Version,


                         }).ToList();
            }

            else if (propIndex == 7)
            {
                items = (from item in iQueryable
                         group iQueryable by new
                         {
                             item.TariffId,
                             item.Step7Price,
                             item.Version,

                         } into g
                         select new TariffResult()
                         {
                             price = g.Min(p => g.Key.Step7Price),
                             tariffid = g.Key.TariffId,
                             TariffVersion = g.Key.Version,


                         }).ToList();
            }

            else if (propIndex == 8)
            {
                items = (from item in iQueryable
                         group iQueryable by new
                         {
                             item.TariffId,
                             item.Step8Price,
                             item.Version,

                         } into g
                         select new TariffResult()
                         {
                             price = g.Min(p => g.Key.Step8Price),
                             tariffid = g.Key.TariffId,
                             TariffVersion = g.Key.Version,


                         }).ToList();
            }


            List<Tariff> TariffList = this.repository.GetAllTariff(items.Select(p => p.tariffid).ToArray(), tenant).Where(p=>!p.InActive).ToList();
            List<TariffVersion> TariffVersionList = this.repository.GetAllTariffVersionsByTariffIds(items.Select(p => p.tariffid).ToArray(), tenant).ToList();
            Dictionary<string, string> Currencies = myCommonContext.Currencies.Where(p => p.Tenant == tenant).ToList();



            foreach (Tariff result in TariffList)
            {
                List<TariffResult> resultItems = items.Where(x =>  x.tariffid==result.Id && TariffVersionList.Where(a=>a.Version==x.TariffVersion && a.TariffId==result.Id).FirstOrDefault()!=null).ToList();
                TariffResult item = resultItems.Where(x => x.price == resultItems.Min(y => y.price)).FirstOrDefault();
                if (item != null)
                {
                        TariffSearchSummary tariffsSummary = new TariffSearchSummary() { Id = result.Id };
                                tariffsSummary.price = Math.Round((double)item.price, 2).ToString("0.00");


                    AirlinePM airline = airlineQuery.GetSinglePM(result.SellerId, tenant);
                                tariffsSummary.Name = airline.Card!=null?airline.Card.EnglishName:"";
                                tariffsSummary.EffictiveDate = result.ExpirationDate;
                                tariffsSummary.Remarks = result.Description;
                    tariffsSummary.decimalprice = item.price;
                    byte[] filedata = DownloadFile(airline.ImageDetailId, "jpg", tenant, "images");
                    string resultImage = "";
                    if (filedata != null)
                    {

                            resultImage = "data:image/" + "jpg" + ";base64," + Convert.ToBase64String(filedata);
                        
                    }

                    tariffsSummary.ImageId = resultImage;


                    tariffsSummary.Currency=result.CurrencyId
                    tariffSearchSummaries.Add(tariffsSummary);

                }

            }
            tariffSearchSummaries= tariffSearchSummaries.OrderBy(p => p.decimalprice).ToList();
            return tariffSearchSummaries;
        }

        private byte[] DownloadFile(string documentId,string type,int tenant,string fileLocation) {
            string fileName = documentId + ".jpg";
            string filePath = "tenant" + tenant.ToString() + "/" + StorageAcountDetails.GetBlobNameByLocation(fileName.ToLower(), "fileLocation");
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = documentId,
                FolderName = fileLocation,
                Extension = "jpg",
                Tenant = tenant,
                


            };
            byte[] datainByte = storageservice.Read(fileInfo);
            return datainByte;
        }
    }

    public class TariffResult
    {
        public string tariffid { get; set; }
        public int TariffVersion { get; set; }
        public decimal? price { get; set; }
    }
}
	 