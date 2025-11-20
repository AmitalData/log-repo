using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts.SIIRequest;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;

namespace Logitude.Customs.BL.BL.SIIRequest
{
    public class SiiStatusUpdateService
    {
        private readonly int _tenant;

        public SiiStatusUpdateService(int tenant)
        {
            _tenant = tenant;
        }

        public void UpdateStatus(SiiStatusMessage msg)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");

            ICustomContext context = CustomContext.GetContext(_tenant);
            var itemsReqListQueryService = new SupplierInvoiceItemsReqListQueryService(context);
            var updater = new SupplierInvoiceItemsReqListUpdateService(context,new Dictionary<string, IContext>(), _tenant);

            var linePm = itemsReqListQueryService.GetLineForSiiStatusUpdate(
                msg.requestNumber,
                msg.lineSerialNumber,
                msg.modelCode,
                _tenant);

            if (linePm == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "SII Status: Line not found. RequestNo={0}, Line={1}, ModelCode={2}, Tenant={3}.",
                        msg.requestNumber, msg.lineSerialNumber, msg.modelCode, _tenant));
            }

            linePm.StatusCode = msg.saleApprovalStatus;
            linePm.StatusDate = msg.saleApprovalStatusDate;
            linePm.DistApprovalAttachmentPath = msg.distributionApprovalAttachmentPath;
            linePm.ChangeSetOp = ChangeSetOperation.Update;
            updater.Update(linePm, true);
        }
    }
}
