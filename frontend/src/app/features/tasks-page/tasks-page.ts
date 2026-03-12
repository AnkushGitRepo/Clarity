import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DragDropModule, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { TaskService, TaskItem } from '../../core/services/task.service';

@Component({
  selector: 'app-tasks-page',
  standalone: true,
  imports: [CommonModule, FormsModule, DragDropModule],
  templateUrl: './tasks-page.html',
  styleUrls: ['./tasks-page.css']
})
export class TasksPageComponent implements OnInit {
  private taskService = inject(TaskService);

  monthlyTasks: TaskItem[] = [];
  weeklyTasks: TaskItem[] = [];
  dailyTasks: TaskItem[] = [];

  isModalOpen = false;
  isEditing = false;
  currentTask: TaskItem = this.getEmptyTask();
  
  presetColors = ['#ef4444', '#3b82f6', '#10b981', '#f59e0b', '#8b5cf6'];
  
  availableParents: TaskItem[] = [];

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe({
      next: (tasks) => {
        this.monthlyTasks = tasks.filter(t => t.type === 'Monthly' && !t.isCompleted);
        this.weeklyTasks = tasks.filter(t => t.type === 'Weekly' && !t.isCompleted);
        this.dailyTasks = tasks.filter(t => t.type === 'Daily' && !t.isCompleted);
      },
      error: (err) => console.error(err)
    });
  }

  getEmptyTask(): TaskItem {
    return { title: '', type: 'Daily', color: '#3b82f6' };
  }

  openModal(task?: TaskItem, targetType?: string) {
    if (task) {
      this.isEditing = true;
      this.currentTask = { ...task };
    } else {
      this.isEditing = false;
      this.currentTask = this.getEmptyTask();
      if (targetType) {
        this.currentTask.type = targetType;
      }
    }
    
    // Evaluate available parents (e.g., if Daily, show Weekly+Monthly options)
    const allTasks = [...this.monthlyTasks, ...this.weeklyTasks, ...this.dailyTasks];
    if (this.currentTask.type === 'Daily') {
      this.availableParents = allTasks.filter(t => t.type === 'Monthly' || t.type === 'Weekly');
    } else if (this.currentTask.type === 'Weekly') {
      this.availableParents = allTasks.filter(t => t.type === 'Monthly');
    } else {
      this.availableParents = [];
      this.currentTask.parentTaskId = null;
    }

    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }
  
  onTypeChange() {
    // Re-evaluate parents on type change
    const allTasks = [...this.monthlyTasks, ...this.weeklyTasks, ...this.dailyTasks];
    if (this.currentTask.type === 'Daily') {
      this.availableParents = allTasks.filter(t => t.type === 'Monthly' || t.type === 'Weekly');
    } else if (this.currentTask.type === 'Weekly') {
      this.availableParents = allTasks.filter(t => t.type === 'Monthly');
    } else {
      this.availableParents = [];
      this.currentTask.parentTaskId = null;
    }
  }

  saveTask() {
    if (!this.currentTask.title) return;

    if (this.isEditing && this.currentTask.id) {
      this.taskService.updateTask(this.currentTask.id, this.currentTask).subscribe(() => {
        this.loadTasks();
        this.closeModal();
      });
    } else {
      this.taskService.createTask(this.currentTask).subscribe(() => {
        this.loadTasks();
        this.closeModal();
      });
    }
  }

  deleteTask(id: string, event: Event) {
    event.stopPropagation();
    if (confirm('Delete this task?')) {
      this.taskService.deleteTask(id).subscribe(() => this.loadTasks());
    }
  }

  markComplete(task: TaskItem, event: Event) {
    event.stopPropagation();
    if (!task.id) return;
    this.taskService.updateTask(task.id, { isCompleted: true }).subscribe(() => this.loadTasks());
  }

  drop(event: CdkDragDrop<TaskItem[]>, newType: string) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      const transferredTask = event.previousContainer.data[event.previousIndex];
      transferredTask.type = newType;
      
      transferArrayItem(
        event.previousContainer.data,
        event.container.data,
        event.previousIndex,
        event.currentIndex,
      );

      if (transferredTask.id) {
        this.taskService.updateTask(transferredTask.id, { type: newType }).subscribe();
      }
    }
  }

  getParentTitle(parentId?: string | null): string {
    if (!parentId) return '';
    const allTasks = [...this.monthlyTasks, ...this.weeklyTasks, ...this.dailyTasks];
    const parent = allTasks.find(t => t.id === parentId);
    return parent ? parent.title : '';
  }
}
