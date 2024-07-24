
import { ChangeDetectorRef, Component, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { ProductTypePM } from '../../../../../Common/EntityPMs/ProductTypePM';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { ProductTypeModificationPM } from '../../../../../Common/EntityPMs/ProductTypeModificationPM';
import { ProductTypeModificationPMService } from '../../../../../Common/Services/StandardPMs/ProductTypeModificationPMService';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
@Component({
    selector: 'ProductTypeGeneralTabComponent',
    templateUrl: './ProductTypeGeneralTabComponent.html',
})

export class ProductTypeGeneralTabComponent extends BaseComponent {

    public EntityPM: ProductTypePM;
    public ProductTypeModificationPM: ProductTypeModificationPM;
    public ObjectTableName: string = "ProductType";
    private CurrentSession = SessionLocator.SelectedSession;

    public SearchTextCostTariffDropButtonProductType: string = "SearchTextCostTariffDropButtonId";
    public SearchCostTariffProductType: string = "SearchCostTariffProductType";
    public SearchTextSaleTariffDropButtonProductType: string = "SearchTextSaleTariffDropButtonId";
    public SearchSaleTariffProductType: string = "SearchSaleTariffProductType";

    public Application: string = "";

    public CostTariffToggleButtonList: Array<TariffTypesClass> = [];
    public SaleTariffToggleButtonList: Array<TariffTypesClass> = [];

    public CostTariffSelectedList: Array<TariffTypesClass> = [];
    public SaleTariffSelectedList: Array<TariffTypesClass> = [];

    public CostTariffTypesToggleButtonList: Array<TariffTypes> = [];
    public SaleTariffTypesToggleButtonList: Array<TariffTypes> = [];

    public CostTariffObslist: Array<TariffObslistItemClass> = [];
    public SaleTariffObslist: Array<TariffObslistItemClass> = [];

    public service = new ProductTypeModificationPMService();


    DataContext: ProductTypeGeneralTabComponent = this;
    constructor(public entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
       
    }

    RunComponent() {
        this.SetApplication();
        this.GetTariffList();
        SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe(isSaved => {
            if (isSaved) {
                var service = new ProductTypeModificationPMService();
                service.update(this.ProductTypeModificationPM).subscribe((res: ServiceResponse) => {
                });
            }
        });
        SessionLocator.SelectedSession.CurrentEditComponent.SaveAndCloseCompleted.subscribe(isSaved => {
            if (isSaved) {
                var service = new ProductTypeModificationPMService();
                service.update(this.ProductTypeModificationPM).subscribe((res: ServiceResponse) => {
                });
            }
        });

    }

    GetProductTypeModificationPM() {
        this.service.get(this.Code).subscribe((res: ServiceResponse) => {
            if (res.Result != null) {
                this.ProductTypeModificationPM = res.Result;
            } else {
                this.ProductTypeModificationPM = new ProductTypeModificationPM();
                this.ProductTypeModificationPM.Tenant = this.EntityPM.Tenant;
                this.ProductTypeModificationPM.ProductTypeCode = this.EntityPM.Code;
                this.ProductTypeModificationPM.InActive = this.EntityPM.InActive;
                this.ProductTypeModificationPM.CostTariffUse = "";
                this.ProductTypeModificationPM.SaleTariffUse = "";
            }
            this.SetCostTariff();
            this.SetSaleTariff();
            this.BuildCostTariffTypesToggleButtonList();
            this.BuildSaleTariffTypesToggleButtonList();
            this.BuildCostTariffTypesObsList();
            this.BuildSaleTariffTypesObsList();
        });
    }

    SetCostTariff() {
        this.savedCostTariffUse = this.CostTariffUse;
        if (this.CostTariffUse != null && this.CostTariffUse != "") {
            var ProductTypeCostTariffUseList = this.ProductTypeModificationPM.CostTariffUse.split(',');
            ProductTypeCostTariffUseList.forEach((i) => {
                if (i != "") {
                    var item = this.CostTariffTypesToggleButtonList.filter(d => d.PriceType == i)[0];
                    if (item != null) {
                        this.CostTariffSelectedList.push(new TariffTypesClass(item, this.CostTariffSelectedList, this, "Cost"));
                    }
                }
            });
        }
    }

    SetSaleTariff() {
        this.savedSaleTariffUse = this.SaleTariffUse;
        if (this.SaleTariffUse != null && this.SaleTariffUse != "") {
            var ProductTypeSaleTariffUseList = this.ProductTypeModificationPM.SaleTariffUse.split(',');
            ProductTypeSaleTariffUseList.forEach((i) => {
                if (i != "") {
                    var item = this.SaleTariffTypesToggleButtonList.filter(d => d.PriceType == i)[0];
                    if (item != null) {
                        this.SaleTariffSelectedList.push(new TariffTypesClass(item, this.SaleTariffSelectedList, this, "Sale"));
                    }
                }
            });
        }
    }

    SetApplication() {
        switch (this.Code) {
            case "AE": {
                this.Application = "E";
                break;
            }
            case "OE": {
                this.Application = "M";
                break;
            }
            case "AI": {
                this.Application = "I";
                break;
            }
            case "OI": {
                this.Application = "I";
                break;
            }
            default: {
                this.Application = this.Code;
                break;
            }
        }
    }

    ngOnInit() {
        this.SetUIPropertiesEnabled();
    }

    SetUIProperties() {
        this.SearchTextCostTariffDropButtonProductType += this.CurrentSession.GetNewId("SearchTextCostTariffDropButtonProductType_1");
        this.SearchCostTariffProductType += this.CurrentSession.GetNewId("SearchCostTariffProductType_1");
        this.SearchTextSaleTariffDropButtonProductType += this.CurrentSession.GetNewId("SearchTextSaleTariffDropButtonProductType_1");
        this.SearchSaleTariffProductType += this.CurrentSession.GetNewId("SearchSaleTariffProductType_1");

    }

    SetUIPropertiesEnabled() {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, false);
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;

        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    savedCostTariffUse: string;
    get CostTariffUse() { return this.ProductTypeModificationPM.CostTariffUse ? this.ProductTypeModificationPM.CostTariffUse : ""}
    set CostTariffUse(value: string) {
        if (value != null && value != "undefined") {
            this.ProductTypeModificationPM.CostTariffUse = value;
        }
    }

    savedSaleTariffUse: string;
    get SaleTariffUse() { return this.ProductTypeModificationPM.SaleTariffUse ? this.ProductTypeModificationPM.SaleTariffUse : ""}
    set SaleTariffUse(value: string) {
        if (value != null && value != "undefined") {
            this.ProductTypeModificationPM.SaleTariffUse = value;
        }
    }


    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;

        }
    }

    get QuotationDefaultTemplateId() { return this.EntityPM.QuotationDefaultTemplateId; }
    set QuotationDefaultTemplateId(value: string) {
        if (this.EntityPM.QuotationDefaultTemplateId != value) {
            this.EntityPM.QuotationDefaultTemplateId = value;
        }
    }

    get RoutingRQuoteDefaultTemplateId() { return this.EntityPM.RoutingRQuoteDefaultTemplateId; }
    set RoutingRQuoteDefaultTemplateId(value: string) {
        if (this.EntityPM.RoutingRQuoteDefaultTemplateId != value) {
            this.EntityPM.RoutingRQuoteDefaultTemplateId = value;
        }
    }

    private noCostTariffVisibility: boolean = false;
    public get NoCostTariffVisibility() { return this.noCostTariffVisibility; }
    public set NoCostTariffVisibility(value: boolean) { this.noCostTariffVisibility = value; }

    private noSaleTariffVisibility: boolean = false;
    public get NoSaleTariffVisibility() { return this.noSaleTariffVisibility; }
    public set NoSaleTariffVisibility(value: boolean) { this.noSaleTariffVisibility = value; }

    public costSearchText: string = null;
    public get CostSearchText() { return this.costSearchText; }
    public set CostSearchText(newValue: string) {
        this.costSearchText = newValue;
        this.BuildCostTariffTypesToggleButtonList();
        this.CD.detectChanges();
    }

    public saleSearchText: string = null;
    public get SaleSearchText() { return this.saleSearchText; }
    public set SaleSearchText(newValue: string) {
        this.saleSearchText = newValue;
        this.BuildSaleTariffTypesToggleButtonList();
        this.CD.detectChanges();
    }

    GetTariffList() {
        var service = new DeclarationWebService();
        service.GetGTBPTYPEItemList(this.Application, this.costSearchText, 30, true).subscribe((res: ServiceResponse) => {
            this.CostTariffToggleButtonList = [];
            if (res.Result != null) {
                res.Result.forEach((i) => {
                    var item = new TariffTypes();
                    item.Application = i.itm.Application;
                    item.Name = i.itm.Name;
                    item.PriceType = i.itm.PriceType;
                    this.CostTariffTypesToggleButtonList.push(item);
                    this.SaleTariffTypesToggleButtonList.push(item);
                });
                this.GetProductTypeModificationPM();

            }
        });
    }

    BuildCostTariffTypesToggleButtonList() {
        this.CostTariffToggleButtonList = [];
        var data: Array<TariffTypes> = null;
        if (this.CostSearchText == null || this.CostSearchText == "") {
            data = this.CostTariffTypesToggleButtonList;
        }

        else {
            data = this.CostTariffTypesToggleButtonList.filter(f => f.Name.toLowerCase().indexOf(this.CostSearchText.toLowerCase()) > -1);
        }

        data.forEach(item => {
            this.CostTariffToggleButtonList.push(new TariffTypesClass(item, this.CostTariffSelectedList, this,"Cost"));
        });
    }

    BuildSaleTariffTypesToggleButtonList() {
        this.SaleTariffToggleButtonList = [];
        var data: Array<TariffTypes> = null;
        if (this.SaleSearchText == null || this.SaleSearchText == "") {
            data = this.SaleTariffTypesToggleButtonList;
        }

        else {
            data = this.SaleTariffTypesToggleButtonList.filter(f => f.Name.toLowerCase().indexOf(this.SaleSearchText.toLowerCase()) > -1);
        }

        data.forEach(item => {
            this.SaleTariffToggleButtonList.push(new TariffTypesClass(item, this.SaleTariffSelectedList, this,"Sale"));
        });
    }

    setToggleButtonMenuCostTariffTemp() {
        var ToggleBTN = document.getElementById(this.SearchTextCostTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }

    setToggleButtonMenuCostTariff() {
        var ToggleBTN = document.getElementById(this.SearchTextCostTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    setToggleButtonMenuSaleTariffTemp() {
        var ToggleBTN = document.getElementById(this.SearchTextSaleTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";
    }

    setToggleButtonMenuSaleTariff() {
        var ToggleBTN = document.getElementById(this.SearchTextSaleTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    CostTariffToggleButtonClicked(item: TariffTypesClass, i) {
        if (item.IsChecked == true) {
            this.BuildCostTariffTypesToggleButtonList();
        }
        this.BuildCostTariffTypesObsList();
    }

    SaleTariffToggleButtonClicked(item: TariffTypesClass, i) {
        if (item.IsChecked == true) {
            this.BuildSaleTariffTypesToggleButtonList();
        }
        this.BuildSaleTariffTypesObsList();
    }

    BuildCostTariffTypesObsList() {
        this.CostTariffObslist = [];
        this.CostTariffSelectedList.forEach(item => {
            this.CostTariffObslist.push(new TariffObslistItemClass(item));
        });
        this.NoCostTariffVisibility = this.CostTariffObslist.length == 0 ? true : false;
    }

    BuildSaleTariffTypesObsList() {
        this.SaleTariffObslist = [];
        this.SaleTariffSelectedList.forEach(item => {
            this.SaleTariffObslist.push(new TariffObslistItemClass(item));
        });
        this.NoSaleTariffVisibility = this.SaleTariffObslist.length == 0 ? true : false;
    }

    deleteItemCostTariffTypesObsList(item: TariffTypesClass) {
        var index = this.CostTariffSelectedList.indexOf(item);
        if (index > -1) {
            this.CostTariffSelectedList.splice(index, 1);
            this.CostTariffUse = this.CostTariffUse.replace("," + item.Entity.PriceType, "");
        }
        if (this.savedCostTariffUse != this.CostTariffUse) {
            this.EntityPM.IsDirty = true;
        } else {
            this.EntityPM.IsDirty = false;
        }
        this.BuildCostTariffTypesToggleButtonList();
        this.BuildCostTariffTypesObsList();
    }

    deleteItemSaleTariffTypesObsList(item: TariffTypesClass) {
        var index = this.SaleTariffSelectedList.indexOf(item);
        if (index > -1) {
            this.SaleTariffSelectedList.splice(index, 1);
            this.SaleTariffUse = this.SaleTariffUse.replace("," + item.Entity.PriceType, "");
        }
        if (this.savedSaleTariffUse != this.SaleTariffUse) {
            this.EntityPM.IsDirty = true;
        } else {
            this.EntityPM.IsDirty = false;
        }
        this.BuildSaleTariffTypesToggleButtonList();
        this.BuildSaleTariffTypesObsList();
    }

    ClearPlaceHolderCostTariff() {
        var temp = document.getElementById(this.SearchCostTariffProductType) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextCostTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

    }

    ClearPlaceHolderSaleTariff() {
        var temp = document.getElementById(this.SearchSaleTariffProductType) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextSaleTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenuTemp";

    }

    OnDeleteValueCostTariff() {
        var temp = document.getElementById(this.SearchCostTariffProductType) as HTMLInputElement;
        temp.value = null;
        this.CostSearchText = null;
        temp.focus();
    }

    OnDeleteValueSaleTariff() {
        var temp = document.getElementById(this.SearchSaleTariffProductType) as HTMLInputElement;
        temp.value = null;
        this.SaleSearchText = null;
        temp.focus();
    }

    FillPlaceHolderCostTariff() {
        if (!this.CostSearchText) {
            var temp = document.getElementById(this.SearchCostTariffProductType) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextCostTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }

    FillPlaceHolderSaleTariff() {
        if (!this.SaleSearchText) {
            var temp = document.getElementById(this.SearchSaleTariffProductType) as HTMLInputElement;
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextSaleTariffDropButtonProductType) as HTMLDivElement;
        ToggleBTN.className = "ToggleButtonMenu";
    }
}
class TariffTypes {
    public Name: string;
    public PriceType: string;
    public Application: string;
}

class TariffTypesClass {
    private entity: TariffTypes;
    tariffType: string;
    constructor(item: TariffTypes, itemsList: Array<TariffTypesClass>, private Parent: ProductTypeGeneralTabComponent,tariffType:string) {
        this.entity = item;
        this.tariffType = tariffType
        var itemInList = itemsList.filter(d => d.Name == this.entity.Name)[0];
        if (itemInList != null) {
            this.isChecked = true;
        }
    }

    public get Name() { return this.entity.Name; }

    public get Entity() { return this.entity; }

    private isChecked: boolean;
    public get IsChecked() { return this.isChecked; }
    public set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
            if (this.tariffType == "Cost") {
                if (value) {
                    this.Parent.CostTariffSelectedList.push(this);
                    this.Parent.CostTariffUse += "," + this.Entity.PriceType;
                } else {
                    this.Parent.CostTariffSelectedList = this.Parent.CostTariffSelectedList.filter(x => x.Name != this.Name);
                    this.Parent.CostTariffUse = this.Parent.CostTariffUse.replace("," + this.Entity.PriceType, "");
                }
                this.Parent.BuildCostTariffTypesObsList();
                this.Parent.BuildCostTariffTypesToggleButtonList();
            }

            if (this.tariffType == "Sale") {
                if (value) {
                    this.Parent.SaleTariffSelectedList.push(this);
                    this.Parent.SaleTariffUse += "," + this.Entity.PriceType;
                } else {
                    this.Parent.SaleTariffSelectedList = this.Parent.SaleTariffSelectedList.filter(x => x.Name != this.Name);
                    this.Parent.SaleTariffUse = this.Parent.SaleTariffUse.replace("," + this.Entity.PriceType, "");
                }
                this.Parent.BuildSaleTariffTypesObsList();
                this.Parent.BuildSaleTariffTypesToggleButtonList();
            }
            if (this.Parent.savedCostTariffUse != this.Parent.CostTariffUse || this.Parent.savedSaleTariffUse != this.Parent.SaleTariffUse) {
                this.Parent.EntityPM.IsDirty = true;
            } else
                if (this.Parent.savedCostTariffUse == this.Parent.CostTariffUse && this.Parent.savedSaleTariffUse == this.Parent.SaleTariffUse) {
                    this.Parent.EntityPM.IsDirty = false;
            }
        }
    }
}

class TariffObslistItemClass {
    private entity: TariffTypesClass;
    constructor(item: TariffTypesClass) {
        this.entity = item;
    }
    public get Name() { return this.entity.Name; }
    public get Entity() { return this.entity }


}

