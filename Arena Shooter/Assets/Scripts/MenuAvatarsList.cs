using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAvatarsList : MonoBehaviour
{
    public GameObject chooseAvatarButton;
    NetworkRoomManagerExtended roomManager;

    void Start()
    {
        roomManager = FindObjectOfType<NetworkRoomManagerExtended>();
        CreateButtons();
    }

    private void CreateButtons()
    {
        Sprite[] allAvatars = Resources.LoadAll<Sprite>("Avatars");

        foreach (Sprite avatar in allAvatars)
        {
            GameObject newAvatar = Instantiate(chooseAvatarButton, transform);
            newAvatar.GetComponent<Image>().sprite = avatar;
            newAvatar.GetComponent<Button>().onClick.AddListener(() => ChooseAvatar(avatar.name));
        }
    }

    private void ChooseAvatar(string avatar)
    {
        roomManager.ChangePlayerAvatar(avatar);
    }
}
