import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowVersionPM } from 'Workflow/EntityPMs/WorkFlowVersionPM';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';

@Component({
    templateUrl: "WorkFlowShortTitleComponent.html",
})

export class WorkFlowShortTitleComponent {
    public EntityPM: WorkFlowPM;
    public ValidVersion: WorkFlowVersionPM;

    constructor(public entityArgs: EntityArgs) { }

    ngOnInit() {
        this.listen()
    }

    private listen() {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe((message: any) => {
                if (message == "RefreshWorkflowShortTitle") {
                    this.EntityPM = this.entityArgs.EntityPM;
                    this.ValidVersion = this.getValidVersion();
                    var clickedRowId = this.entityArgs.EditComponentArgument?.ClickedVersionRow!;
                    var updatedVersionId = this.entityArgs.EditComponentArgument?.UpdatedVersion!;
                    if (updatedVersionId) {
                        var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == updatedVersionId);
                        this.ValidVersion = version;
                    } else if (clickedRowId) {
                        var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == clickedRowId);
                        this.ValidVersion = version;
                    }
                }
            });
        }
    }

    get WorkflowName() {
        var result = "";
        if (this.EntityPM != null) {
            result = this.EntityPM.Name;
        }
        return result;
    }

    get WorkflowVersionNumber() {
        if (this.EntityPM != null && this.ValidVersion) {
            return this.ValidVersion.VersionNumber.toString();
        }
        return "";
    }

    getValidVersion() {
        var versions = this.EntityPM.WorkFlowVersions.sort((a, b) => a.VersionNumber < b.VersionNumber ? 1 : -1);
        var activeVersion = versions.find(e => e.StatusCode == "ACVE");
        var newestVersion = versions[0];
        if (activeVersion) {
            return activeVersion;
        } else {
            return newestVersion;
        }
    }

    editWorkflowVersionClicked(){
        var CurrentDisplayedVersionId = this.entityArgs.EditComponentArgument?.CurrentDisplayedVersionId
        var version = this.EntityPM.WorkFlowVersions.find(v => v.Id == CurrentDisplayedVersionId);

        let propertiesComponentPath = "./Workflow/Components/WorkflowBuilder/EditWorkflowVersionComponent";
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            WorkFlowVersion : version
        };
        propertiesWindow.Height = 340;
        propertiesWindow.Width = 985;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = "Edit Workflow Verison";
        propertiesWindow.WindowArgs = propertiesWindowArgs;

        propertiesWindow.Show(propertiesComponentPath);
        propertiesWindow.WindowClosed.subscribe((data: any) => { this.handleCreateNewVersionResponse(data); });
    }

    handleCreateNewVersionResponse(data: WorkFlowVersionPM) {
        if (data) {
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, UpdatedVersion: data.Id }
            this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, ClickedVersionRow: null }
            this.entityArgs.SendMessage("WorkflowVersionsUpdated");
        }
    }
}