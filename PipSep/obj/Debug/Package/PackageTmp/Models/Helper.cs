﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PipSep.Models
{
	public class Helper
	{
		public static int CountIntersections(List<CommonThings> first, List<CommonThings> second)
		{
			int x = 0;
			for (int i = 0; i < first.Count; ++i)
			{
				for (int j = i + 1; j < second.Count; ++j)
				{
					if (first[i].Type == second[j].Type && first[i].Object == second[j].Object)
						++x;
				}
			}
			return x;
		}
	}

	public class PairStringList
	{
		public string UserName { get; set; }
		public List<string> Friends = new List<string>();
		public List<string> Pages = new List<string>();
	}

	public class PairThingsValue
	{
		public int First { get; set; }
		public int Second { get; set; }
		public int Value { get; set; }
		public bool CommonGender { get; set; }
	}

	public class TheOne
	{
		public string Name { get; set; }
		public bool Gender { get; set; }
	}
}