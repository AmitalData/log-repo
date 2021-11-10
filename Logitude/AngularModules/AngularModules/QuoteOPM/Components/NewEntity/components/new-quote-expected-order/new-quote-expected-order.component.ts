import { Component, Input, OnInit, SimpleChanges, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { TransportModeList } from 'Infrastructure/EntityLists/TransportModeList';
import { requiredOneFromMultiValidator } from 'Infrastructure/Validators/requiredOneFromMultiValidator';
import { MessageService } from 'primeng/api';
import { QuoteOPPackagePM } from 'QuoteOPM/EntityPMs/QuoteOPPackagePM';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { filter, pairwise, startWith } from 'rxjs/operators';
import { ShipmentTypeList } from 'Shipment/EntityLists/ShipmentTypeList';
import { PackageTypeList } from '../../../../../Common/EntityLists/PackageTypeList';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-expected-order',
  templateUrl: './new-quote-expected-order.component.html',
  styleUrls: ['./new-quote-expected-order.component.scss']
})
export class NewQuoteExpectedOrderComponent implements OnInit, AfterViewInit {
  @Input() formGroup: FormGroup = new FormGroup({});
  @Input() EntityPM: QuoteOPPM = null as any;

  formsPackage: FormGroup[] = [];
  formArray: FormArray = null;
  totalQuantity: number = 0;
  totalGrossWeight: number = 0.00;
  totalVolume: number = 0.00;
  isSeaFcl: boolean = true;

  get propForm(): FormGroup {
    const a: any = { b: null };
    a.b = new FormGroup({
      volume: new FormControl(null, requiredOneFromMultiValidator(a, 'b', 'volume', 'grossWeight')),
      grossWeight: new FormControl(null, requiredOneFromMultiValidator(a, 'b', 'grossWeight', 'volume')),
      packageType: new FormControl(),
      Ldimension: new FormControl(),
      Wdimension: new FormControl(),
      Hdimension: new FormControl(),
      quantity: new FormControl(),
    })
    return a.b
  }
  get packages(): any {
    return this.formGroup.get('packages') as any;
  }
  packageTypes: PackageTypeList[] = []
  constructor(
    private newQuoteDataService: NewQuoteDataService,
    private msg: MessageService,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.getPackageTypes();
    this.onTransportAndShipmentChange();
  }

