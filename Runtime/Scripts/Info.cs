using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lotec.Utils.Attributes;
using TMPro;
using UnityEngine;

namespace Lotec.Utils
{
    public interface IInfo
    {
        void SetText(string text);
        void AddMessage(string text, float duration = 2f);
    }

    class InfoMessage
    {
        public Coroutine coroutine;
        public string text;
    }

    public class Info : MonoBehaviour, IInfo
    {
        [SerializeField, NotNull] TextMeshProUGUI _uiText;
        [SerializeField] int _maxMessages = 1;
        readonly List<InfoMessage> _messages = new();
        StringBuilder _stringBuilder = new();

#if UNITY_EDITOR
        void Reset() {
            if (_uiText == null) {
                Debug.Log("Info: _uiText is null, trying to find it in children");
                _uiText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
#endif

        public void AddMessage(string text, float duration = 4f)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            if (_messages.Count >= _maxMessages)
            {
                StopCoroutine(_messages[0].coroutine);
                _messages.RemoveAt(0);
            }
            InfoMessage message = new() { text = text };
            message.coroutine = StartCoroutine(RemoveMessage(message, duration));
            _messages.Add(message);
            SetTextFromMessages();
        }

        public void SetText(string text)
        {
            _uiText.text = text;
            gameObject.SetActive(text != null && text.Length > 0);
        }

        IEnumerator RemoveMessage(InfoMessage message, float duration)
        {
            yield return new WaitForSeconds(duration);
            _messages.Remove(message);
            SetTextFromMessages();
        }

        void SetTextFromMessages()
        {
            _stringBuilder.Clear();
            for (int i = 0; i < _messages.Count; i++)
            {
                _stringBuilder.Append(_messages[i].text);
            }
            string str = _stringBuilder.ToString();
            SetText(str);
        }
    }
}
