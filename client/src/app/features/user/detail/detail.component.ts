import { Component, inject, OnInit } from '@angular/core';
import { UserService } from "../../../core/services/user.service";
import { AppUser } from "@models";
import { ActivatedRoute } from "@angular/router";
import { AccountService } from "../../../core/services/account.service";

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [],
  templateUrl: './detail.component.html',
  styleUrl: './detail.component.scss'
})
export class UserDetailComponent implements OnInit {
    ngOnInit(): void {
        this.getUser();
    }
    
    private activatedRoute = inject(ActivatedRoute);
    private userService = inject(UserService);
    accountService = inject(AccountService);
    private userId?:string = this.activatedRoute.snapshot.paramMap.get("id") ?? "";
    
    user?:AppUser;
    hasErrors = false;
    
    private getUser(){
      
      if (!this.userId) {
        this.hasErrors = true; 
        return;
      }
      
      this.userService.getUser(this.userId).subscribe({
        next: (user) => this.user = user,
        error: (err) => this.hasErrors = true
      });
    }
}
