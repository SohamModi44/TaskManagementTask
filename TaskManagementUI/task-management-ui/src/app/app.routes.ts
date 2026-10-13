import { Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list';
import { TaskFormComponent } from './components/task-form/task-form';

export const routes: Routes = [
  { path: '', redirectTo: 'tasks', pathMatch: 'full' }, // Redirect root to tasks
  { path: 'tasks', component: TaskListComponent },
  { path: 'tasks/new', component: TaskFormComponent },
  { path: 'tasks/edit/:id', component: TaskFormComponent },
  { path: '**', redirectTo: 'tasks' } 
];