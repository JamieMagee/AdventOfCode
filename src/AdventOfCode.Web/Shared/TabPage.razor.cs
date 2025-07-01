using Microsoft.AspNetCore.Components;

namespace AdventOfCode.Web.Shared;

/// <summary>
///     Represents a single page of a <see cref="TabControl" />.
/// </summary>
public sealed class TabPage : ComponentBase
{
    /// <summary>
    ///     The name of the page, displayed on the tab button.
    /// </summary>
    [Parameter]
    public required string Name { get; set; }

    /// <summary>
    ///     True if this page is the active one within the parent <see cref="TabControl" />.
    /// </summary>
    [Parameter]
    public bool IsActive { get; set; }

    /// <summary>
    ///     True if the user is allowed to activate the page.
    /// </summary>
    [Parameter]
    public bool IsEnabled { get; set; } = true;

    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    ///     The content of the page.
    /// </summary>
    [Parameter]
    public required RenderFragment ChildContent { get; set; }

    /// <summary>
    ///     The parent TabControl.
    /// </summary>
    [CascadingParameter(Name = "Parent")]
    private TabControl Parent { get; set; } = null!;

    protected override void OnInitialized()
    {
        this.Parent.RegisterPage(this);
        if (this.IsActive)
        {
            this.Parent.ActivatePage(this);
        }
    }
}
