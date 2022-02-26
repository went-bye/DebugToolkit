using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace DebugToolkit
{
    [RequireComponent(typeof(UIDocument))]
    public class Register : MonoBehaviour
    {
        [SerializeField] protected UIDocument uiDocument;
        protected ScrollView scrollView;

        private void Reset()
        {
            uiDocument = GetComponent<UIDocument>();
            uiDocument.panelSettings = Resources.Load<PanelSettings>("DebugToolkitPanelSettings");
        }

        private void OnEnable()
        {
            var rootVisualElement = uiDocument.rootVisualElement;
            rootVisualElement.styleSheets.Clear();
            var styleSheet = Resources.Load<StyleSheet>("DebugToolkitUSS");
#if UNITY_EDITOR
            rootVisualElement.styleSheets.Add(EditorOnlyRegister.GetDefaultSkin());
#endif
            rootVisualElement.styleSheets.Add(styleSheet);

            // foldout
            var foldout = new Foldout();
            foldout.text = "DebugToolkit";
            foldout.AddToClassList("rootFoldout");
            rootVisualElement.Add(foldout);

            // スクロールビュー
            scrollView = new ScrollView();
            foldout.Add(scrollView);
        }

        private Dictionary<String, Logger> _text = new Dictionary<string, Logger>();
        private Dictionary<String, ProgressBar> _progress = new Dictionary<string, ProgressBar>();

        public void AddButton(string label, Action clickEvent)
        {
            var button = new Button(clickEvent) {text = label};
            scrollView.Add(button);
        }

        public void AddLog(string keyLabel, string log)
        {
            var isExist = _text.ContainsKey(keyLabel);
            if (isExist)
            {
                _text[keyLabel].ChangeLog(log);
                return;
            }

            var logger = new Logger(keyLabel, log);
            _text.Add(keyLabel, logger);
            scrollView.Add(logger);
        }

        public void AddTextfield(string label, Action<string> changedEvent, string defaultValue = "")
        {
            var textField = new TextField(label);
            textField.value = defaultValue;
            textField.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
            scrollView.Add(textField);
        }

        public void AddToggle(string label, Action<bool> changedEvent, bool defaultValue = false)
        {
            var toggle = new Toggle(label);
            toggle.value = defaultValue;
            toggle.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
            toggle.AddToClassList("defaultField");
            scrollView.Add(toggle);
        }


        public void AddSliderInt(string label, Action<int> changedEvent, int defaultValue = 0, int min = 0,
            int max = 100)
        {
            var sliderInt = new SliderInt(label);
            sliderInt.value = defaultValue;
            sliderInt.lowValue = min;
            sliderInt.highValue = max;
            sliderInt.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
            sliderInt.showInputField = true;
            scrollView.Add(sliderInt);
        }

        public void AddSliderFloat(string label, Action<float> changedEvent, float defaultValue = 0, float min = 0,
            float max = 100)
        {
            var sliderInt = new Slider(label);
            sliderInt.value = defaultValue;
            sliderInt.lowValue = min;
            sliderInt.highValue = max;
            sliderInt.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
            sliderInt.showInputField = true;
            scrollView.Add(sliderInt);
        }


        //============================================
        // Vecor2 Int
        //============================================
        public void AddVector2IntField(string label, Action<Vector2> changedEvent)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector2IntField(label, scrollView, changedEvent);
#endif
        }

        public void AddVector2IntField(string label, Action<Vector2> changedEvent, Vector2 defaultValue)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector2IntField(label, scrollView, changedEvent, defaultValue);
#endif
        }

        //============================================
        // Vecor2 Float
        //============================================
        public void AddVector2Field(string label, Action<Vector2> changedEvent)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector2Field(label, scrollView, changedEvent, Vector2.zero);
#endif
        }

        public void AddVector2Field(string label, Action<Vector2> changedEvent, Vector2 defaultValue)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector2Field(label, scrollView, changedEvent, defaultValue);
#endif
        }

        //============================================
        // Vecor3 Int
        //============================================
        public void AddVector3IntField(string label, Action<Vector3> changedEvent)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector3IntFiled(label, scrollView, changedEvent, Vector3.zero);
#endif
        }

        public void AddVector3IntFiled(string label, Action<Vector3> changedEvent, Vector3 defaultValue)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector3IntFiled(label, scrollView, changedEvent, defaultValue);
#endif
        }

        //============================================
        // Vecor3 Float
        //============================================
        public void AddVector3Field(string label, Action<Vector3> changedEvent)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector3FloatField(label, scrollView, changedEvent, Vector3.zero);
#endif
        }

        public void AddVector3FloatField(string label, Action<Vector3> changedEvent, Vector3 defaultValue)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddVector3FloatField(label, scrollView, changedEvent, defaultValue);
#endif
        }

        public void AddColor(string label, Action<Color> changedEvent, Color defaultValue)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddColor(label, scrollView, changedEvent, defaultValue);
#endif
        }

        public void AddGradientColor(string label, Action<Gradient> changedEvent, Gradient defaultValue)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddGradientColor(label, scrollView, changedEvent, defaultValue);
#endif
        }

        /// <summary>
        /// ２次元グラフを追加する
        /// </summary>
        /// <param name="label"></param>
        /// <param name="changedEvent"></param>
        /// <param name="defaultvalue">AnimationCurve.EaseInOut</param>
        public void AddAnimationCurve(string label, Action<AnimationCurve> changedEvent, AnimationCurve defaultValue)
        {
#if UNITY_EDITOR
            EditorOnlyRegister.AddAnimationCurve(label, scrollView, changedEvent, defaultValue);
#endif
        }

        public void AddProgressBar(string keyLabel, float value, float min = 0, float max = 1)
        {
            var progressBar = new ProgressBar();
            if (_progress.ContainsKey(keyLabel))
            {
                _progress[keyLabel].value = value;
                return;
            }

            progressBar.title = keyLabel;
            progressBar.lowValue = min;
            progressBar.highValue = max;
            progressBar.value = value;
            _progress.Add(keyLabel, progressBar);
            scrollView.Add(progressBar);
        }
    }

    public class Logger : VisualElement
    {
        private Label _key;
        private Label _logText;

        public Logger(string keyLabel, string log)
        {
            this.AddToClassList("textElement");
            _key = new Label(keyLabel);
            _logText = new Label(log);
            Add(_key);
            Add(_logText);
        }

        public void ChangeLog(string log)
        {
            _logText.text = log;
        }
    }
}