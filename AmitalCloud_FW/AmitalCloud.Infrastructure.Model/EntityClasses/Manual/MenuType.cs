using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class MenuType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

        //  public List<MenusTable> MenusTables { get; set; }

    }
}