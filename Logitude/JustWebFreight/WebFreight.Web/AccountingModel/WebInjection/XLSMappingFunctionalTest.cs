using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.AccountingModel.WebInjection
{
    /// <summary>
    /// 
    /// </summary>
    public class XLSMappingFunctionalTest
    {

        public void MapIt(byte[] byteArray)
        {
            //Creates a new instance for ExcelEngine
            var excelEngine = new ExcelEngine();
            //byte[] byteArray = Encoding.ASCII.GetBytes(test);
            var stream = new MemoryStream(byteArray);
            //Loads or open an existing workbook through Open method of IWorkbooks
            IWorkbook workbook = excelEngine.Excel.Workbooks.Open(stream);
            
        }
    }
}