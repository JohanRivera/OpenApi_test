import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor 
{
  private secureRoutes = ['https://localhost:44360'];

  constructor(private authService: AuthService) {}

  intercept (request: HttpRequest<any>, next: HttpHandler) 
  {
    if (!this.secureRoutes.find((x) => request.url.startsWith(x))) {
        return next.handle(request);
    }

    this.authService.token.subscribe(token => {
      if (!token)
        return next.handle(request);
        
      request = request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + token),
      });

      return next.handle(request);
    });
    
    return next.handle(request);
  }
}