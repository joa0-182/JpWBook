import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm! : FormGroup;

  constructor(private fb: FormBuilder){}

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.compose([Validators.required, Validators.minLength(6)])]
    });
  }

  get f(){
    return this.registerForm.controls;
  }


  checkName(){
    return this.f['name'].dirty && this.f['name'].errors?.['required'];
  }

  
  checkEmail(){
    return this.f['email'].dirty && this.f['email'].errors?.['required'];
  }

  checkEmailValid(){
    return this.f['email'].dirty && this.f['email'].errors?.['email'];
  }

  checkPassword(){
    return this.f['password'].dirty && this.f['password'].errors?.['required'];
  }
  checkPasswordlength(){
    return this.f['password'].dirty && this.f['password'].errors?.['minlength'];
  }
  onSubmit(){
    if(this.registerForm.valid){
      //enviar os dados ao backend
      console.log(this.registerForm.value);
    }else{
      //disparar o erro
      this.validateAllFormField(this.registerForm);
    }
  }

  private validateAllFormField(formGroup: FormGroup){
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if(control instanceof FormControl){
        control.markAsDirty({ onlySelf: true});
      } else if (control instanceof FormGroup) {
        this.validateAllFormField(control);
      }
    });
  }

}
