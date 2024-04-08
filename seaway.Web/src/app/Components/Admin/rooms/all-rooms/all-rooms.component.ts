import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Room } from 'src/app/Models/room';
import { RoomService } from 'src/app/Services/RoomService/room.service';

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
}
