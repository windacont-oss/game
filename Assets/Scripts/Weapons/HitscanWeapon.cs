using UnityEngine;

public class HitscanWeapon : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float range = 100f;
    [SerializeField] private int damage = 20;
    [SerializeField] private float fireRate = 8f;

    private float nextShotTime;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        if (playerCamera == null) return;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, range))
        {
            Health health = hit.collider.GetComponentInParent<Health>();
            if (health != null)
            {
                health.ApplyDamage(damage);
            }
        }
    }
}
