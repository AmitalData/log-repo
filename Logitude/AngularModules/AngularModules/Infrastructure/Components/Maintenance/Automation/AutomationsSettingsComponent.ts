
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit }  from '@angular/core';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';

import {AppTool, DateTool} from '../../../../Infrastructure/Tools'
declare var System: any;
declare var window: any;
import {AutomationItemViewModel} from './ViewModel/AutomationItemViewModel';
import {AutomationExtendedPMService} from '../../../../Common/Services/ExtendedPMs/AutomationExtendedPMService';
import {AutomationPMService} from '../../../../Common/Services/StandardPMs/AutomationPMService';
import {AutomationPM} from '../../../../Common/EntityPMs/AutomationPMExtended';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {AutomationArgs} from '../../../../Infrastructure/DataContracts/AutomationArgs';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObjectFieldPMExtendedService} from '../../../../Infrastructure/Services/ExtendedPMs/ObjectFieldPMExtendedService';
import { ServiceHelper } from 'Infrastructure/Utilities/ServiceHelper';

@Component({
    

    selector: 'AutomationsSettingsComponent',
    templateUrl: './AutomationsSettingsComponent.html',

    providers: [AutomationExtendedPMService, AutomationPMService],
})
export class AutomationsSettingsComponent implements OnInit {

    IsShowTabUpdate: boolean = false;
    IsShowTabDocumentUpdate: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private objectFieldPMExtendedService: ObjectFieldPMExtendedService;
    


    SelectedTabCode: string = "";
    ObjectTableId: string;
    ObjectTableName: string;
    ShowIncludeInactiveOnCreateCheckBoxKey: string;
    ShowIncludeInactiveOnUpDateCheckBoxKey: string;

    AutomationList: AutomationItemViewModel[] = [];
    ScheduleAutomationList: AutomationItemViewModel[] = [];
    OnUpdateAutomationList: AutomationItemViewModel[] = [];
    OnCreateAutomationList: AutomationItemViewModel[] = [];
    OnDocumentUpdateAutomationList: AutomationItemViewModel[] = [];

    OnCreateAutomationListSelected: AutomationItemViewModel;
    OnUpdateAutomationListSelected: AutomationItemViewModel;

    OnCreateAutomationTabTitle: string;
    OnUpdateAutomationTabTitle: string;
    ScheduleAutomationTabTitle: string;
    OnDocumentUpdateAutomationTabTitle: string;

