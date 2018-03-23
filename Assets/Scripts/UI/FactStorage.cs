using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactStorage : MonoBehaviour {

	private Dictionary<int,string[]> factList = new Dictionary<int, string[]>();

	void Awake()
	{
		LoadFactText();
	}

	private void LoadFactText()
	{
		TextAsset txt = (TextAsset) Resources.Load ("FactsTxt", typeof (TextAsset));

		if(txt == null)
		{
			return;
		}

		string[] splitTxt = txt.text.Split("\n"[0]);

		if(splitTxt.Length <= 1)
		{
			return;
		}

		int dictionaryKey = 0;
		for(int i = 0; i < splitTxt.Length; i += 2)
		{
			factList.Add(dictionaryKey,new string[2]{splitTxt[i],splitTxt[i+1]});
			dictionaryKey++;
		}
	}

	public string[] GetRandomFact()
	{
		string[] factArray = null;
		if (factList.TryGetValue(Random.Range(0,factList.Count), out factArray))
		{
			return factArray;
		}
		else
		{
			return factArray = new string[2]{"Oops...","Whoops! Looks like something went wrong with the fact storage!"};
		}
	}
}
