using UnityEngine;
using YellowPanda.UI.UiManager;
public class SimpleUiManager : UiManager<UiScreen>
{
    [SerializeField] UiScreen[] screens;
    protected override UiScreen[] GetUiScreenList() => screens;

    protected override void OnSetUp() { }
}
