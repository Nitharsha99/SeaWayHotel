import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Room } from 'src/app/Models/room';
import { RoomService } from 'src/app/Services/RoomService/room.service';

@Component({
  selector: 'app-all-rooms',
  templateUrl: './all-rooms.component.html',
  styleUrls: ['./all-rooms.component.css']
})
export class AllRoomsComponent implements OnInit{

  totalItems!: number;
  pageSize:number = 5;
  page: number = 1;
  search: string = '';
  
  rooms: Room[] = [];
  filteredRooms: Room[] = [];

  available: boolean = false;

  constructor(private roomService: RoomService){
  }

  ngOnInit(): void {
    this.roomService.GetAllRooms().subscribe(res => {
      if(res.length > 0){
          res.forEach(element => {
            const defaultDate = new Date('0001-01-01T00:00:00');
            const lastCheckOutDate = new Date(element.lastCheckOut);
            element.lastCheckOut = lastCheckOutDate.getTime() === defaultDate.getTime() ? 'No Bookings' : formatDate(lastCheckOutDate, 'dd-MM-yyyy', 'en-US');
        });
        
        this.rooms = res;
        this.filteredRooms = this.rooms;
        this.totalItems = this.rooms.length;
      }
    });
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
