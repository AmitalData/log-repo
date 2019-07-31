import {Component, ChangeDetectorRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TarrifHeaderPM} from '../../../../Common/EntityPMs/TarrifHeaderPM';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {TarrifChargePM} from '../../../../Common/EntityPMs/TarrifChargePM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AirlineSurchargeTabComponent, TariffHeaderItem} from '../../../../CommonModules/CommonAirline/Components/EditTabs/AirlineSurchargeTabComponent';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {ChargesTypeList} from '../../../../Common/EntityLists/ChargesTypeList';
import {MeasurementList} from '../../../../Common/EntityLists/MeasurementList';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {ChargesTypeListService} from '../../../../Common/Services/StandardLists/ChargesTypeListService';
import {MeasurementListService} from '../../../../Common/Services/StandardLists/MeasurementListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {TarrifHeaderPMService} from '../../../../Common/Services/StandardPMs/TarrifHeaderPMService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ServiceLocator} from '../../../../Infrastructure/Locators/ServiceLocator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditTarrifHeaderComponent.html',
})

export class AddEditTarrifHeaderComponent extends BaseComponent {
    public ObjectTableName: string = "TarrifHeader";
    public DataContext: TariffHeaderItem;
    public EntityPM: TarrifHeaderPM;
    public TenantPM: TenantPM;
    public AirlinePM: AirlinePM;
    public SelectedItem: TariffChargeItem;
    public TarrifChargesObsList: Array<TariffChargeItem> = [];
    public FromToTypeList: FromToType[] = [];
    public tarrifChargePM: TarrifChargePM;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public ItemsSource: any;
    public NewDatesIsChecked: boolean = false;
    public DeleteTarrifChargeIsEnabled: boolean=false;
    public AddTarrifChargeString: string = "";
    public DeleteTarrifChargeString: string = "";
    public TariffRadioTo: string = "";
    public TariffRadio: string = "";
    public InActiveCheck: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        super();
        this.TenantPM = InfraSettings.TenantPM;

