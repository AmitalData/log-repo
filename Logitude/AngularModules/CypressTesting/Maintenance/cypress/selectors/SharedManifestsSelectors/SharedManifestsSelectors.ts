import { RegexSelectors } from '../RegexSelectors';

export class SharedManifestsSelectors extends RegexSelectors {

    public static readonly OperationsTab='[id="GeneralMHOperations"]'
    public static readonly SharedManifests='[id="SHMA"]'
    public static readonly DocumentsPermissions='[data-cy="Documents Permissions"]'
    public static readonly AirManifest='Air Manifest'
    public static readonly AirwaybillLabels='Air waybill- Labels'
    public static readonly ArrivalNotice='Arrival Notice'
    public static readonly AgentTHSharedLogistics='[id="AgentTHSharedLogistics"]'
    public static readonly SendInvitation='[data-cy="Send Invitation"]'
    public static readonly Email='[data-cy="Email"]'
    public static readonly Send='[data-cy="Send"]'
    public static readonly SharedKey='input[data-cy="SharedKey"]'
    public static readonly Cancel='[data-cy="Cancel"]'
    public static readonly AcceptInvitation='[data-cy="Accept invitation"]'
    public static readonly Key='[data-cy="Key"]'
    public static readonly SignOut="img[src='./Images/Icons/Signout.png']"
}