declare var window: any;
import {OnDestroy,Component, ChangeDetectorRef}  from '@angular/core';
import {EntityArgs} from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool, DateTool} from '../../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import {ServiceResponse} from '../../../../../../Infrastructure/DataContracts/ServiceResponse';
import {ConfirmWindow} from '../../../../../../Controls/Windows/ConfirmWindow';
import {DeclarationPMService} from '../../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { ConsignmentInternalTransitionPM } from '../../../../../../Customs/EntityPMs/ConsignmentInternalTransitionPM';
import { ConsignmentPM } from '../../../../../../Customs/EntityPMs/ConsignmentPM';
import { CouriersVatPM } from '../../../../../../Customs/EntityPMs/CouriersVatPM';
import {DeclarationPM} from '../../../../../../Customs/EntityPMs/DeclarationPM';
import {ConsignmentPackagePM} from '../../../../../../Customs/EntityPMs/ConsignmentPackagePM';
import {ClientList} from '../../../../../../Customs/EntityLists/ClientList';
import {LogTab} from '../../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import {CustomsRequiredFieldListService} from '../../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';
import { CustomsRequestMenuService } from '../../../../../../Customs/Services/Others/CustomsRequestMenuService';
import {DeliverySiteTypeListService} from '../../../../../../Customs/Services/StandardLists/DeliverySiteTypeListService';
import {DeclarationEventManager} from '../../../../../../Customs/Utilities/DeclarationEventManager'
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { ApiQueryFilters, FilterItem } from '../../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { CouriersVatPMService } from '../../../../../../Customs/Services/StandardPMs/CouriersVatPMService';
import { CouriersVatExtendedPMService } from '../../../../../../Customs/Services/ExtendedPMs/CouriersVatExtendedPMService';
import { MessageWindow } from '../../../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../../../Controls/Windows/LogitudeWindow';
import { WindowArgs } from '../../../../../../Infrastructure/DataContracts/WindowArgs';
import { CustomsRequiredFieldExtendedListService } from '../../../../../../Customs/Services/ExtendedLists/CustomsRequiredFieldExtendedListService';
 import { Dictionary } from '../../../../../../Infrastructure/GenericTypes/Dictionary';

@Component({
    selector: 'ConsigmentTabContent',
    
    templateUrl: './ConsigmentTabContentComponent.html',
})

