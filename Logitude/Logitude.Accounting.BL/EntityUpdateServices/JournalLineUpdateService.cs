using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class JournalLineUpdateService : EntityUpdateService<JournalLine, JournalLinePM, JournalPM>
    {

        private class JournalLineRepositoryPriv : JournalLineRepository
        {

            public JournalLineRepositoryPriv(IAccountingContext mainContext)
                : base(mainContext)
            { }
        }
        protected override void AddContext(JournalLinePM myTEntityPM)
        {
            base.AddContext(myTEntityPM);
            this.Repository = new JournalLineRepositoryPriv((IAccountingContext)this.MainContext);

        }

        protected override void OnCreating(JournalLinePM entityPM, JournalPM entityParentPM)
        {
            var myName = this.NameOf();
            if (myName!="JournalLineUpdateServicePriv")
            {
                throw new Exception("JournalLineUpdateService must be used only from JournalUpdateService(force check Approved Journal Can Only Change To Voided)");
            }
            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void OnUpdating(JournalLinePM entityPM)
        {
            var myName = this.NameOf();
            //object entityAncestorPOCO; object entityAncestorPM; object entityParentPM;
            //GetAncestor(out entityAncestorPOCO, out entityAncestorPM, out entityParentPM);
            //var journalPM = entityAncestorPM as JournalPM;
            //if (journalPM == null)
            if (myName != "JournalLineUpdateServicePriv")
            {
                throw new Exception("JournalLineUpdateService must be used only from JournalUpdateService(force check Approved Journal Can Only Change To Voided)");
                //throw new Exception("BLException :Approved Journal Can Only Change To Voided");
            }

            base.OnUpdating(entityPM);
        }
    }
}
