using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameStateTable
{
    private Dictionary<(int timeline, int turn), gameState> boardData = new();
    
    public string saveGameState(gameState currentGameState, int timeline, int turn)
    {
        boardData[(timeline,turn)] = currentGameState;
        return (timeline.ToString() + ":" + turn.ToString());
    }

    public gameState getGameState(int timeline, int turn)
    {
        if (boardData.TryGetValue((timeline, turn), out gameState gameState))
        {
            return gameState;
        }
        else
        {
            return null;
        }
    }
}
