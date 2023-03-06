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

	private static SemaphoreSlim SemaphoreHistory = new(1, 1);

	public ApplicationValues(string? profileName = null)
	{
		ProfileName = profileName;
	}

	public string? ProfileName { get; init; }

	public async Task LoadValues()
	{
		await LoadHistories();
	}

	public async Task LoadHistories()
	{
		Current.Histories = await LoadValue<Histories>(ProfileName, "histories.xml", SemaphoreHistory) ?? new Histories();
	}

	public int MaxHistoryCount { get => 100; }

	public async Task<IEnumerable<bool>> AddHistory(params ZXing.Net.Maui.BarcodeResult[] results)
	{
		if (Current.Histories is null) await LoadHistories();
		if (Current.Histories is null) return results.Select(_ => false);

		var resultList = results.ToList();
		var today = DateTimeOffset.UtcNow.Date;
		foreach (var item in Current.Histories.Items)
		{
			//if (item.LastUpdated.Date != today) break;
			foreach (var item2 in resultList) if (item.ResultEqual(item2)) resultList.Remove(item2);
		}

		{
			var excessLength = Current.Histories.Items.Count + resultList.Count - Current.MaxHistoryCount;
			while (excessLength > 0)
			{
				Current.Histories.Items.RemoveAt(Current.Histories.Items.Count - 1);
				excessLength--;
			}
		}

		try
		{
			foreach (var item in results.Select(a => new History(a, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow)))
			{
				//Current.Histories.Items.Add(item);
			}
		}
		catch (Exception e)
		{
		}
		return results.Select(a => resultList.Contains(a));
	}

	private static async Task<T?> LoadValue<T>(string? profileName, string filename, SemaphoreSlim semaphore) where T : class
	{
		T? result = default;
		var dirPath = (profileName is null) ? FileSystem.Current.AppDataDirectory : Path.Combine(FileSystem.Current.AppDataDirectory, profileName);
		if (!Directory.Exists(dirPath)) try { Directory.CreateDirectory(dirPath); } catch { return null; }
		var path = Path.Combine(dirPath, filename);
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
