import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
@Input() user:User;
  constructor(private authservice:AuthService,
    private alertify:AlertifyService,private userservice:UserService) { }

  ngOnInit() {
  }
  sendlike(id:number){
    this.userservice.sendLike(this.authservice.decodedToken.nameid,id).subscribe(
      ()=>{this.alertify.success("لقد قومت بالاعجاب ب"+this.user.knownAs);},
      error=>{this.alertify.error(error);}
    )
  }

}
