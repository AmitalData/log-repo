import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { QuoteOPPackagePM } from 'QuoteOPM/EntityPMs/QuoteOPPackagePM';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';

@Component({
  selector: 'app-new-quote-expected-order',
  templateUrl: './new-quote-expected-order.component.html',
  styleUrls: ['./new-quote-expected-order.component.scss']
})
export class NewQuoteExpectedOrderComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});
  @Input() EntityPM: QuoteOPPM = null as any;

  formsPackage: FormGroup[] = [];
  displayDialog: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('grossWeight')) {
      this.addFormControls();
      this.subscribeCtrl();
    }
  }

  addFormControls() {
    this.formGroup.addControl('grossWeight', new FormControl());
    this.formGroup.addControl('volume', new FormControl());
    this.formGroup.addControl('chargeableWeight', new FormControl());
    this.formGroup.addControl('numberOfPackages', new FormControl());
    this.formGroup.addControl('isDangerous', new FormControl());
    this.formGroup.addControl('descriptionOfGoods', new FormControl());
    this.formGroup.addControl('notes', new FormControl());
  }

  subscribeCtrl() {
    this.formGroup.controls.grossWeight.valueChanges.subscribe(val=> this.EntityPM.GrossWeight = val);
    this.formGroup.controls.volume.valueChanges.subscribe(val=> this.EntityPM.Volume = val);
    this.formGroup.controls.chargeableWeight.valueChanges.subscribe(val=> this.EntityPM.ChargeableWeight = val);
    this.formGroup.controls.numberOfPackages.valueChanges.subscribe(val=> this.EntityPM.NumberOfPackages = val);
    this.formGroup.controls.isDangerous.valueChanges.subscribe(val=> this.EntityPM.IsDangerous = val);
    this.formGroup.controls.descriptionOfGoods.valueChanges.subscribe(val=> this.EntityPM.DescriptionOfGoods = val);
    this.formGroup.controls.notes.valueChanges.subscribe(val=> this.EntityPM.Notes = val);
  }
  
  showDialog() {
    this.displayDialog = true;
  }

  onDilogSave() {
    this.formsPackage.forEach((form: FormGroup)=> {
      const pack:QuoteOPPackagePM = new QuoteOPPackagePM(this.EntityPM);
      const values:any = form.getRawValue();
      
      pack.Quantity = values.quantity;
      pack.Volume = values.volume;
      pack.VolumetricWeight = values.volumetricWeight
      pack.GrossWeight = values.grossWeight;

      this.EntityPM.AddQuoteOPPackage(pack)
    });

    this.displayDialog = false;
    this.formsPackage = [];
  }

  onDilogCancel() {
    this.displayDialog = false;
    this.formsPackage = [];
    this.addNewPackageForm();
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

  removePackage(rowNumber: number) {
    this.formsPackage.splice(rowNumber,1)
  }
}
