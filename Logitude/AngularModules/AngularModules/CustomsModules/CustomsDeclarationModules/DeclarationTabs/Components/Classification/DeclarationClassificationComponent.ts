declare var window;
import { Component, AfterViewInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, FontTool } from '../../../../../Infrastructure/Tools';
import { ItemCodeComponent } from '../../../../../Customsmodules/Customsdeclarationmodules/Declarationsupplierinvoice/Components/Supplierinvoices/SupplierInvoiceGeneralTabComponent';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';

import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';
import { SupplierInvoicePM } from '../../../../../Customs/EntityPMs/SupplierInvoicePM';

import { ClientList } from '../../../../../Customs/EntityLists/ClientList';

import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';

import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { CardPMService } from '../../../../../Common/Services/StandardPMs/CardPMService';
import { CustomsHouseTypeExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import { DeclarationEditComponentController } from '../../../../../Customs/Controller/DeclarationEditComponentController';

import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import { CustomsRequiredFieldListService } from '../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';
import { ApiQueryFilters, FilterItem } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsSettingExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { CustomsRequestMenuService } from '../../../../../Customs/Services/Others/CustomsRequestMenuService';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { SupplierInvoiceExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService';
import { GITITEMDto } from '../../../../../Customs/EntityPMs/Extended/GITITEMDto';
import { GITITEMExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/GITITEMExtendedPMService';
import { GITITEMCacheService } from '../../../../../Customs/Services/Others/GITITEMCacheService';

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationClassificationComponent.html',
})
//
export class DeclarationClassificationComponent extends BaseComponent implements OnDestroy {
    public EntityPM: DeclarationPM;
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: any = this;
    public CurrentEditComponentId: string;
    public IsDisplayOnly: boolean = false;
    public IsGetTableName: boolean = true;

    public IsImporerCodeEnabled: boolean = true;
    public IsTransferImporterEnabled: boolean = true;
    public IsEntitleImporterEnabled: boolean = true;
    public DisplayOnlyMessage: string = "";
    private cardService: CardPMService = new CardPMService;
    private customsHouseTypeExtendedPMService: CustomsHouseTypeExtendedPMService = new CustomsHouseTypeExtendedPMService;
    private declarationPMService: DeclarationPMService = new DeclarationPMService;
    public PreceduralFilterItems: ApiQueryFilters;
    public RefreshDatePicker: boolean;
    SInvoiceTabs: LogTab[] = [];
    public ShowStorageStatusMessage: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService) {
        super();
        this.PreceduralFilterItems = new ApiQueryFilters();
        this.PreceduralFilterItems.addAdditionalFilter("IsImport", true, null, null, "Equals", false, false, false, "boolean");

        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.ConsignmentPackage").subscribe(response => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.ConsignmentInternalTransition").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(response => {
                            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(response => {
                                this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(response => {
                                    this.EntityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(response => {
                                        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(response => {

                                            this.EntityPM = this.entityArgs.EntityPM;
                                            this.ObjectTableName = this.entityArgs.ObjectTableName;
                                            this.Listen();
                                            //var tab;
                                            console.log("DeclarationClassificationComponent/EntityPM ", this.EntityPM);
                                            if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
                                                // create consignment tabs from entity 
                                                //for (let item of this.EntityPM.Consignments) {

                                                //    tab = new LogTab();
                                                //    tab.EntityPM = item;
                                                //    tab.Code = item.SequenceNumeric.toString();
                                                //    tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
                                                //    tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/General/ConsigmentTabContent/ConsigmentTabContentComponent";
                                                //    this.ConsigmentTabs.push(tab);
                                                //}
                                                
                                                //this.checkImportersVisibility();
                                                this.DisplayOnlyCheck();
                                                this.CheckRequrierdFieldsForSend();
                                                
                                                this.BuildScreen();
                                            }

                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });

        //Disable fields
        if (this.IsDisplayOnly) {
            this.SetScreenFieldsEditability();
        }

    }

    //#region XML Errors
    
    IsWindowMode: boolean = false;

   
    ngOnDestroy() {
        console.log("DeclarationClassificationComponent:ngOnDestroy():ConsigmentTabs")
        this.SInvoiceTabs.forEach((tab) => {

            if (tab.ComponentReference && tab.ComponentReference.ngOnDestroy) {
                tab.ComponentReference.ngOnDestroy();

            }
            tab.ComponentReference = null;
        });
        this.SInvoiceTabs = null;
    }
    ForceSave: boolean = false;
    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.ForceSave = true;
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.SInvoiceTabs.forEach(
                        //    tab => {

                        //        (tab.Parent.AddEditSupplierInvoiceDUMMYManager as AddEditSupplierInvoiceDUMMY).
                        //            SaveItemCodeLocalCache();
                        //    });

                      GITITEMCacheService.Instance.SaveItemCodeLocalCache();
                        this.BuildScreen();                                 
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && this.CurrentSession.CurrentEditComponent) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.RefreshDatePicker = false;
                        

                        this.BuildScreen();
                        this.DisplayOnlyCheck();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DCCF") {
                            this.CurrentSession.StartBusyIndicatorLoading();
                            //this.RefreshEntity();
                            this.ForceSave = true;

                            this.SInvoiceTabs = [];
                            this.BuildScreen();

                            this.DisplayOnlyCheck();



                        } 
                        else {
                            if (this.ForceSave && this.EntityPM.IsDirty) {
                                this.CurrentSession.StartBusyIndicatorSaving();
                                this.CurrentSession.CurrentEditComponent.SaveAndCloseCompleted
                                    .subscribe(isSuccess => {
                                        if (isSuccess) {
                                            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                            this.CurrentSession.StopBusyIndicator();
                                        }

                                    });
                                this.CurrentSession.CurrentEditComponent.SaveChanges();
                            }
                            this.ForceSave = false;
                        }
                    }
                    
                })
            );
        }
    }

    SetScreenFieldsEditability() {


        this.UIProperties.SetEnabled("CasualSupplierName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("CalculatedImporterName", this.ObjectTableName, false);

        
        this.UIProperties.SetEnabled("IncotermCode", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("GrossMassMeasure", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("TotalInvoiceAmountInUSD", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("_InvoiceAmount", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("_PackageQuantity", this.ObjectTableName, false);

        this.UIProperties.SetEnabled("_FreightAmount", this.ObjectTableName, false);//41322
        this.UIProperties.SetEnabled("FreightAmount", this.ObjectTableName, false);//41322
        
        this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoDescription", this.ObjectTableName, !this.IsDisplayOnly);

        
        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
        this.IsTransferImporterEnabled = !this.IsDisplayOnly;
        this.IsEntitleImporterEnabled = !this.IsDisplayOnly;
        if (!this.IsDisplayOnly) {

            //this.IsImporerCodeEnabled = AppTool.IsNullOrEmpty(this.EntityPM.ImporterName) && AppTool.IsNullOrEmpty(this.EntityPM.ImporterAddress);
            if (!AppTool.IsNullOrEmpty(this.ImporterCode)) {
                this.IsImporerCodeEnabled = true;
                if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                    this.IsImporerCodeEnabled = false;
                }
            } 

        }

        
    }
    
    //#region Properties

    _GrossMassMeasure: string ="0.00";
    public get GrossMassMeasure() { return this._GrossMassMeasure; }


    _FreightAmount: string = "";//41322
    public get FreightAmount() { return this._FreightAmount; }

    _TotalInvoiceAmountInUSD: string = "";
    public get TotalInvoiceAmountInUSD() { return this._TotalInvoiceAmountInUSD; }
    public set TotalInvoiceAmountInUSD(newValue: string) {
        this._TotalInvoiceAmountInUSD = newValue;
        
    }
    public get DeclarationOfficeCode() { return this.EntityPM.DeclarationOfficeCode; }
    public set DeclarationOfficeCode(newValue: string) {
        this.EntityPM.DeclarationOfficeCode = newValue;
        this.ChangeTransportMode();
    }

    public get ProcedureCurrentCode() { return this.EntityPM.ProcedureCurrentCode; }
    public set ProcedureCurrentCode(newValue: string) {
        this.EntityPM.ProcedureCurrentCode = newValue;
    }

    public get DeclarationDocumentTypeCode() { return this.EntityPM.DeclarationDocumentTypeCode; }
    public set DeclarationDocumentTypeCode(newValue: string) { this.EntityPM.DeclarationDocumentTypeCode = newValue; }

    public DrawMe: boolean = true;


    public get ImporterCode() { return this.EntityPM.ImporterCode; }
    public set ImporterCode(newValue: string) {
        if (this.EntityPM.ImporterCode != newValue) {

            this.EntityPM.ImporterCode = newValue;
            this.EntityPM.ImporterTypeCode = "1";
            //this.EntityPM.ImporterTypeName = "IL";

            this.EntityPM.MainImporterEntitlemntTypeCode = null;
            this.EntityPM.ImporterAddress = null;
            this.EntityPM.ImporterPassportNumber = null;
            // this.EntityPM.ImporterName = null;
            this.EntityPM.ImporterPassCountryCode = null;


            let needTodELETE: boolean = false;
            if (!this.EntityPM.IsCourierDeclaration) {//due courier
                this.EntityPM.ImporterName = "";//

                this.EntityPM.CasualImporterAddress1 = "";
                this.EntityPM.CasualImporterAddress2 = "";
                this.EntityPM.CasualImporterCity = "";
                this.EntityPM.CasualImporterZipCode = "";
                this.EntityPM.CasualImporterFax = "";
                this.EntityPM.CasualImporterEmail = "";
                this.EntityPM.CasualImporterTel = "";
                this.EntityPM.CasualImporterContact = "";
            }
        }


    }

    ImporterLostFocusChange(type: any, item: any, importerSearchBox: any) {

        if (type == 'Importer' && !AppTool.IsNullOrEmpty(this.EntityPM.CustomerVatNo) && !AppTool.IsNullOrEmpty(item) && this.EntityPM.CustomerVatNo != item) {

            var confirmWindow = new ConfirmWindow(); confirmWindow.Width = 400;
            confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.VatChanged"));
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) { // YES
                    this.EntityPM.VatChanged = true;
                    this.ImporterCode = item;
                }
                if (confirmWindow.No) { // NO
                    SessionLocator.SustainFocusOnCell = true;
                    var element = document.getElementById(importerSearchBox.InputId);
                    if (element) {
                        element.focus();
                    }
                }
            });
        }
        else {
            switch (type) {
                case 'Importer': {
                    this.ImporterCode = item;
                    break;
                }

            }
        }
    }

    private _UnifreightCustomerDefualt: string = null;
    ImporterLostFocus4CourierDeclaration(type: any, item: any, importerSearchBox: any) {

        if (this._UnifreightCustomerDefualt == null) {
            var customsSettingExtendedListService: CustomsSettingExtendedListService = new CustomsSettingExtendedListService();
            customsSettingExtendedListService.GetDefault("ISRAEL", "CGO_CUST_CAS", "NON", "NON", this.EntityPM.Tenant)
                .subscribe((response: ServiceResponse) => {
                    let obj = response.Result;
                    if (obj) {
                        let DefaultValue = obj['DefaultValue'];
                        if (!AppTool.IsNullOrEmpty(DefaultValue)) {
                            this._UnifreightCustomerDefualt = DefaultValue;
                        }
                        if (this._UnifreightCustomerDefualt == this.EntityPM.CustomerCode) {
                            switch (type) {
                                case 'Importer': {
                                    this.ImporterCode = item;
                                    break;
                                }
                            }
                        }
                        else {
                            this.ImporterLostFocusChange(type, item, importerSearchBox);
                        }
                    }
                });
        }
        else {
            if (this._UnifreightCustomerDefualt == this.EntityPM.CustomerCode) {
                switch (type) {
                    case 'Importer': {
                        this.ImporterCode = item;
                        break;
                    }
                }
            }
            else {
                this.ImporterLostFocusChange(type, item, importerSearchBox);
            }
        }
    }

    ImporterLostFocus(item: any,importerSearchBox: any) {
        let type = 'Importer';
        if (this.isImporterClicked != true) {
            switch (type) {
                case 'Importer': {
                    this.EntityPM.ImporterId = "";
                    this.CalculatedImporterName = "";
                    break;
                }
              
            }
        }
        this.isImporterClicked = false;

        if (this.EntityPM.IsCourierDeclaration) {
            this.ImporterLostFocus4CourierDeclaration(type, item, importerSearchBox);
        }
        else {
            this.ImporterLostFocusChange(type, item, importerSearchBox);
        }
    }
    private isImporterClicked: boolean = false;

    ImporterClicked(client: ClientList) {
        let type = 'Importer';
        if (client) {
            this.isImporterClicked = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);

            switch (type) {
                case 'Importer': {
                    this.ImporterCode = client.Code;
                    this.EntityPM.ImporterId = client.Id;
                    this.CalculatedImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    break;
                }
               
            }
        }

    }
    EditCasualSupplier() {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
        SessionLocator.SelectedSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = "נתונים נוספים לספק";

        var logWindow = new LogitudeWindow();
        
        ///this.Type = "Importer";
        logWindow.Width = 550;
        logWindow.Height = 250;

        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Classification/CasualSupplierDetailsComponent');
    }
    EditImporter() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();

        this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");

        var logWindow = new LogitudeWindow();
        windowArgs.Type = "Importer";
        ///this.Type = "Importer";
        logWindow.Width = 550;
        logWindow.Height = this.EntityPM.IsCourierDeclaration ? 550 : 350;

        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');
    }
    SetFieldsDisabled(message: string) {
        this.SetScreenFieldsEditability()
        
    }

    public get CasualSupplierName() {
        
        return this.EntityPM.CasualSupplierName;

    }                                       

    public get CalculatedImporterName() {
        if (this.EntityPM.ImporterCode == null && this.EntityPM.ImporterName != null)
            return this.EntityPM.ImporterName;
        else
            return this.EntityPM.CalculatedImporterName;

    }
    public set CalculatedImporterName(newValue: string) { this.EntityPM.CalculatedImporterName = newValue; }

    public get TransferImporterCode() {
        return this.EntityPM.TransferImporterCode;
    }
    public set TransferImporterCode(newValue: string) {

        if (this.EntityPM.TransferImporterCode != newValue) {

            this.EntityPM.TransferImporterCode = newValue;


            this.EntityPM.TransferImporterTypeCode = "1";
            this.EntityPM.TransferImporterTypeName = "IL";

            this.EntityPM.TransImporterEntitleTypeCode = null;
            this.EntityPM.TransferImporterAddress = null;
            this.EntityPM.TransferPassportNumber = null;
            //     this.EntityPM.TransferImporterName = null;
            this.EntityPM.TransferImporterCountryCode = null;
        }
    }

    public get CalculatedTransferImporterName() {
        if (this.EntityPM.TransferImporterCode == null && this.EntityPM.TransferImporterName != null)
            return this.EntityPM.TransferImporterName;
        else
            return this.EntityPM.CalculatedTransferImporterName;
    }
    public set CalculatedTransferImporterName(newValue: string) { this.EntityPM.CalculatedTransferImporterName = newValue; }

    public get EntitleImporterCode() { return this.EntityPM.EntitleImporterCode; }
    public set EntitleImporterCode(newValue: string) {

        if (this.EntityPM.EntitleImporterCode != newValue) {
            this.EntityPM.EntitleImporterCode = newValue;

            this.EntityPM.EntitleImporterTypeCode = "1";
            this.EntityPM.EntitleImporterTypeName = "IL";
            this.EntityPM.ImporterEntitlementTypeCode = null;
            this.EntityPM.EntitleImporterAddress = null;
            this.EntityPM.EntitlePassportNumber = null;
            //   this.EntityPM.EntitleImporterName = null;
            this.EntityPM.EntitleImporterCountryCode = null;
        }
    }

    public get CalculatedEntitleImporterName() {
        if (this.EntityPM.EntitleImporterCode == null && this.EntityPM.EntitleImporterName != null)
            return this.EntityPM.EntitleImporterName;
        else

            return this.EntityPM.CalculatedEntitleImporterName;
    }
    public set CalculatedEntitleImporterName(newValue: string) { this.EntityPM.CalculatedEntitleImporterName = newValue; }

    public get EntitleImporterCountryCode() { return this.EntityPM.EntitleImporterCountryCode; }
    public set EntitleImporterCountryCode(newValue: string) { this.EntityPM.EntitleImporterCountryCode = newValue; }

    public get ImporterPassCountryCode() { return this.EntityPM.ImporterPassCountryCode; }
    public set ImporterPassCountryCode(newValue: string) { this.EntityPM.ImporterPassCountryCode = newValue; }

    public get EntitleImporterCountryName() { return this.EntityPM.EntitleImporterCountryName; }
    public set EntitleImporterCountryName(newValue: string) { this.EntityPM.EntitleImporterCountryName = newValue; }

    public CalculatedClient: any;
    supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();
    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;

        if (this.selectedTab) {
            var si = this.selectedTab.EntityPM  as SupplierInvoicePM;
            //var jsonSI = mySupplierInvoiceExtendedPMService.clone(si);
            //mySupplierInvoiceExtendedPMService.MapSupplierInvoiceItems(si, jsonSI, true);

            this.GetDocumentFilingId(si.InvoiceCounterKey);
            
        }

        //this.selectedTab.EntityPM.InvoiceCounterKey
    }

    //#endregion

    //#region DocumentFiling
    DocumentFilingId: string;
    GetDocumentFilingId(InvoiceCounterKey) {
        console.log(" --->> Getting related document filing ...");
        this.supplierInvoiceExtendedPMService.GetDocumentFilingIdForForInvoice(this.EntityPM.Id, InvoiceCounterKey).subscribe(response => {
            console.log("[Reponse] GetDocumentFilingIdForForInvoice: ", response);
            var result = response.Result;
            if (result) {
                this.DocumentFilingId = response.Result;
              console.log("sending document filing document filing ...");
              let mohammadAdviseNotItzik = true;
              if (mohammadAdviseNotItzik) {
                var t = setTimeout(() => {
                  DeclarationEventManager.DeclarationSplitDocumentSelection.emit(this.DocumentFilingId);;
                  clearTimeout(t);
                }, 400);
              } else {
                DeclarationEventManager.DeclarationSplitDocumentSelection.emit(this.DocumentFilingId);;
              }
                

                //(new MessageWindow()).Show("document filing found: " + this.DocumentFilingId);
            }
            else
                console.log("[!] No related document filing found!!");


        });
    }

    //#region Tabs Component code
    consignmentIndex: number = 0;
    consignmentNumber: number = 0;

   
    OnSelectedChanged(tab: LogTab) {
        if (!AppTool.IsNullOrEmpty(tab)) {
            this.SelectedTab = tab;
            console.log("Tab selected: ", tab);
        }
    }
    //#endregion

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }
    ChangeTransportMode() {
        if (!AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {
            this.customsHouseTypeExtendedPMService.GetHouseTypewithAdditional(this.DeclarationOfficeCode).subscribe((result: ServiceResponse) => {
                if (!AppTool.IsNullOrEmpty(result.Result)) {

                    var houseType = result.Result;
                    if (!AppTool.IsNullOrEmpty(houseType)) {
                        this.EntityPM.TransportModeId = houseType.TransportModeId;
                        console.log("...TransportModeId changed to ", houseType.TransportModeId);
                    }

                }
            });
        }
    }

    DisplayOnlyCheck() {
    
        this.DrawMe = true;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
        }
        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: any) => {
            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (this.EntityPM.StorageStatusCode) {
                this.ShowStorageStatusMessage = true;
                this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
            }
            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
        });
    }
    _CargoDescription;
    public get CargoDescription() { return this._CargoDescription; }
    public set CargoDescription(value) {

        if (this._CargoDescription != value) {
            this._CargoDescription = value;
            this.EntityPM.Consignments.forEach(r => {
                r.CargoDescription = this._CargoDescription;
            });
        }
    }
    
    
    _IncotermCode;
    public get IncotermCode() { return this._IncotermCode ; }
    BuildScreen() {
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.CalcFields();
        this.BuildSInvoiceTabs();
        //this.cd.detectChanges();
    }
    _PackageQuantity: number;
    _InvoiceAmount: any;

    CalcFields() {

        let MyPrimarySupplierInvoice: SupplierInvoicePM
        if (this.EntityPM.SupplierInvoices.length > 0) {
            MyPrimarySupplierInvoice = this.EntityPM.SupplierInvoices.filter(r => r.IsPrimarySupplierInvoice == true)[0];
            if (!AppTool.IsNullOrEmpty(MyPrimarySupplierInvoice)) {
                this._IncotermCode = MyPrimarySupplierInvoice.IncotermCode;
            }
        }
        
        let _My1stConsignmentPM: ConsignmentPM;
        if (this.EntityPM.Consignments.length > 0) {
            _My1stConsignmentPM = this.EntityPM.Consignments[0];
        } else {
            this.EntityPM.AddConsignment(new ConsignmentPM(this.EntityPM));;
            _My1stConsignmentPM = this.EntityPM.Consignments[0];
        }
        this.CargoDescription = _My1stConsignmentPM.CargoDescription;

        let gross = 0;
        this._PackageQuantity = 0;
        for (let c of this.EntityPM.Consignments) {
            for (let cmqa of c.ConsignmentPackages.filter(r => r.PackageMeasureQualifierCode == "2")) {
                this._PackageQuantity += cmqa.PackageQuantity;
                gross += cmqa.GrossMassMeasure;
            }
        }
        this._GrossMassMeasure = Number(gross).toFixed(2);

        let SupplierInvoicesWithInvoiceAmount = this.EntityPM.SupplierInvoices.filter(r => !AppTool.IsNullOrZero(r.InvoiceAmount));

        var ObjectThatEachPropertyIsArray =
            ArrayTool.GroupIt(
                SupplierInvoicesWithInvoiceAmount,
                (item: SupplierInvoicePM) => { return item.InvoiceCurrencyTypeCode }
            );
        this._InvoiceAmount = null;
        if (Object.keys(ObjectThatEachPropertyIsArray).length == 1) {
            let invoiceAmount = ArrayTool.Sum(this.EntityPM.SupplierInvoices, "InvoiceAmount");
            let numberFix2 = Number(invoiceAmount).toFixed(2);
            //this._InvoiceAmount = numberFix2 as string;
            this._InvoiceAmount = this.EntityPM.SupplierInvoices[0].InvoiceCurrencyTypeCode + " " + numberFix2 as string ;//41322
        }

        let SupplierInvoicesWithFreight= this.EntityPM.SupplierInvoices.filter(r => !AppTool.IsNullOrZero(r.TotalFreightInFreightCurrency)); 
        //41322:
        var FreightCurrenciesArray =
            ArrayTool.GroupIt(
                SupplierInvoicesWithFreight,
                (item: SupplierInvoicePM) => { return item.FreightCurrencyTypeCode }
            );
        this._FreightAmount = null;
        if (Object.keys(FreightCurrenciesArray).length == 1) {
            let freightAmount = ArrayTool.Sum(this.EntityPM.SupplierInvoices, "TotalFreightInFreightCurrency");
            if (freightAmount != 0) {
                let freightFix2 = Number(freightAmount).toFixed(2);
                this._FreightAmount = this.EntityPM.SupplierInvoices[0].FreightCurrencyTypeCode + " " + freightFix2 as string; 
            }
        }
        else {
            let freightAmount = ArrayTool.Sum(this.EntityPM.SupplierInvoices, "TotalFreightInNIS");
            if (freightAmount != 0) {
                let freightFix2 = Number(freightAmount).toFixed(2);
                this._FreightAmount = "ILS " + freightFix2 as string;
            }
        }
        let InvoiceAmountInUSD = ArrayTool.Sum(this.EntityPM.SupplierInvoices, "InvoiceAmountInUSD");
        this._TotalInvoiceAmountInUSD = Number(InvoiceAmountInUSD).toFixed(2);
    }
    
    ///public ParentAddEditSupplierInvoiceDUMMY: AddEditSupplierInvoiceDUMMY = new AddEditSupplierInvoiceDUMMY();
    BuildSInvoiceTabs() {

        //this.ReconnectSII();
        

        
        this.SInvoiceTabs = [];
        for (let item of this.EntityPM.SupplierInvoices) {
            if (AppTool.IsNullOrEmpty(item.SequenceNumeric)) {
                continue;
            }
            var si = item as SupplierInvoicePM;
            //var jsonSI = mySupplierInvoiceExtendedPMService.clone(si);
            //mySupplierInvoiceExtendedPMService.MapSupplierInvoiceItems(si, jsonSI, true);
            
            var tab = new LogTab();
            tab.EntityPM = item;
            tab.Parent = {
                DeclarationPM: this.EntityPM,
                AddEditSupplierInvoiceDUMMYManager: new AddEditSupplierInvoiceDUMMY(this.EntityPM.CustomerCode)
            };
            tab.Code = item.SequenceNumeric.toString();
            tab.Header = (si.InvoiceNumber ? (item.InvoiceNumber + '-') : '') + item.SequenceNumeric;
          tab.ComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/Classification/SInvoiceClassificationTabComponent";
            this.SInvoiceTabs.push(tab);
        }
        if (this.SInvoiceTabs.length > 0) {
            this.SelectedTab = this.SInvoiceTabs[0];
        }
        this.CurrentSession.StopBusyIndicator();
    }

    ReconnectSII() {
        var mySupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService()
        //due 
        let reconnectedSupplierInvoices = new Array<SupplierInvoicePM>();
        for (let item of this.EntityPM.SupplierInvoices) {
            var si = item as SupplierInvoicePM;
            //var jsonSI = mySupplierInvoiceExtendedPMService.clone(si);
            //mySupplierInvoiceExtendedPMService.MapSupplierInvoiceItems(si, jsonSI, true);
            let newSIWithSIItemMap = mySupplierInvoiceExtendedPMService.MapJsonToEntityPM(si);

            reconnectedSupplierInvoices.push(newSIWithSIItemMap);

        }

        this.EntityPM.SupplierInvoices = null;
        this.EntityPM.SupplierInvoices = new Array<SupplierInvoicePM>();
        for (let connectedSI of reconnectedSupplierInvoices) {
            connectedSI.IsDirty = false;
            this.EntityPM.SupplierInvoices.push(connectedSI);

            for (let sii of connectedSI.SupplierInvoiceItems) {
                //connectedSI.PropertyChanged.subscribe
                //sii.PropertyChanged.subscribe(s => { this.EntityPM.IsDirty = true; });// should do cleanup ?!?!?
            }
        }
    }
    CheckRequrierdFieldsForSend() {
        var customsRequiredFieldListService: CustomsRequiredFieldListService = new CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(d => d.Name == 'Customs.Declaration')[0];
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        customsRequiredFieldListService.getAllFromCache(filters).subscribe((response: ServiceResponse) => {
            var requiredFields = response.Result;
            requiredFields.forEach((field) => {
                var objectField = window.ObjectFields.filter(d => d.Id == field.ObjectfieldId)[0];
                this.UIProperties.SetWarning(objectField.FieldName, 'Customs.Declaration', true);
            });
        });
    }
    get DifferenceColor() {
        if (this.SelectedTab == null) return null;
        return this.SelectedTab.Parent.AddEditSupplierInvoiceDUMMYManager.DifferenceColor;
    }
    get Difference() {
        if (this.SelectedTab == null) return null;
        return this.SelectedTab.Parent.AddEditSupplierInvoiceDUMMYManager.Difference;
    }
    get TotalForeignCurrency() {
        if (this.SelectedTab == null) return null;
        return this.SelectedTab.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency;
    }

    
}

