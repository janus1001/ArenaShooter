using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerEffects : NetworkBehaviour
{
    public Animator animator;
    Vector3 positionLastFrame;
    float walkingSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        positionLastFrame = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        walkingSpeed = Vector3.Distance(positionLastFrame, gameObject.transform.position);
        animator.SetFloat("walkingSpeed", walkingSpeed);
        positionLastFrame = gameObject.transform.position;
    }
}
