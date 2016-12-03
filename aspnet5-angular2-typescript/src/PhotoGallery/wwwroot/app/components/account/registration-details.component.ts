import { Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { RegistrationDetails } from '../../core/domain/registration-details';
import { OperationResult } from '../../core/domain/operationResult';
import { MembershipService } from '../../core/services/membership.service';
import { NotificationService } from '../../core/services/notification.service';

@Component({
    selector: 'details',
    providers: [MembershipService, NotificationService],
    templateUrl: './app/components/account/registration-details.component.html'
})
export class RegistrationDetailsComponent implements OnInit {

    private _updatingUser: RegistrationDetails;

    constructor(public membershipService: MembershipService,
                public notificationService: NotificationService,
                public router: Router) { }

    ngOnInit() {
        this._updatingUser = new RegistrationDetails('', '', '','','','');
    }

    addDetails(): void {
        /*var _registrationResult: OperationResult = new OperationResult(false, '');
        this.membershipService.register(this._updatingUser)
            .subscribe(res => {
                _registrationResult.Succeeded = res.Succeeded;
                _registrationResult.Message = res.Message;

            },
            error => console.error('Error: ' + error),
            () => {
                if (_registrationResult.Succeeded) {
                    this.notificationService.printSuccessMessage('Dear ' + this._newUser.Username + ', please login with your credentials');
                    this.router.navigate(['account/login']);
                }
                else {
                    this.notificationService.printErrorMessage(_registrationResult.Message);
                }
            });*/
    };
}