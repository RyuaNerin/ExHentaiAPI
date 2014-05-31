//////////////////////////////////////////////////////////////////////////
// JSON Toolkit 4.1.736
// Released: Jul 1, 2013
// http://jsontoolkit.codeplex.com/
//////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Globalization;

namespace ComputerBeacon.Json
{
	/// <summary>
	/// JsonArray class
	/// </summary>
	public class JsonArray : IJsonContainer, IList<object>
	{
		List<object> objects;

		#region Constructor
		/// <summary>
		/// Creates an empty JsonArray
		/// </summary>
		public JsonArray()
		{
			objects = new List<object>();
		}

		/// <summary>
		/// Create a new JsonArray
		/// </summary>
		/// <param name="jsonString">JSON string that represents an array</param>
		/// <exception cref="FormatException">JsonString represents JsonObject instead of JsonArray</exception>
		public JsonArray(string jsonString)
		{
			JsonArray ja = Parser.Parse(jsonString) as JsonArray;
			if (ja == null) throw new FormatException("JsonString represents JsonObject instead of JsonArray");
			this.objects = ja.objects;
		}

		/// <summary>
		/// Creates a JsonArray with initial values
		/// </summary>
		/// <param name="values">Array of initial values</param>
		public JsonArray(object[] values)
			: this()
		{
			foreach (var item in values) this.Add(item);
		}
		#endregion


		#region IJsonContainer
		void IJsonContainer.InternalAdd(string key, object value)
		{
			objects.Add(value);
		}
		bool IJsonContainer.IsArray { get { return true; } }
		#endregion


		#region Indexer
		/// <summary>
		/// Gets the object located at the specified index in the JsonArray
		/// </summary>
		/// <param name="index">Index of object</param>
		/// <returns></returns>
		public object this[int index]
		{
			get
			{
				return objects[index];
			}
			set
			{
				Helper.CheckValidType(value);
				objects[index] = value;
			}
		}

		#endregion


