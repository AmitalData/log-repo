using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class JournalStatusTypeQueryService
    {

        public JournalStatusTypePM GetSinglePM(string id, int tenant)
        {
            JournalStatusTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.JournalStatusID == id
                 select new JournalStatusTypePM()
                 {
                     JournalStatusID = a.JournalStatusID,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,

                 }).FirstOrDefault();

            return entityPM;
        }



        public JournalStatusTypePM GetSinglePM(string id)
        {
            JournalStatusTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.JournalStatusID == id
                 select new JournalStatusTypePM()
                 {
                     JournalStatusID = a.JournalStatusID,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,

                 }).FirstOrDefault();

            return entityPM;
        }
    }
}
