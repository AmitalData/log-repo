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
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
@Component({
    selector: 'NewComputingPartnerConmponent',
    moduleId: module.id,
    templateUrl: './NewComputingPartnerConmponent.html',
})

export class NewComputingPartnerConmponent extends BaseComponent {
    public Session: number = SessionLocator.Tenant;
    public EntityPM: ComputingPartnerPM;
    public DataContext: NewComputingPartnerConmponent = this;
    public ObjectTableName: string = "ComputingPartner";
    public ItemSourceCollection: ObservableCollection;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new ComputingPartnerPM();
        this.EntityPM.LoggedTenantId = SessionLocator.Tenant;
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.ItemSourceCollection = new ObservableCollection([]);
        this.BuildObsList();
    }
    BuildObsList() {
        this.ItemSourceCollection.Clear();
        this.ItemSourceCollection.InsertCollection(this.EntityPM.PartnerTables);

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
    AddTable() {
        var newEntityPM: ComputingPartnerTablePM = new ComputingPartnerTablePM(this.EntityPM);
        newEntityPM.ComputingPartnerId = this.EntityPM.Id;
        newEntityPM.Tenant = SessionLocator.Tenant;
        newEntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        var args: Args = new Args();
        args.entity = newEntityPM;
        args.FatherEntity = this.EntityPM;
        args.IsNewEntity = true;
        var entityResource: EntityResourceService = new EntityResourceService();
        entityResource.getEntityResourceByTableName("ComputingPartnerTable", 0).subscribe(p => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = "New Table";
            logitudeWindow.WindowArgs = args;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureComputingPartner/Components/AddEditComputingPartnerComponent');
            logitudeWindow.WindowClosed.subscribe(p => {
                this.BuildObsList();
            });
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    EditComputingTable(item: ComputingPartnerTablePM) {     
        var args: Args = new Args();
        args.entity = item;
        args.FatherEntity = this.EntityPM;
        args.IsNewEntity = false;      
        var entityResource: EntityResourceService = new EntityResourceService();
        entityResource.getEntityResourceByTableName("ComputingPartnerTable", 0).subscribe(p => {
            var logitudeWindow = new LogitudeWindow();
            logitudeWindow.Title = "Edit Table";
            logitudeWindow.WindowArgs = args;
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
