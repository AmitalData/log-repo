using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.Data.AmitalModel.Repsitories;

namespace Unifreight.BL.BL
{
    public class ExportDeclarationClosing
    {
        private MFIFILEMRepository mFIFILEMRepository;
        private MSPSPEDRepository mSPSPEDRepository;
        private MTBPORTRepository mTBPORTRepository;
        public ExportDeclarationClosing(int tenant)
        {
            mFIFILEMRepository = new MFIFILEMRepository(tenant);
            mSPSPEDRepository = new MSPSPEDRepository(tenant);
            mTBPORTRepository = new MTBPORTRepository(tenant);
        }

        public ExportData GetExportData(string exportfile, bool getFromCache = true)
        {
            //if (getFromCache)
            //{
            //    string cacheId = "ExportDeclarationClosingGetExportData" + exportfile;
            //    return CacheHelper.GetFromCache(cacheId, () => GetExportData(exportfile, false));
            //}
            //var un = mFIFILEMRepository.GetAll().Select(x => x.FILENO.ToString()).ToList();



            MFIFILEM mfifilem = mFIFILEMRepository.GetSingle(int.Parse(exportfile));
            if (mfifilem == null) return null;

            string MAWB = mSPSPEDRepository.GetSingle(mfifilem.SPEDNO.Value)?.MAINAWB;
            string loadingSite = mTBPORTRepository.GetSingle(mfifilem.LOADPORT)?.NAMEENG;

            return new ExportData()
            {
                flightDate = mfifilem.FLIGHTDATE,
                loadingSite = loadingSite,
                HAWB = mfifilem.SMP,
                MAWB = MAWB
            };
        }

        public class ExportData
        {
            public string loadingSite { get; set; }
            public DateTime? flightDate { get; set; }
            public string MAWB { get; set; }
            public string HAWB { get; set; }
        }
    }
}
