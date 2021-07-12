using Logitude.Accounting.Def.EntityPMs;


namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    public class ExternalReconciliationCreationResponse
    {
        public ExternalReconciliationPM CreatedExternalReconciliation { get; set; }
        public int CreatedReconciliationsCount { get; set; }
    }
}
