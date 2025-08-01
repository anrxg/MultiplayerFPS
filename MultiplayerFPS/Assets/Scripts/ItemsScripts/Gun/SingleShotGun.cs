using UnityEngine;
using Photon.Pun;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f));
        ray.origin = cam.transform.position;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            pv.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);

        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, .3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bullectImpactPrefab, hitPosition + hitNormal * .01f, Quaternion.LookRotation(hitNormal, Vector3.up) * bullectImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 3f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }
    

}
