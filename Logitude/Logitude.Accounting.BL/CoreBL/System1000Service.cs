using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class System1000Service : ISystem1000Service
    {
        StringBuilder _sb = new StringBuilder();
        private FullAccountingSettingPM _FullAccountingSettingPM;
        private IQueryable<CardGLAccountDataView> _AllVendorGLAccountCards;



        public System1000Service()
        {
        }


        public string GetSystem1000FlatFile(IAccountingContext accountingContext, int tenant)
        {
            StringBuilder flatFile = new StringBuilder();
            _sb.AppendLine($"GetDeductionFileNumberFromAccSetting({tenant})");
            _FullAccountingSettingPM = GetDeductionFileNumberFromAccSetting(accountingContext, tenant);
            _AllVendorGLAccountCards = GetQAllVendorGLAccountCards(accountingContext, tenant);

            var listOfAccounts = _AllVendorGLAccountCards.ToList();
            if (listOfAccounts.Count == 0)
            {
                return null;
            }
            string header = "A" + _FullAccountingSettingPM.DeductionFileNumber.PadLeft(9, '0').Substring(0, 9);
            flatFile.AppendLine(header);
            int count = 0;
            foreach (var obj in listOfAccounts)
            {
                string line = "B" + obj.InternalNumber.PadLeft(15, '0').Substring(0, 15)
                    + obj.DeductionFileNumber.PadLeft(9, '0').Substring(0,9) 
                    + obj.VatNumber.PadLeft(9, '0').Substring(0, 9);
                flatFile.AppendLine(line);
                count++;
            }
            string footer = "C" + _FullAccountingSettingPM.DeductionFileNumber.PadLeft(9, '0').Substring(0, 9)
                + count.ToString().PadLeft(4, '0');
            flatFile.AppendLine(footer);

            return flatFile.ToString();
        }


        public virtual string TranslateTextsClassTranslate(string textCodeCode, int tenant, bool getLocalDefaultText)
        {
            return TranslateTextsClass.Translate(textCodeCode, tenant, getLocalDefaultText);
        }



        private IQueryable<CardGLAccountDataView> GetQAllVendorGLAccountCards(IAccountingContext accountingContext, int tenant)
        {
            var myGLAccountQueryService = new GLAccountQueryService(accountingContext);
            var myVendorGLAccountCardList = myGLAccountQueryService.GetQAllVendorGLAccountCardsHavingDeduction(tenant);
            return myVendorGLAccountCardList;
        }


        private FullAccountingSettingPM GetDeductionFileNumberFromAccSetting(IAccountingContext accountingContext, int tenant)
        {
            bool useLocal = true;
            string text;
            var myFullAccountingSettingQueryService = new FullAccountingSettingQueryService(accountingContext);
            var myFullAccountingSettingPM = myFullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant);
            if (myFullAccountingSettingPM == null)
            {
                throw new Exception("No FullAccountingSettingPM  for tenant ");
            }
            if (string.IsNullOrWhiteSpace(myFullAccountingSettingPM.DeductionFileNumber))
            {
                //   throw new Exception("No myFullAccountingSettingPM.DeductionFileNumber  for tenant ");
                text = TranslateTextsClassTranslate("System1000.O.DeductionFileNumber", 0, useLocal);
                // Deduction File Number is undefined.
                throw new Exception(text);
            }
            return myFullAccountingSettingPM;
        }




    }
}
