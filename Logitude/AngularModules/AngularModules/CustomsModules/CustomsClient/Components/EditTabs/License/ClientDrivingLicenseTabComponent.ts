import { Component }  from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ClientDrivingLicensePM } from '../../../../../Customs/EntityPMs/ClientDrivingLicensePM';
import { ClientDrivingLicenseTypePM } from '../../../../../Customs/EntityPMs/ClientDrivingLicenseTypePM';
import { ClientPM } from '../../../../../Customs/EntityPMs/ClientPM';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { CustomMessageProgressComponent } from '../../../../CustomsControls/Components/CustomMessageProgressComponent';
import { ClientMessagesService } from '../../../../../Customs/Services/WebServices/ClientMessagesService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';

@Component({
    
    templateUrl: './ClientDrivingLicenseTabComponent.html',
})

export class ClientDrivingLicenseTabComponent extends BaseComponent {
  public DrivingLicenseNumber: any;
  public DriversLicenseTypeCode: any;

    public entityResourceService: EntityResourceService = new EntityResourceService();
    public DataContext: ClientDrivingLicenseTabComponent = this;
    public EntityPM: ClientPM = new ClientPM();
    public ObjectTableName: string = "Customs.Client";
    isNewClient: boolean;
    private isControlEnabled: boolean = true;
    public ValidationErrorsList: string[] = [];

    private Mode: string = "";
    private newAddressButtonVisibility: boolean = true;
    private editButtonVisibility: boolean = true;

