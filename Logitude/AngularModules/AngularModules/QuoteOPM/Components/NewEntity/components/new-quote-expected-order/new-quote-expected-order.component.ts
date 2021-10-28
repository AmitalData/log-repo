import { Component, Input, OnInit, SimpleChanges, AfterViewInit } from '@angular/core';
import { FormGroup, FormControl, FormArray } from '@angular/forms';
import { QuoteOPPackagePM } from 'QuoteOPM/EntityPMs/QuoteOPPackagePM';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { pairwise, startWith } from 'rxjs/operators';
import { PackageTypeList } from '../../../../../Common/EntityLists/PackageTypeList';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-expected-order',
  templateUrl: './new-quote-expected-order.component.html',
  styleUrls: ['./new-quote-expected-order.component.scss']
})
export class NewQuoteExpectedOrderComponent implements OnInit,AfterViewInit {
  @Input() formGroup: FormGroup = new FormGroup({});
  @Input() EntityPM: QuoteOPPM = null as any;

  formsPackage: FormGroup[] = [];
    formArray: FormArray = null;
    totalQuantity: number = 0;
    totalGrossWeight: number = 0.00;
    totalVolume: number = 0.00;
    get propForm(): FormGroup {
        return new FormGroup({
            volume: new FormControl(),
            grossWeight: new FormControl(),
            quantity: new FormControl(),
            Ldimension: new FormControl(),
            Wdimension: new FormControl(),
            Hdimension: new FormControl(),
        })
    }
    get packages(): any {
        return this.formGroup.get('packages') as any;
    }
    packageTypes: PackageTypeList[] = []
    constructor(private newQuoteDataService: NewQuoteDataService,) { }

    ngOnInit(): void {
        this.getPackageTypes();
       

  }
  
  ngAfterViewInit():void{
    if (this.EntityPM != null && this.EntityPM.Id != null) {
      if (this.EntityPM.GrossWeight != null) {
        this.formGroup.controls.grossWeight.setValue(this.EntityPM.GrossWeight);
      }
      if (this.EntityPM.Volume != null) {
        this.formGroup.controls.volume.setValue(this.EntityPM.Volume);
      }
      if (this.EntityPM.ChargeableWeight != null) {
        this.formGroup.controls.chargeableWeight.setValue(this.EntityPM.ChargeableWeight);
      }
      if (this.EntityPM.NumberOfPackages != null) {
        this.formGroup.controls.numberOfPackages.setValue(this.EntityPM.NumberOfPackages);
      }
      if (this.EntityPM.IsDangerous != null) {
        this.formGroup.controls.isDangerous.setValue(this.EntityPM.IsDangerous);
      }
      if (this.EntityPM.DescriptionOfGoods != null) {
        this.formGroup.controls.descriptionOfGoods.setValue(this.EntityPM.DescriptionOfGoods);
      }
      if (this.EntityPM.Notes != null) {
        this.formGroup.controls.notes.setValue(this.EntityPM.Notes);
      }
    }
  }


