using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour {
    //Given my coding, better to have shootFreq in GameController or detect TriggerEnter on player
    //Coded in an weird manner for complete showcase of Singleton
    //Also Singleton in GameController
    private void OnTriggerEnter(Collider other) {
        CharController.shootFrequency *= 1.5f;
    }
}
