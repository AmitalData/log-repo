
   
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
   public class OperatorDetails : Operator, ICloseTable<Operator, OperatorDetails>
   {
       public List<OperatorDetails> GetAll()
       {
		    var all = new List<OperatorDetails>();  
            all.Add(new OperatorDetails()
            {    
                Code = "ADD", 
                Name = "Add", 
                SearchFields = "ADD,Add", 
                Sign = "+", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "SUB", 
                Name = "Subtract", 
                SearchFields = "SUB,Subtract", 
                Sign = "-", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "MUL", 
                Name = "Multiply", 
                SearchFields = "MUL,Multiply", 
                Sign = "*", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "DIV", 
                Name = "Divide", 
                SearchFields = "DIV,Divide", 
                Sign = "/", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "EXP", 
                Name = "Exponentiation", 
                SearchFields = "EXP,Exponentiation", 
                Sign = "^", 
                CategoryCode = "MTH", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "LRT", 
                Name = "Lower than", 
                SearchFields = "LRT,Lower than", 
                Sign = "<", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "LRE", 
                Name = "Lower or equal", 
                SearchFields = "LRE,Lower or equal", 
                Sign = "<=", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "GRT", 
                Name = "Greater than", 
                SearchFields = "GRT,Greater than", 
                Sign = ">", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "GRE", 
                Name = "Greater or equal", 
                SearchFields = "GRE,Greater or equal", 
                Sign = ">=", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "EQL", 
                Name = "Equal", 
                SearchFields = "EQL,Equal", 
                Sign = "=", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "NEL", 
                Name = "Not Equal ", 
                SearchFields = "NEL,Not Equal ", 
                Sign = "!=", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "AND", 
                Name = "AND", 
                SearchFields = "AND", 
                Sign = "&&", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "OR", 
                Name = "OR", 
                SearchFields = "OR", 
                Sign = "||", 
                CategoryCode = "BOL", 
			});
			 
            all.Add(new OperatorDetails()
            {    
                Code = "NGN", 
                Name = "Negation", 
                SearchFields = "NGN,Negation", 
                Sign = "!", 
                CategoryCode = "BOL", 
			});
			
            return all;
       }

	    public void MapPoco(Operator newPoco)
        {   
		    newPoco.Code = this.Code;  
		    newPoco.Name = this.Name;  
			newPoco.SearchFields = GetSearchFields(this);   
		    newPoco.Sign = this.Sign;  
		    newPoco.CategoryCode = this.CategoryCode;   
        }

		public string GetSearchFields(Operator rec)
        {   
           return String.Concat(rec.Code,",",rec.Name,",",rec.Sign,",",rec.CategoryCode,",");
        }
   }
}

