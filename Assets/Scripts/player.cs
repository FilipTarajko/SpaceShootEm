using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 mousePosLastFrame;
    bool mousePressedLastFrame;
    Vector2 mousePos;
    bool mousePressedThisFrame;
    public float movementFactor;
    public bool useSwipeMovement;
    public Camera gameCamera;

    void Update()
    {
        if (useSwipeMovement)
        {
            LastFrame();
            ThisFrame();
            SwipeMovement();
        }
        else
        {
            FollowMovement();
        }
    }

    void FollowMovement()
    {
        mousePos = Input.mousePosition;
        Vector3 worldPos = gameCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        transform.position = worldPos;
    }

    void SwipeMovement()
    {
        if (mousePressedThisFrame && mousePressedLastFrame)
        {
            float xDiff = (mousePos.x - mousePosLastFrame.x)/1000*movementFactor;
            float yDiff = (mousePos.y - mousePosLastFrame.y)/1000*movementFactor;
            transform.Translate(xDiff, yDiff, 0);
        }
    }

    void LastFrame()
    {
        mousePressedLastFrame = mousePressedThisFrame;
        mousePosLastFrame = mousePos;
    }

    void ThisFrame()
    {
        mousePressedThisFrame = Input.GetMouseButton(0);
        mousePos = Input.mousePosition;
    }
}
