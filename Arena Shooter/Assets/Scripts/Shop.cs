using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Shop : NetworkBehaviour
{
    public GameObject itemPreview;
    public Transform detailsPage;

    public Buyable soldItem;

    float slideTarget = 0f;

    private void Update()
    {
        SetDetailsPosition();
    }

    private void SetDetailsPosition()
    {
        Vector3 newLocalPosition = detailsPage.localPosition;
        newLocalPosition.y = Mathf.Lerp(newLocalPosition.y, slideTarget, 5f * Time.deltaTime);
        detailsPage.localPosition = newLocalPosition;
    }

    public void ShowDetails()
    {
        slideTarget = -9f;
    }

    public void HideDetails()
    {
        slideTarget = 0f;
    }
}
