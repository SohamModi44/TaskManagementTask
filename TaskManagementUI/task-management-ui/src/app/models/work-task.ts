export enum TaskStatus {
  Pending = 0,
  InProgress = 1,
  Completed = 2
}

export interface WorkTask {
  id: number;
  title: string;
  description: string;
  status: TaskStatus;
  estimatedHours: number;
  createdAt: string;
}