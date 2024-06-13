import { Component, OnInit } from '@angular/core';
import { CloudinaryService } from 'src/app/Services/CloudinaryService/cloudinary.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RoomService } from 'src/app/Services/RoomService/room.service';
import Swal from 'sweetalert2';
import { Room } from 'src/app/Models/room';
import { PicDocument } from 'src/app/Models/picDocument';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.css']
})
export class AddRoomComponent implements OnInit{

updateMode: boolean = false;
files: File[] = [];
picArrayLength: number = 0;
roomId!: number;
pictures: PicDocument[] = [];
selectedPictures: string[] = [];
imageWidth: number = 170;
imageMargin: number = 2;
imageHeight: number = 110;

  constructor(private cloudinaryService: CloudinaryService, private location: Location, 
              private router: Router, private builder: FormBuilder,
              private roomService: RoomService, private route: ActivatedRoute){}

  roomForm: FormGroup = this.builder.group({
    roomName: ['', Validators.required],
    guestCountMax: ['', Validators.required],
    price: ['', Validators.required],
    discountPercentage: [0],
    roomPics: this.builder.array([
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
        this.roomId = params['id'];
        this.updateMode = true;
        this.roomService.FindRoomById(this.roomId).subscribe(res => {
          if(res != null){
            this.roomForm.patchValue(res);
            console.log('Form Value:', this.roomForm.value);
            if(res.roomPics != null && res.roomPics.length > 0){
              this.pictures = res.roomPics;
              this.pictures.forEach(element => {
                const urlLink = this.convertBase64ToString(element.picValue);
                element.picValue = urlLink;
              });
            }
            console.log("pics", this.pictures);
          }
        });
      }
    });
  }

  onSelect(event: any){
    console.log(event);
    this.files.push(...event.addedFiles);
  }

  onRemove(event: any){
    this.files.splice(this.files.indexOf(event), 1);
    console.log(this.files);
  }

  updateSelectedPics(picId: string){
    this.selectedPictures.push(picId);
    console.log("daaia", this.selectedPictures);
  }

 async uploadImage(){
  const formValue = this.roomForm.value;
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
        const roomPicsArray = this.roomForm.get('roomPics') as FormArray;
        while (roomPicsArray.length <= i) {
          roomPicsArray.push(this.builder.group({
            picName: [null],
            picValue: [null],
            cloudinaryPublicId: [null]
          }));
        }
        console.log(res.public_id, res);

        (roomPicsArray.at(i) as FormGroup).patchValue({
          picName: res.original_filename + "." + res.format,
          picValue: res.url,
          cloudinaryPublicId: res.public_id
        });

        console.log("sajnjadjjjjjj", roomPicsArray);
        formValue.roomPics = roomPicsArray.controls.map((control: AbstractControl<any>) => {
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
          this.callRoomService();
        }
      })
       this.showLoadingNotification();
    }

  }

/****************Click Add or Update Button *********** */
 addRoom(){
  if(this.updateMode == false){
    this.newRoom();
  }
  else{
    this.updateRoom();
  }
 }

 async newRoom(){
  const formValue = this.roomForm.value;

  if(formValue.discount == ""){
    formValue.discount = 0;
  }

  if(!formValue.roomName || !formValue.guestCountMax || !formValue.price){
    Swal.fire({
      title: "Failed to save room!",
      text: "Please fill all mandatory fields...",
      icon: "error"
    });
  }
  else{
    if(this.files.length > 0){
    this.uploadImage();
    }
    else{
      formValue.roomPics = null;
        this.callRoomService();
    }
  }
    

 }

 callRoomService(){
  const formValue = this.roomForm.value;
  if(this.updateMode === false){
    this.roomService.PostRoom(formValue).subscribe((res) => {
      console.log('post result', res);
      Swal.fire({
        title: "Room Saved Successfully!!",
        icon: "success"
      });
      this.files = [];
      this.resetForm();
     },
     (error) =>{
      Swal.fire({
        title: "Error!",
        text: error.message ,
        icon: "error"
      });
     }
    )
  }else{
    alert("no update");
  }

 }

 updateRoom(){}

 resetForm(){
  this.roomForm.reset();
  this.files = [];
  console.log("resert");
 }

 redirectToBack(){
   this.location.back();
 }

 showLoadingNotification() {
  Swal.fire({
    title: 'Loading...',
    allowOutsideClick: false,
    didOpen: () => {
      Swal.showLoading();
    }
  });
}

deleteImages(){
  this.roomService.DeleteImages(this.selectedPictures).subscribe((res) =>{
    console.log("res");
  });
}

convertBase64ToString(base64: string){
  const decodedString = atob(base64);

  return decodedString;
}

}
