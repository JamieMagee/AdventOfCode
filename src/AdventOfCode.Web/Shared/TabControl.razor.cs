using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace AdventOfCode.Web.Shared
{
    /// <summary>
    ///     Tabbed container control with a single active page.
    ///     Add pages using <see cref="TabPage" /> items as child content.
    /// </summary>
    public sealed partial class TabControl
    {
        private readonly List<TabPage> _myPages = new List<TabPage>();
        private TabPage _myActivePage;

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
            get => _myActivePage;
            set
            {
                _myActivePage = value;
                OnActivatePage?.Invoke();
            }
        }

        /// <summary>
        ///     The pages of the TabControl.
        /// </summary>
        private IReadOnlyList<TabPage> Pages => _myPages;

        /// <summary>
        ///     Show the given page of the TabControl.
        /// </summary>
        /// <param name="page">The page to activate, existing within <see cref="Pages" />.</param>
        public void ActivatePage(TabPage page)
        {
            ActivePage = page;
        }

        /// <summary>
        ///     Add a page to the TabControl.
        /// </summary>
        /// <param name="page">The page o be added.</param>
        internal void RegisterPage(TabPage page)
        {
            _myPages.Add(page);
            StateHasChanged();
        }

        protected override void OnParametersSet()
        {
            if (ActivePage != null && (!ActivePage.IsEnabled || !ActivePage.IsVisible))
            {
                var firstEnabledPage = Pages.FirstOrDefault(x => x.IsEnabled && x.IsVisible);
                ActivatePage(firstEnabledPage);
            }
        }
    }
}