using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable

namespace QuickQR.Models;

public class ApplicationValues
{
	public static ApplicationValues Current = new ApplicationValues();

	public Histories? Histories { get; private set; } = null;

	private static System.Threading.SemaphoreSlim SemaphoreHistory = new(1, 1);

	public static async Task LoadValues()
	{
		await LoadHistories();
	}

	public static async Task LoadHistories()
	{
		Current.Histories = await LoadValue<Histories>("histories.xml", SemaphoreHistory);
	}

	private static async Task<T?> LoadValue<T>(string filename, SemaphoreSlim semaphore) where T : class
	{
		T? result = default;
		var path = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
		if (!File.Exists(path)) { Current.Histories = new(); return result; }
		using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
		var xs = new System.Xml.Serialization.XmlSerializer(typeof(Histories));
		await semaphore.WaitAsync();
		try
		{
			await Task.Run(() =>
			{
				try
				{
					result = xs.Deserialize(stream) as T;
				}
				catch
				{
					result = default;
				}
			});
		}
		finally
		{
			semaphore.Release();
		}
		return result;
	}
}
