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

	private void ViewCell_Tapped(object sender, EventArgs e)
	{

	}
}