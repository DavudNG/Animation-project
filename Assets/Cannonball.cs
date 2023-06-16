using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public bool iActive;
    public bool iFired;
    public Rigidbody mRigidbody;
    public float flightTimer;
    public float DelayTimer;
    public GameObject myExplosion;

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        mRigidbody.useGravity = false;
        iFired = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(iActive)
        {
            mRigidbody.AddForce(this.gameObject.transform.forward * 30f);
            
            if(flightTimer <= 0)
            {
                //mRigidbody.useGravity = true;
                iActive = false;
            }
            else
            {
                flightTimer -= Time.deltaTime;
            }
        }
        else if (!iActive && DelayTimer <= 0)
        {
            iActive = true;
            iFired = true;
        }
        else if (!iFired)
        {
            DelayTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ship"))
        {
            Debug.Log("a collision occurred");
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.transform.DetachChildren();
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<TrailRenderer>().enabled = false;
            myExplosion.SetActive(true);
        }

    }
}
