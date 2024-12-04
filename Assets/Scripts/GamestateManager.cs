using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamestateManager : MonoBehaviour
{
    public enum GameState
    {
        PlayerTurn,
        // EnemyTurn,
        Waiting
    };


    GameState currentState = GameState.PlayerTurn;

    void Update() {
        switch (currentState) {
            case GameState.PlayerTurn:
                // Handle player input (covered above)
                break;
            // case GameState.EnemyTurn:
            //     // Handle enemy logic
            //     StartCoroutine(EnemyTurnRoutine());
            //     break;
            case GameState.Waiting:
                // Idle or handle animations
                break;
        }
    }

    IEnumerator EnemyTurnRoutine() {
        // Simulate enemy logic
        yield return new WaitForSeconds(1f);

        // Return to player turn
        currentState = GameState.PlayerTurn;
    }
}