    IsAddAtomationEnable: boolean = false;
    OnUpdateTabVisibility: boolean = false;
    ScheduleTabVisibility: boolean = false;
    IsShowSendInterfaceResult: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _automationExtendedPMService: AutomationExtendedPMService, public _automationPMService: AutomationPMService) {

        this.objectFieldPMExtendedService = new ObjectFieldPMExtendedService();


        if (FeatureLocator.HasFeaturePermession("Automation", "SENDINTERFACERESULT") && SessionLocator.LoggedUserPM.IsCustomerCare) {
            this.IsShowSendInterfaceResult = true;
        }



    }

    ngOnInit(


    ) {


    }
    EntityDisplayName: string;
    SetDataContext(dataContext: any) {
        if (dataContext) {
            var tableName: string = dataContext.ObjectTableName;
            this.EntityDisplayName = dataContext.DisplayName;
            this.AutomationList = [];


            if (tableName) {
                var table = window.ObjectTables.filter(d => d.Name == tableName)[0];

                if (table) {
                    this.ObjectTableName = table.Name;
                    this.ObjectTableId = table.Id;
                    this.LoadAutomationsList();
                }

                this._entityResourceService.getEntityResourceByTableName(tableName).subscribe((response:any) => {


                    if (tableName == "Master") {
                        this._entityResourceService.getEntityResourceByTableName("Shipment").subscribe((response:any) => {
                            this.LoadEntityAuomationAllowedinAutomationConditionsObjectFields(tableName);

                        });
                    } else this.LoadEntityAuomationAllowedinAutomationConditionsObjectFields(tableName);
                });
            }
        }

    }







    public EntityObjectAutomationFieldLists: any[];
    LoadEntityAuomationAllowedinAutomationConditionsObjectFields(tableName: string) {
        var entityObjectTable: string[] =[];
        var tableId: string = this.ObjectTableId; 
        if (this.ObjectTableName == "Master") {
            var table = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
            if (table) tableId = table.Id;
        }

        if (tableName == "Quote") entityObjectTable.push("QuoteTemplate");

        window.ObjectFields.filter(f => f.DisplayInAutomationAsEnitity == true && f.ObjectTableId == tableId && (!f.RecordType || (f.RecordType && f.RecordType.split(',').filter(d => d == tableName)[0]))).forEach((objectField) => {
            if (objectField.LookUpTableId) {
                if (entityObjectTable.indexOf(objectField.ObjectTable_LookUpTableName) == -1) {
                    entityObjectTable.push(objectField.ObjectTable_LookUpTableName);
                }
            }
        });

        if (entityObjectTable.length > 0) this.LoadAdditionalEntityResource(entityObjectTable);
        else this.Start();


    }


    NumberOfLeadedAdditionalEntityResource: number = 0;
    LoadAdditionalEntityResource(additionalentityObjectTableNames: string[]) {
        this.NumberOfLeadedAdditionalEntityResource = 0;
        var additionalentityObjectTableCount: number = additionalentityObjectTableNames.length;

            additionalentityObjectTableNames.forEach((objectTableName) => {
                this._entityResourceService.getEntityResourceByTableName(objectTableName).subscribe((response: any) => { this.NumberOfLeadedAdditionalEntityResource += 1; this.CompleteLoadAdditionalEntityResource(additionalentityObjectTableCount) });

        });
    }

    CompleteLoadAdditionalEntityResource(additionalentityObjectTableCount:number) {

        if (additionalentityObjectTableCount == this.NumberOfLeadedAdditionalEntityResource) this.Start();
    }

    Start() {

        this.ShowIncludeInactiveOnCreateCheckBoxKey = Guid.newGuid();
        this.ShowIncludeInactiveOnUpDateCheckBoxKey = Guid.newGuid();

        this.SelectedTabCode = "CRA";
  

        if (!FeatureLocator.HasFeaturePermession("Automation", "NEW")) {
            this.IsAddAtomationEnable = false;
        }
        else {
            this.IsAddAtomationEnable = true;
        }

        if (this.ObjectTableName != "LogitudeMessagesTransmissionLog") {
            this.ScheduleTabVisibility = true;
            this.OnUpdateTabVisibility = true;
            this.IsShowTabUpdate = true;
            this.IsShowTabDocumentUpdate = true;
        }

    }


    LoadAutomationsList() {

        this.CurrentSession.StartBusyIndicatorLoading();
        this.AutomationList = [];
        this._automationExtendedPMService.getAutomationesByObjectTableId(this.ObjectTableId, SessionLocator.Tenant).subscribe((res:any) => {

            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                myResult.forEach((automationes) => {

                    var automationItemViewModel = new AutomationItemViewModel(automationes);
                    this.AutomationList.push(automationItemViewModel);

                });

                if (!this.IsShowSendInterfaceResult) {
                    this.AutomationList = this.AutomationList.filter(d => d.ResultCode != "SENDINTERFACE");
                }



                this.RefreshAutomationList("OnCreate");
                this.RefreshAutomationList("OnUpdate");
                this.RefreshAutomationList("OnDocumentUpdate");
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    AutomationUpdateListChangeSelected(item: AutomationItemViewModel) {
        this.OnUpdateAutomationListSelected = item;
        this.OnUpdateAutomationList.forEach((automation) => {
            automation.IsShowArrowUpDown = false;
        });

        this.OnUpdateAutomationListSelected.IsShowArrowUpDown = true;

    }

    AutomationOnCreateListChangeSelected(item: AutomationItemViewModel) {
        this.OnCreateAutomationListSelected = item;
        this.OnCreateAutomationList.forEach((automation) => {
            automation.IsShowArrowUpDown = false;
        });
        this.OnCreateAutomationListSelected.IsShowArrowUpDown = true;
    }




    IsInCludeInActiveOnCreateCheckBox: boolean;
    IsInCludeInActiveOnUpdateCheckBox: boolean;
    CheckboxInCludeInActiveClick(type: string) {
        if (type == 'OnCreate') {

            this.IsInCludeInActiveOnCreateCheckBox = !this.IsInCludeInActiveOnCreateCheckBox;
            this.RefreshAutomationList("OnCreate");
        }
        else if (type == 'OnUpdate') {
            this.IsInCludeInActiveOnUpdateCheckBox = !this.IsInCludeInActiveOnUpdateCheckBox;
            this.RefreshAutomationList("OnUpdate");
        }
    }

    SetObjectTableInWindoWArgs(windowArgs:any) {
        windowArgs.ObjectTableId = this.ObjectTableId;
        windowArgs.ObjectTableName = this.ObjectTableName;
        if (this.ObjectTableName == "Master") {
            windowArgs.IsMasterShipment = true;
            var table = window.ObjectTables.filter(d => d.Name == "Shipment")[0];
            if (table) {
                windowArgs.ObjectTableId = table.Id;
                windowArgs.ObjectTableName = table.Name;
            }
        }

        windowArgs.AutomationItemClass = this.AutomationItemClass;

    }

    public AutomationItemClass: any;
    SetWindowArgs(args: any) {
        this.AutomationItemClass = args.AutomationItemClass;
    }

    onAddAutomation(event) {
        this.AddAutomation(event.type);
    }

    onEditAutomation(event) {
        this.EditAutomation(event.type, event.item);
    }

    onRefreshAutomation(event) {
        this.RefreshAutomationList(event.automationListType, event.IsInCludeInActive)
    }

    AddAutomation(type: string) {
        var newEntity: AutomationPM = new AutomationPM();
        newEntity.Type = type;
        newEntity.Tenant = SessionLocator.TenantPM.Id;
        newEntity.CreatedByUserId = SessionLocator.LoggedUserId;
        newEntity.UpdatedByUserId = SessionLocator.LoggedUserId;
        newEntity.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        newEntity.UpdatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
        newEntity.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntity.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        newEntity.Description = "";
        newEntity.Version = 1,
        newEntity.Inactive = false;
        newEntity.ResultCode = "EMAIL";
        newEntity.DocumentTypeId = "";
        newEntity.TemplateId = "";
        newEntity.Id = "";
        newEntity.From = "";
        newEntity.FromEmail = "";
        newEntity.AutomationXML = "";
        newEntity.ObjectTableId = this.ObjectTableId;
        newEntity.Order = this.AutomationList.filter(d => d.EntityPM.Type == type) ? this.AutomationList.filter(d => d.EntityPM.Type == type).length : 0;
        newEntity.Name = "";

        var windowArgs: any = {};

        this.SetObjectTableInWindoWArgs(windowArgs);

        windowArgs.DataViewModel = this;
        windowArgs.AutomationPM = ServiceHelper.CloneEntityPM(newEntity);
        windowArgs.Mode = "Add";
        windowArgs.IsNewEntity = true;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 870;
        logWindow.Height = 815;
        logWindow.Title = "Add " + this.EntityDisplayName+" Automation";
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent");
      

    }


    EditAutomation(type: string, item: AutomationItemViewModel) {
        var isDirty: boolean = item.EntityPM.IsDirty;
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.AutomationPM =  ServiceHelper.CloneEntityPM(item.EntityPM);
        windowArgs.Mode = "Edit";
        this.SetObjectTableInWindoWArgs(windowArgs);

        windowArgs.IsNewEntity = false;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 870;
        logWindow.Height = 815;
        logWindow.Title = "Edit " + this.EntityDisplayName + " Automation";
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./Infrastructure/Components/Maintenance/Automation/AddEditAutomationsComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "Cancel") {
                item.EntityPM.IsDirty = isDirty;
            }
        });


    }



    RefreshAutomationList(automationListType: string, IsInCludeInActive: boolean = false) {

        if (automationListType == "OnCreate") {

            if (this.IsInCludeInActiveOnCreateCheckBox) this.OnCreateAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "OnCreate");
            else this.OnCreateAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "OnCreate" && d.EntityPM.Inactive == false);

            this.OnCreateAutomationList = this.OnCreateAutomationList.sort((a, b) => { return a.Order - b.Order });

        } else if (automationListType == "OnUpdate") {

            if (this.IsInCludeInActiveOnUpdateCheckBox) this.OnUpdateAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "OnUpdate");
            else this.OnUpdateAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "OnUpdate" && d.EntityPM.Inactive == false);

            this.OnUpdateAutomationList = this.OnUpdateAutomationList.sort((a, b) => { return a.Order - b.Order });

        }
        else if (automationListType == "OnDocumentUpdate") {

            if (IsInCludeInActive) this.OnDocumentUpdateAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "OnDocumentUpdate");
            else this.OnDocumentUpdateAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "OnDocumentUpdate" && d.EntityPM.Inactive == false);

            this.OnDocumentUpdateAutomationList = this.OnDocumentUpdateAutomationList.sort((a, b) => { return a.Order - b.Order });

        }
        //else if (automationListType == "Schedule") {

        //    if (Inacive) this.ScheduleAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "Schedule");
        //    else this.ScheduleAutomationList = this.AutomationList.filter(d => d.EntityPM.Type == "Schedule" && d.Inactive == false);
        //    this.ScheduleAutomationList = this.ScheduleAutomationList.sort((a, b) => { return a.Order - b.Order });
        //}

        this.RefreshAutomationTitles();

    }

    RefreshAutomation(item: AutomationPM , pross:string) {

        if (pross == "Edit") this.AutomationList =  this.AutomationList.filter(d=> d.Id != item.Id);
        this.AutomationList.push(new AutomationItemViewModel(item));
        
        this.RefreshAutomationList(item.Type);

    }

    RefreshAutomationTitles()
      {
          this.OnCreateAutomationTabTitle = "On Create (" + this.OnCreateAutomationList.length.toString() + ")";
          this.OnUpdateAutomationTabTitle = "On Update (" + this.OnUpdateAutomationList.length.toString() + ")";
          this.ScheduleAutomationTabTitle = "Schedule (" + this.ScheduleAutomationList.length.toString() + ")";
          this.OnDocumentUpdateAutomationTabTitle = "On Document Update (" + this.OnDocumentUpdateAutomationList.length.toString() + ")";

     }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
     
        var automations: AutomationItemViewModel[] = this.AutomationList.filter(d=> d.EntityPM.IsDirty);
        var automationArgsLists: AutomationArgs[] = [];
        if (automations && automations.length > 0) {

            

            automations.forEach((item) => {
                var automationArgs: AutomationArgs = new AutomationArgs();
                automationArgs.Id = item.Id;
                automationArgs.Tenant = item.Tenant;
                automationArgs.Order = item.Order;
                automationArgsLists.push(automationArgs);
            });

            //automations.forEach((item) => {
            //    item.EntityPM.AutomatedDataBackup = null;
            //    item.EntityPM.AutomationXML = "";
            //    if (item.EntityPM) {
            //        automationsPMList.push(item.EntityPM);
            //    }
            //});

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this._automationExtendedPMService.putAuomationList(automationArgsLists).subscribe((res:any) => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindow();
            });
        }
        else this.CurrentSession.CloseCurrentWindow();

        
    }
    ArrowUpAutomationButtonClicked(item: AutomationItemViewModel, type:string) {

      
        if (item != null) {

            if (type == "OnCreate") {

               var i = this.OnCreateAutomationList.indexOf(item);
               var upColumn = this.OnCreateAutomationList[i - 1];
                if (i > 0) {
                    this.OnCreateAutomationList = this.OnCreateAutomationList.filter(d => d.Id != upColumn.Id);
                    var tempOrder = item.Order;
                    item.EntityPM.Order = item.Order = upColumn.Order;
                    upColumn.Order = upColumn.EntityPM.Order = tempOrder;
                    this.OnCreateAutomationList.splice(i, 0, upColumn);
                }
            }
          
            else if (type == "OnUpdate") {
            
                    var i = this.OnUpdateAutomationList.indexOf(item);
                    var upColumn = this.OnUpdateAutomationList[i - 1];
                    if (i > 0) {
                        this.OnUpdateAutomationList = this.OnUpdateAutomationList.filter(d => d.Id != upColumn.Id);
                        var tempOrder = item.Order;
                        item.EntityPM.Order = item.Order = upColumn.Order;
                        upColumn.Order = upColumn.EntityPM.Order = tempOrder;
                        this.OnUpdateAutomationList.splice(i, 0, upColumn);
                    }
            }
   
        }
  
    }

    ArrowDownAutomationButtonClicked(item: AutomationItemViewModel, type: string) {

        if (item != null) {
            if (type == "OnCreate") {
                var i = this.OnCreateAutomationList.indexOf(item);
                var downColumn = this.OnCreateAutomationList[i + 1];
                if (i < this.OnCreateAutomationList.length - 1) {
                    this.OnCreateAutomationList = this.OnCreateAutomationList.filter(d => d.Id != downColumn.Id);
                    var tempOrder = item.Order;
                    item.EntityPM.Order = item.Order = downColumn.Order;
                    downColumn.Order = downColumn.EntityPM.Order = tempOrder;
                    this.OnCreateAutomationList.splice(i, 0, downColumn);
                }
            }

            else if (type == "OnUpdate") {
                var i = this.OnUpdateAutomationList.indexOf(item);
                var downColumn = this.OnUpdateAutomationList[i + 1];
                if (i < this.OnUpdateAutomationList.length - 1) {
                    this.OnUpdateAutomationList = this.OnUpdateAutomationList.filter(d => d.Id != downColumn.Id);
                    var tempOrder = item.Order;
                    item.EntityPM.Order = item.Order = downColumn.Order;
                    downColumn.Order = downColumn.EntityPM.Order = tempOrder;
                    this.OnUpdateAutomationList.splice(i, 0, downColumn);
                }
            }

        }
    }

      
    

}
