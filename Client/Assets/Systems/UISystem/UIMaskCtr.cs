using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

/// <summary>
/// 界面遮罩管理
/// </summary>
public class UIMaskCtrl : MonoBehaviour {
    
    /**遮罩的图片*/
    [SerializeField]
    Image imgMask;

    /*按钮*/
    [SerializeField]
	GameObject goButton;
	Button btn = null;

    [SerializeField]
    Animator animator;

    //淡出结束回调
    Action FadeOutEndAction;

	void Awake() {
		if (goButton != null)
			btn = goButton.GetComponent<Button>();
	}

    void Start()
    {
        this.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height) * 100;
        this.transform.localPosition = Vector3.zero;
    } 

    public void SetColor( Color vColor )
    {
        this.imgMask.color = vColor;
    }

    /// <summary>
    /// 添加点击事件处理
    /// </summary>
    /// <param name="vAction"></param>
    public void AddListener(UnityAction vAction)
    {
        this.btn.onClick.AddListener(vAction); 
    }

    public void FadeIn()
    {
        //animator.SetBool("Fade", true);
    }

    public void FadeOut(Action vAction = null)
    {
        this.FadeOutEndAction = vAction;
        //animator.SetBool("Fade", false);
    }

    //淡出结束
    public void FadeOutEnd()
    {
        if (null != this.FadeOutEndAction)
        {
            this.FadeOutEndAction();
        }
    }
}
