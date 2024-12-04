using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public HealthSystem healthSystem;
    
    // public Tilemap overworldGroundMap;// The tilemap with obstacles
    // public Tilemap overworldTopMap;// The tilemap with obstacles
    public Tilemap dungeonGroundMap;// The tilemap with obstacles
    public Tilemap dungeonTopMap;// The tilemap with obstacles

    public Tilemap currentGroundMap = new Tilemap();    // The base tilemap
    public Tilemap currentTopMap = new Tilemap();

    public Tilemap CurrentGroundMap => currentGroundMap;

    public TileBase dungeonDoor1;
    public TileBase dungeonDoor2;
    public float moveDelay = 0.5f;  // Delay between moves
    

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
        
        // Get current and target positions
        Vector3Int currentTile = currentGroundMap.WorldToCell(transform.position);
        Vector3Int targetTile = currentTile + direction;

        // Check if target tile is walkable
        if (IsTileWalkable(targetTile - new Vector3Int(0, 1, 0))) {
            
            // Move character
            Vector3 targetPosition = currentGroundMap.GetCellCenterWorld(targetTile - new Vector3Int(0, 1, 0));
            transform.position = targetPosition - new Vector3Int(0, 0, 1);

            // change to dungeon map
            TileBase newTile = currentGroundMap.GetTile(currentGroundMap.WorldToCell(transform.position)- new Vector3Int(0, 1, 0));
            if (newTile == dungeonDoor1 || newTile == dungeonDoor2) {
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                
                currentGroundMap.gameObject.SetActive(false);
                currentTopMap.gameObject.SetActive(false);
                dungeonGroundMap.gameObject.SetActive(true);
                dungeonTopMap.gameObject.SetActive(true);
                
                currentGroundMap = dungeonGroundMap;
                currentTopMap = dungeonTopMap;
            }
            
            
            
            yield return new WaitForSeconds(moveDelay);
        }
    }

    private bool IsTileWalkable(Vector3Int tilePosition) {
        // Ensure the tile exists in the base tilemap and is not occupied in the top layer
        return currentGroundMap.HasTile(tilePosition) && !currentTopMap.HasTile(tilePosition);
    }
}
