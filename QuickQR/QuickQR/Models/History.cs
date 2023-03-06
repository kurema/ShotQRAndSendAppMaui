using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

	[JsonIgnore]
	[XmlIgnore]
	public ValueType ValueType
	{
		get
		{
			var text = BarcodeResult?.Value;
			if (text is null) return ValueType.Text;
			if (BigInteger.TryParse(text, out _)) return ValueType.Number;
			if (Uri.TryCreate(text, UriKind.Absolute, out _)) return ValueType.Uri;
			return ValueType.Text;
		}
	}
}

public enum ValueType
{
	Text, Number, Uri,
}

public record Histories
{
	public ObservableCollection<History> Items { get; } = new();
	//DateTimeOffset LastUpdated { get; init; } = DateTimeOffset.MinValue;
}
