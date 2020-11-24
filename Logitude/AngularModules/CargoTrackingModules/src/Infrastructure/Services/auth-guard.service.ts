import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, CanActivate, Router, RouterState, RouterStateSnapshot } from '@angular/router';
import { AuthService } from 'src/app/auth.service';

@Injectable()
export class AuthGuardService implements CanActivate {
  constructor(public auth: AuthService, public router: Router, public activatedRoute: ActivatedRoute) {}
  
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    let url: string = state.url;

    return this.checkLogin(url);
  }

  checkLogin(url: string): boolean {
    if (this.auth.isAuthenticated()) { return true; }

    this.auth.redirectUrl = url;
    
    let tenant = this.activatedRoute.snapshot.children[0]?.params.Tenant;
    if(!tenant)
      tenant = this.activatedRoute.snapshot.children[0]?.queryParams.tenant;
    if(tenant)
      this.router.navigate(["login"],{ queryParams: {tenant: tenant}});
    else
      this.router.navigate(["login"]);
    
    return false;
  }
}
