import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from "@angular/common/http";
import { authInterceptor } from "./core/interceptors/auth.interceptor";
import { lastValueFrom } from "rxjs";
import { InitializeService } from "./core/services/initialize.service";

const initializeApp = (initializeService:InitializeService) => {
  return () => lastValueFrom(initializeService.init());
}


export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(
        withInterceptors([authInterceptor])
    ),
    {
      provide: APP_INITIALIZER,
      useFactory:initializeApp,
      multi:true,
      deps:[InitializeService],
    }  
  ]
};
