using UnityEngine;
using UnityEngine.Assertions;

namespace Lotec.Utils {
    public class Logger : MonoBehaviour {
        [SerializeField] Transform m_info;

        string _tag;
        IText _info;

        public IText Info => _info;

        void Awake() {
            _info = m_info?.GetComponent<IText>();
            _logger = this;
            _tag = Application.productName;
            if (Debug.isDebugBuild)
                Debug.unityLogger.filterLogType = LogType.Log;
            else
                Debug.unityLogger.filterLogType = LogType.Warning;
        }

        public void _Log(string message) {
            _logger.Info?.SetText(message);
            var methodInfo = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod();
            string parameters = string.Join(", ", (object[])methodInfo.GetParameters());
            string str = $"{methodInfo.ReflectedType.Name}.{methodInfo.Name}({parameters}): {message}";
            Debug.unityLogger.Log(_tag, str);
        }

        // Static helpers        
        static Logger _logger;
        public static void Log(string message) {
            _logger._Log(message);
        }

        void OnValidate() {
            if (m_info) {
                Assert.IsTrue(m_info.GetComponent<IText>() != null, "Info must have an IText Component");
            }
        }
    }
}
