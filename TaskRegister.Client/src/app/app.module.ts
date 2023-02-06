import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { AuthModule } from 'angular-auth-oidc-client';
import { environment } from 'src/environments/environment.prod';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptors';
import { LayoutModule } from './layouts/layout.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    AuthModule.forRoot({
      config: {
        authority: environment.urlIdp.endsWith('/') ? `${environment.urlIdp.substring(0, environment.urlIdp.length - 1)}` : `${environment.urlIdp}`,
        redirectUrl: window.location.origin,
        postLogoutRedirectUri: window.location.origin,
        clientId: 'angularClientForTaskRegisterApi',
        scope: 'openid profile roles taskregisterapi.fullaccess',
        responseType: 'code',
        silentRenew: true,
        useRefreshToken: true
      },
    }),
    NgxDatatableModule.forRoot({
      messages: {
        emptyMessage: 'Sin registros para mostrar', // Message to show when array is presented, but contains no values
        totalMessage: 'registros existentes', // Footer total message
        selectedMessage: 'registro seleccionado' // Footer selected message
      }
    }),
    HttpClientModule,
    BrowserAnimationsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
