using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class TetrisEvents {

    public static UnityEvent e_PieceLanded = new UnityEvent();
    public static UnityEvent e_GameOver = new UnityEvent();

} // ~TetrisEvents
