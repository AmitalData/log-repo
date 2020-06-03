import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool, FormatTool } from '../../../Infrastructure/Tools';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';

@Component({
    templateUrl: './NewHelpResouceComponent.html',
})

export class NewHelpResouceComponent extends BaseComponent {   
    private CurrentSession = SessionLocator.SelectedSession;
    private _entityResourceService: EntityResourceService;
    constructor() {
        super();

        this._entityResourceService = new EntityResourceService();   


    }        
}
