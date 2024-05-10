import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-main-admin-page',
  templateUrl: './main-admin-page.component.html',
  styleUrls: ['./main-admin-page.component.css']
})
export class MainAdminPageComponent implements OnInit{

  constructor(private router: Router, private route: ActivatedRoute){}

  ngOnInit(): void {
    
  }

  navigateToRooms(){
    console.log("HDGAK");
    this.router.navigate(['../Rooms'], {relativeTo: this.route});
  }

  navigateToActivities(){
    this.router.navigate(['../Activities'], {relativeTo: this.route});
  }
}
