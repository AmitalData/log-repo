declare var window: any;
import { Injectable } from '@angular/core';
import { CustomChildEntity } from '../../EntityPMs/CustomChildEntity';
import { Guid } from '../../Utilities/Guid';
import { CustomChildObjectPM } from '../../EntityPMs/CustomChildObjectPM';
import { CustomFieldClass } from '../../DataContracts/CustomFieldClass';

@Injectable()
export class CustomChildObjectPMService {
    public ParentObjectTableName: string;
    public entityPM: any;
    constructor(entityPM: any, parentObjectTableName: string) {
        this.ParentObjectTableName = parentObjectTableName;
        this.entityPM = entityPM;
    }

    public MapCustomChildEntities(jsonPM: any, mapParent: boolean = true) {
        var oldCollection: CustomChildEntity[] = [];
        if (this.entityPM.OldEntityPM && !mapParent) {
            oldCollection = this.entityPM.OldEntityPM.CustomChildEntities;
        }

        this.entityPM.CustomChildEntities = new Array<CustomChildEntity>();
        if (!jsonPM) return;

        for (var customChildEntity in jsonPM.CustomChildEntities) {
            var { itemJson, pmKeys, key, property } = this.MapCustomChildEntitiesKeys(jsonPM, customChildEntity, mapParent);
        }

        let deletedCustomChildEntitiesArgs = new DeletedCustomChildEntitiesArgs();

        deletedCustomChildEntitiesArgs.oldCollection = oldCollection;
        deletedCustomChildEntitiesArgs.customChildEntity = customChildEntity;
        deletedCustomChildEntitiesArgs.itemJson = itemJson;
        deletedCustomChildEntitiesArgs.pmKeys = pmKeys;
        deletedCustomChildEntitiesArgs.key = key;
        deletedCustomChildEntitiesArgs.mapParent = mapParent;
        deletedCustomChildEntitiesArgs.property = property;

        var { customChildEntity, pmKeys, key, property } = this.MapDeletedCustomChildEntities(deletedCustomChildEntitiesArgs);
    }

    private MapCustomChildEntitiesKeys(jsonPM: any, customChildEntity: string, mapParent: boolean) {
        var itemJson = jsonPM.CustomChildEntities[customChildEntity];
        if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
            return { itemJson, pmKeys, key, property };
        }

        var itemPM: CustomChildEntity;
        if (mapParent) {
            itemPM = new CustomChildEntity(this.entityPM, this.ParentObjectTableName, itemJson.Name);
        }
        else {
            itemPM = new CustomChildEntity(this.entityPM, this.ParentObjectTableName, itemJson.Name);
        }

