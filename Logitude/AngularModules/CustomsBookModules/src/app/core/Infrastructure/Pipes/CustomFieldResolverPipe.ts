import {Pipe} from '@angular/core';
import {ObjectFieldPM} from '../EntityPMs/ObjectFieldPM';
import {CustomFieldClass} from '../DataContracts/CustomFieldClass';

@Pipe({ name: 'CustomFieldResolverPipe' })

export class CustomFieldResolverPipe {
    transform(value: string, myObjectFieldPM: ObjectFieldPM = null, myCustomFieldClass: CustomFieldClass = null): any {
        var myResult: any = null;

        if (value && myObjectFieldPM && myCustomFieldClass) {
            myResult = myCustomFieldClass.GetFieldDataTypeValue(myObjectFieldPM, value);
        }

        return myResult;
    }
}