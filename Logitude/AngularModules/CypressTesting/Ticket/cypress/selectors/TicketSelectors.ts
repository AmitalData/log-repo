import { RegexSelectors } from "./RegexSelectors"

export class TicketSelectors extends RegexSelectors
{
	public static readonly TicketCompany = "#Ticket_CompanyId"
	public static readonly TicketContact = "#Ticket_ContactId"
	public static readonly TicketSubject = "#Ticket_Subject"
	public static readonly TicketDescription = "#Ticket_TicketDescription"
	public static readonly TicketClassification = "#Ticket_MainClassificationId"
	public static readonly TicketSearchBar = "#Ticket_Search"
	public static readonly CorrespondenceLine = "#CorrespondenceLine"
	public static readonly TicketCancel = "#TicketBCancel"
	public static readonly TicektReactivate = "#TicektBReactivate"
	public static readonly TicektClosewithoutNotifying = "#TicektBClosewithoutNotifying"
	public static readonly MenuButtons = "#MenuButtons"
	public static readonly SaveMenuDropButton = ".x-button-drop"
	public static readonly SaveMenuButton = ".x-button-drop-menu"
	public static readonly ActivitySubject = "#Activity_Subject"
	public static readonly MarkAsComplete = "#ActivityBMarkAsComplete"
	public static readonly TicketParent = "quicksearchtextbox"
	public static readonly TicketParentClass = ".LogitudeQuickSearchTextBox"

	//#region Contains
	public static readonly ContainsOk="Ok"
	public static readonly ContainsYes="כן"
	public static readonly ContainsNew="New"
	public static readonly ContainsCreate = "Create"
	public static readonly ContainsSendReply = "Send Reply"
	public static readonly ContainsSendInternalNote = "Send Internal Note"
	public static readonly ContainsSaveAsOpen = "Save as Open"
	public static readonly ContainsSaveAsClosed = "Save as Closed"
	public static readonly ContainsSaveAsResolved = "Save as Resolved"
	public static readonly ContainsSendAndSetAsOpen = "Send and set as Open"
	//#endregion
}