using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
  public partial  class JournalTypeQueryService
    {

        public JournalTypePM GetSinglePM(string id, int tenant)
        {
            JournalTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.JournalTypeID == id
                 select new JournalTypePM()
                 {
                     JournalTypeID = a.JournalTypeID,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,

                 }).FirstOrDefault();

            return entityPM;
        }


        public JournalTypePM GetSinglePM(string id)
        {
            JournalTypePM entityPM = null;

            entityPM =
                (from a in repository.All()
                 where a.JournalTypeID == id
                 select new JournalTypePM()
                 {
                     JournalTypeID = a.JournalTypeID,
                     LocalName = a.LocalName,
                     EnglishName = a.EnglishName,

                 }).FirstOrDefault();

            return entityPM;
        }

    }
}
