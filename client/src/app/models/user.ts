export class User {
    constructor(name: string, password: string, email: string, phone: string){
        this.name = name
        this.password = password
        this.email = email
        this.phone = phone
    }
    
    id? : number;
    name : string;
    password : string;
    email : string;
    phone : string;


}
