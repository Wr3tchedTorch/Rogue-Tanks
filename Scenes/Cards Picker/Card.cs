using Godot;
using System;

public partial class Card : Node
{

	public static Card[] cards = new Card[] 
	{
		new AttributeBoostCard("nome", "desc", "CriticalChance", 15),
		new AttributeBoostCard("nome", "desc", "Damage", 10),
		new AttributeBoostCard("nome", "desc", "FireRate", 15),
	};

	public string CardName { get; set; }
	public string CardDescription { get; set; }

	public Card(string cardName, string cardDescription)
	{

		CardName = cardName;
		CardDescription = cardDescription;
	}

	public override void _Ready()
	{
	}
}
