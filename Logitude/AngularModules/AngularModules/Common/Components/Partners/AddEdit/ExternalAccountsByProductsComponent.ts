import {Component} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {CardExternalAccountsByProductPM} from '../../../EntityPMs/CardExternalAccountsByProductPM';
import {PartnersDomainService, PartnerExternalAccountsServicePM} from '../../../Services/PartnersDomainService';
import {ProductTypeList} from '../../../EntityLists/ProductTypeList';
import {ProductTypeListService} from '../../../Services/StandardLists/ProductTypeListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './ExternalAccountsByProductsComponent.html',
})

export class ExternalAccountsByProductsComponent extends BaseComponent {
    public EntityPM: any = null;
    public ObjectTableName: string;
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public ItemsSource: ObservableCollection;
    public SelectedRow: ExternalAccountsByProductsItem = null;
    public IsResourcesReady: boolean = false;
    private myDomainService: PartnersDomainService;
    private myProductTypeListService: ProductTypeListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.myDomainService = new PartnersDomainService();
        this.myProductTypeListService = new ProductTypeListService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ObjectTableName = args['ObjectTableName'];
        this.Clone();

        this.entityResourceService.getEntityResourceByTableName("CardExternalAccountsByProduct").subscribe((res: any) => {
            this.IsResourcesReady = true;
            this.LoadData();
        });
    }

    AllProductTypes: ProductTypeList[] = [];
    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myProductTypeListService.getAllFromCache().subscribe((myResponse1: ServiceResponse) => {
            if (!myResponse1.HasError) {
                this.AllProductTypes = myResponse1.Result;
                this.AllProductTypes = this.AllProductTypes.filter(f => f.Code != "CI" && f.Code != "DL" && f.Code != "IN");
            }

            this.myDomainService.GetCardExternalAccountsByProducts(this.EntityPM.Id).subscribe((myResponse2: ServiceResponse) => {
                if (!myResponse2.HasError) {
                    var myResult: CardExternalAccountsByProductPM[] = myResponse2.Result;
                    this.BuildItemsSource(myResult);
                }

                this.CurrentSession.StopBusyIndicator();
            });
        });
    }

    BuildItemsSource(loadedList: CardExternalAccountsByProductPM[]) {
        var itemsCollection: ExternalAccountsByProductsItem[] = [];

        if (loadedList == null) {
            loadedList = [];
        }

        if (loadedList.length > 0) {
            loadedList = loadedList.filter(f => f.ProductTypeCode != "CI" && f.ProductTypeCode != "DL" && f.ProductTypeCode != "IN");

            loadedList.forEach(itemPM => {
                itemsCollection.push(new ExternalAccountsByProductsItem(itemPM, false, this));
                this.ItemsSource.InsertCollection(itemsCollection);
            });
        }

        else {
            this.AllProductTypes.forEach(item => {
                var itemPM = new CardExternalAccountsByProductPM();
                itemPM.Tenant = SessionLocator.Tenant;
                itemPM.CardId = this.EntityPM.Id;
                itemPM.ProductTypeCode = item.Code;
                itemPM.ProductTypeName = item.Name;
                itemPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                itemPM.UpdatedByUserId = SessionLocator.LoggedUserId;
                itemPM.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
                itemsCollection.push(new ExternalAccountsByProductsItem(itemPM, true, this));
                this.ItemsSource.InsertCollection(itemsCollection);
            });
        }
    }

    get ExternalAccountingBusinessArea() { return this.EntityPM.ExternalAccountingBusinessArea; }
    set ExternalAccountingBusinessArea(value: string) {
        if (this.EntityPM.ExternalAccountingBusinessArea != value) {
            this.EntityPM.ExternalAccountingBusinessArea = value;
        }
    }

    get ExternalId2() { return this.EntityPM.ExternalId2; }
    set ExternalId2(value: string) {
        if (this.EntityPM.ExternalId2 != value) {
            this.EntityPM.ExternalId2 = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            this.CurrentSession.StartBusyIndicatorSaving();

            var errors: string[] = [];
            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

            this.ItemsSource.Collection.forEach((item: ExternalAccountsByProductsItem) => {
                Validator.TryValidateObject(item.EntityPM, item.ObjectTableName, errors);
            });

            this.ValidationErrorsList = errors;

            if (errors.length == 0) {
                var args = new PartnerExternalAccountsServicePM();
                args.Tenant = SessionLocator.Tenant;
                args.CardId = this.EntityPM.Id;
                args.BusinessArea = this.ExternalAccountingBusinessArea;
                args.ExternalId2 = this.ExternalId2;
                args.ObjectTableName = this.ObjectTableName;

                this.ItemsSource.Collection.forEach((item: ExternalAccountsByProductsItem) => {
                    args.Items.push(item.EntityPM);
                });

                this.myDomainService.Put(args).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse.HasError) {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }

                    else {
                        this.EntityPM.IsDirty = false;
                        this.CurrentSession.CloseCurrentWindowEmit("Ok");
                    }
                });
            }

            else {
                this.CurrentSession.StopBusyIndicator();
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ExternalAccountingBusinessArea');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
export class ExternalAccountsByProductsItem extends BaseComponent {
    public EntityPM: CardExternalAccountsByProductPM;
    public ObjectTableName: string = "CardExternalAccountsByProduct";
    public DataContext = this;
    constructor(entityPM: CardExternalAccountsByProductPM, private isNewEntity: boolean, private father: ExternalAccountsByProductsComponent) {
        super();
        this.EntityPM = entityPM;        
    }

    get Id() { return this.EntityPM.Id; }
    get ProductTypeCode() { return this.EntityPM.ProductTypeCode; }
    get ProductTypeName() { return this.EntityPM.ProductTypeName; }

    get GLAccount() { return this.EntityPM.GLAccount; }
    set GLAccount(value: string) {
        if (this.EntityPM.GLAccount != value) {
            this.EntityPM.GLAccount = value;
            this.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            this.UpdatedByUserId = SessionLocator.LoggedUserPM.Id;
            this.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
            this.father.EntityPM.IsDirty = true;
        }
    }

    get CostCenter() { return this.EntityPM.CostCenter; }
    set CostCenter(value: string) {
        if (this.EntityPM.CostCenter != value) {
            this.EntityPM.CostCenter = value;
            this.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            this.UpdatedByUserId = SessionLocator.LoggedUserPM.Id;
            this.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
            this.father.EntityPM.IsDirty = true;
        }
    }

    get UpdateDate() { return this.EntityPM.UpdateDate; }
    set UpdateDate(value: Date) {
        if (this.EntityPM.UpdateDate != value) {
            this.EntityPM.UpdateDate = value;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(value: string) {
        if (this.EntityPM.UpdatedByUserId != value) {
            this.EntityPM.UpdatedByUserId = value;
        }
    }

    get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    set UpdatedByUserName(value: string) {
        if (this.EntityPM.UpdatedByUserName != value) {
            this.EntityPM.UpdatedByUserName = value;
        }
    }
}
