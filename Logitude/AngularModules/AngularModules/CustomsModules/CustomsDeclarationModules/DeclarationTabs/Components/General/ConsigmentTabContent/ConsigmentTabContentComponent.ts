declare var window: any;
import { OnDestroy, Component, ChangeDetectorRef } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool, DateTool } from '../../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConfirmWindow } from '../../../../../../Controls/Windows/ConfirmWindow';
import { DeclarationPMService } from '../../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { ConsignmentInternalTransitionPM } from '../../../../../../Customs/EntityPMs/ConsignmentInternalTransitionPM';
import { ConsignmentPM } from '../../../../../../Customs/EntityPMs/ConsignmentPM';
import { CouriersVatPM } from '../../../../../../Customs/EntityPMs/CouriersVatPM';
import { DeclarationPM } from '../../../../../../Customs/EntityPMs/DeclarationPM';
import { ConsignmentPackagePM } from '../../../../../../Customs/EntityPMs/ConsignmentPackagePM';
import { ClientList } from '../../../../../../Customs/EntityLists/ClientList';
import { LogTab } from '../../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { CustomsRequiredFieldListService } from '../../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';
import { CustomsRequestMenuService } from '../../../../../../Customs/Services/Others/CustomsRequestMenuService';
import { DeliverySiteTypeListService } from '../../../../../../Customs/Services/StandardLists/DeliverySiteTypeListService';
import { DeclarationEventManager } from '../../../../../../Customs/Utilities/DeclarationEventManager'
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { ApiQueryFilters, FilterItem } from '../../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CouriersVatPMService } from '../../../../../../Customs/Services/StandardPMs/CouriersVatPMService';
import { CouriersVatExtendedPMService } from '../../../../../../Customs/Services/ExtendedPMs/CouriersVatExtendedPMService';
import { MessageWindow } from '../../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../../Controls/Windows/LogitudeWindow';
import { WindowArgs } from '../../../../../../Infrastructure/DataContracts/WindowArgs';
import { CustomsRequiredFieldExtendedListService } from '../../../../../../Customs/Services/ExtendedLists/CustomsRequiredFieldExtendedListService';
import { DeclarationCourierStatusList } from '../../../../../../Customs/EntityLists/DeclarationCourierStatusList';
import { DeclarationCourierStatusListService } from '../../../../../../Customs/Services/StandardLists/DeclarationCourierStatusListService';
import { EntityResourceService } from '../../../../../../Infrastructure/Services/EntityResourceService';
import { CargoIdentifireTypeListService } from '../../../../../../Customs/Services/StandardLists/CargoIdentifireTypeListService';
import { DeclarationExtendedListService } from '../../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { CargoIdentifireTypePM } from '../../../../../../Customs/EntityPMs/CargoIdentifireTypePM';
import { InternalBorderSiteTypeListService } from 'Customs/Services/StandardLists/InternalBorderSiteTypeListService';
import { InternalBorderSiteTypePMService } from 'Customs/Services/StandardPMs/InternalBorderSiteTypePMService';

@Component({
    selector: 'ConsigmentTabContent',

    templateUrl: './ConsigmentTabContentComponent.html',
})

