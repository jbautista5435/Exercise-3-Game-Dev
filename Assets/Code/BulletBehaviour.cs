using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float timeLimit;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifespanTimer());
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.name == "Player") {
            other.gameObject.GetComponent<PlayerController>().takeDamage(10);
        }
        Destroy(gameObject);
    }

    IEnumerator LifespanTimer()
    {
        yield return new WaitForSeconds(timeLimit);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
