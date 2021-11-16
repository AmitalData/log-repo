import { Injectable } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { MessageService } from 'primeng/api';

@Injectable()
export class NewQuoteValidateEntityService {
  errorList: string[] = [];
  form: FormGroup = null as any;
  fieldrequiredMsg: string = ' field is required'

  get formValue(): any {
    return this.form.value;
  }

  constructor(
    private messageService: MessageService,
  ) { }

  validate(form: FormGroup): boolean {
    this.errorList = [];
    this.form = form;

    this.checkPartner();
    this.checkProperties();
    this.checkGeneral();
    this.checkExpectedOrder();
    // this.checkValidator(this.form)

    this.showErrorMessage();

    return this.errorList.length === 0;
  }

  private showErrorMessage(): void {
    if (this.errorList.length > 0)
      this.messageService.add({ severity: 'error', summary: 'Create new quote failed.', detail: this.errorList.join('\n'), life: 30 * 1000 });
  }

  private checkPartner(): void {
    // must set shipper or consignee

    const shipperForm: any = this.form.controls.shipper.value.partner;
    const consigneeForm: any = this.form.controls.consignee.value.partner;

    if (!shipperForm && !consigneeForm)
      this.errorList.push('Shipperr or Consignee is requierd.')
  }

  private checkProperties(): void {
    const propertiesForms: AbstractControl[] = (this.form.controls.properties as FormArray).controls.filter((propertyForm: FormGroup) => propertyForm.invalid);
    propertiesForms.forEach((propertyFormGroup: FormGroup, i: number) => {
      const propertyForm: FormGroup["controls"] = propertyFormGroup.controls;
      const propertyPosition: string = propertiesForms.length > 1 ? ' in property ' + (i + 1) : ''

      if (propertyForm.fromPort.invalid) {
        const fieldName: string = TextCodeTranslator.Translate('QuoteOP.S.NewQuote.' + (this.formValue.transportMode?.Id === 'A' ? 'Gateway' : 'LoadingPort'));
        this.errorList.push(fieldName + this.fieldrequiredMsg + propertyPosition)
      }

      if (propertyForm.toPort.invalid) {
        const fieldName: string = TextCodeTranslator.Translate('QuoteOP.S.NewQuote.' + (this.formValue.transportMode?.Id === 'A' ? 'Destination' : 'DischargePort'));
        this.errorList.push(fieldName + this.fieldrequiredMsg + propertyPosition)
      }

      ['pickup', 'delivery'].forEach((formName: string) =>
        ['city', 'country', 'address'].filter(fieldName =>
          (<FormGroup>propertyForm[formName]).controls[fieldName].invalid)
          .forEach(fieldName =>
            this.errorList.push(TextCodeTranslator.Translate('QuoteOP.S.NewQuote.' + this.capitalizeFirstLetter(fieldName)) + this.fieldrequiredMsg + propertyPosition)
          ));
    });
  }

  private checkGeneral() {
    [
      { name: 'startDate', label: 'Start Date' },
      { name: 'expirationDays', label: 'Expiration Days' },
      { name: 'expirationDate', label: 'Expiration Date' },
    ].filter(field => this.form.controls[field.name].invalid)
      .forEach(field => this.errorList.push(field.label + this.fieldrequiredMsg))
  }

  private checkExpectedOrder() {
    const isSeaFcl: boolean = this.form.controls.transportMode?.value?.Id === 'O' && this.form.controls.shipmentType?.value?.Name === 'FCL';
    if (isSeaFcl)
      [
        {name:'quantityType', label: 'Paackage Type'}, 
        {name:'quantity', label: 'Quantity'}
      ].forEach(ctrl =>
        [1, 2, 3, 4]
          .filter(i => this.form.controls[ctrl.name + i].invalid)
          .forEach(i => this.errorList.push(ctrl.label + ' ' + i + this.fieldrequiredMsg))
      );
    else if (this.form.controls.packages.invalid)
      this.errorList.push('Please insert data to section Expected Order Details')
  }

  checkValidator(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach((key: string) => {
      if (formGroup.controls[key] instanceof FormControl)
        Object.keys(formGroup.controls[key].errors || {}).forEach(keyError =>
          this.errorList.push(`${key} field is requierd`));
      else if (formGroup.controls[key] instanceof FormGroup)
        this.checkValidator(formGroup.controls[key] as FormGroup);
      else if (formGroup.controls[key] instanceof FormArray)
        (formGroup.controls[key] as FormArray).controls.forEach((fg: FormGroup) => this.checkValidator(fg))
      else
        console.log('feild cnot now is type !!!!!!!!!!!!!!!!', key, formGroup.controls[key])
    });
  }

  private capitalizeFirstLetter(str: string): string {
    return str?.charAt(0).toUpperCase() + str?.slice(1);
  }
}
