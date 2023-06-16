using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    // Start is called before the first frame update

    public Float myScript;
    public int Health;
    public GameObject[] Smokes;


    void Start()
    {
        Health = 14;
    }

    // Update is called once per frame
    void Update()
    {
        if(Health < 12)
        {
            Smokes[0].SetActive(true);
            Smokes[1].SetActive(true);
        }

        if (Health < 6)
        {
            Smokes[2].SetActive(true);
            Smokes[3].SetActive(true);
            Smokes[4].SetActive(true);
        }

        if (Health <= 0)
        {
            myScript.enabled = false;
            this.GetComponent<Rigidbody>().transform.Rotate(-0.2f, 0, 0);
            this.GetComponent<Rigidbody>().useGravity = true;
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cannonball"))
        {
            Health -= 1;
        }
    }
}
