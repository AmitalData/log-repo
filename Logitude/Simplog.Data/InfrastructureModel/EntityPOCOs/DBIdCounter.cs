using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DBIdCounter
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "The maximum length of the english name is 50!")]
        //[Display(Name = "Name")]
        public string TableName { get; set; }
        //[Required]
        public int LastIdNumber { get; set; }

    }
}