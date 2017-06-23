using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DownloadManager
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string URLStarter = "http://";
		private static object _lcoker = new object();

		private HttpClient _httpClient;
		public MainWindow()
		{
			InitializeComponent();
			this._httpClient = new HttpClient();
		}

		private async void Button_Click(object sender, RoutedEventArgs e)
		{
			TaskScheduler uiContext = TaskScheduler.FromCurrentSynchronizationContext();
			TextRange textRange = new TextRange(this.URLTextBox.Document.ContentStart, this.URLTextBox.Document.ContentEnd);
			string allUrls = textRange.Text;
			//IList<string> urlList = await ProcessUrlsAsync(allUrls);
			IList<string> urlList = Enumerable.Range(1, 20).Select(val => "http://publications.lib.chalmers.se/records/fulltext/223154/223154.pdf").ToList();

			await DownloadAndRenderAsync(urlList, uiContext);
			//foreach (string url in urlList)
			//{
			//	try
			//	{
			//		string response = await DownloadAsync(url);
			//		await OnDownload(url, response.Length, uiContext);
			//	}
			//	catch (Exception exception)
			//	{
			//		await OnError(url, exception.Message, uiContext);
			//	}
			//}
		}

		private Task DownloadAndRenderAsync(IList<string> urls, TaskScheduler uiContext)
		{
			return Task.Factory.StartNew(() =>
			{
				Parallel.ForEach(urls, new ParallelOptions { MaxDegreeOfParallelism = 8 }, async url =>
				{
					try
					{
						string response = await DownloadAsync(url);
						await OnDownload(url, response.Length, uiContext);
					}
					catch (Exception exception)
					{
						await OnError(url, exception.Message, uiContext);
					}
				});
			});
		}

		private Task OnDownload(string url, int contentLength, TaskScheduler uiContext)
		{
			return Task.Factory.StartNew(() =>
			{
				this.PageDowloadResult.Text += $"Url \"{url}\" is downloaded. ContentLength - {contentLength} {Environment.NewLine}";


			}, CancellationToken.None, TaskCreationOptions.None, uiContext);
		}

		private Task OnError(string url, string errorMessage, TaskScheduler uiContext)
		{
			return Task.Factory.StartNew(() =>
			{
				this.PageDowloadResult.Text += $"Url \"{url}\" is not downloaded. Error - {errorMessage} {Environment.NewLine}";

			}, CancellationToken.None, TaskCreationOptions.None, uiContext);
		}

		public Task<List<string>> ProcessUrlsAsync(string urlString)
		{
			return Task.Factory.StartNew(() => urlString.Trim().Split(';').Select((u) =>
			{
				if (!u.StartsWith(URLStarter))
				{
					u = URLStarter + u;
				}
				return u;
			}).ToList());
		}

		public Task<string> DownloadAsync(string url)
		{
			Uri uri;
			try
			{
				uri = new Uri(url);
			}
			catch (UriFormatException e)
			{
				throw new UriFormatException($"{url} is not valid address.");
			}

			return this._httpClient.GetStringAsync(uri);
		}
	}
}
