import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { Directive, Input } from '@angular/core';

@Directive({
  selector: '[appConfirmPasswordValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: ConfirmPasswordValidatorDirective,
    multi: true
  }]
})
export class ConfirmPasswordValidatorDirective implements Validator{

  @Input()
  appConfirmPasswordValidator: string

  validate(control: AbstractControl): { [key: string]: any} | null{
    const controlToCompare = control.parent.get(this.appConfirmPasswordValidator);
    if(controlToCompare && controlToCompare.value !== control.value){
      return { "notEqual": true };
    }
    return null;
  }
}