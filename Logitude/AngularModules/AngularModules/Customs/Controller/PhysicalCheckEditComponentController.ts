import { AmitalGatewayUtil, UnifreightMessageM } from   '../../Infrastructure/Utilities/AmitalGatewayUtil';
import { AppTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { IEditComponentController } from '../../Infrastructure/Components/EditComponent/EditComponent';
import { DeclarationPM } from '../../Customs/EntityPMs/DeclarationPM';
import {MenuButtonsEvents, MenuButtonsStateChangedEventArgs} from '../../Infrastructure/Utilities/events/MenuButtonsEvents';
import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';
import { PhysicalCheckPM } from '../EntityPMs/PhysicalCheckPM';

export class PhysicalCheckEditComponentController implements IEditComponentController {

    private CurrentSession = SessionLocator.SelectedSession;
    private _ControllerOn: boolean = false;

    FilterTabs(allTabs: any[]) {
        let currentEntity: PhysicalCheckPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        var indexOfTab = allTabs.findIndex(t => t.Code == "ANPC");
        if (!currentEntity.NoEscortRequired==true) {
            if (indexOfTab > -1) {
                allTabs.splice(indexOfTab, 1);
            }
        }
    }

    OnFirstTimeAfterSingleDataLoaded(CurrentEntity: any): Promise<boolean> {
        return new Promise((resolve, reject) => {
            this._ControllerOn = true;
            resolve(this._ControllerOn);

            return;
        });
    }
    OnReloadEntityPM(): Promise<any> {
        return new Promise((resolve, reject) => {
            resolve();
            return;
        });   }
    OnCloseEditControl(onCallBack?: () => void): void {
        return;    }
    HaveSaved: boolean;
    InDisplayMode: boolean;
    ToCancell: boolean;
    InDisplayModeMessage: string;
    MustRefresh: boolean;
    MustRefreshMessage: string;
    IsInBatchRequest: boolean;
    ResetMustRefresh(): void {
        return;    }
    IsDisabled(itemTabCode: string): boolean {
        return;    }
 
}
