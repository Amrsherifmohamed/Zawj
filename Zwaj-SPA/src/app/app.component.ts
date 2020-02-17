import { Component, OnInit } from '@angular/core';
import { AuthService } from './_services/auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit { 
  /**
   *
   */
  jwtHelper=new JwtHelperService();
  constructor(public authservic:AuthService) {}
   ngOnInit(){
    const token=localStorage.getItem('token');
    const user =JSON.parse(localStorage.getItem('user'));
    if(token){
      this.authservic.decodedToken=this.jwtHelper.decodeToken(token);
    }
    if(user){
      this.authservic.currentUser=user;
      this.authservic.changeMemberPhoto(this.authservic.currentUser.photourl);
    }

  }
  }
