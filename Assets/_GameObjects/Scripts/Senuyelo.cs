using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senuyelo : MonoBehaviour {
    [SerializeField] GameObject prefabSmoke;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject[] vigilantes = GameObject.FindGameObjectsWithTag("Vigilante");
        foreach (GameObject vigilante in vigilantes)
        {
            vigilante.GetComponent<Douglas>().SetSenyuelo(transform.position);
        }
        Instantiate(prefabSmoke, transform.position, Quaternion.Euler(-90,0,0));
        Destroy(this.gameObject);
    }
}
