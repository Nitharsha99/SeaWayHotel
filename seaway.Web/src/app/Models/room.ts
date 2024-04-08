import { PicDocument } from "./picDocument";

export interface Room{
    roomId: number;
    roomName: string;
    guestCountMax: number;
    price: any;
    discountPercentage: any;
    discountAmount: any;
    roomPics: PicDocument[];
}