using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadFunction : MonoBehaviour
{
    public float x, y;

    public void Save()
    {
        x = transform.position.x;
        y = transform.position.y;

        PlayerPrefs.SetFloat("x", x);
        PlayerPrefs.SetFloat("y", y);
    }

    public void Load()
    {
        x = PlayerPrefs.GetFloat("x");
        y = PlayerPrefs.GetFloat("y");

        Vector3 LoadPosition = new Vector3(x, y);
        transform.position = LoadPosition;
    }

}
