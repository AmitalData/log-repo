import { MixPanelService } from 'Common/Services/ExtendedLists/MixPanelService';
import { MixPanelEvent } from 'Common/DataContracts/MixPanelEvent';

export class MixPanelLocator {
    private static MixPanelService: MixPanelService;
    public static Action(mixPanelEvent: MixPanelEvent) {
        if (this.MixPanelService == null) {
            this.MixPanelService = new MixPanelService();
        }
        this.MixPanelService.PostEvent(mixPanelEvent).subscribe();
    }

    public static PostDashboardAction(mixPanelEvent: MixPanelEvent) {
        mixPanelEvent.ProjectName = "Dashboard";
        if (this.MixPanelService == null) {
            this.MixPanelService = new MixPanelService();
        }
        this.MixPanelService.PostEvent(mixPanelEvent).subscribe();
    }
    
}