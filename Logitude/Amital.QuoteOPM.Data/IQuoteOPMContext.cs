using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Amital.QuoteOPM.Data.EntityPOCOs;

namespace Amital.QuoteOPM.Data
{

    public partial interface IQuoteOPMContext : IContext
    {
   
       	 IDbSet<QuoteOP> QuoteOPs { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}