export class ConsigmentTabContentComponent
    extends BaseComponent
    implements OnDestroy {
    public EntityPM: ConsignmentPM;
    public declarationPM: DeclarationPM;

    public ObjectTableName: string = "Customs.Consignment";
    public DataContext: any = this;
    public Tab: LogTab;

    public IsDisplayOnly: boolean = false;
    public ParentIsDisplayOnly: boolean = false;
    ShowExcludeConsignmentBoolean: boolean = false;
    IsCourierDeclaration: boolean = false;
    _DeclarationCourierStatus: DeclarationCourierStatusList;
    entityResourceService: EntityResourceService = new EntityResourceService();
    _declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    public LoadingPortFilterItems: ApiQueryFilters;//38388
    _CargoIdentifireTypePM: CargoIdentifireTypePM = new CargoIdentifireTypePM();

    // Edit grid array
    ConsimentPackages: ObservableCollection;

    SiteList: ConsignmentInternalTransitionModel[] = [];
    _CargoIdentifireTypeListService: CargoIdentifireTypeListService = new CargoIdentifireTypeListService();
    private _DeliverySiteTypeListService: DeliverySiteTypeListService = new DeliverySiteTypeListService();
    private _CouriersVatPMService: CouriersVatPMService = new CouriersVatPMService();
    private _CouriersVatExtendedPMService: CouriersVatExtendedPMService = new CouriersVatExtendedPMService();
    private _DeclarationPMService: DeclarationPMService = new DeclarationPMService();

    public WeightValueFilterItems: ApiQueryFilters;
    public ExportCargoTypeFilterItems: ApiQueryFilters;
    public ImportCargoTypeFilterItems: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;

    public ConsignmentTypes: ConsignmentType[] = [{ Id: "E", Value: TextCodeTranslator.Translate("Customs.Consignment.O.Export") }, { Id: "I", Value: TextCodeTranslator.Translate("Customs.Consignment.O.Import") }];
    CargoIdKeyOrigin: { a: string, b: string, c: string } = { a: '', b: '', c: '' };


    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this.ConsimentPackages = new ObservableCollection([]);
        // this.declarationPM = entityArgs.EntityPM;
        this.WeightValueFilterItems = new ApiQueryFilters();
        this.WeightValueFilterItems.addAdditionalFilter("Code", "CC,CA,NC,PO,PP", null, null, "InListExact", false, false, false, "string", false, true);
        this.ExportCargoTypeFilterItems = new ApiQueryFilters();
        this.ExportCargoTypeFilterItems.addAdditionalFilter("IsForDeclarationExport", true, null, null, "Equal", false, false, false, "boolean", false, true);
        this.ImportCargoTypeFilterItems = new ApiQueryFilters();
        this.ImportCargoTypeFilterItems.addAdditionalFilter("IsForDeclarationImport", true, null, null, "Equal", false, false, false, "boolean", false, true);

        this.UIProperties.SetEnabled("CrateNumber", "Customs.DeclarationCourierStatus", false);
        this.SiteList = [];
        this.LoadingPortFilterItems = new ApiQueryFilters();//38388

        this.Listen();
       

        var table = window.ObjectTables.filter(d => d.Name === 'Customs.Declaration')[0];

        var IsExcludeManifestFeature = FeatureLocator.Features.filter(f => (f.Code == "ISEXCLUDEMANIFEST") && f.ObjectTableId == table.Id)[0];
        if (IsExcludeManifestFeature) {
              this._ShowExcludeManifest= true;
        }

    }
    private _SubDisplayModeChanged;
    private _SubConsignmentsChanged;

    openKanamDeclaration() {
        this._declarationExtendedListService.GetSingleDeclarationByNumber(this.ManifestNumber?.trim(), SessionLocator.Tenant).subscribe((myResult: any) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                if (entity != null) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Customs.Declaration', BackButtonLabel: TextCodeTranslator.Translate("Customs.Consignment.O.ShahamDeclaration") });
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                DeclarationEventManager.DisplayModeChanged.emit(null);
                                this.CurrentSession.CurrentEditComponent.DisplayModeChanged.emit(this.IsDisplayOnly);
                            });
                        });
                }
                else {
                    var messageWindow = new MessageWindow();
                    messageWindow.Width = 250;
                    messageWindow.Height = 150;
                    messageWindow.RTL = true;
                    messageWindow.Show(TextCodeTranslator.Translate("Customs.Consignment.O.NoDeclarationFound"));
                }
            }
        });
    }

    public ConsignmentTypeSelectionChanged(value) {
        this.ConsignmentType = value;
        if (AppTool.IsNullOrEmpty(this.CargoTypeCode) && this.declarationPM.TransportModeId == 'A' && this.ConsignmentType == 'E') {
            this.CargoTypeCode = "16";
        }

        this.SetTipsInsideCargoIdentifires(this.EntityPM.CargoTypeCode);

        
        var consignmentIndex = 0;

        if (this.declarationPM.Consignments.length > 0) {
            var consignmentsSameType = this.declarationPM.Consignments.filter(x => x.ConsignmentType == this.ConsignmentType && x.ConsignmentNumber != this.EntityPM.ConsignmentNumber);
            if (consignmentsSameType.length > 0) {
                var maxObj = consignmentsSameType.reduce(function (prev, current) { return (prev.SequenceNumeric > current.SequenceNumeric) ? prev : current });
                if (maxObj != null) {
                    if (consignmentIndex <= maxObj.SequenceNumeric)
                        consignmentIndex = maxObj.SequenceNumeric;
                }
            }
        }
            this.EntityPM.SequenceNumeric = ++consignmentIndex;
            this.Tab.Header = (this.EntityPM.ManifestNumber ? (this.EntityPM.ManifestNumber + '-') : '') + this.EntityPM.SequenceNumeric;
        
        
    }
    ngOnDestroy() {
        console.log("ConsigmentTabContentComponent:ngOnDestroy");
        //if (this.Tab.ComponentReference && this.Tab.ComponentReference.ngOnDestroy) {
        //    this.Tab.ComponentReference.ngOnDestroy();
        //}

        if (this.Tab) {
            this.Tab.ComponentReference = null;
            this.Tab = null;
        }
        if (this._SubDisplayModeChanged) {
            this._SubDisplayModeChanged.unsubscribe();
            this._SubDisplayModeChanged = null;
        }
        if (this._SubConsignmentsChanged) {
            this._SubConsignmentsChanged.unsubscribe();
            this._SubConsignmentsChanged = null;
        }
    }

    public FeatureLocatorEXPORTDECLARATIONPSCREEN = FeatureLocator.HasFeaturePermession("Customs.Declaration", "EXPORTDECLARATIONPSCREEN")


    ShowExportConsScreen() {

        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
        SessionLocator.SelectedSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.declarationPM = this.declarationPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        //var windowTitle = TextCodeTranslator.Translate("Customs.ExportDeclarationDataQuery.F.ExportDeclarationData");
        var windowTitle = TextCodeTranslator.Translate("Customs.Consignment.O.AdditionalDataForExport");

        var logWindow = new LogitudeWindow();
        //windowArgs.Type = "Importer";
        //this.Type = "Importer";
        logWindow.Width = 1000;
        logWindow.Height = 250;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        //logWindow.WindowClosed.subscribe(($event: any) => this.SetFieldsDisabled($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ConsigmentTabContent/ExportConsigmentContentComponent');
    }
    private Listen() {
        this._SubDisplayModeChanged =
                this.CurrentSession.CurrentEditComponent.DisplayModeChanged.subscribe((IsDisplayOnly: any) => {
                if (this.ShowExcludeConsignmentBoolean && this.ExcludeConsignment)
                    this.IsDisplayOnly = true;
                else
                    this.IsDisplayOnly = IsDisplayOnly;

                this.ParentIsDisplayOnly = IsDisplayOnly;

                this.SetScreenFieldsEditability();

                this.UIProperties.SetEnabled("CrateNumber", "Customs.DeclarationCourierStatus", false);
                this.BuildSitesList();
                this.SetTipsInsideCargoIdentifires(this.EntityPM.CargoTypeCode);

            });
        this._SubConsignmentsChanged =
            DeclarationEventManager.ConsignmentsChanged.subscribe((e) => {
                console.log("ConsignmentsChanged", this.declarationPM, e);
                this.SetExcludeConsignmentVisibility();

            });
    }

    SetExcludeConsignmentVisibility() {
        this.ShowExcludeConsignmentBoolean = this.declarationPM.Consignments.length == 1;
        if (this.ShowExcludeConsignmentBoolean && this.ExcludeConsignment) {
            this.IsDisplayOnly = true;
        }
    }

    SetTabArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        this.ParentIsDisplayOnly = args.Disabled;
        this.updateCargoIdKeyOrigin();


        if (this.EntityPM.CargoTypeCode == "17")
            this.LoadCouriersVat();

        if (!this.declarationPM)
            this.declarationPM = args.Parent;

        //if (this.declarationPM.Direction == "E") {
        //if (AppTool.IsNullOrEmpty(this.ConsignmentType)) { this.ConsignmentType = "E"; }
        //}

        this.IsCourierDeclaration = this.declarationPM.IsCourierDeclaration;
        this.ShowExcludeConsignmentBoolean = this.declarationPM.Consignments.length == 1;

        this.SetExcludeConsignmentVisibility();

        if (AppTool.IsNullOrEmpty(this.CargoTypeCode) && this.declarationPM.TransportModeId == 'A' && this.ConsignmentType == 'E') {
            this.CargoTypeCode = "16";
        }

        this.SetTipsInsideCargoIdentifires(this.EntityPM.CargoTypeCode);

        //this.EntityPM.PropertyChanged.subscribe((event) => { console.log("PropertyChanged: ", event); });

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsignmentPackages)) {
            for (let pkg of this.EntityPM.ConsignmentPackages) {
                var item = new ConsigmentPackageModel(pkg,this.Tab);
                this.ConsimentPackages.Insert(item);
            }
        }
        //if (this.IsDisplayOnly) {
        //    this.SetScreenFieldsEditability();
        //}

        this.SetScreenFieldsEditability();

        //**
        //this.SetDateVisibilty(); // this make entity dirty on tab loaded, the following should solve it
        //if (this.CargoTypeCode == "17") {
        //    this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, false);
        //    this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, true);
        //}
        //else {
        //    this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, true);
        //    this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, false);
        //}
        //**


        // show xml errors
        if (!AppTool.IsNullOrEmpty(args.DecErrors)) {
            if (!AppTool.IsNullOrEmpty(args.DecErrors.Field)) {
                this.UIProperties.SetValidity(args.DecErrors.Field, "Customs.Consignment", false, args.DecErrors.Description);
            }
        }

        this.BuildSitesList();
        this.InitLOVFilters();//38388
        this.CheckRequrierdFieldsForSend();
        this.GetDeclarationCourierStatusData();

        console.log("Tabs Args: ", args);
    }

    private updateCargoIdKeyOrigin() {
        this.CargoIdKeyOrigin = {
            a: this.EntityPM.ManifestNumber,
            b: this.EntityPM.SecondCargoID,
            c: this.EntityPM.ThirdCargoID,
        };
    }

    GetDeclarationCourierStatusData() {
        if (this.IsCourierDeclaration) {

            let myDeclarationCourierStatusListService: DeclarationCourierStatusListService = new DeclarationCourierStatusListService();

            let filters = new ApiQueryFilters();

            filters.addAdditionalFilter("DeclarationId", this.declarationPM.Id, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
            filters.PageSize = 1;
            myDeclarationCourierStatusListService.getByFilters(filters)
                .subscribe((serviceResponse1: ServiceResponse) => {
                    let mappedDeclarationCourierStatusList: Array<DeclarationCourierStatusList> = serviceResponse1.Result;
                    if (mappedDeclarationCourierStatusList != null && mappedDeclarationCourierStatusList.length > 0) {
                        this._DeclarationCourierStatus = mappedDeclarationCourierStatusList[0];
                        this.CrateNumber = this._DeclarationCourierStatus.CrateNumber;
                    }

                });

        }
    }

    SetDateVisibilty() {
        //if (this.CargoTypeCode == "17") {
        //    this.ThirdCargoID = null;
        //    this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, false);
        //    this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, true);
        //}
        //else {
        //    this.CargoDate = null;
        //    this.UIProperties.SetVisibility("ThirdCargoID", this.ObjectTableName, true);
        //    this.UIProperties.SetVisibility("CargoDate", this.ObjectTableName, false);
        //}
    }

    SetScreenFieldsEditability() {
        console.log("SetScreenFieldsEditability: " + this.EntityPM);
        this.UIProperties.SetEnabled("CargoDescription", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ShipCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ConsignmentType", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("OriginCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ReceiverWarehouseCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExportUnloadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsLastReleaseFromWarehous", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("LoadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExportLoadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoTypeCodeForExport", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeliveryPlaceName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WeightValue", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ExportRecieverWareHouseCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("FinalDestinationPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("RecieverWareHouseCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsDangerousGoods", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageSiteCodeExport", this.ObjectTableName, !this.IsDisplayOnly);
        if (this.declarationPM.TransportModeId != 'O') {
            this.UIProperties.SetEnabled("ShipCode", this.ObjectTableName, false);
        }

    }

    LoadCouriersVat() {
        var vatNumber = this.SecondCargoID;
        this._CouriersVatExtendedPMService.GetSingleCouriersVatByCode(vatNumber).subscribe((response: ServiceResponse) => {

            console.log("[response] GetSingleCouriersVatByCode: ", response);
            var result = response.Result;
            if (!AppTool.IsNullOrEmpty(result)) {
                this.CouriersVatId = result.Id;
                this.CouriersVat = result;
            }
        });


    }
    InitLOVFilters() {//38388
        // initialize query filters for LoadingPort according to CountryCode
        if (!AppTool.IsNullOrEmpty(this.OriginCountryCode)) {
            this.LoadingPortFilterItems.removeAdditionalFilter("CountryTypeCode");
            var CountryFilter = new FilterItem("CountryTypeCode", this.OriginCountryCode, null, null, "Equals", false, false, false, "string", false, true, true);
            this.LoadingPortFilterItems.AdditionalFilters.push(CountryFilter);
            //this.LoadingPortFilterItems.addAdditionalFilter("CountryTypeCode", this.OriginCountryCode, null, null, "Equals", false, false, false, "string", false, true);
            //this.LoadingPortFilterItems.ForceCacheRefresh = true;
        }
        else {
            this.LoadingPortFilterItems.removeAdditionalFilter("CountryTypeCode");
        }
    }
    //#region Properties


    public get CrateNumber() { return this._DeclarationCourierStatus != null ? this._DeclarationCourierStatus.CrateNumber : null; }
    public set CrateNumber(newValue: string) {
        if (this._DeclarationCourierStatus != null) {
            this._DeclarationCourierStatus.CrateNumber = newValue;
        }
    }

    couriersVatId: string;
    public get CouriersVatId() { return this.couriersVatId; }
    public set CouriersVatId(newValue: string) { this.couriersVatId = newValue; }

    couriersVat: CouriersVatPM;
    public get CouriersVat() { return this.couriersVat; }
    public set CouriersVat(newValue: CouriersVatPM) {
        this.couriersVat = newValue;
        if (this.couriersVat) {
            this.SecondCargoID = this.couriersVat.VatNumber;
        }
    }

    CouriersVatTextChanged(text) {
        this.SecondCargoID = text;
    }


    public get CargoTypeCode() { return this.EntityPM ? this.EntityPM.CargoTypeCode : null; }
    public set CargoTypeCode(newValue: string) {
        this.EntityPM.CargoTypeCode = newValue;
        this.SetDateVisibilty();
        this.SetTipsInsideCargoIdentifires(newValue);
        if (newValue == "17")
            this.LoadCouriersVat();
    }

    public get CargoTypeCodeForExport() { return this.EntityPM ? this.EntityPM.CargoTypeCode : null; }
    public set CargoTypeCodeForExport(newValue: string) {
        this.EntityPM.CargoTypeCode = newValue;
     
    if(this.declarationPM.Direction=='E')
        this.SetTipsInsideCargoIdentifires(this.EntityPM.CargoTypeCode);
    }

    public get CargoDescription() { return this.EntityPM ? this.EntityPM.CargoDescription : null; }
    public set CargoDescription(newValue: string) { this.EntityPM.CargoDescription = newValue; }


    public get ConsignmentType() { return this.EntityPM ? this.EntityPM.ConsignmentType : null; }
    public set ConsignmentType(newValue: string) { this.EntityPM.ConsignmentType = newValue; this.SetCargoTypeTranssshipment() }

    public get ShipCode() { return this.EntityPM ? this.EntityPM.ShipCode : null; }
    public set ShipCode(newValue: string) { this.EntityPM.ShipCode = newValue; }


    public get ThirdCargoID() { return this.EntityPM ? this.EntityPM.ThirdCargoID : null; }
    public set ThirdCargoID(newValue: string) {
        this.EntityPM.ThirdCargoID = newValue;
        if (this._CargoIdentifireTypePM != null) {
            this.setRequired();
        }
    }

    public get UnloadDate() { return this.EntityPM ? this.EntityPM.UnloadDate : null; }
    public set UnloadDate(newValue: Date) { this.EntityPM.UnloadDate = newValue; }

    public get ManifestDate() { return this.EntityPM ? this.EntityPM.ManifestDate : null; }
    public set ManifestDate(newValue: Date) { this.EntityPM.ManifestDate = newValue; }

    public get OriginCountryCode() { return this.EntityPM ? this.EntityPM.OriginCountryCode : null; }
    public set OriginCountryCode(newValue: string) {
        this.EntityPM.OriginCountryCode = newValue;
        this.InitLOVFilters();
    }//38388

    private timerToken: any;

    public get SecondCargoID() { return this.EntityPM ? this.EntityPM.SecondCargoID : null; }
    public set SecondCargoID(newValue: string) {
        this.EntityPM.SecondCargoID = newValue;

        if (this._CargoIdentifireTypePM != null) {
            this.setRequired();
        }
    }


    public get ReceiverWarehouseCode() { return this.EntityPM ? this.EntityPM.ReceiverWarehouseCode : null; }
    public set ReceiverWarehouseCode(newValue: string) { this.EntityPM.ReceiverWarehouseCode = newValue; }

    public get DeliveryPlaceName() { return this.EntityPM ? this.EntityPM.DeliveryPlaceName : null; }
    public set DeliveryPlaceName(newValue: string) { this.EntityPM.DeliveryPlaceName = newValue; }



    public get UnloadPortCode() { return this.EntityPM ? this.EntityPM.UnloadPortCode : null; }
    public set UnloadPortCode(newValue: string) {
        this.EntityPM.UnloadPortCode = newValue;
        if (this.declarationPM.TransportModeId == 'A') {

            this._DeliverySiteTypeListService.getSingle(newValue).subscribe((response: ServiceResponse) => {
                if (!AppTool.IsNullOrEmpty(response)) {
                    var entity = response.Result;
                    if (!AppTool.IsNullOrEmpty(entity)) { //Exisit

                        this.EntityPM.StorageSiteCode = newValue;
                        console.log("The StorageSiteCode is set to ", newValue);
                    }
                }
            });

        }
    }

    public get ManifestNumber() { return this.EntityPM ? this.EntityPM.ManifestNumber : null; }
    public set ManifestNumber(newValue: string) {
        this.EntityPM.ManifestNumber = newValue;
            this.Tab.Header = (newValue ? (newValue + '-') : '') + this.EntityPM.SequenceNumeric;
            var IsMarksNumbers=this.declarationPM.Direction=='E'&&this.declarationPM.TransportModeId=='O'&& !AppTool.IsNullOrEmpty(this.EntityPM.ConsignmentPackages[0]?.MarksNumbers)
            if(!IsMarksNumbers) {
               this.Tab.Title=null;

            }            
        if (this._CargoIdentifireTypePM != null) {
            this.setRequired();
        }
    }

    //public get IsLastReleaseFromWarehous() { return this.EntityPM.IsLastReleaseFromWarehous == "T" ? true : false; }
    //public set IsLastReleaseFromWarehous(newValue: boolean) { newValue ? this.EntityPM.IsLastReleaseFromWarehous = "T" : this.EntityPM.IsLastReleaseFromWarehous = "N" ; }

    public get IsLastReleaseFromWarehous() {
        ///return this.EntityPM ? this.EntityPM.IsLastReleaseFromWarehous : null;
        if (this.EntityPM == null) {
            return null;
        }
        if (this.declarationPM.Direction == "E") {
            return this.EntityPM.IsLastReleaseFromWarehous == "T" ? true : false;
        }
        return this.EntityPM.IsLastReleaseFromWarehous;
    }
    public set IsLastReleaseFromWarehous(newValue: any/* string | boolean*/) {
        if (this.declarationPM.Direction == "E") {
            newValue ? this.EntityPM.IsLastReleaseFromWarehous = "T" : this.EntityPM.IsLastReleaseFromWarehous = "N";
        } else {
            let myIsLastReleaseFromWarehous: string = newValue
            this.EntityPM.IsLastReleaseFromWarehous = myIsLastReleaseFromWarehous;
        }
    }


    public get OriginalCountryCode() { return this.EntityPM ? this.EntityPM.OriginCountryCode : null; }
    public set OriginalCountryCode(newValue: string) { this.EntityPM.OriginCountryCode = newValue; }

    public get StorageSiteCode() { return this.EntityPM ? this.EntityPM.StorageSiteCode : null; }
    public set StorageSiteCode(newValue: string) { this.EntityPM.StorageSiteCode = newValue; }

    public get StorageSiteCodeExport() { return this.EntityPM ? this.EntityPM.StorageSiteCode : null; }
    public set StorageSiteCodeExport(newValue: string) { this.EntityPM.StorageSiteCode = newValue; }

    public get LoadingPortCode() { return this.EntityPM ? this.EntityPM.LoadingPortCode : null; }
    public set LoadingPortCode(newValue: string) { this.EntityPM.LoadingPortCode = newValue; }

    public get CargoDate() { return this.EntityPM ? this.EntityPM.CargoDate : null; }
    public set CargoDate(newValue: Date) {
        this.EntityPM.CargoDate = newValue;
        if (!AppTool.IsNullOrEmpty(newValue)) {

            var day = newValue.getDate() + "";
            var month = (newValue.getMonth() + 1) + "";
            //var year = (newValue.getFullYear()) + ""; 
            var year = newValue.getFullYear().toString().substring(2, 4);

            var id = this.ApplyPadding(day) + this.ApplyPadding(month) + year;
            this.ThirdCargoID = id;

            console.log("Date::: ", id);

        } else {
            this.ThirdCargoID = null;
        }
    }
    public SecondCargoIDPlaceholder: string = " ";

    public ManifestNumberPlaceholder: string = " ";
    public ThirdCargoIdPlaceholder: string = " ";

    public get ExcludeConsignment() { return this.declarationPM.ExcludeConsignment; }
    public set ExcludeConsignment(newValue: boolean) {
        this.declarationPM.ExcludeConsignment = newValue;

        if (this.ParentIsDisplayOnly) {
            this.IsDisplayOnly = true;
        } else {
            this.IsDisplayOnly = newValue;
        }
        this.SetScreenFieldsEditability();
    }
    public get ExcludeManifest() { return this.declarationPM.ExcludeManifest; }
    public set ExcludeManifest(newValue: boolean) {
        this.declarationPM.ExcludeManifest = newValue;

    }
    public _ShowExcludeManifest = false;


    public get WeightValue() { return this.declarationPM.WeightValue; }
    public set WeightValue(newValue: string) { this.declarationPM.WeightValue = newValue; }

    addSiteEnabled: boolean;
    public get AddSiteEnabled() { return this.EntityPM ? this.addSiteEnabled : null; }
    public set AddSiteEnabled(newValue: boolean) {
        this.addSiteEnabled = newValue;



    }



    public get ExportLoadingPortCode() { return this.EntityPM.ExportLoadingPortCode; }
    public set ExportLoadingPortCode(newValue: string) {
        this.EntityPM.ExportLoadingPortCode = newValue;

    }



    public get ExportUnloadingPortCode() { return this.EntityPM.ExportUnloadingPortCode; }
    public set ExportUnloadingPortCode(newValue: string) {
        this.EntityPM.ExportUnloadingPortCode = newValue;

    }


    DestinationCountry: any;
    public get FinalDestinationPortCode() { return this.EntityPM.FinalDestinationPortCode; }
    public set FinalDestinationPortCode(newValue: string) {
        this.EntityPM.FinalDestinationPortCode = newValue;

    }



    public get ExportRecieverWareHouseCode() { return this.EntityPM.ExportRecieverWareHouseCode; }
    public set ExportRecieverWareHouseCode(newValue: string) {
        this.EntityPM.ExportRecieverWareHouseCode = newValue;
        //if (newValue) {
        //    this.UIProperties.SetRequired("ExportRecieverWareHouseCode", this.ObjectTableName, false);
        //}
        //else {
        //    this.UIProperties.SetRequired("ExportRecieverWareHouseCode", this.ObjectTableName, true);
        //}
    }

    public get RecieverWareHouseCode() { return this.EntityPM.ReceiverWarehouseCode; }
    public set RecieverWareHouseCode(newValue: string) {
        this.EntityPM.ReceiverWarehouseCode = newValue;
    }

    public get IsDangerousGoods() { return this.EntityPM.IsDangerousGoods; }
    public set IsDangerousGoods(newValue: boolean) {
        this.EntityPM.IsDangerousGoods = newValue;
        //if (newValue) {
        //    this.UIProperties.SetRequired("IsDangerousGoods", this.ObjectTableName, false);
        //}
        //else {
        //    this.UIProperties.SetRequired("IsDangerousGoods", this.ObjectTableName, true);
        //}
    }
    //#endregion

    setRequired() {
        if (this.declarationPM.Direction == 'E') {
            this.UIProperties.SetWarning("ManifestNumber", this.ObjectTableName, true);
            if (!AppTool.IsNullOrEmpty(this.ManifestNumber)) {
                this.UIProperties.SetWarning("ManifestNumber", this.ObjectTableName, false);
            }

            if (this._CargoIdentifireTypePM.IsKey2Mandatory) {
                this.UIProperties.SetWarning("SecondCargoID", this.ObjectTableName, true);
                if (!AppTool.IsNullOrEmpty(this.SecondCargoID)) {
                    this.UIProperties.SetWarning("SecondCargoID", this.ObjectTableName, false);
                }
            } else {
                this.UIProperties.SetWarning("SecondCargoID", this.ObjectTableName, false);
            }
            if (this._CargoIdentifireTypePM.IsKey3Mandatory) {
                
                this.UIProperties.SetWarning("ThirdCargoID", this.ObjectTableName, true);
                if (!AppTool.IsNullOrEmpty(this.ThirdCargoID)) {
                    this.UIProperties.SetWarning("ThirdCargoID", this.ObjectTableName, false);
                }
            } else {
                this.UIProperties.SetWarning("ThirdCargoID", this.ObjectTableName, false);
            }
            if (this.EntityPM.ConsignmentType == 'E') {
                this.UIProperties.SetWarning("ExportUnloadingPortCode", this.ObjectTableName, true);
                if (this.ExportUnloadingPortCode != null) {
                    this.UIProperties.SetWarning("ExportUnloadingPortCode", this.ObjectTableName, false);
                }

                this.UIProperties.SetWarning("ExportLoadingPortCode", this.ObjectTableName, true);
                if (this.ExportLoadingPortCode != null) {
                    this.UIProperties.SetWarning("ExportLoadingPortCode", this.ObjectTableName, false);
                }
            }
            if (this.EntityPM.ConsignmentType == 'I') {
                this.UIProperties.SetWarning("LoadingPortCode", this.ObjectTableName, true);
                if (this.LoadingPortCode != null) {
                    this.UIProperties.SetWarning("LoadingPortCode", this.ObjectTableName, false);
                }

                this.UIProperties.SetWarning("UnloadPortCode", this.ObjectTableName, true);
                if (this.UnloadPortCode != null) {
                    this.UIProperties.SetWarning("UnloadPortCode", this.ObjectTableName, false);
                }
            }
        }
    }   
    SetTipsInsideCargoIdentifires(value: string) {

        if (this.declarationPM.Direction == 'E') {
            this._CargoIdentifireTypeListService.getSingleFromCache(value)
                .subscribe((Response: ServiceResponse) => {
                    if (Response.Result != null) {
                        this.ManifestNumberPlaceholder = Response.Result.CargoIdentifierKey1Name;
                        this.SecondCargoIDPlaceholder = Response.Result.CargoIdentifierKey2Name ?? '';
                        this.ThirdCargoIdPlaceholder = Response.Result.CargoIdentifierKey3Name ?? '';
                        this._CargoIdentifireTypePM = Response.Result;
                        this.setRequired();
                    }
                });
        }
        else {
            switch (value) {
                case '1':
                    {
                        this.ManifestNumberPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterFlightYear");
                        this.SecondCargoIDPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterMainManifest");
                        this.ThirdCargoIdPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterInternalManifest");
                        break;
                    }
                case '2':
                    {
                        this.ManifestNumberPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterPackageNum");
                        this.SecondCargoIDPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterYearCargoCreation");
                        this.ThirdCargoIdPlaceholder = " ";
                        break;
                    }
                case '8':
                    {
                        this.ManifestNumberPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterStorageDeclaration");
                        this.SecondCargoIDPlaceholder = " ";
                        this.ThirdCargoIdPlaceholder = " ";
                        break;
                    }
                case '11':
                    {
                        this.ManifestNumberPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterManifest");
                        this.SecondCargoIDPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterTransactionID");
                        this.ThirdCargoIdPlaceholder = " ";
                        break;
                    }
                case '17':
                    {
                        this.ManifestNumberPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterBOLBaldar");
                        this.SecondCargoIDPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterVatBaldar");
                        this.ThirdCargoIdPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterEstablishmentDate");
                        break;
                    }
                case '20':
                    {
                        this.ManifestNumberPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterTransactionIDMlm");
                        this.SecondCargoIDPlaceholder = " ";
                        this.ThirdCargoIdPlaceholder = " ";
                        break;
                    }
                case '16':
                    {
                        if (this.declarationPM.TransportModeId == 'A' && this.ConsignmentType == 'E') {
                            this.ManifestNumberPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterYear");
                            this.SecondCargoIDPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterMAIBOL");
                            this.ThirdCargoIdPlaceholder = TextCodeTranslator.Translate("Customs.Consignment.O.EnterAirLineAShipper");
                            break;
                        }

                    }
                default:
                    {
                        this.ManifestNumberPlaceholder = " ";
                        this.SecondCargoIDPlaceholder = " ";
                        this.ThirdCargoIdPlaceholder = " ";
                        break;
                    }
            }
        }
    }
    SetCargoTypeTranssshipment() {
        if (this.declarationPM.DeclarationTypeCode == "3" && this.ConsignmentType == 'I') {
            if (this.declarationPM.TransportModeId == 'A' || this.declarationPM.TransportModeId == 'O') {
                this.CargoTypeCode = ""
                this.ManifestNumber = ""
                this.SecondCargoID = ""
                this.ThirdCargoID = ""
                if (this.declarationPM.TransportModeId == 'A')
                    this.CargoTypeCode = "1"
                else
                    this.CargoTypeCode = "11"

            }
        }
    }
    AddPackageButtonClicked() {
        var line = new ConsignmentPackagePM(this.EntityPM);
        this.EntityPM.AddConsignmentPackage(line);
        var item = new ConsigmentPackageModel(line,this.Tab);
        this.ConsimentPackages.Insert(item);
        //this.CurrentSession.ResetRowIndex();
    }

    OpenEditDangerWindow(item) {
        if (!AppTool.IsNullOrEmpty(item)) {

            var windowArgs: any = {};
            windowArgs.ConsignmentPackagesDangerPM = item.EntityPM;
            windowArgs.Declaration = this.declarationPM;

            windowArgs.Parent = item;
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            if (item.EntityPM.LineNumber != item.EntityPM.entityParentPM.consignmentPackages[0].LineNumber) {
                windowArgs.IsDisplayOnlyContact = true;
            }
            var windowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.ConsignmentPackagesDanger");

            var logWindow = new LogitudeWindow();
            logWindow.Width = 900;
            logWindow.Height = 300;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/ConsigmentTabContent/ConsigmentPackagesDanger/ConsigmentPackagesDangerComponent');


        }
    }


    RemovePackageButton(item) {
        
        if (!this.IsDisplayOnly) {
            if (!AppTool.IsNullOrEmpty(item)) {

                var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeletePackage");
                var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 400;
                confirmWindow.Height = 150;
                confirmWindow.Show(msg);
                confirmWindow.WindowClosed.subscribe((event: any) => {

                    if (confirmWindow.Yes) { // YES
                        this.ConsimentPackages.Remove(item);
                        this.EntityPM.RemoveConsignmentPackage(item.EntityPM);

                     if(this.declarationPM.Direction=='E'&&this.declarationPM.TransportModeId=='O'){
                         if(this.ConsimentPackages.Length==0||AppTool.IsNullOrEmpty(this.EntityPM.ConsignmentPackages[0]?.MarksNumbers)){
                             this.Tab.Title=null;
                         }
                         else{                          
                            var val=this.EntityPM.ConsignmentPackages[0]?.MarksNumbers.replace("\n", "");
                             this.Tab.Title = (this.EntityPM.ConsignmentPackages[0]?.MarksNumbers.replace("\n", "") + '-')  + this.EntityPM.SequenceNumeric;
                         }


                     }
                }
            });

            }
        }
        else{
            var messageWindow = new MessageWindow();
            messageWindow.Title = ""
            messageWindow.RTL=true;
            messageWindow.Width = 250;
            messageWindow.Height = 150;
            messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            messageWindow.Show(TextCodeTranslator.Translate("Customs.Consignment.O.LockedDecCantDelete"));
            return;
        }

    }

    OnSecondCargoLostFocus() {
        if (this.CargoTypeCode == "11") {
            if (!AppTool.IsNullOrEmpty(this.SecondCargoID)) {
                if (this.SecondCargoID.length == 9) {
                    if (this.SecondCargoID.charAt(0) != 'I') {
                        this.SecondCargoID = 'I' + this.SecondCargoID;
                    }
                }
            }
        }
    }

    OnRowEnded($event) {
        console.log("this.ConsimentPackages.Length : " + this.ConsimentPackages.Length);
        if (($event) == this.ConsimentPackages.Length) {
            this.AddPackageButtonClicked();
        }
    }

    MasterBOLRequestMethod() {
        if (this.IsDisplayOnly) {
            //return; 
        }

        let customsRequestMenuService = new CustomsRequestMenuService();
        let my = {
            "CustomFileNo": this.declarationPM.CustomFileNo,
            "Date": this.EntityPM.ManifestNumber,
            "MasterBillOfLading": this.EntityPM.SecondCargoID,
            "InternalIdentifier": this.EntityPM.ThirdCargoID,
            //"ReturnAllInernalCargos": this.EntityPM.ThirdCargoID ? false : true,//task 44705 21.11.18
            "ReturnAllInernalCargos": true,
            "DeclarationId": this.EntityPM.DeclarationId,
        };
        customsRequestMenuService.WindowClosed.subscribe(($event: any) => this.OnMasterBOLRequestWindowClosed($event));
        customsRequestMenuService.ShowModalAsEditMenuAction("9020", my);
    }

    OnMasterBOLRequestWindowClosed(arg: any) {
        if (!AppTool.IsNullOrEmpty(arg)) {
            this.ThirdCargoID = arg.CargoIdentifierKey3;

            if (this.ConsimentPackages != null && this.ConsimentPackages.Length > 0) {
                this.ConsimentPackages.Collection.forEach((item) => {
                    if (item.PackageMeasureQualifierCode == "2") {
                        item.PackageQuantity = arg.PacakgesQuantity;
                        item.GrossMassMeasure = arg.TotalWheight;
                        return;
                    }
                });
            }
            else {
                var line = new ConsignmentPackagePM(this.EntityPM);
                this.EntityPM.AddConsignmentPackage(line);
                var item = new ConsigmentPackageModel(line,this.Tab);
                item.PackageMeasureQualifierCode = "2"
                item.PackageQuantity = arg.PacakgesQuantity;
                item.GrossMassMeasure = arg.TotalWheight;

                this.ConsimentPackages.Insert(item);
            }
        }
    }

    CourierBOLRequestMethod() {
        if (this.IsDisplayOnly) {
            //return;
        }

        if (!AppTool.IsNullOrEmpty(this.SecondCargoID)) {
            var secondCargoID: any = this.SecondCargoID;
            if (this.SecondCargoID.length != 9 || isNaN(secondCargoID) || this.SecondCargoID.indexOf('e') >= 0) {
                var messageWindow = new MessageWindow();
                messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show(TextCodeTranslator.Translate("Customs.Consignment.O.EnterNineDigitOnly"));
                return;
            }
        }

        let customsRequestMenuService = new CustomsRequestMenuService();
        let my = {
            "CustomFileNo": this.declarationPM.CustomFileNo,
            "CourierBOL": this.EntityPM.ManifestNumber,
            "CourierVAT": this.EntityPM.SecondCargoID,
            "DeclarationId": this.EntityPM.DeclarationId,
        };
        customsRequestMenuService.WindowClosed.subscribe((arg: any) => {
            if (!AppTool.IsNullOrEmpty(arg)) {
                this.GetDateFromString(arg);
            }
        });
        customsRequestMenuService.ShowModalAsEditMenuAction("9022", my);
    }

    GetDateFromString(cargoDate: string) {
        if (AppTool.IsNullOrEmpty(cargoDate)) {
            return "";
        }

        var day: number = Number(cargoDate.substring(0, 2));
        var month: number = Number(cargoDate.substring(2, 4));
        var year: number = Number("20" + cargoDate.substring(4, 6));

        this.CargoDate = DateTool.GetDate(year, month - 1, day, 0, 0, 0);
    }

    CargoQueryRequestMethod() {
        if (this.IsDisplayOnly && this.EntityPM.CargoTypeCode != "20") {
            //return;
        }

        if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
            //Save changes
            this.SaveChangesAndSendRequest();
        } else {
            this.SendCargoQueryRequestMethod();
        }
    }

    SendCargoQueryRequestMethod() {
        let customsRequestMenuService = new CustomsRequestMenuService();
        let my = {
            "Mode": "SendCargoQueryRequestFromDeclaration",
            "CargoTypeCode": this.EntityPM.CargoTypeCode,
            "ManifestNumber": this.EntityPM.ManifestNumber,
            "SecondCargoID": this.EntityPM.SecondCargoID,
            "DeclarationId": this.EntityPM.DeclarationId,
        };
        customsRequestMenuService.WindowClosed.subscribe((arg: any) => {
            if (!AppTool.IsNullOrEmpty(arg) && arg == "ReloadEntity") {
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }
        });
        customsRequestMenuService.ShowModalAsEditMenuAction("8240", my);
    }

    private SaveChangesAndSendRequest() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var sub =
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((myResult: any) => {
                sub.unsubscribe();
                var res: ServiceResponse = myResult;
                if (!res.HasError) {
                    var entity = res.Result;
                    console.log("..Saved Successfully ", entity);
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.SendCargoQueryRequestMethod();
                }
                else {
                    //this.ValidationErrorsList = res.ErrorsArray;
                }
                this.CurrentSession.StopBusyIndicator();
            });

        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    OnFocus() {
        if (this.ConsimentPackages.Length == 0) {
            this.AddPackageButtonClicked();
        }
    }

    AddSiteButtonClicked() {

        if (this.AddSiteEnabled && !this.IsDisplayOnly) {

            var consignmentNumber: number = 0;

            consignmentNumber = this.EntityPM.ConsignmentNumber;


            var lineNumber: number = 1;


            if (this.EntityPM.ConsignmentInternalTransitions.length != 0) {

                var maxObj = this.EntityPM.ConsignmentInternalTransitions.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current });
                if (maxObj != null) {
                    if (lineNumber <= maxObj.LineNumber)
                        lineNumber = maxObj.LineNumber;
                }

                lineNumber = lineNumber + 1;
            }

            var item: ConsignmentInternalTransitionPM = new ConsignmentInternalTransitionPM(this.EntityPM);
            item.ConsignmentNumber = consignmentNumber;
            item.DeclarationId = this.EntityPM.DeclarationId;
            item.Tenant = this.EntityPM.Tenant;
            item.LineNumber = lineNumber;


            if (!this.EntityPM.ConsignmentInternalTransitions.includes(item)) {
                //this.EntityPM.ConsignmentInternalTransitions.push(item);
                this.EntityPM.AddConsignmentInternalTransition(item);
            }

            this.AddSiteEnabled = false;
            this.BuildSitesList();
        }

    }

    BuildSitesList() {
        var count: number = 1;

        this.SiteList = [];

        this.AddSiteEnabled = true;
        for (var i = 0; i < this.EntityPM.ConsignmentInternalTransitions.length; i++) {
            var viewModel: ConsignmentInternalTransitionModel = new ConsignmentInternalTransitionModel(this.EntityPM.ConsignmentInternalTransitions[i], this);
            viewModel.TransitionNumber = i + 1;
            if (viewModel.SiteCode == null) {
                this.AddSiteEnabled = false;
            }
            this.SiteList.push(viewModel);


        }

        if (this.SiteList.length == 0) {
            var item = new ConsignmentInternalTransitionPM(this.EntityPM);
            var viewModel: ConsignmentInternalTransitionModel = new ConsignmentInternalTransitionModel(item, this);
            viewModel.TransitionNumber
            this.SiteList.push(viewModel);
            //  this.EntityPM.AddConsignmentInternalTransition(item);


            this.AddSiteEnabled = false;
        }


    }

    ApplyPadding(str: string) {
        var pad = "00"
        var ans = pad.substring(0, pad.length - str.length) + str
        return ans;
    }

    CheckRequrierdFieldsForSend() {
        var isExport = false;
        if (this.declarationPM.Direction == 'E') {
            isExport = true;
        }
        var customsRequiredFieldListService: CustomsRequiredFieldListService = new CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(d => d.Name == 'Customs.Consignment')[0];
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");

        var customsRequiredFieldExtendedListService: CustomsRequiredFieldExtendedListService = new CustomsRequiredFieldExtendedListService();
        filters = customsRequiredFieldExtendedListService.GetFilter(filters, isExport)


        customsRequiredFieldListService.getAllFromCache(filters).subscribe((response: ServiceResponse) => {
            var requiredFields = response.Result;
            requiredFields.forEach((field) => {
                var objectField = window.ObjectFields.filter(d => d.FieldCode == field.ObjectfieldCode)[0];
                this.UIProperties.SetWarning(objectField.FieldName, 'Customs.Consignment', true);
            });
        });
    }


    async onBlurCargoId(cargoNumber: 'a' | 'b' | 'c', newVlue: string) {
        if (
            this.declarationPM.Direction !== 'E' ||
            this.declarationPM.TransportModeId !== 'O' ||
            !this.EntityPM.ExportStoragesId ||
            this.CargoIdKeyOrigin[cargoNumber] == newVlue) return;

        if (await this.ConfirmDisconnectExportStorage()) {
            this.updateCargoIdKeyOrigin();
            this.EntityPM.ExportStoragesId = null;
        } else
            switch (cargoNumber) {
                case 'a':
                    this.ManifestNumber = this.CargoIdKeyOrigin[cargoNumber]
                    break;
                case 'b':
                    this.SecondCargoID = this.CargoIdKeyOrigin[cargoNumber]
                    break;
                case 'c':
                    this.ThirdCargoID = this.CargoIdKeyOrigin[cargoNumber]
                    break;
            }
    }

    async ConfirmDisconnectExportStorage() {
        const confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Customs.Declaration.O.ConnectedDelcaration") + '\n' + TextCodeTranslator.Translate("Customs.Declaration.O.ChangeCargoId"));

        return new Promise<boolean>((resolve, reject) => {
            confirmWindow.WindowClosed.subscribe((event: any) => resolve(confirmWindow.Yes));
        });
    }
}

