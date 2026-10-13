import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  imports: [RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.css',
  template: `
    <div class="container mt-4">
      <!-- This dynamically loads TaskListComponent or TaskFormComponent based on the URL -->
      <router-outlet></router-outlet>
    </div>
  `,
})
export class App {
    title = 'task-management-client';
}

