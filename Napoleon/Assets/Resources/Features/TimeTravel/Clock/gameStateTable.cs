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
    
    public gameState GetHeadOfTimeline(int timeline)
    {
        IEnumerable<int> turns = boardData.Keys
            .Where(k => k.timeline == timeline)
            .Select(k => k.turn);

        if (!turns.Any())
            return null;

        int maxTurn = turns.Max();
        return boardData[(timeline, maxTurn)];
    }
    
    public int GetTurnOfHead(int timeline)
    {
        IEnumerable<int> turns = boardData.Keys
            .Where(k => k.timeline == timeline)
            .Select(k => k.turn);

        int maxTurn = turns.Max();
        return maxTurn;
    }

    public int getNumberOfTimelines()
    {
        return boardData.Max(x => x.Key.timeline) + 1;
    }
}
