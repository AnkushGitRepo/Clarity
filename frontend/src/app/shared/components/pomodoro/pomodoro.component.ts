import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Habit, HabitService } from '../../../core/services/habit.service';
import { TimerService, TimerMode, PomodoroPhase } from '../../../core/services/timer.service';

@Component({
    selector: 'app-pomodoro',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './pomodoro.component.html',
    styleUrls: ['./pomodoro.component.css']
})
export class PomodoroComponent implements OnInit {
    private habitService = inject(HabitService);
    public timerService = inject(TimerService);

    habits: Habit[] = [];

    ngOnInit() {
        this.habitService.getHabits().subscribe((data: Habit[]) => this.habits = data);
    }

    get formattedTime(): string {
        const time = this.timerService.mode === 'pomodoro' 
            ? Math.max(0, this.timerService.timeLeft)
            : this.timerService.elapsedTime;
            
        const minutes = Math.floor(time / 60);
        const seconds = time % 60;
        return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    }

    setMode(mode: 'pomodoro' | 'stopwatch') {
        this.timerService.setMode(mode);
    }
    
    setPhase(phase: 'work' | 'break') {
        this.timerService.switchPhase(phase);
    }

    onWorkDurationChange(event: Event) {
        const value = parseInt((event.target as HTMLInputElement).value, 10);
        if (!isNaN(value) && value > 0) {
            this.timerService.updateWorkDuration(value);
        }
    }

    onBreakDurationChange(event: Event) {
        const value = parseInt((event.target as HTMLInputElement).value, 10);
        if (!isNaN(value) && value > 0) {
            this.timerService.updateBreakDuration(value);
        }
    }
}
