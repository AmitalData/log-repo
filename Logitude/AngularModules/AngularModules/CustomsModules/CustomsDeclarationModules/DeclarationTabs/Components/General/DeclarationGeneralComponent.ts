declare var window;
import { Component, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { LuhnAlgorithm } from '../../../../../Customs/Utilities/LuhnAlgorithm';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { CardPMService } from '../../../../../Common/Services/StandardPMs/CardPMService';
import { CustomsHouseTypeExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import { CustomsRequiredFieldListService } from '../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomsSettingExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { DeclarationExportRecipientPM } from '../../../../../Customs/EntityPMs/DeclarationExportRecipientPM';
import { CustomsRequiredFieldExtendedListService } from '../../../../../Customs/Services/ExtendedLists/CustomsRequiredFieldExtendedListService';
import { DeclarationWebService, ExportStorageConnectToDeclaration } from 'Customs/Services/WebServices/DeclarationWebService';
import { ClientIndicationListService } from 'Customs/Services/StandardLists/ClientIndicationListService';

@Component({

    templateUrl: './DeclarationGeneralComponent.html',
    styleUrls: ['./DeclarationGeneralComponent.scss'],
    providers: [DeclarationExtendedListService],
})

export class DeclarationGeneralComponent extends BaseComponent implements OnDestroy {
    public EntityPM: DeclarationPM;
    public ObjectTableName: string = "Customs.Declaration";
    public DataContext: any = this;
    public CurrentEditComponentId: string;
    public IsDisplayOnly: boolean = false;
    public IsDisplayMessage: boolean = false;

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
    ConsigmentTabs: LogTab[] = [];
    public ShowStorageStatusMessage: boolean;
    _DeclarationExportRecipientPM: DeclarationExportRecipientPM[] = [];
    RecipientList: DeclarationExportRecipientModel[];
    AddRecipientEnabled: boolean;
    public DeclarationExportRecipientTableName: string = "Customs.DeclarationExportRecipient";
    private CurrentSession = SessionLocator.SelectedSession;
    exportStorageConnectToDeclaration: ExportStorageConnectToDeclaration = null as any;
    clientIndicationListService: ClientIndicationListService = new ClientIndicationListService();
    clientIndications:any
    _IsIndicationsClientFeature:boolean = false;
    _ShowExcludeManifestFeature:boolean = false;

    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef, private EntityResourceService: EntityResourceService, public declarationExtendedListService: DeclarationExtendedListService) {
        super();

        this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.ConsignmentPackage").subscribe((response: any) => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.ConsignmentInternalTransition").subscribe((response: any) => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe((response: any) => {
                            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
                                this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe((response: any) => {
                                    this.EntityResourceService.getEntityResourceByTableName("Customs.Client").subscribe((response: any) => {
                                        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe((response: any) => {
                                            this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationExportRecipient").subscribe((response: any) => {
                                                this.EntityResourceService.getEntityResourceByTableName("Customs.ClientIndication").subscribe((response: any) => {
                                                
                                                this.EntityPM = this.entityArgs.EntityPM;
                                                this.ObjectTableName = this.entityArgs.ObjectTableName;
                                                this.Listen();
                                                //var tab;
                                                console.log("DeclarationGeneralComponent/EntityPM ", this.EntityPM);
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
                                                    this.BuildConsignments();

                                                    this.checkImportersVisibility();
                                                    this.DisplayOnlyCheck();
                                                    this.BuildRecipientsList();

                                                    this.CheckRequrierdFieldsForSend();
                                                    this.getExportStorageData();
                                                    this.setRequiredTranssshipment();
                                                    this.PreceduralFilterItems = new ApiQueryFilters();
                                                    if (this.EntityPM.Direction != "E") {
                                                        this.PreceduralFilterItems.addAdditionalFilter("IsImport", true, null, null, "Equals", false, false, false, "boolean");

                                                    }
                                                    if (this.EntityPM.Direction == "E") {
                                                        this.CurrentSession.ChangeSessionHeader({ Text: TextCodeTranslator.Translate("Customs.Declaration.O.ExportDeclarations") });
                                                    } else {
                                                        this.CurrentSession.ChangeSessionHeader({ Text: TextCodeTranslator.Translate("Customs.Declaration.O.ImportDeclarations") });
                                                    }
                                                    if (!AppTool.IsNullOrEmpty(this.EntityPM.ImporterId)) {               
                                                        this.getClientIndication();
                                                     }
                                                    var table = window.ObjectTables.filter(d => d.Name === 'Customs.ClientIndication')[0];
                                            
                                                    var IsIndicationsClientFeature = FeatureLocator.Features.filter(f => (f.Code == "IndicationsClient") && f.ObjectTableId == table.Id)[0];
                                                    if (IsIndicationsClientFeature) {
                                                        this._IsIndicationsClientFeature=true
                                                    }
                                                    var tableDec = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

                                                    var IsExcludeManifestFeature = FeatureLocator.Features.filter(f => (f.Code == "ISEXCLUDEMANIFEST") && f.ObjectTableId == tableDec.Id)[0];
                                                    if (IsExcludeManifestFeature) {
                                                          this._ShowExcludeManifestFeature= true;
                                                    }
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
            });
        });

        //Disable fields
        if (this.IsDisplayOnly) {
            this.SetScreenFieldsEditability();
        }

    }

    AddRecipientButtonClicked() {

        if (this.AddRecipientEnabled && !this.IsDisplayOnly) {


            var lineNumber: number = 1;

            if (this.EntityPM.DeclarationExportRecipients.length != 0) {

                var maxObj = this.EntityPM.DeclarationExportRecipients.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current });
                if (maxObj != null) {
                    if (lineNumber <= maxObj.LineNumber)
                        lineNumber = maxObj.LineNumber;
                }

                lineNumber = lineNumber + 1;
            }

            var item: DeclarationExportRecipientPM = new DeclarationExportRecipientPM(this.EntityPM);
            item.DeclarationId = this.EntityPM.Id;
            item.Tenant = this.EntityPM.Tenant;
            item.LineNumber = lineNumber;

            if (!this.EntityPM.DeclarationExportRecipients.includes(item)) {
                //this.EntityPM.ConsignmentInternalTransitions.push(item);
                this.EntityPM.AddDeclarationExportRecipient(item);
            }

            this.AddRecipientEnabled = false;
            this.BuildRecipientsList();
        }

    }

    BuildRecipientsList() {
        var count: number = 1;

        this.RecipientList = [];

        this.AddRecipientEnabled = true;
        for (var i = 0; i < this.EntityPM.DeclarationExportRecipients.length; i++) {
            var viewModel: DeclarationExportRecipientModel = new DeclarationExportRecipientModel(this.EntityPM.DeclarationExportRecipients[i], this, this.EntityResourceService);
            //  viewModel.TransitionNumber = i + 1; AddSiteButtonClicked
            if (viewModel.RecipientName == null) {
                this.AddRecipientEnabled = false;
            }
            this.RecipientList.push(viewModel);


        }

        if (this.RecipientList.length == 0) {
            var item = new DeclarationExportRecipientPM(this.EntityPM);
            var viewModel: DeclarationExportRecipientModel = new DeclarationExportRecipientModel(item, this, this.EntityResourceService);
            // viewModel.TransitionNumber
            this.RecipientList.push(viewModel);
            //  this.EntityPM.AddConsignmentInternalTransition(item);


            this.AddRecipientEnabled = false;
        }


    }




    //#region XML Errors
    XMLErrors: string[] = [];
    IsWindowMode: boolean = false;

    // used in show XML errors process in Customs Answers
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            if (!AppTool.IsNullOrEmpty(args.DeclarationError)) {


                console.log("Error", args.DeclarationError);
                this.entityArgs = args.entityArgs;
                this.EntityPM = args.entityPM;
                this.IsDisplayOnly = args.IsDisplayOnly;
                this.ObjectTableName = args.entityArgs.ObjectTableName;
                this.IsWindowMode = true;

                // initialize consignment tabs

                if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
                    this.BuildRecipientsList()

                    this.BuildConsignments();  // [!] in the pilot branch, you should enable this line to work!!
                    //this.ConsigmentTabs = [];
                    //// create consignment tabs from entity
                    //for (let item of this.EntityPM.Consignments) {
                    //    var tab;
                    //    tab = new LogTab();
                    //    tab.EntityPM = item;
                    //    tab.Parent = this.EntityPM;
                    //    tab.Code = item.SequenceNumeric.toString();
                    //    tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
                    //    tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/General/ConsigmentTabContent/ConsigmentTabContentComponent";
                    //    this.ConsigmentTabs.push(tab);
                    //}
                    this.checkImportersVisibility();
                    this.DisplayOnlyCheck();
                    this.getExportStorageData();

                }

                this.ShowXMLErrors(args.DeclarationError);

            } else if (!AppTool.IsNullOrEmpty(args.AmendmentView)) {
                console.log("[DeclarationGeneral] Amendment", args.AmendmentView);
                this.entityArgs = args.entityArgs;
                this.EntityPM = args.entityPM;
                this.IsDisplayOnly = args.IsDisplayOnly;
                this.ObjectTableName = args.entityArgs.ObjectTableName;
                this.IsWindowMode = true;

                // initialize consignment tabs
                if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
                    this.BuildConsignments();
                    this.BuildRecipientsList()
                    //// create consignment tabs from entity
                    //for (let item of this.EntityPM.Consignments) {
                    //    var tab;
                    //    tab = new LogTab();
                    //    tab.EntityPM = item;
                    //    tab.Code = item.SequenceNumeric.toString();
                    //    tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
                    //    tab.ComponentPath = "./Customs/Components/Declaration/EditTabs/General/ConsigmentTabContent/ConsigmentTabContentComponent";
                    //    this.ConsigmentTabs.push(tab);
                    //}
                    this.checkImportersVisibility();
                    this.DisplayOnlyCheck();
                    this.getExportStorageData();
                }

                this.ShowXMLCorrections(args.AmendmentView);

            }
        }
    }

    ShowXMLErrors(error) {

        if (error.EntityName.toLowerCase() == "declaration") {
            var currentError = error;
            if (!AppTool.IsNullOrEmpty(error.Field)) {
                this.UIProperties.SetValidity(error.Field, "Customs.Declaration", false, error.Description);
            }

            var errors = [];
            var xmlErrors = error.Description.split(/,|:/);
            for (var xmlError in xmlErrors) {
                errors.push(xmlErrors[xmlError]);
            }
            this.XMLErrors = errors;
        }
        else if (error.EntityName.toLowerCase() == "consignment") {

            for (var tab of this.ConsigmentTabs) {
                tab.DecErrors = error;

                currentError = error;
                if (!AppTool.IsNullOrEmpty(error.Field)) {

                    tab.EntityPM.UIProperties.SetValidity(error.Field, "Customs.Consignment", false, error.Description);

                }
                var errors = [];
                errors.push(error.Description);
            }
            this.XMLErrors = errors;
        }
    }

    ShowXMLCorrections(amendment) {
        if (amendment.EntityName.toLowerCase() == "declaration") {
            if (!AppTool.IsNullOrEmpty(amendment.Field)) {
                this.UIProperties.SetValidity(amendment.Field, "Customs.Declaration", false, amendment.ErrorType);
            }
            var errors = [];
            errors.push(amendment.ErrorType);
            this.XMLErrors = errors;
        }
        else if (amendment.EntityName.toLowerCase() == "consignment") {

            for (var tab of this.ConsigmentTabs) {
                tab.DecErrors = amendment;

                if (!AppTool.IsNullOrEmpty(amendment.Field)) {
                    tab.EntityPM.UIProperties.SetValidity(amendment.Field, "Customs.Consignment", false, amendment.Description);
                }
                var errors = [];
                errors.push(amendment.Description);
            }
            var errors = [];
            errors.push(amendment.ErrorType);
            this.XMLErrors = errors;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    OkButtonClicked() {
        if (!this.IsDisplayOnly) {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.declarationPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                var res = response.Result;
                this.CurrentSession.StopBusyIndicator();
                if (response.HasError) {
                    this.XMLErrors = [];
                    this.XMLErrors = response.ErrorsArray;
                } else {
                    this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    }
    //#endregion


    checkImportersVisibility() {
        if (this.IsDisplayOnly) return;

        if (this.EntityPM.IsCourierDeclaration) {
            this.IsImporerCodeEnabled = true;
            if (!AppTool.IsNullOrEmpty(this.ImporterCode)) {
                if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                    this.IsImporerCodeEnabled = false;
                }
            }
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
        } else {
            this.IsImporerCodeEnabled = AppTool.IsNullOrEmpty(this.EntityPM.ImporterName) && AppTool.IsNullOrEmpty(this.EntityPM.ImporterAddress);
            if (!AppTool.IsNullOrEmpty(this.ImporterCode)) {
                this.IsImporerCodeEnabled = true;
                if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                    this.IsImporerCodeEnabled = false;
                    //if (!AppTool.IsNullOrEmpty(this.ImporterCode))
                    //    if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                    //        this.IsImporerCodeEnabled = false;
                    this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
                    this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, true);
                }
            }
        }
        if (!AppTool.IsNullOrEmpty(this.TransferImporterCode))
            if (this.TransferImporterCode.includes("F") || this.TransferImporterCode.includes("P")) {
                this.IsTransferImporterEnabled = false;

            }

        if (!AppTool.IsNullOrEmpty(this.EntitleImporterCode))
            if (this.EntitleImporterCode.includes("F") || this.EntitleImporterCode.includes("P"))
                this.IsEntitleImporterEnabled = false;


    }

    ngOnDestroy() {
        console.log("DeclarationGeneralComponent:ngOnDestroy():ConsigmentTabs")
        this.ConsigmentTabs.forEach((tab) => {

            if (tab.ComponentReference && tab.ComponentReference.ngOnDestroy) {
                tab.ComponentReference.ngOnDestroy();

            }
            tab.ComponentReference = null;
        });
        this.ConsigmentTabs = null;
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildConsignments();
                        this.BuildRecipientsList()
                        this.getExportStorageData();

                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && this.CurrentSession.CurrentEditComponent) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.RefreshDatePicker = false;
                        if (this.timerToken) {
                            clearTimeout(this.timerToken);
                        }
                        this.timerToken = setTimeout(() => {
                            this.RefreshDatePicker = true;
                        }, 200);

                        this.BuildConsignments();
                        this.DisplayOnlyCheck();
                        this.BuildRecipientsList()
                        this.getExportStorageData();

                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "DEGC") {
                            this.CurrentSession.StartBusyIndicatorLoading();
                            //this.RefreshEntity();
                            this.checkImportersVisibility();
                            this.DisplayOnlyCheck();
                            this.CurrentSession.StopBusyIndicator();


                        }
                    }
                })
            );
        }
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("DeclarationOfficeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeclarationOfficeHandlerCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExportDeclarationOfficeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeclarationDocumentTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TaxationDateTime", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExportTaxationDateTime", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AutonomyRegionTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeclarationDocumentId", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExporterImporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ImporterName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferImporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransferExporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("EntitleImporterCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeclarationTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DestinationCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExportAutonomyRegionTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsExporterConfirmation", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientAddress", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientIssueCountryCode", this.ObjectTableName, !this.IsDisplayOnly);

        this.RecipientList?.forEach(element => {
            element.SetScreenFieldsEditability();
        });

        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
        this.IsTransferImporterEnabled = !this.IsDisplayOnly;
        this.IsEntitleImporterEnabled = !this.IsDisplayOnly;
        this.checkImportersVisibility();
    }

    //#region Properties
    public get DeclarationOfficeCode() { return this.EntityPM.DeclarationOfficeCode; }
    public set DeclarationOfficeCode(newValue: string) {
        if (this.EntityPM.DeclarationOfficeCode != newValue) {
            this.EntityPM.DeclarationOfficeCode = newValue;
            this.EntityPM.DeclarationOfficeHandlerCode = newValue;
            this.ChangeTransportMode();
        }
    }

    public get DeclarationOfficeHandlerCode() { return this.EntityPM.DeclarationOfficeCode; }
    public set DeclarationOfficeHandlerCode(newValue: string) {
        if (this.EntityPM.DeclarationOfficeHandlerCode != newValue) {
            this.EntityPM.DeclarationOfficeHandlerCode = newValue;
            this.EntityPM.DeclarationOfficeCode = newValue;
        }
    }


    public get ExportDeclarationOfficeCode() { return this.EntityPM.ExportDeclarationOfficeCode; }
    public set ExportDeclarationOfficeCode(newValue: string) {
        this.EntityPM.ExportDeclarationOfficeCode = newValue;
        this.setRequiredTranssshipment();
    }

    public get ProcedureCurrentCode() { return this.EntityPM.ProcedureCurrentCode; }
    public set ProcedureCurrentCode(newValue: string) {
        this.EntityPM.ProcedureCurrentCode = newValue;
    }

    public get DeclarationDocumentTypeCode() { return this.EntityPM.DeclarationDocumentTypeCode; }
    public set DeclarationDocumentTypeCode(newValue: string) { this.EntityPM.DeclarationDocumentTypeCode = newValue; }

    public DrawMe: boolean = true;
    public get TaxationDateTime() { return this.EntityPM.TaxationDateTime; }
    public set TaxationDateTime(newValue: Date) {
        if (this.EntityPM.TaxationDateTime != newValue) {
            this.EntityPM.TaxationDateTime = newValue;
        }
    }
    public get ExportTaxationDateTime() { return this.EntityPM.TaxationDateTime; }
    public set ExportTaxationDateTime(newValue: Date) {
        if (this.EntityPM.TaxationDateTime != newValue) {
            this.EntityPM.TaxationDateTime = newValue;
        }
    }

    public get AutonomyRegionTypeCode() { return this.EntityPM.AutonomyRegionTypeCode; }
    public set AutonomyRegionTypeCode(newValue: string) { this.EntityPM.AutonomyRegionTypeCode = newValue; }

    public get DeclarationDocumentId() { return this.EntityPM.DeclarationDocumentId; }
    public set DeclarationDocumentId(newValue: string) { this.EntityPM.DeclarationDocumentId = newValue; }

    public get ExporterImporterCode() { return this.EntityPM.ImporterCode; }
    public set ExporterImporterCode(newValue: string) {
        if (this.EntityPM.ExporterImporterCode != newValue) {
            this.EntityPM.ExporterImporterCode = newValue;
            this.EntityPM.ImporterCode = newValue;
        }
    }


    public get ImporterCode() { return this.EntityPM.ImporterCode; }
    public set ImporterCode(newValue: string) {
        if (this.EntityPM.ImporterCode != newValue) {

            this.EntityPM.ImporterCode = newValue;
            this.EntityPM.ExporterImporterCode = newValue;

            this.EntityPM.ImporterTypeCode = "1";
            this.EntityPM.ImporterTypeName = "IL";

            this.EntityPM.MainImporterEntitlemntTypeCode = null;
            //this.EntityPM.ImporterAddress = null;
            this.EntityPM.ImporterPassportNumber = null;
            // this.EntityPM.ImporterName = null;
            this.EntityPM.ImporterPassCountryCode = null;
            if (!this.EntityPM.IsCourierDeclaration) {
                this.EntityPM.ImporterName = "";//
                this.EntityPM.ImporterAddress = null;
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



        if (this.EntityPM.IsCourierDeclaration) {//Task 57181: מספר יבוםן - תצוגת מסך
            if (AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode) && !AppTool.IsNullOrEmpty(this.EntityPM.ImporterName)) { //Task 45507: (בלדרות) שינויים בלוגיקה של שדה מספר יבוםן 
                this.CalculatedImporterName = this.EntityPM.ImporterName;
            }
        } else {
            this.CalculatedImporterName = this.EntityPM.ImporterName;
        }

    }

    public get CalculatedImporterName() {
        if (this.EntityPM.ImporterCode == null && this.EntityPM.ImporterName != null)
            return this.EntityPM.ImporterName;
        else
            return this.EntityPM.CalculatedImporterName;

    }
    public set CalculatedImporterName(newValue: string) {
        this.EntityPM.CalculatedImporterName = newValue;
    }

    //_LoadingDateTime: any = null;
    //public get LoadingDateTime() {
    //    return this._LoadingDateTime;


    //}
    //public set LoadingDateTime(newValue: any) {
    //    this.EntityPM.LoadingDateTime = newValue;
    //    this._LoadingDateTime = newValue;
    //    if (DateTool.IsNullOrMinDateTime(newValue)) {
    //        this._LoadingDateTime = null;
    //    }

    //}

    public get LoadingDateTime() { return this.EntityPM.LoadingDateTime; }
    public set LoadingDateTime(newValue: Date) {
        this.EntityPM.LoadingDateTime = newValue;
    }

    public get TransferExporterCode() { return this.EntityPM.TransferImporterCode; }
    public set TransferExporterCode(newValue: string) {
        if (this.EntityPM.TransferExporterCode != newValue) {
            this.EntityPM.TransferExporterCode = newValue;
            this.EntityPM.TransferImporterCode = newValue;
        }
    }

    public get TransferImporterCode() {
        return this.EntityPM.TransferImporterCode;
    }
    public set TransferImporterCode(newValue: string) {

        if (this.EntityPM.TransferImporterCode != newValue) {

            this.EntityPM.TransferImporterCode = newValue;
            this.EntityPM.TransferExporterCode = newValue;

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



    public get ShipCode() { return this.EntityPM.ShipCode; }
    public set ShipCode(newValue: string) {
        this.EntityPM.ShipCode = newValue;
    }

    _CustomsShip: any;
    public get CustomsShip() { return this._CustomsShip; }
    public set CustomsShip(newValue: string) {
        this._CustomsShip;
    }


    public get ExportAutonomyRegionTypeCode() { return this.EntityPM.ExportAutonomyRegionTypeCode; }
    public set ExportAutonomyRegionTypeCode(newValue: string) {
        this.EntityPM.ExportAutonomyRegionTypeCode = newValue;
    }

    DestinationCountry: any;
    public get DestinationCountryCode() { return this.EntityPM.DestinationCountryCode; }
    public set DestinationCountryCode(newValue: string) {
        this.EntityPM.DestinationCountryCode = newValue;
        //if (newValue) {
        //    this.UIProperties.SetRequired("DestinationCountryCode", this.ObjectTableName, false);
        //}
        //else {
        //    this.UIProperties.SetRequired("DestinationCountryCode", this.ObjectTableName, true);
        //}
    }

    public get DeclarationTypeCode() { return this.EntityPM.DeclarationTypeCode }
    public set DeclarationTypeCode(newValue: string) {
        this.EntityPM.DeclarationTypeCode = newValue;
        this.setRequiredTranssshipment();
    }

    public get IsExporterConfirmation() { return this.EntityPM.IsExporterConfirmation; }
    public set IsExporterConfirmation(newValue: boolean) {
        this.EntityPM.IsExporterConfirmation = newValue;
        this.UIProperties.SetRequired("IsExporterConfirmation", this.ObjectTableName, false);
    }

    public get IsMoreConsignment1() { return this.EntityPM.Consignments.filter(x=>x.ConsignmentType=="E").length>1; }
    public get IsOceanExport() { return this.EntityPM.TransportModeId == 'O' && this.EntityPM.Direction == 'E' ; }


    public CalculatedClient: any;

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    //#endregion

    //#region Commands + Handlers
    private timerToken: any;
    private isImporterClicked: boolean = false;

    setRequiredTranssshipment() {

        if (this.EntityPM.Direction == 'E' && this.EntityPM.DeclarationTypeCode == '3' && this.ExportDeclarationOfficeCode == null) {

            this.UIProperties.SetWarning('ExportDeclarationOfficeCode', 'Customs.Declaration', true);



        }
        else {
            this.UIProperties.SetWarning('ExportDeclarationOfficeCode', 'Customs.Declaration', false);
        }



    }
    ImporterClicked(type, client: ClientList) {
        if (client) {
            this.isImporterClicked = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);

            switch (type) {
                case 'Importer': {
                    this.ImporterCode = client.Code;
                    this.EntityPM.ImporterId = client.Id;
                    //this.CalculatedImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    if (AppTool.IsNullOrEmpty(client)) {
                        this.CalculatedImporterName = this.EntityPM.ImporterName;
                    } else {
                        this.CalculatedImporterName = client.FullName;
                    }

                    if (!AppTool.IsNullOrEmpty(this.EntityPM.ImporterId)) {               
                       this.getClientIndication();
                    }

                    break;
                }
                case 'Transfer': {
                    this.TransferImporterCode = client.Code;
                    this.EntityPM.TransferImporterId = client.Id;
                    this.CalculatedTransferImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    break;
                }
                case 'Entitle': {
                    this.EntitleImporterCode = client.Code;
                    this.EntityPM.EntitleImporterId = client.Id;
                    this.CalculatedEntitleImporterName = AppTool.IsNullOrEmpty(client) ? "" : client.FullName;
                    this.CalculatedClient = AppTool.IsNullOrEmpty(client) ? null : client;

                    this.EntitleImporterCountryCode = AppTool.IsNullOrEmpty(this.CalculatedClient) ? null : this.CalculatedClient.PassportCountryCode;
                    this.EntitleImporterCountryName = AppTool.IsNullOrEmpty(this.CalculatedClient) ? null : this.CalculatedClient.PassportCountryName;
                    break;
                }
            }
        }

    }


    // save previuos importers
    PreviusImporterCode: string;
    PreviusCalculatedImporterName: string;
    PreviusTransferImporterCode: string;
    PreviusTransferCalculatedImporterName: string;
    PreviusEntitleImporterCode: string;
    PreviusEntitleCalculatedImporterName: string;
    PreviusEntitleImporterCountryCode: string;
    PreviusEntitleImporterCountryName: string;
    PreviusCalculatedClient: string;

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
                case 'Transfer': {
                    this.TransferImporterCode = item;
                    break;
                }
                case 'Entitle': {
                    this.EntitleImporterCode = item;
                    break;
                }
            }
        }
    }

    ImporterLostFocus(type: any, item: any, importerSearchBox: any) {

        if (this.isImporterClicked != true) {
            switch (type) {
                case 'Importer': {
                    this.EntityPM.ImporterId = null;
                    //this.CalculatedImporterName = "";
                    if (AppTool.IsNullOrEmpty(this.EntityPM.ImporterCode)) {
                        this.CalculatedImporterName = this.EntityPM.ImporterName;
                    }
                    break;
                }
                case 'Transfer': {
                    this.EntityPM.TransferImporterId = null;
                    this.CalculatedTransferImporterName = "";
                    break;
                }
                case 'Entitle': {
                    this.EntityPM.EntitleImporterId = null;
                    this.CalculatedEntitleImporterName = "";
                    this.CalculatedClient = null;

                    this.EntitleImporterCountryCode = null;
                    this.EntitleImporterCountryName = null;
                    break;
                }
            }
        }
        this.isImporterClicked = false;

        var valid: boolean = true;
        var errorMessage: string = "";
        if (type == "Importer" && !AppTool.IsNullOrEmpty(item)) {
            this.ImporterCode = item;
            if (item.length < 9) {
                valid = false;
                errorMessage = this.EntityPM.Direction !='E' ? TextCodeTranslator.Translate("Customs.Declaration.O.ImporterIsTooShort") : TextCodeTranslator.Translate("Customs.Declaration.O.ExporterIsTooShort");
                //this.UIProperties.SetValidity("ImporterCode", "Customs.Declaration", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
            }
            else if (item.length > 9) {
                valid = false;
                errorMessage = this.EntityPM.Direction !='E' ? TextCodeTranslator.Translate("Customs.Declaration.O.TooLongCode") : TextCodeTranslator.Translate("Customs.Declaration.O.TooLongExporterCode");
                //this.UIProperties.SetValidity("ImporterCode", "Customs.Declaration", false, TextCodeTranslator.Translate("Customs.Declaration.O.TooLongCode"));
            }
            else {
                var digit: string = item.toString().substring(8);
                var checkDigit: number = LuhnAlgorithm.CalculateLuhnAlgorithm(item.substring(0, 8));

                if (digit != checkDigit.toString()) {
                    valid = false;
                    errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString();
                    //this.UIProperties.SetValidity("ImporterCode", "Customs.Declaration", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + checkDigit.toString());
                }
            }
        }

        if (!valid) {
            var messageWindow = new MessageWindow();
            messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show(errorMessage);
            return;
        }
        else {
            if (this.EntityPM.IsCourierDeclaration) {
                this.ImporterLostFocus4CourierDeclaration(type, item, importerSearchBox);
            }
            else {
                this.ImporterLostFocusChange(type, item, importerSearchBox);
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
                                case 'Transfer': {
                                    this.TransferImporterCode = item;
                                    break;
                                }
                                case 'Entitle': {
                                    this.EntitleImporterCode = item;
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
                    case 'Transfer': {
                        this.TransferImporterCode = item;
                        break;
                    }
                    case 'Entitle': {
                        this.EntitleImporterCode = item;
                        break;
                    }
                }
            }
            else {
                this.ImporterLostFocusChange(type, item, importerSearchBox);
            }
        }
    }

    ImporterTextChanged(type, item) {

        //if (AppTool.IsNullOrEmpty(item)) {
        switch (type) {
            case 'Importer': {
                if (!AppTool.IsNullOrEmpty(this.CalculatedImporterName)) {
                    this.PreviusImporterCode = this.ImporterCode;
                    this.PreviusCalculatedImporterName = this.CalculatedImporterName;
                }
                this.ImporterCode = item;
                this.EntityPM.ImporterId = null;

                if (this.EntityPM.ImporterName) {
                    this.CalculatedImporterName = this.EntityPM.ImporterName
                }
                else this.CalculatedImporterName = "";
                break;
            }
            case 'Transfer': {
                if (!AppTool.IsNullOrEmpty(this.CalculatedTransferImporterName)) {
                    this.PreviusTransferImporterCode = this.TransferImporterCode;
                    this.PreviusTransferCalculatedImporterName = this.CalculatedTransferImporterName;
                }

                this.TransferImporterCode = item;
                this.EntityPM.TransferImporterId = null;

                if (this.EntityPM.TransferImporterName) {
                    this.CalculatedTransferImporterName = this.EntityPM.TransferImporterName;
                }
                else this.CalculatedTransferImporterName = "";
                break;
            }
            case 'Entitle': {
                if (!AppTool.IsNullOrEmpty(this.CalculatedEntitleImporterName)) {
                    this.PreviusEntitleImporterCode = this.EntitleImporterCode;
                    this.PreviusEntitleCalculatedImporterName = this.CalculatedEntitleImporterName;
                    this.PreviusEntitleImporterCountryCode = this.EntitleImporterCountryCode;
                    this.PreviusEntitleImporterCountryName = this.EntitleImporterCountryName;
                    this.PreviusCalculatedClient = this.CalculatedClient;
                }

                this.EntitleImporterCode = item;
                this.EntityPM.EntitleImporterId = null;
                this.EntitleImporterCountryCode = null;
                this.EntitleImporterCountryName = null;
                if (this.EntityPM.EntitleImporterName) {
                    this.CalculatedEntitleImporterName = this.EntityPM.EntitleImporterName;
                }
                else this.CalculatedEntitleImporterName = "";
                break;
            }
        }
        //}

    }


    Type: string = null;
    EditImporter() {

        this.CurrentSession.StartBusyIndicatorLoading();

        if(this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty)
        {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
            const unsub = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            this.OpenImporterDetailsComponent();
            unsub.unsubscribe();

            }); 
        }

        else
            this.OpenImporterDetailsComponent();
       
    }

    OpenImporterDetailsComponent(){

        this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        var windowTitle;

        if (this.EntityPM.Direction != 'E')
            windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");
        else
            windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ExporterDetails");


        var logWindow = new LogitudeWindow();
        windowArgs.Type = "Importer";
        this.Type = "Importer";
        logWindow.Width = 550;
        logWindow.Height = this.EntityPM.IsCourierDeclaration ? 550 : 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');

    }

    public FeatureLocatorEXPORTDECLARATIONPSCREEN = FeatureLocator.HasFeaturePermession(this.ObjectTableName, "EXPORTDECLARATIONPSCREEN")
    public VisibleExportDecScreen: boolean;
    //public get VisibleExportDecScreen(): boolean{
    //    if (AppTool.IsNullOrEmpty(this.EntityPM)) {
    //        return false;
    //    }
    //    return this.EntityPM.Direction == "E" && FeatureLocator.HasFeaturePermession(this.ObjectTableName, "EXPORTDECLARATIONPSCREEN");
    //}
    //public set VisibleExportDecScreen(newval: boolean) { }

    EditExportDecScreen() {

        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        var windowTitle = TextCodeTranslator.Translate("Customs.ExportDeclarationDataQuery.F.ExportDeclarationData");

        var logWindow = new LogitudeWindow();
        //windowArgs.Type = "Importer";
        //this.Type = "Importer";
        logWindow.Width = 1000;
        logWindow.Height = 250;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ExportDeclarationComponent');
    }

    showButtonSearchClient:boolean = false;
    SearchClient(type, item) {
        
        if (this.IsDisplayOnly && !this.showButtonSearchClient) {
            return;
        }

        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();

        var importerCode: string;
        var passportNumber: string;
        var passportTypeCode: string;
        var passportCountryCode: string;

        var isExternalId: boolean = true;
        var isPassport: boolean = false;
        switch (type) {
            case "Importer":
                importerCode = this.ImporterCode;
                if (!this.IsImporerCodeEnabled && !this.showButtonSearchClient) {
                    importerCode = "";
                    isExternalId = false;
                    isPassport = true;
                    passportNumber = this.EntityPM.ImporterPassportNumber;
                    passportCountryCode = this.EntityPM.ImporterPassCountryCode;
                    if (this.ImporterCode.includes("P")) {
                        passportTypeCode = "1";
                    }
                    else if (this.ImporterCode.includes("F")) {
                        passportTypeCode = "2";
                    }
                    if (this.EntityPM.ImporterTypeCode == "P") {
                        passportTypeCode = "1";
                    }
                }
                break;
        }
        var isExternalId: boolean = true;
        var isPassport: boolean = false;

        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        
        var logWindow = new LogitudeWindow();
        windowArgs.Mode = "DeclarationGeneralComponent";
        windowArgs.ImporterCode = importerCode;
        windowArgs.IsExternalId = isExternalId;
        windowArgs.IsPassport = isPassport;
        windowArgs.PassportNumber = passportNumber;
        windowArgs.PassportTypeCode = passportTypeCode;
        windowArgs.PassportCountryCode = passportCountryCode;
        logWindow.Width = 850;
        logWindow.Height = 820;
        logWindow.Title = TextCodeTranslator.Translate("Customs.General.O.ClientSearchByIDQuery");
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnCustomFilesScreenWindowClosed(type, $event));
        logWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/ClientSearchByIDComponent');

    }
    SearchImporter(type, item) {

        if (this.IsDisplayOnly) {
            return;
        }

        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();

        var importerCode: string;
        var passportNumber: string;
        var passportTypeCode: string;
        var passportCountryCode: string;

        var isExternalId: boolean = true;
        var isPassport: boolean = false;
        switch (type) {
            case "Importer":
                importerCode = this.ImporterCode;
                if (!this.IsImporerCodeEnabled) {
                    importerCode = "";
                    isExternalId = false;
                    isPassport = true;
                    passportNumber = this.EntityPM.ImporterPassportNumber;
                    passportCountryCode = this.EntityPM.ImporterPassCountryCode;
                    if (this.ImporterCode.includes("P")) {
                        passportTypeCode = "1";
                    }
                    else if (this.ImporterCode.includes("F")) {
                        passportTypeCode = "2";
                    }
                    if (this.EntityPM.ImporterTypeCode == "P") {
                        passportTypeCode = "1";
                    }
                }
                break;
            case "Transfer":
                importerCode = this.TransferImporterCode;
                if (!this.IsTransferImporterEnabled) {
                    isExternalId = false;
                    isPassport = true;

                    importerCode = "";
                    passportNumber = this.EntityPM.TransferPassportNumber;
                    passportCountryCode = this.EntityPM.TransferImporterCountryCode;
                    if (this.TransferImporterCode.includes("P")) {
                        passportTypeCode = "1";
                    }
                    else if (this.TransferImporterCode.includes("F")) {
                        passportTypeCode = "2";
                    }
                }
                break;
            case "Entitle":
                importerCode = this.EntitleImporterCode;
                if (!this.IsEntitleImporterEnabled) {
                    isExternalId = false;
                    isPassport = true;
                }
                break;
        }

        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle;
        if (this.EntityPM.Direction != "E")
            windowTitle = TextCodeTranslator.Translate("Customs.Vendor.O.NewClient");
        else
            windowTitle = TextCodeTranslator.Translate("Customs.Vendor.O.NewClientE");

        var logWindow = new LogitudeWindow();
        windowArgs.Mode = "DeclarationGeneralComponent";
        windowArgs.ImporterCode = importerCode;
        windowArgs.IsExternalId = isExternalId;
        windowArgs.IsPassport = isPassport;
        windowArgs.PassportNumber = passportNumber;
        windowArgs.PassportTypeCode = passportTypeCode;
        windowArgs.PassportCountryCode = passportCountryCode;

        logWindow.Width = 850;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.OnCustomFilesScreenWindowClosed(type, $event));
        logWindow.Show('./CustomsModules/CustomsClient/Components/NewClient/NewClientComponent');

    }

    OnCustomFilesScreenWindowClosed(type: string, arg: any) {
        if (!AppTool.IsNullOrEmpty(arg)) {

            switch (type) {
                case 'Importer': {
                    this.CalculatedImporterName = arg;
                    break;
                }
                case 'Transfer': {
                    this.CalculatedTransferImporterName = arg;
                    break;
                }
                case 'Entitle': {
                    this.CalculatedEntitleImporterName = arg;
                    break;
                }
            }

        }
    }


    EditTransferImporter() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();

        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;

        var windowTitle;
        if (this.EntityPM.Direction != 'E')
            windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");
        else
            windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ExporterDetails");

        var logWindow = new LogitudeWindow();
        windowArgs.Type = "Transfer";
        this.Type = "Transfer";
        logWindow.Width = 550;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');

    }

    EditEntitleImporter() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.CurrentSession.CurrentEditComponent.SaveChanges();
        this.CurrentSession.StopBusyIndicator();

        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ImporterDetails");

        var logWindow = new LogitudeWindow();
        windowArgs.Type = "Entitle";
        this.Type = "Entitle";
        logWindow.Width = 550;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ImporterDetails/ImporterDetailsComponent');


    }
    ShowClientIndication(item) {


        if (this.clientIndications == null || this.clientIndications.length == 0) {
            return;
        }

        var windowArgs: any = {};
        windowArgs.CustomerIndicationList =  this.clientIndications;
        windowArgs.IsClientIndication = true;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 470;
        logitudeWindow.Height = 520;
        logitudeWindow.IsShowCloseButton = false;
        logitudeWindow.Title =TextCodeTranslator.Translate("Customs.ClientIndication.O.IndicationClient"); 
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/CustomerIndicationComponent');

    }
    getClientIndication()
        {
           
            let filters = new ApiQueryFilters();
    
    
            filters.PageSize = 200;
            filters.PageIndex = 0;
            filters.GetAll = false;
            filters.GetCount = true;           
               
            filters.addAdditionalFilter("ClientId", this.EntityPM.ImporterId , null, null, "Equals", false, false, false, "string", false);
            filters.addAdditionalFilter("Tenant", this.EntityPM.Tenant, null, null, "Equals", true, false, false, "string");
           
    
            return this.clientIndicationListService.getByFilters(filters)
                .subscribe(r => {
                    this.clientIndications=r.Result
                });
             
        }
    SetFieldsDisabled(message: string) {
        if (message == "ok") {
            if (this.Type == "Importer") {

                this.IsImporerCodeEnabled = false;

            }

            else if (this.Type == "Transfer") {
                this.IsTransferImporterEnabled = false;
            }

            else if (this.Type == "Entitle") {
                this.IsEntitleImporterEnabled = false;
            }
        }
        else if (message == "!ok") {

            if (this.Type == "Importer") {

                this.IsImporerCodeEnabled = true;

            }

            else if (this.Type == "Transfer") {
                this.IsTransferImporterEnabled = true;
            }

            else if (this.Type == "Entitle") {
                this.IsEntitleImporterEnabled = true;
            }
        }
        this.checkImportersVisibility()
    }

    //#endregion

    //#region Tabs Component code
    consignmentIndex: number = 0;
    consignmentNumber: number = 0;

    AddConsigment(event) {
        if (this.IsDisplayOnly) {
            return;
        }
        //console.log("==>> add clicked");

        this.consignmentIndex = 0;
        this.consignmentNumber = 0;

        if (this.EntityPM.Consignments.length > 0) {
            var maxObj = this.EntityPM.Consignments.reduce(function (prev, current) { return (prev.SequenceNumeric > current.SequenceNumeric) ? prev : current });

            if (this.EntityPM.Direction == "E"){
                var consignmentsSameType = this.EntityPM.Consignments.filter(x => x.ConsignmentType == "E");
                if (consignmentsSameType.length > 0) {
                    maxObj = consignmentsSameType.reduce(function (prev, current) { return (prev.SequenceNumeric > current.SequenceNumeric) ? prev : current });
                }

            }

            if (maxObj != null) {
                if (this.consignmentIndex <= maxObj.SequenceNumeric)
                    this.consignmentIndex = maxObj.SequenceNumeric;
            }

            var maxObj = this.EntityPM.Consignments.reduce(function (prev, current) { return (prev.ConsignmentNumber > current.ConsignmentNumber) ? prev : current });
            if (maxObj != null) {
                if (this.consignmentNumber <= maxObj.ConsignmentNumber)
                    this.consignmentNumber = maxObj.ConsignmentNumber;
            }
        }
        // new consignment
        var consignment: ConsignmentPM = new ConsignmentPM(this.EntityPM);
        consignment.DeclarationId = this.EntityPM.Id;
        consignment.Tenant = SessionLocator.Tenant;
        consignment.IsLastReleaseFromWarehous = "F";
        if (this.EntityPM.Direction == "E") {
            consignment.ConsignmentType = "E";
        }
        consignment.SequenceNumeric = ++this.consignmentIndex;
        consignment.ConsignmentNumber = ++this.consignmentNumber;
        this.EntityPM.AddConsignment(consignment);

        // new tab
        var tab = new LogTab();
        tab.EntityPM = consignment;
        tab.Parent = this.EntityPM;
        tab.Code = this.EntityPM.Direction == "E" ? consignment.ConsignmentNumber.toString() : consignment.SequenceNumeric.toString();
        tab.Header = consignment.SequenceNumeric.toString();
        tab.ComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ConsigmentTabContent/ConsigmentTabContentComponent";
        this.ConsigmentTabs.push(tab);

        // select the tab
        this.SelectedTab = tab;

        DeclarationEventManager.ConsignmentsChanged.emit({});
    }
    DeleteConsigment(tab: LogTab) {
        if (!AppTool.IsNullOrEmpty(tab)) {

            let msg: string = TextCodeTranslator.Translate("Customs.Declaration.O.DeleteConsignment");
            if (this.EntityPM.Direction === 'E' && this.EntityPM.TransportModeId === 'O' && tab.EntityPM.ExportStoragesId)
                msg = TextCodeTranslator.Translate("Customs.Declaration.O.ConnectedDelcaration") + '\n' + msg;

            var confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 300;
            confirmWindow.Height = 150;
            confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.Yes");
            confirmWindow.NoButtonText = TextCodeTranslator.Translate("Customs.Declaration.O.No");
            confirmWindow.Show(msg);
            var t = tab;
            confirmWindow.WindowClosed.subscribe((event: any) => {

                if (confirmWindow.Yes) { // YES
                    tab = t;
                    var index = this.ConsigmentTabs.indexOf(tab);
                    if (index < 0) {
                        console.log("The tab was not found, could not delete it :( ", tab); return;
                    }
                    this.EntityPM.RemoveConsignment(tab.EntityPM);
                    this.ConsigmentTabs.splice(index, 1);

                    if (this.EntityPM.AmendmentDontDisplayInList != true || this.EntityPM.Direction == "I") {
                        if (this.EntityPM.Direction == "E") {
                            //resequence consignments
                            var i_seq = 0, e_seq = 0;
                            for (var i = 0; i < this.EntityPM.Consignments.length; i++) {
                                var consignment = this.EntityPM.Consignments[i];
                                if (consignment.ConsignmentType == "I") {
                                    consignment.SequenceNumeric = i_seq + 1;
                                    i_seq++;
                                }
                                else {
                                    consignment.SequenceNumeric = e_seq + 1;
                                    e_seq++;
                                }
                            }
                            i_seq = 0;
                            e_seq = 0;
                            for (var i = 0; i < this.ConsigmentTabs.length; i++) {
                                var consignment: ConsignmentPM = this.ConsigmentTabs[i].EntityPM;
                                if (consignment.ConsignmentType == "I") {
                                    consignment.SequenceNumeric = i_seq + 1;
                                    i_seq++;
                                }
                                else {
                                    consignment.SequenceNumeric = e_seq + 1;
                                    e_seq++;
                                }
                                this.ConsigmentTabs[i].Code = (i + 1).toString();
                                this.ConsigmentTabs[i].Header = (consignment.ManifestNumber ? (consignment.ManifestNumber + '-') : '') + consignment.SequenceNumeric;
                            }
                        }
                        else {
                            //resequence consignments
                            for (var i = 0; i < this.EntityPM.Consignments.length; i++) {
                                var consignment = this.EntityPM.Consignments[i];
                                consignment.SequenceNumeric = i + 1;
                                //consignment.ConsignmentNumber = i + 1;
                            }
                            for (var i = 0; i < this.ConsigmentTabs.length; i++) {
                                var consignment: ConsignmentPM = this.ConsigmentTabs[i].EntityPM;
                                consignment.SequenceNumeric = i + 1;
                                this.ConsigmentTabs[i].Code = consignment.SequenceNumeric.toString();
                                this.ConsigmentTabs[i].Header = (consignment.ManifestNumber ? (consignment.ManifestNumber + '-') : '') + consignment.SequenceNumeric;
                            }
                        }
                        
                    }

                    // select the last tab
                    var tab = this.ConsigmentTabs[0];
                    this.SelectedTab = tab;

                    DeclarationEventManager.ConsignmentsChanged.emit({});

                }
            });

        }
    }
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
        if (!AppTool.IsNullOrEmpty(this.DeclarationOfficeCode) && this.EntityPM.Direction != "E") {
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
        this.IsDisplayMessage = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.EntityPM.AmendmentMessage != null && this.EntityPM.AmendmentMessage != "") {
            {
                this.IsDisplayMessage = true;
                this.DisplayOnlyMessage = this.EntityPM.AmendmentMessage;
                if (this.EntityPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.EntityPM.IsAmendmentDisplayOnly;
            }
        }
        else if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.DisplayOnly") + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            this.CurrentSession.CurrentEditComponent.DisplayModeChanged.emit(this.IsDisplayOnly);
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.StorageRequestStatus") + " " + this.EntityPM.StorageStatusName;
        }




        var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: any) => {

            var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
            this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (this.EntityPM.AmendmentMessage != null && this.EntityPM.AmendmentMessage != "") {
                {
                    this.IsDisplayMessage = true;

                    this.DisplayOnlyMessage = this.EntityPM.AmendmentMessage;
                    if (this.EntityPM.IsAmendmentDisplayOnly) this.IsDisplayOnly = this.EntityPM.IsAmendmentDisplayOnly;
                }
            }
            else if (this.IsDisplayOnly) {
                this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.DisplayOnly") + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (this.EntityPM.StorageStatusCode) {
                this.ShowStorageStatusMessage = true;
                this.DisplayOnlyMessage = TextCodeTranslator.Translate("Customs.Declaration.O.StorageRequestStatus") + " " + this.EntityPM.StorageStatusName;
            }

            else if (this.CurrentSession.CurrentEditComponent.EditComponentController.IsInBatchRequest && !displayOnlyCheckResult.IsDisplayOnly) {
                this.DisplayOnlyMessage = displayOnlyCheckResult.DisplayOnlyMessage;
                this.IsDisplayMessage = true;
            }

            this.SetScreenFieldsEditability();
            DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            this.CurrentSession.CurrentEditComponent.DisplayModeChanged.emit(this.IsDisplayOnly);

        });

        if((this.EntityPM?.IsSubmitDeclaration && this.EntityPM?.Direction === "E" ) || this.EntityPM?.PaymentDate){
            this.showButtonSearchClient = true;
        }
        else{
            this.showButtonSearchClient = false;
        }
    }

    BuildConsignments() {
        this.ConsigmentTabs = [];
        for (let item of this.EntityPM.Consignments) {

            var tab = new LogTab();
            tab.EntityPM = item;
            tab.Parent = this.EntityPM;
            tab.Code = this.EntityPM.Direction == "E" ? item.ConsignmentNumber.toString() :item.SequenceNumeric.toString();
            tab.Header = (item.ManifestNumber ? (item.ManifestNumber + '-') : '') + item.SequenceNumeric;
            if (this.EntityPM.Direction == 'E' && this.EntityPM.TransportModeId == 'O' && !AppTool.IsNullOrEmpty(item.ConsignmentPackages[0]?.MarksNumbers)) {

                tab.Title = (item.ConsignmentPackages[0].MarksNumbers + '-') + item.SequenceNumeric;
            }

            tab.ComponentPath = "./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ConsigmentTabContent/ConsigmentTabContentComponent";
            this.ConsigmentTabs.push(tab);
        }
        if (this.ConsigmentTabs.length > 0) {
            this.SelectedTab = this.ConsigmentTabs[0];
        }
    }

    CheckRequrierdFieldsForSend() {
        var isExport = false;
        if (this.EntityPM.Direction == 'E') {
            isExport = true;
        }

        var customsRequiredFieldListService: CustomsRequiredFieldListService = new CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(d => d.Name == 'Customs.Declaration')[0];
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");

        var customsRequiredFieldExtendedListService: CustomsRequiredFieldExtendedListService = new CustomsRequiredFieldExtendedListService();
        filters = customsRequiredFieldExtendedListService.GetFilter(filters, isExport)


        customsRequiredFieldListService.getAllFromCache(filters).subscribe((response: ServiceResponse) => {
            var requiredFields = response.Result;
            requiredFields.forEach((field) => {
                var objectField = window.ObjectFields.filter(d => d.FieldCode == field.ObjectfieldCode)[0];
                var isExporterInShortProc = objectField && objectField.Code == "ExporterImporterCode" && this.EntityPM.ShortProcedure;
                
                if (objectField && !isExporterInShortProc){
                    this.UIProperties.SetWarning(objectField.FieldName, 'Customs.Declaration', true);
                }
                    
            });
        }); 



    }


    async getExportStorageData() {
        if (this.EntityPM.TransportModeId == 'O') {
            this.exportStorageConnectToDeclaration = await new DeclarationWebService().getExportStorageConnectToDeclaration(this.EntityPM.Id);
        }
    }
    ComprehensiveUpdate(){
        var args: any = {
            EntityPM: this.EntityPM
        };
        var logWindow = new LogitudeWindow();
        logWindow.Width = 800;
        logWindow.Height = 220;
        logWindow.Title = TextCodeTranslator.Translate("Customs.Consignment.O.ComprehensiveUpdateCon");
        logWindow.WindowArgs = args;
        logWindow.ShowCloseButton = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ConsigmentTabContent/ComprehensiveUpdateConsignment');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        });
    }
}


