import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user'
// const httpOptions={
//   headers:new HttpHeaders(
//     {
//       'Authorization':'Bearer '+localStorage.getItem('token')
//     }
//   )
// }
//in app.module
@Injectable({
  providedIn: 'root'
})
export class UserService {
basurl=environment.ApiUrl+'Users/';
constructor(private http:HttpClient) { }
getusers():Observable<User[]>{
  return this.http.get<User[]>(this.basurl/*,httpOptions*/);
}
getuser(id):Observable<User>{
  return this.http.get<User>(this.basurl+id/*,httpOptions*/);
}
}
