import {Component, OnInit}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    selector: 'SimplogInfoPopup',
    templateUrl: './SimplogInfoPopupComponent.html',  
})

export class SimplogInfoPopupComponent implements OnInit {

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

      
    }

    ngOnInit(


    ) {

    }


    DataContext: any;
    SetDataContext(dataContext: any) {
    }



    CancelButtonClicked() {

        this.CurrentSession.CurrentWindow.Close("Cancel");
    }



    SaveButtonClicked() {
        this.CurrentSession.CurrentWindow.Close("Regenerate");
    }




}
