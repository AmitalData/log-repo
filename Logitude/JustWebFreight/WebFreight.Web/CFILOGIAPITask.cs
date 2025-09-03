using LogicExtensions;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using UnifreightIIG.Common.Extensions;
using System.Data.SqlClient;
using System.Data;
namespace WebFreight.Web
{
    /// <summary>
    /// CFILOGIAPI G_HybridSqlReq
    /// </summary>
    public class CFILOGIAPITask
    {

        static public List<CFILOGIAPI> GetLogiOcc()
        {
            List<CFILOGIAPI> logi_list = new List<CFILOGIAPI>();
            XmlDocument doc = new XmlDocument();

            doc.LoadXml(CfiLogi.DB.Replace("&uSEP;", "|"));
            XmlNodeList nodeList;
            XmlNode root = doc.DocumentElement;
            nodeList = root.SelectNodes("descendant::OCC");
            foreach (XmlNode occ_node in nodeList)
            {
                XmlDocument occ = new XmlDocument();

                doc.LoadXml("<root>" + occ_node.InnerXml + "</root>");
                XmlNodeList occNodeList;
                XmlNode occ_root = doc.DocumentElement;
                occNodeList = occ_root.SelectNodes("descendant::DAT");
                bool IS_INSERT = false;
                string HAS_IN_OPER = "";
                string PARAMETERS_TYPE = string.Empty;
                try
                {
                    IS_INSERT = occ_root.SelectNodes("descendant::DAT[@name='IS_INSERT']").Item(0).InnerText.ToBoolAmitalFormart();
                }
                catch { }
                try
                {
                    PARAMETERS_TYPE = occ_root.SelectNodes("descendant::DAT[@name='PARAMETERS_TYPE']").Item(0).InnerText.ToString();
                }
                catch { }
                try
                {
                    HAS_IN_OPER = occ_root.SelectNodes("descendant::DAT[@name='HAS_IN_OPER']").Item(0).InnerText.ToString();
                }
                catch { }
                logi_list.Add(new CFILOGIAPI
                {
                    CODE = occ_root.SelectNodes("descendant::DAT[@name='CODE']").Item(0).InnerText,
                    EXAMPLE_RESULT = occ_root.SelectNodes("descendant::DAT[@name='EXAMPLE_RESULT']").Item(0).InnerText,
                    TEMPLATE_SQL = occ_root.SelectNodes("descendant::DAT[@name='TEMPLATE_SQL']").Item(0).InnerText,
                    EXAMPLE_SQL = occ_root.SelectNodes("descendant::DAT[@name='EXAMPLE_SQL']").Item(0).InnerText,
                    NAME_ENG = occ_root.SelectNodes("descendant::DAT[@name='NAME_ENG']").Item(0).InnerText,
                    PARAMETERS = occ_root.SelectNodes("descendant::DAT[@name='PARAMETERS']").Item(0).InnerText,
                    REFERENCE = occ_root.SelectNodes("descendant::DAT[@name='REFERENCE']").Item(0).InnerText,
                    LINQ = occ_root.SelectNodes("descendant::DAT[@name='LINQ']").Item(0).InnerText.ToBoolAmitalFormart(),
                    HAS_TENANT = occ_root.SelectNodes("descendant::DAT[@name='HAS_TENANT']").Item(0).InnerText.ToBoolAmitalFormart(),
                    IS_INSERT = IS_INSERT,
                    PARAMETERS_TYPE = PARAMETERS_TYPE,
                    HAS_IN_OPER = HAS_IN_OPER
                });
            }
            return (logi_list);
        }

    }

    public class CFILOGIAPI
    {
        public string CODE;
        public string NAME_ENG;
        public string REFERENCE;
        public string PARAMETERS;
        public string TEMPLATE_SQL;
        public string EXAMPLE_SQL;
        public string EXAMPLE_RESULT;
        public string PARAMETERS_TYPE
        {
            set
            {
                if (!value.IsNullOrEmpty())
                {
                    parameters_type.Clear();
                    foreach (string param in value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] p = param.Split('=');
                        if (p.Length == 2)
                        {
                            string[] p2 = p[1].Split(',');
                            parameters_type.Add(new paramer_type(p[0], p2[0], p2[1]));
                        }
                    }
                }
            }
        }
        public List<paramer_type> parameters_type = new List<paramer_type>();
        public bool LINQ = false;
        public bool HAS_TENANT = true;
        public bool IS_INSERT = false;
        public string HAS_IN_OPER = "";
        
