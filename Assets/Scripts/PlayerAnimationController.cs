using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    
    private IEnumerator CheckMoving()
    {
        Vector3 startPos = this.transform.position;
        yield return new WaitForSeconds(0.01f);
        Vector3 finalPos = this.transform.position;

        if( startPos.x != finalPos.x  || startPos.z != finalPos.z)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

    }

    void Update()
    {
        StartCoroutine(CheckMoving());
    }
}
