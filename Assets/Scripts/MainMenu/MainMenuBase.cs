using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBase : MonoBehaviour
{
    public MainMenuBrain brain;

    public MainMenuStateEnum selectedState;

    public void Back()
    {
        brain.ChangeMenuState(selectedState);
    }
}
