import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  
  isConnected(){
    return this.http.get<{ connected:boolean }>(`${this.baseUrl}/home`);
  }
}