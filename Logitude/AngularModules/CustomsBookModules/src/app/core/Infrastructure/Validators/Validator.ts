import {ClassLevelValidator} from './ClassLevelValidator';

export class Validator {
    public static TryValidateObject(instance: Object, tableName: string, errors: string[]) {

        if (instance != null) {
            if (tableName != null) {
                var levelValidator = new ClassLevelValidator();
                var myResult = levelValidator.Validate(tableName, instance);
                if (myResult != null) {
                    if (errors == null) {
                        errors = [];
                    }

                    myResult.forEach((item) => {
                        errors.push(item);
                    });
                }
            }
        }
    }
}
