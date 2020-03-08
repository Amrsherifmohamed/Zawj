import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
baseUrl=environment.apiUrl+'admin/';
  constructor(private http:HttpClient) { }
  getuserwithroles(){
    return this.http.get(this.baseUrl+"userWithRoles")
  }
  updateUserRoles(user:User,roles:{}){
    return this.http.post(this.baseUrl+"editroles/"+user.username,roles);
  }
  getPhotosForApproval() {
    return this.http.get(this.baseUrl + 'photosForModeration');
  }

  approvePhoto(photoId) {
    return this.http.post(this.baseUrl + 'approvePhoto/' + photoId, {});
  }

  rejectPhoto(photoId) {
    return this.http.post(this.baseUrl + 'rejectPhoto/' + photoId, {});
  }
}
