using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class TextCodeType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }


        //[Include]
        //[Association("TextCodeTextCodeType", "Code", "TextCodeTypeCode")]
        //public List<TextCode> TextCodes { get; set; }

    }
}