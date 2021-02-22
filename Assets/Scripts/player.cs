using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [SerializeField] InputLayer inputLayer;
    Vector3 previousCursorPosition;
    Vector3 cursorDelta;
    public float swipeFactor;
    public bool useSwipeMovement;
    public float toplimit;
    public float sidelimit;
    public float bottomlimit;

    private void Start()
    {
        inputLayer.OnBeginDragAction += StartOffsetMovement;
        inputLayer.OnDragAction += DoOffsetMovement;
        inputLayer.OnDragAction += MouseFollowMovement;
        //inputLayer.OnDragAction += CheckLimits;
    }

    private void Update()
    {
        CheckLimits(null);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{this.name} collided with {other.name}");
    }

    void CheckLimits(PointerEventData eventData)
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = System.Math.Max(-sidelimit, targetPosition.x);
        targetPosition.x = System.Math.Min(sidelimit, targetPosition.x);
        targetPosition.y = System.Math.Min(toplimit, targetPosition.y);
        targetPosition.y = System.Math.Max(-bottomlimit, targetPosition.y);
        transform.position = targetPosition;
    }

    void MouseFollowMovement(PointerEventData eventData)
    {
        if (!useSwipeMovement)
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            transform.position = targetPos;
        }
    }

    void StartOffsetMovement(PointerEventData eventData)
    {
        if (useSwipeMovement)
        {
            previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        }
    }

    void DoOffsetMovement(PointerEventData eventData)
    {
        if (useSwipeMovement)
        {
            cursorDelta = previousCursorPosition - Camera.main.ScreenToWorldPoint(eventData.position);
            previousCursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.Translate(-cursorDelta * swipeFactor);
        }
    }
}
