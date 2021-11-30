using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for LoadNewPortsWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LoadNewPortsWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string LoadPorts(string file)
        {
            ICommonDataContext objectContext = CommonDataContext.GetContext(0);

            Simplog.Data.CommonDataModel.Repositories.CountryRepository countryRepository = new Simplog.Data.CommonDataModel.Repositories.CountryRepository(objectContext);
            PortRepository portRepository = new PortRepository(objectContext);
            GlobalZoneRepository globalZoneRepository = new GlobalZoneRepository(objectContext);

            List<Simplog.Data.CommonDataModel.EntityPOCOs.Country> countries = countryRepository.GetCountries(0).ToList<Simplog.Data.CommonDataModel.EntityPOCOs.Country>();
            List<Port> ports = portRepository.GetPorts(0).ToList();
            List<GlobalZone> globalZones = globalZoneRepository.GetGlobalZones(0).ToList();

            int counter = 0;
            string[] mynewline = null;
            string[] readData2 = null;
            string countryCode = "";
            string portCode = "";
            string portEnglishName = "";
            bool isAir;
            bool isOcean;
            bool isinland;

            mynewline = file.Split('\n');
            mynewline = mynewline.Where(x => x != "/r" && x != "").ToArray();

            foreach (string line in mynewline)
            {
                counter++;
                if (line != "\r" && line != "")
                {
                    readData2 = line.Split(',');
                    portCode = readData2[0].Trim();
                    portEnglishName = readData2[1].TrimEnd();
                    isAir = readData2[3] == "0" ? false : true;
                    isOcean = readData2[4] == "0" ? false : true;
                    isinland = readData2[5] == "0" ? false : true;
                    countryCode = readData2[6].Trim();

                    if (!string.IsNullOrEmpty(portCode))
                    {
                        bool portexists = (from a in ports
                                           where a.Code == portCode && a.Country.Code == countryCode
                                           select a).Any();

                        if (!portexists)
                        {
                            Port newPort = new Port();
                            newPort.Id = IdCounter.GetNumber("Port", 0).ToString();

                            newPort.Code = portCode;
                            newPort.IsAir = isAir;
                            newPort.IsInland = isinland;
                            newPort.IsOcean = isOcean;
                            newPort.InActive = false;
                            newPort.AddedManually = false;
                            newPort.Tenant = 0;
                            newPort.Latitude = 0;
                            newPort.Longtitude = 0;

                            if (portEnglishName.Count() >= 40)
                            {
                                newPort.EnglishName = portEnglishName.Substring(0, 40);
                                newPort.LocalName = portEnglishName.Substring(0, 40);
                            }
                            else
                            {
                                newPort.EnglishName = portEnglishName;
                                newPort.LocalName = portEnglishName;
                            }                            

                            Simplog.Data.CommonDataModel.EntityPOCOs.Country country = countries.Where(c => c.Code == countryCode).FirstOrDefault();

                            if (newPort.Code.Length == 3 && newPort.EnglishName.Length >= 1 && country != null)
                            {
                                newPort.CountryId = country.Id;
                                newPort.Country = country;
                                UpdatePortSearchFieldService.Update(newPort);
                                portRepository.Add(newPort);
                            }
                        }
                        //else
                        //{
                        //    Port oldport = (from a in ports
                        //                    where a.Code == portCode && a.Country.Code == countryCode
                        //                    select a).FirstOrDefault();
                        //    if (oldport != null)
                        //    {
                        //        oldport.IsAir = isAir;
                        //        oldport.IsInland = isinland;
                        //        oldport.IsOcean = isOcean;
                        //    }
                        //    portRepository.Update(oldport);
                        //}
                    }

                    if (counter == 1000)
                    {
                        counter = 0;
                        portRepository.SubmitChanges();
                    }
                }
            }
            portRepository.SubmitChanges();

            string s = string.Empty;
            return s;
        }
    }
}