        if (this.CurrentSession == null) {
            this.TariffRadioTo = "RadioTo_-1_-1";
            this.TariffRadio = "RadioFrom_-1_-1";
            this.InActiveCheck = "Check_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("RadioButton");
            this.TariffRadioTo = "RadioTo_" + idIndex;
            this.TariffRadio = "RadioFrom_" + idIndex;
            this.InActiveCheck = "Check_" + idIndex;
        }
        
    }
    public ChargesTypeIdString: string = "";
    public MeasurementIdString: string = "";
    public CurrencyIdString: string = "";
    public UnitPriceString: string = "";
    public MinPriceString: string = "";
    public MaxPriceString: string = "";    
    setLabels() {

        this.AddTarrifChargeString = TextCodeTranslator.Translate('TarrifCharge.B.AddTarrifCharge');
        this.DeleteTarrifChargeString = TextCodeTranslator.Translate('TarrifCharge.B.DeleteTarrifCharge');
        this.ChargesTypeIdString = TextCodeTranslator.TranslateTablePlural('TarrifCharge.F.ChargesTypeId'); //ChargeType
        this.MeasurementIdString = TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.MeasurementId'); //MeasurementCode
        this.CurrencyIdString = TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.CurrencyId');//CurrencyCode
        this.UnitPriceString = TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.UnitPrice');//Unit Price
        this.MinPriceString = TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.MinPrice');//Min
        this.MaxPriceString = TextCodeTranslator.TranslateTablePlural('TarrifHeader.F.MaxPrice');//Max
    }

   

    SetDataContext(dataContext: TariffHeaderItem) {
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        //this.SetLabels();
        this.setLabels();
        this.FillFromToTypeList();
        this.IsNew = dataContext.IsNewEntity;
        if (!this.IsNew) {
            this.DataContext.savedFromDate = this.DataContext.FromDate;
            this.DataContext.savedToDate = this.DataContext.ToDate;
            this.EntityPM.TarrifCharges.forEach(p => {
                var item: TariffChargeItem = new TariffChargeItem(p, false, this);
                this.TarrifChargesObsList.push(item);
            });

        }

       

        this.DataContext.SetUIProperties();
    }

    public OldFromDate: string = "";
    public OldToDate: string = "";
    NewDateIsClicked() {
        this.DataContext.NewDatesIsChecked = !this.NewDatesIsChecked;
        this.DataContext.SetUIProperties();    
        this.OldFromDate = this.DataContext.FromOldDateText;
        this.OldToDate = this.DataContext.ToOldDateText;
        this.CD.detectChanges();

    }

    private FillFromToTypeList() {

        if (this.FromToTypeList == null) {
            this.FromToTypeList = [];
        }

        var item1: FromToType = new FromToType();
        item1.Code = "A";
        item1.Name = TextCodeTranslator.Translate("TarrifHeader.O.Anyware");
        item1.TypeIsEnabled = true;
        this.FromToTypeList.push(item1);

        var item2: FromToType = new FromToType();
        item2.Code = "P";
        item2.Name = TextCodeTranslator.Translate("TarrifHeader.O.PortsList");
        item2.TypeIsEnabled = false;
        this.FromToTypeList.push(item2);

        var item3: FromToType = new FromToType();
        item3.Code = "C";
        item3.Name = TextCodeTranslator.Translate("TarrifHeader.O.CountriesList");
        item3.TypeIsEnabled = false;
        this.FromToTypeList.push(item3);
    }
    private isNew: boolean = false;
    public get NewDatesIsCheckedIsEnabled() { return !this.isNew; }
    public get IsNew() { return this.isNew; }
    public set IsNew(value: boolean) { this.isNew = value; }
    public BuildData() {
        this.TarrifChargesObsList = [];
        this.EntityPM.TarrifCharges.forEach((item) => {
            var model = new TariffChargeItem(item, false,this);
            this.TarrifChargesObsList.push(model);
        })
    }

    InActiveClick() {
        this.InActive = !this.InActive;
    }

    public inActive: boolean;
    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) { this.inActive = value; }


    //Tarrif Charges
    public AddTarrifCharge() {
        var itemPM = new TarrifChargePM(this.EntityPM);
        itemPM.Tenant = this.TenantPM.Id;
        itemPM.TarrifHeaderId = this.EntityPM.Id;
        var itemViewModel = new TariffChargeItem(itemPM, true, this);
        this.RunChargeWindow(itemViewModel, TextCodeTranslator.Translate("TarrifCharge.B.AddTarrifCharge"));
    }

    public EditCharge(itemViewModel: TariffChargeItem) {
        this.RunChargeWindow(itemViewModel, TextCodeTranslator.Translate("TarrifCharge.O.EditTarrifCharge"));
    }

    private RunChargeWindow(itemComponent: TariffChargeItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/AddEditTariffChargeComponent');
    }



    //Commands 
    public ValidationErrorsList: string[] = [];
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");

    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.DataContext.EntityPM, this.DataContext.ObjectTableName, errors);

        if (this.DataContext.FromDate > this.DataContext.ToDate) {
            errors.push("From Date should be less than To Date");
        }

        if (this.TarrifChargesObsList.length == 0) {
            errors.push("You must add at least one Charge");
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.IsNewEntity) {
                ServiceLocator.SendTotangoUserActivity("Airline", "Tariff Added");
                this.DataContext.fatherComponent.ObsList.push(this.DataContext);
            }
            else
                ServiceLocator.SendTotangoUserActivity("Airline", "Tariff Edited");

            this.SubmitChanges();



        }

    }

    SubmitChanges() {
        //this.EntityPM.TarrifCharges = [];
        //this.TarrifChargesObsList.forEach(p => {
        //    this.EntityPM.AddTarrifChargePM(this.MapFromTarrifChargeItemToPM(p));
        //});
        //console.log(this.EntityPM);
        if (this.inActive != null)
            this.EntityPM.InActive = this.inActive;
        else
            this.EntityPM.InActive = this.InActive;

        var myService: TarrifHeaderPMService = new TarrifHeaderPMService();

           if (this.DataContext.NewDatesIsChecked)
                       this.IsNew = true;

     
           if (!this.IsNew) {
            myService.update(this.EntityPM).subscribe(myResult => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
                , error => {
                    this.CurrentSession.StopBusyIndicator();
                    //var dd: Response = error;
                    //console.log(dd.text);
                });
        }
        else {
               this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
            myService.insert(this.EntityPM).subscribe(myResult => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
                , error => {
                    this.CurrentSession.StopBusyIndicator();
                    //var dd: Response = error;
                    //console.log(dd.text);
                });
        }

        }
    

    public DeleteTarrifCharge() {
        var tempArr: Array<TariffChargeItem> = [];
        if (this.SelectedItem != null) {
           // this.EntityPM.RemoveTarrifChargePM();
            this.TarrifChargesObsList.forEach(p => {
                if (p != this.SelectedItem)
                    tempArr.push(p);
                else
                {
                    this.EntityPM.RemoveTarrifChargePM(this.MapFromTarrifChargeItemToPM(p));
                }
            });
            this.TarrifChargesObsList = tempArr;
        }
        this.DeleteTarrifChargeIsEnabled = false;

    }

    MapFromTarrifChargeItemToPM(item: TariffChargeItem) {
        return item.EntityPM;
    }
}

