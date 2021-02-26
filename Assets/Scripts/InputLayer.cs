using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InputLayer : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerDownHandler
{
    public UnityAction<PointerEventData> OnBeginDragAction;
    public UnityAction<PointerEventData> OnDragAction;
    public UnityAction<PointerEventData> OnPointerDownAction;

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragAction?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragAction?.Invoke(eventData);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownAction?.Invoke(eventData);

    }
}
