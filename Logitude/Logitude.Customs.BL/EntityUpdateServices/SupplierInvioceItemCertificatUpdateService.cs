using Devart.Data.Oracle;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class SupplierInvioceItemCertificatUpdateService
    {
        int? maxCounter;
        protected override void OnCreating(SupplierInvioceItemCertificatPM entityPM, SupplierInvoiceItemPM entityParentPM)
        {
            entityPM.DeclarationId = entityParentPM.DeclarationId;
            entityPM.InvoiceCounterKey = entityParentPM.CounterKey;
            entityPM.LineNumber = entityParentPM.LineNumber;
            bool yaronRevertCS7859 = false;
            ICustomContext _Context = MainContext as CustomContext;
            if (yaronRevertCS7859)
            {
                entityPM.ItemCertificateCounterKey = CodeCounter.GetNumber("Customs.SupplierInvioceItemCertificat", entityPM.Tenant);
            }
            else
            {
                SupplierInvioceItemCertificatQueryService supplierInvioceItemCertificatQueryService = new SupplierInvioceItemCertificatQueryService(_Context);
                if (!this.maxCounter.HasValue)
                {
                    this.maxCounter = supplierInvioceItemCertificatQueryService.GetMaxCounterKey(entityPM.DeclarationId, entityPM.InvoiceCounterKey, entityPM.LineNumber, entityPM.Tenant);
                }
                entityPM.ItemCertificateCounterKey = maxCounter.Value + 1;
                maxCounter = entityPM.ItemCertificateCounterKey;
            }

            base.OnCreating(entityPM, entityParentPM);

        }

        public void FastDeleteComposition(Logitude.Customs.Data.EntityKeys.DeclarationKeys entityKeyFields)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvioceItemCertificatRepository).FastDeleteMulti(entityKeyFields);
        }

        public void UpdateCertificateConnectedItems(List<CertificateConnectedItems> items, string declarationId, string invoiceNumber, string attachmentTypeCode, string certificateNumber, string resConfirmationTypeCode, string certificateExemptionTypeCode, string reqConfirmationTypeCode, int tenant)
        {

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            string oldcertificateExemptionTypeCode = certificateExemptionTypeCode;
            string oldcertificateNumber = certificateNumber;
            string oldresConfirmationTypeCode = resConfirmationTypeCode;
            string oldreqConfirmationTypeCode = reqConfirmationTypeCode;
            if (certificateExemptionTypeCode != null)
            {
                certificateExemptionTypeCode = "'" + certificateExemptionTypeCode + "'";
            }
            if (attachmentTypeCode == "")
            {
                attachmentTypeCode = null;
            }


            if (certificateNumber != null)
            {
                certificateNumber = "'" + certificateNumber + "'";
            }


            if (resConfirmationTypeCode != null)
            {
                resConfirmationTypeCode = "'" + resConfirmationTypeCode + "'";

            }

            if (reqConfirmationTypeCode != null)
            {
                reqConfirmationTypeCode = "'" + reqConfirmationTypeCode + "'";
            }

            string updateCmd;
            string cmd = null;
            string updateCmd1;
            string statusCode;
            string strConnString = GetConnection(tenant);
            List<CertificateConnectedItems> validItems = new List<CertificateConnectedItems>();
            List<CertificateConnectedItems> nonValidItems = new List<CertificateConnectedItems>();


            if (dbms == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    updateCmd = "Update SupplierInvioceItemCertificats set AttachmentTypeCode='" + attachmentTypeCode + "',CertificateNumber= " + (string.IsNullOrEmpty(certificateNumber) ? "NULL" : certificateNumber) + " ,ReqConfirmationTypeCode= " + (string.IsNullOrEmpty(reqConfirmationTypeCode) ? "NULL" : reqConfirmationTypeCode) + ",ResConfirmationTypeCode= " + (string.IsNullOrEmpty(resConfirmationTypeCode) ? "NULL" : resConfirmationTypeCode) + ",CertificateExemptionTypeCode= " + (string.IsNullOrEmpty(certificateExemptionTypeCode) ? "NULL" : certificateExemptionTypeCode) + " where ";


                    int count = 0;

                    foreach (CertificateConnectedItems item in items)
                    {
                        if (attachmentTypeCode == null)
                        {
                            statusCode = "2";

                        }
                        else if (attachmentTypeCode == "1" || attachmentTypeCode == "2")
                        {
                            if (string.IsNullOrEmpty(certificateNumber) || string.IsNullOrEmpty(reqConfirmationTypeCode) || string.IsNullOrEmpty(resConfirmationTypeCode) || !string.IsNullOrEmpty(certificateExemptionTypeCode))
                            {
                                statusCode = "2";

                            }
                            else
                            {
                                statusCode = "1";
                            }

                        }

                        else if (attachmentTypeCode == "4")
                        {
                            if (string.IsNullOrEmpty(certificateExemptionTypeCode) || string.IsNullOrEmpty(reqConfirmationTypeCode) || !string.IsNullOrEmpty(certificateNumber) || !string.IsNullOrEmpty(resConfirmationTypeCode))
                            {
                                statusCode = "2";

                            }

                            else
                            {
                                statusCode = "1";
                            }
                        }
                        else
                        {
                            statusCode = "1";
                        }

                        updateCmd1 = "Update SupplierInvoiceItems set CertificatesStatusCode='" + statusCode + "' where ";

                        updateCmd1 += "(DeclarationId='" + item.DeclarationId + "' and " + "CounterKey=" + item.InvoiceCounterKey + " and " + "LineNumber=" + item.LineNumber + ") or";


                        char[] chars1 = { 'o', 'r' };
                        updateCmd1 = updateCmd1.TrimEnd(chars1);
                        OracleCommand orclCommand1 = new OracleCommand(updateCmd1, cn);

                        cn.Open();
                        orclCommand1.ExecuteNonQuery();
                        cn.Close();


                        if (count == 0)
                        {
                            cmd = updateCmd;
                        }
                        cmd += "(DeclarationId='" + item.DeclarationId + "' and " + "InvoiceCounterKey=" + item.InvoiceCounterKey + " and " + "LineNumber=" + item.LineNumber + " and " + "ItemCertificateCounterKey=" + item.ItemCertificateCounterKey + ") or";
                        count += 1;
                        if (count == 500)
                        {
                            char[] chars = { 'o', 'r' };
                            cmd = cmd.TrimEnd(chars);
                            OracleCommand orclCommand = new OracleCommand(cmd, cn);

                            cn.Open();
                            orclCommand.ExecuteNonQuery();
                            cn.Close();
                            count = 0;
                            cmd = null;
                        }
                    }
                    char[] Chars = { 'o', 'r' };
                    cmd = cmd.TrimEnd(Chars);
                    OracleCommand command = new OracleCommand(cmd, cn);

                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {

                    updateCmd = "Update customs.SupplierInvioceItemCertificats set AttachmentTypeCode='" + attachmentTypeCode + "',CertificateNumber= " + (string.IsNullOrEmpty(certificateNumber) ? "NULL" : certificateNumber) + " ,ReqConfirmationTypeCode= " + (string.IsNullOrEmpty(reqConfirmationTypeCode) ? "NULL" : reqConfirmationTypeCode) + ",ResConfirmationTypeCode= " + (string.IsNullOrEmpty(resConfirmationTypeCode) ? "NULL" : resConfirmationTypeCode) + ",CertificateExemptionTypeCode= " + (string.IsNullOrEmpty(certificateExemptionTypeCode) ? "NULL" : certificateExemptionTypeCode) + " where ";


                    int count = 0;
                    foreach (CertificateConnectedItems item in items)
                    {
                        if (attachmentTypeCode == null)
                        {
                            statusCode = "2";

                        }
                        else if (attachmentTypeCode == "1" || attachmentTypeCode == "2")
                        {
                            if (string.IsNullOrEmpty(certificateNumber) || string.IsNullOrEmpty(reqConfirmationTypeCode) || string.IsNullOrEmpty(resConfirmationTypeCode) || !string.IsNullOrEmpty(certificateExemptionTypeCode))
                            {
                                statusCode = "2";

                            }
                            else
                            {
                                statusCode = "1";
                            }

                        }

                        else if (attachmentTypeCode == "4")
                        {
                            if (string.IsNullOrEmpty(certificateExemptionTypeCode) || string.IsNullOrEmpty(reqConfirmationTypeCode) || !string.IsNullOrEmpty(certificateNumber) || !string.IsNullOrEmpty(resConfirmationTypeCode))
                            {
                                statusCode = "2";

                            }

                            else
                            {
                                statusCode = "1";
                            }
                        }
                        else
                        {
                            statusCode = "1";
                        }

                        updateCmd1 = "Update customs.SupplierInvoiceItems set CertificatesStatusCode='" + statusCode + "' where ";

                        updateCmd1 += "(DeclarationId='" + item.DeclarationId + "' and " + "CounterKey=" + item.InvoiceCounterKey + " and " + "LineNumber=" + item.LineNumber + ") or";


                        char[] chars1 = { 'o', 'r' };
                        updateCmd1 = updateCmd1.TrimEnd(chars1);
                        SqlCommand sqlCommand1 = new SqlCommand(updateCmd1, cn);

                        cn.Open();
                        sqlCommand1.ExecuteNonQuery();
                        cn.Close();

                        if (count == 0)
                        {
                            cmd = updateCmd;
                        }
                        cmd += "(DeclarationId='" + item.DeclarationId + "' and " + "InvoiceCounterKey=" + item.InvoiceCounterKey + " and " + "LineNumber=" + item.LineNumber + " and " + "ItemCertificateCounterKey=" + item.ItemCertificateCounterKey + ") or";
                        count += 1;
                        if (count == 500)
                        {
                            char[] chars = { 'o', 'r' };
                            cmd = cmd.TrimEnd(chars);
                            SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                            cn.Open();
                            sqlCommand.ExecuteNonQuery();
                            cn.Close();
                            count = 0;
                            cmd = null;
                        }
                    }


                    char[] Chars = { 'o', 'r' };
                    cmd = cmd.TrimEnd(Chars);
                    SqlCommand command = new SqlCommand(cmd, cn);

                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();



                }
            }

            this.SetDeclarationChanged(tenant, declarationId);

        }

        private void SetDeclarationChanged(int tenant,string declarationId)
        {
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(tenant);
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                    string cmd = "Update Declarations set IsChanged = 1";
                    cmd = cmd + " where Id=" + "'" + declarationId + "'";

                    OracleCommand sqlCommand = new OracleCommand(cmd, con);

                    con.Open();
                    sqlCommand.ExecuteNonQuery();
                    con.Close();
                }

            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = "Update Customs.Declarations set IsChanged = 1";
                    cmd = cmd + " where Id=" + "'" + declarationId + "'";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
        }

        public void UpdateCertificateConnectedItems(string[] items, string declarationId, string invoiceNumber, string attachmentTypeCode, string certificateNumber, string resConfirmationTypeCode, string certificateExemptionTypeCode, string reqConfirmationTypeCode, int tenant)
        {

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            string oldcertificateExemptionTypeCode = certificateExemptionTypeCode;
            string oldcertificateNumber = certificateNumber;
            string oldresConfirmationTypeCode = resConfirmationTypeCode;
            string oldreqConfirmationTypeCode = reqConfirmationTypeCode;
            if (certificateExemptionTypeCode != null)
            {
                certificateExemptionTypeCode = "'" + certificateExemptionTypeCode + "'";
            }
            if (attachmentTypeCode == "")
            {
                attachmentTypeCode = null;
            }


            if (certificateNumber != null)
            {
                certificateNumber = "'" + certificateNumber + "'";
            }


            if (resConfirmationTypeCode != null)
            {
                resConfirmationTypeCode = "'" + resConfirmationTypeCode + "'";

            }

            if (reqConfirmationTypeCode != null)
            {
                reqConfirmationTypeCode = "'" + reqConfirmationTypeCode + "'";
            }

            string updateCmd;
            string cmd = null;

            string strConnString = GetConnection(tenant);



            if (dbms == "oracle")
            {
                using (OracleConnection cn = new OracleConnection(strConnString))
                {
                    updateCmd = "Update SupplierInvioceItemCertificats set AttachmentTypeCode='" + attachmentTypeCode + "',CertificateNumber= " + (string.IsNullOrEmpty(certificateNumber) ? "NULL" : certificateNumber) + " ,ReqConfirmationTypeCode= " + (string.IsNullOrEmpty(reqConfirmationTypeCode) ? "NULL" : reqConfirmationTypeCode) + ",ResConfirmationTypeCode= " + (string.IsNullOrEmpty(resConfirmationTypeCode) ? "NULL" : resConfirmationTypeCode) + ",CertificateExemptionTypeCode= " + (string.IsNullOrEmpty(certificateExemptionTypeCode) ? "NULL" : certificateExemptionTypeCode) + " where ";


                    int count = 0;
                    string[] values;
                    foreach (var item in items)
                    {
                        values = item.Split(';');
                        if (count == 0)
                        {
                            cmd = updateCmd;
                        }
                        cmd += "(DeclarationId='" + values[0] + "' and " + "InvoiceCounterKey=" + values[1] + " and " + "LineNumber=" + values[2] + " and " + "ItemCertificateCounterKey=" + values[3] + ") or";
                        count += 1;
                        if (count == 500)
                        {
                            char[] chars = { 'o', 'r' };
                            cmd = cmd.TrimEnd(chars);
                            OracleCommand sqlCommand = new OracleCommand(cmd, cn);

                            cn.Open();
                            sqlCommand.ExecuteNonQuery();
                            cn.Close();
                            count = 0;
                            cmd = null;
                        }
                    }
                    char[] Chars = { 'o', 'r' };
                    cmd = cmd.TrimEnd(Chars);
                    OracleCommand command = new OracleCommand(cmd, cn);

                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();
                }
            }

            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {

                    updateCmd = "Update customs.SupplierInvioceItemCertificats set AttachmentTypeCode='" + attachmentTypeCode + "',CertificateNumber= " + (string.IsNullOrEmpty(certificateNumber) ? "NULL" : certificateNumber) + " ,ReqConfirmationTypeCode= " + (string.IsNullOrEmpty(reqConfirmationTypeCode) ? "NULL" : reqConfirmationTypeCode) + ",ResConfirmationTypeCode= " + (string.IsNullOrEmpty(resConfirmationTypeCode) ? "NULL" : resConfirmationTypeCode) + ",CertificateExemptionTypeCode= " + (string.IsNullOrEmpty(certificateExemptionTypeCode) ? "NULL" : certificateExemptionTypeCode) + " where ";


                    int count = 0;
                    string[] values;
                    foreach (var item in items)
                    {
                        values = item.Split(';');
                        if (count == 0)
                        {
                            cmd = updateCmd;
                        }
                        cmd += "(DeclarationId='" + values[0] + "' and " + "InvoiceCounterKey=" + values[1] + " and " + "LineNumber=" + values[2] + " and " + "ItemCertificateCounterKey=" + values[3] + ") or";
                        count += 1;
                        if (count == 500)
                        {
                            char[] chars = { 'o', 'r' };
                            cmd = cmd.TrimEnd(chars);
                            SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                            cn.Open();
                            sqlCommand.ExecuteNonQuery();
                            cn.Close();
                            count = 0;
                            cmd = null;
                        }
                    }


                    char[] Chars = { 'o', 'r' };
                    cmd = cmd.TrimEnd(Chars);
                    SqlCommand command = new SqlCommand(cmd, cn);

                    cn.Open();
                    command.ExecuteNonQuery();
                    cn.Close();

                }
            }

            if (items.Length > 0)
            {
                List<string> validItems = new List<string>();
                List<string> nonValidItems = new List<string>();
               
                foreach (string item in items)
                {
                   
                    if (attachmentTypeCode == null)
                    {
                        nonValidItems.Add(item);

                    }


                    else if (attachmentTypeCode == "1" || attachmentTypeCode == "2")
                    {
                        if (string.IsNullOrEmpty(certificateNumber) || string.IsNullOrEmpty(reqConfirmationTypeCode) || string.IsNullOrEmpty(resConfirmationTypeCode) || !string.IsNullOrEmpty(certificateExemptionTypeCode))
                        {
                        
                            nonValidItems.Add(item);

                        }
                        else
                        {
                          
                            validItems.Add(item);
                        }

                    }


                    else if (attachmentTypeCode == "4")
                    {
                        if (string.IsNullOrEmpty(certificateExemptionTypeCode) || string.IsNullOrEmpty(reqConfirmationTypeCode) || !string.IsNullOrEmpty(certificateNumber) || !string.IsNullOrEmpty(resConfirmationTypeCode))
                        {
                           
                            nonValidItems.Add(item);

                        }

                        else
                        {
                        
                            validItems.Add(item);
                        }
                    }

                    else
                    {
                      
                        validItems.Add(item);
                    }



                }

                string[] values;
                if (nonValidItems.Count > 0)
                {

                    if (dbms == "oracle")
                    {
                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            updateCmd = "Update SupplierInvoiceItems set CertificatesStatusCode='" + 2 + "' where ";


                          
                            foreach (string item in nonValidItems)
                            {
                                values= item.Split(';');
                                updateCmd += "(DeclarationId='" + values[0] + "' and " + "CounterKey=" + values[1] + " and " + "LineNumber=" + values[2] + ") or";
                            }
                            char[] chars = { 'o', 'r' };
                            updateCmd = updateCmd.TrimEnd(chars);
                            OracleCommand sqlCommand = new OracleCommand(updateCmd, cn);

                            cn.Open();
                            sqlCommand.ExecuteNonQuery();
                            cn.Close();
                        }
                    }

                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {

                            updateCmd = "Update customs.SupplierInvoiceItems set CertificatesStatusCode='" + 2 + "' where ";



                            foreach (string item in nonValidItems)
                            {
                                values = item.Split(';');
                                updateCmd += "(DeclarationId='" + values[0] + "' and " + "CounterKey=" + values[1] + " and " + "LineNumber=" + values[2] + ") or";
                            }




                            char[] chars = { 'o', 'r' };
                            updateCmd = updateCmd.TrimEnd(chars);
                            SqlCommand sqlCommand = new SqlCommand(updateCmd, cn);

                            cn.Open();
                            sqlCommand.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                }


                if (validItems.Count > 0)
                {
                    if (dbms == "oracle")
                    {
                        using (OracleConnection cn = new OracleConnection(strConnString))
                        {
                            updateCmd = "Update SupplierInvoiceItems set CertificatesStatusCode='" + 1 + "' where ";



                            foreach (string item in validItems)
                            {
                                values = item.Split(';');
                                updateCmd += "(DeclarationId='" + values[0] + "' and " + "CounterKey=" + values[1] + " and " + "LineNumber=" + values[2] + ") or";
                            }


                            char[] chars = { 'o', 'r' };
                            updateCmd = updateCmd.TrimEnd(chars);
                            OracleCommand sqlCommand = new OracleCommand(updateCmd, cn);

                            cn.Open();
                            sqlCommand.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                    else
                    {
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {

                            updateCmd = "Update customs.SupplierInvoiceItems set CertificatesStatusCode='" + 1 + "' where ";



                            foreach (string item in validItems)
                            {
                                values = item.Split(';');
                                updateCmd += "(DeclarationId='" + values[0] + "' and " + "CounterKey=" + values[1] + " and " + "LineNumber=" + values[2] + ") or";
                            }




                            char[] chars = { 'o', 'r' };
                            updateCmd = updateCmd.TrimEnd(chars);
                            SqlCommand sqlCommand = new SqlCommand(updateCmd, cn);

                            cn.Open();
                            sqlCommand.ExecuteNonQuery();
                            cn.Close();
                        }
                    }
                }

            }

        }
        public void InsertSupplierInvioceItemCertificatByCsvFile(string certificateNumber,string requestNumber,int tenant,string decId,int lineNumber,int invoiceCounterkey, SupplierInvoiceItemPM invoiceItem)
        {
            SupplierInvioceItemCertificatRepository supplierInvioceItemCertificatRepository = new SupplierInvioceItemCertificatRepository(tenant);
            ICustomContext customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatQueryService supplierInvioceItemCertificatQueryService = new SupplierInvioceItemCertificatQueryService(customContext);
            SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
            var entity = supplierInvioceItemCertificatQueryService.GetSupplierInvioceItemCertificatWithExternalRequestTypeCode("0402", decId,invoiceItem.LineNumber);
            if (entity == null)
            {
                entity = new SupplierInvioceItemCertificatPM();
                entity.ChangeSetOp = ChangeSetOperation.Insert;
            }
            else
            {
                entity.ChangeSetOp = ChangeSetOperation.Update;
            }
            entity.InvoiceCounterKey = invoiceCounterkey;
            entity.Tenant = tenant;
            entity.CertificateNumber = certificateNumber;
            entity.ResConfirmationTypeCode = "2402";
            entity.ReqConfirmationTypeCode = "2402";
            entity.AttachmentTypeCode = "2";
            if(entity.SequenceNumeric == 0 )
            {
                entity.SequenceNumeric = supplierInvioceItemCertificatRepository.getNextSequenceNumber(decId, tenant, invoiceItem.LineNumber);
            }
            entity.ApprovalRequestNumber = requestNumber;
            invoiceItem.SupplierInvioceItemCertificats.Add(entity);
            invoiceItem.ChangeSetOp = ChangeSetOperation.Update;
            updateService.Update(invoiceItem, true);
        }
      
        public void UpdateCertificateWithoutCertificateExemptionTypeCode(SupplierInvioceItemCertificatPM cert, int tenant)
        {
            SupplierInvoiceItemQueryService query = new SupplierInvoiceItemQueryService(tenant);
            SupplierInvoiceItemPM item = query.GetSingle(cert.DeclarationId, cert.InvoiceCounterKey, cert.LineNumber, false, false);
            if (!item.SupplierInvioceItemCertificats.Contains(cert))
            {
                item.SupplierInvioceItemCertificats.Add(cert);
            }
            ICustomContext customContext = CustomContext.GetContext(tenant);
            SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
            item.ChangeSetOp = ChangeSetOperation.Update;
            updateService.Update(item, true);
        }


        public void FastDeleteMultiParents(SupplierInvoiceKeys entityKeyFields, List<int> supplierInvoiceItemsParentsLines)
        {
            (Repository as Logitude.Customs.Data.Repsitories.SupplierInvioceItemCertificatRepository).FastDeleteMultiParents(entityKeyFields, supplierInvoiceItemsParentsLines);
        }

        public void UpdateCertificateStatus(SupplierInvioceItemCertificatPM cert, int tenant)
        {
            // update item
            SupplierInvoiceItemQueryService query = new SupplierInvoiceItemQueryService(tenant);
            //SupplierInvoiceItemPM item = query.GetSingleSupplierInvoicePMBySequence(cert.DeclarationId, cert.InvoiceCounterKey, cert.SequenceNumeric);
            SupplierInvoiceItemPM item = query.GetSingle(cert.DeclarationId, cert.InvoiceCounterKey,cert.LineNumber,false,false);

            if (cert != null)
            {

                string statusCode;
                if (cert.AttachmentTypeCode == null)
                {
                    statusCode = "2";
                }
                else if (cert.AttachmentTypeCode == "1" || cert.AttachmentTypeCode == "2")
                {
                    if (string.IsNullOrEmpty(cert.CertificateNumber) || string.IsNullOrEmpty(cert.ReqConfirmationTypeCode) || string.IsNullOrEmpty(cert.ResConfirmationTypeCode) || !string.IsNullOrEmpty(cert.CertificateExemptionTypeCode))
                    {
                        statusCode = "2";
                    }
                    else
                    {
                        statusCode = "1";
                    }
                }
                else if (cert.AttachmentTypeCode == "4")
                {
                    if (string.IsNullOrEmpty(cert.CertificateExemptionTypeCode) || string.IsNullOrEmpty(cert.ReqConfirmationTypeCode) || !string.IsNullOrEmpty(cert.CertificateNumber) || !string.IsNullOrEmpty(cert.ResConfirmationTypeCode))
                    {
                        statusCode = "2";
                    }
                    else
                    {
                        statusCode = "1";
                    }
                }
                else
                {
                    statusCode = "1";
                }

                item.CertificatesStatusCode = statusCode;

                ICustomContext customContext = CustomContext.GetContext(tenant);
                SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                item.ChangeSetOp = ChangeSetOperation.Update;
                updateService.Update(item, true);
            }

        }



        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }

        public int UpdateAllCertificateWithoutResponse(string declarationId, int tenant)
        {
            SupplierInvioceItemCertificatQueryService supplierInvoiceItemRepository = new SupplierInvioceItemCertificatQueryService(tenant);
            var res = supplierInvoiceItemRepository.GetSupplierInvoiceItemsCertificateWithoutResponse(tenant, declarationId);

            int count = 0;
            if (string.IsNullOrWhiteSpace(res.certificateKeys)) return count;
            string whereInCertificateKeys = "";
            string whereInInvoiceItemKeys = "";
            int i = 0;
            if (res.certificateKeys.Split(',').Count() > 990)
            {
                var certificateKeysList = res.certificateKeys.Split(',');
                var invoiceItemKeysList = res.InvoiceItemKeys.Split(',');
                for (var j = 0; j < certificateKeysList.Length; j++)
                {
                    var certificateKeyItem = certificateKeysList[j];
                    var invoiceItemKeyItem = invoiceItemKeysList[j];
                    if (i < 990)
                    {
                        whereInCertificateKeys += certificateKeyItem + ',';
                        whereInInvoiceItemKeys += invoiceItemKeyItem + ',';
                        i++;
                    }
                    else
                    {
                        whereInCertificateKeys = whereInCertificateKeys.TrimEnd(',');
                        whereInCertificateKeys += ") OR  SIIC.InvoiceCounterKey || ' ' || SIIC.LineNumber || ' ' || SIIC.ItemCertificateCounterKey IN (" + certificateKeyItem + ',';
                        i = 0;

                        whereInInvoiceItemKeys = whereInInvoiceItemKeys.TrimEnd(',');
                        whereInInvoiceItemKeys += ") OR  s.CounterKey || ' ' || s.LineNumber IN (" + invoiceItemKeyItem + ',';

                    }
                }
                whereInCertificateKeys = whereInCertificateKeys.TrimEnd(',');
                whereInInvoiceItemKeys = whereInInvoiceItemKeys.TrimEnd(',');
            }
            else
            {
                whereInCertificateKeys = res.certificateKeys;
                whereInInvoiceItemKeys = res.InvoiceItemKeys;
            }

            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            string strConnString = GetConnection(tenant);
            if (dbms == "oracle")
            {
                using (OracleConnection con = new OracleConnection(strConnString))
                {
                   string cmd = @"Update SupplierInvioceItemCertificats SIIC 
                                  set SIIC.AttachmentTypeCode = '4', SIIC.CertificateExemptionTypeCode = '92' 
                                  where SIIC.DeclarationId ='" + declarationId + "' and SIIC.InvoiceCounterKey || ' ' || SIIC.LineNumber || ' ' || SIIC.ItemCertificateCounterKey  in (" + whereInCertificateKeys + ") ";

                    string cmd1 = @"
                                Update supplierInvoiceItems s set s.CertificatesStatusCode = '1'
                                where s.DeclarationId ='" + declarationId + "' and s.CounterKey || ' ' || s.LineNumber in ( " + whereInInvoiceItemKeys + " )";

                    OracleCommand sqlCommand = new OracleCommand(cmd, con);
                    OracleCommand sqlCommand1 = new OracleCommand(cmd1, con);
                    con.Open();
                    count = sqlCommand.ExecuteNonQuery();
                    sqlCommand1.ExecuteNonQuery();
                    con.Close();
                }
            }
            else
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    string cmd = @"Update Customs.SupplierInvioceItemCertificats SIIC
                                   set SIIC.AttachmentTypeCode = '4', SIIC.CertificateExemptionTypeCode = '92'
                                   where SIIC.DeclarationId ='" + declarationId + "' and SIIC.InvoiceCounterKey + ' ' + SIIC.LineNumber + ' ' + SIIC.ItemCertificateCounterKey  in (" + res.certificateKeys + ") ";
                    cmd +=  Environment.NewLine + "Update supplierInvoiceItems s set s.CertificatesStatusCode = '1' " +
                        "where s.DeclarationId = '" + declarationId + "' and s.CounterKey + ' ' + s.LineNumber in (" + res.InvoiceItemKeys + ")";

                    SqlCommand sqlCommand = new SqlCommand(cmd, cn);

                    cn.Open();
                    count = sqlCommand.ExecuteNonQuery();
                    cn.Close();
                }
            }
            return count;
        }

    }
}
