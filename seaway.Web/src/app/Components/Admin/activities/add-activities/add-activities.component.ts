import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { PicDocument } from 'src/app/Models/picDocument';
import { ActivityService } from 'src/app/Services/ActivityService/activity.service';
import { CloudinaryService } from 'src/app/Services/CloudinaryService/cloudinary.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-activities',
  templateUrl: './add-activities.component.html',
  styleUrls: ['./add-activities.component.css']
})
export class AddActivitiesComponent implements OnInit{

  updateMode: boolean = false;
  pictures: PicDocument[] = [];
  imageWidth: number = 170;
  imageMargin: number = 2;
  imageHeight: number = 110;
  files: File[] = [];
  selectedPictures: string[] = [];
  activityId !: number;
  picArrayLength: number = 0;

  constructor(private builder: FormBuilder, private router: Router,  private route: ActivatedRoute,
              private activityService: ActivityService, private commonFunction: CommonFunctionComponent,
              private cloudinaryService: CloudinaryService
  ){}

  activityForm: FormGroup = this.builder.group({
    activityName:[''],
    description:[''],
    createdBy: ['Nitharsha'],
    updatedBy: [''],
    activityPics: this.builder.array([
      this.builder.group({
        picName: [null],
        picValue: [null],
        cloudinaryPublicId: [null]
       })
    ])
  })

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if(params['id']){
        this.activityId = params['id'];
        this.updateMode = true;
        this.activityService.FindActivityById(this.activityId).subscribe(res => {
          if(res != null){
            this.activityForm.patchValue(res);
            if(res.activityPics != null && res.activityPics.length > 0){
              this.pictures = res.activityPics;
              this.pictures.forEach(element => {
                const urlLink = this.commonFunction.convertBase64ToString(element.picValue);
                element.picValue = urlLink;
              });
            }
          }
        })
      }
    })
  }

  addActivity(){
    const formValue = this.activityForm.value;

    if(!formValue.activityName || !formValue.description){
      this.commonFunction.showMandoryFieldPopup();
    }
    else{
      if(this.files.length > 0){
        this.uploadImage();
      }
      else{
        formValue.activityPics = null;
        this.callActivityService();
      }
    }
  }

  async uploadImage(){
    const formValue = this.activityForm.value;
    this.picArrayLength = this.files.length;
    let count = 0;

    for(let i = 0; i < this.files.length; i++){
      const file_data = this.files[i];
      const data = new FormData();
      data.append('file', file_data);
      data.append('upload_preset', 'seaway_hotel');
      data.append('cloud_name', 'dly7yjg1w');

      this.cloudinaryService.uploadImage(data).subscribe(res => {
        console.log('i value', i);
        const activityPicsArray = this.activityForm.get('activityPics') as FormArray;
        while (activityPicsArray.length <= i) {
          activityPicsArray.push(this.builder.group({
            picName: [null],
            picValue: [null],
            cloudinaryPublicId: [null]
          }));
        }
        console.log(res.public_id, res);

        (activityPicsArray.at(i) as FormGroup).patchValue({
          picName: res.original_filename + "." + res.format,
          picValue: res.url,
          cloudinaryPublicId: res.public_id
        });

        formValue.activityPics =activityPicsArray.controls.map((control: AbstractControl<any>) => {
          const formGroup = control as FormGroup;
          return {
            picName: formGroup.get('picName')?.value,
            picValue: formGroup.get('picValue')?.value,
            cloudinaryPublicId: formGroup.get('cloudinaryPublicId')?.value
          };
        });

        count++;
       
        if(count === this.picArrayLength){
          Swal.close();
          this.callActivityService();
        }
      })
       this.commonFunction.showLoadingNotification();
    }
  }

  callActivityService(): void{
    const formValue = this.activityForm.value;
    if(this.updateMode === false){
      formValue.createdBy = 'Nitharsha';
      this.activityService.PostActivity(formValue).subscribe((res) => {
        Swal.fire({
          title: "Activity Saved Successfully!!",
          icon: "success",
          iconColor: '#570254',
          showConfirmButton: true,
          confirmButtonColor: '#570254'
        });
        this.files = [];
        this.resetForm();
       },
       (error) =>{
        this.commonFunction.ShowErrorPopup(error);
       }
      )
    }
    else{
      formValue.updatedBy = 'Nitharsha';
      if(this.activityId != null){
        this.activityService.UpdateActivity(formValue, this.activityId).subscribe(res => {
          Swal.fire({
            title: "Activity Updated Successfully!!",
            icon: "success",
            iconColor: '#570254',
            showConfirmButton: true,
            confirmButtonColor: '#570254'
          }).then(() => {
            this.files = [];
            setTimeout(() => {
            window.location.reload();
          });
          });
        },
        (error) =>{
          this.commonFunction.ShowErrorPopup(error);
         }
      )
      }
    }
  }

  updateSelectedPics(id: string, event: any): void{
    if(event.target.checked){
      this.selectedPictures.push(id);
    }
    else{
      const index = this.selectedPictures.indexOf(id);
      if (index !== -1) {
        this.selectedPictures.splice(index, 1);
      }
    }
  }

  deleteImages(){
    this.commonFunction.showDeleteNotification().then((result: { isConfirmed: any; dismiss: Swal.DismissReason; }) => {
    if(result.isConfirmed){
      this.activityService.DeleteImages(this.selectedPictures).subscribe((res) =>{
        if(res.includes("Deleted")){
          Swal.fire({
            icon: "success",
            title: "Successfully Deleted Images!!! ",
            showConfirmButton: true,
            iconColor: '#570254',
            confirmButtonColor: '#570254'
          }).then(() => {
            setTimeout(() => {
              window.location.reload();
            });
          });
        }
  
      });
    }
    else if (result.dismiss === Swal.DismissReason.cancel) {
      setTimeout(() => {
        window.location.reload();
      });
   }
  });
  }

  onSelect(event: any): void{
    console.log(event);
    this.files.push(...event.addedFiles);
  }

  onRemove(event: any): void{
    this.files.splice(this.files.indexOf(event), 1);
    console.log(this.files);
  }

  redirectToBack(): void{
    if(this.updateMode){
      this.router.navigate(['../../../Activities'], {relativeTo: this.route});
    }
    else{
      this.router.navigate(['../../Activities'], {relativeTo: this.route});
    }
  }

  resetForm(): void{
    this.activityForm.reset();
    this.files = [];
  }
}
