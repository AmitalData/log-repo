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
                    .HasColumnType("decimal");
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
                    .HasColumnType("int64");
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
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.INCOTERMIDN)
                    .HasColumnName(@"INCOTERM_ID_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.COUNTRYIDN)
                    .HasColumnName(@"COUNTRY_ID_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.CURRENCYIDN)
                    .HasColumnName(@"CURRENCY_ID_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");

          
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
                    .HasColumnType("int64");
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
                    .HasColumnType("nvarchar2");
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
                .HasKey(p => p.FILENO)
                .ToTable("CCUPAYHAND", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.DRAFTSTATUS)
                    .HasColumnName(@"DRAFT_STATUS")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.PAYTAX)
                    .HasColumnName(@"PAY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.REJECTTAX)
                    .HasColumnName(@"REJECT_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.HANDTYPE)
                    .HasColumnName(@"HAND_TYPE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.PROCESSWANT)
                    .HasColumnName(@"PROCESS_WANT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TOTALPAYTAX)
                    .HasColumnName(@"TOTAL_PAY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TOTALPAYDEPOSIT)
                    .HasColumnName(@"TOTAL_PAY_DEPOSIT")
                    .HasColumnType("decimal");
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
                .HasKey(p => new { p.FILENO, p.ACCLINENO, p.LINENO })
                .ToTable("CCUSUPITEMS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.RAISEVALUE)
                    .HasColumnName(@"RAISE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NISVALUE)
                    .HasColumnName(@"NIS_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.EXTRAQNTY)
                    .HasColumnName(@"EXTRA_QNTY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.WHOLESALEPRICE)
                    .HasColumnName(@"WHOLESALE_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.IMPORTADDITION)
                    .HasColumnName(@"IMPORT_ADDITION")
                    .HasColumnType("decimal");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AGNTPAYTAX)
                    .HasColumnName(@"AGNT_PAY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AGNTPAYBITHA)
                    .HasColumnName(@"AGNT_PAY_BITHA")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NIDHEMEHESPCNT)
                    .HasColumnName(@"NIDHE_MEHES_PCNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NIDHEMASPCNT)
                    .HasColumnName(@"NIDHE_MAS_PCNT")
                    .HasColumnType("decimal");
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
                    .HasColumnType("decimal");
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
                    .HasColumnType("int64");
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
                    .HasColumnType("int64");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.RAISEVALUE)
                    .HasColumnName(@"RAISE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NISVALUE)
                    .HasColumnName(@"NIS_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.EXTRAQNTY)
                    .HasColumnName(@"EXTRA_QNTY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.WHOLESALEPRICE)
                    .HasColumnName(@"WHOLESALE_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.IMPORTADDITION)
                    .HasColumnName(@"IMPORT_ADDITION")
                    .HasColumnType("decimal");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AGNTPAYTAX)
                    .HasColumnName(@"AGNT_PAY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AGNTPAYBITHA)
                    .HasColumnName(@"AGNT_PAY_BITHA")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NIDHEMEHESPCNT)
                    .HasColumnName(@"NIDHE_MEHES_PCNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NIDHEMASPCNT)
                    .HasColumnName(@"NIDHE_MAS_PCNT")
                    .HasColumnType("decimal");
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
                    .HasColumnType("decimal");
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
                    .HasColumnType("int64");
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
                    .HasColumnType("decimal");
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
                    .HasColumnType("int64");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXAMOUNT)
                    .HasColumnName(@"TAX_AMOUNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.POSTPONEDTAX)
                    .HasColumnName(@"POSTPONED_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXTOPAY)
                    .HasColumnName(@"TAX_TO_PAY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXCALCCODE)
                    .HasColumnName(@"TAX_CALC_CODE")
                    .HasColumnType("int16");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXRATE)
                    .HasColumnName(@"TAX_RATE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.DEFINEDTAX)
                    .HasColumnName(@"DEFINED_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDTAXRATE)
                    .HasColumnName(@"ADD_TAX_RATE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDEFINEDTAX)
                    .HasColumnName(@"AD_DEFINED_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDIMPORT)
                    .HasColumnName(@"ADD_IMPORT")
                    .HasColumnType("decimal");
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
                .HasKey(p => p.FILENO)
                .ToTable("CCUFILEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.REGIONVALUE)
                    .HasColumnName(@"REGION_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANSPVALUE)
                    .HasColumnName(@"TRANSP_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEVALUE)
                    .HasColumnName(@"INSURANCE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.SERVICEVALUE)
                    .HasColumnName(@"SERVICE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.EXPENSEVALUE)
                    .HasColumnName(@"EXPENSE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CLOSUREVALUE)
                    .HasColumnName(@"CLOSURE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FEECARRIER)
                    .HasColumnName(@"FEE_CARRIER")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FEEPLATFORM)
                    .HasColumnName(@"FEE_PLATFORM")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CURRENCYRATE)
                    .HasColumnName(@"CURRENCY_RATE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PRICEINDEX)
                    .HasColumnName(@"PRICE_INDEX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GOODSVALUE)
                    .HasColumnName(@"GOODS_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CIFVALUE)
                    .HasColumnName(@"CIF_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.ACCEPTEDPRICE)
                    .HasColumnName(@"ACCEPTED_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TOTALTAX)
                    .HasColumnName(@"TOTAL_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANSPVALFC)
                    .HasColumnName(@"TRANSP_VAL_FC")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANCURRENCY)
                    .HasColumnName(@"TRAN_CURRENCY")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEPERCENT)
                    .HasColumnName(@"INSURANCE_PERCENT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCECURR)
                    .HasColumnName(@"INSURANCE_CURR")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEAMNT)
                    .HasColumnName(@"INSURANCE_AMNT")
                    .HasColumnType("decimal");
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
                    .HasColumnType("decimal");
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
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RIGHTOWNIDN)
                    .HasColumnName(@"RIGHT_OWN_ID_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CCUFILEM>()
               .Property(p => p.SELLCONDITIONIDN)
                   .HasColumnName(@"SELL_CONDITION_ID_N")
                   .HasMaxLength(35)
                   .HasColumnType("varchar2");


                





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
                    .HasColumnType("int64");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.TRANSPVALFC)
                    .HasColumnName(@"TRANSP_VAL_FC")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.CURRID)
                    .HasColumnName(@"CURR_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.TRANSPVAL)
                    .HasColumnName(@"TRANSP_VAL")
                    .HasColumnType("decimal");
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
                    .HasColumnType("int64");
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
                    .HasColumnType("int64");
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
                    .HasMaxLength(12)
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.FUELTYPE)
                    .HasColumnName(@"FUEL_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("decimal");
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
                    .HasColumnType("int64");
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
                    .HasColumnType("int64");
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
                    .HasMaxLength(12)
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.GENERALTAX)
                    .HasColumnName(@"GENERAL_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.BUYTAX)
                    .HasColumnName(@"BUY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.VATRESHIMON)
                    .HasColumnName(@"VAT_RESHIMON")
                    .HasColumnType("decimal");
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
                    .HasColumnType("int64");
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
            modelBuilder.Entity<CFIMSVLINE>()
.Property(p => p.PROTESTREMARK)
.HasColumnName(@"PROTEST_REMARK")
.HasMaxLength(255)
.HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
            .Property(p => p.MAKATREMARK)
            .HasColumnName(@"MAKAT_REMARK")
            .HasMaxLength(255)
            .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVLINE>()
    .Property(p => p.PRATMEHES2)
        .HasColumnName(@"PRAT_MEHES2")
        .HasMaxLength(11)
        .HasColumnType("varchar2");

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
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.SELLERID)
                .HasMaxLength(36)
                .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVDOC>()
                .Property(p => p.SELLERNAME)
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
            modelBuilder.Entity<GITITEM>()
    .Property(p => p.FACTOR)
        .HasColumnType("decimal");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.VERIFICATIONNUMBER)
                    .HasColumnName(@"VERIFICATION_NUMBER")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.TARIFFID)
                    .HasColumnName(@"TARIFF_ID")
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.IMPAPPROVTYPEID)
                    .HasColumnName(@"IMP_APPROV_TYPE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.SIVUGINSTRUCTION)
                    .HasColumnName(@"SIVUG_INSTRUCTION")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.REMARKSMAKAT)
                    .HasColumnName(@"REMARKS_MAKAT")
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.REMARKSPROTEST)
                    .HasColumnName(@"REMARKS_PROTEST")
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.CLASSIFIER1ID)
                    .HasColumnName(@"CLASSIFIER1_ID")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.CLASSIFIER2ID)
                    .HasColumnName(@"CLASSIFIER2_ID")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GITITEM>()
                .Property(p => p.ITEMNO2)
                    .HasColumnName(@"ITEM_NO2")
                    .HasMaxLength(30)
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
                    .HasColumnType("int64");
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

            #region GDMQUEST

            modelBuilder.Entity<GDMQUEST>()
                .HasKey(p => new { p.COMID, p.PROCESSTYPE })
                .ToTable("GDMQUESTS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.PROCESSTYPE)
                    .HasColumnName(@"PROCESS_TYPE")
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.STATUS)
                    .HasColumnType("decimal")
                    .HasPrecision(20, 0);
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.CURRENTSTAGE)
                    .HasColumnType("decimal")
                    .HasPrecision(20, 0);
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.STARTDATE)
                    .HasColumnName(@"START_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.ENDDATE)
                    .HasColumnName(@"END_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int16");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.CANCELREQUEST)
                    .HasColumnName(@"CANCEL_REQUEST")
                    .HasColumnType("int16");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.ERRORCODE)
                    .HasColumnName(@"ERROR_CODE")
                    .HasColumnType("int");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.JOBNAME)
                    .HasColumnName(@"JOB_NAME")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.MOREDATA)
                    .HasColumnName(@"MORE_DATA")
                    .HasColumnType("clob");
            modelBuilder.Entity<GDMQUEST>()
                .Property(p => p.REMARK)
                    .HasColumnType("clob");

            #endregion

            #region CFIFILEM



            modelBuilder.Entity<CFIFILEM>()
                .HasKey(p => p.FILENO)
                .ToTable("CFIFILEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
#if reserveword
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FILEPREFIX)
                    .HasColumnName(@"FILE_PREFIX")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CUSTOMERID)
                    .HasColumnName(@"CUSTOMER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.SUPPLIERID)
                    .HasColumnName(@"SUPPLIER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CUSSUPPLIERID)
                    .HasColumnName(@"CUS_SUPPLIER_ID")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CUSTOMSBRANCHID)
                    .HasColumnName(@"CUSTOMS_BRANCH_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.USERID)
                    .HasColumnName(@"USER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.PACKTYPEID)
                    .HasColumnName(@"PACK_TYPE_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FILETYPE)
                    .HasColumnName(@"FILE_TYPE")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("double");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.VOLUME)
                    .HasColumnType("double");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.AWBCARRIER)
                    .HasColumnName(@"AWB_CARRIER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.VESSEL)
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FLIGHTNUM)
                    .HasColumnName(@"FLIGHT_NUM")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FREIGHT)
                    .HasColumnType("double");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ORIGINID)
                    .HasColumnName(@"ORIGIN_ID")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.DESTINATIONID)
                    .HasColumnName(@"DESTINATION_ID")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.MAWB)
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.HAWB)
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.HAWBDATE)
                    .HasColumnName(@"HAWB_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ARRIVALDATE)
                    .HasColumnName(@"ARRIVAL_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ISKA)
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FORWARDERID)
                    .HasColumnName(@"FORWARDER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.MANIFESTNO)
                    .HasColumnName(@"MANIFEST_NO")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.TRUCKERID)
                    .HasColumnName(@"TRUCKER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.MVZONE)
                    .HasColumnName(@"MV_ZONE")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CONTACTID)
                    .HasColumnName(@"CONTACT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.TRANSPORTATIONTYPE)
                    .HasColumnName(@"TRANSPORTATION_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.INSURANCE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.HAWBSHORT)
                    .HasColumnName(@"HAWB_SHORT")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.WTVAL)
                    .HasMaxLength(1)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.PAYMENTTERM)
                    .HasColumnName(@"PAYMENT_TERM")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.GUSH)
                    .HasMaxLength(7)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FILECLOSED)
                    .HasColumnName(@"FILE_CLOSED")
                    .HasColumnType("bool");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.STATUSDATE)
                    .HasColumnName(@"STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.LSTSTATUSID)
                    .HasColumnName(@"LST_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.LSTSTATUSDATE)
                    .HasColumnName(@"LST_STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CUSTPACK)
                    .HasColumnName(@"CUST_PACK")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CUSTPORT)
                    .HasColumnName(@"CUST_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.OLDCCFILE)
                    .HasColumnName(@"OLD_CC_FILE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ENTRYFILENO)
                    .HasColumnName(@"ENTRY_FILE_NO")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.RELEASEFILENO)
                    .HasColumnName(@"RELEASE_FILE_NO")
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.RESHIMONNO)
                    .HasColumnName(@"RESHIMON_NO")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.RESHIMONDATE)
                    .HasColumnName(@"RESHIMON_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.RESHIMONTYPE)
                    .HasColumnName(@"RESHIMON_TYPE")
                    .HasMaxLength(7)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ORIGINCOUNTRY)
                    .HasColumnName(@"ORIGIN_COUNTRY")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.COMMODITYID)
                    .HasColumnName(@"COMMODITY_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.MAWBDATE)
                    .HasColumnName(@"MAWB_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.PTERMID)
                    .HasColumnName(@"PTERM_ID")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CHARGWT)
                    .HasColumnName(@"CHARG_WT")
                    .HasColumnType("double");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ETA)
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FUCLOSE)
                    .HasColumnName(@"FU_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ACCOUNTINGCLOSE)
                    .HasColumnName(@"ACCOUNTING_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.GRANTDATE)
                    .HasColumnName(@"GRANT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.OPENBYUSER)
                    .HasColumnName(@"OPEN_BY_USER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.PROFILEID)
                    .HasColumnName(@"PROFILE_ID")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.IMPORTTYPE)
                    .HasColumnName(@"IMPORT_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FILECLASS)
                    .HasColumnName(@"FILE_CLASS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CANCELLED)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.MEDIATORID)
                    .HasColumnName(@"MEDIATOR_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.FCL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.SACREDIT)
                    .HasColumnName(@"SA_CREDIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.PROFITCLOSE)
                    .HasColumnName(@"PROFIT_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.TRACKINGNO)
                    .HasColumnName(@"TRACKING_NO")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.BANKID)
                    .HasColumnName(@"BANK_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.BUYERID)
                    .HasColumnName(@"BUYER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.IIGTYPE)
                    .HasColumnName(@"IIG_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.LOGITUDEFILE)
                    .HasColumnName(@"LOGITUDE_FILE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.CIFVALUE)
                    .HasColumnName(@"CIF_VALUE")
                    .HasColumnType("decimal")
                    .HasPrecision(18, 2);
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.TOTALTAX)
                    .HasColumnName(@"TOTAL_TAX")
                    .HasColumnType("decimal")
                    .HasPrecision(18, 2);
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.DECLARSTSCODE)
                    .HasColumnName(@"DECLAR_STS_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.PACKSFLAG)
                    .HasColumnName(@"PACKS_FLAG")
                    .HasColumnType("bool");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.INSCOMPANY)
                    .HasColumnName(@"INS_COMPANY")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.POLICYNO)
                    .HasColumnName(@"POLICY_NO")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.INSBYUS)
                    .HasColumnName(@"INS_BY_US")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.WAREHOUSEID)
                    .HasColumnName(@"WAREHOUSE_ID")
                    .HasMaxLength(10)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.UNLOADPORTID)
                    .HasColumnName(@"UNLOADPORT_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.BALDARHAWB)
                    .HasColumnName(@"BALDAR_HAWB")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.BALDARHAWBDATE)
                    .HasColumnName(@"BALDAR_HAWB_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.SHIPUSDVAL)
                    .HasColumnName(@"SHIP_USD_VAL")
                    .HasColumnType("double");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ARRIVALTIME)
                    .HasColumnName(@"ARRIVAL_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.ESTARRIVALTIME)
                    .HasColumnName(@"EST_ARRIVAL_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.LASTUPDATETIME)
                    .HasColumnName(@"LAST_UPDATE_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.OLDCUSTPACKTYPEID)
                    .HasColumnName(@"OLDCUST_PACK_TYPE_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.COUWTVAL)
                    .HasColumnName(@"COU_WTVAL")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.UNIQUECHECK)
                    .HasColumnName(@"UNIQUE_CHECK")
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.INTEGRATORID)
                    .HasColumnName(@"INTEGRATOR_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.SHOPID)
                    .HasColumnName(@"SHOP_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.LEADFILE)
                    .HasColumnName(@"LEAD_FILE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.SERVLEVELID)
                    .HasColumnName(@"SERVLEVEL_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<CFIFILEM>()
                .Property(p => p.WITHPAPER)
                    .HasColumnName(@"WITH_PAPER")
                    .HasMaxLength(1)
                    .HasColumnType("char");
#endif
            #endregion

            #region CTBFITYPE

            modelBuilder.Entity<CTBFITYPE>()
                .HasKey(p => p.TYPEID)
                .ToTable("CTBFITYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBFITYPE>()
                .Property(p => p.TYPEID)
                    .HasColumnName(@"TYPE_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBFITYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBFITYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBFITYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBFITYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GTBFUSTATU

            modelBuilder.Entity<GTBFUSTATU>()
                .HasKey(p => new { p.ENTNAME, p.STATUSCODE })
                .ToTable("GTBFUSTATUS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.ENTNAME)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.STATUSCODE)
                    .HasColumnName(@"STATUS_CODE")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.SEQUENCE)
                    .HasColumnType("int");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.CLOSEACTION)
                    .HasColumnName(@"CLOSE_ACTION")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.CLIENTSTATUS)
                    .HasColumnName(@"CLIENT_STATUS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.AGENTSTATUS)
                    .HasColumnName(@"AGENT_STATUS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.MESSFORCLIENT)
                    .HasColumnName(@"MESS_FOR_CLIENT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.MESSFORAGENT)
                    .HasColumnName(@"MESS_FOR_AGENT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.NEXTSTATUS)
                    .HasColumnName(@"NEXT_STATUS")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.OFDAYNEXTST)
                    .HasColumnName(@"OF_DAY_NEXT_ST")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.OFDAYFOLLUP)
                    .HasColumnName(@"OF_DAY_FOLL_UP")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.OFDAYACTIVEA)
                    .HasColumnName(@"OF_DAY_ACTIVE_A")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.SHOSTATUS)
                    .HasColumnName(@"SHO_STATUS")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.VERBYOWNER)
                    .HasColumnName(@"VER_BY_OWNER")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.EXCEPTION)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.STANDALONE)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.OFTIMEFOLLUP)
                    .HasColumnName(@"OF_TIME_FOLL_UP")
                    .HasColumnType("date");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.STATUSSAVE)
                    .HasColumnName(@"STATUS_SAVE")
                    .HasMaxLength(5)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.BATCHMODE)
                    .HasColumnName(@"BATCH_MODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.DEVELOPERMODE)
                    .HasColumnName(@"DEVELOPER_MODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.AUTOSTATUS)
                    .HasColumnName(@"AUTO_STATUS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.NOAPPLLOCK)
                    .HasColumnName(@"NO_APPL_LOCK")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBFUSTATU>()
                .Property(p => p.STATUSINF)
                    .HasColumnName(@"STATUS_INF")
                    .HasColumnType("long");

            #endregion

            #region GAQSTRUCTURE

            modelBuilder.Entity<GAQSTRUCTURE>()
                .HasKey(p => new { p.USERCODE, p.ENTNAME })
                .ToTable("GAQSTRUCTURE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.USERCODE)
                    .HasColumnName(@"USER_CODE")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.ENTNAME)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.ROLETYPE)
                    .HasColumnName(@"ROLE_TYPE")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.QUEUEACTIVE)
                    .HasColumnName(@"QUEUE_ACTIVE")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQSTRUCTURE>()
                .Property(p => p.QUEUEALL)
                    .HasColumnName(@"QUEUE_ALL")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GAQREMARK

            modelBuilder.Entity<GAQREMARK>()
                .HasKey(p => p.COUNTER)
                .ToTable("GAQREMARK", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQREMARK>()
                .Property(p => p.COUNTER)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<GAQREMARK>()
                .Property(p => p.REMARK)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQREMARK>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQREMARK>()
                .Property(p => p.REJECTION)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQREMARK>()
                .Property(p => p.APPROVAL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQREMARK>()
                .Property(p => p.NEWTASK)
                    .HasColumnName(@"NEW_TASK")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQREMARK>()
                .Property(p => p.SUSPEND)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GAQQLOAD

            modelBuilder.Entity<GAQQLOAD>()
                .HasKey(p => p.APPQID)
                .ToTable("GAQQLOAD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQQLOAD>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQQLOAD>()
                .Property(p => p.LOAD)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQQLOAD>()
                .Property(p => p.QCOUNT)
                    .HasColumnName(@"Q_COUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQQLOAD>()
                .Property(p => p.QTIMEH)
                    .HasColumnName(@"Q_TIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQQLOAD>()
                .Property(p => p.REMARK)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");

            #endregion

            #region GAQMERGE

            modelBuilder.Entity<GAQMERGE>()
                .HasKey(p => p.COUNTER)
                .ToTable("GAQMERGE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.COUNTER)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.RSPID)
                    .HasColumnName(@"RSP_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.CREATEDATE)
                    .HasColumnName(@"CREATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.CLOSEDATE)
                    .HasColumnName(@"CLOSE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.CREATEBY)
                    .HasColumnName(@"CREATE_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.UPDATEBY)
                    .HasColumnName(@"UPDATE_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.REFERANTID)
                    .HasColumnName(@"REFERANT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.CLIENTID)
                    .HasColumnName(@"CLIENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.STATUS)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.GROSSTIMEH)
                    .HasColumnName(@"GROSS_TIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.NETTIMEH)
                    .HasColumnName(@"NET_TIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.REMARK)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.LOAD)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.LOADREMARK)
                    .HasColumnName(@"LOAD_REMARK")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.FLAG)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.CALCDATE)
                    .HasColumnName(@"CALC_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.GDATE1)
                    .HasColumnName(@"G_DATE1")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.DATAQUEID)
                    .HasColumnName(@"DATA_QUE_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQMERGE>()
                .Property(p => p.PENDINGDATE)
                    .HasColumnName(@"PENDING_DATE")
                    .HasColumnType("date");

            #endregion

            #region GAQDLOAD

            modelBuilder.Entity<GAQDLOAD>()
                .HasKey(p => new { p.APPQID, p.DEPARTID })
                .ToTable("GAQDLOAD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQDLOAD>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDLOAD>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDLOAD>()
                .Property(p => p.LOAD)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQDLOAD>()
                .Property(p => p.QCOUNT)
                    .HasColumnName(@"Q_COUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDLOAD>()
                .Property(p => p.QTIMEH)
                    .HasColumnName(@"Q_TIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDLOAD>()
                .Property(p => p.REMARK)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");

            #endregion

            #region GAQDEF

            modelBuilder.Entity<GAQDEF>()
                .HasKey(p => p.APPQID)
                .ToTable("GAQDEF", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.CREATEDATE)
                    .HasColumnName(@"CREATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.UPDATEDATE)
                    .HasColumnName(@"UPDATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.CREATEBY)
                    .HasColumnName(@"CREATE_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.UPDATEBY)
                    .HasColumnName(@"UPDATE_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.QGCOUNT)
                    .HasColumnName(@"Q_GCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.QYCOUNT)
                    .HasColumnName(@"Q_YCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.QRCOUNT)
                    .HasColumnName(@"Q_RCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.QGTIMEH)
                    .HasColumnName(@"Q_GTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.QYTIMEH)
                    .HasColumnName(@"Q_YTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.QRTIMEH)
                    .HasColumnName(@"Q_RTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.DGCOUNT)
                    .HasColumnName(@"D_GCOUNT")
                    .HasColumnType("decimal")
                    .HasPrecision(20, 0);
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.DYCOUNT)
                    .HasColumnName(@"D_YCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.DRCOUNT)
                    .HasColumnName(@"D_RCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.DGTIMEH)
                    .HasColumnName(@"D_GTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.DYTIMEH)
                    .HasColumnName(@"D_YTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.DRTIMEH)
                    .HasColumnName(@"D_RTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.UGCOUNT)
                    .HasColumnName(@"U_GCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.UYCOUNT)
                    .HasColumnName(@"U_YCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.URCOUNT)
                    .HasColumnName(@"U_RCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.UGTIMEH)
                    .HasColumnName(@"U_GTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.UYTIMEH)
                    .HasColumnName(@"U_YTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.URTIMEH)
                    .HasColumnName(@"U_RTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.FRTIMEH)
                    .HasColumnName(@"F_RTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.FYTIMEH)
                    .HasColumnName(@"F_YTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.FGTIMEH)
                    .HasColumnName(@"F_GTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.REMARKS)
                    .HasColumnType("clob");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.SERVICENAME)
                    .HasColumnName(@"SERVICE_NAME")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.TGCOUNT)
                    .HasColumnName(@"T_GCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.TYCOUNT)
                    .HasColumnName(@"T_YCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.TRCOUNT)
                    .HasColumnName(@"T_RCOUNT")
                    .HasColumnType("int");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.TGTIMEH)
                    .HasColumnName(@"T_GTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.TYTIMEH)
                    .HasColumnName(@"T_YTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.TRTIMEH)
                    .HasColumnName(@"T_RTIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.REFERENCE)
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.SUCCESSSTATUSID)
                    .HasColumnName(@"SUCCESS_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.FAILEDSTATUSID)
                    .HasColumnName(@"FAILED_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.OPENSTATUSID)
                    .HasColumnName(@"OPEN_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.POOLUSRID)
                    .HasColumnName(@"POOL_USR_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.ROLETYPE)
                    .HasColumnName(@"ROLE_TYPE")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDEF>()
                .Property(p => p.ACTIONBUTTONNAME)
                    .HasColumnName(@"ACTION_BUTTON_NAME")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");

            #endregion

            #region GAQDATALOAD

            modelBuilder.Entity<GAQDATALOAD>()
                .HasKey(p => p.DATAQUEID)
                .ToTable("GAQDATALOAD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.DATAQUEID)
                    .HasColumnName(@"DATA_QUE_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.LOAD)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.LOADREMARK)
                    .HasColumnName(@"LOAD_REMARK")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.CALCDATE)
                    .HasColumnName(@"CALC_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.GROSSTIMEH)
                    .HasColumnName(@"GROSS_TIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.NETTIMEH)
                    .HasColumnName(@"NET_TIMEH")
                    .HasColumnType("double");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.RSPID)
                    .HasColumnName(@"RSP_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.REFERANTID)
                    .HasColumnName(@"REFERANT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.CLIENTID)
                    .HasColumnName(@"CLIENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATALOAD>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region GAQDATA

            modelBuilder.Entity<GAQDATA>()
                .HasKey(p => p.DATAQUEID)
                .ToTable("GAQDATA", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.DATAQUEID)
                    .HasColumnName(@"DATA_QUE_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.APPQID)
                    .HasColumnName(@"APPQ_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.RSPID)
                    .HasColumnName(@"RSP_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.CREATEDATE)
                    .HasColumnName(@"CREATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.CLOSEDATE)
                    .HasColumnName(@"CLOSE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.CREATEBY)
                    .HasColumnName(@"CREATE_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.UPDATEBY)
                    .HasColumnName(@"UPDATE_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.STATUS)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.REFERANTID)
                    .HasColumnName(@"REFERANT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.REMARK)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.LINECLOSE)
                    .HasColumnName(@"LINE_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.CLIENTID)
                    .HasColumnName(@"CLIENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.FLAG)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.GDATE1)
                    .HasColumnName(@"G_DATE1")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.PENDINGDATE)
                    .HasColumnName(@"PENDING_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.PENDING)
                    .HasColumnType("bool");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.CLOSETYPE)
                    .HasColumnName(@"CLOSE_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.MANUALPROCESS)
                    .HasColumnName(@"MANUAL_PROCESS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQDATA>()
                    .Property(p => p.NEWTEAMID)
                    .HasColumnName(@"NEW_TEAM_ID")
                    .HasMaxLength(9)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.PRIMARYNUMCFI)
                    .HasColumnName(@"PRIMARY_NUM_CFI")
                    .IsRequired()
                    .HasColumnType("int64");
            modelBuilder.Entity<GAQDATA>()
                .Property(p => p.BACKTOQUE)
                .HasColumnName(@"BACK_TO_QUE")
                .HasMaxLength(1)
                .HasColumnType("char");

            #endregion

            #region DWGNDCARD

            modelBuilder.Entity<DWGNDCARD>()
                .HasKey(p => p.DWCOUNTER)
                .ToTable("DW_GNDCARD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DWCOUNTER)
                    .HasColumnName(@"DW_COUNTER")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.ENVID)
                    .HasColumnName(@"ENV_ID")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.CARDID)
                    .HasColumnName(@"CARD_ID")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SHNAMEHEB)
                    .HasColumnName(@"SHNAME_HEB")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.GROUPID)
                    .HasColumnName(@"GROUP_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SHNAMEENG)
                    .HasColumnName(@"SHNAME_ENG")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.OLDCARD)
                    .HasColumnName(@"OLD_CARD")
                    .HasMaxLength(16)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLE)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLI)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLM)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLR)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLH)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLT)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLQ)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLC)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLS)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLF)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLJ)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLK)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.APPLL)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.COMPANYID)
                    .HasColumnName(@"COMPANY_ID")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.REMARKS)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.FIRSTSECTION)
                    .HasColumnName(@"FIRST_SECTION")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SECTIONS)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DWUPDATEDATE)
                    .HasColumnName(@"DW_UPDATE_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DWDELETE)
                    .HasColumnName(@"DW_DELETE")
                    .HasColumnType("int");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.ACOUNTBALANCE)
                    .HasColumnName(@"ACOUNT_BALANCE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.POSTDATEDCHECK)
                    .HasColumnName(@"POSTDATED_CHECK")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.CREDIT)
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.CURRMONTHAGING)
                    .HasColumnName(@"CURR_MONTH_AGING")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.MONTH2AGING)
                    .HasColumnName(@"MONTH2_AGING")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.MONTH3AGING)
                    .HasColumnName(@"MONTH3_AGING")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.OBLIGOCREDIT)
                    .HasColumnName(@"OBLIGO_CREDIT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.EXCESSRATE)
                    .HasColumnName(@"EXCESS_RATE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.INTERESTCREDIT)
                    .HasColumnName(@"INTEREST_CREDIT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.INTERESTPRC)
                    .HasColumnName(@"INTEREST_PRC")
                    .HasMaxLength(255)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SYSTEM)
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR1)
                    .HasColumnName(@"STR_1")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR2)
                    .HasColumnName(@"STR_2")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR3)
                    .HasColumnName(@"STR_3")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR4)
                    .HasColumnName(@"STR_4")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR5)
                    .HasColumnName(@"STR_5")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR6)
                    .HasColumnName(@"STR_6")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR7)
                    .HasColumnName(@"STR_7")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR8)
                    .HasColumnName(@"STR_8")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR9)
                    .HasColumnName(@"STR_9")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR10)
                    .HasColumnName(@"STR_10")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR11)
                    .HasColumnName(@"STR_11")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR12)
                    .HasColumnName(@"STR_12")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR13)
                    .HasColumnName(@"STR_13")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR14)
                    .HasColumnName(@"STR_14")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR15)
                    .HasColumnName(@"STR_15")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR16)
                    .HasColumnName(@"STR_16")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR17)
                    .HasColumnName(@"STR_17")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR18)
                    .HasColumnName(@"STR_18")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR19)
                    .HasColumnName(@"STR_19")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.STR20)
                    .HasColumnName(@"STR_20")
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE1)
                    .HasColumnName(@"DATE_1")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE2)
                    .HasColumnName(@"DATE_2")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE3)
                    .HasColumnName(@"DATE_3")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE4)
                    .HasColumnName(@"DATE_4")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE5)
                    .HasColumnName(@"DATE_5")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE6)
                    .HasColumnName(@"DATE_6")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE7)
                    .HasColumnName(@"DATE_7")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE8)
                    .HasColumnName(@"DATE_8")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE9)
                    .HasColumnName(@"DATE_9")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DATE10)
                    .HasColumnName(@"DATE_10")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM1)
                    .HasColumnName(@"NUM_1")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM2)
                    .HasColumnName(@"NUM_2")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM3)
                    .HasColumnName(@"NUM_3")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM4)
                    .HasColumnName(@"NUM_4")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM5)
                    .HasColumnName(@"NUM_5")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM6)
                    .HasColumnName(@"NUM_6")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM7)
                    .HasColumnName(@"NUM_7")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM8)
                    .HasColumnName(@"NUM_8")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM9)
                    .HasColumnName(@"NUM_9")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.NUM10)
                    .HasColumnName(@"NUM_10")
                    .HasColumnType("decimal");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL1)
                    .HasColumnName(@"BOOL_1")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL2)
                    .HasColumnName(@"BOOL_2")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL3)
                    .HasColumnName(@"BOOL_3")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL4)
                    .HasColumnName(@"BOOL_4")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL5)
                    .HasColumnName(@"BOOL_5")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL6)
                    .HasColumnName(@"BOOL_6")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL7)
                    .HasColumnName(@"BOOL_7")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL8)
                    .HasColumnName(@"BOOL_8")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL9)
                    .HasColumnName(@"BOOL_9")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BOOL10)
                    .HasColumnName(@"BOOL_10")
                    .HasColumnType("bool");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.RESTRICTEDI)
                    .HasColumnName(@"RESTRICTED_I")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.RESTRICTEDR)
                    .HasColumnName(@"RESTRICTED_R")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.RESTRICTEDM)
                    .HasColumnName(@"RESTRICTED_M")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.RESTRICTEDE)
                    .HasColumnName(@"RESTRICTED_E")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.RESTRICTEDC)
                    .HasColumnName(@"RESTRICTED_C")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SECTIONNAME)
                    .HasColumnName(@"SECTION_NAME")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.REFERANTE)
                    .HasColumnName(@"REFERANT_E")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.REFERANTI)
                    .HasColumnName(@"REFERANT_I")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.REFERANTC)
                    .HasColumnName(@"REFERANT_C")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.REFERANTR)
                    .HasColumnName(@"REFERANT_R")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.REFERANTM)
                    .HasColumnName(@"REFERANT_M")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SALESMANC)
                    .HasColumnName(@"SALESMAN_C")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SALESMANE)
                    .HasColumnName(@"SALESMAN_E")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SALESMANI)
                    .HasColumnName(@"SALESMAN_I")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SALESMANM)
                    .HasColumnName(@"SALESMAN_M")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SALESMANR)
                    .HasColumnName(@"SALESMAN_R")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SALESMANS)
                    .HasColumnName(@"SALESMAN_S")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasColumnType("int");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.DEFSECTIONID)
                    .HasColumnName(@"DEF_SECTION_ID")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.SECTID)
                    .HasColumnName(@"SECT_ID")
                    .HasColumnType("int64");
            modelBuilder.Entity<DWGNDCARD>()
                .Property(p => p.MULTIENV)
                    .HasColumnName(@"MULTI_ENV")
                    .HasColumnType("bool");

            #endregion

            #region CTBTEAM

            modelBuilder.Entity<CTBTEAM>()
                .HasKey(p => p.TEAMID)
                .ToTable("CTBTEAM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBTEAM>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTEAM>()
                .Property(p => p.LEADERID)
                    .HasColumnName(@"LEADER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTEAM>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTEAM>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTEAM>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBTEAM>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBTEAM>()
                .Property(p => p.APPLICATION)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion


            #region GITITEMCR

            modelBuilder.Entity<GITITEMCR>()
                .HasKey(p => new { p.COUNTER, p.REQCERT })
                .ToTable("GITITEMCR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GITITEMCR>()
                .Property(p => p.COUNTER)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal")
                    .HasPrecision(20, 0);
            modelBuilder.Entity<GITITEMCR>()
                .Property(p => p.REQCERT)
                    .HasColumnName(@"REQ_CERT")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GITITEMCR>()
                .Property(p => p.REMARKS)
                    .HasColumnType("clob");

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



            #region GTBDPTM

            modelBuilder.Entity<GTBDPTM>()
                .HasKey(p => p.DEPARTID)
                .ToTable("GTBDPTM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.TELEPHONE)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.FAX)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBDPTM>()
                .Property(p => p.SYSTEM)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region GSCUSR

            modelBuilder.Entity<GSCUSR>()
                .HasKey(p => p.USRCODE)
                .ToTable("GSCUSR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USRCODE)
                    .HasColumnName(@"USR_CODE")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USRPASS)
                    .HasColumnName(@"USR_PASS")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USRNAMEE)
                    .HasColumnName(@"USR_NAME_E")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USRNAMEH)
                    .HasColumnName(@"USR_NAME_H")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USRGRP)
                    .HasColumnName(@"USR_GRP")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USRSUPER)
                    .HasColumnName(@"USR_SUPER")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USRBRANCH)
                    .HasColumnName(@"USR_BRANCH")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.DEPARTMENT)
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USERTYPE)
                    .HasColumnName(@"USER_TYPE")
                    .HasColumnType("bool");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.ACCESSTYPE)
                    .HasColumnName(@"ACCESS_TYPE")
                    .HasColumnType("bool");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.UPDATEPASS)
                    .HasColumnName(@"UPDATE_PASS")
                    .HasColumnType("date");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USETOKEN)
                    .HasColumnName(@"USE_TOKEN")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.ACCOUNTLOCK)
                    .HasColumnName(@"ACCOUNT_LOCK")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GSCUSR>()
                .Property(p => p.USERID)
                    .HasColumnName(@"USER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region GCBSCRNVWU

            modelBuilder.Entity<GCBSCRNVWU>()
                .HasKey(p => new { p.SCREENID, p.SCREENVIEWID, p.USRCODE })
                .ToTable("GCBSCRNVWU", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GCBSCRNVWU>()
                .Property(p => p.SCREENID)
                    .HasColumnName(@"SCREEN_ID")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GCBSCRNVWU>()
                .Property(p => p.SCREENVIEWID)
                    .HasColumnName(@"SCREEN_VIEW_ID")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GCBSCRNVWU>()
                .Property(p => p.USRCODE)
                    .HasColumnName(@"USR_CODE")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GCBSCRNVWU>()
                .Property(p => p.DEFAULTLAYOUT)
                    .HasColumnName(@"DEFAULT_LAYOUT")
                    .HasColumnType("clob");

            #endregion

            #region GAQUSER

            modelBuilder.Entity<GAQUSER>()
                .HasKey(p => new { p.USERCODE, p.ENTNAME })
                .ToTable("GAQUSERS", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQUSER>()
                .Property(p => p.USERCODE)
                    .HasColumnName(@"USER_CODE")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQUSER>()
                .Property(p => p.ENTNAME)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQUSER>()
                .Property(p => p.USERLIST)
                    .HasColumnName(@"USER_LIST")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQUSER>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region GGGQC

            modelBuilder.Entity<GGGQC>()
                .HasKey(p => new { p.QUEID, p.FIELDID })
                .ToTable("GGGQC", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GGGQC>()
                .Property(p => p.QUEID)
                    .HasColumnName(@"QUE_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQC>()
                .Property(p => p.FIELDID)
                    .HasColumnName(@"FIELD_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGQC>()
                .Property(p => p.FIELDVAL)
                    .HasColumnName(@"FIELD_VAL")
                    .HasColumnType("clob");

            #endregion

            #region GAQTEAMUSR

            modelBuilder.Entity<GAQTEAMUSR>()
                .HasKey(p => new { p.TEAMID, p.USRCODE })
                .ToTable("GAQTEAMUSR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GAQTEAMUSR>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQTEAMUSR>()
                .Property(p => p.USRCODE)
                    .HasColumnName(@"USR_CODE")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");

            #endregion

            #region GGGHDAY

            modelBuilder.Entity<GGGHDAY>()
                .HasKey(p => p.HOLIDAY)
                .ToTable("GGGHDAY", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GGGHDAY>()
                .Property(p => p.HOLIDAY)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("date");
            modelBuilder.Entity<GGGHDAY>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGHDAY>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGHDAY>()
                .Property(p => p.BEGINWORK)
                    .HasColumnName(@"BEGIN_WORK")
                    .HasColumnType("date");
            modelBuilder.Entity<GGGHDAY>()
                .Property(p => p.ENDWORK)
                    .HasColumnName(@"END_WORK")
                    .HasColumnType("date");

            #endregion

            #region GTBPTYPE

            modelBuilder.Entity<GTBPTYPE>()
                .HasKey(p => new { p.APPLICATION, p.PRICETYPE })
                .ToTable("GTBPTYPE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.APPLICATION)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.PRICETYPE)
                    .HasColumnName(@"PRICE_TYPE")
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.OWNERTYPE)
                    .HasColumnName(@"OWNER_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.OWNER)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.QUOTESMANAGM)
                    .HasColumnName(@"QUOTES_MANAGM")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.TARIFFTYPE)
                    .HasColumnName(@"TARIFF_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.STEPBREAKBY)
                    .HasColumnName(@"STEP_BREAK_BY")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.CALCBREAK)
                    .HasColumnName(@"CALC_BREAK")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.STEPTYPE)
                    .HasColumnName(@"STEP_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBPTYPE>()
                .Property(p => p.TARIFFUSE)
                    .HasColumnName(@"TARIFF_USE")
                    .HasColumnType("long");

            #endregion

            #region ETBPAYTR

            modelBuilder.Entity<ETBPAYTR>()
                .HasKey(p => p.PTERMID)
                .ToTable("ETBPAYTR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.PTERMID)
                    .HasColumnName(@"PTERM_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.WTVAL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.OTHER)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPAYTR>()
                .Property(p => p.SERTYPEDEF)
                    .HasColumnName(@"SERTYPE_DEF")
                    .HasColumnType("long");

            #endregion

            #region ETBPORT

            modelBuilder.Entity<ETBPORT>()
                .HasKey(p => p.PORTID)
                .ToTable("ETBPORT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.PORTID)
                    .HasColumnName(@"PORT_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.COUNTRYID)
                    .HasColumnName(@"COUNTRY_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.COLLECTDEBIT)
                    .HasColumnName(@"COLLECT_DEBIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.LOCALCODE)
                    .HasColumnName(@"LOCAL_CODE")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.CONSALLOWED)
                    .HasColumnName(@"CONS_ALLOWED")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.AGENTID)
                    .HasColumnName(@"AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBPORT>()
                .Property(p => p.TIMEZONE)
                    .HasColumnName(@"TIME_ZONE")
                    .HasMaxLength(6)
                    .HasColumnType("char");

            #endregion

            #region ETBAIRLINE

            modelBuilder.Entity<ETBAIRLINE>()
                .HasKey(p => p.AIRLINEID)
                .ToTable("ETBAIRLINE", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.AIRLINEID)
                    .HasColumnName(@"AIRLINE_ID")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.TMPACCCARD)
                    .HasColumnName(@"TMP_ACC_CARD")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.AWBTOTPRT)
                    .HasColumnName(@"AWB_TOT_PRT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.MAINPORT)
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.AWBADRPRT3)
                    .HasColumnName(@"AWB_ADR_PRT_3")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.FILLER)
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.CHECKDIGIT)
                    .HasColumnName(@"CHECK_DIGIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.AWBADRPRT1)
                    .HasColumnName(@"AWB_ADR_PRT_1")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.AWBADRPRT2)
                    .HasColumnName(@"AWB_ADR_PRT_2")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.CONSOLIDATOR)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.AIRLINENUM)
                    .HasColumnName(@"AIRLINE_NUM")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.MINQUAN)
                    .HasColumnName(@"MIN_QUAN")
                    .HasColumnType("int");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.STACKBRDEP)
                    .HasColumnName(@"STACK_BR_DEP")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.AWBACTPRT)
                    .HasColumnName(@"AWB_ACT_PRT")
                    .HasMaxLength(16)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.PRINTRATE)
                    .HasColumnName(@"PRINT_RATE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBAIRLINE>()
                .Property(p => p.MESSAGETEXT)
                    .HasColumnName(@"MESSAGE_TEXT")
                    .HasColumnType("long");

            #endregion

            #region ITBPORT

            modelBuilder.Entity<ITBPORT>()
                .HasKey(p => p.PORTID)
                .ToTable("ITBPORT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.PORTID)
                    .HasColumnName(@"PORT_ID")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.COUNTRYID)
                    .HasColumnName(@"COUNTRY_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.AGENTID)
                    .HasColumnName(@"AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ITBPORT>()
                .Property(p => p.TIMEZONE)
                    .HasColumnName(@"TIME_ZONE")
                    .HasMaxLength(6)
                    .HasColumnType("char");

            #endregion

            #region RTBPORT

            modelBuilder.Entity<RTBPORT>()
                .HasKey(p => p.PORTID)
                .ToTable("RTBPORT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.PORTID)
                    .HasColumnName(@"PORT_ID")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.COUNTRYID)
                    .HasColumnName(@"COUNTRY_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.AGENTID)
                    .HasColumnName(@"AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<RTBPORT>()
                .Property(p => p.TIMEZONE)
                    .HasColumnName(@"TIME_ZONE")
                    .HasMaxLength(6)
                    .HasColumnType("char");

            #endregion

            #region ETBSERLV

            modelBuilder.Entity<ETBSERLV>()
                .HasKey(p => p.SERVLEVELID)
                .ToTable("ETBSERLV", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ETBSERLV>()
                .Property(p => p.SERVLEVELID)
                    .HasColumnName(@"SERVLEVEL_ID")
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBSERLV>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBSERLV>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBSERLV>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBSERLV>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBSERLV>()
                .Property(p => p.TEXTFORACCOUNT)
                    .HasColumnName(@"TEXT_FOR_ACCOUNT")
                    .HasColumnType("long");

            #endregion

            #region GTBSERLV

            modelBuilder.Entity<GTBSERLV>()
                .HasKey(p => p.SERVLEVELID)
                .ToTable("GTBSERLV", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GTBSERLV>()
                .Property(p => p.SERVLEVELID)
                    .HasColumnName(@"SERVLEVEL_ID")
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBSERLV>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBSERLV>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBSERLV>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GTBSERLV>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GTBSERLV>()
                .Property(p => p.SYSTEM)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region ETBVEND

            modelBuilder.Entity<ETBVEND>()
                .HasKey(p => p.VENDORID)
                .ToTable("ETBVEND", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.VENDORID)
                    .HasColumnName(@"VENDOR_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.ADDRESS)
                    .HasMaxLength(35)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.ISHANDAGNT)
                    .HasColumnName(@"IS_HAND_AGNT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.CITY)
                    .HasMaxLength(17)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.ADDRESS2)
                    .HasColumnName(@"ADDRESS_2")
                    .HasMaxLength(35)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.STATE)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.COUNTRY)
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.ZIPCODE)
                    .HasColumnName(@"ZIP_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.VENDORPREFIX)
                    .HasColumnName(@"VENDOR_PREFIX")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.CARDID)
                    .HasColumnName(@"CARD_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.CHECKDIGIT)
                    .HasColumnName(@"CHECK_DIGIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.PRINTRATE)
                    .HasColumnName(@"PRINT_RATE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.TMPACCOUNTNO)
                    .HasColumnName(@"TMP_ACCOUNT_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.CHARACTERS)
                    .HasMaxLength(200)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.TAM)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.TEL)
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.FAX)
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.CONTACT)
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.EMAIL)
                    .HasColumnName(@"E_MAIL")
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.BILLTO)
                    .HasColumnName(@"BILL_TO")
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.FILLERB1)
                    .HasColumnName(@"FILLER_B_1")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ETBVEND>()
                .Property(p => p.FILLERB2)
                    .HasColumnName(@"FILLER_B_2")
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region MTBCARR

            modelBuilder.Entity<MTBCARR>()
                .HasKey(p => p.AIRLINEID)
                .ToTable("MTBCARR", "AMITESTM");
            // Properties:
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.AIRLINEID)
                    .HasColumnName(@"AIRLINE_ID")
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.TMPACCCARD)
                    .HasColumnName(@"TMP_ACC_CARD")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.FWDCREDITNO)
                    .HasColumnName(@"FWD_CREDIT_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.AWBADRPRT3)
                    .HasColumnName(@"AWB_ADR_PRT_3")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.AWBADRPRT1)
                    .HasColumnName(@"AWB_ADR_PRT_1")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.AWBADRPRT2)
                    .HasColumnName(@"AWB_ADR_PRT_2")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.CONTPREFIX)
                    .HasColumnName(@"CONT_PREFIX")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.BLPRNT)
                    .HasColumnName(@"BL_PRNT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.SCAC)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.MESSAGETEXT)
                    .HasColumnName(@"MESSAGE_TEXT")
                    .HasColumnType("clob");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.BLPREFIX)
                    .HasColumnName(@"BL_PREFIX")
                    .HasColumnType("clob");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.TRANSMITYN)
                    .HasColumnName(@"TRANSMIT_YN")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBCARR>()
                .Property(p => p.VENDORID)
                    .HasColumnName(@"VENDOR_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region MTBPORT

            modelBuilder.Entity<MTBPORT>()
                .HasKey(p => p.PORTID)
                .ToTable("MTBPORT", "AMITESTM");
            // Properties:
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.PORTID)
                    .HasColumnName(@"PORT_ID")
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.BRANID)
                    .HasColumnName(@"BRAN_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.COUNTRYID)
                    .HasColumnName(@"COUNTRY_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.COLLECTDEBIT)
                    .HasColumnName(@"COLLECT_DEBIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.CONSALLOWED)
                    .HasColumnName(@"CONS_ALLOWED")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.AGENTID)
                    .HasColumnName(@"AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.NOHRBFEE)
                    .HasColumnName(@"NO_HRB_FEE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.TIMEZONE)
                    .HasColumnName(@"TIME_ZONE")
                    .HasMaxLength(6)
                    .HasColumnType("char");
            modelBuilder.Entity<MTBPORT>()
                .Property(p => p.FRWDCREDIT)
                    .HasColumnName(@"FRWD_CREDIT")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");

            #endregion

            #region EFIFILEM

            modelBuilder.Entity<EFIFILEM>()
                .HasKey(p => p.FILENO)
                .ToTable("EFIFILEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILETYPE)
                    .HasColumnName(@"FILE_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.SMP)
                    .HasMaxLength(12)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.SPEDNO)
                    .HasColumnName(@"SPED_NO")
                    .HasColumnType("int");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.SHIPPERID)
                    .HasColumnName(@"SHIPPER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.ORDERNO)
                    .HasColumnName(@"ORDER_NO")
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.IMPORTERID)
                    .HasColumnName(@"IMPORTER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.BAYERID)
                    .HasColumnName(@"BAYER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.NOTIFYID)
                    .HasColumnName(@"NOTIFY_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.BANKID)
                    .HasColumnName(@"BANK_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.LOADPORT)
                    .HasColumnName(@"LOAD_PORT")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.LOADPORTNAM)
                    .HasColumnName(@"LOAD_PORT_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FINALPORT)
                    .HasColumnName(@"FINAL_PORT")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FINALPORTNAM)
                    .HasColumnName(@"FINAL_PORT_NAM")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FLIGHTPORT)
                    .HasColumnName(@"FLIGHT_PORT")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FLIGHTPORTNAM)
                    .HasColumnName(@"FLIGHT_PORT_NAM")
                    .HasMaxLength(18)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.QUANTITYASS)
                    .HasColumnName(@"QUANTITY_ASS")
                    .HasColumnType("int");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.PRINTTYPE)
                    .HasColumnName(@"PRINT_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.PAYMENTTERM)
                    .HasColumnName(@"PAYMENT_TERM")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.SMPDATE)
                    .HasColumnName(@"SMP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.CONSIGNEE)
                    .HasColumnType("decimal")
                    .HasPrecision(20, 0);
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.CUSTOMSITEM)
                    .HasColumnName(@"CUSTOMS_ITEM")
                    .HasColumnType("int64");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.POD)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.SMPPREFIX)
                    .HasColumnName(@"SMP_PREFIX")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILECLOSE)
                    .HasColumnName(@"FILE_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FLIGHTDATE)
                    .HasColumnName(@"FLIGHT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.WEIGHTASS)
                    .HasColumnName(@"WEIGHT_ASS")
                    .HasColumnType("double");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.REQFLIGHTDATE)
                    .HasColumnName(@"REQ_FLIGHT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.SALESMAN)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.DEPARTMENT)
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.ASSEMBLYCLOSED)
                    .HasColumnName(@"ASSEMBLY_CLOSED")
                    .HasColumnType("bool");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.CONTACTID)
                    .HasColumnName(@"CONTACT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FOLUPDATE)
                    .HasColumnName(@"FOL_UP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.LSTSTATUSID)
                    .HasColumnName(@"LST_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.PROFILEID)
                    .HasColumnName(@"PROFILE_ID")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.LSTSTATUSDATE)
                    .HasColumnName(@"LST_STATUS_DATE")
                    .HasColumnType("date");
#if reserveword
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int");
#endif
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.STOPFU)
                    .HasColumnName(@"STOP_FU")
                    .HasColumnType("bool");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FHLSTATUS)
                    .HasColumnName(@"FHL_STATUS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FLIGHTTIME)
                    .HasColumnName(@"FLIGHT_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.SPLITSHIPMENT)
                    .HasColumnName(@"SPLIT_SHIPMENT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FLIGHTNO)
                    .HasColumnName(@"FLIGHT_NO")
                    .HasMaxLength(6)
                    .HasColumnType("char");
#if reserveword
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.GROUP)
                    .HasMaxLength(3)
                    .HasColumnType("char");
#endif

            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.WAREHOUSE)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.INSURANCECOMPAN)
                    .HasColumnName(@"INSURANCE_COMPAN")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.QUOTENO)
                    .HasColumnName(@"QUOTE_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.ACCOUNTINGCLOSE)
                    .HasColumnName(@"ACCOUNTING_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.MOVETYPE)
                    .HasColumnName(@"MOVE_TYPE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
#if reserveword
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.ZONE)
                    .HasMaxLength(15)
                    .HasColumnType("char");
#endif
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILECANCEL)
                    .HasColumnName(@"FILE_CANCEL")
                    .HasColumnType("bool");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.UNIT)
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.UNITVALUE)
                    .HasColumnName(@"UNIT_VALUE")
                    .HasColumnType("int");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERC1)
                    .HasColumnName(@"FILLER_C_1")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERC2)
                    .HasColumnName(@"FILLER_C_2")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERB1)
                    .HasColumnName(@"FILLER_B_1")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERB2)
                    .HasColumnName(@"FILLER_B_2")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERD1)
                    .HasColumnName(@"FILLER_D_1")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERD2)
                    .HasColumnName(@"FILLER_D_2")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERC3)
                    .HasColumnName(@"FILLER_C_3")
                    .HasMaxLength(64)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.FILLERB3)
                    .HasColumnName(@"FILLER_B_3")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.QUOTE)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIFILEM>()
                .Property(p => p.LOGBOXREF)
                    .HasColumnName(@"LOGBOX_REF")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");