		#region Interface
		/// <summary>
		/// The number of objects contained in this JsonArray
		/// </summary>
		public int Count { get { return objects.Count; } }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return objects.GetEnumerator();
		}
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			return objects.GetEnumerator();
		}

		void ICollection<object>.CopyTo(object[] array, int arrayIndex)
		{
			objects.CopyTo(array, arrayIndex);
		}


		bool ICollection<object>.IsReadOnly
		{
			get { return false; }
		}



		/// <summary>
		/// Adds an item to the JsonArray
		/// </summary>
		/// <param name="item">Item to be added</param>
		/// <returns>Index of the added item</returns>
		public void Add(object item)
		{
			Helper.CheckValidType(item);
			objects.Add(item);
		}

		/// <summary>
		/// Removes all items in the JsonArray
		/// </summary>
		public void Clear()
		{
			objects.Clear();
		}

		/// <summary>
		/// Determines whether the JsonArray contains a specific value
		/// </summary>
		/// <param name="item">Value to be checked</param>
		/// <returns>True if the specified value is found in the JsonArray, otherwise False</returns>
		public bool Contains(object item)
		{
			return objects.Contains(item);
		}

		/// <summary>
		/// Determines the index of the first occurrence of a specific value
		/// </summary>
		/// <param name="item">Value to be checked</param>
		/// <returns>Index of the first occurrence of the specified value, -1 if the value is not found</returns>
		public int IndexOf(object item)
		{
			return objects.IndexOf(item);
		}

		/// <summary>
		/// Inserts an item to the JsonArray at the specified index
		/// </summary>
		/// <param name="index">Index of item to be inserted</param>
		/// <param name="item">Value of item to be inserted</param>
		public void Insert(int index, object item)
		{
			Helper.CheckValidType(item);
			objects.Insert(index, item);
		}

		/// <summary>
		/// Removes the first occurrence of a specified value from the JsonArray
		/// </summary>
		/// <param name="item">Value to be removed</param>
		public bool Remove(object item)
		{
			return objects.Remove(item);
		}

		/// <summary>
		/// Removes the item at the specified index
		/// </summary>
		/// <param name="index">Index of item to be removed</param>
		public void RemoveAt(int index)
		{
			objects.RemoveAt(index);
		}

		#endregion


		#region ToString

		/// <summary>
		/// Returns the shortest string representation of the current JsonArray
		/// </summary>
		/// <returns>A string representation of the current JsonArray</returns>
		public override string ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// Returns the string representation of the current JsonArray
		/// </summary>
		/// <param name="niceFormat">Whether the string is formatted for easy reading</param>
		/// <returns>A string representation of the current JsonArray</returns>
		public string ToString(bool niceFormat)
		{
			CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;


			StringBuilder sb = new StringBuilder();
			Stringifier.stringify(this, sb, 0, niceFormat);

			System.Threading.Thread.CurrentThread.CurrentCulture = culture;

			return sb.ToString();
		}
		#endregion
	}
	/// <summary>
	/// JsonObject class
	/// </summary>
	public class JsonObject : IJsonContainer, IDictionary<string, object>
	{
		/// <summary>
		/// The properties of this JSON Object
		/// </summary>

		Dictionary<string, object> entries;


		#region Constructor
		/// <summary>
		/// Creates an empty JsonObject
		/// </summary>
		public JsonObject()
		{
			entries = new Dictionary<string, object>();
		}

		/// <summary>
		/// Create a new JsonObject from a string
		/// </summary>
		/// <param name="jsonString">JSON string that represents an object</param>
		/// <exception cref="FormatException">JsonString represents JsonArray instead of JsonObject</exception>
		public JsonObject(string jsonString)
		{
			JsonObject jo = Parser.Parse(jsonString) as JsonObject;
			if (jo == null) throw new FormatException("JsonString represents JsonArray instead of JsonObject");
			this.entries = jo.entries;
		}

		/// <summary>
		/// Creates a JsonObject with initial string
		/// </summary>
		/// <param name="values">Annoymous type containing initial values</param>
		public JsonObject(object values)
			: this()
		{
			foreach (var p in values.GetType().GetProperties())
			{
				if (!p.CanRead) continue;
				this.Add(p.Name, p.GetValue(values, null));
			}
		}
		#endregion



		#region IJsonContainer
		void IJsonContainer.InternalAdd(string key, object value)
		{
			entries.Add(key, value);
		}
		bool IJsonContainer.IsArray { get { return false; } }
		#endregion


		#region Indexer
		/// <summary>
		/// Gets a property of the current JSON Object by key
		/// </summary>
		/// <param name="key">Key of property</param>
		/// <returns>Value of property. Returns null if property is not found.</returns>
		public object this[string key]
		{
			get
			{
				if (entries.ContainsKey(key)) return entries[key];
				return null;
			}
			set
			{
				Helper.CheckValidType(value);
				entries[key] = value;
			}
		}
		#endregion


		#region Interface
		/// <summary>
		/// The number of key/value pairs contained in the JsonObject
		/// </summary>
		public int Count { get { return entries.Count; } }
		/// <summary>
		/// Whether the JsonObject is read-only. This value is always true.
		/// </summary>
		public bool IsReadOnly { get { return false; } }
		/// <summary>
		/// All the keys in the JsonObject
		/// </summary>
		public ICollection<string> Keys { get { return entries.Keys; } }
		/// <summary>
		/// All the values in the JsonObject
		/// </summary>
		public ICollection<object> Values { get { return entries.Values; } }
		/// <summary>
		/// Adds the specified key and value to the JsonObject.
		/// </summary>
		/// <param name="item"></param>
		void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			Helper.CheckValidType(item.Value);
			Add(item.Key, item.Value);
		}
		/// <summary>
		/// Adds the specified key and value to the JsonObject.
		/// </summary>
		/// <param name="key">Key of entry</param>
		/// <param name="value">Value of entry</param>
		public void Add(string key, object value)
		{
			Helper.CheckValidType(value);
			entries.Add(key, value);
		}
		/// <summary>
		/// Removes all keys and values from the JsonObject.
		/// </summary>
		public void Clear() { entries.Clear(); }

		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) { throw new NotImplementedException(); }
		/// <summary>
		/// Removes the item with the specified key from the JsonObject.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Remove(string key) { return entries.Remove(key); }
		/// <summary>
		/// Copy all the entries to an array, starting at a particular array index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			int i = 0;
			foreach (KeyValuePair<string, object> KVP in entries)
			{
				array[arrayIndex + (i++)] = KVP;
			}
		}
		/// <summary>
		/// Determines whether the JsonObject contains the specified key/value pair.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(KeyValuePair<string, object> item)
		{
			return entries.ContainsKey(item.Key) && entries[item.Key].Equals(item.Value);
		}
		/// <summary>
		/// Determines whether the JsonObject contains the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool ContainsKey(string key)
		{
			return entries.ContainsKey(key);
		}
		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(string key, out object value)
		{
			return entries.TryGetValue(key, out value);
		}

		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		{
			return entries.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return entries.GetEnumerator();
		}
		#endregion


		#region ToString
		/// <summary>
		/// Returns the shortest string representation of the current JsonObject
		/// </summary>
		/// <returns>A string</returns>
		public override string ToString()
		{
			return ToString(false);
		}

		/// <summary>
		/// Returns the string representation of the current JsonObject
		/// </summary>
		/// <param name="niceFormat">Whether the string is formatted for easy reading</param>
		/// <returns>A string representation of the current JsonObject</returns>
		public string ToString(bool niceFormat)
		{
			CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
			System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

			StringBuilder sb = new StringBuilder();
			Stringifier.stringify(this, sb, 0, niceFormat);

			System.Threading.Thread.CurrentThread.CurrentCulture = culture;

			return sb.ToString();
		}

		#endregion
	}

	/// <summary>
	/// A class for parsing JSON syntax strings
	/// </summary>
	public static class Parser
	{
		/// <summary>
		/// Parse a JSON string into a JsonObject or JsonArray instance
		/// </summary>
		/// <param name="s">JSON string</param>
		/// <returns>a JsonObject or JsonArray instance, depending on the input string</returns>
		/// <exception cref="FormatException">The string contains invalid JSON syntax.</exception>
		public static object Parse(string s)
		{
			Stack<IJsonContainer> stack = new Stack<IJsonContainer>();
			object root = null;

			StringBuilder sb = new StringBuilder();
			string key = null;
			bool aftercomma = false;

			short state = 0;
			int length = s.Length;
			char c;
			uint hexvalue;
			int i = 0;

			int strStart = -1;
			int strLength = 0;
			do
			{
				c = s[i];

				switch (state)
				{

					#region ReadChar
					case 4:
						switch (c)
						{
							case '"':
								if (strLength > 0)
								{
									sb.Append(s, strStart, strLength);
									strLength = 0;
								}
								strStart = -1;
								if (!stack.Peek().IsArray && key == null)
								{
									if (sb.Length == 0) throw MakeException(s, i, "Key in JSON object cannot be empty string");
									state = 7;
								}
								else
								{
									stack.Peek().InternalAdd(key, sb.ToString());
									key = null;
									sb.Length = 0;
									state = 8;

								}
								continue;
							case '\\':
								if (strLength > 0)
								{
									sb.Append(s, strStart, strLength);
									strLength = 0;
								}
								strStart = -1;
								state = 5;
								continue;
							default:
								++strLength;
								continue;
						}
					#endregion

					#region WaitingValue
					case 1:
						if (c == ' ' || c == '\n' || c == '\r' || c == '\t') continue;
						if (c == '"')
						{
							strStart = i + 1;
							state = 4; continue;
						}
						if (c == '{')
						{
							aftercomma = false;
							var jo = new JsonObject();
							stack.Peek().InternalAdd(key, jo);
							stack.Push(jo);
							key = null;

							state = 3; continue;
						}
						if (c == '[')
						{
							aftercomma = false;
							var ja = new JsonArray();
							stack.Peek().InternalAdd(key, ja);
							stack.Push(ja);
							key = null;

							continue;
						}
						if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '-')
						{
							sb.Append(c);
							state = 2; continue;
						}
						if (!aftercomma && c == ']')
						{
							if (!stack.Peek().IsArray) throw MakeException(s, i, "Invalid ']' character");
							stack.Pop();
							if (stack.Count > 0)
							{
								state = 8; continue;
							}
							state = 9; continue;
						}
						throw MakeException(s, i, "Unknown value expression.");
					#endregion

					#region ReadValue
					case 2:
						if (c == ' ' || c == '\n' || c == '\r' || c == '\t') continue;
						if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '.' || c == '+')
						{
							sb.Append(c);
							continue;
						}
						if (c == ',')
						{
							aftercomma = true;
							stack.Peek().InternalAdd(key, ParseJsonValue(sb.ToString()));
							key = null;
							sb.Length = 0;
							if (stack.Peek().IsArray)
							{
								state = 1; continue;
							}
							else
							{
								state = 3; continue;
							}
						}
						if (c == ']')
						{
							if (!stack.Peek().IsArray) throw MakeException(s, i, "Invalid ']' character");
							stack.Peek().InternalAdd(null, ParseJsonValue(sb.ToString()));
							stack.Pop();
							sb.Length = 0;
							if (stack.Count > 0)
							{
								state = 8; continue;
							}
							state = 9; continue;
						}
						if (c == '}')
						{
							if (stack.Peek().IsArray) throw MakeException(s, i, "Invalid '}' character");
							stack.Peek().InternalAdd(key, ParseJsonValue(sb.ToString()));
							stack.Pop();
							key = null;
							sb.Length = 0;
							if (stack.Count > 0)
							{
								state = 8; continue;
							}
							state = 9; continue;
						}
						throw MakeException(s, i, "Invalid character in non-string value");
					#endregion

					#region WaitBeginString
					case 3:
						switch (c)
						{
							case ' ':
							case '\n':
							case '\r':
							case '\t':
								continue;
							case '"':
								strStart = i + 1;
								state = 4;
								continue;
							case '}':
								if (aftercomma) goto default;
								stack.Pop(); //waitbeginstring can only be entered by '{', therefore pop must be valid
								if (stack.Count == 0) state = 9;
								else state = 8;
								continue;
							default:
								throw MakeException(s, i, "Expected double quotation character to mark beginning of string");
						}
					#endregion

					#region ReadEscapedChar
					case 5:
						switch (c)
						{
							case ' ':
							case '\n':
							case '\r':
							case '\t': continue;
							case '\\':
								sb.Append('\\');
								strStart = i + 1;
								state = 4; continue;
							case '/':
								sb.Append('/');
								strStart = i + 1;
								state = 4; continue;
							case '"':
								sb.Append('"');
								strStart = i + 1;
								state = 4; continue;
							case 'n':
								sb.Append('\n');
								strStart = i + 1;
								state = 4; continue;
							case 'r':
								sb.Append('\r');
								strStart = i + 1;
								state = 4; continue;
							case 't':
								sb.Append('\t');
								strStart = i + 1;
								state = 4; continue;
							case 'u':
								if (i + 4 >= length) throw new FormatException("Incomplete JSON string");
								hexvalue = (CharToHex(s[i + 1]) << 12) | (CharToHex(s[i + 2]) << 8) | (CharToHex(s[i + 3]) << 4) | CharToHex(s[i + 4]);
								sb.Append((char)hexvalue);
								i += 4;
								strStart = i + 1;
								state = 4;
								continue;
							default:
								throw MakeException(s, i, "Unknown escaped character");
						}
					#endregion

					#region WaitColon
					case 7:
						switch (c)
						{
							case ' ':
							case '\n':
							case '\r':
							case '\t': continue;
							case ':':
								key = sb.ToString();
								sb.Length = 0;
								state = 1;
								continue;
							default:
								throw MakeException(s, i, "Expected colon(:) to seperate key and values in JSON object");
						}
					#endregion

					#region WaitClose
					case 8:
						switch (c)
						{
							case ' ':
							case '\n':
							case '\r':
							case '\t':
								continue;
							case ',':
								aftercomma = true;
								state = 1;
								continue;
							case ']':
								if (!stack.Peek().IsArray) throw MakeException(s, i, "Invalid ']' character");
								stack.Pop();
								if (stack.Count == 0) state = 9;
								continue;
							case '}':
								if (stack.Peek().IsArray) throw MakeException(s, i, "Invalid '}' character");
								stack.Pop();
								if (stack.Count == 0) state = 9;
								continue;
							default:
								throw MakeException(s, i, "Expect comma or close bracket after value");
						}
					#endregion

					#region Start
					case 0:
						switch (c)
						{
							case ' ':
							case '\n':
							case '\r':
							case '\t':
								continue;
							case '[':
								var ja = new JsonArray();
								root = ja;
								stack.Push(ja);
								state = 1; continue;
							case '{':
								var jo = new JsonObject();
								root = jo;
								stack.Push(jo);
								state = 3; continue;
							default:
								throw MakeException(s, i, "Expect '{' or '[' to begin JSON string");
						}
					#endregion

					#region End
					case 9:
						switch (c)
						{
							case ' ':
							case '\n':
							case '\r':
							case '\t':
								continue;
							default:
								throw MakeException(s, i, "Unexpected character(s) after termination of JSON string");
						}
					#endregion
				}
			} while (++i < length);
			if (state != 9) throw new FormatException("Incomplete JSON string");

			return root;
		}

		private static uint CharToHex(char c)
		{
			if (c >= '0' && c <= '9') return (uint)(c - '0');
			if (c >= 'a' && c <= 'f') return (uint)(c - 'a' + 10);
			if (c >= 'A' && c <= 'F') return (uint)(c - 'A' + 10);
			throw new FormatException(c + " is not a valid hex value");
		}

		private static object ParseJsonValue(string jsonString)
		{
			int result;
			if (int.TryParse(jsonString, NumberStyles.AllowLeadingSign, System.Globalization.NumberFormatInfo.InvariantInfo, out result)) return result;

			long result_long;
			if (long.TryParse(jsonString, NumberStyles.AllowLeadingSign, System.Globalization.NumberFormatInfo.InvariantInfo, out result_long)) return result_long;

			double result_double;
			if (double.TryParse(jsonString, NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
				System.Globalization.NumberFormatInfo.InvariantInfo,
				out result_double)) return result_double;

			if (jsonString == "true" || jsonString == "True") return true;
			if (jsonString == "false" || jsonString == "False") return false;
			if (jsonString == "null") return null;

			throw new FormatException(string.Format("Unknown JSON value: \"{0}\"", jsonString));
		}

		private static FormatException MakeException(string errorString, int position, string message)
		{
			int start = position - 5;
			if (start < 0) start = 0;
			int length = errorString.Length - position;
			if (length > 5) length = 5;
			length += 5;
			StringBuilder sb = new StringBuilder(message);
			sb.Append(" at character position " + position + ", near ");
			Helper.WriteString(sb, errorString.Substring(start, length));
			return new FormatException(sb.ToString());
		}
	}

	/// <summary>
	/// A class for serializing and deserializing JSON objects
	/// </summary>
	public static class Serializer
	{
		/// <summary>
		/// Serializes an object to its JSON representation
		/// </summary>
		/// <param name="o">object to be serialized</param>
		/// <returns>JSON string that represents the object</returns>
		public static string Serialize(object o)
		{
			CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
			System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

			StringBuilder sb = new StringBuilder();
			writeObject(sb, o);

			//restore culture
			System.Threading.Thread.CurrentThread.CurrentCulture = culture;

			return sb.ToString();
		}

		static void writeObject(StringBuilder sb, object o)
		{
			if (o == null)
			{
				sb.Append("null");
				return;
			}

			Type t = o.GetType();
			if (t.IsArray)
			{
				Array a = o as Array;
				sb.Append('[');
				for (int i = 0; i < a.Length; i++)
				{
					if (i != 0) sb.Append(',');
					writeObject(sb, a.GetValue(i));
				}
				sb.Append(']');
			}
			else if (o.GetType().IsGenericType && o.GetType().GetGenericTypeDefinition() == typeof(List<>))
			{
				sb.Append('[');
				bool firstObject = false;
				foreach (var item in o as System.Collections.IEnumerable)
				{
					if (firstObject) sb.Append(',');
					writeObject(sb, item);
					firstObject = true;
				}
				sb.Append(']');
			}
			else

				if (t.IsPrimitive)
				{
					if (t == typeof(bool)) sb.Append((bool)o ? "true" : "false");
					else sb.Append(o.ToString());
				}
				else if (t.IsEnum)
				{
					sb.Append(o.ToString());
				}
				else if (t == typeof(string)) Helper.WriteString(sb, o as string);
				else if (t.IsClass)
				{
					sb.Append('{');
					bool hasFirstValue = false;

					var fields = t.GetFields();
					for (int i = 0; i < fields.Length; i++)
					{
						if (hasFirstValue) sb.Append(',');
						Helper.WriteString(sb, fields[i].Name);
						sb.Append(':');
						writeObject(sb, fields[i].GetValue(o));
						hasFirstValue = true;
					}

					var properties = t.GetProperties();
					for (int i = 0; i < properties.Length; i++)
					{
						if (!properties[i].CanRead) continue;
						if (hasFirstValue) sb.Append(',');
						Helper.WriteString(sb, properties[i].Name);
						sb.Append(':');
						writeObject(sb, properties[i].GetValue(o, null));
						hasFirstValue = true;
					}

					sb.Append('}');
				}
				else throw new NotSupportedException("Cannot serialize type " + t.Name);
		}
	}

	class Stringifier
	{

		const string newline = "\r\n";
		const string indent = "\t";

		public static void stringify(JsonObject jo, StringBuilder sb, int depth, bool niceFormat)
		{
			sb.Append('{');

			++depth;

			bool firstValue = false;
			foreach (var kvp in jo)
			{
				if (firstValue) sb.Append(',');
				if (niceFormat) appendIndent(sb, depth);

				writeEscapedString(sb, kvp.Key);
				sb.Append(':');
				if (kvp.Value is JsonObject) stringify(kvp.Value as JsonObject, sb, depth + 1, niceFormat);
				else if (kvp.Value is JsonArray) stringify(kvp.Value as JsonArray, sb, depth + 1, niceFormat);
				else writeValue(sb, kvp.Value);

				firstValue = true;
			}

			--depth;

			if (niceFormat) appendIndent(sb, depth);
			sb.Append('}');
		}

		public static void stringify(JsonArray ja, StringBuilder sb, int depth, bool niceFormat)
		{
			sb.Append('[');

			++depth;

			for (int i = 0; i < ja.Count; i++)
			{
				if (i > 0) sb.Append(',');

				if (niceFormat) appendIndent(sb, depth);
				if (ja[i] is JsonObject) stringify(ja[i] as JsonObject, sb, depth + 1, niceFormat);
				else if (ja[i] is JsonArray) stringify(ja[i] as JsonArray, sb, depth + 1, niceFormat);
				else writeValue(sb, ja[i]);
			}

			--depth;

			if (niceFormat) appendIndent(sb, depth);
			sb.Append(']');
		}

		static void appendIndent(StringBuilder sb, int depth)
		{
			sb.Append(newline);
			for (int j = 0; j < depth; j++) sb.Append(indent);
		}

		public static void writeEscapedString(StringBuilder sb, string s)
		{
			sb.Append('"');
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				switch (c)
				{
					case '"':
						sb.Append("\\\"");
						continue;
					case '\\':
						sb.Append("\\\\");
						continue;
					case '\n':
						sb.Append("\\n");
						continue;
					case '\r':
						sb.Append("\\r");
						continue;
					case '\t':
						sb.Append("\\t");
						continue;
					default:
						sb.Append(c);
						break;
				}
			}
			sb.Append('"');
		}

		static void writeValue(StringBuilder sb, object o)
		{
			if (o == null) sb.Append("null");
			else if (o is string) writeEscapedString(sb, o as string);
			else if (o is bool) sb.Append((bool)o ? "true" : "false");
			else sb.Append(o.ToString());
		}
	}

	internal static class Helper
	{
		static Type[] ValidTypes = new Type[] {typeof(JsonArray),typeof(JsonObject),
                        typeof(string),typeof(bool),typeof(byte),typeof(sbyte),
                        typeof(short),typeof(ushort),typeof(int),typeof(uint),typeof(long),typeof(ulong),
                        typeof(float),typeof(double),typeof(decimal)};

		internal static void CheckValidType(object Value)
		{
			if (Value != null)
			{
				for (int i = 0; i < ValidTypes.Length; i++) if (Value.GetType() == ValidTypes[i]) return;
				throw new FormatException("Invalid value type: " + Value.GetType().ToString());
			}
		}
		internal static void WriteString(StringBuilder sb, string s)
		{
			sb.Append('"');
			for (int i = 0; i < s.Length; i++)
			{
				char c = s[i];
				if (c == '"')
				{
					sb.Append("\\\"");
					continue;
				}
				if (c == '\\')
				{
					sb.Append("\\\\");
					continue;
				}
				if (c == '\n')
				{
					sb.Append("\\n");
					continue;
				}
				if (c == '\r')
				{
					sb.Append("\\r");
					continue;
				}
				if (c == '\t')
				{
					sb.Append("\\t");
					continue;
				}
				sb.Append(c);
			}
			sb.Append('"');
		}
	}

	interface IJsonContainer
	{
		bool IsArray { get; }
		void InternalAdd(string key, object value);
	}
}
