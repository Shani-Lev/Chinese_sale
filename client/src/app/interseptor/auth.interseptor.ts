import { HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor() { }
    intercept(req: HttpRequest<any>, next: HttpHandler) {
        const authToken = localStorage.getItem("token")
        console.log("auth interceptor");
        
        const authRequst = req.clone({
            headers: req.headers.set('Authorization', "Bearer " + authToken)
        })
        return next.handle(authRequst);

    }
}