    async getPackageTypes() {
        debugger;
        this.packageTypes = await this.newQuoteDataService.getPackageTypeTable();
    }
  ngOnChanges(changes: SimpleChanges) {
      if (!this.formGroup.contains('descriptionOfGoods')) {
      this.addFormControls();
      this.subscribeCtrl();
    }
  }
    get isExportSeaFcl(): boolean {
        return this.formGroup.controls.direction?.value?.Id === "E" &&
            this.formGroup.controls.transportMode?.value?.Id === 'O' &&
            this.formGroup.controls.shipmentType?.value?.Name === 'FCL'
    }
    addFormControls() {
        debugger;
        
            this.formGroup.addControl('quantity1', new FormControl());
            this.formGroup.addControl('quantity2', new FormControl());
            this.formGroup.addControl('quantity3', new FormControl());
            this.formGroup.addControl('quantity4', new FormControl());
            this.formGroup.addControl('quantityType1', new FormControl());
            this.formGroup.addControl('quantityType2', new FormControl());
            this.formGroup.addControl('quantityType3', new FormControl());
            this.formGroup.addControl('quantityType4', new FormControl());
        var form = this.propForm;
        form.controls.volume.disable();
        form.controls.grossWeight.disable();
        form.controls.Ldimension.disable();
        form.controls.Wdimension.disable();
        form.controls.Hdimension.disable();
        form.controls.quantity.valueChanges.pipe(startWith(null), pairwise()).subscribe(([prev, next]: [any, any]) => { this.disableForm(form); this.calcTotalQuantity(prev, next) });
        form.controls.grossWeight.valueChanges.pipe(startWith(null), pairwise()).subscribe(([prev, next]: [any, any]) => { this.calcTotalGrossWeight(prev, next) });
        form.controls.volume.valueChanges.pipe(startWith(null), pairwise()).subscribe(([prev, next]: [any, any]) => { this.calcTotalVolume(prev, next) });
        
        //form.controls.quantity.valueChanges.subscribe(val => { this.disableForm(form); this.calcTotalQuantity(val) })

        this.formArray = new FormArray([form]);
            this.formGroup.addControl('packages', this.formArray)
/*this.formGroup.addControl('grossWeight', new FormControl());
    this.formGroup.addControl('volume', new FormControl());
    this.formGroup.addControl('chargeableWeight', new FormControl());
    this.formGroup.addControl('numberOfPackages', new FormControl());*/
        
      
    this.formGroup.addControl('isDangerous', new FormControl());
    this.formGroup.addControl('descriptionOfGoods', new FormControl());
    this.formGroup.addControl('notes', new FormControl());
  }

    subscribeCtrl() {
        if (this.isExportSeaFcl)
        {
            this.formGroup.controls.quantity1.valueChanges.subscribe(val => this.EntityPM.PackageType1Quantity = val);
            this.formGroup.controls.quantity2.valueChanges.subscribe(val => this.EntityPM.PackageType2Quantity = val);
            this.formGroup.controls.quantity3.valueChanges.subscribe(val => this.EntityPM.PackageType3Quantity = val);
            this.formGroup.controls.quantity4.valueChanges.subscribe(val => this.EntityPM.PackageType4Quantity = val);
            this.formGroup.controls.quantityType1.valueChanges.subscribe(val => this.EntityPM.PackageType1Id = val);
            this.formGroup.controls.quantityType2.valueChanges.subscribe(val => this.EntityPM.PackageType2Id = val);
            this.formGroup.controls.quantityType3.valueChanges.subscribe(val => this.EntityPM.PackageType3Id = val);
            this.formGroup.controls.quantityType4.valueChanges.subscribe(val => this.EntityPM.PackageType4Id = val);
        }
        else
        {
            //this.formGroup.controls.packages.valueChanges.subscribe(val => this.EntityPM.QuotePackages = val);

            //this.attachPackages();
            /*this.formGroup.controls.grossWeight.valueChanges.subscribe(val=> this.EntityPM.GrossWeight = val);
            this.formGroup.controls.volume.valueChanges.subscribe(val=> this.EntityPM.Volume = val);
            this.formGroup.controls.chargeableWeight.valueChanges.subscribe(val=> this.EntityPM.ChargeableWeight = val);
            this.formGroup.controls.numberOfPackages.valueChanges.subscribe(val=> this.EntityPM.NumberOfPackages = val);*/
        }
   
    this.formGroup.controls.isDangerous.valueChanges.subscribe(val=> this.EntityPM.IsDangerous = val);
    this.formGroup.controls.descriptionOfGoods.valueChanges.subscribe(val=> this.EntityPM.DescriptionOfGoods = val);
    this.formGroup.controls.notes.valueChanges.subscribe(val=> this.EntityPM.Notes = val);
  }
  
