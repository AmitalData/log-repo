using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class DocumentFolder
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public bool IsExternalFolder { get; set; }
        public string ParentFolderId { get; set; }
        public string SearchFields { get; set; }

        [ForeignKey("ParentFolderId")]
        public DocumentFolder ParentFolder { get; set; }
    }
}