import { Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../../../Infrastructure/Utilities/LocationDirective';
import { AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';

import { ProceduralFaultPMService } from '../../../../../Customs/Services/StandardPMs/ProceduralFaultPMService'
import { ProceduralFaultPM } from '../../../../../Customs/EntityPMs/ProceduralFaultPM';

@Component({
    moduleId: module.id,
    templateUrl: './ProceduralFaultsGeneralTabComponent.html',
    providers: [EntityArgs],
}) 

export class ProceduralFaultsGeneralTabComponent extends BaseComponent {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public EntityPM: ProceduralFaultPM;
    public ObjectTableName: string = "Customs.ProceduralFault";
    public DataContext: any = this;
    public IsNewEntity: boolean = false;
    public ValidationErrorsList: any[];

    public entityResourceService: EntityResourceService = new EntityResourceService();
    private proceduralFaultPMService: ProceduralFaultPMService = new ProceduralFaultPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.CurrentEntity;
            this.IsNewEntity = args.IsNewEntity;
        }

        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.ProceduralFault";
    }


    //#region Properties
    get DeclarationNumber() { return this.EntityPM ? this.EntityPM.DeclarationNumber : null; }
    set DeclarationNumber(value: string) {
        if (this.EntityPM.DeclarationNumber != value) {
            this.EntityPM.DeclarationNumber = value;
        }
    }

    get InspectionTypeName() { return this.EntityPM ? this.EntityPM.InspectionTypeName : null; }
    set InspectionTypeName(value: string) {
        if (this.EntityPM.InspectionTypeName != value) {
            this.EntityPM.InspectionTypeName = value;
        }
    }

    get InputTypeCode() { return this.EntityPM ? this.EntityPM.InputTypeCode : null; }
    set InputTypeCode(value: string) {
        if (this.EntityPM.InputTypeCode != value) {
            this.EntityPM.InputTypeCode = value;
        }
    }

    get InputTypeName() { return this.EntityPM ? this.EntityPM.InputTypeName : null; }
    set InputTypeName(value: string) {
        if (this.EntityPM.InputTypeName != value) {
            this.EntityPM.InputTypeName = value;
        }
    }

    get IsCustProceduralFaultCountabl() { return this.EntityPM ? this.EntityPM.IsCustProceduralFaultCountabl : null; }
    set IsCustProceduralFaultCountabl(value: boolean) {
        if (this.EntityPM.IsCustProceduralFaultCountabl != value) {
            this.EntityPM.IsCustProceduralFaultCountabl = value;
        }
    }

    get Remarks() { return this.EntityPM ? this.EntityPM.Remarks : null; }
    set Remarks(value: string) {
        if (this.EntityPM.Remarks != value) {
            this.EntityPM.Remarks = value;
        }
    }

    get RansomViolationSum() { return this.EntityPM ? this.EntityPM.RansomViolationSum : null; }
    set RansomViolationSum(value: number) {
        if (this.EntityPM.RansomViolationSum != value) {
            this.EntityPM.RansomViolationSum = value;
        }
    }

    get IsAgentResponsibility() { return this.EntityPM ? this.EntityPM.IsAgentResponsibility : null; }
    set IsAgentResponsibility(value: boolean) {
        if (this.EntityPM.IsAgentResponsibility != value) {
            this.EntityPM.IsAgentResponsibility = value;
        }
    }

    get UpdateDate() { return this.EntityPM ? this.EntityPM.UpdateDate : null; }
    set UpdateDate(value: Date) {
        if (this.EntityPM.UpdateDate != value) {
            this.EntityPM.UpdateDate = value;
        }
    }

    get LeadingDocumentVersion() { return this.EntityPM ? this.EntityPM.LeadingDocumentVersion : null; }
    set LeadingDocumentVersion(value: string) {
        if (this.EntityPM.LeadingDocumentVersion != value) {
            this.EntityPM.LeadingDocumentVersion = value;
        }
    }

    get Notes() { return this.EntityPM ? this.EntityPM.Notes : null; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }
    //#endregion


    OkButtonClicked() {
        this.proceduralFaultPMService.update(this.EntityPM).subscribe(response => {
            var result = response.Result;
            this.CurrentSession.CloseCurrentWindow();

        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    }
}
