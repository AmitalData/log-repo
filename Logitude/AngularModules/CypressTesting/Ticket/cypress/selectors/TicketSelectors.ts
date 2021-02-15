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
}