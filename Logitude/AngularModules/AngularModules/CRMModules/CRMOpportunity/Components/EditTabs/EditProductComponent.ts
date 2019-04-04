import {Component, OnInit} from '@angular/core';
import {OpportunityPM} from '../../../../CRM/EntityPMs/OpportunityPM';
import {OpportunityProductLocationPM} from '../../../../CRM/EntityPMs/OpportunityProductLocationPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ProductData, CountryListViewModel} from './OpportunityProductsTabComponent';
import {CountryListService} from '../../../../Common/Services/StandardLists/CountryListService';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {ClassLevelValidator} from '../../../../Infrastructure/Validators/ClassLevelValidator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './EditProductComponent.html',
})

export class EditProductComponent extends BaseComponent {
    private myCurrencyCode: string = "";
    public ObjectTableName = "OpportunityProduct";
    public EntityPM: ProductData = null;
    public DataContext: EditProductComponent = this;
    private _currencyListService: CurrencyListService;
    public TEUVisibility: boolean = true;
    public CustomerProductionRevenueHeader: string;
    public SearchTextId: string = "SearchTextId_";
    public SearchDropButtonId: string = "SearchDropButtonId_"
    private searchText: string = null;
    private CountriesToggleObsListTemp: Array<CountryListViewModel> = [];
    public ValidationErrorsList: Array<String> = [];
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        this.SearchCountries();
    }
    public DataLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SearchTextId += this.CurrentSession.GetNewId(this.SearchTextId);
        this.SearchDropButtonId += this.CurrentSession.GetNewId(this.SearchTextId);
        this._currencyListService = new CurrencyListService();

        if (!AppTool.IsNullOrEmpty(SessionLocator.TenantPM.ProfitCurrencyId)) {
            this._currencyListService.getAllFromCache().subscribe(result => {

                var list: CurrencyList = result.Result.filter(d => d.Id == (SessionLocator.TenantPM.ProfitCurrencyId))[0];
                if (list != null) {
                    this.myCurrencyCode = list.Code;
                }
                this.CustomerProductionRevenueHeader = TextCodeTranslator.Translate("OpportunityProductLocation.F.Revenue") + " (" + this.myCurrencyCode + ")";
            });
        }
        else {
            this.CustomerProductionRevenueHeader = TextCodeTranslator.Translate("OpportunityProductLocation.F.Revenue");
        }
    }

    OnDeleteValue() {
        var temp = document.getElementById(this.SearchTextId) as HTMLInputElement;
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    }
    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchTextId) as HTMLInputElement;
        temp.placeholder = "";
        this.SearchText = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }
    FillPlaceHolder() {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchTextId) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
    SearchCountries() {
        if (this.EntityPM != null) {
            this.EntityPM.CountriesToggleObsList = [];
            var data: Array<CountryList> = [];
            var _countryListService: CountryListService = new CountryListService();
            if (this.CountriesToggleObsListTemp.length == 0) {
                _countryListService.getAllFromCache().subscribe(result => {
                    data = result.Result.filter(d => d.Tenant == SessionLocator.Tenant);
                    data.sort((a, b) => { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1 }).forEach(item => {
                        this.CountriesToggleObsListTemp.push(new CountryListViewModel(item, this.EntityPM.entityPM, this.EntityPM.DataContext));
                    });
                    this.EntityPM.CountriesToggleObsList = this.CountriesToggleObsListTemp;
                });
            }

            else {
                if (AppTool.IsNullOrEmpty(this.SearchText)) {
                    this.EntityPM.CountriesToggleObsList = this.CountriesToggleObsListTemp;
                }
                else {
                    this.EntityPM.CountriesToggleObsList = this.CountriesToggleObsListTemp.filter(f => f.Name.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1);
                }
            }

        }

    }
    SetWindowArgs(args: ProductData) {
        this.EntityPM = args;
        this.SetColumnVisibility();
        this.SearchCountries();
        this.Clone(args);
        args.SetEnabledFields();
    }
    CountyClicked(item: CountryListViewModel, i) {

    }
    SetColumnVisibility() {
        if (this.EntityPM != null) {
            if (this.EntityPM.TransportModeId == "A") {
                this.TEUVisibility = false;
            }
        }
    }

    //Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }
    OkButtonClicked() {

        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var validator: ClassLevelValidator;

        validator = new ClassLevelValidator();

        var errorsArray = validator.Validate("OpportunityProduct", this.EntityPM.entityPM);

        var anyMinusAmount: boolean = false;


        if (this.EntityPM.TEU < 0 || this.EntityPM.Revenue < 0 || this.EntityPM.ChargeableWeight < 0 || this.EntityPM.NumberOfShipments < 0) {
            anyMinusAmount = true;
        }

        else {
            if (this.EntityPM.ProductLocations.filter(d => d.TEU < 0)[0]) {
                anyMinusAmount = true;
            }

            else if (this.EntityPM.ProductLocations.filter(d => d.Revenue < 0)[0]) {
                anyMinusAmount = true;
            }

            else if (this.EntityPM.ProductLocations.filter(d => d.ChargeableWeight < 0)[0]) {
                anyMinusAmount = true;
            }

            else if (this.EntityPM.ProductLocations.filter(d => d.NumberOfShipments < 0)[0]) {
                anyMinusAmount = true;
            }
        }

        if (anyMinusAmount) {
            errorsArray.push("Minus amounts are not allowed");
        }

        this.ValidationErrorsList = errorsArray;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    }

    private myCloner: Cloner;
    private oldLocations: OpportunityProductLocationPM[] = [];
    private Clone(myDataContext: ProductData) {

        myDataContext.entityPM.OpportunityProductLocations.forEach(item => {
            var oldItem: OpportunityProductLocationPM = new OpportunityProductLocationPM(null);
            oldItem.TEU = item.TEU;
            oldItem.Revenue = item.Revenue;
            oldItem.ChargeableWeight = item.ChargeableWeight;
            oldItem.NumberOfShipments = item.NumberOfShipments;
            oldItem.CountryId = item.CountryId;
            oldItem.LocationCode = item.LocationCode;
            oldItem.LocationName = item.LocationName;
            oldItem.OpportunityId = item.OpportunityId;
            oldItem.OpportunityProductTypeCode = item.OpportunityProductTypeCode;
            oldItem.Tenant = item.Tenant;
            oldItem.LineNumber = item.LineNumber;
            oldItem.ChangeSetOp = item.ChangeSetOp;            
            oldItem.OldEntityPM = item.OldEntityPM;            
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            oldItem.IsDirty = item.IsDirty;
            oldItem.EntityParentPM = item.EntityParentPM;            
            this.oldLocations.push(oldItem);
        });

        this.myCloner = new Cloner(myDataContext);
        this.myCloner.AddField('NumberOfShipments');
        this.myCloner.AddField('ChargeableWeight');
        this.myCloner.AddField('Revenue');
        this.myCloner.AddField('TEU');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('PrepaidCollectId');
        this.myCloner.AddEntity(myDataContext.entityPM);
        this.myCloner.AddEntity(myDataContext.trigger.EntityPM);        
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldLocations.forEach(item => {
            var existingItem = this.EntityPM.entityPM.OpportunityProductLocations.filter(f => f.OpportunityProductTypeCode == item.OpportunityProductTypeCode && f.CountryId == item.CountryId)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.entityPM.OpportunityProductLocations.forEach(item => {
            var oldItem = this.oldLocations.filter(f => f.OpportunityProductTypeCode == item.OpportunityProductTypeCode && f.CountryId == item.CountryId)[0];
            if (oldItem) {
                if (item.TEU != oldItem.TEU) {
                    item.TEU = oldItem.TEU;
                }

                if (item.ChargeableWeight != oldItem.ChargeableWeight) {
                    item.ChargeableWeight = oldItem.ChargeableWeight;
                }

                if (item.Revenue != oldItem.Revenue) {
                    item.Revenue = oldItem.Revenue;
                }

                if (item.NumberOfShipments != oldItem.NumberOfShipments) {
                    item.NumberOfShipments = oldItem.NumberOfShipments;
                }
            }

            else {
                addedItems.push(item);
            }
        });

        addedItems.forEach(item => {
            this.EntityPM.entityPM.RemoveOpportunityProductLocation(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.entityPM.AddOpportunityProductLocation(item);
        });

        this.EntityPM.BuildProductLocations();

        this.myCloner.RejectChanges();
    }
}
