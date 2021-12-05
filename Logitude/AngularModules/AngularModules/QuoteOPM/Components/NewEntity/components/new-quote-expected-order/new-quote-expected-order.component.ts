import { Component, Input, OnInit, SimpleChanges, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { TenantList } from 'Common/EntityLists/TenantList';
import { TransportModeList } from 'Infrastructure/EntityLists/TransportModeList';
import { requiredOneFromMultiValidator } from 'Infrastructure/Validators/requiredOneFromMultiValidator';
import { MessageService } from 'primeng/api';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { filter, pairwise, startWith } from 'rxjs/operators';
import { ShipmentTypeList } from 'Shipment/EntityLists/ShipmentTypeList';
import { PackageTypeList } from '../../../../../Common/EntityLists/PackageTypeList';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';
import { NewQuoteUnitsService } from '../../Services/new-quote-units/new-quote-units.service';

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
  billableWeight: number = 0.0;
  isSeaFcl: boolean = true;
  tenant: TenantList = null as any;

  get propForm(): FormGroup {
    const a: any = { b: null };
    a.b = new FormGroup({
      volume: new FormControl(null, requiredOneFromMultiValidator(a, 'b', 'volume', 'grossWeight')),
      grossWeight: new FormControl(null, requiredOneFromMultiValidator(a, 'b', 'grossWeight', 'volume')),
      packageType: new FormControl(),
      Ldimension: new FormControl({ value: null, disabled: true }),
      Wdimension: new FormControl({ value: null, disabled: true }),
      Hdimension: new FormControl({ value: null, disabled: true }),
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
    private unitsService: NewQuoteUnitsService,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.initTenantsData();
    this.getPackageTypes();
    this.onTransportAndShipmentChange();

    this.resetForm();
  }

  async initTenantsData() {
    this.tenant = await this.newQuoteDataService.getTenantsData();
    console.log(this.tenant)
  }

  resetForm() {
    this.newQuoteDataService.$resetForm.subscribe(() => {
      this.totalQuantity = this.totalGrossWeight = this.totalVolume = this.billableWeight = 0;
      this.formArray.clear();
      this.addPackage();

      ['quantity', 'quantityType'].forEach(ctrl =>
        [1, 2, 3, 4].forEach(i => this.formGroup.controls[ctrl + i].reset()))
    })
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
    this.formGroup.addControl('quantityType2', new FormControl({ value: null, disabled: true }));
    this.formGroup.addControl('quantityType3', new FormControl({ value: null, disabled: true }));
    this.formGroup.addControl('quantityType4', new FormControl({ value: null, disabled: true }));

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
    this.formGroup.controls.quantityType1.valueChanges.subscribe((val: PackageTypeList) => this.EntityPM.PackageType1Id = val?.Id);
    this.formGroup.controls.quantityType2.valueChanges.subscribe((val: PackageTypeList) => this.EntityPM.PackageType2Id = val?.Id);
    this.formGroup.controls.quantityType3.valueChanges.subscribe((val: PackageTypeList) => this.EntityPM.PackageType3Id = val?.Id);
    this.formGroup.controls.quantityType4.valueChanges.subscribe((val: PackageTypeList) => this.EntityPM.PackageType4Id = val?.Id);

    this.formGroup.controls.isDangerous.valueChanges.subscribe(val => this.EntityPM.IsDangerous = val);
    this.formGroup.controls.descriptionOfGoods.valueChanges.subscribe(val => this.EntityPM.DescriptionOfGoods = val);
    this.formGroup.controls.notes.valueChanges.subscribe(val => this.EntityPM.Notes = val);

    this.formGroup.controls.transportMode.valueChanges.subscribe((val: TransportModeList) => this.onTransportAndShipmentChange());
    this.formGroup.controls.shipmentType.valueChanges.subscribe((val: ShipmentTypeList) => this.onTransportAndShipmentChange());

    [1, 2, 3, 4].forEach(i => this.formGroup.controls['quantity' + i].valueChanges.subscribe(val => {
      this.formGroup.controls['quantityType' + i].reset();
      this.formGroup.controls['quantityType' + i][val ? 'enable' : 'disable']()
      this.formGroup.controls['quantityType' + i].setValidators(val ? Validators.required : null)
      this.formGroup.controls['quantityType' + i].updateValueAndValidity();
    }));
  }

  addPackage() {
    if (this.formArray.invalid) {
      let msg: string = 'Volume or Gross Weight fields required';

      if (this.formArray.controls.some(ctrl => (<FormGroup>ctrl).controls.packageType.errors.notIdentityValue))
        msg = 'value in package type not exist';
      else if (this.formArray.length === 1)
        msg = 'Please insert data to the first package';

      this.msg.add({ severity: 'error', summary: 'Add new package failed', detail: msg })
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
    const vu = this.unitsService.getVolumeUnit(this.tenant.VolumeUnitCode);
    const du = this.unitsService.getDimensionsUnit(this.tenant.DimensionsUnitCode);

    const val = form.controls.Ldimension.value * form.controls.Wdimension.value * form.controls.Hdimension.value * du ** 3 / (vu * 1000000);

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
    this.EntityPM.NumberOfPackages = this.totalQuantity;
  }

  calcTotalGrossWeight(prev_quantity: number, current_quantity: number) {

    this.totalGrossWeight = this.totalGrossWeight - prev_quantity + current_quantity;
    this.EntityPM.GrossWeight = this.totalGrossWeight;
    this.calcChargeableWeight()
  }

  calcTotalVolume(prev_quantity: number, current_quantity: number) {
    this.totalVolume = this.totalVolume - prev_quantity + current_quantity;
    this.EntityPM.Volume = this.totalVolume;
    this.calcChargeableWeight()
  }

  calcChargeableWeight() {
    const factor = this.formGroup.value.transportMode?.Id === 'A' ? 1000 / 6 : 1000
    const gu: number = this.unitsService.getWeightUnit(this.tenant?.GrossWeightUnitCode, 'GrossWeightUnitCode');
    const vu: number = this.unitsService.getVolumeUnit(this.tenant?.VolumeUnitCode);
    const bu: number = this.formGroup.value.transportMode?.Id === 'A' ?
      this.unitsService.getWeightUnit(this.tenant?.ChargeableWeightUnitCode, 'ChargeableWeightUnitCode') :
      this.unitsService.getWeightUnit(this.tenant?.WeightMeasurementUnitCode, 'WeightMeasurementUnitCode');

    let bulk: number = 0;


    this.formArray.controls.forEach((propertyForm: FormGroup, i: number) => {
      let volume: number = propertyForm.controls.volume.value / vu * factor / bu;
      let grossWeight: number = propertyForm.controls.grossWeight.value * gu / bu;

      if (!volume && grossWeight)
        bulk += grossWeight;
      else if (volume) {
        bulk += grossWeight > volume ? grossWeight : volume;
      }
    });

    this.EntityPM.ChargeableWeight = this.billableWeight = bulk;
  }
}
