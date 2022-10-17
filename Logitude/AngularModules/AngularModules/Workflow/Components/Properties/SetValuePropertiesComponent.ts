import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { SetValue } from "Workflow/Models/SetValue";

@Component({
    templateUrl: "./SetValuePropertiesComponent.html"
})

export class SetValuePropertiesComponent extends BaseComponent {
    public DataContext: any = this;
    public Name: string = null;
    public SetValues: SetValue[];
    public ObjectFieldsDictionary: any = {};

    public Data: any;
    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldPM[];
    public CurrentSession = SessionLocator.SelectedSession;

    public IsValidSetValues: boolean = true;
    public ValidationErrorsList: string[];

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];

        this.initialize();
    }

    ngOnInit() {
        
    }

    ngOnChanges() {
        
    }

    initialize() {
        this.Name = this.Data["name"] || null;
        this.SetValues = this.Data["setValues"] || [];

        this.initializeSetValue();

        this.setUIProperties();
    }

    initializeSetValue() {
        if (this.SetValues.length == 0) {
            let setValue = new SetValue();
            this.SetValues.push(setValue);
            this.IsValidSetValues = false;
        }
    }

    updateName(name: any) {
        this.Data["name"] = name;
        this.Name = name;
        this.setUIProperties();
    }

    updateIsValidSetValues(isValidSetValues: boolean) {
        this.IsValidSetValues = isValidSetValues;
    }

    setUIProperties() {
        this.UIProperties.SetRequired("Name", null, AppTool.IsNullOrEmpty(this.Name));
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        let notValidUIProperties = this.UIProperties.UIPropertyList.filter(u => !u.ValidValue);
        if (notValidUIProperties.length === 0 && this.IsValidSetValues) {
            this.setValuesData();

            //console.log(this.Data);

            this.CurrentSession.CurrentWindow.Close(this.Data);
        } else {
            let validationErrors = notValidUIProperties.map(t => { return t.ValidationError; });
            this.ValidationErrorsList = validationErrors

            if (!this.IsValidSetValues)
                this.ValidationErrorsList.push("Invalid Set Values");
        }
    }

    setValuesData() {
        this.Data["setValues"] = this.SetValues;
    }
}