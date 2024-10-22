using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    // Enemy health stored
    [Tooltip("Total amount of starting health")] public float HealthLevel = 5;
    [Tooltip("Whether this class should destroy the GameObject when HP = 0 (or let an animator do it)")] public bool IsDirectlyDestroyed;
    public bool IsImmune;  // Boolean used to toggle an invincibility state (i-frames)
    [Tooltip("Likelihood that object spawns health pickup")] public float DropRate = 1.0f;
    public GameObject SpawnObject;
    public bool IsGoldenBarrel;
    [SerializeField, Tooltip("Special type of golden barrel that only spawns one item")] private bool _isExtraGoldenBarrel = false;
    public bool Animated;
    private GameObject Player;
    double chance;
    public GameObject EffectParticles;
    public bool ShouldSpawn;
    private bool alreadySpawned = false;
    [SerializeField, Tooltip("Height at which the health pickup spawns (to prevent out of reach pickups)")] private float _pickupHeight = 0.5f;
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        IsImmune = false;
        chance = Random.Range(0, 1.0f);
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthLevel <= 0)
        {
            if (ShouldSpawn)
            {
                if (IsGoldenBarrel)
                {
                    GameObject parent = new();
                    parent.transform.position = gameObject.transform.position;
                    parent.transform.rotation = Player.transform.rotation;
                    parent.name = "Upgrade Options";
                    parent.AddComponent<UpgradeBarrelPickups>();

                    GameObject pickup1 = Instantiate(SpawnObject, parent.transform);
                    pickup1.transform.position = parent.transform.position;
                    pickup1.transform.rotation = parent.transform.rotation;

                    GameObject pickup2 = Instantiate(SpawnObject, parent.transform);
                    pickup2.transform.position = parent.transform.position;
                    pickup2.transform.rotation = parent.transform.rotation;
                    UpgradeMove upgrade1 = pickup2.AddComponent<UpgradeMove>();
                    upgrade1.Direction = -1.0f;

                    GameObject pickup3 = Instantiate(SpawnObject, parent.transform);
                    pickup3.transform.position = parent.transform.position;
                    pickup3.transform.rotation = parent.transform.rotation;
                    UpgradeMove upgrade2 = pickup3.AddComponent<UpgradeMove>();
                    upgrade2.Direction = 1.0f;
                }
                else if (_isExtraGoldenBarrel) // found randomly within maze levels
                {
                    GameObject pickup1 = Instantiate(SpawnObject);
                    pickup1.transform.position = gameObject.transform.position;
                    pickup1.transform.rotation = gameObject.transform.rotation;
                }
                else if (chance < DropRate)
                {
                    Spawn();
                }
            }
            if (IsDirectlyDestroyed)
            {
                GameObject soundObj = new GameObject();
                soundObj.transform.position = transform.position;
                AudioSource audioSource = soundObj.AddComponent<AudioSource>();
                if(_clips[0] != null) // prevent null audio playing for objects that don't use this sound
                    audioSource.PlayOneShot(_clips[0], GameManager.Instance.GetEnvironmentVolume());

                // get rid of it
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject DamagerObject = collision.gameObject;

        if (DamagerObject.CompareTag("PlayerBullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();
            if (bulletStats == null)
            {
                bulletStats = DamagerObject.GetComponentInChildren<BulletStats>();
            }

            if (!IsImmune)
            {
                // destroy damage receiver only if it reaches 0 health
                HealthLevel -= bulletStats.DamageLevel * GameManager.Instance.PlayerData.BulletDamageMult;

                // check if ant needs to be woken
                if (gameObject.TryGetComponent<MeleeMovement>(out MeleeMovement ant))
                {
                    ant.SetTrackingPositionIfIdle(collision.transform.position);
                }
                // check if wasp needs to be woken
                else if (gameObject.TryGetComponent<RangedMovement>(out RangedMovement wasp))
                {
                    wasp.WakeUp();
                }
            }
        }
    }

    private void Spawn()
    {
        if (!alreadySpawned)
        {
            GameObject healthPickup = Instantiate(SpawnObject);
            healthPickup.transform.position = new Vector3(gameObject.transform.position.x, _pickupHeight, gameObject.transform.position.z);
            alreadySpawned = true;
        }
    }

    private void OnDestroy()
    {
        if (Animated && gameObject.scene.isLoaded)
        {
            Instantiate(EffectParticles, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}

