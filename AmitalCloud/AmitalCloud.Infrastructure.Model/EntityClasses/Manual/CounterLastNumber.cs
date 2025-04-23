using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CounterLastNumber
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[StringLength(40, ErrorMessage = "The maximum length of the english name is 40!")]
        //[Display(Name = "Name")]
        public string TableName { get; set; }
        //[Required]

        public int Tenant { get; set; }

        public int LastNumber { get; set; }

    }
}