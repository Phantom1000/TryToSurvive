using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    public Rigidbody body;
    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public GameObject exp;
    [SerializeField]
    public Transform SpawnPosition;
    [SerializeField]
    public AudioSource FireSound;
    public int HitPoints { get; set; }
    public int Ammo { get; set; }
    public bool IsAlive { get; set; }
    public bool IsAttack { get; set; }
    public bool IsReload { get; set; }

    public virtual void TakeDamage(int damage)
    {
        if (IsAlive)
        {
            if (damage < HitPoints)
            {
                HitPoints -= damage;
            }
            else
            {
                HitPoints = 0;
                IsAlive = false;
                Die();
            }
        }
    }

    protected virtual IEnumerator Shoot(float time)
    {
        if ((!IsAttack) && (!IsReload))
        {
            IsAttack = true;
            GameObject BulletInstance = Instantiate(bullet, SpawnPosition.position, SpawnPosition.rotation * Quaternion.Euler(Random.Range(-1, 1), Random.Range(-1, 1), 0));
            Destroy(BulletInstance, 3);
            BulletInstance.GetComponent<Hit>().Shooter = gameObject;
            GameObject ExpInstance = Instantiate(exp, SpawnPosition.position, Quaternion.identity);
            Destroy(ExpInstance, 0.15f);
            Rigidbody rb = BulletInstance.GetComponent<Rigidbody>();
            rb.velocity = BulletInstance.transform.forward * 100;
            BulletInstance.transform.rotation *= Quaternion.Euler(90, 0, 0);
            FireSound.Play();
            Ammo--;
            yield return new WaitForSeconds(time);
            IsAttack = false;
        }
    }

    protected virtual void Die()
    {
        body.freezeRotation = false;
    }
}
