import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormGroup,
  FormControl,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from '../../services/task.service';
import { TaskStatus, WorkTask } from '../../models/work-task';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-form.html'
})
export class TaskFormComponent implements OnInit {
  taskId: number | null = null;
  TaskStatus = TaskStatus;

  taskForm = new FormGroup({
    title: new FormControl('', [
      Validators.required,
      Validators.maxLength(100)
    ]),

    description: new FormControl(''),

    status: new FormControl<TaskStatus>(
      TaskStatus.Pending,
      Validators.required
    ),

    estimatedHours: new FormControl<number | null>(1, [
      Validators.required,
      Validators.min(0.01)
    ])
  });

  constructor(
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (id > 0) {
      this.taskId = id;

      this.taskService.getById(id).subscribe({
        next: task => this.taskForm.patchValue({
          title: task.title,
          description: task.description,
          status: task.status,
          estimatedHours: task.estimatedHours
        })
      });
    }
  }

 save(): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    const value = this.taskForm.getRawValue();

    const task: Partial<WorkTask> = {
      title: value.title!.trim(),
      description: value.description ?? '',
      status: value.status!,
      estimatedHours: value.estimatedHours!
    };

    if (this.taskId) {
      this.taskService.update(this.taskId, task).subscribe({
        next: () => this.router.navigate(['/tasks']),
        error: error => console.error('Unable to update task', error)
      });
    } else {
      this.taskService.create(task).subscribe({
        next: () => this.router.navigate(['/tasks']),
        error: error => console.error('Unable to create task', error)
      });
    }
  }
}