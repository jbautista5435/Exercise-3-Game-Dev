using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;



public class PlayerAttack : MonoBehaviour

{

    private float raycastDist = 50;

    public LayerMask enemyLayer;

    public Transform camTrans;

    public ParticleSystem muzzleFlash;


    [SerializeField]
    public Transform bulletSpawnPoint;

    [SerializeField]
    public TrailRenderer bulletTrail;

    

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

                TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));

                
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

                }

            }

        }

        

    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        while(time < 1) {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = hit.point;
        Destroy(trail.gameObject, trail.time);
    }

}