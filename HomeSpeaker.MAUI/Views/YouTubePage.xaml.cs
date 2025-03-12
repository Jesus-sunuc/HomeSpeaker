using HomeSpeaker.MAUI.ViewModels;

namespace HomeSpeaker.MAUI.Views;

public partial class YouTubePage : ContentPage
{
	public YouTubePage(YouTubeViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}