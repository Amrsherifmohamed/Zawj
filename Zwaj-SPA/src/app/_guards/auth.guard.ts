import { Injectable } from '@angular/core';
import { CanActivate, Router} from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  /**
   *
   */
  constructor(private authservice:AuthService,private router:Router,private altefiy:AlertifyService) {}
  canActivate(): boolean {
    if(this.authservice.logedin()){return true;}
    this.altefiy.error("يجب عليك تسجيل الدخول");
    this.router.navigate(['']);
    return false;
  }
}
