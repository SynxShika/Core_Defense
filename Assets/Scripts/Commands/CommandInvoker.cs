using System;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    [SerializeField] private int maxHistory = 50;
    [SerializeField] private bool logCommands = true;

    private readonly List<IUndoableCommand> _undoHistory = new List<IUndoableCommand>();
    private readonly Stack<IUndoableCommand> _redoStack = new Stack<IUndoableCommand>();

    public int UndoCount => _undoHistory.Count;
    public int RedoCount => _redoStack.Count;

    public event Action OnHistoryChanged;

    private void OnEnable()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.HasInstance)
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
    }

    public bool ExecuteUndoable(IUndoableCommand command)
    {
        command.Execute();
        if (!command.Succeeded) return false;

        _undoHistory.Add(command);
        if (_undoHistory.Count > maxHistory) _undoHistory.RemoveAt(0);
        _redoStack.Clear();

        Log("Execute", command);
        OnHistoryChanged?.Invoke();
        return true;
    }

    public void Undo()
    {
        if (_undoHistory.Count == 0) return;

        int last = _undoHistory.Count - 1;
        IUndoableCommand command = _undoHistory[last];
        _undoHistory.RemoveAt(last);

        command.Undo();
        _redoStack.Push(command);

        Log("Undo", command);
        OnHistoryChanged?.Invoke();
    }

    public void Redo()
    {
        if (_redoStack.Count == 0) return;

        IUndoableCommand command = _redoStack.Pop();
        command.Execute();

        if (command.Succeeded) _undoHistory.Add(command);

        Log(command.Succeeded ? "Redo" : "Redo failed", command);
        OnHistoryChanged?.Invoke();
    }

    public void ClearHistory()
    {
        _undoHistory.Clear();
        _redoStack.Clear();
        OnHistoryChanged?.Invoke();
    }

    private void HandleStateChanged(GameState state)
    {
        if (state != GameState.Build) ClearHistory();
    }

    private void Log(string action, IUndoableCommand command)
    {
        if (logCommands)
            Debug.Log($"[Command] {action}: {command} | Undo: {_undoHistory.Count} Redo: {_redoStack.Count}");
    }
}
