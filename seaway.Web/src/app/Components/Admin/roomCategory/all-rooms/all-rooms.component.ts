import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonFunctionComponent } from 'src/app/commonFunction';
import { DiscountRate, PriceRange } from 'src/app/Models/Enum/discount';
import { RoomCategory } from 'src/app/Models/roomCategory';
import { RoomCategoryService } from 'src/app/Services/RoomCategoryService/room.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-all-rooms',
  templateUrl: './all-rooms.component.html',
  styleUrls: ['./all-rooms.component.css']
})
export class AllRoomsComponent implements OnInit{

  roomCategory:RoomCategory[] = [];
  pageSize:number = 5;
  page: number = 1;
  totalItems: number = 0;
  filters = {
    search: '',
    count: '',
    priceRange: '',
    discount: ''
  }

  filteredCategory: RoomCategory[] = [];
  DiscountRate = DiscountRate;
  priceRange = PriceRange;
 
  constructor(private roomCategoryService: RoomCategoryService, private router: Router,
              private route: ActivatedRoute, private commonFunction: CommonFunctionComponent){}

  ngOnInit(): void {
    this.roomCategoryService.GetAllRoomCategories().subscribe(res => {
      console.log(res)
      this.roomCategory = res;
      this.filteredCategory = this.roomCategory;
      this.totalItems = this.filteredCategory.length;
      console.log(this.totalItems)
    });

    this.updateDisplayedRooms();
  }

  getDiscountRates(): DiscountRate[] {
    return Object.values(this.DiscountRate);
  }

  getPriceRates(): PriceRange[] {
    return Object.values(this.priceRange);
  }

  onPageChange(event: number) {
    this.page = event;
  }

  onFilterChange(): void{
    console.log(this.filters)
    this.filteredCategory = this.roomCategory.filter(room => {
      return(
        (this.filters.search === '' || room.roomName.toLowerCase().includes(this.filters.search.toLowerCase())) &&
        (this.filters.count === '' || room.guestCountMax === +this.filters.count) &&
        this.commonFunction.filterByPriceRange(room.price, this.filters.priceRange) &&
        this.commonFunction.filterByDincountPercentage(room.discountPercentage, this.filters.discount)
      )
    });
    this.updateDisplayedRooms();
  }


  navigateToNewRoom(): void{
    this.router.navigate(['addRoom'], {relativeTo: this.route});
  }

  navigateToUpdate(id: number){
    console.log("id", id);
    this.router.navigate(['editRoom', id], {relativeTo: this.route});
  }

  deleteRoom(id: number){
   this.commonFunction.showDeleteNotification().then((result: { isConfirmed: any; dismiss: Swal.DismissReason; }) => {
      if (result.isConfirmed) {
       this.roomCategoryService.DeleteRoomCategory(id).subscribe(res => {
        Swal.fire({
          icon: "success",
          title: "Successfully Deleted " + res.roomName + " !!! ",
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

  onReset(): void{
    this.filters = {
      search: '',
      count: '',
      priceRange: '',
      discount: ''
    }
    this.onFilterChange();
  }

  updateDisplayedRooms(): void {
    this.page =1;
    this.pageSize = 5;
    this.totalItems = this.filteredCategory.length;
  }

}
