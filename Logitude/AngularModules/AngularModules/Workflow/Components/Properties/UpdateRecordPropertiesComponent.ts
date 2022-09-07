import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

@Component({
    templateUrl: "./UpdateRecordPropertiesComponent.html"
})

export class UpdateRecordPropertiesComponent extends BaseComponent {

    public DataContext: any = this;
    public Data: any;

    public CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: any) {
        this.Data = args.Data ? args.Data : {};
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.CurrentSession.CurrentWindow.Close(this.Data);
    }
}