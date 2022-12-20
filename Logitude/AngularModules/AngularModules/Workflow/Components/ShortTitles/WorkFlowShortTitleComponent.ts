import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { WorkFlowVersionPMService } from 'Workflow/Services/StandardPMs/WorkFlowVersionPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { WorkFlowVersionPM } from 'Workflow/EntityPMs/WorkFlowVersionPM';

@Component({
    templateUrl: "WorkFlowShortTitleComponent.html",
})

export class WorkFlowShortTitleComponent {
    public EntityPM: WorkFlowPM;
    public ValidVersion: WorkFlowVersionPM;

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ValidVersion = this.getValidVersion();
        this.Listen()
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "RefreshWorkflowShortTitle") {
                        var clickedRowId = this.entityArgs.EditComponentArgument?.ClickedVersionRow!
                        var updatedVersionId = this.entityArgs.EditComponentArgument?.UpdatedVersion!
                        if (updatedVersionId) {
                            var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == updatedVersionId);
                            this.ValidVersion = version;
                        }
                        else if (clickedRowId) {
                            var version = this.EntityPM.WorkFlowVersions.find(e => e.Id == clickedRowId);
                            this.ValidVersion = version;
                        } else {
                            this.ValidVersion = this.getValidVersion();
                        }
                    }
                }
            );
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
            return activeVersion
        } else {
            return newestVersion
        }
    }

}
