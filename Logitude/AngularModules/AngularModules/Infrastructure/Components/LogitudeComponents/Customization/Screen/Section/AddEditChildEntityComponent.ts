import { Component, OnInit, AfterViewInit, ViewChild, ViewContainerRef } from '@angular/core';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenSectionPM } from '../../../../../../Infrastructure/EntityPMs/ScreenSectionPM';
import { ScreenFieldPM } from '../../../../../../Infrastructure/EntityPMs/ScreenFieldPM';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { ScreenPM } from '../../../../../EntityPMs/ScreenPM';
import { ConfirmWindow } from '../../../../../../Controls/Windows/ConfirmWindow';
import { EntityArgs } from '../../../../../DataContracts/EntityArgs';
import { CustomChildObjectPM } from '../../../../../EntityPMs/CustomChildObjectPM';
import { CustomChildEntity } from '../../../../../EntityPMs/CustomChildEntity';
import { DateTool } from '../../../../../Tools';
import { Validator } from '../../../../../Validators/Validator';
import { CustomFieldClass } from '../../../../../DataContracts/CustomFieldClass';
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
    public EntityPM: CustomChildObjectPM;
    public ObjectTableName: string;
    public ParentEntityPM: any;
    public ParentObjectTableName: string;
    public FatherComponent: any;
    private numberOfCustomChildObjectCustomFields: number = 50; //FromTable: CustomFieldsCount
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
        this.ParentEntityPM = args.ParentEntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.ParentObjectTableName = args.ParentObjectTableName;
        this.IsEditMode = args.IsEditMode;
        this.FatherComponent = args.FatherComponent;
        this.SetEntityPM();
        this.Clone();
        this.LoadGeneratedArea();
    }

    private SetEntityPM() {
        if (this.IsEditMode) return;

        this.EntityPM = new CustomChildObjectPM(this.ObjectTableName);
        this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.CreatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedBy = SessionLocator.LoggedUserId;
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.ObjectTableId = this.FatherComponent?.ObjectTable?.Id;
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);

        if (this.ValidationErrorsList.length == 0) {
            this.Save();
        }
    }

    private Save() {
        this.AddCustomChildEntity();

        let index = this.ParentEntityPM.CustomChildEntities.findIndex(a => a.Name == this.ObjectTableName);
        if (index < 0) return;
        if (!this.IsEditMode) {
            this.ParentEntityPM.CustomChildEntities[index].AddCustomChildObject(this.EntityPM);
        }
        this.ParentEntityPM.IsDirty = true;
        this.FatherComponent.LoadData();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    private AddCustomChildEntity() {
        if (this.ParentEntityPM.CustomChildEntities.filter(a => a.Name == this.ObjectTableName).length != 0) {
            return;
        }

        this.ParentEntityPM.CustomChildEntities.push(new CustomChildEntity(this.ParentEntityPM, this.ParentObjectTableName, this.ObjectTableName));
    }

    CancelButtonClicked() {
        if (this.IsEditMode) {
            this.RejectChanges();
        }
        this.CurrentSession.CloseCurrentWindow();
    }

    private cloneCustomChildObjectPM: CustomChildObjectPM;
    private Clone() {
        if (!this.IsEditMode) return;
        this.cloneCustomChildObjectPM = new CustomChildObjectPM(this.EntityPM.ObjectTableName);
        for (let i = 1; i < this.numberOfCustomChildObjectCustomFields + 1; i++) {
            this.cloneCustomChildObjectPM['Field' + i] = this.GetCustomChildObjectCustomFieldClass(this.EntityPM, i);
        }
    }

    private RejectChanges() {
        for (let i = 1; i < this.numberOfCustomChildObjectCustomFields+1; i++) {
            this.EntityPM['Field' + i] = this.GetCustomChildObjectCustomFieldClass(this.cloneCustomChildObjectPM, i);
        }
        this.FatherComponent.LoadData();
    }

    private GetCustomChildObjectCustomFieldClass(cloneEntityPM: CustomChildObjectPM, index: number) {
        let customFieldClass = cloneEntityPM['Field' + index];
        if (!customFieldClass) {
            return null;
        }

        return new CustomFieldClass(customFieldClass.Value, customFieldClass.FieldName, customFieldClass.TableName);
    }
}
