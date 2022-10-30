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

namespace WebFreight.Web.Controllers.DigitalPortal
{
    public class DigitalContactLastSettingsController : ApiController
    {
        [HttpGet]
        [Route("DigitalContactLastSettings/GetDigitalContactLastSettings")]
        public IHttpActionResult GetDigitalContactLastSettings(string contactId, string  entity, string cardId)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, cardId);

                var objectTableId = GetObjectTableId(entity, authToken.Tenant);
                DigitalContactLastSettingRepository contactLastSettingRepository =  new DigitalContactLastSettingRepository(authToken.Tenant);
                var digitalContactLastSettings = contactLastSettingRepository.GetDigitalContactLastSettings(contactId, objectTableId, authToken.Tenant);

                if(digitalContactLastSettings.Count() == 0)
                {
                    digitalContactLastSettings = GetInitialFiltersList(entity);
                }

                return Ok(digitalContactLastSettings);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
            }
        }

        [HttpPut]
        [Route("DigitalUploader/PutDigitalContactLastSettings")]
        public IHttpActionResult PutDigitalContactLastSettings(DigitalContactLastSettingInfo digitalContactLastSettings)
        {
            try
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(HttpContext.Current.Request.Headers["Token"]);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckDigitalUserAuthentication(authToken.Tenant, digitalContactLastSettings.CardId);

                var objectTableId = GetObjectTableId(digitalContactLastSettings.Entity, authToken.Tenant);
                DigitalContactLastSettingRepository contactLastSettingRepository = new DigitalContactLastSettingRepository(authToken.Tenant);
                UpdateDigitalContactLastSettings(digitalContactLastSettings, authToken.Tenant, objectTableId);

                return Ok(digitalContactLastSettings);
            }
            catch (Exception ex)
            {
                return BadRequest(ApiExceptionBuilder.BuildException(ex).ErrorMessage);
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

            return null;
        }

        private List<DigitalContactLastSetting> GetShipmentInitialFiltersList()
        {
            var result = new List<DigitalContactLastSetting>();
            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "TransportModeShipmentTypeFilters",
                FilterName = "Transport mode",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "StatusId",
                FilterName = "Shipment status",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "ConsigneeShipperIds",
                FilterName = "My partner",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "From",
                FilterName = "From",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "To",
                FilterName = "Final destination",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "CreateDateTime",
                FilterName = "Create date",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "MainCarriageETA",
                FilterName = "ETA date",
                IsChecked = true
            });

            return result;
        }

        private List<DigitalContactLastSetting> GetARInvoiceInitialFiltersList()
        {
            var result = new List<DigitalContactLastSetting>();
            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "DigitalPaidStatus",
                FilterName = "Status",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "InvoiceDate",
                FilterName = "Create date",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "DueDate",
                FilterName = "Due date",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "AmountInInvoiceCurrency",
                FilterName = "Total Amount",
                IsChecked = true
            });

            result.Add(new DigitalContactLastSetting
            {
                FilterCode = "AmountDue",
                FilterName = "Open Amount",
                IsChecked = true
            });

            return result;
        }

        public void UpdateDigitalContactLastSettings(DigitalContactLastSettingInfo digitalContactLastSettings, int tenant, string objectTableId)
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