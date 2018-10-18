import {InfrastructureDomainService} from './InfrastructureDomainService';

export class Services {
    public static InfrastructureDomainService: InfrastructureDomainService;
    public static BuildServices() {
        this.InfrastructureDomainService = new InfrastructureDomainService();
    }
}

