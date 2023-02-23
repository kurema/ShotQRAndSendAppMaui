using QuickQR.Models;

namespace QuickQR.Pages;

[QueryProperty(nameof(Item), nameof(Item))]
public partial class ResultPage : ContentPage
{
	public ResultPage()
	{
		InitializeComponent();
	}

	public ResultPage(History vm) : this()
	{
		this.BindingContext = vm;
	}

	public History Item
	{
		set
		{
			this.BindingContext = value;
		}
	}
}