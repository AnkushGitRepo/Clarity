import { Injectable, inject } from '@angular/core';
import { HabitService, PomodoroSession } from './habit.service';

export type TimerMode = 'pomodoro' | 'stopwatch';
export type PomodoroPhase = 'work' | 'break';

@Injectable({
  providedIn: 'root'
})
export class TimerService {
  private habitService = inject(HabitService);

  // Settings
  mode: TimerMode = 'pomodoro';
  phase: PomodoroPhase = 'work';
  workDuration: number = 25; // in minutes
  breakDuration: number = 5; // in minutes

  // State
  timeLeft: number = 25 * 60; // used for pomodoro
  elapsedTime: number = 0; // used for stopwatch
  isRunning: boolean = false;
  timerInterval: any;

  selectedHabitId: string | null = null;

  start() {
    if (this.isRunning) return;
    this.isRunning = true;
    
    this.timerInterval = setInterval(() => {
      if (this.mode === 'pomodoro') {
        this.timeLeft--;
        if (this.timeLeft <= 0) {
          this.completeSession();
        }
      } else {
        this.elapsedTime++;
      }
    }, 1000);
  }

  pause() {
    this.isRunning = false;
    clearInterval(this.timerInterval);
  }

  reset() {
    this.pause();
    if (this.mode === 'pomodoro') {
      this.timeLeft = this.phase === 'work' ? this.workDuration * 60 : this.breakDuration * 60;
    } else {
      this.elapsedTime = 0;
    }
  }

  setMode(newMode: TimerMode) {
    this.mode = newMode;
    this.reset();
  }

  switchPhase(newPhase?: PomodoroPhase) {
    if (newPhase) {
      this.phase = newPhase;
    } else {
      this.phase = this.phase === 'work' ? 'break' : 'work';
    }
    this.reset();
  }

  updateWorkDuration(minutes: number) {
    this.workDuration = minutes;
    if (!this.isRunning && this.phase === 'work') {
      this.timeLeft = this.workDuration * 60;
    }
  }

  updateBreakDuration(minutes: number) {
    this.breakDuration = minutes;
    if (!this.isRunning && this.phase === 'break') {
      this.timeLeft = this.breakDuration * 60;
    }
  }

  completeSession() {
    this.pause();
    
    if (this.phase === 'work' && this.mode === 'pomodoro') {
      const sessionData: PomodoroSession = {
        durationMinutes: this.workDuration,
        habitId: this.selectedHabitId || null
      };

      this.habitService.logPomodoroSession(sessionData).subscribe({
        next: () => console.log(`Logged ${this.workDuration} min session.`),
        error: (err) => console.error('Error logging session', err)
      });
      alert('Work session complete! Take a break.');
      // The user must manually switch to break timer so we don't auto-switch.
      this.timeLeft = 0; 
    }
  }

  finishStopwatchSession() {
    this.pause();
    const minutes = Math.floor(this.elapsedTime / 60);
    if (minutes > 0) {
       const sessionData: PomodoroSession = {
         durationMinutes: minutes,
         habitId: this.selectedHabitId || null
       };
       this.habitService.logPomodoroSession(sessionData).subscribe();
       alert(`Logged ${minutes} min session.`);
    } else {
       alert('Session too short to log (under 1 minute).');
    }
    this.reset();
  }
}
