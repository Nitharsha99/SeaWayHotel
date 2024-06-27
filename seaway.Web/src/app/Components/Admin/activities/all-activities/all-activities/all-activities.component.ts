import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Activity } from 'src/app/Models/activity';

@Component({
  selector: 'app-all-activities',
  templateUrl: './all-activities.component.html',
  styleUrls: ['./all-activities.component.css']
})
export class AllActivitiesComponent implements OnInit{

  pageSize: number = 2;
  activities: Activity[] = [];

  constructor(private router: Router, private route: ActivatedRoute){
  }

  ngOnInit(): void {
    
  }

  navigateToAddActivity(): void{
    this.router.navigate(['addActivity'], {relativeTo: this.route});
  }

}
