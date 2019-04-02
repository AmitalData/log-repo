import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {SLAHeaderPM} from '../../../../CRM/EntityPMs/SLAHeaderPM';
import {SLAHeaderListService} from '../../../../CRM/Services/StandardLists/SLAHeaderListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {CRMDomainService} from '../../../../CRM/Services/CRMDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './SLAMainWindowComponent.html',
})

export class SLAMainWindowComponent extends BaseComponent {
    public DataContext = this;
    public SLAList: SLAItem[] = []; 
    private SLAHeaderListService: SLAHeaderListService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.SLAHeaderListService = new SLAHeaderListService();
        this.LoadSLAList(false);
    }
    LoadSLAList(arg: boolean) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.SLAList = [];
        var service = new CRMDomainService();
        service.GetActiveSLAbyTenant().subscribe((myResponse: ServiceResponse) => {
            var pmResponse: ServiceResponse = myResponse;
            if (!pmResponse.HasError) {
                var myResult: SLAHeaderPM[] = pmResponse.Result;
                if (arg == false) {
                    myResult = pmResponse.Result.filter(d => d.Inactive == false);
                }
                var order = 1; 
                myResult.forEach((item) => {
                    var slsItem = new SLAItem(item, order);
                    order = order + 1;
                    this.SLAList.push(slsItem);
                });
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    private includeInactive = false; 
    get IncludeInactive() {
        return this.includeInactive;
    }
    set IncludeInactive(value: boolean) {
        if (this.includeInactive != value) {
            this.includeInactive = value;
        }
    }

    AddSLA() {
        var item: SLAItem = new SLAItem(null, null);
        item.EntityPM = new SLAHeaderPM();
        this.RunSLAWindow(item, "New SLA", true);
    }
    EditSLA(item: SLAItem) {
        this.RunSLAWindow(item, "Edit SLA", false);
    }
    private RunSLAWindow(itemComponent: SLAItem, windowTitle: string, isNew: boolean) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 815;
        var args : any = {};
        args.IsNewEntity = isNew;
        args.EntityPM = itemComponent.EntityPM;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Show('./CRMModules/CRMOthers/Components/SLA/NewSLAComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            if ($event == "ok") {
                this.LoadSLAList(this.IncludeInactive);
            }
        });
    }

    IncludeInactiveChecked(arg: boolean) {
        this.LoadSLAList(arg);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
export class SLAItem {
    public EntityPM: SLAHeaderPM;
    public Order: number; 
    public DataContext = this;

    constructor(entityPM: SLAHeaderPM, order: number) {
        this.EntityPM = entityPM;
        this.Order = order;
    }

    get CreateDate() {
        return this.EntityPM.CreateDate;
    }
    set CreateDate(newValue: Date) {
        if (this.EntityPM.CreateDate != newValue) {
            this.EntityPM.CreateDate = newValue;
        }
    }

    get UpdateDate() {
        return this.EntityPM.UpdateDate;
    }
    set UpdateDate(newValue: Date) {
        if (this.EntityPM.UpdateDate != newValue) {
            this.EntityPM.UpdateDate = newValue;
        }
    }

    get UpdatedByUserName() {
        return this.EntityPM.UpdatedByUserName;
    }
    set UpdatedByUserName(newValue: string) {
        if (this.EntityPM.UpdatedByUserName != newValue) {
            this.EntityPM.UpdatedByUserName = newValue;
        }
    }

    get CreatedByUserName() {
        return this.EntityPM.CreatedByUserName;
    }
    set CreatedByUserName(newValue: string) {
        if (this.EntityPM.CreatedByUserName != newValue) {
            this.EntityPM.CreatedByUserName = newValue;
        }
    }

    get Description() {
        return this.EntityPM.Description;
    }
    set Description(newValue: string) {
        if (this.EntityPM.Description != newValue) {
            this.EntityPM.Description = newValue;
        }
    }

    get Name() {
        return this.EntityPM.Name;
    }
    set Name(newValue: string) {
        if (this.EntityPM.Name != newValue) {
            this.EntityPM.Name = newValue;
        }
    }

    get Inactive() {
        return this.EntityPM.Inactive;
    }
    set Inactive(newValue: boolean) {
        if (this.EntityPM.Inactive != newValue) {
            this.EntityPM.Inactive = newValue;
        }
    }
}
