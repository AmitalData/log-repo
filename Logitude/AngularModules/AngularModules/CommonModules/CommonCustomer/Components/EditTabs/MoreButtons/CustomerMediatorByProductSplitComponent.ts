import {Component} from '@angular/core';
import {CustomerPM} from '../../../../../Common/EntityPMs/CustomerPM';
import {ProductTypeList} from '../../../../../Common/EntityLists/ProductTypeList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {CustomerMediatorByProductPM} from '../../../../../Common/EntityPMs/CustomerMediatorByProductPM';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    
    templateUrl: './CustomerMediatorByProductSplitComponent.html',
})

export class CustomerMediatorByProductSplitComponent extends BaseComponent {
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
            var itemPM = this.EntityPM.CustomerMediatorByProducts.filter(d => d.ProductTypeCode == item.Code)[0];
            var itemClass = new CustomerMediatorByProductSplitLineViewModel(this.EntityPM, item, itemPM);

            this.ItemsSource.Insert(itemClass);
        });
    }
    CencelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var myPipe = new GroupByPipe();
        var myBaseData = this.EntityPM.CustomerMediatorByProducts.filter(f => f.MediatorId != null);

        var Forwarders = myPipe.transform(myBaseData, "MediatorId");

        if (Forwarders.length == 1) {
            this.EntityPM.MediatorId = myBaseData[0].MediatorId;
            this.EntityPM.MediatorName = myBaseData[0].MediatorName;
        }

        else {
            this.EntityPM.MediatorId = null;
            this.EntityPM.MediatorName = null;
        }

        this.CurrentSession.CurrentWindow.Close("OK");
    }

    private myCloner: Cloner;
    private oldItems: CustomerMediatorByProductPM[] = [];
    private Clone() {
        this.EntityPM.CustomerMediatorByProducts.forEach(item => {
            var oldItem = new CustomerMediatorByProductPM(null);
            oldItem.MediatorId = item.MediatorId;
            oldItem.MediatorName = item.MediatorName;
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
        this.myCloner.AddField('MediatorId');
        this.myCloner.AddField('MediatorName');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldItems.forEach(item => {
            var existingItem = this.EntityPM.CustomerMediatorByProducts.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.CustomerMediatorByProducts.forEach(item => {
            var oldItem = this.oldItems.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (oldItem) {
                if (item.MediatorId != oldItem.MediatorId) {
                    item.MediatorId = oldItem.MediatorId;
                }

                if (item.MediatorName != oldItem.MediatorName) {
                    item.MediatorName = oldItem.MediatorName;
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
            this.EntityPM.RemoveCustomerMediatorByProductPM(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.AddCustomerMediatorByProductPM(item);
        });

        this.myCloner.RejectChanges();
    }
}

export class CustomerMediatorByProductSplitLineViewModel extends BaseComponent {
    public Customer: CustomerPM;
    public EntityList: ProductTypeList;
    public EntityPM: CustomerMediatorByProductPM;
    constructor(myCustomerPM: CustomerPM, entityList: ProductTypeList, entityPM: CustomerMediatorByProductPM) {
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
            this.PartnerId = this.EntityPM.MediatorId;
            this.PartnerName = this.EntityPM.MediatorName;
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
                        if (this.EntityPM.MediatorId != value.Id) {
                            this.EntityPM.MediatorId = value.Id;
                        }

                        if (this.EntityPM.MediatorName != value.EnglishName) {
                            this.EntityPM.MediatorName = value.EnglishName;
                        }
                    }

                    else {
                        this.EntityPM = new CustomerMediatorByProductPM(null);
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                        this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                        this.EntityPM.CustomerId = this.Customer.Id;
                        this.EntityPM.MediatorId = value.Id;
                        this.EntityPM.MediatorName = value.EnglishName;
                        this.Customer.AddCustomerMediatorByProductPM(this.EntityPM);
                    }
                }

                else {
                    this.Customer.RemoveCustomerMediatorByProductPM(this.EntityPM);
                    this.EntityPM = null;
                }

                this.SetPartner();
            }
        }
    }
}
