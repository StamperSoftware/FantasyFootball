import { inject, Injectable } from '@angular/core';
import { AccountService } from "./account.service";
import { SiteSettingsComponent } from "../../features/admin/site-settings/site-settings.component";
import { SiteSettingsService } from "./site-settings.service";
import { forkJoin } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class InitializeService {
  
  private accountService = inject(AccountService);  
  private siteSettingsService = inject(SiteSettingsService);
  
  init() {
    const user = this.accountService.getUserInfo();
    const siteSettings = this.siteSettingsService.getSettings();
    return forkJoin([user, siteSettings]);
  }
}
