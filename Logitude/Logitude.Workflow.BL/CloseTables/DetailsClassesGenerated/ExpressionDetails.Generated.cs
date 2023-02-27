
   
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
                Code = "MIN", 
                Name = "Min", 
                SearchFields = "MIN", 
                Body = "(number1,number2)", 
                Description = "Return the smallest value from the numbers supplied", 
                CategoryCode = "MTH", 
                Title = "Min(number1,number2)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "MAX", 
                Name = "Max", 
                SearchFields = "MAX", 
                Body = "(number1,number2)", 
                Description = "Return the biggest value from the numbers supplied", 
                CategoryCode = "MTH", 
                Title = "Max(number1,number2)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "COUNT", 
                Name = "Count", 
                SearchFields = "COUNT", 
                Body = "(list)", 
                Description = "Return the count of list elements", 
                CategoryCode = "MTH", 
                Title = "Count(list)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "TIMEDIF", 
                Name = "TimeDif", 
                SearchFields = "TIMEDIF,TimeDif", 
                Body = "(time1,time2,unit)", 
                Description = "Calculates the number of hours, minutes, or seconds between two dates. Can be: H, M, S", 
                CategoryCode = "DTE", 
                Title = "TimeDif(time1,time2,unit)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "NOW", 
                Name = "Now", 
                SearchFields = "NOW,Now", 
                Body = "()", 
                Description = "Returns Datetime.Now", 
                CategoryCode = "DTE", 
                Title = "Now()", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "TODAY", 
                Name = "Today", 
                SearchFields = "TODAY,Today", 
                Body = "()", 
                Description = "Returns the current date", 
                CategoryCode = "DTE", 
                Title = "Today()", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "DAY", 
                Name = "Day", 
                SearchFields = "DAY,Day", 
                Body = "(date)", 
                Description = "Returns a day of the month in the form of a number between 1 and 31.", 
                CategoryCode = "DTE", 
                Title = "Day(date)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "MONTH", 
                Name = "Month", 
                SearchFields = "MONTH,Month", 
                Body = "(date)", 
                Description = "Returns the month, a number between 1 (January) and 12 (December) in number format of a given date.", 
                CategoryCode = "DTE", 
                Title = "Month(date)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "YEAR", 
                Name = "Year", 
                SearchFields = "YEAR,Year", 
                Body = "(date)", 
                Description = "Returns a year from dateTime", 
                CategoryCode = "DTE", 
                Title = "Year(date)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "TODATETIME", 
                Name = "ToDateTime", 
                SearchFields = "TODATETIME,ToDateTime", 
                Body = "(string)", 
                Description = "Returns DateTime value from string", 
                CategoryCode = "DTE", 
                Title = "ToDateTime(string)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "WEEKDAY", 
                Name = "WeekDay", 
                SearchFields = "WEEKDAY,WeekDay", 
                Body = "(date)", 
                Description = "Takes a date and returns a number between 1-7 representing the day of week", 
                CategoryCode = "DTE", 
                Title = "WeekDay(date)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Name = "DateDif", 
                SearchFields = "DAYS,Days", 
                Code = "DAYS", 
                Body = "(Start_date,End_date, 'D')", 
                Description = "Calculate the number of days between two dates.", 
                CategoryCode = "DTE", 
                Title = "Days(Start_date,End_date)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Name = "DateDif", 
                SearchFields = "MONTHS,Months", 
                Code = "MONTHS", 
                Body = "(Start_date,End_date, 'M')", 
                Description = "Calculate the number of months between two dates.", 
                CategoryCode = "DTE", 
                Title = "Months(Start_date,End_date)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Name = "DateDif", 
                SearchFields = "YEARS,Years", 
                Code = "YEARS", 
                Body = "(Start_date,End_date, 'Y')", 
                Description = "Calculate the number of years between two dates.", 
                CategoryCode = "DTE", 
                Title = "Years(Start_date,End_date)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "LOWER", 
                Name = "Lower", 
                SearchFields = "LOWER,Lower", 
                Body = "(text)", 
                Description = "Convert all letters in the value to lower case.", 
                CategoryCode = "TXT", 
                Title = "Lower(text)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Code = "UPPER", 
                Name = "Upper", 
                SearchFields = "UPPER,Upper", 
                Body = "(text)", 
                Description = "Convert all letters in the value to upper case.", 
                CategoryCode = "TXT", 
                Title = "Upper(text)", 
			});
			 
            all.Add(new ExpressionDetails()
            {    
                Name = "Concat", 
                SearchFields = "CONCAT,Concat", 
                Code = "CONCAT", 
                Body = "(value1,value2,...)", 
                Description = "Concates values.", 
                CategoryCode = "TXT", 
                Title = "Concat(value1,value2,...)", 
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
		    newPoco.Title = this.Title;   
        }

		public string GetSearchFields(Expression rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.Body,",",rec.Description,",",rec.CategoryCode,",",rec.Title,",");
        }
   }
}

