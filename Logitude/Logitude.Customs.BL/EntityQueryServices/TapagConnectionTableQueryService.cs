using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial  class TapagConnectionTableQueryService: EntityQueryService<TapagConnectionTable,TapagConnectionTableKeys,TapagConnectionTablePM,object,TapagConnectionTableKeys>
    {
      public string GetTapagIdByFileAndNumeral(string fileNumber, int numeral, int tenant)
      {
          if (string.IsNullOrEmpty(fileNumber) && numeral <= 0) return "";
          return repository.GetTapagIdByFileAndNumeral(fileNumber, numeral, tenant);
      }

      public string GetTapagIdByRequestFileNumber(string requestFileNumber, int tenant)
      {
          if (string.IsNullOrEmpty(requestFileNumber)) return "";
          return repository.GetTapagIdByRequestFileNumber(requestFileNumber, tenant);
      }
   
      public TapagConnectionTablePM GetTapagConnectionByFileAndNumeral(string fileNumber, int numeral, int tenant)
      {
          TapagConnectionTablePM tapagConnectionTablePM = null;
          if (string.IsNullOrEmpty(fileNumber) && numeral <= 0) return tapagConnectionTablePM;
          TapagConnectionTable tapagConnectionTable = repository.GetTapagConnectionByFileAndNumeral(fileNumber, numeral, tenant);

          if (tapagConnectionTable != null)
          {
              tapagConnectionTablePM = this.GetEntityPM(tapagConnectionTable);
          }
          return tapagConnectionTablePM;
      }

      public List<TapagConnectionTablePM> GetTapagConnectionByTapagId(string tapagId, int tenant)
      {
          List<TapagConnectionTablePM> tapagConnectionTablePMList = null;
          if (string.IsNullOrEmpty(tapagId)) return tapagConnectionTablePMList;
          List<TapagConnectionTable> tapagConnectionTable = repository.GetTapagConnectionByTapagId(tapagId, tenant);

          if (tapagConnectionTable != null)
          {
              tapagConnectionTablePMList = new List<TapagConnectionTablePM>();
              foreach (var tapagConnectionTableItem in tapagConnectionTable)
              {
                  tapagConnectionTablePMList.Add(this.GetEntityPM(tapagConnectionTableItem));
              }
          }
          return tapagConnectionTablePMList;
      }
        
    }
}
