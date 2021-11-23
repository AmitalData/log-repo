import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../Utilities/LocationDirective';
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';
import { MultiEntityUpdateLogPMService } from '../../Services/StandardPMs/MultiEntityUpdateLogPMService';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { MultiEntityUpdateLogPM } from '../../EntityPMs/MultiEntityUpdateLogPM';
import { MultiUpdateComponent } from './MultiUpdateComponent';

@Component({
    selector: 'MultiEntityUpdateBaseComponent',
    templateUrl: 'MultiEntityUpdateBaseComponent.html',
})

export class MultiEntityUpdateBaseComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private PageChild_MTUP: any = null;
    private PageChild_MTHE: any = null;
    MultiEntityUpdatedLogPM: MultiEntityUpdateLogPM;
    private MultiUpdateComponent:any;
    WindowArgs: any;

    constructor() {
    }

    ngOnInit() {
    }

    SetWindowArgs(windowArgs) {
        this.WindowArgs = windowArgs;
        this.RunComponent();
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;
        if (this.timerToken)  clearTimeout(this.timerToken);
        if (this.Retries < 20)  this.timerToken = setTimeout(() => this.RunComponent(), 1);
    }

    RunComponent() {
        if (!this.AllLocations) this.RunComponentTimer();
        if (this.AllLocations.toArray().length == 0) this.RunComponentTimer();
        else this.SetSelectedItem("MTUP");
    }

    SetSelectedItem(tabCode: string) {
        this.SelectedTabCode = tabCode;
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    SelectionChanged() {
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
        if (myLocation == null) return;
        switch (this.SelectedTabCode) {
            //Multi Update
            case "MTUP": {
                if (this.PageChild_MTUP == null) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/MultiUpdateComponent/MultiUpdateComponent', myLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_MTUP = cmpRef.instance;
                            this.PageChild_MTUP.SetWindowArgs({ args: this.WindowArgs, parentComponent: this });
                        });
                }
                break;
            }
            //Handle Errors
            case "MTHE": {
                if (this.PageChild_MTHE == null) {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/MultiUpdateComponent/MultiEntityUpdateErrorHandlerComponent', myLocation.viewContainerRef)
                        .then(cmpRef => {
                            this.PageChild_MTHE = cmpRef.instance;
                            this.PageChild_MTHE.SetWindowArgs({ multiEntityUpdateLogPM: this.MultiEntityUpdatedLogPM });
                        });
                }
                break;
            }
        }
    }

    NextButtonClicked() {
        this.SelectedTabCode = "MTHE";
    }

    UpdateButtonClicked(multiEntityUpdateLogPM, multiEntityUpdateLogPMService,multiUpdateComponent) {
        this.MultiUpdateComponent = multiUpdateComponent;
        this.StartCheckMultiEntityUpdateViaWorkerRoleTimer(multiEntityUpdateLogPM, multiEntityUpdateLogPMService);
    }

    InitializeStartCheckMultiEntityUpdateViaWorkerRoleTimer() {
        return interval(250).pipe(timeInterval());
    }

    private StartCheckMultiEntityUpdateViaWorkerRoleTimerTimersub: any = null;
    IsStartCheckMultiEntityUpdateViaWorkerRoleTimer: boolean = false;
    StartCheckMultiEntityUpdateViaWorkerRoleTimer(multiEntityUpdateLogPM, multiEntityUpdateLogPMService) {
        if (this.IsStartCheckMultiEntityUpdateViaWorkerRoleTimer)
            this.StartCheckMultiEntityUpdateViaWorkerRoleTimerTimersub.unsubscribe();

        this.IsStartCheckMultiEntityUpdateViaWorkerRoleTimer = true;
        this.StartCheckMultiEntityUpdateViaWorkerRoleTimerTimersub = this.InitializeStartCheckMultiEntityUpdateViaWorkerRoleTimer().subscribe(respose => {
            if (this.PageChild_MTUP.IsMultiEntityUpdatedSuccessfully || !this.IsStartCheckMultiEntityUpdateViaWorkerRoleTimer)
                return this.StopCheckMultiEntityUpdateViaWorkerRoleTimer();

            this.HandleMultiEntityUpdateStatuses(multiEntityUpdateLogPMService, multiEntityUpdateLogPM);
        });
    }

    private StopCheckMultiEntityUpdateViaWorkerRoleTimer() {
        this.StartCheckMultiEntityUpdateViaWorkerRoleTimerTimersub.unsubscribe();
        this.IsStartCheckMultiEntityUpdateViaWorkerRoleTimer = false;
        return;
    }

    private HandleMultiEntityUpdateStatuses(multiEntityUpdateLogPMService: any, multiEntityUpdateLogPM: any) {
        if (!this.IsStartCheckMultiEntityUpdateViaWorkerRoleTimer) return;
        if (multiEntityUpdateLogPMService == null) multiEntityUpdateLogPMService = new MultiEntityUpdateLogPMService();
        
        this.HandleGetMultiEntityUpdateLogPM(multiEntityUpdateLogPMService, multiEntityUpdateLogPM);
    }

    private HandleGetMultiEntityUpdateLogPM(multiEntityUpdateLogPMService: any, multiEntityUpdateLogPM: any) {
        multiEntityUpdateLogPMService.get(multiEntityUpdateLogPM.Id).subscribe((serviceResponse: any) => {
            if (serviceResponse != null && !serviceResponse.HasError)
                this.HandleGetMultiEntityUpdateLogPMSuccessfully(serviceResponse);
            else
                this.HandleGetMultiEntityUpdateLogPMError(serviceResponse);
        });
    }

    private HandleGetMultiEntityUpdateLogPMSuccessfully(serviceResponse: any) {
        let pmResponse = serviceResponse.Result;
        if (!this.IsStartCheckMultiEntityUpdateViaWorkerRoleTimer) return;
        if (!pmResponse) this.ShowErrorMessage("Multi Entity Update Log not found");
        this.CurrentSession.StartBusyIndicator("Updating " + pmResponse.UpdatedEntitiesNumber + " / " + pmResponse.MultiEntityUpdateData.Entities.length);
        if (pmResponse.StatusCode == "F") this.ShowErrorMessage("Update Failed!");
        else if (pmResponse.StatusCode == "D") this.HandleUpdateSuccessfully(pmResponse);
    }

    private HandleUpdateSuccessfully(pmResponse: any) {
        this.CurrentSession.StopBusyIndicator();
        this.PageChild_MTUP.IsMultiEntityUpdatedSuccessfully = true;
        this.MultiEntityUpdatedLogPM = pmResponse;
        this.MultiUpdateComponent.OnUpdateFinish(pmResponse.MultiEntityUpdateData.Entities);
    }

    private HandleGetMultiEntityUpdateLogPMError(serviceResponse: any) {
        let messageError: string;
        if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0)
            messageError = serviceResponse.ErrorsArray[0];

        this.StartCheckMultiEntityUpdateViaWorkerRoleTimerTimersub.unsubscribe();
        this.IsStartCheckMultiEntityUpdateViaWorkerRoleTimer = false;
        this.ShowErrorMessage(messageError);
    }

    public ShowErrorMessage(message: string) {
        this.StopCheckMultiEntityUpdateViaWorkerRoleTimer();
        this.CurrentSession.StopBusyIndicator();
        let messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message ? message : "error");
    }
}
