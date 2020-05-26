using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.CloseTables
{
    public class DeclaraionDetails
    {
        /*
                * 
A- סוכן
R- נציג מטפל
B - בלדר

                */
        public List<AgentRoleCodeDetail> GetAllAgentRoleCodeDetails()
        {
            var all = new List<AgentRoleCodeDetail>()
            {
                new AgentRoleCodeDetail()
                {
                    Code = "A",
                    Name = "Agent",

                },

                new AgentRoleCodeDetail()
                {
                    Code = "R",
                    Name = "Referant",

                },
                 new AgentRoleCodeDetail()
                {
                    Code = "B",
                    Name = "בלדר",

                },

            };
            return all;

        }
    }

    public class AgentRoleCodeDetail
    {
        public string Code { get; internal set; }
        public string Name { get; internal set; }
    }
}
