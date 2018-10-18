using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.ClosedTables
{
    public class AutonomyDetails
    {
        static List<AutonomyDetails> _All;
        static AutonomyDetails()
        {
            _All = new List<AutonomyDetails>(){
                new AutonomyDetails() { Code = "0" , Value="לא אוטונומיה"},
                new AutonomyDetails() { Code = "1" , Value="אוטונומיה"},
                new AutonomyDetails() { Code = "3" , Value="הסכם יו'ש"},    
            };
        }
        
        public string Code { get; set; }
        public string Value { get; set; }
        public  static List<AutonomyDetails> GetAll()
        {
            return _All;      
        }

        public AutonomyDetails GetSingle(string code)
        {
            return _All.FirstOrDefault(rec => rec.Code == code);
        }
    }
}
