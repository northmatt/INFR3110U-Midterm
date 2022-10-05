using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BellFunctions {
    None,
    changeShoot,
    changeMove
}

public class Bell : MonoBehaviour {
    public BellFunctions currentFunction = BellFunctions.None;

    private void Start() {
        if (Random.value > 0.5f)
            GameController.instance.subject.AddObserver(new BellObserver(gameObject, new MoveFunction()));
        else
            GameController.instance.subject.AddObserver(new BellObserver(gameObject, new NoFunction()));

        currentFunction = BellFunctions.changeShoot;
    }

    private void OnTriggerEnter(Collider other) {
        switch (currentFunction) {
            default:
            case BellFunctions.None:
                break;
            case BellFunctions.changeShoot:
                CharController.shootFrequency *= 1.5f;
                break;
            case BellFunctions.changeMove:
                //Should have the component reference saved somewhere, prolly GameController
                GameController.instance.player.GetComponent<CharController>().moveForce *= 1.5f;
                break;
        }
    }
}
