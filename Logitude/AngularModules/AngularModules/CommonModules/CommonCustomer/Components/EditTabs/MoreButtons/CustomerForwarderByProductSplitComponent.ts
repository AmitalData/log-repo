import {Component} from '@angular/core';
import {CustomerPM} from '../../../../../Common/EntityPMs/CustomerPM';
import {ProductTypeList} from '../../../../../Common/EntityLists/ProductTypeList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {CustomerForwarderByProductPM} from '../../../../../Common/EntityPMs/CustomerForwarderByProductPM';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    
    templateUrl: './CustomerForwarderByProductSplitComponent.html',
})

export class CustomerForwarderByProductSplitComponent extends BaseComponent {
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
            var itemPM = this.EntityPM.CustomerForwarderByProducts.filter(d => d.ProductTypeCode == item.Code)[0];
            var itemClass = new CustomerForwarderByProductSplitLineViewModel(this.EntityPM, item, itemPM);

            this.ItemsSource.Insert(itemClass);
        });
    }
    CencelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var myPipe = new GroupByPipe();

        var Forwarders = myPipe.transform(this.EntityPM.CustomerForwarderByProducts.filter(f => f.ForwarderId != null), "ForwarderId");

        if (Forwarders.length == 1) {
            this.EntityPM.ForwarderId = this.EntityPM.CustomerForwarderByProducts.filter(f => f.ForwarderId != null)[0].ForwarderId;
            this.EntityPM.ForwarderName = this.EntityPM.CustomerForwarderByProducts.filter(f => f.ForwarderId != null)[0].ForwarderName;
        }

        else {
            this.EntityPM.ForwarderId = null;
            this.EntityPM.ForwarderName = null;
        }

        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    private myCloner: Cloner;
    private oldItems: CustomerForwarderByProductPM[] = [];
    private Clone() {
        this.EntityPM.CustomerForwarderByProducts.forEach(item => {
            var oldItem = new CustomerForwarderByProductPM(null);
            oldItem.ForwarderId = item.ForwarderId;
            oldItem.ForwarderName = item.ForwarderName;
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
        this.myCloner.AddField('ForwarderId');
        this.myCloner.AddField('ForwarderName');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldItems.forEach(item => {
            var existingItem = this.EntityPM.CustomerForwarderByProducts.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.CustomerForwarderByProducts.forEach(item => {
            var oldItem = this.oldItems.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (oldItem) {
                if (item.ForwarderId != oldItem.ForwarderId) {
                    item.ForwarderId = oldItem.ForwarderId;
                }

                if (item.ForwarderName != oldItem.ForwarderName) {
                    item.ForwarderName = oldItem.ForwarderName;
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
            this.EntityPM.RemoveCustomerForwarderByProductPM(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.AddCustomerForwarderByProductPM(item);
        });

        this.myCloner.RejectChanges();
    }
}

export class CustomerForwarderByProductSplitLineViewModel extends BaseComponent {
    public Customer: CustomerPM;
    public EntityList: ProductTypeList;    
    public EntityPM: CustomerForwarderByProductPM;
    constructor(myCustomerPM: CustomerPM, entityList: ProductTypeList, entityPM: CustomerForwarderByProductPM) {
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
            this.PartnerId = this.EntityPM.ForwarderId;
            this.PartnerName = this.EntityPM.ForwarderName;
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

    private myPartner: CardList = null;
    get Partner() { return this.myPartner; }
    set Partner(value: CardList) {
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
                        if (this.EntityPM.ForwarderId != value.Id) {
                            this.EntityPM.ForwarderId = value.Id;
                        }

                        if (this.EntityPM.ForwarderName != value.EnglishName) {
                            this.EntityPM.ForwarderName = value.EnglishName;
                        }
                    }

                    else {
                        this.EntityPM = new CustomerForwarderByProductPM(null);
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                        this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                        this.EntityPM.CustomerId = this.Customer.Id;
                        this.EntityPM.ForwarderId = value.Id;
                        this.EntityPM.ForwarderName = value.EnglishName;
                        this.Customer.AddCustomerForwarderByProductPM(this.EntityPM);
                    }
                }

                else {
                    this.Customer.RemoveCustomerForwarderByProductPM(this.EntityPM);
                    this.EntityPM = null;
                }

                this.SetPartner();
            }
        }
    }
}
