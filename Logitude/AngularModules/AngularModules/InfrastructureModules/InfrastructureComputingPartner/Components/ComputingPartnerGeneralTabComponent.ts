import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {ComputingPartnerPM} from '../../../Common/EntityPMs/ComputingPartnerPM';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ComputingPartnerTablePM} from '../../../Common/EntityPMs/ComputingPartnerTablePM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ComputingPartnerPMService} from '../../../Common/Services/StandardPMs/ComputingPartnerPMService';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {AddEditComputingPartnerComponent} from './AddEditComputingPartnerComponent';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'ComputingPartnerGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './ComputingPartnerGeneralTabComponent.html',
})

export class ComputingPartnerGeneralTabComponent extends BaseComponent {
    public Session: number = SessionLocator.Tenant;
    public EntityPM: ComputingPartnerPM;
    public DataContext: ComputingPartnerGeneralTabComponent = this;
    public ObjectTableName: string = "ComputingPartner";
    public ItemSourceCollection: ObservableCollection;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;       
        this.ItemSourceCollection = new ObservableCollection([]);
        this.BuildObsList();
        this.SetUIProperties();
    }
    
    public InActiveEnabled: boolean = true;
    SetUIProperties() {
        var isFieldsEnabled: boolean = SessionLocator.Tenant == this.EntityPM.Tenant? true : false;
        this.UIProperties.SetEnabled("Name", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, isFieldsEnabled);
        this.UIProperties.SetEnabled("Description", this.ObjectTableName, isFieldsEnabled);
        this.InActiveEnabled = isFieldsEnabled;

    }






    BuildObsList() {
        this.ItemSourceCollection.Clear();
        var list: Array<AddEditComputingPartnerComponent> = [];
        this.EntityPM.PartnerTables.sort((a, b) => { return (a.Tenant === b.Tenant) ? 0 : (a.Tenant < b.Tenant) ? -1 : 1 }).forEach(item => {
            var model = new AddEditComputingPartnerComponent();
            model.EntityPM = item;
            model.myComputingPartnerPM = this.EntityPM;
            model.IsNewEntity = false;
            list.push(model);
        });
        this.ItemSourceCollection.InsertCollection(list);

    }

    OkButtonClicked() {


        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();
            var myService: ComputingPartnerPMService = new ComputingPartnerPMService();
            myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {

                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }


    }
    OpenTranslation(item: AddEditComputingPartnerComponent) {

        var logitudeWindow: LogitudeWindow = new LogitudeWindow();

        var entityResource: EntityResourceService = new EntityResourceService();
        logitudeWindow.Title = "Translation: " + item.myComputingPartnerPM.Name + " (" + item.EntityPM.Name + ")";
        logitudeWindow.WindowArgs = item.EntityPM;
        entityResource.getEntityResourceByTableName(item.ObjectTableName, 0).subscribe(p => {
            logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/ComputingPartnerTranslateComponent');
            logitudeWindow.WindowClosed.subscribe(p => {


            });

        });

    }

   

    AddTable() {
        var newEntityPM: ComputingPartnerTablePM = new ComputingPartnerTablePM(null);
        newEntityPM.ComputingPartnerId = this.EntityPM.Id;
        newEntityPM.Tenant = SessionLocator.Tenant;
        newEntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        var args: Args = new Args();
        args.entity = newEntityPM;
        args.FatherEntity = this.EntityPM;
        args.IsNewEntity = true;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "New Table";
        logitudeWindow.WindowArgs = args;
        var entityResource: EntityResourceService = new EntityResourceService();
        entityResource.getEntityResourceByTableName("ComputingPartnerTable", 0).subscribe(p => {
            logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/AddEditComputingPartnerComponent');
            logitudeWindow.WindowClosed.subscribe(p => {
                this.BuildObsList();
            });

        });
    }


    EditComputingTable(item: AddEditComputingPartnerComponent) {
        var args: Args = new Args();
        args.entity = item.EntityPM;
        args.FatherEntity = this.EntityPM;
        args.IsNewEntity = false;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Table";
        logitudeWindow.WindowArgs = args;
        var entityResource: EntityResourceService = new EntityResourceService();
        entityResource.getEntityResourceByTableName("ComputingPartnerTable", 0).subscribe(p => {
            logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/AddEditComputingPartnerComponent');
        });
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value)
            this.EntityPM.Name = value;
    }

    public get Remarks() { return this.EntityPM.Remarks; }
    public set Remarks(value: string) {
        if (this.EntityPM.Remarks != value)
            this.EntityPM.Remarks = value;
    }

    public get Code() { return this.EntityPM.Code; }
    public set Code(value: string) {
        if (this.EntityPM.Code != value)
            this.EntityPM.Code = value;
    }

    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(value: boolean) {
        if (this.EntityPM.InActive != value)
            this.EntityPM.InActive = value;
    }

    public get Description() { return this.EntityPM.Description; }
    public set Description(value: string) {
        if (this.EntityPM.Description != value)
            this.EntityPM.Description = value;
    }



}






class Args {
    public entity: ComputingPartnerTablePM;
    public FatherEntity: ComputingPartnerPM;
    public IsNewEntity: boolean

}
