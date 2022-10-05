using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Events
public abstract class BellEvents {
    public abstract BellFunctions BellFunction();
}

public class NoFunction : BellEvents {
    public override BellFunctions BellFunction() {
        return BellFunctions.None;
    }
}

public class MoveFunction : BellEvents {
    public override BellFunctions BellFunction() {
        return BellFunctions.changeMove;
    }
}
