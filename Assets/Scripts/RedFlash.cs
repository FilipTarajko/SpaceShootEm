using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlash : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Data data;
    private float targetAlpha;
    private float alpha;

    public void Flash(float targetAlpha)
    {
        this.targetAlpha = targetAlpha;
        StopCoroutine(FlashColor());
        StartCoroutine(FlashColor());
    }

    public IEnumerator FlashColor()
    {
        for (int i = 0; i < 5; i++)
        {
            alpha = System.Math.Min(alpha + targetAlpha/5, targetAlpha);
            spriteRenderer.color = ChangeAlpha(spriteRenderer.color, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 25; i++)
        {
            if (data.isAlive)
            alpha -= targetAlpha/25;
            spriteRenderer.color = ChangeAlpha(spriteRenderer.color, alpha);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private Color ChangeAlpha(Color color, float value)
    {
        return new Color(color.r, color.g, color.b, value);
    }
}
