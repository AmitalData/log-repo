using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.Base.Extensions
{
    [Binding]
    public class Transforms
    {
        private static Dictionary<string, string> mapper = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
        #region Directions
            {"Export", "E"},
            {"Import", "I"},
            {"Drop", "R"},
            {"Domestic", "D"},
            {"Customs Import", "C"},
        #endregion
        #region TransportMode
            {"Air", "A"},
            {"Inland", "I"},
            {"Ocean", "O"},
        #endregion
        #region ShipmentLevel
            {"Master", "C"},
            {"Direct", "D"},
            {"House", "H"},
            {"Customs", "A"},
        #endregion
        #region PrepaidCollect
            {"Prepaid", "P"},
            {"Collect", "C"},
            {"Both", "B"},
        #endregion
        #region QuoteTypes
            {"Spot Rate","A" },
            {"Routing Rates","P" },
            {"Shipper" ,"SHI" },
        #endregion
        #region ActivityTimeTypes
            {"Busy","BS" },
            {"Free","FR" },
            {"Out Of Office" ,"OF" },
            {"Tentative" ,"TN" },
        #endregion
        #region ActivityTimeTypes
            {"Cold","C" },
            {"Hot","H" },
            {"Neutral" ,"N" },
            {"Warm" ,"W" }
        #endregion
        };

        [StepArgumentTransformation]
        public Table TableTransform(Table table)
        {
            return ChangeValues(table);
        }

        public void AddListToMapper(Dictionary<string, string> list)
        {
            mapper.Concat(list);
        }

        public void AddItemToMapper(string key, string value)
        {
            mapper.Add(key, value);
        }

        private static Table ChangeValues(Table table)
        {
            var mappedTable = new Table(table.Header.ToArray());
            foreach (var row in table.Rows)
            {
                mappedTable.AddRow(row.Values.Select(x => mapper.ContainsKey(x) ? mapper[x] : x).ToArray());
            }
            return mappedTable;
        }
    }
}
