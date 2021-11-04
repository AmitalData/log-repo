import { ValidatorFn, FormGroup, AbstractControl, ValidationErrors, FormControl } from "@angular/forms";
import { filter } from "rxjs/operators";

export function requiredOneFromMultiValidator(t: any, formGroupName: string, thisCtrlName: string, ...ctrlsName: string[]): ValidatorFn {
    (async () => {
      let fromGroupNotFound = true;
  
      while (fromGroupNotFound) {
        fromGroupNotFound = !t[formGroupName]
        await new Promise(resolve => setTimeout(resolve, 100));
      }
  
      const fg = t[formGroupName] as FormGroup
      ctrlsName.forEach(ctrlName =>
        fg.controls[ctrlName].valueChanges.pipe(filter(newVal => fg.value[ctrlName] !== newVal)).subscribe(x => fg.controls[thisCtrlName].updateValueAndValidity()))
    })()
  
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.parent) return null;
  
      return !control.value &&
        !ctrlsName.some(ctrlName => !!((control.parent?.controls as any)[ctrlName] as FormControl).value) ?
        { requiredOneFromMulti: { value: control.value } } : null;
    };
  }