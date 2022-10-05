using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Observer {
    public abstract void OnNotify(int hashID);
}

public class BellObserver : Observer {
    GameObject obserservedGameobject;
    BellEvents bellEvent;

    public BellObserver(GameObject _obserservedGameobject, BellEvents _bellEvent) {
        obserservedGameobject = _obserservedGameobject;
        bellEvent = _bellEvent;
    }

    //What the box will do if the event fits it (will always fit but you will probably change that on your own)
    public override void OnNotify(int hashID) {
        if (hashID == obserservedGameobject.GetHashCode())
            ChangeBellFunction(bellEvent.BellFunction());
    }

    //The box will always change color
    void ChangeBellFunction(BellFunctions newFunction) {
        if(!obserservedGameobject)
            return;

        switch (newFunction) {
            default:
                break;
            case BellFunctions.None:
                obserservedGameobject.GetComponentInChildren<Renderer>().materials[0].color = Color.white;
                obserservedGameobject.GetComponent<Bell>().currentFunction = BellFunctions.None;
                break;
            case BellFunctions.changeMove:
                obserservedGameobject.GetComponentInChildren<Renderer>().materials[0].color = Color.red;
                obserservedGameobject.GetComponent<Bell>().currentFunction = BellFunctions.changeMove;
                break;
        }
    }
}
