import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/Services/AuthService/auth.service';

@Component({
  selector: 'app-main-admin-page',
  templateUrl: './main-admin-page.component.html',
  styleUrls: ['./main-admin-page.component.css']
})
export class MainAdminPageComponent implements OnInit{

  activeTab!: string;

  constructor(private router: Router, private route: ActivatedRoute, private authService: AuthService){}

  ngOnInit(): void {
    this.activeTab = 'daily';
  }

  selectTab(tab: string, event: any) {
    event.preventDefault();
    this.activeTab = tab;
  }

  get currentUser(): any {
    return this.authService.getCurrentUser();
  }

  navigateToRooms(){
    console.log("HDGAK");
    this.router.navigate(['../Rooms'], {relativeTo: this.route});
  }

  navigateToActivities(){
    this.router.navigate(['../Activities'], {relativeTo: this.route});
  }
}