    addPackage() {
        var form = this.propForm;
        form.controls.volume.disable();
        form.controls.grossWeight.disable();
        form.controls.Ldimension.disable();
        form.controls.Wdimension.disable();
        form.controls.Hdimension.disable();
        //form.controls.quantity.valueChanges.subscribe(val => this.disableForm(form))
        form.controls.quantity.valueChanges.pipe(startWith(null), pairwise()).subscribe(([prev, next]: [any, any]) => { this.disableForm(form); this.calcTotalQuantity(prev, next) });
        form.controls.grossWeight.valueChanges.pipe(startWith(null), pairwise()).subscribe(([prev, next]: [any, any]) => { this.calcTotalGrossWeight(prev, next) });
        form.controls.volume.valueChanges.pipe(startWith(null), pairwise()).subscribe(([prev, next]: [any, any]) => { this.calcTotalVolume(prev, next) });

        this.formArray.push(form)
    }
    removePackage( index: number ) {
        if (this.formArray.controls.length > 1) {
            debugger;
            var remove_quantity = (this.formArray.at(index) as FormGroup).controls.quantity.value;
            this.calcTotalQuantity(remove_quantity, 0);
            var remove_grossWeight = (this.formArray.at(index) as FormGroup).controls.grossWeight.value;
            this.calcTotalGrossWeight(remove_grossWeight, 0);
            var remove_Volume = (this.formArray.at(index) as FormGroup).controls.volume.value;
            this.calcTotalVolume(remove_Volume, 0);

            this.formArray.removeAt(index);
        }
    }

    attachPackages() {
        this.formGroup.controls.packages.value.forEach((form: FormGroup) => {
      const pack:QuoteOPPackagePM = new QuoteOPPackagePM(this.EntityPM);
      const values:any = form.getRawValue();
      
      pack.Quantity = values.quantity;
      pack.Volume = values.volume;
      pack.GrossWeight = values.grossWeight;

      this.EntityPM.AddQuoteOPPackage(pack)
    });

    
  }

  

  addNewPackageForm() {
    let form: FormGroup = new FormGroup({});
    form = new FormGroup({
      quantity: new FormControl(),
      l: new FormControl(),
      w: new FormControl(),
      h: new FormControl(),
      volume: new FormControl(),
      volumetricWeight: new FormControl(),
      grossWeight: new FormControl(),
    })
    form.controls.volumetricWeight.disable();

    this.formsPackage.push(form);

    form.controls.l.valueChanges.subscribe(val => this.disablevolumeForm(form))
    form.controls.w.valueChanges.subscribe(val => this.disablevolumeForm(form))
    form.controls.h.valueChanges.subscribe(val => this.disablevolumeForm(form))

    form.controls.volume.valueChanges.subscribe(val => {
      if (val && form.controls.l.enabled) {
        form.controls.l.disable();
        form.controls.w.disable();
        form.controls.h.disable();
      } else if (form.controls.l.disabled){
        form.controls.l.enable();
        form.controls.w.enable();
        form.controls.h.enable();
      }
    })
  }

  disablevolumeForm(form: FormGroup) {  
    if (form.controls.volume.enabled && (form.controls.l.value || form.controls.w.value || form.controls.l.value))
      form.controls.volume.disable();
    else if (form.controls.volume.disabled)
      form.controls.volume.enable();
  }
    disableForm(form: FormGroup) {
        if (form.controls.quantity.value) {
            form.controls.volume.enable();
            form.controls.grossWeight.enable();
            form.controls.Ldimension.enable();
            form.controls.Wdimension.enable();
            form.controls.Hdimension.enable();
           
        }
        else {
            form.controls.volume.disable();
            form.controls.grossWeight.disable();
            form.controls.Ldimension.disable();
            form.controls.Wdimension.disable();
            form.controls.Hdimension.disable();
        }
    }
    calcTotalQuantity(prev_quantity: number, current_quantity: number) {
        this.totalQuantity = this.totalQuantity - prev_quantity + current_quantity;
    }
    
    calcTotalGrossWeight(prev_quantity: number, current_quantity: number) {
        this.totalGrossWeight = this.totalGrossWeight - prev_quantity + current_quantity;
    }
    
    calcTotalVolume(prev_quantity: number, current_quantity: number) {
        this.totalVolume = this.totalVolume - prev_quantity + current_quantity;
    }
}
