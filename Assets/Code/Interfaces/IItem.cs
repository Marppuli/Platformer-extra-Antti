using System.Collections;
using UnityEngine;

namespace GA.Platformer
{
	public interface IItem
	{
		int ID { get; set; }
		string Name { get; set; }
		string Description { get; set; }
		float Weight { get; set; }
		int Count { get; set; }
		bool IsUnique { get; set; }
		Sprite Icon { get; set; }
		float TotalWeight { get; }
	}
}