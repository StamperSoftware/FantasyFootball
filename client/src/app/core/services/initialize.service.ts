import { inject, Injectable } from '@angular/core';
import { AccountService } from "./account.service";

@Injectable({
  providedIn: 'root'
})
export class InitializeService {
  private accountService = inject(AccountService);  
  
  init() {
    const user = this.accountService.getUserInfo();
    return user;
  }
}
