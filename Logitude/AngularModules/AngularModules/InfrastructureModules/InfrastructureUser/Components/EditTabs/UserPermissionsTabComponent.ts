import {Component, OnDestroy}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UserPM} from '../../../../Common/EntityPMs/UserPM';
import {UserPermittedBranchPM} from '../../../../Common/EntityPMs/UserPermittedBranchPM';
import {UserPermittedProductPM} from '../../../../Common/EntityPMs/UserPermittedProductPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {BranchList} from '../../../../Common/EntityLists/BranchList';
import {ProductTypeList} from '../../../../Common/EntityLists/ProductTypeList';
import {BranchListService} from '../../../../Common/Services/StandardLists/BranchListService';
import {ProductTypeListService} from '../../../../Common/Services/StandardLists/ProductTypeListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './UserPermissionsTabComponent.html',
})

export class UserPermissionsTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: UserPM;
    public ObjectTableName: string = "User";
    public DataContext = this;
    public UserBranches: UserBranchClass[] = [];
    public UserProducts: UserProductClass[] = [];
    public ComboBoxBranches: BranchList[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.isNotBranchRestricted = !this.IsBranchRestricted;
        this.isNotProductRestricted = !this.IsProductRestricted;
        this.SetUIProperties();
        this.BuildPermittedBranchesList();
        this.BuildPermittedProductsList();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetUIProperties();
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.SetUIProperties();
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsEditingEnabled: boolean = false;
    public IsProductsVisible: boolean = false;
    SetUIProperties() {
        var isEditingEnabled = true;
        if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString())) {
            if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = false;
            }
        }

        this.IsEditingEnabled = isEditingEnabled;

        if (FeatureLocator.HasFeaturePermession("User", "PRODUCTS")) {
            this.IsProductsVisible = true;
        }
    }

    // Branches
    public get BranchId() { return this.EntityPM.BranchId; }
    public set BranchId(value: string) {
        if (this.EntityPM.BranchId != value) {
            this.EntityPM.BranchId = value;
        }
    }

    private isNotBranchRestricted: boolean = false;
    public get IsNotBranchRestricted() { return this.isNotBranchRestricted; }
    public set IsNotBranchRestricted(value: boolean) {
        if (this.isNotBranchRestricted != value) {
            this.isNotBranchRestricted = value;
            this.EntityPM.IsBranchRestricted = !value;
            this.EntityPM.UserPermittedBranches = [];
            this.BuildPermittedBranchesList();
        }
    }

    public get IsBranchRestricted() { return this.EntityPM.IsBranchRestricted; }
    public set IsBranchRestricted(value: boolean) {
        if (this.EntityPM.IsBranchRestricted != value) {
            this.isNotBranchRestricted = !value;
            this.EntityPM.IsBranchRestricted = value;
            this.BuildPermittedBranchesList();
        }
    }

    private selectedBranch: BranchList;
    public get SelectedBranch() { return this.selectedBranch; }
    public set SelectedBranch(value: BranchList) {
        if (this.selectedBranch != value) {
            this.selectedBranch = value;
        }
    }
    SelectedBranchChanged(myBranch: BranchList) {
        if (this.SelectedBranch != myBranch) {
            this.SelectedBranch = myBranch;

            if (myBranch == null) {
                this.BranchId = null;
            }

            else {
                this.BranchId = myBranch.Id;
            }
        }
    }

    // Products
    public get ProductTypeCode() { return this.EntityPM.ProductTypeCode; }
    public set ProductTypeCode(value: string) {
        if (this.EntityPM.ProductTypeCode != value) {
            this.EntityPM.ProductTypeCode = value;
        }
    }

    private isNotProductRestricted: boolean = false;
    public get IsNotProductRestricted() { return this.isNotProductRestricted; }
    public set IsNotProductRestricted(value: boolean) {
        if (this.isNotProductRestricted != value) {
            this.isNotProductRestricted = value;
            this.EntityPM.IsProductRestricted = !value;

            this.EntityPM.UserPermittedProducts = [];
            this.BuildPermittedProductsList();
        }
    }

    public get IsProductRestricted() { return this.EntityPM.IsProductRestricted; }
    public set IsProductRestricted(value: boolean) {
        if (this.EntityPM.IsProductRestricted != value) {
            this.isNotProductRestricted = !value;
            this.EntityPM.IsProductRestricted = value;
            this.BuildPermittedProductsList();
        }
    }

    BuildPermittedBranchesList() {
        var myService = new BranchListService();
        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            this.UserBranches = [];
            this.ComboBoxBranches = [];

            if (!myResponse.HasError) {
                var allItems: BranchList[] = myResponse.Result;

                allItems = allItems.sort(function (a, b) { return a.Id.toLowerCase() == b.Id.toLowerCase() ? 0 : a.Id.toLowerCase() < b.Id.toLowerCase() ? -1 : 1; });
                allItems = allItems.filter(bra => bra.InActive == false);

                allItems.forEach(item => {
                    var newBranch = new UserBranchClass(item, this);
                    this.UserBranches.push(newBranch);
                    
                    if (this.IsNotBranchRestricted) {
                        this.ComboBoxBranches.push(item);
                    }

                    else {
                        if (newBranch.IsPermitted) {
                            this.ComboBoxBranches.push(item);
                        }
                    }
                });

                this.SelectedBranch = this.ComboBoxBranches.filter(f => f.Id == this.BranchId)[0];
            }
        });
    }
    BuildPermittedProductsList() {
        if (this.IsProductsVisible) {
            var myService = new ProductTypeListService();
            myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
                this.UserProducts = [];

                if (!myResponse.HasError) {
                    var allItems: ProductTypeList[] = myResponse.Result;

                    allItems = allItems.filter(f => f.InActive == false);
                    allItems = allItems.sort(function (a, b) { return a.Name.toLowerCase() == b.Name.toLowerCase() ? 0 : a.Name.toLowerCase() < b.Name.toLowerCase() ? -1 : 1; });

                    allItems.forEach(item => {
                        this.UserProducts.push(new UserProductClass(item, this));
                    });
                }
            });
        }
    }
}
export class UserBranchClass {
    public Entity: BranchList;
    constructor(entity: BranchList, private fatherComponent: UserPermissionsTabComponent) {
        this.Entity = entity;

        if (fatherComponent.EntityPM.UserPermittedBranches.filter(f => f.BranchId == this.Id).length > 0) {
            this.isPermitted = true;
        }
    }

