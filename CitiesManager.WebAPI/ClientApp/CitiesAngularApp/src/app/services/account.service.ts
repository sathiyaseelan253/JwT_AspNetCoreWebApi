import { HttpClient } from '@angular/common/http'
import { RegisterUser } from '../models/register-user';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { LoginUser } from '../models/login-user';

const api_base_url: string = "https://localhost:7221/api/account/";
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  public currentUserName: string | null = null;

  constructor(private http: HttpClient) { }
  public postRegister(registerUser: RegisterUser):Observable<any> {
    return this.http.post<any>(`${api_base_url}register`, registerUser);
  }
  public postLogin(loginUser: LoginUser): Observable<any> {
    return this.http.post<any>(`${api_base_url}login`, loginUser);
  }
  public getLogOutLogin(): Observable<string> {
    return this.http.get<string>(`${api_base_url}logout`);
  }
  public postGenerateJwtToken(): Observable<any> {
    let token: string = localStorage["token"];
    let refreshToken: string = localStorage["refreshToken"];
    return this.http.post<any>(`${api_base_url}generate-new-jwtToken`, { token: token, refreshToken: refreshToken });
  }
}
