import {Component} from '@angular/core';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {UIProperty, UIProperties}  from '../../../../../Infrastructure/Components/LogitudeComponents/UIProperties'
import {AWBOCIPM} from '../../../../../Shipment/EntityPMs/AWBOCIPM';
import {ShipmentPM} from '../../../../../Shipment/EntityPMs/ShipmentPM';
import {AWBWizardComponent} from '../AWBWizardComponent';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {ShipmentTool} from '../../../../../Shipment/Tools';
import {AppTool} from '../../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
        selector: 'OCITabComponent',
    templateUrl: './OCITabComponent.html',
})

export class OCITabComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    public Wizard: AWBWizardComponent;
    public DataContext: OCITabComponent = this;
    public ObjectTableName: string;
    public ItemsSource: AWBWizardOCIItem[];
    constructor() {
        super();  
    }

    InitTab(wizard: AWBWizardComponent) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();        
        this.BuildData();
        this.SetUIProperties();
    }

    RefreshTab() {

    }

    private Listen() {
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.BuildData();
                    this.SetUIProperties();
                }
            });

            this.Wizard.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.Wizard.EntityPM;
                    this.BuildData();
                    this.SetUIProperties();
                }
            });
        }
    }

    public IsEditingEnabled: boolean = false;
    private SetUIProperties() {
        this.IsEditingEnabled = ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });
    }

    BuildData() {
        if (this.ItemsSource == null) {
            this.ItemsSource = new Array<AWBWizardOCIItem>();
        }

        else {
            this.ItemsSource = [];
        }

        var list: AWBOCIPM[] = new Array<AWBOCIPM>();
        this.EntityPM.AWBOCIPMs.forEach((item) => {
            list.push(item);
        });

        if (list.length < 5) {
            for (var i = list.length; i < 5; i++) {

                var item: AWBOCIPM = new AWBOCIPM(null);
                item.Tenant = this.EntityPM.Tenant;
                item.ShipmentId = this.EntityPM.Id;
                item.IsAWBWizardDefault = true;
                list.push(item);
            }
        }

        list.sort((a, b) => { return (a === b) ? 0 : a ? -1 : 1 }).forEach((item) => {
            this.ItemsSource.push(new AWBWizardOCIItem(item, false, this));
        })
    }

    public Add() {
        var itemPM = new AWBOCIPM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        var itemViewModel = new AWBWizardOCIItem(itemPM, true, this);
        this.RunWindow(itemViewModel, "Add OCI line");
    }
    public Edit(itemViewModel: AWBWizardOCIItem) {
        this.RunWindow(itemViewModel, "Edit OCI line");
    }    
    public Delete(itemViewModel: AWBWizardOCIItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show('Delete this line ?');
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {                
                var itemIndex = this.ItemsSource.indexOf(itemViewModel);
                if (itemIndex > -1) {
                    this.ItemsSource.splice(itemIndex, 1);
                }

                this.EntityPM.RemoveOCI(itemViewModel.EntityPM);
            }
        });
    }
    private RunWindow(item: AWBWizardOCIItem, windowTitle: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.DataContext = item;
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/OCI/AddEditOCIComponent");
    }
}

export class AWBWizardOCIItem extends BaseComponent {
    public DataContext: AWBWizardOCIItem = this;
    public EntityPM: AWBOCIPM;
    public ShipmentPM: ShipmentPM;
    public ObjectTableName: string = "AWBOCI";    
    public IsNewEntity: boolean = false;
    public IsWindowMode: boolean = false;
    constructor(entityPM: AWBOCIPM, isNew: boolean, public fatherComponent: OCITabComponent) {
        super();
        this.EntityPM = entityPM;
        this.ShipmentPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
        this.SetUIProperties();
    }

    public IsEditingEnabled: boolean = false;
    public SetUIProperties() {

        var isFieldRequired = false;

        if (AppTool.IsNullOrEmpty(this.SupplementaryCustomsInfo)) {
            if (this.IsWindowMode) {
                isFieldRequired = true;
            }

            else {
                if (!AppTool.IsNullOrEmpty(this.CountryId) || !AppTool.IsNullOrEmpty(this.AWBInformationCode) || !AppTool.IsNullOrEmpty(this.AWBCustomsInformationCode)) {
                    isFieldRequired = true;
                }
            }
        }

        var isEditingEnabled = this.fatherComponent.IsEditingEnabled;
        this.UIProperties.SetRequired("SupplementaryCustomsInfo", this.ObjectTableName, isFieldRequired);
        this.UIProperties.SetEnabled('CountryId', this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled('AWBInformationCode', this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled('AWBCustomsInformationCode', this.ObjectTableName, isEditingEnabled);
        this.UIProperties.SetEnabled('SupplementaryCustomsInfo', this.ObjectTableName, isEditingEnabled);
        this.IsEditingEnabled = isEditingEnabled;
    }

    // Properties
    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(newValue: string) {
        if (this.EntityPM.CountryId != newValue) {
            this.EntityPM.CountryId = newValue;
            this.OnDataChanged();
        }
    }

    get AWBInformationCode() { return this.EntityPM.AWBInformationCode; }
    set AWBInformationCode(newValue: string) {
        if (this.EntityPM.AWBInformationCode != newValue) {
            this.EntityPM.AWBInformationCode = newValue;
            this.OnDataChanged();
        }
    }

    get AWBCustomsInformationCode() { return this.EntityPM.AWBCustomsInformationCode; }
    set AWBCustomsInformationCode(newValue: string) {
        if (this.EntityPM.AWBCustomsInformationCode != newValue) {
            this.EntityPM.AWBCustomsInformationCode = newValue;
            this.OnDataChanged();
        }
    }

    get SupplementaryCustomsInfo() { return this.EntityPM.SupplementaryCustomsInfo; }
    set SupplementaryCustomsInfo(newValue: string) {
        if (this.EntityPM.SupplementaryCustomsInfo != newValue) {
            this.EntityPM.SupplementaryCustomsInfo = newValue;
            this.OnDataChanged();
        }
    }

    private OnDataChanged() {

        this.SetUIProperties();

        if (!this.IsWindowMode) {

            var itemIndex = this.fatherComponent.EntityPM.AWBOCIPMs.indexOf(this.EntityPM);

            if (!AppTool.IsNullOrEmpty(this.CountryId) || !AppTool.IsNullOrEmpty(this.AWBCustomsInformationCode) || !AppTool.IsNullOrEmpty(this.AWBInformationCode) || !AppTool.IsNullOrEmpty(this.SupplementaryCustomsInfo)) {
                if (itemIndex == -1) {
                    this.fatherComponent.EntityPM.AddOCI(this.EntityPM);
                }
            }

            else {
                if (itemIndex > -1) {
                    this.fatherComponent.EntityPM.RemoveOCI(this.EntityPM);
                }
            }

            this.SetUIProperties();
            this.fatherComponent.Wizard.ValidateScreen_OCI();
        }
    }
}
