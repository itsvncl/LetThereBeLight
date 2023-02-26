using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLogic : MonoBehaviour
{
    [SerializeField] private DeathyrinthController controller;

    private void Start() {
        controller = GameObject.Find("GameController").GetComponent<DeathyrinthController>();
    }

    void OnTriggerExit2D(Collider2D other) {        
        Collider2D[] hits = Physics2D.OverlapPointAll(this.gameObject.transform.position);
            
        if(hits.Length < 2) {
            controller.ResetGame();
        }
    }
}
