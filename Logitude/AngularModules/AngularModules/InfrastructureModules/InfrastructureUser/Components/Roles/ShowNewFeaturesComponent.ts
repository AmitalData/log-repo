import {Component, OnInit} from '@angular/core';
import {FeatureList} from '../../../../Infrastructure/EntityLists/FeatureList';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './ShowNewFeaturesComponent.html',
})

export class ShowNewFeaturesComponent implements OnInit {
    public ItemsSource: FeatureList[] = [];
    private myDomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.myDomainService = new InfrastructureDomainService();
    }

    ngOnInit() {
        this.LoadData();
    }

    LoadData() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.myDomainService.GetNewFeaturesList().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ItemsSource = myResponse.Result;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
