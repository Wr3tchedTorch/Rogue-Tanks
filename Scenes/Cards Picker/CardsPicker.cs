using Godot;
using System;
using System.Collections.Generic;

public partial class CardsPicker : Control
{

	public Dictionary<string, Card> cardsList = new();

	private Tank _player;
	private readonly Random _RNG = new();

	private Card _firstCard;
	private Card _secondCard;
	private Card _thirdCard;

	public override void _Ready()
	{

		_player = GetTree().GetFirstNodeInGroup("Player") as Tank;
	}

	public override void _Process(double delta)
	{

		if (Input.IsActionJustPressed("random_cards"))
			ShowRandomCards();
	}

	public void ShowRandomCards()
	{

		int _firstCardIndex = GetRandomNonRepeatedIndex(-1, -1);
		int _secondCardIndex = GetRandomNonRepeatedIndex(_firstCardIndex, -1);
		int _thirdCardIndex = GetRandomNonRepeatedIndex(_firstCardIndex, _secondCardIndex);

		_firstCard = Card.cardsList[_firstCardIndex];
		_secondCard = Card.cardsList[_secondCardIndex];
		_thirdCard = Card.cardsList[_thirdCardIndex];

		RenderCards();

		Visible = true;
	}

	public void OnFirstCardPicked()
		=> PickCard(_firstCard);

	public void OnSecondCardPicked()
		=> PickCard(_secondCard);

	public void OnThirdCardPicked()
		=> PickCard(_thirdCard);

	private void PickCard(Card card)
	{

		if (card is AttributeBoostCard attCard)
			UseAttributeCard(attCard);
		if (card is ComponentCard componentCard)
			UseComponentCard(componentCard);

		Visible = false;
		GetTree().Paused = false;
	}

	private void UseAttributeCard(AttributeBoostCard card)
	{

		if (_player.GetType().GetProperty(card.AttributeName) == null)
			throw new ArgumentException($"{card.AttributeName} does not exist in Tank class");

		float oldValue = (float)_player.GetType().GetProperty(card.AttributeName).GetValue(_player, null);

		float newValue = card.BoostPercentage / 100 * oldValue + oldValue;
		_player.GetType().GetProperty(card.AttributeName).SetValue(_player, newValue, null);
	}

	private void UseComponentCard(ComponentCard card)
	{

		_player.AddComponent(card.ComponentScene);
	}

	private void RenderCards()
	{

		Node firstCardGroup = GetNode<Node>("%FirstCardInfo");
		Node secondCardGroup = GetNode<Node>("%SecondCardInfo");
		Node thirdCardGroup = GetNode<Node>("%ThirdCardInfo");

		UpdateCardInfo(firstCardGroup, _firstCard);
		UpdateCardInfo(secondCardGroup, _secondCard);
		UpdateCardInfo(thirdCardGroup, _thirdCard);
	}

	private void UpdateCardInfo(Node cardGroup, Card card)
	{

		cardGroup.GetNode<Label>("Title").Text = card.CardName;
		cardGroup.GetNode<Label>("Description").Text = card.CardDescription;
	}

	private int GetRandomNonRepeatedIndex(int previousIndex, int anotherPreviousIndex)
	{

		int index = _RNG.Next(0, Card.cardsList.Length - 1);

		if (index == previousIndex ||
			index == anotherPreviousIndex)
		{
			return GetRandomNonRepeatedIndex(previousIndex, anotherPreviousIndex);
		}

		return index;
	}
}

