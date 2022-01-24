using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Unifreight.BL.BL
{
    public class QuoteOPPortsHelper
    {
        public static IQueryable<Ports> GetQuery(string portId, IQueryable<QPorts> baseQ, AmitalContext mainContext)
        {
            baseQ = baseQ.Where(port => port.PORTID == portId);

            IQueryable<Ports> q = baseQ.Join(mainContext.CTBCOUNTRIES, port => port.COUNTRYID, country => country.COUNTRYID,
                 (port, country) => new Ports { Name = port.NAMEENG, Code = port.PORTID, CountryName = country.NAMEENG, CountryId = country.COUNTRYID, SEARCHENG = port.SEARCHENG });
            return q;
        }

        public class QPorts
        {
            public string NAMEENG { get; set; }
            public string PORTID { get; set; }
            public string COUNTRYID { get; set; }
            public string SEARCHENG { get; set; }
        }

        public class Ports
        {
            public string Name;
            public string Code;
            public string CountryName;
            public string CountryId;
            public string SEARCHENG;
        }
    }
}
