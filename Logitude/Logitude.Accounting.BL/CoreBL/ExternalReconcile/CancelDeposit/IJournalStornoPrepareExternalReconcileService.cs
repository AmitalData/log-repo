using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile.CancelDeposit
{
    public interface IJournalStornoPrepareExternalReconcileService
    {
        JournalExternalReconcilePM JournalExternalReconcilePM { get; }

        bool CreateJournalExternalReconcileFromStorno(JournalPM theStorno);
    }
}