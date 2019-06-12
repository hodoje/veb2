import { ValidatorFn, ValidationErrors, FormControl, FormGroup, AbstractControl } from '@angular/forms';

  export const checkIfContainsAtLeastOneUpperCaseLetterValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    let value = control.value;
    let regexp = new RegExp("(?=.*[A-Z])");
    if(!regexp.test(value)){
      return {
        "notUpperCase": true
      }
    }
    return null;
  }

  export const checkIfContainsAtLeastOneLowerCaseLetterValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    let value = control.value;
    let regexp = new RegExp("(?=.*[a-z])");
    if(!regexp.test(value)){
      return {
        "notLowerCase": true
      }
    }
    return null;
  }

  export const checkIfContainsAtLeastOneNumberValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    let value = control.value;
    let regexp = new RegExp("(?=.*[0-9])");
    if(!regexp.test(value)){
      return {
        "notNumber": true
      }
    }
    return null;
  }

  export const checkIfContainsAtLeastOnePunctuationMarkValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    let value = control.value;
    let regexp = new RegExp("(?=.*[-+_!@#$%^&*.,?])")
    if(!regexp.test(value)){
      return {
        "notPunctuation": true
      }
    }
    return null;
  }

  export function checkIfPasswordsEqualValidator(controlToCompareName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if(control.parent){
        const controlToCompare = control.parent.get(controlToCompareName);
        if(controlToCompareName === "confirmPassword"){
          // in this case control is "password"
          if(controlToCompare){
            if(controlToCompare.value === control.value){
              controlToCompare.markAsUntouched();
              controlToCompare.setErrors({
                "notEqual": false
              });
            }
            else{
              if(controlToCompare.dirty){
                if(controlToCompare.value){
                  controlToCompare.markAsTouched();
                  controlToCompare.setErrors({
                    "notEqual": true
                  })
                }
              }
            }
            return null;
          }
        }
        else if(controlToCompareName === "newPassword"){
          // in this case control is "confirmPassword"
          if(controlToCompare && controlToCompare.value !== control.value){
            return { "notEqual": true };
          }
          return null;
        }
        else if(controlToCompareName === "oldPassword"){
          if(controlToCompare && controlToCompare.value === control.value){
            return { "oldNewEqual": true };
          }
          return null;
        }
      }
    }
  }