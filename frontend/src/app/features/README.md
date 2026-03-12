# Features Module (`src/app/features/`)

The Features layer contains the main application "Pages". These are smart, routable components that combine data from Core Services and render it using Shared UI Components. 

Each feature is conceptually isolated; the Pomodoro page doesn't care how the Dashboard works.

## 1. `auth/` (Login & Signup)
*   **Logic:** Reactive Forms mapped to `AuthService`. Handles client-side validation (ensuring emails look like emails) before submitting to the backend. Displays error banners using `*ngIf` if the backend returns unauthorized (e.g., incorrect password).

## 2. `dashboard/`
*   **Logic:** The hub for habits. Uses the `HabitService` to fetch the user's current habits. It renders a grid of custom `<app-habit-card>` elements, passing individual habit data to each card via `@Input()`. When a user interacts with a card, the Dashboard handles the master API update.

## 3. `tasks-page/` (Kanban Board)
*   **Logic:** Integrates the `@angular/cdk/drag-drop` library. 
*   **Structure:** It maps an array of Tasks into three separate arrays: `monthlyTasks`, `weeklyTasks`, and `dailyTasks`. Each section is rendered as a drop-zone container (`cdkDropList`). 
*   **Movement:** When a user drags a task between columns, the `drop(event)` function triggers, intercepting the exact index. The component immediately fires a `PUT` request to update the task's `Type` in MongoDB, creating a seamless, physical organizational experience.

## 4. `pomodoro-page/`
*   **Logic:** Subscribes to the global `TimerService` observables.
*   **Action:** Displays dynamic formatting of minutes and seconds (`mm:ss`). When the timer completes, it plays a browser Notification API sound, automatically evaluates if the user completed a Work or Break cycle, and triggers a `POST` request to the backend to permanently log the focused session.

## 5. `analytics/`
*   **Logic:** The data visualization center. It fetches aggregate counts from the backend and constructs arrays tailored for UI rendering.
*   **Heatmap:** Meticulously constructs a 30-day sequential grid array, matching dates against the raw dictionary counts returned by MongoDB to determine the exact CSS background-opacity for the "GitHub-style" contribution graph. Defines explicit chronological sorting for recently completed sessions and tasks.
