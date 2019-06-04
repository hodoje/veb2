import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';
import { Directive, Input } from '@angular/core';

@Directive({
  selector: '[appPasswordPatternValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: PasswordPatternValidatorDirective,
    multi: true
  }]
})
export class PasswordPatternValidatorDirective implements Validator{

  validate(control: AbstractControl): { [key: string]: any} | null{
    let value = control.value;
    let validations = {
        "isValid": true,
        "notUpperCase": false,
        "notLowerCase": false,
        "notNumber": false,
        "notPunctuation": false
    };
    if(!this.checkIfContainsAtLeastOneUpperCaseLetter(value)){
        validations.notUpperCase = true;
        validations.isValid = false;
    }
    if(!this.checkIfContainsAtLeastOneLowerCaseLetter(value)){
        validations.notLowerCase = true;
        validations.isValid = false;
    }
    if(!this.checkIfContainsAtLeastOneNumber(value)){
        validations.notNumber = true;
        validations.isValid = false;
    }
    if(!this.checkIfContainsAtLeastOnePunctuationMark(value)){
        validations.notPunctuation = true;
        validations.isValid = false;
    }
    if(!validations.isValid){
        return validations;
    }
    else{
        return null;
    }
  }

  checkIfContainsAtLeastOneUpperCaseLetter(value: string){
    let regexp = new RegExp("(?=.*[A-Z])");
    return regexp.test(value);
  }

  checkIfContainsAtLeastOneLowerCaseLetter(value: string){
    let regexp = new RegExp("(?=.*[a-z])");
    return regexp.test(value);
  }

  checkIfContainsAtLeastOneNumber(value: string){
    let regexp = new RegExp("(?=.*[0-9])");
    return regexp.test(value);
  }

  checkIfContainsAtLeastOnePunctuationMark(value: string){
    let regexp = new RegExp("(?=.*[-+_!@#$%^&*.,?])")
    return regexp.test(value);
  }
}