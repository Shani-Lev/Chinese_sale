import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Lara from '@primeng/themes/lara';
import { HTTP_INTERCEPTORS, provideHttpClient , withInterceptors, withInterceptorsFromDi } from '@angular/common/http';
import { IMAGE_CONFIG } from '@angular/common';
import { AuthInterceptor } from './interseptor/auth.interseptor';
import { InformationInterceptor } from './interseptor/informetion.interseptor';
import { MessageService } from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    MessageService,
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
        theme: {
            preset: Lara
        },

    }),
    provideHttpClient(
      withInterceptorsFromDi()
    ),
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: InformationInterceptor, multi: true },
    {
        provide: IMAGE_CONFIG,
        useValue: {
          disableImageSizeWarning: true, 
          disableImageLazyLoadWarning: true
        }
      },
      
]
};

