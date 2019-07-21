using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyAnim : MonoBehaviour
{
    public float delay;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
