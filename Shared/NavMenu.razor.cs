using Telerik.Blazor.Components.Menu;
using ThreeSC.Net6Lib.BlazorTools.Authorization;

namespace UniANPR.Shared
{
    public partial class NavMenu
    {
        #region Menu item class

        private class MenuItem
        {
            public string Id { get; set; }
            public string Text { get; set; }
            public bool Disabled { get; set; }
            public bool Separator { get; set; }
            public string Icon { get; set; }
            public string Url { get; set; }
            public string ImageClass { get; set; }
            public string ImageUrl { get; set; }
            public string Class { get; set; }
            public MenuItem Parent { get; set; }
            public IEnumerable<MenuItem> Items { get; set; }
        }
        #endregion

        #region Declarations

        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        /// <summary>
        /// List of all possible main menu options
        /// </summary>
        private List<MenuItem> MenuItems { get; set; }

        /// <summary>
        /// Selected menu option
        /// </summary>
        private MenuItem SelectedItem { get; set; }

        #endregion

        #region OnInitialization
        protected override void InitialiseComponentStaticData()
        {
            PopulateMenuOptions();
            SelectedItem = new MenuItem();
        }

        protected override void OnDispose()
        {
     
        }


        protected override void OnFirstRender()
        {
        }
        #endregion


        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        /// <summary>
        /// On user selecting a menu option set the option highlighting rules 
        /// </summary>
        /// <param name="item">Selected menu option</param>
        private void OnClickHandler(MenuItem item)
        {
            foreach (MenuItem thisParentItem in MenuItems)
            {
                thisParentItem.Class = "";

                if (thisParentItem.Items != null)
                {
                    foreach (MenuItem thisChildItem in thisParentItem.Items)
                    {
                        thisChildItem.Class = "";
                    }
                }
            }

            SelectedItem = item;
            SelectedItem.Class = "highlightedOption";

            if (SelectedItem.Parent != null)
            {
                MenuItem thisSelectedParent = SelectedItem.Parent;
                thisSelectedParent.Class = "highlightedOption";
            }
        }

        /// <summary>
        /// Populate the menu Items based on the current Users role
        /// </summary>
        private void PopulateMenuOptions()
        {
            // Build up the dynamic menu
            // -------------------------
            MenuItems = new List<MenuItem>();

            if (IsInRole(ThreeSCRole.UserAdmin) || IsInRole("Operator"))
            {
                MenuItem dashboardMenu = new MenuItem()
                {
                    Id = "dashboard",
                    Url = "/Dashboard",
                    Text = "Dashboard",
                    Icon = "icn_MenuContainer icn_SiteOverview",
                };

                MenuItems.Add(dashboardMenu);
            }

            // Add Site overview menus that user is authorised for 
            // ---------------------------------------------------
            if (IsInRole("Operator"))
            {

                MenuItem activeRaceMenu = new MenuItem()
                {
                    Id = "operatorActiveRace",
                    Url = "/ActiveRace",
                    Text = "ActiveRace",
                    Icon = "icn_MenuContainer icn_SiteOverview",
                };

                MenuItem userManagementMenu = new MenuItem()
                {
                    Id = "operatorUserManagement",
                    Url = "/UserManagement",
                    Text = "UserManagement",
                    Icon = "icn_MenuContainer icn_SiteOverview",
                };

                MenuItems.Add(activeRaceMenu);
                MenuItems.Add(userManagementMenu);
            }


            if (IsInRole(ThreeSCRole.UserAdmin))
            {
                MenuItem applicationLogsMenu = new MenuItem()
                {
                    Id = "userAdminAppLogs",
                    Url = "/ApplicationLogs",
                    Text = "Application Logs",
                    Icon = "icn_MenuContainer icn_SiteManagement",
                };

                MenuItems.Add(applicationLogsMenu);
            }

            // Add Setting menus that user is authorised for 
            // ---------------------------------------------
            //if (IsInRole("UserAdmin"))
            //{
            //    MenuItem userAdmin = new MenuItem()
            //    {
            //        Id = "userAdmin",
            //        Url = "",
            //        Text = "User Admin",
            //        Icon = "icn_MenuContainer icn_SuperAdmin",
            //        Items = new List<MenuItem>()
            //        {
            //            new MenuItem()
            //            {
            //                Id = "userAdminAppLogs",
            //                Url = "/ApplicationLogs",
            //                Text = GetLocalisedString("lblApplicationLogs"),
            //                Icon = "icn_MenuContainer icn_SiteManagement",
            //            },
            //            new MenuItem()
            //            {
            //                Id = "userAdminVehicleCommsLog",
            //                Url = "/VehicleCommsLog",
            //                Text = GetLocalisedString("lblVehicleCommsLog"),
            //                Icon = "icn_MenuContainer icn_SiteManagement",
            //            },
            //        }
            //    };

            //    MenuItems.Add(userAdmin);
            //}
        }
    }
}
