using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class DocumentStatusPM : BaseEntityPM
    {
        public DocumentStatusPM() : base() { }
        public DocumentStatusPM(DocumentStatus entity) : base()  { }
    }
}
