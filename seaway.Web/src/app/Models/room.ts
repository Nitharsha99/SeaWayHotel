import { PicDocument } from "./picDocument";

export interface Room{
    roomId: number;
    roomName: string;
    maxGuestCount: number;
    pricePerRoom: any;
    discountPercentage: any;
    discountAmount: any;
    picDocuments: PicDocument[];
}