export class FromToType {
    public Code: string;
    public Name: string;
    public TypeIsEnabled: boolean;
}

export class QueryFilterItem {

    FieldName: string;
    FieldValue: any;
    FieldValue2: any;
    FieldValue3: any;
    Operator: string;
    IsCustom: boolean;
    DisplayInList: boolean;
    IsCustomField: boolean;
    FieldDataType: string;
}

export class TariffChargeItem extends BaseComponent {

    public EntityPM: TarrifChargePM;
    public TarrifHeaderPM: TarrifHeaderPM;
    public IsNewEntity: boolean;
    public ObjectTableName: string = "TarrifCharge";
    public TenantPM: TenantPM;
    public ChargeTypesQueryFilters: QueryFilterItem[] = [];
   
    constructor(entityPM: TarrifChargePM, private isNew: boolean, public fatherComponent: AddEditTarrifHeaderComponent) {
        super();
        this.EntityPM = entityPM;
      

        this.TarrifHeaderPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.TenantPM = InfraSettings.TenantPM;
        this.LoadCachLists();
        this.InserQueryFilterItem();
    }

    private LoadCachLists() {
        this.LoadChargesTypeListMethod();
        this.LoadMeasurementListMethod();
        this.LoadCurrencyListMethod();
    }

    private InserQueryFilterItem() {

        if (this.TarrifHeaderPM.TarrifTypeCode == "S") {
            var query = new QueryFilterItem();
            query.FieldName = "ChargesGroupCode";
            query.FieldValue = "SCH";
            query.Operator = "Equals";

            if (this.ChargeTypesQueryFilters == null) {
                this.ChargeTypesQueryFilters = [];
            }

            this.ChargeTypesQueryFilters.push(query);
        }
    }

    //Cach Lists
    public ChargesTypeList: ChargesTypeList[] = [];
    private LoadChargesTypeListMethod() {
        var myService: ChargesTypeListService = new ChargesTypeListService();
        myService.getAll().subscribe((myResult:ServiceResponse) => {
            this.ChargesTypeList = myResult.Result;
        });
    }

    public MeasurementList: MeasurementList[] = [];
    private LoadMeasurementListMethod() {
        var myService: MeasurementListService = new MeasurementListService();
        myService.getAll().subscribe((myResult: ServiceResponse) => {
            this.MeasurementList = myResult.Result;
        });
    }

    public CurrencyList: CurrencyList[] = [];
    private LoadCurrencyListMethod() {
        var myService: CurrencyListService = new CurrencyListService();
        myService.getAll().subscribe((myResult: ServiceResponse) => {
            this.CurrencyList = myResult.Result;
        });
    }

    //Props 
    get ChargesTypeId() { return this.EntityPM.ChargesTypeId; }
    set ChargesTypeId(value: string) {
        if (this.EntityPM.ChargesTypeId != value) {
            this.EntityPM.ChargesTypeId = value;
            this.GetChargesTypeData();
        }
    }

