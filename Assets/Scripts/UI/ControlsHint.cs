using TMPro;
using UnityEngine;

public class ControlsHint : MonoBehaviour
{
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private TMP_Text hintText;

    private void OnEnable()
    {
        inputHandler.OnBindingsChanged += Refresh;
    }

    private void OnDisable()
    {
        inputHandler.OnBindingsChanged -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        hintText.text =
            $"{Key(GameAction.SelectGun)} Gun (20)  ·  {Key(GameAction.SelectCannon)} Cannon (40)  ·  " +
            $"{Key(GameAction.PlaceTurret)} Place  ·  {Key(GameAction.SellTurret)} Sell  ·  " +
            $"{Key(GameAction.Undo)} Undo  ·  {Key(GameAction.Redo)} Redo  ·  " +
            $"{Key(GameAction.StartWave)} Start Wave  ·  Tab Controls";
    }

    private string Key(GameAction action)
    {
        return KeyDisplay.Format(inputHandler.GetBinding(action));
    }
}
