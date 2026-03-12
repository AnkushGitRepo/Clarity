import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Habit, HabitLog, HabitService } from '../../core/services/habit.service';
import { HabitCardComponent } from '../../shared/components/habit-card/habit-card.component';
import confetti from 'canvas-confetti';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [CommonModule, FormsModule, HabitCardComponent],
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    private habitService = inject(HabitService);

    habits: Habit[] = [];
    todayLogs: HabitLog[] = [];

    todayStr: string = new Date().toISOString().split('T')[0];
    todayDisplay: string = new Date().toLocaleDateString(undefined, { weekday: 'long', month: 'long', day: 'numeric' });

    ngOnInit() {
        this.loadDashboardData();
    }

    presetColors = ['#f56565', '#4299e1', '#48bb78', '#ecc94b'];

    isModalOpen = false;
    isEditing = false;
    currentHabit: Habit = {
        title: '',
        description: '',
        frequency: 'Daily',
        color: '#3b82f6'
    };

    loadDashboardData() {
        this.habitService.getHabits().subscribe((habits: Habit[]) => this.habits = habits);
        this.habitService.getTodayLogs().subscribe((logs: HabitLog[]) => {
            logs.forEach(log => {
                this.isCompletedTurnedOnLocally[log.habitId] = log.status;
            });
        });
    }

    openModal(habit?: Habit) {
        if (habit) {
            this.isEditing = true;
            this.currentHabit = { ...habit };
        } else {
            this.isEditing = false;
            this.currentHabit = { title: '', description: '', frequency: 'Daily', color: '#3b82f6' };
        }
        this.isModalOpen = true;
    }

    closeModal() {
        this.isModalOpen = false;
        this.isEditing = false;
        this.currentHabit = { title: '', description: '', frequency: 'Daily', color: '#3b82f6' };
    }

    saveHabit() {
        if (!this.currentHabit.title) return;

        if (this.isEditing && this.currentHabit.id) {
            this.habitService.updateHabit(this.currentHabit.id, this.currentHabit).subscribe({
                next: () => {
                    const idx = this.habits.findIndex(h => h.id === this.currentHabit.id);
                    if (idx !== -1) this.habits[idx] = { ...this.currentHabit };
                    this.closeModal();
                }
            });
        } else {
            this.habitService.createHabit(this.currentHabit).subscribe({
                next: (habit) => {
                    this.habits.push(habit);
                    this.closeModal();
                }
            });
        }
    }

    deleteHabit(id: string) {
        if (confirm('Are you sure you want to delete this habit?')) {
            this.habitService.deleteHabit(id).subscribe({
                next: () => {
                    this.habits = this.habits.filter(h => h.id !== id);
                }
            });
        }
    }

    isCompletedTurnedOnLocally: { [key: string]: boolean } = {};

    onToggleHabit(habitId: string) {
        this.habitService.logHabit(habitId).subscribe((log: HabitLog) => {
            this.isCompletedTurnedOnLocally[habitId] = log.status;

            if (log.status) {
                // Trigger confetti if marked as complete!
                confetti({
                    particleCount: 100,
                    spread: 70,
                    origin: { y: 0.6 }
                });
            }

            // Refetch habits to instantly get the newly updated Current and Longest Streak metrics
            // from the backend calculations
            this.habitService.getHabits().subscribe((habits: Habit[]) => this.habits = habits);
        });
    }

    isHabitCompleted(habitId: string): boolean {
        return !!this.isCompletedTurnedOnLocally[habitId];
    }
}
