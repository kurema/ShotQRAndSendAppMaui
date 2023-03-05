using QuickQR.Models;
using ZXing.Net.Maui;

namespace QuickQR.Pages;

public partial class CameraPage : ContentPage
{
	//public string LastReadText { get => (string)GetValue(LastReadTextProprty); private set => SetValue(LastReadTextProprty, value); }

	//public static readonly BindableProperty LastReadTextProprty = BindableProperty.CreateAttached(nameof(LastReadText), typeof(string), typeof(CameraPage), string.Empty);

	public System.Collections.ObjectModel.ObservableCollection<Models.History> LastestHistories { get; } = new();


	public CameraPage()
	{
		InitializeComponent();

		cameraBarcodeReaderView.Options = new BarcodeReaderOptions()
		{
			//To specify all formats:
			//(BarcodeFormat) int.MaxValue
			//But it brings too many noises.
			Formats = BarcodeFormat.QrCode | BarcodeFormat.DataMatrix | BarcodeFormat.Pdf417 | BarcodeFormat.Aztec | BarcodeFormat.Ean13 | BarcodeFormat.Ean8 | BarcodeFormat.UpcE | BarcodeFormat.UpcA | BarcodeFormat.Code128 | BarcodeFormat.Code93 | BarcodeFormat.Code39 | BarcodeFormat.Itf,
			Multiple = true,
		};
	}


	private async void cameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
	{
		if (e.Results.Length <= 0) return;
		await ApplicationValues.Current.AddHistory(e.Results);

		await this.Dispatcher.DispatchAsync(async () =>
		{
			try
			{
				List<History> appenddedItems = new();
				for (int i = 0; i < e.Results.Length; i++)
				{
					if (LastestHistories.Any(a => a.ResultEqual(e.Results[i]))) continue;
					//if (e.Results[i].Format is not BarcodeFormat.QrCode && e.Results[i].Value.Length <= 2) continue;
					var result = new History(e.Results[i], DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
					appenddedItems.Add(result);
					LastestHistories.Add(result);
				}
				await Task.Delay(3000);
				foreach (var item in appenddedItems)
				{
					LastestHistories.Remove(item);
				}
			}
			catch
			{
			}
		});
	}

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		var history = (sender as BindableObject)?.BindingContext as History;
		//Note:
		//How to pass object. Basic.
		//https://www.youtube.com/watch?v=8z8qz-PePlc
		//https://learn.microsoft.com/ja-jp/xamarin/xamarin-forms/app-fundamentals/shell/navigation
		await Shell.Current.GoToAsync(new ShellNavigationState(nameof(ResultPage)), new Dictionary<string, object>()
		{
			{ nameof(ResultPage.Item),history }
		});
	}

	protected override void OnAppearing()
	{
		try
		{
			//https://github.com/Redth/ZXing.Net.Maui/issues/67
			cameraBarcodeReaderView.CameraLocation = CameraLocation.Front;
			cameraBarcodeReaderView.CameraLocation = CameraLocation.Rear;
		}
		catch { }
		base.OnAppearing();
	}

	private void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		cameraBarcodeReaderView.IsTorchOn = !cameraBarcodeReaderView.IsTorchOn;
		//Binding do not work somehow.
		//https://github.com/dotnet/maui/issues/10186
		if ((sender is ToolbarItem tbi))
		{
			tbi.IconImageSource = new FontImageSource()
			{
				FontFamily = "MaterialIconsR",
				Glyph = cameraBarcodeReaderView.IsTorchOn ? "\ue3e7" : "\ue3e6",
			};
		}
	}
}