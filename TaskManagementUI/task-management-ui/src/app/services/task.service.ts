import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WorkTask } from '../models/work-task';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  // Ensure this matches your running .NET Core API port (e.g., 7041 or 5000)
  private apiUrl = 'https://localhost:7041/api/tasks';

  constructor(private http: HttpClient) {}

  getAll(): Observable<WorkTask[]> {
    return this.http.get<WorkTask[]>(this.apiUrl);
  }

  getById(id: number): Observable<WorkTask> {
    return this.http.get<WorkTask>(`${this.apiUrl}/${id}`);
  }

  create(task: Partial<WorkTask>): Observable<WorkTask> {
    return this.http.post<WorkTask>(this.apiUrl, task);
  }

  update(id: number, task: Partial<WorkTask>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, task);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}