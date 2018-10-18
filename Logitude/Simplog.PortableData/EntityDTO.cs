using System;
using System.Collections.Generic;
using System.Text;

namespace Simplog.PortableData
{
    public abstract class EntityDTO<T>
    {
        public ChangeSetOperations ChangeSetOperation { get; set; }
        public T OriginalEntity { get; set; }

        public List<ValidationErrorInfo> ValidationErrorsList = new List<ValidationErrorInfo>();//{ get; set; }

        public bool HasErrors { get { return ValidationErrorsList.Count > 0; } }
    }
}
