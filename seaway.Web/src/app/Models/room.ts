export interface Room{
    id: number;
    roomNumber: string;
    roomType: string;
    lastCheckOut: string;
    isAvailable: boolean;
    created: Date;
    updated: Date;
    createdBy: string;
    updatedBy: string;
}