import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from "rxjs/operators";
import {JwtHelperService} from '@auth0/angular-jwt';
import {environment} from "src/environments/environment";
import { User } from '../_models/user';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper=new JwtHelperService();
basurl=environment.ApiUrl+'auth/';
decodedToken:any;
currentUser:User;
photoUrl=new BehaviorSubject<string>('../../assets/User.png');
currentphotoUrl=this.photoUrl.asObservable();
constructor(private http:HttpClient) { }
changeMemberPhoto(newPhotoUrl:string){
this.photoUrl.next(newPhotoUrl);
}
login(model:any){
  return this.http.post(this.basurl+"login",model).pipe(
    map((respons:any)=>{
      const user=respons;
      if(user){
        localStorage.setItem('token',user.token);
        localStorage.setItem('user',JSON.stringify(user.user));
      this.decodedToken=this.jwtHelper.decodeToken(user.token);
      this.currentUser=user.user;
      this.changeMemberPhoto(this.currentUser.photourl);

     // console.log(this.decodedToken);
    }}))

}
register(user:User){
return this.http.post(this.basurl+"register",user);
}
logedin(){
  try{
  const token=localStorage.getItem('token');
  return ! this.jwtHelper.isTokenExpired(token);
  }catch{
    return false
  }
}
}
