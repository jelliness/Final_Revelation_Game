using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class HideUserInput : MonoBehaviour
{
    public GameObject UserInput;
    public void HideInput()
    {
        UserInput = GameObject.FindWithTag("UserInput");

        UserInput.GetComponent<Canvas>().enabled = false;
    }
}
