import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '../Services/AuthService';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'AngularClient';

  claims: { type: string, value: string }[] = [];

  constructor(private authService: AuthService) {}

  login(){
    this.authService.login().subscribe();
  }

  logout(){
    this.authService.logout().subscribe();
  }

  GetClaims(){
    this.authService.getClaims().subscribe((claims) => {
      console.log(claims);
      this.claims = claims;
    });
  }
}
