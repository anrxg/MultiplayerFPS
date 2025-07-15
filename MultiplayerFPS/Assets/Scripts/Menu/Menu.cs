using UnityEngine;
using Photon.Pun;

public class Menu : MonoBehaviourPunCallbacks
{
    public static Menu instance;
    public string menuName;
    [HideInInspector] public bool isOpen;

    void Start()
    {
        instance = this;
    }
    public void Open()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        isOpen =  false;
        gameObject.SetActive(false);
    }
}