export class DeclarationExportRecipientModel extends BaseComponent {
    public EntityPM: DeclarationExportRecipientPM;
    private Parent: DeclarationGeneralComponent;
    ObjectTableName: string = "Customs.DeclarationExportRecipient";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: DeclarationExportRecipientPM, parent: DeclarationGeneralComponent, public EntityResourceService: EntityResourceService) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationExportRecipient").subscribe((response: any) => {

            this.EntityPM = item;
            this.Parent = parent;
            this.UIProperties.SetEnabled("RecipientIssueCountryCode", this.ObjectTableName, !this.Parent.IsDisplayOnly);
            this.UIProperties.SetEnabled("RecipientAddress", this.ObjectTableName, !this.Parent.IsDisplayOnly);
            this.UIProperties.SetEnabled("RecipientName", this.ObjectTableName, !this.Parent.IsDisplayOnly);
            var customsRequiredFieldListService: CustomsRequiredFieldListService = new CustomsRequiredFieldListService();
            var table = window.ObjectTables.filter(d => d.Name == 'Customs.DeclarationExportRecipient')[0];
            var filters = new ApiQueryFilters();
            filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");

            var customsRequiredFieldExtendedListService: CustomsRequiredFieldExtendedListService = new CustomsRequiredFieldExtendedListService();
            filters = customsRequiredFieldExtendedListService.GetFilter(filters, true)


            customsRequiredFieldListService.getAllFromCache(filters).subscribe((response: ServiceResponse) => {
                var requiredFields = response.Result;
                requiredFields.forEach((field) => {
                    var objectField = window.ObjectFields.filter(d => d.FieldCode == field.ObjectfieldCode)[0];
                    this.UIProperties.SetWarning(objectField.FieldName, 'Customs.DeclarationExportRecipient', true);
                });
            });
        });


    }
    public SetScreenFieldsEditability(): void {
        this.UIProperties.SetEnabled("RecipientIssueCountryCode", this.ObjectTableName, !this.Parent.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientAddress", this.ObjectTableName, !this.Parent.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecipientName", this.ObjectTableName, !this.Parent.IsDisplayOnly);

    }

    //#region Properties
    deleteRecipientVisible: boolean = false;
    public get DeleteRecipientVisible() { return this.deleteRecipientVisible; }
    public set DeleteRecipientVisible(newValue: boolean) { this.deleteRecipientVisible = newValue; }

    public get Tenant() { return this.EntityPM.Tenant; }
    public set Tenant(newValue: number) { this.EntityPM.Tenant = newValue; }

    public get LineNumber() { return this.EntityPM.LineNumber; }
    public set LineNumber(newValue: number) { this.EntityPM.LineNumber = newValue; }

    public get DeclarationId() { return this.EntityPM.DeclarationId; }
    public set DeclarationId(newValue: string) { this.EntityPM.DeclarationId = newValue; }


    public get RecipientName() { return this.EntityPM.RecipientName; }
    public set RecipientName(newValue: string) {
        this.EntityPM.RecipientName = newValue;
        if (newValue != null) {
            if (this.Parent.RecipientList.length == 1) {
                this.Parent.EntityPM.AddDeclarationExportRecipient(this.EntityPM);
            }
            this.Parent.AddRecipientEnabled = true;
        }
        else {
            this.Parent.AddRecipientEnabled = false;
        }

    }

    public get RecipientAddress() { return this.EntityPM.RecipientAddress; }
    public set RecipientAddress(newValue: string) {
        this.EntityPM.RecipientAddress = newValue;
        if (newValue != null) {
            if (this.Parent.RecipientList.length == 1) {
                this.Parent.EntityPM.AddDeclarationExportRecipient(this.EntityPM);
            }
            this.Parent.AddRecipientEnabled = true;
        }
        else {
            this.Parent.AddRecipientEnabled = false;
        }
    }

    public get RecipientIssueCountryCode() { return this.EntityPM.RecipientIssueCountryCode; }
    public set RecipientIssueCountryCode(newValue: string) {
        this.EntityPM.RecipientIssueCountryCode = newValue;
        if (newValue != null) {
            if (this.Parent.RecipientList.length == 1) {
                this.Parent.EntityPM.AddDeclarationExportRecipient(this.EntityPM);
            }
            this.Parent.AddRecipientEnabled = true;
        }
        else {
            this.Parent.AddRecipientEnabled = false;
        }
    }
    //#endregion

    OnMouseOver() {
        if (this.EntityPM.LineNumber > 1) {
            this.DeleteRecipientVisible = true;
        }
    }

    OnMouseLeave() {
        if (!this.overCloseButton) {
            this.DeleteRecipientVisible = false;
        }
    }

    // close button
    overCloseButton: boolean = false;
    OnIconButtonMouseOver() {
        this.overCloseButton = true;
    }

    OnIconButtonMouseLeave() {
        this.overCloseButton = false;
    }


    DeleteExportRecipientButtonClicked() {

        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeleteExportRecipient");
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 150;
        confirmWindow.Show(msg);
        confirmWindow.WindowClosed.subscribe((event: any) => {

            if (confirmWindow.Yes) { // YES
                this.Parent.EntityPM.RemoveDeclarationExportRecipient(this.EntityPM);
                this.Parent.BuildRecipientsList();
            }
        });


    }



}
