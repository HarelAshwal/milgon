using System;
using System.Collections.Generic;
using System.Globalization;

namespace Calendar
{
	internal class MyHebrewCalendar
	{
		public MyHebrewCalendar()
		{
		}

		public static DateTime GetDateTimeInGeorgianManner(string HebrewFullString)
		{
			string[] strArrays = HebrewFullString.Split(new char[] { '/' });
			string str = strArrays[2].Replace("\"", "");
			string str1 = strArrays[1];
			string str2 = strArrays[0].Replace("'", "").Replace("\"", "");
			return MyHebrewCalendar.GetDateTimeInGeorgianManner(str, str1, str2);
		}

		public static DateTime GetDateTimeInGeorgianManner(string HebrewYearString, string HebrewMonthString, string HebrewDayString)
		{
			HebrewCalendar hebrewCalendar = new HebrewCalendar();
			int yearInGeorgianManner = MyHebrewCalendar.GetYearInGeorgianManner(HebrewYearString);
			int monthInGeorgianManner = MyHebrewCalendar.GetMonthInGeorgianManner(HebrewMonthString, hebrewCalendar.IsLeapYear(yearInGeorgianManner));
			int dayInGeorgianManner = MyHebrewCalendar.GetDayInGeorgianManner(HebrewDayString);
			DateTime dateTime = hebrewCalendar.ToDateTime(yearInGeorgianManner, monthInGeorgianManner, dayInGeorgianManner, 0, 0, 0, 0);
			return dateTime;
		}

		public static int GetDayInGeorgianManner(string HebrewDayString)
		{
			return MyHebrewCalendar.HebrewStringToInt(HebrewDayString.Trim());
		}

		public static string GetDayInHebrewString(int intDayIndex)
		{
			string str = MyHebrewCalendar.GetGeometria(intDayIndex).Replace("יה", "טו").Replace("יו", "טז");
			return str;
		}

		public static string[] GetDaysInMonth(int intYearsCount, int intMonthIndex)
		{
			int daysInMonth = (new HebrewCalendar()).GetDaysInMonth(intYearsCount, intMonthIndex);
			List<string> strs = new List<string>();
			for (int i = 1; i <= daysInMonth; i++)
			{
				strs.Add(MyHebrewCalendar.GetDayInHebrewString(i));
			}
			return strs.ToArray();
		}

		public static string GetGeometria(int Number)
		{
			int number = Number;
			List<char> chrs = new List<char>()
			{
				'\u05D0',
				'\u05D1',
				'\u05D2',
				'\u05D3',
				'\u05D4',
				'\u05D5',
				'\u05D6',
				'\u05D7',
				'\u05D8',
				'\u05D9',
				'\u05DB',
				'\u05DC',
				'\u05DE',
				'\u05E0',
				'\u05E1',
				'\u05E2',
				'\u05E4',
				'\u05E6',
				'\u05E7',
				'\u05E8',
				'\u05E9',
				'\u05EA'
			};
			string str = "";
			for (int i = chrs.Count - 1; i >= 0; i--)
			{
				while (number >= MyHebrewCalendar.GetLetterValue(chrs[i]))
				{
					str = string.Concat(str, chrs[i]);
					number -= MyHebrewCalendar.GetLetterValue(chrs[i]);
				}
			}
			return str;
		}

		public static DateTime GetHebrewDateToday()
		{
			DateTime today = DateTime.Today;
			bool flag = false;
			bool flag1 = false;
			DateTime now = DateTime.Now;
			DateTime dateTime = DateTime.Now;
			SunTimes.Instance.CalculateSunRiseSetTimes(new SunTimes.LatitudeCoords(32, 4, 0, SunTimes.LatitudeCoords.Direction.North), new SunTimes.LongitudeCoords(34, 46, 0, SunTimes.LongitudeCoords.Direction.East), today, ref now, ref dateTime, ref flag, ref flag1);
			DateTime now1 = DateTime.Now;
			if (now1 > dateTime)
			{
				now1 = now1.AddDays(1);
			}
			return now1;
		}

		public static int GetLetterValue(char Letter)
		{
			int num;
			switch (Letter)
			{
				case '\u05D0':
				{
					num = 1;
					break;
				}
				case '\u05D1':
				{
					num = 2;
					break;
				}
				case '\u05D2':
				{
					num = 3;
					break;
				}
				case '\u05D3':
				{
					num = 4;
					break;
				}
				case '\u05D4':
				{
					num = 5;
					break;
				}
				case '\u05D5':
				{
					num = 6;
					break;
				}
				case '\u05D6':
				{
					num = 7;
					break;
				}
				case '\u05D7':
				{
					num = 8;
					break;
				}
				case '\u05D8':
				{
					num = 9;
					break;
				}
				case '\u05D9':
				{
					num = 10;
					break;
				}
				case '\u05DA':
				case '\u05DD':
				case '\u05DF':
				case '\u05E3':
				case '\u05E5':
				{
					num = 0;
					break;
				}
				case '\u05DB':
				{
					num = 20;
					break;
				}
				case '\u05DC':
				{
					num = 30;
					break;
				}
				case '\u05DE':
				{
					num = 40;
					break;
				}
				case '\u05E0':
				{
					num = 50;
					break;
				}
				case '\u05E1':
				{
					num = 60;
					break;
				}
				case '\u05E2':
				{
					num = 70;
					break;
				}
				case '\u05E4':
				{
					num = 80;
					break;
				}
				case '\u05E6':
				{
					num = 90;
					break;
				}
				case '\u05E7':
				{
					num = 100;
					break;
				}
				case '\u05E8':
				{
					num = 200;
					break;
				}
				case '\u05E9':
				{
					num = 300;
					break;
				}
				case '\u05EA':
				{
					num = 400;
					break;
				}
				default:
				{
					goto case '\u05E5';
				}
			}
			return num;
		}

