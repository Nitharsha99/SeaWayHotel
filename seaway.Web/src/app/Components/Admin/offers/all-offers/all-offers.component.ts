import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { DiscountRate, PriceRange } from 'src/app/Models/Enum/discount';
import { Offer } from 'src/app/Models/offer';
import { OfferService } from 'src/app/Services/OfferService/offer.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';

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

  navigateToUpdateOffer(id: number): void{
    this.router.navigate(['editOffer', id], {relativeTo: this.route});
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
    this.pageSize = 5;
    this.totalItems = this.filteredOffers.length;
  }

  onPageChange(event: number) {
    this.page = event;
  }

  deleteOffer(id: number): void{
    this.commonFunction.showDeleteNotification().then((result: { isConfirmed: any; dismiss: Swal.DismissReason; }) => {
      if (result.isConfirmed) {
       this.offerService.DeleteOffer(id).subscribe(res => {
        Swal.fire({
          icon: "success",
          title: "Successfully Deleted " + res.name + " !!! ",
          showConfirmButton: true,
          iconColor: '#570254',
          confirmButtonColor: '#570254'
        }).then(() => {
          setTimeout(() => {
            window.location.reload();
          });
        });
       });
      } else if (result.dismiss === Swal.DismissReason.cancel) {
        setTimeout(() => {
          window.location.reload();
        });
      }
  });
  }

  onChange(item: any){
    Swal.fire({
      text: `Do you want to ${item.isActive ? 'activate' : 'deactivate'}?`,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      iconColor: '#570254',
      confirmButtonColor: '#570254',
      didOpen: () => {
        const cancelButton = document.querySelectorAll('.swal2-cancel')[0] as HTMLElement;
        cancelButton.style.backgroundColor = '#fff';
        cancelButton.style.border = '1px solid #570254';
        cancelButton.style.color = '#570254';

        // Add hover style
        cancelButton.addEventListener('mouseover', () => {
          cancelButton.style.backgroundColor = '#f8edf7'; // Change background color on hover
          cancelButton.style.borderColor = '#570254'; // Change border color on hover
          cancelButton.style.cursor = 'pointer'; // Change cursor on hover
        });

        cancelButton.addEventListener('mouseout', () => {
          cancelButton.style.backgroundColor = '#fff'; // Reset background color on mouse out
          cancelButton.style.borderColor = '#570254';
        });
      }
    }).then((result) => {
      if (result.isConfirmed) {
        this.changeStatus(item.isActive, item.offerId);
      } else {
        window.location.reload();
      }
    });
  }

  changeStatus(status: boolean, id: number): void{
    this.offerService.ChangeStatus(status, id).subscribe(() => {
      setTimeout(() => {
        window.location.reload();
      });
    });
  }

}


