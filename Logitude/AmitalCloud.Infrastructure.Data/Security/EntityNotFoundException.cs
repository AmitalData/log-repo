using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Security
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }
        public EntityNotFoundException(string errorMessage) : base(errorMessage)
        {
        }
    }
}

