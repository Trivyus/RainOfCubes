using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandom
{
    public Vector3 Position(MeshRenderer meshRenderer)
    {
        Bounds bounds = meshRenderer.bounds;

        return new Vector3 { x = Random.Range(bounds.min.x, bounds.max.x) , y = 7, z = Random.Range(bounds.min.z, bounds.max.z)};
    }

    public void Color(Renderer renderer) => renderer.material.color = new Color(Random.value, Random.value, Random.value);

    public int Range(int min, int max) => Random.Range(min, max);
}
