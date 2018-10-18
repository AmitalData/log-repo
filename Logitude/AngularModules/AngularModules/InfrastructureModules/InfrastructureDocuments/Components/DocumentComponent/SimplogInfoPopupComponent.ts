import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    selector: 'SimplogInfoPopup',
    templateUrl: './SimplogInfoPopupComponent.html',  
})

export class SimplogInfoPopupComponent implements OnInit {


    constructor() {

      
    }

    ngOnInit(


    ) {

    }


    DataContext: any;
    SetDataContext(dataContext: any) {
    }



    CancelButtonClicked() {

        SessionLocator.CurrentSession.CurrentWindow.Close("Cancel");
    }



    SaveButtonClicked() {
        SessionLocator.CurrentSession.CurrentWindow.Close("Regenerate");
    }




}