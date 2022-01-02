import { Component, ElementRef } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { NewQuoteDataShareService } from "QuoteOPM/Components/NewEntity/Services/new-quote-data-share/new-quote-data-share.service";
import { NewQuoteDataService } from "QuoteOPM/Components/NewEntity/Services/new-quote-data/new-quote-data.service";
import { NewQuoteHandleLinkedDataService } from "QuoteOPM/Components/NewEntity/Services/new-quote-handle-linked-data/new-quote-handle-linked-data.service";
import { NewQuoteInsertFromEntityService } from "QuoteOPM/Components/NewEntity/Services/new-quote-insert-from-entity/new-quote-insert-from-entity.service";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";

@Component({
    selector: 'QuoteOPDataTabComponent',
    templateUrl: './QuoteOPDataTabComponent.html',
    styleUrls: [
        './QuoteOPDataTabComponent.scss',
        '../../../../QuoteOPM/Components/NewEntity/NewQuoteComponent.scss'
    ],
})

export class QuoteOPDataTabComponent extends BaseComponent {
    public EntityPM: QuoteOPPM;
    formGroup = new FormGroup({});
    public ObjectTableName: string = "QuoteOP";
    public DataContext = this;
    public IsSubjectVisible: boolean = false;
    public isSubmit: boolean = true;


    constructor(
        private entityArgs: EntityArgs,
        private newQuoteDataService: NewQuoteDataService,
        private elmRef: ElementRef,
        private dataShareService: NewQuoteDataShareService,
        private handleLinkedDataService: NewQuoteHandleLinkedDataService,
        private insertFromEntityService: NewQuoteInsertFromEntityService,
    ) {
        super();
    }

    
    ngOnInit() {
        this.EntityPM = this.entityArgs.EntityPM;
        this.EntityPM.DisableMarkAsDirty = true;
        this.insertFromEntityService.EntityPM = this.EntityPM;
        this.setLeftSideData();
        this.dataShareService.EntityPM = this.EntityPM;
        this.dataShareService.newQuoteRef = this.elmRef;
        this.dataShareService.formGroup = this.formGroup;
    }
    
    
    ngAfterViewInit() {        
        this.formGroup.controls.properties.valueChanges.subscribe(()=> {
            this.handleLinkedDataService.addProperty();
            this.handleLinkedDataService.updatePropertiesTable();
        })
        
        this.formGroup.controls.packages.valueChanges.subscribe(()=> {
            this.handleLinkedDataService.attachPackages()
        })
        
        setTimeout(() => this.EntityPM.DisableMarkAsDirty = false, 2000);
        
        SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe(()=> SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM());
    }


    async setLeftSideData() {        
        this.formGroup.addControl('direction', new FormControl(null, Validators.required));
        this.formGroup.addControl('transportMode', new FormControl(null, Validators.required));
        this.formGroup.addControl('shipmentType', new FormControl());

        const ctrls = this.formGroup.controls;
        
        this.setDirection(ctrls);
        this.setTransportMode(ctrls);
        this.setShipmentType(ctrls);        
    }

    private async setShipmentType(ctrls) {
        ctrls.shipmentType.setValue(await this.newQuoteDataService.getShipmentTypeById(this.EntityPM.ShipmentSubTypeId));
    }

    private async setTransportMode(ctrls) {
        ctrls.transportMode.setValue(await this.newQuoteDataService.getTransportModeById(this.EntityPM.TransportModeId));
    }

    private async setDirection(ctrls) {
        ctrls.direction.setValue(await this.newQuoteDataService.getDirectionById(this.EntityPM.DirectionId));
    }
}