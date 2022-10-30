import { Component, OnInit, AfterViewInit, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenSectionPM } from '../../../../../../Infrastructure/EntityPMs/ScreenSectionPM';
import { ScreenFieldPM } from '../../../../../../Infrastructure/EntityPMs/ScreenFieldPM';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { ScreenPM } from '../../../../../EntityPMs/ScreenPM';
import { ConfirmWindow } from '../../../../../../Controls/Windows/ConfirmWindow';
import { EntityArgs } from '../../../../../DataContracts/EntityArgs';
declare var window;

@Component({
    templateUrl: './AddEditChildEntityComponent.html',
})

export class AddEditChildEntityComponent extends BaseComponent implements OnInit, AfterViewInit {
    private CurrentSession = SessionLocator.SelectedSession;
    Screen: any;
    public DataSource: ObservableCollection;
    public ValidationErrorsList: string[] = [];
    public IsEditMode: boolean;
    public EntityPM: any;
    @ViewChild('GeneratedArea', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;

    constructor(private entityArgs: EntityArgs) {
        super();
    }

    public LoadGeneratedArea() {
        if (!this.viewContainerRef) {
            this.RunComponentTimer();
            return;
        }

        this.LoadChildComponent();
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.LoadGeneratedArea(), 1);
        }
    }

    LoadChildComponent() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.HideLastColumn = true;
                cmpRef.instance.IsFromGrid = true;
                cmpRef.instance.Run(this.EntityPM, this.Screen.ObjectTableName, this.Screen.Code);
            });
    }

    ngAfterViewInit(): void {

    }

    ngOnInit() {
    }

    SetWindowArgs(args: any) {
        this.Screen = args.Screen;
        if (!this.Screen) return;
        this.EntityPM = args.EntityPM;
        this.LoadGeneratedArea();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
