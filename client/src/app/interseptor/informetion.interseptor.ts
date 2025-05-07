import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { MessageService } from 'primeng/api';
import { Observable, throwError } from "rxjs";
import { catchError, delay, tap } from "rxjs/operators";

@Injectable()
export class InformationInterceptor implements HttpInterceptor {
    constructor(private messageService: MessageService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler) : Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            tap((response: HttpEvent<any>) => {
                if (req.method !== 'GET' && response && response.type === 4) { 
                    let severity = "contrast";
                    let summary = "הודעה:";
                    
                    switch (response.status) {
                        case 200: severity = "secondary"; summary = "פעולה בוצעה בהצלחה"; break;
                        case 201: severity = "success"; summary = "נוסף בהצלחה"; break;
                        case 204: severity = "success"; summary = "נוסף בהצלחה"; break;
                    }

                    this.messageService.add({ severity: "secondary", summary: summary, detail: '', life: 3000 });
                }
            }),
            catchError((info: HttpErrorResponse) => {
                let message = "";
                console.log("information interceptor");
                console.log(info);

                if (info.error) {
                    message = info.error?.message || info.statusText;
                }
                console.log(info);

                let severity = "danger";
                let summary = "שגיאה:";
                switch (info.status) {
                    case 400: summary = "שגיאת נתונים"; break;
                    case 401: summary = "לא מורשה"; break;
                    case 403: summary = "שגיאת גישה"; break;
                    case 404: summary = "פעולה לא נמצאה"; break;
                    case 409: summary = "נתונים כפולים"; break;
                    case 500: summary = "בעיה בגישה לנתונים"; break;
                    default: summary = info.status + " " + info.statusText;
                }
                this.messageService.add({ severity: "contrast", summary: summary, detail: message, life: 3000 });
                return throwError(info);
            })
        );
    }
}
