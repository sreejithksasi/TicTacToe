# C# Dynamic Tic-Tac-Toe Engine

A highly scalable, UI-agnostic Tic-Tac-Toe engine built in C#. This project goes beyond the standard 3x3 console game by implementing a fully dynamic $N \times N$ board using modern software architecture principles.

## 🚀 Features

* **Dynamic Scaling:** Play on any board size (from $3 \times 3$ up to $100 \times 100$). The win conditions and rendering logic scale mathematically.
* **MVC Architecture:** Strict separation of concerns between the Game Logic (Model), Input/Display (View), and Turn Execution (Controller).
* **Dependency Injection:** Player classes (`Move` and `OMoveEasyAI`) rely on an `IInputProvider` interface, making the core game loop completely decoupled from the console. This engine is ready to be dropped into a WPF, WinForms, or Unity application.
* **Asynchronous Execution:** Utilizes `async/await` Tasks for the game loop and input reading, ensuring non-blocking operations for future GUI implementations.
* **Optimized Rendering:** Uses `StringBuilder` for O(1) string manipulation, preventing memory allocation bloat during board redraws.

## 🏗️ Architecture

* **`Board` (Model):** Manages the 1D array state, handles $N \times N$ win evaluations (Row, Column, Main Diagonal, Off-Diagonal), and validates moves.
* **`IRenderer / IInputProvider` (View):** Interface-driven display and input boundaries. Currently implemented via `ConsoleRenderer` and `ConsoleInputProvider`.
* **`IMove` (Controller):** Defines how a turn is executed. Implemented by human controllers and an Easy AI controller.
* **`GameManager`:** The application bootstrap that wires up dependencies and handles session loops.

## 🎮 Getting Started

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) (or compatible modern .NET version)

### Installation & Run
1. Clone the repository:
  ```bash
git clone https://github.com/sreejithksasi/TicTacToe-Engine.git
```
