import {Component, OnInit, EventEmitter, Output}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';


@Component({
    moduleId: module.id,
    selector: 'SharedLogisticsWizard',
    templateUrl: './SharedLogisticsWizardComponent.html',

})
export class SharedLogisticsWizardComponent implements OnInit {
    @Output() OnCloseWindowEvent = new EventEmitter();
    SelectedTabCode: string;
    TenantPM: TenantPM;
    constructor() {

    }

    ngOnInit(

    ) {

        this.SelectedTabCode = "GEN";



    }

    SetDataContext(entityPM: any) {
    

    }




    CloseButtonClicked() {

        SessionLocator.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {

        this.OnCloseWindowEvent.emit("Save");//pass the Id

    }


    SetWindowArgs(args: any) {
        this.TenantPM = args.TenantPM;
        // this.Run();

    }

}