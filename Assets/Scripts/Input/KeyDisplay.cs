using System.Text.RegularExpressions;
using UnityEngine;

public static class KeyDisplay
{
    public static string Format(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Mouse0: return "Left Click";
            case KeyCode.Mouse1: return "Right Click";
            case KeyCode.Mouse2: return "Middle Click";
            case KeyCode.Return: return "Enter";
            case KeyCode.Escape: return "Esc";
        }

        string name = key.ToString();
        if (name.StartsWith("Alpha")) return name.Substring(5);
        if (name.StartsWith("Keypad")) return "Num " + name.Substring(6);
        return name;
    }

    public static string FormatAction(GameAction action)
    {
        return Regex.Replace(action.ToString(), "(\\B[A-Z])", " $1");
    }
}
