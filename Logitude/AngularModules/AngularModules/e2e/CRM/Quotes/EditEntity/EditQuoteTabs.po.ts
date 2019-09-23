import { browser, by, element, WebDriver, protractor } from 'protractor';
import { FieldsHelper } from '../../../Helpers/FieldsHelper';
import { GeneralFunctions } from '../../../Helpers/GeneralFunctions';
import { OverviewTabComponent } from './OverviewTab';
import { DetailsTabComponent} from './DetailsTab';
import { PartnerTabComponent} from './PartnerTab';
import { PackagesTabComponent} from './PackagesTab';
import { RoutingsTabComponent} from './RoutingsTab';
import { ChargesTabComponent} from './ChargesTab';
import { QuoteActions } from '../EditEntity/QuoteActions';

export class EditTabsComponent {
    private Helper: FieldsHelper;
    private Operation: GeneralFunctions;
    private OverviewTabScenario: OverviewTabComponent;
    private DetailsTabScenario: DetailsTabComponent;
    private PartnerTabScenario: PartnerTabComponent;
    private PackagesTabScenario: PackagesTabComponent;
    private RoutingsTabScenario: RoutingsTabComponent;
    private ChargesTabScenario: ChargesTabComponent;
    private QuoteActions: QuoteActions;


    constructor() {
        this.Helper = new FieldsHelper();
        this.Operation = new GeneralFunctions();
        this.OverviewTabScenario = new OverviewTabComponent();
        this.DetailsTabScenario = new DetailsTabComponent();
        this.PartnerTabScenario = new PartnerTabComponent();
        this.PackagesTabScenario = new PackagesTabComponent();
        this.RoutingsTabScenario = new RoutingsTabComponent();
        this.ChargesTabScenario = new ChargesTabComponent();
        this.QuoteActions = new QuoteActions();

    }


    EditTabs(shipperRef1: string, ShipmentType: string, Direction: string, TransportMode: string, QuoteType: string) {
        this.OverviewTabScenario.OverviewTab(ShipmentType);
        this.DetailsTabScenario.DetailsTab(ShipmentType);
        this.PartnerTabScenario.PartnerTab(ShipmentType);
        this.PackagesTabScenario.PackagesTab(ShipmentType, QuoteType);
        this.RoutingsTabScenario.RoutingsTab(ShipmentType);
        this.ChargesTabScenario.ChargesTab(ShipmentType);
        this.Helper.WaitByIdAndClick('Quote-Save');
        this.Helper.WaitEditComponentBusyIndicator();
        this.QuoteActions.QuoteMenubuttonActions('copybuild', Direction, TransportMode, QuoteType);
    }

}

