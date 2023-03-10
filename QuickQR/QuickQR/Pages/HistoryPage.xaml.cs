using QuickQR.Models;

namespace QuickQR.Pages;

[QueryProperty(nameof(Item), nameof(Item))]
public partial class HistoryPage : ContentPage
{
	public HistoryPage()
	{
		InitializeComponent();

		Item = Models.ApplicationValues.Current?.Histories;
	}

	public Histories Item
	{
		set
		{
			this.BindingContext = value;
		}
	}

	private async void ViewCell_Tapped(object sender, EventArgs e)
	{
		if ((sender as BindableObject)?.BindingContext is not History h) return;
		await Shell.Current.GoToAsync(new ShellNavigationState(nameof(ResultPage)), new Dictionary<string, object>()
		{
			{ nameof(ResultPage.Item),h }
		});
	}
}