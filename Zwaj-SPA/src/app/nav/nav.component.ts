import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model:any={};
  photoUrl:string;

  constructor(public authservic:AuthService,private alertify:AlertifyService,private router:Router) { }

  ngOnInit() {
    this.authservic.currentphotoUrl.subscribe(
      photoUrl=>this.photoUrl=photoUrl
    );
  }
  login(){
    this.authservic.login(this.model).subscribe(
      next=>{this.alertify.success("تم تسجيل الدخول بنجاح ")},// api succses to connect
      error=>{this.alertify.error(error)},//api field to conect
      ()=>{this.router.navigate(["/member"])}//rout when success go to page member
    )
  }
  logedin(){
    
    return this.authservic.logedin();
  }
  logedout(){
    localStorage.removeItem('token');
    this.authservic.decodedToken=null;
    localStorage.removeItem('user');
    this.authservic.currentUser=null;
    this.alertify.success("تم تسجيل خروجك"); //success to logedout
    this.router.navigate(["/home"]);//route to do to home
  }
}
