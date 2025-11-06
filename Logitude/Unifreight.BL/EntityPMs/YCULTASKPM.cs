using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class YCULTASKPM : EntityPM
    {     
        public static short calcPriority(String taskType)
        {
            switch (taskType)
            {
                case "L2U":
                    return 1;
                case "LD2U":
                    return 2;
                case "LP2U":
                case "LP2UB":
                    return 3;
                case "L2UCREF":
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
