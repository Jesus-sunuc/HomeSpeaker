using HomeSpeaker.MAUI.ViewModels;
using HomeSpeaker.Shared;
using System.Collections.ObjectModel;

namespace HomeSpeaker.MAUI.Views;

public partial class ServerSettingsPage : ContentPage
{
	public ServerSettingsPage(ServerSettingsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}