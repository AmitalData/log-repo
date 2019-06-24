import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    moduleId: module.id,
    templateUrl: './OccasionWorkspaceComponent.html',
})

export class OccasionWorkspaceComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    public IsNewOccasionVisible = false;

    constructor(private _entityResourceService: EntityResourceService) {
        if (FeatureLocator.HasFeaturePermession("Occasion", "New")) {
            this.IsNewOccasionVisible = true;
        }
    }

    NewOccasionClicked() {
        this._entityResourceService.getEntityResourceByTableName("OccasionType", 0).subscribe(response => {
            var windowTitle = "New Occasion";
            var logWindow = new LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 700;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(($event: any) => this.OnNewWindowClosed($event));
            logWindow.Show('./CRMModules/CRMOccasion/Components/NewEntity/NewOccasionComponent');
        });
    }

    OnNewWindowClosed(arg) {
        this.LoadQueriesCounts();
    }

    // Queries Count
    LoadQueriesCounts() {

    }
}
