using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Build,
    Defend,
    Victory,
    Defeat
}

public class GameManager : Singleton<GameManager>
{
    [Header("Run Settings")]
    [SerializeField] private int totalWaves = 10;
    [SerializeField] private int startingScrap = 50;
    [SerializeField] private float buildPhaseDuration = 20f;
    [SerializeField] private int waveClearBonus = 20;
    [SerializeField] private int bonusPerWave = 5;
    [SerializeField] private float playerRespawnDelay = 3f;

    public GameState State { get; private set; }
    public int Wave { get; private set; }
    public int TotalWaves => totalWaves;
    public int Scrap { get; private set; }
    public float BuildTimeRemaining { get; private set; }
    public bool IsPaused { get; private set; }
    public CoreObjective Core { get; private set; }
    public PlayerController Player { get; private set; }

    public event Action<GameState> OnStateChanged;
    public event Action<int> OnScrapChanged;
    public event Action<int, int> OnWaveChanged;

    private Vector3 _playerSpawnPoint;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this) return;
        ResetRun();
    }

    private void Update()
    {
        if (State != GameState.Build) return;

        BuildTimeRemaining -= Time.deltaTime;
        if (BuildTimeRemaining <= 0f) SetState(GameState.Defend);
    }

    public void RegisterCore(CoreObjective core)
    {
        Core = core;
        core.Health.OnDied += HandleCoreDestroyed;
    }

    public void RegisterPlayer(PlayerController player)
    {
        Player = player;
        _playerSpawnPoint = player.transform.position;
        player.Health.OnDied += HandlePlayerDied;
    }

    public void AddScrap(int amount)
    {
        if (amount <= 0) return;
        Scrap += amount;
        OnScrapChanged?.Invoke(Scrap);
    }

    public bool TrySpendScrap(int amount)
    {
        if (amount < 0 || amount > Scrap) return false;
        Scrap -= amount;
        OnScrapChanged?.Invoke(Scrap);
        return true;
    }

    public void SetPaused(bool paused)
    {
        if (State == GameState.Victory || State == GameState.Defeat) return;

        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    public void SkipBuildPhase()
    {
        if (State == GameState.Build) SetState(GameState.Defend);
    }

    public void CompleteWave()
    {
        if (State != GameState.Defend) return;

        AddScrap(waveClearBonus + bonusPerWave * Wave);

        if (Wave >= totalWaves)
        {
            SetState(GameState.Victory);
            return;
        }

        Wave++;
        OnWaveChanged?.Invoke(Wave, totalWaves);
        SetState(GameState.Build);
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        ResetRun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetRun()
    {
        Wave = 1;
        Scrap = startingScrap;
        OnWaveChanged?.Invoke(Wave, totalWaves);
        OnScrapChanged?.Invoke(Scrap);
        SetState(GameState.Build);
    }

    private void SetState(GameState newState)
    {
        State = newState;
        IsPaused = false;

        if (newState == GameState.Build)
            BuildTimeRemaining = buildPhaseDuration;

        bool gameOver = newState == GameState.Victory || newState == GameState.Defeat;
        Time.timeScale = gameOver ? 0f : 1f;

        OnStateChanged?.Invoke(newState);
    }

    private void HandleCoreDestroyed()
    {
        if (State == GameState.Victory || State == GameState.Defeat) return;
        SetState(GameState.Defeat);
    }

    private void HandlePlayerDied()
    {
        StartCoroutine(RespawnPlayer(Player));
    }

    private IEnumerator RespawnPlayer(PlayerController player)
    {
        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(playerRespawnDelay);

        if (player == null) yield break;

        player.transform.position = _playerSpawnPoint;
        player.gameObject.SetActive(true);
        player.Health.Revive();
    }
}
