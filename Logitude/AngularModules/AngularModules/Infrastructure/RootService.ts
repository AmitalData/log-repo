import { ComponentRef } from "@angular/core";
import { ServiceHelper } from "./Utilities/ServiceHelper";
import { SessionLocator } from "./Utilities/SessionLocator";
import { LandingPageComponent } from "./Components/LandingPage/LandingPageComponent";

export class RootService {
    public static redirectToExternalLink(): boolean {
        const menuName = SessionLocator.ExternalParams?.Menu?.toLocaleLowerCase();
        const token: string = new URLSearchParams(window.location.search).get('Token');

        if (menuName !== 'redi' || !token)
            return false;

        const origin: string = window.location.origin.replace('localhost:4200', 'localhost:9996');
        ServiceHelper.HttpClient.get(origin + '/api/ExternalLink/GetForward?Token=' + token).subscribe(
            (response: any) => location.href = response,
            async (error: any) => {
                const errorMessage = error?.error || 'An error occurred while processing your request.';
                const cmpRef: ComponentRef<LandingPageComponent> = await SessionLocator.DynamicLoader.Load(
                    "./Infrastructure/Components/LandingPage/LandingPageComponent",
                    SessionLocator.RootComponent.Child.Location);
                cmpRef.instance.message = errorMessage;
                cmpRef.instance.error = true;
            }
        );

        return true;
    }
}