import { Component, inject, OnInit } from '@angular/core';
import { UserService } from "../../../core/services/user.service";
import { AppUser } from "../../../models";

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class UserListComponent implements OnInit {
    ngOnInit(): void {
        this.getUsers();    
    }
    
    private userService = inject(UserService);
    
    users?:AppUser[];
    hasErrors=false;
    
    private getUsers() {
        this.userService.getUsers().subscribe({
            next: (response) => this.users = response,
            error  : (err) => this.hasErrors = true
        });
    }
}
