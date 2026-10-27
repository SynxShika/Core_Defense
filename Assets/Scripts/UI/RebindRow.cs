using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindRow : MonoBehaviour
{
    [SerializeField] private TMP_Text actionLabel;
    [SerializeField] private TMP_Text keyLabel;
    [SerializeField] private Button rebindButton;

    public GameAction BoundAction { get; private set; }

    public void Setup(GameAction action, Action<GameAction> onClicked)
    {
        BoundAction = action;
        actionLabel.text = KeyDisplay.FormatAction(action);
        rebindButton.onClick.AddListener(() => onClicked(BoundAction));
    }

    public void SetKey(KeyCode key)
    {
        keyLabel.text = KeyDisplay.Format(key);
    }

    public void ShowListening()
    {
        keyLabel.text = "Press a key...";
    }
}