    GetChargesTypeData() {

        if (this.EntityPM.ChargesTypeId == null) {
            this.ChargesGroupCode = null;
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.MeasurementId = null;
            this.CurrencyId = null;
        }
        else {

            var list: ChargesTypeList = this.ChargesTypeList.filter(d => d.Id == this.EntityPM.ChargesTypeId)[0];
            if (list != null) {
                this.ChargesGroupCode = list.ChargesGroupCode;
                this.ChargesTypeCode = list.Code;
                this.ChargesTypeName = list.EnglishName;
                this.MeasurementId = list.MeasurementId;

                if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                    this.CurrencyId = this.TenantPM.FreightCurrencyId;
                }

                else {
                    this.CurrencyId = this.TenantPM.OtherChargesCurrencyId;
                }
            }
        }
    }

    private chargesGroupCode: string;

    public get ChargesGroupCode() { return this.chargesGroupCode; }
   public  set ChargesGroupCode(value: string) {
        if (this.chargesGroupCode != value) {
            this.chargesGroupCode = value;
        }
    }

    get ChargesTypeCode() { return this.EntityPM.ChargesTypeCode; }
    set ChargesTypeCode(value: string) {
        if (this.EntityPM.ChargesTypeCode != value) {
            this.EntityPM.ChargesTypeCode = value;
        }
    }

    get ChargesTypeName() { return this.EntityPM.ChargesTypeName; }
    set ChargesTypeName(value: string) {
        if (this.EntityPM.ChargesTypeName != value) {
            this.EntityPM.ChargesTypeName = value;
        }
    }

    public get ChargeType() {
        var str: string = null;
        if (!AppTool.IsNullOrEmpty(this.EntityPM.ChargesTypeId)) {
            str = "(" + this.ChargesTypeCode + ") " + this.ChargesTypeName;
        }

        return str;
    }

    get MeasurementId() { return this.EntityPM.MeasurementId; }
    set MeasurementId(value: string) {
        if (this.EntityPM.MeasurementId != value) {
            this.EntityPM.MeasurementId = value;
            this.GetMeasurementData();
        }
    }

    private GetMeasurementData() {
        if (this.EntityPM.MeasurementId == null) {
            this.MeasurementCode = null;
        }

        else {
            var list: MeasurementList = this.MeasurementList.filter(d => d.Id == this.EntityPM.MeasurementId)[0];
            if (list != null) {
                this.MeasurementCode = list.Code;
            }
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
            this.GetCurrencyData();
        }
    }

    private GetCurrencyData() {

        if (this.EntityPM.CurrencyId == null) {
            this.CurrencyCode = null;
        }

        else {
            var list: CurrencyList = this.CurrencyList.filter(d => d.Id == this.EntityPM.CurrencyId)[0];
            if (list != null) {
                this.CurrencyCode = list.Code;
            }
        }
    }

    get CurrencyCode() { return this.EntityPM.CurrencyCode; }
    set CurrencyCode(value: string) {
        if (this.EntityPM.CurrencyCode != value) {
            this.EntityPM.CurrencyCode = value;
        }
    }

    get MeasurementCode() { return this.EntityPM.MeasurementCode; }
    set MeasurementCode(value: string) {
        if (this.EntityPM.MeasurementCode != value) {
            this.EntityPM.MeasurementCode = value;
        }
    }

    get UnitPrice() { return this.EntityPM.UnitPrice; }
    set UnitPrice(value: number) {
        if (this.EntityPM.UnitPrice != value) {
            if (value != null)
                this.EntityPM.UnitPrice = +value.toFixed(2);
            else this.EntityPM.UnitPrice = +value;


            
        }
    }

    public get MaxPrice() { return this.EntityPM.MaxPrice; }
    public set MaxPrice(value: number) {
        if (this.EntityPM.MaxPrice != value) {
            if (value != null) {
                this.EntityPM.MaxPrice = +value.toFixed(2);
            }
            else this.EntityPM.MaxPrice = +value;
        }
    }

   public get MinPrice() { return this.EntityPM.MinPrice; }
    public set MinPrice(value: number) {
        if (this.EntityPM.MinPrice != value) {
            if(value!=null)
                this.EntityPM.MinPrice = +value.toFixed(2);
            else this.EntityPM.MinPrice = +value;
        }
    }
}
