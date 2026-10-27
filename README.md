Core Defense

Name: Dylan Quan, ID: 100982485, Role: Solo dev, 

1. Game Description

Core Defense is a top-down 2D defense game made in Unity 6. A reactor core sits in the middle of a single arena, and enemies attack it from every direction. The player fights directly with a mouse-aimed weapon and builds automated turrets around the core between waves. The project is the foundation for the course: every system is built around design patterns (Singleton, Factory, Command) and a custom C++ plugin.

2. Interactive Media Scenario

The purpose is to protect a central reactor core while enemies attack from every direction. The player must survive 10 waves. Each wave starts with a 20-second build phase in which the player spends scrap to place Gun and Cannon turrets on a grid around the core and can sell, undo or redo any placement. In the defend phase, Walkers, Runners and Brutes pour in from the arena edges; the player moves, aims and shoots manually while turrets fire automatically at the nearest enemy. Enemies drop scrap, Runners explode on contact and Brutes wreck turrets. All controls can be rebound in the Controls menu. The game is lost if the core's health reaches zero and won when wave 10 is cleared.
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

3. 
