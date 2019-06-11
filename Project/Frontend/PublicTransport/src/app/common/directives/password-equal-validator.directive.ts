import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { Directive, Input } from '@angular/core';

@Directive({
  selector: '[appPasswordEqualValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: PasswordEqualValidatorDirective,
    multi: true
  }]
})
export class PasswordEqualValidatorDirective implements Validator{

  @Input()
  appPasswordEqualValidator: string

  validate(control: AbstractControl): { [key: string]: any} | null{
    const controlToCompare = control.parent.get(this.appPasswordEqualValidator);
    if(controlToCompare && controlToCompare.value !== control.value){
      return { "notEqual": true };
    }
    return null;
  }
}