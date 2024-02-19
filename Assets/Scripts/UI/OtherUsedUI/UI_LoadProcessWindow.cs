using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[UIWindowData(nameof(UI_LoadProcessWindow), false, nameof(UI_LoadProcessWindow), 2)]
public class UI_LoadProcessWindow : UI_WindowBase
{
    [SerializeField] public Image ProcessBarImage;
    [SerializeField] public Text PeocessText;
}
