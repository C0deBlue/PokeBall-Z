using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GroundObject : MonoBehaviour
{
    public ScriptableInventoryObject attachedObject;
    public SpriteRenderer objectSprite;
    public float pickupDistance = 5.0f;
    public float pickupClickDistance = 0.1f;
    public AnimationCurve scaleModCurve;

    ScriptableInventoryObject lastObject = null;
    float startTime = 0.0f;
    Vector3 normalScale;

    void OnEnable()
    {
        if (Application.isPlaying)
        {
            normalScale = transform.localScale;
        }
    }

    public void Update()
    {
        if (attachedObject != lastObject)
        {
            objectSprite.sprite = attachedObject == null ? null : attachedObject.GetInventorySprite();
            name = attachedObject == null ? "GroundObject_NULL" : "GroundObject_" + attachedObject.GetShortName();
            lastObject = attachedObject;
        }

        if (Application.isPlaying)
        {
            if (Vector3.Distance(transform.position, PlayerMovement.playerTransform.position) < pickupDistance)
            {
                if (startTime < 0.0f)
                {
                    startTime = Time.time;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (Vector3.Distance(Camera.main.WorldToScreenPoint(transform.position), Input.mousePosition) < pickupClickDistance)
                    {
                        PickUp();
                    }
                }
                Highlight();
            }
            else
            {
                startTime = -1f;
                ReturnToNormal();
            }
        }
    }

    void PickUp()
    {
        InventoryManager.instance.PickUpObject(attachedObject);
        gameObject.SetActive(false);
    }

    void Highlight()
    {
        transform.localScale = normalScale * scaleModCurve.Evaluate((Time.time - startTime) % scaleModCurve.keys[scaleModCurve.keys.Length - 1].time);
    }

    void ReturnToNormal()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, normalScale, Time.deltaTime * 4.0f);
    }
}