export class ConsignmentType {
    public Id: string;
    public Value: string;
}
export class ConsigmentPackageModel extends BaseComponent {
    public EntityPM: ConsignmentPackagePM;
    private CurrentSession = SessionLocator.SelectedSession;
    public Tab: LogTab;

    constructor(line: ConsignmentPackagePM ,Tab: LogTab) {
        super();
        this.EntityPM = line;
        this.Tab=Tab;
    }

    //#region Properties
    public get PackageMeasureQualifierCode() { return this.EntityPM.PackageMeasureQualifierCode; }
    public set PackageMeasureQualifierCode(newValue: string) { this.EntityPM.PackageMeasureQualifierCode = newValue; }

    public get PackageMeasureQualifierName() { return this.EntityPM.PackageMeasureQualifierName; }
    public set PackageMeasureQualifierName(newValue: string) { this.EntityPM.PackageMeasureQualifierName = newValue; }

    public get PackageTypeName() { return this.EntityPM.PackageTypeName; }
    public set PackageTypeName(newValue: string) { this.EntityPM.PackageTypeName = newValue; }

    public get PackageTypeCode() { return this.EntityPM.PackageTypeCode; }
    public set PackageTypeCode(newValue: string) { this.EntityPM.PackageTypeCode = newValue; }

    public get MarksNumbers() { return this.EntityPM.MarksNumbers; }
    public set MarksNumbers(newValue: string) { 
        this.EntityPM.MarksNumbers = newValue; 
       
        if(this.CurrentSession.CurrentEditComponent.EntityPM.Direction=='E'&&this.CurrentSession.CurrentEditComponent.EntityPM.TransportModeId=='O') {

            this.Tab.Title =(this.Tab.EntityPM.consignmentPackages[0].MarksNumbers.replace("\n","") + '-')  + this.Tab.EntityPM.SequenceNumeric;
        }
    }

