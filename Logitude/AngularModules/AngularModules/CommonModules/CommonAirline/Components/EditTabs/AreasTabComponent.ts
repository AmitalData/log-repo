import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {TarrifHeaderPM} from '../../../../Common/EntityPMs/TarrifHeaderPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {DateTimeToDatePipe} from '../../../../Controls/Pipes/DateTimeToDatePipe';
import {TarrifFromToTypePM} from '../../../../Common/EntityPMs/TarrifFromToTypePM';
import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {AppTool, DateTool, FontTool} from '../../../../Infrastructure/Tools';
import {TarrifChargePM} from '../../../../Common/EntityPMs/TarrifChargePM';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AreasTabComponent.html',
})

export class AreasTabComponent extends BaseComponent implements OnInit {
    public EntityPM: AirlinePM;
    public ObsList: TariffHeaderItem[];
    public ObjectTableName: string = "Airline";
    public TenantPM: TenantPM;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ShowActiveTarrifsString: string = "";
    public ShowAllTarrifsString: string = "";
    public AddSurchargeTarrif: string = "";
    public TarrifHeaderDate: string = "";
    public TarrifHeaderCreateDate: string = "";
    public TarrifHeaderFromLocation: string = "";
    public TarrifHeaderToLocation: string = "";
    public TarrifHeaderNotes: string = "";
    public ActiveTariff: string = "";
    public AllTariff: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("TarrifHeader", 0).subscribe(response => {
            this.EntityPM = entityArgs.EntityPM;
            this.TenantPM = SessionLocator.TenantPM;
            this.setLabels();
            this.ObsList = [];
            this.FillData();

            if (this.CurrentSession == null) {
                this.ActiveTariff = "Active_-1_-1";
                this.AllTariff = "All_-1_-1";
            }

            else {
                var idIndex = this.CurrentSession.GetNewId("RadioButton");

                this.ActiveTariff = "Active_" + idIndex;
                this.AllTariff = "All_" + idIndex;

            }
        });

    }
   private setLabels() {
       this.ShowActiveTarrifsString = TextCodeTranslator.Translate('TarrifHeader.O.ShowActiveTarrifs'); 
       this.ShowAllTarrifsString = TextCodeTranslator.Translate('TarrifHeader.O.ShowAllTarrifs'); 
       this.AddSurchargeTarrif = TextCodeTranslator.Translate('TarrifHeader.O.AddSurchargeTarrif');
       this.TarrifHeaderDate = TextCodeTranslator.Translate('TarrifHeader.O.Date');
       this.TarrifHeaderCreateDate = TextCodeTranslator.Translate('TarrifHeader.O.CreateDate');
       this.TarrifHeaderFromLocation = TextCodeTranslator.Translate('TarrifHeader.O.FromLocation');
       this.TarrifHeaderToLocation = TextCodeTranslator.Translate('TarrifHeader.O.ToLocation');
       this.TarrifHeaderNotes = TextCodeTranslator.Translate('TarrifHeader.O.Notes');
       var test = TextCodeTranslator.Translate('TarrifHeader.F.MeasurementId');


    }

   OpenEdit(item: TariffHeaderItem) {
       var itemPM = item.EntityPM;    
       var Detector: ChangeDetectorRef;

       var itemViewModel = new TariffHeaderItem(itemPM, false, this, Detector);
     //  var itemviewmodel2: TariffHeaderItem = Object.assign({}, itemViewModel);
     //  itemviewmodel2.InActive = true;
     //  console.log(itemViewModel.InActive);
     //  console.log(itemviewmodel2.InActive);
       this.RunNewWindow(itemViewModel, TextCodeTranslator.Translate("TarrifHeader.O.EditSurchargeTarrif"));

   }


    ngOnInit() {
       
    }

    FillData() {
        this.LoadTarrifHeaders();
    }

    private LoadTarrifHeaders() {

        if (this.ObsList == null) {
            this.ObsList = new Array<TariffHeaderItem>();
        }

        else {
            this.ObsList = [];
        }

        var list: TarrifHeaderPM[] = new Array<TarrifHeaderPM>();
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetTarrifHeadersByCardIdAndTypeCode(this.EntityPM.Id, "S", this.ShowAllTarrifs).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                list = myResponse.Result;

                list.forEach((item) => {
                   // item.OldEntityPM = item;
                    var Detector: ChangeDetectorRef;

                    var itemViewModel: TariffHeaderItem = new TariffHeaderItem(item, false, this, Detector);
                    this.ObsList.push(itemViewModel);
                })
            }
        });
    }
    ClickAllTariff() {
        this.ShowAllTarrifs = true;
        this.ShowActiveTarrifs = false;
    }
    ClickShowActiveTarrifs() {
        this.ShowAllTarrifs = false;
        this.ShowActiveTarrifs = true;
    }

    CheckColor(item: TariffHeaderItem) {
        if (item.InActive)
            return '#FCDCDC';       
        return "";
    }
   
    // Props 
    private showAllTarrifs: boolean = false;
    public get ShowAllTarrifs() { return this.showAllTarrifs; }
    public  set ShowAllTarrifs(newValue: boolean) {
        if (this.showAllTarrifs != newValue) {
            this.showAllTarrifs = newValue;
            this.FillData();
        }
    }

    private showActiveTarrifs: boolean = true;
   public get ShowActiveTarrifs() { return this.showActiveTarrifs; }
   public  set ShowActiveTarrifs(newValue: boolean) {
        if (this.showActiveTarrifs != newValue) {
            this.showActiveTarrifs = newValue;
        }
   }


     private myCloner: Cloner;
     private Clone(item: TariffHeaderItem) {
         //this.myCloner = new Cloner(item);
         //this.myCloner.AddField("FromDate");
         //this.myCloner.AddField("ToDate");
      //   this.myCloner.AddField("TarrifCharges");
         //this.myCloner.AddEntity(item.fatherComponent.EntityPM);
        // this.myCloner.AddEntity(item.EntityPM);

       //  this.myCloner.AddEntity(this.EntityPM);
       //  this.myCloner.AddEntity(item);
       //  this.myCloner.AddField("TarrifCharges");

    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    //Commands
    public AddSurchargeTariff() {
        var todayDate: Date = new Date();
        var itemPM = new TarrifHeaderPM();
        itemPM.Tenant = this.TenantPM.Id;
        itemPM.TarrifTypeCode = "S";
        itemPM.CardId = this.EntityPM.Id;
        itemPM.CreateDate = todayDate;
        itemPM.InActive = false;
        itemPM.FromLocationCode = "A";
        itemPM.ToLocationCode = "A";
        itemPM.FromLocationString = "Anywhere";
        itemPM.ToLocationString = "Anywhere";
        var Detector: ChangeDetectorRef;
        var itemViewModel = new TariffHeaderItem(itemPM, true, this, Detector);
        this.RunNewWindow(itemViewModel, TextCodeTranslator.Translate("TarrifHeader.O.AddSurchargeTarrif"));
    }
    //TarrifHeader.O.EditSurchargeTarrif

    private RunNewWindow(itemComponent: TariffHeaderItem, windowTitle: string) {
        this._entityResourceService.getEntityResourceByTableName("TarrifCharge", 0).subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("TarrifHeader", 0).subscribe(response => {
                this.Clone(itemComponent);
                var logitudeWindow = new LogitudeWindow();
                logitudeWindow.Title = windowTitle;
                logitudeWindow.IsFillScreen = true;
                logitudeWindow.DataContext = itemComponent;
                logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnNewTariffWindowClosed($event));
                logitudeWindow.Show("./CommonModules/CommonAirline/Components/AddEdit/AddEditTarrifHeaderComponent");
            });

        });
      
    }

    OnNewTariffWindowClosed(arg: any) {
        this.LoadTarrifHeaders();
    }
}

