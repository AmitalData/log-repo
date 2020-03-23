
   
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.CloseTablesClasses;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL
{
   public class MeasurmentUnitDetails : MeasurmentUnit, ICloseTable<MeasurmentUnit, MeasurmentUnitDetails>
   {
       public List<MeasurmentUnitDetails> GetAll()
       {
		    var all = new List<MeasurmentUnitDetails>();  
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "2I", 
                SearchFields = "2I,BTU לשעה", 
                Inactive = false, 
                LocalName = "BTU לשעה", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "ANN", 
                SearchFields = "ANN,שנה", 
                Inactive = false, 
                LocalName = "שנה", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "C26", 
                SearchFields = "C26,מילי-שניות", 
                Inactive = false, 
                LocalName = "מילי-שניות", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "C47", 
                SearchFields = "C47,ננו-שניות", 
                Inactive = false, 
                LocalName = "ננו-שניות", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "CEL", 
                SearchFields = "CEL,מעלות צלזיוס", 
                Inactive = false, 
                LocalName = "מעלות צלזיוס", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "CLT", 
                SearchFields = "CLT,סנטיליטר", 
                Inactive = false, 
                LocalName = "סנטיליטר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "CMK", 
                SearchFields = "CMK,סנטימטר רבוע", 
                Inactive = false, 
                LocalName = "סנטימטר רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "CMQ", 
                SearchFields = "CMQ,ס''מ מעוקב", 
                Inactive = false, 
                LocalName = "ס''מ מעוקב", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "CMT", 
                SearchFields = "CMT,סנטימטר", 
                Inactive = false, 
                LocalName = "סנטימטר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "CTM", 
                SearchFields = "CTM,קרט", 
                Inactive = false, 
                LocalName = "קרט", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "DAY", 
                SearchFields = "DAY,יממה", 
                Inactive = false, 
                LocalName = "יממה", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "DMK", 
                SearchFields = "DMK,דצימטר רבוע", 
                Inactive = false, 
                LocalName = "דצימטר רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "DMT", 
                SearchFields = "DMT,דצימטר", 
                Inactive = false, 
                LocalName = "דצימטר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "DZN", 
                SearchFields = "DZN,תריסר", 
                Inactive = false, 
                LocalName = "תריסר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "EA", 
                SearchFields = "EA,כל אחד", 
                Inactive = false, 
                LocalName = "כל אחד", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "FOT", 
                SearchFields = "FOT,רגל", 
                Inactive = false, 
                LocalName = "רגל", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "FTK", 
                SearchFields = "FTK,רגל רבוע", 
                Inactive = false, 
                LocalName = "רגל רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "GLL", 
                SearchFields = "GLL,גאלון", 
                Inactive = false, 
                LocalName = "גאלון", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "GRM", 
                SearchFields = "GRM,גרם", 
                Inactive = false, 
                LocalName = "גרם", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "GRO", 
                SearchFields = "GRO,גרוס", 
                Inactive = false, 
                LocalName = "גרוס", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "HUR", 
                SearchFields = "HUR,שעה", 
                Inactive = false, 
                LocalName = "שעה", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "ILA", 
                SearchFields = "ILA,ליטר אלכוהול", 
                Inactive = false, 
                LocalName = "ליטר אלכוהול", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "INH", 
                SearchFields = "INH,אינצ'", 
                Inactive = false, 
                LocalName = "אינצ'", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "INK", 
                SearchFields = "INK,אינצ' רבוע", 
                Inactive = false, 
                LocalName = "אינצ' רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "INQ", 
                SearchFields = "INQ,אינצ' מעוקב", 
                Inactive = false, 
                LocalName = "אינצ' מעוקב", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "KGM", 
                SearchFields = "KGM,קילוגרם", 
                Inactive = false, 
                LocalName = "קילוגרם", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "KMK", 
                SearchFields = "KMK,ק''מ רבוע", 
                Inactive = false, 
                LocalName = "ק''מ רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "KMT", 
                SearchFields = "KMT,קילומטר", 
                Inactive = false, 
                LocalName = "קילומטר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "LBR", 
                SearchFields = "LBR,ליברה (פאונד)", 
                Inactive = false, 
                LocalName = "ליברה (פאונד)", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "LTR", 
                SearchFields = "LTR,ליטר", 
                Inactive = false, 
                LocalName = "ליטר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "M5", 
                SearchFields = "M5,מיקרו קירי", 
                Inactive = false, 
                LocalName = "מיקרו קירי", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MIK", 
                SearchFields = "MIK,מייל רבוע", 
                Inactive = false, 
                LocalName = "מייל רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MIN", 
                SearchFields = "MIN,דקה", 
                Inactive = false, 
                LocalName = "דקה", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MMK", 
                SearchFields = "MMK,מילימטר רבוע", 
                Inactive = false, 
                LocalName = "מילימטר רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MMT", 
                SearchFields = "MMT,מילימטר", 
                Inactive = false, 
                LocalName = "מילימטר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MON", 
                SearchFields = "MON,חודש", 
                Inactive = false, 
                LocalName = "חודש", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MTK", 
                SearchFields = "MTK,מטר רבוע", 
                Inactive = false, 
                LocalName = "מטר רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MTQ", 
                SearchFields = "MTQ,מ''ק", 
                Inactive = false, 
                LocalName = "מ''ק", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "MTR", 
                SearchFields = "MTR,מטר", 
                Inactive = false, 
                LocalName = "מטר", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "ONZ", 
                SearchFields = "ONZ,מסה )משקל(", 
                Inactive = false, 
                LocalName = "מסה )משקל(", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "PK", 
                SearchFields = "PK,חבילה", 
                Inactive = false, 
                LocalName = "חבילה", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "PR", 
                SearchFields = "PR,זוג", 
                Inactive = false, 
                LocalName = "זוג", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "PT", 
                SearchFields = "PT,פינט", 
                Inactive = false, 
                LocalName = "פינט", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "SEC", 
                SearchFields = "SEC,שניה", 
                Inactive = false, 
                LocalName = "שניה", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "SMI", 
                SearchFields = "SMI,מייל", 
                Inactive = false, 
                LocalName = "מייל", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "T3", 
                SearchFields = "T3,אלף יחידות", 
                Inactive = false, 
                LocalName = "אלף יחידות", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "T3C", 
                SearchFields = "T3C,אלף סיגריות", 
                Inactive = false, 
                LocalName = "אלף סיגריות", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "TNE", 
                SearchFields = "TNE,טון", 
                Inactive = false, 
                LocalName = "טון", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "VLT", 
                SearchFields = "VLT,מתח חשמלי", 
                Inactive = false, 
                LocalName = "מתח חשמלי", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "WEE", 
                SearchFields = "WEE,שבוע", 
                Inactive = false, 
                LocalName = "שבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "YDK", 
                SearchFields = "YDK,יארד רבוע", 
                Inactive = false, 
                LocalName = "יארד רבוע", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "YDQ", 
                SearchFields = "YDQ,יארד מעוקב", 
                Inactive = false, 
                LocalName = "יארד מעוקב", 
			});
			 
            all.Add(new MeasurmentUnitDetails()
            {    
                Code = "YRD", 
                SearchFields = "YRD,יארד", 
                Inactive = false, 
                LocalName = "יארד", 
			});
			
            return all;
       }

	    public void MapPoco(MeasurmentUnit newPoco)
        {   
		    newPoco.Code = this.Code;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Inactive = this.Inactive;  
		    newPoco.LocalName = this.LocalName;   
        }

		public string GetSearchFields(MeasurmentUnit rec)
        {   
           return String.Concat(rec.Code,",",rec.Inactive,",",rec.LocalName,",");
        }
   }
}

