using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class MenuType
    {
        [Key]
        public string  Code { get; set; }
        public string Name { get; set; }

      //  public List<MenusTable> MenusTables { get; set; }

    }
}