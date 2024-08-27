import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { AuthService } from '../Services/AuthService';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { BaseAddressInterceptor } from '../Services/HttpInterceptor';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes), AuthService,
    provideHttpClient(withInterceptors([BaseAddressInterceptor])),
  ]
};
