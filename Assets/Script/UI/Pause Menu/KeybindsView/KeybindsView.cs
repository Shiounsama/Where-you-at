public class KeybindsView : View
{
    public override void Show(object args = null)
    {
        base.Show(args);

        TchatPanel?.SetActive(false);
    }

    public override void OnClick_Return()
    {
        base.OnClick_Return();

        ViewManager.Instance.Show<PauseMenuView>();
    }
}
