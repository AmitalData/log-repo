import { SessionLocator } from './../../../../Infrastructure/Utilities/SessionLocator';
declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../../Infrastructure/Tools';

import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { CourierPendingReasonPM } from '../../../../Customs/EntityPMs/CourierPendingReasonPM';
import { CourierPendingReasonPMService } from '../../../../Customs/Services/StandardPMs/CourierPendingReasonPMService';
import { CourierPendingReasonExtendedListService } from '../../../../Customs/Services/ExtendedLists/CourierPendingReasonExtendedListService';

@Component({
    
    templateUrl: './AddCourierPendingToUnifreightStatusComponent.html',
})

export class AddCourierPendingToUnifreightStatusComponent extends BaseComponent implements OnInit {
  public PendingCode: any;
  public FooterMethods: any;


    public DataContext: AddCourierPendingToUnifreightStatusComponent = this;
    public ObjectTableName: string = "Customs.CourierPendingReason";
    ValidationErrorsList: any[] = [];

    _CourierPendingReasonPMService: CourierPendingReasonPMService = new CourierPendingReasonPMService();
    _CourierPendingReasonExtendedListService: CourierPendingReasonExtendedListService = new CourierPendingReasonExtendedListService();
    _entityResourceService: EntityResourceService = new EntityResourceService();

    public CourierPendingReasonList: ObservableCollection = new ObservableCollection([]);
    public DeleteCourierPendingReasonList: ObservableCollection = new ObservableCollection([]);
    private currentSession=SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    Loaded: boolean = false;
    ngOnInit() {
        this.currentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this.Loaded = true;
            SessionLocator.SelectedSession.StopBusyIndicator();
        });

    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {

            if (args.FromUnifreight && !AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.UnifreightStatusCode = args.UnifreightStatusCode;
                this.currentSession.StartBusyIndicatorLoading();
                this._CourierPendingReasonExtendedListService.GetCourierPendingReasonByUnifreightStatus(this.UnifreightStatusCode).subscribe((response:any) => {
                    this.currentSession.StopBusyIndicator();
                    var courierPendingReasonResult: CourierPendingReasonPM[] = response.Result;
                    this.BuildCourierPendingReasonList(courierPendingReasonResult);
                });
            }
        }
    }

    BuildCourierPendingReasonList(courierPendingReasonResult: CourierPendingReasonPM[] ) {
        this.CourierPendingReasonList = new ObservableCollection([]);

        if (courierPendingReasonResult != null && courierPendingReasonResult.length > 0) {
            for (let item of courierPendingReasonResult) {
                this.CourierPendingReasonList.Insert(new CourierPendingReasonLineComponent(item, false, this));
            }
        }
    }

    ///#region Properties
    private _UnifreightStatusCode: string;
    public get UnifreightStatusCode() { return this._UnifreightStatusCode; }
    public set UnifreightStatusCode(newValue: string) {
        this._UnifreightStatusCode = newValue;
    }


    //#endregion

    AddNewPendingCommand() {
        var newClaimsRelatedEntityPM: CourierPendingReasonPM = new CourierPendingReasonPM();
        newClaimsRelatedEntityPM.Tenant = SessionLocator.Tenant;
        newClaimsRelatedEntityPM.UnifreightStatusCode = this.UnifreightStatusCode;

        let newClaimsRelatedEntityLineComponent = new CourierPendingReasonLineComponent(newClaimsRelatedEntityPM, true, this);
        this.CourierPendingReasonList.Insert(newClaimsRelatedEntityLineComponent);
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }


    OkButtonClicked() {
        if (this.CourierPendingReasonList != null) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.CourierPendingReasonList.Collection.forEach((item: CourierPendingReasonLineComponent) => {
                if (item.isNew) {
                    item.entityPM.UnifreightStatusCode = this.UnifreightStatusCode;
                    this._CourierPendingReasonPMService.update(item.entityPM).subscribe((myResult:any) => {
                        if (myResult.HasError) {
                            this.ValidationErrorsList = [];
                            this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                            return;
                        }
                    });
                }
            });
            SessionLocator.SelectedSession.StopBusyIndicator();
        }

        if (this.DeleteCourierPendingReasonList != null) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.DeleteCourierPendingReasonList.Collection.forEach((deleteItem: CourierPendingReasonLineComponent) => {
                this._CourierPendingReasonExtendedListService.DeleteCourierPendingReasonUnifreightStatus(deleteItem.PendingCode).subscribe((response:any) => {
                    if (response.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(response.ErrorsArray[0]);
                        return;
                    }
                });
            });
            SessionLocator.SelectedSession.StopBusyIndicator();
        }
            
        this.CancelButtonClicked();
    }

    DeletePendingCommand(item: CourierPendingReasonLineComponent) {
        if (!item.isNew) {
            this.DeleteCourierPendingReasonList.Insert(item);
        }
        this.CourierPendingReasonList.Remove(item);
    }
}

export class CourierPendingReasonLineComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.CourierPendingReason";
    public DataContext = this;
    private currentSession=SessionLocator.SelectedSession;
    constructor(public entityPM: CourierPendingReasonPM, public isNew: boolean, private parent: AddCourierPendingToUnifreightStatusComponent) {
        super();
        if (!isNew) {
            this.UIProperties.SetEnabled("PendingCode", this.ObjectTableName, false);
        }
    }

    //private _PendingCode: string;
    public get PendingCode() { return this.entityPM.Id; }
    public set PendingCode(newValue: string) {
        if (newValue) {
            this.currentSession.StartBusyIndicatorLoading();
            this.parent._CourierPendingReasonExtendedListService.GetSingleCourierPendingReasonPMByCode(newValue).subscribe((response: any) => {
                this.currentSession.StopBusyIndicator();
                if (!response.HasError && response.Result != null) {
                    if (!AppTool.IsNullOrEmpty(response.Result.UnifreightStatusCode) && response.Result.UnifreightStatusCode != this.parent.UnifreightStatusCode) {
                            var confirm = new ConfirmWindow();
                            confirm.Width = 350;
                            confirm.Height = 200;
                            confirm.Title = "קישור Pending לסטטוס";
                            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
                            confirm.ShowNoButton = true;
                            confirm.Show("לקוד זה כבר קושר סטטוס " + response.Result.UnifreightStatusCode + " האם להחליף לסטטוס " + this.parent.UnifreightStatusCode + "?");
                            confirm.WindowClosed.subscribe((event: any) => {
                                if (confirm.Yes) {
                                    this.entityPM = response.Result;
                                }
                                confirm.Close();
                            });
                    }
                    else {
                        this.entityPM = response.Result;
                    }
                }
            });
        }
    }

    public get PendingLocalName() { return this.entityPM.LocalName; }
    public set PendingLocalName(newValue: string) { this.entityPM.LocalName = newValue; }

}
