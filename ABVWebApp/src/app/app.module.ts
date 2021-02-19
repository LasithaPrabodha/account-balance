import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { NotFoundComponent } from './pages/error-pages/not-found/not-found.component';
import { ForbiddenComponent } from './pages/forbidden/forbidden.component';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { ComponentsModule } from './components/components.module';
import { environment } from 'src/environments/environment';

function tokenGetter(): string {
    return localStorage.getItem('accessToken');
}

@NgModule({
    declarations: [AppComponent, HomeComponent, NotFoundComponent, ForbiddenComponent],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule,
        ComponentsModule,
        JwtModule.forRoot({
            config: {
                tokenGetter,
                allowedDomains: ['localhost:5000'],
                disallowedRoutes: [/(registration|login)$/gm],
            },
        }),
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: JwtInterceptor,
            multi: true,
        },
        { provide: 'BASE_API_URL', useValue: environment.urlAddress },
    ],
    bootstrap: [AppComponent],
})
export class AppModule {}
