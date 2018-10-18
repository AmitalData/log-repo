using Logitude.Server.Tools.Helpers;
using Logitude.XSD;
using Logitude.XSD.DataContracts;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.ReportsWebServices;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        public AWBResultClass GetValidatingAWBSending(string myShipmentId, int myTenant, string myRecipient, bool isSendingFHLs, bool isSendingCargonaut, bool isSendingDEXX, string mainCarriageCarrierId)
        {
            AWBValidator validator = new AWBValidator(myTenant);

            AWBResultClass myResult = validator.GetSendingValidating(myShipmentId, myRecipient, isSendingFHLs, isSendingCargonaut, isSendingDEXX, mainCarriageCarrierId);

            return myResult;
        }

        public List<FHLShipmentValidator> GetFHLsValidation(string myMasterId, int myTenant)
        {
            AWBValidator validator = new AWBValidator(myTenant);

            List<FHLShipmentValidator> myResult = validator.GetFHLsValidation(myMasterId);

            return myResult;
        }

        public AWBPrintResult GetAWBPrintingStock(string myShipmentId, bool isCargonautSending, bool isDEXXSending, bool isConfirmedByUser, int myTenant)
        {
            AWBPrintingManager myPrintingManager = new AWBPrintingManager();

            AWBPrintResult myResult = myPrintingManager.GetPrintingResult(myShipmentId, isCargonautSending, isDEXXSending, isConfirmedByUser, myTenant);
            
            return myResult;
        }
    }
}