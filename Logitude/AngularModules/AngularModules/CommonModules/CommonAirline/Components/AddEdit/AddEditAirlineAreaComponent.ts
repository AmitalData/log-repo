import {Component, Output, EventEmitter} from '@angular/core';
import { AirlinePM } from '../../../../Common/EntityPMs/AirlinePM';
import { AirlineAreaPM } from '../../../../Common/EntityPMs/AirlineAreaPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { PortList } from '../../../../Common/EntityLists/PortList';
import { AirlineAreasPortPM } from '../../../../Common/EntityPMs/AirlineAreasPortPM';
import { PortListService } from '../../../../Common/Services/StandardLists/PortListService';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAirlineAreaComponent.html',
})

export class AddEditAirlineAreaComponent extends BaseComponent {
    public EntityPM: AirlineAreaPM;
    public AirlinePM: AirlinePM;

    public ObjectTableName: string="AirlineArea";
    public DataContext: AddEditAirlineAreaComponent = this;
    public IsNew: boolean;
    public ItemList = [];
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public portListService: PortListService = new PortListService();

    constructor() {
        super();
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) { this.EntityPM.Name = value; }


    public get Description() { return this.EntityPM.Description; }
    public set Description(value: string) { this.EntityPM.Description = value; }
    
    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.AirlinePM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];
        if (this.IsNew) {
            this.EntityPM = new AirlineAreaPM(this.AirlinePM);
        }
        
        else {
            this.EntityPM = windowArgs['Entity'];
            this.EntityPM.AirlineAreasPorts.forEach(item => {
                this.portListService.getSingleFromCache(item.PortId).subscribe(p => {
                    if (!p.HasError) {
                        if (p.Result) {
                            this.ItemList.push(new DestinationClass(this, p.Result,false));
                        }
                    }
                    else {
                        this.ValidationErrorsList = p.ErrorsArray;
                    }
                });

            });


        }
        this.IsResourcesReady = true;
    }

    ChoosePort() {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 320;
        logWindow.Height = 170;

        var itemComponent = new DestinationClass(this, null,true);
        logWindow.DataContext = itemComponent;

        logWindow.Title = "Choose Ports";
        logWindow.Show('./CommonModules/CommonAirline/Components/AddEdit/ChoosePortComponent');
    }

    DeletePort(Item: DestinationClass) {
        var index = this.ItemList.indexOf(Item);
        if (index > -1) {
            this.ItemList.splice(index,1);
        }
        this.EntityPM.RemoveAirlineAreasPortPM(Item.EntityPM);
    }
   

    SaveButtonClicked() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name is required");
        }

        if (this.EntityPM.AirlineAreasPorts.filter(p => p.ChangeSetOp != "3")[0] == null) {
            this.ValidationErrorsList.push("At Least one port is required");
        }
       
      
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {

                this.EntityPM.CreatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
                this.EntityPM.UpdatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
                this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                this.EntityPM.AirlineId = this.AirlinePM.Id;
                this.AirlinePM.AddAirlineAreaPM(this.EntityPM);

            }
            this.CurrentSession.CloseCurrentWindow();

        }       
    }

     CancelButtonClicked() {
         this.CurrentSession.CloseCurrentWindow();
    }

  


}


export class DestinationClass extends BaseComponent {
    public Indication: string;
    public Name: string;
    public Code: string;
    public Id: string;

    public EntityPM: AirlineAreasPortPM;
    constructor(public fatherComponent: AddEditAirlineAreaComponent, Port: PortList,IsNew: boolean) {
        super();   
        if (IsNew && Port!=null) {
            this.Indication = "Port";
            this.Name = Port.EnglishName;
            this.Code = Port.Code;
            this.Id = Port.Id;
            this.EntityPM = new AirlineAreasPortPM(fatherComponent.EntityPM);
            this.EntityPM.Name = this.Name;
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.AirlineAreaId = fatherComponent.EntityPM.Id;
            this.EntityPM.PortId = this.Id;
            fatherComponent.EntityPM.AddAirlineAreasPortPM(this.EntityPM);
        }
        else {
            if (Port != null) {
                this.Indication = "Port";
                this.Name = Port.EnglishName;
                this.Code = Port.Code;
                this.Id = Port.Id;
                this.EntityPM = fatherComponent.EntityPM.AirlineAreasPorts.filter(p => p.PortId == Port.Id)[0];

            }
        }
    }
}
