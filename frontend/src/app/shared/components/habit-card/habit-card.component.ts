import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Habit } from '../../../core/services/habit.service';

@Component({
    selector: 'app-habit-card',
    standalone: true,
    imports: [CommonModule],
    templateUrl: './habit-card.component.html',
    styleUrls: ['./habit-card.component.css']
})
export class HabitCardComponent {
    @Input() habit!: Habit;
    @Input() isCompleted: boolean = false;

    @Output() toggleCompletion = new EventEmitter<string>();
    @Output() edit = new EventEmitter<Habit>();
    @Output() delete = new EventEmitter<string>();

    onToggle() {
        if (this.habit.id) {
            this.toggleCompletion.emit(this.habit.id);
        }
    }

    onEdit() {
        this.edit.emit(this.habit);
    }

    onDelete() {
        if (this.habit.id) {
            this.delete.emit(this.habit.id);
        }
    }
}
