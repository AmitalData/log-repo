using Logitude.BL.Security;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class VendorController : ApiController
    {

        public HttpResponseMessage PostAddNewVendorRequest(VE_MSG010_VendorInsertUpdateDeleteMessageRequestParams addNewVendorParams)
        {
            try
            {
                VE_MSG010_VendorInsertUpdateDeleteResponseData responseData = null;
                if (addNewVendorParams.TestCase != null && addNewVendorParams.TestCase.Type == "webservice" && addNewVendorParams.TestCase.Code != "Real Logic")
                {
                    responseData = new VE_MSG010_VendorInsertUpdateDeleteResponseData();
                    switch (addNewVendorParams.TestCase.Code)
                    {
                        case "Send Succeeded":
                            {
                                Random rand = new Random();
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;
                                responseData.ApplicationID = rand.Next(1000000).ToString();
                                break;
                            }
                        case "Send Failed":
                            {
                                responseData.HasException = true;
                                responseData.Succeeded = false;
                                responseData.UserMessage = "this is a test fail exception for add edit delete vendor message!";
                                break;
                            }
                        case "Send Succeeded With Warning":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = "this vendor already exists in customs do you want to continue?";
                                responseData.IsCustomWarning = true;
                                break;
                            }
                    }
                }
                else
                {

                    // use messageing service
                    var messagingService = new VE_MSG010_VendorInsertUpdateDeleteMessagingService();
                    responseData = messagingService.Send(addNewVendorParams);

                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            
        }

        public HttpResponseMessage PostAddNewVendorCommunicationRequest(VE_MSG013_VendorAddCommunicationDeviceRequestParams addNewVendorParams)
        {
            try
            {
                INF_MSG_GenericResponseData responseData = null;
                if (addNewVendorParams.TestCase != null && addNewVendorParams.TestCase.Type == "webservice" && addNewVendorParams.TestCase.Code != "Real Logic")
                {
                    responseData = new INF_MSG_GenericResponseData();
                    switch (addNewVendorParams.TestCase.Code)
                    {
                        case "Send Succeeded":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;
                                break;
                            }
                        case "Send Failed":
                            {
                                responseData.HasException = true;
                                responseData.Succeeded = false;
                                responseData.UserMessage = "this is a test fail exception for add new vendor communication message!";
                                break;
                            }
                    }
                }
                else
                {
                    // use messageing service
                    VE_MSG013_VendorAddCommunicationDeviceMessageService messagingService = new VE_MSG013_VendorAddCommunicationDeviceMessageService();
                    responseData = messagingService.Send(addNewVendorParams);
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSearchVendorRequest(VE_MSG051_VendorSearchByCustomsAgentRequestParams newSearchVendorParams)
        {

            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            string loggedUserEmail = authToken.Email;

            ICustomContext customContext = CustomContext.GetContext(tenant);
            VendorStatusQueryService statusQS = new VendorStatusQueryService(customContext);
            CustomsCountryQueryService countryQS = new CustomsCountryQueryService(customContext);
            SubCountryQueryService subCountryQS = new SubCountryQueryService(customContext);
            CommunicationTypeQueryService commTypeRepoeQS = new CommunicationTypeQueryService(customContext);

            try
            {
                VE_MSG052_VendorSearchResultsForCustomsAgentResponseData responseData = null;
                if (newSearchVendorParams.TestCase != null && newSearchVendorParams.TestCase.Type == "webservice" && newSearchVendorParams.TestCase.Code != "Real Logic")
                {
                    responseData = new VE_MSG052_VendorSearchResultsForCustomsAgentResponseData();
                    switch (newSearchVendorParams.TestCase.Code)
                    {
                        case "Return a list regarding search values":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;

                                responseData.VendorResults = GetTestVendorResultList(newSearchVendorParams, false);

                                responseData.NumberOfResult = responseData.VendorResults.Count;

                                break;
                            }
                        case "Return a list regarding search values and different result count":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;

                                responseData.VendorResults = GetTestVendorResultList(newSearchVendorParams, false);
                                Random rand = new Random();
                                responseData.NumberOfResult = responseData.VendorResults.Count + rand.Next(1, 20);
                                break;
                            }
                        case "Return a list neglecting search values":
                            {
                                responseData.HasException = false;
                                responseData.Succeeded = true;
                                responseData.UserMessage = null;

                                responseData.VendorResults = GetTestVendorResultList(newSearchVendorParams, true);

                                responseData.NumberOfResult = responseData.VendorResults.Count;
                                break;
                            }
                        case "Return an empty list":
                            {
                                responseData.HasException = true;
                                responseData.Succeeded = false;
                                responseData.UserMessage = "Search Returned an empty list(test)";

                                break;
                            }
                        case "Search failed":
                            {
                                responseData.HasException = true;
                                responseData.Succeeded = false;
                                responseData.UserMessage = "this is a test fail exception for search vendor!";
                                break;
                            }
                    }
                }
                else
                {
                    // use messageing service
                    VE_MSG051_VendorSearchByCustomsAgentMessagingService messagingService = new VE_MSG051_VendorSearchByCustomsAgentMessagingService();
                    responseData = messagingService.Send(newSearchVendorParams);

                    if (responseData.VendorResults != null)
                    {
                        // Fill names
                        if (responseData.VendorResults.Count > 0)
                        {
                            foreach (var item in responseData.VendorResults)
                            {
                                if (item.StatusCode != null) item.StatusName = statusQS.GetSingle(item.StatusCode, false, true).LocalName;
                                if (item.CountryCode != null) item.CountryName = countryQS.GetSingle(item.CountryCode, false, true).LocalName;
                                if (item.SubCountryCode != null) item.SubCountryName = subCountryQS.GetSingle(item.SubCountryCode, false, true).LocalName;

                                item.VendorCommunications.ForEach((el) =>
                                {
                                    if (el.CommunicationType != null)
                                        el.CommunicationTypeName = commTypeRepoeQS.GetSingle(el.CommunicationType, false, true).LocalName;
                                });
                            }
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public List<VendorResult> GetTestVendorResultList(VE_MSG051_VendorSearchByCustomsAgentRequestParams newSearchVendorParams, bool neglectValues)
        {
            #region list
            List<VendorResult> vendorResults = new List<VendorResult>()
                            {
                                new VendorResult()
                                {
                                    CityName="Ramallah",
                                    CountryCode="PS",
                                    DunsNumber="96857334",
                                    MainAddressLine="Ramallah-al bereh",
                                    PostalCode="970",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="4455665544",
                                    VendorName="Othman",
                                    VendorNumber="41526398",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    StatusCode = "1",
                                    InActive = true,
                                    IsPalestinian = true,
                                    Id = Guid.NewGuid().ToString(),
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Ramallah", CommunicationType="TE"},
                                        new VendorCommunicationResult(){ CommunicationAddress="Nablus", CommunicationType="AH"},
                                    },

                                },

                                 new VendorResult()
                                {
                                    CityName="Ramallah",
                                    CountryCode="PS",
                                    DunsNumber="96857334",
                                    MainAddressLine="Ramallah-al bereh",
                                    PostalCode="970",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="4455665544",
                                    VendorName="Othman",
                                    VendorNumber="4155598",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    StatusCode = "1",
                                    InActive = true,
                                    IsPalestinian = true,
                                    Id = Guid.NewGuid().ToString(),
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Ramallah", CommunicationType="TE"},
                                        new VendorCommunicationResult(){ CommunicationAddress="Nablus", CommunicationType="AH"},
                                    },

                                },

                                new VendorResult()
                                {
                                    CityName="Ramallah",
                                    CountryCode="PS",
                                    DunsNumber="112233",
                                    MainAddressLine="Ramallah-al bereh",
                                    PostalCode="970",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="4455665544",
                                    VendorName="Anas",
                                    VendorNumber="65235421",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    StatusCode = "1",
                                    InActive = true,
                                      IsPalestinian = true,
                                    Id = Guid.NewGuid().ToString(),
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Ramallah", CommunicationType="TE"},
                                        new VendorCommunicationResult(){ CommunicationAddress="Nablus", CommunicationType="AH"},
                                    },

                                },

                                new VendorResult()
                                {
                                    CityName="Berlin",
                                    CountryCode="GR",
                                    DunsNumber="9987745",
                                    MainAddressLine="berlin",
                                    PostalCode="56",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="654987",
                                    VendorName="Mark",
                                    VendorNumber="555222",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    StatusCode = "2",
                                    InActive = true,
                                      IsPalestinian = true,
                                    Id = Guid.NewGuid().ToString(),
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Berlin", CommunicationType="TE"},

                                    },

                                },

                                new VendorResult()
                                {
                                    CityName="London",
                                    CountryCode="GB",
                                    DunsNumber="654987654",
                                    MainAddressLine="London",
                                    PostalCode="98",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="321456",
                                    VendorName="Harry",
                                    VendorNumber="2220001",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    StatusCode = "1",
                                    InActive = false,
                                      IsPalestinian = true,
                                    Id = Guid.NewGuid().ToString(),
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="London", CommunicationType="TE"},
                                        new VendorCommunicationResult(){ CommunicationAddress="Manchester", CommunicationType="AH"},

                                    },

                                },

                                new VendorResult()
                                {
                                    CityName="Tokyo",
                                    CountryCode="JP",
                                    DunsNumber="8855225",
                                    MainAddressLine="Tokyo",
                                    PostalCode="110",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="555887",
                                    VendorName="Korosaki",
                                    VendorNumber="3365221",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    StatusCode = "4",
                                    InActive = false,
                                    Id = Guid.NewGuid().ToString(),
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Tokyo", CommunicationType="AH"},

                                    },

                                },

                                 new VendorResult()
                                {
                                    CityName="Jenin",
                                    CountryCode="PS",
                                    DunsNumber="988774455",
                                    MainAddressLine="Jenin",
                                    PostalCode="970",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="3332225554",
                                    VendorName="Masoud",
                                    VendorNumber="1002245",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    InActive = false,
                                    Id = Guid.NewGuid().ToString(),
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Jenin", CommunicationType="AH"},

                                    },

                                },

                                 new VendorResult()
                                {
                                    CityName="Dubi",
                                    CountryCode="AE",
                                    DunsNumber="988774455",
                                    MainAddressLine="Dubi",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="55447788",
                                    VendorName="Masoud",
                                    VendorNumber="30021",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Dubi", CommunicationType="AH"},

                                    },

                                },


                                 new VendorResult()
                                {
                                    CityName="Jenin",
                                    CountryCode="PS",
                                    DunsNumber="9345555",
                                    MainAddressLine="Jenin",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="45678765",
                                    VendorName="Alaa",
                                    VendorNumber="33451",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    SubCountryName = "Jenin",
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = true,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Jenin", CommunicationType="AH"},

                                    },

                                },

                                 new VendorResult()
                                {
                                    CityName="Jerusalem",
                                    CountryCode="PS",
                                    DunsNumber="4565454",
                                    MainAddressLine="Jerusalem",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="55447788",
                                    VendorName="Maram",
                                    VendorNumber="456754",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Jerusalem", CommunicationType="AH"},

                                    },

                                },

                                  new VendorResult()
                                {
                                    CityName="Nablus",
                                    CountryCode="PS",
                                    DunsNumber="455443",
                                    MainAddressLine="Nablus",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="Khaled",
                                    VendorNumber="6786",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Nablus", CommunicationType="AH"},

                                    },

                                },

                                    new VendorResult()
                                {
                                    CityName="Malisya",
                                    CountryCode="MY",
                                    DunsNumber="455443",
                                    MainAddressLine="Malisya",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="Lama",
                                    VendorNumber="6786",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Malisya", CommunicationType="AH"},

                                    },

                                },


                                  new VendorResult()
                                {
                                    CityName="China",
                                    CountryCode="CN",
                                    DunsNumber="455443",
                                    MainAddressLine="China",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="Abood",
                                    VendorNumber="6786",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="China", CommunicationType="AH"},

                                    },

                                },


                                  new VendorResult()
                                {
                                    CityName="Jordan",
                                    CountryCode="JD",
                                    DunsNumber="455443",
                                    MainAddressLine="Jordan",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="Doaa",
                                    VendorNumber="6786",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = true,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Jordan", CommunicationType="AH"},

                                    },

                                },

                                  new VendorResult()
                                {
                                    CityName="Syria",
                                    CountryCode="SY",
                                    DunsNumber="455443",
                                    MainAddressLine="Syria",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="Fatenah",
                                    VendorNumber="6786",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                   InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Syria", CommunicationType="AH"},

                                    },

                                },
                                  new VendorResult()
                                {
                                    CityName="Malisya",
                                    CountryCode="MY",
                                    DunsNumber="889988",
                                    MainAddressLine="Malisya",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="joker",
                                    VendorNumber="5544",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Malisya", CommunicationType="AH"},

                                    },

                                },

                                  new VendorResult()
                                {
                                    CityName="Malisya",
                                    CountryCode="MY",
                                    DunsNumber="22222",
                                    MainAddressLine="Malisya",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="Zaki",
                                    VendorNumber="6655",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Malisya", CommunicationType="AH"},

                                    },

                                },

                                    new VendorResult()
                                {
                                    CityName="Malisya",
                                    CountryCode="MY",
                                    DunsNumber="11112",
                                    MainAddressLine="Malisya",
                                    PostalCode="654",
                                    Tenant=newSearchVendorParams.Tenant,
                                    VATNumber="456754",
                                    VendorName="Shamatak",
                                    VendorNumber="2211",
                                    VendorTypeCode="2",
                                    VendorTypeName="לקוח חו\"ל" ,
                                    Id = Guid.NewGuid().ToString(),
                                    InActive = false,
                                    VendorCommunications=new List<VendorCommunicationResult>()
                                    {
                                        new VendorCommunicationResult(){ CommunicationAddress="Malisya", CommunicationType="AH"},

                                    },

                                },

                            };
            #endregion
            CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(newSearchVendorParams.Tenant);
            foreach (VendorResult vendor in vendorResults)
            {
                bool exists = vendorQueryService.DoesVendorExist(vendor.VendorNumber, newSearchVendorParams.Tenant);
                vendor.Exists = exists;
            }

            List<VendorResult> filteredResult = vendorResults;

            if (!neglectValues)
            {

                if (newSearchVendorParams.VendorNumber != null)
                {
                    filteredResult = filteredResult.Where(d => d.VendorNumber == newSearchVendorParams.VendorNumber.Value.ToString()).ToList();
                }
                if (!string.IsNullOrEmpty(newSearchVendorParams.VendorName))
                {
                    filteredResult = filteredResult.Where(d => d.VendorName.ToLower() == newSearchVendorParams.VendorName.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(newSearchVendorParams.CountryCode))
                {
                    filteredResult = filteredResult.Where(d => d.CountryCode == newSearchVendorParams.CountryCode).ToList();
                }
                if (!string.IsNullOrEmpty(newSearchVendorParams.SubCountryCode))
                {
                    filteredResult = filteredResult.Where(d => d.SubCountryCode == newSearchVendorParams.SubCountryCode).ToList();
                }
                if (!string.IsNullOrEmpty(newSearchVendorParams.CityName))
                {
                    filteredResult = filteredResult.Where(d => d.CityName.ToLower() == newSearchVendorParams.CityName.ToLower()).ToList();
                }
                if (!string.IsNullOrEmpty(newSearchVendorParams.MainAddressLine))
                {
                    filteredResult = filteredResult.Where(d => d.MainAddressLine.ToLower() == newSearchVendorParams.MainAddressLine.ToLower()).ToList();
                }

                if (!string.IsNullOrEmpty(newSearchVendorParams.PostalCode))
                {
                    filteredResult = filteredResult.Where(d => d.PostalCode == newSearchVendorParams.PostalCode).ToList();
                }
                if (newSearchVendorParams.DunsNumber != null)
                {
                    string dunsNumber = newSearchVendorParams.DunsNumber != null ? newSearchVendorParams.DunsNumber.Value.ToString() : null;
                    filteredResult = filteredResult.Where(d => d.DunsNumber == dunsNumber).ToList();
                }

            }
            return filteredResult;
        }

        public HttpResponseMessage GetVendorByNumber(string vendorNumber)
        {
            try
            { 
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                CustomsVendorQueryService query = new CustomsVendorQueryService(customContext);
                CustomsVendorPM entity = query.GetVendorByNumber(vendorNumber, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PutRecallSuppliersFromFileRequest(ImageParameter fileUploadParamerter)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string documentId = "";
                if (fileUploadParamerter != null && !string.IsNullOrEmpty(fileUploadParamerter.Base64String))
                {
                    byte[] data = Convert.FromBase64String(fileUploadParamerter.Base64String);
                    string decodedString = Encoding.UTF8.GetString(data);

                    VendorsWebService vendorsWebService = new VendorsWebService();
                    vendorsWebService.RecallSuppliersFromFileRequest(fileUploadParamerter.Key, fileUploadParamerter.Tenant, decodedString);
                }

                return Request.CreateResponse(HttpStatusCode.OK, documentId);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}