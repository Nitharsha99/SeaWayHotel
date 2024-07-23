import { PicDocument } from "./picDocument";

export interface RoomCategory{
    categoryId: number;
    roomName: string;
    guestCountMax: number;
    price: any;
    discountPercentage: any;
    discountAmount: any;
    roomPics: PicDocument[];
    created: any;
    createdBy: string;
    updated: any;
    updatedBy: string;
}