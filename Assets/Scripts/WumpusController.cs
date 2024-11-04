using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WumpusController : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartGameOverAnimation()
    {
        animator.SetTrigger("Gameover");
    }
    public void StartWinAnimation()
    {
        animator.SetTrigger("Win");
    }
}
