using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    // TODO
    // Devise a way to spawn 3 of these and destroy them all when one is chosen

    public enum UpgradeType
    {
        Health = 0,
        Armor = 1,
        Damage = 2,
        FireSpeed = 3,
        MoveSpeed = 4,
        Light = 5
    };

    [SerializeField] private List<GameObject> _modelOptions;
    [SerializeField] private AudioSource _audioSoure;
    [Tooltip("0 = Health Pickup, 1 = Badge Pickup, 2 = Other Pickups")]
    [SerializeField] private List<AudioClip> _clips;

    public bool RandomizeOnStartup = true;  // When this upgrade spawns, its type is randomly selected if this is true
    public UpgradeType Type; // What upgrade is this?

    void Start()
    {
        if (RandomizeOnStartup)
        {
            RandomlyChooseType();
        }
        setModel(Type);
        _audioSoure.volume = GameManager.Instance.GetEnvironmentVolume();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // late update ensures smoothness with respect to camera
        // align upgrade towards player camera
        transform.LookAt(Camera.main.transform, Vector3.up);
    }

    private void RandomlyChooseType()
    {
        Type = (UpgradeType)Random.Range(0, 6);
        
    }

    private void setModel(UpgradeType type) // Set the model for whatever we want this upgrade to be
    {
        if (type == UpgradeType.Health)
            _modelOptions[0].SetActive(true);
        else if (type == UpgradeType.Armor)
            _modelOptions[1].SetActive(true);
        else if (type == UpgradeType.Damage)
            _modelOptions[2].SetActive(true);
        else if (type == UpgradeType.FireSpeed)
            _modelOptions[3].SetActive(true);
        else if (type == UpgradeType.MoveSpeed)
            _modelOptions[4].SetActive(true);
        else if(type == UpgradeType.Light)
            _modelOptions[5].SetActive(true);
    }

    private void OnTriggerEnter(Collider other) // For checking if the player has touched the upgrade
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ApplyUpgradeToStats(Type);
            ViewManager.GetView<InGameUIView>().CallItemCard((int)Type);

            AudioClip clip;

            if (Type == UpgradeType.Health)
            {
                clip = _clips[0];
            }
            else if (Type == UpgradeType.Armor)
            {
                clip = _clips[1];
            }
            else
            {
                clip = _clips[2];
            }

            GameObject soundObj = new GameObject();
            soundObj.transform.position = transform.position;
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.PlayOneShot(clip, GameManager.Instance.GetPlayerVolume());

            Destroy(this.gameObject);
        }
    }
}
