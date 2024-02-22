using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObtainedItemCard : MonoBehaviour
{
    [SerializeField] List<Sprite> _icons;
    [SerializeField] Image _currentImage;
    [SerializeField] TextMeshProUGUI _titleText1;
    [SerializeField] TextMeshProUGUI _titleText2;
    [SerializeField] TextMeshProUGUI _bodyText1;
    [SerializeField] TextMeshProUGUI _bodyText2;

    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextAndShow(int type, string title, string body)   // Whenever we set what's on the card, we show it anyway
    {
        _currentImage.sprite = _icons[type];
        _titleText1.text = title;
        _titleText2.text = title;
        _bodyText1.text = body;
        _bodyText2.text = body;
        _anim.Play("MoveUp", 0, 0);

    }

    public void Hide()
    {
        _anim.Play("MoveDown", 0, 0);
    }


}
