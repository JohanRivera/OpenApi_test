import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-crud',
  templateUrl: './crud.component.html',
  styleUrls: ['./crud.component.css']
})
export class CrudComponent implements OnInit {

  userData$: Observable<any> | undefined;
  secretData$: Observable<any> | undefined;
  isAuthenticated$: Observable<Boolean> | undefined;

  constructor(private authService: AuthService, private httpClient: HttpClient) {}

  ngOnInit(): void {
    this.userData$ = this.authService.userData;
    this.isAuthenticated$ = this.authService.isLoggedIn.pipe(map(({ isAuthenticated }) => {
      if (isAuthenticated) {
        return true;
      }
      return false;
    }));

    // this.secretData$ = this.httpClient
    //   .get('https://localhost:44360/WeatherForecast')
    //   .pipe(catchError((error) => of(error)));
  }

  login() {
    this.authService.doLogin();
  }

  logout() {
    this.authService.signOut();
  }
}
