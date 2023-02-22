using QuickQR.Models;

namespace QuickQR.Pages;

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
}