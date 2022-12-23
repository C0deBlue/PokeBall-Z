using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3f;
    private float zoomLerpSpeed = 10;
    private float yVelocity = 0.0f;

    [Header("Cam Clamp")]
    public Transform[] camBoundsObjects;
    public Bounds orthoBounds;
    public Bounds generatedBounds;
    Vector3 tempVec;

    [Header("Cam Movement")]
    public Transform player;
    public Transform camTransform;
    public float playerDist = 5.0f;

    public static CameraMovementController instance;
   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
        GenerateWorldBounds();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0.05f)
        {
            float scrollData;
            scrollData = Input.GetAxis("Mouse ScrollWheel");

            targetZoom -= scrollData * zoomFactor;
            targetZoom = Mathf.Clamp(targetZoom, 2.25f, 4f);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref yVelocity, Time.deltaTime * zoomLerpSpeed);
        }
        UpdateCamBounds();

        tempVec = player.position - playerDist * Vector3.forward;
        tempVec.x = Mathf.Clamp(tempVec.x, generatedBounds.min.x + orthoBounds.extents.x, generatedBounds.max.x - orthoBounds.extents.x);
        tempVec.y = Mathf.Clamp(tempVec.y, generatedBounds.min.y + orthoBounds.extents.y, generatedBounds.max.y - orthoBounds.extents.y);
        camTransform.position = tempVec;
    }

    public void GenerateWorldBounds()
    {
        generatedBounds = new Bounds((camBoundsObjects[0].position + camBoundsObjects[1].position) / 2.0f, 
                                        new Vector3(Mathf.Abs(camBoundsObjects[0].position.x - camBoundsObjects[1].position.x), 
                                                    Mathf.Abs(camBoundsObjects[0].position.y - camBoundsObjects[1].position.y), 
                                                    Mathf.Abs(camBoundsObjects[0].position.z - camBoundsObjects[1].position.z)));

        UpdateCamBounds();
    }

    void UpdateCamBounds()
    {
        Vector3 orthoUpperLeft = cam.ScreenToWorldPoint(new Vector3(0f, cam.pixelHeight, 0.0f));
        Vector3 orthoLowerRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0f, 0.0f));
        Vector3 orthoMiddle = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2.0f, cam.pixelHeight / 2.0f, 0.0f));
        orthoBounds = new Bounds(orthoMiddle, 
                                    new Vector3(Mathf.Abs(orthoUpperLeft.x - orthoLowerRight.x), 
                                                Mathf.Abs(orthoUpperLeft.y - orthoLowerRight.y), 
                                                Mathf.Abs(orthoUpperLeft.z - orthoLowerRight.z)));
    }
}
