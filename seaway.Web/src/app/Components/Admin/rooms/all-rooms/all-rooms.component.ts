import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { ActivatedRoute, Router } from '@angular/router';
import { Room } from 'src/app/Models/room';
import { RoomService } from 'src/app/Services/RoomService/room.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-all-rooms',
  templateUrl: './all-rooms.component.html',
  styleUrls: ['./all-rooms.component.css']
})
export class AllRoomsComponent implements OnInit{

  rooms:Room[] = [];
  pageSize:number = 2;
  page: number = 1;
  totalItems: number = 0;
  filters = {
    search: '',
    count: '',
    priceRange: '',
    discount: ''
  }

  filteredRooms: Room[] = [];
  displayedRooms: Room[] = [];

  dummyRoom: Room[] = [
    {roomId: 1, roomName: 'room1', guestCountMax: 2, price: 4000, discountAmount: 0, discountPercentage: 0, roomPics: []},
    {roomId: 2, roomName: 'room2', guestCountMax: 3, price: 5000, discountAmount: 0, discountPercentage: 0, roomPics: []},
    {roomId: 3, roomName: 'room3', guestCountMax: 3, price: 8000, discountAmount: 400, discountPercentage: 5, roomPics: []},
    {roomId: 4, roomName: 'room4', guestCountMax: 4, price: 12000, discountAmount: 1200, discountPercentage: 10, roomPics: []}
  ]
 
  constructor(private roomService: RoomService, private router: Router,
              private route: ActivatedRoute){}

  ngOnInit(): void {
    this.rooms = this.dummyRoom;
    this.filteredRooms = this.rooms;
    this.totalItems = this.filteredRooms.length;

    this.updateDisplayedRooms();
    // this.roomService.GetAllRooms().subscribe(res => {
    //   this.rooms = res;
    // this.filteredRooms = this.rooms;
    // this.totalItems = this.filteredRooms.length;
    // });
  }

  onPageChange(event: number) {
    this.page = event;
    this.updateDisplayedRooms();
  }

  onFilterChange(): void{
    console.log(this.filters.count)
    this.filteredRooms = this.rooms.filter(room => {
      return(
        (this.filters.search === '' || room.roomName.toLowerCase().includes(this.filters.search.toLowerCase())) &&
        (this.filters.count === '' || room.guestCountMax === +this.filters.count) &&
        this.filterByPriceRange(room.price) &&
        this.filterByDincountPercentage(room.discountPercentage)
      )
    });
    this.updateDisplayedRooms();
  }

  filterByPriceRange(price: number): any{
    switch(this.filters.priceRange){
      case '1':
        return price < 3000; 
      case '2':
        return price >= 3000 && price < 5000;
      case '3':
        return price >= 5000 && price < 10000;
      case '4':
        return price >= 10000;
      default:
        return true;
    }
  }

  filterByDincountPercentage(discount: number): any{
    switch(this.filters.discount){
      case '1':
        return discount < 5;
      case '2':
        return discount >= 5 && discount < 10;
      case '3':
        return discount >= 10 && discount < 20;
      case '4':
       return discount >= 20;
      default:
        return true;
    }
  }

  navigateToNewRoom(): void{
    this.router.navigate(['addRoom'], {relativeTo: this.route});
  }

  navigateToUpdate(id: number){
    console.log("id", id);
    this.router.navigate(['editRoom', id], {relativeTo: this.route});
  }

  deleteRoom(id: number){
    Swal.fire({
      title: 'Are you sure?',
      text: 'You won\'t be able to revert this!',
      icon: 'warning',
      showCancelButton: true,
      cancelButtonText: 'No, keep it',
      confirmButtonText: 'Yes, delete it!',
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      iconColor: "#d33"
  }).then((result) => {
    console.log(result, "jsasjasojaia");
      if (result.isConfirmed) {
       this.roomService.DeleteRoom(id).subscribe(res => {
        Swal.fire({
          icon: "success",
          title: "Successfully Deleted " + res.roomName + " !!! ",
          showConfirmButton: true
        }).then(() => {
          setTimeout(() => {
            window.location.reload();
          });
        });
       });
      } else if (result.dismiss === Swal.DismissReason.cancel) {
          Swal.fire('Process Cancelled', 'Your Record is safe now !!');
      }
  });
  }

  onReset(): void{
    this.filters = {
      search: '',
      count: '',
      priceRange: '',
      discount: ''
    }
    this.onFilterChange();
  }

  updateDisplayedRooms(): void {
    this.page =1;
    this.pageSize = 2;
    this.totalItems = this.filteredRooms.length;
  }

}
