import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AdminService } from 'src/app/Services/AdminService/admin.service';
import { AuthService } from 'src/app/Services/AuthService/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{

  visible: boolean = false;

  constructor(private router: Router, private route: ActivatedRoute, private builder: FormBuilder,
            private adminService: AdminService, private authService: AuthService
  ){}

  loginForm: FormGroup = this.builder.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
})

  ngOnInit(): void {
    
  }

  login(){
    console.log(this.loginForm.value);
    var formValue = this.loginForm.value;

    if(!formValue.username || !formValue.password){
      Swal.fire({
        title: "Login Failed!",
        text: "Please fill all mandatory fields...",
        icon: "error"
      });
    }
    else{
      this.adminService.login(formValue).subscribe(res => {
        console.log("bool", res);
        if(res == true){
          this.authService.login(formValue.username, formValue.password);
          Swal.fire({
            title: "Successfully Login!",
            text: "Welcome " + formValue.username + " !",
            icon: "success"
          }).then((result) => {
            if (result.isConfirmed) {
              this.router.navigate(['Home'], { relativeTo: this.route });
            }
          });
        }
        else{
          Swal.fire({
            title: "Login Failed!",
            text: "Invalid Username or Password",
            icon: "error"
          });
        }
      })
    }
  }

  passwordVisible(){
    this.visible = !this.visible;
  }

}
