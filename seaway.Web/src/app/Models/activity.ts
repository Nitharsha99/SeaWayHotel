import { PicDocument } from "./picDocument";

export interface Activity{
    id: number;
    activityName: string;
    description: string;
    isActive: boolean;
    activityPics: PicDocument[];
}