import { OnInit, Component } from '@angular/core';
import { ExportDeclarationClosingDataPM } from '../../../../../Customs/EntityPMs/ExportDeclarationClosingDataPM';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ExportDeclarationClosingDataPMService } from '../../../../../Customs/Services/StandardPMs/ExportDeclarationClosingDataPMService';
import { Time } from '@angular/common';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'ExportDeclarationClosingDataComponent',
    templateUrl: './ExportDeclarationClosingDataComponent.html',
})

export class ExportDeclarationClosingDataComponent extends BaseComponent {
    public DataContext: any = this;
    public EntityPM: ExportDeclarationClosingDataPM;
    public DecPM: DeclarationPM;
    public SendButtonEnabled: boolean = false;
    public ObjectTableName: string = "Customs.ExportDeclarationClosingData";
    public IsReady: boolean = false;
    exportDeclarationClosingDataPMService: ExportDeclarationClosingDataPMService = new ExportDeclarationClosingDataPMService();
    private CurrentSession = SessionLocator.SelectedSession;


    constructor(private EntityResourceService: EntityResourceService) {
        super();
    }

    SetUIProperty() {
        this.UIProperties.SetEnabled("LoadingDateTime", this.ObjectTableName, true);
        if (this.DecPM.TransportModeId != "O") {
            this.UIProperties.SetEnabled("FinalShipCode", this.ObjectTableName, false);
        }
    }


    SetWindowArgs(args: any) {
        this.EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this.DecPM = args.EntityPM;
            this.GetExportDeclarationClosingData(this.DecPM.Id);
            this.SetUIProperty();

        });
    }

    GetExportDeclarationClosingData(id :string) {
        if (id != null) {
            this.exportDeclarationClosingDataPMService.get(id).subscribe((response: any) => {
                this.EntityPM = response.Result;
                if (this.EntityPM == null) {
                    this.EntityPM = new ExportDeclarationClosingDataPM();
                    debugger;
                }
                this.IsReady = true;
            });
        }
    }


    get LoadingDateTime() { return this.EntityPM.LoadingDateTime ? this.EntityPM.LoadingDateTime : new Date(); }
    set LoadingDateTime(value: Date) {
        if (this.EntityPM.LoadingDateTime != value) {
            this.EntityPM.LoadingDateTime = value;
        }
    }

    get FinalShipCode() { return this.EntityPM ? this.EntityPM.FinalShipCode : null; }
    set FinalShipCode(value: string) {
        if (this.EntityPM.FinalShipCode != value) {
            this.EntityPM.FinalShipCode = value;
        }
    }

    get FinalLoadingSite() { return this.EntityPM ? this.EntityPM.FinalLoadingSite : null; }
    set FinalLoadingSite(value: string) {
        if (this.EntityPM.FinalLoadingSite != value) {
            this.EntityPM.FinalLoadingSite = value;
        }
    }

    get FinalCargoTypeCode() { return this.EntityPM ? this.EntityPM.FinalCargoTypeCode : null; }
    set FinalCargoTypeCode(value: string) {
        if (this.EntityPM.FinalCargoTypeCode != value) {
            this.EntityPM.FinalCargoTypeCode = value;
        }
    }

    get FinalManifestNumber() { return this.EntityPM ? this.EntityPM.FinalManifestNumber : null; }
    set FinalManifestNumber(value: string) {
        if (this.EntityPM.FinalManifestNumber != value) {
            this.EntityPM.FinalManifestNumber = value;
        }
    }
    get FinalSecondCargoId() { return this.EntityPM ? this.EntityPM.FinalSecondCargoId : null; }
    set FinalSecondCargoId(value: string) {
        if (this.EntityPM.FinalSecondCargoId != value) {
            this.EntityPM.FinalSecondCargoId = value;
        }
    }
    get FinalThirdCargoId() { return this.EntityPM ? this.EntityPM.FinalThirdCargoId : null; }
    set FinalThirdCargoId(value: string) {
        if (this.EntityPM.FinalThirdCargoId != value) {
            this.EntityPM.FinalThirdCargoId = value;
        }
    }
 

    SendButtonClicked(event) {
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
    }
    OkButtonClicked() {

    }
}


     

