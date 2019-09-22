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
import { GLAccountPMService } from '../../Services/StandardPMs/GLAccountPMService';
@Component({
    moduleId: module.id,
    templateUrl: "./GLAccountShortTitleComponent.html",
}) 






export class GLAccountShortTitleComponent {
    public EntityPM: GLAccountPM;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    GLAccountPMService: GLAccountPMService = new GLAccountPMService();
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
    ObjectTableName: string;
    SelectedTabCode: string;
    OpenCardScreen() {

       this.GetObjectTableNameAndTabCode();

        if (AppTool.IsNullOrEmpty(this.EntityPM.CustomerGLAccountId)) {
            this.OpenCardScreenForNotSplittedGLAccount(this.ObjectTableName, this.SelectedTabCode);
        }
        else {
           
            this.OpenCardScreenForSplittedGLAccount(this.ObjectTableName, this.SelectedTabCode);
           
        }
    }

    private GetObjectTableNameAndTabCode() {

        if (this.EntityPM.AccountTypeCode == "3") {
            this.ObjectTableName = "Vendor";
            this.SelectedTabCode = null;
        }
        else if (this.EntityPM.AccountTypeCode == "2") {
            this.ObjectTableName = "Customer";
            this.SelectedTabCode = "CLOV";
        }

    }

    OpenCardScreenForNotSplittedGLAccount(objectTableName: string, selectedTabCode:string) {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CardId)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.EntityPM.CardId, ObjectTableName: objectTableName, SelectedTabCode: selectedTabCode });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }




    }

    OpenCardScreenForSplittedGLAccount(objectTableName: string, selectedTabCode: string) {

        this.CurrentSession.StartBusyIndicatorLoading();
        this.GLAccountPMService.get(this.EntityPM.CustomerGLAccountId).subscribe((myResponse: ServiceResponse) => {

            if (myResponse) {
                if (!myResponse.HasError) {
                    var ParentAccount = myResponse.Result;
                    if (!AppTool.IsNullOrEmpty(ParentAccount)) {
                        if (!AppTool.IsNullOrEmpty(ParentAccount.CardId)) {
                            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.CurrentSession.StopBusyIndicator();
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run({ EntityId: ParentAccount.CardId, ObjectTableName: objectTableName, SelectedTabCode: selectedTabCode });
                                    cmpRef.instance.BackCompleted.subscribe(bk => {
                                    });
                                });
                        }
                    }
                }
            }

        });


    }
}
