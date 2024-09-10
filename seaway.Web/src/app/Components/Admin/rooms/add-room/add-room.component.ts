import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RoomCategory } from 'src/app/Models/roomCategory';
import { RoomCategoryService } from 'src/app/Services/RoomCategoryService/room.service';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.css']
})
export class AddRoomComponent implements OnInit{

  updateMode: boolean = false;
  categories: RoomCategory[] = [];

  constructor( private router: Router, private builder: FormBuilder,
               private route: ActivatedRoute, private categoryService: RoomCategoryService
  ){}

  roomForm: FormGroup = this.builder.group({
    roomName: ['', Validators.required],
    roomCategory: ['', Validators.required],
    createdBy: ['Nitharsha'],
    updatedBy: ['Nitharsha']
  }) 

  ngOnInit(): void {
    this.categoryService.GetAllRoomCategories().subscribe(res => {
      this.categories = res;
    });
  }

  addRoom(){

  }

  resetForm(){

  }

  redirectToBack(){

  }
}