		public static int GetMonthInGeorgianManner(string HebrewMonthString, bool IsLeapYear)
		{
			int num;
			string str = HebrewMonthString.Trim();
			if (str != null)
			{
				switch (str)
				{
					case "תשרי":
					{
						num = 1;
						break;
					}
					case "חשון":
					case "חשוון":
					{
						num = 2;
						break;
					}
					case "כסלו":
					{
						num = 3;
						break;
					}
					case "טבת":
					{
						num = 4;
						break;
					}
					case "שבט":
					{
						num = 5;
						break;
					}
					case "אדר":
					{
						num = 6;
						break;
					}
					case "אדר א":
					{
						num = 6;
						break;
					}
					case "אדר ב":
					{
						num = 7;
						break;
					}
					case "ניסן":
					{
						if (IsLeapYear)
						{
							num = 8;
							break;
						}
						else
						{
							num = 7;
							break;
						}
					}
					case "אייר":
					{
						if (IsLeapYear)
						{
							num = 9;
							break;
						}
						else
						{
							num = 8;
							break;
						}
					}
					case "סיון":
					case "סיוון":
					{
						if (IsLeapYear)
						{
							num = 10;
							break;
						}
						else
						{
							num = 9;
							break;
						}
					}
					case "תמוז":
					{
						if (IsLeapYear)
						{
							num = 11;
							break;
						}
						else
						{
							num = 10;
							break;
						}
					}
					case "אב":
					{
						if (IsLeapYear)
						{
							num = 12;
							break;
						}
						else
						{
							num = 11;
							break;
						}
					}
					case "אלול":
					{
						if (IsLeapYear)
						{
							num = 13;
							break;
						}
						else
						{
							num = 12;
							break;
						}
					}
					default:
					{
						num = -1;
						return num;
					}
				}
			}
			else
			{
				num = -1;
				return num;
			}
			return num;
		}

		public static string GetMonthInHebrewString(int intMonthIndex, bool IsLeapYear)
		{
			string str;
			switch (intMonthIndex)
			{
				case 1:
				{
					str = "תשרי";
					break;
				}
				case 2:
				{
					str = "חשוון";
					break;
				}
				case 3:
				{
					str = "כסלו";
					break;
				}
				case 4:
				{
					str = "טבת";
					break;
				}
				case 5:
				{
					str = "שבט";
					break;
				}
				case 6:
				{
					if (IsLeapYear)
					{
						str = "אדר א";
						break;
					}
					else
					{
						str = "אדר";
						break;
					}
				}
				case 7:
				{
					if (IsLeapYear)
					{
						str = "אדר ב";
						break;
					}
					else
					{
						str = "ניסן";
						break;
					}
				}
				case 8:
				{
					if (IsLeapYear)
					{
						str = "ניסן";
						break;
					}
					else
					{
						str = "אייר";
						break;
					}
				}
				case 9:
				{
					if (IsLeapYear)
					{
						str = "אייר";
						break;
					}
					else
					{
						str = "סיוון";
						break;
					}
				}
				case 10:
				{
					if (IsLeapYear)
					{
						str = "סיוון";
						break;
					}
					else
					{
						str = "תמוז";
						break;
					}
				}
				case 11:
				{
					if (IsLeapYear)
					{
						str = "תמוז";
						break;
					}
					else
					{
						str = "אב";
						break;
					}
				}
				case 12:
				{
					if (IsLeapYear)
					{
						str = "אב";
						break;
					}
					else
					{
						str = "אלול";
						break;
					}
				}
				case 13:
				{
					if (IsLeapYear)
					{
						str = "אלול";
						break;
					}
					else
					{
						str = "";
						break;
					}
				}
				default:
				{
					str = "";
					break;
				}
			}
			return str;
		}

		public static string[] GetMonthsInYear(int intYearsCount)
		{
			HebrewCalendar hebrewCalendar = new HebrewCalendar();
			List<string> strs = new List<string>();
			for (int i = 1; i <= hebrewCalendar.GetMonthsInYear(intYearsCount); i++)
			{
				strs.Add(MyHebrewCalendar.GetMonthInHebrewString(i, hebrewCalendar.IsLeapYear(intYearsCount)));
			}
			return strs.ToArray();
		}

		public static int GetYearInGeorgianManner(string HebrewYearString)
		{
			return MyHebrewCalendar.HebrewStringToInt(HebrewYearString.Trim()) + 5000;
		}

		public static string[] GetYearsCloserToYear(int intYearsCount)
		{
			HebrewCalendar hebrewCalendar = new HebrewCalendar();
			List<string> strs = new List<string>();
			for (int i = intYearsCount - 1; i <= intYearsCount + 1; i++)
			{
				strs.Add(MyHebrewCalendar.GetYearsInHebrewString(i));
			}
			return strs.ToArray();
		}

		public static string GetYearsInHebrewString(int intYearsCount)
		{
			return MyHebrewCalendar.GetGeometria(intYearsCount - 5000);
		}

		public static int HebrewStringToInt(string HebrewString)
		{
			int letterValue = 0;
			for (int i = 0; i < HebrewString.Length; i++)
			{
				letterValue += MyHebrewCalendar.GetLetterValue(HebrewString[i]);
			}
			return letterValue;
		}
	}
}