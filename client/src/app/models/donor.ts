import { Gift } from "./gift";

export class Donor {
    constructor(name: string, detailes?: string, phone?: string, email?: string, logo?: string, showMe: boolean = false) {
        this.name = name;
        this.details = detailes;
        this.phone = phone;
        this.email = email;
        this.logo = logo;
        this.showMe = showMe;
    }

    
    id : number = -1
    name!: string;
    details?: string;
    phone?: string;
    email?: string;
    logo?: string;
    showMe?: boolean;
    gifts?: Gift[];
}
