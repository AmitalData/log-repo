using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
    [DataContract(Namespace = "")]
    public class TreeFilter : WhereFilter
    {
        public TreeFilter()
        {
            OperatorType = TreeFilterType.None;
        }
        [DataMember]
        public TreeFilterType OperatorType { get; set; }
        [DataMember]
        public List<TreeFilter> AdditionalFilters { get; set; }
    }

    [DataContract(Namespace = "")]
    public class WhereFilter
    {
        public WhereFilter()
        {
            FilterType = WhereFilterType.None;
            MainEntityName = string.Empty;
            Field = string.Empty;
            SecondaryEntityName = string.Empty;
        }
        [DataMember]
        public string MainEntityName { get; set; }
        [DataMember]
        public string Field { get; set; }
        [DataMember]
        public WhereFilterType FilterType { get; set; }
        [DataMember]
        public string SecondaryEntityName { get; set; }
        [DataMember]
        public object Value { get; set; }
    }

    public enum TreeFilterType
    {
        None,
        And,
        Or
    }

    public enum WhereFilterType
    {
        None,
        Equal,
        NotEqual,
        LessThan,
        GreaterThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        Contains,
        NotContains,
        StartsWith,
        NotStartsWith,
        EndsWith,
        NotEndsWith,
        Any,
        NotAny,
        IsNull,
        IsNotNull,
        IsEmpty,
        IsNotEmpty,
        IsNullOrEmpty,
        IsNotNullOrEmpty
    }
}
