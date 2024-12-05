using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA.Platformer
{
	public class Inventory
	{
		public List<IItem> Items
		{
			get;
		}

		public float Weight
		{
			get
			{
				float weight = 0;
				foreach (IItem item in Items)
				{
					weight += item.TotalWeight;
				}

				return weight;
			}
		}

		public float WeightLimit
		{
			get;
		}

		// The constucture initializes a plain C# object. It is called immediately after
		// the object is created.
		// NOTE: You CAN'T define a constructor with classes that are derived from 
		// MonoBehaviour!
		public Inventory(float weightLimit)
		{
			// You can set the value of a read-only property only in a constructor!
			WeightLimit = weightLimit;

			Items = new List<IItem>();
		}

		/// <summary>
		/// Adds a new item to the inventory
		/// </summary>
		/// <param name="item">The item to be added</param>
		/// <returns>True, if adding was successful, false otherwise.</returns>
		public bool AddItem(IItem item)
		{
			if (Weight + item.TotalWeight > WeightLimit)
			{
				// TODO: Should we be able to add items until the weight limit is reached?
				return false;
			}

			IItem existing = null;
			foreach (IItem inspectedItem in Items)
			{
				if (inspectedItem.ID == item.ID)
				{
					// There's already an item of the same type we try to add in the inventory,
					// we may be able to increase the count.
					existing = inspectedItem;
					break; // Early exit from the loop, the existing object is found.
				}
			}

			if (existing != null && !existing.IsUnique)
			{
				// Just increase the item count.
				existing.Count += item.Count;
			}
			else
			{
				Items.Add(item);
				Debug.Log("Added item " + item.Name);
			}

			return true;
		}

		// Method overloading. We can define more than one method with a same name 
		// if their parameters differ (the type or number of parameters).
		public IItem GetItem(IItem item)
		{
			return GetItem(item.ID);
		}

		// The parameter count has a default value. It is used if no other value is
		// passed to the method.
		public IItem GetItem(int ID, int count = 1)
		{
			IItem result = null;
			IItem remove = null;

			foreach (IItem inspectedItem in Items)
			{
				if (inspectedItem.ID == ID)
				{
					// This is one way of initializing a new object.
					result = new Item()
					{
						ID = inspectedItem.ID,
						Weight = inspectedItem.Weight,
						Name = inspectedItem.Name,
						Description = inspectedItem.Description,
						IsUnique = inspectedItem.IsUnique
					};

					if (inspectedItem.Count > count)
					{
						// No need to remove the item completely from the inventory.
						// Just subtract the count from inspectedItem.
						inspectedItem.Count -= count;
						result.Count = count;
					}
					else if(inspectedItem.Count == count)
					{
						// Remove the item completely
						result.Count = count;
						remove = inspectedItem;
					}
					else
					{
						// Can't get enough of these items from the inventory. Return all there
						// is and remove the item.
						result.Count = inspectedItem.Count;
						remove = inspectedItem;
					}

					break; // Early exit
				}
			}

			if (remove != null)
			{
				// Let's remove the item
				Items.Remove(remove);
			}

			return result;
		}

		public List<IItem> GetItems()
		{
			// Creates a copy of the existing item list.
			List<IItem> result = new List<IItem>(Items);
			// Clear the original list.
			Items.Clear();
			// Return the copy.
			return result;
		}

		public void Save()
		{
			List<int> ids = new List<int>();

			foreach (Item item in Items)
			{
				int ID = item.ID;
				ids.Add(ID);

				string nameKey = ID + "_name";
				PlayerPrefs.SetString(nameKey, item.Name);

				string countKey = ID + "_count";
				PlayerPrefs.SetInt(countKey, item.Count);

				string weightKey = ID + "_weight";
				PlayerPrefs.SetFloat(weightKey, item.Weight);
			}

			string idList = "";
			foreach(int id in ids)
			{
				idList += id + ",";
			}

			PlayerPrefs.SetString("IDs", idList);
		}

		public void Load()
		{
			string idList = PlayerPrefs.GetString("IDs", "");
			string[] ids = idList.Split(",");
			foreach(string idString in ids)
			{
				if (int.TryParse(idString, out int ID))
				{
					Item item = new Item()
					{
						ID = ID
					};

					string nameKey = ID + "_name";
					item.Name = PlayerPrefs.GetString(nameKey, "");

					string countKey = ID + "_count";
					item.Count = PlayerPrefs.GetInt(countKey, 0);

					string weightKey = ID + "_weight";
					item.Weight = PlayerPrefs.GetFloat(weightKey, 0f);

					AddItem(item);
				}
			}
		}

		public void ClearSave()
		{
			string idList = PlayerPrefs.GetString("IDs", "");
			string[] ids = idList.Split(",");

			foreach (string idString in ids)
			{
				if (int.TryParse(idString, out int ID))
				{
					string nameKey = ID + "_name";
					string countKey = ID + "_count";
					string weightKey = ID + "_weight";

					PlayerPrefs.DeleteKey(nameKey);
					PlayerPrefs.DeleteKey(countKey);
					PlayerPrefs.DeleteKey(weightKey);
				}
			}

			PlayerPrefs.DeleteKey("IDs");
		}
	}
}
