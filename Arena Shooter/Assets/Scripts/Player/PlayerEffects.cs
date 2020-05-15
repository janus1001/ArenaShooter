using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerEffects : NetworkBehaviour
{
    public static PlayerEffects singleton;

    public Animator animator;
    Vector3 positionLastFrame;
    float walkingSpeed = 0.0f;

    public GameObject pistolRepresentation;
    public GameObject rifleRepresentation;
    public GameObject sniperRepresentation;

    // Start is called before the first frame update
    void Start()
    {
        if(isLocalPlayer)
        {
            singleton = this;
        }

        positionLastFrame = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        walkingSpeed = Vector3.Distance(positionLastFrame, gameObject.transform.position);
        animator.SetFloat("walkingSpeed", walkingSpeed);
        positionLastFrame = gameObject.transform.position;
    }

    [Command]
    public void CmdMakeShoot()
    {
        RpcGoShoot();
    }

    [ClientRpc]
    void RpcGoShoot()
    {
        animator.SetTrigger("shooting");
    }

    [Command]
    public void CmdSetWeapon(int id)
    {
        RpcSetWeapon(id);
    }

    [ClientRpc]
    void RpcSetWeapon(int id)
    {
        animator.SetInteger("weapon", id);
        animator.SetTrigger("weaponChange");

        if (!isLocalPlayer)
        {
            if (id == 0)
            {
                pistolRepresentation.SetActive(false);
                rifleRepresentation.SetActive(false);
                sniperRepresentation.SetActive(false);
            }
            else if (id == 1)
            {
                pistolRepresentation.SetActive(true);
                rifleRepresentation.SetActive(false);
                sniperRepresentation.SetActive(false);
            }
            else if (id == 2)
            {
                pistolRepresentation.SetActive(false);
                rifleRepresentation.SetActive(true);
                sniperRepresentation.SetActive(false);
            }
            else if (id == 3)
            {
                pistolRepresentation.SetActive(false);
                rifleRepresentation.SetActive(false);
                sniperRepresentation.SetActive(true);
            }
        }
    }
}
