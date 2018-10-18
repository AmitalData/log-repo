//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Logitude.Customs.Data.EntityPOCOs
//{
//   public class ImporterDepositionVendorsView
//    {
//       [Key]
//       [Required]
//       [StringLength(15, MinimumLength = 0)]
//       [Column("VendorId", TypeName = "varchar")]
//       public string VendorId { get; set; }

//       [StringLength(9, MinimumLength = 0)]
//       [Column("VendorNumber", TypeName = "varchar")]
//       public string VendorNumber { get; set; }

      
//       [StringLength(55, MinimumLength = 0)]
//       [Column("VendorName", TypeName = "varchar")]
//       public string VendorName { get; set; }

//       [StringLength(2, MinimumLength = 0)]
//       [Column("CountryCode", TypeName = "varchar")]
//       public string CountryCode { get; set; }

//       [StringLength(35, MinimumLength = 0)]
//       [Column("CityName", TypeName = "varchar")]
//       public string CityName { get; set; }
//       [StringLength(70, MinimumLength = 0)]
//       [Column("MainAddressLine", TypeName = "varchar")]
//       public string MainAddressLine { get; set; }

//       [StringLength(15, MinimumLength = 0)]
//       [Column("VATNumber", TypeName = "varchar")]
//       public string VATNumber { get; set; }

//       [StringLength(9, MinimumLength = 0)]
//       [Column("DunsNumber", TypeName = "varchar")]
//       public string DunsNumber { get; set; }
//    }
//}
