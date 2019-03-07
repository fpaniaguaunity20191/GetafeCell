using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warning : MonoBehaviour {
    [SerializeField] Camera currentCam;
	void Update () {
        //Finalmente hemos utilizado AIM Constraint para la orientación del quad
        /*transform.LookAt(new Vector3(
            currentCam.transform.position.x,
            transform.position.y,
            currentCam.transform.position.z));*/
	}
}
