Core Defense

Name: Dylan Quan, ID: 100982485, Role: Solo dev, 

1. Game Description

Core Defense is a top-down 2D defense game made in Unity. A reactor core sits in the middle of a single arena, and enemies attack it from every direction. The player fights directly with a mouse-aimed weapon and builds automated turrets around the core between waves. The project is the foundation for the course: every system is built around design patterns (Singleton, Factory, Command) and a custom C++ plugin.

2. Interactive Media Scenario

The purpose is to protect a central reactor core while enemies attack from every direction. The player must survive 10 waves. Each wave starts with a 20-second build phase in which the player spends scrap to place Gun and Cannon turrets on a grid around the core and can sell, undo or redo any placement. In the defend phase, Walkers, Runners and Brutes pour in from the arena edges; the player moves, aims and shoots manually while turrets fire automatically at the nearest enemy. Enemies drop scrap, Runners explode on contact and Brutes wreck turrets. All controls can be rebound in the Controls menu. The game is lost if the core's health reaches zero and won when wave 10 is cleared.

Why this scene suits the project: a single global game state (phase, wave, scrap) needs a Singleton; several enemy types spawned by a wave system suit the Factory pattern; turret placement with undo/redo and rebindable controls are natural uses of the Command pattern; and dozens of enemies moving at once make enemy steering a meaningful job for a native plugin.
<img width="821" height="459" alt="image" src="https://github.com/user-attachments/assets/c8dbe0d0-bb07-46cd-a795-fb1d7f70545d" />
<img width="816" height="458" alt="image" src="https://github.com/user-attachments/assets/ed81fba7-538b-45b0-be9d-287d35d38b50" />
<img width="815" height="459" alt="image" src="https://github.com/user-attachments/assets/f3ddd68a-6f34-4b6d-b780-1f91c0b29f9f" />

Controls

Move - W A S D

Aim - Mouse

Shoot - Left Click (hold)

Dash - Space

Select Gun / Cannon turret - 1 / 2

Place turret / Sell turret - Right Click / X

Cancel selection - Esc

Undo / Redo (build phase) - Z / Y

Start wave early - Enter

Controls menu - Tab

Switch steering backend (C++ / C#) - F2

3. Group Formation
I completed this project solo as my own producer, splitting the work into small milestones with planned dates, tracking each task from to-do to done, and committing to the repository after every milestone. I covered every role: producer, gameplay, systems, plugin and UI programming, QA, and documentation.

Gantt Chart:
<img width="2768" height="632" alt="image" src="https://github.com/user-attachments/assets/6d426ca7-31c7-4b33-9e8f-4e07e55b9ff4" />

4. Singleton - GameManager

<img width="588" height="717" alt="image" src="https://github.com/user-attachments/assets/7c0a024a-b55c-45d9-bf4a-5a227c58ac6f" />


How: generic Singleton<T> keeps one persistent instance (DontDestroyOnLoad); GameManager holds the state, wave and scrap.

Why: the game needs one source of truth that every system can reach.

Benefit: consistent game flow and restart; the core and player register themselves, and the UI listens to events, so coupling stays low.

5. Factory – Enemy spawners
<img width="771" height="757" alt="image" src="https://github.com/user-attachments/assets/7814499e-1261-4fe8-b315-21f476b1cfdf" />

How: WaveManager only calls the abstract SpawnEnemy(); each spawner creates its own enemy type.

Why: new enemy types are added without changing the wave code.

Benefit: waves mix Walkers, Runners and Brutes and are balanced from the Inspector.

6. Command – Undo/redo turret building and key rebinding
<img width="1160" height="533" alt="image" src="https://github.com/user-attachments/assets/7e21273d-0161-4993-ba78-c3db4b240650" />
<img width="720" height="202" alt="image" src="https://github.com/user-attachments/assets/a9ffd00d-d80f-4d9f-961b-4dd2c0f6ff6d" />

How: every key press runs a command object; placing and selling turrets are undoable commands kept in undo/redo stacks.

Why: it separates input from game logic, and each command remembers enough to reverse itself.

Benefit: free undo/redo while planning the turret layout, and every key can be rebound (saved between sessions).

7. Plugin – C++ enemy steering (DLL)
<img width="494" height="958" alt="image" src="https://github.com/user-attachments/assets/25b316d6-240c-4069-8656-d724c4066232" />
<img width="321" height="96" alt="image" src="https://github.com/user-attachments/assets/7fc26fde-68cc-474f-ac81-46db60b4b2a6" />

How: SteeringPlugin.dll (C++, extern "C") computes all enemy velocities in one call per physics step; Unity calls it with [DllImport].

Why: comparing every enemy with every other one is heavy math that suits native code; a C# fallback keeps the game running if the DLL is missing.

Benefit: enemies spread out and surround the core instead of clumping, so every side must be defended.

8. How to Run

Download the zip from the latest Release, extract it, and run the .exe (Windows 10+, 64-bit).

9. Contribution Statement

Dylan Quan (100982485) – 100%. I declare that I am the sole member of this project and responsible for all submitted work.

10. Credits
- Unity 6 and TextMeshPro – Unity Technologies
- INFR 3110U Lectures 03 and 06 (Singleton, Factory, Command, native plugins)
