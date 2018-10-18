using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class ScreenFieldPM
    {
        [Key]
        public string Id { get; set; }

       
        public int Tenant { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }

        public string ScreenId { get; set; }
              
        public string ObjectFieldId { get; set; }

        public string ObjectFieldName { get; set; }

        public string ScreenCode { get; set; }

        public string ObjectFieldObjectTableName { get; set; }

       

    }
}
