using MahApps.Metro;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace CompuStore.ViewModels
{
    public class MainSettingsViewModel : BindableBase
    {
        public List<AccentColorMenuData> AccentColors { get; set; }
        private AccentColorMenuData selectedAccent;
        public AccentColorMenuData SelectedAccent
        {
            get { return selectedAccent; }
            set { SetProperty(ref selectedAccent, value); DoChange(); }
        }
        private AppThemeMenuData selectedTheme;
        public AppThemeMenuData SelectedTheme
        {
            get { return selectedTheme; }
            set { SetProperty(ref selectedTheme, value); DoChange(); }
        }

        private void DoChange()
        {
            var accent = ThemeManager.GetAccent(SelectedAccent.Name);
            var appTheme = ThemeManager.GetAppTheme(SelectedTheme.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, appTheme);
        }

        public List<AppThemeMenuData> AppThemes { get; set; }
        public MainSettingsViewModel()
        {
            this.AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                                            .ToList();

            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData() { Name = a.Name, BorderColorBrush = a.Resources["BlackColorBrush"] as Brush, ColorBrush = a.Resources["WhiteColorBrush"] as Brush })
                                           .ToList();
            selectedTheme = AppThemes.Find(x => x.Name.Contains("Light"));
            selectedAccent = AccentColors.Find(x => x.Name.Contains("Blue"));
            DoChange();
        }
    }
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }

        private ICommand changeAccentCommand;
        public DelegateCommand ChangeAccentCommand => new DelegateCommand(()=>DoChangeTheme());


        protected virtual void DoChangeTheme()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
    }

    public class AppThemeMenuData : AccentColorMenuData
    {
        protected override void DoChangeTheme()
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
        }
    }
}
