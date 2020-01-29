using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data; 
using Logitude.Accounting.Data.EntityMapping;

namespace Logitude.Accounting.Data
{

    public partial interface IAccountingContext : IContext
    {
   
       	 IDbSet<AccountingCompanyType> AccountingCompanyTypes { get; }
		 IDbSet<AccountingEntity> AccountingEntities { get; }
		 IDbSet<AccountingIntegrityCheck> AccountingIntegrityChecks { get; }
		 IDbSet<AccountingNote> AccountingNotes { get; }
	