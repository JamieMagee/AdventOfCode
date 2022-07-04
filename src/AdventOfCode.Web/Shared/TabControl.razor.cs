using Microsoft.AspNetCore.Components;

namespace AdventOfCode.Web.Shared;

public sealed partial class TabControl
{
    private readonly List<TabPage> myPages = new();
    private TabPage myActivePage;

    /// <summary>
    ///     The content of the TabControl, consisting of <see cref="TabPage" /> items.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; }

    /// <summary>
    ///     Called when the <see cref="ActivePage" /> is changed.
    /// </summary>
    [Parameter]
    public Action OnActivatePage { get; set; }

    /// <summary>
    ///     The active page of the TabControl.
    /// </summary>
    private TabPage ActivePage
    {
        get => this.myActivePage;
        set
        {
            this.myActivePage = value;
            this.OnActivatePage?.Invoke();
        }
    }

    /// <summary>
    ///     The pages of the TabControl.
    /// </summary>
    private IReadOnlyList<TabPage> Pages => this.myPages;

    /// <summary>
    ///     Show the given page of the TabControl.
    /// </summary>
    /// <param name="page">The page to activate, existing within <see cref="Pages" />.</param>
    public void ActivatePage(TabPage page) => this.ActivePage = page;

    /// <summary>
    ///     Add a page to the TabControl.
    /// </summary>
    /// <param name="page">The page o be added.</param>
    internal void RegisterPage(TabPage page)
    {
        this.myPages.Add(page);
        this.StateHasChanged();
    }

    protected override void OnParametersSet()
    {
        if (this.ActivePage != null && (!this.ActivePage.IsEnabled || !this.ActivePage.IsVisible))
        {
            var firstEnabledPage = this.Pages.FirstOrDefault(x => x.IsEnabled && x.IsVisible);
            this.ActivatePage(firstEnabledPage);
        }
    }
}
