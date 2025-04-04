import { CanActivateFn, Router } from '@angular/router';
import { inject } from "@angular/core";
import { AccountService } from "../services/account.service";

export const siteAdminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);
  
  if (!accountService.isSiteAdmin()){
    router.navigateByUrl("/");
    return false;
  }
  return true;
};
