using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimationRandom : MonoBehaviour
{
    [SerializeField] string _stateName;

    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
        if (animator == null)
            Destroy(this);
        else
            animator.Play(_stateName, 0, Random.Range(0f, 1f));
    }
}
