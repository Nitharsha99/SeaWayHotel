import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { PicDocument } from 'src/app/Models/picDocument';
import { CloudinaryService } from 'src/app/Services/CloudinaryService/cloudinary.service';
import { OfferService } from 'src/app/Services/OfferService/offer.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-offers',
  templateUrl: './add-offers.component.html',
  styleUrls: ['./add-offers.component.css']
})
export class AddOffersComponent implements OnInit{

  updateMode: boolean = false;
  pictures: PicDocument[] = [];
  imageWidth: number = 170;
  imageMargin: number = 2;
  imageHeight: number = 110;
  files: File[] = [];
  selectedPictures: string[] = [];
  offerId !: number;
  picArrayLength: number = 0;
  minValidTo!: string;
  minValidFrom!: string;
  currentDate!: string;

  constructor(private builder: FormBuilder, private router: Router,  private route: ActivatedRoute,
              private cloudinaryService: CloudinaryService, private offerService: OfferService,
              private commonFunction: CommonFunctionComponent
  ){}

  offerForm: FormGroup = this.builder.group({
    offerName:['', Validators.required],
    description:['', Validators.required],
    validFrom:['', Validators.required],
    validTo:['', Validators.required],
    price:['', Validators.required],
    discount:[0],
    isRoomOffer: [false],
    createdBy: ['Nitharsha'],
    updatedBy: [''],
    offerPics: this.builder.array([
      this.builder.group({
        picName: [null],
        picValue: [null],
        cloudinaryPublicId: [null]
       })
    ])
  })

  ngOnInit(): void {
    this.currentDate = this.formatDate(new Date());
    this.minValidFrom = this.currentDate;
    // this.offerForm.get('validTo')?.valueChanges.subscribe((value: string) => {
    //   this.minValidFrom = value;
    // });

    this.offerForm.get('validFrom')?.valueChanges.subscribe((value: string) => {
      this.minValidTo = value;
    });
  }

  addOffer(): void{
    const formValue = this.offerForm.value;

    if(formValue.discount == ""){
      formValue.discount = 0;
    }

    if(!formValue.offerName || !formValue.description || !formValue.validFrom || !formValue.validTo || !formValue.price){
      this.commonFunction.showMandoryFieldPopup();
    }
    else{
      if(this.files.length > 0){
        this.uploadImage();
        }
        else{
          formValue.roomPics = null;
            this.callOfferService();
        }
    }
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = (`0${date.getMonth() + 1}`).slice(-2);
    const day = (`0${date.getDate()}`).slice(-2);

    return `${year}-${month}-${day}`;
  }

  updateSelectedPics(id: string, event: any){
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

  deleteImages(): void{

  }

  onSelect(event: any){
    console.log(event.addedFiles[0].name.length);
    event.addedFiles.forEach((file: File) => {
      if(file.name.length <= 50){
        this.files.push(file);
      }
      else{
        Swal.fire({
          title: file.name,
          text: "File name legnth should not more than 50. Please reduce your image name length....",
          icon: "info",
          iconColor: '#570254'
        });
      }
    });
    
  }

  onRemove(event: any): void{
    this.files.splice(this.files.indexOf(event), 1);
  }

  redirectToBack(): void{
    if(this.updateMode){
      this.router.navigate(['../../../Offers'], {relativeTo: this.route});
    }
    else{
      this.router.navigate(['../../Offers'], {relativeTo: this.route});
    }
  }

  resetForm(): void{
    this.offerForm.reset();
    this.files = [];
  }

  async uploadImage(){
    const formValue = this.offerForm.value;
    this.picArrayLength = this.files.length;
    let count = 0;
  
      for(let i = 0; i < this.files.length; i++){
        const file_data = this.files[i];
        const data = new FormData();
        data.append('file', file_data);
        data.append('upload_preset', 'seaway_hotel');
        data.append('cloud_name', 'dly7yjg1w');
  
        this.cloudinaryService.uploadImage(data).subscribe(res => {
          const picsArray = this.offerForm.get('offerPics') as FormArray;
          while (picsArray.length <= i) {
            picsArray.push(this.builder.group({
              picName: [null],
              picValue: [null],
              cloudinaryPublicId: [null]
            }));
          }
          console.log(res.public_id, res);
  
          (picsArray.at(i) as FormGroup).patchValue({
            picName: res.original_filename + "." + res.format,
            picValue: res.url,
            cloudinaryPublicId: res.public_id
          });
  
          console.log("sajnjadjjjjjj", picsArray);
          formValue.offerPics = picsArray.controls.map((control: AbstractControl<any>) => {
            const formGroup = control as FormGroup;
            return {
              picName: formGroup.get('picName')?.value,
              picValue: formGroup.get('picValue')?.value,
              cloudinaryPublicId: formGroup.get('cloudinaryPublicId')?.value
            };
          });
  
          count++;
         
          if(count === this.picArrayLength){
            console.log("auifhcyieaufajka", count, this.picArrayLength);
            Swal.close();
            this.callOfferService();
          }
        })
         this.commonFunction.showLoadingNotification();
      }
  
    }

    callOfferService(): void{
      const formValue = this.offerForm.value;
      if(this.updateMode === false){
        this.offerService.PostOffer(formValue).subscribe((res) => {
          console.log('post result', res);
          Swal.fire({
            title: "Offer Saved Successfully!!",
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
        if(this.offerId != null){
          this.offerService.updateOffer(formValue, this.offerId).subscribe((res) => {
            Swal.fire({
              title: "Offer Updated Successfully!!",
              icon: "success",
              iconColor: '#570254',
              showConfirmButton: true,
              confirmButtonColor: '#570254'
            }).then(() =>{
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

}
