// ========================================================
// Description: UIEventListener.cs
// Author: Seven 
// Create time: 2020/04/22 20:55:24
// ========================================================
using UnityEngine;
using UnityEngine.EventSystems;
[AutoRegistLua]
public class UIEventListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject obj);

    public delegate void VectorDelegate(PointerEventData data);
    public VoidDelegate onClick;
    public VectorDelegate onDown;
    public VectorDelegate onUp;
	//public VoidDelegate onEnter;
	//public VoidDelegate onExit;
	//public VoidDelegate onSelect;
	//public VoidDelegate onUpdateSelect;
	public VectorDelegate onBeginDrag;
	public VectorDelegate onDrag;
	public VectorDelegate onEndDrag;

	public static UIEventListener Get(GameObject obj)
    {
        UIEventListener listener = obj.GetComponent<UIEventListener>();
        if (listener == null)
        {
            listener = obj.AddComponent<UIEventListener>();
	    	DebugLog.Log("creat UIEventListener");
        }
        return listener;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
		DebugLog.Log("UIEventListener OnPointerClick");
        onClick?.Invoke(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
			DebugLog.Log("UIEventListener OnPointerDown");
        onDown?.Invoke(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
		DebugLog.Log("UIEventListener OnPointerUp");
        onUp?.Invoke(eventData);
    }

	//public override void OnPointerEnter(PointerEventData eventData)
	//{
	//    onEnter?.Invoke(gameObject);
	//}

	//public override void OnPointerExit(PointerEventData eventData)
	//{
	//    onExit?.Invoke(gameObject);
	//}

	//public override void OnSelect(BaseEventData eventData)
	//{
	//    onSelect?.Invoke(gameObject);
	//}

	//public override void OnUpdateSelected(BaseEventData eventData)
	//{
	//    onUpdateSelect?.Invoke(gameObject);
	//}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		onBeginDrag?.Invoke(eventData);
	}

	public override void OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke(eventData);
    }

	public override void OnEndDrag(PointerEventData eventData)
	{
		onEndDrag?.Invoke(eventData);
	}
}