export class ConsigmentTabContentComponent
    extends BaseComponent
    implements OnDestroy
{
    public EntityPM: ConsignmentPM;
    public declarationPM: DeclarationPM;

    public ObjectTableName: string = "Customs.Consignment";
    public DataContext: any = this;
    public Tab: LogTab;

    public IsDisplayOnly: boolean = false;
    public ParentIsDisplayOnly: boolean = false;
    ShowExcludeConsignmentBoolean: boolean = false;
    IsCourierDeclaration: boolean = false;

    public LoadingPortFilterItems: ApiQueryFilters;//38388

    // Edit grid array
    ConsimentPackages: ObservableCollection;

    SiteList: ConsignmentInternalTransitionModel[] = [];

    private _DeliverySiteTypeListService: DeliverySiteTypeListService = new DeliverySiteTypeListService();
    private _CouriersVatPMService: CouriersVatPMService = new CouriersVatPMService();
    private _CouriersVatExtendedPMService: CouriersVatExtendedPMService = new CouriersVatExtendedPMService();
    private _DeclarationPMService: DeclarationPMService = new DeclarationPMService();

    public WeightValueFilterItems: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;

  public  ConsignmentTypes: ConsignmentType[] = [{ Id: "E", Value: "יצוא" }, { Id: "I", Value: "יבוא" }];
    constructor(public entityArgs: EntityArgs, private cd: ChangeDetectorRef) {
        super();
        this.ConsimentPackages = new ObservableCollection([]);
       // this.declarationPM = entityArgs.EntityPM;
        this.WeightValueFilterItems = new ApiQueryFilters();
        this.WeightValueFilterItems.addAdditionalFilter("Code", "CC,CA,NC,PO,PP", null, null, "InListExact", false, false, false, "string", false, true);

        this.SiteList = [];
        this.LoadingPortFilterItems = new ApiQueryFilters();//38388
        this.Listen();


    }
    private _SubDisplayModeChanged;
    private _SubConsignmentsChanged;

    public ConsignmentTypeSelectionChanged(value) {
        this.ConsignmentType = value;

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
        var windowTitle = "נתונים נוספים ליצוא - חטיבת משגור";

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
        this._SubDisplayModeChanged=
        DeclarationEventManager.DisplayModeChanged.subscribe((IsDisplayOnly: any) => {

            if (this.ShowExcludeConsignmentBoolean && this.ExcludeConsignment)
                this.IsDisplayOnly = true;
            else
                this.IsDisplayOnly = IsDisplayOnly;

            this.ParentIsDisplayOnly = IsDisplayOnly;

            this.SetScreenFieldsEditability();
            if (this.declarationPM.TransportModeId != 'O') {
                this.UIProperties.SetEnabled("ShipCode", this.ObjectTableName, this.IsDisplayOnly);
            }
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

        this.SetTipsInsideCargoIdentifires(this.EntityPM.CargoTypeCode);

        //this.EntityPM.PropertyChanged.subscribe((event) => { console.log("PropertyChanged: ", event); });

        if (!AppTool.IsNullOrEmpty(this.EntityPM.ConsignmentPackages)) {
            for (let pkg of this.EntityPM.ConsignmentPackages) {
                var item = new ConsigmentPackageModel(pkg);
                this.ConsimentPackages.Insert(item);
            }
        }
        if (this.IsDisplayOnly) {
            this.SetScreenFieldsEditability();
        }

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

        console.log("Tabs Args: ", args);
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
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsLastReleaseFromWarehous", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("LoadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeliveryPlaceName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WeightValue", this.ObjectTableName, !this.IsDisplayOnly);
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
            this.LoadingPortFilterItems.addAdditionalFilter("CountryTypeCode", this.OriginCountryCode, null, null, "Equals", false, false, false, "string", false, true);
            //this.LoadingPortFilterItems.ForceCacheRefresh = true;
        }
        else {
            this.LoadingPortFilterItems.removeAdditionalFilter("CountryTypeCode");
        }
    }
    //#region Properties

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

    public get CargoDescription() { return this.EntityPM ? this.EntityPM.CargoDescription : null; }
    public set CargoDescription(newValue: string) { this.EntityPM.CargoDescription = newValue; }


    public get ConsignmentType() { return this.EntityPM ? this.EntityPM.ConsignmentType : null; }
    public set ConsignmentType(newValue: string) { this.EntityPM.ConsignmentType = newValue; }

    public get ShipCode() { return this.EntityPM ? this.EntityPM.ShipCode : null; }
    public set ShipCode(newValue: string) { this.EntityPM.ShipCode = newValue; }


    public get ThirdCargoID() { return this.EntityPM ? this.EntityPM.ThirdCargoID : null; }
    public set ThirdCargoID(newValue: string) { this.EntityPM.ThirdCargoID = newValue; }

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
    }

    public get IsLastReleaseFromWarehous() { return this.EntityPM.IsLastReleaseFromWarehous == "T" ? true : false; }
    public set IsLastReleaseFromWarehous(newValue: boolean) { newValue ? this.EntityPM.IsLastReleaseFromWarehous = "T" : this.EntityPM.IsLastReleaseFromWarehous = "N" ; }

    public get OriginalCountryCode() { return this.EntityPM ? this.EntityPM.OriginCountryCode : null; }
    public set OriginalCountryCode(newValue: string) { this.EntityPM.OriginCountryCode = newValue; }

    public get StorageSiteCode() { return this.EntityPM ? this.EntityPM.StorageSiteCode : null; }
    public set StorageSiteCode(newValue: string) { this.EntityPM.StorageSiteCode = newValue; }

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


    SetTipsInsideCargoIdentifires(value: string) {
        switch (value) {
            case '1':
                {
                    this.ManifestNumberPlaceholder = "הזן שנת טיסה";
                    this.SecondCargoIDPlaceholder = "הזן שט”מ ראשי";
                    this.ThirdCargoIdPlaceholder = "הזן שט”מ פנימי";
                    break;
                }
            case '2':
                {
                    this.ManifestNumberPlaceholder = "הזן מספר חבילה";
                    this.SecondCargoIDPlaceholder = "הזן שנת יצירת מטען";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
            case '8':
                {
                    this.ManifestNumberPlaceholder = "הזן הצהרת אחסנה";
                    this.SecondCargoIDPlaceholder = " ";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
            case '11':
                {
                    this.ManifestNumberPlaceholder = "הזן מצהר";
                    this.SecondCargoIDPlaceholder = " הזן מזהה עסקה";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
                }
            case '17':
                {
                    this.ManifestNumberPlaceholder = "הזן ש.מ בלדר";
                    this.SecondCargoIDPlaceholder = "הזן ח.פ בלדר";
                    this.ThirdCargoIdPlaceholder = "הזן תאריך הקמה";
                    break;
                }
            case '20':
                {
                    this.ManifestNumberPlaceholder = "הזן מזהה עסקה מלא";
                    this.SecondCargoIDPlaceholder = " ";
                    this.ThirdCargoIdPlaceholder = " ";
                    break;
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
    AddPackageButtonClicked() {
        var line = new ConsignmentPackagePM(this.EntityPM);
        this.EntityPM.AddConsignmentPackage(line);
        var item = new ConsigmentPackageModel(line);
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
                }
            });

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
                var item = new ConsigmentPackageModel(line);
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
            if(this.SecondCargoID.length != 9 || isNaN(secondCargoID) || this.SecondCargoID.indexOf('e') >= 0) {
                var messageWindow = new MessageWindow();
                messageWindow.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
                messageWindow.Width = 250;
                messageWindow.Height = 150;
                messageWindow.OkButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
                messageWindow.Show("יש להזין מספר בעל 9 ספרות בלבד בשדה מזהה מטען שני");
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

    SendCargoQueryRequestMethod(){
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
        var sub=
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((myResult:any) => {
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

}

export class ConsignmentType {
    public Id: string;
    public Value: string;
}
export class ConsigmentPackageModel extends BaseComponent {
    public EntityPM: ConsignmentPackagePM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(line: ConsignmentPackagePM) {
        super();
        this.EntityPM = line;
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
    public set MarksNumbers(newValue: string) { this.EntityPM.MarksNumbers = newValue; }

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


    public get SiteCode() { return this.EntityPM.SiteCode; }
    public set SiteCode(newValue: string) {
        this.EntityPM.SiteCode = newValue;
        if (newValue != null) {
            if (this.Parent.SiteList.length == 1) {
                this.Parent.EntityPM.AddConsignmentInternalTransition(this.EntityPM);
            }
            this.Parent.AddSiteEnabled = true;
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



