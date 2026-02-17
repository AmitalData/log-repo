using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class TenantAdditionalDataMapping
    {
        public static void MapEntity(TenantAdditionalDataPM tenantAdditionalDataPm, TenantAdditionalData tenantAdditionalData, bool isNewState)
        {

            if (isNewState)
            {
               
                tenantAdditionalData.Tenant = tenantAdditionalDataPm.Tenant;
                tenantAdditionalData.Id = tenantAdditionalDataPm.Id;
            }


            tenantAdditionalData.DropBoxAccessToken = tenantAdditionalDataPm.DropBoxAccessToken;
            tenantAdditionalData.DropBoxState = tenantAdditionalDataPm.DropBoxState;
            tenantAdditionalData.DropBoxUEmail = tenantAdditionalDataPm.DropBoxUEmail;
            tenantAdditionalData.DropBoxUID = tenantAdditionalDataPm.DropBoxUID;
            tenantAdditionalData.PaymentGatewayPartnerCode = tenantAdditionalDataPm.PaymentGatewayPartnerCode;
            tenantAdditionalData.PaymentGatewayConnectionString = tenantAdditionalDataPm.PaymentGatewayConnectionString;



           



        } } }