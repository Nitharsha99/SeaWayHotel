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

  showDeleteNotification(): any{
    Swal.fire({
      title: 'Are you sure?',
      text: 'You won\'t to delete this item',
      showCancelButton: true,
      padding: 'none',
      cancelButtonText: 'No, keep it',
      confirmButtonText: 'Yes, delete it!',
      confirmButtonColor: "#570254",
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
        cancelButton.style.borderColor = '#570254'; // Reset border color on mouse out
      });
      }
  });
  }

  ShowErrorPopup(error: any){
    Swal.fire({
      title: "Error!",
      text: error ,
      icon: "error",
      iconColor: '#EE0004'
    });
  }

  showMandoryFieldPopup(){
    Swal.fire({
      text: "Please fill all mandatory fields...",
      icon: "error",
      iconColor: '#EE0004',
      showConfirmButton: true,
      confirmButtonColor: '#570254'
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