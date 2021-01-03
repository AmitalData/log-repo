using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Interfaces
{
    public abstract class FieldShortNameGetter
    {
        public Dictionary<string, string> FieldShortNames { get; set; }
        public FieldShortNameGetter()
        {
            InitializeShortNames();
        }
        public abstract void InitializeShortNames();
        public string GetFieldShortName(string fieldName)
        {
            if (!FieldShortNames.Keys.Contains(fieldName))
            {
                throw new Exception(fieldName + "doesn't have a short name please add it to the FieldShortNames dictionary");
            }
            return FieldShortNames[fieldName];
        }
    }
}
