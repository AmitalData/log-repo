import { RouteDirection } from "./RouteDirection";

export class RoutingStep
{
    IsActive: boolean;
    FromPortLabel: string;
    ToPortLabel: string;
    ToolTipFromPortLabel: string;
    ToolTipToPortLabel: string;
    Description: string;
    TransportModeCode: string;
    Directions: RouteDirection[] = [];
    Type: string;
}
