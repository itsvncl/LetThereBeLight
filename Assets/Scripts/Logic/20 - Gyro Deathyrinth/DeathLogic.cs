using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    [SerializeField] private DeathyrinthGame game;

    private void Start() {
        game = GameObject.Find("GameController").GetComponent<DeathyrinthGame>();
    }

    void OnTriggerExit2D(Collider2D other) {        
        Collider2D[] hits = Physics2D.OverlapPointAll(this.gameObject.transform.position);
            
        if(hits.Length < 2) {
            game.ResetGame();
        }
    }
}
