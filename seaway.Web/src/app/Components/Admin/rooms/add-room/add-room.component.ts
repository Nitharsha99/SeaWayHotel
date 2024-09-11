import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { RoomCategory } from 'src/app/Models/roomCategory';
import { RoomCategoryService } from 'src/app/Services/RoomCategoryService/room.service';
import { RoomService } from 'src/app/Services/RoomService/room.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.css']
})
export class AddRoomComponent implements OnInit{

  updateMode: boolean = false;
  categories: RoomCategory[] = [];
  roomId!: number;

  constructor( private router: Router, private builder: FormBuilder,
               private route: ActivatedRoute, private categoryService: RoomCategoryService,
               private roomService: RoomService, private commonFunction: CommonFunctionComponent
  ){}

  roomForm: FormGroup = this.builder.group({
    roomNumber: ['', Validators.required],
    roomTypeId: ['', Validators.required],
    createdBy: ['Nitharsha'],
    updatedBy: ['Nitharsha']
  }) 

  ngOnInit(): void {
    this.categoryService.GetAllRoomCategories().subscribe(res => {
      this.categories = res;
    });

    this.route.params.subscribe(params => {
      if(params['id']){
        this.roomId = params['id'];
        this.updateMode = true;
        this.roomService.FindRoomById(this.roomId).subscribe(res => {
          if(res != null){
            this.roomForm.patchValue(res);
          }
        })
      }
    });
  }

  addRoom(){
    const formValue = this.roomForm.value;

    if(formValue.roomNumber && formValue.roomTypeId){
      if(this.updateMode){
        if(this.roomId > 0){
          this.roomService.UpdateRoom(formValue, this.roomId).subscribe(res => {
            Swal.fire({
              title: "Room Updated Successfully!!",
              icon: "success",
              iconColor: '#570254',
              showConfirmButton: true,
              confirmButtonColor: '#570254'
            }).then(() => {
              setTimeout(() => {
                window.location.reload();
              });
            });
          },
          (error) =>{
            this.commonFunction.ShowErrorPopup(error);
           }
        );
        }
      }
      else{
        this.roomService.PostRoom(formValue).subscribe(res => {
          Swal.fire({
            title: "Room Saved Successfully!!",
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
  }

  resetForm(){
    this.roomForm.reset();
  }

  redirectToBack(){
    if(this.updateMode){
      this.router.navigate(['../../../Rooms'], {relativeTo: this.route});
    }
    else{
      this.router.navigate(['../../Rooms'], {relativeTo: this.route});
    }
  }
}
