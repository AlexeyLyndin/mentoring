using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AsyncCounter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private CancellationTokenSource cts;
		private CancellationTokenSource ctsPrev;

		private int _prevValue;
		public MainWindow()
		{
			InitializeComponent();

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			cts = new CancellationTokenSource();
			CancellationToken token = cts.Token;
			int insertedValue;

			try
			{
				insertedValue = Convert.ToInt32(this.NumberTextBox.Text);
			}
			catch (FormatException exception)
			{
				this.TextBlock.Text += "Wrong Number!!!";
				return;
			}

			ctsPrev?.Cancel();

			IProgress<int> prg = new Progress<int>(percent =>
			{
				this.PercentProgress.Content = $"{percent} %";
			});

			Task<long> counterTask = this.GetStringAsync(insertedValue, token, prg);

			TaskScheduler uiContext = TaskScheduler.FromCurrentSynchronizationContext();

			counterTask.ContinueWith(this.OnRanToCompletion, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, uiContext);
			counterTask.ContinueWith(this.OnCanceled, CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, uiContext);

			ctsPrev = cts;
		}

		private void OnRanToCompletion(Task<long> ant)
		{
			this.ResultLabel.Content = ant.Result;
		}

		private void OnCanceled(Task<long> ant)
		{
			this.TextBlock.Text += $"Canceled {Environment.NewLine}";
		}

		private Task<long> GetStringAsync(int upperBound, CancellationToken token, IProgress<int> prg)
		{
			return Task<long>.Factory.StartNew(() =>
			{
				long result = this.CountSomethingLong(upperBound, token, prg);
				return result;
			}, token);
		}

		private long CountSomethingLong(int upperBound, CancellationToken token, IProgress<int> prg)
		{
			long counter = 0;
			for (int i = 1; i <= upperBound; i++)
			{
				prg.Report(i * 100 / upperBound);
				token.ThrowIfCancellationRequested();
				counter += i;
				Thread.Sleep(100);
			}

			return counter;
		}
	}
}
