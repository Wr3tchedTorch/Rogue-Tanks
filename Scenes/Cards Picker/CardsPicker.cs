using Godot;
using System;
using System.Collections.Generic;

public partial class CardsPicker : Control
{

	public Dictionary<string, Card> cardsList = new();

	private Tank _player;

	public override void _Ready()
	{

		_player = GetTree().GetFirstNodeInGroup("Player") as Tank;
	}

	public void PickRandomCards()
	{

	}

	public void OnCardPicked(Card card)
	{

		if (card is AttributeBoostCard attributeCard)
			PickAttributeCard(attributeCard);
	}

	private void PickAttributeCard(AttributeBoostCard card)
	{

		if (card.GetType().GetProperty(card.AttributeName) == null)
			throw new ArgumentException($"{card.AttributeName} does not exist in Tank class");

		float oldValue = (float)_player.GetType().GetProperty(card.AttributeName).GetValue(_player, null);

		float newValue = card.BoostPercentage / 100 * oldValue + oldValue;
		_player.GetType().GetProperty(card.AttributeName).SetValue(_player, newValue, null);
	}
}
