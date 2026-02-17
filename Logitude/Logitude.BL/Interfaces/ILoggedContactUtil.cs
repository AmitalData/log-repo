using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Interfaces
{
    public interface ILoggedContactUtil
    {
        ContactPM GetLoggedContact(int tenant);
    }
}
