using Photon.Pun;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager instance;
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
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), Vector3.zero, Quaternion.identity);
    } 
}
