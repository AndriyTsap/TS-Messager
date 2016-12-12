import { Component, OnInit, Inject, ViewChild} from '@angular/core';
import { UserFull } from "../core/domain/user-full";
import { OperationResult } from "../core/domain/operationResult";
import { UserService } from "../core/services/user.service";
import { NotificationService } from '../core/services/notification.service';
import { MembershipService } from '../core/services/membership.service';
import { Router } from '@angular/router';

@Component({
    selector: 'profile',
    templateUrl: './app/components/profile.component.html',
    styleUrls:[ './css/profile.component.css'],
})

export class ProfileComponent implements OnInit{
    //public photo:string = "https://drive.google.com/uc?id=0B9BOAdwQ-aZRYU41c2dRREFrRFE";
    @ViewChild("photo") photo;
    user:UserFull ;

    constructor(public userService: UserService,
                public membershipService: MembershipService,
                public notificationService: NotificationService,
                public router: Router) {
        this.user=new UserFull('','','','','','','','','');
    }

    ngOnInit() {
        this.userService.getByToken(localStorage.getItem("token")).subscribe(res => {
            this.user= res.json();
            },
            error => {
                if (error.status == 401 || error.status == 404) {
                    this.notificationService.printErrorMessage('User don\'t exist');
                }
            });   
    }
    onChangePhoto(event){
        console.log(event.target.files); 
        console.log(event);
    }


    addFile(): void {
        let photo = this.photo.nativeElement;
        if (photo.files && photo.files[0]) {
            let photoToUpload = photo.files[0];
            this.userService
                .uploadPhoto(photoToUpload)
                .subscribe(res => {
                    console.log(res);
                });
        }
    }

    save(){
        var _updateResult: OperationResult = new OperationResult(false, '');
        this.userService.update(localStorage.getItem("token"),this.user)
            .subscribe(res => {
                _updateResult.Succeeded = res.Succeeded;
                _updateResult.Message = res.Message;
            },
            error => console.error('Error: ' + error),
            () => {
                if (_updateResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Dear ' + this.user.Username + ', your date updated');
                    
                }
                else {
                    console.log(_updateResult.Message)
                    this.notificationService.printErrorMessage(_updateResult.Message);
                }
            });
    }

    delete(){
        var _removeResult: OperationResult = new OperationResult(false, '');
        this.notificationService.printConfirmationDialog('Are you sure you want to delete the account?',
            () => {
                this.userService.delete(localStorage.getItem("token"))
                    .subscribe(res => {
                        _removeResult.Succeeded = res.Succeeded;
                        _removeResult.Message = res.Message;
                    },
                    error => console.error('Error: ' + error),
                    () => {
                        if (_removeResult.Succeeded) {
                            this.notificationService.printSuccessMessage('Your account removed!');
                            this.membershipService.logout()
                                .subscribe(res => {
                                    localStorage.removeItem('user');
                                    localStorage.removeItem('token');
                                },
                                error => console.error('Error: ' + error),
                                () => { });
                            this.router.navigate(['home']);
                        }
                        else {
                            this.notificationService.printErrorMessage('Failed to remove account');
                        }
                    });
            });
    }

}

