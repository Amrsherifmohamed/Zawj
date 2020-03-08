import { Component, OnInit } from '@angular/core';
import { AdminService } from 'src/app/_services/admin.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-managment',
  templateUrl: './photo-managment.component.html',
  styleUrls: ['./photo-managment.component.css']
})
export class PhotoManagmentComponent implements OnInit {

  constructor(private adminService: AdminService , private alertify:AlertifyService) { }
  photos: any[];
  ngOnInit() {
    this.getPhotosForApproval();
  }
   getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe((photos:any[]) => {
      this.photos = photos;
    }, () => {
      this.alertify.error('توجد مشكلة في عرض الصور');
    });
  }

  approvePhoto(photoId) {
    this.adminService.approvePhoto(photoId).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
    }, () => {
      this.alertify.error('توجد مشكلة في قبول الصورة');
    });
  }

  rejectPhoto(photoId) {
    this.adminService.rejectPhoto(photoId).subscribe(() => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId), 1);
    }, () => {
      this.alertify.error('توجد مشكلة في رفض الصورة');
    });
  }

}
