using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public GameObject[] ParticleSystems;

    public float DelayTimer0;
    public float DelayTimer1;
    public float DelayTimer2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(DelayTimer0 <= 0)
        {
            ParticleSystems[0].SetActive(true);
        }

        if (DelayTimer1 <= 0)
        {
            ParticleSystems[1].SetActive(true);
        }
        
        if (DelayTimer2 <= 0)
        {
            ParticleSystems[2].SetActive(true);
        }


        DelayTimer0 -= Time.deltaTime;
        DelayTimer1 -= Time.deltaTime;
        DelayTimer2 -= Time.deltaTime;
    }
}
