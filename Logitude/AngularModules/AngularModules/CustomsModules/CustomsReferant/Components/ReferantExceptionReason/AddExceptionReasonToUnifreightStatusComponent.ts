declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ExceptionReasonPM } from '../../../../Customs/EntityPMs/ExceptionReasonPM';
import { ExceptionReasonPMService } from '../../../../Customs/Services/StandardPMs/ExceptionReasonPMService';
import { ExceptionReasonExtendedListService } from '../../../../Customs/Services/ExtendedLists/ExceptionReasonExtendedListService';

@Component({
    templateUrl: './AddExceptionReasonToUnifreightStatusComponent.html',
})

export class AddExceptionReasonToUnifreightStatusComponent extends BaseComponent implements OnInit {
  public Code: any;
  public FooterMethods: any;


    public DataContext: AddExceptionReasonToUnifreightStatusComponent = this;
    public ObjectTableName: string = "Customs.ExceptionReason";
    ValidationErrorsList: any[] = [];

    _ExceptionReasonPMService: ExceptionReasonPMService = new ExceptionReasonPMService();
    _ExceptionReasonExtendedListService: ExceptionReasonExtendedListService = new ExceptionReasonExtendedListService();
    _entityResourceService: EntityResourceService = new EntityResourceService();

    public ExceptionReasonList: ObservableCollection = new ObservableCollection([]);
    public DeleteExceptionReasonList: ObservableCollection = new ObservableCollection([]);

    constructor() {
        super();
    }

    Loaded: boolean = false;
    ngOnInit() {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this.Loaded = true;
            SessionLocator.SelectedSession.StopBusyIndicator();
        });

    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {

            if (args.FromUnifreight && !AppTool.IsNullOrEmpty(args.UnifreightStatusCode)) {
                this.UnifreightStatusCode = args.UnifreightStatusCode;
                SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                this._ExceptionReasonExtendedListService.GetExceptionReasonByUnifreightStatus(this.UnifreightStatusCode).subscribe((response:any) => {
                    SessionLocator.SelectedSession.StopBusyIndicator();
                    var ExceptionReasonResult: ExceptionReasonPM[] = response.Result;
                    this.BuildExceptionReason(ExceptionReasonResult);
                });
            }
        }
    }

    BuildExceptionReason(ExceptionReasonResult: ExceptionReasonPM[]) {
        this.ExceptionReasonList = new ObservableCollection([]);

        if (ExceptionReasonResult != null && ExceptionReasonResult.length > 0) {
            for (let item of ExceptionReasonResult) {
                this.ExceptionReasonList.Insert(new ExceptionReasonLineComponent(item, false, this));
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

    AddNewExceptionReasonCommand() {
        var newClaimsRelatedEntityPM: ExceptionReasonPM = new ExceptionReasonPM();
        newClaimsRelatedEntityPM.Tenant = SessionLocator.Tenant;
        newClaimsRelatedEntityPM.UnifreightStatusCode = this.UnifreightStatusCode;

        let newClaimsRelatedEntityLineComponent = new ExceptionReasonLineComponent(newClaimsRelatedEntityPM, true, this);
        this.ExceptionReasonList.Insert(newClaimsRelatedEntityLineComponent);
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }


    OkButtonClicked() {
        if (this.ExceptionReasonList != null) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.ExceptionReasonList.Collection.forEach((item: ExceptionReasonLineComponent) => {
                if (item.isNew) {
                    item.entityPM.UnifreightStatusCode = this.UnifreightStatusCode;
                    this._ExceptionReasonPMService.update(item.entityPM).subscribe(myResult => {
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

        if (this.DeleteExceptionReasonList != null) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.DeleteExceptionReasonList.Collection.forEach((deleteItem: ExceptionReasonLineComponent) => {
                this._ExceptionReasonExtendedListService.DeleteExceptionReasonByUnifreightStatus(deleteItem.Code).subscribe((response:any) => {
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

    DeleteExceptionCommand(item: ExceptionReasonLineComponent) {
        if (!item.isNew) {
            this.DeleteExceptionReasonList.Insert(item);
        }
        this.ExceptionReasonList.Remove(item);
    }
}

export class ExceptionReasonLineComponent extends BaseComponent {
    public ObjectTableName: string = "Customs.ExceptionReason";
    public DataContext = this;

    constructor(public entityPM: ExceptionReasonPM, public isNew: boolean, private parent: AddExceptionReasonToUnifreightStatusComponent) {
        super();
        if (!isNew) {
            this.UIProperties.SetEnabled("ExceptionReasonsCode", this.ObjectTableName, false);
        }
    }

    public get Code() { return this.entityPM.Code; }
    public set Code(newValue: string) {
        if (newValue) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.parent._ExceptionReasonPMService.get(newValue).subscribe(response => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                if (!response.HasError && response.Result != null) {
                    if (!AppTool.IsNullOrEmpty(response.Result.UnifreightStatusCode) && response.Result.UnifreightStatusCode != this.parent.UnifreightStatusCode) {
                        var confirm = new ConfirmWindow();
                        confirm.Width = 350;
                        confirm.Height = 200;
                        confirm.Title = "קישור סטטוס לחריג רפרנט";
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
    public get ReasonLocalName() { return this.entityPM.LocalName; }
    public set ReasonLocalName(newValue: string) { this.entityPM.LocalName = newValue; }
}
