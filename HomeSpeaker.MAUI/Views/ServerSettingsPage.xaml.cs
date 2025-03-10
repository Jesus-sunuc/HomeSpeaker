using HomeSpeaker.MAUI.ViewModels;

namespace HomeSpeaker.MAUI.Views;

public partial class ServerSettingsPage : ContentPage
{
	public ServerSettingsPage(ServerSettingsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}