import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-all-rooms',
  templateUrl: './all-rooms.component.html',
  styleUrls: ['./all-rooms.component.css']
})
export class AllRoomsComponent implements OnInit{

  totalItems: number = 3;
  pageSize:number = 5;
  page: number = 1;
  filters = {
    search: ''
  }

  available: boolean = false;

  constructor(){
  }

  ngOnInit(): void {
    
  }

  onFilterChange(): void{

  }

  navigateToNewRoom(): void{

  }

  onReset(): void{

  }

  onPageChange(event: any): void{

  }

  deleteRoom(){

  }

  navigateToUpdate(){
    
  }

  navigateToCategories(){

  }

}
