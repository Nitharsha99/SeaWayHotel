import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { Room } from 'src/app/Models/room';
import { RoomService } from 'src/app/Services/RoomService/room.service';

@Component({
  selector: 'app-all-rooms',
  templateUrl: './all-rooms.component.html',
  styleUrls: ['./all-rooms.component.css']
})
export class AllRoomsComponent implements OnInit{

  totalItems!: number;
  pageSize:number = 8;
  page: number = 1;
  search: string = '';
  
  rooms: Room[] = [];
  filteredRooms: Room[] = [];

  available: boolean = false;

  constructor(private roomService: RoomService, private route: ActivatedRoute,
              private router: Router, private commonFunction: CommonFunctionComponent
  ){
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
    this.filteredRooms = this.rooms.filter(r => {
      return(
        this.search === '' || 
        (
          r.roomNumber.toLowerCase().includes(this.search.toLowerCase()) || 
          r.roomType.toLowerCase().includes(this.search.toLowerCase())
        )
      )
    });

    this.updateFilteredRooms();
  }

  navigateToNewRoom(): void{
    this.router.navigate(['addRoom'], {relativeTo: this.route});
  }

  onPageChange(event: any): void{
    this.page = event;
  }

  deleteRoom(){

  }

  navigateToUpdate(){
    //this.router.navigate(['editRoom', id], {relativeTo: this.route});
  }

  navigateToCategories(){
    this.router.navigate(['types'], {relativeTo: this.route});
  }

  updateFilteredRooms(){
    this.page =1;
    this.pageSize = 8;
    this.totalItems = this.filteredRooms.length;
  }

}
