using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Models;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityKeys;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public class UnifreightDeclarationPaymentUpdateServiceD
    {
        private EntityPMs.DeclarationPaymentPM _DirtyDeclarationPaymentPM;
        private EntityPMs.DeclarationPaymentPM _DBOccDeclarationPaymentPM;
        private string _LoggingUserId;
        private AmitalContext _AmitalContext;
        private CCUPAYHAND _CCUPAYHAND;

        public UnifreightDeclarationPaymentUpdateServiceD(EntityPMs.DeclarationPaymentPM entityPM)
        {
            // TODO: Complete member initialization
            this._DirtyDeclarationPaymentPM = entityPM;
        }

        internal void Update()
        {
            try
            {
                if (!Environment.MachineName.Equals("itzik-7-new", StringComparison.OrdinalIgnoreCase)) return;
                if (_DirtyDeclarationPaymentPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.None) return;
                if (String.IsNullOrWhiteSpace(_DirtyDeclarationPaymentPM.DeclarationId))
                {

                    return;
                }

                using (_AmitalContext = new AmitalContext())
                {
                    var context = CustomContext.GetContext(_DirtyDeclarationPaymentPM.Tenant);
                    var myQueryService = new DeclarationQueryService(context);
                    this._MyDeclarationPM = myQueryService.GetSingle(_DirtyDeclarationPaymentPM.DeclarationId, true, false);

                    if (this._MyDeclarationPM == null)
                    {
                        LogMessagingUtil.Instance.AppendLine("Can not found declaration" + _DirtyDeclarationPaymentPM.DeclarationId);
                        return;
                    }

                    if (!int.TryParse(_MyDeclarationPM.CustomFileNo, out _lCUSTOMFILENO))
                    {
                        throw new BusinessErrorException("_MyDeclarationPM.CustomFileNo could not convert to int ");
                    }

                    _CCUPAYHAND = (
                            from row in _AmitalContext.CCUPAYHANDs
                            where row.FILENO == _lCUSTOMFILENO
                            select row
                            ).FirstOrDefault();

                    if (_CCUPAYHAND == null)
                    {
                        _CCUPAYHAND = CreateNewCCUPAYHAND(_DirtyDeclarationPaymentPM); ;
                        _AmitalContext.AddToCCUPAYHANDs(_CCUPAYHAND);
                    }
                    else
                    {
                        _AmitalContext.CCUPAYHANDs.DeleteObject(_CCUPAYHAND);//mark 2 delete
                    }


                    if (_DirtyDeclarationPaymentPM.IsProcessA == true)
                    {
                        _CCUPAYHAND.PROCESSWANT = "A";
                    }
                    _CCUPAYHAND.HANDDATE = _DirtyDeclarationPaymentPM.PaymentDate;
                    _CCUPAYHAND.RESHIMONSIGN = _DirtyDeclarationPaymentPM.SignatoryIdentification;

                    DoDeclarationPaymentMethods();

                    _AmitalContext.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void DoDeclarationPaymentMethods()
        {
            if (_DirtyDeclarationPaymentPM.DeclarationPaymentMethods.Count < 1) return;

            _CCUPAYLINEFs = (
                from row in _AmitalContext.CCUPAYLINEFs
                where row.FILENO == _CCUPAYHAND.FILENO
                select row
                    )
                .ToList();

            foreach (var declarationPaymentMethods in _DirtyDeclarationPaymentPM.DeclarationPaymentMethods)
            {
                var curCCUPAYLINEF = _CCUPAYLINEFs.FirstOrDefault(rec => rec.FILENO == _lCUSTOMFILENO && rec.LINENO == declarationPaymentMethods.Line);
                switch (declarationPaymentMethods.ChangeSetOp)
                {
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Insert:
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Update:
                        if (curCCUPAYLINEF != null)
                        {
                            _AmitalContext.CCUPAYLINEFs.DeleteObject(curCCUPAYLINEF);//mark 2 delete
                        }
                        DeclarationPaymentMethodUpsert(declarationPaymentMethods);
                        break;
                    case Simplog.Server.Infrastructure.ChangeSetOperation.Delete:
                        if (curCCUPAYLINEF != null)
                        {
                            _AmitalContext.CCUPAYLINEFs.DeleteObject(curCCUPAYLINEF);//mark 2 delete
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void DeclarationPaymentMethodUpsert(EntityPMs.DeclarationPaymentMethodPM declarationPaymentMethods)
        {
            var curCCUPAYLINEF = _CCUPAYLINEFs.FirstOrDefault(rec => rec.FILENO == _lCUSTOMFILENO && rec.LINENO == declarationPaymentMethods.Line);

            if (curCCUPAYLINEF == null)
            {
                curCCUPAYLINEF = new CCUPAYLINEF() { FILENO = _lCUSTOMFILENO, LINENO = declarationPaymentMethods.Line };
                _AmitalContext.AddToCCUPAYLINEFs(curCCUPAYLINEF);
                _CCUPAYLINEFs.Add(curCCUPAYLINEF);
            }
            curCCUPAYLINEF.PAYMETHOD = declarationPaymentMethods.MethodTypeCode;
            double paymentamount = 0;
            if (!double.TryParse(declarationPaymentMethods.Amount.ToString(), out paymentamount))
            {
                throw new BusinessErrorException("declarationPaymentMethods.Amount could not convert to double ");
            }
            curCCUPAYLINEF.PAYAMOUNT = paymentamount;
        }

        private CCUPAYHAND CreateNewCCUPAYHAND(EntityPMs.DeclarationPaymentPM _DeclarationPaymentPM)
        {
            return new CCUPAYHAND()
            {
                FILENO = GetCounter(_DeclarationPaymentPM)
            };
        }

        private int GetCounter(EntityPMs.DeclarationPaymentPM _DeclarationPaymentPM)
        {
            int i = Convert.ToInt32(_DeclarationPaymentPM.DeclarationId.Substring(2));
            return 90000000 + i;
        }
    }

}
