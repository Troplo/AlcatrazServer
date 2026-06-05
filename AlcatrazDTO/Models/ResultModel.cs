using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alcatraz.DTO.Models
{
	public class ResultModel
	{
		public ResultModel()
		{
		}

		public ResultModel(string errMessage)
		{
			ErrorMessage = errMessage;
		}

		public ResultModel(Guid id, uint pid)
		{
			Success = true;
			Id = id;
			PlayerId = pid;
		}

		public bool Success { get; set; } = true;
		public Guid Id { get; set; } = Guid.Empty;
		public uint PlayerId { get; set; } = 0;
		private string _Message;
		public string ErrorMessage { 
			get { return _Message; } 
			set { _Message = value; Success = false; } 
		}
	}

	public class ResultModel<T> : ResultModel
	{
		public ResultModel()
		{
		}

		public ResultModel(string errMessage)
		{
			ErrorMessage = errMessage;
		}

		public ResultModel(T data)
		{
			Success = true;
			Data = data;
		}

		public T Data { get; set; }
	}
}
