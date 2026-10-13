import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router'; // <-- Add Router import
import { Subject, debounceTime, distinctUntilChanged, startWith, takeUntil } from 'rxjs';

import { TaskService } from '../../services/task.service';
import { WorkTask, TaskStatus } from '../../models/work-task';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-list.html'
})
export class TaskListComponent implements OnInit, OnDestroy {
  tasks: WorkTask[] = [];
  filteredTasks: WorkTask[] = [];
  searchText = '';

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();
  TaskStatus = TaskStatus;

  constructor(private taskService: TaskService, private router: Router) {} // <-- Inject Router

  ngOnInit(): void {
    this.loadTasks();

    this.searchSubject.pipe(
      startWith(''),
      debounceTime(200),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(search => {
      const term = search.trim().toLowerCase();
      this.filteredTasks = this.tasks.filter(task =>
        task.title.toLowerCase().includes(term)
      );
    });
  }

  loadTasks(): void {
    this.taskService.getAll().subscribe({
      next: tasks => {
        this.tasks = tasks;
        this.searchSubject.next(this.searchText);
      },
      error: error => console.error('Unable to load tasks', error)
    });
  }

  onSearch(value: string): void {
    this.searchSubject.next(value);
  }

  deleteTask(id: number): void {
    if (!confirm('Are you sure you want to delete this task?')) {
      return;
    }

    this.taskService.delete(id).subscribe({
      next: () => this.loadTasks(),
      error: error => console.error('Unable to delete task', error)
    });
  }

  editTask(id: number): void {
    this.router.navigate(['/tasks/edit', id]); // <-- Fixed navigation
  }

  getStatusName(status: TaskStatus): string {
    return TaskStatus[status];
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}