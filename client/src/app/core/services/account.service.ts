import { computed, inject, Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { AppUser, LoginDto, RegisterDto } from "../../models";
import { map } from "rxjs";
import { Router } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AccountService {
    
    private http = inject(HttpClient);
    private baseUrl = environment.apiUrl;
    private accountUrl = `${this.baseUrl}/accounts`;
    private router = inject(Router);
    
    currentUser = signal<AppUser | null>(null);
    
    isSiteAdmin = computed(()=>{
      const roles = this.currentUser()?.roles;
      return Array.isArray(roles) ? roles.includes("SiteAdmin") : roles === "SiteAdmin";
    });
    
    registerUser(registerDto:RegisterDto){
        let params = new HttpParams();
        params = params.append('useCookies', true);
        
        return this.http.post(`${this.accountUrl}/register`, registerDto, {params});
    }
    
    login(loginDto:LoginDto, success?:()=>void, fail?:(err?:string)=>void) {
        
        let params = new HttpParams();
        params = params.append('useCookies', true);
        
        return this.http.post(`${this.baseUrl}/login`, loginDto, {params})
            .subscribe({
                next: () => {
                    this.getUserInfo().subscribe({
                        next:() => success?.(),
                        error : (err) => fail?.(err),
                    });
                },
                error : (err) => fail?.(err),
            });
    }
    
    logout(){
      return this.http.post(`${this.accountUrl}/logout`, {}).subscribe({
          next:() => {
              this.currentUser.set(null);
              this.router.navigateByUrl("/");
          },
      });
    }
    
    getAuthStatus() {
      return this.http.get<{isAuthenticated:boolean}>(`${this.accountUrl}/auth-status`);
    }
    
    getUserInfo(){
        return this.http.get<AppUser>(`${this.accountUrl}/user-info`)
        .pipe(
            map(user=>{
              this.currentUser.set(user);
              return user;
            })
        );
    }
}
