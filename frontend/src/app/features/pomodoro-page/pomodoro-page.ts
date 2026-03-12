import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PomodoroComponent } from '../../shared/components/pomodoro/pomodoro.component';

@Component({
  selector: 'app-pomodoro-page',
  standalone: true,
  imports: [CommonModule, PomodoroComponent],
  templateUrl: './pomodoro-page.html',
  styleUrls: ['./pomodoro-page.css']
})
export class PomodoroPage {}