  ngAfterViewInit(): void {
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
    this.packageTypes = await this.newQuoteDataService.getPackageTypeTable();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('descriptionOfGoods')) {
      this.addFormControls();
      this.subscribeCtrl();
    }
  }

  onTransportAndShipmentChange(): void {
    this.isSeaFcl = this.formGroup.controls.transportMode?.value?.Id === 'O' && this.formGroup.controls.shipmentType?.value?.Name === 'FCL';

    ['quantity1', 'quantityType1'].forEach(ctrl => {
      this.formGroup.controls[ctrl].setValidators(this.isSeaFcl ? Validators.required : null)
      this.formGroup.controls[ctrl].updateValueAndValidity()
    })

    this.isSeaFcl ? this.formArray.disable() : this.formArray.enable();
  }

  addFormControls() {
    this.formGroup.addControl('quantity1', new FormControl());
    this.formGroup.addControl('quantity2', new FormControl());
    this.formGroup.addControl('quantity3', new FormControl());
    this.formGroup.addControl('quantity4', new FormControl());
    this.formGroup.addControl('quantityType1', new FormControl());
    this.formGroup.addControl('quantityType2', new FormControl({value: null, disabled: true}));
    this.formGroup.addControl('quantityType3', new FormControl({value: null, disabled: true}));
    this.formGroup.addControl('quantityType4', new FormControl({value: null, disabled: true}));

    this.formArray = new FormArray([]);
    this.addPackage()

    this.formGroup.addControl('packages', this.formArray);
    this.formGroup.addControl('isDangerous', new FormControl());
    this.formGroup.addControl('descriptionOfGoods', new FormControl());
    this.formGroup.addControl('notes', new FormControl());
  }

  subscribeCtrl() {
    this.formGroup.controls.quantity1.valueChanges.subscribe(val => this.EntityPM.PackageType1Quantity = val);
    this.formGroup.controls.quantity2.valueChanges.subscribe(val => this.EntityPM.PackageType2Quantity = val);
    this.formGroup.controls.quantity3.valueChanges.subscribe(val => this.EntityPM.PackageType3Quantity = val);
    this.formGroup.controls.quantity4.valueChanges.subscribe(val => this.EntityPM.PackageType4Quantity = val);
    this.formGroup.controls.quantityType1.valueChanges.subscribe(val => this.EntityPM.PackageType1Id = val);
    this.formGroup.controls.quantityType2.valueChanges.subscribe(val => this.EntityPM.PackageType2Id = val);
    this.formGroup.controls.quantityType3.valueChanges.subscribe(val => this.EntityPM.PackageType3Id = val);
    this.formGroup.controls.quantityType4.valueChanges.subscribe(val => this.EntityPM.PackageType4Id = val);

    this.formGroup.controls.isDangerous.valueChanges.subscribe(val => this.EntityPM.IsDangerous = val);
    this.formGroup.controls.descriptionOfGoods.valueChanges.subscribe(val => this.EntityPM.DescriptionOfGoods = val);
    this.formGroup.controls.notes.valueChanges.subscribe(val => this.EntityPM.Notes = val);

    this.formGroup.controls.transportMode.valueChanges.subscribe((val: TransportModeList) => this.onTransportAndShipmentChange());
    this.formGroup.controls.shipmentType.valueChanges.subscribe((val: ShipmentTypeList) => this.onTransportAndShipmentChange());

    [2, 3, 4].forEach(i => this.formGroup.controls['quantity' + i].valueChanges.subscribe(val => {
      this.formGroup.controls['quantityType' + i][val ? 'enable' : 'disable']()
      this.formGroup.controls['quantityType' + i].setValidators(val ? Validators.required : null)
    }));
  }

  addPackage() {
    if (this.formArray.invalid) {
      this.msg.add({ severity: 'error', summary: 'Add new package failed', detail: 'some packeges not have "Gross Weight" or "volume"' })
      return;
    }

    const form = this.propForm;
    form.controls.quantity.valueChanges.pipe(startWith(null as string), pairwise()).subscribe(([prev, next]: [any, any]) => {/*  this.disablePackageType(form); */ this.disableDimensionsForm(form); this.calcTotalQuantity(prev, next) });
    form.controls.grossWeight.valueChanges.pipe(startWith(null as string), pairwise()).subscribe(([prev, next]: [any, any]) => { this.calcTotalGrossWeight(prev, next) });
    form.controls.volume.valueChanges.pipe(startWith(null as string), pairwise()).subscribe(([prev, next]: [any, any]) => { if (prev != next) { this.disableDimensionsForm(form); this.calcTotalVolume(prev, next) } });
    form.controls.Ldimension.valueChanges.pipe(startWith(null as string), pairwise(), filter(([prev, next]: [any, any]) => prev != next)).subscribe(() => { this.disablevolumeForm(form); this.calcVolume(form) });
    form.controls.Wdimension.valueChanges.pipe(startWith(null as string), pairwise(), filter(([prev, next]: [any, any]) => prev != next)).subscribe(() => { this.disablevolumeForm(form); this.calcVolume(form) });
    form.controls.Hdimension.valueChanges.pipe(startWith(null as string), pairwise(), filter(([prev, next]: [any, any]) => prev != next)).subscribe(() => { this.disablevolumeForm(form); this.calcVolume(form) });
    this.formArray.push(form)
  }

  removePackage(index: number) {
    if (this.formArray.controls.length > 1) {
      var remove_quantity = (this.formArray.at(index) as FormGroup).controls.quantity.value;
      this.calcTotalQuantity(remove_quantity, 0);
      var remove_grossWeight = (this.formArray.at(index) as FormGroup).controls.grossWeight.value;
      this.calcTotalGrossWeight(remove_grossWeight, 0);
      var remove_Volume = (this.formArray.at(index) as FormGroup).controls.volume.value;
      this.calcTotalVolume(remove_Volume, 0);

      this.formArray.removeAt(index);
    }
  }

  calcVolume(form: FormGroup) {
    const val = form.controls.Ldimension.value * form.controls.Wdimension.value * form.controls.Hdimension.value;

    if (val)
      form.controls.volume.setValue(val)
  }

  disableDimensionsForm(form: FormGroup) {
    if (!form.controls.quantity.value || (form.controls.volume.value && !form.controls.Ldimension.value)) {
      form.controls.Ldimension.disable();
      form.controls.Wdimension.disable();
      form.controls.Hdimension.disable();
    } else {
      form.controls.Ldimension.enable();
      form.controls.Wdimension.enable();
      form.controls.Hdimension.enable();
    }
  }

  disablevolumeForm(form: FormGroup) {
    if (form.controls.Ldimension.value || form.controls.Wdimension.value || form.controls.Hdimension.value)
      form.controls.volume.disable();
    else
      form.controls.volume.enable();
  }

  // disablePackageType(form: FormGroup) {
  //   if (form.controls.quantity.value)
  //     form.controls.packageType.enable();
  //   else
  //     form.controls.packageType.disable();
  // }

  calcTotalQuantity(prev_quantity: number, current_quantity: number) {
    this.totalQuantity = this.totalQuantity - prev_quantity + current_quantity;
  }

  calcTotalGrossWeight(prev_quantity: number, current_quantity: number) {
    this.totalGrossWeight = this.totalGrossWeight - prev_quantity + current_quantity;
  }

  calcTotalVolume(prev_quantity: number, current_quantity: number) {
    this.totalVolume = this.totalVolume - prev_quantity + current_quantity;
  }

  attachPackages() {
    this.formGroup.controls.packages.value.forEach((form: FormGroup) => {
      const pack: QuoteOPPackagePM = new QuoteOPPackagePM(this.EntityPM);
      const values: any = form.getRawValue();

      pack.Quantity = values.quantity;
      pack.Volume = values.volume;
      pack.GrossWeight = values.grossWeight;
      pack.PackageTypeId = (<PackageTypeList>values.packageType).Id;

      this.EntityPM.AddQuoteOPPackage(pack)
    });
  }
}
