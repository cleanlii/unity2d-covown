using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickEvent : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum ClickEventType
    {
        None,
        Up,
        Click,
        DoubleClick,
        Down,
        Press,
        Continue,
        Exit,
        Enter,

        [Tooltip("只用于枚举个数")]
        Count,
    }
    [Serializable]
    public class ClickEventClass : UnityEvent
    {
        public ClickEventClass()
        { }
    }

    [Range(0, maxDoubleClickTime)]
    public float DoubleClickTime = 0.5f;
    [Range(0, maxcurrentPressTime)]
    public float PressTime = 0.5f;
    public ClickEventType[] ClickEventTypes = new ClickEventType[1];

    [SerializeField]
    protected ClickEventClass _OnClick;
    public ClickEventClass OnClick
    {
        get
        {
            return _OnClick;
        }
    }

    [SerializeField]
    protected ClickEventClass _OnClickDown;
    public ClickEventClass OnClickDown
    {
        get
        {
            return _OnClickDown;
        }
    }

    [SerializeField]
    protected ClickEventClass _OnClickUp;
    public ClickEventClass OnClickUp
    {
        get
        {
            return _OnClickUp;
        }
    }
    [SerializeField]
    protected ClickEventClass _OnDoubleClick;
    public ClickEventClass OnDoubleClick
    {
        get
        {
            return _OnDoubleClick;
        }
    }
    [SerializeField]
    protected ClickEventClass _OnClickPress;
    public ClickEventClass OnClickPress
    {
        get
        {
            return _OnClickPress;
        }
    }
    [SerializeField]
    protected ClickEventClass _OnClickContinue;
    public ClickEventClass OnClickContinue
    {
        get
        {
            return _OnClickContinue;
        }
    }
    [SerializeField]
    protected ClickEventClass _OnClickEnter;
    public ClickEventClass OnClickEnter
    {
        get
        {
            return _OnClickEnter;
        }
    }
    [SerializeField]
    protected ClickEventClass _OnClickExit;
    public ClickEventClass OnClickExit
    {
        get
        {
            return _OnClickExit;
        }
    }


    public const float maxDoubleClickTime = 10;
    public const float maxcurrentPressTime = 1000;
    private float currentDoubleClickTime = -maxDoubleClickTime;  //初始值为相反数，防止差值小于DoubleClickTime
    private float currentPressTime = maxcurrentPressTime * 10;    //初始值设置要比最大值大，防止第一次差值大于PressTime

    public object Handle
    {
        get; set;
    }

    ClickEventType currentClickEventType;
    PointerEventData currentPointerEventData;
    #region 事件接口类实现
    public void OnPointerClick(PointerEventData eventData)
    {
        OnEvent(ClickEventType.Click, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentPointerEventData = eventData;
        OnEvent(ClickEventType.Down, eventData);
        currentClickEventType = ClickEventType.Down;

        currentPressTime = Time.realtimeSinceStartup;

        if (Time.realtimeSinceStartup - currentDoubleClickTime < DoubleClickTime)
        {
            OnEvent(ClickEventType.DoubleClick, eventData);
            currentDoubleClickTime = -maxDoubleClickTime;
        }
        else
        {
            currentDoubleClickTime = Time.realtimeSinceStartup;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        currentClickEventType = ClickEventType.Up;
        OnEvent(ClickEventType.Up, eventData);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEvent(ClickEventType.Enter, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnEvent(ClickEventType.Exit, eventData);
    }
    #endregion 事件接口类实现

    private void OnEvent(ClickEventType clickEventType, PointerEventData eventData)
    {
        if (!getClickType(clickEventType))
        {
            return;
        }
        switch (clickEventType)
        {
            case ClickEventType.Click:
                {
                    _OnClick.Invoke();
                    break;
                }
            case ClickEventType.Up:
                {
                    _OnClickUp.Invoke();
                    break;
                }
            case ClickEventType.Down:
                {
                    _OnClickDown.Invoke();
                    break;
                }
            case ClickEventType.DoubleClick:
                {
                    //Debug.LogError("Continue");
                    _OnDoubleClick.Invoke();
                    break;
                }
            case ClickEventType.Continue:
                {
                    //Debug.LogError("Continue");
                    _OnClickContinue.Invoke();
                    break;
                }
            case ClickEventType.Press:
                {
                    Debug.LogError("Press");
                    _OnClickPress.Invoke();
                    break;
                }
            case ClickEventType.Enter:
                {
                    _OnClickEnter.Invoke();
                    break;
                }
            case ClickEventType.Exit:
                {
                    Debug.LogError("Exit");
                    _OnClickExit.Invoke();
                    break;
                }
        }
    }
    private bool getClickType(ClickEventType clickEventType)
    {
        return Array.Exists(ClickEventTypes, clickEventTypeItem => clickEventTypeItem == clickEventType);
    }

    void Update()
    {
        if (currentClickEventType == ClickEventType.Down)
        {
            OnEvent(ClickEventType.Continue, currentPointerEventData);
            if (Time.realtimeSinceStartup - currentPressTime > PressTime)
            {
                OnEvent(ClickEventType.Press, currentPointerEventData);
                currentPressTime = maxcurrentPressTime * 10;
            }

        }
    }
}

[Serializable]
public class ClickEventData
{
    public enum ClickEventType
    {
        None,
        Up,
        Click,
        DoubleClick,
        Down,
        Press,
        Continue,
        Exit,
        Enter,
    }
    [Serializable]
    public class ClickEventClass : UnityEvent
    {
        public ClickEventClass()
        { }
    }
    public ClickEventType clickEventType;

    public ClickEventClass Event;
}