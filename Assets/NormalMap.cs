using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalMap : MonoBehaviour
{
    public Texture2D normal;
    // Start is called before the first frame update
    void Start()
    {
        if(normal != null)
        {
            GetComponent<MeshRenderer>().material.mainTexture = normal;
        }
    }

    public Texture2D getTexture()
    {
        return normal;
    }
}
