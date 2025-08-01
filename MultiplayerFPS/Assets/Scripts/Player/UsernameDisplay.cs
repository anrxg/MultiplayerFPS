using UnityEngine;
using Photon.Pun;
using TMPro;        

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerpv;
    [SerializeField] TMP_Text text;   
    
    void Start()
    {
        if (playerpv.IsMine)
            gameObject.SetActive(false);
            
        text.text = playerpv.Owner.NickName;
    }
}
