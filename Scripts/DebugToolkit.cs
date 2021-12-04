using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

// gameobject.AddComponent<NiuGUI>(); で取得できます。
public class DebugToolkit : MonoBehaviour
{
    // メンバ変数
    private Foldout _foldout;
    private ScrollView _scrollView;
    private Dictionary<String, Logger> _text = new Dictionary<string, Logger>();
    private Dictionary<String, ProgressBar> _progress = new Dictionary<string, ProgressBar>();

    public void AddButton(string label, Action clickEvent)
    {
        var button = new Button(clickEvent) {text = label};
        _scrollView.Add(button);
    }

    public void AddLog(string keyLabel, string log)
    {
        var isExist = _text.ContainsKey(keyLabel);
        if (isExist)
        {
            _text[keyLabel].ChangeLog(log);
            return;
        }

        //要素追加
        var logger = new Logger(keyLabel, log);
        _text.Add(keyLabel, logger);
        _scrollView.Add(logger);
    }

    public void AddTextfield(string label, Action<string> changedEvent, string defaultValue = "")
    {
        var textField = new TextField(label);
        textField.value = defaultValue;
        textField.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        _scrollView.Add(textField);
    }

    public void AddToggle(string label, Action<bool> changedEvent, bool defaultValue = false)
    {
        var toggle = new Toggle(label);
        toggle.value = defaultValue;
        toggle.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        toggle.AddToClassList("defaultField");
        _scrollView.Add(toggle);
    }


    public void AddSliderInt(string label, Action<int> changedEvent, int defaultValue = 0, int min = 0, int max = 100)
    {
        var sliderInt = new SliderInt(label);
        sliderInt.value = defaultValue;
        sliderInt.lowValue = min;
        sliderInt.highValue = max;
        sliderInt.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        sliderInt.showInputField = true;
        _scrollView.Add(sliderInt);
    }

    //TODO sliderのfloat版を作る
    public void AddSliderFloat(string label, Action<float> changedEvent, float defaultValue = 0, float min = 0,
        float max = 100)
    {
        var sliderInt = new Slider(label);
        sliderInt.value = defaultValue;
        sliderInt.lowValue = min;
        sliderInt.highValue = max;
        sliderInt.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        sliderInt.showInputField = true;
        _scrollView.Add(sliderInt);
    }


    //============================================
    // Vecor2 Int
    //============================================
    public void AddVector2IntField(string label, Action<Vector2> changedEvent)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector2IntField(label, _scrollView, changedEvent);
#endif
    }

    public void AddVector2IntField(string label, Action<Vector2> changedEvent, Vector2 defaultValue)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector2IntField(label, _scrollView, changedEvent, defaultValue);
#endif
    }

    //============================================
    // Vecor2 Float
    //============================================
    public void AddVector2Field(string label, Action<Vector2> changedEvent)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector2Field(label, _scrollView, changedEvent, Vector2.zero);
#endif
    }

    public void AddVector2Field(string label, Action<Vector2> changedEvent, Vector2 defaultValue)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector2Field(label, _scrollView, changedEvent, defaultValue);
#endif
    }

    //============================================
    // Vecor3 Int
    //============================================
    public void AddVector3IntField(string label, Action<Vector3> changedEvent)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector3IntFiled(label, _scrollView, changedEvent, Vector3.zero);
#endif
    }

    public void AddVector3IntFiled(string label, Action<Vector3> changedEvent, Vector3 defaultValue)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector3IntFiled(label, _scrollView, changedEvent, defaultValue);
#endif
    }

    //============================================
    // Vecor3 Float
    //============================================
    public void AddVector3Field(string label, Action<Vector3> changedEvent)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector3FloatField(label, _scrollView, changedEvent, Vector3.zero);
#endif
    }

    public void AddVector3FloatField(string label, Action<Vector3> changedEvent, Vector3 defaultValue)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddVector3FloatField(label, _scrollView, changedEvent, defaultValue);
#endif
    }

    // colorfieldが使えないので、gradientの0秒で代用
    //TODO https://light11.hatenadiary.com/entry/2020/04/26/124956
    public void AddColor(string label, Action<Color> changedEvent, Color defaultValue)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddColor(label, _scrollView, changedEvent, defaultValue);
#endif
    }

    public void AddGradientColor(string label, Action<Gradient> changedEvent, Gradient defaultValue)
    {
#if UNITY_EDITOR
        DebugToolkitForEditor.AddGradientColor(label, _scrollView, changedEvent, defaultValue);
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
        DebugToolkitForEditor.AddAnimationCurve(label, _scrollView, changedEvent, defaultValue);
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
        _scrollView.Add(progressBar);
    }


    private void OnEnable()
    {
        var comp = gameObject.AddComponent<UIDocument>();
        comp.panelSettings = Resources.Load<PanelSettings>("DebugToolkitPanelSettings");
        var rootVisualElement = comp.rootVisualElement;


        rootVisualElement.styleSheets.Clear();
        var styleSheet = Resources.Load<StyleSheet>("DebugToolkitUSS");
#if UNITY_EDITOR
        rootVisualElement.styleSheets.Add(DebugToolkitForEditor.GetDefaultSkin());
#endif
        rootVisualElement.styleSheets.Add(styleSheet);

        // foldout
        _foldout = new Foldout();
        _foldout.text = "Foldout";
        _foldout.AddToClassList("rootFoldout");
        rootVisualElement.Add(_foldout);

        // スクロールビュー
        _scrollView = new ScrollView(); //箱を用意する
        _foldout.Add(_scrollView);


        // ドロップダウン
        var dropList = new List<string>() {"Test", "Hey"};
        var dropbox = new DropdownField("Choise", dropList, "Test") { };
        _scrollView.Add(dropbox);
    }
}

public class Logger : VisualElement
{
    private Label _key;
    private Label _logText;

    public Logger(string keyLabel, string log)
    {
        this.AddToClassList("textElement"); //NiuUSS
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