#endregion

             #region ESPSPED

            modelBuilder.Entity<ESPSPED>()
                .HasKey(p => p.SPDNO)
                .ToTable("ESPSPED", "AMITESTM");
            // Properties:
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.SPDNO)
                    .HasColumnName(@"SPD_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.AWBDATE)
                    .HasColumnName(@"AWB_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.SPDTYPE)
                    .HasColumnName(@"SPD_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.MAINCARRIER)
                    .HasColumnName(@"MAIN_CARRIER")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.MAINAWB)
                    .HasColumnName(@"MAIN_AWB")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.LEADCARRIER)
                    .HasColumnName(@"LEAD_CARRIER")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.LEADAWB)
                    .HasColumnName(@"LEAD_AWB")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.SHIPPERID)
                    .HasColumnName(@"SHIPPER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.BANK)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.AGENTID)
                    .HasColumnName(@"AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.BUYERID)
                    .HasColumnName(@"BUYER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.LOADPORTID)
                    .HasColumnName(@"LOADPORT_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.DESTPORTID)
                    .HasColumnName(@"DESTPORT_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PRINTTYPE)
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.CURRID)
                    .HasColumnName(@"CURR_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.CHARGCODE)
                    .HasColumnName(@"CHARG_CODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.WTVALCODE)
                    .HasColumnName(@"WTVAL_CODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.OTHERCODE)
                    .HasColumnName(@"OTHER_CODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.CARRVAL)
                    .HasColumnName(@"CARR_VAL")
                    .HasColumnType("double");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.CUSTVAL)
                    .HasColumnName(@"CUST_VAL")
                    .HasColumnType("double");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.INSURVAL)
                    .HasColumnName(@"INSUR_VAL")
                    .HasColumnType("double");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.VALCHARGE)
                    .HasColumnName(@"VAL_CHARGE")
                    .HasColumnType("double");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.TAX)
                    .HasColumnType("double");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.DEALNO)
                    .HasColumnName(@"DEAL_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.COMMPER)
                    .HasColumnName(@"COMM_PER")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.CONSIGNID)
                    .HasColumnName(@"CONSIGN_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.LOADPORTNAM)
                    .HasColumnName(@"LOADPORT_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.DESTPORTNAM)
                    .HasColumnName(@"DESTPORT_NAM")
                    .HasMaxLength(18)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.SPEDCLOSE)
                    .HasColumnName(@"SPED_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FLIGHTDATE)
                    .HasColumnName(@"FLIGHT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.HANDAGNT)
                    .HasColumnName(@"HAND_AGNT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.WGTUNIT)
                    .HasColumnName(@"WGT_UNIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PRINTDIMS)
                    .HasColumnName(@"PRINT_DIMS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PRINTOTHER)
                    .HasColumnName(@"PRINT_OTHER")
                    .HasColumnType("bool");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.VENDORREF)
                    .HasColumnName(@"VENDOR_REF")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.SPEDACCCLOSE)
                    .HasColumnName(@"SPED_ACC_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.MANCLOSE)
                    .HasColumnName(@"MAN_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.DEPARTMENT)
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FWBSTATUS)
                    .HasColumnName(@"FWB_STATUS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FLIGHTTIME)
                    .HasColumnName(@"FLIGHT_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FLIGHTNO)
                    .HasColumnName(@"FLIGHT_NO")
                    .HasMaxLength(6)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.WAREHOUSE)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PROFITCLOSE)
                    .HasColumnName(@"PROFIT_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.ACCOUNTINGCLOSE)
                    .HasColumnName(@"ACCOUNTING_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.ISSUINGCARRIERAGENT)
                    .HasColumnName(@"ISSUING_CARRIER_AGENT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FOLUPDATE)
                    .HasColumnName(@"FOL_UP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.LSTSTATUSID)
                    .HasColumnName(@"LST_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.LSTSTATUSDATE)
                    .HasColumnName(@"LST_STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PROFILEID)
                    .HasColumnName(@"PROFILE_ID")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.STOPFU)
                    .HasColumnName(@"STOP_FU")
                    .HasColumnType("bool");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.BOOKSTATUS)
                    .HasColumnName(@"BOOK_STATUS")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.COPYDATE)
                    .HasColumnName(@"COPY_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.EFREIGHTTYPE)
                    .HasColumnName(@"E_FREIGHT_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PCLSWOSTM)
                    .HasColumnName(@"PCLS_WO_STM")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERC1)
                    .HasColumnName(@"FILLER_C_1")
                    .HasMaxLength(40)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERC2)
                    .HasColumnName(@"FILLER_C_2")
                    .HasMaxLength(40)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERD1)
                    .HasColumnName(@"FILLER_D_1")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERD2)
                    .HasColumnName(@"FILLER_D_2")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERN1)
                    .HasColumnName(@"FILLER_N_1")
                    .HasColumnType("double");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERN2)
                    .HasColumnName(@"FILLER_N_2")
                    .HasColumnType("double");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERC3)
                    .HasColumnName(@"FILLER_C_3")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERC4)
                    .HasColumnName(@"FILLER_C_4")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERB1)
                    .HasColumnName(@"FILLER_B_1")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.FILLERB2)
                    .HasColumnName(@"FILLER_B_2")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PRIORITYACI)
                    .HasColumnName(@"PRIORITY_ACI")
                    .HasMaxLength(25)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PROFITCLSDATE)
                    .HasColumnName(@"PROFIT_CLS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<ESPSPED>()
                .Property(p => p.PROFITCLSTIME)
                    .HasColumnName(@"PROFIT_CLS_TIME")
                    .HasColumnType("date");

            #endregion

            #region GITITEMAP

            modelBuilder.Entity<GITITEMAP>()
                .HasKey(p => new { p.COUNTER, p.APPROVTYPEID })
                .ToTable("GITITEMAP", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GITITEMAP>()
                .Property(p => p.COUNTER)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal")
                    .HasPrecision(20, 0);
            modelBuilder.Entity<GITITEMAP>()
                .Property(p => p.APPROVTYPEID)
                    .HasColumnName(@"APPROV_TYPE_ID")
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GITITEMAP>()
                .Property(p => p.REMARKS)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");

            #endregion

            #region MSPSPED

            modelBuilder.Entity<MSPSPED>()
                .HasKey(p => p.SPDNO)
                .ToTable("MSPSPED", "AMITESTM");
            // Properties:
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SPDNO)
                    .HasColumnName(@"SPD_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.TRANSPORTTYPE)
                    .HasColumnName(@"TRANSPORT_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.AWBDATE)
                    .HasColumnName(@"AWB_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SPDTYPE)
                    .HasColumnName(@"SPD_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.MAINCARRIER)
                    .HasColumnName(@"MAIN_CARRIER")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SHIPPINGAGENTID)
                    .HasColumnName(@"SHIPPING_AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.MAINAWB)
                    .HasColumnName(@"MAIN_AWB")
                    .HasMaxLength(20)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.AGENTID)
                    .HasColumnName(@"AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.LOADPORTID)
                    .HasColumnName(@"LOADPORT_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.DESTPORTID)
                    .HasColumnName(@"DESTPORT_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FINALPORT)
                    .HasColumnName(@"FINAL_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FINALPORTNAM)
                    .HasColumnName(@"FINAL_PORT_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.CURRID)
                    .HasColumnName(@"CURR_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.WTVALCODE)
                    .HasColumnName(@"WTVAL_CODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.OTHERCODE)
                    .HasColumnName(@"OTHER_CODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SPEDCLOSE)
                    .HasColumnName(@"SPED_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FLIGHTDATE)
                    .HasColumnName(@"FLIGHT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.HANDAGNT)
                    .HasColumnName(@"HAND_AGNT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.VENDORREF)
                    .HasColumnName(@"VENDOR_REF")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.WGTUNIT)
                    .HasColumnName(@"WGT_UNIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.MOVETYPE)
                    .HasColumnName(@"MOVE_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SPDSUBTYPE)
                    .HasColumnName(@"SPD_SUBTYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.VESSELCODE)
                    .HasColumnName(@"VESSEL_CODE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.ALLIN)
                    .HasColumnName(@"ALL_IN")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.NOTIFYID)
                    .HasColumnName(@"NOTIFY_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.BANKID)
                    .HasColumnName(@"BANK_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SHIPPERID)
                    .HasColumnName(@"SHIPPER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.HAZARDID)
                    .HasColumnName(@"HAZARD_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.VOLUNIT)
                    .HasColumnName(@"VOL_UNIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.MANCLOSE)
                    .HasColumnName(@"MAN_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.BOOKID)
                    .HasColumnName(@"BOOK_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.DEPARTMENT)
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.VOYAGENO)
                    .HasColumnName(@"VOYAGE_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.TRANSHIPPORT)
                    .HasColumnName(@"TRANSHIP_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.TRANSHIPPORT1)
                    .HasColumnName(@"TRANSHIP_PORT_1")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.CONTINPORT)
                    .HasColumnName(@"CONT_IN_PORT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.ETSDATE)
                    .HasColumnName(@"ETS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.MANIFESTNO)
                    .HasColumnName(@"MANIFEST_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SERVICEID)
                    .HasColumnName(@"SERVICE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.VOYAGEID)
                    .HasColumnName(@"VOYAGE_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.TALLY)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.ACCOUNTINGCLOSE)
                    .HasColumnName(@"ACCOUNTING_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.OPENCLOSEAC)
                    .HasColumnName(@"OPEN_CLOSE_AC")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.RANAR)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.CARGOTYPE)
                    .HasColumnName(@"CARGO_TYPE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.STOPFU)
                    .HasColumnName(@"STOP_FU")
                    .HasColumnType("bool");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.LSTSTATUSID)
                    .HasColumnName(@"LST_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.LSTSTATUSDATE)
                    .HasColumnName(@"LST_STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.STATUSDATE)
                    .HasColumnName(@"STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.RECAPID)
                    .HasColumnName(@"RECAP_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.ETSTIME)
                    .HasColumnName(@"ETS_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.VOYAGEYEAR)
                    .HasColumnName(@"VOYAGE_YEAR")
                    .HasColumnType("int");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FLIGHTTIME)
                    .HasColumnName(@"FLIGHT_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SPDCANCEL)
                    .HasColumnName(@"SPD_CANCEL")
                    .HasColumnType("bool");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.INLANDMODE)
                    .HasColumnName(@"INLAND_MODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.TRUCKTYPE)
                    .HasColumnName(@"TRUCK_TYPE")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.TRUCKNO)
                    .HasColumnName(@"TRUCK_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.PROFITCLOSE)
                    .HasColumnName(@"PROFIT_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SHIPPERSTUFFYN)
                    .HasColumnName(@"SHIPPER_STUFF_YN")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.CARRIERVOYAGENO)
                    .HasColumnName(@"CARRIER_VOYAGE_NO")
                    .HasMaxLength(20)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.PAYMENTTERM)
                    .HasColumnName(@"PAYMENT_TERM")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FILLERC1)
                    .HasColumnName(@"FILLER_C_1")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FILLERC2)
                    .HasColumnName(@"FILLER_C_2")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FILLERB1)
                    .HasColumnName(@"FILLER_B_1")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FILLERB2)
                    .HasColumnName(@"FILLER_B_2")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.PROFITCLSDATE)
                    .HasColumnName(@"PROFIT_CLS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.PROFITCLSTIME)
                    .HasColumnName(@"PROFIT_CLS_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FILLERD1)
                    .HasColumnName(@"FILLER_D_1")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.FILLERD2)
                    .HasColumnName(@"FILLER_D_2")
                    .HasColumnType("date");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.CONTNO)
                    .HasColumnName(@"CONT_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.SEAL1)
                    .HasColumnName(@"SEAL_1")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MSPSPED>()
                .Property(p => p.PACKTYPE)
                    .HasColumnName(@"PACK_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("varchar2");

            #endregion

            #region MFIFILEM

            modelBuilder.Entity<MFIFILEM>()
                .HasKey(p => p.FILENO)
                .ToTable("MFIFILEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<MFIFILEM>()
          
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRANSPORTTYPE)
                    .HasColumnName(@"TRANSPORT_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILETYPE)
                    .HasColumnName(@"FILE_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SMP)
                    .HasMaxLength(20)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SPEDNO)
                    .HasColumnName(@"SPED_NO")
                    .HasColumnType("int");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SHIPPERID)
                    .HasColumnName(@"SHIPPER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.IMPORTERID)
                    .HasColumnName(@"IMPORTER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.NOTIFYID)
                    .HasColumnName(@"NOTIFY_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.NOTIFY2)
                    .HasColumnName(@"NOTIFY_2")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.NOTIFY3)
                    .HasColumnName(@"NOTIFY_3")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.BANKID)
                    .HasColumnName(@"BANK_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.LOADPORT)
                    .HasColumnName(@"LOAD_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.LOADPORTNAM)
                    .HasColumnName(@"LOAD_PORT_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FINALPORT)
                    .HasColumnName(@"FINAL_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FINALPORTNAM)
                    .HasColumnName(@"FINAL_PORT_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FLIGHTPORT)
                    .HasColumnName(@"FLIGHT_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FLIGHTPORTNAM)
                    .HasColumnName(@"FLIGHT_PORT_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.PAYMENTTERM)
                    .HasColumnName(@"PAYMENT_TERM")
                    .HasMaxLength(3)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SMPDATE)
                    .HasColumnName(@"SMP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.POD)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.INLANDYN)
                    .HasColumnName(@"INLAND_YN")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILECLOSE)
                    .HasColumnName(@"FILE_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FLIGHTDATE)
                    .HasColumnName(@"FLIGHT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.REQFLIGHTDATE)
                    .HasColumnName(@"REQ_FLIGHT_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.MOVETYPE)
                    .HasColumnName(@"MOVE_TYPE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.ORIGION)
                    .HasMaxLength(17)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.ORIGIONCODE)
                    .HasColumnName(@"ORIGION_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.VESSELCODE)
                    .HasColumnName(@"VESSEL_CODE")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.AGENTID)
                    .HasColumnName(@"AGENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.HAZARD)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.DEPARTMENT)
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.CONTACTID)
                    .HasColumnName(@"CONTACT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SALESMAN)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.CONTACTPERSON)
                    .HasColumnName(@"CONTACT_PERSON")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.VOYAGENO)
                    .HasColumnName(@"VOYAGE_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRANSHIPPORT)
                    .HasColumnName(@"TRANSHIP_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRANSHIPPORT1)
                    .HasColumnName(@"TRANSHIP_PORT_1")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.EXPDDATE)
                    .HasColumnName(@"EXPD_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.EXPADATE)
                    .HasColumnName(@"EXPA_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FOLUPDATE)
                    .HasColumnName(@"FOL_UP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.LSTSTATUSID)
                    .HasColumnName(@"LST_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.PROFILEID)
                    .HasColumnName(@"PROFILE_ID")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.LSTSTATUSDATE)
                    .HasColumnName(@"LST_STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.USERID)
                    .HasColumnName(@"USER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.EXCEPTIONYN)
                    .HasColumnName(@"EXCEPTION_YN")
                    .HasColumnType("bool");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.STOPFU)
                    .HasColumnName(@"STOP_FU")
                    .HasColumnType("bool");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.UNSTUFFING)
                    .HasColumnType("bool");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILECANCEL)
                    .HasColumnName(@"FILE_CANCEL")
                    .HasColumnType("bool");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SERVICEID)
                    .HasColumnName(@"SERVICE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.CLIENTID)
                    .HasColumnName(@"CLIENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.BOOKID)
                    .HasColumnName(@"BOOK_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.ACCOUNTINGCLOSE)
                    .HasColumnName(@"ACCOUNTING_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.CLIENTCOMMISSION)
                    .HasColumnName(@"CLIENT_COMMISSION")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TARIFFREF)
                    .HasColumnName(@"TARIFF_REF")
                    .HasMaxLength(20)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SERVICEMODE)
                    .HasColumnName(@"SERVICE_MODE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TARIFFOWNER)
                    .HasColumnName(@"TARIFF_OWNER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.COMMOWNER)
                    .HasColumnName(@"COMM_OWNER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.LINE)
                    .HasMaxLength(5)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRANSPORTNAM)
                    .HasColumnName(@"TRANS_PORT_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRANSPORT1NAM)
                    .HasColumnName(@"TRANS_PORT_1_NAM")
                    .HasMaxLength(36)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.NOTEXPORTERID)
                    .HasColumnName(@"NOTEXPORTER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.ORIGINPRINTHBL)
                    .HasColumnName(@"ORIGIN_PRINT_HBL")
                    .HasColumnType("int");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.INLANDMODE)
                    .HasColumnName(@"INLAND_MODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRUCKTYPE)
                    .HasColumnName(@"TRUCK_TYPE")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRUCKNO)
                    .HasColumnName(@"TRUCK_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.SWITCHPORT)
                    .HasColumnName(@"SWITCH_PORT")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.BILLTOPARTNER)
                    .HasColumnName(@"BILLTO_PARTNER")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.QUOTENO)
                    .HasColumnName(@"QUOTE_NO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.INSURANCECOMPAN)
                    .HasColumnName(@"INSURANCE_COMPAN")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.BILLTO)
                    .HasColumnName(@"BILL_TO")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.UNIT)
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.UNITVALUE)
                    .HasColumnName(@"UNIT_VALUE")
                    .HasColumnType("int");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILLERC1)
                    .HasColumnName(@"FILLER_C_1")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILLERC2)
                    .HasColumnName(@"FILLER_C_2")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILLERB1)
                    .HasColumnName(@"FILLER_B_1")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILLERB2)
                    .HasColumnName(@"FILLER_B_2")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILLERD1)
                    .HasColumnName(@"FILLER_D_1")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.FILLERD2)
                    .HasColumnName(@"FILLER_D_2")
                    .HasColumnType("date");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.QUOTE)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.ISFMODE)
                    .HasColumnName(@"ISF_MODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.ISFNUMBER)
                    .HasColumnName(@"ISF_NUMBER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.NONEEDCUSTOMS)
                    .HasColumnName(@"NO_NEED_CUSTOMS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.OPERATIONREFERANT)
                    .HasColumnName(@"OPERATION_REFERANT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.TRACINGREFERANT)
                    .HasColumnName(@"TRACING_REFERANT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.ISFTRANSACTIONNO)
                    .HasColumnName(@"ISF_TRANSACTION_NO")
                    .HasMaxLength(40)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<MFIFILEM>()
                .Property(p => p.LOGBOXREF)
                    .HasColumnName(@"LOGBOX_REF")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");

            #endregion

            #region CFIMSVSTATL

            modelBuilder.Entity<CFIMSVSTATL>()
                .HasKey(p => p.GUID)
                .ToTable("CFIMSVSTATL", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.GUID)
                    .IsRequired()
                    .HasMaxLength(36)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasColumnType("int64");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.COMID)
                    .HasColumnName(@"COM_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.CREATEDATE)
                    .HasColumnName(@"CREATE_DATE")
                    .IsRequired()
                    .HasColumnType("date");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.PAGENUM)
                    .HasColumnName(@"PAGE_NUM")
                    .IsRequired()
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.LINENUM)
                    .HasColumnName(@"LINE_NUM")
                    .IsRequired()
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.LINECOUNTER)
                    .HasColumnName(@"LINE_COUNTER")
                    .HasColumnType("int");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.TARIFFCODE)
                    .HasColumnName(@"TARIFF_CODE")
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.CUSTOMSSUPPLIERID)
                    .HasColumnName(@"CUSTOMS_SUPPLIER_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.CUSTOMERID)
                    .HasColumnName(@"CUSTOMER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                 .Property(p => p.CATALOGID)
                     .HasColumnName(@"CATALOG_ID")
                     .HasMaxLength(128)
                     .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.CATALOGNAME)
                    .HasColumnName(@"CATALOG_NAME")
                    .HasMaxLength(128)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.ITEMPRICE)
                    .HasColumnName(@"ITEM_PRICE")
                    .HasColumnType("decimal")
                    .HasPrecision(19, 4);
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.OCRQUANTITY)
                    .HasColumnName(@"OCR_QUANTITY")
                    .HasColumnType("double");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.OCRQUANTITYTYPE)
                    .HasColumnName(@"OCR_QUANTITY_TYPE")
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.AMOUNT)
                    .HasColumnType("double");
            modelBuilder.Entity<CFIMSVSTATL>()
                .Property(p => p.ORIGINID)
                    .HasColumnName(@"ORIGIN_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region EFIMMN


            modelBuilder.Entity<EFIMMN>()
                .HasKey(p => new { p.FILENO, p.STORGENO })
                .ToTable("EFIMMN", "AMITESTM");
            // Properties:
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.DESOFGOOD)
                    .HasColumnName(@"DES_OF_GOOD")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.DIRECTLOAD)
                    .HasColumnName(@"DIRECT_LOAD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                 .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int64");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.FINALPORT)
                    .HasColumnName(@"FINAL_PORT")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.STORGENO)
                    .HasColumnName(@"STORGE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.MAWBINPORT)
                    .HasColumnName(@"MAWB_INPORT")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.MEHESOK)
                    .HasColumnName(@"MEHES_OK")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.MMNDATE)
                    .HasColumnName(@"MMN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.MMNTIME)
                    .HasColumnName(@"MMN_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.MZAR)
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.PACKQNTY)
                    .HasColumnName(@"PACK_QNTY")
                    .HasColumnType("int");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.PACKTYPE)
                    .HasColumnName(@"PACK_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.PART)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.REMARK)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.SHTONTYPE)
                    .HasColumnName(@"SHTON_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.SMPINPORT)
                    .HasColumnName(@"SMP_INPORT")
                    .HasMaxLength(8)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.SPECIALSTORAGE)
                    .HasColumnName(@"SPECIAL_STORAGE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.STATUS)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.VALDOLAR)
                    .HasColumnName(@"VAL_DOLAR")
                    .HasColumnType("double");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.WGTKG)
                    .HasColumnName(@"WGT_KG")
                    .HasColumnType("double");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.YEZUAN)
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.YEZUANNAME)
                    .HasColumnName(@"YEZUAN_NAME")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.TYPEOFGOOD)
                    .HasColumnName(@"TYPE_OF_GOOD")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.DNGGOODS)
                    .HasColumnName(@"DNG_GOODS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.WAREHOUSE)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.DRIVERNAME)
                    .HasColumnName(@"DRIVER_NAME")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<EFIMMN>()
                .Property(p => p.BARCODE)
                    .HasMaxLength(9)
                    .HasColumnType("varchar2");

            #endregion

            #region CTBCARMOD

            modelBuilder.Entity<CTBCARMOD>()
                .HasKey(p => new { p.CUSTOMERID, p.CARMODEL })
                .ToTable("CTBCARMOD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.CUSTOMERID)
                    .HasColumnName(@"CUSTOMER_ID")
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.CARMODEL)
                    .HasColumnName(@"CAR_MODEL")
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.NAME)
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(70)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.FOB)
                    .HasColumnType("double");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.FREIGHTAMOUNT)
                    .HasColumnName(@"FREIGHT_AMOUNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.FREIGHTCURR)
                    .HasColumnName(@"FREIGHT_CURR")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.INSPERC)
                    .HasColumnName(@"INS_PERC")
                    .HasColumnType("double");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.PRAT)
                    .HasMaxLength(11)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("int");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.ENGINEVOL)
                    .HasColumnName(@"ENGINE_VOL")
                    .HasColumnType("int");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.EXPAMOUNT)
                    .HasColumnName(@"EXP_AMOUNT")
                    .HasColumnType("double");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.EXPCURR)
                    .HasColumnName(@"EXP_CURR")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.COMMERCIAL)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.TARIFF)
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.ABS)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.ABG)
                    .HasColumnType("int");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.EPS)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.PORTFEENIS)
                    .HasColumnName(@"PORT_FEE_NIS")
                    .HasColumnType("double");
            modelBuilder.Entity<CTBCARMOD>()
                .Property(p => p.MOTORCYCLE)
                    .HasMaxLength(1)
                    .HasColumnType("char");

            #endregion

            #region LFIFILEM

            modelBuilder.Entity<LFIFILEM>()
                .HasKey(p => p.DELIVERYNO)
                .ToTable("LFIFILEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DELIVERYNO)
                    .HasColumnName(@"DELIVERY_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("int");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(30)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DELIVERYTYPE)
                    .HasColumnName(@"DELIVERY_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CLIENTID)
                    .HasColumnName(@"CLIENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CARRIERID)
                    .HasColumnName(@"CARRIER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CONTACTNAME)
                    .HasColumnName(@"CONTACT_NAME")
                    .HasMaxLength(150)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.PHONENO)
                    .HasColumnName(@"PHONE_NO")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.EMAIL)
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CLIENTDEBIT)
                    .HasColumnName(@"CLIENT_DEBIT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.WAREHOUSEFROM)
                    .HasColumnName(@"WAREHOUSE_FROM")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.WAREHOUSETO)
                    .HasColumnName(@"WAREHOUSE_TO")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ZONEFROM)
                    .HasColumnName(@"ZONE_FROM")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ZONETO)
                    .HasColumnName(@"ZONE_TO")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ADDRESSFROM)
                    .HasColumnName(@"ADDRESS_FROM")
                    .HasMaxLength(250)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ADDRESSTO)
                    .HasColumnName(@"ADDRESS_TO")
                    .HasMaxLength(250)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LASTDATE)
                    .HasColumnName(@"LAST_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.PACKTYPEID)
                    .HasColumnName(@"PACKTYPE_ID")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("int64");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("double");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.VOLUME)
                    .HasColumnType("double");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CHARGWT)
                    .HasColumnName(@"CHARG_WT")
                    .HasColumnType("double");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.RATIO)
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DRIVERID)
                    .HasColumnName(@"DRIVER_ID")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.TRUCKID)
                    .HasColumnName(@"TRUCK_ID")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.TRUCKTYPE)
                    .HasColumnName(@"TRUCK_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.FILECLOSE)
                    .HasColumnName(@"FILE_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.FUCLOSE)
                    .HasColumnName(@"FU_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ACCOUNTINGCLOSE)
                    .HasColumnName(@"ACCOUNTING_CLOSE")
                    .HasColumnType("bool");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.STATUSID)
                    .HasColumnName(@"STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.STATUSDATE)
                    .HasColumnName(@"STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LSTSTATUSID)
                    .HasColumnName(@"LST_STATUS_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LSTSTATUSDATE)
                    .HasColumnName(@"LST_STATUS_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.FOLUPDATE)
                    .HasColumnName(@"FOL_UP_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.OPENBYUSER)
                    .HasColumnName(@"OPEN_BY_USER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.BRANCHID)
                    .HasColumnName(@"BRANCH_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.OPENDATE)
                    .HasColumnName(@"OPEN_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DEPARTID)
                    .HasColumnName(@"DEPART_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LASTTIME)
                    .HasColumnName(@"LAST_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LOADDATEFROM)
                    .HasColumnName(@"LOAD_DATE_FROM")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LOADDATETO)
                    .HasColumnName(@"LOAD_DATE_TO")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LOADTIMEFROM)
                    .HasColumnName(@"LOAD_TIME_FROM")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.LOADTIMETO)
                    .HasColumnName(@"LOAD_TIME_TO")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DESTDATEFROM)
                    .HasColumnName(@"DEST_DATE_FROM")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DESTDATETO)
                    .HasColumnName(@"DEST_DATE_TO")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DESTTIMEFROM)
                    .HasColumnName(@"DEST_TIME_FROM")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DESTTIMETO)
                    .HasColumnName(@"DEST_TIME_TO")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CITYFROM)
                    .HasColumnName(@"CITY_FROM")
                    .HasMaxLength(7)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CITYTO)
                    .HasColumnName(@"CITY_TO")
                    .HasMaxLength(7)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CHARACTERS)
                    .HasMaxLength(200)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CHARGWTCAR)
                    .HasColumnName(@"CHARG_WT_CAR")
                    .HasColumnType("double");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.RATIOCAR)
                    .HasColumnName(@"RATIO_CAR")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.PODDATE)
                    .HasColumnName(@"POD_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.PODTIME)
                    .HasColumnName(@"POD_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.REMARKS)
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.RECEIVERNAME)
                    .HasColumnName(@"RECEIVER_NAME")
                    .HasMaxLength(50)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CARGOTYPE)
                    .HasColumnName(@"CARGO_TYPE")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.COMMODITYID)
                    .HasColumnName(@"COMMODITY_ID")
                    .HasMaxLength(5)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CLASSNO)
                    .HasColumnName(@"CLASS_NO")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.UNNO)
                    .HasColumnName(@"UN_NO")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.PCKGROUP)
                    .HasColumnName(@"PCK_GROUP")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DELIVERYREF)
                    .HasColumnName(@"DELIVERY_REF")
                    .HasMaxLength(6)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CUSTOMSREF)
                    .HasColumnName(@"CUSTOMS_REF")
                    .HasMaxLength(35)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.TRASNPORTBY)
                    .HasColumnName(@"TRASNPORT_BY")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.FROMAREA)
                    .HasColumnName(@"FROM_AREA")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.FROMITUR)
                    .HasColumnName(@"FROM_ITUR")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.FORWARDREF)
                    .HasColumnName(@"FORWARD_REF")
                    .HasMaxLength(16)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DLVRYINPRGRS)
                    .HasColumnName(@"DLVRY_IN_PRGRS")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.CUSTOMSAGNTCODE)
                    .HasColumnName(@"CUSTOMS_AGNT_CODE")
                    .HasMaxLength(4)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.GATEPASSNO)
                    .HasColumnName(@"GATEPASS_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ORIGINALMODEOFTRANSP)
                    .HasColumnName(@"ORIGINAL_MODE_OF_TRANSP")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.DRIVERDOC)
                    .HasColumnName(@"DRIVER_DOC")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.TRACKNO)
                    .HasColumnName(@"TRACK_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.TRACKADDNO)
                    .HasColumnName(@"TRACK_ADD_NO")
                    .HasMaxLength(10)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ARRDATE)
                    .HasColumnName(@"ARR_DATE")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.ARRTIME)
                    .HasColumnName(@"ARR_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.INCTRANSPDET)
                    .HasColumnName(@"INC_TRANSP_DET")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.WTVALCODE)
                    .HasColumnName(@"WTVAL_CODE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.SENDPORT)
                    .HasColumnName(@"SEND_PORT")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<LFIFILEM>()
                .Property(p => p.QUOTE)
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");

            #endregion

            #region GAQTEAM

            modelBuilder.Entity<GAQTEAM>()
                .HasKey(p => p.TEAMID)
                .ToTable("GAQTEAM", "V5111");
            // Properties:
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.TEAMID)
                    .HasColumnName(@"TEAM_ID")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.NAMEHEB)
                    .HasColumnName(@"NAME_HEB")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.NAMEENG)
                    .HasColumnName(@"NAME_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.ROLEID)
                    .HasColumnName(@"ROLE_ID")
                    .HasMaxLength(9)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.TEAMLEADER)
                    .HasColumnName(@"TEAM_LEADER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.BLOCKRECORD)
                    .HasColumnName(@"BLOCK_RECORD")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GAQTEAM>()
                .Property(p => p.SEARCHENG)
                    .HasColumnName(@"SEARCH_ENG")
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");

            #endregion

            #region GGGEXTSRVACT

            modelBuilder.Entity<GGGEXTSRVACT>()
                .HasKey(p => new { p.EXTSRVID, p.INST })
                .ToTable("GGGEXTSRVACTs", "V5122");
            // Properties:
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.EXTSRVID)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.INST)
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("bool");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PORT)
                    .HasMaxLength(8)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.SERVER)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.REFERENCE)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.WINDOWSUSER)
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.BASEPATH)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM1DESC)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM1VALUE)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM2DESC)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM2VALUE)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM3DESC)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM3VALUE)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM4DESC)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM4VALUE)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM5DESC)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM5VALUE)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.BLOCKRECORD)
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.BASELOGPATH1)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.BASELOGPATH2)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.CONFIGURATIONPARAMS)
                    .HasMaxLength(1024)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.ENVID)
                    .HasMaxLength(30)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.WINDOWSDOMAIN)
                    .HasMaxLength(256)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM6DESC)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM7DESC)
                    .HasMaxLength(32)
                    .HasColumnType("varchar2");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM6VALUE)
                    .HasColumnType("clob");
            modelBuilder.Entity<GGGEXTSRVACT>()
                .Property(p => p.PARAM7VALUE)
                    .HasColumnType("clob");

            #endregion



            #region Disabled conventions


            #endregion
            return modelBuilder;///base.OnModelCreating(modelBuilder);
        }
        public static DbModelBuilder GetBuilderToSql()//protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            DbModelBuilder modelBuilder = new DbModelBuilder(DbModelBuilderVersion.V4_1);

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
                    .HasColumnType("decimal");
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
                    .HasColumnType("bit");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.CURRENCYID)
                    .HasColumnName(@"CURRENCY_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.VALUE)
                    .HasColumnType("float");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.COMMISSION)
                    .HasColumnType("float");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.DECLARATIONNO)
                    .HasColumnName(@"DECLARATION_NO")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.CHANGINGVALUE)
                    .HasColumnName(@"CHANGING_VALUE")
                    .HasColumnType("float");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.SUPPLIERACCOUNTN)
                    .HasColumnName(@"SUPPLIER_ACCOUNT_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.COMMISSIONPERCENT)
                    .HasColumnName(@"COMMISSION_PERCENT")
                    .HasColumnType("float");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.INCOTERMIDN)
                .HasColumnName(@"INCOTERM_ID_N")
                .HasMaxLength(35)
                .HasColumnType("varchar");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.COUNTRYIDN)
                    .HasColumnName(@"COUNTRY_ID_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUACCSUP>()
                .Property(p => p.CURRENCYIDN)
                    .HasColumnName(@"CURRENCY_ID_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");

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
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.REF)
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.DEPENDENCYREF)
                    .HasColumnName(@"DEPENDENCY_REF")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.DONEOPERATION)
                    .HasColumnName(@"DONE_OPERATION")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.STATUS)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnType("varchar");
