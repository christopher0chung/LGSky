using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_MenuScreen : MonoBehaviour
{
    public MenuMode currentPlayMode = MenuMode.None;
}

public enum MenuMode { SingleFam, DoubleFam, SingleGame, DoubleGame, Controls, None}