    public get GrossMassMeasure() { return this.EntityPM.GrossMassMeasure; }
    public set GrossMassMeasure(newValue: number) { this.EntityPM.GrossMassMeasure = newValue; }

    public get GrossMassMeasureTypeCode() { return this.EntityPM.GrossMassMeasureTypeCode; }
    public set GrossMassMeasureTypeCode(newValue: string) { this.EntityPM.GrossMassMeasureTypeCode = newValue; }

    public get GrossMassMeasureTypeName() { return this.EntityPM.GrossMassMeasureTypeName; }
    public set GrossMassMeasureTypeName(newValue: string) { this.EntityPM.GrossMassMeasureTypeName = newValue; }

    public get PackageQuantity() { return this.EntityPM.PackageQuantity; }
    public set PackageQuantity(newValue: number) { this.EntityPM.PackageQuantity = newValue; }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}

export class ConsignmentInternalTransitionModel extends BaseComponent {
    public EntityPM: ConsignmentInternalTransitionPM;
    private Parent: ConsigmentTabContentComponent;
    ObjectTableName: string = "Customs.ConsignmentInternalTransition";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(item: ConsignmentInternalTransitionPM, parent: ConsigmentTabContentComponent) {
        super();
        this.EntityPM = item;
        this.Parent = parent;
        this.UIProperties.SetEnabled("SiteCode", this.ObjectTableName, !this.Parent.IsDisplayOnly);

    }

