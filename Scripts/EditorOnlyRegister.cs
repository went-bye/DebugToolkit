using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorOnlyRegister
{
    //============================================
    // Vecor2 Int
    //============================================
    public static void AddVector2IntField(string label, VisualElement element, Action<Vector2> changedEvent)
    {
        AddVector2IntField(label, element, changedEvent, Vector2.zero);
    }

    public static void AddVector2IntField(string label, VisualElement element, Action<Vector2> changedEvent,
        Vector2 defaultValue)
    {
        var vector2Field = new Vector2IntField(label);
        vector2Field.value = new Vector2Int((int) defaultValue.x, (int) defaultValue.y);
        vector2Field.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        element.Add(vector2Field);
    }

    //============================================
    // Vecor2 Float
    //============================================
    public static void AddVector2Field(string label, VisualElement element, Action<Vector2> changedEvent)
    {
        AddVector2Field(label, element, changedEvent, Vector2.zero);
    }

    public static void AddVector2Field(string label, VisualElement element, Action<Vector2> changedEvent,
        Vector2 defaultValue)
    {
        var vector2Field = new Vector2Field(label);
        vector2Field.value = new Vector2Int((int) defaultValue.x, (int) defaultValue.y);
        vector2Field.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        element.Add(vector2Field);
    }

    //============================================
    // Vecor3 Int
    //============================================
    public static void AddVector3IntField(string label, VisualElement element, Action<Vector3> changedEvent)
    {
        AddVector3IntFiled(label, element, changedEvent, Vector3.zero);
    }

    public static void AddVector3IntFiled(string label, VisualElement element, Action<Vector3> changedEvent,
        Vector3 defaultValue)
    {
        var vector3Field = new Vector3IntField(label);
        vector3Field.value = new Vector3Int((int) defaultValue.x, (int) defaultValue.y, (int) defaultValue.z);
        vector3Field.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        element.Add(vector3Field);
    }

    //============================================
    // Vecor3 Float
    //============================================
    public static void AddVector3Field(string label, VisualElement element, Action<Vector3> changedEvent)
    {
        AddVector3FloatField(label, element, changedEvent, Vector3.zero);
    }

    public static void AddVector3FloatField(string label, VisualElement element, Action<Vector3> changedEvent,
        Vector3 defaultValue)
    {
        var vector3Field = new Vector3Field(label);
        vector3Field.value = defaultValue;
        vector3Field.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        element.Add(vector3Field);
    }

    // colorfieldが使えないので、gradientの0秒で代用
    //TODO https://light11.hatenadiary.com/entry/2020/04/26/124956
    public static void AddColor(string label, VisualElement element, Action<Color> changedEvent, Color defaultValue)
    {
        var gradientField = new GradientField(label);
        // 色の設定
        var colorKey = new GradientColorKey[1];
        colorKey[0].color = defaultValue;
        colorKey[0].time = 0.0f;
        // 透明度(仮)
        var alphaKey = new GradientAlphaKey[1];
        alphaKey[0].alpha = defaultValue.a;
        alphaKey[0].time = 0.0f;
        var gradient = new Gradient();
        gradient.SetKeys(colorKey, alphaKey);

        gradientField.value = gradient;
        gradientField.RegisterValueChangedCallback(_ => { changedEvent(_.newValue.Evaluate(0.0f)); });
        element.Add(gradientField);
    }

    public static void AddGradientColor(string label, VisualElement element, Action<Gradient> changedEvent,
        Gradient defaultValue)
    {
        var gradientField = new GradientField(label);
        gradientField.value = defaultValue;
        gradientField.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        element.Add(gradientField);
    }

    /// <summary>
    /// ２次元グラフを追加する
    /// </summary>
    /// <param name="label"></param>
    /// <param name="changedEvent"></param>
    /// <param name="defaultvalue">AnimationCurve.EaseInOut</param>
    public static void AddAnimationCurve(string label, VisualElement element, Action<AnimationCurve> changedEvent,
        AnimationCurve defaultValue)
    {
        var curveField = new CurveField(label);
        curveField.value = defaultValue;
        curveField.RegisterValueChangedCallback(_ => { changedEvent(_.newValue); });
        element.Add(curveField);
    }

    public static StyleSheet GetDefaultSkin()
    {
        //UnityEditor.UIElementsのアセンブリを取得
        var assembly = typeof(Toolbar).Assembly;

        //UIElementsEditorUtilityのTypeを取得
        var type = assembly.GetType("UnityEditor.UIElements.UIElementsEditorUtility");
        if (type == null) return null;

        //StyleSheetを格納するフィールドを取得
        var darkField = type.GetField("s_DefaultCommonDarkStyleSheet", BindingFlags.Static | BindingFlags.NonPublic);

        if (darkField == null) return null;

        return (StyleSheet) darkField.GetValue(null);
    }
}