    public get Id() { return this.Entity.Id; }
    public get Name() { return this.Entity.EnglishName; }
    public get IsDefauldBranch() { return this.Id == this.fatherComponent.BranchId ? true : false; }

    private isPermitted: boolean = false;
    public get IsPermitted() { return this.isPermitted; }
    public set IsPermitted(value: boolean) {
        if (this.isPermitted != value) {
            this.isPermitted = value;

            var itemPM = this.fatherComponent.EntityPM.UserPermittedBranches.filter(f => f.BranchId == this.Id)[0];

            if (value) {
                if (itemPM == null) {
                    itemPM = new UserPermittedBranchPM(null);
                    itemPM.BranchId = this.Id;
                    itemPM.Tenant = SessionLocator.Tenant;
                    itemPM.UserId = this.fatherComponent.EntityPM.Id;

                    this.fatherComponent.EntityPM.AddUserPermittedBranchPM(itemPM);
                }
            }

            else {
                if (itemPM != null) {
                    this.fatherComponent.EntityPM.RemoveUserPermittedBranchPM(itemPM);
                }
            }

            this.UpdateFather();
        }
    }

    UpdateFather() {
        if (this.fatherComponent.EntityPM.UserPermittedBranches.length == 1) {
            this.fatherComponent.BranchId = this.fatherComponent.EntityPM.UserPermittedBranches[0].BranchId;
        }

        else if (this.fatherComponent.EntityPM.UserPermittedBranches.filter(f => f.BranchId == this.fatherComponent.BranchId).length == 0) {
            if (this.fatherComponent.EntityPM.UserPermittedBranches.length == 0) {
                this.fatherComponent.BranchId = null;
            }

            else {
                this.fatherComponent.BranchId = this.fatherComponent.EntityPM.UserPermittedBranches[0].BranchId;
            }
        }
    }
}
export class UserProductClass {
    public Entity: ProductTypeList;
    constructor(entity: ProductTypeList, private fatherComponent: UserPermissionsTabComponent) {
        this.Entity = entity;

        if (fatherComponent.EntityPM.UserPermittedProducts.filter(f => f.ProductTypeCode == this.Code).length > 0) {
            this.isPermitted = true;
        }
    }

    public get Id() { return this.Entity.Id; }
    public get Code() { return this.Entity.Code; }
    public get Name() { return this.Entity.Name; }

    public get DirectionId() {
        if (this.Code == "CI") {
            return "C";
        }

        else {
            return this.Code[1];
        }
    }

    public get TransportModeId() {
        if (this.Code == "CI") {
            return null;
        }

        else {
            return this.Code[0];
        }
    }

    public get IsDefauldProduct() { return this.Code == this.fatherComponent.ProductTypeCode ? true : false; }

    private isPermitted: boolean = false;
    public get IsPermitted() { return this.isPermitted; }
    public set IsPermitted(value: boolean) {
        if (this.isPermitted != value) {
            this.isPermitted = value;

            var itemPM = this.fatherComponent.EntityPM.UserPermittedProducts.filter(f => f.ProductTypeCode == this.Code)[0];

            if (value) {
                if (itemPM == null) {
                    itemPM = new UserPermittedProductPM(null);
                    itemPM.ProductTypeCode = this.Code;
                    itemPM.Tenant = SessionLocator.Tenant;
                    itemPM.UserId = this.fatherComponent.EntityPM.Id;
                    this.fatherComponent.EntityPM.AddUserPermittedProductPM(itemPM);
                }
            }

            else {
                if (itemPM != null) {
                    this.fatherComponent.EntityPM.RemoveUserPermittedProductPM(itemPM);
                }
            }

            this.UpdateFather();
        }
    }    

    UpdateFather() {
        if (this.fatherComponent.EntityPM.UserPermittedProducts.length == 1) {
            this.fatherComponent.ProductTypeCode = this.fatherComponent.EntityPM.UserPermittedProducts[0].ProductTypeCode;
        }

        else if (this.fatherComponent.EntityPM.UserPermittedProducts.filter(f => f.ProductTypeCode == this.fatherComponent.ProductTypeCode).length == 0) {
            if (this.fatherComponent.EntityPM.UserPermittedProducts.length == 0) {
                this.fatherComponent.ProductTypeCode = null;
            }

            else {
                this.fatherComponent.ProductTypeCode = this.fatherComponent.EntityPM.UserPermittedProducts[0].ProductTypeCode;
            }
        }
    }
}
