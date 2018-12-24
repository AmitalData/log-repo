import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BIReportPM } from '../../../../Infrastructure/EntityPMs/BIReportPM';
import { BIReportPMService } from '../../../../Infrastructure/Services/StandardPMs/BIReportPMService';
import { ServiceArgs } from '../../../../Infrastructure/DataContracts/ServiceArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    moduleId: module.id,
    templateUrl: './NewBIReport.html',
})

export class NewBIReport extends BaseComponent {
    public EntityPM: BIReportPM;
    public ValidationErrorsList: string[] = [];
    private myService: BIReportPMService;

    constructor() {
        super();
        this.EntityPM = new BIReportPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.myService = new BIReportPMService();
    }


    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit('cancel');
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();

            this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                SessionLocator.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

            });
        }
    }
}
