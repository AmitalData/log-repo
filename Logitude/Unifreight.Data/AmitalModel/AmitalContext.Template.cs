//OpenAccess please define!!! 
//logitude please undefine!!!  
//#define reserveword


using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.Data.AmitalModel
{

    public partial class AmitalContext : DbContextBase
    {
        public override LogitudeDBSchema LogitudeDBSchema
        {
            get { return Simplog.Server.Infrastructure.LogitudeDBSchema.AMITAL_DB; }
        }

        
        public static DbModelBuilder GetBuilder()//protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DbModelBuilder modelBuilder = new DbModelBuilder(DbModelBuilderVersion.V4_1);
            bool its4Grant = true;
            if (its4Grant)
            {

                modelBuilder.Entity<CFIFILEM>()
                    .HasKey(p => new { p.FILE_NO })
                    .ToTable("CFIFILEM", "AMITESTM");

            }


            #region YCULPROCESS

            modelBuilder.Entity<YCULPROCESS>()
                    .HasKey(p => new { p.ENTNAME, p.PRIMARYNUM })
                    .ToTable("YCULPROCESS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<YCULPROCESS>()
                .Property(p => p.ENTNAME)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YCULPROCESS>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULPROCESS>()
                .Property(p => p.PROCESSID)
                    .HasColumnName(@"PROCESS_ID")
                    .HasMaxLength(10)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YCULPROCESS>()
                .Property(p => p.PROCESSTIME)
                    .HasColumnName(@"PROCESS_TIME")
                    .HasColumnType("date");

            #endregion

            #region GGGQ

            modelBuilder.Entity<GGGQ>()
                .HasKey(p => new { p.QUEID })
                .ToTable("GGGQ", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.QUEID)
                    .HasColumnName(@"QUE_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.CREATEDATE)
                    .HasColumnName(@"CREATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.EXECDATE)
                    .HasColumnName(@"EXEC_DATE")
                    .IsRequired()
                    .HasColumnType("date");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.STARTDATE)
                    .HasColumnName(@"START_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.ORIGINQUE)
                    .HasColumnName(@"ORIGIN_QUE")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.USERID)
                    .HasColumnName(@"USER_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.REF)
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.DEPENDENCYREF)
                    .HasColumnName(@"DEPENDENCY_REF")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.DONEOPERATION)
                    .HasColumnName(@"DONE_OPERATION")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.STATUS)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
#if reserveword
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.DESC)
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
#endif
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.EXPTASKTIME)
                    .HasColumnName(@"EXP_TASK_TIME")
                    .HasColumnType("double");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.TRY)
                    .HasColumnType("int");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.LOGLEVEL)
                    .HasColumnName(@"LOG_LEVEL")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.SECNUMBER)
                    .HasColumnName(@"SEC_NUMBER")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.PROCESSID)
                    .HasColumnName(@"PROCESS_ID")
                    .HasColumnType("int64");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.FORMID)
                    .HasColumnName(@"FORM_ID")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.COMPUTERID)
                    .HasColumnName(@"COMPUTER_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING1)
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING2)
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING3)
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING4)
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.QUEUEMANAGEMENT)
                    .HasColumnName(@"QUEUE_MANAGEMENT")
                    .HasColumnType("bool");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.OTHERASNFILE)
                    .HasColumnName(@"OTHER_ASN_FILE")
                    .HasColumnType("bool");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.STOPPEDBYSM)
                    .HasColumnName(@"STOPPED_BY_SM")
                    .HasColumnType("bool");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING5)
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.DEBUG)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.WEAKREF)
                    .HasColumnName(@"WEAK_REF")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.HUGERECORD)
                    .HasColumnName(@"HUGE_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.POSTFAILED)
                    .HasColumnName(@"POST_FAILED")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING6)
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.POSTSUCCESS)
                    .HasColumnName(@"POST_SUCCESS")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.FAILED)
                    .HasColumnType("int");

            #endregion

            #region CCUACCSUP

            modelBuilder.Entity<CCUACCSUP>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUACCSUP", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.ACCOUNTTYPE)
                    .HasColumnName(@"ACCOUNT_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.MAINACCOUNT)
                    .HasColumnName(@"MAIN_ACCOUNT")
                    .HasColumnType("bool");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.SUPPLIERACCOUNT)
                    .HasColumnName(@"SUPPLIER_ACCOUNT")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.SUPPLIERID)
                    .HasColumnName(@"SUPPLIER_ID")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.COUNTRYID)
                    .HasColumnName(@"COUNTRY_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.INCOTERMID)
                    .HasColumnName(@"INCOTERM_ID")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.CURRENCYID)
                    .HasColumnName(@"CURRENCY_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.VALUE)
                    .HasColumnType("double");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.COMMISSION)
                    .HasColumnType("double");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.DECLARATIONNO)
                    .HasColumnName(@"DECLARATION_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.CHANGINGVALUE)
                    .HasColumnName(@"CHANGING_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.SUPPLIERACCOUNTN)
                    .HasColumnName(@"SUPPLIER_ACCOUNT_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.COMMISSIONPERCENT)
                    .HasColumnName(@"COMMISSION_PERCENT")
                    .HasColumnType("double");
            #endregion

            #region CCUMSHGR

            modelBuilder.Entity<CCUMSHGR>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUMSHGR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.MISHGORNO)
                    .HasColumnName(@"MISHGOR_NO")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.TRANSPTYPE)
                    .HasColumnName(@"TRANSP_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.MANIFESTNO)
                    .HasColumnName(@"MANIFEST_NO")
                    .HasMaxLength(6)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.IDENTIFIERTYPE)
                    .HasColumnName(@"IDENTIFIER_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.IDENTIFIERNO)
                    .HasColumnName(@"IDENTIFIER_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.HAWB)
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.HAWBDATE)
                    .HasColumnName(@"HAWB_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.CARNETNUMBER)
                    .HasColumnName(@"CARNET_NUMBER")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.STORAGESITE)
                    .HasColumnName(@"STORAGE_SITE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.WAREHOUSEID)
                    .HasColumnName(@"WAREHOUSE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.WAREHOUSEREC)
                    .HasColumnName(@"WAREHOUSE_REC")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.EXPORTLAND)
                    .HasColumnName(@"EXPORT_LAND")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.LOADPORTID)
                    .HasColumnName(@"LOADPORT_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.DESCOFGOODS1)
                    .HasColumnName(@"DESC_OF_GOODS1")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.DESCOFGOODS2)
                    .HasColumnName(@"DESC_OF_GOODS2")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.DESCOFGOODS3)
                    .HasColumnName(@"DESC_OF_GOODS3")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.CARRIERID)
                    .HasColumnName(@"CARRIER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.UNLOADPORTID)
                    .HasColumnName(@"UNLOADPORT_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.UNLOADDATE)
                    .HasColumnName(@"UNLOAD_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.PACKTYPEID)
                    .HasColumnName(@"PACKTYPE_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.PARTIALITYID)
                    .HasColumnName(@"PARTIALITY_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.MISHGORTYPE)
                    .HasColumnName(@"MISHGOR_TYPE")
                    .HasMaxLength(6)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.SEALQTY)
                    .HasColumnName(@"SEAL_QTY")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.PACKDET)
                    .HasColumnName(@"PACK_DET")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.ADDQUANTITY)
                    .HasColumnName(@"ADD_QUANTITY")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.WAREHOUSEIDN)
                    .HasColumnName(@"WAREHOUSE_ID_N")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.WAREHOUSERECN)
                    .HasColumnName(@"WAREHOUSE_REC_N")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.EXPORTLANDN)
                    .HasColumnName(@"EXPORT_LAND_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.PACKTYPEIDN)
                    .HasColumnName(@"PACKTYPE_ID_N")
                    .HasMaxLength(4)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.HAWBN)
                    .HasColumnName(@"HAWB_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.IDENTIFIERTYPEN)
                    .HasColumnName(@"IDENTIFIER_TYPE_N")
                    .HasMaxLength(4)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.FIRSTCARGOID)
                    .HasColumnName(@"FIRST_CARGO_ID")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.SECONDCARGOID)
                    .HasColumnName(@"SECOND_CARGO_ID")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");

            #endregion

            #region CCUPAYHAND

            modelBuilder.Entity<CCUPAYHAND>()
                .HasKey(p => new { p.FILENO })
                .ToTable("CCUPAYHAND", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.DRAFTSTATUS)
                    .HasColumnName(@"DRAFT_STATUS")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.PAYTAX)
                    .HasColumnName(@"PAY_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.REJECTTAX)
                    .HasColumnName(@"REJECT_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.HANDTYPE)
                    .HasColumnName(@"HAND_TYPE")
                    .HasColumnType("bool");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.PROCESSWANT)
                    .HasColumnName(@"PROCESS_WANT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TOTALPAYTAX)
                    .HasColumnName(@"TOTAL_PAY_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TOTALPAYDEPOSIT)
                    .HasColumnName(@"TOTAL_PAY_DEPOSIT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.HANDDATE)
                    .HasColumnName(@"HAND_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.RESHIMONSIGNTYPE)
                    .HasColumnName(@"RESHIMON_SIGN_TYPE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.RESHIMONSIGN)
                    .HasColumnName(@"RESHIMON_SIGN")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.REQUESTCODE)
                    .HasColumnName(@"REQUEST_CODE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.AGENTEXPLAIN)
                    .HasColumnName(@"AGENT_EXPLAIN")
                    .HasMaxLength(75)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.OBJECTIONEXPLAIN)
                    .HasColumnName(@"OBJECTION_EXPLAIN")
                    .HasMaxLength(75)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.DATE7)
                    .HasColumnName(@"DATE_7")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TIME7)
                    .HasColumnName(@"TIME_7")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.IMPORTERNAME)
                    .HasColumnName(@"IMPORTER_NAME")
                    .HasMaxLength(55)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TRANSIMPORTERNAME)
                    .HasColumnName(@"TRANS_IMPORTER_NAME")
                    .HasMaxLength(55)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.BONDEDNAME)
                    .HasColumnName(@"BONDED_NAME")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.ENTRYID)
                    .HasColumnName(@"ENTRY_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.SIGNERID)
                    .HasColumnName(@"SIGNER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region CCUSUPITEM

            modelBuilder.Entity<CCUSUPITEM>()
                .HasKey(p => new { p.ACCLINENO, p.FILENO, p.LINENO })
                .ToTable("CCUSUPITEMS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ACCLINENO)
                    .HasColumnName(@"ACC_LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.TARIFFCODE)
                    .HasColumnName(@"TARIFF_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ESSENTIALITEM)
                    .HasColumnName(@"ESSENTIAL_ITEM")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.BITHATAXITEM)
                    .HasColumnName(@"BITHA_TAX_ITEM")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ORIGINCOUNTRY)
                    .HasColumnName(@"ORIGIN_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PURCHCOUNTRY)
                    .HasColumnName(@"PURCH_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.FOREIGNCURRVAL)
                    .HasColumnName(@"FOREIGN_CURR_VAL")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.RAISEPERCENT)
                    .HasColumnName(@"RAISE_PERCENT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.RAISEVALUE)
                    .HasColumnName(@"RAISE_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NISVALUE)
                    .HasColumnName(@"NIS_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.EXTRAQNTY)
                    .HasColumnName(@"EXTRA_QNTY")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.WHOLESALEPRICE)
                    .HasColumnName(@"WHOLESALE_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.IMPORTADDITION)
                    .HasColumnName(@"IMPORT_ADDITION")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.DISCOUNTCODE)
                    .HasColumnName(@"DISCOUNT_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.LICENSENO)
                    .HasColumnName(@"LICENSE_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.STANDARDNO)
                    .HasColumnName(@"STANDARD_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AGNTPAYCUST)
                    .HasColumnName(@"AGNT_PAY_CUST")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AGNTPAYTAX)
                    .HasColumnName(@"AGNT_PAY_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AGNTPAYBITHA)
                    .HasColumnName(@"AGNT_PAY_BITHA")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NIDHEMEHESPCNT)
                    .HasColumnName(@"NIDHE_MEHES_PCNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NIDHEMASPCNT)
                    .HasColumnName(@"NIDHE_MAS_PCNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.VEHICLECODE)
                    .HasColumnName(@"VEHICLE_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ABSAMOUNT)
                    .HasColumnName(@"ABS_AMOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AIRBAGSAMOUNT)
                    .HasColumnName(@"AIRBAGS_AMOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ACAMOUNT)
                    .HasColumnName(@"AC_AMOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.EXPRESHIMONNO)
                    .HasColumnName(@"EXP_RESHIMON_NO")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.EXPPRAT)
                    .HasColumnName(@"EXP_PRAT")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.GUARANTEENO)
                    .HasColumnName(@"GUARANTEE_NO")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.GUARANPERCENT)
                    .HasColumnName(@"GUARAN_PERCENT")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.GUARANTEETYPE)
                    .HasColumnName(@"GUARANTEE_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.EXEMPTIONCODE)
                    .HasColumnName(@"EXEMPTION_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.GOODSDESC)
                    .HasColumnName(@"GOODS_DESC")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.CURRENCYCODE)
                    .HasColumnName(@"CURRENCY_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PRATMEHESCAN)
                    .HasColumnName(@"PRAT_MEHES_CAN")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AUTONOMYBOOK)
                    .HasColumnName(@"AUTONOMY_BOOK")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PRIVATEIMPCURR)
                    .HasColumnName(@"PRIVATE_IMP_CURR")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.TSVIRA)
                    .HasColumnType("bool");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.KATALOGNO)
                    .HasColumnName(@"KATALOG_NO")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ITEMNO)
                    .HasColumnName(@"ITEM_NO")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ITEMLINENO)
                    .HasColumnName(@"ITEM_LINE_NO")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ORIGINVALUE)
                    .HasColumnName(@"ORIGIN_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.UNITID)
                    .HasColumnName(@"UNIT_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.STSQNTY)
                    .HasColumnName(@"STS_QNTY")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PRATMEHESN)
                    .HasColumnName(@"PRAT_MEHES_N")
                    .HasMaxLength(12)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ORIGINCOUNTRYN)
                    .HasColumnName(@"ORIGIN_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PURCHCOUNTRYN)
                    .HasColumnName(@"PURCH_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");

            #endregion

            #region GDFDATA

            modelBuilder.Entity<GDFDATA>()
                .HasKey(p => new { p.BRANCHID, p.CARDID, p.DEFID, p.DISTRID })
                .ToTable("GDFDATA", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDFDATA>()
                .Property(p => p.DISTRID)
                    .HasColumnName(@"DISTR_ID")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GDFDATA>()
                .Property(p => p.DEFID)
                    .HasColumnName(@"DEF_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDFDATA>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GDFDATA>()
                .Property(p => p.CARDID)
                    .HasColumnName(@"CARD_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDFDATA>()
                .Property(p => p.SHORTDEFDATA)
                    .HasColumnName(@"SHORT_DEF_DATA")
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDFDATA>()
                .Property(p => p.DEFDATA)
                    .HasColumnName(@"DEF_DATA")
                    .HasColumnType("long");

            #endregion

            #region CCUCRREQ

            modelBuilder.Entity<CCUCRREQ>()
                .HasKey(p => new { p.ACCLINENO, p.ENTNAME, p.FILENO, p.ITEMLINE, p.LINENO })
                .ToTable("CCUCRREQ", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.ENTNAME)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.ACCLINENO)
                    .HasColumnName(@"ACC_LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.ITEMLINE)
                    .HasColumnName(@"ITEM_LINE")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.REQCERTID)
                    .HasColumnName(@"REQ_CERT_ID")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.GCRCRTFID)
                    .HasColumnName(@"GCRCRTF_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.GCRCRTFCLOSE)
                    .HasColumnName(@"GCRCRTF_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.CERTIFICATENO)
                    .HasColumnName(@"CERTIFICATE_NO")
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.APPROVCODE)
                    .HasColumnName(@"APPROV_CODE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.APPROVTYPE)
                    .HasColumnName(@"APPROV_TYPE")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.SINUMBER)
                    .HasColumnName(@"SI_NUMBER")
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.SUPPLIERID)
                    .HasColumnName(@"SUPPLIER_ID")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.SUPPLIERCOUNTRY)
                    .HasColumnName(@"SUPPLIER_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.ITEMNO)
                    .HasColumnName(@"ITEM_NO")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.FENTNAME)
                    .HasColumnName(@"F_ENTNAME")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.REMARKS)
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.ADDREQUESTNO)
                    .HasColumnName(@"ADD_REQUEST_NO")
                    .HasMaxLength(10)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.REQUESTNO)
                    .HasColumnName(@"REQUEST_NO")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            #endregion

            #region GTRTRAN

            modelBuilder.Entity<GTRTRAN>()
                .HasKey(p => new { p.LOCALCODE, p.PARTNERCODE, p.PARTNERID, p.TABLEID })
                .ToTable("GTRTRAN", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTRTRAN>()
                .Property(p => p.PARTNERID)
                    .HasColumnName(@"PARTNER_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTRTRAN>()
                .Property(p => p.TABLEID)
                    .HasColumnName(@"TABLE_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTRTRAN>()
                .Property(p => p.PARTNERCODE)
                    .HasColumnName(@"PARTNER_CODE")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTRTRAN>()
                .Property(p => p.LOCALCODE)
                    .HasColumnName(@"LOCAL_CODE")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTRTRAN>()
                .Property(p => p.ISPRIMARY)
                    .HasColumnName(@"IS_PRIMARY")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GRTRATE

            modelBuilder.Entity<GRTRATE>()
                .HasKey(p => new { p.COINID, p.CURRTABLECODE, p.RATEDATE, p.RATEID })
                .ToTable("GRTRATE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GRTRATE>()
                .Property(p => p.COINID)
                    .HasColumnName(@"COIN_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GRTRATE>()
                .Property(p => p.RATEDATE)
                    .HasColumnName(@"RATE_DATE")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("date");
            modelBuilder.Entity<GRTRATE>()
                .Property(p => p.RATEID)
                    .HasColumnName(@"RATE_ID")
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GRTRATE>()
                .Property(p => p.RATE)
                    .HasColumnType("double");
            modelBuilder.Entity<GRTRATE>()
                .Property(p => p.CURRTABLECODE)
                    .HasColumnName(@"CURR_TABLE_CODE")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");

            #endregion

            #region CCUCUSTITEM

            modelBuilder.Entity<CCUCUSTITEM>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUCUSTITEMS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.SUPPLIERACCOUNT)
                    .HasColumnName(@"SUPPLIER_ACCOUNT")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.TARIFFCODE)
                    .HasColumnName(@"TARIFF_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ESSENTIALITEM)
                    .HasColumnName(@"ESSENTIAL_ITEM")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.BITHATAXITEM)
                    .HasColumnName(@"BITHA_TAX_ITEM")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ORIGINCOUNTRY)
                    .HasColumnName(@"ORIGIN_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PURCHCOUNTRY)
                    .HasColumnName(@"PURCH_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.FOREIGNCURRVAL)
                    .HasColumnName(@"FOREIGN_CURR_VAL")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.RAISEPERCENT)
                    .HasColumnName(@"RAISE_PERCENT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.RAISEVALUE)
                    .HasColumnName(@"RAISE_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NISVALUE)
                    .HasColumnName(@"NIS_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.EXTRAQNTY)
                    .HasColumnName(@"EXTRA_QNTY")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.WHOLESALEPRICE)
                    .HasColumnName(@"WHOLESALE_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.IMPORTADDITION)
                    .HasColumnName(@"IMPORT_ADDITION")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.DISCOUNTCODE)
                    .HasColumnName(@"DISCOUNT_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.LICENSENO)
                    .HasColumnName(@"LICENSE_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.STANDARDNO)
                    .HasColumnName(@"STANDARD_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AGNTPAYCUST)
                    .HasColumnName(@"AGNT_PAY_CUST")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AGNTPAYTAX)
                    .HasColumnName(@"AGNT_PAY_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AGNTPAYBITHA)
                    .HasColumnName(@"AGNT_PAY_BITHA")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NIDHEMEHESPCNT)
                    .HasColumnName(@"NIDHE_MEHES_PCNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NIDHEMASPCNT)
                    .HasColumnName(@"NIDHE_MAS_PCNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.VEHICLECODE)
                    .HasColumnName(@"VEHICLE_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ABSAMOUNT)
                    .HasColumnName(@"ABS_AMOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AIRBAGSAMOUNT)
                    .HasColumnName(@"AIRBAGS_AMOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ACAMOUNT)
                    .HasColumnName(@"AC_AMOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.EXPRESHIMONNO)
                    .HasColumnName(@"EXP_RESHIMON_NO")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.EXPPRAT)
                    .HasColumnName(@"EXP_PRAT")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.GUARANTEENO)
                    .HasColumnName(@"GUARANTEE_NO")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.GUARANPERCENT)
                    .HasColumnName(@"GUARAN_PERCENT")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.GUARANTEETYPE)
                    .HasColumnName(@"GUARANTEE_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.EXEMPTIONCODE)
                    .HasColumnName(@"EXEMPTION_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.GOODSDESC)
                    .HasColumnName(@"GOODS_DESC")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.CURRENCYCODE)
                    .HasColumnName(@"CURRENCY_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PRATMEHESCAN)
                    .HasColumnName(@"PRAT_MEHES_CAN")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AUTONOMYBOOK)
                    .HasColumnName(@"AUTONOMY_BOOK")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PRIVATEIMPCURR)
                    .HasColumnName(@"PRIVATE_IMP_CURR")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.TSVIRA)
                    .HasColumnType("bool");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.KATALOGNO)
                    .HasColumnName(@"KATALOG_NO")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ORDERLINE)
                    .HasColumnName(@"ORDER_LINE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.UNITID)
                    .HasColumnName(@"UNIT_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.STSQNTY)
                    .HasColumnName(@"STS_QNTY")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PRATMEHESN)
                    .HasColumnName(@"PRAT_MEHES_N")
                    .HasMaxLength(12)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ORIGINCOUNTRYN)
                    .HasColumnName(@"ORIGIN_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PURCHCOUNTRYN)
                    .HasColumnName(@"PURCH_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");

            #endregion

            ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
            #region CCUQUELOCK
            ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
            modelBuilder.Entity<CCUQUELOCK>()
                .HasKey(p => new
                {
                    p.ENTNAME,
                    ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                    ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                    p.FILE_NO
                    ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                    ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                })////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                .ToTable("CCUQUELOCK", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUQUELOCK>()
                .Property(p => p.ENTNAME)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUQUELOCK>()
                .Property(p =>
                ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                p.FILE_NO
                ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                ////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                )
                    .HasColumnName(@"FILE_NO")////**** ITZIK :YUVAL PLS SET THE PROPERTY FROM FILENO TO FILE_NO !!!!
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");

            #endregion

            #region CCUPAYLINEF

            modelBuilder.Entity<CCUPAYLINEF>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUPAYLINEF", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYORDNO)
                    .HasColumnName(@"PAY_ORD_NO")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYMETHOD)
                    .HasColumnName(@"PAY_METHOD")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.TYPE)
                    .HasColumnType("int16");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYEETYPE)
                    .HasColumnName(@"PAYEE_TYPE")
                    .HasColumnType("int16");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYAMOUNT)
                    .HasColumnName(@"PAY_AMOUNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.ACCOUNTNAME)
                    .HasColumnName(@"ACCOUNT_NAME")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.BANKID)
                    .HasColumnName(@"BANK_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.BANKBRANCH)
                    .HasColumnName(@"BANK_BRANCH")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.BANKACCOUNT)
                    .HasColumnName(@"BANK_ACCOUNT")
                    .HasMaxLength(11)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYREF)
                    .HasColumnName(@"PAY_REF")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYDATE)
                    .HasColumnName(@"PAY_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.TREATFILE)
                    .HasColumnName(@"TREAT_FILE")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.VATBANK)
                    .HasColumnName(@"VAT_BANK")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.HASHAVUTCODE)
                    .HasColumnName(@"HASHAVUT_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");

            #endregion

            #region CCUTAX

            modelBuilder.Entity<CCUTAX>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUTAX", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.GOODSNO)
                    .HasColumnName(@"GOODS_NO")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXTYPE)
                    .HasColumnName(@"TAX_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXBASIS)
                    .HasColumnName(@"TAX_BASIS")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXAMOUNT)
                    .HasColumnName(@"TAX_AMOUNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.POSTPONEDTAX)
                    .HasColumnName(@"POSTPONED_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXTOPAY)
                    .HasColumnName(@"TAX_TO_PAY")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXCALCCODE)
                    .HasColumnName(@"TAX_CALC_CODE")
                    .HasColumnType("int16");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXRATE)
                    .HasColumnName(@"TAX_RATE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.DEFINEDTAX)
                    .HasColumnName(@"DEFINED_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDTAXRATE)
                    .HasColumnName(@"ADD_TAX_RATE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDEFINEDTAX)
                    .HasColumnName(@"AD_DEFINED_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDIMPORT)
                    .HasColumnName(@"ADD_IMPORT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.PRATMEHESN)
                    .HasColumnName(@"PRAT_MEHES_N")
                    .HasMaxLength(12)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXTYPEN)
                    .HasColumnName(@"TAX_TYPE_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");

            #endregion

            #region CCUFILEM

            modelBuilder.Entity<CCUFILEM>()
                .HasKey(p => new { p.FILENO })
                .ToTable("CCUFILEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMERID)
                    .HasColumnName(@"CUSTOMER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMFILENO)
                    .HasColumnName(@"CUSTOM_FILE_NO")
                    .HasColumnType("int64");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.DRAWNO)
                    .HasColumnName(@"DRAW_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RESHIMONTYPE)
                    .HasColumnName(@"RESHIMON_TYPE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RESHIMONNO)
                    .HasColumnName(@"RESHIMON_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.DRAFTDATE)
                    .HasColumnName(@"DRAFT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.AUTONOMY)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.STORAGEREQUEST)
                    .HasColumnName(@"STORAGE_REQUEST")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMSBRANCH)
                    .HasColumnName(@"CUSTOMS_BRANCH")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMAGENT)
                    .HasColumnName(@"CUSTOM_AGENT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.IMPORTERID)
                    .HasColumnName(@"IMPORTER_ID")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FOLUPDATE)
                    .HasColumnName(@"FOL_UP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FILECLOSE)
                    .HasColumnName(@"FILE_CLOSE")
                    .HasColumnType("int16");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.OPENBYUSER)
                    .HasColumnName(@"OPEN_BY_USER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CHANGE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANSIMPORTID)
                    .HasColumnName(@"TRANS_IMPORT_ID")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RIGHTOWNID)
                    .HasColumnName(@"RIGHT_OWN_ID")
                    .HasColumnType("int16");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RESHMDATE)
                    .HasColumnName(@"RESHM_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.WARNINGDATE)
                    .HasColumnName(@"WARNING_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.SELLCONDITIONID)
                    .HasColumnName(@"SELL_CONDITION_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INDEXVALUE)
                    .HasColumnName(@"INDEX_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.COINID)
                    .HasColumnName(@"COIN_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CHANGINGVALUE)
                    .HasColumnName(@"CHANGING_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.REGIONVALUE)
                    .HasColumnName(@"REGION_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANSPVALUE)
                    .HasColumnName(@"TRANSP_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEVALUE)
                    .HasColumnName(@"INSURANCE_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.SERVICEVALUE)
                    .HasColumnName(@"SERVICE_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.EXPENSEVALUE)
                    .HasColumnName(@"EXPENSE_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CLOSUREVALUE)
                    .HasColumnName(@"CLOSURE_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FEECARRIER)
                    .HasColumnName(@"FEE_CARRIER")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FEEPLATFORM)
                    .HasColumnName(@"FEE_PLATFORM")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CURRENCYRATE)
                    .HasColumnName(@"CURRENCY_RATE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PRICEINDEX)
                    .HasColumnName(@"PRICE_INDEX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GOODSVALUE)
                    .HasColumnName(@"GOODS_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CIFVALUE)
                    .HasColumnName(@"CIF_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.ACCEPTEDPRICE)
                    .HasColumnName(@"ACCEPTED_PRICE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TOTALTAX)
                    .HasColumnName(@"TOTAL_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANSPVALFC)
                    .HasColumnName(@"TRANSP_VAL_FC")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANCURRENCY)
                    .HasColumnName(@"TRAN_CURRENCY")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEPERCENT)
                    .HasColumnName(@"INSURANCE_PERCENT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCECURR)
                    .HasColumnName(@"INSURANCE_CURR")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEAMNT)
                    .HasColumnName(@"INSURANCE_AMNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PAYDATE)
                    .HasColumnName(@"PAY_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PAYTIME)
                    .HasColumnName(@"PAY_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.MEHESDRAFTSTATUS)
                    .HasColumnName(@"MEHES_DRAFT_STATUS")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GRANTDATE)
                    .HasColumnName(@"GRANT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GRANTTIME)
                    .HasColumnName(@"GRANT_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INDICATORS)
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMPRINT)
                    .HasColumnName(@"CUSTOM_PRINT")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TSHUMOTTAXPRINT)
                    .HasColumnName(@"TSHUMOT_TAX_PRINT")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CHARGESYSPRINT)
                    .HasColumnName(@"CHARGE_SYS_PRINT")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMAGENTPRINT)
                    .HasColumnName(@"CUSTOM_AGENT_PRINT")
                    .HasColumnType("date");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GRNTTYPEID)
                    .HasColumnName(@"GRNT_TYPE_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GUARANTEEAMNT)
                    .HasColumnName(@"GUARANTEE_AMNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.IMPORTTYPE)
                    .HasColumnName(@"IMPORT_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PASSPORTNO)
                    .HasColumnName(@"PASSPORT_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PASSPCTRY)
                    .HasColumnName(@"PASSP_CTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.IMPORTERSTS)
                    .HasColumnName(@"IMPORTER_STS")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FAMILYNAME)
                    .HasColumnName(@"FAMILY_NAME")
                    .HasMaxLength(18)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FIRSTNAME)
                    .HasColumnName(@"FIRST_NAME")
                    .HasMaxLength(12)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CITYSYMBOL)
                    .HasColumnName(@"CITY_SYMBOL")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.STREET)
                    .HasMaxLength(17)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.HOUSE)
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.ENTRANCE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.APARTMENTNO)
                    .HasColumnName(@"APARTMENT_NO")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.ZIPCODE)
                    .HasColumnName(@"ZIP_CODE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FROMIIG)
                    .HasColumnName(@"FROM_IIG")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RESHIMONTYPEN)
                    .HasColumnName(@"RESHIMON_TYPE_N")
                    .HasMaxLength(7)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RESHIMONNON)
                    .HasColumnName(@"RESHIMON_NO_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.COINIDN)
                    .HasColumnName(@"COIN_ID_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANCURRENCYN)
                    .HasColumnName(@"TRAN_CURRENCY_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCECURRN)
                    .HasColumnName(@"INSURANCE_CURR_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CANCELLED)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.LOANAMOUNT)
                    .HasColumnName(@"LOAN_AMOUNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CURRENCYRATENEW)
                    .HasColumnName(@"CURRENCY_RATE_NEW")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.DRAWNON)
                    .HasColumnName(@"DRAW_NO_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PRATMEHESLIST)
                    .HasColumnName(@"PRAT_MEHES_LIST")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.ALLPRATMEHESLIST)
                    .HasColumnName(@"ALL_PRAT_MEHES_LIST")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.NOOFINVOICES)
                    .HasColumnName(@"NO_OF_INVOICES")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TOTALINVOICELINESNO)
                    .HasColumnName(@"TOTAL_INVOICE_LINES_NO")
                    .HasColumnType("int");








            #endregion

            #region GTBMANDT

            modelBuilder.Entity<GTBMANDT>()
                .HasKey(p => new { p.CLIENTCODE, p.ENTITY, p.FIELDNAME, p.FORMNAME })
                .ToTable("GTBMANDT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBMANDT>()
                .Property(p => p.CLIENTCODE)
                    .HasColumnName(@"CLIENT_CODE")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBMANDT>()
                .Property(p => p.FORMNAME)
                    .HasColumnName(@"FORM_NAME")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBMANDT>()
                .Property(p => p.ENTITY)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBMANDT>()
                .Property(p => p.FIELDNAME)
                    .HasColumnName(@"FIELD_NAME")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBMANDT>()
                .Property(p => p.ATTR)
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBMANDT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GNDCARD

            modelBuilder.Entity<GNDCARD>()
                .HasKey(p => new { p.CARDID })
                .ToTable("GNDCARD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.CARDID)
                    .HasColumnName(@"CARD_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.SHNAMEHEB)
                    .HasColumnName(@"SHNAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.GROUPID)
                    .HasColumnName(@"GROUP_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.SHNAMEENG)
                    .HasColumnName(@"SHNAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.OLDCARD)
                    .HasColumnName(@"OLD_CARD")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLI)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLM)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLR)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLH)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLT)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLQ)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLC)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLS)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLF)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLJ)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLK)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLN)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.PROFID)
                    .HasColumnName(@"PROF_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.COMPANYID)
                    .HasColumnName(@"COMPANY_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.OLDCARDAUTOCREATED)
                    .HasColumnName(@"OLD_CARD_AUTO_CREATED")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.APPLL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.BLOCKREASON)
                    .HasColumnName(@"BLOCK_REASON")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.EXPORTCOMPANYID)
                    .HasColumnName(@"EXPORT_COMPANY_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDCARD>()
                .Property(p => p.REMARKS)
                    .HasColumnType("clob");

            #endregion

            #region CTBPACKTYPE

            modelBuilder.Entity<CTBPACKTYPE>()
                .HasKey(p => new { p.PACKTYPEID })
                .ToTable("CTBPACKTYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBPACKTYPE>()
                .Property(p => p.PACKTYPEID)
                    .HasColumnName(@"PACK_TYPE_ID")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBPACKTYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBPACKTYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBPACKTYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBPACKTYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBPACKTYPE>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GAQFILEDATA

            modelBuilder.Entity<GAQFILEDATA>()
                .HasKey(p => new { p.APPQID, p.ENTNAME, p.FIELDID, p.PATH, p.PRIMARYNUM })
                .ToTable("GAQFILEDATA", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQFILEDATA>()
                .Property(p => p.ENTNAME)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQFILEDATA>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQFILEDATA>()
                .Property(p => p.PATH)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQFILEDATA>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQFILEDATA>()
                .Property(p => p.FIELDID)
                    .HasColumnName(@"FIELD_ID")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQFILEDATA>()
                .Property(p => p.FIELDVALUE)
                    .HasColumnName(@"FIELD_VALUE")
                    .HasMaxLength(1000)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBBONDED

            modelBuilder.Entity<CTBBONDED>()
                .HasKey(p => new { p.WAREHOUSEID })
                .ToTable("CTBBONDED", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.WAREHOUSEID)
                    .HasColumnName(@"WAREHOUSE_ID")
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.STORAGESITE)
                    .HasColumnName(@"STORAGE_SITE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.CUSTOMSBRANCH)
                    .HasColumnName(@"CUSTOMS_BRANCH")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBBONDED>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBCOUNTRY

            modelBuilder.Entity<CTBCOUNTRY>()
                .HasKey(p => new { p.COUNTRYID })
                .ToTable("CTBCOUNTRY", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.COUNTRYID)
                    .HasColumnName(@"COUNTRY_ID")
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.COUNTRYCODE)
                    .HasColumnName(@"COUNTRY_CODE")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.TARIFFID)
                    .HasColumnName(@"TARIFF_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCOUNTRY>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBIDNTP

            modelBuilder.Entity<CTBIDNTP>()
                .HasKey(p => new { p.IDENTIFITYPE })
                .ToTable("CTBIDNTP", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBIDNTP>()
                .Property(p => p.IDENTIFITYPE)
                    .HasColumnName(@"IDENTIFI_TYPE")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBIDNTP>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBIDNTP>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBIDNTP>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBIDNTP>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBIDNTP>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBIMPORT

            modelBuilder.Entity<CTBIMPORT>()
                .HasKey(p => new { p.IMPORTERID })
                .ToTable("CTBIMPORT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBIMPORT>()
                .Property(p => p.IMPORTERID)
                    .HasColumnName(@"IMPORTER_ID")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBIMPORT>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(55)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBIMPORT>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(55)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBIMPORT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBIMPORT>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(55)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBIMPORT>()
                .Property(p => p.ADDRESS)
                    .HasMaxLength(100)
                    .HasColumnType("char");

            #endregion

            #region CTBMISHGUR

            modelBuilder.Entity<CTBMISHGUR>()
                .HasKey(p => new { p.SUGMISHGUR })
                .ToTable("CTBMISHGUR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBMISHGUR>()
                .Property(p => p.SUGMISHGUR)
                    .HasColumnName(@"SUG_MISHGUR")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBMISHGUR>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBMISHGUR>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBMISHGUR>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBMISHGUR>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBRESHTYPE

            modelBuilder.Entity<CTBRESHTYPE>()
                .HasKey(p => new { p.RESHIMONTYPE })
                .ToTable("CTBRESHTYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBRESHTYPE>()
                .Property(p => p.RESHIMONTYPE)
                    .HasColumnName(@"RESHIMON_TYPE")
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBRESHTYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBRESHTYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBRESHTYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBRESHTYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBRESHTYPE>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBRGOWN

            modelBuilder.Entity<CTBRGOWN>()
                .HasKey(p => new { p.RIGHTID })
                .ToTable("CTBRGOWN", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBRGOWN>()
                .Property(p => p.RIGHTID)
                    .HasColumnName(@"RIGHT_ID")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int16");
            modelBuilder.Entity<CTBRGOWN>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBRGOWN>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBRGOWN>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBRGOWN>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBSTORAGE

            modelBuilder.Entity<CTBSTORAGE>()
                .HasKey(p => new { p.STORAGESITE })
                .ToTable("CTBSTORAGE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBSTORAGE>()
                .Property(p => p.STORAGESITE)
                    .HasColumnName(@"STORAGE_SITE")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBSTORAGE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBSTORAGE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBSTORAGE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBSTORAGE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBTRANSP

            modelBuilder.Entity<CTBTRANSP>()
                .HasKey(p => new { p.TRANSPTYPE })
                .ToTable("CTBTRANSP", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBTRANSP>()
                .Property(p => p.TRANSPTYPE)
                    .HasColumnName(@"TRANSP_TYPE")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTRANSP>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTRANSP>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTRANSP>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTRANSP>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBUNLOAD

            modelBuilder.Entity<CTBUNLOAD>()
                .HasKey(p => new { p.ULPORTID })
                .ToTable("CTBUNLOAD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBUNLOAD>()
                .Property(p => p.ULPORTID)
                    .HasColumnName(@"UL_PORT_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBUNLOAD>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBUNLOAD>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBUNLOAD>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBUNLOAD>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBUNLOAD>()
                .Property(p => p.IULPORTID)
                    .HasColumnName(@"I_UL_PORT_ID")
                    .HasMaxLength(10)
                    .HasColumnType("char");

            #endregion

            #region ATBPTIL

            modelBuilder.Entity<ATBPTIL>()
                .HasKey(p => new { p.BRANID })
                .ToTable("ATBPTIL", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.BRANID)
                    .HasColumnName(@"BRAN_ID")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(40)
                    .HasColumnType("char");
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(40)
                    .HasColumnType("char");
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.MODEOFTRANSP)
                    .HasColumnName(@"MODE_OF_TRANSP")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.UNLOADPORTID)
                    .HasColumnName(@"UNLOADPORT_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ATBPTIL>()
                .Property(p => p.PRINTERID)
                    .HasColumnName(@"PRINTER_ID")
                    .HasMaxLength(200)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBLOAD

            modelBuilder.Entity<CTBLOAD>()
                .HasKey(p => new { p.LPORTID })
                .ToTable("CTBLOAD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.LPORTID)
                    .HasColumnName(@"L_PORT_ID")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.LOCALCODEA)
                    .HasColumnName(@"LOCAL_CODE_A")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.LOCALCODEO)
                    .HasColumnName(@"LOCAL_CODE_O")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBLOAD>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GNDADR

            modelBuilder.Entity<GNDADR>()
                .HasKey(p => new { p.CARDID, p.LINE })
                .ToTable("GNDADR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.CARDID)
                    .HasColumnName(@"CARD_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.LINE)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.ADDRESSHEB)
                    .HasColumnName(@"ADDRESS_HEB")
                    .HasMaxLength(35)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.ADDRESSENG)
                    .HasColumnName(@"ADDRESS_ENG")
                    .HasMaxLength(35)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.ADDRESSENG2)
                    .HasColumnName(@"ADDRESS_ENG_2")
                    .HasMaxLength(35)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.ZIPCODE)
                    .HasColumnName(@"ZIP_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.POB)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.ZIPPOBCODE)
                    .HasColumnName(@"ZIP_POB_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.CITYHEB)
                    .HasColumnName(@"CITY_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.CITYENG)
                    .HasColumnName(@"CITY_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.TELEPHONE)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.FAX)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.LIAISONHEB)
                    .HasColumnName(@"LIAISON_HEB")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.LIAISONENG)
                    .HasColumnName(@"LIAISON_ENG")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.COUNTRYID)
                    .HasColumnName(@"COUNTRY_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.STATE)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.FAXMAILPRIO)
                    .HasColumnName(@"FAX_MAIL_PRIO")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.NAME)
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.EMAIL)
                    .HasColumnName(@"E_MAIL")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.SEARCHEMAIL)
                    .HasColumnName(@"SEARCH_E_MAIL")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.STATECODE)
                    .HasColumnName(@"STATE_CODE")
                    .HasMaxLength(9)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.CITYCODE)
                    .HasColumnName(@"CITY_CODE")
                    .HasMaxLength(7)
                    .HasColumnType("char");
            modelBuilder.Entity<GNDADR>()
                .Property(p => p.ADDRESS3)
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBPART

            modelBuilder.Entity<CTBPART>()
                .HasKey(p => new { p.PARTIALITYID })
                .ToTable("CTBPART", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBPART>()
                .Property(p => p.PARTIALITYID)
                    .HasColumnName(@"PARTIALITY_ID")
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBPART>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBPART>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBPART>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBPART>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBPKDT

            modelBuilder.Entity<CTBPKDT>()
                .HasKey(p => new { p.PACKDETAIL })
                .ToTable("CTBPKDT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBPKDT>()
                .Property(p => p.PACKDETAIL)
                    .HasColumnName(@"PACK_DETAIL")
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBPKDT>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBPKDT>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBPKDT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBPKDT>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBAPPROV

            modelBuilder.Entity<CTBAPPROV>()
                .HasKey(p => new { p.APPROVCODEID })
                .ToTable("CTBAPPROV", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBAPPROV>()
                .Property(p => p.APPROVCODEID)
                    .HasColumnName(@"APPROV_CODE_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBAPPROV>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBAPPROV>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBAPPROV>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBAPPROV>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBAPPROVTYPE

            modelBuilder.Entity<CTBAPPROVTYPE>()
                .HasKey(p => new { p.APPROVTYPEID })
                .ToTable("CTBAPPROVTYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBAPPROVTYPE>()
                .Property(p => p.APPROVTYPEID)
                    .HasColumnName(@"APPROV_TYPE_ID")
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBAPPROVTYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBAPPROVTYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBAPPROVTYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBAPPROVTYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBAPPROVTYPE>()
                .Property(p => p.APPROVCODE)
                    .HasColumnName(@"APPROV_CODE")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBAPPROVTYPE>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GTBREQCERT

            modelBuilder.Entity<GTBREQCERT>()
                .HasKey(p => new { p.ENTITY, p.REQCERT })
                .ToTable("GTBREQCERT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBREQCERT>()
                .Property(p => p.REQCERT)
                    .HasColumnName(@"REQ_CERT")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBREQCERT>()
                .Property(p => p.ENTITY)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBREQCERT>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBREQCERT>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBREQCERT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBREQCERT>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");

            #endregion

            #region CCUTRANSPVAL

            modelBuilder.Entity<CCUTRANSPVAL>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUTRANSPVAL", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.TRANSPVALFC)
                    .HasColumnName(@"TRANSP_VAL_FC")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.CURRID)
                    .HasColumnName(@"CURR_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.TRANSPVAL)
                    .HasColumnName(@"TRANSP_VAL")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.CURRIDN)
                    .HasColumnName(@"CURR_ID_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBCURRENCY

            modelBuilder.Entity<CTBCURRENCY>()
                .HasKey(p => new { p.CURRENCYID })
                .ToTable("CTBCURRENCY", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBCURRENCY>()
                .Property(p => p.CURRENCYID)
                    .HasColumnName(@"CURRENCY_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCURRENCY>()
                .Property(p => p.LOCALCODE)
                    .HasColumnName(@"LOCAL_CODE")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCURRENCY>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCURRENCY>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCURRENCY>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCURRENCY>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCURRENCY>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBINCOTERM

            modelBuilder.Entity<CTBINCOTERM>()
                .HasKey(p => new { p.PTERMID })
                .ToTable("CTBINCOTERMS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.PTERMID)
                    .HasColumnName(@"PTERM_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.WTVAL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.OTHER)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.IMPORTCODE)
                    .HasColumnName(@"IMPORT_CODE")
                    .HasMaxLength(200)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.SERTYPEDEF)
                    .HasColumnName(@"SERTYPE_DEF")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.TEXTFORHANDLNG)
                    .HasColumnName(@"TEXT_FOR_HANDLNG")
                    .HasColumnType("clob");
            modelBuilder.Entity<CTBINCOTERM>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBTARIFF

            modelBuilder.Entity<CTBTARIFF>()
                .HasKey(p => new { p.TARIFFID })
                .ToTable("CTBTARIFF", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBTARIFF>()
                .Property(p => p.TARIFFID)
                    .HasColumnName(@"TARIFF_ID")
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTARIFF>()
                .Property(p => p.HEBCODE)
                    .HasColumnName(@"HEB_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTARIFF>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTARIFF>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTARIFF>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTARIFF>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTARIFF>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CCUMESSAGE

            modelBuilder.Entity<CCUMESSAGE>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUMESSAGE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.GROUPNO)
                    .HasColumnName(@"GROUP_NO")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.GROUPKEY)
                    .HasColumnName(@"GROUP_KEY")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.REFERENCE)
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.MESSAGENO)
                    .HasColumnName(@"MESSAGE_NO")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.APPROVCODEID)
                    .HasColumnName(@"APPROV_CODE_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.APPROVTYPEID)
                    .HasColumnName(@"APPROV_TYPE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.APPROVNO)
                    .HasColumnName(@"APPROV_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.ADDITIONID)
                    .HasColumnName(@"ADDITION_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.GENERAL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.APPROVELEVEL)
                    .HasColumnName(@"APPROVE_LEVEL")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMESSAGE>()
                .Property(p => p.MESSAGETXT)
                    .HasColumnName(@"MESSAGE_TXT")
                    .HasColumnType("clob");

            #endregion

            #region CTBERROR

            modelBuilder.Entity<CTBERROR>()
                .HasKey(p => new { p.ERRORCODE })
                .ToTable("CTBERRORS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBERROR>()
                .Property(p => p.ERRORCODE)
                    .HasColumnName(@"ERROR_CODE")
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBERROR>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBERROR>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(100)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBERROR>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBERROR>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(100)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBERROR>()
                .Property(p => p.EVENTID)
                    .HasColumnName(@"EVENT_ID")
                    .HasMaxLength(7)
                    .HasColumnType("varchar2");

            #endregion

            #region CCUCARL

            modelBuilder.Entity<CCUCARL>()
                .HasKey(p => new { p.COUNTER, p.FILENO, p.LINENO })
                .ToTable("CCUCARL", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.COUNTER)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.RIHBIT)
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.ENGINEVOL)
                    .HasColumnName(@"ENGINE_VOL")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.SHEILDNO)
                    .HasColumnName(@"SHEILD_NO")
                    .HasMaxLength(20)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.MNFDATE)
                    .HasColumnName(@"MNF_DATE")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.A)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.B)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.E)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.MEMIRTYPE)
                    .HasColumnName(@"MEMIR_TYPE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.MADADRATE)
                    .HasColumnName(@"MADAD_RATE")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.FUELTYPE)
                    .HasColumnName(@"FUEL_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.FFU1)
                    .HasMaxLength(50)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.FFU2)
                    .HasMaxLength(50)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.ABSDEDUCT)
                    .HasColumnName(@"ABS_DEDUCT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.KARITDEDUCT)
                    .HasColumnName(@"KARIT_DEDUCT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.BAKARADEDUCT)
                    .HasColumnName(@"BAKARA_DEDUCT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.MEMIRDEDUCT)
                    .HasColumnName(@"MEMIR_DEDUCT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.MADADDEDUCT)
                    .HasColumnName(@"MADAD_DEDUCT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.HYBRID)
                    .HasColumnType("int");

            #endregion

            #region CTBMEMIRTYPE

            modelBuilder.Entity<CTBMEMIRTYPE>()
                .HasKey(p => new { p.MEMIRTYPE })
                .ToTable("CTBMEMIRTYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBMEMIRTYPE>()
                .Property(p => p.MEMIRTYPE)
                    .HasColumnName(@"MEMIR_TYPE")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CTBMEMIRTYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBMEMIRTYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBMEMIRTYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBMEMIRTYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");

            #endregion

            #region CCUTSRUFOT

            modelBuilder.Entity<CCUTSRUFOT>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUTSRUFOT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUTSRUFOT>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTSRUFOT>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTSRUFOT>()
                .Property(p => p.TSRUFAID)
                    .HasColumnName(@"TSRUFA_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTSRUFOT>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTSRUFOT>()
                .Property(p => p.TSRUFANO)
                    .HasColumnName(@"TSRUFA_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");

            #endregion

            #region CTBTSRUFTYPE

            modelBuilder.Entity<CTBTSRUFTYPE>()
                .HasKey(p => new { p.TSRUFAID })
                .ToTable("CTBTSRUFTYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBTSRUFTYPE>()
                .Property(p => p.TSRUFAID)
                    .HasColumnName(@"TSRUFA_ID")
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTSRUFTYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(240)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTSRUFTYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(240)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTSRUFTYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(100)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTSRUFTYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTSRUFTYPE>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region CTBTAXTYPE

            modelBuilder.Entity<CTBTAXTYPE>()
                .HasKey(p => new { p.TAXTYPE })
                .ToTable("CTBTAXTYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBTAXTYPE>()
                .Property(p => p.TAXTYPE)
                    .HasColumnName(@"TAX_TYPE")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTAXTYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTAXTYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .IsRequired()
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTAXTYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTAXTYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");

            #endregion

            #region ITBPCKTY

            modelBuilder.Entity<ITBPCKTY>()
                .HasKey(p => new { p.PACKTYPEID })
                .ToTable("ITBPCKTY", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.PACKTYPEID)
                    .HasColumnName(@"PACKTYPE_ID")
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.SEPARPRC)
                    .HasColumnName(@"SEPAR_PRC")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.CONTSIZE)
                    .HasColumnName(@"CONT_SIZE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.CONTTYPE)
                    .HasColumnName(@"CONT_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.REFRI)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.VENTY)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.TEU)
                    .HasColumnType("double");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.MODEOFTRANSP)
                    .HasColumnName(@"MODE_OF_TRANSP")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.DEFTARA)
                    .HasColumnName(@"DEF_TARA")
                    .HasColumnType("double");
            modelBuilder.Entity<ITBPCKTY>()
                .Property(p => p.DEFVOLUME)
                    .HasColumnName(@"DEF_VOLUME")
                    .HasColumnType("double");

            #endregion

            #region GDMFILING

            modelBuilder.Entity<GDMFILING>()
                .HasKey(p => new { p.COMID })
                .ToTable("GDMFILING", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.FOLDERCODE)
                    .HasColumnName(@"FOLDER_CODE")
                    .HasMaxLength(9)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.LASTVERSION)
                    .HasColumnName(@"LAST_VERSION")
                    .HasColumnType("int");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.EXTENSION)
                    .HasMaxLength(40)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.ORIGNALFILE)
                    .HasColumnName(@"ORIGNAL_FILE")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.DOCID)
                    .HasColumnName(@"DOC_ID")
                    .HasMaxLength(7)
                    .HasColumnType("varchar2");
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.DESC)
                    .HasMaxLength(60)
                    .HasColumnType("varchar2");
#endif
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.SEARCHDESC)
                    .HasColumnName(@"SEARCH_DESC")
                    .HasMaxLength(60)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.REMARKS)
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.SEARCHREMARKS)
                    .HasColumnName(@"SEARCH_REMARKS")
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.ISCLOSE)
                    .HasColumnName(@"IS_CLOSE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.FOLUPDATE)
                    .HasColumnName(@"FOL_UP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.LSTSTATUSID)
                    .HasColumnName(@"LST_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.LSTSTATUSDATE)
                    .HasColumnName(@"LST_STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.UPDATEBY)
                    .HasColumnName(@"UPDATE_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.AUTOYN)
                    .HasColumnName(@"AUTO_YN")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.DELETEAUTO)
                    .HasColumnName(@"DELETE_AUTO")
                    .HasColumnType("int");
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.SOURCE)
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
#endif
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.ORIGIN)
                    .HasMaxLength(1)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.OWNERCODE)
                    .HasColumnName(@"OWNER_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.USRCODE)
                    .HasColumnName(@"USR_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.LASTOPENDATE)
                    .HasColumnName(@"LAST_OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.LASTEDITDATE)
                    .HasColumnName(@"LAST_EDIT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.VENDORID)
                    .HasColumnName(@"VENDOR_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.OLDCARD)
                    .HasColumnName(@"OLD_CARD")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.FUCLOSED)
                    .HasColumnName(@"FU_CLOSED")
                    .HasColumnType("int16");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.CONID)
                    .HasColumnName(@"CON_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
#if reserveword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.FROM)
                    .HasMaxLength(512)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.TO)
                    .HasMaxLength(512)
                    .HasColumnType("varchar2");
#endif
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.FILINGDATE)
                    .HasColumnName(@"FILING_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.NOTVALID)
                    .HasColumnName(@"NOT_VALID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.COMPANYID)
                    .HasColumnName(@"COMPANY_ID")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.CARDIDLIST)
                    .HasColumnName(@"CARD_ID_LIST")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.CUSTOMDOCID)
                    .HasColumnName(@"CUSTOM_DOC_ID")
                    .HasMaxLength(36)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.SPLITTEDSTATUS)
                    .HasColumnName(@"SPLITTED_STATUS")
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.PARENTCOMID)
                    .HasColumnName(@"PARENT_COM_ID")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.MD5HASH)
                    .HasMaxLength(36)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.SPLITRESULT)
                    .HasColumnName(@"SPLIT_RESULT")
                    .HasColumnType("clob");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.SERVERVER)
                    .HasColumnName(@"SERVER_VER")
                    .HasMaxLength(1)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.DELETED)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.HASSIGN)
                    .HasColumnName(@"HAS_SIGN")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.SIGNMETADATA)
                    .HasColumnName(@"SIGN_METADATA")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.OCR)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.CONVERT2TIFF)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.ISSHAREDWITHIMPORTER)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.ISORIGINAL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.PAGECOUNT)
                    .HasColumnName(@"PAGE_COUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.ISREQUESTED)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILING>()
                .Property(p => p.ISDECLARATIONRELATED)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GDMFLDRTR

            modelBuilder.Entity<GDMFLDRTR>()
                .HasKey(p => new { p.FOLDERCODE })
                .ToTable("GDMFLDRTR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.FOLDERCODE)
                    .HasColumnName(@"FOLDER_CODE")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.CODENAME)
                    .HasColumnName(@"CODE_NAME")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.DESCRIPTION)
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.PARENTCODE)
                    .HasColumnName(@"PARENT_CODE")
                    .HasMaxLength(9)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.SYSTEMFOLDER)
                    .HasColumnName(@"SYSTEM_FOLDER")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.ALLOWEDFILING)
                    .HasColumnName(@"ALLOWED_FILING")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.DOCID)
                    .HasColumnName(@"DOC_ID")
                    .HasMaxLength(7)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MPRIMARYNUM)
                    .HasColumnName(@"M_PRIMARY_NUM")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MCARDID)
                    .HasColumnName(@"M_CARD_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MVENDORID)
                    .HasColumnName(@"M_VENDOR_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MREFERENCE)
                    .HasColumnName(@"M_REFERENCE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MOLDCARD)
                    .HasColumnName(@"M_OLD_CARD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MUSRCODE)
                    .HasColumnName(@"M_USR_CODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MCONID)
                    .HasColumnName(@"M_CON_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.SENDPDF)
                    .HasColumnName(@"SEND_PDF")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.ALLOWEDUPDATEDOC)
                    .HasColumnName(@"ALLOWED_UPDATE_DOC")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.HYBRIDMAPPING)
                    .HasColumnName(@"HYBRID_MAPPING")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.DECLARATIONMAPPING)
                    .HasColumnName(@"DECLARATION_MAPPING")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.MANAGEDBYCLOUD)
                    .HasColumnName(@"MANAGED_BY_CLOUD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.NOPURGE)
                    .HasColumnName(@"NO_PURGE")
                    .HasColumnType("int");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.OCR)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFLDRTR>()
                .Property(p => p.CONVERT2TIFF)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GDMFILEVER

            modelBuilder.Entity<GDMFILEVER>()
                .HasKey(p => new { p.COMID, p.VERSION })
                .ToTable("GDMFILEVER", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.VERSION)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
#if reverseword //Message=ORA-01747: צוין צירוף לא תקף: משתמש.טבלה.עמודה, טבלה.עמודה או עמודה
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.DESC)
                    .HasMaxLength(100)
                    .HasColumnType("varchar2");
#endif
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.CHECKED)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.CHECKOUT)
                    .HasColumnName(@"CHECK_OUT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.CHECKOUTW)
                    .HasColumnName(@"CHECK_OUT_W")
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.FILESIZE)
                    .HasColumnName(@"FILE_SIZE")
                    .HasColumnType("double");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.HAVESIGNED)
                    .HasColumnName(@"HAVE_SIGNED")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.SIGNSTATUS)
                    .HasColumnName(@"SIGN_STATUS")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.ONSIGNSEND)
                    .HasColumnName(@"ON_SIGN_SEND")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.HAVEPDF)
                    .HasColumnName(@"HAVE_PDF")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.NEWSIGN)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.MD5HASH)
                    .HasMaxLength(36)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMFILEVER>()
                .Property(p => p.EXTENSION)
                    .HasMaxLength(40)
                    .HasColumnType("char");

            #endregion

            #region YCULTASK

            modelBuilder.Entity<YCULTASK>()
                .HasKey(p => new { p.TASKID })
                .ToTable("YCULTASK", "AMITESTM");
            // Properties:
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.TASKID)
                    .HasColumnName(@"TASK_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(12)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.TYPE)
                    .HasMaxLength(10)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.LOGTIME)
                    .HasColumnName(@"LOG_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int16");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.PROCESSSTARTTIME)
                    .HasColumnName(@"PROCESS_START_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.PROCESSENDTIME)
                    .HasColumnName(@"PROCESS_END_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.ARCHIVE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.STATUS)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.REQUESTDATA)
                    .HasColumnName(@"REQUEST_DATA")
                    .HasColumnType("clob");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.RESPONSE)
                    .HasColumnType("clob");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.CLIENTID)
                    .HasColumnName(@"CLIENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.USRCODE)
                    .HasColumnName(@"USR_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region YTBCUSTTB

            modelBuilder.Entity<YTBCUSTTB>()
                .HasKey(p => new { p.CUSTTB })
                .ToTable("YTBCUSTTB", "AMITESTM");
            // Properties:
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.CUSTTB)
                    .HasColumnName(@"CUST_TB")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.IIGUPDTDATE)
                    .HasColumnName(@"IIG_UPDT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.EDITABLE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<YTBCUSTTB>()
                .Property(p => p.RELATEDTABLE)
                    .HasColumnName(@"RELATED_TABLE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBCUSTSUP

            modelBuilder.Entity<CTBCUSTSUP>()
                .HasKey(p => new { p.SUPPLIERID })
                .ToTable("CTBCUSTSUP", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.SUPPLIERID)
                    .HasColumnName(@"SUPPLIER_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.CNTRYCODE)
                    .HasColumnName(@"CNTRY_CODE")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.ADDRESS)
                    .HasMaxLength(60)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.ZIPCODE)
                    .HasColumnName(@"ZIP_CODE")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.AREACODE1)
                    .HasColumnName(@"AREA_CODE_1")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.PHONENO1)
                    .HasColumnName(@"PHONE_NO_1")
                    .HasMaxLength(11)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.AREACODE2)
                    .HasColumnName(@"AREA_CODE_2")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.PHONENO2)
                    .HasColumnName(@"PHONE_NO_2")
                    .HasMaxLength(11)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.AREACODEFAX)
                    .HasColumnName(@"AREA_CODE_FAX")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.FAXNO)
                    .HasColumnName(@"FAX_NO")
                    .HasMaxLength(11)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.EMAIL)
                    .HasMaxLength(55)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.BUSINESSNO)
                    .HasColumnName(@"BUSINESS_NO")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.DUNSNO)
                    .HasColumnName(@"DUNS_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.CFISUPPLIERID)
                    .HasColumnName(@"CFI_SUPPLIER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCUSTSUP>()
                .Property(p => p.CODESCOPE)
                    .HasColumnName(@"CODE_SCOPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GTBITEM

            modelBuilder.Entity<GTBITEM>()
                .HasKey(p => new { p.ITEMID, p.PARTNERID })
                .ToTable("GTBITEMS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.PARTNERID)
                    .HasColumnName(@"PARTNER_ID")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.ITEMID)
                    .HasColumnName(@"ITEM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.UNITID)
                    .HasColumnName(@"UNIT_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.PRICEQTY)
                    .HasColumnName(@"PRICE_QTY")
                    .HasColumnType("int");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.COINID)
                    .HasColumnName(@"COIN_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.PRICE)
                    .HasColumnType("double");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.HARMONIZEID)
                    .HasColumnName(@"HARMONIZE_ID")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.PRATID)
                    .HasColumnName(@"PRAT_ID")
                    .HasMaxLength(50)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.BARCODE)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("double");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.WEIGHTUM)
                    .HasColumnName(@"WEIGHT_UM")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.SERIAL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.ITEMTYPE)
                    .HasColumnName(@"ITEM_TYPE")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.CHARGE)
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.NOOVERHEAD)
                    .HasColumnName(@"NO_OVERHEAD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.ORIGINCOUNTRY)
                    .HasColumnName(@"ORIGIN_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.STANDARTSIV)
                    .HasColumnName(@"STANDART_SIV")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.LICENSENO)
                    .HasColumnName(@"LICENSE_NO")
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.DESCRIPTION)
                    .HasColumnType("clob");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.LICENCESIV)
                    .HasColumnName(@"LICENCE_SIV")
                    .HasColumnType("clob");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.DESCRIPTIONHEB)
                    .HasColumnName(@"DESCRIPTION_HEB")
                    .HasColumnType("clob");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.NOSTANDART)
                    .HasColumnName(@"NO_STANDART")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITEM>()
                .Property(p => p.APPROVTYPEID)
                    .HasColumnName(@"APPROV_TYPE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");

            #endregion

            #region CCUCAR

            modelBuilder.Entity<CCUCAR>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CCUCAR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUCAR>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCAR>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCAR>()
                .Property(p => p.ABSDEDUCT)
                    .HasColumnName(@"ABS_DEDUCT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCAR>()
                .Property(p => p.KARITDEDUCT)
                    .HasColumnName(@"KARIT_DEDUCT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCAR>()
                .Property(p => p.BAKARADEDUCT)
                    .HasColumnName(@"BAKARA_DEDUCT")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCAR>()
                .Property(p => p.MEMIRDEDUCT)
                    .HasColumnName(@"MEMIR_DEDUCT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCAR>()
                .Property(p => p.MADADDEDUCT)
                    .HasColumnName(@"MADAD_DEDUCT")
                    .HasColumnType("decimal");

            #endregion

            #region CCUCARSC

            modelBuilder.Entity<CCUCARSC>()
                .HasKey(p => new { p.COUNTER, p.FILENO, p.LINENO })
                .ToTable("CCUCARSC", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.COUNTER)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.VEHICLEFILE)
                    .HasColumnName(@"VEHICLE_FILE")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.CARMODEL)
                    .HasColumnName(@"CAR_MODEL")
                    .HasMaxLength(25)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.CHASSISNO)
                    .HasColumnName(@"CHASSIS_NO")
                    .HasMaxLength(18)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.ENGINENO)
                    .HasColumnName(@"ENGINE_NO")
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.WINDOWNO)
                    .HasColumnName(@"WINDOW_NO")
                    .HasMaxLength(13)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.FOB)
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.GENERALTAX)
                    .HasColumnName(@"GENERAL_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.BUYTAX)
                    .HasColumnName(@"BUY_TAX")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.VATRESHIMON)
                    .HasColumnName(@"VAT_RESHIMON")
                    .HasColumnType("double");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.EXEMPTTYPE)
                    .HasColumnName(@"EXEMPT_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");

            #endregion

            #region CFIGOODDESC

            modelBuilder.Entity<CFIGOODDESC>()
                .HasKey(p => new { p.FILENO })
                .ToTable("CFIGOODDESC", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIGOODDESC>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIGOODDESC>()
                .Property(p => p.GOODDESC)
                    .HasColumnName(@"GOOD_DESC")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIGOODDESC>()
                .Property(p => p.CUSTOMERID)
                    .HasColumnName(@"CUSTOMER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");

            #endregion

            #region CCUSIGNUM

            modelBuilder.Entity<CCUSIGNUM>()
                .HasKey(p => new { p.FILENO, p.LINENOMSHGR, p.LINENOSIGN })
                .ToTable("CCUSIGNUM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUSIGNUM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSIGNUM>()
                .Property(p => p.LINENOMSHGR)
                    .HasColumnName(@"LINE_NO_MSHGR")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSIGNUM>()
                .Property(p => p.LINENOSIGN)
                    .HasColumnName(@"LINE_NO_SIGN")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSIGNUM>()
                .Property(p => p.SIGNNUM)
                    .HasColumnName(@"SIGN_NUM")
                    .HasMaxLength(30)
                    .HasColumnType("char");

            #endregion

            #region CFIMSVLINE

            modelBuilder.Entity<CFIMSVLINE>()
                .HasKey(p => new { p.FILENO, p.COMID, p.PAGENUM, p.LINENUM, p.QUETYPE })
                .ToTable("CFIMSVLINE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.PAGENUM)
                    .HasColumnName(@"PAGE_NUM")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.LINENUM)
                    .HasColumnName(@"LINE_NUM")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.TARIFFCODE)
                    .HasColumnName(@"TARIFF_CODE")
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.TOP)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.HEIGHT)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.GROUPNUM)
                    .HasColumnName(@"GROUP_NUM")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.REMARK)
                    .HasMaxLength(512)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.YEVU)
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.SUGGESTM)
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.SUGGESTI)
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.SUGGESTDET1)
                    .HasColumnName(@"SUGGESTDET")
                    .HasMaxLength(2048)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.STATUS)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.TAXEXEMPT)
                    .HasColumnName(@"TAX_EXEMPT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.INVOICEQUANTITY)
                    .HasColumnName(@"INVOICE_QUANTITY")
                    .HasColumnType("double");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.INVOICEQUANTITYTYPE)
                    .HasColumnName(@"INVOICE_QUANTITY_TYPE")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.QUETYPE)
                    .HasColumnName(@"QUE_TYPE")
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.CATALOGID)
                    .HasColumnName(@"CATALOG_ID")
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.CATALOGNAME)
                    .HasColumnName(@"CATALOG_NAME")
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.AMOUNT)
                    .HasColumnType("double");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.STATAMOUNT)
                    .HasColumnName(@"STAT_AMOUNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.STATTYPE)
                    .HasColumnName(@"STAT_TYPE")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.ORIGINID)
                    .HasColumnName(@"ORIGIN_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.ITEMPRICE)
                    .HasColumnName(@"ITEM_PRICE")
                    .HasColumnType("decimal")
                    .HasPrecision(16, 4);
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.OCRQUANTITY)
                    .HasColumnName(@"OCR_QUANTITY")
                    .HasColumnType("double");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.OCRQUANTITYTYPE)
                    .HasColumnName(@"OCR_QUANTITY_TYPE")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
                .Property(p => p.LINECOUNTER)
                    .HasColumnName(@"LINE_COUNTER")
                    .HasColumnType("int");
            #endregion

            #region CFIMSVDOC

            modelBuilder.Entity<CFIMSVDOC>()
                .HasKey(p => new { p.FILENO, p.COMID })
                .ToTable("CFIMSVDOC", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.CREATEDATE)
                    .HasColumnName(@"CREATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.UPDATEDATE)
                    .HasColumnName(@"UPDATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.CREATEBY)
                    .HasColumnName(@"CREATE_BY")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.UPDATEBY)
                    .HasColumnName(@"UPDATE_BY")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.STATUS)
                    .HasColumnType("int16");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.REMARK)
                    .HasMaxLength(512)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.TOTALPAGES)
                    .HasColumnName(@"TOTAL_PAGES")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.CUSTOMERID)
                    .HasColumnName(@"CUSTOMER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.HASCHANGED)
                    .HasColumnName(@"HAS_CHANGED")
                    .HasColumnType("int16");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.QUETYPE)
                    .HasColumnName(@"QUE_TYPE")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.GSTRING1)
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.GSTRING2)
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.GSTRING3)
                    .HasMaxLength(512)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                 .Property(p => p.INVOICEDATE)
                     .HasColumnName(@"INVOICE_DATE")
                     .HasColumnType("date");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.INVOICEAMOUNT)
                    .HasColumnName(@"INVOICE_AMOUNT")
                    .HasColumnType("decimal")
                    .HasPrecision(16, 4);
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.CURRENCYID)
                    .HasColumnName(@"CURRENCY_ID")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.CUSTOMSSUPPLIERID)
                    .HasColumnName(@"CUSTOMS_SUPPLIER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.INCOTERMS)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.TAX)
                    .HasColumnType("decimal")
                    .HasPrecision(16, 4);
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.DISCOUNT)
                    .HasColumnType("decimal")
                    .HasPrecision(16, 4);
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.ORIGINID)
                    .HasColumnName(@"ORIGIN_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.MORE)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.ADDITIONAL)
                    .HasColumnType("decimal")
                    .HasPrecision(16, 4);
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.INVOICENO)
                    .HasColumnName(@"INVOICE_NO")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            #endregion

            #region GTBDOC

            modelBuilder.Entity<GTBDOC>()
                .HasKey(p => new { p.DOCID })
                .ToTable("GTBDOC", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.DOCID)
                    .HasColumnName(@"DOC_ID")
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.USERDOC)
                    .HasColumnName(@"USER_DOC")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.DISCLIENT)
                    .HasColumnName(@"DIS_CLIENT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.DISFOREIGNCL)
                    .HasColumnName(@"DIS_FOREIGN_CL")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.DISAGENT)
                    .HasColumnName(@"DIS_AGENT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.COPYDESC)
                    .HasColumnName(@"COPY_DESC")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.FOLDERCODE)
                    .HasColumnName(@"FOLDER_CODE")
                    .HasMaxLength(9)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.DECLARATIONMAPPING)
                    .HasColumnName(@"DECLARATION_MAPPING")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDOC>()
                .Property(p => p.OCR)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GDMENTITY

            modelBuilder.Entity<GDMENTITY>()
                .HasKey(p => new { p.COMID, p.PRIMARYID, p.PRIMARYNUM })
                .ToTable("GDMENTITY", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDMENTITY>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMENTITY>()
                .Property(p => p.PRIMARYID)
                    .HasColumnName(@"PRIMARY_ID")
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMENTITY>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");

            #endregion

            #region GDMREF

            modelBuilder.Entity<GDMREF>()
                .HasKey(p => new { p.COMID, p.REFERENCE, p.REFID })
                .ToTable("GDMREF", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDMREF>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMREF>()
                .Property(p => p.REFID)
                    .HasColumnName(@"REF_ID")
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMREF>()
                .Property(p => p.REFERENCE)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMREF>()
                .Property(p => p.METADATA)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMREF>()
                .Property(p => p.MDNEW)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GAQDOC

            modelBuilder.Entity<GAQDOC>()
                .HasKey(p => new { p.APPQID, p.DOCID, p.FOLDERCODE })
                .ToTable("GAQDOC", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQDOC>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDOC>()
                .Property(p => p.FOLDERCODE)
                    .HasColumnName(@"FOLDER_CODE")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDOC>()
                .Property(p => p.DOCID)
                    .HasColumnName(@"DOC_ID")
                    .IsRequired()
                    .HasMaxLength(7)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");

            #endregion

            #region CFIMSVFILE

            modelBuilder.Entity<CFIMSVFILE>()
                .HasKey(p => new { p.FILENO })
                .ToTable("CFIMSVFILE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIMSVFILE>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIMSVFILE>()
                .Property(p => p.REMARK)
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVFILE>()
                .Property(p => p.AQOPERATION)
                    .HasColumnName(@"AQ_OPERATION")
                    .HasColumnType("int");
            #endregion

            #region CFIMSVPAGE

            modelBuilder.Entity<CFIMSVPAGE>()
                .HasKey(p => new { p.FILENO, p.COMID, p.PAGENUM, p.QUETYPE })
                .ToTable("CFIMSVPAGE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.PAGENUM)
                    .HasColumnName(@"PAGE_NUM")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.WIDTH)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.HEIGHT)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.LEFTDATA)
                    .HasColumnName(@"LEFT_DATA")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.TOPDATA)
                    .HasColumnName(@"TOP_DATA")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.WIDTHDATA)
                    .HasColumnName(@"WIDTH_DATA")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.HEIGHTDATA)
                    .HasColumnName(@"HEIGHT_DATA")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVPAGE>()
                .Property(p => p.QUETYPE)
                    .HasColumnName(@"QUE_TYPE")
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");

            #endregion

            #region GTBITMCN

            modelBuilder.Entity<GTBITMCN>()
                .HasKey(p => new { p.ITEM2ID, p.ITEMID, p.PARTNER2ID, p.PARTNERID })
                .ToTable("GTBITMCN", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBITMCN>()
                .Property(p => p.PARTNERID)
                    .HasColumnName(@"PARTNER_ID")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITMCN>()
                .Property(p => p.ITEMID)
                    .HasColumnName(@"ITEM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITMCN>()
                .Property(p => p.PARTNER2ID)
                    .HasColumnName(@"PARTNER2_ID")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBITMCN>()
                .Property(p => p.ITEM2ID)
                    .HasColumnName(@"ITEM2_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");

            #endregion

            #region GITITEM

            modelBuilder.Entity<GITITEM>()
                .HasKey(p => new { p.COUNTER })
                .ToTable("GITITEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.COUNTER)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.PARTNERID)
                    .HasColumnName(@"PARTNER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ITEMNO)
                    .HasColumnName(@"ITEM_NO")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.SAPAKID)
                    .HasColumnName(@"SAPAK_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ACCOUNTINGCLOSE)
                    .HasColumnName(@"ACCOUNTING_CLOSE")
                    .HasColumnType("int16");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ITEMCLOSE)
                    .HasColumnName(@"ITEM_CLOSE")
                    .HasColumnType("int16");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ITEMCANCELLED)
                    .HasColumnName(@"ITEM_CANCELLED")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ITEMOPENUSER)
                    .HasColumnName(@"ITEM_OPEN_USER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ITEMUPDATEDATE)
                    .HasColumnName(@"ITEM_UPDATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.PRATID)
                    .HasColumnName(@"PRAT_ID")
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ITEMUPDATEUSER)
                    .HasColumnName(@"ITEM_UPDATE_USER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.NOSTANDART)
                    .HasColumnName(@"NO_STANDART")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.APPROVTYPEID)
                    .HasColumnName(@"APPROV_TYPE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(300)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.LICENCESIV)
                    .HasColumnName(@"LICENCE_SIV")
                    .HasColumnType("clob");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ORIGINCOUNTRY)
                    .HasColumnName(@"ORIGIN_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
               .Property(p => p.UNITID)
                   .HasColumnName(@"UNIT_ID")
                   .HasMaxLength(3)
                   .HasColumnType("varchar2");
            #endregion

            #region CFICONN

            modelBuilder.Entity<CFICONN>()
                .HasKey(p => new { p.CUSTOMFILE, p.FILENO })
                .ToTable("CFICONN", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFICONN>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CFICONN>()
                .Property(p => p.CUSTOMFILE)
                    .HasColumnName(@"CUSTOM_FILE")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");

            #endregion

            #region CFIPACK

            modelBuilder.Entity<CFIPACK>()
                .HasKey(p => new { p.FILENO, p.LINENO })
                .ToTable("CFIPACKS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.CONTNO)
                    .HasColumnName(@"CONT_NO")
                    .HasMaxLength(14)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.CONTTYPEID)
                    .HasColumnName(@"CONT_TYPE_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.SEAL)
                    .HasMaxLength(12)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("double");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.QTYADD)
                    .HasColumnName(@"QTY_ADD")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.AVAILABILITY)
                    .HasColumnType("date");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.EXITFROMPORT)
                    .HasColumnName(@"EXIT_FROM_PORT")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.DAMAGE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIPACK>()
                .Property(p => p.LACK)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion


            #region CCUSUPITEMSI

            modelBuilder.Entity<CCUSUPITEMSI>()
                .HasKey(p => new { p.ACCLINENO, p.FILENO, p.LINEID, p.LINENO, p.SICOUNTER })
                .ToTable("CCUSUPITEMSI", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUSUPITEMSI>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEMSI>()
                .Property(p => p.ACCLINENO)
                    .HasColumnName(@"ACC_LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEMSI>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEMSI>()
                .Property(p => p.SICOUNTER)
                    .HasColumnName(@"SI_COUNTER")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEMSI>()
                .Property(p => p.LINEID)
                    .HasColumnName(@"LINE_ID")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUSUPITEMSI>()
                .Property(p => p.MOREDATA)
                    .HasColumnName(@"MORE_DATA")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");

            #endregion

            #region VRELEASE2ENTRY

            modelBuilder.Entity<VRELEASE2ENTRY>()
                .HasKey(p => new { p.CUFILENO, p.ENTRYCAFILENO })
                .ToTable("V_RELEASE2ENTRY", "AMITESTM");
            // Properties:
            modelBuilder.Entity<VRELEASE2ENTRY>()
                .Property(p => p.CUFILENO)
                    .HasColumnName(@"CU_FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<VRELEASE2ENTRY>()
                .Property(p => p.CUSTOMFILENO)
                    .HasColumnName(@"CUSTOM_FILE_NO")
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<VRELEASE2ENTRY>()
                .Property(p => p.CAFILENO)
                    .HasColumnName(@"CA_FILE_NO")
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<VRELEASE2ENTRY>()
                .Property(p => p.RELEASEFILENO)
                    .HasColumnName(@"RELEASE_FILE_NO")
                    .HasColumnType("int64");
            modelBuilder.Entity<VRELEASE2ENTRY>()
                .Property(p => p.ENTRYFILENO)
                    .HasColumnName(@"ENTRY_FILE_NO")
                    .HasColumnType("int");
            modelBuilder.Entity<VRELEASE2ENTRY>()
                .Property(p => p.ENTRYCAFILENO)
                    .HasColumnName(@"ENTRY_CA_FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");

            #endregion

            #region YTBTABLE

            modelBuilder.Entity<YTBTABLE>()
                .HasKey(p => new { p.CUSTTB, p.TBCODE })
                .ToTable("YTBTABLE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.CUSTTB)
                    .HasColumnName(@"CUST_TB")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.TBCODE)
                    .HasColumnName(@"TB_CODE")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(512)
                    .HasColumnType("nvarchar2");
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(512)
                    .HasColumnType("nvarchar2");
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(512)
                    .HasColumnType("nvarchar2");
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.IIGUPDTDATE)
                    .HasColumnName(@"IIG_UPDT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<YTBTABLE>()
                .Property(p => p.TBCODENUM)
                    .HasColumnName(@"TB_CODE_NUM")
                    .HasColumnType("int64");

            #endregion

            #region CFIMSVREM

            modelBuilder.Entity<CFIMSVREM>()
                .HasKey(p => new { p.COMID, p.FILENO, p.HEIGHT, p.LEFT, p.PAGENUM, p.REMARK, p.TOP, p.WIDTH })
                .ToTable("CFIMSVREM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.PAGENUM)
                    .HasColumnName(@"PAGE_NUM")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.TOP)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.LEFT)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.HEIGHT)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.WIDTH)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVREM>()
                .Property(p => p.REMARK)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");

            #endregion

            #region CFIMSVFLINE

            modelBuilder.Entity<CFIMSVFLINE>()
                .HasKey(p => new { p.FILENO, p.COMID, p.LINENUM })
                .ToTable("CFIMSVFLINE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIMSVFLINE>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIMSVFLINE>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVFLINE>()
                .Property(p => p.LINENUM)
                    .HasColumnName(@"LINE_NUM")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVFLINE>()
                .Property(p => p.ITEMNAME)
                    .HasColumnName(@"ITEM_NAME")
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVFLINE>()
                .Property(p => p.ITEMVALUE)
                    .HasColumnName(@"ITEM_VALUE")
                    .HasColumnType("double");
            modelBuilder.Entity<CFIMSVFLINE>()
                .Property(p => p.ITEMTYPE)
                    .HasColumnName(@"ITEM_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");

            #endregion


            #region GDMLOCK

            modelBuilder.Entity<GDMLOCK>()
                .HasKey(p => p.COMID)
                .ToTable("GDMLOCK", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDMLOCK>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMLOCK>()
                .Property(p => p.FILINGUPDATED)
                    .HasColumnName(@"FILING_UPDATED")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMLOCK>()
                .Property(p => p.OCR)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMLOCK>()
                .Property(p => p.CONVERT2TIFF)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GDMLOCK>()
                .Property(p => p.PAGECOUNT)
                    .HasColumnName(@"PAGE_COUNT")
                    .HasColumnType("int");

            #endregion
            #region Disabled conventions


            #endregion
            return modelBuilder;///base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// There are no comments for YCULPROCESS in the schema.
        /// </summary>
        public DbSet<YCULPROCESS> YCULPROCESSES { get; set; }

        /// <summary>
        /// There are no comments for GGGQ in the schema.
        /// </summary>
        public DbSet<GGGQ> GGGQs { get; set; }

        /// <summary>
        /// There are no comments for CCUACCSUP in the schema.
        /// </summary>
        public DbSet<CCUACCSUP> CCUACCSUPs { get; set; }

        /// <summary>
        /// There are no comments for CCUMSHGR in the schema.
        /// </summary>
        public DbSet<CCUMSHGR> CCUMSHGRs { get; set; }

        /// <summary>
        /// There are no comments for CCUPAYHAND in the schema.
        /// </summary>
        public DbSet<CCUPAYHAND> CCUPAYHANDs { get; set; }

        /// <summary>
        /// There are no comments for CCUSUPITEM in the schema.
        /// </summary>
        public DbSet<CCUSUPITEM> CCUSUPITEMs { get; set; }

        /// <summary>
        /// There are no comments for GDFDATA in the schema.
        /// </summary>
        public DbSet<GDFDATA> GDFDATAs { get; set; }

        /// <summary>
        /// There are no comments for CCUCRREQ in the schema.
        /// </summary>
        public DbSet<CCUCRREQ> CCUCRREQs { get; set; }

        /// <summary>
        /// There are no comments for GTRTRAN in the schema.
        /// </summary>
        public DbSet<GTRTRAN> GTRTRANs { get; set; }

        /// <summary>
        /// There are no comments for GRTRATE in the schema.
        /// </summary>
        public DbSet<GRTRATE> GRTRATEs { get; set; }

        /// <summary>
        /// There are no comments for CCUCUSTITEM in the schema.
        /// </summary>
        public DbSet<CCUCUSTITEM> CCUCUSTITEMs { get; set; }

        /// <summary>
        /// There are no comments for CCUQUELOCK in the schema.
        /// </summary>
        public DbSet<CCUQUELOCK> CCUQUELOCKs { get; set; }

        /// <summary>
        /// There are no comments for CCUPAYLINEF in the schema.
        /// </summary>
        public DbSet<CCUPAYLINEF> CCUPAYLINEFs { get; set; }

        /// <summary>
        /// There are no comments for CCUTAX in the schema.
        /// </summary>
        public DbSet<CCUTAX> CCUTAXES { get; set; }

        /// <summary>
        /// There are no comments for CCUFILEM in the schema.
        /// </summary>
        public DbSet<CCUFILEM> CCUFILEMs { get; set; }

        /// <summary>
        /// There are no comments for GTBMANDT in the schema.
        /// </summary>
        public DbSet<GTBMANDT> GTBMANDTs { get; set; }

        /// <summary>
        /// There are no comments for GNDCARD in the schema.
        /// </summary>
        public DbSet<GNDCARD> GNDCARDs { get; set; }

        /// <summary>
        /// There are no comments for CTBPACKTYPE in the schema.
        /// </summary>
        public DbSet<CTBPACKTYPE> CTBPACKTYPEs { get; set; }

        /// <summary>
        /// There are no comments for GAQFILEDATA in the schema.
        /// </summary>
        public DbSet<GAQFILEDATA> GAQFILEDATAs { get; set; }

        /// <summary>
        /// There are no comments for CTBBONDED in the schema.
        /// </summary>
        public DbSet<CTBBONDED> CTBBONDEDs { get; set; }

        /// <summary>
        /// There are no comments for CTBCOUNTRY in the schema.
        /// </summary>
        public DbSet<CTBCOUNTRY> CTBCOUNTRIES { get; set; }

        /// <summary>
        /// There are no comments for CTBIDNTP in the schema.
        /// </summary>
        public DbSet<CTBIDNTP> CTBIDNTPs { get; set; }

        /// <summary>
        /// There are no comments for CTBIMPORT in the schema.
        /// </summary>
        public DbSet<CTBIMPORT> CTBIMPORTs { get; set; }

        /// <summary>
        /// There are no comments for CTBMISHGUR in the schema.
        /// </summary>
        public DbSet<CTBMISHGUR> CTBMISHGURs { get; set; }

        /// <summary>
        /// There are no comments for CTBRESHTYPE in the schema.
        /// </summary>
        public DbSet<CTBRESHTYPE> CTBRESHTYPEs { get; set; }

        /// <summary>
        /// There are no comments for CTBRGOWN in the schema.
        /// </summary>
        public DbSet<CTBRGOWN> CTBRGOWNs { get; set; }

        /// <summary>
        /// There are no comments for CTBSTORAGE in the schema.
        /// </summary>
        public DbSet<CTBSTORAGE> CTBSTORAGEs { get; set; }

        /// <summary>
        /// There are no comments for CTBTRANSP in the schema.
        /// </summary>
        public DbSet<CTBTRANSP> CTBTRANSPs { get; set; }

        /// <summary>
        /// There are no comments for CTBUNLOAD in the schema.
        /// </summary>
        public DbSet<CTBUNLOAD> CTBUNLOADs { get; set; }

        /// <summary>
        /// There are no comments for ATBPTIL in the schema.
        /// </summary>
        public DbSet<ATBPTIL> ATBPTILs { get; set; }

        /// <summary>
        /// There are no comments for CTBLOAD in the schema.
        /// </summary>
        public DbSet<CTBLOAD> CTBLOADs { get; set; }

        /// <summary>
        /// There are no comments for GNDADR in the schema.
        /// </summary>
        public DbSet<GNDADR> GNDADRs { get; set; }

        /// <summary>
        /// There are no comments for CTBPART in the schema.
        /// </summary>
        public DbSet<CTBPART> CTBPARTs { get; set; }

        /// <summary>
        /// There are no comments for CTBPKDT in the schema.
        /// </summary>
        public DbSet<CTBPKDT> CTBPKDTs { get; set; }

        /// <summary>
        /// There are no comments for CTBAPPROV in the schema.
        /// </summary>
        public DbSet<CTBAPPROV> CTBAPPROVs { get; set; }

        /// <summary>
        /// There are no comments for CTBAPPROVTYPE in the schema.
        /// </summary>
        public DbSet<CTBAPPROVTYPE> CTBAPPROVTYPEs { get; set; }

        /// <summary>
        /// There are no comments for GTBREQCERT in the schema.
        /// </summary>
        public DbSet<GTBREQCERT> GTBREQCERTs { get; set; }

        /// <summary>
        /// There are no comments for CCUTRANSPVAL in the schema.
        /// </summary>
        public DbSet<CCUTRANSPVAL> CCUTRANSPVALs { get; set; }

        /// <summary>
        /// There are no comments for CTBCURRENCY in the schema.
        /// </summary>
        public DbSet<CTBCURRENCY> CTBCURRENCIES { get; set; }

        /// <summary>
        /// There are no comments for CTBINCOTERM in the schema.
        /// </summary>
        public DbSet<CTBINCOTERM> CTBINCOTERMs { get; set; }

        /// <summary>
        /// There are no comments for CTBTARIFF in the schema.
        /// </summary>
        public DbSet<CTBTARIFF> CTBTARIFFs { get; set; }

        /// <summary>
        /// There are no comments for CCUMESSAGE in the schema.
        /// </summary>
        public DbSet<CCUMESSAGE> CCUMESSAGEs { get; set; }

        /// <summary>
        /// There are no comments for CTBERROR in the schema.
        /// </summary>
        public DbSet<CTBERROR> CTBERRORs { get; set; }

        /// <summary>
        /// There are no comments for CCUCARL in the schema.
        /// </summary>
        public DbSet<CCUCARL> CCUCARLs { get; set; }

        /// <summary>
        /// There are no comments for CTBMEMIRTYPE in the schema.
        /// </summary>
        public DbSet<CTBMEMIRTYPE> CTBMEMIRTYPEs { get; set; }

        /// <summary>
        /// There are no comments for CCUTSRUFOT in the schema.
        /// </summary>
        public DbSet<CCUTSRUFOT> CCUTSRUFOTs { get; set; }

        /// <summary>
        /// There are no comments for CTBTSRUFTYPE in the schema.
        /// </summary>
        public DbSet<CTBTSRUFTYPE> CTBTSRUFTYPEs { get; set; }

        /// <summary>
        /// There are no comments for CTBTAXTYPE in the schema.
        /// </summary>
        public DbSet<CTBTAXTYPE> CTBTAXTYPEs { get; set; }

        /// <summary>
        /// There are no comments for ITBPCKTY in the schema.
        /// </summary>
        public DbSet<ITBPCKTY> ITBPCKTIES { get; set; }

        /// <summary>
        /// There are no comments for GDMFILING in the schema.
        /// </summary>
        public DbSet<GDMFILING> GDMFILINGs { get; set; }

        /// <summary>
        /// There are no comments for GDMFLDRTR in the schema.
        /// </summary>
        public DbSet<GDMFLDRTR> GDMFLDRTRs { get; set; }

        /// <summary>
        /// There are no comments for GDMFILEVER in the schema.
        /// </summary>
        public DbSet<GDMFILEVER> GDMFILEVERs { get; set; }

        /// <summary>
        /// There are no comments for YCULTASK in the schema.
        /// </summary>
        public DbSet<YCULTASK> YCULTASKs { get; set; }

        /// <summary>
        /// There are no comments for YTBCUSTTB in the schema.
        /// </summary>
        public DbSet<YTBCUSTTB> YTBCUSTTBs { get; set; }

        /// <summary>
        /// There are no comments for CTBCUSTSUP in the schema.
        /// </summary>
        public DbSet<CTBCUSTSUP> CTBCUSTSUPs { get; set; }

        /// <summary>
        /// There are no comments for GTBITEM in the schema.
        /// </summary>
        public DbSet<GTBITEM> GTBITEMs { get; set; }

        /// <summary>
        /// There are no comments for CCUCAR in the schema.
        /// </summary>
        public DbSet<CCUCAR> CCUCARs { get; set; }

        /// <summary>
        /// There are no comments for CCUCARSC in the schema.
        /// </summary>
        public DbSet<CCUCARSC> CCUCARSCs { get; set; }

        /// <summary>
        /// There are no comments for CFIGOODDESC in the schema.
        /// </summary>
        public DbSet<CFIGOODDESC> CFIGOODDESCs { get; set; }

        /// <summary>
        /// There are no comments for CCUSIGNUM in the schema.
        /// </summary>
        public DbSet<CCUSIGNUM> CCUSIGNUMs { get; set; }

        /// <summary>
        /// There are no comments for CFIMSVLINE in the schema.
        /// </summary>
        public DbSet<CFIMSVLINE> CFIMSVLINEs { get; set; }

        /// <summary>
        /// There are no comments for CFIMSVDOC in the schema.
        /// </summary>
        public DbSet<CFIMSVDOC> CFIMSVDOCs { get; set; }

        /// <summary>
        /// There are no comments for GTBDOC in the schema.
        /// </summary>
        public DbSet<GTBDOC> GTBDOCs { get; set; }

        /// <summary>
        /// There are no comments for GDMENTITY in the schema.
        /// </summary>
        public DbSet<GDMENTITY> GDMENTITIES { get; set; }

        /// <summary>
        /// There are no comments for GDMREF in the schema.
        /// </summary>
        public DbSet<GDMREF> GDMREFs { get; set; }

        /// <summary>
        /// There are no comments for GAQDOC in the schema.
        /// </summary>
        public DbSet<GAQDOC> GAQDOCs { get; set; }

        /// <summary>
        /// There are no comments for CFIMSVFILE in the schema.
        /// </summary>
        public DbSet<CFIMSVFILE> CFIMSVFILEs { get; set; }

        /// <summary>
        /// There are no comments for CFIMSVPAGE in the schema.
        /// </summary>
        public DbSet<CFIMSVPAGE> CFIMSVPAGEs { get; set; }

        /// <summary>
        /// There are no comments for GTBITMCN in the schema.
        /// </summary>
        public DbSet<GTBITMCN> GTBITMCNs { get; set; }

        /// <summary>
        /// There are no comments for GITITEM in the schema.
        /// </summary>
        public DbSet<GITITEM> GITITEMs { get; set; }

        /// <summary>
        /// There are no comments for CFICONN in the schema.
        /// </summary>
        public DbSet<CFICONN> CFICONNs { get; set; }

        /// <summary>
        /// There are no comments for CFIPACK in the schema.
        /// </summary>
        public DbSet<CFIPACK> CFIPACKs { get; set; }

        /// <summary>
        /// There are no comments for CCUSUPITEMSI in the schema.
        /// </summary>
        public DbSet<CCUSUPITEMSI> CCUSUPITEMSIs { get; set; }

        /// <summary>
        /// There are no comments for VRELEASE2ENTRY in the schema.
        /// </summary>
        public DbSet<VRELEASE2ENTRY> VRELEASE2ENTRIES { get; set; }

        /// <summary>
        /// There are no comments for YTBTABLE in the schema.
        /// </summary>
        public DbSet<YTBTABLE> YTBTABLEs { get; set; }

        /// <summary>
        /// There are no comments for CFIMSVREM in the schema.
        /// </summary>
        public DbSet<CFIMSVREM> CFIMSVREMs { get; set; }

        /// <summary>
        /// There are no comments for CFIMSVFLINE in the schema.
        /// </summary>
        public virtual DbSet<CFIMSVFLINE> CFIMSVFLINEs { get; set; }

        public virtual DbSet<GDMLOCK> GDMLOCKs { get; set; }
    }
}