    //#region Properties
    deleteSiteVisible: boolean = false;
    public get DeleteSiteVisible() { return this.deleteSiteVisible; }
    public set DeleteSiteVisible(newValue: boolean) { this.deleteSiteVisible = newValue; }

    transitionNumber: number = 1;
    public get TransitionNumber() { return this.transitionNumber; }
    public set TransitionNumber(newValue: number) { this.transitionNumber = newValue; }

    public get LineNumber() { return this.EntityPM.LineNumber; }
    public set LineNumber(newValue: number) { this.EntityPM.LineNumber = newValue; }
    private timerToken: any;

    SiteCodeChanged(item:any){
        var MyInternalBorderSiteTypeListService: InternalBorderSiteTypeListService = new InternalBorderSiteTypeListService();

        MyInternalBorderSiteTypeListService.getSingleFromCache(this.EntityPM.SiteCode).subscribe((myResponse: ServiceResponse) => {
           
            
        });
    }
    public get SiteCode() { return this.EntityPM.SiteCode; }
    public set SiteCode(newValue: string) {
        this.EntityPM.SiteCode = newValue;
        if (newValue != null) {
            if (this.Parent.SiteList.length == 1) {
                this.Parent.EntityPM.AddConsignmentInternalTransition(this.EntityPM);
            }
            this.Parent.AddSiteEnabled = true;
            if(AppTool.IsNullOrEmpty(this.EntityPM.EntityParentPM.EntityParentPM.AutonomyRegionTypeCode) && this.EntityPM.EntityParentPM.EntityParentPM.IsCourierDeclaration){
                let internalBorderSiteTypePMService: InternalBorderSiteTypePMService = new InternalBorderSiteTypePMService();
                internalBorderSiteTypePMService.get(this.SiteCode).subscribe( response =>{
                    if(!response.HasError){
                        this.EntityPM.EntityParentPM.EntityParentPM.AutonomyRegionTypeCode = response.Result?.autonomyRegionTypeCode;
                    }
                })
            }
        }
        else {
            this.Parent.AddSiteEnabled = false;
        }

    }
 
    //#endregion

    OnMouseOver() {
        if (this.EntityPM.LineNumber > 1) {
            this.DeleteSiteVisible = true;
        }
    }

    OnMouseLeave() {
        if (!this.overCloseButton) {
            this.DeleteSiteVisible = false;
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


    DeleteSiteButtonClicked() {

        var msg = TextCodeTranslator.Translate("Customs.Declaration.O.DeleteSite");
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Height = 150;
        confirmWindow.Show(msg);
        confirmWindow.WindowClosed.subscribe((event: any) => {

            if (confirmWindow.Yes) { // YES
                this.Parent.EntityPM.RemoveConsignmentInternalTransition(this.EntityPM);
                this.Parent.BuildSitesList();
            }
        });
    }
}
