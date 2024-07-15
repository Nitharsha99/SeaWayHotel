import { Component } from '@angular/core';
import Swal from 'sweetalert2';
import { DiscountRate, PriceRange } from './Models/Enum/discount';

@Component({
  selector: 'app-common-function',
  template: '<p>common function component</p>'
})

export class CommonFunctionComponent{

  //Convert the base64 to String
  convertBase64ToString(base64: string){
      const decodedString = atob(base64);
    
      return decodedString;
    }

  // Loading Notification
  showLoadingNotification() {
    Swal.fire({
      title: 'Loading...',
      allowOutsideClick: false,
      didOpen: () => {
        Swal.showLoading();
      }
    });
  }

  //Price range switch function for filter
  filterByPriceRange(price: number, priceRange: any): any{
    switch(priceRange){
      case PriceRange.Below5K:
        return price < 5000; 
      case PriceRange.FiveKToTenK:
        return price >= 5000 && price <= 10000;
      case PriceRange.Above10KTo20K:
        return price > 10000 && price <= 20000;
      case PriceRange.Above20KTo30K:
        return price > 20000 && price <= 30000;
      case PriceRange.Above30K:
        return price > 30000;
      default:
        return true;
    }
  }

  //Discount Percentage switch function for filter
  filterByDincountPercentage(discount: number, discountRange: any): any{
    switch(discountRange){
      case DiscountRate.Below5Percent:
        return discount < 5;
      case DiscountRate.FiveToTenPercent:
        return discount >= 5 && discount <= 10;
      case DiscountRate.ElevenToTwentyPercent:
        return discount > 10 && discount <= 20;
      case DiscountRate.AboveTwentyPercent:
       return discount >= 20;
      default:
        return true;
    }
  }


}