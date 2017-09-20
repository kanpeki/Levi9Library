namespace Levi9Library.Core
{
	using System;

	public class Result
	{
		public bool IsSuccess { get; }
		public string Error { get; }
		public bool IsFailure => !IsSuccess;

		protected Result(bool isSucces, string error)
		{
			if (isSucces && error != string.Empty)
				throw new InvalidOperationException();
			if (!isSucces && error == string.Empty)
				throw new InvalidOperationException();

			IsSuccess = isSucces;
			Error = error;
		}

		public static Result Fail(string message)
		{
			return new Result(false, message);
		}

		public static Result<T> Fail<T>(string message)
		{
			return new Result<T>(default(T), false, message);
		}

		public static Result Ok()
		{
			return new Result(true, string.Empty);
		}

		public static Result<T> Ok<T>(T value)
		{
			return new Result<T>(value, true, string.Empty);
		}
	}

	public class Result<T> : Result
	{
		private readonly T _value;

		public T Value
		{
			get
			{
				if (!IsSuccess)
					throw new InvalidOperationException();

				return _value;
			}
		}

		protected internal Result(T value, bool isSucces, string error)
			: base(isSucces, error)
		{
			_value = value;
		}
	}
}