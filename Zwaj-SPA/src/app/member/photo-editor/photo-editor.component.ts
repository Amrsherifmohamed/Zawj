import { Component, OnInit, Input } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { error } from 'util';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[]
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.ApiUrl;
  curretphoto :Photo;
  user:User;


  constructor(private authService: AuthService,
    private userservice:UserService,private route:ActivatedRoute,private alertifyservice:AlertifyService)  
    { }

  ngOnInit() {
    this.initializeUploader();
    this.route.data.subscribe(data=>{
      this.user=data['user'];
    })
  }


  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader(
      {
        url: this.baseUrl+'users/'+this.authService.decodedToken.nameid+'/photos',
        authToken: 'Bearer '+localStorage.getItem('token'),
        isHTML5: true,
        allowedFileType: ['image'],
        removeAfterUpload: true,
        autoUpload: false,
        maxFileSize: 10 * 1024 * 1024,
        
      }
    );
    this.uploader.onAfterAddingFile=(file)=>{file.withCredentials=false;};
    this.uploader.onSuccessItem=(item,Response,status,headers)=>{
      if(Response){
        const res:Photo = JSON.parse(Response);
        const photo ={
          id:res.id,
          url:res.url,
          dateAdded:res.dateAdded,
          isMain:res.isMain
        };
        this.photos.push(photo);
        if(photo.isMain){
          this.authService.changeMemberPhoto(photo.url);
          this.authService.currentUser.photourl=photo.url;
          localStorage.setItem('user',JSON.stringify(this.authService.currentUser));
        }
      }
    }
    
  }
  setMainphoto(photo:Photo){
     this.userservice.setMainPhoto(this.authService.decodedToken.nameid,photo.id).subscribe(
       ()=>{this.curretphoto=this.photos.filter(p=>p.isMain===true)[0];
      this.curretphoto.isMain=false;
      photo.isMain=true;
      // this.user.photourl=photo.url; 
      this.authService.changeMemberPhoto(photo.url);
      this.authService.currentUser.photourl=photo.url;
      localStorage.setItem('user',JSON.stringify(this.authService.currentUser));

      },
       ()=>{this.alertifyservice.error("حدث خطا ما ");}
     )
  }
  delete(id:number){
    this.alertifyservice.confirm("هل تريد حذف تلك الصورة",()=>{
      this.userservice.deletephoto(this.authService.decodedToken.nameid,id).subscribe(
        ()=>{
          this.photos.splice(this.photos.findIndex(p=>p.id===id),1);
          this.alertifyservice.success("تم حذف الصورة بنجاح");
        },
        error=>{this.alertifyservice.error("حدث خطأ أثناء حذف الصورة");}

      );
    });
  }

}
