using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
	void Update ()
    {
        transform.Rotate(new Vector3(15, 28, 22) * Time.deltaTime);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (tag == "Medhit")
            {
                other.GetComponent<Character>().HitPoints += 50;
            }
            if (tag == "Ammopack")
            {
                other.GetComponent<PlayerController>().Rem += 20;
            }
            other.GetComponent<PlayerController>().UpdateInfo();
            Destroy(gameObject);
        }
    }
}
