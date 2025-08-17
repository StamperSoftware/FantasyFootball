import { Component, input, model } from '@angular/core';
import { FormsModule } from "@angular/forms";

@Component({
  selector: 'app-floating-input',
  standalone: true,
  imports: [
    FormsModule
  ],
  templateUrl: './floating-input.component.html',
  styleUrl: './floating-input.component.scss'
})
export class FloatingInputComponent {

  placeholder = input("");
  type = input<Type>("text");
  id = input("");
  name = input("");
  value=model();
 
  updateValue(e: Event){
    const newValue = (e.target as HTMLInputElement).value;
    this.value.set(newValue);
  }
}

type Type = "number" | "text"