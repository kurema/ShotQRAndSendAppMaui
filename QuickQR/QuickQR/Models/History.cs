using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQR.Models;

[QueryProperty(nameof(BarcodeResult), nameof(BarcodeResult))]
[QueryProperty(nameof(LastUpdated), nameof(LastUpdated))]
[QueryProperty(nameof(Created), nameof(Created))]

public record History(ZXing.Net.Maui.BarcodeResult BarcodeResult, DateTimeOffset LastUpdated, DateTimeOffset Created)
{
	public bool ResultEqual(History other)
	{
		return ResultEqual(other.BarcodeResult);
	}

	public bool ResultEqual(ZXing.Net.Maui.BarcodeResult other)
	{
		return BarcodeResult?.Value == other?.Value && BarcodeResult?.Format == other?.Format;
	}
}

public record Histories
{
	public List<History> Items { get; init; } = new List<History>();
	//DateTimeOffset LastUpdated { get; init; } = DateTimeOffset.MinValue;
}
