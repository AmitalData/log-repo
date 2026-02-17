using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AmitalMessaging.Infrastructure
{
    public partial class GenericResponseObj
    {
        public enum StatusEnum
        {
            TecinicalFailure,
            BusinessError,
            Success
        }
        StatusEnum _StatusType;

        public StatusEnum StatusType
        {
            get { return _StatusType; }
            set
            {
                _StatusType = value;
                this.Status = _StatusType.ToString();
            }
        }

        
    }
}
