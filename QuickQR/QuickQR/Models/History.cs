using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickQR.Models;

public record History(ZXing.Net.Maui.BarcodeResult BarcodeResult, DateTimeOffset Date);

public record Histories
{
	public List<History> Items { get; init; } = new List<History>();
	//DateTimeOffset LastUpdated { get; init; } = DateTimeOffset.MinValue;
}
