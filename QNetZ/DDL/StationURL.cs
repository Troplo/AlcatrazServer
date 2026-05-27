using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QNetZ.DDL
{
	public class StationURL : IAnyData
	{
		public string _urlString;   // cached string

		public string _urlScheme;
		public string _address;
		private Dictionary<string, string> _parameters;
		private bool _dirty;

		public StationURL()
		{
			_urlScheme = "";
			_address = "";
			_parameters = new Dictionary<string, string>();
			_dirty = false;

			Valid = false;
		}

		public StationURL(string urlStringText) : this()
		{
			ParseStationUrl(urlStringText);
		}

		public StationURL(string scheme, string address, IDictionary<string, int> parameters) : this()
		{
			_urlScheme = scheme;
			_address = address;

			if (parameters != null)
			{
				foreach (var key in parameters.Keys)
				{
					_parameters.TryAdd(key, parameters[key].ToString());
				}
			}
			_dirty = true;
		}

		public StationURL(string scheme, string address, IDictionary<string, string> parameters) : this()
		{
			_urlScheme = scheme;
			_address = address;

			if (parameters != null)
			{
				foreach (var key in parameters.Keys)
				{
					_parameters.TryAdd(key, parameters[key]);
				}
			}
			_dirty = true;
		}

		public string urlString {
			get
			{
				BuildUrlString();
				return _urlString;
			}
			set
			{

				ParseStationUrl(value);
			}
		}

		public bool Valid { get; private set; }

		public int this[string key]
		{
			get
			{
				if (_parameters.TryGetValue(key, out var strVal) && int.TryParse(strVal, out var intVal))
					return intVal;
				return 0;
			}
			set
			{
				_parameters[key] = value.ToString();
				_dirty = true;
			}
		}

		public string UrlScheme { get => _urlScheme; set { _urlScheme = value; _dirty = true; } }  // "prudp" or "prudps"

		public string Address { get => _address; set { _address = value; _dirty = true; } }
		public Dictionary<string, string> Parameters { get => _parameters; set { _parameters = value; _dirty = true; } }

		public bool IsPublic {
			get {
				if (_parameters.TryGetValue("type", out var typeStr) && int.TryParse(typeStr, out var type))
					return (type & 2) != 0; 
				return false;
			}
		}
		public bool IsBehindNAT {
			get {
				if (_parameters.TryGetValue("type", out var typeStr) && int.TryParse(typeStr, out var type))
					return (type & 1) != 0; 
				return false;
			}
		}
		public bool IsGlobal { get => IsPublic && IsBehindNAT; }

		void BuildUrlString()
		{
			if (!_dirty)
				return;

			_dirty = false;
			Valid = false;

			// check the data
			if (string.IsNullOrWhiteSpace(_urlScheme))
			{
				_urlString = "";
				return;
			}

			if (string.IsNullOrWhiteSpace(_address))
			{
				_urlString = $"{ _urlScheme }:/";
				return;
			}

			var paramsString = string.Join(";", _parameters.Keys.Select(x => $"{x}={_parameters[x]}"));

			var strSep = paramsString.Length > 0 ? ";" : "";

			_urlString = $"{ _urlScheme }:/address={ _address }{strSep}{ paramsString }";
			Valid = true;
		}

		public void ParseStationUrl(string newUrlValue)
		{
			_dirty = false;
			Valid = false;

			var urlParts = newUrlValue.Split(new[] { ":/" }, 2, StringSplitOptions.None);
			if (urlParts.Length != 2)
				return;

			_urlScheme = urlParts[0];

			var paramString = urlParts[1].Replace('#', ';');
			var parameterList = paramString.Split(';');
			foreach(var param in parameterList)
			{
				if (string.IsNullOrEmpty(param)) continue;

				var key_value = param.Split(new[] { '=' }, 2);
				if (key_value.Length != 2)
					continue;

				if (key_value[0] == "address")
					_address = key_value[1];
				else
					_parameters.TryAdd(key_value[0], key_value[1]);
			}

			_urlString = newUrlValue;
			Valid = true;
		}

		public override string ToString()
		{
			return urlString;
		}

		public void Read(Stream s)
		{
			string value = Helper.ReadString(s);
			ParseStationUrl(value);
		}

		public void Write(Stream s)
		{
			Helper.WriteString(s, urlString);
		}

		public bool Compare(StationURL otherUrl, IEnumerable<string> compareParameters = null)
		{
			if(otherUrl == null)
				return false;

			if (_urlScheme != otherUrl._urlScheme)
				return false;

			if (_address != otherUrl._address)
				return false;

			if (compareParameters == null)
				return true;

			foreach (var paramName in compareParameters)
			{
				if(_parameters.TryGetValue(paramName, out var param) && otherUrl._parameters.TryGetValue(paramName, out var otherParam))
				{
					if (param != otherParam)
						return false;
				}
			}
			return true;
		}
	}
}
