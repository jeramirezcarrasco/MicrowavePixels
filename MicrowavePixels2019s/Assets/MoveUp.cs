using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{

    public float MoveSpeed;

    void Update()
    {
        transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
    }
}
