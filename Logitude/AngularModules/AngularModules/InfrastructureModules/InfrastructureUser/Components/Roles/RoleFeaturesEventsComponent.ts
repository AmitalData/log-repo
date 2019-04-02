import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    moduleId: module.id,
    templateUrl: './RoleFeaturesEventsComponent.html',
})

export class RoleFeaturesEventsComponent {
    public ItemsSource: any[] = [];
    private myDomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myDomainService = new InfrastructureDomainService();
    }

    SetWindowArgs(myRoleId: string) {
        if (myRoleId) {
            this.myDomainService.GetRoleFeaturesChanges(myRoleId).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;
                }
            });
        }
    }

    ShowNotesClicked(notes: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Role Events Changes";
        logWindow.WindowArgs = notes;
        logWindow.Width = 700;
        logWindow.Height = 450;
        logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Roles/FeaturesEventChangesComponent');
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
