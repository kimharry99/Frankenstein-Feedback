using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public int i = 0;
    public GameObject panel;
    public void OnClick()
    {
        panel.SetActive(true);
    }
}
