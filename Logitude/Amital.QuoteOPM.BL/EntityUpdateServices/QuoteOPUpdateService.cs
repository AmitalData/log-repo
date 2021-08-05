using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{
    public partial class QuoteOPUpdateService : EntityUpdateService<QuoteOP, QuoteOPPM, EntityPM>
    {
        //C:\C21R01\Logitude\Logitude.BL\QuoteModel\Tools\EntityService\QuoteService.cs
        
        protected override void OnCreating(QuoteOPPM entityPM, EntityPM entityParentPM)
        {

            EntityPM.Id = IdCounter.GetNumber("Quote", entityPM.Tenant).ToString();
            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            string resolveLoggingUserId = AuthenticationUtil.ResolveUserIdentityName(entityPM.Tenant);
            var contact = contactRep.GetSingleContactByEmail(resolveLoggingUserId, entityPM.Tenant);
            entityPM.CreatedByUserId = contact.Id;
            ;

            //entityPM.OpenDate;
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            //entityPoco.Tenant = entityPM.Tenant;
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DirectionId);
            //entityPoco.DirectionId = entityPM.DirectionId;
            //this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ProductCode);
            //entityPoco.ProductCode = entityPM.ProductCode;
            if (!entityPM.IsHybrid)
            {
                entityPM.QuoteNumber = TableCounter.GetNumber(entityPM.Tenant, "QUOT", entityPM.DirectionId, entityPM.TransportModeId);
            }
            base.OnCreating(entityPM, entityParentPM);
        }
#if false


        private void InitializeComponent()
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            this.entityPM.MarkFollowUpsAsDone = false;

            this.isAdhoc = entityPM.QuoteTypeCode == "A" ? true : false;
            this.isInlandDomestic = (entityPM.DirectionId == "D" && entityPM.TransportModeId == "I");

            if (string.IsNullOrEmpty(entityPM.ShipmentTypeId) && entityPM.TransportModeId == "A")
            {
                entityPM.ShipmentTypeId = "Air";
            }

            this.isLCLQuote = false;
            if (entityPM.TransportModeId.ToUpper() == "A")
            {
                this.isLCLQuote = true;
            }

            else if (entityPM.TransportModeId.ToUpper() == "O" && entityPM.ShipmentTypeId.ToUpper() == "LCLD")
            {
                this.isLCLQuote = true;
            }

            else if (entityPM.TransportModeId.ToUpper() == "I" && entityPM.ShipmentTypeId.ToUpper() == "LTL")
            {
                this.isLCLQuote = true;
            }

            this.isFCLQuote = !this.isLCLQuote;

            this.InitializeVATs();

            if (isNewEntity)
            {
                entityPM.CreatedByUserId = initializer.LoggedContactId;
                entityPM.OpenDate = entityPM.IsHybrid ? entityPM.OpenDate : todayDateTime;
                entityPM.LastStageDate = todayDateTime;

                if (!entityPM.IsHybrid)
                {
                    entityPM.QuoteNumber = TableCounter.GetNumber(tenant, "QUOT", entityPM.DirectionId, entityPM.TransportModeId);
                }

                if (entityPM.IsCreatedFromTicket)
                {
                    entityPM.RequestDate = entityPM.TicketCreateDate;
                }
                else
                {
                    entityPM.RequestDate = entityPM.OpenDate;
                }

                this.InitializeStage();
                this.InitializeSaleCurrency();
                //this.InitializeSalesman();
                this.InitializeProfitCurrency();

                if (!entityPM.IsHybrid)
                {
                    if (this.entityPM.QuoteCharges.Count == 0)
                    {
                        this.GenerateDefaultCharges();
                    }

                    else
                    {
                        // Ayman
                        // if Quote is Copy from another
                        // and Quantites details are edited by user
                        // then we need to calculate in server
                        this.ComputeChargesAmounts();
                    }
                }
            }

            entityPM.UpdatedByUserId = initializer.LoggedContactId;
            entityPM.UpdateDate = todayDateTime;

            if (entityPM.Ratio == null)
            {
                entityPM.Ratio = (entityPM.TransportModeId == "A") ? 6 : 1;
            }

            if (string.IsNullOrEmpty(entityPM.RatingCode))
            {
                entityPM.RatingCode = "N";
            }

            InitializePartners();
            InitializeInlandDomestic();
            InitializePickupDelivery();
            SetCustomerDateFields(entityPM, entityPoco);
            ComputeChargesSaleFieldsInSaleCurrency();
            ComputeCountryForStatisticsId();

            if (!entityPM.IsHybrid)
            {
                InitializeSubject();
            }

            InitializeExpirationValues();
            InitializeAutomaticallyClose();
            InitializeQuoteConversionProcess();

            this.ComputeExpectedProfit();
            this.ComputeProfit();
            this.FillDefaultSubType();
        }
#endif
    }
}
