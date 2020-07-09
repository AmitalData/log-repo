import { Component} from '@angular/core';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from 'Infrastructure/Locators/ObjectsLocator';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
 

@Component({
    
    templateUrl: './CargoTrackingServiceComponent.html',
})

export class CargoTrackingServiceComponent {
  public ItemsSource: CargoTrackingMenuItem[];
  LayoutDirection: string = 'ltr';
  private CurrentSession = SessionLocator.SelectedSession;
  constructor() {
      this.ItemsSource = [];
      this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
      this.BuildItemsSource();
  } 
  CancelButtonClicked() {
     this.CurrentSession.CloseCurrentWindow();
}


  private BuildItemsSource() {

        var item1 = new CargoTrackingMenuItem();
        item1.TranslatedName = "Build Cargo Tracking Shipments";
        item1.ImageIconSource = this.SetImageIconSource("Settings");
        item1.Code ="CargoShipment";
        // item1.DescriptionText = "Cargo Tracking";
        this.ItemsSource.push(item1);
  

}


ItemClicked(item:CargoTrackingMenuItem){

  switch(item.Code){
    case "CargoShipment":{
      var windowTitle = "Build Cargo Tracking Shipments";
      var logWindow = new LogitudeWindow();
      logWindow.Width = 950;
      logWindow.Height = 500;
      logWindow.Title = windowTitle;
      logWindow.IsShowCloseButton = true;
      logWindow.Show('./Accounting/Components/Others/CargoTrackingService/CargoTrackingBuildShipmentComponent');
    
      break;
    }
  }
 
}


private SetImageIconSource(ImageIconSource:string) {
  switch (ImageIconSource) {
      case "Settings": {
        ImageIconSource = "./Images/Maintenance/Settings.png";
          break;
      }
      default: {
        ImageIconSource= "./Images/Maintenance/Table.png";
          break;
      }
  }
  return ImageIconSource;
}


}

export class CargoTrackingMenuItem{

    public ImageIconSource: string;
    public DescriptionText: string;
    public TranslatedName: string;
    public Code:string;
}
