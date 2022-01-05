using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.IO;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Update
{
    public partial class CopyData : Form
    {
        string LogitudeconnectionString = "";
        string AmitalconnectionString = "";
        BackgroundWorker worker;
        public CopyData()
        {
            InitializeComponent();
            try
            {
                //  < add name = "SourceStr" connectionString = "LogitudeMain,logitudemanager,!LO852456,ebup282itq.database.windows.net" />
                //<!--<add name="DestinationStr" connectionString="Logbox_Main,sa,Saas256,amitaldata.cloudapp.net" />-->
                //<!--<add name="DestinationGlobalStr" connectionString="Global,sa,Saas256,amitaldata.cloudapp.net" />-->

                LogitudeconnectionString = ConfigurationManager.ConnectionStrings["SourceStr"] != null ? ConfigurationManager.ConnectionStrings["SourceStr"].ConnectionString : null;
                AmitalconnectionString = ConfigurationManager.ConnectionStrings["DestinationStr"] != null ? ConfigurationManager.ConnectionStrings["DestinationStr"].ConnectionString : null;

                //ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString = ConfigurationManager.ConnectionStrings["DestinationGlobalStr"].ConnectionString;
            }
            catch (Exception eee)
            {

                MessageBox.Show("Exception eee =" + eee.ToString());
                throw;
            }
        }

        private void txtStartCopy_Click(object sender, EventArgs e)
        {
            txtStartCopy.Enabled = false;
            label1.Visible = false;
            StartCopyForTenantZero();
        }

        private void StartCopyForTenantZero()
        {
            DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(LogitudeconnectionString);
            CommonDataContext Logitudecontext = new CommonDataContext(Logitudeconnection);
            WebFreightContext LogitudeWebFreightContext = new WebFreightContext(Logitudeconnection);

            DbConnection Amitalconnection = DatabaseInitializer.GetConnection(AmitalconnectionString);
            CommonDataContext Amitalcontext = new CommonDataContext(Amitalconnection);
            WebFreightContext AmitalWebFreightContext = new WebFreightContext(Amitalconnection);


            this.CopyPorts(Logitudecontext, Amitalcontext);
            this.CopyDocumentTypes(Logitudecontext, Amitalcontext, LogitudeWebFreightContext, AmitalWebFreightContext);
            this.CopyPackageTypes(Logitudecontext, Amitalcontext);




            label1.Visible = true;
            txtStartCopy.Enabled = true;

        }


        private void CopyPorts(CommonDataContext Logitudecontext, CommonDataContext Amitalcontext)
        {
            #region Ports
            // GLobal Zones

            GlobalZoneRepository LogitudeglobalZoneRepository = new GlobalZoneRepository(Logitudecontext);
            GlobalZoneRepository AmitalglobalZoneRepository = new GlobalZoneRepository(Amitalcontext);

            var LogitudeGlobalzones = LogitudeglobalZoneRepository.GetGlobalZones(0);
            var AmitalGlobalzones = AmitalglobalZoneRepository.GetGlobalZones(0);
            foreach (var item in LogitudeGlobalzones)
            {
                var AmitalItem = AmitalGlobalzones.Where(a => a.Code == item.Code).FirstOrDefault();
                if (AmitalItem == null)
                {
                    GlobalZone NewGlobalZone = new GlobalZone()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("Globalzone", 0),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant
                    };
                    AmitalglobalZoneRepository.Add(NewGlobalZone);
                }
            }
            AmitalglobalZoneRepository.SubmitChanges();

            // Countries

            CountryRepository LogitudeCountriesRepository = new CountryRepository(Logitudecontext);
            CountryRepository AmitalCountriesRepository = new CountryRepository(Amitalcontext);

            var LogitudeCountries = LogitudeCountriesRepository.GetCountries(0);
            var AmitalCountries = AmitalCountriesRepository.GetCountries(0);

            foreach (var item in LogitudeCountries)
            {
                var AmitalItem = AmitalCountries.Where(a => a.Code == item.Code).FirstOrDefault();
                var LogitudeGZone = LogitudeglobalZoneRepository.GetSingleGlobalZone(item.GlobalZoneId, item.Tenant);
                var AmitalGZone = AmitalglobalZoneRepository.GetSingleGlobalZoneByCode(LogitudeGZone.Code, item.Tenant);
                if (AmitalItem == null)
                {
                    Country NewCountry = new Country()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("Country", 0),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant,
                        AddedManually = item.AddedManually,
                        EC = item.EC,
                        HasCitiesList = item.HasCitiesList,
                        HasStates = item.HasStates,
                        IsStateRequired = item.IsStateRequired,
                        GlobalZoneId = AmitalGZone.Id

                    };
                    AmitalCountriesRepository.Add(NewCountry);

                }
            }
            AmitalCountriesRepository.SubmitChanges();

            // States

            StateRepository LogitudeStatesRepository = new StateRepository(Logitudecontext);
            StateRepository AmitalStatesRepository = new StateRepository(Amitalcontext);

            var LogitudeStates = LogitudeStatesRepository.GetStates(0);
            var AmitalStates = AmitalStatesRepository.GetStates(0);

            foreach (var item in LogitudeStates)
            {
                var AmitalItem = AmitalStates.Where(a => a.Code == item.Code).FirstOrDefault();
                //var LogitudeGZone = LogitudeglobalZoneRepository.GetSingleGlobalZone(item.GlobalZoneId,item.Tenant);
                var AmitalCountry = AmitalCountriesRepository.GetSingleCountryByCode(item.Country.Code, item.Tenant);
                if (AmitalItem == null)
                {
                    State NewState = new State()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("Country", 0),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant,
                        AddedManually = item.AddedManually,
                        CountryId = AmitalCountry.Id


                    };
                    AmitalStatesRepository.Add(NewState);

                }
            }
            AmitalStatesRepository.SubmitChanges();

            // Ports

            PortRepository LogitudePortsRepository = new PortRepository(Logitudecontext);
            PortRepository AmitalPortsRepository = new PortRepository(Amitalcontext);

            var LogitudePorts = LogitudePortsRepository.GetPorts(0);
            var AmitalPorts = AmitalPortsRepository.GetPorts(0);
            int counter = 0;
            Port NewPort;
            foreach (var item in LogitudePorts)
            {
                var cont = item.Country.Code;
                var AmitalItem = AmitalPorts.Where(a => (a.Code == item.Code && a.Country.Code == item.Country.Code)).FirstOrDefault();
                //var LogitudeGZone = LogitudeglobalZoneRepository.GetSingleGlobalZone(item.GlobalZoneId, item.Tenant);
                var AmitalCountry = AmitalCountries.FirstOrDefault(s => s.Code == item.Country.Code && s.Tenant == item.Tenant);//AmitalCountriesRepository.GetSingleCountryByCode(item.Country.Code, item.Tenant);
                State LogitudeState = null;
                LogitudeState = LogitudeStates.FirstOrDefault(s => s.Id == item.StateId && s.Tenant == item.Tenant);//LogitudeStatesRepository.GetSingleState(item.StateId, item.Tenant);
                State AmitalState = null;
                if (LogitudeState != null)
                {
                    AmitalState = AmitalStates.FirstOrDefault(s => s.Code == LogitudeState.Code && s.Tenant == item.Tenant);//AmitalStatesRepository.GetSingleStateByCode(LogitudeState.Code, item.Tenant);
                }

                if (AmitalItem == null)
                {
                    NewPort = new Port()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("Country", 0),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant,
                        AddedManually = item.AddedManually,
                        CountryId = AmitalCountry.Id,
                        Field1 = item.Field1,
                        Field2 = item.Field2,
                        Field3 = item.Field3,
                        Field4 = item.Field4,
                        Field5 = item.Field5,
                        Field6 = item.Field6,
                        Field7 = item.Field7,
                        Field8 = item.Field8,
                        Field9 = item.Field9,
                        Field10 = item.Field10,
                        IsAir = item.IsAir,
                        IsInland = item.IsInland,
                        IsOcean = item.IsOcean,
                        Latitude = item.Latitude,
                        Longtitude = item.Longtitude,
                        StateId = AmitalState != null ? AmitalState.Id : null,


                    };
                    UpdatePortSearchFieldService.Update(NewPort);

                    AmitalPortsRepository.Add(NewPort);
                }

                counter++;
                if (counter == 1000)
                {
                    counter = 0;
                    AmitalPortsRepository.SubmitChanges();
                }
            }
            if (counter != 0)
            {
                AmitalPortsRepository.SubmitChanges();
            }

            #endregion
        }
        private void CopyDocumentTypes(CommonDataContext sourceContext, CommonDataContext destContext, WebFreightContext sourceWebFreightContext, WebFreightContext destWebFreightContext)
        {
            #region DocumentTypes + Template

            DocumentTypeRepository SourceDocumentTypeRepository = new DocumentTypeRepository(sourceContext);
            DocumentTypeRepository DestinationDocumentTypeRepository = new DocumentTypeRepository(destContext);
            DocumentTypeTemplateRepository SourceDocumentTypeTemplateRepository = new DocumentTypeTemplateRepository(sourceContext);
            DocumentTypeTemplateRepository DestinationDocumentTypeTemplateRepository = new DocumentTypeTemplateRepository(destContext);
            ObjectTableRepository SourceObjectTabelRepository = new ObjectTableRepository(sourceWebFreightContext);
            ObjectTableRepository DestinationObjectTabelRepository = new ObjectTableRepository(destWebFreightContext);
            UserRepository DestinationUserRepository = new UserRepository(destContext);
            var SourceDocumentTypes = SourceDocumentTypeRepository.GetDocumentTypes(0).Where(d=>d.InActive == false);
            var DestinationDocumentTypes = DestinationDocumentTypeRepository.GetDocumentTypes(0);
            var SourceObjectTables = SourceObjectTabelRepository.GetObjectsByTenant(0);
            var DestinationObjectTables = DestinationObjectTabelRepository.GetObjectsByTenant(0);

            int c1 = SourceDocumentTypes.Count();
            int c2 = DestinationDocumentTypes.Count();

            var DestinationSystemUser = DestinationUserRepository.GetSingleUserByEmail("system@tenant0.com", 0, false);

           

            //system@tenant0.com,System,99999999
            foreach (var item in SourceDocumentTypes)
            {
                var DestinationItem = DestinationDocumentTypes.Where(a => a.Code == item.Code).FirstOrDefault();
                if (DestinationItem == null)
                {
                    string DocTypeId = IdCounter.GetNumber("DocumentType", 0);
                    var SourceObjectTable = SourceObjectTables.FirstOrDefault(t => t.Id == item.ObjectTableId); //ObjectTabelRepository.GetSingleObjectTableById(item.ObjectTableId, item.Tenant);
                    var DestinationObjectTable = DestinationObjectTables.FirstOrDefault(t => t.Name == SourceObjectTable.Name);//AmitalObjectTabelRepository.GetObjectTableByName(LogitudeObjectTable.Name, item.Tenant, false);
                    var SourceDocumentTypeHtmlTemplate = SourceDocumentTypeTemplateRepository.GetSingleDocumentTypeTemplateByTenant(item.DocumentTypeDefaultHTMLTemplateId, item.Tenant);
                    var SourceDocumentTypeReportTemplate = SourceDocumentTypeTemplateRepository.GetSingleDocumentTypeTemplateByTenant(item.DocumentTypeDefaultHTMLTemplateId, item.Tenant);
                    DocumentTypeTemplate AmitalDocumentTypeHtmlTemplate = null;
                    DocumentTypeTemplate AmitalDocumentTypeReportTemplate = null;
                    if (SourceDocumentTypeHtmlTemplate != null)
                    {
                        AmitalDocumentTypeHtmlTemplate = new DocumentTypeTemplate()
                        {
                            CountryCode = SourceDocumentTypeHtmlTemplate.CountryCode,
                            Description = SourceDocumentTypeHtmlTemplate.Description,
                            DocumentTypeId = DocTypeId,
                            EditorTool = SourceDocumentTypeHtmlTemplate.EditorTool,
                            HorizontalShift = SourceDocumentTypeHtmlTemplate.HorizontalShift,
                            Id = IdCounter.GetNumber("DocumentTypeTemplate", 0),
                            InActive = SourceDocumentTypeHtmlTemplate.InActive,
                            InternalRemarks = SourceDocumentTypeHtmlTemplate.InternalRemarks,
                            IsCopiedAtSignup = SourceDocumentTypeHtmlTemplate.IsCopiedAtSignup,
                            IsEnabledForCustomers = SourceDocumentTypeHtmlTemplate.IsEnabledForCustomers,
                            Language = SourceDocumentTypeHtmlTemplate.Language,
                            LastUpdateDate = SourceDocumentTypeHtmlTemplate.LastUpdateDate,
                            LastUpdatedByUserId = DestinationSystemUser.Id,
                            OriginalTemplateId = SourceDocumentTypeHtmlTemplate.OriginalTemplateId,
                            Subject = SourceDocumentTypeHtmlTemplate.Subject,
                            TemplateBody = SourceDocumentTypeHtmlTemplate.TemplateBody,
                            TemplateType = SourceDocumentTypeHtmlTemplate.TemplateType,
                            Tenant = SourceDocumentTypeHtmlTemplate.Tenant,
                            VerticalShift = SourceDocumentTypeHtmlTemplate.VerticalShift,
                            BCC = SourceDocumentTypeHtmlTemplate.BCC,
                            CC = SourceDocumentTypeHtmlTemplate.CC,
                            From = SourceDocumentTypeHtmlTemplate.From,
                            TemplateTechnologyCode = SourceDocumentTypeHtmlTemplate.TemplateTechnologyCode,
                            TemplateBodyHtml = SourceDocumentTypeHtmlTemplate.TemplateBodyHtml,
                            TemplateFooterHeight = SourceDocumentTypeHtmlTemplate.TemplateFooterHeight,
                            TemplateFooterHtml = SourceDocumentTypeHtmlTemplate.TemplateFooterHtml,
                            TemplateBodyjson = SourceDocumentTypeHtmlTemplate.TemplateBodyjson,
                            TemplateHeaderHeight = SourceDocumentTypeHtmlTemplate.TemplateHeaderHeight,
                            TemplateHeaderHtml = SourceDocumentTypeHtmlTemplate.TemplateHeaderHtml,
                            IsSystem = SourceDocumentTypeHtmlTemplate.IsSystem,



                        };
                        DestinationDocumentTypeTemplateRepository.Add(AmitalDocumentTypeHtmlTemplate);
                    }

                    if (SourceDocumentTypeReportTemplate != null)
                    {
                        AmitalDocumentTypeReportTemplate = new DocumentTypeTemplate()
                        {
                            CountryCode = SourceDocumentTypeReportTemplate.CountryCode,
                            Description = SourceDocumentTypeReportTemplate.Description,
                            DocumentTypeId = DocTypeId,
                            EditorTool = SourceDocumentTypeReportTemplate.EditorTool,
                            HorizontalShift = SourceDocumentTypeReportTemplate.HorizontalShift,
                            Id = IdCounter.GetNumber("DocumentTypeTemplate", 0),
                            InActive = SourceDocumentTypeReportTemplate.InActive,
                            InternalRemarks = SourceDocumentTypeReportTemplate.InternalRemarks,
                            IsCopiedAtSignup = SourceDocumentTypeReportTemplate.IsCopiedAtSignup,
                            IsEnabledForCustomers = SourceDocumentTypeReportTemplate.IsEnabledForCustomers,
                            Language = SourceDocumentTypeReportTemplate.Language,
                            LastUpdateDate = SourceDocumentTypeReportTemplate.LastUpdateDate,
                            LastUpdatedByUserId = DestinationSystemUser.Id,
                            //OriginalTemplateId = LogitudeDocumentTypeReportTemplate.OriginalTemplateId,
                            Subject = SourceDocumentTypeReportTemplate.Subject,
                            TemplateBody = SourceDocumentTypeReportTemplate.TemplateBody,
                            TemplateType = SourceDocumentTypeReportTemplate.TemplateType,
                            Tenant = SourceDocumentTypeReportTemplate.Tenant,
                            VerticalShift = SourceDocumentTypeReportTemplate.VerticalShift,
                            BCC = SourceDocumentTypeReportTemplate.BCC,
                            CC = SourceDocumentTypeReportTemplate.CC,
                            From = SourceDocumentTypeReportTemplate.From,
                            TemplateTechnologyCode = SourceDocumentTypeReportTemplate.TemplateTechnologyCode,
                            TemplateBodyHtml = SourceDocumentTypeReportTemplate.TemplateBodyHtml,
                            TemplateFooterHeight = SourceDocumentTypeReportTemplate.TemplateFooterHeight,
                            TemplateFooterHtml = SourceDocumentTypeReportTemplate.TemplateFooterHtml,
                            TemplateBodyjson = SourceDocumentTypeReportTemplate.TemplateBodyjson,
                            TemplateHeaderHeight = SourceDocumentTypeReportTemplate.TemplateHeaderHeight,
                            TemplateHeaderHtml = SourceDocumentTypeReportTemplate.TemplateHeaderHtml,
                            IsSystem = SourceDocumentTypeHtmlTemplate.IsSystem,

                        };
                        DestinationDocumentTypeTemplateRepository.Add(AmitalDocumentTypeReportTemplate);
                    }


                    DocumentType NewDocumentType = new DocumentType()
                    {
                        Code = item.Code,
                        Id = DocTypeId,
                        InActive = item.InActive,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant,
                        AgentRoleId = item.AgentRoleId,
                        CountryCode = item.CountryCode,
                        CustomControl = item.CustomControl,
                        CustomerRoleId = item.CustomerRoleId,
                        DocumentsDataProviderCode = item.DocumentsDataProviderCode,
                        DocumentTypeCategoryCode = item.DocumentTypeCategoryCode,
                        DocumentTypeDefaultEditorTool = item.DocumentTypeDefaultEditorTool,
                        DocumentTypeDefaultHTMLTemplateId = AmitalDocumentTypeHtmlTemplate != null ? AmitalDocumentTypeHtmlTemplate.Id : null,
                        DocumentTypeDefaultReportTemplateId = AmitalDocumentTypeReportTemplate != null ? AmitalDocumentTypeReportTemplate.Id : null,
                        IsAgentView = item.IsAgentView,
                        IsAir = item.IsAir,
                        IsCopiedAtSignup = item.IsCopiedAtSignup,
                        IsCustomerView = item.IsCustomerView,
                        IsDirect = item.IsDirect,
                        IsDocIn = item.IsDocIn,
                        IsDocOut = item.IsDocOut,
                        IsDocumentOneTimePrintLimited = item.IsDocumentOneTimePrintLimited,
                        IsEnabledForCustomers = item.IsEnabledForCustomers,
                        IsHouse = item.IsHouse,
                        IsInland = item.IsInland,
                        IsMaster = item.IsMaster,
                        IsOcean = item.IsOcean,
                        IsReadOnly = item.IsReadOnly,
                        LimitedPrintCopyId = item.LimitedPrintCopyId,
                        Name = item.Name,
                        ObjectTableId = DestinationObjectTable.Id,
                        Subject = item.Subject,
                        TemplateFormatCode = item.TemplateFormatCode,
                      
                        DocumentsDataProvider = item.DocumentsDataProvider,
                        PrintingFieldsScreenCode = item.PrintingFieldsScreenCode,
                        OrderBy = item.OrderBy,
                        OnPrintPopulateDateFieldName = item.OnPrintPopulateDateFieldName,
                        OnSendPopulateDateFieldName = item.OnSendPopulateDateFieldName,
                        OnUploadPopulateDateFieldName = item.OnUploadPopulateDateFieldName,



                    };
                    DestinationDocumentTypeRepository.Add(NewDocumentType);

                    //DocumentTypeCopy
                    DocumentTypeCopyRepository LogitudeDocumentTypeCopyRepository = new DocumentTypeCopyRepository(sourceContext);
                    DocumentTypeCopyRepository AmitalDocumentTypeCopyRepository = new DocumentTypeCopyRepository(destContext);
                    var LogitudeDocumentTypeCopies = LogitudeDocumentTypeCopyRepository.GetDocumentTypeCopiesByDocumentTypeIdTenant(item.Id, item.Tenant);


                    foreach (var item2 in LogitudeDocumentTypeCopies)
                    {
                        DocumentTypeCopy NewDocumentTypeCopy = new DocumentTypeCopy()
                        {
                            Code = item2.Code,
                            Id = IdCounter.GetNumber("DocumentTypeCopy", 0),
                            InActive = item2.InActive,
                            Tenant = item2.Tenant,
                            Name = item2.Name,
                            DocumentTypeId = NewDocumentType.Id,
                            IndexOrder = item2.IndexOrder,
                            IsSelectedByDefault = item2.IsSelectedByDefault, 

                        };
                        AmitalDocumentTypeCopyRepository.Add(NewDocumentTypeCopy);
                        AmitalDocumentTypeCopyRepository.SubmitChanges();
                    }


                    //DocumentTypeCustomField
                    DocumentTypeCustomFieldRepository LogitudeDocumentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(sourceContext);
                    DocumentTypeCustomFieldRepository AmitalDocumentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(destContext);
                    var LogitudeDocumentTypeCustomFields = LogitudeDocumentTypeCustomFieldRepository.GetDocumentTypeCusotmFieldsByDocumentTypeId(item.Id, item.Tenant);


                    foreach (var item1 in LogitudeDocumentTypeCustomFields)
                    {
                        DocumentTypeCustomField NewDocumentTypeCustomField = new DocumentTypeCustomField()
                        {
                            Id = IdCounter.GetNumber("DocumentTypeCustomField", 0),
                            InActive = item1.InActive,
                            Tenant = item1.Tenant,
                            Name = item1.Name,
                            DocumentTypeId = NewDocumentType.Id,
                            IndexOrder = item1.IndexOrder,
                            DefaultValue = item1.DefaultValue,
                            FieldCode = item1.FieldCode,
                            FieldDataTypeCode = item1.FieldDataTypeCode,
                            IsRequired = item1.IsRequired,
                            MultiLine = item1.MultiLine

                        };
                        AmitalDocumentTypeCustomFieldRepository.Add(NewDocumentTypeCustomField);
                    }
                    AmitalDocumentTypeCustomFieldRepository.SubmitChanges();



                }
            }
            DestinationDocumentTypeRepository.SubmitChanges();
            DestinationDocumentTypeTemplateRepository.SubmitChanges();



            #endregion


        }

        private void CopyDocumentsMetaDataTypes(CommonDataContext Logitudecontext, CommonDataContext Amitalcontext, WebFreightContext LogitudeWebFreightContext, WebFreightContext AmitalWebFreightContext)
        {
            #region DocumentTypes + Template

            DocumentsMetaDataTypeRepository LogitudeDocumentTypeRepository = new DocumentsMetaDataTypeRepository(Logitudecontext);
            DocumentsMetaDataTypeRepository AmitalDocumentTypeRepository = new DocumentsMetaDataTypeRepository(Amitalcontext);
            UserRepository AmitalUserRepository = new UserRepository(Amitalcontext);
            var LogitudeDocumentTypes = LogitudeDocumentTypeRepository.GetDocumentsMetaDataTypes(0);
            var AmitalDocumentTypes = AmitalDocumentTypeRepository.GetDocumentsMetaDataTypes(0);

            var AmitalSystemUser = AmitalUserRepository.GetSingleUserByEmail("system@tenant0.com", 0, false);

            foreach (var item in LogitudeDocumentTypes)
            {
                string DocTypeId = IdCounter.GetNumber("DocumentsMetaDataType", 0);
                var AmitalItem = AmitalDocumentTypes.Where(a => a.Code == item.Code).FirstOrDefault();

                if (AmitalItem == null)
                {
                    DocumentsMetaDataType NewDocumentType = new DocumentsMetaDataType()
                    {
                        Code = item.Code,
                        Id = DocTypeId,
                        InActive = item.InActive,
                        CustomsMetaDataCode = item.CustomsMetaDataCode,
                        EnglishName = item.EnglishName,
                        Format = item.Format,
                        LocalName = item.LocalName,
                        Tenant = item.Tenant

                    };
                    AmitalDocumentTypeRepository.Add(NewDocumentType);
                }
            }
            AmitalDocumentTypeRepository.SubmitChanges();

            #endregion


        }

        private void CopyPackageTypes(CommonDataContext Logitudecontext, CommonDataContext Amitalcontext)
        {
            #region Package Types

            // Measurements

            MeasurementRepository LogitudeMeasurementRepository = new MeasurementRepository(Logitudecontext);
            MeasurementRepository AmitalMeasurementRepository = new MeasurementRepository(Amitalcontext);

            var LogitudeMeasurements = LogitudeMeasurementRepository.GetMeasurementsByTenant(0);
            var AmitalMeasurements = AmitalMeasurementRepository.GetMeasurementsByTenant(0);
            foreach (var item in LogitudeMeasurements)
            {
                var AmitalItem = AmitalMeasurements.Where(a => a.Code == item.Code).FirstOrDefault();
                if (AmitalItem == null)
                {
                    Measurement NewMeasurement = new Measurement()
                    {
                        Code = item.Code,
                        Id = IdCounter.GetNumber("Measurement", 0),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant,
                        IsContainer = item.IsContainer,
                        IsContainerMeasurement = item.IsContainerMeasurement,
                        Name = item.Name,
                        ShortName = item.ShortName

                    };
                    AmitalMeasurementRepository.Add(NewMeasurement);
                }
            }
            AmitalMeasurementRepository.SubmitChanges();

            // PackageTypes

            PackageTypeRepository LogitudePackageTypeRepository = new PackageTypeRepository(Logitudecontext);
            PackageTypeRepository AmitalPackageTypeRepository = new PackageTypeRepository(Amitalcontext);

            var LogitudePackageTypes = LogitudePackageTypeRepository.GetPackageTypes(0);
            var AmitalPackageTypes = AmitalPackageTypeRepository.GetPackageTypes(0);
            foreach (var item in LogitudePackageTypes)
            {
                var AmitalItem = AmitalPackageTypes.Where(a => a.Code == item.Code).FirstOrDefault();
                var LogitudeMeasurement = LogitudeMeasurementRepository.GetSingleMeasurement(item.Id, item.Tenant);
                Measurement AmitalMeasurement = null;
                if (LogitudeMeasurement != null)
                {
                    AmitalMeasurement = AmitalMeasurementRepository.GetMeasurementbyCode(LogitudeMeasurement.Code, LogitudeMeasurement.Tenant);
                }
                if (AmitalItem == null)
                {
                    PackageType NewPackageType = new PackageType()
                    {
                        Code = item.Code,
                        Id = IdCounter.GetNumber("PackageType", 0),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant,
                        IsContainer = item.IsContainer,
                        AddedManually = item.AddedManually,
                        ContainerSize = item.ContainerSize,
                        EnglishName = item.EnglishName,
                        IsAir = item.IsAir,
                        IsInland = item.IsInland,
                        IsOcean = item.IsOcean,
                        MeasurementId = AmitalMeasurement != null ? AmitalMeasurement.Id : null,
                        Notes = item.Notes,
                        PrintAs = item.PrintAs,
                        TEU = item.TEU,
                        Volume = item.Volume

                    };
                    AmitalPackageTypeRepository.Add(NewPackageType);
                }
            }
            AmitalPackageTypeRepository.SubmitChanges();

            #endregion
        }



        private void CopyPackageTypesFromTenantZeroToTarget(CommonDataContext Amitalcontext, int targetTenant)
        {
            #region Package Types

            MeasurementRepository AmitalMeasurementRepository = new MeasurementRepository(Amitalcontext);
            PackageTypeRepository AmitalPackageTypeRepository = new PackageTypeRepository(Amitalcontext);

            var Amital_Zero_Measurements = AmitalMeasurementRepository.GetMeasurementsByTenant(0).ToList();
            var Amital_Tenant_Measurements = AmitalMeasurementRepository.GetMeasurementsByTenant(targetTenant).ToList();

            foreach (var item in Amital_Zero_Measurements)
            {
                var AmitalItem = Amital_Tenant_Measurements.Where(a => a.Code == item.Code).FirstOrDefault();
                if (AmitalItem == null)
                {
                    Measurement NewMeasurement = new Measurement()
                    {
                        Code = item.Code,
                        Id = IdCounter.GetNumber("Measurement", targetTenant),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        SearchFields = item.SearchFields,
                        Tenant = targetTenant,
                        IsContainer = item.IsContainer,
                        IsContainerMeasurement = item.IsContainerMeasurement,
                        Name = item.Name,
                        ShortName = item.ShortName

                    };
                    AmitalMeasurementRepository.Add(NewMeasurement);
                    Amital_Tenant_Measurements.Add(NewMeasurement);
                }
            }

            AmitalMeasurementRepository.SubmitChanges();



            var Amital_Zero_PackageTypes = AmitalPackageTypeRepository.GetPackageTypes(0);
            var Amital_Tenant_PackageTypes = AmitalPackageTypeRepository.GetPackageTypes(targetTenant);
            foreach (var item in Amital_Zero_PackageTypes)
            {
                var tenant_Item = Amital_Tenant_PackageTypes.Where(a => a.Code == item.Code).FirstOrDefault();

                if (tenant_Item == null)
                {
                    Measurement Amital_Tenant_Measurement = null;
                    if (!string.IsNullOrEmpty(item.MeasurementId))
                    {

                        var tenant_Zero_Measurement = Amital_Zero_Measurements.FirstOrDefault(p => p.Id == item.MeasurementId);
                        Amital_Tenant_Measurement = Amital_Tenant_Measurements.First(m => m.Code == tenant_Zero_Measurement.Code); //AmitalMeasurementRepository.GetMeasurementbyCode(LogitudeMeasurement.Code, LogitudeMeasurement.Tenant);
                    }
                    PackageType NewPackageType = new PackageType()
                    {
                        Code = item.Code,
                        Id = IdCounter.GetNumber("PackageType", targetTenant),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        SearchFields = item.SearchFields,
                        Tenant = targetTenant,
                        IsContainer = item.IsContainer,
                        AddedManually = item.AddedManually,
                        ContainerSize = item.ContainerSize,
                        EnglishName = item.EnglishName,
                        IsAir = item.IsAir,
                        IsInland = item.IsInland,
                        IsOcean = item.IsOcean,
                        MeasurementId = Amital_Tenant_Measurement != null ? Amital_Tenant_Measurement.Id : null,
                        Notes = item.Notes,
                        PrintAs = item.PrintAs,
                        TEU = item.TEU,
                        Volume = item.Volume

                    };
                    AmitalPackageTypeRepository.Add(NewPackageType);
                }
            }
            AmitalPackageTypeRepository.SubmitChanges();

            #endregion
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int targetTenant = 0;
            if (int.TryParse(txtTargetTenant.Text, out targetTenant))
            {
                DbConnection Amitalconnection = DatabaseInitializer.GetConnection(AmitalconnectionString);
                CommonDataContext Amitalcontext = new CommonDataContext(Amitalconnection);
                this.CopyPackageTypesFromTenantZeroToTarget(Amitalcontext, targetTenant);
            }
            else
            {
                MessageBox.Show("Invalid Tenant");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            #region Entity Status

            DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(LogitudeconnectionString);
            WebFreightContext LogitudeWebFreightContext = new WebFreightContext(Logitudeconnection);

            DbConnection Amitalconnection = DatabaseInitializer.GetConnection(AmitalconnectionString);
            WebFreightContext AmitalWebFreightContext = new WebFreightContext(Amitalconnection);



            ObjectTableRepository AmitalObjectTabelRepository = new ObjectTableRepository(AmitalWebFreightContext);
            ObjectTableRepository LogitudeObjectTabelRepository = new ObjectTableRepository(LogitudeWebFreightContext);
            var LogitudeObjectTables = LogitudeObjectTabelRepository.GetObjectsByTenant(0);
            var AmitalObjectTables = AmitalObjectTabelRepository.GetObjectsByTenant(0);
            EntityStatusRepository LogitudeEntityStatusRepository = new EntityStatusRepository(LogitudeWebFreightContext);
            EntityStatusRepository AmitalEntityStatusRepository = new EntityStatusRepository(AmitalWebFreightContext);

            var LogitudeEntityStatus = LogitudeEntityStatusRepository.GetEntityStatusByTenant(0);
            var AmitalEntityStatus = AmitalEntityStatusRepository.GetEntityStatusByTenant(0);
            foreach (var item in LogitudeEntityStatus)
            {
                var LogitudeObjectTable = LogitudeObjectTables.FirstOrDefault(t => t.Id == item.ObjectTableId); //ObjectTabelRepository.GetSingleObjectTableById(item.ObjectTableId, item.Tenant);
                var AmitalObjectTable = AmitalObjectTables.Where(t => t.Name == LogitudeObjectTable.Name).FirstOrDefault();//AmitalObjectTabelRepository.GetObjectTableByName(LogitudeObjectTable.Name, item.Tenant, false);

                var AmitalItem = AmitalEntityStatus.Where(a => a.Code == item.Code).FirstOrDefault();

                if (AmitalItem == null)
                {
                    EntityStatus NewEntityStatus = new EntityStatus()
                    {
                        Code = item.Code,
                        Id = IdCounter.GetNumber("EntityStatus", 0),
                        InActive = item.InActive,
                        SearchFields = item.SearchFields,
                        Tenant = item.Tenant,
                        Name = item.Name,
                        ObjectTableId = AmitalObjectTable.Id,
                        StatusWeight = item.StatusWeight
                    };
                    AmitalEntityStatusRepository.Add(NewEntityStatus);
                }
                else
                {
                    AmitalItem.Code = item.Code;
                    AmitalItem.InActive = item.InActive;
                    AmitalItem.SearchFields = item.SearchFields;
                    AmitalItem.Tenant = item.Tenant;
                    AmitalItem.Name = item.Name;
                    AmitalItem.ObjectTableId = AmitalObjectTable.Id;
                    AmitalItem.StatusWeight = item.StatusWeight;
                    AmitalEntityStatusRepository.Update(AmitalItem);
                }
            }
            AmitalEntityStatusRepository.SubmitChanges();

            #endregion
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(LogitudeconnectionString);
            CommonDataContext Logitudecontext = new CommonDataContext(Logitudeconnection);
            WebFreightContext LogitudeWebFreightContext = new WebFreightContext(Logitudeconnection);

            DbConnection Amitalconnection = DatabaseInitializer.GetConnection(AmitalconnectionString);
            CommonDataContext Amitalcontext = new CommonDataContext(Amitalconnection);
            WebFreightContext AmitalWebFreightContext = new WebFreightContext(Amitalconnection);

            this.CopyDocumentsMetaDataTypes(Logitudecontext, Amitalcontext, LogitudeWebFreightContext, AmitalWebFreightContext);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFromPort.Text) && !string.IsNullOrEmpty(txtToPort.Text))
            {
                DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(LogitudeconnectionString);
                CommonDataContext Logitudecontext = new CommonDataContext(Logitudeconnection);
                WebFreightContext LogitudeWebFreightContext = new WebFreightContext(Logitudeconnection);

                DbConnection Amitalconnection = DatabaseInitializer.GetConnection(AmitalconnectionString);
                CommonDataContext Amitalcontext = new CommonDataContext(Amitalconnection);
                WebFreightContext AmitalWebFreightContext = new WebFreightContext(Amitalconnection);

                int fromtenant = int.Parse(txtFromPort.Text);
                int totenant = int.Parse(txtToPort.Text);
                this.CopyPortsFromTenants(Logitudecontext, Amitalcontext, fromtenant, totenant);

                label1.Visible = true;
                //txtStartCopy.Enabled = true;
            }
        }

        private void CopyPortsFromTenants(CommonDataContext Logitudecontext, CommonDataContext Amitalcontext, int FromTenant, int ToTenant)
        {
            #region Ports
            // GLobal Zones

            GlobalZoneRepository LogitudeglobalZoneRepository = new GlobalZoneRepository(Logitudecontext);
            GlobalZoneRepository AmitalglobalZoneRepository = new GlobalZoneRepository(Amitalcontext);

            var LogitudeGlobalzones = LogitudeglobalZoneRepository.GetGlobalZones(FromTenant);
            var AmitalGlobalzones = AmitalglobalZoneRepository.GetGlobalZones(ToTenant);
            foreach (var item in LogitudeGlobalzones)
            {
                var AmitalItem = AmitalGlobalzones.Where(a => a.Code == item.Code).FirstOrDefault();
                if (AmitalItem == null)
                {
                    GlobalZone NewGlobalZone = new GlobalZone()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("Globalzone", ToTenant),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = ToTenant
                    };
                    AmitalglobalZoneRepository.Add(NewGlobalZone);
                }
            }
            AmitalglobalZoneRepository.SubmitChanges();

            // Countries

            CountryRepository LogitudeCountriesRepository = new CountryRepository(Logitudecontext);
            CountryRepository AmitalCountriesRepository = new CountryRepository(Amitalcontext);

            var LogitudeCountries = LogitudeCountriesRepository.GetCountries(FromTenant);
            var AmitalCountries = AmitalCountriesRepository.GetCountries(ToTenant);

            foreach (var item in LogitudeCountries)
            {
                var AmitalItem = AmitalCountries.Where(a => a.Code == item.Code).FirstOrDefault();
                var LogitudeGZone = LogitudeglobalZoneRepository.GetSingleGlobalZone(item.GlobalZoneId, item.Tenant);
                if (LogitudeGZone == null)
                {
                    LogitudeGZone = LogitudeglobalZoneRepository.GetSingleGlobalZone(item.GlobalZoneId, 0);
                }
                var AmitalGZone = AmitalglobalZoneRepository.GetSingleGlobalZoneByCode(LogitudeGZone.Code, ToTenant);
                if (AmitalItem == null)
                {
                    Country NewCountry = new Country()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("Country", ToTenant),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = ToTenant,
                        AddedManually = item.AddedManually,
                        EC = item.EC,
                        HasCitiesList = item.HasCitiesList,
                        HasStates = item.HasStates,
                        IsStateRequired = item.IsStateRequired,
                        GlobalZoneId = AmitalGZone.Id

                    };
                    AmitalCountriesRepository.Add(NewCountry);

                }
            }
            AmitalCountriesRepository.SubmitChanges();

            // States

            StateRepository LogitudeStatesRepository = new StateRepository(Logitudecontext);
            StateRepository AmitalStatesRepository = new StateRepository(Amitalcontext);

            var LogitudeStates = LogitudeStatesRepository.GetStates(FromTenant);
            var AmitalStates = AmitalStatesRepository.GetStates(ToTenant);

            foreach (var item in LogitudeStates)
            {
                var AmitalItem = AmitalStates.Where(a => a.Code == item.Code).FirstOrDefault();
                //var LogitudeGZone = LogitudeglobalZoneRepository.GetSingleGlobalZone(item.GlobalZoneId,item.Tenant);
                var AmitalCountry = AmitalCountriesRepository.GetSingleCountryByCode(item.Country.Code, ToTenant);
                if (AmitalItem == null)
                {
                    State NewState = new State()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("State", ToTenant),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = ToTenant,
                        AddedManually = item.AddedManually,
                        CountryId = AmitalCountry.Id


                    };
                    AmitalStatesRepository.Add(NewState);

                }
            }
            AmitalStatesRepository.SubmitChanges();

            // Ports

            PortRepository LogitudePortsRepository = new PortRepository(Logitudecontext);
            PortRepository AmitalPortsRepository = new PortRepository(Amitalcontext);

            var LogitudePorts = LogitudePortsRepository.GetPorts(FromTenant);
            var AmitalPorts = AmitalPortsRepository.GetPorts(ToTenant);
            int counter = 0;
            Port NewPort;
            foreach (var item in LogitudePorts)
            {
                var cont = item.Country.Code;
                var AmitalItem = AmitalPorts.Where(a => (a.Code == item.Code && a.Country.Code == item.Country.Code)).FirstOrDefault();
                //var LogitudeGZone = LogitudeglobalZoneRepository.GetSingleGlobalZone(item.GlobalZoneId, item.Tenant);
                var AmitalCountry = AmitalCountries.FirstOrDefault(s => s.Code == item.Country.Code && s.Tenant == ToTenant);//AmitalCountriesRepository.GetSingleCountryByCode(item.Country.Code, item.Tenant);
                State LogitudeState = null;
                LogitudeState = LogitudeStates.FirstOrDefault(s => s.Id == item.StateId && s.Tenant == item.Tenant);//LogitudeStatesRepository.GetSingleState(item.StateId, item.Tenant);
                State AmitalState = null;
                if (LogitudeState != null)
                {
                    AmitalState = AmitalStates.FirstOrDefault(s => s.Code == LogitudeState.Code && s.Tenant == ToTenant);//AmitalStatesRepository.GetSingleStateByCode(LogitudeState.Code, item.Tenant);
                }

                if (AmitalItem == null)
                {
                    NewPort = new Port()
                    {
                        Code = item.Code,
                        EnglishName = item.EnglishName,
                        Id = IdCounter.GetNumber("Port", ToTenant),
                        InActive = item.InActive,
                        LocalName = item.LocalName,
                        Notes = item.Notes,
                        SearchFields = item.SearchFields,
                        Tenant = ToTenant,
                        AddedManually = item.AddedManually,
                        CountryId = AmitalCountry.Id,
                        Field1 = item.Field1,
                        Field2 = item.Field2,
                        Field3 = item.Field3,
                        Field4 = item.Field4,
                        Field5 = item.Field5,
                        Field6 = item.Field6,
                        Field7 = item.Field7,
                        Field8 = item.Field8,
                        Field9 = item.Field9,
                        Field10 = item.Field10,
                        IsAir = item.IsAir,
                        IsInland = item.IsInland,
                        IsOcean = item.IsOcean,
                        Latitude = item.Latitude,
                        Longtitude = item.Longtitude,
                        StateId = AmitalState != null ? AmitalState.Id : null,


                    };
                    UpdatePortSearchFieldService.Update(NewPort);

                    AmitalPortsRepository.Add(NewPort);
                }

                counter++;
                if (counter == 1000)
                {
                    counter = 0;
                    AmitalPortsRepository.SubmitChanges();
                }
            }
            if (counter != 0)
            {
                AmitalPortsRepository.SubmitChanges();
            }

            #endregion
        }

        private void btnCopyUserContactsToCrm_Click(object sender, EventArgs e)
        {

            CopyContactsToCrmTenant();

           

            label1.Visible = true;
            txtStartCopy.Enabled = true;
        }

        private void CopyContactsToCrmTenant()
        {
            try
            {
                List<int> copiedTenants = new List<int>();
                int crmTenant = 341;

                LogitudeconnectionString = ConfigurationManager.ConnectionStrings["MainStr"].ConnectionString;
                DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(LogitudeconnectionString);
                CommonDataContext Logitudecontext = new CommonDataContext(Logitudeconnection);
                WebFreightContext LogitudeWebFreightContext = new WebFreightContext(Logitudeconnection);

                UserRepository userRepository = new UserRepository(Logitudecontext);


                List<string> crmContactEmails = (from a in userRepository.context.Contacts
                                                 where a.Tenant == 341
                                                 select a.Email).ToList();

                List<Card> crmCustomerAccountingCards = (from a in userRepository.context.Cards
                                                         where a.Tenant == 341 && !string.IsNullOrEmpty(a.ReceivablesAccountingCard)
                                                         select a).ToList();


                int count = 0;
                foreach (Card customerCard in crmCustomerAccountingCards)
                {
                    int crmCustomerAccountTenant = 0;
                    if (int.TryParse(customerCard.ReceivablesAccountingCard, out crmCustomerAccountTenant))
                    {
                        if (copiedTenants.Contains(crmCustomerAccountTenant))
                        {
                            continue;
                        }

                        List<Contact> customerTenantContacts = (from a in userRepository.context.Users.Include("Contact")
                                                                where a.Tenant == crmCustomerAccountTenant
                                                                && !string.IsNullOrEmpty(a.Contact.Email)
                                                                && a.Contact.Email != "customercare@logitudeworld.com"
                                                                && !a.Contact.Email.Contains("system@tenant")
                                                                && a.Contact.InActive == false
                                                                select a.Contact).ToList();


                        foreach (Contact oldContact in customerTenantContacts)
                        {
                            //Card accountingCard = crmCustomerAccountingCards.FirstOrDefault(a => a.AccountingCard == oldContact.Tenant.ToString());
                            if (!crmContactEmails.Contains(oldContact.Email))
                            {
                                count++;

                                Contact contact = new Contact()
                                {
                                    Id = IdCounter.GetNumber("Contact", crmTenant),
                                    Tenant = crmTenant,
                                    Email = oldContact.Email,
                                    Anniversary = oldContact.Anniversary,
                                    AnniversaryReminder = oldContact.AnniversaryReminder,
                                    Birthday = oldContact.Birthday,
                                    BirthDayOfYear = oldContact.BirthDayOfYear,
                                    BirthdayReminder = oldContact.BirthdayReminder,
                                    BusinessPhone = oldContact.BusinessPhone,
                                    ColorIndex = oldContact.ColorIndex,
                                    CompanyName = oldContact.CompanyName,
                                    ComputedKey = oldContact.ComputedKey,
                                    ContactDoneMethod = oldContact.ContactDoneMethod,
                                    ContactDoneMethodCode = oldContact.ContactDoneMethodCode,
                                    ContactLastLogin = oldContact.ContactLastLogin,
                                    DisplayGettingStarted = oldContact.DisplayGettingStarted,
                                    DoneDate = oldContact.DoneDate,
                                    DontShowLocalLabels = oldContact.DontShowLocalLabels,
                                    EnglishName = oldContact.EnglishName,
                                    ExternalId = oldContact.ExternalId,
                                    FacebookId = oldContact.FacebookId,
                                    Fax = oldContact.Fax,
                                    ImageDetailId = oldContact.ImageDetailId,
                                    InActive = oldContact.InActive,
                                    IndexColor = oldContact.IndexColor,
                                    LocalName = oldContact.LocalName,
                                    Mobile = oldContact.Mobile,
                                    Name = oldContact.Name,
                                    Notes = oldContact.Notes,
                                    Position = oldContact.Position,
                                    SearchFields = oldContact.SearchFields,
                                    Signature = oldContact.Signature,
                                    SignatureHtml = oldContact.SignatureHtml,
                                    UserType = oldContact.UserType,
                                };

                                CardContact cardContact = new CardContact()
                                {
                                    Id = IdCounter.GetNumber("CardContact", crmTenant),
                                    CardId = customerCard.Id,
                                    ContactId = contact.Id,
                                    Tenant = crmTenant,
                                };


                                Logitudecontext.Contacts.Add(contact);
                                Logitudecontext.CardContacts.Add(cardContact);

                                if (count == 500)
                                {
                                    count = 0;

                                    Logitudecontext.SaveChanges();
                                }

                            }
                            else
                            {
                                this.WriteToFile("Contact Id:" + oldContact.Id + " ,Email:" + oldContact.Email + " ,Tenant:" + oldContact.Tenant + " was not copied (already exist in crm tenant!)");

                            }
                        }

                        copiedTenants.Add(crmCustomerAccountTenant);
                    }
                }

                //List<Contact> notCopiedContacts = (from a in userRepository.context.Users.Include("Contact")
                //                                   where a.Tenant != 341 && a.Tenant != 0 && !crmUserEmails.Contains(a.Contact.Email)
                //                                   select a.Contact).ToList();


                Logitudecontext.SaveChanges();
            }
            catch (Exception ex)
            {
                this.WriteToFile(ex.Message);
            }
        }

        private void WriteToFile(string message)
        {
            string path = @".\output.txt";

            // This text is added only once to the file. 
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                string createText = "Hello and Welcome" + Environment.NewLine;
                File.WriteAllText(path, createText);
            }

            
            string appendText = message + Environment.NewLine;
            File.AppendAllText(path, appendText);

           
            string readText = File.ReadAllText(path);

            Console.WriteLine(readText);
             
        }

        private void btnCopyChargesTypesToCloud_Click(object sender, EventArgs e)
        {

            return;
            int logitudeTenant = 0;
            int amitalTenant = 0;
            DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(LogitudeconnectionString);
            WebFreightContext LogitudeWebFreightContext = new WebFreightContext(Logitudeconnection);
            CommonDataContext LogitudeCommonDataContext = new CommonDataContext(Logitudeconnection);

            DbConnection Amitalconnection = DatabaseInitializer.GetConnection(AmitalconnectionString);
            WebFreightContext AmitalWebFreightContext = new WebFreightContext(Amitalconnection);
            CommonDataContext AmitalCommonDataContext = new CommonDataContext(Amitalconnection);

 
            ChargesTypeRepository LogitudeChargesTypeRepository = new ChargesTypeRepository(LogitudeCommonDataContext);
            ChargesTypeRepository AmitalChargesTypeRepository = new ChargesTypeRepository(AmitalCommonDataContext);

            ChargesGroupRepository LogitudeChargesGroupRepository = new ChargesGroupRepository(LogitudeWebFreightContext);
            ChargesGroupRepository AmitalChargesGroupRepository = new ChargesGroupRepository(AmitalWebFreightContext);

            MeasurementRepository LogitudeMeasurementRepository = new MeasurementRepository(LogitudeCommonDataContext);
            MeasurementRepository AmitalMeasurementRepository = new MeasurementRepository(AmitalCommonDataContext);

            List<ChargesGroup> LogitudeChargesGroups = LogitudeChargesGroupRepository.GetChargesGroups(logitudeTenant).ToList();
            List<ChargesGroup> AmitalChargesGroups = AmitalChargesGroupRepository.GetChargesGroups(amitalTenant).ToList();

            List<Measurement> LogitudeMeasurements = LogitudeMeasurementRepository.GetMeasurementsByTenant(logitudeTenant).ToList();
            List<Measurement> AmitalMeasurements = AmitalMeasurementRepository.GetMeasurementsByTenant(amitalTenant).ToList();

            List<ChargesType> LogitudeChargesTypes = LogitudeChargesTypeRepository.GetChargesTypes(logitudeTenant).ToList();
            List<ChargesType> AmitalChargesTypes = AmitalChargesTypeRepository.GetChargesTypes(amitalTenant).ToList();
            foreach (var item in LogitudeChargesTypes)
            {
                var AmitalItem = AmitalChargesTypes.Where(a => a.Code == item.Code).FirstOrDefault();
                if (AmitalItem == null)
                {
                    ChargesGroup amitalChargesGroup = CopyChargesGroupToAmital(amitalTenant, AmitalChargesGroupRepository, LogitudeChargesGroups, AmitalChargesGroups, item.ChargesGroupId);
                    Measurement amitalMeasurement = (!string.IsNullOrEmpty(item.MeasurementId) ? CopyMeasurementToAmital(amitalTenant, AmitalMeasurementRepository, LogitudeMeasurements, AmitalMeasurements, item.MeasurementId) : null);
                    Measurement amitalContainerMeasurement = (!string.IsNullOrEmpty(item.ContainerMeasurementId) ? CopyMeasurementToAmital(amitalTenant, AmitalMeasurementRepository, LogitudeMeasurements, AmitalMeasurements, item.ContainerMeasurementId) : null);

                    ChargesType NewChargesType = new ChargesType()
                    {
                        Code = item.Code,
                        Id = IdCounter.GetNumber("ChargesType", amitalTenant),
                        InActive = item.InActive,
                        SearchFields = item.SearchFields,
                        Tenant = amitalTenant,
                        EnglishName = item.EnglishName,
                        LocalName = item.LocalName,
                        AccountingVATSplit = item.AccountingVATSplit,
                        ChargesGroupId = amitalChargesGroup.Id,
                        ChargesGroupCode = item.ChargesGroupCode,
                        Description = item.Description,
                        IsAir = item.IsAir,
                        IsAutoDisplayInConsolidation = item.IsAutoDisplayInConsolidation,
                        IsAutoDisplayInCustoms = item.IsAutoDisplayInCustoms,
                        IsAutoDisplayInQuote = item.IsAutoDisplayInQuote,
                        IsAutoDisplayInShipment = item.IsAutoDisplayInShipment,
                        IsBackToBack = item.IsBackToBack,
                        IsCustoms = item.IsCustoms,
                        IsExpense = item.IsExpense,
                        IsInland = item.IsInland,
                        IsOcean = item.IsOcean,
                        IsPayable = item.IsPayable,
                        IsReceivable = item.IsReceivable,
                        ViewOrder = item.ViewOrder,
                        MeasurementId = amitalMeasurement != null ? amitalMeasurement.Id : null,
                        ContainerMeasurementId = amitalContainerMeasurement != null ? amitalContainerMeasurement.Id : null,
                    };

                    AmitalChargesTypeRepository.Add(NewChargesType);
                }
                else
                {
                     // no update needed
                }
            }

            AmitalChargesGroupRepository.SubmitChanges();
            AmitalChargesTypeRepository.SubmitChanges();

            
        }

        private  Measurement CopyMeasurementToAmital(int amitalTenant, MeasurementRepository AmitalMeasurementRepository, List<Measurement> LogitudeMeasurements, List<Measurement> AmitalMeasurements, string logitudeMeasurementId)
        {
            Measurement logitudeMeasurement = LogitudeMeasurements.FirstOrDefault(c => c.Id == logitudeMeasurementId);
            Measurement amitalMeasurement = AmitalMeasurements.FirstOrDefault(c => c.Code == logitudeMeasurement.Code);
            if (amitalMeasurement == null)
            {
                amitalMeasurement = new Measurement()
                {
                    Id = IdCounter.GetNumber("Measurement", amitalTenant),
                    Tenant = amitalTenant,
                    Code = logitudeMeasurement.Code,
                    Name = logitudeMeasurement.Name,
                    LocalName = logitudeMeasurement.LocalName,
                    SearchFields = logitudeMeasurement.SearchFields,
                    IsContainer = logitudeMeasurement.IsContainer,
                    IsContainerMeasurement = logitudeMeasurement.IsContainerMeasurement,
                    ShortName = logitudeMeasurement.ShortName,
                    //InActive = logitudeMeasurement.InActive,
                };

                AmitalMeasurementRepository.Add(amitalMeasurement);
            }

            return amitalMeasurement;
        }

        private  ChargesGroup CopyChargesGroupToAmital(int amitalTenant, ChargesGroupRepository AmitalChargesGroupRepository, List<ChargesGroup> LogitudeChargesGroups, List<ChargesGroup> AmitalChargesGroups, string logitudeChargesGroupId)
        {
            ChargesGroup logitudeChargesGroup = LogitudeChargesGroups.FirstOrDefault(c => c.Id == logitudeChargesGroupId);
            ChargesGroup amitalChargesGroup = AmitalChargesGroups.FirstOrDefault(c => c.Code == logitudeChargesGroup.Code);
            if (amitalChargesGroup == null)
            {
                amitalChargesGroup = new ChargesGroup()
                {
                    Id = IdCounter.GetNumber("ChargesType", amitalTenant),
                    Tenant = amitalTenant,
                    Code = logitudeChargesGroup.Code,
                    Name = logitudeChargesGroup.Name,
                    LocalName = logitudeChargesGroup.LocalName,
                    SearchFields = logitudeChargesGroup.SearchFields,
                    ViewOrder = logitudeChargesGroup.ViewOrder,
                };

                AmitalChargesGroupRepository.Add(amitalChargesGroup);
            }

            return amitalChargesGroup;
        }

        
        private void btnCopyDocumentTypes_Click_1(object sender, EventArgs e)
        {
            //logbox-global,logboxadmin,London2015!London2015!,logboxdbs.database.windows.net
            //Global,sa,Saas256,amitaldata.cloudapp.net
            if (!string.IsNullOrEmpty(txtSourceConnStr.Text) && !string.IsNullOrEmpty(txtDestinationConnStr.Text))
            {
                DbConnection Logitudeconnection = DatabaseInitializer.GetConnection(txtSourceConnStr.Text);
                CommonDataContext Logitudecontext = new CommonDataContext(Logitudeconnection);
                WebFreightContext LogitudeWebFreightContext = new WebFreightContext(Logitudeconnection);

                DbConnection Amitalconnection = DatabaseInitializer.GetConnection(txtDestinationConnStr.Text);
                CommonDataContext Amitalcontext = new CommonDataContext(Amitalconnection);
                WebFreightContext AmitalWebFreightContext = new WebFreightContext(Amitalconnection);



                this.CopyDocumentTypes(Logitudecontext, Amitalcontext, LogitudeWebFreightContext, AmitalWebFreightContext);

                MessageBox.Show("Copy finished successfully");
            }
            else
            {
                MessageBox.Show("Please fill the source and destination connection strings");
            }
        }
    }
}