#if reserveword
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.DESC)
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
#endif
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.EXPTASKTIME)
                    .HasColumnName(@"EXP_TASK_TIME")
                    .HasColumnType("decimal");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.TRY)
                    .HasColumnType("int");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("int");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.ENTNAME)
                    .HasMaxLength(32)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.LOGLEVEL)
                    .HasColumnName(@"LOG_LEVEL")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.SECNUMBER)
                    .HasColumnName(@"SEC_NUMBER")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.PROCESSID)
                    .HasColumnName(@"PROCESS_ID")
                    .HasColumnType("decimal");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.FORMID)
                    .HasColumnName(@"FORM_ID")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.COMPUTERID)
                    .HasColumnName(@"COMPUTER_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING1)
                    .HasMaxLength(255)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING2)
                    .HasMaxLength(255)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING3)
                    .HasMaxLength(255)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING4)
                    .HasMaxLength(255)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.QUEUEMANAGEMENT)
                    .HasColumnName(@"QUEUE_MANAGEMENT")
                    .HasColumnType("bit");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.OTHERASNFILE)
                    .HasColumnName(@"OTHER_ASN_FILE")
                    .HasColumnType("bit");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.STOPPEDBYSM)
                    .HasColumnName(@"STOPPED_BY_SM")
                    .HasColumnType("bit");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING5)
                    .HasMaxLength(255)
                    .HasColumnType("varchar");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.GSTRING6)
                    .HasMaxLength(255)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.POSTSUCCESS)
                    .HasColumnName(@"POST_SUCCESS")
                    .HasMaxLength(16)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQ>()
                .Property(p => p.FAILED)
                    .HasColumnType("int");

            #endregion


            #region GGGQC

            modelBuilder.Entity<GGGQC>()
                .HasKey(p => new { p.QUEID, p.FIELDID })
                .ToTable("GGGQC", "AMITESTM");
            // Properties:
            modelBuilder.Entity<GGGQC>()
                .Property(p => p.QUEID)
                    .HasColumnName(@"QUE_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("char");
            modelBuilder.Entity<GGGQC>()
                .Property(p => p.FIELDID)
                    .HasColumnName(@"FIELD_ID")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("varchar");
            modelBuilder.Entity<GGGQC>()
                .Property(p => p.FIELDVAL)
                    .HasColumnName(@"FIELD_VAL")
                    .HasColumnType("varchar(max)");

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
                    .HasColumnType("bigint");
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
                    .HasColumnType("decimal");
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
                    .HasColumnType("DateTime");
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
                    .HasColumnType("nvarchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.DESCOFGOODS2)
                    .HasColumnName(@"DESC_OF_GOODS2")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.DESCOFGOODS3)
                    .HasColumnName(@"DESC_OF_GOODS3")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.CARRIERID)
                    .HasColumnName(@"CARRIER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.UNLOADPORTID)
                    .HasColumnName(@"UNLOADPORT_ID")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.UNLOADDATE)
                    .HasColumnName(@"UNLOAD_DATE")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.PACKTYPEID)
                    .HasColumnName(@"PACKTYPE_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("decimal");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.WAREHOUSERECN)
                    .HasColumnName(@"WAREHOUSE_REC_N")
                    .HasMaxLength(256)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.EXPORTLANDN)
                    .HasColumnName(@"EXPORT_LAND_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.PACKTYPEIDN)
                    .HasColumnName(@"PACKTYPE_ID_N")
                    .HasMaxLength(4)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.HAWBN)
                    .HasColumnName(@"HAWB_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.IDENTIFIERTYPEN)
                    .HasColumnName(@"IDENTIFIER_TYPE_N")
                    .HasMaxLength(4)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.FIRSTCARGOID)
                    .HasColumnName(@"FIRST_CARGO_ID")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUMSHGR>()
                .Property(p => p.SECONDCARGOID)
                    .HasColumnName(@"SECOND_CARGO_ID")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");

            #endregion

            #region CCUPAYHAND

            modelBuilder.Entity<CCUPAYHAND>()
                .HasKey(p => p.FILENO)
                .ToTable("CCUPAYHAND", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("bigint");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.DRAFTSTATUS)
                    .HasColumnName(@"DRAFT_STATUS")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.PAYTAX)
                    .HasColumnName(@"PAY_TAX")
                 .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.REJECTTAX)
                    .HasColumnName(@"REJECT_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.HANDTYPE)
                    .HasColumnName(@"HAND_TYPE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.PROCESSWANT)
                    .HasColumnName(@"PROCESS_WANT")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TOTALPAYTAX)
                    .HasColumnName(@"TOTAL_PAY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TOTALPAYDEPOSIT)
                    .HasColumnName(@"TOTAL_PAY_DEPOSIT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.HANDDATE)
                    .HasColumnName(@"HAND_DATE")
                    .HasColumnType("DateTime");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.OBJECTIONEXPLAIN)
                    .HasColumnName(@"OBJECTION_EXPLAIN")
                    .HasMaxLength(75)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.DATE7)
                    .HasColumnName(@"DATE_7")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TIME7)
                    .HasColumnName(@"TIME_7")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.IMPORTERNAME)
                    .HasColumnName(@"IMPORTER_NAME")
                    .HasMaxLength(55)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.TRANSIMPORTERNAME)
                    .HasColumnName(@"TRANS_IMPORTER_NAME")
                    .HasMaxLength(55)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.BONDEDNAME)
                    .HasColumnName(@"BONDED_NAME")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.ENTRYID)
                    .HasColumnName(@"ENTRY_ID")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYHAND>()
                .Property(p => p.SIGNERID)
                    .HasColumnName(@"SIGNER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("varchar");

            #endregion

            #region CCUSUPITEM

            modelBuilder.Entity<CCUSUPITEM>()
                .HasKey(p => new { p.FILENO, p.ACCLINENO, p.LINENO })
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.RAISEVALUE)
                    .HasColumnName(@"RAISE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NISVALUE)
                    .HasColumnName(@"NIS_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.EXTRAQNTY)
                    .HasColumnName(@"EXTRA_QNTY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.WHOLESALEPRICE)
                    .HasColumnName(@"WHOLESALE_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.IMPORTADDITION)
                    .HasColumnName(@"IMPORT_ADDITION")
                    .HasColumnType("decimal");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AGNTPAYTAX)
                    .HasColumnName(@"AGNT_PAY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.AGNTPAYBITHA)
                    .HasColumnName(@"AGNT_PAY_BITHA")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NIDHEMEHESPCNT)
                    .HasColumnName(@"NIDHE_MEHES_PCNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.NIDHEMASPCNT)
                    .HasColumnName(@"NIDHE_MAS_PCNT")
                    .HasColumnType("decimal");
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
                    .HasColumnType("bit");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.KATALOGNO)
                    .HasColumnName(@"KATALOG_NO")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.STSQNTY)
                    .HasColumnName(@"STS_QNTY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PRATMEHESN)
                    .HasColumnName(@"PRAT_MEHES_N")
                    .HasMaxLength(12)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.ORIGINCOUNTRYN)
                    .HasColumnName(@"ORIGIN_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUSUPITEM>()
                .Property(p => p.PURCHCOUNTRYN)
                    .HasColumnName(@"PURCH_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar");

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
                    .HasColumnType("varchar");
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
                    .HasColumnType("bit");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(11)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.CERTIFICATENO)
                    .HasColumnName(@"CERTIFICATE_NO")
                    .HasMaxLength(20)
                    .HasColumnType("varchar");
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
                    .HasColumnType("varchar");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.REMARKS)
                    .HasMaxLength(256)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.ADDREQUESTNO)
                    .HasColumnName(@"ADD_REQUEST_NO")
                    .HasMaxLength(10)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCRREQ>()
                .Property(p => p.REQUESTNO)
                    .HasColumnName(@"REQUEST_NO")
                    .HasMaxLength(50)
                    .HasColumnType("varchar");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.RAISEVALUE)
                    .HasColumnName(@"RAISE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NISVALUE)
                    .HasColumnName(@"NIS_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.QUANTITY)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.EXTRAQNTY)
                    .HasColumnName(@"EXTRA_QNTY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.WHOLESALEPRICE)
                    .HasColumnName(@"WHOLESALE_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.IMPORTADDITION)
                    .HasColumnName(@"IMPORT_ADDITION")
                    .HasColumnType("decimal");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AGNTPAYTAX)
                    .HasColumnName(@"AGNT_PAY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.AGNTPAYBITHA)
                    .HasColumnName(@"AGNT_PAY_BITHA")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NIDHEMEHESPCNT)
                    .HasColumnName(@"NIDHE_MEHES_PCNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.NIDHEMASPCNT)
                    .HasColumnName(@"NIDHE_MAS_PCNT")
                    .HasColumnType("decimal");
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
                    .HasColumnType("bit");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.KATALOGNO)
                    .HasColumnName(@"KATALOG_NO")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ORDERLINE)
                    .HasColumnName(@"ORDER_LINE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.UNITID)
                    .HasColumnName(@"UNIT_ID")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.STSQNTY)
                    .HasColumnName(@"STS_QNTY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PRATMEHESN)
                    .HasColumnName(@"PRAT_MEHES_N")
                    .HasMaxLength(12)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.ORIGINCOUNTRYN)
                    .HasColumnName(@"ORIGIN_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCUSTITEM>()
                .Property(p => p.PURCHCOUNTRYN)
                    .HasColumnName(@"PURCH_COUNTRY_N")
                    .HasMaxLength(2)
                    .HasColumnType("varchar");

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
                    .HasColumnType("varchar");
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
                    .HasColumnType("varchar");

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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal");
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
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYEETYPE)
                    .HasColumnName(@"PAYEE_TYPE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.PAYAMOUNT)
                    .HasColumnName(@"PAY_AMOUNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.ACCOUNTNAME)
                    .HasColumnName(@"ACCOUNT_NAME")
                    .HasMaxLength(30)
                    .HasColumnType("nvarchar");
            modelBuilder.Entity<CCUPAYLINEF>()
                .Property(p => p.BANKID)
                    .HasColumnName(@"BANK_ID")
                    .HasColumnType("decimal");
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
                    .HasColumnType("DateTime");
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
                    .HasColumnType("bigint");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.PRATMEHES)
                    .HasColumnName(@"PRAT_MEHES")
                    .HasMaxLength(9)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.GOODSNO)
                    .HasColumnName(@"GOODS_NO")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXTYPE)
                    .HasColumnName(@"TAX_TYPE")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXBASIS)
                    .HasColumnName(@"TAX_BASIS")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXAMOUNT)
                    .HasColumnName(@"TAX_AMOUNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.POSTPONEDTAX)
                    .HasColumnName(@"POSTPONED_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXTOPAY)
                    .HasColumnName(@"TAX_TO_PAY")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXCALCCODE)
                    .HasColumnName(@"TAX_CALC_CODE")
                    .HasColumnType("SMALLINT");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXRATE)
                    .HasColumnName(@"TAX_RATE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.DEFINEDTAX)
                    .HasColumnName(@"DEFINED_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDTAXRATE)
                    .HasColumnName(@"ADD_TAX_RATE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDEFINEDTAX)
                    .HasColumnName(@"AD_DEFINED_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.ADDIMPORT)
                    .HasColumnName(@"ADD_IMPORT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.PRATMEHESN)
                    .HasColumnName(@"PRAT_MEHES_N")
                    .HasMaxLength(12)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUTAX>()
                .Property(p => p.TAXTYPEN)
                    .HasColumnName(@"TAX_TYPE_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar");

            #endregion

            #region CCUFILEM

            modelBuilder.Entity<CCUFILEM>()
                .HasKey(p => p.FILENO)
                .ToTable("CCUFILEM", "AMITESTM");
            // Properties:
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FILENO)
                    .HasColumnName(@"FILE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("bigint");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMERID)
                    .HasColumnName(@"CUSTOMER_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMFILENO)
                    .HasColumnName(@"CUSTOM_FILE_NO")
                    .HasColumnType("decimal");
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
                    .HasColumnType("DateTime");
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
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FOLUPDATE)
                    .HasColumnName(@"FOL_UP_DATE")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FILECLOSE)
                    .HasColumnName(@"FILE_CLOSE")
                    .HasColumnType("int");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.OPENBYUSER)
                    .HasColumnName(@"OPEN_BY_USER")
                    .HasMaxLength(15)
                    .HasColumnType("varchar");
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
                    .HasColumnType("SMALLINT");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RESHMDATE)
                    .HasColumnName(@"RESHM_DATE")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.WARNINGDATE)
                    .HasColumnName(@"WARNING_DATE")
                    .HasColumnType("DateTime");
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.REGIONVALUE)
                    .HasColumnName(@"REGION_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANSPVALUE)
                    .HasColumnName(@"TRANSP_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEVALUE)
                    .HasColumnName(@"INSURANCE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.SERVICEVALUE)
                    .HasColumnName(@"SERVICE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.EXPENSEVALUE)
                    .HasColumnName(@"EXPENSE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CLOSUREVALUE)
                    .HasColumnName(@"CLOSURE_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FEECARRIER)
                    .HasColumnName(@"FEE_CARRIER")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.FEEPLATFORM)
                    .HasColumnName(@"FEE_PLATFORM")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CURRENCYRATE)
                    .HasColumnName(@"CURRENCY_RATE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PRICEINDEX)
                    .HasColumnName(@"PRICE_INDEX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GOODSVALUE)
                    .HasColumnName(@"GOODS_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CIFVALUE)
                    .HasColumnName(@"CIF_VALUE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.ACCEPTEDPRICE)
                    .HasColumnName(@"ACCEPTED_PRICE")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TOTALTAX)
                    .HasColumnName(@"TOTAL_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANSPVALFC)
                    .HasColumnName(@"TRANSP_VAL_FC")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANCURRENCY)
                    .HasColumnName(@"TRAN_CURRENCY")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEPERCENT)
                    .HasColumnName(@"INSURANCE_PERCENT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCECURR)
                    .HasColumnName(@"INSURANCE_CURR")
                    .HasMaxLength(3)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCEAMNT)
                    .HasColumnName(@"INSURANCE_AMNT")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PAYDATE)
                    .HasColumnName(@"PAY_DATE")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PAYTIME)
                    .HasColumnName(@"PAY_TIME")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.MEHESDRAFTSTATUS)
                    .HasColumnName(@"MEHES_DRAFT_STATUS")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GRANTDATE)
                    .HasColumnName(@"GRANT_DATE")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GRANTTIME)
                    .HasColumnName(@"GRANT_TIME")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INDICATORS)
                    .HasMaxLength(20)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMPRINT)
                    .HasColumnName(@"CUSTOM_PRINT")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TSHUMOTTAXPRINT)
                    .HasColumnName(@"TSHUMOT_TAX_PRINT")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CHARGESYSPRINT)
                    .HasColumnName(@"CHARGE_SYS_PRINT")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.CUSTOMAGENTPRINT)
                    .HasColumnName(@"CUSTOM_AGENT_PRINT")
                    .HasColumnType("DateTime");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GRNTTYPEID)
                    .HasColumnName(@"GRNT_TYPE_ID")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.GUARANTEEAMNT)
                    .HasColumnName(@"GUARANTEE_AMNT")
                    .HasColumnType("decimal");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RESHIMONNON)
                    .HasColumnName(@"RESHIMON_NO_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.COINIDN)
                    .HasColumnName(@"COIN_ID_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TRANCURRENCYN)
                    .HasColumnName(@"TRAN_CURRENCY_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.INSURANCECURRN)
                    .HasColumnName(@"INSURANCE_CURR_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar");
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.PRATMEHESLIST)
                    .HasColumnName(@"PRAT_MEHES_LIST")
                    .HasMaxLength(30)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.ALLPRATMEHESLIST)
                    .HasColumnName(@"ALL_PRAT_MEHES_LIST")
                    .HasMaxLength(1024)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.NOOFINVOICES)
                    .HasColumnName(@"NO_OF_INVOICES")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.TOTALINVOICELINESNO)
                    .HasColumnName(@"TOTAL_INVOICE_LINES_NO")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUFILEM>()
                .Property(p => p.RIGHTOWNIDN)
                    .HasColumnName(@"RIGHT_OWN_ID_N")
                    .HasMaxLength(35)
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUFILEM>()
               .Property(p => p.SELLCONDITIONIDN)
                   .HasColumnName(@"SELL_CONDITION_ID_N")
                   .HasMaxLength(35)
                   .HasColumnType("varchar");








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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.LINENO)
                    .HasColumnName(@"LINE_NO")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.TRANSPVALFC)
                    .HasColumnName(@"TRANSP_VAL_FC")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.CURRID)
                    .HasColumnName(@"CURR_ID")
                    .HasMaxLength(2)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.TRANSPVAL)
                    .HasColumnName(@"TRANSP_VAL")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUTRANSPVAL>()
                .Property(p => p.CURRIDN)
                    .HasColumnName(@"CURR_ID_N")
                    .HasMaxLength(3)
                    .HasColumnType("varchar");

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
                    .HasColumnType("varchar(max)");

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
                    .HasMaxLength(12)
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
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.FUELTYPE)
                    .HasColumnName(@"FUEL_TYPE")
                    .HasMaxLength(1)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARL>()
                .Property(p => p.WEIGHT)
                    .HasColumnType("decimal");
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
                    .HasMaxLength(12)
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
                    .HasColumnType("varchar");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.WINDOWNO)
                    .HasColumnName(@"WINDOW_NO")
                    .HasMaxLength(13)
                    .HasColumnType("char");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.FOB)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.GENERALTAX)
                    .HasColumnName(@"GENERAL_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.BUYTAX)
                    .HasColumnName(@"BUY_TAX")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.VATRESHIMON)
                    .HasColumnName(@"VAT_RESHIMON")
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUCARSC>()
                .Property(p => p.EXEMPTTYPE)
                    .HasColumnName(@"EXEMPT_TYPE")
                    .HasMaxLength(2)
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
                    .HasColumnType("bigint");
            modelBuilder.Entity<CCUSIGNUM>()
                .Property(p => p.LINENOMSHGR)
                    .HasColumnName(@"LINE_NO_MSHGR")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSIGNUM>()
                .Property(p => p.LINENOSIGN)
                    .HasColumnName(@"LINE_NO_SIGN")
                    .IsRequired()
                    .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)
                    .HasColumnType("decimal");
            modelBuilder.Entity<CCUSIGNUM>()
                .Property(p => p.SIGNNUM)
                    .HasColumnName(@"SIGN_NUM")
                    .HasMaxLength(30)
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
                    .HasColumnType("varchar");

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
                    .HasColumnType("varchar");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.PRIMARYNUM)
                    .HasColumnName(@"PRIMARY_NUM")
                    .HasMaxLength(12)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.TYPE)
                    .HasMaxLength(10)
                    .HasColumnType("varchar");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.LOGTIME)
                    .HasColumnName(@"LOG_TIME")
                    .HasColumnType("date");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.PRIORITY)
                    .HasColumnType("SMALLINT");
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
                    .HasColumnType("nvarchar(max)");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.RESPONSE)
                    .HasColumnType("varchar(max)");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.CLIENTID)
                    .HasColumnName(@"CLIENT_ID")
                    .HasMaxLength(15)
                    .HasColumnType("char");
            modelBuilder.Entity<YCULTASK>()
                .Property(p => p.USRCODE)
                    .HasColumnName(@"USR_CODE")
                    .HasMaxLength(15)
                    .HasColumnType("varchar");

            #endregion

            #region SYNRECORD

            modelBuilder.Entity<SyncRecord>()
                .HasKey(p => new { p.Id })
                .ToTable("SYNCRECORD", "AMITESTM");
            // Properties:
            modelBuilder.Entity<SyncRecord>()
                .Property(p => p.Id)
                .HasColumnName(@"Id")
                .IsRequired()
                .HasMaxLength(36)
                .HasColumnType("nvarchar");
            modelBuilder.Entity<SyncRecord>()
            .Property(p => p.Tenant)
                .HasColumnName(@"Tenant")
                .IsRequired()
                .HasColumnType("int");
            modelBuilder.Entity<SyncRecord>()
            .Property(p => p.Entname)
                .HasColumnName(@"Entname")
                .HasMaxLength(60)
                .HasColumnType("nvarchar");
            modelBuilder.Entity<SyncRecord>()
                .Property(p => p.KeyVal)
                .HasColumnName(@"KeyVal")
                .HasMaxLength(255)
                .HasColumnType("nvarchar");
            modelBuilder.Entity<SyncRecord>()
                .Property(p => p.FileNo)
                .HasColumnName(@"FileNo")
                .HasMaxLength(50)
                .HasColumnType("nvarchar");
            modelBuilder.Entity<SyncRecord>()
                .Property(p => p.TrigAction)
                .HasColumnName(@"TrigAction")
                .HasMaxLength(1)
                .HasColumnType("nvarchar");
            modelBuilder.Entity<SyncRecord>()
                .Property(p => p.CreateDate)
                .IsRequired()
                .HasColumnName(@"CreateDate")
                .HasColumnType("Datetime");
            modelBuilder.Entity<SyncRecord>()
                .Property(p => p.SyncDT)
                .HasColumnName(@"SyncDT")
                .HasColumnType("Datetime");
            modelBuilder.Entity<SyncRecord>()
                .Property(p => p.IsSync)
                .IsRequired()
                .HasColumnName(@"IsSync")
                .HasColumnType("int");


            #endregion

            #region Disabled conventions


            #endregion
            return modelBuilder;///base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<YCULPROCESS> YCULPROCESSES { get; set; }
        public virtual DbSet<GGGQ> GGGQs { get; set; }
        public virtual DbSet<CCUACCSUP> CCUACCSUPs { get; set; }
        public virtual DbSet<CCUMSHGR> CCUMSHGRs { get; set; }
        public virtual DbSet<CCUPAYHAND> CCUPAYHANDs { get; set; }
        public virtual DbSet<CCUSUPITEM> CCUSUPITEMs { get; set; }
        public virtual DbSet<GDFDATA> GDFDATAs { get; set; }
        public virtual DbSet<CCUCRREQ> CCUCRREQs { get; set; }
        public virtual DbSet<GTRTRAN> GTRTRANs { get; set; }
        public virtual DbSet<GRTRATE> GRTRATEs { get; set; }
        public virtual DbSet<CCUCUSTITEM> CCUCUSTITEMs { get; set; }
        public virtual DbSet<CCUQUELOCK> CCUQUELOCKs { get; set; }
        public virtual DbSet<CCUPAYLINEF> CCUPAYLINEFs { get; set; }
        public virtual DbSet<CCUTAX> CCUTAXES { get; set; }
        public virtual DbSet<CCUFILEM> CCUFILEMs { get; set; }
        public virtual DbSet<GTBMANDT> GTBMANDTs { get; set; }
        public virtual DbSet<GNDCARD> GNDCARDs { get; set; }
        public virtual DbSet<CTBPACKTYPE> CTBPACKTYPEs { get; set; }
        public virtual DbSet<GAQFILEDATA> GAQFILEDATAs { get; set; }
        public virtual DbSet<CTBBONDED> CTBBONDEDs { get; set; }
        public virtual DbSet<CTBCOUNTRY> CTBCOUNTRIES { get; set; }
        public virtual DbSet<CTBIDNTP> CTBIDNTPs { get; set; }
        public virtual DbSet<CTBIMPORT> CTBIMPORTs { get; set; }
        public virtual DbSet<CTBMISHGUR> CTBMISHGURs { get; set; }
        public virtual DbSet<CTBRESHTYPE> CTBRESHTYPEs { get; set; }
        public virtual DbSet<CTBRGOWN> CTBRGOWNs { get; set; }
        public virtual DbSet<CTBSTORAGE> CTBSTORAGEs { get; set; }
        public virtual DbSet<CTBTRANSP> CTBTRANSPs { get; set; }
        public virtual DbSet<CTBUNLOAD> CTBUNLOADs { get; set; }
        public virtual DbSet<ATBPTIL> ATBPTILs { get; set; }
        public virtual DbSet<CTBLOAD> CTBLOADs { get; set; }
        public virtual DbSet<GNDADR> GNDADRs { get; set; }
        public virtual DbSet<CTBPART> CTBPARTs { get; set; }
        public virtual DbSet<CTBPKDT> CTBPKDTs { get; set; }
        public virtual DbSet<CTBAPPROV> CTBAPPROVs { get; set; }
        public virtual DbSet<CTBAPPROVTYPE> CTBAPPROVTYPEs { get; set; }
        public virtual DbSet<GTBREQCERT> GTBREQCERTs { get; set; }
        public virtual DbSet<CCUTRANSPVAL> CCUTRANSPVALs { get; set; }
        public virtual DbSet<CTBCURRENCY> CTBCURRENCIES { get; set; }
        public virtual DbSet<CTBINCOTERM> CTBINCOTERMs { get; set; }
        public virtual DbSet<CTBTARIFF> CTBTARIFFs { get; set; }
        public virtual DbSet<CCUMESSAGE> CCUMESSAGEs { get; set; }
        public virtual DbSet<CTBERROR> CTBERRORs { get; set; }
        public virtual DbSet<CCUCARL> CCUCARLs { get; set; }
        public virtual DbSet<CTBMEMIRTYPE> CTBMEMIRTYPEs { get; set; }
        public virtual DbSet<CCUTSRUFOT> CCUTSRUFOTs { get; set; }
        public virtual DbSet<CTBTSRUFTYPE> CTBTSRUFTYPEs { get; set; }
        public virtual DbSet<CTBTAXTYPE> CTBTAXTYPEs { get; set; }
        public virtual DbSet<ITBPCKTY> ITBPCKTIES { get; set; }
        public virtual DbSet<GDMFILING> GDMFILINGs { get; set; }
        public virtual DbSet<GDMFLDRTR> GDMFLDRTRs { get; set; }
        public virtual DbSet<GDMFILEVER> GDMFILEVERs { get; set; }
        public virtual DbSet<YCULTASK> YCULTASKs { get; set; }
        public virtual DbSet<YTBCUSTTB> YTBCUSTTBs { get; set; }
        public virtual DbSet<CTBCUSTSUP> CTBCUSTSUPs { get; set; }
        public virtual DbSet<GTBITEM> GTBITEMs { get; set; }
        public virtual DbSet<CCUCAR> CCUCARs { get; set; }
        public virtual DbSet<CCUCARSC> CCUCARSCs { get; set; }
        public virtual DbSet<CFIGOODDESC> CFIGOODDESCs { get; set; }
        public virtual DbSet<CCUSIGNUM> CCUSIGNUMs { get; set; }
        public virtual DbSet<CFIMSVLINE> CFIMSVLINEs { get; set; }
        public virtual DbSet<CFIMSVDOC> CFIMSVDOCs { get; set; }
        public virtual DbSet<GTBDOC> GTBDOCs { get; set; }
        public virtual DbSet<GDMENTITY> GDMENTITIES { get; set; }
        public virtual DbSet<GDMREF> GDMREFs { get; set; }
        public virtual DbSet<GAQDOC> GAQDOCs { get; set; }
        public virtual DbSet<CFIMSVFILE> CFIMSVFILEs { get; set; }
        public virtual DbSet<CFIMSVPAGE> CFIMSVPAGEs { get; set; }
        public virtual DbSet<GTBITMCN> GTBITMCNs { get; set; }
        public virtual DbSet<GITITEM> GITITEMs { get; set; }
        public virtual DbSet<CFICONN> CFICONNs { get; set; }
        public virtual DbSet<CFIPACK> CFIPACKs { get; set; }
        public virtual DbSet<CCUSUPITEMSI> CCUSUPITEMSIs { get; set; }
        public virtual DbSet<VRELEASE2ENTRY> VRELEASE2ENTRIES { get; set; }
        public virtual DbSet<YTBTABLE> YTBTABLEs { get; set; }
        public virtual DbSet<CFIMSVREM> CFIMSVREMs { get; set; }
        public virtual DbSet<CFIMSVFLINE> CFIMSVFLINEs { get; set; }
        public virtual DbSet<GDMQUEST> GDMQUESTs { get; set; }
        public virtual DbSet<CFIFILEM> CFIFILEMs { get; set; }
        public virtual DbSet<CTBFITYPE> CTBFITYPEs { get; set; }
        public virtual DbSet<GTBFUSTATU> GTBFUSTATUs { get; set; }
        public virtual DbSet<GAQSTRUCTURE> GAQSTRUCTUREs { get; set; }
        public virtual DbSet<GAQREMARK> GAQREMARKs { get; set; }
        public virtual DbSet<GAQQLOAD> GAQQLOADs { get; set; }
        public virtual DbSet<GAQMERGE> GAQMERGEs { get; set; }
        public virtual DbSet<GAQDLOAD> GAQDLOADs { get; set; }
        public virtual DbSet<GAQDEF> GAQDEFs { get; set; }
        public virtual DbSet<GAQDATALOAD> GAQDATALOADs { get; set; }
        public virtual DbSet<GAQDATA> GAQDATAs { get; set; }
        public virtual DbSet<DWGNDCARD> DWGNDCARDs { get; set; }
        public virtual DbSet<CTBTEAM> CTBTEAMs { get; set; }
        public virtual DbSet<GITITEMCR> GITITEMCRs { get; set; }
        public virtual DbSet<GDMLOCK> GDMLOCKs { get; set; }

        public virtual DbSet<GTBDPTM> GTBDPTMs { get; set; }
        public virtual DbSet<GSCUSR> GSCUSRs { get; set; }
        public virtual DbSet<GCBSCRNVWU> GCBSCRNVWUs { get; set; }
        public virtual DbSet<GAQUSER> GAQUSERs { get; set; }
        public virtual DbSet<GGGQC> GGGQCs { get; set; }
        public virtual DbSet<GAQTEAMUSR> GAQTEAMUSRs { get; set; }

        public virtual DbSet<GTBPTYPE> GTBPTYPEs { get; set; }
        public virtual DbSet<ETBPAYTR> ETBPAYTRs { get; set; }
        public virtual DbSet<ETBPORT> ETBPORTs { get; set; }
        public virtual DbSet<ETBAIRLINE> ETBAIRLINEs { get; set; }
        public virtual DbSet<ITBPORT> ITBPORTs { get; set; }
        public virtual DbSet<RTBPORT> RTBPORTs { get; set; }
        public virtual DbSet<ETBSERLV> ETBSERLVs { get; set; }
        public virtual DbSet<GTBSERLV> GTBSERLVs { get; set; }
        public virtual DbSet<ETBVEND> ETBVENDs { get; set; }
        public virtual DbSet<MTBCARR> MTBCARRs { get; set; }
        public virtual DbSet<MTBPORT> MTBPORTs { get; set; }
        public virtual DbSet<GGGHDAY> GGGHDAYS { get; set; }
        public virtual DbSet<EFIFILEM> EFIFILEMs { get; set; }
        public virtual DbSet<ESPSPED> ESPSPEDs { get; set; }
        public virtual DbSet<GITITEMAP> GITITEMAPs { get; set; }
        public virtual DbSet<CFIMSVSTATL> CFIMSVSTATLs { get; set; }
        public virtual DbSet<CTBCARMOD> CTBCARMODs { get; set; }
        public virtual DbSet<MSPSPED> MSPSPEDs { get; set; }
        public virtual DbSet<MFIFILEM> MFIFILEMs { get; set; }
        public virtual DbSet<EFIMMN> EFIMMNs { get; set; }
        public virtual DbSet<LFIFILEM> LFIFILEMs { get; set; }
        public virtual DbSet<GAQTEAM> GAQTEAMs { get; set; }
        public virtual DbSet<SyncRecord> SyncRecord { get; set; }
    }
}