using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class TextCodeType
    {
        [Key]
        public string  Code { get; set; }
        public string Name { get; set; }
        
        
        //[Include]
        //[Association("TextCodeTextCodeType", "Code", "TextCodeTypeCode")]
        //public List<TextCode> TextCodes { get; set; }

    }
}