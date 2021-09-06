
export class RootContext
{

    public static AppComponent: any;
    public static ShipmentsScrollPosition: number = 0;
    public static LastSearchText: string = '';

    public static StartBusyIndicator(myText: string)
    {
        RootContext.AppComponent.BusyIndicatorText = myText;
        RootContext.AppComponent.ShowBusyIndicator = true;
    }
    public static StartBusyIndicatorSaving()
    {
        RootContext.StartBusyIndicator('Saving');
    }
    public static StartBusyIndicatorLoading()
    {
        RootContext.StartBusyIndicator('Loading');
    }
    public static StartBusyIndicatorCreating()
    {
        RootContext.StartBusyIndicator("Creating");
    }

    public static StartBusyIndicatorRemoving()
    {
        RootContext.StartBusyIndicator("Removing");
    }

    public static StartBusyIndicatorUpdating()
    {
        RootContext.StartBusyIndicator("Updating");
    }
    public static StopBusyIndicator()
    {
        RootContext.AppComponent.BusyIndicatorText = null;
        RootContext.AppComponent.ShowBusyIndicator = false;
    }
}


