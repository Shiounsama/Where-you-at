public class KeybindsView : View
{
    public override void OnClick_Return()
    {
        base.OnClick_Return();

        ViewManager.Instance.Show<PauseMenuView>();
    }
}
