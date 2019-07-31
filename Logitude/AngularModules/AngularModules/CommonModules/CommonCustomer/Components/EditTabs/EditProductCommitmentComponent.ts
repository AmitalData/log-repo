import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceViewModelData} from './CustomerGeneralTabComponent';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ProductViewModelData, CountryListViewModel} from './CustomerCommitmentsTabComponent';
import {CountryListService} from '../../../../Common/Services/StandardLists/CountryListService';
import {CountryList} from '../../../../Common/EntityLists/CountryList';
import {ClassLevelValidator} from '../../../../Infrastructure/Validators/ClassLevelValidator';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {CustomerProductLocationPM} from '../../../../Common/EntityPMs/CustomerProductLocationPM';

@Component({
    moduleId: module.id,
    templateUrl: './EditProductCommitmentComponent.html',
})

export class EditProductCommitmentComponent extends BaseComponent {
    private myCurrencyCode: string = "";
    public ObjectTableName = "CustomerProduct";
    public EntityPM: ProductViewModelData = null;
    public DataContext: EditProductCommitmentComponent = this;
    private _currencyListService: CurrencyListService;
    public CommitmentTEUVisibility: boolean = true;
    public CustomerProductionRevenueHeader: string;
    public SearchTextId: string = "SearchTextId_";
    public SearchDropButtonId: string = "SearchDropButtonId_"
    private searchText: string = null;
    public ValidationErrorsList: Array<String> = [];
    private CountriesToggleObsListTemp: Array<CountryListViewModel> = [];
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            setTimeout(() => this.SearchCountries(), 500);
        }
    }
    public DataLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        super();
        this.SearchTextId += this.CurrentSession.GetNewId(this.SearchTextId);
        this.SearchDropButtonId += this.CurrentSession.GetNewId(this.SearchDropButtonId);
        this._currencyListService = new CurrencyListService();

        if (!AppTool.IsNullOrEmpty(SessionLocator.TenantPM.ProfitCurrencyId)) {
            this._currencyListService.getAllFromCache().subscribe(result => {

                var list: CurrencyList = result.Result.filter(d => d.Id == (SessionLocator.TenantPM.ProfitCurrencyId))[0];
                if (list != null) {
                    this.myCurrencyCode = list.Code;
                }
                this.CustomerProductionRevenueHeader = TextCodeTranslator.Translate("CustomerProductLocation.F.Revenue") + " (" + this.myCurrencyCode + ")";
            });
        }
        else {
            this.CustomerProductionRevenueHeader = TextCodeTranslator.Translate("CustomerProductLocation.F.Revenue");
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

    private ToggledEntered: boolean = false;

    setToggleButtonMenuTemp() {
        var ToggleBTN = document.getElementById(this.SearchDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }

    setToggleButtonMenu() {
        var ToggleBTN = document.getElementById(this.SearchDropButtonId) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
        ToggleBTN.style.visibility = "none";
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
    SetWindowArgs(args: ProductViewModelData) {
        this.EntityPM = args;
        this.SetColumnVisibility();
        this.SearchCountries();    
        this.Clone(this.EntityPM);
    }

    CountyClicked(item: CountryListViewModel, i) {
       // if (item.IsChecked == true && !this.EntityPM.CountriesToggleObsList.filter(d => d.Code == item.Code))
      ////      this.EntityPM.buil.BuildProductsToggleButtonList();
       // this.BuildProductsObsList();

    }
    SetColumnVisibility() {
        if (this.EntityPM != null) {
            if (this.EntityPM.TransportModeId == "A") {
                this.CommitmentTEUVisibility = false;
            }
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }
    OkButtonClicked() {

        var msg: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var validator: ClassLevelValidator;

        validator = new ClassLevelValidator();

        var errorsArray = validator.Validate("CustomerProduct", this.EntityPM.entityPM);

     

        var anyMinusAmount: boolean = false;

        if (this.EntityPM.isPotential) {
            if (this.EntityPM.PotentialTEU < 0 || this.EntityPM.PotentialRevenue < 0 || this.EntityPM.PotentialChargeableWeight < 0 || this.EntityPM.PotentialNumberOfShipments < 0) {
                anyMinusAmount = true;
            }

            else {
                if (this.EntityPM.ProductLocations.filter(d => d.PotentialTEU < 0)[0]) {
                    anyMinusAmount = true;
                }

                else if (this.EntityPM.ProductLocations.filter(d => d.PotentialRevenue < 0)[0]) {
                    anyMinusAmount = true;
                }

                else if (this.EntityPM.ProductLocations.filter(d => d.PotentialChargeableWeight < 0)[0]) {
                    anyMinusAmount = true;
                }

                else if (this.EntityPM.ProductLocations.filter(d => d.PotentialNumberOfShipments < 0)[0]) {
                    anyMinusAmount = true;
                }
            }
        }

        else {
            if (this.EntityPM.CommitmentTEU < 0 || this.EntityPM.CommitmentRevenue < 0 || this.EntityPM.CommitmentChargeableWeight < 0 || this.EntityPM.CommitmentNumberOfShipments < 0) {
                anyMinusAmount = true;
            }

            else {
                if (this.EntityPM.ProductLocations.filter(d => d.CommitmentTEU < 0)[0]) {
                    anyMinusAmount = true;
                }

                else if (this.EntityPM.ProductLocations.filter(d => d.CommitmentRevenue < 0)[0]) {
                    anyMinusAmount = true;
                }

                else if (this.EntityPM.ProductLocations.filter(d => d.CommitmentChargeableWeight < 0)[0]) {
                    anyMinusAmount = true;
                }

                else if (this.EntityPM.ProductLocations.filter(d => d.CommitmentNumberOfShipments < 0)[0]) {
                    anyMinusAmount = true;
                }
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
    private oldLocations: CustomerProductLocationPM[] = [];
    private Clone(myDataContext: ProductViewModelData) {

        myDataContext.entityPM.ProductLocations.forEach(item => {
            var oldItem: CustomerProductLocationPM = new CustomerProductLocationPM(null);
            oldItem.CommitmentTEU = item.CommitmentTEU;
            oldItem.CommitmentRevenue = item.CommitmentRevenue;
            oldItem.CommitmentChargeableWeight = item.CommitmentChargeableWeight;
            oldItem.CommitmentNumberOfShipments = item.CommitmentNumberOfShipments;
            oldItem.PotentialTEU = item.PotentialTEU;
            oldItem.PotentialRevenue = item.PotentialRevenue;
            oldItem.PotentialChargeableWeight = item.PotentialChargeableWeight;
            oldItem.PotentialNumberOfShipments = item.PotentialNumberOfShipments;
            oldItem.ProductTypeCode = item.ProductTypeCode;
            oldItem.CountryId = item.CountryId;
            oldItem.CountryCode = item.CountryCode;
            oldItem.CountryName = item.CountryName;
            oldItem.CustomerId = item.CustomerId;
            oldItem.IsDirty = item.IsDirty;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.Tenant = item.Tenant;
            oldItem.EntityParentPM = item.EntityParentPM;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            this.oldLocations.push(oldItem);
        });

        this.myCloner = new Cloner(myDataContext);
        this.myCloner.AddField('PotentialNumberOfShipments');
        this.myCloner.AddField('PotentialChargeableWeight');
        this.myCloner.AddField('PotentialRevenue');
        this.myCloner.AddField('PotentialTEU');
        this.myCloner.AddField('CommitmentNumberOfShipments');
        this.myCloner.AddField('CommitmentChargeableWeight');
        this.myCloner.AddField('CommitmentRevenue');
        this.myCloner.AddField('CommitmentTEU');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(myDataContext.entityPM);
        this.myCloner.AddEntity(myDataContext.DataContext.customerPM)
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldLocations.forEach(item => {
            var existingItem = this.EntityPM.entityPM.ProductLocations.filter(f => f.ProductTypeCode == item.ProductTypeCode && f.CountryId == item.CountryId)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.entityPM.ProductLocations.forEach(item => {
            var oldItem = this.oldLocations.filter(f => f.ProductTypeCode == item.ProductTypeCode && f.CountryId == item.CountryId)[0];
            if (oldItem) {
                if (item.CommitmentTEU != oldItem.CommitmentTEU) {
                    item.CommitmentTEU = oldItem.CommitmentTEU;
                }

                if (item.CommitmentRevenue != oldItem.CommitmentRevenue) {
                    item.CommitmentRevenue = oldItem.CommitmentRevenue;
                }

                if (item.CommitmentChargeableWeight != oldItem.CommitmentChargeableWeight) {
                    item.CommitmentChargeableWeight = oldItem.CommitmentChargeableWeight;
                }

                if (item.CommitmentNumberOfShipments != oldItem.CommitmentNumberOfShipments) {
                    item.CommitmentNumberOfShipments = oldItem.CommitmentNumberOfShipments;
                }

                if (item.PotentialTEU != oldItem.PotentialTEU) {
                    item.PotentialTEU = oldItem.PotentialTEU;
                }

                if (item.PotentialRevenue != oldItem.PotentialRevenue) {
                    item.PotentialRevenue = oldItem.PotentialRevenue;
                }

                if (item.PotentialChargeableWeight != oldItem.PotentialChargeableWeight) {
                    item.PotentialChargeableWeight = oldItem.PotentialChargeableWeight;
                }

                if (item.PotentialNumberOfShipments != oldItem.PotentialNumberOfShipments) {
                    item.PotentialNumberOfShipments = oldItem.PotentialNumberOfShipments;
                }
            }

            else {
                addedItems.push(item);
            }
        });

        addedItems.forEach(item => {
            this.EntityPM.entityPM.RemoveCustomerProductLocationPM(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.entityPM.AddCustomerProductLocationPM(item);
        });

        this.EntityPM.BuildProductLocations();

        this.myCloner.RejectChanges();
    }
}
