using System.Collections;
using Event;
using TMPro;
using UnityEngine;

namespace Time
{
    public class CountDown : MonoBehaviour
    {

        private TextMeshProUGUI _textMeshProUGUI;
    
        private int _number;
        private int _duration;

        public CustomEvent finishedEvent { get; private set; }

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        public void StartCountDown(int pDuration)
        {
            finishedEvent = new CustomEvent();
            
            StopAllCoroutines();
            _number = pDuration;
        
            StartCoroutine(OneDown());
        }
    
        private IEnumerator OneDown()
        {
            yield return new WaitForSeconds(1);

            _number -= 1;
            _textMeshProUGUI.text = _number.ToString();

            if (_number > 0) StartCoroutine(OneDown());
            else finishedEvent.Raise();
        }
    
        private void Update()
        {
            _textMeshProUGUI.text = _number.ToString();
        }
    }
}
