using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-50)]
public class SteeringManager : MonoBehaviour
{
    public enum SteeringBackend
    {
        NativePlugin,
        CSharp
    }

    [SerializeField] private SteeringBackend backend = SteeringBackend.NativePlugin;
    [SerializeField] private float separationRadius = 0.8f;
    [SerializeField] private float separationWeight = 1.5f;
    [SerializeField] private KeyCode toggleBackendKey = KeyCode.F2;
    [SerializeField] private int initialCapacity = 128;

    private Vector2[] _positions;
    private float[] _speeds;
    private Vector2[] _velocities;

    public bool PluginAvailable { get; private set; }
    public SteeringBackend ActiveBackend =>
        backend == SteeringBackend.NativePlugin && PluginAvailable ? SteeringBackend.NativePlugin : SteeringBackend.CSharp;

    public event Action<SteeringBackend> OnBackendChanged;

    private void Awake()
    {
        Allocate(initialCapacity);
        PluginAvailable = TryLoadPlugin();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleBackendKey)) ToggleBackend();
    }

    private void FixedUpdate()
    {
        List<Enemy> enemies = Enemy.Active;
        int count = enemies.Count;
        if (count == 0) return;

        CoreObjective core = GameManager.Instance.Core;
        if (core == null) return;

        if (count > _positions.Length) Allocate(Mathf.NextPowerOfTwo(count));

        for (int i = 0; i < count; i++)
        {
            _positions[i] = enemies[i].Position;
            _speeds[i] = enemies[i].MoveSpeed;
        }

        Vector2 target = core.transform.position;

        if (ActiveBackend == SteeringBackend.NativePlugin)
        {
            SteeringPlugin.SetSeparation(separationRadius, separationWeight);
            SteeringPlugin.ComputeSteering(_positions, _speeds, count, target, _velocities);
        }
        else
        {
            CSharpSteering.Compute(_positions, _speeds, count, target, separationRadius, separationWeight, _velocities);
        }

        for (int i = 0; i < count; i++)
            enemies[i].SetSteeringVelocity(_velocities[i]);
    }

    public void ToggleBackend()
    {
        backend = backend == SteeringBackend.NativePlugin ? SteeringBackend.CSharp : SteeringBackend.NativePlugin;

        if (backend == SteeringBackend.NativePlugin && !PluginAvailable)
            Debug.LogWarning("[Steering] Native plugin not available, staying on C#.");

        Debug.Log($"[Steering] Backend: {ActiveBackend}");
        OnBackendChanged?.Invoke(ActiveBackend);
    }

    private bool TryLoadPlugin()
    {
        try
        {
            int version = SteeringPlugin.GetPluginVersion();
            SteeringPlugin.SetSeparation(separationRadius, separationWeight);
            Debug.Log($"[Steering] Native plugin loaded (version {version}).");
            return true;
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError($"[Steering] SteeringPlugin.dll not found, using C# fallback. {e.Message}");
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogError($"[Steering] Function missing in SteeringPlugin.dll, using C# fallback. {e.Message}");
        }

        return false;
    }

    private void Allocate(int capacity)
    {
        _positions = new Vector2[capacity];
        _speeds = new float[capacity];
        _velocities = new Vector2[capacity];
    }
}
