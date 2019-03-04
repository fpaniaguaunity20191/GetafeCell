using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private const string ANIM_ISWALKING = "isWalking";
    private float y;
    private float x;
    private Animator animador;
    private void Awake()
    {
        animador = GetComponent<Animator>();
    }
    void Update () {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if (y > 0.1f)
        {
            Avanzar();
        } else
        {
            Parar();
        }
        if (x != 0)
        {
            Rotar();
        }
	}
    private void Avanzar()
    {
        animador.SetBool(ANIM_ISWALKING, true);
    }
    private void Parar()
    {
        animador.SetBool(ANIM_ISWALKING, false);
    }
    private void Rotar()
    {
        transform.Rotate(0, x, 0);
    }
}
