import { PicDocument } from "./picDocument";

export interface Offer{
    offerId: number;
    name: string;
    description: string;
    price: any;
    discountAmount: any;
    discountPercentage: any;
    validFrom: Date;
    validTo: Date;
    isActive: boolean;
    isRoomOffer: boolean
    offerPics: PicDocument[];
    created: Date;
    createdBy: string;
    updated: Date;
    updatedBy: string;
}