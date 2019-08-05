import { Component, OnInit} from '@angular/core';
import { SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';


@Component({
    moduleId: module.id,
    templateUrl: './AMANACComponent.html',
})

export class AMANACComponent implements OnInit {

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    ngOnInit() {
       
    }

}
