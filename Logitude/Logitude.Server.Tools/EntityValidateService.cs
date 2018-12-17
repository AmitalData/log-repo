using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public abstract class EntityValidateService<TEntityPM>
    {
        public List<string> ErrorsList;
        public EntityValidateService()
        {
            ErrorsList = new List<string>();
        }
        public virtual void Validate(TEntityPM entityPM) { }
        public void AddValidationError(string error)
        {
            ErrorsList.Add(error);
        }
    }
}
