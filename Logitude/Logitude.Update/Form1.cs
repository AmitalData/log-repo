using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using System.Xml;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.MetaDataUpdate;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.ServiceBus.Messaging;
using Logitude.BL.CommonDataModel.EntityLists;
using System.Data.Entity.Validation;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.StimulReport;
using Logitude.Server.Tools;
using WebFreight.Web.MetaDataUpdate.SendBox;
using System.Runtime.Serialization;
using System.Xml.Schema;
using WebFreight.Web.CommonDataModel.DomainServices;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using WebFreight.Web.Helpers;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using WebFreight.Web.InfrastructureModel;
using System.Drawing;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Text;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Data.SqlClient;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Utils;
using System.Data.Common;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.InfrastructureModel;
using System.Text.RegularExpressions;
using Logitude.Server.Tools.QueueService;
using Simplog.Global.Data.GlobalModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Customs.BL.PatchDistribution;

namespace Logitude.Update
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            CacheOnClientUpdate();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
                LogitudeSettings.DatabaseManagementSystem = dbms;
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");
                LogitudeSettings.Id = setting.Id;
                LogitudeSettings.ChampEnv = setting.ChampEnv;
                LogitudeSettings.ChampURL = setting.ChampURL;
                LogitudeSettings.ChampTestAPIURL = setting.ChampTestAPIURL;
                LogitudeSettings.ChampTestAPIPassword = setting.ChampTestAPIPassword;
                LogitudeSettings.ChampProdAPIURL = setting.ChampProdAPIURL;
                LogitudeSettings.ChampProdAPIPassword = setting.ChampProdAPIPassword;
                LogitudeSettings.CustomerCareIP = setting.CustomerCareIP;
                LogitudeSettings.DeploymentStage = setting.DeploymentStage;
                LogitudeSettings.IsLogEnabled = setting.IsLogEnabled;
                LogitudeSettings.LogitudeURL = setting.LogitudeURL;
                LogitudeSettings.TotangoServiceId = setting.TotangoServiceId;
                LogitudeSettings.UsingAzure = setting.UsingAzure;
                LogitudeSettings.StorageAccountKey = setting.StorageAccountKey;
                LogitudeSettings.StorageAccountName = setting.StorageAccountName;
                LogitudeSettings.StorageType = setting.StorageType;
                LogitudeSettings.LogitudeCRMTenantNumber = setting.LogitudeCRMTenantNumber;
                LogitudeSettings.AutoSignupEmail = setting.AutoSignupEmail;
                LogitudeSettings.AutoSignupPassword = setting.AutoSignupPassword;
                LogitudeSettings.WorkEnvironment = setting.WorkEnvironment; // maybe we need to init more fields ?
                LogitudeSettings.StorageServiceMode = setting.StorageServiceMode;

                //if (LogitudeSettings.IsCostomsDeploy) 
                LogitudeSettings.ABMProductId = setting.ABMProductId;
                LogitudeSettings.AzureFolderName = setting.AzureFolderName;
                //if (LogitudeSettings.IsCostomsDeploy)
                {
                    //LogitudeSettings.GetUnfDBConnectionInfoFromTenantInject = CustomsSettingQueryService.GetUnfDBConnectionInfo;
                    LogitudeSettings.GetLogitudeCustomsSettingsMInject = CustomsSettingQueryService.GetLogitudeCustomsSettingsM;
                }
                string storageServiceMode = "fs";
                string queueServiceMode = "azure";
                Logitude.Server.Tools.ContainerAccessor.InitContainer();
                InjectionUtil.Init(null, null, null, () => (new ByteCompressorUtil()) as IByteCompressorUtil,null, null);

                CacheManager.CacheWrapper = new CacheWrapper(WorkerEntryPoint.Cache);
            }
            catch (Exception eee)
            {
                MessageBox.Show("Exception eee =" + eee.ToString());
                throw;
            }
        }


        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var err = e.ExceptionObject.ToString();
            MessageBox.Show
            ("CurrentDomain_UnhandledException!!!" + err);
            //System.Diagnostics.Debugger.Launch();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "UpdateTenantZero", UpdateTenant0lbl));
            thread.IsBackground = true;
            thread.Start();
        }

        Stopwatch globalStopwatch;
        Label generalLabel;
        private void UpdateModule(int tenant, string name, Label lable)
        {
            SetControlPropertyValue(lable, "Text", "Updating...");
            SetControlPropertyValue(lable, "ForeColor", Color.Black); // timer

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // for timer
            if (generalLabel != null) SetControlPropertyValue(generalLabel, "Text", "Updating...");
            globalStopwatch = stopWatch;
            generalLabel = lable;
            timer1.Enabled = true;
            timer1.Start();

            TenantsUpdateClass.UpdateDataForTenant(tenant, name);

            // for timer
            globalStopwatch = null;
            generalLabel = null;
            timer1.Start();

            if (name == "accounting" || name == "UpdateTenantZeroNew")
                UpdateZipFiles();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            SetControlPropertyValue(lable, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue(lable, "ForeColor", Color.Green); // timer
            SetControlPropertyValue(lable, "Text", "Done in " + ts.ToString(@"hh\:mm\:ss"));

        }

        public void UpdateModule(int tenant, string name)
        {
            TenantsUpdateClass.UpdateDataForTenant(tenant, name);
        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
        void GETGIT()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git.exe");

            startInfo.UseShellExecute = false;
            startInfo.WorkingDirectory = "dir Here";
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = "rev-parse --abbrev-ref HEAD";

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            string branchname = process.StandardOutput.ReadLine();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                UpdateModule(0, "customs", UpdateCustomslbl);
                var repo = new CustomsSettingRepository(_SeedTenant);
                if (repo.AnyCourierTenant())
                {
                    MessageBox.Show("נמצא סביבת בלדרות פעילה - וודא שאין מסרים לחתימה - שאל את איתן ענת !!!");
                }
                Func<string> GetConnetionStringFunc = () =>
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox(
                        @"System Connection String is : 
User/Pass",
                               "I Need System ConnString For Grant",
                               "system/manager1",
                               0,
                               0);
                    if (String.IsNullOrWhiteSpace(input))
                    {
                        input = "system/manager1";
                    }
                    return input;
                };
                Logitude.Customs.BL.Utils.GrantCCUTableUtil.GrantCCUTo(1, GetConnetionStringFunc);
            }
            );
            thread.IsBackground = true;
            thread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(UpdateTenants);
            thread.IsBackground = true;
            thread.Start();
        }

        private void UpdateTenants()
        {
            SetControlPropertyValue(UpdateTenantslbl, "Text", "Updating...");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<GlobalTenant> globalTenants;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                globalTenants = GlobalTenantRepository.GetGlobalTenants();

            }

            if (globalTenants != null)
            {
                GlobalTenant tenantZero = globalTenants.Where(d => d.Id == 0).FirstOrDefault();
                List<GlobalTenant> upgradableTenants = (from a in globalTenants
                                                        where a.Version != tenantZero.Version && a.Id != 0 && a.Version != -1 && a.IsActive == true
                                                        select a).ToList();
                if (upgradableTenants.Count > 0)
                {
                    foreach (GlobalTenant tenant in upgradableTenants)
                    {
                        if (tenant.Id != 0)
                        {
                            TenantsUpdateClass.UpdateDataForTenant(tenant.Id, "");
                        }
                    }


                }
            }

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            SetControlPropertyValue(UpdateTenantslbl, "Text", "Done in " + ts.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //try
            //{
            //LoadCustomClosedTables.FillCustomClosedTablesData();

            // LoadCustomClosedTables.FillCustomsClosedTablesInDb();
            //   LoadCustomClosedTables.UpdateSingleClosedTable("1892",null);


            //LoadCustomClosedTables.UpdateAllClosedTables(0);
            LoadCustomClosedTables.UpdateAllClosedTables(1);



            //}
            //catch (Exception exception)
            //{

            //    throw;
            //}


            label1.Text = "Filling custom closed tables completed successfully";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //string mainConnectionString = @"Logitude2-5_MainMigrationTest,sa,Saas256,.";
            //DbConnection connection = DatabaseInitializer.GetConnection(mainConnectionString);

            //LogitudeMigrationContext context = new LogitudeMigrationContext(connection);//new CommonDataContext(connection);
            //ChargeTypeAccounting entities = (from a in context.ChargeTypeAccountings
            //                                 select a).FirstOrDefault();


        }

        private void CRM_Button_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "CRM", UpdateCRMlbl));
            thread.IsBackground = true;
            thread.Start();
        }

        private void buttonBooking_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "Booking", UpdateBookinglbl));
            thread.IsBackground = true;
            thread.Start();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Thread thread = new Thread(UpdateAll);
            thread.IsBackground = true;
            thread.Start();
        }

        private void UpdateAll(object obj)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            SetControlPropertyValue(button5, "Text", "Updating Tenant Zero...");
            TenantsUpdateClass.UpdateDataForTenant(0, "UpdateTenantZero");
            SetControlPropertyValue(button5, "Text", "UpdateTenantZero : Done!" + Environment.NewLine + "Updating CRM...");
            TenantsUpdateClass.UpdateDataForTenant(0, "CRM");
            SetControlPropertyValue(button5, "Text", "Updating Booking...");
            TenantsUpdateClass.UpdateDataForTenant(0, "Booking");
            SetControlPropertyValue(button5, "Text", "Updating social...");
            TenantsUpdateClass.UpdateDataForTenant(0, "social");
            SetControlPropertyValue(button5, "Text", "Updating accounting...");
            TenantsUpdateClass.UpdateDataForTenant(0, "accounting");
            SetControlPropertyValue(button5, "Text", "Updating Tenants...");
            UpdateTenants();
            SetControlPropertyValue(button5, "Text", "Updating Tenants : Done!" + Environment.NewLine + "Building Zip files...");
            UpdateZipFiles();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            SetControlPropertyValue(button5, "Text", "Done in " + ts.ToString() + Environment.NewLine + "Enjoy :)");


        }

        static void ExportXSD()
        {
            XsdDataContractExporter exporter = new XsdDataContractExporter();
            if (exporter.CanExport(typeof(DeclarationSReport)))
            {
                exporter.Export(typeof(DeclarationSReport));
                Console.WriteLine("number of schemas: {0}", exporter.Schemas.Count);
                Console.WriteLine();
                XmlSchemaSet mySchemas = exporter.Schemas;

                XmlQualifiedName XmlNameValue = exporter.GetRootElementName(typeof(DeclarationSReport));
                string EmployeeNameSpace = XmlNameValue.Namespace;
                var file = @"c:\1.xsd";
                using (var sw = File.Create(file))
                {
                    foreach (XmlSchema schema in mySchemas.Schemas(EmployeeNameSpace))
                    {
                        //Console.Out
                        schema.Write(sw);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            SupplierInvoicePM invoice = new SupplierInvoicePM()
            { DeclarationId = "1-104235", InvoiceCounterKey = 1, Tenant = 1, SequenceNumeric = 1, InvoiceNumber = "1000", ChangeSetOp = ChangeSetOperation.Insert };
            for (int a = 1; a < 15000; a = a + 1)
            {
                SupplierInvoiceItemPM item = new SupplierInvoiceItemPM()
                {
                    DeclarationId = "1-104235",
                    CounterKey = 1,
                    LineNumber = a,
                    ItemCode = "a",
                    Tenant = 1,
                    SequenceNumeric = a,
                    ChangeSetOp = ChangeSetOperation.Insert,
                };

                SupplierInvioceItemCertificatPM certificate = new SupplierInvioceItemCertificatPM()
                {

                    DeclarationId = "1-104235",
                    InvoiceCounterKey = 1,
                    LineNumber = a,
                    ItemCertificateCounterKey = a,
                    AttachmentTypeCode = "3",
                    Tenant = 1,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    SequenceNumeric = a,

                };
                item.SupplierInvioceItemCertificats.Add(certificate);
                invoice.SupplierInvoiceItems.Add(item);
            }
            ICustomContext context = CustomContext.GetContext(1);
            SupplierInvoiceUpdateService serivce = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), 1);
            serivce.Update(invoice, true);


            //public EntitySet GetMetaDataEntitySet<TEntity>(DbContext dbCtx)
            //{
            //    Type entityType = typeof(TEntity);
            //    string entityName = entityType.Name;
            //    MetadataWorkspace metaDataWS = ((IObjectContextAdapter)dbCtx).ObjectContext.MetadataWorkspace;

            //    //IEnumerable<EntitySet> entitySets;
            //    var entitySets = metaDataWS.GetItemCollection(DataSpace.SSpace)
            //                     .GetItems<EntityContainer>()
            //                     .Single()
            //                     .BaseEntitySets
            //                     .OfType<EntitySet>()
            //                     .Where(entitySet => !entitySet.MetadataProperties.Contains("Type")
            //                                        || entitySet.MetadataProperties["Type"].ToString() == "Tables");

            //    List<EntitySet> provisionedTables = entitySets.ToList();
            //    EntitySet returnValue = provisionedTables.FirstOrDefault(t => t.Name == entityName);
            //    //When an Entity inherits a base class, the corresponding
            //    //table is sometimes named for the base class
            //    while (null == returnValue && null != entityType)
            //    {
            //        entityType = entityType.BaseType;
            //        entityName = entityType.Name;
            //        returnValue = provisionedTables.FirstOrDefault(t => t.Name == entityName);
            //    }
            //    return returnValue;
            //}


            //    entities.Add(entity1);
            //    entities.Add(entity2);
            //    entities.Add(entity3);

            //    dError.Entitites = entities;

            //    MemoryStream memstream = new MemoryStream();
            //    XmlSerializer ser = new XmlSerializer(typeof(DeclarationError));
            //    ser.Serialize(memstream, dError);
            //    memstream.Seek(0, SeekOrigin.Begin);
            //    var reader = new StreamReader(memstream);
            //    string content = reader.ReadToEnd();
            //    byte[] bytearray = memstream.ToArray();


        }

        private void button7_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "social", UpdateSociallbl));
            thread.IsBackground = true;
            thread.Start();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            List<ChargesTypePM> tenantZeroChargesTypes;
            List<VatType> tenantZeroVatTypes;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ChargesTypeRepository zeroChargesTypeRepository = new ChargesTypeRepository(0);
                VatTypeRepository vatTypeRepository = new VatTypeRepository(0);
                tenantZeroVatTypes = vatTypeRepository.GetVatTypes(0).ToList();
                ChargesTypeQuery chargesTypeQuery = new ChargesTypeQuery(zeroChargesTypeRepository);
                tenantZeroChargesTypes = chargesTypeQuery.GetChargesTypePMsByTenant(0).ToList();
                scope.Complete();
            }

            List<TenantManagement> tenantManagements;
            List<GlobalTenant> tenants;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                GlobalTenantRepository globalTenantRepository = new Simplog.Global.Data.GlobalModel.Repositories.GlobalTenantRepository();
                tenantManagements = tenantManagementRepository.GetTenantManagementsForPackage("EAWB");
                tenants = globalTenantRepository.GetActiveTenants();
                scope.Complete();
            }

            foreach (TenantManagement tenantManagement in tenantManagements)
            {
                int theTenant = tenantManagement.Id;
                bool active = (from a in tenants
                               where a.Id == theTenant
                               select a).Any();
                if (active)
                {
                    MeasurementRepository measurementRepository = new MeasurementRepository(theTenant);
                    ChargesTypeRepository theChargesTypeRepository = new ChargesTypeRepository(theTenant);
                    VatTypeRepository vatTypeRepository = new VatTypeRepository(theTenant);
                    List<VatType> currentTenantVatTypes = vatTypeRepository.GetVatTypes(theTenant).ToList();
                    List<Measurement> currentTenantMeasurement = measurementRepository.GetMeasurementsByTenant(theTenant).ToList();
                    Dictionary<string, ChargesType> currentChargesType = theChargesTypeRepository.GetChargesTypes(theTenant).ToDictionary(d => d.Code + d.ChargesGroupCode, c => c);
                    foreach (ChargesTypePM a in tenantZeroChargesTypes)
                    {
                        if (currentChargesType.Keys.Contains(a.Code + a.ChargesGroupCode))
                        {
                            ChargesType updatedChargesType = currentChargesType[a.Code + a.ChargesGroupCode];

                            updatedChargesType.MeasurementId = currentTenantMeasurement.Where(d => d.Code == a.MeasurementCode && d.Tenant == theTenant).FirstOrDefault().Id;

                            updatedChargesType.LocalName = a.LocalName;
                            updatedChargesType.ContainerMeasurementId = currentTenantMeasurement.Where(d => d.Code == a.ContainerMeasurementCode && d.Tenant == theTenant).FirstOrDefault() != null ? currentTenantMeasurement.Where(d => d.Code == a.ContainerMeasurementCode && d.Tenant == theTenant).FirstOrDefault().Id : null;
                            updatedChargesType.IsAutoDisplayInQuote = a.IsAutoDisplayInQuote;
                            updatedChargesType.EnglishName = a.EnglishName;
                            updatedChargesType.AWBPrintDescription = a.AWBPrintDescription;
                            updatedChargesType.ChargesGroupCode = a.ChargesGroupCode;
                            updatedChargesType.IATACodeId = a.IATACodeId;
                            updatedChargesType.Description = a.Description;
                            updatedChargesType.IsAir = a.IsAir;
                            updatedChargesType.IsOcean = a.IsOcean;
                            updatedChargesType.IsInland = a.IsInland;
                            updatedChargesType.IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation;
                            updatedChargesType.IsAutoDisplayInShipment = a.IsAutoDisplayInShipment;
                            updatedChargesType.IsPayable = a.IsPayable;
                            updatedChargesType.IsReceivable = a.IsReceivable;
                            updatedChargesType.DueTypeCode = a.DueTypeCode;
                            updatedChargesType.ViewOrder = a.ViewOrder;
                            updatedChargesType.SearchFields = a.SearchFields;
                            updatedChargesType.AccountingVATSplit = a.AccountingVATSplit;
                            updatedChargesType.ReceivableCreditAccount = a.ReceivableCreditAccount;
                            updatedChargesType.PayableDebitAccount = a.PayableDebitAccount;
                            theChargesTypeRepository.Update(updatedChargesType);

                        }
                        else
                        {
                            ChargesType charge = new ChargesType()
                            {
                                Id = IdCounter.GetNumber("ChargesType", theTenant).ToString(),
                                AddedManually = false,
                                Tenant = theTenant,
                                MeasurementId = currentTenantMeasurement.Where(d => d.Code == a.MeasurementCode && d.Tenant == theTenant).FirstOrDefault().Id,
                                Code = a.Code,
                                InActive = false,
                                LocalName = a.LocalName,
                                ContainerMeasurementId = currentTenantMeasurement.Where(d => d.Code == a.ContainerMeasurementCode && d.Tenant == theTenant).FirstOrDefault() != null ? currentTenantMeasurement.Where(d => d.Code == a.ContainerMeasurementCode && d.Tenant == theTenant).FirstOrDefault().Id : null,
                                IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                                EnglishName = a.EnglishName,
                                AWBPrintDescription = a.AWBPrintDescription,
                                ChargesGroupCode = a.ChargesGroupCode,
                                IATACodeId = a.IATACodeId,
                                Description = a.Description,
                                IsAir = a.IsAir,
                                IsOcean = a.IsOcean,
                                IsInland = a.IsInland,
                                IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                                IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                                IsPayable = a.IsPayable,
                                IsReceivable = a.IsReceivable,
                                DueTypeCode = a.DueTypeCode,
                                ViewOrder = a.ViewOrder,
                                SearchFields = a.SearchFields,
                                AccountingVATSplit = a.AccountingVATSplit,
                                ReceivableCreditAccount = a.ReceivableCreditAccount,
                                PayableDebitAccount = a.PayableDebitAccount,
                            };

                            VatType vattype = tenantZeroVatTypes.Where(d => d.Id == a.VatTypeId).FirstOrDefault();
                            if (vattype != null)
                            {
                                VatType newVat = currentTenantVatTypes.Where(d => d.Code == vattype.Code && d.Tenant == theTenant).FirstOrDefault();
                                charge.VatTypeId = newVat.Id;
                            }
                            theChargesTypeRepository.Add(charge);

                        }

                    }
                    theChargesTypeRepository.SubmitChanges();
                }
            }
            label1.Text = "update charges types for EAWB package tenants completed successfully.";
        }

        //private void button9_Click(object sender, EventArgs e)
        //{
        //    int tenant = 1;
        //    nsoftware.IPWorksSSL.Pops pops1 = new nsoftware.IPWorksSSL.Pops();
        //    pops1.MailServer = "outlook.office365.com";
        //    pops1.MailPort = 995;
        //    pops1.User = "AutoSignup@logitudeworld.com";
        //    pops1.Password = "$Gen)(876";
        //    pops1.Connect();
        //    pops1.MaxLines = 0;
        //    pops1.MessageNumber = 555;//pops1.MessageCount;
        //    pops1.Retrieve();


        //    string htmlString = pops1.MessageText;

        //    HtmlStringParsingParams htmlParams = new HtmlStringParsingParams();
        //    ParseHtmlString(htmlParams, htmlString);

        //    ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
        //    UserRepository userRepository = new UserRepository(objectContext);
        //    BranchRepository branchRep = new BranchRepository(objectContext);
        //    DepartmentRepository departmentRep = new DepartmentRepository(objectContext);
        //    RoleRepository roleRep = new RoleRepository(objectContext);

        //    Branch branch = branchRep.GetBranchByName("Main Office", tenant);
        //    Simplog.Data.CommonDataModel.EntityPOCOs.Department department = departmentRep.GetDepartmentByName("Management", tenant);
        //    Role role = roleRep.GetSingleByCode("ADMN", 0);



        //    UserPM user = new UserPM()
        //    {
        //        Email = htmlParams.Email,
        //        EnglishName = htmlParams.ContactName,
        //        BusinessPhone = htmlParams.Phone,
        //        Password = "123",
        //        Notes = htmlParams.Company + '-' + htmlParams.Country,
        //        BranchId = branch.Id,
        //        DepartmentId = department.Id,

        //    };
        //    user.Roles = new List<UserRolesPM>() { new UserRolesPM() { Added = true, Tenant = 0, Id = role.Id, Name = role.Name, } };
        //    bool exists = userRepository.DoesUserExist(user.Email, user.Tenant);
        //    if (!exists)
        //    {

        //        UserService service = new UserService(objectContext, user.Tenant);
        //        service.Create(user);
        //    }


        //}

        private void ParseHtmlString(HtmlStringParsingParams htmlParams, string htmlString)
        {
            string[] result = htmlString.Split(new string[] { "<p" }, StringSplitOptions.None);

            for (int i = 0; i < result.Count(); i++)
            {
                if (result[i].Contains("Company Name"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Company = secondSplit[0];

                }
                if (result[i].Contains("Contact Name"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.ContactName = secondSplit[0];

                }
                if (result[i].Contains("Phone Number"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Phone = secondSplit[0];

                }
                if (result[i].Contains("Country"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Country = secondSplit[0];

                }
                if (result[i].Contains("E-mail"))
                {
                    //class="MsoNormal"><a href="mailto:adrian@sabit.ro">adrian@sabit.ro</a><o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[2];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.Email = secondSplit[0];

                }
                if (result[i].Contains("Number of Branches"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.NumberOfBranches = secondSplit[0];

                }
                if (result[i].Contains("Number of Users"))
                {
                    //class="MsoNormal">S.C. SABIT NETWORKS S.R.L<o:p></o:p></p>
                    string startString = result[i + 1];
                    string[] firstSplit = startString.Split('>');
                    string firstResult = firstSplit[1];
                    string[] secondSplit = firstResult.Split('<');
                    htmlParams.NumberOfUsers = secondSplit[0];

                }

            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            #region queue test
            //string emailQueueName = Environment.MachineName + "_" + "MyTestQueue";


            //if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
            //{
            //    QueueDescription queueDescription = new QueueDescription(emailQueueName);
            //    queueDescription.MaxSizeInMegabytes = 5120;
            //    queueDescription.LockDuration = new TimeSpan(0, 5, 0);
            //    // queueDescription.DefaultMessageTimeToLive = new TimeSpan(3, 1, 0);

            //    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
            //}

            //QueueDescription queueDescription2 = StorageAcountDetails.NameSpaceManager.GetQueue(emailQueueName);

            //queueDescription2.LockDuration = new TimeSpan(0, 1, 0);
            //StorageAcountDetails.NameSpaceManager.UpdateQueue(queueDescription2);
            //QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);

            //BrokeredMessage message = new BrokeredMessage();
            //message.Properties["Test"] = "this is a test2";

            //client.Send(message);
            #endregion

            // Create the email object first, then add the properties.
            //            var myMessage = SendGrid.GetInstance();

            //            // Add the message properties.
            //            myMessage.From = new MailAddress("mohammad@logitudeworld.com");

            //            // Add multiple addresses to the To field.
            //            List<String> recipients = new List<String>()
            //{
            //   "mmasoud2010@gmail.com",
            //};

            //            myMessage.AddTo(recipients);

            //            myMessage.Subject = "Testing the SendGrid Library";

            //            //Add the HTML and Text bodies
            //            myMessage.Html = "<p>Hello World!</p>";
            //            myMessage.Text = "Hello World plain text!";


            //            // Create network credentials to access your SendGrid account.
            //            var username = "M.Masoud";
            //            var pswd = "!M123$%^";

            //            var credentials = new NetworkCredential(username, pswd);



            //            //// Create the email object first, then add the properties.
            //            //SendGrid myMessage = SendGrid.GetInstance();
            //            //myMessage.AddTo("anna@example.com");
            //            //myMessage.From = new MailAddress("john@example.com", "John Smith");
            //            //myMessage.Subject = "Testing the SendGrid Library";
            //            //myMessage.Text = "Hello World!";

            //            //// Create credentials, specifying your user name and password.
            //            //var credentials = new NetworkCredential("username", "password");

            //            // Create an Web transport for sending email.
            //            var transportWeb = Web.GetInstance(credentials);

            //            // Send the email.
            //            transportWeb.DeliverAsync(myMessage);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string emailQueueName = Environment.MachineName + "_" + "MyTestQueue";
            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
            var message = client.Receive();



        }

        private void button12_Click(object sender, EventArgs e)
        {
            ICommonDataContext context = CommonDataContext.GetContext(1);

            ContactQuery contactQuery = new ContactQuery(1);
            ContactPM contactPM = contactQuery.GetContactByEmailOnly("majed2@fnarsoft.com", 1);

            //ContactService contactService = new ContactService(context, 1);
            //contactService.Update(contactPM);

            UserQuery userQuery = new UserQuery(1);

            UserPM userPM = userQuery.GetSinglePM(contactPM.Id, 1);

            userPM.Email = "majed3@fnarsoft.com";
            UserService userService = new UserService(context, 1);
            userService.Update(userPM);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //if (Environment.MachineName == "ABDULLAH-PC") btnUpdateAccounting_Click(null, null);
            this.Text += " Environment=" + LogitudeSettings.LogitudeURL;// 4 customs env its must to know which company u updating 

            //string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");
            //LogitudeSettings.DatabaseManagementSystem = dbms;
        }

        private void button13_Click(object sender, EventArgs e)
        {

            //ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(1);
            //List<ARInvoicePM> invoices = invoiceQuery.GetInvoicesByCustomer("1-722",1).ToList();
            //List<CustomerCurrencyCode> result = (from a in invoices

            //                                      group a by new { a.InvoiceCurrencyCode } into gr
            //                                      select new CustomerCurrencyCode()
            //                                      {

            //                                          InvoiceCurrencyCode = gr.Key.InvoiceCurrencyCode,
            //                                      }).ToList();



        }

        private void button14_Click(object sender, EventArgs e)
        {
            //eExact exact = new eExact();
            //exact.Invoices = new Invoice[1];
            //exact.Invoices[0] = new Invoice()
            //{
            //    ordernumber = "1",
            //    invoicenumber = "25",
            //    type = ExactOnlineIntegration.exact.InvoiceType.Item8020,
            //    status = InvoiceStatus.Item50,
            //    InvoiceDate = DateTime.Now,
            //    InvoiceDateSpecified = true,
            //    statusSpecified = true,
            //    typeSpecified = true,
            //    DueDate = DateTime.Now.AddDays(30),
            //    Description = new typeDescription(){ Value = "Invoice1"},
            //    YourRef = "123",
            //    Notes= "Nothing",
            //    OrderDateSpecified = true,
            //    OrderedBy = new typeAccount() { Name = "test1", AccountManager = new typeUser() { FirstName = "Alaa" } },
            //    DeliverTo = new typeAccount() { Name = "test1"},
            //    DeliveryAddress = new DeliveryAddress() {Country = new ExactOnlineIntegration.exact.Country(){code = "PS"} },
            //    InvoiceTo = new typeAccount(){Name = "test1"},
            //    PaymentCondition = new PaymentCondition() { code = "CC", Description = new typeDescription() { Value = "Cash condition" } },

            //    Journal = new Journal() { code = "JJ1", Description = new typeDescription() { Value = "Journal  1" } },


            //};


            // exact.Invoices[0].InvoiceLine = new InvoiceInvoiceLine[2];

            // exact.Invoices[0].InvoiceLine[0] = new InvoiceInvoiceLine()
            // {
            //     line = "1",
            //     Quantity = 2.2f,
            //     UnitPrice = new typePrice() { Value = 20, Currency = new typeCurrency() { code = "USD" } },
            //     Description = new typeDescription() { Value = "line1" },
            //     Item = new typeItem() { code = "AFT", Description = new typeDescription() { Value = "Air Freight" } },
            // };


            // exact.Invoices[0].InvoiceLine[1] = new InvoiceInvoiceLine()
            // {
            //     Description = new typeDescription() { Value = "line2" },
            //     Item = new typeItem() { code = "AFT", Description = new typeDescription() {Value = "Air Freight"}  },
            //     line = "2",
            //     Quantity = 1.3f,
            //     UnitPrice =  new typePrice() { Value = 30, Currency = new typeCurrency() { code = "USD" } },
            //     Unit= new Unit() { code = "cm", Description = new typeDescription() { Value = "Distance" } },

            // };




            ////SalesOrder salesOrder = new SalesOrder()
            ////{
            ////    salesordernumber = "1",
            ////    OrderDate = DateTime.Now,
            ////    DeliveryDate = DateTime.Now.AddDays(30),
            ////    YourRef = "123",



            ////};

            ////salesOrder.OrderedBy = new typeAccount() { code = "AA"};
            //XmlSerializer xsSubmit = new XmlSerializer(typeof(eExact));

            //StringWriter sww = new StringWriter();
            //XmlWriter writer = XmlWriter.Create(sww);
            //xsSubmit.Serialize(writer, exact);
            //var xml = sww.ToString();


            //string path = @"c:\MyTest.txt";

            //// This text is added only once to the file. 
            //if (!File.Exists(path))
            //{
            //    // Create a file to write to. 
            //    string createText = "Hello and Welcome" + Environment.NewLine;
            //    File.WriteAllText(path, createText);
            //}

            //// This text is always added, making the file longer over time 
            //// if it is not deleted. 
            //string appendText = "This is extra text" + Environment.NewLine;
            //File.AppendAllText(path, appendText);

            //// Open the file to read from. 
            //string readText = File.ReadAllText(path);

            //Console.WriteLine(readText);
            ////invoice.InvoiceLine = new InvoiceInvoiceLine[]
            ////{

            ////};

        }

        private void button15_Click(object sender, EventArgs e)
        {
            DateTime fromdate = DateTime.Now.AddDays(-29);
            string from = fromdate.ToString("dd MMM yy");
            from = from.Substring(0, 7) + "'" + from.Substring(7);
            //DateTime fromdate = DateTime.Now.AddDays(-29);
            // string from = fromdate.ToString("dd MMM yy");
            //string s = from.Substring(from.Length - 3);
            //string x = from.Substring(from.Length +1);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //LoadCustomClosedTables.FillCustomsClosedTablesInDb(1);
        }

        //private void button10_Click(object sender, EventArgs e)
        //{
        //    //LoadCustomClosedTables.FillCustomsClosedTablesInDb();
        //    //LoadCustomClosedTables.FillCustomClosedTablesData();
        //    ////error = TranslateTextsClass.("General.M.MinMax", field.FullNameTextCodeCode, field.MinLength.ToString(), field.MaxLength.ToString());
        //    //Customs.BL.Validators.CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclaration("1-1", 1);
        //    //ICustomContext context=CustomContext.GetContext(1);
        //    //CustomsSettingPM customSettings = new CustomsSettingPM() {IsConnectedToUniFreight=true,ChangeSetOp=ChangeSetOperation.Insert };
        //    //CustomsSettingUpdateService serivce = new CustomsSettingUpdateService(context, new Dictionary<string, IContext>(), 1);
        //    //serivce.Update(customSettings,true);


        //    ICustomContext customContext = CustomContext.GetContext(0);
        //    CustomDocumentTypeMetaDataRepository customDocumentTypeMetaDataRepository = new CustomDocumentTypeMetaDataRepository(customContext);
        //    List<CustomDocumentTypeMetaData> customDocumentTypeMetaDataList = customDocumentTypeMetaDataRepository.GetAll().ToList();
        //    CustomMetaDataTypeRepository metaDataTypeRepository = new CustomMetaDataTypeRepository(customContext);
        //    List<CustomMetaDataType> metaDataTypeList = metaDataTypeRepository.GetAll().ToList();
        //    List<CustomMetaDataType> newAddedMetaDataTypeList = new List<CustomMetaDataType>();
        //    Stream myStream = null;
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog();

        //    openFileDialog1.InitialDirectory = "c:\\";
        //    openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        //    openFileDialog1.FilterIndex = 2;
        //    openFileDialog1.RestoreDirectory = true;

        //    if (openFileDialog1.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            if ((myStream = openFileDialog1.OpenFile()) != null)
        //            {
        //                using (myStream)
        //                {
        //                    StreamReader reader = new StreamReader(myStream);
        //                    string responseString = reader.ReadToEnd();
        //                    XmlDocument document = new XmlDocument();
        //                    document.LoadXml(responseString);
        //                    XmlNodeList contentNodes = document.GetElementsByTagName("Content");
        //                    foreach (XmlNode node in contentNodes)
        //                    {
        //                        XmlNode documentTypeCodeNode = node.ChildNodes[0];
        //                        XmlNode metaDataTypeCodeNode = node.ChildNodes[8];
        //                        XmlNode localNameNode = node.ChildNodes[5];
        //                        XmlNode formatNode = node.ChildNodes[4];
        //                        XmlNode mandatoryNode = node.ChildNodes[6];

        //                        string documentTypeCode = documentTypeCodeNode.InnerText;
        //                        string metaDataTypeCode = metaDataTypeCodeNode.InnerText;
        //                        string localName = localNameNode.InnerText;
        //                        string format = formatNode.InnerText;
        //                        bool mandatory = bool.Parse(mandatoryNode.InnerText);


        //                        CustomMetaDataType type = (from a in metaDataTypeList
        //                                                   where a.Code == metaDataTypeCode
        //                                                   select a).FirstOrDefault();

        //                        CustomMetaDataType addedType = (from a in newAddedMetaDataTypeList
        //                                                        where a.Code == metaDataTypeCode
        //                                                        select a).FirstOrDefault();
        //                        if (type == null && addedType == null)
        //                        {
        //                            type = new CustomMetaDataType()
        //                            {
        //                                Code = metaDataTypeCode,
        //                                LocalName = localName,
        //                                SearchFields = metaDataTypeCode + "," + localName,
        //                            };
        //                            metaDataTypeRepository.Add(type);
        //                            newAddedMetaDataTypeList.Add(type);
        //                        }
        //                        else if (type != null)
        //                        {
        //                            type.LocalName = localName;
        //                            type.SearchFields = metaDataTypeCode + "," + localName;
        //                            metaDataTypeRepository.Update(type);
        //                        }

        //                    }
        //                    metaDataTypeRepository.SubmitChanges();
        //                    foreach (XmlNode node in contentNodes)
        //                    {
        //                        XmlNode documentTypeCodeNode = node.ChildNodes[0];
        //                        XmlNode metaDataTypeCodeNode = node.ChildNodes[8];
        //                        XmlNode localNameNode = node.ChildNodes[5];
        //                        XmlNode formatNode = node.ChildNodes[4];
        //                        XmlNode mandatoryNode = node.ChildNodes[6];

        //                        string documentTypeCode = documentTypeCodeNode.InnerText;
        //                        string metaDataTypeCode = metaDataTypeCodeNode.InnerText;
        //                        string localName = localNameNode.InnerText;
        //                        string format = formatNode.InnerText;
        //                        bool mandatory = bool.Parse(mandatoryNode.InnerText);

        //                        CustomDocumentTypeMetaData metadataRecord = (from a in customDocumentTypeMetaDataList
        //                                                                     where a.MetaDataTypeCode == metaDataTypeCode && a.DocumentTypeCode == documentTypeCode
        //                                                                     select a).FirstOrDefault();

        //                        if (metadataRecord == null)
        //                        {
        //                            metadataRecord = new CustomDocumentTypeMetaData()
        //                            {
        //                                DocumentTypeCode = documentTypeCode,
        //                                MetaDataTypeCode = metaDataTypeCode,
        //                                Format = format,
        //                                Mandatory = mandatory,
        //                            };
        //                            customDocumentTypeMetaDataRepository.Add(metadataRecord);
        //                            customDocumentTypeMetaDataList.Add(metadataRecord);
        //                        }
        //                        else
        //                        {
        //                            metadataRecord.Format = format;
        //                            metadataRecord.Mandatory = mandatory;
        //                            customDocumentTypeMetaDataRepository.Update(metadataRecord);
        //                        }
        //                        customDocumentTypeMetaDataRepository.SubmitChanges();
        //                    }
        //                    try
        //                    {
        //                        customDocumentTypeMetaDataRepository.SubmitChanges();
        //                    }
        //                    catch (System.Exception ex)
        //                    {

        //                    }

        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
        //        }
        //    }

        //}

        //private void button11_Click(object sender, EventArgs e)
        //{
        //    string num = "90229000";
        //    int a = 0;
        //    int sum = 0, d;
        //    for (int i = 0; i < num.Length; i++)
        //    {
        //       d = Convert.ToInt32(num.Substring(num.Length - 1 - i, 1));
        //        if (a % 2 == 0)
        //            d = d * 2;
        //        if (d > 9)
        //            d -= 9;
        //        sum += d;
        //        a++;
        //    }

        //    var checkDigit = 10 - (sum % 10);
        //}

        private void FillCustomsTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.toolStripProgressBar1.Visible = true;
                var updateCustomsTest = new Logitude.Update.SandBox.UpdateCustomsTest();
                //tester.FillCustomsRequestsSheetStatusTable();
                updateCustomsTest.Test();
                this.toolStripProgressBar1.Visible = false;
                this.toolStripStatusLabel1.Text = "done !!";
            }
            catch (DbEntityValidationException ex)
            {
                var formatedException = ExceptionFormatUtil.GetFormated(ex);
                //_sbLog.Insert(0, "ProccessRequest():Exception " + formatedException.ToString() + Environment.NewLine + "---------------------------------------------");

            }
            catch (Exception e1)
            {

                throw;
            }

        }

        //private void button12_Click(object sender, EventArgs e)
        //{
        //    ICustomContext context=CustomContext.GetContext(1);
        //    PaymentOrderPM order = new PaymentOrderPM()
        //    {
        //        Id =  IdCounter.GetNumber("Customs.PaymentOrder",1),
        //        Tenant = 1,
        //        Closed = true,
        //        ActualPayDate = DateTime.Now,
        //        PaymentNumber = "55",
        //        ChangeSetOp = ChangeSetOperation.Insert,
        //    };

        //    order.PaymentOrderConnectionTables = new List<PaymentOrderConnectionTablePM>()
        //        {
        //             new PaymentOrderConnectionTablePM()
        //            {
        //                ConnectedEntityId = "1-1",
        //                PaymentOrderId = order.Id,
        //                Tenant = 1,
        //                 ChangeSetOp=ChangeSetOperation.Insert,
        //            },

        //        };

        //        //PaymentOrderMethods = new List<PaymentOrderMethodPM>()
        //        //{
        //        //  new PaymentOrderMethodPM()
        //        //  {
        //        //      Line = 1,
        //        //      Amount = (decimal) 3.02,
        //        //  }
        //        //}


        //  //  PaymentOrderPM order = query.GetSingle("1-1", true, false);
        //  //  order.Closed = true;
        //    PaymentOrderUpdateService updateService = new PaymentOrderUpdateService(context, new Dictionary<string, IContext>(), 1);
        //    updateService.Update(order, true);


        //}

        private void button16_Click(object sender, EventArgs e)
        {
            TenantRepository tenantRep = new TenantRepository(0);
            List<Tenant> tenants = tenantRep.GetTenants().ToList();

            foreach (Tenant tenant in tenants)
            {
                DocumentRepository documentRep = new DocumentRepository(tenant.Id);
                List<Document> documents = documentRep.GetDocuments(tenant.Id).ToList();
                int counter = 0;
                foreach (Document document in documents)
                {
                    if (document.Extension != null)
                    {
                        byte[] theDatainByte = null;
                        //string containername = StorageAcountDetails.GetCurrentContainer(tenant.Id).Name;
                        //CloudBlobContainer blobContainer = StorageAcountDetails.BlobClient.GetContainerReference(containername);
                        //string filename = StorageAcountDetails.GetBlobNameByLocation(document.Id + "." + document.Extension, document.Folder);
                        //var blobfile = blobContainer.GetBlockBlobReference(filename);
                        //if (blobfile.Exists())
                        //{

                        Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                        {
                            FileName = document.Id,
                            FolderName = document.Folder,
                            Extension = document.Extension,
                            Tenant = document.Tenant,
                            FileSize = theDatainByte.Length,
                        };
                        Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                        theDatainByte = storageservice.Read(fileInfo);

                        //using (MemoryStream memstream = new MemoryStream())
                        //    {
                        //        blobfile.DownloadToStream(memstream);
                        //        theDatainByte = memstream.ToArray();
                        //    }

                        document.FileSize = theDatainByte.Length;
                        documentRep.Update(document);
                        counter++;
                        //}
                    }
                    if (counter == 500)
                    {
                        counter = 0;
                        documentRep.SubmitChanges();
                    }
                }
                if (counter != 0)
                    documentRep.SubmitChanges();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {

            ICustomContext context = CustomContext.GetContext(1);
            PhysicalCheckQueryService query = new PhysicalCheckQueryService(1);

            PhysicalCheckPM check = query.GetSingle("1", false, false);
            check.CheckId = "5";

            PhysicalCheckUpdateService updateService = new PhysicalCheckUpdateService(context, new Dictionary<string, IContext>(), 1);
            check.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            updateService.Update(check, true);

        }

        private void btnUpdateAccounting_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "accounting", UpdateAccountinglbl));
            thread.IsBackground = true;
            thread.Start();
        }

        private void updateDocTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<GlobalTenant> globalTenants;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                globalTenants = GlobalTenantRepository.GetGlobalTenants();

            }

            if (globalTenants != null)
            {
                GlobalTenant tenantZero = globalTenants.Where(d => d.Id == 0).FirstOrDefault();
                List<GlobalTenant> upgradableTenants = (from a in globalTenants
                                                        where a.Version != tenantZero.Version && a.Id != 0 && a.Version != -1 && a.IsActive == true
                                                        select a).ToList();
                if (upgradableTenants.Count > 0)
                {
                    foreach (GlobalTenant tenant in upgradableTenants)
                    {
                        if (tenant.Id != 0)
                        {
                            DocumentTypeUpdateClass.UpdateDataForTenant(tenant.Id, "");
                            label1.Text = "Update tenant" + tenant.Id + "completed successfully";
                        }
                    }

                    label1.Text = "Update all tenants completed successfully";


                }
            }


        }

        List<Customer> customers;
        //private void button18_Click(object sender, EventArgs e)
        //{

        //    customers=new List<Customer>();

        //    for (int i = 0; i < 2000; i++)
        //    {
        //        ICommonDataContext context = CommonDataContext.GetContext(1);
        //        Customer customer = context.Customers.FirstOrDefault();
        //        CommonDataContext usedContext = context.GetActiveDbContext() as CommonDataContext;

        //        customers.Add(customer);
        //}

        private void button18_Click(object sender, EventArgs e)
        {
            IShipmentsContext context = ShipmentsContext.GetContext(1);
            ShipmentsContext usedcontext = context.GetActiveDbContext() as ShipmentsContext;
            IQueryable<Shipment> shipments = context.Shipments.Where(d => d.Tenant == 1 && d.SearchFields.Contains("123")).FullTextSearch("123");
            string valueToSearch = "123";
            string queryToExecute = shipments.ToString();

            queryToExecute = queryToExecute + @"and contains(SearchFields,'" + "\"" + valueToSearch + "*\"" + "')";
            // queryToExecute = @"select * from Shipments where contains(SearchFields,'" + "\""+valueToSearch+"*\"" + "')";
            //List<Shipment> shipfromquery = shipments.FullTextSearch<Shipment>(usedcontext, "SearchFields", "123",false); // usedcontext.Database.SqlQuery<Shipment>(queryToExecute).ToList();

            //string querytext=shipments2.ToString();

        }

        private void button19_Click(object sender, EventArgs e)
        {
            int tenant = 1;
            PortRepository portRepository = new PortRepository(tenant);
            PortQuery portQuery = new PortQuery(portRepository);

            IQueryable<PortPM> ports = (from a in portRepository.context.Ports.Include("Country")
                                        where a.Tenant == tenant && a.Country.EnglishName == "Germany"
                                        select new PortPM()
                                        {
                                            AddedManually = a.AddedManually,
                                            Code = a.Code,
                                            CountryId = a.CountryId,
                                            EnglishName = a.EnglishName,
                                            Field1 = a.Field1,
                                            Field2 = a.Field2,
                                            Field3 = a.Field3,
                                            Field4 = a.Field4,
                                            Field5 = a.Field5,
                                            Field6 = a.Field6,
                                            Field7 = a.Field7,
                                            Field8 = a.Field8,
                                            Field9 = a.Field9,
                                            Field10 = a.Field10,
                                            Id = a.Id,
                                            InActive = a.InActive,
                                            IsAir = a.IsAir,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Latitude = a.Latitude,
                                            LocalName = a.LocalName,
                                            Longtitude = a.Longtitude,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            CountryName = a.Country.EnglishName,
                                            SearchFields = a.SearchFields,
                                            CountryCode = a.Country.Code,
                                            CountryEC = a.Country.EC,
                                            StateId = a.StateId,
                                        });
        }

        List<CustomerPM> customerPMs;
        //private void button19_Click(object sender, EventArgs e)
        //{
        //    customerPMs=new List<CustomerPM>();

        //    for (int i = 0; i < 2000; i++)
        //    {
        //        CustomerQuery customerQuery = new CustomerQuery(1);
        //        CustomerPM customer = customerQuery.GetSinglePM("1-711", 1);
        //        customerPMs.Add(customer);
        //    }
        //}






        //IQueryable<ProductTypeList> result = from entity in iQueryable
        //                                     select new ProductTypeList()
        //                                     {
        //                                         Name = entity.Name,
        //                                         Code = entity.Code,
        //                                         SearchFields = entity.SearchFields,

        //                                     };

        private void loadTextCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItemExportTofil_Click(object sender, EventArgs e)
        {
            Logitude.Update.SandBox.TextCodeTasks.SaveTextCodeToDisk(toolStripTextBoxFilePath.Text);
        }

        private void loadTextCodeFromDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logitude.Update.SandBox.TextCodeTasks.LoadTextCodeFromDisk(toolStripTextBoxFilePath.Text);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            new CopyData().Show();
        }

        // Participant
        private void button21_Click(object sender, EventArgs e)
        {
            IQueryable<TenantManagement> ARTenants;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                ARTenants = tenantManagementRepository.GetAllTenants();
                ARTenants = ARTenants.Where(d => d.DistributorCode == "AR");

                scope.Complete();
            }

            foreach (TenantManagement item in ARTenants)
            {
                TenantRepository tenantRepository = new TenantRepository(item.Id);
                AddressRepository addressRepository = new AddressRepository(item.Id);
                AirlineRepository airlineRepository = new AirlineRepository(item.Id);
                ParticipantRepository participantRepository = new ParticipantRepository(item.Id);
                Participant myExistParticipant = participantRepository.GetSingleParticipantByForwarderandAirlineTenant(item.Id, 957);

                Address myAddress = null;
                Airline myAirline = airlineRepository.GetSingleAirlineByCode("AR", item.Id);
                Tenant myTenant = tenantRepository.GetSingleByTenant(item.Id);

                if (myTenant != null)
                {
                    if (myExistParticipant == null)
                    {
                        myAddress = addressRepository.GetSingleAddress(myTenant.AddressId, item.Id);

                        ParticipantPM myParticipant = new ParticipantPM()
                        {
                            Tenant = 957,
                            EnglishName = item.Name,
                            LocalName = item.Name,
                            TTY = item.TTY,
                            ForwarderTenant = item.Id,
                            RegistrationRequested = myAirline == null ? false : myAirline.ChampRegistrationRequested,
                            Registered = myAirline == null ? false : myAirline.IsChampRegistered,
                            PartnerTypeId = "PT",
                            IsDirect = item.IsRestrictedByAirline,
                        };

                        if (myAddress != null)
                        {
                            myParticipant.Addresses.Add(new AddressPM()
                            {
                                Tenant = 957,
                                AddressTypeId = "M",
                                Description = "Main Address",
                                Address1 = myAddress.Address1,
                                Address2 = myAddress.Address2,
                                ATTN = myAddress.ATTN,
                                City = myAddress.City,
                                CountryId = myAddress.CountryId,
                                PhoneNumber = myAddress.PhoneNumber,
                                FaxNumber = myAddress.FaxNumber,
                                Name = myAddress.Name,
                                StateId = myAddress.StateId,
                                ZipCode = myAddress.ZipCode,
                            });
                        }

                        ContactRepository contactRepository = new ContactRepository(957);
                        Contact loggedContact = contactRepository.GetSingleContactByEmail("system@tenant957.com", 957);

                        ICommonDataContext context = CommonDataContext.GetContext(957);
                        ParticipantService service = new ParticipantService(context, myParticipant, loggedContact.Id);
                        service.Create(myParticipant);
                    }
                }
            }
        }

        // AWBDescriptionOfGoods
        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                string mynewline = "";
                string stringOfLines = "";

                string path = @"c:\temp\LH_AWBDescriptionOfGoods.txt";
                FileStream fs = File.OpenRead(path);
                StreamReader reader = new StreamReader(fs);

                while ((mynewline = reader.ReadLine()) != null)
                {
                    stringOfLines = stringOfLines + "\n" + mynewline + "\n";
                }

                this.LoadAWBDescriptionOfGoods(stringOfLines, 0);
            }

            catch (Exception ex)
            {

            }
        }

        int lineCount = 0;
        string sentData = "";
        private void LoadAWBDescriptionOfGoods(string stringOfLines, int startIndex)
        {
            string[] lines = null;
            sentData = "";
            lines = stringOfLines.Split('\n');
            lineCount = lines.Count();

            int endIndex = startIndex + 1000;
            if (endIndex >= lineCount)
            {
                endIndex = lineCount;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i <= lineCount)
                {
                    if (lines[i] != "\r" && lines[i] != "")
                    {
                        sentData = sentData + lines[i] + Environment.NewLine;
                    }
                }

                else
                {
                    break;
                }
            }

            CommonDataDomainService service = new CommonDataDomainService();
            service.LoadAWBDescriptionOfGoodsFromFile(sentData);
        }

        // set is direct on Participant
        private void button23_Click(object sender, EventArgs e)
        {
            IQueryable<TenantManagement> ARTenants;
            IQueryable<TenantManagement> allTenants;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                allTenants = tenantManagementRepository.GetAllTenants();
                ARTenants = allTenants.Where(d => d.TenantTypeCode == "AIR");

                scope.Complete();
            }

            foreach (TenantManagement item in ARTenants)
            {
                ParticipantRepository participantRepository = new ParticipantRepository(item.Id);
                IQueryable<Participant> myParticipants = participantRepository.GetParticipants(item.Id);

                foreach (Participant participant in myParticipants)
                {
                    bool isRestricted = false;
                    TenantManagement myTenant = allTenants.Where(d => d.Id == participant.ForwarderTenant).FirstOrDefault();

                    if (myTenant != null)
                    {
                        isRestricted = myTenant.IsRestrictedByAirline;
                    }

                    participant.IsDirect = isRestricted;
                    participantRepository.Update(participant);
                }

                participantRepository.SubmitChanges();
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sivug.CustomsBookImport.CustomsBookUpsert();
        }

        private void btnAddBatchServicesDefinitions_Click(object sender, EventArgs e)
        {
            TenantsUpdateClass.AddBatchServicesDefinitions();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                string mynewline = "";
                string stringOfLines = "";

                string path = @"c:\temp\currencies.txt";
                FileStream fs = File.OpenRead(path);
                StreamReader reader = new StreamReader(fs);

                while ((mynewline = reader.ReadLine()) != null)
                {
                    stringOfLines = stringOfLines + "\n" + mynewline + "\n";
                }

                this.Loadcurrencies(stringOfLines, 0);
            }

            catch (Exception ex)
            {

            }
        }

        private void Loadcurrencies(string stringOfLines, int startIndex)
        {
            string[] lines = null;
            sentData = "";
            lines = stringOfLines.Split('\n');
            lineCount = lines.Count();

            int endIndex = startIndex + 1000;
            if (endIndex >= lineCount)
            {
                endIndex = lineCount;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i <= lineCount)
                {
                    if (lines[i] != "\r" && lines[i] != "")
                    {
                        sentData = sentData + lines[i] + Environment.NewLine;
                    }
                }

                else
                {
                    break;
                }
            }

            CommonDataDomainService service = new CommonDataDomainService();
            service.LoadCurrenciesFromFile(sentData);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            List<TenantManagement> myTenants = new List<TenantManagement>();
            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                myTenants = tenantManagementRepository.GetAllTenants().ToList();
                scope.Complete();
            }

            var list = new List<List<TenantManagement>>();
            for (int i = 0; i < myTenants.Count; i += 50)
            {
                List<TenantManagement> temp = myTenants.GetRange(i, Math.Min(50, myTenants.Count - i));
                list.Add(temp);
            }

            AirlineRepository airlineRepository = new AirlineRepository(0);
            List<Airline> tenantZeroAirlines = airlineRepository.GetAirlines(0).ToList();

            foreach (var item in list)
            {
                this.FillAirlinesCodes(item, tenantZeroAirlines, tenantManagementRepository);
            }

            label1.Text = "Fill data completed successfully";
        }

        private void FillAirlinesCodes(List<TenantManagement> myTenants, List<Airline> tenantZeroAirlines, TenantManagementRepository repository)
        {
            try
            {
                foreach (TenantManagement item in myTenants)
                {
                    AirlineRepository airlineRepository = new AirlineRepository(item.Id);

                    if (item.AWBMessagesCCSTypeCode == "GLSHK")
                    {
                        tenantZeroAirlines = tenantZeroAirlines.Where(d => d.GLSHKPIMA != null).ToList();
                    }

                    else
                    {
                        tenantZeroAirlines = tenantZeroAirlines.Where(d => d.TTY != null).ToList();
                    }

                    IQueryable<Airline> myTenantAilrines = airlineRepository.GetAirlines(item.Id);
                    List<Airline> myResultAirlines = new List<Airline>();
                    foreach (Airline tenantZeroItem in tenantZeroAirlines)
                    {
                        Airline myTenantItem = myTenantAilrines.Where(d => d.Card.Code == tenantZeroItem.Card.Code).FirstOrDefault();

                        if (myTenantItem != null)
                        {
                            myResultAirlines.Add(myTenantItem);
                        }
                    }

                    List<Airline> airlines_Reg = new List<Airline>();
                    List<Airline> airlines_Req = new List<Airline>();
                    List<Airline> airlines_ReqNotRegDec = new List<Airline>();

                    if (item.AWBMessagesCCSTypeCode == "GLSHK")
                    {
                        airlines_Reg = myResultAirlines.Where(d => d.IsGLSHKRegistered).ToList();
                        airlines_Req = myResultAirlines.Where(d => d.GLSHKRegistrationRequested).ToList();
                        airlines_ReqNotRegDec = myResultAirlines.Where(d => d.GLSHKRegistrationRequested && !d.IsGLSHKRegistered && !d.IsDeclined).ToList();
                    }

                    else
                    {
                        airlines_Reg = myResultAirlines.Where(d => d.IsChampRegistered).ToList();
                        airlines_Req = myResultAirlines.Where(d => d.ChampRegistrationRequested).ToList();
                        airlines_ReqNotRegDec = myResultAirlines.Where(d => d.ChampRegistrationRequested && !d.IsChampRegistered && !d.IsDeclined).ToList();
                    }

                    string str_reg = null;
                    string str_req = null;
                    string str_pen = null;

                    foreach (Airline a in airlines_Reg)
                    {
                        if (string.IsNullOrEmpty(str_reg))
                        {
                            str_reg = a.Card.Code;
                        }
                        else
                        {
                            str_reg = str_reg + ", " + a.Card.Code;
                        }
                    }

                    foreach (Airline a in airlines_Req)
                    {
                        if (string.IsNullOrEmpty(str_req))
                        {
                            str_req = a.Card.Code;
                        }
                        else
                        {
                            str_req = str_req + ", " + a.Card.Code;
                        }
                    }

                    foreach (Airline a in airlines_ReqNotRegDec)
                    {
                        if (string.IsNullOrEmpty(str_pen))
                        {
                            str_pen = a.Card.Code;
                        }
                        else
                        {
                            str_pen = str_pen + ", " + a.Card.Code;
                        }
                    }

                    item.RegisteredAirlines = str_reg;
                    item.RequestedAirlines = str_req;
                    item.PendingAirlines = str_pen;

                    repository.Update(item);
                }

                repository.SubmitChanges();
            }

            catch (Exception ex)
            {
            }


        }

        private void button26_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(UpdateZipFiles);
            thread.IsBackground = true;
            thread.Start();
        }

        bool buildCustomsZipFiles = false;
        private int _SeedTenant=0;

        private void UpdateZipFiles()
        {
            //timer 
            if (generalLabel != null) SetControlPropertyValue(generalLabel, "Text", "Updating...");

            if (buildCustomsZipFiles)
            {
                SetControlPropertyValue(CustZibFilesLbl, "Text", "Building...");
                generalLabel = CustZibFilesLbl; // timer
            }
            else
            {
                SetControlPropertyValue(BuildZipFileslbl, "Text", "Building...");
                generalLabel = BuildZipFileslbl; // timer
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // timer
            globalStopwatch = stopWatch;
            timer1.Enabled = true;
            timer1.Start();

            TenantsUpdateClass.BuildObjectTablesZipFilesData(checkBox1.Checked, buildCustomsZipFiles);

            // timer
            globalStopwatch = null;
            generalLabel = null;
            timer1.Start();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            if (buildCustomsZipFiles)
            {
                SetControlPropertyValue(CustZibFilesLbl, "Text", "Done in " + ts.ToString(@"hh\:mm\:ss"));
                SetControlPropertyValue(CustZibFilesLbl, "ForeColor", Color.Green);
                buildCustomsZipFiles = false;
            }
            else
            {
                SetControlPropertyValue(BuildZipFileslbl, "ForeColor", Color.Green);
                SetControlPropertyValue(BuildZipFileslbl, "Text", "Done in " + ts.ToString(@"hh\:mm\:ss"));
            }

        }

        private void DownLoadZipFile_Click(object sender, EventArgs e)
        {
            TenantsUpdateClass.DownloadEntityResource();

            label1.Text = "DownLoad Zip File completed successfully";
        }

        private void productionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myPatchDistributionManager = new PatchDistributionManager();
            myPatchDistributionManager.CreateList();

        }

        private void button27_Click(object sender, EventArgs e)
        {
            new CopyData().Show();
        }

        private void ConvertXmalTemplateToHtmlButton_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(() => UpdateModule(0, "converttemplatefromxmaltohtml", ConvertXmalTemplateLable));
            thread.IsBackground = true;
            thread.Start();
        }

        private void WarehouseButton_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "warehouse", WarehouseLable));
            thread.IsBackground = true;
            thread.Start();
        }

        private void internationalSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "csv Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = false;

            // Call the ShowDialog method to show the dialog box.
            DialogResult res = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                var lines = new List<String>(File.ReadAllLines(openFileDialog1.FileName));

                var log = Logitude.CustomsMessaging.ResponseServices.SYSTBL_NG_9001_MSG_SystemTablesResponseService
                    .internationalSiteUpSert(lines);
                MessageBox.Show(log);
            }

        }

        private void CacheOnClientUpdate()
        {
            var cacheOnClientUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "cacheOnClientUpdateToolStripMenuItem",
                Size = new System.Drawing.Size(192, 22),
                Text = "CacheOnClientUpdate"

            };
            cacheOnClientUpdateToolStripMenuItem.Click += //new System.EventHandler(this.cacheOnClientUpdateToolStripMenuItem_Click);
                (s1, e1) =>
                {
                    Logitude.BL.Helpers.TableLastUpdateClass.UpdateCacheTableHistory();
                };
            this.sandBoxToolStripMenuItem.DropDownItems.Add(cacheOnClientUpdateToolStripMenuItem);
        }

        private void createDecToolStripMenuItem_Click(object sender, EventArgs e)
        {


            try
            {

                string MoreParams = "";
                string MessageOut = "";
                var ListEntry = new Dictionary<string, string>();
                ListEntry.Add("tenant", "1");
                ListEntry.Add("UNIFREIGHT_USER_ID", "ITZIK");
                MoreParams = UnifreightListsUtil.Serialize(ListEntry);
                int iTenanat; string contactEmail;
                AuthenticationUtil.UnifreightImpersonate(MoreParams, out iTenanat, out contactEmail);
                //Test1();
                //616400280
                //311100000

                //using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(10)))//new TransactionScope(TransactionScopeOption.RequiresNew, TimeSpan.FromMinutes(10)))

                //Task.Factory.StartNew(() =>
                //{
                //    for (int i = 150000; i < 160000; i++)
                //    {
                //        DeclarationUpsertService.TestDeclarationAdd(i, "1-2861");
                //    }
                //});

                Task.Factory.StartNew(() =>
                {
                    for (int i = 169046; i < 170000; i++)
                    {
                        DeclarationUpsertService.TestDeclarationAdd(i, "1-3077");
                    }
                });

                //Task.Factory.StartNew(() =>
                //{
                //    for (int i = 170000; i < 180000; i++)
                //    {
                //        DeclarationUpsertService.TestDeclarationAdd(i, "1-2970");

                //    }
                //});

                //Task.Factory.StartNew(() =>
                //{
                //    for (int i = 180000; i < 190000; i++)
                //    {
                //        DeclarationUpsertService.TestDeclarationAdd(i, "1-2889");
                //    }
                //});
                Task.Factory.StartNew(() =>
                {
                    for (int i = 199138; i < 200000; i++)
                    {
                        DeclarationUpsertService.TestDeclarationAdd(i, "1-2940");
                    }
                });


            }
            catch (Exception eee)
            {

                MessageBox.Show(eee.ToString());

            }

        }

        private void button28_Click(object sender, EventArgs e)
        {
            this.buildCustomsZipFiles = true;
            Thread thread = new Thread(UpdateZipFiles);
            thread.IsBackground = true;
            thread.Start();
        }

        private void ConvertSignatureButton_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "convertsignaturefromxmaltohtml", ConvertSignatureLable));
            thread.IsBackground = true;
            thread.Start();
        }
        //private void button29_Click(object sender, EventArgs e)
        //{
        //    Thread thread = new Thread(() => UpdateMetaDataForCustoms(true));
        //    thread.IsBackground = true;
        //    thread.Start();
        //}

        private void button30_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateMetaDataForCustoms(false));
            thread.IsBackground = true;
            thread.Start();
        }

        public void UpdateMetaDataForCustoms(bool withTenant0)
        {
            if (withTenant0)
            {
                try
                {
                    SetControlPropertyValue(UTZSLabel, "Text", "Updating Tenant 0...");
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    UpdateModule(0, "UpdateTenantZero", UpdateTenant0lbl);
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    SetControlPropertyValue(UTZSLabel, "Text", "Updating Tenant 0 Is Done in:" + ts.ToString());
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("Exception Update tenant 0 =" + ex1.ToString());
                    throw;
                }
            }

            try
            {

                SetControlPropertyValue(lblUShipment, "Text", "Updating Customs...");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                UpdateModule(0, "customs", UpdateCustomslbl);
                Func<string> GetConnetionStringFunc = () =>
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox(
                        @"System Connection String is : 
User/Pass",
                               "I Need System ConnString For Grant",
                               "system/manager1",
                               0,
                               0);
                    if (String.IsNullOrWhiteSpace(input))
                    {
                        input = "system/manager1";
                    }
                    return input;
                };
                Logitude.Customs.BL.Utils.GrantCCUTableUtil.GrantCCUTo(1, GetConnetionStringFunc);

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                SetControlPropertyValue(lblUShipment, "Text", "Updating Customs Is Done in: " + ts.ToString());
            }
            catch (Exception ex2)
            {
                MessageBox.Show("Exception in Update Customs =" + ex2.ToString());
                throw;

            }

            try
            {
                SetControlPropertyValue(UTenantsStatusLabel, "Text", "Updating Tenants...");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                UpdateTenants();

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                SetControlPropertyValue(UTenantsStatusLabel, "Text", "Updating Tenants Is Done in: " + ts.ToString());

            }
            catch (Exception ex3)
            {
                MessageBox.Show("Exception in Update Tenants =" + ex3.ToString());
                throw;

            }

            try
            {
                SetControlPropertyValue(BuildZipFilesStatusLabel, "Text", "Building Zip Files For General MetaData...");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                this.UpdateZipFiles();

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                SetControlPropertyValue(BuildZipFilesStatusLabel, "Text", "Building Zip Files For General MetaData Is Done in: " + ts.ToString());
            }
            catch (Exception ex3)
            {
                MessageBox.Show("Exception in Build Zip Files =" + ex3.ToString());
                throw;

            }

            try
            {
                SetControlPropertyValue(BuildZipFilesCustomsStatusLabel, "Text", "Building Zip Files For Customs MetaData...");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                this.buildCustomsZipFiles = true;
                this.UpdateZipFiles();

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                SetControlPropertyValue(BuildZipFilesCustomsStatusLabel, "Text", "Building Zip Files For Customs MetaData Is Done in: " + ts.ToString());
            }
            catch (Exception ex3)
            {
                MessageBox.Show("Exception in Build Zip Files for Customs =" + ex3.ToString());
                throw;

            }


        }

        private void button31_Click(object sender, EventArgs e)
        {
            EnhancedUpdateForm form = new Logitude.Update.EnhancedUpdateForm();
            form.ParentForm1 = this;
            form.Show();
            //XmlTextReader reader = new XmlTextReader("UpdateSettings.xml");
            //while (reader.Read())
            //{
            //    switch (reader.NodeType)
            //    {
            //        case XmlNodeType.Element: // The node is an element.
            //            Console.Write("<" + reader.Name);
            //            Console.WriteLine(">");
            //            break;
            //        case XmlNodeType.Text: //Display the text in each element.
            //            Console.WriteLine(reader.Value);
            //            break;
            //        case XmlNodeType.EndElement: //Display the end of the element.
            //            Console.Write("</" + reader.Name);
            //            Console.WriteLine(">");
            //            break;
            //    }
            //}
            //XmlDataDocument xmldoc = new XmlDataDocument();
            //XmlNodeList xmlnode;
            //int i = 0;
            //string str = null;
            //FileStream fs = new FileStream("UpdateSettings.xml", FileMode.Open, FileAccess.Read);
            //xmldoc.Load(fs);
            //xmlnode = xmldoc.GetElementsByTagName("Update");
            //for (i = 0; i <= xmlnode.Count - 1; i++)
            //{
            //    xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
            //    str = xmlnode[i].InnerText.Trim();
            //    switch (str)
            //    {
            //        case "tenant0":
            //            {
            //                break;
            //            }
            //        case "customs":
            //            {
            //                break;
            //            }
            //    }
            //}
        }

        private void metaDataUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnhancedUpdateForm form = new Logitude.Update.EnhancedUpdateForm();
            form.ParentForm1 = this;
            form.Show();
        }

        private void _UpdatePortsButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "csv|*.csv";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stream stream = openFileDialog.OpenFile();
                StreamReader streamReader = new StreamReader(stream);
                this.UpdatePortsMethod(streamReader);
                //Thread thread = new Thread(() => UpdatePortsMethod(streamReader));
                //thread.IsBackground = true;
                //thread.Start();


                //List<DataItem> distinctItems = AllDataLines.GroupBy(x => new { x.PortCode, x.CountryCode }).Select(y => y.First()).ToList();
                //var count = distinctItems.Count();
                //var counr2 = AllDataLines.Count();

                //Thread thread = new Thread(() => this.Run(distinctItems));
                //thread.IsBackground = true;
                //thread.Start();
            }
        }

        private void UpdatePortsMethod(StreamReader streamReader)
        {
            List<DataItem> AllDataLines = new List<DataItem>();

            string line = "";
            string[] lineParts = null;
            while ((line = streamReader.ReadLine()) != null)
            {
                lineParts = line.Split(',');

                if (lineParts.Count() >= 3)
                {
                    string countryCode = this.GetText(lineParts, 0);
                    string portCode = this.GetText(lineParts, 1);

                    if (portCode != null && countryCode != null)
                    {
                        portCode = portCode.ToUpper();
                        countryCode = countryCode.ToUpper();

                        string portEnglishName = this.GetText(lineParts, 2);
                        string portLocalName = this.GetText(lineParts, 3);
                        bool isOcean = this.GetText(lineParts, 4) == "1" ? true : false;
                        bool isAir = this.GetText(lineParts, 5) == "1" ? true : false;
                        bool isInland = this.GetText(lineParts, 6) == "1" ? true : false;

                        if (portEnglishName.Length >= 40)
                        {
                            portEnglishName = portEnglishName.Substring(0, 40);
                        }
                        if (portLocalName.Length >= 40)
                        {
                            portLocalName = portLocalName.Substring(0, 40);
                        }

                        DataItem myDataItem = new DataItem();
                        myDataItem.CountryCode = countryCode;
                        myDataItem.PortCode = portCode;
                        myDataItem.PortEnglishName = portEnglishName;
                        myDataItem.PortLocalName = portLocalName;
                        myDataItem.IsOcean = isOcean;
                        myDataItem.IsAir = isAir;
                        myDataItem.IsInland = isInland;
                        AllDataLines.Add(myDataItem);
                    }
                }
            }

            List<DataItem> distinctItems = AllDataLines.GroupBy(p => new { p.PortCode, p.CountryCode }).Select(g => g.Last()).ToList();
            Thread thread = new Thread(() => this.Run(distinctItems));
            thread.IsBackground = true;
            thread.Start();
        }

        private string GetText(string[] lineParts, int index)
        {
            string myResult = null;

            if (lineParts != null)
            {
                if (lineParts.Count() >= index + 1)
                {
                    myResult = this.TrimString(lineParts[index]);

                    if (myResult == "NULL")
                    {
                        myResult = null;
                    }
                }
            }

            return myResult;
        }
        private string TrimString(string myString)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(myString))
            {
                myResult = myString.Trim();
            }

            return myResult;
        }
        private void Run(List<DataItem> allDataLines)
        {
            if (allDataLines.Count > 0)
            {
                //List<string> allPortsCodes = allDataLines.Where(d => d.PortCode != null).Select(s => s.PortCode).ToList();

                List<DataItem> portsList = allDataLines.GroupBy(p => p.PortCode).Select(g => g.First()).ToList();
                List<string> allPortsCodes = portsList.Select(a => a.PortCode).ToList();

                //List<string> allCountriesCodes = allDataLines.Where(d => d.CountryCode != null).Select(s => s.CountryCode).ToList();
                List<DataItem> countriesList = allDataLines.GroupBy(p => p.CountryCode).Select(g => g.First()).ToList();
                List<string> allCountriesCodes = countriesList.Select(a => a.CountryCode).ToList();

                SetControlPropertyValue(UpdatePortslbl, "Text", "Updating...");

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                ICommonDataContext myCommonContext = CommonDataContext.GetContext(0);
                PortRepository portRepository = new PortRepository(myCommonContext);
                CountryRepository countryRepository = new CountryRepository(myCommonContext);

                List<Port> allPorts = (from d in myCommonContext.Ports
                                       where d.Tenant == 0
                                       && allPortsCodes.Contains(d.Code)
                                       select d).ToList();

                List<Country> allCountries = (from d in myCommonContext.Countries
                                              where d.Tenant == 0
                                              && allCountriesCodes.Contains(d.Code)
                                              select d).ToList();

                var myCount = 0;
                var count = 0;
                var missedCountries = "";
                var isUpdated = false;
                foreach (DataItem item in allDataLines)
                {
                    count++;

                    Country myCountry = allCountries.Where(d => d.Code == item.CountryCode).FirstOrDefault();
                    if (myCountry == null)
                    {
                        missedCountries = missedCountries + item.CountryCode + ", ";
                    }
                    else
                    {
                        Port myPort = allPorts.Where(d => d.Code == item.PortCode && d.CountryId == myCountry.Id && d.Tenant == 0).FirstOrDefault();
                        if (myPort != null && myPort.EnglishName != item.PortEnglishName
                            && myPort.CountryId != myCountry.Id
                            && myPort.IsAir != item.IsAir
                            && myPort.IsOcean != item.IsOcean
                            && myPort.IsInland != item.IsInland) // update port
                        {
                            isUpdated = true;
                            myPort.EnglishName = item.PortEnglishName;
                            myPort.LocalName = item.PortLocalName;
                            myPort.IsAir = item.IsAir;
                            myPort.IsOcean = item.IsOcean;
                            myPort.IsInland = item.IsInland;
                            myPort.CountryId = myCountry.Id;
                            myPort.SearchFields = BuildPortSearchFields(myPort, myCountry);
                            myPort.Tenant = 0;
                            portRepository.Update(myPort);
                        }

                        else if (myPort == null) // insert port
                        {
                            isUpdated = true;
                            myPort = new Port()
                            {
                                Id = IdCounter.GetNumber("Port", 0).ToString(),
                                Code = item.PortCode,
                                EnglishName = item.PortEnglishName,
                                LocalName = item.PortLocalName,
                                IsAir = item.IsAir,
                                IsOcean = item.IsOcean,
                                IsInland = item.IsInland,
                                CountryId = myCountry.Id,
                                Tenant = 0,
                            };

                            myPort.SearchFields = BuildPortSearchFields(myPort, myCountry);
                            portRepository.Add(myPort);
                        }
                    }

                    if (myCount == 1000)
                    {
                        if (isUpdated)
                        {
                            portRepository.SubmitChanges();
                        }
                        myCount = 0;
                        isUpdated = false;
                    }

                    myCount++;
                }

                portRepository.SubmitChanges();

                stopWatch.Stop();

                if (!string.IsNullOrEmpty(missedCountries))
                {
                    MessageBox.Show("All missing Countries: " + missedCountries, "Missing Countries", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                TimeSpan ts = stopWatch.Elapsed;
                SetControlPropertyValue(UpdatePortslbl, "Text", "Done in " + ts.ToString());
            }
        }
        private string BuildPortSearchFields(Port entity, Country country)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entity.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entity.LocalName);

            if (!string.IsNullOrEmpty(entity.CountryId))
            {
                MethodHelper.AddToSearchFields(ref mySearchFields, country.Code);
                MethodHelper.AddToSearchFields(ref mySearchFields, country.EnglishName);
            }

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            return mySearchFields;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            //Thread thread = new Thread(() => UpdateModule(0, "TimeManagement", UpdateCRMlbl));
            //thread.IsBackground = true;
            //thread.Start();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "TimeManagement", UpdateTMlbl));
            thread.IsBackground = true;
            thread.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (globalStopwatch != null && generalLabel != null)
            {
                TimeSpan elapsedTime = globalStopwatch.Elapsed;

                if (generalLabel == BuildZipFileslbl || generalLabel == CustZibFilesLbl)
                    SetControlPropertyValue(generalLabel, "Text", "Building " + elapsedTime.ToString(@"hh\:mm\:ss"));
                else
                    SetControlPropertyValue(generalLabel, "Text", "Updating " + elapsedTime.ToString(@"hh\:mm\:ss"));
            }
        }

        private void CopyReportButton_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "copyreportonalltenant", CopyReportButtonLable));
            thread.IsBackground = true;
            thread.Start();
        }

        private string GetValue(string[] columns, int index)
        {
            string myResult = null;

            if (columns != null)
            {
                if (columns.Count() >= index + 1)
                {
                    myResult = this.TrimString(columns[index]);

                    if (myResult == "NULL")
                    {
                        myResult = null;
                    }
                }
            }

            return myResult;
        }


        //Warehouses
        private void button33_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "csv|*.csv";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stream stream = dialog.OpenFile();
                StreamReader streamReader = new StreamReader(stream);
                this.AddWarehousesMethod(streamReader);
            }
        }

        private void AddWarehousesMethod(StreamReader streamReader)
        {
            SetControlPropertyValue(addWarehouseLabel, "Text", "Reading Excel...");
            List<WarehouseItem> AllDataLines = new List<WarehouseItem>();

            string line = "";
            string[] columns = null;
            while ((line = streamReader.ReadLine()) != null)
            {
                columns = line.Split(',');

                string code = this.GetValue(columns, 0);
                string name = this.GetValue(columns, 1);
                string address1 = this.GetValue(columns, 2);
                string city = this.GetValue(columns, 3);
                string state = this.GetValue(columns, 4);
                string zipCode = this.GetValue(columns, 5);

                if (!string.IsNullOrEmpty(code))
                {
                    if (code.Length > 5)
                    {
                        code = code.Substring(0, 5);
                    }
                }

                if (!string.IsNullOrEmpty(name))
                {
                    if (name.Length > 70)
                    {
                        name = name.Substring(0, 70);
                    }
                }

                if (!string.IsNullOrEmpty(address1))
                {
                    if (address1.Length > 65)
                    {
                        address1 = address1.Substring(0, 65);
                    }
                }

                if (!string.IsNullOrEmpty(city))
                {
                    if (city.Length > 25)
                    {
                        city = city.Substring(0, 25);
                    }
                }

                if (!string.IsNullOrEmpty(zipCode))
                {
                    if (zipCode.Length > 15)
                    {
                        zipCode = zipCode.Substring(0, 15);
                    }
                }

                WarehouseItem myItem = new WarehouseItem();
                myItem.Code = code;
                myItem.Name = name;
                myItem.Address1 = address1;
                myItem.City = city;
                myItem.State = state;
                myItem.ZipCode = zipCode;

                WarehouseItem xItem = AllDataLines.Where(d => d.Code == code).FirstOrDefault();
                if (xItem == null)
                {
                    AllDataLines.Add(myItem);
                }
            }

            Thread thread = new Thread(() => this.RunAddingWarehouse(AllDataLines));
            thread.IsBackground = true;
            thread.Start();

        }

        private void exportRoleFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RoleRepository roleRep = new RoleRepository(0);
            RoleFeatureRepository rep = new RoleFeatureRepository(0);
            FeatureRepository featurrep = new FeatureRepository(0);
            ObjectTableRepository objecttablerep = new ObjectTableRepository(0);
            List<Role> roles = roleRep.GetRoles(0).ToList();
            List<RoleFeature> rolefeatures = rep.GetRoleFeaturesByTenant(0).OrderBy(f => f.RoleId).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (RoleFeature rolefeature in rolefeatures)
            {
                Feature feature = featurrep.GetSingleFeature(rolefeature.FeatureId);
                ObjectTable table = objecttablerep.GetSingleObjectTable(feature.ObjectTableId, 0, true);
                Role role = roles.Where(d => d.Id == rolefeature.RoleId).FirstOrDefault();
                string line = role.Code + "," + table.Name + "," + feature.Code + "," + rolefeature.FeatureAccessLevelCode;
                sb.AppendLine(line);
            }


            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());

            if (buffer != null)
            {
                //MessageBox window = new MessageBox();
                DialogResult result = MessageBox.Show("Export ? ", "", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    try
                    {
                        SaveFileDialog dlg = new SaveFileDialog();
                        dlg.Filter = "Microsoft Excel (*.csv)|*.csv";
                        dlg.DefaultExt = "csv";

                        string defaultname = "Role features" + "_" + DateTime.Now.ToShortDateString();
                        dlg.FileName = defaultname.Replace("/", "-");

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {

                            Stream fs = (Stream)dlg.OpenFile();
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                        }

                        else
                        {
                            return;
                        }
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }

            }


        }

        private void exportPackageFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PackageFeatureRepository rep = new PackageFeatureRepository(0);
            FeatureRepository featurrep = new FeatureRepository(0);
            ObjectTableRepository objecttablerep = new ObjectTableRepository(0);
            List<PackageFeature> packagefeatures = rep.GetPackageFeaturesByTenant(0).OrderBy(f => f.PackageCode).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (PackageFeature packagefeature in packagefeatures)
            {
                Feature feature = featurrep.GetSingleFeature(packagefeature.FeatureId);
                ObjectTable table = objecttablerep.GetSingleObjectTable(feature.ObjectTableId, 0, true);
                string line = packagefeature.PackageCode + "," + table.Name + "," + feature.Code;
                sb.AppendLine(line);
            }


            byte[] buffer = Encoding.ASCII.GetBytes(sb.ToString());
            if (buffer != null)
            {
                DialogResult result = MessageBox.Show("Export ? ", "", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    try
                    {
                        SaveFileDialog dlg = new SaveFileDialog();
                        dlg.Filter = "Microsoft Excel (*.csv)|*.csv";
                        dlg.DefaultExt = "csv";

                        string defaultname = "package features" + "_" + DateTime.Now.ToShortDateString();
                        dlg.FileName = defaultname.Replace("/", "-");

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {

                            Stream fs = (Stream)dlg.OpenFile();
                            fs.Write(buffer, 0, buffer.Length);
                            fs.Close();
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + Environment.NewLine + ex.StackTrace);
                    }
                }
            }
        }

        private void impPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Microsoft Excel (*.csv)|*.csv";
                dlg.DefaultExt = "csv";

                string defaultname = "package features" + "_*"; ///+ DateTime.Now.ToShortDateString();
                dlg.FileName = defaultname.Replace("/", "-");

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    var data = File.ReadAllBytes(dlg.FileName);

                    var helper = new WebFreight.Web.WebServices.ExportImportHelper();
                    string message = helper.ImportPackageFeatures(data);
                }
            }


        }

        private void expPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportPackageFeaturesToolStripMenuItem_Click(sender, e);
        }

        private void expRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportRoleFeaturesToolStripMenuItem_Click(sender, e);
        }

        private void impRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Microsoft Excel (*.csv)|*.csv";
                dlg.DefaultExt = "csv";

                string defaultname = "Role features" + "_*";// + DateTime.Now.ToShortDateString();
                dlg.FileName = defaultname.Replace("/", "-");

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    var data = File.ReadAllBytes(dlg.FileName);

                    var helper = new WebFreight.Web.WebServices.ExportImportHelper();
                    string message = helper.ImportRoleFeatures(data);
                }
            }
        }

        private void evaluateInvoiceTotalFrieghtToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //Thread thread = new Thread(() => UpdateModule(0, "UpdateTenantZero", UpdateTenant0lbl));
            Thread thread = new Thread(() =>
            {
                SetControlPropertyValue(FrieghtTotalLabel, "Text", "Evaluate fright totals...");

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                EvaluateInvoiceTotalFrieght();

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                SetControlPropertyValue(FrieghtTotalLabel, "Text", "Evaluate fright totals DONE in " + ts.ToString());

            });
            thread.IsBackground = true;
            thread.Start();

        }
        void EvaluateInvoiceTotalFrieght()
        {
            var tenant = 1;

            //get invoices which has no total freight
            SupplierInvoiceRepository invoiceRepo = new SupplierInvoiceRepository(tenant);
            DeclarationRepository decRepo = new DeclarationRepository(tenant);
            CustomsExchangeRateQueryService rateQuery = new CustomsExchangeRateQueryService(tenant);
            SupplierInvoiceFreightAmountRepository freightRepo = new SupplierInvoiceFreightAmountRepository(tenant);

            List<SupplierInvoice> invoices = invoiceRepo.GetSupplierInvoicesWithoutTotalFrieght(tenant);

            invoices.ForEach((inv) =>
            {
                //get taxation date
                Declaration declaration = decRepo.GetSingle(inv.DeclarationId, tenant);
                DateTime? taxDate = declaration.TaxationDateTime;

                //get frieghts
                List<SupplierInvoiceFreightAmount> invoiceFreights = freightRepo.GetMulti(new SupplierInvoiceKeys() { DeclarationId = declaration.Id, InvoiceCounterKey = inv.InvoiceCounterKey });

                //calculate
                decimal? totalFreightInNIS = 0;

                foreach (SupplierInvoiceFreightAmount item in invoiceFreights)
                {
                    decimal? amountInNIS;

                    if (item.CurrencyTypeCode == "ILS")
                    {
                        amountInNIS = item.Amount;
                        totalFreightInNIS += item.Amount;
                    }
                    else
                    {
                        CustomsExchangeRatePM rate;
                        rate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.CurrencyTypeCode, declaration.TaxationDateTime, tenant);
                        if (rate != null)
                        {
                            amountInNIS = item.Amount * rate.ExchangeRate;
                            totalFreightInNIS += amountInNIS;
                        }
                    }
                }


                decimal? totalFreightInInvoice = 0;
                if (inv.FreightCurrencyTypeCode == "ILS")
                {
                    totalFreightInInvoice = totalFreightInNIS;
                }
                else
                {
                    CustomsExchangeRatePM invoiceRate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(inv.FreightCurrencyTypeCode, declaration.TaxationDateTime, tenant);
                    if (invoiceRate != null)
                    {
                        totalFreightInInvoice = totalFreightInNIS / invoiceRate.ExchangeRate;
                    }
                }


                inv.TotalFreightInFreightCurrency = totalFreightInInvoice;

                inv.TotalFreightInNIS = totalFreightInNIS;

                //save changes
                invoiceRepo.SubmitChanges();

            });



        }



        private void RunAddingWarehouse(List<WarehouseItem> allDataLines)
        {
            allDataLines = allDataLines.Where(d => !string.IsNullOrEmpty(d.Code) && !string.IsNullOrEmpty(d.Name) && !string.IsNullOrEmpty(d.Address1) && !string.IsNullOrEmpty(d.City)).ToList();

            if (allDataLines.Count > 0)
            {
                ICommonDataContext myCommonContext = CommonDataContext.GetContext(0);
                Country US_Country = myCommonContext.Countries.Where(d => d.Tenant == 0 && d.Code == "US").FirstOrDefault();
                Contact systemContact = myCommonContext.Contacts.Where(d => d.Email == "system@tenant0.com" && d.Tenant == 0).FirstOrDefault();

                if (US_Country != null)
                {
                    SetControlPropertyValue(addWarehouseLabel, "Text", "Adding...");
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();

                    var myCount = 0;
                    foreach (WarehouseItem item in allDataLines)
                    {
                        State myState = myCommonContext.States.Where(d => d.Tenant == 0 && d.Code == item.State).FirstOrDefault();
                        Card entityCard = myCommonContext.Cards.Where(d => d.Code == item.Code && d.Tenant == 0 && d.PartnerTypeId == "WH").FirstOrDefault();

                        if (myState != null && entityCard == null)
                        {
                            string entityId = IdCounter.GetNumber("Card", 0).ToString();

                            entityCard = new Card()
                            {
                                Id = entityId,
                                Tenant = 0,
                                PartnerTypeId = "WH",
                                CreateDate = DateTime.Now,
                                UpdateDate = DateTime.Now,
                                CreatedByUserId = systemContact.Id,
                                UpdatedByUserId = systemContact.Id,
                                CityName = item.City,
                                CountryCode = US_Country.Code,
                                CountryId = US_Country.Id,
                                CountryName = US_Country.EnglishName,
                                Code = item.Code,
                                EnglishName = item.Name,
                            };

                            Warehouse entityWarehouse = new Warehouse()
                            {
                                Id = entityId,
                                Tenant = 0,
                                FirmCode = item.Code,
                            };

                            Address entityAddress = new Address()
                            {
                                Id = IdCounter.GetNumber("Address", 0).ToString(),
                                Tenant = 0,
                                CardId = entityId,
                                Address1 = item.Address1,
                                CountryId = US_Country.Id,
                                AddressTypeId = "M",
                                City = item.City,
                                Description = item.Name,
                                Name = item.Name,
                                StateId = myState.Id,
                                ZipCode = item.ZipCode,
                            };

                            entityCard.SearchFields = this.BuildCardSearchFields(entityCard);
                            entityAddress.SearchFields = this.BuildAddressSearchFields(entityAddress, US_Country.EnglishName);

                            myCommonContext.Cards.Add(entityCard);
                            myCommonContext.Warehouses.Add(entityWarehouse);
                            myCommonContext.Addresses.Add(entityAddress);

                            if (myCount == 1000)
                            {
                                myCommonContext.SaveChanges();
                                myCount = 0;
                            }

                            myCount++;
                        }
                    }

                    myCommonContext.SaveChanges();
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    SetControlPropertyValue(addWarehouseLabel, "Text", "Done in " + ts.ToString());
                }
            }
        }

        private string BuildCardSearchFields(Card entityCard)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CityName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CountryName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            return mySearchFields;
        }

        private string BuildAddressSearchFields(Address entityAddress, string countryName)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityAddress.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityAddress.Address1);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityAddress.ZipCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityAddress.City);
            MethodHelper.AddToSearchFields(ref mySearchFields, countryName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            return mySearchFields;
        }

        //Computing Partner
        private void button34_Click(object sender, EventArgs e)
        {
            ComputingPartnerFillings("Card");
        }

        private void ComputingPartnerFillings(string TableName)
        {
            if (tenant_TXT.Text.Trim() != "")
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "csv|*.csv";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Stream stream = dialog.OpenFile();
                    StreamReader streamReader = new StreamReader(stream);
                    this.AddComputingPartner(streamReader, TableName);
                }
            }
            else
            {
                MessageBox.Show("Please fill the Tenant Number above ! .");
            }

        }
        private void AddComputingPartner(StreamReader streamReader, string TableName)
        {
            List<ComputingPartnerTranslationListData> AllDataLines = new List<ComputingPartnerTranslationListData>();

            using (TextFieldParser csvParser = new TextFieldParser(streamReader))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                string[] columns = null;
                while ((columns = csvParser.ReadFields()) != null)
                {

                    string Code = this.GetValue(columns, 0);
                    string TranslatedCode = null;
                    if (TableName == "Card")
                        TranslatedCode = this.GetValue(columns, 2);
                    else if (TableName == "Port")
                        TranslatedCode = this.GetValue(columns, 1);


                    if (!string.IsNullOrEmpty(TranslatedCode))
                    {
                        if (TranslatedCode.Length > 50)
                        {
                            TranslatedCode = TranslatedCode.Substring(0, 50);
                        }
                    }

                    ComputingPartnerTranslationListData myItem = new ComputingPartnerTranslationListData();
                    myItem.OurCode = Code;
                    myItem.PartnerCode = TranslatedCode;
                    ComputingPartnerTranslationListData xItem = AllDataLines.Where(d => d.OurCode == Code).FirstOrDefault();
                    if (xItem == null)
                    {
                        AllDataLines.Add(myItem);
                    }
                }

                Thread thread = new Thread(() => this.RunAddingComputingPartners(AllDataLines, TableName));
                thread.IsBackground = true;
                thread.Start();
            }
        }
        private void RunAddingComputingPartners(List<ComputingPartnerTranslationListData> allDataLines, string TableName)
        {

            allDataLines = allDataLines.Where(d => !string.IsNullOrEmpty(d.OurCode) && !string.IsNullOrEmpty(d.PartnerCode)).ToList();
            if (allDataLines.Count > 0)
            {
                int tenant = int.Parse(tenant_TXT.Text);
                ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                ComputingPartner ComputingPartner = myCommonContext.ComputingPartners.Where(d => d.Tenant == tenant && d.Code == "API").FirstOrDefault();
                if (ComputingPartner != null)
                {
                    ComputingPartnerTable Table = myCommonContext.ComputingPartnerTables.Where(d => d.Tenant == tenant && d.ComputingPartnerId == ComputingPartner.Id && d.ObjectTable.Name == TableName).FirstOrDefault();
                    if (Table != null)
                    {
                        if (TableName == "Card")
                            SetControlPropertyValue(updateFillComputingLBL, "Text", "Adding...");
                        else if (TableName == "Port")
                            SetControlPropertyValue(updateFillComputingPortsLBL, "Text", "Adding...");

                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();

                        var myCount = 0;
                        foreach (ComputingPartnerTranslationListData item in allDataLines)
                        {
                            object Object = null;
                            if (TableName == "Card")
                                Object = myCommonContext.Cards.Where(d => d.Code == item.OurCode && d.Tenant == tenant).FirstOrDefault();
                            else if (TableName == "Port")
                                Object = myCommonContext.Ports.Where(d => d.CombinedCode == item.OurCode && d.Tenant == tenant).FirstOrDefault();

                            if (Object != null)
                            {
                                ComputingPartnerTranslation entityCard = myCommonContext.ComputingPartnerTranslations.Where(d => d.OurCode == item.OurCode && d.Tenant == tenant && d.ComputingPartnerId == ComputingPartner.Id && d.ObjectTableId == Table.ObjectTableId).FirstOrDefault();

                                if (entityCard == null)
                                {
                                    string entityId = IdCounter.GetNumber("ComputingPartnerTranslation", tenant).ToString();

                                    entityCard = new ComputingPartnerTranslation()
                                    {
                                        Id = entityId,
                                        Tenant = tenant,
                                        CreateDate = DateTime.Now,
                                        UpdateDate = DateTime.Now,
                                        CreatedByUserId = "1-1",
                                        UpdatedByUserId = "1-1",
                                        OurCode = item.OurCode,
                                        PartnerCode = item.PartnerCode,
                                        ComputingPartnerId = ComputingPartner.Id,
                                        ObjectTableId = Table.ObjectTableId,
                                    };
                                    entityCard.SearchFields = this.BuildComputingPartnerTranslationSearchFields(entityCard);
                                    myCommonContext.ComputingPartnerTranslations.Add(entityCard);


                                }
                                else
                                {
                                    if (entityCard.PartnerCode != item.PartnerCode)
                                    {
                                        entityCard.PartnerCode = item.PartnerCode;
                                        entityCard.UpdateDate = DateTime.Now;
                                        entityCard.SearchFields = this.BuildComputingPartnerTranslationSearchFields(entityCard);
                                    }
                                }
                            }


                            if (myCount == 1000)
                            {
                                myCommonContext.SaveChanges();
                                myCount = 0;
                            }

                            myCount++;
                        }

                        myCommonContext.SaveChanges();
                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;
                        if (TableName == "Card")
                            SetControlPropertyValue(updateFillComputingLBL, "Text", "Done in " + ts.ToString());
                        else if (TableName == "Port")
                            SetControlPropertyValue(updateFillComputingPortsLBL, "Text", "Done in " + ts.ToString());



                    }


                }

            }
        }
        private string BuildComputingPartnerTranslationSearchFields(ComputingPartnerTranslation entityCard)
        {
            string mySearchFields = "";
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.OurCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.PartnerCode);
            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }
            return mySearchFields;
        }

        private void EncryptionDocumentButton_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "encryptiondocument", EncryptionDocumentLabel));
            thread.IsBackground = true;
            thread.Start();
        }

        private void expPackagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.exportPackageFeaturesToolStripMenuItem_Click(sender, e);
        }

        private void expRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.exportRoleFeaturesToolStripMenuItem_Click(sender, e);
        }

        private void impPackagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.impPackToolStripMenuItem_Click(sender, e);
            //this.impRoleToolStripMenuItem_Click()
        }

        private void impRolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.impRoleToolStripMenuItem_Click(sender, e);
        }

        private void button35_Click(object sender, EventArgs e)
        {
            ComputingPartnerFillings("Port");
        }

        private void btnUpdateShipment_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "Shipment", lblUShipment));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnUpdateQuote_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "Quote", lblUQuote));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnUpdateInvoice_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "Invoice", lblUInvoice));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnUpdateCommon_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "Common", lblUCommon));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnUpdateInfrastructure_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "Infrastructure", lblUInfra));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnUpdateGlobal_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "Global", lblUGlobal));
            thread.IsBackground = true;
            thread.Start();
        }

        private void updateReportLocalNamesBtn_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "updateReportLocalNames", updateReportLocalNamesLabel));
            thread.IsBackground = true;
            thread.Start();
        }

        private void btnExecuteSqlScriptFiles_Click(object sender, EventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
                string solutionDirectory = solutionDir.FullName;
                string mainDirPath = solutionDirectory + @"\General\Main Branch\2018";
                dialog.RootFolder = Environment.SpecialFolder.MyComputer;
                dialog.SelectedPath = mainDirPath;
                dialog.ShowNewFolderButton = false;

                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    string rootDirecPath = dialog.SelectedPath;

                    SqlFilesExecuter.ExecuteAllSqlFiles2(rootDirecPath);
                }
            }
        }

        private void RecalculateCashbookBtn_Click(object sender, EventArgs e)
        {
            RecalculateCashbookLbl.Text = "Working...";

            Thread thread = new Thread(() =>
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    MessageBox.Show("Enter the tenant !!!!!");
                    SetControlPropertyValue(RecalculateCashbookLbl, "Text", "Enter tenant!");
                    return;
                }

                CashbookService cashbookService = new CashbookService();
                string res = cashbookService.RecalculateCashbookTotals(Convert.ToInt16(textBox1.Text));

                SetControlPropertyValue(RecalculateCashbookLbl, "Text", "Done, updated: " + res);

            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Recalculate totals of cheque cashbooks according to its lines. \n (As shown inside cashbooks)");
        }

        private void button36_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "infrastructurem", UpdateINFlble));
            thread.IsBackground = true;
            thread.Start();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button37_Click(object sender, EventArgs e)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(0);
            JournalQueryService journalQuery = new JournalQueryService(1);
            List<Journal> journals = accountingContext.Journals.ToList();
            foreach (Journal line in journals)
            {
                JournalMoreDataPM moreDataPM = new JournalMoreDataPM()
                {
                    JournalId = line.Id,
                    Line = 1,
                    Tenant = 1,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    GeneralData = "empty",
                };

                JournalMoreDataUpdateService serivce = new JournalMoreDataUpdateService(accountingContext, new Dictionary<string, IContext>(), 1);
                serivce.Update(moreDataPM, true);
            }

        }

        private void button38_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }




        private void btnUpdateTenantZeroNew_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "UpdateTenantZeroNew", lblTenantNew));
            thread.IsBackground = true;
            thread.Start();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            FutureOpenChequesBatch batch = new FutureOpenChequesBatch();
            batch.SetTotalFutureOpenChequesInLocalCurrency();
        }

        private void btnDownloadMrt_Click(object sender, EventArgs e)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
            //if (contact != null)
            //{

            var logitudeUser = (from a in commonDataContext.Users
                                where a.Id == "1-149534"
                                select a).FirstOrDefault();

            //    if (logitudeUser != null)
            //    {
            //        if (logitudeUser.Tenant == 0)
            //        {
            //            distributor = logitudeUser.IsDistributor;
            //            customerCare = !logitudeUser.IsDistributor;

            //        }
            //    }

            //}
            //string connectionString = "LogitudeMain_PreR1,logitudemanager,!LO009008,logitudetest.database.windows.net";// "LogitudeMain,logitudemanager,!LO852456,ebup282itq.database.windows.net";
            //DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(connectionString);
            //CommonDataContext Logitudecontext = new CommonDataContext(Logitudeconnection);
            //DocumentTypeTemplate template = (from a in Logitudecontext.DocumentTypeTemplates
            //                                 where a.Id == "1-149534"
            //                                 select a).FirstOrDefault();

            //File.WriteAllBytes("template.mrt", template.TemplateBody); // Requires System.IO

        }

        private void HarmonizeCodesButton_Click(object sender, EventArgs e)
        {
            HarmonizeCodesLabel.Text = null;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.Filter = "csv Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.Filter = "txt|*.txt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;

            DialogResult res = openFileDialog1.ShowDialog();

            if (res == System.Windows.Forms.DialogResult.OK)
            {
                List<string> lines = new List<String>(File.ReadAllLines(openFileDialog1.FileName));

                if (lines.Count == 0)
                {
                    MessageBox.Show("No Lines in this file");
                }

                else
                {
                    Thread thread = new Thread(() => UpdateHarmonizeCodes(lines, HarmonizeCodesLabel));
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
        }

        private void UpdateHarmonizeCodes(List<string> lines, Label lable)
        {
            SetControlPropertyValue(lable, "Text", "Updating...");
            SetControlPropertyValue(lable, "ForeColor", Color.Black);

            HarmonizeCodeRepository iRepository = new HarmonizeCodeRepository(0);
            List<HarmonizeCode> db_Items = iRepository.GetAll().ToList();

            int index = 0;
            int errorLinesCount = 0;
            foreach (string item in lines)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string line = item.Replace("\"", "");

                    string[] lineArray = line.Split('\t');

                    if (lineArray.Count() == 6)
                    {
                        HarmonizeCode iEntity = db_Items.Where(d => d.Code == lineArray[0]).FirstOrDefault();
                        if (iEntity == null)
                        {
                            iEntity = new HarmonizeCode()
                            {
                                Code = lineArray[0],
                                Description = lineArray[1],
                                ChapterCode = lineArray[2],
                                ChapterDescription = lineArray[3],
                                SubChapterCode = lineArray[4],
                                SubChapterDescription = lineArray[5],
                            };

                            iEntity.SearchFields = iEntity.Code + "," + iEntity.ChapterCode + "," + iEntity.SubChapterCode + "," + iEntity.Description;
                            iRepository.Add(iEntity);
                        }

                        else
                        {
                            iEntity.Description = lineArray[1];
                            iEntity.ChapterCode = lineArray[2];
                            iEntity.ChapterDescription = lineArray[3];
                            iEntity.SubChapterCode = lineArray[4];
                            iEntity.SubChapterDescription = lineArray[5];
                            iEntity.SearchFields = iEntity.Code + "," + iEntity.ChapterCode + "," + iEntity.SubChapterCode + "," + iEntity.Description;
                            iRepository.Update(iEntity);
                        }

                        index++;

                        if (index >= 500)
                        {
                            index = 0;
                            iRepository.SubmitChanges();
                        }
                    }

                    else
                    {
                        errorLinesCount++;
                    }
                }
            }

            iRepository.SubmitChanges();

            SetControlPropertyValue(lable, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue(lable, "ForeColor", Color.Green);

            if (errorLinesCount == 0)
            {
                SetControlPropertyValue(lable, "Text", "Done");
            }

            else
            {
                SetControlPropertyValue(lable, "Text", "Done with " + errorLinesCount + " lines error");
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            AddStates addStatesForm = new AddStates();
            addStatesForm.Show();

        }

        private void btnCompareData_Click(object sender, EventArgs e)
        {
            string connectionString1 = "LogitudeMain_Copy,logitudemanager,!LO852456,ebup282itq.database.windows.net";//"LogitudeMain_PreR1,logitudemanager,!LO009008,logitudetest.database.windows.net";// "LogitudeMain,logitudemanager,!LO852456,ebup282itq.database.windows.net";
            DbConnection Logitudeconnection1 = DatabaseInitializer.GetConnection(connectionString1);
            CommonDataContext Logitudecontext1 = new CommonDataContext(Logitudeconnection1);

            //List<Feature> preFeatures = (from a in Logitudecontext1.Features
            //							 where a.IsOld == false
            //							 select a).ToList();


            string productionConnectionString = "LogitudeMain,logitudemanager,!LO852456,ebup282itq.database.windows.net";
            DbConnection productionConnection = DatabaseInitializer.GetConnection(productionConnectionString);
            CommonDataContext productionLogitudeContext = new CommonDataContext(productionConnection);

            WebFreightContext webFreightContext = new WebFreightContext(productionConnection);
            List<ObjectTable> allTables = (from a in webFreightContext.ObjectTables
                                           select a).ToList();


            List<MyFeature> onlineFeatureCodes = (from a in productionLogitudeContext.Features.Include("NameTextCode")
                                                  select
                                               new MyFeature()
                                               {
                                                   Id = a.Id,
                                                   Tenant = a.Tenant,
                                                   Code = a.Code,
                                                   ObjectTableId = a.ObjectTableId,
                                                   Name = a.NameTextCode.DefaultText,
                                                   FeatureTypeCode = a.FeatureTypeCode,
                                                   Packagable = a.Packagable,
                                                   IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                                                   IsCoreFeature = a.IsCoreFeature,

                                               }).ToList();

            //featureDetails.Code + featureDetails.ObjectTableId
            //List<Feature> onlineFeatures = (from a in Logitudecontext2.Features
            //								select a).ToList();

            //List<Feature> newlyAddedFeatures = preFeatures.Where(f => !onlineFeatureCodes.Any(pf => pf == (f.Code + f.ObjectTableId))).ToList();

            List<MyFeature> backupFeatures = (from a in Logitudecontext1.Features.Include("NameTextCode")
                                              select
                                              new MyFeature()
                                              {
                                                  Id = a.Id,
                                                  Tenant = a.Tenant,
                                                  Code = a.Code,
                                                  ObjectTableId = a.ObjectTableId,
                                                  Name = a.NameTextCode.DefaultText,
                                                  FeatureTypeCode = a.FeatureTypeCode,
                                                  Packagable = a.Packagable,
                                                  IsBusinessUnitEnabled = a.IsBusinessUnitEnabled,
                                                  IsCoreFeature = a.IsCoreFeature,

                                              }).ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var f in backupFeatures)
            {
                MyFeature prodFeature = onlineFeatureCodes.FirstOrDefault(o => o.Code == f.Code && o.ObjectTableId == f.ObjectTableId);
                if (prodFeature != null)
                {
                    sb.Append(f.Code);
                    sb.Append(",");
                    sb.Append(f.Name);
                    sb.Append(",");
                    sb.Append(f.FeatureTypeCode);
                    sb.Append(",");
                    sb.Append(allTables.First(t => t.Id == f.ObjectTableId).Name);
                    sb.Append(",");

                    sb.Append(f.Packagable);
                    sb.Append(",");
                    sb.Append(prodFeature.Packagable);
                    sb.Append(",");

                    sb.Append(f.IsBusinessUnitEnabled);
                    sb.Append(",");
                    sb.Append(prodFeature.IsBusinessUnitEnabled);
                    sb.Append(",");

                    sb.Append(f.IsCoreFeature);
                    sb.Append(",");
                    sb.Append(prodFeature.IsCoreFeature);
                    sb.Append(",");

                    sb.AppendLine();
                }

            }

            //foreach (var f in newlyAddedFeatures)
            //{


            //	sb.Append(f.Code);
            //	sb.Append(",");
            //	sb.Append(f.NameTextCode);
            //	sb.Append(",");
            //	sb.Append(f.FeatureTypeCode);
            //	sb.Append(",");
            //	sb.Append(allTables.First(t => t.Id == f.ObjectTableId).Name);
            //	sb.Append(",");
            //	sb.AppendLine();

            //}

            Encoding currentEncoding = Encoding.GetEncoding(Encoding.UTF8.CodePage);
            byte[] sbByte = currentEncoding.GetBytes(sb.ToString());
            //string encodedString = currentEncoding.GetString(sbByte);



            File.WriteAllBytes("ComparingFeatures.csv", sbByte); // Requires System.IO
        }

        private void button41_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => UpdateModule(0, "TariffModule", lblUTariffModule));
            thread.IsBackground = true;
            thread.Start();
        }

        private void rtlBtn_Click(object sender, EventArgs e)
        {
            ChangeTenantLayoutDirection("rtl");
        }

        private void ChangeTenantLayoutDirection(string dir)
        {
            CommonDataContext Context = CommonDataContext.GetContextByDBId("0");
            string connectionString = Context.GetConnection().ConnectionString;
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);

            int tenant = Convert.ToInt32(tenantTxtBox.Text);

            SqlCommand cmd = new SqlCommand
            {
                CommandText = String.Format("UPDATE Tenants set LayoutDirection = '{1}' where Id = {0}", tenant, dir),
                Connection = sqlConnection1
            };

            try
            {
                sqlConnection1.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                sqlConnection1.Close();

                MessageBox.Show(string.Format("Tenant {0}: {1}",tenant, dir));
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed! Tenant {0}: {1} \n {2}", tenant, dir, ex.Message));
                throw;
            }
        }

        private void ltrBtn_Click(object sender, EventArgs e)
        {
            ChangeTenantLayoutDirection("ltr");
            
        }

        private void button42_Click_FixingDouplicated(object sender, EventArgs e)
        {

            //string text = "APP4030";

            //string input = "1030-015849-1";
            //input = input.Replace("-", "");
            //var array = Regex.Matches(input, @"\D+|\d+")
            //                 .Cast<Match>()
            //                 .Select(m => m.Value)
            //                 .ToArray();

            //TaxReportQueryService taxReportQueryService = new TaxReportQueryService(1064);
            //TaxReportPM taxReportPM = taxReportQueryService.GetSingle("1-1913", false, false);
            //TaxReportService.CreateTaxReportLines(taxReportPM, 1064);


        }

        private void button42_Click(object sender, EventArgs e)
        {
            //DocumentRepository DocR = new DocumentRepository(0); 
            var result = new List<DocumentsFiling>();
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                DbConnection connection = DatabaseInitializer.GetConnection("logbox-main,logboxadmin,London2015!London2015!,logboxdbs.database.windows.net");// "Main,sa,Saas256,amitaldata.cloudapp.net");
                CommonDataContext context = new CommonDataContext(connection);
                result = (from a in context.Documents
                          join b in context.DocumentsFilings on a.Id equals b.DocumentId
                          where b.ForwarderDocumentId != null && b.IsDeleted == false && a.HasFile == false
                          select b).ToList();
                scope.Complete();
            }

            foreach (var item1 in result)
            {
                DocumentRepository DocR = new DocumentRepository(0);
                var item = (from a in DocR.context.DocumentsFilings
                            where a.Id == item1.ForwarderDocumentId
                            select a).FirstOrDefault();
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ImportersShipmentDocumentsQueue", 0);
                queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", item.EntityId }, { "DocumentFilingId", item.Id }, { "Tenant", item.Tenant.ToString() }, { "CorrelationId", Guid.NewGuid().ToString() } });
            }
        }

        private void button43_Click(object sender, EventArgs e)
        {
            //GlobalContact contact = null;
            IGlobalContext globalObjectContext = GlobalContext.GetContext();
            ContactPasswordRepository GCRepo = new ContactPasswordRepository(globalObjectContext);
            var Tenants = new List<int>() { 493, 839, 1177, 558, 570, 545, 286, 996, 1264, 1573, 1402, 1427, 1245, 1604, 796, 1275, 1326, 1333, 42, 1256, 877, 1423, 1293, 2043 };
            foreach (var tenant in Tenants)
            {
                var contacts = globalObjectContext.GlobalContacts.Where(c => c.GlobalTenantId == tenant && (c.IsUser == true) && c.InActive == false).ToList();
                foreach (var contact in contacts)
                {
                    ContactPassword contactPassword = globalObjectContext.ContactPasswords.Where(a => a.Email == contact.Email).FirstOrDefault();//AuthenticationUtil.VerifyContactPassword(contact.Email, "123", globalObjectContext);
                    if (contactPassword != null)
                    {
                        string HashedPass = PasswordGenerator.GetBCryptHashedPassword(contact.Email, "123");
                        contactPassword.Password = HashedPass;
                        contactPassword.IsBCrypt = true;
                        GCRepo.SubmitChanges();
                    }

                }
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            IInvoiceContext context = InvoiceContext.GetContext(1);
            APInvoiceQuery service = new APInvoiceQuery(1);
                APInvoicePM invoice = service.GetSinglePM("1-18", 1);
            byte[] serialized = LogitudeXmlSerializer.SerializeObject(invoice);
            using (MemoryStream ms = new MemoryStream(serialized))
            {
                StreamWriter writer = new StreamWriter(ms);

                writer.WriteLine("asdasdasasdfasdasd");
                writer.Flush();

                //You have to rewind the MemoryStream before copying
                ms.Seek(0, SeekOrigin.Begin);

                using (FileStream fs = new FileStream("m_output.txt", FileMode.OpenOrCreate))
                {
                    ms.CopyTo(fs);
                    fs.Flush();
                }
            }

        }
    }



    public class MyFeature
    {

        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string ObjectTableId { get; set; }
        public string Name { get; set; }
        public string FeatureTypeCode { get; set; }
        public bool Packagable { get; set; }
        public bool IsBusinessUnitEnabled { get; set; }
        public bool IsOld { get; set; }
        public bool IsCoreFeature { get; set; }

    }


    public class HtmlStringParsingParams
    {
        public string Company { get; set; }
        public string Country { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NumberOfBranches { get; set; }
        public string NumberOfUsers { get; set; }
    }

    public class DataItem
    {
        public string Id { get; set; }
        public string CountryCode { get; set; }
        public string PortCode { get; set; }
        public string PortEnglishName { get; set; }
        public string PortLocalName { get; set; }
        public bool IsAir { get; set; }
        public bool IsOcean { get; set; }
        public bool IsInland { get; set; }
    }

    public class WarehouseItem
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class ComputingPartnerTranslationListData
    {
        public string OurCode { get; set; }
        public string PartnerCode { get; set; }
    }
}

