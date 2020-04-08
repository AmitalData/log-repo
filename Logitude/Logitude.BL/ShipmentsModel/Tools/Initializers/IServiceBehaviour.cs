using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Initializers
{
    public interface IServiceBehaviour
    {
        void Handle(IServiceInitializer initializer);
    }
}
