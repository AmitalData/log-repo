export function CloneEntityPM(entityPM: any) {
    var clone = require("safe-clone-deep");
    var clonedEntityPM = clone(entityPM);
    clonedEntityPM.UIProperties = null;
    clonedEntityPM.entityParentPM = null;
    clonedEntityPM.OldEntityPM = null;
    return clonedEntityPM;
}