        public SqlParameter get_SqlParameter(string name, string val)
        {
            SqlParameter ret = null;
            paramer_type pt = parameters_type.Where(p => p.name.ToUpper() == name.ToUpper()).FirstOrDefault();
            if (pt==null)
            {
                ret = new SqlParameter(name, SqlDbType.VarChar);
            }
            else
            {
                int size = string.IsNullOrEmpty(pt.psize) ? -1 : Convert.ToInt32(pt.psize);
                if (pt.ptype.ToLower() == "varchar")
                {
                    ret = new SqlParameter(name, SqlDbType.VarChar, size);
                }
                else if (pt.ptype.ToLower() == "nvarchar")
                {
                    ret = new SqlParameter(name, SqlDbType.NVarChar, size);
                }
                else if (pt.ptype.ToLower() == "int")
                {
                    ret = new SqlParameter(name, SqlDbType.Int);
                }
                else if (pt.ptype.ToLower() == "datetime")
                {
                    ret = new SqlParameter(name, SqlDbType.DateTime);
                }
                else if (pt.ptype.ToLower() == "datetime2")
                {
                    ret = new SqlParameter(name, SqlDbType.DateTime2,size);
                }
                else if (pt.ptype.ToLower() == "date")
                {
                    ret = new SqlParameter(name, SqlDbType.Date);
                }
                else if (pt.ptype.ToLower() == "bit")
                {
                    ret = new SqlParameter(name, SqlDbType.Bit);
                }
                
            }


            switch (ret.SqlDbType)
            {
                case SqlDbType.Int:
                    ret.Value = int.TryParse(val, out var i) ? (object)i : DBNull.Value;
                    break;
                case SqlDbType.DateTime:
                    ret.Value = DateTime.TryParse(val, out var dt) ? (object)dt : DBNull.Value;
                    break;
                case SqlDbType.Bit:
                    ret.Value = bool.TryParse(val, out var b) ? (object)b : DBNull.Value;
                    break;
                default:
                    ret.Value = val ?? string.Empty;
                    break;
            }

            
            return (ret);
        }

    }
    public class paramer_type
    {
        public string name;
        public string ptype;
        public string psize;
        public paramer_type(string name, string ptype, string psize)
        {
            this.name = name;
            this.ptype = ptype;
            this.psize = psize;
        }
    }

    /*
    SUPPLIERINVOICES
    DECLARATIONID=varchar,15

    CONSIGNMENTS
    DECLARATIONID=varchar,15

    */

    public class CfiLogi
    {
        static public string DB = @"<root>
<OCC>
<DAT name=""CODE"">A114</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIUDIAMONDS</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select ID,ISSIGNEDVERSION,IsValidTicketsDiamond,IsMissMandatoryDiamond from CUSTOMS.DECLARATIONS where ID in (@ID) AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select ID,ISSIGNEDVERSION,IsValidTicketsDiamond,IsMissMandatoryDiamond from CUSTOMS.DECLARATIONS where ID in (@ID) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
<DAT name=""HAS_IN_OPER"">@ID</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A113</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIRPNDCOU</DAT>
<DAT name=""REFERENCE"">CourierPendingReasons</DAT>
<DAT name=""PARAMETERS"">CODE=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select ENGLISHNAME from CUSTOMS.CourierPendingReasons where code=@CODE and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select ENGLISHNAME from CUSTOMS.CourierPendingReasons where code=@CODE and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CODE=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A112</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFCOUDISTXML</DAT>
<DAT name=""REFERENCE"">DECLARATIONCOURIERSTATUSES</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select TruckerId from CUSTOMS.DeclarationCourierStatuses where declarationid=@DECLARATIONID  and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select TruckerId from CUSTOMS.DeclarationCourierStatuses where declarationid=@DECLARATIONID  and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A111</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFCOUDISTXML</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select CourierCustomStatusCode,AcceptanceStatusCode from CUSTOMS.Declarations where ID=@ID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select CourierCustomStatusCode,AcceptanceStatusCode from CUSTOMS.Declarations where ID=@ID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A110</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIUDIAMONDS</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select ISSIGNEDVERSION,IsValidTicketsDiamond,IsMissMandatoryDiamond from CUSTOMS.DECLARATIONS where ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select ISSIGNEDVERSION,IsValidTicketsDiamond,IsMissMandatoryDiamond from CUSTOMS.DECLARATIONS where ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A109</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIQGRSTSRCOU</DAT>
<DAT name=""REFERENCE"">DECLARATIONCOURIERSTATUSES</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select DECLARATIONCOURIERSTATUSES.COURIERPENDINGREASONCODE from CUSTOMS.DECLARATIONCOURIERSTATUSES where DECLARATIONCOURIERSTATUSES.DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select DECLARATIONCOURIERSTATUSES.COURIERPENDINGREASONCODE from CUSTOMS.DECLARATIONCOURIERSTATUSES where DECLARATIONCOURIERSTATUSES.DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A108</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFCOUDECXML</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT top(1) DECLARATIONS.CARGODESCRIPTION FROM CUSTOMS.CONSIGNMENTS,CUSTOMS.DECLARATIONS WHERE 
CONSIGNMENTS.DECLARATIONID = DECLARATIONS.ID AND DECLARATIONS.CUSTOMFILENO=@CUSTOMFILENO and DECLARATIONS.TENANT=@Tenant  and CONSIGNMENTS.TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT top(1) DECLARATIONS.CARGODESCRIPTION FROM CUSTOMS.CONSIGNMENTS,CUSTOMS.DECLARATIONS WHERE 
CONSIGNMENTS.DECLARATIONID = DECLARATIONS.ID AND DECLARATIONS.CUSTOMFILENO=@CUSTOMFILENO and DECLARATIONS.TENANT=@Tenant  and CONSIGNMENTS.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A107</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFCOUDECXML</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT top(1) InvoiceAmount,InvoiceCurrencyTypeCode  FROM CUSTOMS.SUPPLIERINVOICES,CUSTOMS.DECLARATIONS 
WHERE SUPPLIERINVOICES.DECLARATIONID = DECLARATIONS.ID AND DECLARATIONS.CUSTOMFILENO=@CUSTOMFILENO and DECLARATIONS.TENANT=@Tenant  and SUPPLIERINVOICES.TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT top(1) InvoiceAmount,InvoiceCurrencyTypeCode  FROM CUSTOMS.SUPPLIERINVOICES,CUSTOMS.DECLARATIONS 
WHERE SUPPLIERINVOICES.DECLARATIONID = DECLARATIONS.ID AND DECLARATIONS.CUSTOMFILENO=@CUSTOMFILENO and DECLARATIONS.TENANT=@Tenant  and SUPPLIERINVOICES.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A106</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CREUCARREL</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select ISSIGNEDVERSION from CUSTOMS.DECLARATIONS where ID=@ID  and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select ISSIGNEDVERSION from CUSTOMS.DECLARATIONS where ID=@ID  and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A105</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CENUPACKIIG</DAT>
<DAT name=""REFERENCE"">SUPPLIERINVOICEITEMS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True·;LINENUMBER=True·;COUNTERKEY=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select SEQUENCENUMERIC from CUSTOMS.SUPPLIERINVOICEITEMS where DECLARATIONID=@DECLARATIONID and COUNTERKEY=@COUNTERKEY and LINENUMBER=@LINENUMBER and  TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select SEQUENCENUMERIC from CUSTOMS.SUPPLIERINVOICEITEMS where DECLARATIONID=@DECLARATIONID and COUNTERKEY=@COUNTERKEY and LINENUMBER=@LINENUMBER and  TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;COUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A104</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFDEF</DAT>
<DAT name=""REFERENCE"">PAYMENTORDERS,PAYMENTORDERLINES,PARAGRAPHTYPES</DAT>
<DAT name=""PARAMETERS"">PAYMENTNUMBER=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select PAYMENTORDERS.ID,PARAGRAPHTYPES.LOCALNAME,PAYMENTORDERLINES.AMOUNT 
from CUSTOMS.PAYMENTORDERS,CUSTOMS.PAYMENTORDERLINES,CUSTOMS.PARAGRAPHTYPES 
where PAYMENTORDERS.PAYMENTNUMBER = @PAYMENTNUMBER AND 
PAYMENTORDERS.ID = PAYMENTORDERLINES.PAYMENTORDERID AND 
PAYMENTORDERLINES.PARAGRAPHTYPECODE =  PARAGRAPHTYPES.CODE  and PAYMENTORDERS.TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select PAYMENTORDERS.ID,PARAGRAPHTYPES.LOCALNAME,PAYMENTORDERLINES.AMOUNT 
from CUSTOMS.PAYMENTORDERS,CUSTOMS.PAYMENTORDERLINES,CUSTOMS.PARAGRAPHTYPES 
where PAYMENTORDERS.PAYMENTNUMBER = @PAYMENTNUMBER AND 
PAYMENTORDERS.ID = PAYMENTORDERLINES.PAYMENTORDERID AND 
PAYMENTORDERLINES.PARAGRAPHTYPECODE =  PARAGRAPHTYPES.CODE  and PAYMENTORDERS.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE""></DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A103</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CENUPACKIIG</DAT>
<DAT name=""REFERENCE"">SUPPLIERINVOICES</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True·;INVOICECOUNTERKEY=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select SEQUENCENUMERIC from CUSTOMS.SUPPLIERINVOICES where DECLARATIONID=@DECLARATIONID and INVOICECOUNTERKEY=@INVOICECOUNTERKEY  and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select SEQUENCENUMERIC from CUSTOMS.SUPPLIERINVOICES where DECLARATIONID=@DECLARATIONID and INVOICECOUNTERKEY=@INVOICECOUNTERKEY  and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A102</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIRNOSTSCOU</DAT>
<DAT name=""REFERENCE"">COURIERMASTERS</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select ESTIMATEDARRIVALDATE,LANDINGDATE  from CUSTOMS.COURIERMASTERS where COURIERMASTERS.ID=@ID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select ESTIMATEDARRIVALDATE,LANDINGDATE  from CUSTOMS.COURIERMASTERS where COURIERMASTERS.ID=@ID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A101</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIRNOSTSCOU</DAT>
<DAT name=""REFERENCE"">COURIERDECLARATIONS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select COURIERDECLARATIONS.COURIERMASTERID from CUSTOMS.COURIERDECLARATIONS where COURIERDECLARATIONS.DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select COURIERDECLARATIONS.COURIERMASTERID from CUSTOMS.COURIERDECLARATIONS where COURIERDECLARATIONS.DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A100</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFTOCOU</DAT>
<DAT name=""REFERENCE"">CONSIGNMENTPACKAGES</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select GROSSMASSMEASURE, PACKAGEQUANTITY from CUSTOMS.CONSIGNMENTPACKAGES where DECLARATIONID = @DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select GROSSMASSMEASURE, PACKAGEQUANTITY from CUSTOMS.CONSIGNMENTPACKAGES where DECLARATIONID = @DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE""></DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A99</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFTOCOU</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select PROCEDURECURRENTCODE from CUSTOMS.DECLARATIONS where CUSTOMFILENO = @CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select PROCEDURECURRENTCODE from CUSTOMS.DECLARATIONS where CUSTOMFILENO = @CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A98</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CENUPACKC</DAT>
<DAT name=""REFERENCE"">MEASURMENTUNITS</DAT>
<DAT name=""PARAMETERS"">MALAMID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select code from customs.measurmentunits where malamid=@MALAMID</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select code from customs.measurmentunits where malamid=@MALAMID</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">false</DAT>
<DAT name=""PARAMETERS_TYPE"">MALAMID=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A97</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CENUPACKC</DAT>
<DAT name=""REFERENCE"">PROPERTIESDETAILSHISTORYS</DAT>
<DAT name=""PARAMETERS"">CUSTOMSITEMID=True·;ENDDATE=True·;STARTDATE=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select measurementunitid from CUSTOMS.propertiesdetailshistorys where 
CUSTOMSITEMID =@CUSTOMSITEMID  and startdate &lt;= cast(@STARTDATE as date)
and enddate &gt; cast(@ENDDATE as date)</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select measurementunitid from CUSTOMS.propertiesdetailshistorys where 
CUSTOMSITEMID =@CUSTOMSITEMID  and startdate &lt;= cast(@STARTDATE as date)
and enddate &gt; cast(@ENDDATE as date)</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">false</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMSITEMID=varchar,9&uSEP;STARTDATE=date,7&uSEP;ENDDATE=date,7</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A96</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CENUPACKC</DAT>
<DAT name=""REFERENCE"">CUSTOMSITEMS</DAT>
<DAT name=""PARAMETERS"">FULLCLASSIFICATION=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select id from CUSTOMS.customsitems where fullclassification =@FULLCLASSIFICATION  </DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select id from CUSTOMS.customsitems where fullclassification =@FULLCLASSIFICATION  </DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">false</DAT>
<DAT name=""PARAMETERS_TYPE"">FULLCLASSIFICATION=varchar,13</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A95</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFFORMS</DAT>
<DAT name=""REFERENCE"">CONSIGNMENTS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select ConsignmentNumber, CargoTypeCode, ManifestNumber, SecondCargoId, ThirdCargoId
from CUSTOMS.Consignments where(1=1) 
and Tenant=@Tenant and DeclarationId=@DECLARATIONID</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select ConsignmentNumber, CargoTypeCode, ManifestNumber, SecondCargoId, ThirdCargoId
from CUSTOMS.Consignments where(1=1) 
and Tenant=@Tenant and DeclarationId=@DECLARATIONID</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A94</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFFORMS</DAT>
<DAT name=""REFERENCE"">CONSIGNMENTS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select count(ConsignmentNumber) from CUSTOMS.Consignments where(1=1) and 
Tenant=@Tenant and DeclarationId=@DECLARATIONID</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select count(ConsignmentNumber) from CUSTOMS.Consignments where(1=1) and 
Tenant=@Tenant and DeclarationId=@DECLARATIONID</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A93</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFMAIN</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONNUMBER=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID from CUSTOMS.DECLARATIONS WHERE DeclarationNumber =@DECLARATIONNUMBER and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID from CUSTOMS.DECLARATIONS WHERE DeclarationNumber =@DECLARATIONNUMBER and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A92</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIHRSA</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONNUMBER=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select DECLARATIONS.CUSTOMFILENO from CUSTOMS.DECLARATIONS where DECLARATIONS.DECLARATIONNUMBER = @DECLARATIONNUMBER  and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select DECLARATIONS.CUSTOMFILENO from CUSTOMS.DECLARATIONS where DECLARATIONS.DECLARATIONNUMBER = @DECLARATIONNUMBER  and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A91</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFUMOTORINV</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">ID=False</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select LOADINGFACTOR from CUSTOMS.Declarations where id=@ID AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select LOADINGFACTOR from CUSTOMS.Declarations where id=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A90</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIHYFI</DAT>
<DAT name=""REFERENCE"">CONSIGNMENTS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CARGOTYPECODE,MANIFESTNUMBER,SECONDCARGOID,THIRDCARGOID FROM CUSTOMS.CONSIGNMENTS  where CONSIGNMENTS.DECLARATIONID =@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CARGOTYPECODE,MANIFESTNUMBER,SECONDCARGOID,THIRDCARGOID FROM CUSTOMS.CONSIGNMENTS  where CONSIGNMENTS.DECLARATIONID =@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-1,1,1,1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A89</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIHYFI</DAT>
<DAT name=""REFERENCE"">CONSIGNMENTS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CARGOTYPECODE,MANIFESTNUMBER,THIRDCARGOID FROM CUSTOMS.CONSIGNMENTS  where CONSIGNMENTS.DECLARATIONID =@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CARGOTYPECODE,MANIFESTNUMBER,THIRDCARGOID FROM CUSTOMS.CONSIGNMENTS  where CONSIGNMENTS.DECLARATIONID =@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-1,1,1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A88</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIFAUTOPAY</DAT>
<DAT name=""REFERENCE"">SUPPLIERINVOICES</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select INCOTERMCODE from CUSTOMS.SUPPLIERINVOICES where DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select INCOTERMCODE from CUSTOMS.SUPPLIERINVOICES where DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A87</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIHMAIN</DAT>
<DAT name=""REFERENCE"">DECLARATIONS</DAT>
<DAT name=""PARAMETERS"">EXTERNALDECLARATIONNUMBER=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select ID from CUSTOMS.DECLARATIONS where EXTERNALDECLARATIONNUMBER=@EXTERNALDECLARATIONNUMBER and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select ID from CUSTOMS.DECLARATIONS where EXTERNALDECLARATIONNUMBER=@EXTERNALDECLARATIONNUMBER and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12&uSEP;EXTERNALDECLARATIONNUMBER=varchar,35</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A86</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIHMAIN</DAT>
<DAT name=""REFERENCE"">CUSTOMSDOCUMENTS</DAT>
<DAT name=""PARAMETERS"">DOCUMENTSFILINGID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT IsPartOfDeclaration,DOCUMENTTYPECODE FROM CUSTOMS.CustomsDocuments WHERE DOCUMENTSFILINGID=@DOCUMENTSFILINGID AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT IsPartOfDeclaration,DOCUMENTTYPECODE FROM CUSTOMS.CustomsDocuments WHERE DOCUMENTSFILINGID=@DOCUMENTSFILINGID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1,1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A85</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIHMAIN</DAT>
<DAT name=""REFERENCE"">Select UNLOADPORTCODE</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select UNLOADPORTCODE from CUSTOMS.CONSIGNMENTS where DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select UNLOADPORTCODE from CUSTOMS.CONSIGNMENTS where DECLARATIONID=@DECLARATIONID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A84</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIRMEVAKERREJ</DAT>
<DAT name=""REFERENCE"">Select count(LINENUMBER)</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select count(LINENUMBER) from CUSTOMS.SUPPLIERINVOICEITEMS where DECLARATIONID='1-1' AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select count(LINENUMBER) from CUSTOMS.SUPPLIERINVOICEITEMS where DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A83</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIRMEVAKERREJ</DAT>
<DAT name=""REFERENCE"">Select count(DECLARATIONID)</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select count(DECLARATIONID) from CUSTOMS.SUPPLIERINVOICES where DECLARATIONID='1-1' AND TENANT=1</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select count(DECLARATIONID) from CUSTOMS.SUPPLIERINVOICES where DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A82</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CCUHMAIN.Lp_HAWB_Details</DAT>
<DAT name=""REFERENCE"">CCUHMAIN.Lp_HAWB_Details</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select DECLARATIONSTATUSTYPECODE,ISCLOSE,ISCANCELLED from CUSTOMS.Declarations where Declarations.ID = '1-1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select DECLARATIONSTATUSTYPECODE,ISCLOSE,ISCANCELLED from CUSTOMS.Declarations where Declarations.ID = @ID and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>12	2066 6	0</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A81</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CFIUDEL.IsOK2Delete</DAT>
<DAT name=""REFERENCE"">CFIUDEL.IsOK2Delete</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select COUNT(*) from CUSTOMS.CUSTOMSREQUESTSSHEETS where CUSTOMSREQUESTSSHEETS.INTERFACETYPECODE = '2755' and CUSTOMSREQUESTSSHEETS.CUSTOMFILENO = '123' and TENANT=1</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select COUNT(*) from CUSTOMS.CUSTOMSREQUESTSSHEETS where CUSTOMSREQUESTSSHEETS.INTERFACETYPECODE = '2755' and CUSTOMSREQUESTSSHEETS.CUSTOMFILENO = @CUSTOMFILENO and TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A80</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>PrepareCourierDefault</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>PrepareCourierDefault</DAT>
<DAT name=""REFERENCE"">GGGQWBLOGITUDE.Lp_PrepareCourierDefault</DAT>
<DAT name=""PARAMETERS"">CODE=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select * from %%vTable%%%s where code = '1-1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select * from CUSTOMS.@CLOSE_TABLE where code = @CODE</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">false</DAT>
<DAT name=""PARAMETERS_TYPE""></DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A79</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>CENFCARS.Lp_Get_Freight_NIS</DAT>
<DAT name=""REFERENCE"">CENFCARS.Lp_Get_Freight_NIS</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select AMOUNT FROM CUSTOMS.SupplierInvoiceFreightAmounts
where TENANT=@Tenant
and DECLARATIONID=@DECLARATIONID
and INVOICECOUNTERKEY='1'
and CURRENCYTYPECODE='ILS'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select AMOUNT FROM CUSTOMS.SupplierInvoiceFreightAmounts
where TENANT=@Tenant
and DECLARATIONID=@DECLARATIONID
and INVOICECOUNTERKEY='1'
and CURRENCYTYPECODE='ILS'</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A68</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select InvoiceCounterKey, InvoiceItemLineNumber from SupplierInvoiceItemVehicles</DAT>
<DAT name=""REFERENCE"">CENFMAIN.Lp_UpdateCars</DAT>
<DAT name=""PARAMETERS"">DeclarationId=True&uSEP;VehicleTypeCode=True&uSEP;VehicleChassisNumber=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select INVOICECOUNTERKEY, INVOICEITEMLINENUMBER from CUSTOMS.SUPPLIERINVOICEITEMVEHICLES where TENANT='1' and DECLARATIONID='1-111' and VEHICLETYPECODE='1' and VEHICLECHASSISNUMBER='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select INVOICECOUNTERKEY, INVOICEITEMLINENUMBER from CUSTOMS.SUPPLIERINVOICEITEMVEHICLES where TENANT=@Tenant and DECLARATIONID=@DECLARATIONID and VEHICLETYPECODE=@VEHICLETYPECODE and VEHICLECHASSISNUMBER=@VEHICLECHASSISNUMBER</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;VEHICLETYPECODE=varchar,4&uSEP;VEHICLECHASSISNUMBER=varchar,20</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A69</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select ClassificationCode, TradeAgreemenCode, OriginCountryCode, InvoiceQuantityType</DAT>
<DAT name=""REFERENCE"">CENFMAIN.Lp_UpdateCars</DAT>
<DAT name=""PARAMETERS"">ClassificationCode=True&uSEP;TradeAgreemenCode=True&uSEP;OriginCountryCode=True&uSEP;InvoiceQuantityType=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select CLASSIFICATIONCODE, TRADEAGREEMENTCODE, ORIGINCOUNTRYCODE, INVOICEQUANTITYTYPE from CUSTOMS.SUPPLIERINVOICEITEMS where TENANT='1' and  DECLARATIONID='1-111' and COUNTERKEY='1' and LINENUMBER='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select CLASSIFICATIONCODE, TRADEAGREEMENTCODE, ORIGINCOUNTRYCODE, INVOICEQUANTITYTYPE from CUSTOMS.SUPPLIERINVOICEITEMS where TENANT=@Tenant and  DECLARATIONID=@DECLARATIONID and COUNTERKEY=@COUNTERKEY and LINENUMBER=@LINENUMBER</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;COUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A70</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select * from SUPPLIERINVIOCEITEMCERTIFICATS</DAT>
<DAT name=""REFERENCE"">CENFMAIN.Lp_UpdateCars</DAT>
<DAT name=""PARAMETERS"">DeclarationId=True&uSEP;InvoiceCounterKey=True&uSEP;LineNumber=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select * from CUSTOMS.SUPPLIERINVIOCEITEMCERTIFICATS where TENANT='1' and  DECLARATIONID='1-111' and INVOICECOUNTERKEY='1' and LINENUMBER='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select * from CUSTOMS.SUPPLIERINVIOCEITEMCERTIFICATS where TENANT=@Tenant and  DECLARATIONID=@DECLARATIONID and INVOICECOUNTERKEY=@INVOICECOUNTERKEY and LINENUMBER=@LINENUMBER</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A71</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select top 1  * from customs.CustomsDocumentsTickets</DAT>
<DAT name=""REFERENCE"">CENFMAIN.Lp_UpdateCars</DAT>
<DAT name=""PARAMETERS"">DOCUMENTSFILINGID=True&uSEP;ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select top 1  * from customs.CustomsDocumentsTickets t inner join customs.CustomsDocumentPointers p on t.ID= p.CUSTOMSDOCUMENTSTICKETID inner join 
customs.Declarations d on p.PARENTENTITYID = d.id where t.DOCUMENTSFILINGID='DOC_ID' and d.ID ='LOGIDUTE_FILE_ID' and d.Tenant = 6</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select top 1  * from customs.CustomsDocumentsTickets t inner join customs.CustomsDocumentPointers p on t.ID= p.CUSTOMSDOCUMENTSTICKETID inner join 
customs.Declarations d on p.PARENTENTITYID = d.id where t.DOCUMENTSFILINGID=@DOCUMENTSFILINGID and d.ID =@ID and d.Tenant = @Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40&uSEP;ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A72</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select STORAGESITENAME from CUSTOMS.DECLARATIONS</DAT>
<DAT name=""REFERENCE"">CENFMAIN.Lp_Get_StorageSite</DAT>
<DAT name=""PARAMETERS"">Id=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select STORAGESITENAME from CUSTOMS.DECLARATIONS where TENANT='6' and ID='11'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select STORAGESITENAME from CUSTOMS.DECLARATIONS where TENANT=@Tenant and ID=@ID</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A73</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select CONSIGNMENTNUMBER, STORAGESITECODE from CUSTOMS.CONSIGNMENTS</DAT>
<DAT name=""REFERENCE"">CENFMAIN.Lp_Get_StorageSite</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select CONSIGNMENTNUMBER, STORAGESITECODE from CUSTOMS.CONSIGNMENTS where TENANT='6' and DECLARATIONID='11'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select CONSIGNMENTNUMBER, STORAGESITECODE from CUSTOMS.CONSIGNMENTS where TENANT=@Tenant and DECLARATIONID=@DECLARATIONID</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A74</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT ID from Declarations </DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Get_LogiFile</DAT>
<DAT name=""PARAMETERS"">Declarations.CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT ID FROM Declarations WHERE Declarations.CUSTOMFILENO = '5043' AND Declarations.AmendmentDontDisplayInList = 0 and TENANT=1</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT ID FROM Customs.Declarations WHERE Declarations.CUSTOMFILENO = @CUSTOMFILENO AND Declarations.AmendmentDontDisplayInList = 0 AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">455993853</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A75</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select invoicenumber,InvoiceCurrencyTypeCode,IssueCountryCode from supplierinvoices</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Currency_Check2</DAT>
<DAT name=""PARAMETERS"">DeclarationId=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select invoicenumber,InvoiceCurrencyTypeCode,IssueCountryCode from Customs.supplierinvoices where  Tenant=@Tenant and DeclarationId=@DeclarationId</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select invoicenumber,InvoiceCurrencyTypeCode,IssueCountryCode from Customs.supplierinvoices where Tenant=@Tenant and DeclarationId=@DeclarationId</DAT>
<DAT name=""EXAMPLE_RESULT"">111,3333,1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A76</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Select TAXRATE,TAXBASEAMOUNT from SUPPLIERINVOICEITEMSTAXES</DAT>
<DAT name=""REFERENCE"">CFIRDEC</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select TAXRATE,TAXBASEAMOUNT from Customs.SUPPLIERINVOICEITEMSTAXES where SUPPLIERINVOICEITEMSTAXES.DECLARATIONID = @DECLARATIONID  and SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY = @INVOICECOUNTERKEY and SUPPLIERINVOICEITEMSTAXES.LINENUMBER = @LINENUMBER and SUPPLIERINVOICEITEMSTAXES.TENANT = @Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select TAXRATE,TAXBASEAMOUNT from Customs.SUPPLIERINVOICEITEMSTAXES where SUPPLIERINVOICEITEMSTAXES.DECLARATIONID = @DECLARATIONID  and SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY = @INVOICECOUNTERKEY and SUPPLIERINVOICEITEMSTAXES.LINENUMBER = @LINENUMBER and SUPPLIERINVOICEITEMSTAXES.TENANT = @Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">111,3333</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A77</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select count(*) from SupplierInvioceItemCertificats</DAT>
<DAT name=""REFERENCE"">CFIFFORMS</DAT>
<DAT name=""PARAMETERS"">DeclarationId=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select count(*) from SupplierInvioceItemCertificats where(1=1) and Tenant='1' and DeclarationId='45345' and CertificateExemptionTypeCode in ('60','61','62','63')</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select count(*) from Customs.SupplierInvioceItemCertificats where(1=1) and Tenant=@Tenant and DeclarationId=@DeclarationId and CertificateExemptionTypeCode in ('60','61','62','63')</DAT>
<DAT name=""EXAMPLE_RESULT"">4</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A78</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select Documentsfilings.id from DECLARATIONS,Documentsfilings</DAT>
<DAT name=""REFERENCE"">HYBRID SERVICE</DAT>
<DAT name=""PARAMETERS"">DeclarationId=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>test</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select Documentsfilings.id from Customs.DECLARATIONS,dbo.Documentsfilings where
documentsfilings.externalentityreference=declarations.customfileno and
declarations.customfileno is not null and
declarations.hatradate >= cast(@hatradate as date) and
DECLARATIONS.tenant =@Tenant  and
Documentsfilings.tenant=@Tenant and
(externalentityname=@Entname or externalentityname is null)
order by  hatradate
OFFSET @OFFSETNUM ROWS FETCH NEXT @NEXTNUM ROWS ONLY
</DAT>
<DAT name=""EXAMPLE_RESULT"">4645</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">hatradate=date,1&uSEP;Entname=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A1</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT LoadingFactor</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_LogiDeclarationsDB</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT LoadingFactor FROM declarations WHERE CUSTOMFILENO ='51340315'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT LoadingFactor FROM Customs.declarations WHERE CUSTOMFILENO = @CUSTOMFILENO AND TENANT = @Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1.0661107449</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A10</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get FACILITATIONTYPECODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_FACILITATION,GMNLRUNI_VB.Lp_LogoIntialize</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>ID=False&uSEP;CODE=False</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT FACILITATIONTYPECODE FROM CLIENTS WHERE ID='1-261' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT FACILITATIONTYPECODE FROM Customs.CLIENTS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A11</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get DECLARATIONCOURIERSTATUSES.FASTINDIVIDUALPROCESSCODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_FastindividualProcessCode</DAT>
<DAT name=""PARAMETERS"">DECLARATIONCOURIERSTATUSES.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONCOURIERSTATUSES.FASTINDIVIDUALPROCESSCODE FROM DECLARATIONCOURIERSTATUSES WHERE DECLARATIONCOURIERSTATUSES.DECLARATIONID='1234' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONCOURIERSTATUSES.FASTINDIVIDUALPROCESSCODE FROM Customs.DECLARATIONCOURIERSTATUSES WHERE DECLARATIONCOURIERSTATUSES.DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-555</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A12</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get DECLARATIONS.ACCEPTANCESTATUSCODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_AvailabilityCode</DAT>
<DAT name=""PARAMETERS"">DECLARATIONS.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ACCEPTANCESTATUSCODE FROM DECLARATIONS WHERE DECLARATIONS.ID='1-7054' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ACCEPTANCESTATUSCODE FROM Customs.DECLARATIONS WHERE DECLARATIONS.ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A13</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get Count(*) from SUPPLIERINVOICES</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_SupplierInvoices_QTY,GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">SUPPLIERINVOICES.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT COUNT(*) FROM SUPPLIERINVOICES WHERE SUPPLIERINVOICES.DECLARATIONID='1-7606' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT COUNT(*) FROM Customs.SUPPLIERINVOICES WHERE SUPPLIERINVOICES.DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A14</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get Count(*) from DECLARATIONPAYMENTMETHODS</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_National_Insurance_Payment</DAT>
<DAT name=""PARAMETERS"">DECLARATIONPAYMENTMETHODS.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT COUNT(*) FROM DECLARATIONPAYMENTMETHODS WHERE DECLARATIONPAYMENTMETHODS.DECLARATIONID='1-100917' AND DECLARATIONPAYMENTMETHODS.METHODTYPECODE='11'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT COUNT(*) FROM Customs.DECLARATIONPAYMENTMETHODS WHERE (DECLARATIONPAYMENTMETHODS.DECLARATIONID=@DECLARATIONID AND DECLARATIONPAYMENTMETHODS.METHODTYPECODE=@METHODTYPECODE) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">0</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;METHODTYPECODE=varchar,3</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A15</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get Select Count(*) from DECLARATIONS</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_National_Insurance_Payment</DAT>
<DAT name=""PARAMETERS"">DECLARATIONS.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>Select Count(*) from DECLARATIONS where DECLARATIONS.ID='1-100533' and (DECLARATIONS.IMPORTERENTITLEMENTTYPECODE='17' or DECLARATIONS.IMPORTERENTITLEMENTTYPECODE='18')</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>Select Count(*) from Customs.DECLARATIONS where DECLARATIONS.ID=@ID and (DECLARATIONS.IMPORTERENTITLEMENTTYPECODE='17' or DECLARATIONS.IMPORTERENTITLEMENTTYPECODE='18') AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">0</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A16</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get ENTITLEIMPORTERID,ENTITLEIMPORTERCODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_PrivacyProtection,Lp_PrivacyProtectionEvent,GDMFCFIFILEM.Lp_PrivacyProtection,CUSTOMFILENO.Lp_DelPrivacyProtection</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT ENTITLEIMPORTERID,ENTITLEIMPORTERCODE FROM Declarations WHERE CUSTOMFILENO='51340294'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT ENTITLEIMPORTERID,ENTITLEIMPORTERCODE FROM Customs.Declarations WHERE CUSTOMFILENO=@CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-261	511525743</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A17</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Select DECLARATIONS.ID</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Si_Test</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID FROM DECLARATIONS WHERE CUSTOMFILENO='51340294' and TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID FROM Customs.DECLARATIONS WHERE CUSTOMFILENO=@CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-6499</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A18</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Select SUPPLIERINVOICEITEMS.DECLARATIONID,SUPPLIERINVOICEITEMS.COUNTERKEY,SUPPLIERINVOICEITEMS.LINENUMBER,SUPPLIERINVOICEITEMS.ORIGINCOUNTRYCODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Si_Test</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVOICEITEMS.DECLARATIONID,SUPPLIERINVOICEITEMS.COUNTERKEY,SUPPLIERINVOICEITEMS.LINENUMBER,SUPPLIERINVOICEITEMS.ORIGINCOUNTRYCODE FROM SUPPLIERINVOICEITEMS WHERE DECLARATIONID='1-6286' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVOICEITEMS.DECLARATIONID,SUPPLIERINVOICEITEMS.COUNTERKEY,SUPPLIERINVOICEITEMS.LINENUMBER,SUPPLIERINVOICEITEMS.ORIGINCOUNTRYCODE FROM Customs.SUPPLIERINVOICEITEMS WHERE DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-6286	1	1	IL</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A19</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT SUM(SUPPLIERINVOICEITEMSTAXES.TAXAMOUNT) FROM SUPPLIERINVOICEITEMSTAXES</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Si_Test</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>SUPPLIERINVOICEITEMSTAXES.DECLARATIONID=True&uSEP;SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY=True&uSEP;SUPPLIERINVOICEITEMSTAXES.LINENUMBER=True&uSEP;SUPPLIERINVOICEITEMSTAXES.TAXTYPECODE=False</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT SUM(SUPPLIERINVOICEITEMSTAXES.TAXAMOUNT) FROM SUPPLIERINVOICEITEMSTAXES WHERE 
	SUPPLIERINVOICEITEMSTAXES.DECLARATIONID = '1-6442' AND 
	SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY = '1' AND 
	SUPPLIERINVOICEITEMSTAXES.LINENUMBER = '1' AND SUPPLIERINVOICEITEMSTAXES.TAXTYPECODE = '1'
 AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT SUM(SUPPLIERINVOICEITEMSTAXES.TAXAMOUNT) FROM Customs.SUPPLIERINVOICEITEMSTAXES WHERE 
	SUPPLIERINVOICEITEMSTAXES.DECLARATIONID = @DECLARATIONID AND 
	SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY = @INVOICECOUNTERKEY AND 
	SUPPLIERINVOICEITEMSTAXES.LINENUMBER = @LINENUMBER AND SUPPLIERINVOICEITEMSTAXES.TAXTYPECODE = '1'
 AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">2266</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>

<OCC>
<DAT name=""CODE"">A2</DAT>
<DAT name=""NAME_ENG"">CONTAINERNUBMER,LIMITDATE,CHECKID</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_ScreenerDates</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>AMENDMENTORIGINALDECLARTATION=True&uSEP;ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CONTAINERNUBMER,TO_CHAR(LIMITDATE,'DD/MM/YYYY HH24:MI'),CHECKID FROM PHYSICALCHECKS WHERE PHYSICALCHECKS.DECLARATIONID IN (SELECT id FROM declarations WHERE id = '1-202' OR AMENDMENTORIGINALDECLARTATION = '1-202') AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CONTAINERNUBMER,format(LIMITDATE,'dd/MM/yyyy hh:mm'),CHECKID FROM Customs.PHYSICALCHECKS WHERE PHYSICALCHECKS.DECLARATIONID IN (SELECT id FROM Customs.declarations WHERE id = @id OR AMENDMENTORIGINALDECLARTATION = @AMENDMENTORIGINALDECLARTATION) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>SUDU3070079	22/09/2022 17:00	2289475</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;AMENDMENTORIGINALDECLARTATION=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A20</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Select COUNT(*) FROM  SUPPLIERINVOICEITEMS</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_TaxExemptCode</DAT>
<DAT name=""PARAMETERS"">SUPPLIERINVOICEITEMS.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'> SELECT COUNT(*) FROM  SUPPLIERINVOICEITEMS WHERE  SUPPLIERINVOICEITEMS.DECLARATIONID='1-6286' AND SUPPLIERINVOICEITEMS.TAXEXEMPTCODE IS NULL AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'> SELECT COUNT(*) FROM  Customs.SUPPLIERINVOICEITEMS WHERE  SUPPLIERINVOICEITEMS.DECLARATIONID=@DECLARATIONID AND SUPPLIERINVOICEITEMS.TAXEXEMPTCODE IS NULL AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A21</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT DECLARATIONS.TRANSFERIMPORTERCODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_TransferImporter</DAT>
<DAT name=""PARAMETERS"">DECLARATIONS.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.TRANSFERIMPORTERCODE FROM DECLARATIONS WHERE DECLARATIONS.ID='1-7297' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.TRANSFERIMPORTERCODE FROM Customs.DECLARATIONS WHERE DECLARATIONS.ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">049028392</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A22</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT DECLARATIONS.TRANSFERIMPORTERID</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_TransferImporter</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.TRANSFERIMPORTERID FROM DECLARATIONS WHERE CUSTOMFILENO='5312' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.TRANSFERIMPORTERID FROM Customs.DECLARATIONS WHERE CUSTOMFILENO=@CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-263</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A23</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT CLIENTS.CODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_TransferImporter</DAT>
<DAT name=""PARAMETERS"">CLIENTS.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CLIENTS.CODE FROM CLIENTS WHERE CLIENTS.ID='1-270'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CLIENTS.CODE FROM Customs.CLIENTS WHERE CLIENTS.ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">562477760</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A24</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT DECLARATIONTAXES.TOTALAMOUNT</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_TotalAmount</DAT>
<DAT name=""PARAMETERS"">DECLARATIONTAXES.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONTAXES.TOTALAMOUNT FROM  DECLARATIONTAXES WHERE  DECLARATIONTAXES.DECLARATIONID='1-6271' AND DECLARATIONTAXES.TAXTYPECODE ='16' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONTAXES.TOTALAMOUNT FROM  Customs.DECLARATIONTAXES WHERE  DECLARATIONTAXES.DECLARATIONID=@DECLARATIONID AND DECLARATIONTAXES.TAXTYPECODE =@TAXTYPECODE AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1757</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;TAXTYPECODE=varchar,3</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A25</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT DECLARATIONS.ID</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Prat_List</DAT>
<DAT name=""PARAMETERS"">DECLARATIONS.CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID FROM DECLARATIONS WHERE DECLARATIONS.CUSTOMFILENO='5312' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID FROM Customs.DECLARATIONS WHERE DECLARATIONS.CUSTOMFILENO=@CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-7297</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A26</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT IsPartOfDeclaration</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_BuildTableFiling</DAT>
<DAT name=""PARAMETERS"">DOCUMENTSFILINGID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT IsPartOfDeclaration FROM CustomsDocuments WHERE DOCUMENTSFILINGID='a3wt3rtawuqt4fpgovfaxg00000000' AND CUSTOMSDOCID IS NOT NULL  AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT IsPartOfDeclaration FROM Customs.CustomsDocuments WHERE DOCUMENTSFILINGID=@DOCUMENTSFILINGID AND CUSTOMSDOCID IS NOT NULL  AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A27</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT DISTINCT(DECLARATIONID)</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_RetrieveProcessType</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>PROCESSTYPECODE=True&uSEP;DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DISTINCT(DECLARATIONID) FROM SUPPLIERINVOICEITEMPROCESTYPES WHERE PROCESSTYPECODE='1100105' AND DECLARATIONID IN  ('1-100535','1-100536') AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DISTINCT(DECLARATIONID) FROM Customs.SUPPLIERINVOICEITEMPROCESTYPES WHERE PROCESSTYPECODE=@PROCESSTYPECODE AND DECLARATIONID IN  (@DECLARATIONID) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-100535 1-100536</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">PROCESSTYPECODE=varchar,7&uSEP;DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A28</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT CUSTOMSVENDORS.VENDORNUMBER,CUSTOMSVENDORS.VENDORNAME</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Read</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CUSTOMSVENDORS.VENDORNUMBER,CUSTOMSVENDORS.VENDORNAME FROM CUSTOMSVENDORS WHERE ID='1-582' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CUSTOMSVENDORS.VENDORNUMBER,CUSTOMSVENDORS.VENDORNAME FROM Customs.CUSTOMSVENDORS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>2657119	DEEJAY.DE</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A29</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT SUPPLIERINVOICEFREIGHTAMOUNTS.CURRENCYTYPECODE</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Read</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>SUPPLIERINVOICEFREIGHTAMOUNTS.DECLARATIONID=True&uSEP;SUPPLIERINVOICEFREIGHTAMOUNTS.INVOICECOUNTERKEY=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVOICEFREIGHTAMOUNTS.CURRENCYTYPECODE FROM SUPPLIERINVOICEFREIGHTAMOUNTS WHERE 	 SUPPLIERINVOICEFREIGHTAMOUNTS.DECLARATIONID='1-101415' AND SUPPLIERINVOICEFREIGHTAMOUNTS.INVOICECOUNTERKEY='1'	 AND TENANT=1</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVOICEFREIGHTAMOUNTS.CURRENCYTYPECODE FROM Customs.SUPPLIERINVOICEFREIGHTAMOUNTS WHERE 	 SUPPLIERINVOICEFREIGHTAMOUNTS.DECLARATIONID=@DECLARATIONID AND SUPPLIERINVOICEFREIGHTAMOUNTS.INVOICECOUNTERKEY=@INVOICECOUNTERKEY AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>EUR USD</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A3</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>get DECLARATIONS.ID,CustomFileNo,ImporterId,importername,FullName,DeclarationNumber,VersionId,TotalTax,CLIENTS.CODE</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_ConfirmationOfDocuments</DAT>
<DAT name=""PARAMETERS"">DECLARATIONS.CustomFileNo=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID,CustomFileNo,ImporterId,importername,FullName,DeclarationNumber,VersionId,TotalTax,CLIENTS.CODE
FROM DECLARATIONS, CLIENTS WHERE DECLARATIONS.ImporterId = CLIENTS.ID(+) AND DECLARATIONS.CustomFileNo = '51340315'
AND DECLARATIONS.TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONS.ID,CustomFileNo,ImporterId,importername,FullName,DeclarationNumber,VersionId,TotalTax,CLIENTS.CODE
FROM Customs.DECLARATIONS, Customs.CLIENTS WHERE (DECLARATIONS.ImporterId = CLIENTS.ID AND DECLARATIONS.CustomFileNo = @CustomFileNo) and DECLARATIONS.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-6826	51340315	1-263		ירון כהן	22041168208005	0.6	527	049028392</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A30</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT SUPPLIERINVOICEITEMSTAXES.TAXRATE</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Read</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>SUPPLIERINVOICEITEMSTAXES.DECLARATIONID=True&uSEP;SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY=True&uSEP;SUPPLIERINVOICEITEMSTAXES.LINENUMBER=True&uSEP;SUPPLIERINVOICEITEMSTAXES.TAXTYPECODE=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVOICEITEMSTAXES.TAXRATE  FROM SUPPLIERINVOICEITEMSTAXES WHERE 
	SUPPLIERINVOICEITEMSTAXES.DECLARATIONID = '1-6442' AND 
	SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY = '1' AND 
	SUPPLIERINVOICEITEMSTAXES.LINENUMBER = '1' AND SUPPLIERINVOICEITEMSTAXES.TAXTYPECODE = '1'
 AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVOICEITEMSTAXES.TAXRATE  FROM Customs.SUPPLIERINVOICEITEMSTAXES WHERE 
	SUPPLIERINVOICEITEMSTAXES.DECLARATIONID = @DECLARATIONID AND 
	SUPPLIERINVOICEITEMSTAXES.INVOICECOUNTERKEY = @INVOICECOUNTERKEY AND 
	SUPPLIERINVOICEITEMSTAXES.LINENUMBER = @LINENUMBER AND SUPPLIERINVOICEITEMSTAXES.TAXTYPECODE = @TAXTYPECODE
 AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">2266</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A31</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT SUPPLIERINVIOCEITEMCERTIFICATS.CERTIFICATENUMBER</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Read</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>SUPPLIERINVIOCEITEMCERTIFICATS.DECLARATIONID=True&uSEP;SUPPLIERINVIOCEITEMCERTIFICATS.INVOICECOUNTERKEY=True&uSEP;SUPPLIERINVIOCEITEMCERTIFICATS.LINENUMBER=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVIOCEITEMCERTIFICATS.CERTIFICATENUMBER FROM SUPPLIERINVIOCEITEMCERTIFICATS WHERE 
	SUPPLIERINVIOCEITEMCERTIFICATS.DECLARATIONID = '1-6442' AND 
	SUPPLIERINVIOCEITEMCERTIFICATS.INVOICECOUNTERKEY = '1' AND 
	SUPPLIERINVIOCEITEMCERTIFICATS.LINENUMBER = '1' AND SUPPLIERINVIOCEITEMCERTIFICATS.TENANT = '1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT SUPPLIERINVIOCEITEMCERTIFICATS.CERTIFICATENUMBER FROM Customs.SUPPLIERINVIOCEITEMCERTIFICATS WHERE 
	SUPPLIERINVIOCEITEMCERTIFICATS.DECLARATIONID = @DECLARATIONID AND 
	SUPPLIERINVIOCEITEMCERTIFICATS.INVOICECOUNTERKEY = @INVOICECOUNTERKEY AND 
	SUPPLIERINVIOCEITEMCERTIFICATS.LINENUMBER = @LINENUMBER AND SUPPLIERINVIOCEITEMCERTIFICATS.TENANT = @Tenant</DAT>
<DAT name=""EXAMPLE_RESULT""></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A32</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT MODIFICATIONANDDISCOUNTTYPES.EXTRANUMERICDATA</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Read</DAT>
<DAT name=""PARAMETERS"">MODIFICATIONANDDISCOUNTTYPES.CODE=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT MODIFICATIONANDDISCOUNTTYPES.EXTRANUMERICDATA FROM MODIFICATIONANDDISCOUNTTYPES WHERE MODIFICATIONANDDISCOUNTTYPES.CODE = 'I21'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT MODIFICATIONANDDISCOUNTTYPES.EXTRANUMERICDATA FROM Customs.MODIFICATIONANDDISCOUNTTYPES WHERE MODIFICATIONANDDISCOUNTTYPES.CODE = @CODE</DAT>
<DAT name=""EXAMPLE_RESULT"">2</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">false</DAT>
<DAT name=""PARAMETERS_TYPE"">CODE=varchar,3</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A33</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALS.ID</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Get_Mas_Arvut,Lp_Get_Collateral_Amount</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>CUSTOMSCOLLATERALS.DECLARATIONID=True&uSEP;CUSTOMSCOLLATERALS.COLLATERALREQUESTSTATUSCODE=False</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALS.ID FROM CUSTOMSCOLLATERALS WHERE CUSTOMSCOLLATERALS.DECLARATIONID='1-100845' AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALS.ID FROM Customs.CUSTOMSCOLLATERALS WHERE CUSTOMSCOLLATERALS.DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-348</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;INVOICECOUNTERKEY=int,1&uSEP;LINENUMBER=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A34</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALSANSWERS.CUSTOMSTAPGFILE,CUSTOMSCOLLATERALSANSWERS.CUSTOMSNUMERAL</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Get_Mas_Arvut</DAT>
<DAT name=""PARAMETERS"">CUSTOMSCOLLATERALID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALSANSWERS.CUSTOMSTAPGFILE,CUSTOMSCOLLATERALSANSWERS.CUSTOMSNUMERAL FROM 
CUSTOMSCOLLATERALSANSWERS WHERE CUSTOMSCOLLATERALID IN ('1-320','1-325') AND TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALSANSWERS.CUSTOMSTAPGFILE,CUSTOMSCOLLATERALSANSWERS.CUSTOMSNUMERAL FROM 
Customs.CUSTOMSCOLLATERALSANSWERS WHERE CUSTOMSCOLLATERALID IN (@CUSTOMSCOLLATERALID) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>31213	1 20062022	11</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMSCOLLATERALID=varchar,15</DAT>
<DAT name=""HAS_IN_OPER"">@CUSTOMSCOLLATERALID</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A35</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Select CUSTOMSCOLLATERALSANSWERS.ALLOCATEDAMOUNT</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Get_Collateral_Amount</DAT>
<DAT name=""PARAMETERS"">CUSTOMSCOLLATERALID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALSANSWERS.ALLOCATEDAMOUNT FROM CUSTOMSCOLLATERALSANSWERS 
WHERE CUSTOMSCOLLATERALID IN ('1-325','1-324') AND CUSTOMSCOLLATERALSANSWERS.TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CUSTOMSCOLLATERALSANSWERS.ALLOCATEDAMOUNT FROM Customs.CUSTOMSCOLLATERALSANSWERS 
WHERE CUSTOMSCOLLATERALID IN (@CUSTOMSCOLLATERALID) AND CUSTOMSCOLLATERALSANSWERS.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>509 463</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMSCOLLATERALID=varchar,15</DAT>
<DAT name=""HAS_IN_OPER"">@CUSTOMSCOLLATERALID</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A36</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT SUM (CONSIGNMENTPACkAGES.GROSSMASSMEASURE)</DAT>
<DAT name=""REFERENCE"">CFIRDEC.Lp_Get_Weight_Measure</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>CONSIGNMENTPACkAGES.PACkAGEMEASUREQUALIFIERCODE=True&uSEP;CONSIGNMENTPACkAGES.DECLARATIONID=True&uSEP;CONSIGNMENTPACKAGES.GROSSMASSMEASURETYPECODE=False</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT SUM (CONSIGNMENTPACkAGES.GROSSMASSMEASURE) FROM CONSIGNMENTPACkAGES WHERE CONSIGNMENTPACkAGES.DECLARATIONID ='1-6261'
 AND CONSIGNMENTPACkAGES.PACkAGEMEASUREQUALIFIERCODE='2'AND CONSIGNMENTPACkAGES.TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT SUM (CONSIGNMENTPACkAGES.GROSSMASSMEASURE) FROM Customs.CONSIGNMENTPACkAGES WHERE CONSIGNMENTPACkAGES.DECLARATIONID =@DECLARATIONID
 AND CONSIGNMENTPACkAGES.PACKAGEMEASUREQUALIFIERCODE=@PACKAGEMEASUREQUALIFIERCODE AND CONSIGNMENTPACKAGES.GROSSMASSMEASURETYPECODE=@GROSSMASSMEASURETYPECODE AND  CONSIGNMENTPACkAGES.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">50</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;PACKAGEMEASUREQUALIFIERCODE=varchar,4&uSEP;GROSSMASSMEASURETYPECODE=varchar,3</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A37</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT CLASSIFICATIONCODE</DAT>
<DAT name=""REFERENCE"">GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">SUPPLIERINVOICEITEMS.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CLASSIFICATIONCODE FROM SUPPLIERINVOICEITEMS WHERE SUPPLIERINVOICEITEMS.DECLARATIONID='1-6286' AND TENANT=1</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CLASSIFICATIONCODE FROM Customs.SUPPLIERINVOICEITEMS WHERE SUPPLIERINVOICEITEMS.DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">84158100002</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A38</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select count(distinct(checkid||CONTAINERNUBMER))</DAT>
<DAT name=""REFERENCE"">GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">customfileno=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT COUNT(DISTINCT(checkid||CONTAINERNUBMER)) FROM PHYSICALCHECKS WHERE PHYSICALCHECKS.DECLARATIONID IN (SELECT id FROM declarations WHERE customfileno =  '5043') AND CONTAINERNUBMER IS NOT NULL AND operationcode!='3'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT COUNT(DISTINCT(checkid + CONTAINERNUBMER)) FROM Customs.PHYSICALCHECKS WHERE PHYSICALCHECKS.DECLARATIONID IN (SELECT id FROM Customs.declarations WHERE customfileno =  @customfileno AND TENANT=@Tenant) AND CONTAINERNUBMER IS NOT NULL AND operationcode!=3 AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">3</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A39</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select count(*) from consignments</DAT>
<DAT name=""REFERENCE"">GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT COUNT(*) FROM consignments WHERE DECLARATIONID = '1-100536'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT COUNT(*) FROM Customs.consignments WHERE DECLARATIONID = @DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">2</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A4</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>get D.ID,DT.TaxTypeCode,PT.LOCALNAME,DT.TotalAmount,DT.DeferredTaxAmount ,DT.TotalAmount , DT.DeferredTaxAmount</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_BuildTableTax</DAT>
<DAT name=""PARAMETERS"">D.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT D.ID,DT.TaxTypeCode,PT.LOCALNAME,DT.TotalAmount,DT.DeferredTaxAmount ,DT.TotalAmount , DT.DeferredTaxAmount  FROM DECLARATIONS D, DECLARATIONTAXES DT , PARAGRAPHTYPES PT 
WHERE D.ID = DT.DECLARATIONID(+) AND DT.TAXTYPECODE = PT.CODE(+) AND D.ID='1-101193'
 AND D.TENANT='1' AND DT.TENANT='1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT D.ID,DT.TaxTypeCode,PT.LOCALNAME,DT.TotalAmount,DT.DeferredTaxAmount ,DT.TotalAmount , DT.DeferredTaxAmount  FROM Customs.DECLARATIONS D, Customs.DECLARATIONTAXES DT , Customs.PARAGRAPHTYPES PT 
WHERE D.ID = DT.DECLARATIONID AND DT.TAXTYPECODE = PT.CODE AND D.ID=@ID AND D.TENANT=@Tenant AND DT.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-101193	15	מע""מ יבוא	99	0	99	0 1-101193	16	מס קניה יבוא	0	0	0	0</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A40</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Select GROSSMASSMEASURE</DAT>
<DAT name=""REFERENCE"">GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">COURIERMASTERS.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT GROSSMASSMEASURE FROM COURIERMASTERS WHERE COURIERMASTERS.ID='1-1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT GROSSMASSMEASURE FROM Customs.COURIERMASTERS WHERE COURIERMASTERS.ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">0</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A41</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT TOTALINVOICEAMOUNTINUSD</DAT>
<DAT name=""REFERENCE"">GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">DECLARATIONCOURIERSTATUSES.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT TOTALINVOICEAMOUNTINUSD FROM DECLARATIONCOURIERSTATUSES WHERE DECLARATIONCOURIERSTATUSES.DECLARATIONID='1-100536'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT TOTALINVOICEAMOUNTINUSD FROM Customs.DECLARATIONCOURIERSTATUSES WHERE DECLARATIONCOURIERSTATUSES.DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT""></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A42</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT COURIERCUSTOMSTATUSCODE</DAT>
<DAT name=""REFERENCE"">GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">DECLARATIONS.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT COURIERCUSTOMSTATUSCODE FROM DECLARATIONS WHERE DECLARATIONS.ID='1-6364'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT COURIERCUSTOMSTATUSCODE FROM Customs.DECLARATIONS WHERE DECLARATIONS.ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A43</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT LINE FROM DECLARATIONPAYMENTMETHODS</DAT>
<DAT name=""REFERENCE"">GPRFC.Lp_GetVars</DAT>
<DAT name=""PARAMETERS"">DECLARATIONPAYMENTMETHODS.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT LINE FROM DECLARATIONPAYMENTMETHODS WHERE DECLARATIONPAYMENTMETHODS.DECLARATIONID='1-100977' AND DECLARATIONPAYMENTMETHODS.METHODTYPECODE = '79'  AND DECLARATIONPAYMENTMETHODS.PAYERACTIVITYTYPECODE = '3'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT LINE FROM Customs.DECLARATIONPAYMENTMETHODS WHERE DECLARATIONPAYMENTMETHODS.DECLARATIONID=@DECLARATIONID AND DECLARATIONPAYMENTMETHODS.METHODTYPECODE = '79'  AND DECLARATIONPAYMENTMETHODS.PAYERACTIVITYTYPECODE = '3' AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;METHODTYPECODE=varchar,3</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A44</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT TOTALINVOICEAMOUNTINUSD</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_Do_CreatePayment</DAT>
<DAT name=""PARAMETERS"">Declarations.CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT PaymentOrderNumber FROM Declarations WHERE Declarations.CUSTOMFILENO = '5043' AND Declarations.AmendmentDontDisplayInList = 0 and TENANT=1</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT PaymentOrderNumber FROM Customs.Declarations WHERE Declarations.CUSTOMFILENO = @CUSTOMFILENO AND Declarations.AmendmentDontDisplayInList = 0 AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">455993853</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A45</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT MethodTypeCode</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_Do_CreatePayment,GDSUPAYE.Lf_GOLD_CARD</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT MethodTypeCode FROM DeclarationPaymentMethods WHERE DECLARATIONID='1-100977'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT MethodTypeCode FROM Customs.DeclarationPaymentMethods WHERE DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A46</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select DECLARATIONID</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_UpsertMasterCourier</DAT>
<DAT name=""PARAMETERS"">CourierDeclarations.COURIERMASTERID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select DECLARATIONID from CourierDeclarations where CourierDeclarations.COURIERMASTERID = '1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select DECLARATIONID from Customs.CourierDeclarations where CourierDeclarations.COURIERMASTERID = @COURIERMASTERID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">COURIERMASTERID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A47</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select CUSTOMFILENO</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_UpsertMasterCourier</DAT>
<DAT name=""PARAMETERS"">Declarations.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CUSTOMFILENO FROM Declarations WHERE Declarations.ID = '1-6467'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CUSTOMFILENO FROM Customs.Declarations WHERE Declarations.ID = @ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">51340290</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A48</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select AIRLINEID,HAWB,MAWB,TRUCKERID,ISCANCELLED,ISREADYFORINVOICE,GATEWAYPORTCODE</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_UpsertMasterCourier</DAT>
<DAT name=""PARAMETERS"">CourierMasters.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT AIRLINEID,HAWB,MAWB,TRUCKERID,ISCANCELLED,ISREADYFORINVOICE,GATEWAYPORTCODE FROM CourierMasters WHERE CourierMasters.ID = '1-1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT AIRLINEID,HAWB,MAWB,TRUCKERID,ISCANCELLED,ISREADYFORINVOICE,GATEWAYPORTCODE FROM Customs.CourierMasters WHERE CourierMasters.ID = @ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-1	74455445	1532453		0	0	</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A49</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT AIRLINEPREFIX</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_UpsertMasterCourier</DAT>
<DAT name=""PARAMETERS"">CUSTOMSAIRLINES.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT AIRLINEPREFIX FROM CUSTOMSAIRLINES WHERE CUSTOMSAIRLINES.ID = '1-1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT AIRLINEPREFIX FROM Customs.CUSTOMSAIRLINES WHERE CUSTOMSAIRLINES.ID = @ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">235</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A5</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>get D.ID,  SI.InvoiceNumber,  SI.VendorId,  SI.InvoiceAmount,  SI.InvoiceCurrencyTypeCode,  CUSTOMSVENDORS.VendorName</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_BuildTableInv</DAT>
<DAT name=""PARAMETERS"">D.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT   D.ID,  SI.InvoiceNumber,  SI.VendorId,  SI.InvoiceAmount,  SI.InvoiceCurrencyTypeCode,  CUSTOMSVENDORS.VendorName FROM Customs.DECLARATIONS D, Customs.SupplierInvoices SI ,  Customs.CUSTOMSVENDORS WHERE D.ID = SI.DECLARATIONID AND  SI.VendorId = CUSTOMSVENDORS.ID AND  D.ID=@ID
 AND D.TENANT=@Tenant AND SI.TENANT=@Tenant AND CUSTOMSVENDORS.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-101193	1212	1-582	3000	USD	DEEJAY.DE</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT   D.ID,  SI.InvoiceNumber,  SI.VendorId,  SI.InvoiceAmount,  SI.InvoiceCurrencyTypeCode,  CUSTOMSVENDORS.VendorName FROM Customs.DECLARATIONS D, Customs.SupplierInvoices SI ,  Customs.CUSTOMSVENDORS WHERE D.ID = SI.DECLARATIONID AND  SI.VendorId = CUSTOMSVENDORS.ID AND  D.ID=@ID
 AND D.TENANT=@Tenant AND SI.TENANT=@Tenant AND CUSTOMSVENDORS.TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-101193	1212	1-582	3000	USD	DEEJAY.DE</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A50</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT TO_CHAR(ESTIMATEDARRIVALDATE ,'dd/MM/yyyy')</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_UpsertMasterCourier</DAT>
<DAT name=""PARAMETERS"">CourierMasters.ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT TO_CHAR(ESTIMATEDARRIVALDATE ,'dd/MM/yyyy') FROM CourierMasters WHERE CourierMasters.ID = '1-1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT format(ESTIMATEDARRIVALDATE,'dd/MM/yyyy') FROM Customs.CourierMasters WHERE CourierMasters.ID = @ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">10.12.2023</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A51</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select DOCUMENTTYPEID</DAT>
<DAT name=""REFERENCE"">YCUHLTASK.Lp_CreateSIDocument</DAT>
<DAT name=""PARAMETERS"">DOCUMENTTYPECUSTOMSDATA.CUSTOMSDOUCUMENTTYPECODE=False</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select DOCUMENTTYPEID from DOCUMENTTYPECUSTOMSDATA where DOCUMENTTYPECUSTOMSDATA.CUSTOMSDOUCUMENTTYPECODE= '380'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select DOCUMENTTYPEID from Customs.DOCUMENTTYPECUSTOMSDATA where DOCUMENTTYPECUSTOMSDATA.CUSTOMSDOUCUMENTTYPECODE= @CUSTOMSDOUCUMENTTYPECODE AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">FSI</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMSDOUCUMENTTYPECODE=varchar,7uSEP;DOCUMENTTYPEID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A52</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT CUSTOMSDOUCUMENTTYPECODE</DAT>
<DAT name=""REFERENCE"">GCRQFILE.Lp_Get_Filing_Data,GDMFCFIFILEM.Lp_SendToCustoms</DAT>
<DAT name=""PARAMETERS"">DOCUMENTTYPECUSTOMSDATA.DOCUMENTTYPEID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CUSTOMSDOUCUMENTTYPECODE FROM DOCUMENTTYPECUSTOMSDATA WHERE DOCUMENTTYPECUSTOMSDATA.DOCUMENTTYPEID = 'LETR'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CUSTOMSDOUCUMENTTYPECODE FROM Customs.DOCUMENTTYPECUSTOMSDATA WHERE DOCUMENTTYPECUSTOMSDATA.DOCUMENTTYPEID = @DOCUMENTTYPEID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">IL_463</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMSDOUCUMENTTYPECODE=varchar,7uSEP;DOCUMENTTYPEID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A53</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT DECLARATIONSTATUSTYPECODE,INSTR(ERROSXML,'&lt;ListVersionID&gt;1')</DAT>
<DAT name=""REFERENCE"">GAQQMOVETO2.Lp_Mandatory</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DECLARATIONSTATUSTYPECODE,INSTR(ERROSXML,'&lt;ListVersionID&gt;1') FROM DECLARATIONS WHERE CUSTOMFILENO = '1023'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DECLARATIONSTATUSTYPECODE,CHARINDEX(ERROSXML,'&lt;ListVersionID&gt;1') FROM Customs.DECLARATIONS WHERE CUSTOMFILENO = @CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>12	2066 6	0</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A54</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT ID FROM CustomsDocumentsTickets</DAT>
<DAT name=""REFERENCE"">GDMFCFIFILEM.Lp_PurgeDoc</DAT>
<DAT name=""PARAMETERS"">DOCUMENTSFILINGID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT ID FROM CustomsDocumentsTickets WHERE DOCUMENTSFILINGID='qy0uqizmte23n40ucq4m+g00000000'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT ID FROM Customs.CustomsDocumentsTickets WHERE DOCUMENTSFILINGID=@DOCUMENTSFILINGID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-4563
1-4585</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DOCUMENTSFILINGID=varchar,40</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A55</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Select ID,PARENTENTITYCODE,PARENTENTITYID</DAT>
<DAT name=""REFERENCE"">GDMFCFIFILEM.Lp_PurgeDoc</DAT>
<DAT name=""PARAMETERS"">CUSTOMSDOCUMENTSTICKETID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT ID,PARENTENTITYCODE,PARENTENTITYID FROM CustomsDocumentPointers WHERE CUSTOMSDOCUMENTSTICKETID IN ('1-4561')</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT ID,PARENTENTITYCODE,PARENTENTITYID FROM Customs.CustomsDocumentPointers WHERE CUSTOMSDOCUMENTSTICKETID IN (@CUSTOMSDOCUMENTSTICKETID) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>1-4605	Declaration	1-6287</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">CUSTOMSDOCUMENTSTICKETID=varchar,15</DAT>
<DAT name=""HAS_IN_OPER"">@CUSTOMSDOCUMENTSTICKETID</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A56</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT CLAIMSUBMITERNUMBER</DAT>
<DAT name=""REFERENCE"">GDMFCFIFILEM.Lp_PurgeDoc</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT CLAIMSUBMITERNUMBER FROM Claims WHERE ID='1-551'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CLAIMSUBMITERNUMBER FROM Customs.Claims WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">550221105</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A57</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT  CustomsVendors.VendorNumber</DAT>
<DAT name=""REFERENCE"">GDMFFILE.Lp_SendSivugToOCR</DAT>
<DAT name=""PARAMETERS"">UnfInvoiceCounterKey=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT  CustomsVendors.VendorNumber   FROM CustomsVendors   WHERE id  =(SELECT vendorid FROM SupplierInvoices WHERE UnfInvoiceCounterKey='8gb9+gempkg9bdnm2kxvkg00000000')</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT  CustomsVendors.VendorNumber   FROM Customs.CustomsVendors   WHERE id  =(SELECT vendorid FROM Customs.SupplierInvoices WHERE UnfInvoiceCounterKey=@UnfInvoiceCounterKey AND TENANT=@Tenant) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">2015002</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">UNFINVOICECOUNTERKEY=varchar,30</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A58</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT IsReadyForInvoice,IsCancelled</DAT>
<DAT name=""REFERENCE"">GDSFMAIN.Lp_DQFlight_CreditLetter_Inner</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT IsReadyForInvoice,IsCancelled FROM CourierMasters WHERE Id='1-1'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT IsReadyForInvoice,IsCancelled FROM Customs.CourierMasters WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>0	0</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A59</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT Declarations.CustomFileNo</DAT>
<DAT name=""REFERENCE"">GDSFMAIN.Lp_DQFlight_CreditLetter_Inner</DAT>
<DAT name=""PARAMETERS"">CourierDeclarations.CourierMasterId=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT Declarations.CustomFileNo FROM Declarations,CourierDeclarations 
WHERE CourierDeclarations.CourierMasterId='1-1' 
AND CourierDeclarations.DeclarationId=Declarations.Id 
ORDER BY Declarations.CustomFileNo</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT Declarations.CustomFileNo FROM Customs.Declarations,Customs.CourierDeclarations 
WHERE CourierDeclarations.CourierMasterId=@CourierMasterId 
AND CourierDeclarations.DeclarationId=Declarations.Id  AND Declarations.TENANT=@Tenant AND CourierDeclarations.TENANT=@Tenant 
ORDER BY Declarations.CustomFileNo</DAT>
<DAT name=""EXAMPLE_RESULT"">2015002</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15&uSEP;COURIERMASTERID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A6</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>get SI.InvoiceNumber,SIM.TypeCode,SIM.CurrencyTypeCode,SIM.Amount</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_BuildTableInv</DAT>
<DAT name=""PARAMETERS"">DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT 
  SI.InvoiceNumber,
  SIM.TypeCode,
  SIM.CurrencyTypeCode,
  SIM.Amount 
FROM 
  SupplierInvoices SI ,
  SupplierInvoiceModifications SIM 
WHERE 
  SI.DECLARATIONID = SIM.DECLARATIONID(+) AND
  si.DECLARATIONID = sim.DECLARATIONID AND
  si.INVOICECOUNTERKEY=sim.INVOICECOUNTERKEY AND
   SIM.TypeCode = 'I10' AND si.DECLARATIONID='1-101193'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT 
  SI.InvoiceNumber,
  SIM.TypeCode,
  SIM.CurrencyTypeCode,
  SIM.Amount
FROM 
  Customs.SupplierInvoices SI ,
  Customs.SupplierInvoiceModifications SIM 
WHERE 
  SI.DECLARATIONID = SIM.DECLARATIONID AND
  si.DECLARATIONID = sim.DECLARATIONID AND
  si.INVOICECOUNTERKEY=sim.INVOICECOUNTERKEY AND
   SIM.TypeCode = 'I10' AND si.DECLARATIONID=@DECLARATIONID and
   SI.Tenant=@Tenant and SIM.Tenant=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"" xml:space='preserve'>32424	I10	EUR	12
INV5153	I10	USD	1
IN6566	I10	USD	1
75675	I10	USD	2</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">UNFINVOICECOUNTERKEY=varchar,30&uSEP;DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A60</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT IMPORTERCODE</DAT>
<DAT name=""REFERENCE"">GMNLRUNI_VB.Lp_LogoIntialize</DAT>
<DAT name=""PARAMETERS"">CUSTOMFILENO=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT IMPORTERCODE  FROM DECLARATIONS WHERE CUSTOMFILENO ='1007'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT IMPORTERCODE  FROM Customs.DECLARATIONS WHERE CUSTOMFILENO =@CUSTOMFILENO AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">511525743</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A61</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT email</DAT>
<DAT name=""REFERENCE"">GSCQOPN.Lp_Ok</DAT>
<DAT name=""PARAMETERS"">CODE=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT email FROM CONTACTS WHERE ID =(SELECT id FROM USERS WHERE code ='AMITAL' AND tenant=1 )</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT email FROM CONTACTS WHERE ID =(SELECT id FROM USERS WHERE code ='AMITAL' AND TENANT=@Tenant) AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">v5111@amital.co.il</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE""></DAT>
</OCC>
<OCC>
<DAT name=""CODE"" insert=""true"">A62</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>INSERT INTO AUTHENTICATIONTOKENS</DAT>
<DAT name=""REFERENCE"">GUHHBUILD.Lp_OnPremiseLogIn</DAT>
<DAT name=""PARAMETERS"" xml:space='preserve'>TOKEN=True&uSEP;EMAIL=True&uSEP;PASSWORD=True&uSEP;TENANT=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>INSERT INTO AUTHENTICATIONTOKENS  (TOKEN, TENANT, EMAIL, PASSWORD , CREATEDATE)    VALUES  ('aaaaaa','1','MOTI','moti@amital.co.il', SYSDATE)</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>INSERT INTO AUTHENTICATIONTOKENS  (TOKEN, TENANT, EMAIL, PASSWORD , CREATEDATE)    VALUES  (@TOKEN,@Tenant,@EMAIL,@PASSWORD, GETDATE())</DAT>
<DAT name=""EXAMPLE_RESULT""></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">false</DAT>
<DAT name=""IS_INSERT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">TOKEN=varchar,100&uSEP;EMAIL=varchar,70&uSEP;PASSWORD=varchar,60&uSEP;CREATEDATE=datetime,1&uSEP;TENANT=int,1</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A63</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT vendorid</DAT>
<DAT name=""REFERENCE"">GDMQOCRTRIFF.Lp_Read</DAT>
<DAT name=""PARAMETERS"">UnfInvoiceCounterKey=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT vendorid FROM SupplierInvoices WHERE UnfInvoiceCounterKey='8gb9+gempkg9bdnm2kxvkg00000000'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT vendorid FROM Customs.SupplierInvoices WHERE UnfInvoiceCounterKey=@UnfInvoiceCounterKey AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-579</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">UNFINVOICECOUNTERKEY=varchar,30&uSEP;DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A64</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT  CustomsVendors.VendorNumber</DAT>
<DAT name=""REFERENCE"">GDMQOCRTRIFF.Lp_Read</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT  CustomsVendors.VendorNumber   FROM CustomsVendors   WHERE id  ='1-579'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT  CustomsVendors.VendorNumber   FROM Customs.CustomsVendors   WHERE id  =@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">2015002</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A65</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>SELECT DISTINCT (SUPPLIERINVOICEITEMS.CLASSIFICATIONCODE)</DAT>
<DAT name=""REFERENCE"">GITUEXP.Lp_CheckDate</DAT>
<DAT name=""PARAMETERS"">SUPPLIERINVOICEITEMS.DECLARATIONID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT DISTINCT (SUPPLIERINVOICEITEMS.CLASSIFICATIONCODE) FROM SUPPLIERINVOICEITEMS WHERE  SUPPLIERINVOICEITEMS.DECLARATIONID='1-6546'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT DISTINCT (SUPPLIERINVOICEITEMS.CLASSIFICATIONCODE) FROM Customs.SUPPLIERINVOICEITEMS WHERE  SUPPLIERINVOICEITEMS.DECLARATIONID=@DECLARATIONID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">84158100002</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">DECLARATIONID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A66</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select Currency from VendorCurrencies</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Currency_Check</DAT>
<DAT name=""PARAMETERS"">VendorId=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>select Currency from CUSTOMS.VendorCurrencies where Tenant='1' and VendorId='1-584'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>select Currency from CUSTOMS.VendorCurrencies where Tenant=@Tenant and VendorId=@VendorId</DAT>
<DAT name=""EXAMPLE_RESULT""></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">VENDORID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A67</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>select Currency from CountryCurrencies</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Currency_Check</DAT>
<DAT name=""PARAMETERS"">CountryId=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT Currency FROM CUSTOMS.CountryCurrencies WHERE Tenant='1' AND CountryId='AF'</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT Currency FROM CUSTOMS.CountryCurrencies WHERE Tenant=@Tenant AND CountryId=@CountryId</DAT>
<DAT name=""EXAMPLE_RESULT""></DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">COUNTRYID=varchar,15</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A7</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>get VERSIONID</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_Importer_Decl_Conf</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT VERSIONID FROM Customs.DECLARATIONS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT VERSIONID FROM Customs.DECLARATIONS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">0.1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A8</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>get ISCHANGED</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_ISCHANGED</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT ISCHANGED FROM Customs.DECLARATIONS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT CONVERT(int, ISCHANGED) FROM Customs.DECLARATIONS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
<OCC>
<DAT name=""CODE"">A9</DAT>
<DAT name=""NAME_ENG"" xml:space='preserve'>Get IMPORTERID</DAT>
<DAT name=""REFERENCE"">CFIFFORMS.Lp_FACILITATION</DAT>
<DAT name=""PARAMETERS"">ID=True</DAT>
<DAT name=""EXAMPLE_SQL"" xml:space='preserve'>SELECT IMPORTERID FROM Customs.DECLARATIONS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""TEMPLATE_SQL"" xml:space='preserve'>SELECT IMPORTERID FROM Customs.DECLARATIONS WHERE ID=@ID AND TENANT=@Tenant</DAT>
<DAT name=""EXAMPLE_RESULT"">1-261</DAT>
<DAT name=""LINQ"">true</DAT>
<DAT name=""HAS_TENANT"">true</DAT>
<DAT name=""PARAMETERS_TYPE"">ID=varchar,15&uSEP;CUSTOMFILENO=varchar,12</DAT>
</OCC>
</root>";
    }
}