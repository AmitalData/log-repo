import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    templateUrl: "./ConditionPropertiesComponent.html"
})

export class ConditionPropertiesComponent extends BaseComponent {

    private CurrentSession = SessionLocator.SelectedSession;

    DataContext: any = this;
    Data: any;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
    }

    ChangeName(name: string) {
        this.Data["name"] = name;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveButtonClicked() {
        this.CurrentSession.CurrentWindow.Close(this.Data);
    }
}