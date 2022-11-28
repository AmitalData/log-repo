
   
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
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs; 
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.CLoseTable
{
   public class ExpressionDetails : Expression, ICloseTable<Expression, ExpressionDetails>
   {
       public List<ExpressionDetails> GetAll()
       {
		    var all = new List<ExpressionDetails>();  
            all.Add(new ExpressionDetails()
            {    
                Code = "DATEDIF", 
                Name = "DateDif", 
                SearchFields = "DATEDIF,DateDif", 
                Body = "(date1,date2,unit)", 
                Description = "Calculates the number of days, months, or years between two dates. Can be: Y, M, D", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "MIN", 
                Name = "Min", 
                SearchFields = "MIN", 
                Body = "(number1,number2)", 
                Description = "Return the smallest value from the numbers supplied", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "MAX", 
                Name = "Max", 
                SearchFields = "MAX", 
                Body = "(number1,number2)", 
                Description = "Return the biggest value from the numbers supplied", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "COUNT", 
                Name = "Count", 
                SearchFields = "COUNT", 
                Body = "(list)", 
                Description = "Return the count of list elements", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "TIMEDIF", 
                Name = "TimeDif", 
                SearchFields = "TIMEDIF,TimeDif", 
                Body = "(time1,time2,unit)", 
                Description = "Calculates the number of hours, minutes, or seconds between two dates. Can be: H, M, S", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "NOW", 
                Name = "Now", 
                SearchFields = "NOW,Now", 
                Body = "()", 
                Description = "Returns Datetime.Now", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "TODAY", 
                Name = "Today", 
                SearchFields = "TODAY,Today", 
                Body = "()", 
                Description = "Returns the current date", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "DAY", 
                Name = "Day", 
                SearchFields = "DAY,Day", 
                Body = "(date)", 
                Description = "Returns a day from dateTime", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "MONTH", 
                Name = "Month", 
                SearchFields = "MONTH,Month", 
                Body = "(date)", 
                Description = "Returns a month from dateTime", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "YEAR", 
                Name = "Year", 
                SearchFields = "YEAR,Year", 
                Body = "(date)", 
                Description = "Returns a year from dateTime", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "TODATETIME", 
                Name = "ToDateTime", 
                SearchFields = "TODATETIME,ToDateTime", 
                Body = "(string)", 
                Description = "Returns DateTime value from string", 
                CategoryCode = "DTE", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "WEEKDAY", 
                Name = "WeekDay", 
                SearchFields = "WEEKDAY,WeekDay", 
                Body = "(date)", 
                Description = "Takes a date and returns a number between 1-7 representing the day of week", 
                CategoryCode = "DTE", 
			});
			
            return all;
       }

	    public void MapPoco(Expression newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Body = this.Body;  
		    newPoco.Description = this.Description;  
		    newPoco.CategoryCode = this.CategoryCode;   
        }

		public string GetSearchFields(Expression rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.Body,",",rec.Description,",",rec.CategoryCode,",");
        }
   }
}

