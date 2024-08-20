import { Component, OnInit } from '@angular/core';
import { ExportStoragePM } from 'Customs/EntityPMs/ExportStoragePM';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    selector: 'app-edit-export-storage',
    templateUrl: './ExportStorageGeneralTabComponent.html',
    styleUrls: ['./ExportStorageGeneralTabComponent.scss'],
})
export class ExportStorageGeneralTabComponent extends BaseComponent {
    public DataContext: ExportStorageGeneralTabComponent = this;
    EntityPM: ExportStoragePM = null as any;
    public ObjectTableName: string = '';

    constructor(private entityArgs: EntityArgs) {
        super();
    }

    ngOnInit(): void {
        this.initEntity();

        this.disabledInputs();
    }

    initEntity() {
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
    }

    disabledInputs() {
        [
            'ShipType',
            'CargoTypeCodeName',
            'CargoTypeName',
            'SecondCargoID',
            'ExporterCode',
            'FirstCargoID',
            'ThirdCargoID',
            'ExportDealIdentification',
            'ShipName',
            'PackageQuantity',
            'GrossMassMeasure',
            'MarksNumbers',
            'ExportLoadingPortcode',
            'StorageSiteCode',
            'ExportUnloadingPortCode',
            'FinalDestinationPortCode',
            'IsDangerousGoods',
            'ContainerTypeWCO',
        ].forEach((fieldName) =>
            this.UIProperties.SetEnabled(fieldName, this.ObjectTableName, false)
        );
    }

    public get ShipType() {
        return this.EntityPM.DeclarationId
            ? TextCodeTranslator.Translate('Customs.General.O.Export')
            : '';
    }
    public get CargoTypeCodeName() {
        return this.EntityPM.CargoTypeCodeName;
    }
    public get CargoTypeName() {
        return this.EntityPM.CargoTypeName;
    }
    public get SecondCargoID() {
        return this.EntityPM.SecondCargoID;
    }
    public get ExporterID() {
        return this.EntityPM.ExporterID;
    }
    public get FirstCargoID() {
        return this.EntityPM.FirstCargoID;
    }
    public get ThirdCargoID() {
        return this.EntityPM.ThirdCargoID;
    }
    public get ExportDealIdentification() {
        return this.EntityPM.ExportDealIdentification;
    }
    public get ExporterCode() {
        return this.EntityPM.ExporterCode;
    }

    public get ShipCode() {
        return this.EntityPM.ShipCode;
    }
    public get ShipName() {
        return this.EntityPM.ShipName;
    }
    public get PackageQuantity() {
        return this.EntityPM.PackageQuantity;
    }

    public get GrossMassMeasure() {
        return this.EntityPM.GrossMassMeasure;
    }

    public get MarksNumbers() {
        return this.EntityPM.MarksNumbers;
    }

    public get ExportLoadingPortcode() {
        return this.EntityPM.ExportLoadingPortcode;
    }

    public get StorageSiteCode() {
        return this.EntityPM.StorageSiteCode;
    }

    public get ExportUnloadingPortCode() {
        return this.EntityPM.ExportUnloadingPortCode;
    }

    public get FinalDestinationPortCode() {
        return this.EntityPM.FinalDestinationPortCode;
    }

    public get IsDangerousGoods() {
        return this.EntityPM.IsDangerousGoods === undefined
            ? false
            : this.EntityPM.IsDangerousGoods;
    }

    public get ContainerTypeWCO() {
        return this.EntityPM.ContainerTypeWCO;
    }
}
