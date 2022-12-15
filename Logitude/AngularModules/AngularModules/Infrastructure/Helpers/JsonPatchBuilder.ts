import { compare, Operation } from 'fast-json-patch';
import { CloneEntityPM, CloneObject } from './SafeCloneDeep';

export class JsonPatchBuilder {
    private LeftObject: any;
    private RightObject: any;
    private Invertible: boolean;

    private ObjectPath: any = require("object-path");

    private InvalidProperties: string[] = [
        "OldEntityPM",
        "entityParentPM",
        "IsDirty",
        "DisableMarkAsDirty",
        "UIProperties",
        "UIProperty",
        "PropertyChanged",
        "tEU"
    ];

    constructor(leftObject: any, rightObject: any, invertible: boolean = false) {
        this.initialize(leftObject, rightObject, invertible);
    }

    private initialize(leftObject: any, rightObject: any, invertible: boolean) {
        this.LeftObject = leftObject;
        this.RightObject = rightObject;
        this.Invertible = invertible;
        this.InvalidProperties = this.InvalidProperties.map(p => { return p ? p.toLowerCase() : null; }).filter(p => p !== null);
    }

    public build() {
        if (this.LeftObject && this.RightObject) {
            let clonedLeftObject = CloneEntityPM(this.LeftObject);
            let clonedRightObject = CloneEntityPM(this.RightObject);
            let patch = compare(clonedLeftObject, clonedRightObject, this.Invertible);
            let cleanedPatch = this.cleanPatch(patch);
            let finalPatch = this.appendTestOperations(cleanedPatch);
            console.log(finalPatch);
            return finalPatch;
        }
        return null;
    }

    private cleanPatch(patch: Operation[]) {
        if (patch && patch.length > 0) {
            let cleanedPatch = patch.filter(o => this.isValidOperation(o));
            return cleanedPatch;
        }
        return patch;
    }

    private isValidOperation(operation: Operation) {
        if (operation && operation.op && operation.path && operation.path !== "") {
            let operationPathProperties = operation.path.toLowerCase().split("/");
            let isInvalidOperation = operationPathProperties.some(p => this.InvalidProperties.includes(p));
            if (!isInvalidOperation) {
                return true;
            }
        }
        return false;
    }

    private appendTestOperations(patch: Operation[]) {
        if (patch && patch.length > 0) {

            let clonedPatch: Operation[] = CloneObject(patch);

            clonedPatch.forEach((operation: Operation, operationIndex: number) => {
                let pathIndexRegex = /\/(\d+)/;
                if (operation.op && operation.op === "replace" && operation.path && pathIndexRegex.test(operation.path)) {

                    let testOperations: Operation[] = [];

                    let operationPaths = operation.path.split(pathIndexRegex);

                    operationPaths.forEach((operationPath: string, operationPathIndex: number) => {
                        if (this.isIntegerNumber(operationPath)) {
                            let keyField = "id";
                            let path = operationPaths.slice(0, operationPathIndex + 1).map(p => { return p.replace(/\//g, ""); }).join(".") + "." + keyField;
                            let pathValue = this.ObjectPath.get(this.LeftObject, path);
                            let testOperation: Operation = { op: "test", path: ("/" + path.replace(/\./g, "/")), value: (pathValue ? pathValue.toString() : null) };
                            testOperations.push(testOperation);
                        }
                    });


                    testOperations.forEach((testOperation: Operation, testOperationIndex: number) => {
                        patch.splice((operationIndex + testOperationIndex), 0, testOperation);
                    });

                }
            });

        }

        return patch;
    }

    private isIntegerNumber(value: string | number) {
        if (value && value.toString().includes(".")) {
            return false;
        }
        return ((value != null) &&
            (value !== '') &&
            !isNaN(Number(value.toString())));
    }
}