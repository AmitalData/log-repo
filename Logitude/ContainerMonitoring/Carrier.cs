using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unifreight.ContainerTasks
{
    public class Carrier
    {
        public string short_name;
        public string official_name;
        public string scac;
        public override string ToString()
        {
            return (scac + "-" + official_name);
        }

    }
}
