import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ClaimsRelatedEntityPM } from '../../../../../Customs/EntityPMs/ClaimsRelatedEntityPM';
import { ClaimPM } from '../../../../../Customs/EntityPMs/ClaimPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

@Component({
    
    templateUrl: './ClaimRelatedEntityCustomAnswerTabComponent.html',
})

export class ClaimRelatedEntityCustomAnswerTabComponent extends BaseComponent {
    public DataContext: ClaimRelatedEntityCustomAnswerTabComponent = this;
    public EntityPM: ClaimsRelatedEntityPM = new ClaimsRelatedEntityPM(new ClaimPM);
    public ObjectTableName: string = "Customs.ClaimsRelatedEntity";

    public ClaimRelatedEntityCustomAnswerlist: ObservableCollection;

    public CurrentEditComponentId: string;
    private isControlEnabled: boolean = true;

    private _ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.ClaimRelatedEntityCustomAnswerlist = new ObservableCollection([]);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildCustomAnswerList();
                    }
                })
            );

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.CurrentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        //if (tabCode == "CLMG") {
                        //    this.RefreshEntity();
                        //    this.BuildReasonslist();
                        //}
                    }
                })
            );
        }
    }

    InitTab(entityPM: ClaimsRelatedEntityPM, claimPM: any, isEnable: boolean) {

        this.EntityPM = entityPM;
        this.isControlEnabled = isEnable;

        this.EntityResourceService.getEntityResourceByTableName("Customs.Claim").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.ClaimsRelatedEntity").subscribe((response:any) => {
                this.BuildCustomAnswerList();
                        this.Listen();
            });
        });
    }

    RefreshEntity() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }

    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }

    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

    public get IsControlEnabled() { return this.isControlEnabled; }
    public set IsControlEnabled(newValue: boolean) { this.isControlEnabled = newValue; }

    public get ValidationErrorsList() { return this._ValidationErrorsList; }
    public set ValidationErrorsList(newValue: string[]) { this._ValidationErrorsList = newValue; }

    _MyResponseObjectToShow: any = null;
    BuildCustomAnswerList() {
        this.ClaimRelatedEntityCustomAnswerlist = new ObservableCollection([]);

        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomsExceptions)) {
            this._MyResponseObjectToShow = JSON.parse(this.EntityPM.CustomsExceptions);

            if (this._MyResponseObjectToShow != null
                && this._MyResponseObjectToShow.ClaimCustomsExceptions != null
                && this._MyResponseObjectToShow.ClaimCustomsExceptions.CustomsExceptions != null)
                    this.ClaimRelatedEntityCustomAnswerlist.Insert(this._MyResponseObjectToShow.ClaimCustomsExceptions.CustomsExceptions.CustomsException);
            }
    }

    ShowMore(itemContent, itemSubject) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 1000;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = itemSubject;
        logitudeWindow.WindowArgs = itemContent;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureCommunications/Components/Communications/LogFieldComponent');
    }

}

    //#endregion


export class CustomsException extends BaseComponent {
    public DataContext: CustomsException = this;

    constructor(exceptionType: string, exceptionDescription: string) {
        super();

        this.ExceptionType = exceptionType;
        this.ExceptionDescription = exceptionDescription;
    }

    private _ExceptionType: string;
    public get ExceptionType() { return this._ExceptionType; }
    public set ExceptionType(newValue: string) { this._ExceptionType; }

    private _ExceptionDescription: string;
    public get ExceptionDescription() { return this._ExceptionDescription; }
    public set ExceptionDescription(newValue: string) { this._ExceptionDescription; }
}

export class CustomsExceptions extends BaseComponent {
    public DataContext: CustomsExceptions = this;

    constructor() {
        super();
    }

    private _CustomsNotes: string;
    public get CustomsNotes() { return this._CustomsNotes; }
    public set CustomsNotes(newValue: string) { this._CustomsNotes = newValue; }

    private _CustomsExceptions: CustomsException;
    public get CustomsExceptions() { return this._CustomsExceptions; }
    public set CustomsExceptions(newValue: CustomsException) { this._CustomsExceptions = newValue; }
}
