using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{

    public interface IContainerAccessor
    {
        IUnityContainer Container { get; }
    }
}
