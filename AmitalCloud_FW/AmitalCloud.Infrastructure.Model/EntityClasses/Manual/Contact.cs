using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public partial class Contact
    {
        public virtual User User { get; set; }
        [DataMember]
        public IEnumerable<string> Roles
        {
            get { return this.Email.Split(','); }
            set { this.Email = string.Join(",", value.ToArray()); }
        }

        public string Name
        {
            get
            {
                return Email;
            }
            set
            {
                Email = value;
            }
        }
    }
}
