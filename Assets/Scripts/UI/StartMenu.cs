using GameInformation;
using States;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StartMenu : MonoBehaviour
    {
        private void Start()
        {
            GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                GameManager.instance.gameStateManager.SetState(new UpgradingState());
            });
        }
    }
}