    public ClientDrivingLicenseList: ObservableCollection = new ObservableCollection([]);;
    public ClientDrivingLicenseTypeList: ObservableCollection = new ObservableCollection([]);;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _EntityArgs: EntityArgs) {
        super();

       
    }

    InitTab(EntityPM: ClientPM, IsNew: boolean) {
        this.ValidationErrorsList = [];
        this.EntityPM = EntityPM;
        this.isNewClient = IsNew;
        if (!this.EntityPM.Code.startsWith("5")) {
            this.isControlEnabled = false;
        }

        this.BuildClientDrivingLicenseList();
    }

    public get NewAddressButtonVisibility() { return this.newAddressButtonVisibility; }
    public set NewAddressButtonVisibility(newValue: boolean) { this.newAddressButtonVisibility = newValue; }

    public get EditButtonVisibility() { return this.editButtonVisibility; }
    public set EditButtonVisibility(newValue: boolean) { this.editButtonVisibility = newValue; }

    private _SelectedRow: ClientDrivingLicenseItemModel = null;
    public get SelectedRow(): ClientDrivingLicenseItemModel {
        return this._SelectedRow;
    }
    public set SelectedRow(value: ClientDrivingLicenseItemModel) {
        if (this._SelectedRow != value) {
            this._SelectedRow = value;
            this.SelectedRow.BuildClientDrivingLicenseTypeList();
        }
    }
    OnRowSelected(itemComponent: ClientDrivingLicenseItemModel) {
        this.SelectedRow = itemComponent;
        //if (itemComponent != null) {
        //    this.SelectedRow.BuildClientDrivingLicenseTypeList();
        //}
    }
    onCellSelected($event, Item: ClientDrivingLicenseItemModel) {
        if (this.SelectedRow != Item) {
            this.OnRowSelected(Item);
        }
    }
    ReloadEntityPM() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    AddClientDrivingLicense() {
        this.ValidationErrorsList = [];

        var newClientDrivingLicensePM = new ClientDrivingLicensePM(this.EntityPM);
        newClientDrivingLicensePM.Tenant = this.EntityPM.Tenant;
        newClientDrivingLicensePM.ClientId = this.EntityPM.Id;

        if (!this.EntityPM.ClientDrivingLicenses.includes(newClientDrivingLicensePM)) {
            this.EntityPM.AddClientDrivingLicense(newClientDrivingLicensePM);
            var newClientDrivingLicenseItemModel = new ClientDrivingLicenseItemModel(newClientDrivingLicensePM, this)
            this.ClientDrivingLicenseList.Insert(newClientDrivingLicenseItemModel);
            this.OnRowSelected(newClientDrivingLicenseItemModel);
        }

    }

    BuildClientDrivingLicenseList() {
        this.ClientDrivingLicenseList = new ObservableCollection([]);
        this.ClientDrivingLicenseTypeList = new ObservableCollection([]);

        if (this.EntityPM.ClientDrivingLicenses != null && this.EntityPM.ClientDrivingLicenses.length > 0) {
            for (let item of this.EntityPM.ClientDrivingLicenses) {
                this.ClientDrivingLicenseList.Insert(new ClientDrivingLicenseItemModel(item, this));
            }
            this.OnRowSelected(this.ClientDrivingLicenseList.Collection[0]);
        }
    }

    RemoveClientDrivingLicense(item: ClientDrivingLicenseItemModel) {

        if (!AppTool.IsNullOrEmpty(item)) {
            item.RemoveAllDrivingLicenseType();
            this.ClientDrivingLicenseList.Remove(item);
            this.EntityPM.RemoveClientDrivingLicense(item.ClientDrivingLicensePM);
        }

        if (this.SelectedRow == item && this.ClientDrivingLicenseList != null) {
            this.ClientDrivingLicenseTypeList = new ObservableCollection([]);
            if (this.ClientDrivingLicenseList.Collection.length == 0) {
                this.OnRowSelected(null);
            }
            else {
                this.OnRowSelected(this.ClientDrivingLicenseList.Collection[this.ClientDrivingLicenseList.Collection.length - 1]);
            }
        }
    }

    AddClientDrivingLicenseType() {
        this.ValidationErrorsList = [];

        if (this.SelectedRow == null) {
            this.ValidationErrorsList.push("חובה להזין/לבחור רישיון נהיגה");
            return;
        }

        var newClientDrivingLicenseTypePM = new ClientDrivingLicenseTypePM(this.EntityPM);
        newClientDrivingLicenseTypePM.Tenant = this.EntityPM.Tenant;
        newClientDrivingLicenseTypePM.ClientId = this.EntityPM.Id;

        if (!this.ClientDrivingLicenseTypeList.Collection.includes(newClientDrivingLicenseTypePM)) {
            this.SelectedRow.AddClientDrivingLicenseType(newClientDrivingLicenseTypePM);
            this.ClientDrivingLicenseTypeList.Insert(new ClientDrivingLicenseTypeItemModel(newClientDrivingLicenseTypePM, this));
        }

    }

    RemoveClientDrivingLicenseType(item: ClientDrivingLicenseTypeItemModel) {
        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClientDrivingLicenseTypeList.Remove(item);
            this.SelectedRow.RemoveClientDrivingLicenseType(item.ClientDrivingLicenseTypePM);
        }
    }
}


export class ClientDrivingLicenseItemModel extends BaseComponent {
    public ClientDrivingLicensePM: ClientDrivingLicensePM = null;
    public ObjectTableName = "Customs.ClientDrivingLicense";
    public DataContext = this;
    Parent: ClientDrivingLicenseTabComponent;

    constructor(private clientDrivingLicensePM: ClientDrivingLicensePM, parent: ClientDrivingLicenseTabComponent) {
        super();
        this.ClientDrivingLicensePM = clientDrivingLicensePM;
        this.Parent = parent;
        this.BuildClientDrivingLicenseTypeList();
    }

    //#region Properties

    get DrivingLicenseNumber() { return this.ClientDrivingLicensePM.DrivingLicenseNumber; }
    set DrivingLicenseNumber(value: string) {
        if (this.ClientDrivingLicensePM.DrivingLicenseNumber != value) {
            this.ClientDrivingLicensePM.DrivingLicenseNumber = value;
            if (this.Parent.SelectedRow != this) {
                this.Parent.OnRowSelected(this);
            }
        }
    }

