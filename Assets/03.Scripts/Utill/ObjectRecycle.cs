using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRecycle : MonoBehaviour
{

    public float Recycle_Time = 0.0F;

    private void OnEnable()
    {
        StartCoroutine("Recycle");
    }

    IEnumerator Recycle()
    {
        yield return new WaitForSeconds(Recycle_Time);

        ObjectPool.Recycle(gameObject);
    }
}
