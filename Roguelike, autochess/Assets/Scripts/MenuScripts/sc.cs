using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc : MonoBehaviour
{
    public Animator animator;
    private int level;
    public void ToLevel(int ind)
    {
        level = ind;
        animator.SetTrigger("toGame");
    }
    public void OnAnimationComplete()
    {
        SceneManager.LoadScene(level);
    }
}
