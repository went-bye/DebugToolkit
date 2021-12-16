using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace DebugToolkit
{
    public class VSRegister : BaseRegister
    {
        public void AddButton(string eventName, string label = "Button")
        {
            var button = new Button(() => { CustomEvent.Trigger(gameObject, eventName); }) {text = label};
            scrollView.Add(button);
        }
    }
}