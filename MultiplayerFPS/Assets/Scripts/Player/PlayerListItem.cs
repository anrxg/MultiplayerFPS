using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text playerNameText;
    public void Setup(Player _player)
    {
        player = _player;
        playerNameText.text = _player.NickName;
    }
 
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(this.gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(this.gameObject);
    }
}
