using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using TMPro;



public class PlayerAttack : MonoBehaviour

{
    public TextMeshProUGUI score;

    public LayerMask enemyLayer;

    public Transform camTrans;

    public ParticleSystem muzzleFlash;

    private float raycastDist = 50;

    private int enemiesKilled = 0;
    

    //public Image reticle;

    //private bool reticleTarget = false;

    AudioSource _audioSource;
    public AudioClip shootSound;

    // Update is called once per frame

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()

    {   

        if (Input.GetMouseButtonDown(0))

        {

            RaycastHit hit;
            muzzleFlash.Play();
            _audioSource.PlayOneShot(shootSound);
            

            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, raycastDist, enemyLayer))

            {
                GameObject enemy = hit.collider.gameObject;

                
                /*
                if (enemy.CompareTag("Target"))

                {

                    Rigidbody enemyRB = enemy.GetComponent<RigidBody>();

                    enemyRB.AddForce(transform.forward * 800 + Vector3.up * 200);

                    enemyRB.AddTorque(new Vector3(Random.Range(-50,50), Random.Range(-50,50),Random.Range(-50,50)));

                }
                */
                

                if (enemy.CompareTag("Enemy"))

                {

                    Destroy(enemy);

                    score.text = "Score: " + (++enemiesKilled).ToString();
                }

            }

        }

        

    }

}