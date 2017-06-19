
using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace MultiThreading.Console._3
{
	public class Mat
	{
		public int[][] Struct { get; private set; }

		public Mat(int[][] array)
		{
			this.Struct = array;
		}

		public Mat Multiply(Mat matrix)
		{
			int length = matrix.Struct.Length;
			int[][] result = new int[length][];
			for (int i = 0; i < length; i++)
			{
				result[i] = new int[length];
			}

			for (int k = 0; k < length; k++)
			{
				for (int i = 0; i < length; i++)
				{
					for (int j = 0; j < length; j++)
					{
						result[j][i] += this.Struct[k][i] * matrix.Struct[j][k];
					}
				}
			}

			return new Mat(result);
		}

		public Mat MultiplyP(Mat matrix)
		{
			int length = matrix.Struct.Length;
			int[][] result = new int[length][];
			for (int i = 0; i < length; i++)
			{
				result[i] = new int[length];
			}

			Parallel.For(0, length, new ParallelOptions { MaxDegreeOfParallelism = 8 }, k =>
			{
				for (int i = 0; i < length; i++)
				{
					for (int j = 0; j < length; j++)
					{
						result[j][i] += this.Struct[k][i] * matrix.Struct[j][k];
					}
				}
			});

			return new Mat(result);
		}

		public Mat MultiplyPNoOpts(Mat matrix)
		{
			int length = matrix.Struct.Length;
			int[][] result = new int[length][];
			for (int i = 0; i < length; i++)
			{
				result[i] = new int[length];
			}

			Parallel.For(0, length, k =>
			{
				for (int i = 0; i < length; i++)
				{
					for (int j = 0; j < length; j++)
					{
						result[j][i] += this.Struct[k][i] * matrix.Struct[j][k];
					}
				}
			});

			return new Mat(result);
		}

		public static int[][] GenerateArray(int length)
		{
			Random rnd = new Random(Environment.TickCount);
			int[][] array = new int[length][];
			for (int i = 0; i < length; i++)
			{
				array[i] = new int[length];
			}

			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					array[i][j] = rnd.Next();
				}
			}

			return array;
		}
	}

	public class MatrixBenchmark
	{
		private Mat m1;
		private Mat m2;

		public MatrixBenchmark()
		{
			int[][] array1 = Mat.GenerateArray(500);
			int[][] array2 = Mat.GenerateArray(500);

			this.m1 = new Mat(array1);
			this.m2 = new Mat(array2);
		}

		[Benchmark]
		public Mat Mul() => this.m1.Multiply(m2);

		[Benchmark]
		public Mat MulP() => this.m1.MultiplyP(m2);

		[Benchmark]
		public Mat MulPNoOpts() => this.m1.MultiplyPNoOpts(m2);
	}
}
