using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public bool disableRendererOnStart = true;
    public MeshRenderer meshRenderer;

    private void Start()
    {
        if (disableRendererOnStart)
        {
            // turn off my renderer on start-up
            meshRenderer.enabled = false;
        }
    }
}