    get DriverLicenseValidityDate() { return this.ClientDrivingLicensePM.DriverLicenseValidityDate; }
    set DriverLicenseValidityDate(value: Date) {
        if (this.ClientDrivingLicensePM.DriverLicenseValidityDate != value) {
            this.ClientDrivingLicensePM.DriverLicenseValidityDate = value;

        }
    }

    get DrivingLicenseCountryID() { return this.ClientDrivingLicensePM.DrivingLicenseCountryID; }
    set DrivingLicenseCountryID(value: string) {
        if (this.ClientDrivingLicensePM.DrivingLicenseCountryID != value) {
            this.ClientDrivingLicensePM.DrivingLicenseCountryID = value;
        }
    }

    get DrivingLicenseCountryName() { return this.ClientDrivingLicensePM.DrivingLicenseCountryName; }
    set DrivingLicenseCountryName(value: string) {
        if (this.ClientDrivingLicensePM.DrivingLicenseCountryName != value) {
            this.ClientDrivingLicensePM.DrivingLicenseCountryName = value;
        }
    }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }
    }

    BuildClientDrivingLicenseTypeList() {
        this.Parent.ClientDrivingLicenseTypeList = new ObservableCollection([]);

        if (this.ClientDrivingLicensePM.ClientDrivingLicenseTypes != null && this.ClientDrivingLicensePM.ClientDrivingLicenseTypes.length > 0) {
            for (let item of this.ClientDrivingLicensePM.ClientDrivingLicenseTypes) {
                this.Parent.ClientDrivingLicenseTypeList.Insert(new ClientDrivingLicenseTypeItemModel(item, this.Parent));
            }
        }
    }

    AddClientDrivingLicenseType(newClientDrivingLicenseTypePM: ClientDrivingLicenseTypePM) {
        this.ClientDrivingLicensePM.AddClientDrivingLicenseType(newClientDrivingLicenseTypePM);
    }

    RemoveAllDrivingLicenseType() {
        if (this.ClientDrivingLicensePM.ClientDrivingLicenseTypes != null && this.ClientDrivingLicensePM.ClientDrivingLicenseTypes.length > 0) {
            for (let item of this.ClientDrivingLicensePM.ClientDrivingLicenseTypes) {
                this.ClientDrivingLicensePM.RemoveClientDrivingLicenseType(item);
            }
        }
    }

    RemoveClientDrivingLicenseType(item: ClientDrivingLicenseTypePM) {
        if (!AppTool.IsNullOrEmpty(item)) {
            this.ClientDrivingLicensePM.RemoveClientDrivingLicenseType(item);
        }
    }
}

export class ClientDrivingLicenseTypeItemModel extends BaseComponent {
    public ClientDrivingLicenseTypePM: ClientDrivingLicenseTypePM = null;
    public ObjectTableName = "Customs.ClientDrivingLicenseType";
    public DataContext = this;
    Parent: ClientDrivingLicenseTabComponent;

    constructor(private clientDrivingLicenseTypePM: ClientDrivingLicenseTypePM, parent: ClientDrivingLicenseTabComponent) {
        super();
        this.ClientDrivingLicenseTypePM = clientDrivingLicenseTypePM;
        this.Parent = parent;
    }

    //#region Properties
    get DriversLicenseTypeCode() { return this.ClientDrivingLicenseTypePM.DriversLicenseTypeCode; }
    set DriversLicenseTypeCode(value: string) {
        if (this.ClientDrivingLicenseTypePM.DriversLicenseTypeCode != value) {
            this.ClientDrivingLicenseTypePM.DriversLicenseTypeCode = value;
        }
    }
    //#endregion

    OnDriversLicenseLostFocus(event) {
        this.Parent.ValidationErrorsList = [];

        if (!AppTool.IsNullOrEmpty(this.DriversLicenseTypeCode)) {
            if (this.DriversLicenseTypeCode.toString().length > 2) {
                this.Parent.ValidationErrorsList.push("סוג רשיון ארוך מדי (ניתן להזין עד 2 תווים)");
            }
        }
    }
}