        itemPM.DisableMarkAsDirty = true;
        var pmKeys = Object.keys(itemJson);
        for (var key in pmKeys) {
            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
                continue;
            }
            var property = pmKeys[key];
            itemPM[property] = itemJson[property];
        }

        this.MapCustomChildEntitiesKey(mapParent, itemPM, itemJson);

        itemPM.DisableMarkAsDirty = false;
        itemPM.IsDirty = false;
        this.entityPM.CustomChildEntities.push(itemPM); //tobesure
        return { itemJson, pmKeys, key, property };
    }

    private MapCustomChildEntitiesKey(mapParent: boolean, itemPM: CustomChildEntity, itemJson: any) {
        if (!mapParent) {
            this.MapCustomChildEntitiesKeyWithoutParent(itemPM, itemJson, mapParent);
            return;
        }
        itemPM.UniqueKey = Guid.newGuid();
        itemPM.ChangeSetOp = "None";
        itemJson.ChangeSetOp = "None";
        itemPM.OldEntityPM = this.clone(itemPM);

        this.MapCustomChildObjects(itemPM, itemJson, mapParent);
        itemPM.OldEntityPM.Values = [];
        for (var k1 in itemPM.Values) {
            var clonedInside = this.clone(itemPM.Values[k1]);
            itemPM.OldEntityPM.Values.push(clonedInside);
        }
    }

    private MapCustomChildEntitiesKeyWithoutParent(itemPM: CustomChildEntity, itemJson: any, mapParent: boolean) {
        if (itemPM.UniqueKey) {
            itemPM.ChangeSetOp = itemJson.IsDirty ? "Update" : itemPM.ChangeSetOp;
        }
        else {
            itemPM.ChangeSetOp = "Insert";
        }

        this.MapCustomChildObjects(itemPM, itemJson, mapParent);
        itemPM.EntityParentPM = null;
        itemPM.OldEntityPM = null;
    }

    private MapDeletedCustomChildEntities(deletedCustomChildEntitiesArgs: DeletedCustomChildEntitiesArgs) {
        var oldCollection = deletedCustomChildEntitiesArgs.oldCollection;
        var customChildEntity = deletedCustomChildEntitiesArgs.customChildEntity;
        var pmKeys = deletedCustomChildEntitiesArgs.pmKeys;
        var key = deletedCustomChildEntitiesArgs.key;
        var property = deletedCustomChildEntitiesArgs.property;

        if (!oldCollection) return { customChildEntity, pmKeys, key, property };

        for (var customChildEntity in oldCollection) {
            deletedCustomChildEntitiesArgs.customChildEntity = customChildEntity;
            var { pmKeys, key, property } = this.MapDeletedCustomChildEntity(deletedCustomChildEntitiesArgs);
        }
        return { customChildEntity, pmKeys, key, property };
    }

    private MapDeletedCustomChildEntity(deletedCustomChildEntitiesArgs: DeletedCustomChildEntitiesArgs) {
        var oldCollection = deletedCustomChildEntitiesArgs.oldCollection;
        var customChildEntity = deletedCustomChildEntitiesArgs.customChildEntity;
        var pmKeys = deletedCustomChildEntitiesArgs.pmKeys;
        var itemJson = deletedCustomChildEntitiesArgs.itemJson;
        var key = deletedCustomChildEntitiesArgs.key;
        var mapParent = deletedCustomChildEntitiesArgs.mapParent;
        var property = deletedCustomChildEntitiesArgs.property;
        if (this.entityPM.CustomChildEntities.filter(p => p.UniqueKey === oldCollection[customChildEntity].UniqueKey).length !== 0) return { pmKeys, key, property };
        if (!oldCollection[customChildEntity]) return { pmKeys, key, property };
        var oldpackageJson = oldCollection[customChildEntity];
        var deletedPM: CustomChildEntity = new CustomChildEntity(this.entityPM, this.ParentObjectTableName, itemJson.Name);
        var pmKeys = Object.keys(oldpackageJson);
        for (var key in pmKeys) {
            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = pmKeys[key];
            deletedPM[property] = oldpackageJson[property];
        }

        deletedPM.IsDirty = false;
        deletedPM.ChangeSetOp = "Delete";

        deletedPM.OldEntityPM = null;
        this.entityPM.CustomChildEntities.push(deletedPM);

        return { pmKeys, key, property };
    }

    private MapCustomChildObjects(customChildEntity: CustomChildEntity, jsonPM: any, mapParent: boolean = true) {

        var oldCollection: CustomChildObjectPM[] = [];
        if (customChildEntity.OldEntityPM && !mapParent) {
            oldCollection = customChildEntity.OldEntityPM.Values;
        }
        customChildEntity.Values = new Array<CustomChildObjectPM>();
        if (!jsonPM) return;
        for (var value in jsonPM.Values) {
            let customChildObjectsKeysMapperArgs = new CustomChildObjectsKeysMapperArgs();
            customChildObjectsKeysMapperArgs.jsonPM = jsonPM;
            customChildObjectsKeysMapperArgs.value = value;
            customChildObjectsKeysMapperArgs.mapParent = mapParent;
            customChildObjectsKeysMapperArgs.customChildEntity = customChildEntity;
            var { pmKeys, key, property } = this.MapCustomChildObjectsKeys(customChildObjectsKeysMapperArgs);
        }

        let deletedCustomChildObjectsArgs = new DeletedCustomChildObjectsArgs();

        deletedCustomChildObjectsArgs.oldCollection = oldCollection;
        deletedCustomChildObjectsArgs.value = value;
        deletedCustomChildObjectsArgs.customChildEntity = customChildEntity;
        deletedCustomChildObjectsArgs.pmKeys = pmKeys;
        deletedCustomChildObjectsArgs.key = key;
        deletedCustomChildObjectsArgs.mapParent = mapParent;
        deletedCustomChildObjectsArgs.property = property;

        var { value, pmKeys, key, property } = this.MapDeletedCustomChildObjects(deletedCustomChildObjectsArgs);
    }

    private MapCustomChildObjectsKeys(customChildObjectsKeysMapperArgs: CustomChildObjectsKeysMapperArgs) {

        var jsonPM = customChildObjectsKeysMapperArgs.jsonPM;
        var value = customChildObjectsKeysMapperArgs.value;
        var mapParent = customChildObjectsKeysMapperArgs.mapParent;
        var customChildEntity = customChildObjectsKeysMapperArgs.customChildEntity;

        var itemJson = jsonPM.Values[value];
        if (mapParent && (itemJson.ChangeSetOp == "Delete" || itemJson.ChangeSetOp == 3)) {
            return { pmKeys, key, property };
        }

        var itemPM: CustomChildObjectPM;
        if (mapParent) {
            itemPM = new CustomChildObjectPM(customChildEntity.Name);
        }
        else {           
            itemPM = new CustomChildObjectPM(customChildEntity.Name);
        }

        itemPM.DisableMarkAsDirty = true;
        var pmKeys = Object.keys(itemJson);
        for (var key in pmKeys) {
            customChildObjectsKeysMapperArgs = new CustomChildObjectsKeysMapperArgs();
            customChildObjectsKeysMapperArgs.mapParent = mapParent;
            customChildObjectsKeysMapperArgs.pmKeys = pmKeys;
            customChildObjectsKeysMapperArgs.key = key;
            customChildObjectsKeysMapperArgs.itemJson = itemJson;
            customChildObjectsKeysMapperArgs.itemPM = itemPM;

            var property = this.MapCustomChildObjectsKey(customChildObjectsKeysMapperArgs);
        }

        itemPM.DisableMarkAsDirty = false;
        this.SetChangeSetOperationKey(mapParent, itemPM, itemJson);
        itemPM.IsDirty = false;
        customChildEntity.Values.push(itemPM);
        return { pmKeys, key, property };
    }

    private MapCustomChildObjectsKey(customChildObjectsKeysMapperArgs: CustomChildObjectsKeysMapperArgs) {
        var mapParent = customChildObjectsKeysMapperArgs.mapParent;
        var pmKeys = customChildObjectsKeysMapperArgs.pmKeys;
        var key = customChildObjectsKeysMapperArgs.key;
        var itemJson = customChildObjectsKeysMapperArgs.itemJson;
        var itemPM = customChildObjectsKeysMapperArgs.itemPM;

        if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties") {
            return property;
        }

        var customFields: Array<string> = [];
        for (var i = 1; i < 51; i++) {
            customFields.push("Field" + i);
        }

        var property = pmKeys[key];

        if (customFields.indexOf(property) <= -1) {
            itemPM[property] = itemJson[property];
            return property;
        }

        if (itemJson[property]) {
            var customFieldClass: CustomFieldClass = new CustomFieldClass(itemJson[property].Value, itemJson[property].FieldName, itemJson[property].TableName);
            itemPM[property] = customFieldClass;
        }

        return property;
    }

    private SetChangeSetOperationKey(mapParent: boolean, itemPM: CustomChildObjectPM, itemJson: any) {
        if (mapParent) {
            this.SetNoneChangeSetOperationKey(itemPM, itemJson);
            return;
        }
        
        if (itemPM.UniqueKey) {
            itemPM.ChangeSetOp = itemPM.IsDirty ? "Update" : itemPM.ChangeSetOp;
        }
        else {
            itemPM.ChangeSetOp = "Insert";
        }

        itemPM.OldEntityPM = null;
    }

    private SetNoneChangeSetOperationKey(itemPM: CustomChildObjectPM, itemJson: any) {
        itemPM.UniqueKey = Guid.newGuid();
        itemPM.ChangeSetOp = "None";
        itemJson.ChangeSetOp = "None";
        itemPM.OldEntityPM = this.clone(itemPM);
    }

    private MapDeletedCustomChildObjects(deletedCustomChildObjectsArgs: DeletedCustomChildObjectsArgs) {
        var oldCollection = deletedCustomChildObjectsArgs.oldCollection;
        var value = deletedCustomChildObjectsArgs.value;
        var pmKeys = deletedCustomChildObjectsArgs.pmKeys;
        var key = deletedCustomChildObjectsArgs.key;
        var property = deletedCustomChildObjectsArgs.property;


        if (!oldCollection) return { value, pmKeys, key, property };

        for (var value in oldCollection) {
            deletedCustomChildObjectsArgs.value = value;
            var { pmKeys, key, property } = this.MapDeletedCustomChildObject(deletedCustomChildObjectsArgs);
        }
        return { value, pmKeys, key, property };
    }

    private MapDeletedCustomChildObject(deletedCustomChildObjectsArgs: DeletedCustomChildObjectsArgs) {
        var oldCollection = deletedCustomChildObjectsArgs.oldCollection;
        var value = deletedCustomChildObjectsArgs.value;
        var customChildEntity = deletedCustomChildObjectsArgs.customChildEntity;
        var pmKeys = deletedCustomChildObjectsArgs.pmKeys;
        var key = deletedCustomChildObjectsArgs.key;
        var mapParent = deletedCustomChildObjectsArgs.mapParent;
        var property = deletedCustomChildObjectsArgs.property;

        if (customChildEntity.Values.filter(p => p.UniqueKey === oldCollection[value].UniqueKey).length !== 0) return { pmKeys, key, property };
        if (!oldCollection[value]) return { pmKeys, key, property };
        var oldpackageJson = oldCollection[value];
        var deletedPM: CustomChildObjectPM = new CustomChildObjectPM(customChildEntity.Name);
        var pmKeys = Object.keys(oldpackageJson);
        for (var key in pmKeys) {
            if ((!mapParent && pmKeys[key] === "entityParentPM") || pmKeys[key] === "UIProperties" || pmKeys[key] === "OldEntityPM") {
                continue;
            }
            var property = pmKeys[key];
            deletedPM[property] = oldpackageJson[property];
        }
        deletedPM.IsDirty = false;
        deletedPM.ChangeSetOp = "Delete";
        deletedPM.OldEntityPM = null;
        customChildEntity.Values.push(deletedPM);

        return { pmKeys, key, property };
    }

    private clone(jsonPM: any) {
        var entityPM: any;
        entityPM = {};
        try {
            let jsonPMKeys = Object.keys(jsonPM);
            for (var key in jsonPMKeys) {
                this.CloneSingleJsonKey(jsonPMKeys[key], entityPM, jsonPM);
            }
        }
        catch (e) {
            //
        }

        return entityPM;
    }

    private CloneSingleJsonKey(jsonPMKey: string, entityPM: any, jsonPM: any) {
        if ((jsonPMKey === "entityParentPM") || jsonPMKey === "UIProperties" || jsonPMKey === "OldEntityPM") {
            return;
        }

        entityPM[jsonPMKey] = jsonPM[jsonPMKey];
    }
}

class DeletedCustomChildEntitiesArgs {
    public oldCollection: CustomChildEntity[];
    public customChildEntity: string;
    public itemJson: any;
    public pmKeys: string[];
    public key: string;
    public mapParent: boolean;
    public property: string;
}

class DeletedCustomChildObjectsArgs {
    public oldCollection: CustomChildObjectPM[];
    public value: string;
    public customChildEntity: CustomChildEntity;
    public pmKeys: string[];
    public key: string;
    public mapParent: boolean;
    public property: string;
}

class CustomChildObjectsKeysMapperArgs {
    public mapParent: boolean;
    public pmKeys: string[];
    public key: string;
    public itemJson: any;
    public itemPM: CustomChildObjectPM;
    public jsonPM: any;
    public value: string;
    public customChildEntity: CustomChildEntity;
}