export class TariffHeaderItem extends BaseComponent {
    public EntityPM: TarrifHeaderPM;
    public IsNewEntity: boolean;
    public ObjectTableName = "TarrifHeader";
    public FromToTypeList: Array<FromToType>;
    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) { this.EntityPM.InActive = value; }
    public set TarrifCharges(value: Array<TarrifChargePM>) { if (value != null) this.EntityPM.TarrifCharges = value; }
    public get TarrifCharges() { return this.EntityPM.TarrifCharges; }
    constructor(entityPM: TarrifHeaderPM, private isNew: boolean, public fatherComponent: AreasTabComponent, public CD: ChangeDetectorRef) {
        super();
        this.FromToTypeList = new Array<FromToType>();
        var Tarrifi: FromToType = new FromToType();
        Tarrifi.Code = "A"; Tarrifi.Name = TextCodeTranslator.Translate('TarrifHeader.O.Anyware');
        Tarrifi.TypeIsEnabled = true;
        this.FromToTypeList.push(Tarrifi);
        Tarrifi = new FromToType();
        Tarrifi.Code = "P"; Tarrifi.Name = TextCodeTranslator.Translate('TarrifHeader.O.PortsList');
        this.FromToTypeList.push(Tarrifi);
        Tarrifi = new FromToType();
        Tarrifi.Code = "C"; Tarrifi.Name = TextCodeTranslator.Translate('TarrifHeader.O.PortsList');
        this.FromToTypeList.push(Tarrifi);
        this.EntityPM = entityPM;
        this.IsNewEntity = isNew;
        this.GetDatesText();
        this.GetDateStatus();
    }

    public SetUIProperties() {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, this.IsDatePickerEnabled);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, this.IsDatePickerEnabled);
    }

    public FromOldDateText: string;
    public ToOldDateText: string;
    GetDatesText() {

        this.FromOldDateText = null;
        this.ToOldDateText = null;

        if (this.NewDatesIsChecked) {
            this.FromOldDateText = "Old Date: " +  DateTimeToDatePipe.Pipe(this.savedFromDate);
            this.ToOldDateText = "Old Date: " + DateTimeToDatePipe.Pipe(this.savedToDate);
        }
    }

    private makeInActiveIsChecked: boolean = false;
    public get MakeInActiveIsChecked() {
        if (this.EntityPM.InActive)
            this.makeInActiveIsChecked = this.EntityPM.InActive;

        return this.makeInActiveIsChecked;
    }
    public set MakeInActiveIsChecked(value: boolean) {
        this.makeInActiveIsChecked = value;
    }


    public get MakeInActiveVisibility() { return this.EntityPM.Id == null ? false : true; }
    public get MakeInActiveIsEnabled() { return !this.EntityPM.InActive; }
    public get FromRedDotVisibility() { return this.FromDate == null ? true : false; }

    public get FromLocationCode() { return this.EntityPM.FromLocationCode; }
    public set FromLocationCode(value: string) {
        if (this.EntityPM.FromLocationCode != value)
            this.EntityPM.FromLocationCode = value;
    }

    public get FromLocationString() { return this.EntityPM.FromLocationString; }
    public set FromLocationString(value: string) { this.EntityPM.FromLocationString = value; }
    public get ToLocationCode() { return this.EntityPM.ToLocationCode; }
    public set ToLocationCode(value: string) { this.EntityPM.ToLocationCode = value; }

    private selectedFromType: FromToType;
    public get SelectedFromType() {
        this.selectedFromType = this.FromToTypeList.filter(p => p.Code == this.EntityPM.FromLocationCode)[0];
        return this.selectedFromType;
    }

    public set SelectedFromType(value: FromToType) { value != null ? this.FromLocationCode = value.Code : -1; }


    private selectedToType: FromToType;
    public get SelectedToType() {
        this.selectedToType = this.FromToTypeList.filter(p => p.Code == this.EntityPM.ToLocationCode)[0];
        return this.selectedToType;
    }

    public get AddFromPortIsEnabled() { return !AppTool.IsNullOrEmpty(this.FromPortId); }

    public set SelectedToType(value: FromToType) { }

    public get FromPortsAreaVisibility() { return this.FromLocationCode == "P" ? true : false; }
    public get ToPortsAreaVisibility() { return this.ToPortsAreaVisibility == "P" ? true : false; }

    //Props
    private fromPortId: string;
    public get FromPortId() { return this.fromPortId; }
    public set FromPortId(value: string) { this.fromPortId = value; }
    public savedFromDate: Date = null;
    public savedToDate: Date = null;
    private IsNew: boolean = false;

    private toPortId: string;
    public get ToPortId() { return this.toPortId; }
    public set ToPortId(value: string) { this.toPortId = value; }

    public get IsDatePickerEnabled() {
        var result = this.IsNewEntity || this.NewDatesIsChecked;
        return result;
    }


    public DateStatusColor: string;

    public DateStatus: string;
    public GetDateStatus() {

        if (!AppTool.IsNullOrEmpty(this.FromDate) && !AppTool.IsNullOrEmpty(this.ToDate)) {
            var todayDateTicks = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateTicks;
            var fromDateTicks = DateTool.GetDateParts(this.FromDate).DateTicks;
            var toDateTicks = DateTool.GetDateParts(this.ToDate).DateTicks;

            if (todayDateTicks >= fromDateTicks && todayDateTicks <= toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator.Translate("General.O.Present") + ")";
                this.DateStatusColor = FontTool.Green;
            }

            else if (todayDateTicks < fromDateTicks && todayDateTicks < toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator.Translate("General.O.Future") + ")";
                this.DateStatusColor = FontTool.Magenta;
            }

            else if (todayDateTicks > fromDateTicks && todayDateTicks > toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator.Translate("General.O.Past") + ")";
                this.DateStatusColor = FontTool.Red;
            }
        }

    }


    private newDatesIsChecked: boolean = false;
    public get NewDatesIsChecked() { return this.newDatesIsChecked; }
    public set NewDatesIsChecked(newValue: boolean) {
        if (this.newDatesIsChecked != newValue) {
            this.newDatesIsChecked = newValue;
            if (newValue) {
                this.FromDate = null;
                this.ToDate = null;

            }
            else {
                this.FromDate = this.savedFromDate;
                this.ToDate = this.savedToDate;

            }
        }
    }

    public get ToLocationString() { return this.EntityPM.ToLocationString; }
    public set ToLocationString(value: string) { this.EntityPM.ToLocationString = value; }


    public get FromDate() {
        return this.EntityPM.FromDate;
    }
    public set FromDate(value: Date) {
        if (this.EntityPM.FromDate != value) {
            this.EntityPM.FromDate = value;
            //  this.CD.detectChanges();
        }
    }



    public get Notes() { return this.EntityPM.Notes; }
    public set Notes(value: string) { this.EntityPM.Notes = value; }

    public get TransitTimeNotes() { return this.EntityPM.TransitTimeNotes; }
    public set TransitTimeNotes(value: string) { this.EntityPM.TransitTimeNotes = value; }


    public get FromDateInDate() {


        if (this.EntityPM.FromDate != null) {
            if (this.EntityPM.FromDate instanceof Date)
                return this.EntityPM.FromDate;
            return DateTool.GetDateParts(this.EntityPM.FromDate).DateObject;
        }
        else
            return null;


    }

    public get ToDateInDate() {

        if (this.EntityPM.ToDate != null) {
            if (this.EntityPM.ToDate instanceof Date)
                return this.EntityPM.ToDate;
            return DateTool.GetDateParts(this.EntityPM.FromDate).DateObject;

        }
        else return null;

    }

    public get ToDate() { return this.EntityPM.ToDate; }
    public set ToDate(value: Date) {
        if (this.EntityPM.ToDate != value) {
            this.EntityPM.ToDate = value;
        }
    }

    public get CreateDate() { return this.EntityPM.CreateDate; }
    public set CreateDate(value: Date) { this.EntityPM.CreateDate = value; }
}

export class FromToType {
    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }


    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }


    private typeIsEnabled: boolean;
    public get TypeIsEnabled() { return this.typeIsEnabled; }
    public set TypeIsEnabled(newValue: boolean) { this.typeIsEnabled = newValue; }



}
