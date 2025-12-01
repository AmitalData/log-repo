
export class ObjectCloner {
    static Clone(baseObject: any) {
       
        var clonedObject: any;
        clonedObject = <any>(new baseObject.constructor);

        var baseKeys = Object.keys(baseObject);

        for (var key in baseKeys) {
            var property = baseKeys[key];
            clonedObject[property] = baseObject[property];
        }
            
        return clonedObject;
    }

    static CopyObject(sourceObject: any, targetObject: any) {
        var targetObject: any;
        var baseKeys = Object.keys(sourceObject);
        for (var key in baseKeys) {
            var property = baseKeys[key];
            targetObject[property] = sourceObject[property];
        }

        return targetObject;
    }
}