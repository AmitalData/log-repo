import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowVersionPM } from 'Workflow/EntityPMs/WorkFlowVersionPM';

@Component({
    templateUrl: "WorkFlowShortTitleComponent.html",
})

export class WorkFlowShortTitleComponent {
    public EntityPM: WorkFlowPM;
    public ValidVersion: WorkFlowVersionPM;
    public WarningErrorsList: string[] = [];
    public WarningErrorTitle: string = null;

    constructor(public entityArgs: EntityArgs) {
    }

    ngOnInit() {
        this.Listen()
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "RefreshWorkflowShortTitle") {
                        this.EntityPM = this.entityArgs.EntityPM;
                        this.ValidVersion = this.getValidVersion();
                        var clickedRowId = this.entityArgs.EditComponentArgument?.ClickedVersionRow!
                        var updatedVersionId = this.entityArgs.EditComponentArgument?.UpdatedVersion!
                        if (updatedVersionId) {
                            var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == updatedVersionId);
                            this.ValidVersion = version;
                        }
                        else if (clickedRowId) {
                            var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == clickedRowId);
                            this.ValidVersion = version;
                        }
                        this.setWarningErrorMessage();
                    }
                }
            );
        }
    }

    setWarningErrorMessage() {
        var warnings: string[] = [];
        if (this.ValidVersion.StatusCode != "DRFT") {
            this.WarningErrorTitle = "This version is currently active or was activated at least once. To make changes create a new version."
            warnings.push(this.WarningErrorTitle);
        }

        this.WarningErrorsList = warnings;
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
            return activeVersion
        } else {
            return newestVersion
        }
    }

}
