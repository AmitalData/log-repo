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
    public static readonly SharingActions='[data-cy="Sharing Actions"]'
    public static readonly ShareManifest='[data-cy="Share Manifest"]'
    public static readonly ShareDocuments='[data-cy="Share Documents"]'
    public static readonly ShareDocumentsViaEmail='[data-cy="Share Documents Via Email"]'
    public static readonly UpdateSharedAgent='[data-cy="Update Shared Agent"]'
    public static readonly ShipmentRoutings='[id="ShipmentTHRoutings"]'
    public static readonly MainCarriage='[id="Edit-MainCarriage"]'
    public static readonly MainCarriageCarrier='[id="Shipment_MainCarriageCarrierId"]'
    public static readonly ShipmentMasterNo='[id="Shipment_Master"]'
    public static readonly ShipmentPartners='[id="ShipmentTHPartners"]'
    public static readonly DeleteConsignee='[id="Delete-Consignee"]'
    public static readonly ShipmentSave='[id="Shipment-Save"]'
    public static readonly AddPartner='[id="PartnerToggle"]'
    public static readonly AddConsignee= '[id="CONSI"]'
    public static readonly ConsigneeId='[id="Shipment_ConsigneeId"]'
    public static readonly BackBottonBody='[class="BackBottonBody"]'
    public static readonly Close='[data-cy="Close"]'
    public static readonly Search='[placeholder="SearchFields"]'
    public static readonly FirstRowMAWB='[id="row0col2"]'
    public static readonly Create='[data-cy="Create"]'
    public static readonly MasterMainCarriageCarrier='[id="Master_MainCarriageCarrierId"]'
    public static readonly CreateMaster='[data-cy="Create Master"]'
    public static readonly AgentReference='[data-cy="AgentReference"]'
    public static readonly CancelManifest='[data-cy="Cancel Manifest"]'
    public static readonly MarkAsCompleted='[data-cy="Mark as Completed"]'
    public static readonly GeneralMHSharedLogistics="#GeneralMHSharedLogistics"
    public static readonly AgentView='[data-cy="Agent View"]'
    public static readonly AgentSearch="#SearchFieldsId_0_0"
    public static readonly CheckBox='[data-cy="Air Manifest_Master"]'
    public static readonly ShipmentTHDocsOut="#ShipmentTHDocsOut"
    public static readonly BuildDocsOut="#785A-P-DocsOut"
    public static readonly SearchBox='[placeholder="Search"]'
    public static readonly CloseDocument="#closeButtonId"
    public static readonly Share=".RedButton"
    public static readonly MessageOK="#MessageWindow_Ok_0"
    public static readonly ShipmentTHDocsIn="#ShipmentTHDocsIn"
    
}