import { inject, Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { environment } from "@environments";
import { AppUser, Pagination } from "@models";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  
    private http = inject(HttpClient);
    private baseUrl = environment.apiUrl;
    private userUrl = `${this.baseUrl}/users`;
    
    getUsers(){
      return this.http.get<AppUser[]>(`${this.userUrl}`);
    }
    getUser(id:string){
      return this.http.get<AppUser>(`${this.userUrl}/${id}`);
    }
}
