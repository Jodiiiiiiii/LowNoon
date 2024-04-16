using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSpacer : MonoBehaviour
{
    [SerializeField] private GameObject _hpBar;
    private RectTransform _rectTransform;
    private HorizontalLayoutGroup _horizontalLayoutGroup;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _horizontalLayoutGroup = gameObject.GetComponent<HorizontalLayoutGroup>();

        int healthBars = GameManager.Instance.PlayerData.MaxHealth;

        _hpBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (healthBars * 200) + ((healthBars - 1) * 15));
        _rectTransform.localPosition = new Vector3(-((healthBars / 2f * 200) + ((healthBars / 2f - 1) * 15)), 20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        int healthBars = GameManager.Instance.PlayerData.MaxHealth;

        _hpBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (healthBars * 200) + ((healthBars - 1) * 15));

        _rectTransform.localPosition = new Vector3(-((healthBars / 2f * 200) + ((healthBars / 2f - 1) * 15)), 5, 0);

    }
}
