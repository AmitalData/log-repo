using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Helpers
{
    public class MenuItemClass
    {
        public MenuItemClass()
        {
        }
        public string StatusCode { get; set; }
        public string LocalName { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string ExceptionMessage { get; set; }
        public string ItemId { get; set; }
        public string FilterXML { get; set; }
        public string TemplateId { get; set; }
        public bool NotDisplayInMenu { get; set; }
        public int ItemType { get; set; }
        public string Id { get; set; }
    }

   public   enum MenuTypes
    {
       
        ReportExecutionLog = 0,
        BatchTaskExecution = 1,
        ExcelExport = 2
    }
    
}
