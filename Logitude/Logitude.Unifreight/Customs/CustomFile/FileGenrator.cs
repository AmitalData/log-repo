using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AmitalMessaging.Customs.CustomFile

{
    public class FileGenrator
    {
        public static  string GetCFIFILEMTest()
        {
            var root = new CFIFILEM();
            var listcustoms_file = new List<customs_file>();
            var curr_customs_file = new customs_file();
            curr_customs_file.entry_file_no = "234234";
            curr_customs_file.eta = "10.02.2013";
            curr_customs_file.file_number = new file_number() { primary = "100", Value = "100" };

            listcustoms_file.Add(curr_customs_file);
            root.customs_file = listcustoms_file.ToArray();

            var mySerilazeObject = "";
            //var mySerilazeObject = XmlGenericUtil<CFIFILEM>
            //    .SerilazeObject(root, true);

            Debug.WriteLine(mySerilazeObject);


            return mySerilazeObject;
 
        }
    }
}
