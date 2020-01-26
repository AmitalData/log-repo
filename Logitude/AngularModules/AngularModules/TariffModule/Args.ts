import { CodeNameClass } from '../Infrastructure/DataContracts/CodeNameClass';
import { TariffVersionPM } from './EntityPMs/TariffVersionPM';

export class UpdateTariffArgs {
    Version: TariffVersionPM;
    TariffCharges: CodeNameClass[];
    CarrierId: string;
    TypeCode: string;
    FatherComponent: any;
}
