import {ViewContainerRef, Output, EventEmitter, ViewChild} from '@angular/core';
 
export class RootContext {
 
 
  public static AppComponent: any;
 
   
 
    public static StartBusyIndicator(myText: string) {
      RootContext.AppComponent.BusyIndicatorText = myText;
      RootContext.AppComponent.ShowBusyIndicator = true;
    }
    public static StartBusyIndicatorSaving() {
      RootContext.StartBusyIndicator('Saving');
    }
    public static StartBusyIndicatorLoading() {
      RootContext.StartBusyIndicator('Loading');
    }
    public static StartBusyIndicatorCreating() {
      RootContext.StartBusyIndicator("Creating");
  }

  public static StartBusyIndicatorRemoving() {
    RootContext.StartBusyIndicator("Removing");
  }

  public static StartBusyIndicatorUpdating() {
    RootContext.StartBusyIndicator("Updating");
  }
    public static StopBusyIndicator() {
      RootContext.AppComponent.BusyIndicatorText = null;
      RootContext.AppComponent.ShowBusyIndicator = false;
    }
}

export class SessionIdCounter {
    public Name: string;
    public Counter: number;
    constructor(name: string) {
        this.Name = name;
        this.Counter = 0;
    }
}
export class UnReadChat {
  public SenderId: string;
  public IsFocus: boolean;
  public IsDeleted:boolean = false;
  constructor(SenderId: string, IsFocus: boolean) {
    this.SenderId = SenderId;
    this.IsFocus = IsFocus;
  }
}



