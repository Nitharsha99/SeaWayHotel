import { Component, OnInit } from '@angular/core';
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

  constructor(private roomService: RoomService, private router: Router,
              private route: ActivatedRoute){}

  ngOnInit(): void {
    this.roomService.GetAllRooms().subscribe(res => {
      this.rooms = res;
      console.log('ch', this.rooms);
    })
  }

  navigateToNewRoom(){
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
      if (result.isConfirmed) {
       this.roomService.DeleteRoom(id).subscribe(res => {
        Swal.fire({
          icon: "success",
          title: "Successfully Deleted " + res.roomName + " !!! ",
          showConfirmButton: false,
          timer: 1700
        });
        setTimeout(() => {
          window.location.reload();
        });
       });
      } else if (result.dismiss === Swal.DismissReason.cancel) {
          Swal.fire('Process Cancelled', 'Your Record is safe now !!');
      }
  });
  }

}
