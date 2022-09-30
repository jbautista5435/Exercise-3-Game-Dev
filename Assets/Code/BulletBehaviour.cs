using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float timeLimit;
    private bool debounce = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifespanTimer());
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Bullet hit collision");
        if(other.gameObject.name == "Player" && debounce) {
            debounce = false;
            other.gameObject.GetComponent<PlayerController>().takeDamage(10);
        }
        // Destroy(gameObject);
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
