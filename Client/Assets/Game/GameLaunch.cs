using UnityEngine;
using System.Collections;
using UISystem;
public class GameLaunch : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CfgSvc.Instance.Init();
        UISystemFacade.Inst.Init();
        //UISystemFacade.Inst.OpenWindow(1);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UISystemFacade.Inst.OpenWindow(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            UISystemFacade.Inst.CloseOpenWindowByTypeId(1);
        }
        if ( Input.GetKeyDown(KeyCode.W))
        {
            UISystemFacade.Inst.OpenWindow(2);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            UISystemFacade.Inst.CloseOpenWindowByTypeId(2);
        }
        if ( Input.GetKeyDown(KeyCode.E))
        {
            UISystemFacade.Inst.OpenWindow(3);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            UISystemFacade.Inst.CloseOpenWindowByTypeId(3);
        }
        if ( Input.GetKeyDown(KeyCode.R))
        {
            UISystemFacade.Inst.OpenWindow(4);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            UISystemFacade.Inst.CloseOpenWindowByTypeId(4);
        }
        if (  Input.GetKeyDown(KeyCode.T))
        {
            UISystemFacade.Inst.OpenWindow(5);
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            UISystemFacade.Inst.CloseOpenWindowByTypeId(5);
        }
    }
}
