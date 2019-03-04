using UnityEngine;

public class Hit : MonoBehaviour
{
    public GameObject Shooter { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if ((Shooter.tag == "Player") && (other.tag == "Enemy"))
        {
            other.GetComponent<EnemyController>().TakeDamage(5);
        }
        if ((Shooter.tag == "Enemy") && (other.tag == "Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(5);
        }
        if (other.tag != Shooter.tag)
        {
            Destroy(gameObject);
        }
        
    }
}