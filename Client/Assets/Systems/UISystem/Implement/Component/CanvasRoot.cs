using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CanvasRoot:MonoBehaviour
{
    [SerializeField]
   public Canvas FirstWindowCanvas;

    [SerializeField]
    public Canvas SecondWindowCanvas;

    [SerializeField]
    public Canvas TopWindowCanvas;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}