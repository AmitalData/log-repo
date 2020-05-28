using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class LogBoxTenantSettingList
    {
        [Key]
        public int Id { get; set; }
       
        public bool IsDocumentsArchive { get; set; }
        public bool CustomerTenantShareImportFile { get; set; }
    }
}