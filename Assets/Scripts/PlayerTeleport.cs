using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    PlayerMovement playerController;
    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            StartCoroutine("Teleport");

        }
    }
    IEnumerator Teleport()
    {
        //playerController.disabled = true;
        yield return new WaitForSeconds(0.01f);
        player.transform.position = new Vector3(25f, 0f, 0f);
        yield return new WaitForSeconds(0.01f);
        //playerController.disabled = false;
    }
}
