using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField userNameInput;

    void Start()
    {
        if (PlayerPrefs.HasKey("username"))
        {
            userNameInput.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(0, 10000).ToString("0000");
            OnUserNameInputValueChanged();
        }
    }
    public void OnUserNameInputValueChanged()
    {
        PhotonNetwork.NickName = userNameInput.text;
        PlayerPrefs.SetString("username", userNameInput.text);
    }
}
