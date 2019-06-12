import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { Directive } from '@angular/core';
import { AccountHttpService } from 'src/app/services/account-http.service';

@Directive({
  selector: '[appEmailExistsValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: EmailExistsValidatorDirective,
    multi: true
  }]
})
export class EmailExistsValidatorDirective implements Validator{

    constructor(private accountService: AccountHttpService){}

    validate(control: AbstractControl): { [key: string]: any} | null{
        let email = control.value;
        this.accountService.checkIfEmailExists(email).subscribe(
            (data: any) => {
                if(data.exists){
                    control.setErrors({
                        "alreadyExists": true
                    });
                    control.markAsUntouched();
                }
                else{
                    return null;
                }
            }
        );
        return null;
    }
}