using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplog.Server.Infrastructure
{
    public abstract class EntityKeyFields
    {
        public abstract string GetFullKey();

        public abstract string GetEntityPMName();
        

    }
}
