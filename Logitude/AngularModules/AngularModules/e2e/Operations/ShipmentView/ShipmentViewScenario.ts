import { LoginComp } from '../../Login/Login.po';
import { FieldsHelper } from '../../Helpers/FieldsHelper';
import { ShipmentView } from './ShipmentView';


export class ShipmentViewScenario{

    private login: LoginComp = new LoginComp();
    private helper = new FieldsHelper();
    private ShipmentView:ShipmentView=new ShipmentView();


constructor() {

}

public OpenShipmentView(){
    
    this.ShipmentView.OpenShipmentView();


}

public CreatNewView(){
    this.ShipmentView.CreatNewView();


}

public EditNewView(){
    this.ShipmentView.EditNewView();


}

public DeleteNewView(){

    this.ShipmentView.DeleteNewView();


}

}