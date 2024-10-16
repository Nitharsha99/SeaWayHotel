import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { AdminService } from 'src/app/Services/AdminService/admin.service';
import { CloudinaryService } from 'src/app/Services/CloudinaryService/cloudinary.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-manager',
  templateUrl: './add-manager.component.html',
  styleUrls: ['./add-manager.component.css']
})
export class AddManagerComponent implements OnInit{

  updateMode: boolean = false;
  profilePic: string = "";
  file!: File;
  selectedPictures: string[] = [];
  fileSize: number = 0;
  managerId:number = 0;
  defaultPic: string = "../../../../../assets/images/profile.jpg";
  confirmPassword: string = '';

  constructor(private builder: FormBuilder, private router: Router,  private route: ActivatedRoute,
            private adminService: AdminService, private commonFunction: CommonFunctionComponent,
            private cloudinaryService: CloudinaryService
  ){}

  managerForm: FormGroup = this.builder.group({
    username:['', Validators.required],
    password:['', [Validators.required, Validators.maxLength(8), Validators.minLength(6)]],
    confirmPassword:['', [Validators.required]],
    isAdmin:[false],
    createdBy: ['Nitharsha'],
    updatedBy: [''],
    picName: [null],
    picPath: [null],
    publicId: [null]
  }, { validators: this.checkPassword});

  ngOnInit(): void {
    if(!this.updateMode){
      this.profilePic = this.defaultPic;
    }
    else{

    }
  }

  onSelectProfilePic(event: any): void{
    console.log(event);
    const files = event.addedFiles;
    if (files && files.length > 0) {
        this.file = files[0];
        this.fileSize = this.file.size;
    }
    console.log('file', this.file, this.profilePic);
  }

  onRemove(): void{
    this.file = new File([], '');
    this.fileSize = 0;
    console.log(this.file);
  }

  redirectToBack(): void{
    if(this.updateMode){
      this.router.navigate(['../../../Managers'], {relativeTo: this.route});
    }
    else{
      this.router.navigate(['../../Managers'], {relativeTo: this.route});
    }
  }

  resetForm(): void{
    this.managerForm.reset();
    this.file = new File([], '');
  }

  addManager(): void{
    if(this.fileSize > 0){
      this.uploadImage();
    }
    else{
      this.callManagerService();
    }
    
  }

  checkPassword(group: FormGroup): any{
    const passwordControl = group.get('password');
    const confirmPasswordControl = group.get('confirmPassword');

    if (passwordControl && confirmPasswordControl) {
      const password = passwordControl.value;
      const confirmPassword = confirmPasswordControl.value;
      return password === confirmPassword ? null : { mismatch: true };
    } else {
      return null; 
    }
  }

  callManagerService(){
    const formValue = this.managerForm.value;

    if(this.updateMode){

    }
    else{
      formValue.createdBy = 'Nitharsha';
      this.adminService.PostAdmin(formValue).subscribe(res => {
        Swal.fire({
          title: "Manager Saved Successfully!!",
          icon: "success",
          iconColor: '#570254',
          showConfirmButton: true,
          confirmButtonColor: '#570254'
        });
        this.resetForm();
       },
       (error) =>{
        this.commonFunction.ShowErrorPopup(error);
       }
      );
    }
  }

  async uploadImage(){
    const data = new FormData();
    data.append('file', this.file);
    data.append('upload_preset', 'seaway_hotel');
    data.append('cloud_name', 'dly7yjg1w');

    this.cloudinaryService.uploadImage(data).subscribe(res => {
      console.log(res.public_id, res);

       this.managerForm.patchValue({
        picName: res.original_filename + "." + res.format,
        picPath: res.url,
        publicId: res.public_id
      });
      this.callManagerService();
    })
     this.commonFunction.showLoadingNotification();
  }
}
