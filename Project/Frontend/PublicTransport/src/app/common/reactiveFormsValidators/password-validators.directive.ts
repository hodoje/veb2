import { ValidatorFn, ValidationErrors, FormControl, FormGroup, AbstractControl } from '@angular/forms';

  export const checkIfContainsAtLeastOneUpperCaseLetterValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    if(!control.pristine){
      let value = control.value;
      let regexp = new RegExp("(?=.*[A-Z])");
      if(!regexp.test(value)){
        return {
          "notUpperCase": true
        };
      }
      return null;
    }
  }

  export const checkIfContainsAtLeastOneLowerCaseLetterValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    if(!control.pristine){
      let value = control.value;
      let regexp = new RegExp("(?=.*[a-z])");
      if(!regexp.test(value)){
        return {
          "notLowerCase": true
        };
      }
      return null;
    }
  }

  export const checkIfContainsAtLeastOneNumberValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    if(!control.pristine){
      let value = control.value;
      let regexp = new RegExp("(?=.*[0-9])");
      if(!regexp.test(value)){
        return {
          "notNumber": true
        };
      }
      return null;
    }
  }

  export const checkIfContainsAtLeastOnePunctuationMarkValidator: ValidatorFn = (control: FormControl): ValidationErrors | null => {
    if(!control.pristine){
      let value = control.value;
      let regexp = new RegExp("(?=.*[-+_!@#$%^&*.,?])")
      if(!regexp.test(value)){
        return {
          "notPunctuation": true
        };
      }
      return null;
    }
  }

  export function checkIfPasswordsEqualValidator(controlToCompareName: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if(control.parent){
        const controlToCompare = control.parent.get(controlToCompareName);
        if(controlToCompareName === "confirmPassword"){
          if(controlToCompare){
            if(controlToCompare.value === control.value){
              controlToCompare.setErrors(null);
              controlToCompare.markAsUntouched();
            }
            else{
              if(controlToCompare.dirty){
                if(controlToCompare.value){
                  controlToCompare.setErrors({
                    "notEqual": true
                  });
                  controlToCompare.markAsTouched();
                }
              }
            }
            return null;
          }
        }
        else if(controlToCompareName === "newPassword"){
          if(controlToCompare && controlToCompare.value !== control.value){
            return { "notEqual": true };
          }
          return null;
        }
      }
    }
  }

  export function checkOldNewPasswordValidator(controlToCompareName: string): ValidatorFn {
    // return (control: AbstractControl): ValidationErrors | null => {
    //   if(control.parent){
    //     const controlToCompare = control.parent.get(controlToCompareName);
    //     if(controlToCompareName === "newPassword"){
    //       if(controlToCompare){
    //         if(controlToCompare.value !== control.value){
    //           //controlToCompare.setErrors(null);
    //           controlToCompare.markAsUntouched();
    //           console.log(controlToCompare);
    //         }
    //         else{
    //           console.log(controlToCompare);
    //           // if(controlToCompare.touched){
    //             if(controlToCompare.value){
    //               controlToCompare.setErrors({
    //                 "oldNewEqual": true
    //               });
    //               controlToCompare.markAsTouched();
    //             }
    //           // }
    //           console.log(controlToCompare);
    //         }
    //         return null;
    //       }
    //     }
    //     else if(controlToCompareName === "oldPassword"){
    //       if(controlToCompare && controlToCompare.value === control.value){
    //         return { "oldNewEqual": true };
    //       }
    //       return null;
    //     }
    //   }
    // }
    return (control: AbstractControl): ValidationErrors | null => {
      if(control.parent){
        const controlToCompare = control.parent.get(controlToCompareName);
        if(controlToCompareName === "newPassword"){
          // in this case control is "oldPassword"
          if(controlToCompare){
            if(controlToCompare.value === control.value){
              controlToCompare.setErrors({
                "oldNewEqual": true
              });
            }
            // else{
            //   if(controlToCompare.value){
            //     controlToCompare.setErrors({"oldNewEqual": false});
            //     controlToCompare.markAsUntouched();
            //   }
            //   else{
            //     controlToCompare.setErrors({"oldNewEqual": false});
            //     controlToCompare.markAsTouched();
            //   }

            //   console.log(controlToCompare);
            // }
          }
        }
        else if(controlToCompareName === "oldPassword"){
          // in this case control is "newPassword"
          if(controlToCompare && controlToCompare.value === control.value){
            return { "oldNewEqual": true };
          }
          return null;
        }
      }
    }
  }