using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text scrapText;
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private GameObject startWaveButton;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TMP_Text endText;

    private void OnEnable()
    {
        GameManager gm = GameManager.Instance;
        gm.OnWaveChanged += UpdateWave;
        gm.OnScrapChanged += UpdateScrap;
        gm.OnStateChanged += UpdateState;
    }

    private void OnDisable()
    {
        if (!GameManager.HasInstance) return;

        GameManager gm = GameManager.Instance;
        gm.OnWaveChanged -= UpdateWave;
        gm.OnScrapChanged -= UpdateScrap;
        gm.OnStateChanged -= UpdateState;
    }

    private void Start()
    {
        GameManager gm = GameManager.Instance;
        UpdateWave(gm.Wave, gm.TotalWaves);
        UpdateScrap(gm.Scrap);
        UpdateState(gm.State);
    }

    private void Update()
    {
        GameManager gm = GameManager.Instance;
        if (gm.State == GameState.Build)
            phaseText.text = $"BUILD PHASE  {Mathf.CeilToInt(gm.BuildTimeRemaining)}s";
    }

    private void UpdateWave(int wave, int total)
    {
        waveText.text = $"Wave {wave}/{total}";
    }

    private void UpdateScrap(int scrap)
    {
        scrapText.text = $"Scrap: {scrap}";
    }

    private void UpdateState(GameState state)
    {
        startWaveButton.SetActive(state == GameState.Build);

        bool gameOver = state == GameState.Victory || state == GameState.Defeat;
        endPanel.SetActive(gameOver);

        if (state == GameState.Defend) phaseText.text = "DEFEND THE CORE!";
        if (state == GameState.Victory) endText.text = "VICTORY\nThe core survived!";
        if (state == GameState.Defeat) endText.text = "DEFEAT\nThe core was destroyed";
    }

    public void OnStartWaveClicked()
    {
        GameManager.Instance.SkipBuildPhase();
    }

    public void OnRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }
}
