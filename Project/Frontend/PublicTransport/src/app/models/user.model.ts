import { UserType } from './user-type.model';

export class User{
    name: string;
    lastname: string;
    birthday: Date;
    address: string;
    email: string;
    registrationStatus: string;
    userType: UserType;
    documentImage: File;
    banned: boolean;
    role: string;
}