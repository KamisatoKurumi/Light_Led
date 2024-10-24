
using System;

public static class EventHandler
{
    public static event Action<DialoguePiece> ShowDialogueEvent;
    public static void CallShowDialogueEvent(DialoguePiece dialoguePiece)
    {
        ShowDialogueEvent?.Invoke(dialoguePiece);
    }

    public static event Action<GameState> UpdateGameStateEvent;
    public static void CallUpdateGameStateEvent(GameState gameState)
    {
        UpdateGameStateEvent?.Invoke(gameState);
    }
}
