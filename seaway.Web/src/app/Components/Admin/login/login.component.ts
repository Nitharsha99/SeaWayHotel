import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/Services/AuthService/auth.service';
import { LoginService } from 'src/app/Services/LoginService/login.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{

  visible: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private builder: FormBuilder,
            private loginService: LoginService, private authService: AuthService
  ){}

  loginForm: FormGroup = this.builder.group({
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.maxLength(8), Validators.minLength(6)]],
    isAdmin: [false]
})

  ngOnInit(): void {
    
  }

  login(){
    console.log(this.loginForm.value);
    var formValue = this.loginForm.value;

    if(this.loginForm.valid){
      this.loginService.login(formValue).subscribe({
        next: (res: any) => {
          localStorage.setItem('token', res.token);
          Swal.fire({
            title: "Successfully Login!",
            text: "Welcome " + formValue.username + " !",
            icon: "success",
            showConfirmButton: false,
            timer: 1500
          }).then(() => {
            this.router.navigate(['Home'], { relativeTo: this.route });
          });
        },
        error: err => {
          if(err.status == 400){
            Swal.fire({
              title: "Login Failed!",
              text: "Incorrect username or Password",
              icon: "error"
            })
          }
          else
            console.log('error during login:\n', err);
        }
      })
    }
  }

  passwordVisible(){
    this.visible = !this.visible;
  }

}
