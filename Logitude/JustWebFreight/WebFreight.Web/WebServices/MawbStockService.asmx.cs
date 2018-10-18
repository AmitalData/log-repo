using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for MawbStockService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MawbStockService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetAirlineAvailableStockSeries(string airlineId, int tenant)
        {
            MAWBStackQuery stackQuery = new MAWBStackQuery(tenant);

            List<MAWBStackPM> availableStocks = stackQuery.GetNotAssignedMAWBStackPMsByAirlineId(airlineId, tenant).OrderBy(s => s.Number).ToList();

            StockSeriesListClass result = new StockSeriesListClass();

            List<StockSeries> list = GetSeriesList(availableStocks);
            result.StockSeriesList = list;


            XmlSerializer serializer = new XmlSerializer(typeof(StockSeriesListClass));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, result);
            memstream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = memstream.ToArray();

            return bytearray;
 
        }

        private List<StockSeries> GetSeriesList(List<MAWBStackPM> availableStocks)
        {
            StockSeries currentSeries = null;
            List<StockSeries> seriesList = new List<StockSeries>();

            bool isNewSeries = true;
            for (int i = 0; i < availableStocks.Count; i++)
            {


                if (isNewSeries)
                {

                    currentSeries = new StockSeries()
                    {
                        From = availableStocks[i].Number,
                        AirlineId = availableStocks[i].AirlineId,
                        AirlineName = availableStocks[i].AirlineName,
                        CustomerId = availableStocks[i].AssignedToId,
                        Tenant = availableStocks[i].Tenant,
                        To = availableStocks[i].Number,
                    };

                    seriesList.Add(currentSeries);

                    isNewSeries = false;

                    if (i < availableStocks.Count - 1)
                    {
                        int currentnumber = GetStockNumber(availableStocks[i].Number);//int.Parse(currStockString.Substring(0, currStockString.Length - 1));
                        int nextnumber = GetStockNumber(availableStocks[i + 1].Number);//int.Parse(nextStockString.Substring(0, nextStockString.Length - 1));

                        if ((nextnumber - currentnumber) > 1)
                        {
                            currentSeries.To = availableStocks[i].Number;

                            isNewSeries = true;
                        }

                    }
                    //else
                    //{
                    //    currentSeries.To = availableStocks[i].Number;
                    //    isNewSeries = true;
                    //}

                }
                else
                {
                    if (i < availableStocks.Count - 1)
                    {
                        //string currStockString = GetStockAsString(availableStocks[i].Number);//availableStocks[i].Number.ToString();
                       // string nextStockString = GetStockAsString(availableStocks[i + 1].Number);//availableStocks[i + 1].Number.ToString();

                        int currentnumber = GetStockNumber(availableStocks[i].Number);//int.Parse(currStockString.Substring(0, currStockString.Length - 1));
                        int nextnumber = GetStockNumber(availableStocks[i + 1].Number);//int.Parse(nextStockString.Substring(0, nextStockString.Length - 1));

                        if ((nextnumber - currentnumber) > 1)
                        {
                            currentSeries.To = availableStocks[i].Number;

                            isNewSeries = true;
                        }

                    }
                    else
                    {
                        currentSeries.To = availableStocks[i].Number;
                        isNewSeries = true;
                    }


                }

                //if (isNewSeries || availableStocks.Count == 1)
                //{

                //  //  string fromstring = GetStockAsString(currentSeries.From);
                   // string tostring = GetStockAsString(currentSeries.To);
                    int from = GetStockNumber(currentSeries.From);//int.Parse(fromstring.Substring(0, fromstring.Length - 1));
                    int to = GetStockNumber(currentSeries.To);//int.Parse(tostring.Substring(0, tostring.Length - 1));

                    currentSeries.Total = (to - from) + 1;

                //}

            }




            return seriesList;
        }

        private int GetStockNumber(int number)
        {
           string numberstring = String.Format("{0:D8}", number);
           return int.Parse(numberstring.Substring(0, 7));
             
        }

        [WebMethod]
        public byte[] GetCustomerStockSeries(string customerId, int tenant)
        {
            MAWBStackQuery stackQuery = new MAWBStackQuery(tenant);

            StockSeriesListClass result = new StockSeriesListClass() { StockSeriesList = new List<StockSeries>() };

            IQueryable<IGrouping<string, MAWBStackPM>> availableStocks = stackQuery.GetAssignedMAWBStackPMsByCustomerId(customerId, tenant).GroupBy(s => s.AirlineId);

            foreach (IGrouping<string, MAWBStackPM> group in availableStocks)
            {
                List<MAWBStackPM> ordered = group.OrderBy(s => s.Number).ToList();

                List<StockSeries> series = GetSeriesList(ordered);

                foreach (StockSeries ser in series)
                {
                    result.StockSeriesList.Add(ser);
                }

            }




            XmlSerializer serializer = new XmlSerializer(typeof(StockSeriesListClass));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, result);
            memstream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = memstream.ToArray();

            return bytearray;

        }

        [WebMethod]
        public byte[] GetAllAvailableStockSeries(int tenant)
        {
            MAWBStackQuery stackQuery = new MAWBStackQuery(tenant);

            StockSeriesListClass result = new StockSeriesListClass() { StockSeriesList = new List<StockSeries>() };

            IQueryable<IGrouping<string, MAWBStackPM>> availableStocks = stackQuery.GetAllNotAssignedMAWBStackPMs(tenant).GroupBy(s => s.AirlineId);

            foreach (IGrouping<string, MAWBStackPM> group in availableStocks)
            {
                List<MAWBStackPM> ordered = group.OrderBy(s => s.Number).ToList();

                List<StockSeries> series = GetSeriesList(ordered);

                foreach (StockSeries ser in series)
                {
                    result.StockSeriesList.Add(ser);
                }

            }




            XmlSerializer serializer = new XmlSerializer(typeof(StockSeriesListClass));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, result);
            memstream.Seek(0, SeekOrigin.Begin);
            byte[] bytearray = memstream.ToArray();

            return bytearray;

        }

        [WebMethod]
        public void AssignStockSeriesToCustomer(int start, int end, string airlineId, string customerId,int tenant)
        {
            MAWBStackQuery stackQuery = new MAWBStackQuery(tenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            List<MAWBStack> availableStocks = (from a in commonContext.MAWBStacks
                                                where a.Tenant == tenant && a.AirlineId == airlineId &&(a.Number >= start && a.Number <= end)
                                                select a).ToList();
            //commonContext.MAWBStacks.Where(s=>s.AirlineId == airlineId && s.Tenant == tenant).OrderBy(s => s.Number).
            //List<MAWBStackPM> availableStocks = stackQuery.GetAvailableMAWBStackPMsByAirlineId(airlineId, tenant).OrderBy(s => s.Number).ToList();

            //List<MAWBStackPM> range = availableStocks.Where(s => (s.Number >= from && s.Number <= to) && s.AirlineId == airlineId).ToList();

            foreach (MAWBStack stack in availableStocks)
            {
                stack.AssignedToId = customerId;
            }


            commonContext.SaveChanges();

        }

        [WebMethod]
        public void UnAssignStockSeriesToUser(int start, int end, string airlineId, int tenant)
        {
            MAWBStackQuery stackQuery = new MAWBStackQuery(tenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            List<MAWBStack> availableStocks = (from a in commonContext.MAWBStacks
                                               where a.Tenant == tenant && a.AirlineId == airlineId && (a.Number >= start && a.Number <= end)
                                               select a).ToList();
            //commonContext.MAWBStacks.Where(s=>s.AirlineId == airlineId && s.Tenant == tenant).OrderBy(s => s.Number).
            //List<MAWBStackPM> availableStocks = stackQuery.GetAvailableMAWBStackPMsByAirlineId(airlineId, tenant).OrderBy(s => s.Number).ToList();

            //List<MAWBStackPM> range = availableStocks.Where(s => (s.Number >= from && s.Number <= to) && s.AirlineId == airlineId).ToList();

            foreach (MAWBStack stack in availableStocks)
            {
                stack.AssignedToId = null;
            }


            commonContext.SaveChanges();

        }        
    }
}
