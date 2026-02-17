import {Component} from '@angular/core';
import {CustomerPM} from '../../../../../Common/EntityPMs/CustomerPM';
import {ProductTypeList} from '../../../../../Common/EntityLists/ProductTypeList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {UserList} from '../../../../../Common/EntityLists/UserList';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {CustomerSalesmanByProductPM} from '../../../../../Common/EntityPMs/CustomerSalesmanByProductPM';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerSalesmanByProductSplitComponent.html',
})

export class CustomerSalesmanByProductSplitComponent extends BaseComponent {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public ProductTypes: ProductTypeList[] = [];
    public ItemsSource: ObservableCollection;
    public IsUnifreightEditable: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ProductTypes = args['ProductTypes'];
        this.Clone();

        if (args.IsUnifreightEditable) {
            this.IsUnifreightEditable = args.IsUnifreightEditable;
            //if (this.IsUnifreightEditable === true) {
               // this.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
                //this.EntityPM.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
           // }
        }

        this.ProductTypes.forEach(item => {
            var itemPM = this.EntityPM.CustomerSalesmanByProducts.filter(d => d.ProductTypeCode == item.Code)[0];
            var itemClass = new CustomerSalesmanByProductSplitLineViewModel(this.EntityPM, item, itemPM);
           // itemClass.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, false);
            //itemPM.UIProperties.SetEnabled("SalesmanUserId", this.ObjectTableName, !this.IsUnifreightEditable);
            this.ItemsSource.Insert(itemClass);
        });

        
    }
    CencelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var myPipe = new GroupByPipe();
        var myBaseData = this.EntityPM.CustomerSalesmanByProducts.filter(f => f.SalesmanUserId != null);

        var Forwarders = myPipe.transform(myBaseData, "SalesmanUserId");

        if (Forwarders.length == 1) {
            this.EntityPM.SalesmanUserId = myBaseData[0].SalesmanUserId;
            this.EntityPM.SalesmanUserEnglishName = myBaseData[0].SalesmanUserName;
        }

        else {
            this.EntityPM.SalesmanUserId = null;
            this.EntityPM.SalesmanUserEnglishName = null;
        }

        this.CurrentSession.CurrentWindow.Close("OK");
    }

    private myCloner: Cloner;
    private oldItems: CustomerSalesmanByProductPM[] = [];
    private Clone() {
        this.EntityPM.CustomerSalesmanByProducts.forEach(item => {
            var oldItem = new CustomerSalesmanByProductPM(null);
            oldItem.SalesmanUserId = item.SalesmanUserId;
            oldItem.SalesmanUserName = item.SalesmanUserName;
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
        this.myCloner.AddField('SalesmanUserId');
        this.myCloner.AddField('SalesmanUserEnglishName');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldItems.forEach(item => {
            var existingItem = this.EntityPM.CustomerSalesmanByProducts.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.CustomerSalesmanByProducts.forEach(item => {
            var oldItem = this.oldItems.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (oldItem) {
                if (item.SalesmanUserId != oldItem.SalesmanUserId) {
                    item.SalesmanUserId = oldItem.SalesmanUserId;
                }

                if (item.SalesmanUserName != oldItem.SalesmanUserName) {
                    item.SalesmanUserName = oldItem.SalesmanUserName;
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
            this.EntityPM.RemoveCustomerSalesmanByProductPM(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.AddCustomerSalesmanByProductPM(item);
        });

        this.myCloner.RejectChanges();
    }
}

export class CustomerSalesmanByProductSplitLineViewModel extends BaseComponent {
    public Customer: CustomerPM;
    public EntityList: ProductTypeList;
    public EntityPM: CustomerSalesmanByProductPM;
    constructor(myCustomerPM: CustomerPM, entityList: ProductTypeList, entityPM: CustomerSalesmanByProductPM) {
        super();
        this.Customer = myCustomerPM;
        this.EntityList = entityList;
        this.EntityPM = entityPM;
        this.SetPartner();
    }

    SetPartner() {
        if (this.EntityPM) {
            this.PartnerId = this.EntityPM.SalesmanUserId;
            this.PartnerName = this.EntityPM.SalesmanUserName;
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
                        if (this.EntityPM.SalesmanUserId != value.Id) {
                            this.EntityPM.SalesmanUserId = value.Id;
                        }

                        if (this.EntityPM.SalesmanUserName != value.EnglishName) {
                            this.EntityPM.SalesmanUserName = value.EnglishName;
                        }
                    }

                    else {
                        this.EntityPM = new CustomerSalesmanByProductPM(null);
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                        this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                        this.EntityPM.CustomerId = this.Customer.Id;
                        this.EntityPM.SalesmanUserId = value.Id;
                        this.EntityPM.SalesmanUserName = value.EnglishName;
                        this.Customer.AddCustomerSalesmanByProductPM(this.EntityPM);
                    }
                }

                else {
                    this.Customer.RemoveCustomerSalesmanByProductPM(this.EntityPM);
                    this.EntityPM = null;
                }

                this.SetPartner();
            }
        }
    }
}
