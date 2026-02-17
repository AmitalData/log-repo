using CustomsWorkerRole.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmitalCustomsWindowsService.Tester.CustomMessage
{
    public partial class LoadTesterForm : Form
    {
        private LoadTestService _LoadTestService;
        private int _Tenant;
        private List<String> _list;
        public LoadTesterForm()
        {
            InitializeComponent();
            
        }
        public void Initialize(int tenant)
        {
            _Tenant = tenant;
            _LoadTestService = new LoadTestService(tenant);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _list = _LoadTestService.GetDeclarationPMLoadTestTop(50);
            
            listViewDec.Items.Clear();
            foreach (var item in _list)
            {
                listViewDec.Items.Add(item);    
            } 
            
        }

        private void buttonBuildAndListen_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(comboBoxTotalRequest.Text);
            MyUserId = "";
            if (_radioButtonPersonal.Checked)
            {
                MyUserId = LoadTestService.GetUserIdByPersonalId(_textBoxPersonalId.Text, _Tenant);

                if (String.IsNullOrWhiteSpace(MyUserId))
                {
                    MessageBox.Show("GetUserIdByPersonalId return null");
                    _radioButtonNoSign.Checked = true;
                    return;
                }
                
            }
            _LoadTestService.Start(i, MyUserId);
        }

        private void _radioButtonPersonal_CheckedChanged(object sender, EventArgs e)
        {
            MyUserId =LoadTestService.GetUserIdByPersonalId(_textBoxPersonalId.Text, _Tenant);

            if (String.IsNullOrWhiteSpace(MyUserId))
            {
                MessageBox.Show("GetUserIdByPersonalId return null");
                _radioButtonNoSign.Checked = true;
            }

        }





        public string MyUserId { get; set; }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void _buttonLoadDoc50_Click(object sender, EventArgs e)
        {
            _list = _LoadTestService.GetCustomsDocumentLoadTestTop(50);

            _listViewDocs.Items.Clear();
            foreach (var item in _list)
            {
                _listViewDocs.Items.Add(item);
            } 
        }

        private void _StressWebcheckBox_CheckedChanged(object sender, EventArgs e)
        {
           // _LoadTestService.StressWeb = _StressWebcheckBox.Checked;

        }

        private void checkBoxExchangeRate_CheckedChanged(object sender, EventArgs e)
        {
            _LoadTestService.CheckExchangeRate = checkBoxExchangeRate.Checked;
        }

        private void _radioButtonCompany_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
/*
update [Amital1_Main].[Customs].[Declarations]  set UserNotes='LoadTest' , [CreateDateTime] = GETDATE()  where      CustomFileNo = '513200394'
SELECT TOP 1000 [Id] ,[UserNotes] ,[CreateDateTime] ,[ConcurrencyGUID]
      ,[Tenant]
      ,[CustomFileNo]
      ,[DeclarationNumber]
      ,[CustomerId]
      ,[SearchFields]
      ,[VersionId]
      ,[ExternalDeclarationNumber]
      ,[DeclarationOfficeCode]
      ,[TaxationDateTime]
      ,[AgentId]
      ,[ProcedureCurrentCode]
      ,[AutonomyRegionTypeCode]
      ,[ImporterId]
      ,[ImporterPassCountryCode]
      ,[TransferImporterId]
      ,[TransferImporterCountryCode]
      ,[EntitleImporterId]
      ,[ImporterEntitlementTypeCode]
      ,[EntitleImporterCountryCode]
      ,[DeclarationDocumentId]
      ,[DeclarationDocumentTypeCode]
      ,[CreatedByUserId]
      ,[IsChanged]
      ,[PaymentDate]
      ,[HatraDate]
      ,[DeclarationStatusTypeCode]
      ,[LoadingFactor]
      ,[DealValue]
      ,[CIFValue]
      ,[TotalTax]
      ,[FileState]
      ,[TransportModeId]
      ,[ErrosXml]
      ,[DepartmentId]
      ,[ReferentUserId]
      ,[StorageSiteCode]
      ,[PlatformFee]
      
      ,[UpdateDateTime]
      ,[IsCancelled]
      ,[DealValueWithoutFactor]
      ,[ImporterCode]
      ,[TransferImporterCode]
      ,[EntitleImporterCode]
      
      
      ,[ImporterTypeCode]
      ,[TransferImporterTypeCode]
      ,[EntitleImporterTypeCode]
      ,[HasConstraint]
      ,[PrimaryInvoiceCounterKey]
      ,[PaymentOrderNumber]
      ,[PaymentStatusCode]
      ,[IsSignedVersion]
      ,[SignedByUserId]
      ,[StorageSiteName]
  FROM [Amital1_Main].[Customs].[Declarations] 
  where      CustomFileNo in ( '513200388' ,'513400075' , '513200442' , '513200394' )

*/