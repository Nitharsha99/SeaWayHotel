import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { Offer } from 'src/app/Models/offer';
import { OfferService } from 'src/app/Services/OfferService/offer.service';

@Component({
  selector: 'app-all-offers',
  templateUrl: './all-offers.component.html',
  styleUrls: ['./all-offers.component.css']
})
export class AllOffersComponent implements OnInit{
  pageSize: number = 5;
  page: number = 1;
  offers: Offer[] = [];
  filteredOffers: Offer[] = [];
  search = '';

  constructor(private router: Router, private route: ActivatedRoute,
              private offerService: OfferService, private commonFunction: CommonFunctionComponent
  ){}

  ngOnInit(): void {
    this.offerService.GetOffers().subscribe(res => {
      this.offers = res;
      this.filteredOffers = this.offers;
    })
  }

  onSearch(): void{
    this.filteredOffers = this.offers.filter(o => {
      return o.name.toLowerCase().includes(this.search.toLowerCase());
    });
  }

  navigateToAddOffer(): void{
    this.router.navigate(['addOffer'], {relativeTo: this.route});
  }


}
