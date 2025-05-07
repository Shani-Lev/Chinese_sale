import { Donor } from "./donor";
import { Category } from "./Category";


export class Gift {
  constructor( title: string,  price: number, donor: Donor[],kategory: Category[], size: any, details?: string, image?: string, id?: number,){
    this.id = id;
    this.title = title;
    this.details = details;
    this.price = price;
    this.donors = donor;
    this.categories = kategory;
    this.size = size;
    this.image = image;
  }

  id?: number = -1;
  title!: string;
  details?: string;
  price!: number;
  donors!: Donor[];
  categories!: Category[];
  size!: any;
  image?: string;
}

