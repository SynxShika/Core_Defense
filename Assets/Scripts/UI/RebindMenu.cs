using System;
using System.Collections.Generic;
using UnityEngine;

public class RebindMenu : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform rowContainer;
    [SerializeField] private RebindRow rowPrefab;
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    private static readonly KeyCode[] AllKeys = (KeyCode[])Enum.GetValues(typeof(KeyCode));

    private readonly List<RebindRow> _rows = new List<RebindRow>();
    private bool _isListening;
    private GameAction _listeningAction;

    private void OnEnable()
    {
        inputHandler.OnBindingsChanged += RefreshRows;
    }

    private void OnDisable()
    {
        inputHandler.OnBindingsChanged -= RefreshRows;
    }

    private void Start()
    {
        foreach (GameAction action in Enum.GetValues(typeof(GameAction)))
        {
            RebindRow row = Instantiate(rowPrefab, rowContainer);
            row.Setup(action, StartListening);
            _rows.Add(row);
        }

        RefreshRows();
        panel.SetActive(false);
    }

    private void Update()
    {
        if (_isListening)
        {
            ListenForKey();
            return;
        }

        if (Input.GetKeyDown(toggleKey)) Toggle();
    }

    public void Toggle()
    {
        SetOpen(!panel.activeSelf);
    }

    public void SetOpen(bool open)
    {
        GameState state = GameManager.Instance.State;
        if (open && (state == GameState.Victory || state == GameState.Defeat)) return;

        panel.SetActive(open);
        _isListening = false;
        inputHandler.InputEnabled = !open;
        GameManager.Instance.SetPaused(open);
        RefreshRows();
    }

    public void ResetToDefaults()
    {
        _isListening = false;
        inputHandler.ResetToDefaults();
    }

    private void StartListening(GameAction action)
    {
        RefreshRows();
        _isListening = true;
        _listeningAction = action;

        foreach (RebindRow row in _rows)
        {
            if (row.BoundAction == action) row.ShowListening();
        }
    }

    private void ListenForKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isListening = false;
            RefreshRows();
            return;
        }

        foreach (KeyCode key in AllKeys)
        {
            if (key == KeyCode.None || key == toggleKey) continue;
            if (!Input.GetKeyDown(key)) continue;

            _isListening = false;
            inputHandler.Rebind(_listeningAction, key);
            return;
        }
    }

    private void RefreshRows()
    {
        foreach (RebindRow row in _rows)
            row.SetKey(inputHandler.GetBinding(row.BoundAction));
    }
}
