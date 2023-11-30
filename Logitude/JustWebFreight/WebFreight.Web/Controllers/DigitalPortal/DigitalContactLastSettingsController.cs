using System;
using System.Web.Http;
using WebFreight.Web.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using System.Web;
using Simplog.Server.Infrastructure.DataContracts.Models;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System.Collections.Generic;
using System.Linq;
using Logitude.SystemLogs;
using System.Net;
using System.Net.Http;

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalContactLastSettingsController : ApiController
    {
        [HttpGet]
        [Route("DigitalContactLastSettings/GetDigitalContactLastSettings")]
        public HttpResponseMessage GetDigitalContactLastSettings(string contactId, string  entity, string cardId)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                var objectTableId = GetObjectTableId(entity, authToken.Tenant);
                DigitalContactLastSettingRepository contactLastSettingRepository =  new DigitalContactLastSettingRepository(authToken.Tenant);
                var digitalContactLastSettings = contactLastSettingRepository.GetDigitalContactLastSettings(contactId, objectTableId, authToken.Tenant);

                if(digitalContactLastSettings.Count() == 0)
                {
                    digitalContactLastSettings = GetInitialFiltersList(entity);
                }

                return Request.CreateResponse(HttpStatusCode.OK, digitalContactLastSettings);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPut]
        [Route("DigitalContactLastSettings/PutDigitalContactLastSettings")]
        public HttpResponseMessage PutDigitalContactLastSettings(DigitalContactLastSettingInfo digitalContactLastSettings)
        {
            int tenant = 0;
            string email = "";
            try
            {
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                tenant = authToken.Tenant;
                email = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, digitalContactLastSettings.CardId);

                var objectTableId = GetObjectTableId(digitalContactLastSettings.Entity, authToken.Tenant);
                DigitalContactLastSettingRepository contactLastSettingRepository = new DigitalContactLastSettingRepository(authToken.Tenant);
                UpdateDigitalContactLastSettings(digitalContactLastSettings, authToken.Tenant, objectTableId);

                return Request.CreateResponse(HttpStatusCode.OK, digitalContactLastSettings);
            }
            catch (AutenticationException ex)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, ApiExceptionBuilder.BuildException(ex));
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, email, $"Digital portal {tenant}", "", null);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        #region private methods 

        private string GetObjectTableId(string objectTableName, int tenant)
        {
            ObjectTableQuery objectTableQuery = new ObjectTableQuery(tenant);
            string objectTableId = objectTableQuery.GetObjectTableIdByName(objectTableName);
            return objectTableId;
        }

        private List<DigitalContactLastSetting> GetInitialFiltersList(string entity)
        {
            if(entity.Equals("Shipment", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetShipmentInitialFiltersList();
            }
            else if (entity.Equals("ARInvoice", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetARInvoiceInitialFiltersList();
            }
            else if (entity.Equals("Quote", StringComparison.InvariantCultureIgnoreCase))
            {
                return GetQuoteInitialFiltersList();
            }

            return new List<DigitalContactLastSetting>();
        }

        private List<DigitalContactLastSetting> GetShipmentInitialFiltersList()
        {
            var result = new List<DigitalContactLastSetting>
            {
                new DigitalContactLastSetting
                {
                    FilterCode = "TransportModeShipmentTypeFilters",
                    FilterName = "Shipment.G.TransportMode",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "StatusId",
                    FilterName = "Shipment.G.ShipmentStatus",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "ConsigneeShipperIds",
                    FilterName = "Shipment.G.MyPartner",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "From",
                    FilterName = "Shipment.F.From",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "To",
                    FilterName = "Shipment.G.FinalDestination",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "CreateDateTime",
                    FilterName = "Shipment.G.CreateDate",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "MainCarriageETA",
                    FilterName = "Shipment.G.ETADate",
                    IsChecked = true
                }
            };

            return result;
        }

        private List<DigitalContactLastSetting> GetARInvoiceInitialFiltersList()
        {
            var result = new List<DigitalContactLastSetting>
            {
                new DigitalContactLastSetting
                {
                    FilterCode = "DigitalPaidStatus",
                    FilterName = "ARInvoice.F.StatusName",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "InvoiceDate",
                    FilterName = "ARInvoice.G.CreateDate",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "DueDate",
                    FilterName = "ARInvoice.G.DueDate",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "AmountInInvoiceCurrency",
                    FilterName = "ARInvoice.G.TotalAmount",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "AmountDue",
                    FilterName = "ARInvoice.G.OpenAmount",
                    IsChecked = true
                }
            };

            return result;
        }

        private List<DigitalContactLastSetting> GetQuoteInitialFiltersList()
        {
            var result = new List<DigitalContactLastSetting>
            {
                new DigitalContactLastSetting
                {
                    FilterCode = "TransportModeFilter",
                    FilterName = "Quote.G.TransportMode",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "StatusFilter",
                    FilterName = "Quote.G.Status",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "From",
                    FilterName = "Quote.G.OriginCountry",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "To",
                    FilterName = "Quote.G.DestinationCountry",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "OpenDate",
                    FilterName = "Quote.G.CreateDate",
                    IsChecked = true
                },
                new DigitalContactLastSetting
                {
                    FilterCode = "ExpirationDate",
                    FilterName = "Quote.G.ExpireDate",
                    IsChecked = true
                }
            };

            return result;
        }

        private void UpdateDigitalContactLastSettings(DigitalContactLastSettingInfo digitalContactLastSettings, int tenant, string objectTableId)
        {
            var filtersList = digitalContactLastSettings.FilterCodes;
            var context = CommonDataContext.GetContext(tenant);
            var repository = new DigitalContactLastSettingRepository(context);
            var contactFilters = repository.GetSingleByEntityAndContact(digitalContactLastSettings.ContactId, tenant, objectTableId);

            filtersList.ForEach(item =>
            {
                var contactFilter =  contactFilters.Where(a => a.FilterCode.Equals(item.FilterCode, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (contactFilter == null)
                {
                    repository.Add(AddContactFilter(digitalContactLastSettings.ContactId, objectTableId, item, tenant));
                }
                else
                {
                    contactFilter.FilterCode = item.FilterCode;
                    contactFilter.FilterName = item.FilterName;
                    contactFilter.IsChecked = item.IsChecked;
                    repository.Update(contactFilter);
                }
            });

            repository.SubmitChanges();
        }

        private DigitalContactLastSetting AddContactFilter(string contactId, string objectTableId, FilterCodes item, int tenant)
        {
            return new DigitalContactLastSetting()
            {
                Id = IdCounter.GetNumber("DigitalContactLastSetting", tenant).ToString(),
                Tenant = tenant,
                ContactId = contactId,
                ObjectTableId = objectTableId,
                FilterCode = item.FilterCode,
                FilterName = item.FilterName,
                IsChecked = item.IsChecked,
            };
        }

        #endregion
    }
}