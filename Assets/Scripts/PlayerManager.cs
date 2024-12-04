using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public HealthSystem healthSystem = new HealthSystem();
    
    public Tilemap baseTilemap;      // The base tilemap
    public Tilemap topLayerTilemap; // The tilemap with obstacles
    public float moveDelay = 0.5f;  // Delay between moves

    private bool isMoving = false;  // Prevent input during movement
    // GamestateManager.GameState currentState;

    void Update() {
        
        Vector3Int moveDirection = Vector3Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = new Vector3Int(0, 1, 0);   // Up
        if (Input.GetKeyDown(KeyCode.S)) moveDirection = new Vector3Int(0, -1, 0);  // Down
        if (Input.GetKeyDown(KeyCode.A)) moveDirection = new Vector3Int(-1, 0, 0);  // Left
        if (Input.GetKeyDown(KeyCode.D)) moveDirection = new Vector3Int(1, 0, 0);   // Right

        if (moveDirection != Vector3Int.zero) {
            StartCoroutine(TryMove(moveDirection));
        }
    }

    private IEnumerator TryMove(Vector3Int direction) {
        isMoving = true;

        // Get current and target positions
        Vector3Int currentTile = baseTilemap.WorldToCell(transform.position);
        Vector3Int targetTile = currentTile + direction;

        // Check if target tile is walkable
        if (IsTileWalkable(targetTile - new Vector3Int(0, 1, 0))) {
            // Move character
            Vector3 targetPosition = baseTilemap.GetCellCenterWorld(targetTile - new Vector3Int(0, 1, 0));
            transform.position = targetPosition - new Vector3Int(0, 0, 1);

            // Switch to enemy turn or other game logic
            yield return new WaitForSeconds(moveDelay);
            // currentState = GamestateManager.GameState.EnemyTurn;
        }

        isMoving = false;
    }

    private bool IsTileWalkable(Vector3Int tilePosition) {
        // Ensure the tile exists in the base tilemap and is not occupied in the top layer
        return baseTilemap.HasTile(tilePosition) && !topLayerTilemap.HasTile(tilePosition);
    }
}
