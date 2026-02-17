import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './PackageFeaturesEventsComponent.html',
})

export class PackageFeaturesEventsComponent {
    public ItemsSource: any[] = [];
    private myDomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myDomainService = new InfrastructureDomainService();
    }

    SetWindowArgs(myPackageCode: string) {
        if (myPackageCode) {
            this.myDomainService.GetPackageFeaturesChanges(myPackageCode).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                }
            });
        }
    }

    ShowNotesClicked(notes: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Package Events Changes";
        logWindow.WindowArgs = notes;
        logWindow.Width = 700;
        logWindow.Height = 450;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/FeaturesEventChangesComponent');
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
