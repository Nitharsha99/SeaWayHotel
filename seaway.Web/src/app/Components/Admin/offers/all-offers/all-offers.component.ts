import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-all-offers',
  templateUrl: './all-offers.component.html',
  styleUrls: ['./all-offers.component.css']
})
export class AllOffersComponent implements OnInit{
  pageSize: number = 5;
  page: number = 1;

  constructor(){}

  ngOnInit(): void {
    
  }

  onSearch(): void{

  }

  navigateToAddOffer(): void{

  }


}
