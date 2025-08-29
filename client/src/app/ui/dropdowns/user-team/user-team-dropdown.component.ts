import { Component, input, model } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { UserTeam } from "@models";

@Component({
  selector: 'app-user-team-dropdown',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './user-team-dropdown.component.html',
  styleUrl: './user-team-dropdown.component.scss'
})
export class UserTeamDropdownComponent {

  userTeamId = model<number|undefined>(undefined);
  options = input<UserTeam[]>();
}
