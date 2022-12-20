using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DPadMovement : MonoBehaviour
{
    public RectTransform crossTransform;
    public Image[] crossHandles;
    public Bounds crossBounds;

    public Color normalColor = Color.white;
    public Color clickedColor = Color.yellow;

    public float pixelBuffer = 20.0f;

    public void OnEnable()
    {
        crossBounds = new Bounds(Camera.main.WorldToScreenPoint(crossTransform.position), 
                                 new Vector3((Camera.main.WorldToScreenPoint(crossHandles[0].rectTransform.position).x - Camera.main.WorldToScreenPoint(crossHandles[1].rectTransform.position).x) + 10.0f, 
                                             (Camera.main.WorldToScreenPoint(crossHandles[2].rectTransform.position).y - Camera.main.WorldToScreenPoint(crossHandles[3].rectTransform.position).y) + 10.0f, 0f));
        crossBounds.center = new Vector3(crossBounds.center.x, crossBounds.center.y);
    }

    public void Update()
    {
        for (int i = 0; i < crossHandles.Length; i++)
        {
            crossHandles[i].color = normalColor;
        }

        if (Input.GetMouseButton(0))
        {
            if (crossBounds.Contains(Input.mousePosition))
            {
                if (Input.mousePosition.x > crossBounds.center.x + pixelBuffer)
                {
                    PlayerMovement.instance.movement.x = 1.0f;
                    crossHandles[0].color = clickedColor;
                }
                else if (Input.mousePosition.x < crossBounds.center.x - pixelBuffer)
                {
                    PlayerMovement.instance.movement.x = -1.0f;
                    crossHandles[1].color = clickedColor;
                }
                else
                {
                    PlayerMovement.instance.movement.x = 0.0f;
                }

                if (Input.mousePosition.y > crossBounds.center.y + pixelBuffer)
                {
                    PlayerMovement.instance.movement.y = 1.0f;
                    crossHandles[2].color = clickedColor;
                }
                else if (Input.mousePosition.y < crossBounds.center.y - pixelBuffer)
                {
                    PlayerMovement.instance.movement.y = -1.0f;
                    crossHandles[3].color = clickedColor;
                }
                else
                {
                    PlayerMovement.instance.movement.y = 0.0f;
                }
                PlayerMovement.instance.OverrideMovement();
            }
        }
    }
}
