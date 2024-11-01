using System;
using System.Collections.Generic;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class GlobalTenantCounter
    {
        public int Id { get; set; }
        public int LastNumber { get; set; }
    }
}
