import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from "rxjs/operators";
import {JwtHelperService} from '@auth0/angular-jwt';
import {environment} from "src/environments/environment";
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper=new JwtHelperService();
basurl=environment.ApiUrl+'auth/';
decodedToken:any;
constructor(private http:HttpClient) { }
login(model:any){
  return this.http.post(this.basurl+"login",model).pipe(
    map((respons:any)=>{
      const user=respons;
      if(user){localStorage.setItem('token',user.token);
      this.decodedToken=this.jwtHelper.decodeToken(user.token);
     // console.log(this.decodedToken);
    }}))

}
register(model:any){
return this.http.post(this.basurl+"register",model);
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
