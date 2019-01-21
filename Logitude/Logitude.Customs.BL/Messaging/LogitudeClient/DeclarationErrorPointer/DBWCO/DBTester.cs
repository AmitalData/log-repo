using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO
{
    public class DBTester
    {
        
        public static void test()
        {
            //dec.GoodShip.Consignment.
            //var list = WCO.Instance.CreateDB().GetCopyList();
            //var logList = list.Where(rec => rec.LogitudeFieldID != null).ToList();  

            if (false)
            {
                var myDB = WCO.Instance.CreateDB();
                myDB = myDB.GetNode(0, "42A");
                myDB = myDB.GetNode(1, "67A");
                myDB = myDB.GetNode(2, "28A");
                myDB = myDB.GetNode(3, "01L");
                var myCargoDescription = myDB.GetTagID(4, "138");

            }




            var myDB1 = WCO.Instance.CreateDB(WCOTypeEnum.Manifest);
            myDB1 = myDB1.GetNode(0, "42A");
            myDB1 = myDB1.GetNode(1, "28A");
            myDB1 = myDB1.GetNode(2, "38B");
            
            var myCargoDescription1 = myDB1.GetTagID(4, "138");
            


        }
        private void dd()
        {
            string[] lines;
            string text;
            string ResourceStreamPath="Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DB.csv";
            bool fromResource = true;
            if (fromResource)
            {
                text = WCOResource.DB;
            }
            else
            {
                text = GetResource(ResourceStreamPath);
            }
            lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var rows= lines.Skip(1);//remove header
            int MyIndex=0;
            //var query = from line in rows
            //            let data = line.Split(',')
            //            select new WCOErrorPointerModel()
            //            {
            //                IndexSeq =MyIndex++ ,

            //                Key =  Convert.ToInt32( data[0]), //Key { get; set; } // A=1
            //                Level = Convert.ToInt32( data[3]), //Level { get; set; } //D =4
            //                WCOID = data[4], //WCOID { get; set; }//E=5
            //                XmlTag = data[7], //{ get; set; } // H=8

            //            };
            var query = rows.Select((rec, currSeq) => GetWCOErrorPointerModel(rec, currSeq));
            TestCopy(query);
            


            query.ToList().ForEach(rec => Debug.WriteLine(rec.ToString()));
            var myGoodsShipment= query.FirstOrDefault(rec => rec.Level == 1 & rec.WCOID == "67A");

            var myGoodsShipmentChilds = GetChildren(query.ToList() ,myGoodsShipment);
            var level1 = GetChildren(query.ToList(), query.FirstOrDefault(rec => rec.Level == 0 ));


        }

        private static void TestCopy(IEnumerable<WCOErrorPointerModel> query)
        {
            var myDB1 = query.ToList();
            var myDB2 = myDB1.Select(rec => rec.CreateNew()).ToList();

            var my1a = myDB1.FirstOrDefault();
            
            my1a.XmlTag = "1111";

            if (myDB2.First().XmlTag == my1a.XmlTag)
            {
                Debug.WriteLine("bad");
            }
            
        }

        private static WCOErrorPointerModel GetWCOErrorPointerModel(string line, int currSeq)
        {
            var data = line.Split(',');
            return new WCOErrorPointerModel()
                        {
                            IndexSeq = currSeq,

                            Key = Convert.ToInt32(data[0]), //Key { get; set; } // A=1
                            Level = Convert.ToInt32(data[3]), //Level { get; set; } //D =4
                            WCOID = data[4], //WCOID { get; set; }//E=5
                            XmlTag = data[7], //{ get; set; } // H=8

                        };
        }

        private static List<WCOErrorPointerModel> GetChildren(List<WCOErrorPointerModel> list, WCOErrorPointerModel myElement)
        {
            if (myElement == null) return null;
            if (list == null) return null;

            var elmIndex = myElement.IndexSeq;
            var elmLevel = myElement.Level;
            var siblingElment = list.OrderBy( rec => rec.IndexSeq).FirstOrDefault(rec => rec.Level == myElement.Level & rec.IndexSeq > myElement.IndexSeq);
            var nextSiblingElmentindex = list.Last().IndexSeq;
            if (siblingElment != null)
            {
                nextSiblingElmentindex = siblingElment.IndexSeq;
            }
            int numOfElment = nextSiblingElmentindex - elmIndex;
            var allChilds=  list.GetRange((elmIndex+1), numOfElment);
            var only1stChilds = allChilds.OrderBy( rec => rec.IndexSeq).Where(rec => rec.Level == (elmLevel + 1)).ToList() ;
            return only1stChilds;

        }

       

        private static string GetResource(string ResourceStreamPath)
        {
            var myStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceStreamPath);
            StreamReader reader = new StreamReader(myStream);
            var text = reader.ReadToEnd();
            return text;
        }
       
    }
}