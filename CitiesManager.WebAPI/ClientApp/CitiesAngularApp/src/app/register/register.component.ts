import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { AccountService } from '../services/account.service';
import { RegisterUser } from '../models/register-user';
import { CompareValidation } from '../validators/custom-validators';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  isRegisterFormSubmitted: boolean = false;
  registerForm: FormGroup;
  errorMessages:string[]=[];
  err:string='';

  constructor(private accountService:AccountService, private router : Router) {
    this.registerForm = new FormGroup({
      personName: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required]),
      phoneNumber: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required,Validators.minLength(5)]),
      confirmPassword: new FormControl(null, [Validators.required]),
      dateOfBirth:new FormControl(null,[Validators.required]),
      currentSalary:new FormControl(null,[Validators.required]),
      expectedSalary: new FormControl(null,[Validators.required])
    }, {
        validators:[CompareValidation("password","confirmPassword")]
      });    
  }
  get register_personNameControl(): any {
    return this.registerForm.controls["personName"];
  }
  get register_emailControl(): any {
    return this.registerForm.controls["email"];
  }

  get register_phoneNumberControl(): any {
    return this.registerForm.controls["phoneNumber"];
  }

  get register_passwordControl(): any {
    return this.registerForm.controls["password"];
  }

  get register_confirmPasswordControl(): any {
    return this.registerForm.controls["confirmPassword"];
  }
  get register_dateOfBirthControl(): any {
    return this.registerForm.controls["dateOfBirth"];
  }

  get register_currentSalaryControl(): any {
    return this.registerForm.controls["currentSalary"];
  }

  get register_expectedSalaryControl(): any {
    return this.registerForm.controls["expectedSalary"];
  }

  registerSubmitted() {
    this.isRegisterFormSubmitted = true;
    
    if (this.registerForm.valid) {
      this.accountService.postRegister(this.registerForm.value).subscribe({
        next: (response: any) => {
          console.log(response);

          this.isRegisterFormSubmitted = false;
          this.accountService.currentUserName = response.email;
          localStorage["token"] = response.token;
          localStorage["refreshToken"] = response.refreshToken;
          this.router.navigate(['/cities']);
          this.registerForm.reset();
        },

        error: (errors) => {
          this.errorMessages = [];
          // error.error.errors.foreach((err)=>{
          //   this.errorMessages.push(err);
          // })
          Object.keys(errors.error.errors).forEach(key => {
            this.errorMessages.push(errors.error.errors[key][0]);
        });
          // this.errorMessages.push(error.error.errors?.['DateOfBirth'][0]);
          // this.errorMessages.push(error.error.errors?.['ExpectedSalary'][0]);
          //console.log(error.errors[Object.keys(error.error)[0]]);
          console.log(errors);
          alert('Error occured in registering the user');
        },

        complete: () => { },
      });
    }
  }

}
