import { RegexSelectors } from '../RegexSelectors';

export class SharedManifestsSelectors extends RegexSelectors {

    public static readonly OperationsTab='[id="GeneralMHOperations"]'
    public static readonly SharedManifests='[id="SHMA"]'
    public static readonly DocumentsPermissions='[data-cy="Documents Permissions"]'
    public static readonly AirManifest='Air Manifest'
    public static readonly AirwaybillLabels='Air waybill- Labels'
    public static readonly ArrivalNotice='Arrival Notice'
}