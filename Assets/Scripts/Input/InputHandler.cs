using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    private const string PrefsPrefix = "Binding_";

    [SerializeField] private PlayerController player;
    [SerializeField] private TurretPlacer turretPlacer;
    [SerializeField] private CommandInvoker invoker;

    private static readonly Dictionary<GameAction, KeyCode> DefaultBindings = new Dictionary<GameAction, KeyCode>
    {
        { GameAction.MoveUp, KeyCode.W },
        { GameAction.MoveDown, KeyCode.S },
        { GameAction.MoveLeft, KeyCode.A },
        { GameAction.MoveRight, KeyCode.D },
        { GameAction.Shoot, KeyCode.Mouse0 },
        { GameAction.Dash, KeyCode.Space },
        { GameAction.SelectGun, KeyCode.Alpha1 },
        { GameAction.SelectCannon, KeyCode.Alpha2 },
        { GameAction.PlaceTurret, KeyCode.Mouse1 },
        { GameAction.SellTurret, KeyCode.X },
        { GameAction.CancelSelection, KeyCode.Escape },
        { GameAction.Undo, KeyCode.Z },
        { GameAction.Redo, KeyCode.Y },
        { GameAction.StartWave, KeyCode.Return }
    };

    private readonly Dictionary<GameAction, KeyCode> _bindings = new Dictionary<GameAction, KeyCode>();
    private readonly Dictionary<GameAction, ICommand> _commands = new Dictionary<GameAction, ICommand>();
    private MoveCommand _moveCommand;
    private Camera _cam;

    public bool InputEnabled { get; set; } = true;

    public event Action OnBindingsChanged;

    private void Awake()
    {
        _cam = Camera.main;
        LoadBindings();
        BuildCommands();
    }

    private void BuildCommands()
    {
        _moveCommand = new MoveCommand(player);

        _commands[GameAction.Shoot] = new ShootCommand(player);
        _commands[GameAction.Dash] = new DashCommand(player);
        _commands[GameAction.SelectGun] = new SelectTurretCommand(turretPlacer, 0);
        _commands[GameAction.SelectCannon] = new SelectTurretCommand(turretPlacer, 1);
        _commands[GameAction.PlaceTurret] = new RequestPlaceTurretCommand(turretPlacer, invoker);
        _commands[GameAction.SellTurret] = new RequestSellTurretCommand(turretPlacer, invoker);
        _commands[GameAction.CancelSelection] = new CancelSelectionCommand(turretPlacer);
        _commands[GameAction.Undo] = new UndoCommand(invoker);
        _commands[GameAction.Redo] = new RedoCommand(invoker);
        _commands[GameAction.StartWave] = new StartWaveCommand();
    }

    private void Update()
    {
        if (!InputEnabled || Time.timeScale == 0f)
        {
            _moveCommand.Direction = Vector2.zero;
            _moveCommand.Execute();
            return;
        }

        _moveCommand.Direction = ReadMoveDirection();
        _moveCommand.Execute();

        if (player.isActiveAndEnabled)
            player.AimAt(_cam.ScreenToWorldPoint(Input.mousePosition));

        bool pointerOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

        foreach (KeyValuePair<GameAction, ICommand> pair in _commands)
        {
            KeyCode key = _bindings[pair.Key];
            if (pointerOverUI && IsMouseKey(key)) continue;

            bool triggered = pair.Key == GameAction.Shoot ? Input.GetKey(key) : Input.GetKeyDown(key);
            if (triggered) pair.Value.Execute();
        }
    }

    public KeyCode GetBinding(GameAction action)
    {
        return _bindings[action];
    }

    public void Rebind(GameAction action, KeyCode newKey)
    {
        KeyCode oldKey = _bindings[action];

        foreach (GameAction other in new List<GameAction>(_bindings.Keys))
        {
            if (other != action && _bindings[other] == newKey)
                _bindings[other] = oldKey;
        }

        _bindings[action] = newKey;
        SaveBindings();
        OnBindingsChanged?.Invoke();
    }

    public void ResetToDefaults()
    {
        _bindings.Clear();
        foreach (KeyValuePair<GameAction, KeyCode> pair in DefaultBindings)
            _bindings[pair.Key] = pair.Value;

        SaveBindings();
        OnBindingsChanged?.Invoke();
    }

    private Vector2 ReadMoveDirection()
    {
        Vector2 direction = Vector2.zero;
        if (Input.GetKey(_bindings[GameAction.MoveUp])) direction.y += 1f;
        if (Input.GetKey(_bindings[GameAction.MoveDown])) direction.y -= 1f;
        if (Input.GetKey(_bindings[GameAction.MoveLeft])) direction.x -= 1f;
        if (Input.GetKey(_bindings[GameAction.MoveRight])) direction.x += 1f;
        return direction;
    }

    private void LoadBindings()
    {
        _bindings.Clear();
        foreach (KeyValuePair<GameAction, KeyCode> pair in DefaultBindings)
        {
            int saved = PlayerPrefs.GetInt(PrefsPrefix + pair.Key, (int)pair.Value);
            _bindings[pair.Key] = (KeyCode)saved;
        }
    }

    private void SaveBindings()
    {
        foreach (KeyValuePair<GameAction, KeyCode> pair in _bindings)
            PlayerPrefs.SetInt(PrefsPrefix + pair.Key, (int)pair.Value);

        PlayerPrefs.Save();
    }

    private static bool IsMouseKey(KeyCode key)
    {
        return key >= KeyCode.Mouse0 && key <= KeyCode.Mouse6;
    }
}
