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

4. Singleton - GameManager

How it was implemented in my project:
Singleton<T> is a generic MonoBehaviour base class. A private static _instance holds the only instance, and the public static Instance property returns it, finding it in the scene or creating it if it does not exist yet.

Awake() keeps the first instance and marks it DontDestroyOnLoad; any duplicate destroys itself.

HasInstance lets scripts check whether the manager exists without accidentally creating one.

An _isQuitting guard stops a new GameManager from being created during application shutdown.

GameManager:
Singleton<GameManager> owns the game state (Build, Defend, Victory, Defeat), the wave number, the scrap economy, the build timer, pausing, player respawn and restarting.

Scene objects register themselves: CoreObjective and PlayerController call RegisterCore / RegisterPlayer. Because the manager survives scene reloads, this keeps it pointed at the current scene's objects instead of destroyed ones.

The manager raises OnStateChanged, OnScrapChanged and OnWaveChanged events; the HUD, WaveManager and CommandInvoker subscribe to them instead of polling.

Why:

The game has exactly one source of truth for the phase, wave and scrap, and almost every system needs it: enemies add scrap, turrets spend it, the wave system completes waves, and the HUD displays everything. A Singleton gives all of them one controlled access point and guarantees there can never be two conflicting game states. I chose self-registration and events to limit the usual Singleton drawback of tight coupling: the GameManager never searches for objects or calls the UI directly.

Benefits:

Phases, waves, victory/defeat, respawning and restarting are all coordinated in one place, so the game flow is consistent and easy to debug. Restart works by reloading the scene while the manager resets its own state.

5. Factory – Enemy spawners

How it was implemented in my project:

Abstract product: Enemy (abstract Attack(Health target))

Concrete products: Walker (contact damage), Runner (fast, explodes on contact), Brute (tanky, triple damage to turrets)

Abstract creator: EnemySpawner with abstract Enemy SpawnEnemy(Vector2 position)

Concrete creators; WalkerSpawner, RunnerSpawner, BruteSpawner (each instantiates its own prefab)

Client: WaveManager

- Enemy holds all shared behaviour: moving toward the core, attacking whatever it touches on a timer, rewarding scrap and reporting its removal through OnRemoved.
- WaveManager holds a data-driven list of wave entries (spawner, first wave, base count, extra per wave) set in the Inspector. Each wave it builds a shuffled spawn queue and calls spawner.SpawnEnemy(point) at random edge spawn points. It only knows the abstract EnemySpawner and Enemy types.
- The wave is complete when spawning has finished and every enemy has been removed; the manager then calls GameManager.Instance.CompleteWave().
6. 

