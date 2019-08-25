import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {CustomerList} from '../../../Common/EntityLists/CustomerList';
import {CustomerListService} from '../../../Common/Services/StandardLists/CustomerListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./GLAccountShortTitleComponent.html",
}) 






export class GLAccountShortTitleComponent {
    public EntityPM: GLAccountPM;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        if (this.EntityPM != null) {
        }
        this.Listen();
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    
                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    
                }
            });
        }
    }

    OpenCardScreen() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CardId)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityPM.CardId, ObjectTableName: 'Vendor' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

}
