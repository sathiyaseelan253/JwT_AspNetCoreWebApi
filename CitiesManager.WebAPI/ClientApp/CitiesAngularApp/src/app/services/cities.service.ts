import { Injectable } from '@angular/core';
import { City } from "../models/city";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs";

const API_BASE_URL: string = "https://localhost:7221/api/Cities";

@Injectable({
  providedIn: 'root'
})
export class CitiesService {
  cities: City[] = [];

  constructor(private httpClient: HttpClient) {
  }

  public getCities(): Observable<City[]> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);

    return this.httpClient.get<City[]>(`${API_BASE_URL}`, { headers: headers })
  }

  public postCity(city: City): Observable<City> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);
    return this.httpClient.post<City>(`${API_BASE_URL}`, city, { headers: headers })
  }

  public putCity(city: City): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);
    return this.httpClient.put<string>(`${API_BASE_URL}/${city.cityID}`, city, { headers: headers })
  }

  public deleteCity(cityID: string | null): Observable<string> {
    let headers = new HttpHeaders();
    headers = headers.append("Authorization", `Bearer ${localStorage['token']}`);
    return this.httpClient.delete<string>(`${API_BASE_URL}/${cityID}`, { headers: headers })
  }
 
}
