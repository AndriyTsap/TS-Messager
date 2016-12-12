import { RegistrationDetails } from "./registrationDetails";
export class UserFull extends RegistrationDetails {
    Username:string;
    Password:string;
    Email:string;

    constructor(username: string,
        password: string,
        email:string,
        firstName: string,
        lastName: string,
        phone: string,
        birthDate: string,
        photo: string,
        about: string) {
        super(firstName, lastName, phone, birthDate, photo, about);
        this.Username = username;
        this.Password = password;
        this.Email = email;
    }
}