import { HttpEvent, HttpHandlerFn, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { inject } from '@angular/core';
import { MessageService } from 'primeng/api';

export function errorInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  return next(req).pipe(
    tap(event => {
      if (event instanceof HttpResponse) {
        // טיפול בתגובה מוצלחת
      }
    }),
    catchError((error: HttpErrorResponse) => {
      const messageService = inject(MessageService);
      let errorMessage = "An unknown error occurred!";
      if (error.error.message) {
        errorMessage = error.error.message;
      }
      messageService.add({ severity: 'error', summary: 'Error', detail: errorMessage });
      return throwError(error);
    })
  );
}