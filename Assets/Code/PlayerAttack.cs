using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

using TMPro;




public class PlayerAttack : MonoBehaviour

{
    public int enemiesOnLevel;
    
    public GameObject levelLoader;

    public TextMeshProUGUI score;

    public LayerMask enemyLayer;

    public Transform camTrans;

    public ParticleSystem muzzleFlash;

    private float raycastDist = 50;

    private int enemiesKilled = 0;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    public Animator animator;

    



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
        score.text = "Score: 0";
        _audioSource = GetComponent<AudioSource>();
        currentAmmo = maxAmmo;
    }

    void Update()

    {   
        if (isReloading){
            return;
        }
        if (currentAmmo <= 0 || (Input.GetKeyDown(KeyCode.R) && (currentAmmo != maxAmmo))){
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            muzzleFlash.Play();
            _audioSource.PlayOneShot(shootSound);
            currentAmmo--;

            if (Physics.Raycast(bulletSpawnPoint.position, camTrans.forward, out hit, raycastDist, (1 << LayerMask.NameToLayer("Ground") | (1 << LayerMask.NameToLayer("Enemy")))))

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
                    enemy.GetComponent<EnemyAIManager>().TakeDamage(50);
                    if(enemy.GetComponent<EnemyAIManager>().health == 0) {
                        Destroy(enemy);
                        score.text = "Score: " + (++enemiesKilled).ToString();
                    }
                    // Destroy(enemy);

                    // score.text = "Score: " + (++enemiesKilled).ToString();
                }

            }

        }
        if (enemiesKilled >= enemiesOnLevel) {
            levelLoader.GetComponent<LevelLoader>().LoadNextLevel();
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

    IEnumerator Reload(){
        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool ("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);

        animator.SetBool ("Reloading", false);

        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }

}