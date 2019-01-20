using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO
{
    public  class WCO
    {
        private static WCO _Instance;
        private readonly IReadOnlyCollection<WCOErrorPointerModel> _DB;
        private readonly IReadOnlyCollection<WCOErrorPointerModel> _DBManifest;

        private List<WCOErrorPointerModel> MyCopyOfDB
        {
            get
            {
                var myCopyOfDB = _DB.Select(rec => rec.CreateNew()).ToList();
                return myCopyOfDB;
            }
        }
        private List<WCOErrorPointerModel> MyCopyOfDBManifest
        {
            get
            {
                var myCopyOfDB = _DBManifest.Select(rec => rec.CreateNew()).ToList();
                return myCopyOfDB;
            }
        }


        public static WCO Instance
        {
            get
            {
                if (WCO._Instance == null)
                {
                    WCO._Instance = new WCO(); 
                }
                return WCO._Instance;
            }
            
        }
        WCO()
        {
            var rows = BuildDB();
            this._DB = rows.Select((rec, currSeq) => GetWCOErrorPointerModel(rec, currSeq)).ToList();


            var rowsDBManifest = BuildDBManifest();
            this._DBManifest = rowsDBManifest.Select((rec, currSeq) => GetWCOErrorPointerModelManifest(rec, currSeq)).ToList();
        }
        private List<string> BuildDBManifest()
        {
            string ResourceStreamPath = "Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DBManifest_18.csv";
            ResourceStreamPath = "Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DBManifest_18UTF8.csv";
            //string text = GetResource(ResourceStreamPath);
            string text;
            //bool fromResource = false;
            //if (fromResource)
            //{
            //    //text = WCOResource.DBManifest_18;
            //}
            //else
            {
                text = UnifreightIIG.Resources.IIGResource.DBManifest_18UTF8; // GetResource(ResourceStreamPath);
            }
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var rows = lines.Skip(3);//remove header
            return rows.ToList();

        }
        private List<string> BuildDB()
        {
            
            string ResourceStreamPath = "Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DB.csv";
            ResourceStreamPath = "Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO.DBUTF8.csv";
            string text;
            //bool fromResource = false;
            //if (fromResource)
            //{
            //    //text = WCOResource.DB;
            //}
            //else
            {
                text = UnifreightIIG.Resources.IIGResource.DBUTF8; ///GetResource(ResourceStreamPath);
            }
            //string text = GetResource(ResourceStreamPath);
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var rows = lines.Skip(1);//remove header
            return rows.ToList();

        }
        
        private string GetResource(string ResourceStreamPath)
        {
            var myStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceStreamPath);
            if (myStream == null)
            {
                throw new Exception("ManifestResourceStream is null (" + ResourceStreamPath + ") ");
            }

            StreamReader reader = new StreamReader(myStream);
            var text = reader.ReadToEnd();
            return text;
        }
        private WCOErrorPointerModel GetWCOErrorPointerModelManifest(string line, int currSeq)
        {
            try
            {
                var data = line.Split(',');
                int Key = -1;
                int.TryParse(data[0], out Key);//Key { get; set; } // A=1
                int Level = -1;
                int.TryParse(data[2], out Level);//Level { get; set; } //C =3
                if (Level > 0) Level--;

                var wco = new WCOErrorPointerModel()
                {
                    IndexSeq = currSeq,

                    Key = Key,// Convert.ToInt32(data[0]), //Key { get; set; } // A=1
                    Level = Level ,//Convert.ToInt32(data[2]), //Level { get; set; } //C =3
                    WCOID = data[3], //WCOID { get; set; }//D=4
                    XmlTag = data[13], //{ get; set; } // M=14

                };
                var logitudePointer = LogitudePointerDB.Rows.FirstOrDefault(rec => rec.Key == wco.Key & rec.WCOID == wco.WCOID);
                if (logitudePointer != null)
                {
                    wco.LogitudeEntity = logitudePointer.LogitudeEntity;
                    wco.LogitudeFieldID = logitudePointer.LogitudeFieldID;
                }

                return wco;

            }
            catch (Exception e)
            {

                throw new Exception("bad format line =" + line);
            }
        }

        private WCOErrorPointerModel GetWCOErrorPointerModel(string line, int currSeq)
        {
            try
            {
                var data = line.Split(',');

                var wco = new WCOErrorPointerModel()
                {
                    IndexSeq = currSeq,

                    Key = Convert.ToInt32(data[0]), //Key { get; set; } // A=1
                    Level = Convert.ToInt32(data[3]), //Level { get; set; } //D =4
                    WCOID = data[4], //WCOID { get; set; }//E=5
                    XmlTag = data[7], //{ get; set; } // H=8

                };
                var logitudePointer = LogitudePointerDB.Rows.FirstOrDefault(rec => rec.Key == wco.Key & rec.WCOID == wco.WCOID);
                if (logitudePointer != null)
                {
                    wco.LogitudeEntity = logitudePointer.LogitudeEntity;
                    wco.LogitudeFieldID = logitudePointer.LogitudeFieldID;
                }
                
                return wco;

            }
            catch (Exception e)
            {

                throw new Exception("bad format line =" + line);
            }
        }


        
        public WCOFluent CreateDB(WCOTypeEnum myWCOTypeEnum= WCOTypeEnum.WCO)
        {
            WCOFluent myWCOFluent = null;
            switch (myWCOTypeEnum)
            {
                case WCOTypeEnum.Manifest:
                    myWCOFluent = new WCOFluent(MyCopyOfDBManifest);
                    break;

                case WCOTypeEnum.WCO:
                default:
                    myWCOFluent = new WCOFluent(MyCopyOfDB);
                    break;
            }
            
            return myWCOFluent;
        }
    }

    public enum WCOTypeEnum
    {
        WCO = 0,
        Manifest = 1

    }
}
