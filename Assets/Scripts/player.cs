using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public Vector2 mousePosLastFrame;
    public bool mousePressedLastFrame;
    public Vector2 mousePos;
    public bool mousePressedThisFrame;
    public float movementFactor;

    void Update()
    {
        LastFrame();
        ThisFrame();
        Movement();
    }

    void Movement()
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
