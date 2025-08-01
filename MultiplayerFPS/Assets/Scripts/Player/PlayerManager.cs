using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.IO;
using System.Linq;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static PlayerManager instance;
    GameObject controller;
    PhotonView pv;
    int kills = 0;
    int deaths = 0;
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
        Transform spawnPoint = SpawnManager.instance.GetSpawnPoints();
        controller = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), spawnPoint.position, spawnPoint.rotation, 0, new object[] { pv.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(controller);
        Invoke(nameof(CreateController), 0.5f);
        deaths++;
        HashTable hash = new HashTable();
        hash.Add("deaths", deaths);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public static PlayerManager Find(Player player)
    {
        return FindObjectsByType<PlayerManager>(FindObjectsSortMode.None).SingleOrDefault(x => x.pv.Owner == player);
    }
    public void GetKill()
    {
        pv.RPC(nameof(RPC_GetKill), pv.Owner);
    }

    [PunRPC]
    void RPC_GetKill() 
    {
        kills++;
        HashTable hash = new HashTable();
        hash.Add("kills", kills);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }
}
