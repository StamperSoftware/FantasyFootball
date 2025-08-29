import { Component, input, model, signal } from '@angular/core';
import { Position } from "@models";
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-position-dropdown',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './position-dropdown.component.html',
  styleUrl: './position-dropdown.component.scss'
})
export class PositionDropdownComponent {
    position = model<Position|'all'>();
    protected readonly Position = Position;
}
