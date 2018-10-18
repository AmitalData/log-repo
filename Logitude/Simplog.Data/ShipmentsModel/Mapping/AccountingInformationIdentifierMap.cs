using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Mapping
{
    public class AccountingInformationIdentifierMap : EntityTypeConfiguration<AccountingInformationIdentifier>
    {
        public AccountingInformationIdentifierMap()
        {
            this.HasKey(t => t.Code);

            this.Property(t => t.Code).IsRequired().HasMaxLength(3).IsUnicode(false);
            this.Property(t => t.Name).IsRequired().HasMaxLength(100).IsUnicode(false);
            this.Property(t => t.SearchFields).HasMaxLength(1000).IsUnicode(true);
//#if ORACLE_DB
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            if (dbms == "oracle")
            {
                this.ToTable("AccountingInfoIdentifiers");
            }
            //#else
            else
            {
                this.ToTable("AccountingInformationIdentifiers");
            }
           
//#endif

            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SearchFields).HasColumnName("SearchFields");
        }
    }
}
