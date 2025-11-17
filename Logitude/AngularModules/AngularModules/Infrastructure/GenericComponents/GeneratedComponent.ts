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
import { EntityResourceService } from '../Services/EntityResourceService';

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
    public IsFromGrid: boolean;
    public BuildLighteningScreenAsClassicScreen: boolean;
    public IsNewEntityCall: boolean;
    public ShowNoFieldsText: boolean = false;
    ShowTitle: boolean = false;
    public IsCustomerCare: boolean = false;
    public IsCustomerCareOrDistributor: boolean = false;
    public HideColumns: boolean = false;
    public HideLastColumn: boolean = false;
    public ScreenSections: ScreenSection[];
    private generalTextCode: string = "General.O.General";
    private CurrentSession = SessionLocator.SelectedSession;
    private entityResourceService: EntityResourceService = new EntityResourceService();
    @Output() LoadCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    screenSectionService = new ScreenSectionListService();
    private objectTableTab: any;
    public IsDisplaySummarySection: boolean = false;
    public ObjectTable: any;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.IsCustomerCare = SessionLocator.LoggedUserPM.IsCustomerCare;
        this.IsCustomerCareOrDistributor = SessionLocator.LoggedUserPM.IsCustomerCare || SessionLocator.LoggedUserPM.IsDistributor;
   }

    public Run(entityPM: any, objectTableName: string, screenCode: string, isNewEntityCall: boolean = false, showTitle: boolean = false, childObjectTableName: string = null) {
        this.EntityPM = entityPM;
        this.ScreenCode = screenCode ? screenCode.replace("Customs.", "") : screenCode;
        this.ObjectTableName = objectTableName;
        this.ObjectTable = window.ObjectTables.filter(table => table.Name == objectTableName)[0];
        this.ObjectTableId = window.ObjectTables.filter((x: any) => x.Name === this.ObjectTableName)[0].Id;
        this.IsNewEntityCall = isNewEntityCall;
        this.ShowTitle = showTitle;
        this.ChildObjectTableName = childObjectTableName;
        this.ChildObjectTableId = window.ObjectTables.filter((x: any) => x.Name === this.ChildObjectTableName)[0]?.Id;
        this.objectTableTab = window.ObjectTableTabs.filter((x: any) => x.Code === this.entityArgs.SelectedTabCode)[0];
        this.BuildLighteningScreenAsClassicScreen = this.IsFromGrid ? true : this.BuildLighteningScreenAsClassicScreen;

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

    IsFocused(objectField: any) {
        if (!this.IsFromGrid) return false;
        if (this.ScreenSections[0]?.Type == "Summary") return false;
        if (objectField.Id == this.ScreenSections[0]?.ScreenColumns[0]?.ObjectFields[0]?.Id) return true;
        return false;
    }

    BuildScreen(fireEmit: boolean = false) {
        let selectedScreen = this.GetSelectedScreen();
        let gridScreenTypeCode = "Grid";
        if (this.BuildLighteningScreenAsClassicScreen && selectedScreen && selectedScreen.Type != gridScreenTypeCode) {
            this.BuildLighteningScreen(fireEmit, selectedScreen);
            return;
        }

        if (!this.objectTableTab || !this.objectTableTab.ScreenCode) {
            this.BuildClassicScreen(fireEmit);
            return;
        }

        this.BuildLighteningScreen(fireEmit);
    }


    private GetSelectedScreen() {
        if (this.EntityPM != null) {
            return window.Screens.filter((x: any) => x.ObjectTableId === (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId) && x.Code.toLowerCase() == this.ScreenCode.toLowerCase())[0];
        }
        return null;
    }

    private BuildClassicScreen(fireEmit: boolean = false) {
        if (this.EntityPM != null) {
            if (this.isViewEnited == true) {
                this.ScreenSections = [];
                var myScreen = window.Screens.filter((x: any) => x.ObjectTableId === (!AppTool.IsNullOrEmpty(this.ChildObjectTableId) ? this.ChildObjectTableId : this.ObjectTableId) && x.Code.toLowerCase() == this.ScreenCode.toLowerCase())[0];

                if (myScreen != null) {
                    
                    let myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id && x.Tenant === SessionInfo.LoggedUserTenant);
                    if (myScreenFields.length == 0) {
                        myScreenFields = window.ScreenFields.filter((x: any) => x.ScreenId === myScreen.Id);
                    }
                    if (this.entityArgs?.customObjectFields?.length > 0) {
                        myScreenFields = this.entityArgs.customObjectFields;
                    }

                    if (myScreenFields.length == 0) {
                        this.ShowNoFieldsText = true;
                    }

                    else {
                        var myObjectFields = window.ObjectFields.filter((x: any) => x.ObjectTableId === this.ObjectTableId);
                        if (!AppTool.IsNullOrEmpty(this.ChildObjectTableId))
                            myObjectFields = myObjectFields.concat(window.ObjectFields.filter((x: any) => x.ObjectTableId === this.ChildObjectTableId));

                        var myScreenColumns: ScreenColumn[] = this.GetClassicScreenColumns(myScreen, myScreenFields, myObjectFields);
                    }
                }

                this.ScreenColumns = myScreenColumns?.filter(c => c.ObjectFields?.length > 0);
                let screenSection = new ScreenSectionPM();
                screenSection.Number = 0;
                screenSection.Name = this.ShowTitle ? TextCodeTranslationPipe.apply(this.generalTextCode) : "";
                this.ScreenSections.push(new ScreenSection(screenSection, this.ScreenColumns))
                this.SetEnabled(!myScreen?.IsReadOnly)
                if (fireEmit) {
                    this.LoadCompleted.emit(true);
                }
            }
        }
    }

    private GetClassicScreenColumns(myScreen: any, myScreenFields: any, myObjectFields: any) {

        if (this.BuildLighteningScreenAsClassicScreen) {
            return this.GetClassicGridScreenColumns(myScreen, myScreenFields, myObjectFields);
        }

        var myScreenColumns: ScreenColumn[] = [];
        for (var c = 0; c < myScreen.NumberOfColumns; c++) {
            var myScreenColumn = new ScreenColumn(c);

            for (var r = 0; r < myScreen.NumberOfRows; r++) {
                var selectedScreenFields = myScreenFields.filter((f: any) => f.Column == c && f.Row == r);
                this.AddSelectedScreenFields(selectedScreenFields, myObjectFields, myScreenColumn);
            }

            myScreenColumns.push(myScreenColumn);
        }
        return myScreenColumns;
    }

    private GetClassicGridScreenColumns(myScreen: any, myScreenFields: any, myObjectFields: any) {
        var myScreenColumns: ScreenColumn[] = [];
        var myScreenColumn = new ScreenColumn(0);

        for (var c = 0; c < myScreen.NumberOfColumns; c++) {
            var selectedScreenFields = myScreenFields.filter((f: any) => f.Column == c && f.Row == 0);
            this.AddSelectedScreenFields(selectedScreenFields, myObjectFields, myScreenColumn);
        }

        myScreenColumns.push(myScreenColumn);
        return myScreenColumns;
    }

    private AddSelectedScreenFields(selectedScreenFields: any, myObjectFields: any, myScreenColumn: ScreenColumn) {
        if (selectedScreenFields == null) return;

        selectedScreenFields.forEach(screenField => {
            this.AddScreenField(myObjectFields, screenField, myScreenColumn);
        });
    }

    private AddScreenField(myObjectFields: any, screenField: any, myScreenColumn: ScreenColumn) {
        var myObjectField = myObjectFields.filter((f: any) => f.FieldCode == screenField.ObjectFieldCode)[0];
        if (myObjectField == null || (!myObjectField.IsCustom && this.IsFromGrid && !this.IsDisplaySummarySection)) return;

        this.SetValidityForCommunicationLog(myObjectField);

        myScreenColumn.ObjectFields.push(myObjectField);
    }

    private BuildLighteningScreen(fireEmit: boolean = false, lighteningScreen = null) {

        if (this.EntityPM == null || this.isViewEnited == false)
            return;

        let screen: ScreenPM = this.objectTableTab ? window.Screens.filter((x: any) => x.Code === this.objectTableTab.ScreenCode)[0] : lighteningScreen;
        if (screen == null)
            return;

        this.BuildScreenSections(screen, fireEmit);
        
    }



    private isScreenEnabled: boolean = true;
    private BuildScreenSections(screen: ScreenPM, fireEmit: any)
    {
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.GetScreenSections(screen)
            .subscribe(response =>
            {
                this.ScreenSections = [];

                let sections: any[] = this.ReOrderScreenSections(response.Result);
                sections = this.IsDisplaySummarySection ? sections.filter(d => d.Type == "Summary") : sections.filter(d => d.Type != "Summary");
                let childEntityResourcesArgs: ChildEntityResourcesArgs = new ChildEntityResourcesArgs();
                childEntityResourcesArgs.Screen = screen;
                childEntityResourcesArgs.Sections = sections;
                childEntityResourcesArgs.FireEmit = fireEmit;
                childEntityResourcesArgs.Index = 0;
                this.LoadAllChildEntityResources(childEntityResourcesArgs);
            });
    }

    LoadAllChildEntityResources(childEntityResourcesArgs: ChildEntityResourcesArgs) {
        if (childEntityResourcesArgs.Sections.length <= childEntityResourcesArgs.Index) {
            this.LoadAllChildEntityResourcesCompleted(childEntityResourcesArgs);
            return;
        }

        let relatedScreen = window.Screens.filter((screen: any) => screen.Code === childEntityResourcesArgs.Sections[childEntityResourcesArgs.Index].RelatedScreenCode)[0];
        childEntityResourcesArgs.Index = childEntityResourcesArgs.Index + 1;
        if (!relatedScreen) {
            this.LoadAllChildEntityResources(childEntityResourcesArgs);
            return;
        }
        let childObjectTable = window.ObjectTables.filter((table: any) => table.Id === relatedScreen.ObjectTableId)[0];
        if (!childObjectTable) {
            this.LoadAllChildEntityResources(childEntityResourcesArgs);
            return;
        }

        if (childObjectTable.IsCustom) {
            this.LoadAllChildEntityResources(childEntityResourcesArgs);
            return;
        }

        this.entityResourceService.getEntityResourceByTableName(childObjectTable.Name).subscribe((response: any) => {
            if (childEntityResourcesArgs.Sections.length != childEntityResourcesArgs.Index)
                this.LoadAllChildEntityResources(childEntityResourcesArgs);
            else
                this.LoadAllChildEntityResourcesCompleted(childEntityResourcesArgs);
        });
    }

    private LoadAllChildEntityResourcesCompleted(childEntityResourcesArgs: ChildEntityResourcesArgs) {
        const screenFields = GetScreenFields(childEntityResourcesArgs.Screen, childEntityResourcesArgs.Sections);
        if (screenFields.length == 0) {
            this.CurrentSession.StopBusyIndicator();
            return this.ShowNoFieldsText = true;
        }

        childEntityResourcesArgs.Sections.forEach(section => this.BuildScreenSection(childEntityResourcesArgs.Screen, section, screenFields, this.GetObjectFields()));

        if (childEntityResourcesArgs.FireEmit) {
            this.LoadCompleted.emit(true);
        }
        this.CurrentSession.StopBusyIndicator();
    }

    private BuildScreenSection(screen: ScreenPM, section: ScreenSectionPM, screenFields: any, objectFields: any)
    {
        if (section.Type == "Grid") {
            this.BuildScreenGridSection(section, screenFields);
            return;
        }
        const columns: ScreenColumn[] = [];

        for (let i = 0; i < screen.NumberOfColumns; i++){

            const column = this.BuildScreenColumn(i, section, screenFields, objectFields);
            if(column.ObjectFields.length == 0)
                continue;
            columns.push(column);
        }

        this.ScreenSections.push(new ScreenSection(section, columns));
    }

    BuildScreenGridSection(section: ScreenSectionPM, screenFields: any) {
        const columns: ScreenColumn[] = [];
        this.AddGridSection(screenFields, section, columns);
    }

    private AddGridSection(screenFields: any, section: ScreenSectionPM, columns: ScreenColumn[]) {
        screenFields.filter(screenField => screenField.ScreenCode === section.RelatedScreenCode).forEach(field => {
            const column = new ScreenColumn(field.Column);
            const objectField = window.ObjectFields.filter((f: any) => f.FieldCode == field.ObjectFieldCode)[0];
            if (objectField) {
                column.ObjectFields.push(objectField);
                columns.push(column);
            }
        });
        this.ScreenSections.push(new ScreenSection(section, columns));
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
            if (!objectField || (!objectField.IsCustom && this.IsFromGrid && !this.IsDisplaySummarySection))
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

    private ReOrderScreenSections(screenSections: any[]) {
        return screenSections.sort((a, b) => {
            return (a.Number === b.Number) ? 0 : (a.Number < b.Number) ? -1 : 1
        });
    }
    public ShowSectionName(screenSection: any) {
        return screenSection.Type != 'Grid' && screenSection.Title && screenSection.ScreenColumns && screenSection.ScreenColumns.length > 0 && !(this.ScreenSections && screenSection == this.ScreenSections[0] && this.BuildLighteningScreenAsClassicScreen)
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

export class ChildEntityResourcesArgs {
    public Screen: ScreenPM;
    public Sections: any[];
    public FireEmit: any;
    public Index: number;
}

export class ScreenSection {
    public SectionNumber: number;
    public Title: string;
    public ScreenColumns: ScreenColumn[];
    public Type: string;
    public RelatedScreenCode: string;
    actualColumnsCount = 0;
    constructor(screenSectionPM: ScreenSectionPM, screenColumns: ScreenColumn[]) {
        this.SectionNumber = screenSectionPM.Number;
        this.Title = screenSectionPM.Name;
        this.Type = screenSectionPM.Type;
        this.RelatedScreenCode = screenSectionPM.RelatedScreenCode;
        this.ScreenColumns = screenColumns;
        this.actualColumnsCount = screenColumns?.filter(c=>c.ObjectFields.length > 0).length;
    }

}
function GetScreenFields(screen: ScreenPM, sections: any)
{
    var screenFields = window.ScreenFields.filter((x: any) => x.ScreenId === screen.Id && x.Tenant === SessionInfo.LoggedUserTenant);

    sections?.forEach(section => {
        if (section.Type == "Grid") {
            screenFields = screenFields.concat(window.ScreenFields.filter((x: any) => x.ScreenCode === section.RelatedScreenCode && x.Tenant === SessionInfo.LoggedUserTenant));
        }
    });

    if (screenFields.length == 0) {
        screenFields = window.ScreenFields.filter((x: any) => x.ScreenId === screen.Id);
    }
    return screenFields;
}

