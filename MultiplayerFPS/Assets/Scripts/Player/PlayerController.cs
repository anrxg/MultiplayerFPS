using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] float mouseSensi = 3f, sprintSpeed = 6f, walkSpeed = 3f, jumpForce = 250f, smoothTime = .15f;
    [SerializeField] float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity, moveAmount;
    Rigidbody rb;
    [SerializeField] GameObject camHolder;
    PhotonView pv;

    [SerializeField] Items[] items;
    int itemIndex;
    int previousItemIndex = -1;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;
    PlayerManager playerManager;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pv = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)pv.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            EquipItems(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }
        Look();
        Move();
        Jump();
        ChangeItems();
        if (Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

        if (transform.position.y < -10f)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (!pv.IsMine) return;  
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed),
        ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensi);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensi;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        camHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }


    #region Item
    void EquipItems(int _index)
    {
        if (_index < 0 || _index >= items.Length)
        {
            Debug.LogWarning("Tried to equip invalid item index: " + _index);
            return;
        }

        if (previousItemIndex == _index)
            return;

        if (previousItemIndex >= 0)
            items[previousItemIndex].itemGameObject.SetActive(false);

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);
        previousItemIndex = itemIndex;
    }


    void ChangeItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                EquipItems(i);
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if (itemIndex >= items.Length - 1)
            {
                EquipItems(0);
            }
            else
            {
                EquipItems(itemIndex + 1);
            }
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipItems(items.Length - 1);
            }
            else
            {
                EquipItems(itemIndex - 1);
            }
        }

    }


    #endregion

    public void TakeDamage(float damage)
    {
        pv.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    #region RPC

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!pv.IsMine)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        playerManager.Die();
    }
    #endregion
}
