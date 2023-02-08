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
import { ApiQueryFilters } from '../../../../../DataContracts/ApiQueryFilters';
import { ScreenSectionListService } from '../../../../../Services/StandardLists/ScreenSectionListService';
import { ClassLevelValidator } from '../../../../../Validators/ClassLevelValidator';
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
    public IsSummarySectionAvailable:boolean = false;
    public SummarySectionName: string;
    @ViewChild('GeneratedArea', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    @ViewChild('SummaryGeneratedArea', { read: ViewContainerRef, static: false }) summaryViewContainerRef: ViewContainerRef;
    screenSectionService = new ScreenSectionListService();

    constructor(private entityArgs: EntityArgs) {
        super();
    }

    public LoadGeneratedArea() {
        if (!this.viewContainerRef) {
            this.RunComponentTimer("GeneratedArea");
            return;
        }
        this.LoadChildComponent(this.viewContainerRef);
    }

    IsSummaryGeneratedAreaLoaded: boolean = false;
    public LoadSummaryGeneratedArea() {
        if (this.IsSummaryGeneratedAreaLoaded) return;
        this.Retries = 0;
        if (!this.summaryViewContainerRef) {
            this.RunComponentTimer("SummaryGeneratedArea");
            return;
        }
        this.LoadChildComponent(this.summaryViewContainerRef);
        this.IsSummaryGeneratedAreaLoaded = true;
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer(componentName: String) {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = componentName == "SummaryGeneratedArea" ? setTimeout(() => this.LoadSummaryGeneratedArea(), 1) : setTimeout(() => this.LoadGeneratedArea(), 1);
        }
    }

    LoadChildComponent(viewContainerRef) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.HideLastColumn = true;
                cmpRef.instance.IsFromGrid = true;
                cmpRef.instance.IsDisplaySummarySection = !(viewContainerRef == this.viewContainerRef);
                cmpRef.instance.Run(this.EntityPM, this.Screen.ObjectTableName, this.Screen.Code);
                if(viewContainerRef == this.viewContainerRef) this.LoadSummaryGeneratedArea();
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
        this.SetSummarySectionAvailablity();
    }
    SetSummarySectionAvailablity() {
        this.GetSummaryScreenSection()
            .subscribe(response => {
                if (response.HasError) return;
                let screenSections = response.Result;
                let summarySection = screenSections.filter(section => !section.InActive && section.Type == "Summary");
                if (!summarySection) return;
                if (!summarySection[0]) return;
                this.IsSummarySectionAvailable = true;
                this.SummarySectionName = summarySection[0].Name;
            });
        
    }

    private GetSummaryScreenSection() {
        const filters = new ApiQueryFilters();
        filters.GetAll = true;
        filters.PageSize = 1000;
        filters.addAdditionalFilter("ScreenCode", this.Screen.Code, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Type", "Summary", null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Inactive", false, null, null, "Equals", false, false, false, "Boolean");
        return this.screenSectionService.getByFilters(filters)
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
    SaveChangesAndOpen() {
        this.OkButtonClicked();
        if (this.ValidationErrorsList.length != 0) return;
        this.FatherComponent.AddChildEntityClicked();
    }
    private AddCustomChildEntity() {
        if (this.ParentEntityPM.CustomChildEntities.filter(a => a.Name == this.ObjectTableName).length != 0) {
            return;
        }

        this.ParentEntityPM.CustomChildEntities.push(new CustomChildEntity(this.ParentEntityPM, this.ParentObjectTableName, this.ObjectTableName));
    }

    CloseButtonClicked() {
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

    private RejectChanges()
    {
        for (let i = 1; i < this.numberOfCustomChildObjectCustomFields+1; i++)
        {
            this.ResetChangedCustomFields(i);
        }
        this.FatherComponent.LoadData();
    }
    private ResetChangedCustomFields(CustomFieldIndex: number) {
        let oldCustomFieldValue = this.GetCustomChildObjectCustomFieldClass(this.cloneCustomChildObjectPM, CustomFieldIndex);
        if (this.EntityPM['Field' + CustomFieldIndex].Value != oldCustomFieldValue.Value) {
            this.EntityPM['Field' + CustomFieldIndex] = oldCustomFieldValue;
            this.EntityPM.IsDirty = false;
        }
    }
    private GetCustomChildObjectCustomFieldClass(cloneEntityPM: CustomChildObjectPM, index: number) {
        let customFieldClass = cloneEntityPM['Field' + index];
        if (!customFieldClass) {
            return null;
        }

        return new CustomFieldClass(customFieldClass.Value, customFieldClass.FieldName, customFieldClass.TableName);
    }
}
