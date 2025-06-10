

export class Package {
    id!: number; // maps to PackageId
    name!: string; // maps to PackageName
    description?: string;
    durationType?: number; // Enum, e.g., 1 = Days, 2 = Weeks
    price!: number;
    userType?: number; // Enum for user type
    isActive!: boolean;
    createdBy?: string;
    created?: Date;
    updatedBy?: string;
    updated?: Date;
  }