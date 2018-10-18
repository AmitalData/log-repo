using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        public void LoadHelpResources()
        {
            //FeatureRepository featureRepository = new FeatureRepository(0);
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();

            //#region Features
            //Feature Feature01 = tenantFeatures.Where(d => d.Code == "CREATESIGNATURE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature02 = tenantFeatures.Where(d => d.Code == "MEASUREMENT" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature03 = tenantFeatures.Where(d => d.Code == "MANAGEAWBSTOCK" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature04 = tenantFeatures.Where(d => d.Code == "CANCELINVOICE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature05 = tenantFeatures.Where(d => d.Code == "SHAREDLOGISTICS" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature06 = tenantFeatures.Where(d => d.Code == "GETTINGAROUND" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature07 = tenantFeatures.Where(d => d.Code == "AWBTUTORIAL" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature08 = tenantFeatures.Where(d => d.Code == "CONSOLINVOICE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature09 = tenantFeatures.Where(d => d.Code == "AWBTUTORIALFR" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature10 = tenantFeatures.Where(d => d.Code == "BUILDCONSOLIDATION" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature11 = tenantFeatures.Where(d => d.Code == "GENERICINTERFACE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature12 = tenantFeatures.Where(d => d.Code == "MANAGEUSERS" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature13 = tenantFeatures.Where(d => d.Code == "ADVANCEDWORKBOOK" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature14 = tenantFeatures.Where(d => d.Code == "AWBQUICKTOUR" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature15 = tenantFeatures.Where(d => d.Code == "MAILTEMPLATES" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature16 = tenantFeatures.Where(d => d.Code == "MANAGECURRENCY" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature17 = tenantFeatures.Where(d => d.Code == "ANALYZINGCRM" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature18 = tenantFeatures.Where(d => d.Code == "MANAGEOPPORTUNITY" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature19 = tenantFeatures.Where(d => d.Code == "ACTIVITYWORK" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature20 = tenantFeatures.Where(d => d.Code == "AWBTUTORIALSP" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature21 = tenantFeatures.Where(d => d.Code == "GETTINGAROUNDSP" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature22 = tenantFeatures.Where(d => d.Code == "MANAGECUSTOMERS" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature23 = tenantFeatures.Where(d => d.Code == "OUTLOOKCONNETION" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature24 = tenantFeatures.Where(d => d.Code == "CUSTOMROLES" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature25 = tenantFeatures.Where(d => d.Code == "AIRLINEACCOUNT" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature26 = tenantFeatures.Where(d => d.Code == "MANAGECUSTOMERSHE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature27 = tenantFeatures.Where(d => d.Code == "EBOOKWORK" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature28 = tenantFeatures.Where(d => d.Code == "CHANGEPASSWORD" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature29 = tenantFeatures.Where(d => d.Code == "BLUESNAP" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature30 = tenantFeatures.Where(d => d.Code == "UNIFREIGHTGUIDE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature31 = tenantFeatures.Where(d => d.Code == "UNIFREIGHTSHARING" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature32 = tenantFeatures.Where(d => d.Code == "OUTLOOKCONNECTIONHEBREW" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature33 = tenantFeatures.Where(d => d.Code == "ANALYZINGCRMHEBREW" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature34 = tenantFeatures.Where(d => d.Code == "OUTLOOKINSTALLATIONHEBREW" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature35 = tenantFeatures.Where(d => d.Code == "MANAGEOPPORTUNITYHEBREW" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature36 = tenantFeatures.Where(d => d.Code == "UNIFREIGHTCRMR52015" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature37 = tenantFeatures.Where(d => d.Code == "UNIFREIGHTCRMR12016" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature38 = tenantFeatures.Where(d => d.Code == "UNIFREIGHTCRMR22016" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature39 = tenantFeatures.Where(d => d.Code == "OUTLOOKCONNECTIONSETUP" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature40 = tenantFeatures.Where(d => d.Code == "LOGITUDEMOBILE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature41 = tenantFeatures.Where(d => d.Code == "SHAREDLOGISTICSANDMOBILESETUP" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature42 = tenantFeatures.Where(d => d.Code == "ANALYZINGCRMFRENCH" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature43 = tenantFeatures.Where(d => d.Code == "MANAGEOPPORTUNITYFRENCH" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature44 = tenantFeatures.Where(d => d.Code == "ACTIVITYWORKFRENCH" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature45 = tenantFeatures.Where(d => d.Code == "QUOTESTUTORIAL" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature46 = tenantFeatures.Where(d => d.Code == "MANAGEUSERFRENCHTUTORIAL" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature47 = tenantFeatures.Where(d => d.Code == "QUICKBOOKSCONNECTION" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature48 = tenantFeatures.Where(d => d.Code == "VATTYPEMANAGEMENT" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature49 = tenantFeatures.Where(d => d.Code == "BUILDCONSOLIDATIONSPANISH" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature50 = tenantFeatures.Where(d => d.Code == "SHAREDLOGISTICSANDMOBILESPANISH" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature Feature51 = tenantFeatures.Where(d => d.Code == "FOLLOWUPSTORIAL" && d.FeatureTypeCode == "AREA").FirstOrDefault();

            //Feature FeatureR01 = tenantFeatures.Where(d => d.Code == "RELEASEDEC15" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR02 = tenantFeatures.Where(d => d.Code == "RELEASEFEB16" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR03 = tenantFeatures.Where(d => d.Code == "RELEASEMAY16" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR04 = tenantFeatures.Where(d => d.Code == "RELEASEJUL16" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR05 = tenantFeatures.Where(d => d.Code == "RELEASEOCT16" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR06 = tenantFeatures.Where(d => d.Code == "RELEASEDEC16" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR07 = tenantFeatures.Where(d => d.Code == "RELEASEFEB17" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR08 = tenantFeatures.Where(d => d.Code == "RELEASEMAY17" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureR09 = tenantFeatures.Where(d => d.Code == "RELEASEJUL17" && d.FeatureTypeCode == "AREA").FirstOrDefault();

            //Feature FeatureV01 = tenantFeatures.Where(d => d.Code == "LOGITUDEINTRO" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureV02 = tenantFeatures.Where(d => d.Code == "BUILDSHIPMENT" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureV03 = tenantFeatures.Where(d => d.Code == "ISSUEINVOICE" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureV04 = tenantFeatures.Where(d => d.Code == "BUSINESSTOOLS" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureV05 = tenantFeatures.Where(d => d.Code == "BILLINGTOOLS" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //Feature FeatureV06 = tenantFeatures.Where(d => d.Code == "AWBWORLD" && d.FeatureTypeCode == "AREA").FirstOrDefault();
            //#endregion

            //HelpResourceRepository helpResourceRepository = new HelpResourceRepository();
            //Dictionary<string, HelpResource> TenantHelpResources = helpResourceRepository.GetAllHelpResources().ToDictionary(d => d.Code, a => a);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails() 
            //{ 
            //    Code = "1",
            //    Name = "Create Your Own Signature", 
            //    Language = "EN", 
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "define-signature.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature01.Code 
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "2",
            //    Name = "Change the Units of Measurement",
            //    Language = "EN",
            //    Type = "HOW",
            //    Category = "OPE",
            //    FileName = "change-measurements.html",
            //    IsNew = false,
            //    FeatureCode = Feature02.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "3",
            //    Name = "Manage AWB Stock by Airlines",
            //    Language = "EN",
            //    Type = "HOW",
            //    Category = "AWB",
            //    FileName = "manage-awb-stock.html",
            //    IsNew = false,
            //    FeatureCode = Feature03.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "4",
            //    Name = "Cancel an Invoice",
            //    Language = "EN",
            //    Type = "HOW",
            //    Category = "ACC",
            //    FileName = "cancel-invoice.html",
            //    IsNew = false,
            //    FeatureCode = Feature04.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "5",
            //    Name = "Activate Shared Logistics",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "shared-logistics.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature05.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "6",
            //    Name = "Getting Around in Logitude",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "getting_around.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature06.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "7",
            //    Name = "e-AWB Tutorial",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "AWB",
            //    FileName = "eawb_tutorial.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature07.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "8",
            //    Name = "Issue a Consolidated Invoice",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "howto_issue_consolidatedinvoice.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature08.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "9",
            //    Name = "e-AWB Tutorial (French)",
            //    Language = "FR",
            //    Type = "TUT",
            //    Category = "AWB",
            //    FileName = "eawb_tutorial_french.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature09.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "10",
            //    Name = "Build a Consolidation Shipment",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "howto_build_consolidationshipment.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature10.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "11",
            //    Name = "Generic Invoice Interface",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "generic_invoice_interface.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature11.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "12",
            //    Name = "Manage Users",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "manage_users.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature12.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "13",
            //    Name = "Advanced Features Workbook",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "advanced_features_workbook.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature13.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "14",
            //    Name = "e-AWB Quick Tour",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "AWB",
            //    FileName = "eawb_quicktour.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature14.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "15",
            //    Name = "Managing Mail Templates",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "managing_mail_templates.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature15.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "16",
            //    Name = "Currency Management",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "currency_management.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature16.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "17",
            //    Name = "Analyzing CRM Data",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "analyzing_crm_data.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature17.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "18",
            //    Name = "Managing Opportunities",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "managing_opportunities.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature18.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "19",
            //    Name = "Working With Activities",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "working_with_activities.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature19.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "20",
            //    Name = "e-AWB Tutorial (Spanish)",
            //    Language = "SP",
            //    Type = "TUT",
            //    Category = "AWB",
            //    FileName = "eawb_tutorial_spanish.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature20.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "21",
            //    Name = "Getting Around in Logitude (Spanish)",
            //    Language = "SP",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "getting_around_spanish.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature21.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "22",
            //    Name = "Managing Customers",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "managing_customers.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature22.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "23",
            //    Name = "Logitude Outlook Connection",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "outlook_connection.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature23.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "24",
            //    Name = "Custom Roles",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "custom_roles.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature24.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "25",
            //    Name = "Airline Account Number",
            //    Language = "EN",
            //    Type = "HOW",
            //    Category = "AWB",
            //    FileName = "airline_account_number.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature25.Code
            //}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "26",
            ////    Name = "Managing Customers (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "managing_customers_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature26.Code
            ////}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "27",
            //    Name = "Working with eBooking",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "AWB",
            //    FileName = "ebooking_tutorial.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature27.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "28",
            //    Name = "How to Change Password",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "howto_change_password.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature28.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "29",
            //    Name = "Subscribe to Logitude BlueSnap",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "subscribe_to_bluesnap.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature29.Code
            //}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "30",
            ////    Name = "Unifreight Mobile - User Guide (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "OPE",
            ////    FileName = "unifreight_mobile_user_guide_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature30.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "31",
            ////    Name = "Unifreight Mobile - Invitation and Sharing Data (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "OPE",
            ////    FileName = "unifreight_mobile_invitation_sharing_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature31.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "32",
            ////    Name = "Outlook Connection (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "outlook_connection_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature32.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "33",
            ////    Name = "Analyzing CRM Data (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "analyzing_crm_data_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature33.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "34",
            ////    Name = "Outlook Connection Installation (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "outlook_installation_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature34.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "35",
            ////    Name = "Managing Opportunities (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "managing_opportunities_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature35.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "36",
            ////    Name = "Unifreight CRM (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "unifreight_crm_r5_2015_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature36.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "37",
            ////    Name = "Unifreight CRM (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "unifreight_crm_r1_2016_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature37.Code
            ////}, helpResourceRepository, TenantHelpResources);

            ////AddHelpResources.AddHelpResource(new HelpResourceDetails()
            ////{
            ////    Code = "38",
            ////    Name = "Unifreight CRM (Hebrew)",
            ////    Language = "HE",
            ////    Type = "TUT",
            ////    Category = "CRM",
            ////    FileName = "unifreight_crm_r2_2016_hebrew.pdf",
            ////    IsNew = false,
            ////    FeatureCode = Feature38.Code
            ////}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "39",
            //    Name = "Logitude Outlook Connection Setup Guide",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "outlook_connection_setup_guide.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature39.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "40",
            //    Name = "Logitude Mobile",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "logitude_mobile.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature40.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "41",
            //    Name = "Shared Logistics & Mobile Setup",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "shared_logistics_mobile_setup.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature41.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "42",
            //    Name = "Analyzing CRM Data (French)",
            //    Language = "FR",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "analyzing_crm_data_french.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature42.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "43",
            //    Name = "Managing Opportunities  (French)",
            //    Language = "FR",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "managing_opportunities_french.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature43.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "44",
            //    Name = "Working With Activities (French)",
            //    Language = "FR",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "working_with_activities_french.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature44.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "45",
            //    Name = "Quotes Tutorial",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "quotes_tutorial.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature45.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "46",
            //    Name = "How to manage users (French)",
            //    Language = "FR",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "manage_users_french.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature46.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "47",
            //    Name = "QuickBooks Online Connection Setup and Activation",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "quick_books_connection.pdf",
            //    IsNew = true,
            //    FeatureCode = Feature47.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "48",
            //    Name = "VAT Type Management",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "vat_type_management.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature48.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "49",
            //    Name = "How to Build a Consolidation Shipment (Spanish)",
            //    Language = "SP",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "build_consolidation_spanish.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature49.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "50",
            //    Name = "Shared Logistics & Mobile (Spanish)",
            //    Language = "SP",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "shared_logistics_mobile_spanish.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature50.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "51",
            //    Name = "Follow Ups",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "follow_ups.pdf",
            //    IsNew = false,
            //    FeatureCode = Feature51.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "52",
            //    Name = "Shared Logistics with Agents",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "shared_logistics_with_agents.pdf",
            //    IsNew = false,
            //    //FeatureCode = Feature52.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "53",
            //    Name = "SAT Profact Connection",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "sat_profact_connection.pdf",
            //    IsNew = true,
            //    //FeatureCode = Feature53.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "54",
            //    Name = "SAT Profact Connection (Spanish)",
            //    Language = "SP",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "sat_profact_connection_spanish.pdf",
            //    IsNew = true,
            //    //FeatureCode = Feature54.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "55",
            //    Name = "CRM Quick Tour",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "CRM",
            //    FileName = "crm_quick_tour.pdf",
            //    IsNew = true,
            //    //FeatureCode = Feature55.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "56",
            //    Name = "How to use Percent of Freight and Percent of Value?",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "OPE",
            //    FileName = "use_percent_freight_value.pdf",
            //    IsNew = true,
            //    //FeatureCode = Feature56.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "57",
            //    Name = "Multiple VAT Management",
            //    Language = "EN",
            //    Type = "TUT",
            //    Category = "ACC",
            //    FileName = "multiple_vat_management.pdf",
            //    IsNew = true,
            //    //FeatureCode = Feature57.Code
            //}, helpResourceRepository, TenantHelpResources);
            
            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL001",
            //    Name = "December 2015 - Version R5.15",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "december_2015_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR01.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL002",
            //    Name = "February 2016 - Version R1.16",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "february_2016_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR02.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL003",
            //    Name = "May 2016 - Version R2.16",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "may_2016_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR03.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL004",
            //    Name = "July 2016 - Version R3.16",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "july_2016_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR04.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL005",
            //    Name = "October 2016 - Version R4.16",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "october_2016_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR05.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL006",
            //    Name = "December 2016 - Version R5.16",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "december_2016_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR06.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL007",
            //    Name = "Feb 2017 - Version R1.17",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "feb_2017_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR07.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL008",
            //    Name = "May 2017 - Version R2.17",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "may_2017_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR08.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL009",
            //    Name = "July 2017 - Version R3.17",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "july_2017_release.pdf",
            //    IsNew = false,
            //    FeatureCode = FeatureR09.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL010",
            //    Name = "October 2017 - Version R4.17",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "october_2017_release.pdf",
            //    IsNew = false,
            //    //FeatureCode = FeatureR10.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "REL011",
            //    Name = "December 2017 - Version R5.17",
            //    Language = "EN",
            //    Type = "REL",
            //    Category = "OPE",
            //    FileName = "december_2017_release.pdf",
            //    IsNew = true,
            //    //FeatureCode = FeatureR11.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "VID001",
            //    Name = "Introduction to Logitude",
            //    Language = "EN",
            //    Type = "VID",
            //    Category = "OPE",
            //    VideoURL = "http://youtu.be/l6U2TaBQGjs?hd=1",
            //    Duration = "2:31",
            //    FileName = "introduction_to_logitude",
            //    IsNew = false,
            //    FeatureCode = FeatureV01.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "VID002",
            //    Name = "Building a New Shipment",
            //    Language = "EN",
            //    Type = "VID",
            //    Category = "OPE",
            //    VideoURL = "http://youtu.be/Kl3i6XcWDw4?hd=1",
            //    Duration = "4:49",
            //    FileName = "building_shipment",
            //    IsNew = false,
            //    FeatureCode = FeatureV02.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "VID003",
            //    Name = "Issuing an Invoice",
            //    Language = "EN",
            //    Type = "VID",
            //    Category = "ACC",
            //    VideoURL = "http://youtu.be/Jb6UsZmHG90?hd=1",
            //    Duration = "2:25",
            //    FileName = "issuing_nvoice",
            //    IsNew = false,
            //    FeatureCode = FeatureV03.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "VID004",
            //    Name = "Business Tools",
            //    Language = "EN",
            //    Type = "VID",
            //    Category = "OPE",
            //    VideoURL = "http://youtu.be/0t_CgfOusxA?hd=1",
            //    Duration = "2:43",
            //    FileName = "business_tools",
            //    IsNew = false,
            //    FeatureCode = FeatureV04.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "VID005",
            //    Name = "Billing & Accounting Tools",
            //    Language = "EN",
            //    Type = "VID",
            //    Category = "ACC",
            //    VideoURL = "https://www.youtube.com/watch?v=wPbbpTyHf04&hd=1",
            //    Duration = "2:42",
            //    FileName = "accounting_tools",
            //    IsNew = false,
            //    FeatureCode = FeatureV05.Code
            //}, helpResourceRepository, TenantHelpResources);

            //AddHelpResources.AddHelpResource(new HelpResourceDetails()
            //{
            //    Code = "VID006",
            //    Name = "Logitude World e-AWB",
            //    Language = "EN",
            //    Type = "VID",
            //    Category = "AWB",
            //    VideoURL = "https://www.youtube.com/watch?v=rKaaKbxe0q4&hd=1",
            //    Duration = "2:35",
            //    FileName = "logitude_world_awb",
            //    IsNew = false,
            //    FeatureCode = FeatureV06.Code
            //}, helpResourceRepository, TenantHelpResources);

            //helpResourceRepository.SubmitChanges();
        }
    }
}