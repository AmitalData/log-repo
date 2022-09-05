import {Component, AfterContentInit, Output, EventEmitter, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../DataContracts/EntityArgs';
import {BaseComponent} from '../Components/LogitudeComponents/BaseComponent';
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';
import {SessionInfo} from '../Utilities/SessionInfo';
import { AppTool } from '../Tools';
import { SessionLocator } from '../Utilities/SessionLocator';
import { TextCodeTranslationPipe } from '../../Controls/Pipes/TextCodeTranslationPipe';
import { ScreenSectionListService } from 'Infrastructure/Services/StandardLists/ScreenSectionListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ScreenPM } from 'Infrastructure/EntityPMs/ScreenPM';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ScreenSectionPM } from 'Infrastructure/EntityPMs/ScreenSectionPM';
import { TextCodeTranslator } from '../Utilities/TextCodeTranslator';

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
    public LabelWidth: number = 190;
    public IsNewEntityCall: boolean;
    public ShowNoFieldsText: boolean = false;
    ShowTitle: boolean = false;
    public IsCustomerCare: boolean = false;
    public IsCustomerCareOrDistributor: boolean = false;
    public HideColumns: boolean = false;
    public HideLastColumn: boolean = false;
    public ScreenSections: ScreenSection[];
    private generalTextCode: string = "General.O.General";
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    screenSectionService = new ScreenSectionListService();
    private objectTableTab: any;
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
        this.objectTableTab = window.ObjectTableTabs.filter((x: any) => x.Code === this.entityArgs.SelectedTabCode)[0];

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


    BuildScreen(fireEmit: boolean = false) {
        if (!this.objectTableTab || !this.objectTableTab.ScreenCode) {
            this.BuildClassicScreen(fireEmit);
            return;
        }

        this.BuildLighteningScreen(fireEmit);
    }


    private BuildClassicScreen(fireEmit: boolean = false) {
        if (this.EntityPM != null) {
            if (this.isViewEnited == true) {
                this.ScreenSections = [];
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
                this.ScreenSections.push(new ScreenSection(0, this.ShowTitle ? TextCodeTranslationPipe.apply(this.generalTextCode) : "", this.ScreenColumns))
                if (fireEmit) {
                    this.LoadCompleted.emit(true);
                }
            }
        }
    }


    private BuildLighteningScreen(fireEmit: boolean = false) {

        if (this.EntityPM == null || this.isViewEnited == false)
            return;

        let screen:ScreenPM = window.Screens.filter((x: any) => x.Code === this.objectTableTab.ScreenCode)[0];
        if (screen == null)
            return;

        this.BuildScreenSections(screen);

        if (fireEmit)
        this.LoadCompleted.emit(true);
    }



    private isScreenEnabled: boolean = true;
    private BuildScreenSections(screen: ScreenPM)
    {
        this.GetScreenSections(screen)
            .subscribe(response =>
            {
                this.ScreenSections = [];

                const sections: any[] = response.Result;

                const screenFields = GetScreenFields(screen);
                if (screenFields.length == 0)
                    return this.ShowNoFieldsText = true;

                sections.forEach(section => this.BuildScreenSection(screen, section, screenFields, this.GetObjectFields()) );
            });
    }



    private BuildScreenSection(screen: ScreenPM, section: ScreenSectionPM, screenFields: any, objectFields: any)
    {
        const columns: ScreenColumn[] = [];

        for (let i = 0; i < screen.NumberOfColumns; i++){

            const column = this.BuildScreenColumn(i, section, screenFields, objectFields);
            if(column.ObjectFields.length == 0)
                continue;
            columns.push(column);
        }

        this.ScreenSections.push(new ScreenSection(section.Number, section.Name, columns));
    }




    GetTableName(): string {
        const tab: any = window.ObjectTableTabs.find(d => d.Code == this.entityArgs.SelectedTabCode);
        if (tab?.TabNameTextCodeDefaultText)
            return  tab.TabNameTextCodeDefaultText;

        return TextCodeTranslator.Translate(this.generalTextCode);
    }


    BuildScreenColumn(columnIndex,section: ScreenSectionPM,screenFields, objectFields){
        const column = new ScreenColumn(columnIndex);
        let row = 0;

        for (let r = 0; r < section.NumberOfRows; r++) {
            const screenField = screenFields.filter((f: any) => f.Column == columnIndex && f.Row == r && f.SectionNumber == section.Number)[0];
            if(!screenField)
                continue;

                const objectField = objectFields.filter((f: any) => f.FieldCode == screenField.ObjectFieldCode)[0];
                if (!objectField)
                    continue;

                this.SetValidityForCommunicationLog(objectField);
                row = this.AddEmptyRows(screenField, row, column);
               column.ObjectFields.push(objectField);

            row += 1;
        }
        return column;
    }

    private AddEmptyRows(screenField: any, row: number, column: ScreenColumn) {
  
        if (screenField.Row == row) return row;
        var count = 0;
        while (count < (screenField.Row - row)) {
            column.ObjectFields.push(new ObjectFieldPM());
            count += 1;
        }
        return screenField.Row;
    }

    private SetValidityForCommunicationLog(objectField)
    {
        if (this.ObjectTableName != "CommunicationLog")
            return;

        this.EntityPM.UIProperties.SetEnabled(objectField.FieldName, this.ObjectTableName, false);
        this.EntityPM.UIProperties.SetRequired(objectField.FieldName, this.ObjectTableName, false);
    }

    private GetObjectFields()
    {
        var objectFields = window.ObjectFields.filter((x: any) => x.ObjectTableId === this.ObjectTableId);
        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId))
            objectFields = objectFields.concat(window.ObjectFields.filter((x: any) => x.ObjectTableId === this.ChildObjectTableId));
        return objectFields;
    }

    private GetScreenSections(screen: ScreenPM)
    {
        const filters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.PageSize = 1000;
        filters.addAdditionalFilter("ScreenCode", screen.Code, null, null, "Equals", false, false, false, "string");
        return this.screenSectionService.getByFilters(filters)
    }

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

export class ScreenSection {
    public SectionNumber: number;
    public Title: string;
    public ScreenColumns: ScreenColumn[];
    actualColumnsCount = 0;
    constructor(sectionNumber: number, title: string,screenColumns:ScreenColumn[]) {
        this.SectionNumber = sectionNumber;
        this.Title = title;
        this.ScreenColumns = screenColumns;
        this.actualColumnsCount = screenColumns.filter(c=>c.ObjectFields.length > 0).length;
    }

}
function GetScreenFields(screen: ScreenPM)
{
    var screenFields = window.ScreenFields.filter((x: any) => x.ScreenId === screen.Id && x.Tenant === SessionInfo.LoggedUserTenant);

    if (screenFields.length == 0) {
        screenFields = window.ScreenFields.filter((x: any) => x.ScreenId === screen.Id);
    }
    return screenFields;
}

