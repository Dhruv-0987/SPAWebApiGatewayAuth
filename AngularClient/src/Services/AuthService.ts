import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NEVER, Observable } from 'rxjs';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  constructor(private http: HttpClient) {}

  baseUrl: string = environment.BASE_URL;

  login(): Observable<any> {
    window.location.href = `${this.baseUrl}/Account/login`;
    return NEVER;
  }

  logout(): Observable<any> {
    window.location.href = `${this.baseUrl}/Account/logout`;
    return NEVER;
  }

  getClaims(): Observable<any> {
    return this.http.get<any>('/Account/info', { withCredentials: true });
  }
}
