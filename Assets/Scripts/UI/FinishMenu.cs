using GameInformation;
using States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class FinishMenu : MonoBehaviour
	{
		[SerializeField] private Button _restartButton;
		[SerializeField] private TextMeshProUGUI _survivedText;
		[SerializeField] private TextMeshProUGUI _victoryText;
		
		private void Start()
		{
			int wavesSurvived = GameManager.instance.waveCount; 
			
			if (wavesSurvived == GameManager.instance.numberOfWaves) 
				_victoryText.gameObject.SetActive(true);

			else
			{
				_survivedText.gameObject.SetActive(true);
				_survivedText.text = $"You survived {wavesSurvived} waves!";
			}

			_restartButton.onClick.AddListener(() =>
			{
				GameManager.instance.gameStateManager.SetState(GameState.Upgrading);
			});
		}
	}
}