using QuickQR.Models;

namespace QuickQR.Pages;

public partial class CameraPage : ContentPage
{
	//public string LastReadText { get => (string)GetValue(LastReadTextProprty); private set => SetValue(LastReadTextProprty, value); }

	//public static readonly BindableProperty LastReadTextProprty = BindableProperty.CreateAttached(nameof(LastReadText), typeof(string), typeof(CameraPage), string.Empty);

	public System.Collections.ObjectModel.ObservableCollection<Models.History> LastestHistories { get; } = new();


	public CameraPage()
	{
		InitializeComponent();
	}


	private async void cameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
	{
		if (e.Results.Length <= 0) return;
		(await ApplicationValues.Current.AddHistory(e.Results)).ToArray();

		await this.Dispatcher.DispatchAsync(async () =>
		{
			try
			{
				List<History> appenddedItems = new();
				for (int i = 0; i < e.Results.Length; i++)
				{
					if (LastestHistories.Any(a => a.BarcodeResult.Value == e.Results[i].Value)) continue;
					var result = new History(e.Results[i], DateTimeOffset.UtcNow);
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
		//https://www.youtube.com/watch?v=8z8qz-PePlc
		await Shell.Current.GoToAsync(new ShellNavigationState(nameof(ResultPage)), new Dictionary<string, object>()
		{
			{nameof(history.Date),history.Date},
			{nameof(history.BarcodeResult),history.BarcodeResult }
		});
	}
}