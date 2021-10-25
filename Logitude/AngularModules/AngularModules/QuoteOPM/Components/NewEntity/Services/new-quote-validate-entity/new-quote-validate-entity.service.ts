import { Injectable } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup } from '@angular/forms';
import { MessageService } from 'primeng/api';

@Injectable()
export class NewQuoteValidateEntityService {
  errorList: string[] = [];
  form: FormGroup = null as any;

  constructor(
    private messageService: MessageService,
  ) { }

  validate(form: FormGroup): boolean {
    this.errorList = [];
    this.form = form;

    this.checkPartner();
    // this.checkProperties();
    this.checkValidator(this.form)

    this.showErrorMessage();

    return this.errorList.length > 0;
  }

  private showErrorMessage(): void {
    this.messageService.add({ severity: 'error', summary: 'Create new quote failed.', detail: this.errorList.join('\n'), life: 30 * 1000 });
  }

  checkPartner(): void {
    // must set shipper or consignee

    const shipperForm: any = this.form.controls.shipper.value;
    const consigneeForm: any = this.form.controls.consignee.value;

    if (!(shipperForm.partner && shipperForm.contact) && !(consigneeForm.partner && consigneeForm.contact))
      this.errorList.push('Shipperr or consignee and is contact is requierd.')
  }

  checkCloseDate() {
    const value: any = this.form.controls.value;
    
    if (value.isAutomaticallyClosed) {
      if (!value.automaticallyCloseDays)
        this.errorList.push('Close days is requierd.')
      if (!value.automaticallyCloseDate)
        this.errorList.push('Close date is requierd.')
    }
  }

  // checkProperties(): void {
  //   const propertiesForms: AbstractControl[] = (this.form.controls.properties as FormArray).controls.filter((propertyForm: FormGroup) => propertyForm.valid);
  //   propertiesForms.forEach((propertyFormGroup: FormGroup) => {
  //     const propertyForm: FormGroup["controls"] = propertyFormGroup.controls;
  //   });
  // }

  checkValidator(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach((key: string) => {
      if (formGroup.controls[key] instanceof FormControl)
        Object.keys(formGroup.controls[key].errors || {}).forEach(keyError =>
          this.errorList.push(`${key} feild is requierd`));
      else if (formGroup.controls[key] instanceof FormGroup)
        this.checkValidator(formGroup.controls[key] as FormGroup);
      else if (formGroup.controls[key] instanceof FormArray)
        (formGroup.controls[key] as FormArray).controls.forEach((fg: FormGroup) => this.checkValidator(fg))
      else
        console.log('feild cnot now is type !!!!!!!!!!!!!!!!', key, formGroup.controls[key])
    });
  }



}
