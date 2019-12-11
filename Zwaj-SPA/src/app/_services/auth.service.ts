import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from "rxjs/operators";
@Injectable({
  providedIn: 'root'
})
export class AuthService {
basurl='http://localhost:5000/api/auth/';

constructor(private http:HttpClient) { }
login(model:any){
  return this.http.post(this.basurl+"login",model).pipe(
    map((respons:any)=>{
      const user=respons;
      if(user){localStorage.setItem('token',user.token);}}))
}
register(model:any){
return this.http.post(this.basurl+"register",model);
}
}
