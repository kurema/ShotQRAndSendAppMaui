using QuickQR.Models;

namespace QuickQR.Pages;

public partial class CameraPage : ContentPage
{
	public CameraPage()
	{
		InitializeComponent();
	}

	private async void cameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
	{
		_ = ApplicationValues.Current.AddHistory(e.Results);

		notificationBorder.IsVisible = true;
		await Task.Delay(3000);
		notificationBorder.IsVisible = false;
	}
}