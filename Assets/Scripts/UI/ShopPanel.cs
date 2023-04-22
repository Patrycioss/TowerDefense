using System;
using GameInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private Image _image; 
        [SerializeField] private Button _button;

        private void OnValidate()
        {
            if (_costText == null) Debug.LogWarning("No cost set for " + name);
            if (_itemNameText == null) Debug.LogWarning("No itemName set for " + name);
            if (_image == null) Debug.LogWarning("No image set for " + name);
            if (_button == null) Debug.LogWarning("No button set for " + name);
        }

        public void Load(Tower pTower)
        {
            _button.onClick.AddListener(() => GameManager.instance.shop.BuyTower(pTower));
            _image.sprite = pTower.sprite;
            _itemNameText.text = pTower.shopName;
            _costText.text = pTower.cost.ToString();
        }
    }
}
