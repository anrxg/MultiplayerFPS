using Photon.Pun;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager instance;
    GameObject controller;
    PhotonView pv;
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        instance = this;
    }

    void Start()
    {
        if (pv.IsMine)
        {
            CreateController();
        }
    }

    void CreateController()
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), Vector3.zero, Quaternion.identity, 0, new object[] { pv.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        Invoke(nameof(CreateController), 0.5f);
    }
}
