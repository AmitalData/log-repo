using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXMLFixer.Models
{
    public class LXMLFileFixer
    {
        public string FilePath { get; set; }
        public List<LXMLAttribute> Attributes { get; set; }
    }
    
    public class LXMLAttribute
    {
        public string ElementName { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public LXMLAttributeFilter AttributeFilter { get; set; }
    }

    public class LXMLAttributeFilter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
