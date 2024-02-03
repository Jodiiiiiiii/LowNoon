using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    // TODO
    // Look at player
    // Devise a way to spawn 3 of these and destroy them all when one is chosen

    public enum UpgradeType
    {
        Health,
        Armor,
        Damage,
        FireSpeed,
        MoveSpeed,
        Light
    };

    [SerializeField] private List<GameObject> modelOptions;

    public bool RandomizeOnStartup = true;  // When this upgrade spawns, its type is randomly selected if this is true
    public UpgradeType Type; // What upgrade is this?

    void Start()
    {
        if (RandomizeOnStartup)
        {
            RandomlyChooseType();
        }
        setModel(Type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RandomlyChooseType()
    {
        int random = Random.Range(0, 6);
        if(random == 0)
            Type = UpgradeType.Health;
        else if(random == 1)
            Type = UpgradeType.Armor;
        else if(random == 2)
            Type = UpgradeType.Damage;
        else if(random == 3)
            Type = UpgradeType.FireSpeed;
        else if(random == 4)
            Type = UpgradeType.MoveSpeed;
        else if(random == 5)
            Type = UpgradeType.Light;
    }

    private void setModel(UpgradeType type) // Set the model for whatever we want this upgrade to be
    {
        if (type == UpgradeType.Health)
            modelOptions[0].SetActive(true);
        else if (type == UpgradeType.Armor)
            modelOptions[1].SetActive(true);
        else if (type == UpgradeType.Damage)
            modelOptions[2].SetActive(true);
        else if (type == UpgradeType.FireSpeed)
            modelOptions[3].SetActive(true);
        else if (type == UpgradeType.MoveSpeed)
            modelOptions[4].SetActive(true);
        else if(type == UpgradeType.Light)
            modelOptions[5].SetActive(true);
    }

    private void OnTriggerEnter(Collider other) // For checking if the player has touched the upgrade
    {
        if (other.CompareTag("Player"))
        {
            if (Type == UpgradeType.Health)
            {
                // Insert call to appropriate function
            }
            else if (Type == UpgradeType.Armor)
            {
                // Insert call to appropriate function
            }
            else if (Type == UpgradeType.Damage)
            {
                // Insert call to appropriate function
            }
            else if (Type == UpgradeType.FireSpeed)
            {
                // Insert call to appropriate function
            }
            else if (Type == UpgradeType.MoveSpeed)
            {
                // Insert call to appropriate function
            }
            else if (Type == UpgradeType.Light)
            {
                // Insert call to appropriate function
            }
            Destroy(this.gameObject);
        }
    }
}
