using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InputLayer : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public UnityAction<PointerEventData> OnBeginDragAction;
    public UnityAction<PointerEventData> OnDragAction;

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragAction.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragAction.Invoke(eventData);
    }
}
