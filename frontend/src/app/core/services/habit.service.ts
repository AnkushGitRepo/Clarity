import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Habit {
  id?: string;
  userId?: string;
  title: string;
  description?: string;
  frequency: string;
  color: string;
  streakCurrent?: number;
  streakLongest?: number;
}

export interface PomodoroSession {
  id?: string;
  userId?: string;
  habitId?: string | null;
  durationMinutes: number;
  completedAt?: string;
}

export interface HabitLog {
  id: string;
  habitId: string;
  userId: string;
  completedDate: string;
  status: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class HabitService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5239/api';

  getHabits(): Observable<Habit[]> {
    return this.http.get<Habit[]>(`${this.apiUrl}/habits`);
  }

  createHabit(habit: Habit): Observable<Habit> {
    return this.http.post<Habit>(`${this.apiUrl}/habits`, habit);
  }

  updateHabit(id: string, habit: Habit): Observable<any> {
    return this.http.put(`${this.apiUrl}/habits/${id}`, habit);
  }

  deleteHabit(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/habits/${id}`);
  }

  logHabit(id: string): Observable<HabitLog> {
    return this.http.post<HabitLog>(`${this.apiUrl}/habits/${id}/log`, {});
  }

  getTodayLogs(): Observable<HabitLog[]> {
    return this.http.get<HabitLog[]>(`${this.apiUrl}/habits/logs/today`);
  }

  getHeatmap(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/analytics/heatmap`);
  }

  getSummary(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/analytics/summary`);
  }

  logPomodoroSession(session: PomodoroSession): Observable<PomodoroSession> {
    return this.http.post<PomodoroSession>(`${this.apiUrl}/pomodoro/log`, session);
  }

  getPomodoroSessions(): Observable<PomodoroSession[]> {
    return this.http.get<PomodoroSession[]>(`${this.apiUrl}/pomodoro/sessions`);
  }
}
