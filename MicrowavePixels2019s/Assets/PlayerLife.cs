using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public int MaxLife;
    public int Damage;
    public Slider LifeSlider;
    public GameObject GameOver;
    public Animator animator;
    public float invulFrames;
    private bool invul = false;

    public void TakeDamageTrigger()
    {
        if (!invul)
        {
            StartCoroutine(Invunlenaribility());
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        MaxLife = MaxLife - Damage;
        LifeSlider.value = MaxLife;
        if (MaxLife <= 0)
        {
            animator.SetBool("GameOver", true);
            yield return new WaitForSeconds(2);
            GameOver.SetActive(true);
            Time.timeScale = 0.00001f;

        }
    }
    IEnumerator Invunlenaribility()
    {
        invul = true;
        yield return new WaitForSeconds(invulFrames);
        invul = false;

    }
}
