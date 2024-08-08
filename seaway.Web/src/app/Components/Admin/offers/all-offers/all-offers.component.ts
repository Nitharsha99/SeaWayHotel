import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-all-offers',
  templateUrl: './all-offers.component.html',
  styleUrls: ['./all-offers.component.css']
})
export class AllOffersComponent implements OnInit{
  pageSize: number = 5;
  page: number = 1;

  constructor(private router: Router, private route: ActivatedRoute){}

  ngOnInit(): void {
    
  }

  onSearch(): void{

  }

  navigateToAddOffer(): void{
    this.router.navigate(['addOffer'], {relativeTo: this.route});
  }


}
