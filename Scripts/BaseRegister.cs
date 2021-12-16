using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DebugToolkit
{
    [RequireComponent(typeof(UIDocument))]
    public class BaseRegister : MonoBehaviour
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
    }
}