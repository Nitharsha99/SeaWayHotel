import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as moment from 'moment';
import { BookingList } from 'src/app/Models/bookings';
import { AuthService } from 'src/app/Services/AuthService/auth.service';
import { BookingsService } from 'src/app/Services/BookingsService/bookings.service';
import { OfferService } from 'src/app/Services/OfferService/offer.service';
import { RoomService } from 'src/app/Services/RoomService/room.service';

@Component({
  selector: 'app-main-admin-page',
  templateUrl: './main-admin-page.component.html',
  styleUrls: ['./main-admin-page.component.css']
})
export class MainAdminPageComponent implements OnInit{

  activeTab!: string;
  bookings: BookingList[] = [];
  filteredBookings: BookingList[] = [];
  page: number = 1;
  pageSize: number = 5;
  availablePackages: number = 0;
  availableoffers: number = 0;
  totalAvailableRooms: number = 0;
  totalBookingRooms: number = 0;

  constructor(private router: Router, private route: ActivatedRoute, private authService: AuthService,
              private bookingService: BookingsService, private offerService: OfferService, 
              private roomService: RoomService
  ){}

  ngOnInit(): void {
    this.activeTab = 'daily';
    this.bookingService.GetAllBookings().subscribe(res => {
      this.bookings = res;
      this.getFilteredBookingList();
    });
    this.getAvailableOffers();
    this.getBookingRoomsCount();
    this.getAvailablePackages();
  }

  selectTab(tab: string, event: any) {
    event.preventDefault();
    this.activeTab = tab;
    this.getFilteredBookingList();
    console.log(this.filteredBookings, 'asd');
  }

  get currentUser(): any {
    return this.authService.getCurrentUser();
  }

  getFilteredBookingList(){
    if(this.activeTab === 'daily'){
      this.filteredBookings = this.bookings.filter(book => {
        return moment(book.bookingDate).isSame(moment(), 'day');
      });
    }
    else if(this.activeTab === 'weekly'){
      this.filteredBookings = this.bookings.filter(book => {
        return moment(book.bookingDate).isSame(moment(), 'week');
      });
    }
    else{
      this.filteredBookings = this.bookings.filter(book => {
        return moment(book.bookingDate).isSame(moment(), 'month');
      });
    }
  }

  onPageChange(event: any): void{

  }

  getAvailableOffers(): void{
    this.offerService.GetOffers().subscribe(res => {
      let item = res.filter(o => {
        return o.isActive === true
      });

      this.availableoffers = item.length;
    });
  }

  getAvailablePackages(): void{

  }

  getBookingRoomsCount(): void{
    this.roomService.GetAllRooms().subscribe(res => {
      this.totalAvailableRooms = res.filter(r => {
        return r.isAvailable === true
      }).length;

      this.totalBookingRooms = res.filter(r => {
        return r.isAvailable === false
      }).length;
    });
  }

}
