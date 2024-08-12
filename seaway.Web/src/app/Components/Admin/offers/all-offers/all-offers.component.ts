import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { DiscountRate, PriceRange } from 'src/app/Models/Enum/discount';
import { Offer } from 'src/app/Models/offer';
import { OfferService } from 'src/app/Services/OfferService/offer.service';
import * as moment from 'moment';

@Component({
  selector: 'app-all-offers',
  templateUrl: './all-offers.component.html',
  styleUrls: ['./all-offers.component.css']
})
export class AllOffersComponent implements OnInit{
  pageSize: number = 1;
  page: number = 1;
  offers: Offer[] = [];
  filteredOffers: Offer[] = [];
  totalItems: number = 0;
  filters = {
    search: '',
    roomOffer: '',
    priceRange: '',
    discount: '',
    validTo: '',
    validFrom: ''
  }

  priceRange = PriceRange;
  DiscountRate = DiscountRate;

  constructor(private router: Router, private route: ActivatedRoute,
              private offerService: OfferService, private commonFunction: CommonFunctionComponent
  ){}

  ngOnInit(): void {
    this.offerService.GetOffers().subscribe(res => {
      this.offers = res;
      this.filteredOffers = this.offers;
      this.totalItems = this.filteredOffers.length;
      console.log(this.offers);
    });
    this.updateDisplayeditems();
  }

  getPriceRates(): PriceRange[] {
    return Object.values(this.priceRange);
  }

  getDiscountRates(): DiscountRate[] {
    return Object.values(this.DiscountRate);
  }

  onSearch(): void{
    console.log(this.filters);
    this.filteredOffers = this.offers.filter(o => {
      console.log('date', new Date(o.validFrom).getTime())
      return (
        (this.filters.search === '' || o.name.toLowerCase().includes(this.filters.search.toLowerCase())) &&
        (this.filters.roomOffer === '' || o.isRoomOffer.toString().toLowerCase().includes(this.filters.roomOffer.toLowerCase())) &&
        (this.filters.validFrom === '' || new Date(moment(o.validFrom).format('YYYY-MM-DD')).getTime() >= new Date(this.filters.validFrom).getTime()) &&
        (this.filters.validTo === '' || new Date(moment(o.validTo).format('YYYY-MM-DD')).getTime() >= new Date(this.filters.validTo).getTime()) &&
        this.commonFunction.filterByPriceRange(o.price, this.filters.priceRange) &&
        this.commonFunction.filterByDincountPercentage(o.discountPercentage, this.filters.discount)
      )
    });
    this.updateDisplayeditems();
  }

  navigateToAddOffer(): void{
    this.router.navigate(['addOffer'], {relativeTo: this.route});
  }

  onReset(): void{
    this.filters = {
      search: '',
      roomOffer: '',
      priceRange: '',
      discount: '',
      validTo: '',
      validFrom: ''
    }
    this.onSearch();
  }

  updateDisplayeditems(): void {
    this.page =1;
    this.pageSize = 1;
    this.totalItems = this.filteredOffers.length;
  }

  onPageChange(event: number) {
    this.page = event;
  }

}


