import {Component} from '@angular/core';
import {CustomerPM} from '../../../../../Common/EntityPMs/CustomerPM';
import {ProductTypeList} from '../../../../../Common/EntityLists/ProductTypeList';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {CardList} from '../../../../../Common/EntityLists/CardList';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {GroupByPipe} from '../../../../../Infrastructure/Pipes/GroupByPipe';
import {CustomerCustomsAgentByProductPM} from '../../../../../Common/EntityPMs/CustomerCustomsAgentByProductPM';
import {Cloner} from '../../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerCustomsAgentByProductSplitComponent.html',
})

export class CustomerCustomsAgentByProductSplitComponent extends BaseComponent {
    public EntityPM: CustomerPM;
    public ObjectTableName: string = "Customer";
    public ProductTypes: ProductTypeList[] = [];
    public ItemsSource: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.ProductTypes = args['ProductTypes'];
        this.Clone();

        this.ProductTypes.forEach(item => {
            var itemPM = this.EntityPM.CustomerCustomsAgentByProducts.filter(d => d.ProductTypeCode == item.Code)[0];
            var itemClass = new CustomerCustomsAgentByProductSplitLineViewModel(this.EntityPM, item, itemPM);

            this.ItemsSource.Insert(itemClass);
        });
    }
    CencelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var myPipe = new GroupByPipe();
        var myBaseData = this.EntityPM.CustomerCustomsAgentByProducts.filter(f => f.CustomsAgentId != null);

        var Forwarders = myPipe.transform(myBaseData, "CustomsAgentId");

        if (Forwarders.length == 1) {
            this.EntityPM.CustomsAgentId = myBaseData[0].CustomsAgentId;
            this.EntityPM.CustomsAgentName = myBaseData[0].CustomsAgentName;
        }

        else {
            this.EntityPM.CustomsAgentId = null;
            this.EntityPM.CustomsAgentName = null;
        }

        this.CurrentSession.CurrentWindow.Close("OK");
    }

    private myCloner: Cloner;
    private oldItems: CustomerCustomsAgentByProductPM[] = [];
    private Clone() {
        this.EntityPM.CustomerCustomsAgentByProducts.forEach(item => {
            var oldItem = new CustomerCustomsAgentByProductPM(null);
            oldItem.CustomsAgentId = item.CustomsAgentId;
            oldItem.CustomsAgentName = item.CustomsAgentName;
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
        this.myCloner.AddField('CustomsAgentId');
        this.myCloner.AddField('CustomsAgentName');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldItems.forEach(item => {
            var existingItem = this.EntityPM.CustomerCustomsAgentByProducts.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.CustomerCustomsAgentByProducts.forEach(item => {
            var oldItem = this.oldItems.filter(f => f.ProductTypeCode == item.ProductTypeCode)[0];
            if (oldItem) {
                if (item.CustomsAgentId != oldItem.CustomsAgentId) {
                    item.CustomsAgentId = oldItem.CustomsAgentId;
                }

                if (item.CustomsAgentName != oldItem.CustomsAgentName) {
                    item.CustomsAgentName = oldItem.CustomsAgentName;
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
            this.EntityPM.RemoveCustomerCustomsAgentByProductPM(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.AddCustomerCustomsAgentByProductPM(item);
        });

        this.myCloner.RejectChanges();
    }
}

export class CustomerCustomsAgentByProductSplitLineViewModel extends BaseComponent {
    public Customer: CustomerPM;
    public EntityList: ProductTypeList;
    public EntityPM: CustomerCustomsAgentByProductPM;
    constructor(myCustomerPM: CustomerPM, entityList: ProductTypeList, entityPM: CustomerCustomsAgentByProductPM) {
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
            this.PartnerId = this.EntityPM.CustomsAgentId;
            this.PartnerName = this.EntityPM.CustomsAgentName;
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
                        if (this.EntityPM.CustomsAgentId != value.Id) {
                            this.EntityPM.CustomsAgentId = value.Id;
                        }

                        if (this.EntityPM.CustomsAgentName != value.EnglishName) {
                            this.EntityPM.CustomsAgentName = value.EnglishName;
                        }
                    }

                    else {
                        this.EntityPM = new CustomerCustomsAgentByProductPM(null);
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                        this.EntityPM.ProductTypeCode = this.ProductTypeCode;
                        this.EntityPM.CustomerId = this.Customer.Id;
                        this.EntityPM.CustomsAgentId = value.Id;
                        this.EntityPM.CustomsAgentName = value.EnglishName;
                        this.Customer.AddCustomerCustomsAgentByProductPM(this.EntityPM);
                    }
                }

                else {
                    this.Customer.RemoveCustomerCustomsAgentByProductPM(this.EntityPM);
                    this.EntityPM = null;
                }

                this.SetPartner();
            }
        }
    }
}
