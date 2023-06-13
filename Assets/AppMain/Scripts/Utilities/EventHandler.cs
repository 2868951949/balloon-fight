using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    // 加载场景前需要做的事
    public static event Action BeforeSceneLoadEvent;
    public static void CallBeforeSAceneLoadEvent()
    {
        BeforeSceneLoadEvent?.Invoke();
    }

    // 加载场景后需要做的事
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }

}
