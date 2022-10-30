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
declare var window;

@Component({
    selector: 'GridScreenSectionComponent',
    templateUrl: './GridScreenSectionComponent.html',
    inputs: ['ScreenSection']
})

export class GridScreenSectionComponent extends BaseComponent implements OnInit, AfterViewInit {
    DataContext: GridScreenSectionComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    ScreenSection: any;
    Screen: any;
    public DataSource: ObservableCollection;

    constructor() {
        super();
        this.DataSource = new ObservableCollection([]);
        this.FillDataSource();
    }

    FillDataSource() {
        for (let i = 0; i < 25; i++) {
            this.DataSource.Insert('Sample');
        }
    }

    ngAfterViewInit(): void {
        this.SetScreen();
    }

    SetScreen() {
        if (!this.ScreenSection) return;
        this.OrderScreenSectionColumns();
        this.Screen = window.Screens.filter((screen: any) => screen.Code === this.ScreenSection.RelatedScreenCode)[0];
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

    AddChildEntityClicked() {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Add New " + this.ScreenObjectTableName;
        logitudeWindow.WindowArgs = { EntityPM: new ChildEntity(), Screen: this.Screen }; 
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 530;
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/Customization/Screen/Section/AddEditChildEntityComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            if ($event == "AddFollowUpSucceeded") {
            }
        });
    }

    EditChildEntityClicked(childEntity) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit " + this.ScreenObjectTableName;
        logitudeWindow.WindowArgs = { IsEditMode: true, Screen: this.Screen, EntityPM: new ChildEntity()/*childEntity*/ }; 
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 530;
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/Customization/Screen/Section/AddEditChildEntityComponent');
    }

    DeleteChildEntityClicked(childEntity) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Delete? soon...");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
            }
        });
    }
}

export class ChildEntity {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    public IsDirty: boolean;
}
