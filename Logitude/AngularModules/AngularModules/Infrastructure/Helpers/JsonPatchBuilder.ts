import { compare, Operation } from 'fast-json-patch';
import { CloneDeep } from './LodashClone';
declare var require: any

export class JsonPatchBuilder {
    private LeftObject: any;
    private RightObject: any;

    private ObjectPath: any = require("object-path");

    private InvalidProperties: string[] = [
        "OldEntityPM",
        "entityParentPM",
        "IsDirty",
        "DisableMarkAsDirty",
        "UIProperties",
        "UIProperty",
        "PropertyChanged",
        "UniqueKey",
        "$id",
        "undefined"
    ];

    constructor(leftObject: any, rightObject: any) {
        this.initialize(leftObject, rightObject);
    }

    private initialize(leftObject: any, rightObject: any) {
        this.LeftObject = leftObject;
        this.RightObject = rightObject;
        this.InvalidProperties = this.InvalidProperties.map(p => { return p ? p.toLowerCase() : null; }).filter(p => p !== null);
    }

    public build() {
        if (this.LeftObject && this.RightObject) {
            let clonedLeftObject = CloneDeep(this.LeftObject);
            let clonedRightObject = CloneDeep(this.RightObject);
            let patch = compare(clonedLeftObject, clonedRightObject);
            let cleanedPatch = this.cleanPatch(patch);
            let modifiedPatch = this.modifyAddToListOperations(cleanedPatch);
            let patchWithTestOperations = this.addTestOperations(modifiedPatch);
            //console.log(patchWithTestOperations);
            return patchWithTestOperations;
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
            if (!this.isValidOperationPath(operation.path)) {
                return false;
            }
            let operationPathProperties = operation.path.toLowerCase().split("/");
            let isInvalidOperation = operationPathProperties.some(p => this.InvalidProperties.includes(p));
            if (!isInvalidOperation) {
                return true;
            }
        }
        return false;
    }

    private isValidOperationPath(operationPath: string) {
        if (operationPath && operationPath !== "") {
            if (/customchildentities\/([0-9]\d*)\/changesetop/.test(operationPath.toLowerCase())) {
                return false;
            }
            if (/field([1-9]\d*)\/isnotvalid/.test(operationPath.toLowerCase())) {
                return false;
            }
            if (/field([1-9]\d*)\/ischange/.test(operationPath.toLowerCase())) {
                return false;
            }
            return true;
        }
        return false;
    }

    private modifyAddToListOperations(patch: Operation[]) {
        if (patch && patch.length > 0) {
            patch.filter(o => o.op === "add" && o.path).forEach((operation: Operation) => {
                let operationPathProperties = operation.path.split("/");
                let lastOperationPathProperty = operationPathProperties[operationPathProperties.length - 1];
                if (this.isIntegerNumber(lastOperationPathProperty)) {
                    operation.path = operationPathProperties.slice(0, -1).join("/") + "/-";
                }
            });
        }
        return patch;
    }

    private addTestOperations(patch: Operation[]) {
        if (patch && patch.length > 0) {
            let testOperations: Operation[] = this.buildPatchTestOperations(patch);
            testOperations.forEach((testOperation: Operation, testOperationIndex: number) => {
                patch.splice(testOperationIndex, 0, testOperation);
            });
        }
        return patch;
    }

    private buildPatchTestOperations(patch: Operation[]) {
        if (patch && patch.length > 0) {
            let testOperations: Operation[] = [];
            let clonedPatch: Operation[] = CloneDeep(patch);
            clonedPatch.forEach((operation: Operation) => {
                let pathIndexRegex = /\/(\d+)/;
                if (operation.path && pathIndexRegex.test(operation.path)) {
                    let operationPaths = operation.path.split(pathIndexRegex);
                    operationPaths.forEach((operationPath: string, operationPathIndex: number) => {
                        if (this.isIntegerNumber(operationPath)) {
                            let pathProperties = operationPaths.slice(0, operationPathIndex + 1).map(p => { return p.replace(/\//g, ""); });
                            let pathTestOperations = this.buildPathTestOperations(pathProperties);
                            pathTestOperations.forEach((testOperation: Operation) => {
                                if (testOperations.filter(o => o.path === testOperation.path).length === 0) {
                                    testOperations.push(testOperation);
                                }
                            });
                        }
                    });
                }
            });
            return testOperations;
        }
        return [];
    }

    private buildPathTestOperations(pathProperties: string[]) {
        if (pathProperties && pathProperties.length >= 2) {
            let testOperations: Operation[] = [];
            let pathProperty = pathProperties[pathProperties.length - 2];
            let keyFields = this.getPathPropertyKeyFields(pathProperty);
            keyFields.forEach((keyField: string) => {
                let path = pathProperties.join(".") + "." + keyField;
                let pathValue = this.ObjectPath.get(this.LeftObject, path);
                let testOperation: Operation = { op: "test", path: ("/" + path.replace(/\./g, "/")), value: (pathValue || null) };
                testOperations.push(testOperation);
            });
            return testOperations;
        }
        return [];
    }

    private getPathPropertyKeyFields(pathProperty: string) {
        if (pathProperty) {
            switch (pathProperty.toLowerCase()) {
                case "shipmentpackageitems": { return ["packageId", "lineNumber"]; }
                case "customchildentities": { return ["name"]; }
                default: { return ["id"]; }
            }
        }
        return [];
    }

    private isIntegerNumber(value: string | number) {
        if (value && value.toString().includes(".")) {
            return false;
        }
        return (value && value !== "" && !isNaN(Number(value.toString())));
    }
}