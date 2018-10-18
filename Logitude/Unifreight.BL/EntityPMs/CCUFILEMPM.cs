using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class CCUFILEMPM 
    {
        
        List<CCUMSHGRPM> _CCUMSHGRs;

        public List<CCUMSHGRPM> CCUMSHGRs
        {
            get { return _CCUMSHGRs = _CCUMSHGRs ?? new List<CCUMSHGRPM>(); }
            set
            {
                if (value == null)
                {
                    CCUMSHGRLastLine = 0;
                }
                _CCUMSHGRs = value;
            }
        }

        public int CCUMSHGRLastLine { get; set; }

        private List<SupplierInvoicePM> _DeletedSupplierInvoices;

        public List<SupplierInvoicePM> DeletedSupplierInvoices
        {
            get { return _DeletedSupplierInvoices = _DeletedSupplierInvoices?? new List<SupplierInvoicePM>(); }
            set { _DeletedSupplierInvoices = value;  }
        }

        private List<CCUMSHGRPM> _DeletedCCUMSHGRs;

        public List<CCUMSHGRPM> DeletedCCUMSHGRs
        {
            get { return _DeletedCCUMSHGRs = _DeletedCCUMSHGRs ?? new List<CCUMSHGRPM>(); ; }
            set { _DeletedCCUMSHGRs = value; }
        }

        List<SupplierInvoicePM> _SupplierInvoices;

        public List<SupplierInvoicePM> SupplierInvoices
        {
            get { return _SupplierInvoices = _SupplierInvoices ?? new List<SupplierInvoicePM>() ; }
            set
            {
                if (value == null)
                {
                    SupplierInvoicesLastLine = 0;
                }
                _SupplierInvoices = value;
            }
        }

        public int SupplierInvoicesLastLine { get; set; }

        List<CCUTAXPM> _CCUTAXPM;
        public List<CCUTAXPM> CCUTAXPM
        {
            get { return _CCUTAXPM = _CCUTAXPM ?? new List<CCUTAXPM>(); }
            set { _CCUTAXPM = value; }
        }

        public int CCUTAXPMLastLine { get; set; }

        private List<CCUTAXPM> _DeletedCCUTAXPM;

        public List<CCUTAXPM> DeletedCCUTAXPM
        {
            get { return _DeletedCCUTAXPM = _DeletedCCUTAXPM ?? new List<CCUTAXPM>(); }
            set
            {
                if (value == null)
                {
                    CCUTAXPMLastLine = 0;
                }
                _DeletedCCUTAXPM = value;
            }
        }

        LastLine105PM _LastLine105PM;

        public LastLine105PM LastLine105PM
        {
            get { return _LastLine105PM = _LastLine105PM ?? new LastLine105PM(); }
            set { _LastLine105PM = value; }
        }

        public string DeclarationId { get; set; }

        List<CCUMESSAGEPM> _CCUMESSAGEs;

        public List<CCUMESSAGEPM> CCUMESSAGEs
        {
            get { return _CCUMESSAGEs = _CCUMESSAGEs ?? new List<CCUMESSAGEPM>(); }
            set
            {
                _CCUMESSAGEs = value;
            }
        }

        List<CCUTSRUFOTPM> _CCUTSRUFOTs;

        public List<CCUTSRUFOTPM> CCUTSRUFOTs
        {
            get { return _CCUTSRUFOTs = _CCUTSRUFOTs ?? new List<CCUTSRUFOTPM>(); }
            set
            {
                _CCUTSRUFOTs = value;
            }
        }

        //<--- Yuval Chalup 14.06.2015 TASK-13951
        List<CCUTRANSPVALPM> _CCUTRANSPVALs;
        public List<CCUTRANSPVALPM> CCUTRANSPVALs
        {
            get { return _CCUTRANSPVALs = _CCUTRANSPVALs ?? new List<CCUTRANSPVALPM>(); }
            set
            {
                if (value == null)
                {
                    CCUTRANSPVALLastLine = 0;
                }
                _CCUTRANSPVALs = value;
            }
        }

        public int CCUTRANSPVALLastLine { get; set; }
        private List<CCUTRANSPVALPM> _DeletedCCUTRANSPVALs;
        public List<CCUTRANSPVALPM> DeletedCCUTRANSPVALs
        {
            get { return _DeletedCCUTRANSPVALs = _DeletedCCUTRANSPVALs ?? new List<CCUTRANSPVALPM>(); ; }
            set { _DeletedCCUTRANSPVALs = value; }
        }
        //Yuval Chalup 14.06.2015 TASK-13951 --->
    }
}
