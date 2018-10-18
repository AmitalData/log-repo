using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    class SystemCheckService
    {
        public void CheckTenant(int tenant)
        {
            StartCheckUpToDown();
        }

        private void StartCheckUpToDown()
        {
            try
            {
                CheckAllTotalSumRZero();
            }
            catch (SystemCheckException mySystemCheckException)
            {
                CheckReversePerMonth(mySystemCheckException.Year, mySystemCheckException.Month);


                    
                    return;
            }
            
        }

        private void CheckReversePerMonth(int year, int Month)
        {
            try
            {


                CheckAllJournalAreValid(year, Month);
                CheckAllGLAccountThatHaveLedgerTransIn(year, Month);
            }
            catch (SystemCheckException systemCheckException)
            {
                string JournalId = systemCheckException.JournalId;
                throw;
            }
        }

        private void CheckAllGLAccountThatHaveLedgerTransIn(int year, int month)
        {
            throw new NotImplementedException();
        }

        private void CheckAllJournalAreValid(int year, int month)
        {
            throw new NotImplementedException();
        }

        private void CheckAllTotalSumRZero()
        {
            throw new NotImplementedException();
        }
    }
    class SystemCheckException : Exception
    {
        public int Year { get; internal set; }
        public int Month { get; internal set; }
        public string JournalId { get; internal set; }
    }
}
