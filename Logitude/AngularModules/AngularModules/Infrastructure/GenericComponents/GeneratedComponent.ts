import {Component, AfterContentInit, Output, EventEmitter, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../DataContracts/EntityArgs';
import {BaseComponent} from '../Components/LogitudeComponents/BaseComponent';
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';
import {SessionInfo} from '../Utilities/SessionInfo';
import { AppTool } from '../Tools';
import { SessionLocator } from '../Utilities/SessionLocator';

declare var window: any;

@Component({
    
    templateUrl: './GeneratedComponent.html',
})

export class GeneratedComponent extends BaseComponent implements AfterContentInit, OnDestroy {
    public EntityPM: any;
    //public EntityArgs: EntityArgs;
    public ObjectTableId: string;
    public ObjectTableName: string;
    public ChildObjectTableId: string;
    public ChildObjectTableName: string;
    public ScreenCode: string;
    public ScreenColumns: ScreenColumn[];
    public LabelWidth: number = 160;
    public IsNewEntityCall: boolean;
    public ShowNoFieldsText: boolean = false;
    ShowTitle: boolean = false;
    public IsCustomerCare: boolean = false;
    public IsCustomerCareOrDistributor: boolean = false;
    public HideColumns: boolean = false;
    public HideLastColumn: boolean = false;

    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    constructor(private entityArgs: EntityArgs) {
        super();
        this.IsCustomerCare = SessionLocator.LoggedUserPM.IsCustomerCare;
        this.IsCustomerCareOrDistributor = SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor;
    }

    public Run(entityPM: any, objectTableName: string, screenCode: string, isNewEntityCall: boolean = false, showTitle: boolean = false, childObjectTableName: string = null) {
        this.EntityPM = entityPM;
        this.ScreenCode = screenCode ? screenCode.replace("Customs.", "") : screenCode;
        this.ObjectTableName = objectTableName;
        this.ObjectTableId = window.ObjectTables.filter((x: any) => x.Name === this.ObjectTableName)[0].Id;
        this.IsNewEntityCall = isNewEntityCall;
        this.ShowTitle = showTitle;
        this.ChildObjectTableName = childObjectTableName;
        this.ChildObjectTableId = window.ObjectTables.filter((x: any) => x.Name === this.ChildObjectTableName)[0]?.Id;
        this.BuildScreen();
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs) {
            if (this.entityArgs.EditComponent) {

                this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                        this.BuildScreen();                        
                    }
                });

                this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                        this.BuildScreen();
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    private isViewEnited: boolean = false;
    ngAfterContentInit() {
        this.isViewEnited = true;
        this.BuildScreen(true);       
    }

    private BuildScreen(fireEmit: boolean = false) {
        if (this.EntityPM != null) {
            if (this.isViewEnited == true) {

                var myScreenColumns: ScreenColumn[] = [];
                var myScreen = window.Screens.filter((x: any) => x.ObjectTableId === (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId) && x.Code.toLowerCase() == this.ScreenCode.toLowerCase())[0];
                
                if (myScreen != null) {

                    var myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id && x.Tenant === SessionInfo.LoggedUserTenant);

                    if (myScreenFields.length == 0) {
                        myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id);
                    }

                    if (myScreenFields.length == 0) {
                        this.ShowNoFieldsText = true;
                    }

                    else {
                        var myObjectFields = window.ObjectFields.filter((x: any) => x.ObjectTableId === this.ObjectTableId);
                        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId))
                            myObjectFields = myObjectFields.concat(window.ObjectFields.filter((x: any) => x.ObjectTableId === this.ChildObjectTableId));

                        for (var c = 0; c < myScreen.NumberOfColumns; c++) {
                            var myScreenColumn = new ScreenColumn(c);

                            for (var r = 0; r < myScreen.NumberOfRows; r++) {
                                var myScreenField = myScreenFields.filter((f: any) => f.Column == c && f.Row == r)[0];
                                if (myScreenField != null) {
                                    var myObjectField = myObjectFields.filter((f: any) => f.FieldCode == myScreenField.ObjectFieldCode)[0];
                                    if (myObjectField != null) {

                                        if (this.ObjectTableName == "CommunicationLog") {
                                            this.EntityPM.UIProperties.SetEnabled(myObjectField.FieldName, this.ObjectTableName, false);
                                            this.EntityPM.UIProperties.SetRequired(myObjectField.FieldName, this.ObjectTableName, false);
                                        }

                                        myScreenColumn.ObjectFields.push(myObjectField);
                                    }
                                }
                            }

                            myScreenColumns.push(myScreenColumn);
                        }
                    }
                }

                this.ScreenColumns = myScreenColumns.filter(c => c.ObjectFields?.length > 0);

                if (fireEmit) {
                    this.LoadCompleted.emit(true);
                }
            }
        }
    }

    private isScreenEnabled: boolean = true;
    SetEnabled(isEnabled: boolean) {
        this.isScreenEnabled = isEnabled;

        if (this.EntityPM) {
            if (this.ScreenColumns) {
                this.ScreenColumns.forEach(item => {
                    item.ObjectFields.forEach(field => {
                        this.EntityPM.UIProperties.SetEnabled(field.FieldName, this.ObjectTableName, isEnabled);
                    });
                });
            }
        }
    }
}

export class ScreenColumn {
    public Index: number;
    public ObjectFields: ObjectFieldPM[];
    constructor(index: number) {
        this.Index = index;
        this.ObjectFields = [];
    }
}
