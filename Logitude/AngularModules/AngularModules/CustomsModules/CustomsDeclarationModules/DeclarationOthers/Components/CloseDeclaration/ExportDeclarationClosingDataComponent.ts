import { OnInit, Component } from '@angular/core';
import { ExportDeclarationClosingDataPM } from '../../../../../Customs/EntityPMs/ExportDeclarationClosingDataPM';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ExportDeclarationClosingDataPMService } from '../../../../../Customs/Services/StandardPMs/ExportDeclarationClosingDataPMService';
import { Time } from '@angular/common';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';

@Component({
    selector: 'ExportDeclarationClosingDataComponent',
    templateUrl: './ExportDeclarationClosingDataComponent.html',
})

export class ExportDeclarationClosingDataComponent extends BaseComponent {
    public DataContext: any = this;
    public EntityPM: ExportDeclarationClosingDataPM;
    public DecPM: DeclarationPM;
    public ConPM: ConsignmentPM;
    public SendButtonEnabled: boolean = false;
    public ObjectTableName: string = "Customs.ExportDeclarationClosingData";
    public IsReady: boolean = false;
    exportDeclarationClosingDataPMService: ExportDeclarationClosingDataPMService = new ExportDeclarationClosingDataPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    public IsNew: boolean = false;

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

    GetExportDeclarationClosingData(id: string) {
        if (id != null) {
            this.exportDeclarationClosingDataPMService.get(id).subscribe((response: any) => {
                this.EntityPM = response.Result;
                if (this.EntityPM == null) {
                    this.EntityPM = new ExportDeclarationClosingDataPM();
                    this.EntityPM.DeclarationId = id;
                    this.EntityPM.Tenant = this.DecPM.Tenant;
                    var consignments = this.DecPM.Consignments.filter(x => x.ConsignmentType == 'E');
                    if (consignments.length == 1) {
                        this.EntityPM.FinalCargoTypeCode = consignments[0].CargoTypeCode;
                        this.EntityPM.FinalSecondCargoId = consignments[0].SecondCargoID;
                        this.EntityPM.FinalThirdCargoId = consignments[0].ThirdCargoID;
                        this.EntityPM.FinalManifestNumber = consignments[0].ManifestNumber;
                        this.EntityPM.FinalShipCode = consignments[0].ShipCode;
                        this.EntityPM.FinalLoadingSite = consignments[0].ExportLoadingPortCode;
                    }
                    this.IsNew = true;
                } 
                this.IsReady = true;
                this.ConPM = this.DecPM.Consignments[0];
            }); 
        }
    }


    get LoadingDateTime() { return this.EntityPM ? this.EntityPM.LoadingDateTime : null; }
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
        if (this.IsNew) {
            this.exportDeclarationClosingDataPMService.insert(this.EntityPM).subscribe((response: ServiceResponse) => {
                SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
            });
        } else {
            this.exportDeclarationClosingDataPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
            });
        }
    }
}




