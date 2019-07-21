using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserScript : MonoBehaviour
{

    public float WakeUp;
    public float OnDuration;
    public float OffDuration;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(LaserActivation());
    }

    IEnumerator LaserActivation()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(WakeUp);
        while (true)
        {
            animator.SetBool("LaserOn", true);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            GetComponent<Collider2D>().enabled = true;
            yield return new WaitForSeconds(OnDuration);
            animator.SetBool("LaserOn", false);
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(OffDuration);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerLife PlayerLife = collision.GetComponent<PlayerLife>();
            PlayerLife.TakeDamageTrigger();
        }
    }
}
