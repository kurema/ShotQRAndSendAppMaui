using QuickQR.Models;

namespace QuickQR.Pages;

public partial class CameraPage : ContentPage
{
	public string LastReadText { get => (string)GetValue(LastReadTextProprty); private set => SetValue(LastReadTextProprty, value); }

	public static readonly BindableProperty LastReadTextProprty = BindableProperty.CreateAttached(nameof(LastReadText), typeof(string), typeof(CameraPage), string.Empty);

	//public System.Collections.ObjectModel.ObservableCollection<string> ReadTexts { get; } = new();


	public CameraPage()
	{
		InitializeComponent();
	}

	private SemaphoreSlim SemaphoreShowResult = new(1, 1);

	private async void cameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
	{
		if (e.Results.Length <= 0) return;
		_ = ApplicationValues.Current.AddHistory(e.Results);

		await this.Dispatcher.DispatchAsync(async () =>
		{
			var newText = e.Results.Last().Value;
			if (LastReadText == newText) return;
			await SemaphoreShowResult.WaitAsync();
			try
			{
				if (LastReadText == newText) return;
				LastReadText = newText;
				notificationBorder.IsVisible = true;
				await Task.Delay(3000);
				notificationBorder.IsVisible = false;
			}
			finally
			{
				SemaphoreShowResult.Release();
			}
		});
	}
}