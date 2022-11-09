import { Component, OnInit, AfterViewInit } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenSectionPM } from '../../../../../../Infrastructure/EntityPMs/ScreenSectionPM';
import { ScreenFieldPM } from '../../../../../../Infrastructure/EntityPMs/ScreenFieldPM';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { ScreenPM } from '../../../../../EntityPMs/ScreenPM';
import { ConfirmWindow } from '../../../../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../../../../Controls/Windows/LogitudeWindow';
import { UIProperties } from '../../../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { Output, EventEmitter } from '@angular/core';
import { PropertyChangedArgs } from '../../../../../EventEmitterArgs/PropertyChangedArgs';
import { ObjectsLocator } from '../../../../../Locators/ObjectsLocator';
declare var window;

@Component({
    selector: 'GridScreenSectionComponent',
    templateUrl: './GridScreenSectionComponent.html',
    inputs: ['ScreenSection', 'ParentEntityPM', 'ParentObjectTableName']
})

export class GridScreenSectionComponent extends BaseComponent implements OnInit, AfterViewInit {
    DataContext: GridScreenSectionComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    ScreenSection: any;
    Screen: any;
    ParentEntityPM: any
    ParentObjectTableName: string;
    ObjectTable: any;
    public Direction: string = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    public TextAlign = this.Direction == 'rtl' ? 'right' : 'left';
    public DataSource: ObservableCollection;

    constructor() {
        super();
    }

    LoadData() {
        let data = this.ParentEntityPM?.CustomChildEntities?.filter(x => x.Name == this.ScreenObjectTableName)[0]?.Values;
        if (!data) data = [];
        data = this.SortDataBySortingField(data);
        this.DataSource = new ObservableCollection(data);
    }

    SortDataBySortingField(data) {
        if (!this.Screen) return data;
        let sortingFieldCode: string = 'CreateDate';
        let SortedByField = window.ObjectFields.filter(field => field.FieldCode == this.Screen.SortedByFieldCode)[0];
        let isCustomSortedByField: boolean = false;
        if (SortedByField) { sortingFieldCode = SortedByField.IsCustom ? SortedByField.FieldName : SortedByField.Code; }
        if (SortedByField) { isCustomSortedByField = SortedByField.IsCustom; }
        let descendingSortedType: boolean = this.Screen.SortedType == 'Descending';
        return data.sort((customChildObject1, customChildObject2) => {
            let value1 = isCustomSortedByField ? customChildObject1[sortingFieldCode].Value : customChildObject1[sortingFieldCode];
            let value2 = isCustomSortedByField ? customChildObject2[sortingFieldCode].Value : customChildObject2[sortingFieldCode];
            return this.GetSortIndexValue(value1, value2, descendingSortedType);
        });
    }

    private GetSortIndexValue(value1: any, value2: any, descendingSortedType: boolean) {
        if (value1 > value2) {
            return descendingSortedType ? -1 : 1;
        }

        if (value1 < value2) {
            return descendingSortedType ? 1 : -1;
        }

        return 0;
    }

    ngAfterViewInit(): void {
        this.SetScreen();
        this.LoadData();
    }

    SetScreen() {
        if (!this.ScreenSection) return;
        this.OrderScreenSectionColumns();
        this.Screen = window.Screens.filter((screen: any) => screen.Code === this.ScreenSection.RelatedScreenCode)[0];
        this.ObjectTable = window.ObjectTables.filter(table => table.Name == this.ScreenObjectTableName)[0];
    }

    OrderScreenSectionColumns() {
        this.ScreenSection.ScreenColumns = this.ScreenSection.ScreenColumns.sort((screenColumn1, screenColumn2) => {
            if (screenColumn1.Index > screenColumn2.Index) {
                return 1;
            }

            if (screenColumn1.Index < screenColumn2.Index) {
                return -1;
            }

            return 0;
        });
    }

    ngOnInit() {
    }

    SetWindowArgs(args: any) {

    }

    get ScreenObjectTableName() {
        if (!this.Screen) return "";
        return this.Screen.ObjectTableName;
    }

    get ScreenSectionName() {
        if (!this.ScreenSection) return "";
        return this.ScreenSection.Name;
    }

    AddChildEntityClicked() {
        let logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Add " + this.ScreenSectionName;
        logitudeWindow.WindowArgs = this.GetWindowArgs();
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 530;
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/Customization/Screen/Section/AddEditChildEntityComponent');
    }

    EditChildEntityClicked(childEntity) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit " + this.ScreenSectionName;
        logitudeWindow.WindowArgs = this.GetWindowArgs();
        logitudeWindow.WindowArgs.IsEditMode = true;
        logitudeWindow.WindowArgs.EntityPM = childEntity;
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 530;
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/Customization/Screen/Section/AddEditChildEntityComponent');
    }

    private GetWindowArgs() {
        let windowArgs: any = {};
        windowArgs.ParentEntityPM = this.ParentEntityPM;
        windowArgs.Screen = this.Screen;
        windowArgs.ObjectTableName = this.ScreenObjectTableName;
        windowArgs.ParentObjectTableName = this.ParentObjectTableName;
        windowArgs.FatherComponent = this;

        return windowArgs;
    }

    DeleteChildEntityClicked(childEntity) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete This Line?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            this.DeleteChildEntity(confirmWindow, childEntity);
        });
    }

    DeleteChildEntity(confirmWindow: ConfirmWindow, childEntity) {
        if (!confirmWindow.Yes) return;
        let index = this.ParentEntityPM.CustomChildEntities.findIndex(a => a.Name == this.ScreenObjectTableName);
        if (index < 0) return;
        this.ParentEntityPM.CustomChildEntities[index].RemoveCustomChildObject(childEntity);
        this.ParentEntityPM.IsDirty = true;
        this.LoadData();
    }
}
