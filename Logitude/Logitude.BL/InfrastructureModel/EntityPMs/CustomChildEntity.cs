using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class CustomChildEntity
    {
        public string Name { get; set; }
        public List<CustomChildObjectPM> Values { get; set; }
    }
}
