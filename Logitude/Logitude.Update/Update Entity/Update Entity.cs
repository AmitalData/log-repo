using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using WebFreight.Web.AccountingModel;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;

namespace Logitude.Update.Update_Entity
{
    public partial class Update_Entity : Form
    {
        private int EntitiesCount;
        private int SuccessededEntitiesCount;
        private int maxParrallelThreadsCount = 10;

        public Update_Entity()
        {
            InitializeComponent();
            SourceConnectionTxt.Text = "Logitude2-5_Main,sa,Saas256,.";
        }


        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateClicked();
            }
            catch (Exception exception)
            {
                DisplayExceptionMessage(exception);
            }
        }

        private void UpdateClicked()
        {
            InitializeValues();
            DataTable entititesDataTable = GetEntitiesDataTable();
            EntitiesCount = entititesDataTable.Rows.Count;
            ResultLbl.Text = "InProgress ... Count:" + EntitiesCount;

            ThreadStart updateEntityThreadStart = (() => UpdateAllEntities(entititesDataTable));
            Thread thread = new Thread(updateEntityThreadStart) { IsBackground = true };
            thread.Start();
            thread.Join();
            SetResultLable("Done > ");
            EnabledTextBoxes(true);
        }

        private void InitializeValues()
        {
            AccountingRegistrations.Register();
            EntitiesCount = 0;
            SuccessededEntitiesCount = 0;
            ResultLbl.Text = "";
            EnabledTextBoxes(false);
        }

        private void EnabledTextBoxes(bool isEnabled)
        {
            SourceConnectionTxt.Enabled = isEnabled;
            TenantTxt.Enabled = isEnabled;
            FieldCodeText.Enabled = isEnabled;
            NewFieldValueTxt.Enabled = isEnabled;
            additionalScriptTxt.Enabled = isEnabled;
            UpdateBtn.Enabled = isEnabled;
        }

        private DataTable GetEntitiesDataTable()
        {
            string requiredSQL = "Select Id From ARInvoices Where Tenant = " + TenantTxt.Text + " and (" + FieldCodeText.Text + " is null or " + FieldCodeText.Text + "<> 1)";
           
            requiredSQL += !string.IsNullOrEmpty(additionalScriptTxt.Text) ? " and (" + additionalScriptTxt.Text + ")" : "";
            
            string connectionString = BuildConnectionString(SourceConnectionTxt.Text);
            return ExecuteReader(connectionString, requiredSQL);
        }

        private string BuildConnectionString(string sourceConnection)
        {
            string[] sourceConnections = sourceConnection.Split(',');
            string catalog = sourceConnections[0];
            string userName = sourceConnections[1];
            string password = sourceConnections[2];
            string server = sourceConnections[3];

            string connectionString = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return connectionString;
        }

        private DataTable ExecuteReader(string connectionString, string sqlString)
        {
            var result = new DataTable();
            using (SqlConnection sourceConnection = new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandSourceData = new SqlCommand(sqlString, sourceConnection);
                SqlDataReader reader = commandSourceData.ExecuteReader();
                result.Load(reader);
                reader.Close();

            }
            return result;
        }

        private void UpdateAllEntities(DataTable entititesDataTable)
        {
            int tenant = Int32.Parse(TenantTxt.Text);
            ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(tenant);
            List<string> arInvoiceIds = FillARInvoiceIds(entititesDataTable);

            List<ARInvoicePM> aRInvoicePMs = new List<ARInvoicePM>();
            IEnumerable<List<string>> listOfARInvoiceIds = SplitListIntoNList(arInvoiceIds, maxParrallelThreadsCount);

            listOfARInvoiceIds.ToList().ForEach(Ids =>
            {
                HandleUpdateEntities(aRInvoiceQuery.GetARInvoicePMsByIdList(Ids, tenant));
            });
        }

        private List<string> FillARInvoiceIds(DataTable entititesDataTable)
        {
            List<string> arInvoiceIds = new List<string>();

            foreach (DataRow row in entititesDataTable.Rows)
            {
                arInvoiceIds.Add(row["Id"].ToString());
            }

            return arInvoiceIds;
        }

        private void HandleUpdateEntities(List<ARInvoicePM> entityList)
        {
            Parallel.ForEach(((IEnumerable)entityList).Cast<ARInvoicePM>().ToList(), (entityPM) =>
            {
                UpdateEntity(entityPM);
            });
        }

        public IEnumerable<List<T>> SplitListIntoNList<T>(List<T> fullList, int nSize)
        {
            for (int i = 0; i < fullList.Count; i += nSize)
            {
                yield return fullList.GetRange(i, Math.Min(nSize, fullList.Count - i)).ToList();
            }
        }

        private void UpdateEntity(ARInvoicePM aRInvoicePM)
        {
            try
            {
                HandleUpdateEntity(aRInvoicePM);
            }
            catch (Exception ex)
            {}
        }

        private void HandleUpdateEntity(ARInvoicePM aRInvoicePM)
        {
            int tenant = Int32.Parse(TenantTxt.Text);
            IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
            ARInvoiceService aRInvoiceService = new ARInvoiceService(MyContext, tenant);
            SetPropertyValue(aRInvoicePM, FieldCodeText.Text, new CustomFieldClass(FieldCodeText.Text, "ARInvoice", NewFieldValueTxt.Text));
            aRInvoiceService.Update(aRInvoicePM, true);
            SuccessededEntitiesCount += 1;
        }

        private void SetPropertyValue(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null)
            {
                prop.SetValue(obj, value, null);
            }
        }

        private void DisplayExceptionMessage(Exception exception)
        {
            //SetResultLable("Fail> ");
            string message = exception.Message + (exception.InnerException != null ? exception.InnerException.ToString() : "");
            if (message.Length > 1500) message = message.Substring(0, 1500);
            MessageBox.Show(message);
        }

        private void SetResultLable(string messageWord)
        {
            ResultLbl.Text = messageWord + SuccessededEntitiesCount + " Success From " + EntitiesCount;
            ResultLbl.ForeColor = messageWord == "Done > " ? Color.Green : Color.Red;
        }
    }
}
