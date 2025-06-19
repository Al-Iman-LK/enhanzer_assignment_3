import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';

export interface LoginRequest {
  API_Action: string;
  Device_Id: string;
  Sync_Time: string;
  Company_Code: string;
  API_Body: {
    Username: string;
    Pw: string;
  };
}

export interface LoginResponse {
  Success: boolean;
  Message: string;
  User_Locations: UserLocation[];
}

export interface UserLocation {
  Location_Code: string;
  Location_Name: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5045/api';
  private isLoggedInSubject = new BehaviorSubject<boolean>(false);
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();
  constructor(private http: HttpClient, @Inject(PLATFORM_ID) private platformId: Object) {
    // Check if user is already logged in (only in browser)
    if (isPlatformBrowser(this.platformId)) {
      const isLoggedIn = localStorage.getItem('isLoggedIn') === 'true';
      this.isLoggedInSubject.next(isLoggedIn);
    }
  }
  login(username: string, password: string): Observable<LoginResponse> {
    const request: LoginRequest = {
      API_Action: "GetLoginData",
      Device_Id: "D001",
      Sync_Time: "",
      Company_Code: username, // Use email from form as per API documentation
      API_Body: {
        Username: username,
        Pw: password
      }
    };return new Observable(observer => {
      this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, request).subscribe({
        next: (response) => {
          if (response.Success && isPlatformBrowser(this.platformId)) {
            localStorage.setItem('isLoggedIn', 'true');
            localStorage.setItem('userLocations', JSON.stringify(response.User_Locations));
            this.isLoggedInSubject.next(true);
          }
          observer.next(response);
          observer.complete();
        },
        error: (error) => {
          observer.error(error);
        }
      });
    });
  }
  logout(): void {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem('isLoggedIn');
      localStorage.removeItem('userLocations');
    }
    this.isLoggedInSubject.next(false);
  }

  getUserLocations(): UserLocation[] {
    if (isPlatformBrowser(this.platformId)) {
      const locations = localStorage.getItem('userLocations');
      return locations ? JSON.parse(locations) : [];
    }
    return [];
  }

  isAuthenticated(): boolean {
    if (isPlatformBrowser(this.platformId)) {
      return localStorage.getItem('isLoggedIn') === 'true';
    }
    return false;
  }
}
