import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user'
import { PaginationResult } from '../_models/Pagination';
import { map } from 'rxjs/operators';
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

getusers(page?,itemsPerPage?,userParams?):Observable<PaginationResult<User[]>>{
  const paginationResult : PaginationResult<User[]> = new PaginationResult<User[]>();
  let params = new HttpParams();
  if(page != null&& itemsPerPage != null){
    params = params.append('pageNumber',page);
    params= params.append('pageSize',itemsPerPage);
  }
  if(userParams != null){
    params = params.append('minAge',userParams.minAge);
    params= params.append('maxAge',userParams.maxAge);
    params= params.append('gender',userParams.gender);
    params= params.append('orderBy',userParams.orderBy);
  }
  return this.http.get<User[]>(this.basurl,{observe:'response',params}).pipe(
    map(response=>{
      paginationResult.result=response.body;
      if(response.headers.get('Pagination') != null){
        paginationResult.pagination = JSON.parse(response.headers.get('Pagination'))
      }
      return paginationResult;
    })
    
  );
}
getuser(id:number):Observable<User>{
  return this.http.get<User>(this.basurl+id/*,httpOptions*/);
}
updateuser(id:number,user:User){
  return this.http.put(this.basurl+id,user);
}
setMainPhoto(userid:number,id:number){
  return this.http.post(this.basurl+userid+'/photos/'+id+'/setMain',{});
}
deletephoto(userid:number,id:number){
  return this.http.delete(this.basurl+userid+'/photos/'+id);
}
}
