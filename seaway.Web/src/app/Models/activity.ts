import { PicDocument } from "./picDocument";

export interface Activity{
    activityId: number;
    activityName: string;
    description: string;
    isActive: boolean;
    activityPics: PicDocument[];
}