export class AddEditSupplierInvoiceDUMMY {
  constructor(public declarationCustomerCode) { }

  public IsNewEntity = false;
  //#region properties
  ///public ItemCode_LocalCache: ItemCodeComponent[]=[];
  public DifferenceColor: string = "#282E30";

  private difference: number = 0;
  get Difference() { return this.difference; }
  set Difference(newValue: number) {
    if (this.difference != newValue) {
      this.difference = newValue;

      if (this.TotalForeignCurrency != 0) {
        if (this.difference != null) {
          if (this.difference != 0) {
            this.DifferenceColor = FontTool.Red; //red
          }
          else {
            this.DifferenceColor = FontTool.Green; //green
          }
        }
      }
      else {
        this.DifferenceColor = FontTool.Black;
      }
    }
  }

  private totalForeignCurrency: number = 0;
  get TotalForeignCurrency() { return this.totalForeignCurrency; }
  set TotalForeignCurrency(newValue: number) {
    if (this.totalForeignCurrency != newValue) {
      this.totalForeignCurrency = newValue;
    }
  }

  //#endregion
  public GITITEMExtendedPMService: GITITEMExtendedPMService = new GITITEMExtendedPMService();

  //SaveItemCodeLocalCache() {
  //    if (this.ItemCode_LocalCache != null && this.ItemCode_LocalCache.length > 0) {
  //        for (let item of this.ItemCode_LocalCache) {
  //            if (item.IsNew) {
  //                var myGITITEMPM = new GITITEMDto();
  //                myGITITEMPM.PARTNERID = this.declarationCustomerCode;
  //                if (AppTool.IsNullOrEmpty(item.VendorNumber)) {
  //                    item.VendorNumber = "NULL";
  //                }
  //                myGITITEMPM.SAPAKID = item.VendorNumber;
  //                myGITITEMPM.ITEMNO = item.ItemCode;
  //                myGITITEMPM.PRATID = item.ClassificationCode;
  //                myGITITEMPM.NAMEENG = item.ItemDescription;
  //                myGITITEMPM.ORIGINCOUNTRY = item.OriginCountryCode;

  //                this.GITITEMExtendedPMService.insert(myGITITEMPM).subscribe(myResult => {
  //                    var mm: ServiceResponse = myResult;
  //                    if (!mm.HasError) {
  //                        //this.entity = mm.Result;
  //                    }
  //                });
  //            }
  //        }
  //    }
  //}
}
