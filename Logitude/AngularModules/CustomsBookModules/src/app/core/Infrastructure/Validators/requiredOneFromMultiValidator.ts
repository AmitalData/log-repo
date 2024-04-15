import { ValidatorFn, FormGroup, AbstractControl, ValidationErrors, FormControl } from "@angular/forms";
import { filter, pairwise, startWith } from "rxjs/operators";

export function requiredOneFromMultiValidator(classComponent: any, formGroupName: string, thisCtrlName: string, ...ctrlsName: string[]): ValidatorFn {
    (async () => {
      let fromGroupNotFound = true;
  
      while (fromGroupNotFound) {
        fromGroupNotFound = !classComponent[formGroupName]
        await new Promise(resolve => setTimeout(resolve, 100));
      }
  
      const fg = classComponent[formGroupName] as FormGroup
      ctrlsName.forEach(ctrlName =>
        fg.controls[ctrlName].valueChanges.pipe(startWith(null as string), pairwise(), filter(([prev, next]: [any, any]) => prev != next)).subscribe(x => fg.controls[thisCtrlName].updateValueAndValidity()))
    })()
  
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.parent) return null;
  
      return !control.value &&
        !ctrlsName.some(ctrlName => !!((control.parent?.controls as any)[ctrlName] as FormControl).value) ?
        { requiredOneFromMulti: { value: control.value } } : null;
    };
  }