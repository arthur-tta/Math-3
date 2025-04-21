using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    //dict for quick query UI prefab
    //dict dung de lu thong tin prefab canvas truy cap cho nhanh
    private Dictionary<System.Type, UICanvas> uiCanvasPrefab = new Dictionary<System.Type, UICanvas>();

    //list from resource
    //list load ui resource
    private UICanvas[] uiResources;

    //dict for UI active
    //dict luu cac ui dang dung
    private Dictionary<System.Type, UICanvas> uiCanvas = new Dictionary<System.Type, UICanvas>();

    //canvas container, it should be a canvas - root
    //canvas chua dung cac canvas con, nen la mot canvas - root de chua cac canvas nay
    public Transform CanvasParentTF;
    //public GameObject targetIndicatior;

    #region Canvas

    //open UI
    //mo UI canvas
    public T OpenUI<T>() where T : UICanvas
    {
        UICanvas canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open();

        return canvas as T;
    }

    //close UI directly
    //dong UI canvas ngay lap tuc
    public void CloseUI<T>() where T : UICanvas
    {
        if (IsOpened<T>())
        {
            GetUI<T>().CloseDirectly();
        }
    }

    //close UI with delay time
    //dong ui canvas sau delay time
    public void CloseUI<T>(float delayTime) where T : UICanvas
    {
        if (IsOpened<T>())
        {
            GetUI<T>().Close(delayTime);
        }
    }

    //check UI is Opened
    //kiem tra UI dang duoc mo len hay khong
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && uiCanvas[typeof(T)].gameObject.activeInHierarchy;
    }

    //check UI is loaded
    //kiem tra UI da duoc khoi tao hay chua
    public bool IsLoaded<T>() where T : UICanvas
    {
        System.Type type = typeof(T);
        return uiCanvas.ContainsKey(type) && uiCanvas[type] != null;
    }

    //Get component UI 
    //lay component cua UI hien tai
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            UICanvas canvas = Instantiate(GetUIPrefab<T>(), CanvasParentTF);
            uiCanvas[typeof(T)] = canvas;
        }

        return uiCanvas[typeof(T)] as T;
    }

    //Close all UI
    //dong tat ca UI ngay lap tuc -> tranh truong hop dang mo UI nao dong ma bi chen 2 UI cung mot luc
    public void CloseAll()
    {
        foreach (var item in uiCanvas)
        {
            if (item.Value != null && item.Value.gameObject.activeInHierarchy)
            {
                item.Value.CloseDirectly();
            }
        }
    }

    //Get prefab from resource
    //lay prefab tu Resources/UI 
    private T GetUIPrefab<T>() where T : UICanvas
    {
        if (CanvasParentTF == null)
        {
            Debug.LogError("CanvasParentTF is not assigned!");
            return null;
        }

        if (!uiCanvasPrefab.ContainsKey(typeof(T)))
        {
            if (uiResources == null)
            {
                Debug.Log("Loading UI resources...");
                uiResources = Resources.LoadAll<UICanvas>("UI");
                if (uiResources.Length == 0)
                {
                    Debug.LogError("No UI resources found in Resources/UI");
                    return null;
                }
            }

            foreach (var canvas in uiResources)
            {
                if (canvas is T)
                {
                    Debug.Log($"Found UI prefab: {canvas.name}");
                    uiCanvasPrefab[typeof(T)] = canvas;
                    break;
                }
            }
        }

        if (!uiCanvasPrefab.ContainsKey(typeof(T)))
        {
            //Debug.LogError($"UI prefab for {typeof(T).Name} not found!");
        }

        return uiCanvasPrefab[typeof(T)] as T;
    }
    /*private void Start()
    {
        UIManager.Ins.OpenUI<MianMenu>();
    }*/

    #endregion

    #region Back Button

    private Dictionary<UICanvas, UnityAction> BackActionEvents = new Dictionary<UICanvas, UnityAction>();
    private List<UICanvas> backCanvas = new List<UICanvas>();
    UICanvas BackTopUI
    {
        get
        {
            UICanvas canvas = null;
            if (backCanvas.Count > 0)
            {
                canvas = backCanvas[backCanvas.Count - 1];
            }

            return canvas;
        }
    }


    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Escape) && BackTopUI != null)
        {
            BackActionEvents[BackTopUI]?.Invoke();
        }
    }

    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        if (!BackActionEvents.ContainsKey(canvas))
        {
            BackActionEvents.Add(canvas, action);
        }
    }

    public void AddBackUI(UICanvas canvas)
    {
        if (!backCanvas.Contains(canvas))
        {
            backCanvas.Add(canvas);
        }
    }

    public void RemoveBackUI(UICanvas canvas)
    {
        backCanvas.Remove(canvas);
    }

    /// <summary>
    /// CLear backey when comeback index UI canvas
    /// </summary>
    public void ClearBackKey()
    {
        backCanvas.Clear();
    }

    #endregion
}

