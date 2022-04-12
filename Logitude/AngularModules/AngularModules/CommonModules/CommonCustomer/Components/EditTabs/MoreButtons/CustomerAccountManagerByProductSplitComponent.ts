import {Component} from '@angular/core';
import {CustomerPM} from '../../../../../Common/EntityPMs/CustomerPM';
import {ProductTypeList} from '../../../../../Common/EntityLists/ProductTypeList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {UserList} from '../../../../../Common/EntityLists/UserList';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {CustomerAccountManagerByProductPM} from '../../../../../Common/EntityPMs/CustomerAccountManagerByProductPM';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    
    templateUrl: './CustomerAccountManagerByProductSplitComponent.html',
})

export class CustomerAccountManagerByProductSplitComponent extends BaseComponent {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public ProductTypes: ProductTypeList[] = [];
    public ItemsSource: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsDisabled: boolean = false;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ProductTypes = args['ProductTypes'];
        this.IsDisabled = args['IsDisabled'] ?? false;
        this.Clone();

        this.ProductTypes.forEach(item => {
            var itemPM = this.EntityPM.CustomerAccountManagerByProducts.filter(d => d.ProductTypeCode == item.Code)[0];
            var itemClass = new CustomerAccountManagerByProductSplitLineViewModel(this.EntityPM, item, itemPM);

            this.ItemsSource.Insert(itemClass);
        });
    }
    CencelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var myPipe = new GroupByPipe();
        var myBaseData = this.EntityPM.CustomerAccountManagerByProducts.filter(f => f.AccountManagerId != null);

        var Forwarders = myPipe.transform(myBaseData, "AccountManagerId");

        if (Forwarders.length == 1) {
            this.EntityPM.AccountManagerUserId = myBaseData[0].AccountManagerId;
            //this.EntityPM.AccountManagerUserName = myBaseData[0].AccountManagerName;
        }

        else {
            this.EntityPM.AccountManagerUserId = null;
            //this.EntityPM.AccountManagerUserName = null;
        }

        this.CurrentSession.CurrentWindow.Close("OK");
    }

    private myCloner: Cloner;
    private oldItems: CustomerAccountManagerByProductPM[] = [];
    private Clone() {
        this.EntityPM.CustomerAccountManagerByProducts.forEach(item => {
            var oldItem = new CustomerAccountManagerByProductPM(null);
            oldItem.AccountManagerId = item.AccountManagerId;
            oldItem.AccountManagerName = item.AccountManagerName;
            oldItem.ProductTypeCode = item.ProductTypeCode;
            oldItem.CustomerId = item.CustomerId;
            oldItem.IsDirty = item.IsDirty;
            oldItem.ChangeSetOp = item.ChangeSetOp;
            oldItem.Tenant = item.Tenant;
            oldItem.EntityParentPM = item.EntityParentPM;
            oldItem.OldEntityPM = item.OldEntityPM;
            oldItem.UIProperties = item.UIProperties;
            oldItem.UniqueKey = item.UniqueKey;
            this.oldItems.push(oldItem);
        });

        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('AccountManagerUserId');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldItems.forEach(item => {
            var existingItem = this.EntityPM.CustomerAccountManagerByProducts.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.CustomerAccountManagerByProducts.forEach(item => {
            var oldItem = this.oldItems.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (oldItem) {
                if (item.AccountManagerId != oldItem.AccountManagerId) {
                    item.AccountManagerId = oldItem.AccountManagerId;
                }

                if (item.AccountManagerName != oldItem.AccountManagerName) {
                    item.AccountManagerName = oldItem.AccountManagerName;
                }

                if (item.IsDirty != oldItem.IsDirty) {
                    item.IsDirty = oldItem.IsDirty;
                }
            }

            else {
                addedItems.push(item);
            }
        });

        addedItems.forEach(item => {
            this.EntityPM.RemoveCustomerAccountManagerByProductPM(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.AddCustomerAccountManagerByProductPM(item);
        });

        this.myCloner.RejectChanges();
    }
}

export class CustomerAccountManagerByProductSplitLineViewModel extends BaseComponent  {
    public Customer: CustomerPM;
    public EntityList: ProductTypeList;
    public EntityPM: CustomerAccountManagerByProductPM;
    constructor(myCustomerPM: CustomerPM, entityList: ProductTypeList, entityPM: CustomerAccountManagerByProductPM) {
        super();
        this.Customer = myCustomerPM;
        this.EntityList = entityList;
        this.EntityPM = entityPM;
        this.SetPartner();
        this.SetVisibility();
    }

    public LOVIsVisible: boolean = true;
    SetVisibility() {
        if (SessionLocator.TenantPM.IsHybrid && (this.Customer.CustomerStatusCode == "ACT" || this.Customer.CustomerStatusCode == "WAC")) {
            this.LOVIsVisible = false;
        }
    }

    SetPartner() {
        if (this.EntityPM) {
            this.PartnerId = this.EntityPM.AccountManagerId;
            this.PartnerName = this.EntityPM.AccountManagerName;
        }

        else {
            this.PartnerId = null;
            this.PartnerName = null;
        }
    }

    public get ProductTypeCode() { return this.EntityList.Code; }
    public get ProductTypeName() { return this.EntityList.Name; }

    private myPartnerId: string = null;
    get PartnerId() { return this.myPartnerId; }
    set PartnerId(value: string) {
        if (this.myPartnerId != value) {
            this.myPartnerId = value;
        }
    }

    private myPartnerName: string = null;
    get PartnerName() { return this.myPartnerName; }
    set PartnerName(value: string) {
        if (this.myPartnerName != value) {
            this.myPartnerName = value;
        }
    }

    private myPartner: UserList = null;
    get Partner() { return this.myPartner; }
    set Partner(value: UserList) {
        if (this.myPartner || value) {
            var isChanged = true;

            if (this.myPartner && value) {
                isChanged = false;

                if (this.myPartner.Id != value.Id) {
                    isChanged = true;
                }
            }

            if (isChanged) {
                this.myPartner = value;

                if (value) {
                    if (this.EntityPM) {
                        if (this.EntityPM.AccountManagerId != value.Id) {
                            this.EntityPM.AccountManagerId = value.Id;
                        }

                        if (this.EntityPM.AccountManagerName != value.EnglishName) {
                            this.EntityPM.AccountManagerName = value.EnglishName;
                        }
                    }

                    else {
                        this.EntityPM = new CustomerAccountManagerByProductPM(null);
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                        this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                        this.EntityPM.CustomerId = this.Customer.Id;
                        this.EntityPM.AccountManagerId = value.Id;
                        this.EntityPM.AccountManagerName = value.EnglishName;
                        this.Customer.AddCustomerAccountManagerByProductPM(this.EntityPM);
                    }
                }

                else {
                    this.Customer.RemoveCustomerAccountManagerByProductPM(this.EntityPM);
                    this.EntityPM = null;
                }

                this.SetPartner();
            }
        }
    }
}
