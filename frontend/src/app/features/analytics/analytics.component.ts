import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HabitService } from '../../core/services/habit.service';
import { TaskService } from '../../core/services/task.service';

@Component({
    selector: 'app-analytics',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './analytics.component.html',
    styleUrls: ['./analytics.component.css']
})
export class AnalyticsComponent implements OnInit {
    private habitService = inject(HabitService);
    private taskService = inject(TaskService);

    summary: any = { totalHabits: 0, totalCompletions: 0 };
    heatmapData: any[] = [];
    sessionLogs: any[] = []; // Pomodoro sessions
    completedTasks: any[] = []; // Task logs

    // Create a 30-day looking back grid array
    last30Days: { date: string, count: number }[] = [];
    habits: any[] = [];

    ngOnInit() {
        this.habitService.getSummary().subscribe((data: any) => this.summary = data);
        this.habitService.getHeatmap().subscribe((data: any) => {
            this.heatmapData = data;
            this.generateGrid();
        });

        // Fetch Pomodoro Session Logs
        this.habitService.getPomodoroSessions().subscribe((data) => {
            this.sessionLogs = data;
        });

        // Fetch Habits to map names
        this.habitService.getHabits().subscribe((habits: any[]) => {
            this.habits = habits;
        });

        // Fetch Completed Tasks for the new Task Logs
        this.taskService.getTasks().subscribe((tasks: any[]) => {
            this.completedTasks = tasks.filter(t => t.isCompleted).sort((a,b) => {
                const dateB = b.completedAt ? new Date(b.completedAt) : new Date(b.createdAt || '');
                const dateA = a.completedAt ? new Date(a.completedAt) : new Date(a.createdAt || '');
                return dateB.getTime() - dateA.getTime();
            });
        });
    }

    getHabitName(habitId: string): string {
        const habit = this.habits.find(h => h.id === habitId);
        return habit ? habit.title : habitId;
    }

    generateGrid() {
        this.last30Days = [];
        const today = new Date();

        for (let i = 29; i >= 0; i--) {
            const d = new Date(today);
            d.setDate(d.getDate() - i);
            const dateStr = d.toISOString().split('T')[0];

            const found = this.heatmapData.find(x => x.Date === dateStr || x.date === dateStr);

            this.last30Days.push({
                date: dateStr,
                count: found ? found.Count || found.count : 0
            });
        }
    }

    getColorIntensity(count: number): string {
        if (count === 0) return 'var(--surface-color)';
        if (count === 1) return 'rgba(16, 185, 129, 0.4)';
        if (count === 2) return 'rgba(16, 185, 129, 0.7)';
        return 'rgba(16, 185, 129, 1)'; // Max intensity for 3+
    }
}
