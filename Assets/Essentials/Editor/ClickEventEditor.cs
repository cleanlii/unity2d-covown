using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ClickEvent))]
public class ClickEventEditor : Editor
{
    private ClickEvent clickEvent;
    private SerializedProperty clickEventTypes;
    private int clickEventTypesCount;

    private SerializedProperty _OnClick;
    private SerializedProperty _OnClickUp;
    private SerializedProperty _OnClickDown;
    private SerializedProperty _OnClickContinue;
    private SerializedProperty _OnClickPress;
    private SerializedProperty _OnDoubleClick;
    private SerializedProperty _OnClickEnter;
    private SerializedProperty _OnClickExit;
    private SerializedProperty[] Events;
    // Use this for initialization
    private void OnEnable()
    {
        clickEvent = (ClickEvent)target;
        clickEventTypes = serializedObject.FindProperty("ClickEventTypes");
        clickEventTypesCount = (int)ClickEvent.ClickEventType.Count;
        Events = new SerializedProperty[clickEventTypesCount];
        Events[(int)ClickEvent.ClickEventType.Click] = _OnClick = serializedObject.FindProperty("_OnClick");
        Events[(int)ClickEvent.ClickEventType.Up] = _OnClickUp = serializedObject.FindProperty("_OnClickUp");
        Events[(int)ClickEvent.ClickEventType.Down] = _OnClickDown = serializedObject.FindProperty("_OnClickDown");
        Events[(int)ClickEvent.ClickEventType.Continue] = _OnClickContinue = serializedObject.FindProperty("_OnClickContinue");
        Events[(int)ClickEvent.ClickEventType.Press] = _OnClickPress = serializedObject.FindProperty("_OnClickPress");
        Events[(int)ClickEvent.ClickEventType.DoubleClick] = _OnDoubleClick = serializedObject.FindProperty("_OnDoubleClick");
        Events[(int)ClickEvent.ClickEventType.Enter] = _OnClickEnter = serializedObject.FindProperty("_OnClickEnter");
        Events[(int)ClickEvent.ClickEventType.Exit] = _OnClickExit = serializedObject.FindProperty("_OnClickExit");


    }


    public override void OnInspectorGUI()
    {

        if (getClickType(ClickEvent.ClickEventType.DoubleClick))
        {
            clickEvent.DoubleClickTime = EditorGUILayout.Slider("双击时间", clickEvent.DoubleClickTime, 0, ClickEvent.maxDoubleClickTime);
        }
        if (getClickType(ClickEvent.ClickEventType.Press))
        {
            clickEvent.PressTime = EditorGUILayout.Slider("按住时间", clickEvent.PressTime, 0, ClickEvent.maxcurrentPressTime);

        }
        //clickEvent.DoubleClickTime = EditorGUILayout.Slider("双击时间", clickEvent.DoubleClickTime, 0, ClickEvent.maxDoubleClickTime);
        if (!EditorGUILayout.PropertyField(clickEventTypes, new GUIContent { text = "所有事件", tooltip = "所有事件设置,最多只有8个不同事件" }, true))
        {
            serializedObject.ApplyModifiedProperties();
        }
        if (clickEvent.ClickEventTypes.Length >= clickEventTypesCount)
        {
            clickEventTypes.arraySize = clickEventTypesCount - 1;
            serializedObject.ApplyModifiedProperties();
        }
        checkClickType();
        setEvents();
    }
    private int index;
    private void setEvents()
    {

        for (int i = 0; i < clickEvent.ClickEventTypes.Length; i++)
        {

            index = (int)clickEvent.ClickEventTypes[i];
            if (index == 0 || index == clickEventTypesCount)
            {
                continue;
            }
            EditorGUILayout.PropertyField(Events[index], true);
        }
        serializedObject.ApplyModifiedProperties();
    }
    private void checkClickType()
    {
        ClickEvent.ClickEventType clickEventType;
        ClickEvent.ClickEventType clickEventTypeTemp;
        for (int i = 0; i < clickEvent.ClickEventTypes.Length; i++)
        {
            clickEventType = clickEvent.ClickEventTypes[i];
            if (clickEventType == ClickEvent.ClickEventType.Count)
            {
                clickEvent.ClickEventTypes[i] = ClickEvent.ClickEventType.None;
            }
            for (int j = i + 1; j < clickEvent.ClickEventTypes.Length; j++)
            {
                clickEventTypeTemp = clickEvent.ClickEventTypes[j];
                if (clickEventTypeTemp == clickEventType)
                {
                    clickEvent.ClickEventTypes[j] = ClickEvent.ClickEventType.None;
                }
            }
        }

    }
    private bool getClickType(ClickEvent.ClickEventType clickEventType)
    {
        return Array.Exists(clickEvent.ClickEventTypes, clickEventTypeItem => clickEventTypeItem == clickEventType);
    }

}